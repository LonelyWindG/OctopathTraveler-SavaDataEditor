using System;
using System.IO;
using System.Text;

namespace GvasFormat.Serialization
{
  public static class BinaryReaderEx
  {
    private static readonly Encoding Utf8 = new UTF8Encoding(false);

    public static void Terminator(this BinaryReader reader)
    {
      var terminator = reader.ReadByte();
      if (terminator != 0)
      {
        throw new FormatException($"Offset: 0x{reader.BaseStream.Position - 1:x8}. Expected terminator (0x00), but was (0x{terminator:x2})");
      }
    }

    public static string ReadUEString(this BinaryReader reader)
    {
      if (reader.PeekChar() < 0)
        return null;

      // ue字符串通常以0结尾，length包含null
      var lengthOffset = reader.BaseStream.Position;
      var length = reader.ReadInt32();
      if (length == 0)
        return null;

      if (length == 1)
        return "";

      var valueBytes = new byte[length];

      int i = 0;
      for (; i < length; i++)
      {
        var b = reader.ReadByte();
        if (b == 0) break;
        valueBytes[i] = b;
      }

      var str = Utf8.GetString(valueBytes, 0, length - 1);

      // 如果读出来和length不一样，那么肯定是哪里分析错了
      if (length != str.Length + 1)
        throw new FormatException($"Offset: 0x{lengthOffset:x8} read string error.");

      return str;
    }
  }
}
