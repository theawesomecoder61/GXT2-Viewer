namespace GXT2_Viewer
{
    public static class Utils
    {
        public static uint GetHash(string str)
        {
            char[] charArray = str.ToLower().ToCharArray();
            int num = 0;
            uint num1 = (uint)num;
            uint num2 = (uint)num;
            while (num1 < charArray.Length)
            {
                num2 += charArray[num1];
                num2 = num2 + (num2 << 10);
                num2 = num2 ^ num2 >> 6;
                num1++;
            }
            num2 = num2 + (num2 << 3);
            num2 = num2 ^ num2 >> 11;
            num2 = num2 + (num2 << 15);
            return num2;
        }

        public static uint GetHash(byte[] data)
        {
            int num = 0;
            uint num1 = (uint)num;
            uint num2 = (uint)num;
            while (num1 < data.Length)
            {
                num2 += data[num1];
                num2 = num2 + (num2 << 10);
                num2 = num2 ^ num2 >> 6;
                num1++;
            }
            num2 = num2 + (num2 << 3);
            num2 = num2 ^ num2 >> 11;
            num2 = num2 + (num2 << 15);
            return num2;
        }
    }
}