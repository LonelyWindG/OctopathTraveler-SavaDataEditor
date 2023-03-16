using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using GvasFormat.Utils;
using System.Collections.Generic;

namespace GvasFormat.Serialization.UETypes
{
    public abstract class UEProperty
    {
        public string Name;
        public string Type;
        public long Offset;
        public virtual object ToObject() => this;
        public static UEProperty Read(BinaryReader br)
        {
            if (br.PeekChar() < 0)
                return null;

            var name = br.ReadUEString();
            if (name == null || name == "None") return null;

            var type = br.ReadUEString();
            var valLen = br.ReadInt64();
            return UESerializer.Deserialize(name, type, valLen, br);
        }

        public static UEProperty[] Read(BinaryReader br, int count)
        {
            if (br.PeekChar() < 0)
                return null;

            var name = br.ReadUEString();
            var type = br.ReadUEString();
            var valLen = br.ReadInt64();
            return UESerializer.Deserialize(name, type, valLen, count, br);
        }
    }

    [DebuggerDisplay("{Value}", Name = "{Name}")]
    public sealed class UEBoolProperty : UEProperty
    {
        public bool Value;
        public UEBoolProperty(BinaryReader br, long valLen)
        {
            Offset = br.BaseStream.Position;
            var val = valLen == -1 ? br.ReadByte() : br.ReadInt16();
            if (val == 0)
                Value = false;
            else if (val == 1)
                Value = true;
            else
                throw new InvalidOperationException($"Offset: 0x{br.BaseStream.Position - 1:x8}. Expected bool value, but was {val}");
        }

        public override object ToObject()
        {
            if (Name == null)
                return Value;
            return new { Offset, Name, Value };
        }
    }

    [DebuggerDisplay("{Value}", Name = "{Name}")]
    public sealed class UEFloatProperty : UEProperty
    {
        public UEFloatProperty() { }
        public UEFloatProperty(BinaryReader br, long valLen)
        {
            br.Terminator();
            Offset = br.BaseStream.Position;
            Value = br.ReadSingle();
        }

        public float Value;
        public override object ToObject()
        {
            if (Name == null)
                return Value;

            return new { Offset, Name, Value };
        }
    }

    [DebuggerDisplay("{Value}", Name = "{Name}")]
    public sealed class UEIntProperty : UEProperty
    {
        public UEIntProperty() { }
        public UEIntProperty(BinaryReader br, long valLen)
        {
            // -1 来自array
            if (valLen > -1)
                br.Terminator();

            Offset = br.BaseStream.Position;
            Value = br.ReadInt32();
        }

        public int Value;
        public override object ToObject()
        {
            if (Name == null)
                return Value;

            return new { Offset, Name, Value };
        }
    }

    [DebuggerDisplay("{Value}", Name = "{Name}")]
    public sealed class UEUInt64Property : UEProperty
    {
        public UEUInt64Property() { }
        public UEUInt64Property(BinaryReader br, long valLen)
        {
            // valLen = -1 来自 array
            if (valLen > -1)
                br.Terminator();

            Offset = br.BaseStream.Position;
            Value = br.ReadUInt64();
        }

        public ulong Value;
        public override object ToObject()
        {
            if (Name == null)
                return Value;

            return new { Offset, Name, Value };
        }
    }

    [DebuggerDisplay("Count = {Items.Length}", Name = "{Name}")]
    public sealed class UEArrayProperty : UEProperty
    {
        public string ItemType;
        public UEProperty[] Items;
        public UEArrayProperty(BinaryReader br, long valLen)
        {
            ItemType = br.ReadUEString();
            br.Terminator();

            // valLen 从这里开始
            var count = br.ReadInt32();
            Items = new UEProperty[count];

            // 定位到第一个元素位置
            Offset = br.BaseStream.Position;

            switch (ItemType)
            {
                case "StructProperty":
                    Items = Read(br, count);
                    break;
                case "ByteProperty":
                    Items = UEByteProperty.Read(br, valLen, count);
                    break;
                default:
                    {
                        for (var i = 0; i < count; i++)
                            Items[i] = UESerializer.Deserialize(null, ItemType, -1, br);
                        break;
                    }
            }
        }
        public override object ToObject()
        {
            // 优化json输出,如果觉得数据有用也可以输出
            var items = Items.Length >= 50 ? new string[] { "..." } : Items.Select(it => it.ToObject());
            return new { Offset, Name, ItemType, Items.Length, Items = items };
        }
    }

