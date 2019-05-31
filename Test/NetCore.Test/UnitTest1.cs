using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCore.Library.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Test
{
    [TestClass]
    public class UnitTest1 : Testbase
    {
        private const string ImageBase64 = @"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";
        private const string Text = "RoofMaxx - DealerSoft";

        [TestMethod]
        public async Task TestMethod1()
        {
            IEncryptionService encryption = Provider.GetService<IEncryptionService>();

            byte[] saltData = StringToByteArray(Text);
            byte[] salthash = encryption.Hash(saltData);
            string saltKey = WriteHexaString(salthash);

            byte[] enciptionKeyData = StringToByteArray(ImageBase64);
            byte[] enciptionKeyHash = encryption.Hash(enciptionKeyData);
            string enciptionKey = WriteHexaString(enciptionKeyHash);
        }

        private byte[] StringToByteArray(string text)
        {
            return Encoding.Unicode.GetBytes(text);
        }

        private string WriteHexaString(byte[] array)
        {
            StringBuilder builder = new StringBuilder(array.Length * 2);

            byte last = array.Last();

            foreach (byte item in array)
            {
                builder.AppendFormat(item.Equals(last) ? "0x{0:X2}" : "0x{0:X2}, ", item);
            }
            return builder.ToString();
        }
    }
}