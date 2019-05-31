using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NetCore.Library.Services
{
    public class EncryptionService : IEncryptionService
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        private static byte[] DefaultSalt = {
            0x8F, 0x5C, 0xCB, 0xF4, 0x87, 0xA8, 0x10, 0xDE, 0xC3,
            0xAB, 0x8E, 0x6D, 0xAB, 0xCB, 0x2A, 0xAE, 0xAD, 0x8F,
            0xBA, 0x01, 0x97, 0xDD, 0xEF, 0x15, 0x0F, 0x16, 0x5D,
            0x31, 0x2B, 0x8E, 0x8B, 0x77
        };

        /// <summary>
        ///
        /// </summary>
        private static byte[] EncryptionKey = {
            0x93, 0x94, 0x05, 0x38, 0x24, 0x69, 0x2E, 0x2E, 0x67,
            0x38, 0x28, 0xC6, 0xF1, 0x6D, 0xFF, 0xAB, 0x03, 0x50,
            0x3C, 0x74, 0x76, 0x82, 0x1A, 0x80, 0xEA, 0xD3, 0xB9, 0xF4,
            0x94, 0xA1, 0x78, 0x7E };

        /// <summary>
        ///
        /// </summary>
        private static int Iterations = 1000;

        #endregion Properties

        public string Decrypt(string text, byte[] password = null)
        {
            return Encoding.Unicode.GetString(
                Decrypt(Convert.FromBase64String(text), password)
            );
        }

        public byte[] Decrypt(byte[] data, byte[] password = null)
        {
            byte[] result = null;
            using (MemoryStream input = new MemoryStream(data))
            {
                using (MemoryStream output = new MemoryStream())
                {
                    using (CryptoStream crypto = Decrypt(input, password))
                    {
                        crypto.CopyTo(output);
                    }
                    result = output.ToArray();
                }
            }
            return result;
        }

        public CryptoStream Decrypt(Stream input, byte[] password = null)
        {
            return new CryptoStream(input, DefaultCryptoAlgorithm(password ?? EncryptionKey).CreateDecryptor(), CryptoStreamMode.Read);
        }

        public string Encrypt(string text, byte[] password = null)
        {
            return Convert.ToBase64String(
                Encrypt(Encoding.Unicode.GetBytes(text), password)
            );
        }

        public byte[] Encrypt(byte[] clearData, byte[] password = null)
        {
            byte[] result = null;
            using (MemoryStream input = new MemoryStream(clearData))
            {
                using (MemoryStream output = new MemoryStream())
                {
                    using (CryptoStream crypto = Encrypt(output, password))
                    {
                        input.CopyTo(crypto);
                    }
                    result = output.ToArray();
                }
            }
            return result;
        }

        public CryptoStream Encrypt(Stream output, byte[] password = null)
        {
            return new CryptoStream(output, DefaultCryptoAlgorithm(password ?? EncryptionKey).CreateEncryptor(), CryptoStreamMode.Write);
        }

        public string GenerateSalt()
        {
            byte[] buffer = new byte[16];
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(buffer);
            }
            return Convert.ToBase64String(buffer);
        }

        public string Hash(string text)
        {
            return Hash(text, DefaultHashAlgorithm());
        }

        public string Hash(string text, HashAlgorithm algorithm)
        {
            return Convert.ToBase64String(Hash(Encoding.UTF8.GetBytes(text), algorithm));
        }

        public byte[] Hash(byte[] data)
        {
            return Hash(data, DefaultHashAlgorithm());
        }

        public byte[] Hash(byte[] data, HashAlgorithm algorithm)
        {
            byte[] result = null;

            using (algorithm)
            {
                result = algorithm.ComputeHash(data);
            }

            return result;
        }

        private SymmetricAlgorithm DefaultCryptoAlgorithm(byte[] password)
        {
            Rfc2898DeriveBytes derivate = new Rfc2898DeriveBytes(password, DefaultSalt, Iterations);
            SymmetricAlgorithm alg = Aes.Create();

            alg.Key = derivate.GetBytes(32);
            alg.IV = derivate.GetBytes(16);
            return alg;
        }

        private HashAlgorithm DefaultHashAlgorithm()
        {
            return SHA256.Create();
        }
    }
}