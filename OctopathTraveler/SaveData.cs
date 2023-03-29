using System;
using System.Collections.Generic;
using System.IO;

namespace OctopathTraveler
{
    class SaveData
    {
        public static bool IsReadonlyMode = false;

        private static SaveData mThis;
        private string mFileName = null;
        private byte[] mBuffer = null;
        private readonly System.Text.Encoding mEncode = System.Text.Encoding.ASCII;
        public uint Adventure { private get; set; } = 0;
        public string FileName => mFileName;

        private SaveData()
        { }

        public static SaveData Instance()
        {
            return mThis ??= new SaveData();
        }

        public bool Open(string filename)
        {
            mFileName = filename;
            mBuffer = File.ReadAllBytes(mFileName);

            Backup();
            return true;
        }

        public bool Save()
        {
            if (IsReadonlyMode || mFileName == null || mBuffer == null) return false;
            File.WriteAllBytes(mFileName, mBuffer);
            return true;
        }

        public bool SaveAs(string filenname)
        {
            if (IsReadonlyMode || mBuffer == null) return false;
            mFileName = filenname;
            return Save();
        }

        public (bool, Exception) SaveAsJson(string filePath)
        {
            if (mBuffer == null) return (false, null);
            return GvasFormat.GvasConverter.Convert2JsonFile(filePath, new MemoryStream(mBuffer, false));
        }

        public uint ReadNumber(uint address, uint size)
        {
            if (mBuffer == null) return 0;
            address = CalcAddress(address);
            if (address + size > mBuffer.Length) return 0;
            uint result = 0;
            for (int i = 0; i < size; i++)
            {
                result += (uint)(mBuffer[address + i]) << (i * 8);
            }
            return result;
        }

        // 0 to 7.
        public bool ReadBit(uint address, uint bit)
        {
            if (bit < 0) return false;
            if (bit > 7) return false;
            if (mBuffer == null) return false;
            address = CalcAddress(address);
            if (address > mBuffer.Length) return false;
            byte mask = (byte)(1 << (int)bit);
            byte result = (byte)(mBuffer[address] & mask);
            return result != 0;
        }

        public string ReadText(uint address, uint size)
        {
            if (mBuffer == null) return "";
            address = CalcAddress(address);
            if (address + size > mBuffer.Length) return "";

            byte[] tmp = new byte[size];
            for (uint i = 0; i < size; i++)
            {
                //if (mBuffer[address + i] == 0) break;
                tmp[i] = mBuffer[address + i];
            }
            return mEncode.GetString(tmp).Trim('\0');
        }

        public void WriteNumber(uint address, uint size, uint value)
        {
            if (mBuffer == null) return;
            address = CalcAddress(address);
            if (address + size > mBuffer.Length) return;
            for (uint i = 0; i < size; i++)
            {
                mBuffer[address + i] = (byte)(value & 0xFF);
                value >>= 8;
            }
        }

        // 0 to 7.
        public void WriteBit(uint address, uint bit, bool value)
        {
            if (bit < 0) return;
            if (bit > 7) return;
            if (mBuffer == null) return;
            address = CalcAddress(address);
            if (address > mBuffer.Length) return;
            byte mask = (byte)(1 << (int)bit);
            if (value) mBuffer[address] = (byte)(mBuffer[address] | mask);
            else mBuffer[address] = (byte)(mBuffer[address] & ~mask);
        }

        public void WriteText(uint address, uint size, string value)
        {
            if (mBuffer == null) return;
            address = CalcAddress(address);
            if (address + size > mBuffer.Length) return;
            byte[] tmp = mEncode.GetBytes(value);
            Array.Resize(ref tmp, (int)size);
            for (uint i = 0; i < size; i++)
            {
                mBuffer[address + i] = tmp[i];
            }
        }

        public void Fill(uint address, uint size, byte number)
        {
            if (mBuffer == null) return;
            address = CalcAddress(address);
            if (address + size > mBuffer.Length) return;
            for (uint i = 0; i < size; i++)
            {
                mBuffer[address + i] = number;
            }
        }

        public void Copy(uint from, uint to, uint size)
        {
            if (mBuffer == null) return;
            from = CalcAddress(from);
            to = CalcAddress(to);
            if (from + size > mBuffer.Length) return;
            if (to + size > mBuffer.Length) return;
            for (uint i = 0; i < size; i++)
            {
                mBuffer[to + i] = mBuffer[from + i];
            }
        }

        public void Swap(uint from, uint to, uint size)
        {
            if (mBuffer == null) return;
            from = CalcAddress(from);
            to = CalcAddress(to);
            if (from + size > mBuffer.Length) return;
            if (to + size > mBuffer.Length) return;
            for (uint i = 0; i < size; i++)
            {
                byte tmp = mBuffer[to + i];
                mBuffer[to + i] = mBuffer[from + i];
                mBuffer[from + i] = tmp;
            }
        }

        public List<uint> FindAddress(string name, uint index)
        {
            List<uint> result = new List<uint>();
            for (; index < mBuffer.Length; index++)
            {
                if (mBuffer[index] != name[0]) continue;

                int len = 1;
                for (; len < name.Length; len++)
                {
                    if (mBuffer[index + len] != name[len]) break;
                }
                if (len >= name.Length)
                {
                    result.Add(index);
                }

                index += (uint)len;
            }
            return result;
        }

        private uint CalcAddress(uint address)
        {
            return address;
        }

        private void Backup()
        {
            if (IsReadonlyMode)
                return;

            string path = Path.Combine(Directory.GetCurrentDirectory(), "OctopathTraveler Backup");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, $"{Path.GetFileName(mFileName)}-{DateTime.Now:yyyy-MM-dd-HH-mm}");
            File.WriteAllBytes(path, mBuffer);
        }
    }
}
