namespace OwinSample.Worker.Infrastructure.Authentication
{
    public interface IPasswordHashingService
    {
        string SaltAndHash(string plainTextPassword);
        bool TryVerify(string plainTextPassword, string saltedAndHashedPassword);
    }
}