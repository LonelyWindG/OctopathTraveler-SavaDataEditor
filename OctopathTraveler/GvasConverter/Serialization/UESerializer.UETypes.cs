using System;
using System.IO;
using GvasFormat.Serialization.UETypes;

namespace GvasFormat.Serialization
{
  public static partial class UESerializer
  {
    internal static UEProperty Deserialize(string name, string type, long valLen, BinaryReader reader)
    {
      UEProperty result;
      var itemOffset = reader.BaseStream.Position;
      switch (type)
      {
        case "BoolProperty":
          result = new UEBoolProperty(reader, valLen);
          break;
        case "IntProperty":
          result = new UEIntProperty(reader, valLen);
          break;
        case "FloatProperty":
          result = new UEFloatProperty(reader, valLen);
          break;
        case "NameProperty":
        case "StrProperty":
          result = new UEStringProperty(reader, valLen);
          break;
        case "TextProperty":
          result = new UETextProperty(reader, valLen);
          break;
        case "EnumProperty":
          result = new UEEnumProperty(reader, valLen);
          break;
        case "StructProperty":
          result = UEStructProperty.Read(reader, valLen);
          break;
        case "ArrayProperty":
          result = new UEArrayProperty(reader, valLen);
          break;
        case "MapProperty":
          result = new UEMapProperty(reader, valLen);
          break;
        case "ByteProperty":
          result = UEByteProperty.Read(reader, valLen);
          break;
        case "UInt64Property":
          result = new UEUInt64Property(reader, valLen);
          break;
        default:
          throw new FormatException($"Offset: 0x{itemOffset:x8}. Unknown value type '{type}' of item '{name}'");
      }
      result.Name = name;
      result.Type = type;
      return result;
    }

    internal static UEProperty[] Deserialize(string name, string type, long valLen, int count, BinaryReader reader)
    {
      UEProperty[] result;
      switch (type)
      {
        case "StructProperty":
          result = UEStructProperty.Read(reader, valLen, count);
          break;
        case "ByteProperty":
          result = UEByteProperty.Read(reader, valLen, count);
          break;
        default:
          throw new FormatException($"Unknown value type '{type}' of item '{name}'");
      }
      foreach (var item in result)
      {
        item.Name = name;
        item.Type = type;
      }
      return result;
    }
  }
}