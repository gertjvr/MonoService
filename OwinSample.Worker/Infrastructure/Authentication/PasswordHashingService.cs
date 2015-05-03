using System;
using System.Security.Cryptography;
using System.Text;

namespace OwinSample.Worker.Infrastructure.Authentication
{
    public class PasswordHashingService : IPasswordHashingService
    {
        private readonly Random _rand;
        private const int _hashRounds = 50;

        public PasswordHashingService()
        {
            _rand = new Random();
        }

        public string SaltAndHash(string plainTextPassword)
        {
            var salt = GenerateSalt();
            var hash = Hash(plainTextPassword, salt);
            var result = string.Format("{0}:{1}", salt, hash);
            return result;
        }

        public bool TryVerify(string plainTextPassword, string saltedAndHashedPassword)
        {
            var tokens = saltedAndHashedPassword.Split(':');
            if (tokens.Length != 2) return false;

            var salt = tokens[0];
            var expectedHash = tokens[1];
            var actualHash = Hash(plainTextPassword, salt);

            return string.Compare(expectedHash, actualHash, StringComparison.Ordinal) == 0;
        }

        private string GenerateSalt()
        {
            var saltBytes = new byte[32];
            _rand.NextBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);
            return salt;
        }

        private static string Hash(string plainTextPassword, string salt)
        {
            var hasher = new SHA256Managed();
            var bytesToHash = Encoding.UTF8.GetBytes(salt + plainTextPassword);
            for (var i = 0; i < _hashRounds; i++)
            {
                var hashedBytes = hasher.ComputeHash(bytesToHash);
                var hashedString = Convert.ToBase64String(hashedBytes);
                var stringToHash = salt + hashedString;
                bytesToHash = Encoding.UTF8.GetBytes(stringToHash);
            }
            var hash = Convert.ToBase64String(bytesToHash);
            return hash;
        }
    }
}