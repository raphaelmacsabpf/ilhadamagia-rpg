using LZ4;
using System;
using System.Text;

namespace Server.Application
{
    public class LZ4Utils
    {
        public static string Compress(string text)
        {
            var compressed = Convert.ToBase64String(LZ4Codec.Wrap(Encoding.UTF8.GetBytes(text)));
            return compressed;
        }

        public static string Decompress(string compressed)
        {
            var uncompressed = Encoding.UTF8.GetString(LZ4Codec.Unwrap(Convert.FromBase64String(compressed)));
            return uncompressed;
        }
    }
}