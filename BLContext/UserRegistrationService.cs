using LitteraCore.DBContext;
using LitteraCore.Models;

namespace LitteraCore.BLContext
{
    public sealed class UserRegistrationResult
    {
        public bool Created { get; init; }
        public bool ShouldSendCreationEmail { get; init; }
    }

    public sealed class UserRegistrationService
    {
        private readonly IConfiguration _configuration;

        public UserRegistrationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public UserRegistrationResult Create(LoginUser user)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(user.agency);

            if (user.branchid == null)
            {
                user.branchid = Common.CommonEnum.Branchid;
            }

            // Admin-created accounts (e.g. the Employee/Administrator creation
            // form) never collect a password - that form has no password
            // field. UserDB.Save_User binds this value straight into
            // @Password on yuser.proc_yuser_ins_upd_user_vr1 with no
            // fallback, so a null/blank password made that stored-procedure
            // call fail; UserDB.Save_User silently swallows any SQL failure
            // to `false`, and Save_User_Data turns that into the generic
            // "User could not be created." error the controller returns.
            // Auto-generate one here using the same Encrypt(MD5(raw), true)
            // scheme already required by login (AppAuthService.VerifyPassword
            // decrypts the stored value and expects an MD5 hex string back)
            // and by AuthenticationController.BulkUpdatePassword.
            if (string.IsNullOrWhiteSpace(user.password))
            {
                user.password = GenerateDefaultStoredPassword();
            }

            var userBl = new UserBL(_configuration);
            var mobileExists = EnsureIdentifierIsAvailable(
                userBl,
                user,
                user.mobileno,
                isEmail: false,
                "This mobile no already registerd with another user.");
            var emailExists = EnsureIdentifierIsAvailable(
                userBl,
                user,
                user.emailid,
                isEmail: true,
                "This email id already registerd with another user.");

            return new UserRegistrationResult
            {
                Created = userBl.Save_User_Data(user),
                ShouldSendCreationEmail = !mobileExists && !emailExists
            };
        }

        // Fixed: this used to call userBl.Check_Mobile(...) for BOTH the mobile
        // AND the email identifier, so the email half of the duplicate check
        // searched the mobile-number column for an email string and could
        // never match - meaning a second account could always claim an email
        // already in use via this path with no server-side guard. Now takes an
        // explicit isEmail flag and calls the matching Check_EMAIL/Check_Mobile
        // method (both thin wrappers over UserDB.Check_Mobile_EMAIL, differing
        // only in the @type flag the shared stored procedure uses to pick
        // which column to match).
        private static bool EnsureIdentifierIsAvailable(
            UserBL userBl,
            LoginUser user,
            string? identifier,
            bool isEmail,
            string conflictMessage)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return false;
            }

            var existingAgency = isEmail
                ? userBl.Check_EMAIL(identifier, null, user.agency.AgencyTypeId)
                : userBl.Check_Mobile(identifier, null, user.agency.AgencyTypeId);
            if (existingAgency.agencyid == null)
            {
                return false;
            }

            // Reverted the agencytype-scoped bypass this method had for a
            // few hours (2026-09-23): product decision is that one person's
            // identity IS shared across every role/type they hold (an
            // Administrator and an Employee record for the same person
            // already share one agencyid/userid per the 9.12 linking
            // design; the same now deliberately applies to a Participant
            // becoming an Administrator/Employee too - "only difference is
            // of type id and tat typeid"). So ANY existing match here - same
            // agencytype or, via Check_Mobile_EMAIL's cross-agencytype
            // fallback (see UserDB.cs; confirmed identical in the old app,
            // not a migration regression), a different one entirely, e.g. a
            // Participant - is real and must be honored: whatever userid the
            // caller is submitting has to be THAT person's actual userid.
            // The false "already registerd" conflict this used to throw for
            // a Participant's mobile/email wasn't caused by that - it was
            // EmployeeFormModal.jsx's handleSave conflating agencyid and
            // userid into one reused value, when a person's agencyid and
            // userid are NOT always equal (confirmed: neither this app's own
            // participant registration nor the old app's own Administrator/
            // Staff creation ever assume they are - see that fix's own
            // comment). Fixed there instead, so this check goes back to
            // being an unconditional identity guard.
            if (!string.Equals(
                    existingAgency.userid?.ToString(),
                    user.userid,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(conflictMessage);
            }

            return true;
        }

        // Matches the storage convention AppAuthService.VerifyPassword and
        // AuthenticationController's UpdatePassword/BulkUpdatePassword
        // actions all already rely on: the DB password column holds
        // Encrypt(MD5hex(rawPassword), true) - never the raw value and never
        // a bare MD5 hash on its own. A random raw value is fine here since
        // this path never needs to reveal the password to the created user:
        // the Employee-creation flow calls CreateUser without APPURL, so
        // UserController.SendUserCreationEmail's password reveal never
        // triggers for it. This only needs to satisfy the non-null/non-blank
        // value the stored procedure requires.
        private static string GenerateDefaultStoredPassword()
        {
            var raw = Guid.NewGuid().ToString("N");
            var hashed = AuthDB.GetMD5Hash(raw);
            return YEncryptDecryptData.YEncryptDecryptData.Encrypt(hashed, true);
        }
    }
}
