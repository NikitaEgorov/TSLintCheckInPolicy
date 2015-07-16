namespace FileEncodingCheckInPolicy
{
    public static class TextFileEncodingDetector
    {
        public static System.Text.Encoding DetectEncoding(byte[] buffer)
        {
            if (buffer == null)
            {
                return null;
            }

            // Now find out the first time Encoding:
            // I'm currently only using the BOM
            // If no BOM is present, use default encoding
            if (buffer.Length > 3)
            {
                if ((buffer[0] == 0xEF) && (buffer[1] == 0xBB) && (buffer[2] == 0xBF))
                {
                    return System.Text.Encoding.UTF8;
                }

                // Everything else is just not supported yet ...
                // FF FE UTF-16, little-endian
                // FE FF UTF-16, big-endian
                // FF FE 00 00 UTF-32 little-endian
                // 00 00 FE FF UTF-32, big-endian (.NET has no support yet ;))
            }

            return null;
        }
    }
}