using LZ4;
using System;
using System.Text;

namespace Client
{
    public class ClientNetworkManager
    {
        public ClientNetworkManager()
        {

        }

        public string Compress(string text)
        {
            var compressed = Convert.ToBase64String(LZ4Codec.Wrap(Encoding.UTF8.GetBytes(text)));
            return compressed;
        }

        public string Decompress(string compressed, int maxLength)
        {
            var uncompressedBytes = LZ4Codec.Unwrap(Convert.FromBase64String(compressed));
            var decoder = Encoding.UTF8.GetDecoder();
            var chars = new char[maxLength];
            decoder.Convert(uncompressedBytes, 0, uncompressedBytes.Length, chars, 0, chars.Length, true, out _, out int charsUsed, out _);
            return new string(chars, 0, charsUsed);
        }
    }
}
