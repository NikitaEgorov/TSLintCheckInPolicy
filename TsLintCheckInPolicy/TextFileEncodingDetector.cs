using System.IO;
using System.Text;
using Ude;

namespace FileEncodingCheckInPolicy
{
    public static class TextFileEncodingDetector
    {
        public static Encoding DetectEncoding(Stream stream)
        {
            if (stream == null)
            {
                return null;
            }
            var detector = new CharsetDetector();

            detector.Feed(stream);
            detector.DataEnd();
            if (detector.Charset == null)
            {
                return null;
            }
            return Encoding.GetEncoding(detector.Charset);
        }
    }
}