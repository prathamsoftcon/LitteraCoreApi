using LitteraCore.Models;

namespace LitteraCore.Common.Token
{
    public class UserToken
    {
        public string AuthToken { get; set; }

        public UserInfo userdetails { get; set; }

        public Agency? agencydetail { get; set; }
    }
}
