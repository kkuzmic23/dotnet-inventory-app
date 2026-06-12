using System;
using System.Security.Cryptography;
namespace BusinessLogicLayer
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100000;
        private const string Prefix = "pbkdf2";

        public static string HashPassword(string password)
        {
            var salt = new byte[SaltSize];
            var rng = new Random();
            for (var i = 0; i < salt.Length; i++)
            {
                salt[i] = (byte)rng.Next(0, 256);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                var key = pbkdf2.GetBytes(KeySize);
                return string.Join("$",
                    Prefix,
                    Iterations.ToString(),
                    Convert.ToBase64String(salt),
                    Convert.ToBase64String(key));
            }
        }

        public static bool Verify(string password, string hashedPassword)
        {
            if (!IsHashFormat(hashedPassword))
            {
                return false;
            }

            var parts = hashedPassword.Split('$');
            if (parts.Length != 4)
            {
                return false;
            }

            if (!int.TryParse(parts[1], out var iterations))
            {
                return false;
            }

            var salt = Convert.FromBase64String(parts[2]);
            var expected = Convert.FromBase64String(parts[3]);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                var actual = pbkdf2.GetBytes(expected.Length);
                return SlowEquals(actual, expected);
            }
        }

        public static bool IsHashFormat(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return value.StartsWith(Prefix + "$", StringComparison.Ordinal);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = a.Length ^ b.Length;
            var length = Math.Min(a.Length, b.Length);

            for (var i = 0; i < length; i++)
            {
                diff |= a[i] ^ b[i];
            }

            return diff == 0;
        }
    }
}