    [DebuggerDisplay("{Value}", Name = "{Name}")]
    public sealed class UEByteProperty : UEProperty
    {
        public UEByteProperty() { }
        public static UEByteProperty Read(BinaryReader br, long valLen)
        {
            if (valLen == 1)
            {
                br.ReadUEString(); // None
                br.ReadInt16();// 不清楚
                return new UEByteProperty { Value = "" };
            }

            if (br.PeekChar() == 0)
            {
                br.Terminator();
                // valLen starts here
                var arrayLength = br.ReadInt32();
                var bytes = br.ReadBytes(arrayLength);
                return new UEByteProperty { Value = bytes.AsHex() };
            }
            else
            {
                var str = "";
                while (true)
                {
                    str += br.ReadUEString();
                    if (br.PeekChar() != 0)
                        break;
                    br.ReadByte(); // 0
                }
                return new UEByteProperty { Value = str };
            }

        }

        public static UEProperty[] Read(BinaryReader br, long valLen, int count)
        {
            var Offset = br.BaseStream.Position;
            if (sizeof(int) + count == valLen)
            {
                // 纯字节
                var bytes = br.ReadBytes(count);
                return new UEProperty[] { new UEByteProperty { Offset = Offset, Value = bytes.AsHex() } };
            }
            else
            {
                // 虽然itemtype是ByteProperty，单数数据是字符串
                var r = new UEByteProperty[count];
                for (int i = 0; i < count; i++)
                    r[i] = new UEByteProperty { Offset = Offset, Value = br.ReadUEString() };
                return r;
            }
        }

        public string Value;

        public override object ToObject()
        {
            if (Name == null)
                return Value;
            return new { Offset, Name, Value };
        }
    }

    [DebuggerDisplay("{Value}", Name = "{Name}")]
    public sealed class UEDateTimeStructProperty : UEStructProperty
    {
        public DateTime Value;
        public UEDateTimeStructProperty(BinaryReader br)
        {
            Offset = br.BaseStream.Position;
            Value = DateTime.FromBinary(br.ReadInt64());
        }
        public override object ToObject() => Value;
    }

    [DebuggerDisplay("{Value}", Name = "{Name}")]
    public sealed class UEEnumProperty : UEProperty
    {
        public string EnumType;
        public string Value;
        public UEEnumProperty(BinaryReader br, long valLen)
        {
            EnumType = br.ReadUEString();
            br.Terminator();
            // valLen 从这里开始
            Offset = br.BaseStream.Position;
            Value = br.ReadUEString();
        }
    }

    [DebuggerDisplay("Count = {Properties.Count}", Name = "{Name}")]
    public sealed class UEGenericStructProperty : UEStructProperty
    {
        public List<UEProperty> Properties = new List<UEProperty>();

        public override object ToObject()
        {
            var _Properties = Properties.Select(it => it.ToObject());
            if (Name == null)
                return _Properties;

            return new { Offset, Name, StructType, Properties = _Properties };
        }
    }
    [DebuggerDisplay("{Value}", Name = "{Name}")]
    public sealed class UEGuidStructProperty : UEStructProperty
    {
        public Guid Value;
        public UEGuidStructProperty(BinaryReader br)
        {
            Offset = br.BaseStream.Position;
            Value = new Guid(br.ReadBytes(16));
        }
    }

    [DebuggerDisplay("R = {R}, G = {G}, B = {B}, A = {A}", Name = "{Name}")]
    public sealed class UELinearColorStructProperty : UEStructProperty
    {
        public float R, G, B, A;
        public UELinearColorStructProperty(BinaryReader br)
        {
            Offset = br.BaseStream.Position;
            R = br.ReadSingle();
            G = br.ReadSingle();
            B = br.ReadSingle();
            A = br.ReadSingle();
        }
    }

