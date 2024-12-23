namespace LitteraCore.Common.Token
{
    public interface IAppAuthService
    {
        Task<UserToken> Authenticate(string username);
    }
}
