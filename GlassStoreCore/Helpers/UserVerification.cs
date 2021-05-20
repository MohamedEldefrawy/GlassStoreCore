using GlassStoreCore.BL.Models;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace GlassStoreCore.Helpers
{
    public static class UserVerification
    {
        public static bool VerifyHashedPassword(ApplicationUser user, string hashedPassword)
        {

            if (user.PasswordHash == hashedPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string Hash(string pw)
        {
            // STEP 1 Create the salt value with a cryptographic PRNG:
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            // Create the Rfc2898DeriveBytes and get the hash value:
            var pbkdf2 = new Rfc2898DeriveBytes(pw, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            // STEP 3 Combine the salt and password bytes for later use:
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool UnHash(string pw, string hashedPw)
        {
            // Exectract the bytes
            byte[] hashBytes = Convert.FromBase64String(hashedPw);

            // Get the salt
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Compute the hash on the password the user entered 
            var pbkdf2 = new Rfc2898DeriveBytes(pw, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }
}
