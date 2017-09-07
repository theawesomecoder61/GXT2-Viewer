using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GXT2_Viewer
{
    public class GXT2
    {
        public const int Header = 1196971058;

        public Dictionary<uint, byte[]> DataItems;

        public GXT2()
        {
            DataItems = new Dictionary<uint, byte[]>();
        }

        public GXT2(Stream xIn)
        {
            DataItems = new Dictionary<uint, byte[]>();
            BinaryReader binaryReader = new BinaryReader(xIn, Encoding.UTF8);
            binaryReader.ReadInt32();
            int num = binaryReader.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                uint num1 = binaryReader.ReadUInt32();
                int num2 = binaryReader.ReadInt32();
                long position = binaryReader.BaseStream.Position;
                binaryReader.BaseStream.Position = num2;
                List<byte> nums = new List<byte>();
                while (true)
                {
                    byte num3 = binaryReader.ReadByte();
                    if (num3 == 0)
                    {
                        break;
                    }
                    nums.Add(num3);
                }
                binaryReader.BaseStream.Position = position;
                DataItems.Add(num1, nums.ToArray());
            }
        }

        public void AddStringItem(string name, string str)
        {
            DataItems.Add(Utils.GetHash(name), Encoding.UTF8.GetBytes(str));
        }

        public void AddStringItem(uint hash, string str)
        {
            DataItems.Add(hash, Encoding.UTF8.GetBytes(str));
        }

        public void WriteToStream(Stream xOut)
        {
            BinaryWriter binaryWriter = new BinaryWriter(xOut, Encoding.UTF8);
            binaryWriter.Write(1196971058);
            binaryWriter.Write(DataItems.Count);
            long position = binaryWriter.BaseStream.Position;
            foreach (KeyValuePair<uint, byte[]> dataItem in DataItems)
            {
                binaryWriter.Write(dataItem.Key);
                binaryWriter.Write(0);
            }
            binaryWriter.Write(1196971058);
            long num = binaryWriter.BaseStream.Position;
            binaryWriter.Write(0);
            int num1 = 0;
            foreach (KeyValuePair<uint, byte[]> keyValuePair in DataItems)
            {
                byte[] value = keyValuePair.Value;
                long position1 = binaryWriter.BaseStream.Position;
                binaryWriter.BaseStream.Position = position + num1 * 8 + 4;
                binaryWriter.Write((int)position1);
                binaryWriter.BaseStream.Position = position1;
                binaryWriter.Write(value, 0, (int)value.Length);
                binaryWriter.Write((byte)0);
                num1++;
            }
            long position2 = binaryWriter.BaseStream.Position;
            binaryWriter.BaseStream.Position = num;
            binaryWriter.Write((int)position2);
            binaryWriter.Flush();
        }
    }
}