using System;
using System.Security.Cryptography;
using System.Text;
namespace MyProject.Models
{
 
    public class PasswordHelper
    {

        private const int SaltSize = 16;
        /* the size of the salt in bytes*/
        private const int HashSize = 32; // the size of the hash in bytes
        private const int Iterations = 10000; // the number of iterations for the PBKDF2 algorithm
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create()) { rng.GetBytes(saltBytes); }
            return Convert.ToBase64String(saltBytes);
        }
        public static string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt); byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            using (var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, saltBytes, Iterations))
            { byte[] hashBytes = pbkdf2.GetBytes(HashSize); byte[] hashSaltBytes = new byte[SaltSize + HashSize]; Array.Copy(saltBytes, 0, hashSaltBytes, 0, SaltSize); Array.Copy(hashBytes, 0, hashSaltBytes, SaltSize, HashSize); return Convert.ToBase64String(hashSaltBytes); }
        }
        public static bool VerifyPassword(string password, string hashedPassword)
        { byte[] hashSaltBytes = Convert.FromBase64String(hashedPassword);
            byte[] saltBytes = new byte[SaltSize];
            byte[] hashBytes = new byte[HashSize];
            Array.Copy(hashSaltBytes, 0, saltBytes, 0, SaltSize); 
            Array.Copy(hashSaltBytes, SaltSize, hashBytes, 0, HashSize); 
            using (var pbkdf2 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), saltBytes, Iterations)) { byte[] testHashBytes = pbkdf2.GetBytes(HashSize); 
                return SlowEquals(hashBytes, testHashBytes); } }
        private static bool SlowEquals(byte[] a, byte[] b) { uint diff = (uint)a.Length ^ (uint)b.Length; for (int i = 0; i < a.Length && i < b.Length; i++) { diff |= (uint)(a[i] ^ b[i]); } return diff == 0; }
    }
}

