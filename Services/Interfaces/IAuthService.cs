namespace CMS_.Introduction.Services.Interfaces
{
    public interface IAuthService
    {
        string Authenticate(string username, string password);
    }
}
