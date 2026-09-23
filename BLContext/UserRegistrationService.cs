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

            var userBl = new UserBL(_configuration);
            // mobileExists/emailExists is true only when the identifier
            // already resolves to an agency AND that agency's userid matches
            // user.userid (EnsureIdentifierIsAvailable throws for anyone
            // else's identifier) - i.e. this save is reusing an EXISTING
            // person's own identity. Per product decision, the same person
            // is allowed to hold both an Administrator and an Employee
            // account: the frontend now resolves `user.userid` to that
            // existing person's own id when it finds them under a different
            // role (EmployeeFormModal.jsx's `linkedExisting` case), instead
            // of minting a new one - so this save is "grant an additional
            // role to an existing person," not "register someone new."
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
            var isNewPerson = !mobileExists && !emailExists;

            // Admin-created accounts (e.g. the Employee/Administrator
            // creation form) never collect a password - that form has no
            // password field. UserDB.Save_User binds this value straight
            // into @Password on yuser.proc_yuser_ins_upd_user_vr1 with no
            // fallback, so a null/blank password made that stored-procedure
            // call fail; UserDB.Save_User silently swallows any SQL failure
            // to `false`, and Save_User_Data turns that into the generic
            // "User could not be created." error the controller returns.
            // Auto-generate one here using the same Encrypt(MD5(raw), true)
            // scheme already required by login (AppAuthService.VerifyPassword
            // decrypts the stored value and expects an MD5 hex string back)
            // and by AuthenticationController.BulkUpdatePassword.
            //
            // Only do this for a genuinely NEW person (isNewPerson): when
            // reusing an existing person's identity to grant them a second
            // role, user.password is still blank (this form never collects
            // one), but UserDB.Save_User's proc is an upsert keyed on
            // @UserID - regenerating a password here would silently reset
            // that person's real, already-working login credential on every
            // additional-role save.
            if (isNewPerson && string.IsNullOrWhiteSpace(user.password))
            {
                user.password = GenerateDefaultStoredPassword();
            }

            return new UserRegistrationResult
            {
                Created = userBl.Save_User_Data(user),
                ShouldSendCreationEmail = isNewPerson
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
