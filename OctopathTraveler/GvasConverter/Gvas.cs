using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using GvasFormat.Serialization.UETypes;

namespace GvasFormat
{
    /*
     * General format notes:
     * Strings are 4-byte length + value + \0, length includes \0
     *
     */
    [DataContract]
    public class Gvas
    {
        public static readonly byte[] Header = Encoding.ASCII.GetBytes("GVAS");
        [DataMember(Order = 0)]
        public int SaveGameVersion;
        [DataMember(Order = 1)]
        public int PackageVersion;
        [DataMember(Order = 2)]
        public EngineVersion EngineVersion = new EngineVersion();
        [DataMember(Order = 3)]
        public int CustomFormatVersion;
        [DataMember(Order = 4)]
        public CustomFormatData CustomFormatData = new CustomFormatData();
        [DataMember(Order = 5)]
        public string SaveGameType;
        [DataMember(Order = 6)]
        public List<UEProperty> Properties = new List<UEProperty>();
    }
}