    [DebuggerDisplay("Count = {Map.Count}", Name = "{Name}")]
    public sealed class UEMapProperty : UEProperty
    {
        public UEMapProperty(BinaryReader br, long valLen)
        {
            var keyType = br.ReadUEString();
            var valueType = br.ReadUEString();
            var unknown = br.ReadBytes(5);
            if (unknown.Any(b => b != 0))
                throw new InvalidOperationException($"Offset: 0x{br.BaseStream.Position - 5:x8}. Expected ??? to be 0, but was 0x{unknown.AsHex()}");

            var count = br.ReadInt32();

            Offset = br.BaseStream.Position;
            for (var i = 0; i < count; i++)
            {
                UEProperty key, value;
                if (keyType == "StructProperty")
                    key = Read(br);
                else
                    key = UESerializer.Deserialize(null, keyType, -1, br);
                var values = new List<UEProperty>();
                do
                {
                    if (valueType == "StructProperty")
                        value = Read(br);
                    else
                        value = UESerializer.Deserialize(null, valueType, -1, br);
                    values.Add(value);
                } while (!(value is UENoneProperty));
                Map.Add(new UEKeyValuePair { Key = key, Values = values });
            }
        }
        public List<UEKeyValuePair> Map = new List<UEKeyValuePair>();

        public class UEKeyValuePair
        {
            public UEProperty Key;
            public List<UEProperty> Values;
        }
    }

    [DebuggerDisplay("", Name = "{Name}")]
    public sealed class UENoneProperty : UEProperty
    {
        public override object ToObject() => null;
    }

    [DebuggerDisplay("{Value}", Name = "{Name}")]
    public sealed class UEStringProperty : UEProperty
    {
        public string Value;
        public UEStringProperty(BinaryReader br, long valLen)
        {
            if (valLen > -1) br.Terminator();
            Offset = br.BaseStream.Position;
            Value = br.ReadUEString();
        }

    }
    public abstract class UEStructProperty : UEProperty
    {
        public static UEStructProperty Read(BinaryReader br, long valLen)
        {
            var type = br.ReadUEString();
            // new Guid(br.ReadBytes(16));
            br.ReadBytes(16);
            br.Terminator();
            return ReadStructValue(type, br);
        }

        public static UEStructProperty[] Read(BinaryReader br, long valLen, int count)
        {
            var type = br.ReadUEString();
            br.ReadBytes(16); // uuid
            br.Terminator();
            var result = new UEStructProperty[count];
            for (var i = 0; i < count; i++)
                result[i] = ReadStructValue(type, br);
            return result;
        }

        protected static UEStructProperty ReadStructValue(string type, BinaryReader br)
        {
            UEStructProperty result;
            var Offset = br.BaseStream.Position;
            switch (type)
            {
                case "DateTime":
                    result = new UEDateTimeStructProperty(br);
                    break;
                case "Guid":
                    result = new UEGuidStructProperty(br);
                    break;
                case "Vector":
                case "Rotator":
                    result = new UEVectorStructProperty(br);
                    break;
                case "LinearColor":
                    result = new UELinearColorStructProperty(br);
                    break;
                default:
                    var tmp = new UEGenericStructProperty();
                    while (Read(br) is UEProperty prop)
                    {
                        tmp.Properties.Add(prop);
                        if (prop is UENoneProperty)
                            break;
                    }
                    result = tmp;
                    break;
            }
            result.StructType = type;
            result.Type = type;
            result.Offset = Offset;
            return result;
        }

        public string StructType;
    }

    [DebuggerDisplay("{Value}", Name = "{Name}")]
    public sealed class UETextProperty : UEProperty
    {
        public UETextProperty(BinaryReader br, long valLen)
        {
            br.Terminator();
            // valLen starts here
            Flags = br.ReadInt64();
            br.Terminator();
            Id = br.ReadUEString();
            Offset = br.BaseStream.Position;
            Value = br.ReadUEString();
        }

        public long Flags;
        public string Id;
        public string Value;
    }

    [DebuggerDisplay("X = {X}, Y = {Y}, Z = {Z}", Name = "{Name}")]
    public sealed class UEVectorStructProperty : UEStructProperty
    {
        public UEVectorStructProperty(BinaryReader br)
        {
            Offset = br.BaseStream.Position;
            X = br.ReadSingle();
            Y = br.ReadSingle();
            Z = br.ReadSingle();
        }

        public float X, Y, Z;
    }

}
