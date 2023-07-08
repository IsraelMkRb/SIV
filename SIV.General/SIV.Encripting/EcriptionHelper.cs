using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV
{
    public static class EcriptionHelper
    {
        public static string Encript(string PlainText,RSAParameters publicKey)
        {
            byte[] encryptedBytes;
            
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(PlainText), true);
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string cipherText, RSAParameters privateKey)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);
                byte[] decryptedBytes = rsa.Decrypt(cipherBytes, true);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
