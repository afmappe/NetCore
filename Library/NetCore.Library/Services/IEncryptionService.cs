using System.IO;
using System.Security.Cryptography;

namespace NetCore.Library.Services
{
    public interface IEncryptionService
    {
        string Decrypt(string text, byte[] password = null);

        byte[] Decrypt(byte[] data, byte[] password = null);

        CryptoStream Decrypt(Stream input, byte[] password = null);

        string Encrypt(string text, byte[] password = null);

        byte[] Encrypt(byte[] clearData, byte[] password = null);

        CryptoStream Encrypt(Stream output, byte[] password = null);

        string GenerateSalt();

        string Hash(string text);

        string Hash(string text, HashAlgorithm algorithm);

        byte[] Hash(byte[] data);

        byte[] Hash(byte[] data, HashAlgorithm algorithm);
    }
}