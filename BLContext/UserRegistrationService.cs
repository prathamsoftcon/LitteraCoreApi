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
            var mobileExists = EnsureIdentifierIsAvailable(
                userBl,
                user,
                user.mobileno,
                "This mobile no already registerd with another user.");
            var emailExists = EnsureIdentifierIsAvailable(
                userBl,
                user,
                user.emailid,
                "This email id already registerd with another user.");

            return new UserRegistrationResult
            {
                Created = userBl.Save_User_Data(user),
                ShouldSendCreationEmail = !mobileExists && !emailExists
            };
        }

        private static bool EnsureIdentifierIsAvailable(
            UserBL userBl,
            LoginUser user,
            string? identifier,
            string conflictMessage)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return false;
            }

            var existingAgency = userBl.Check_Mobile(
                identifier,
                null,
                user.agency.AgencyTypeId);
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
    }
}
