using System.Collections.Generic;

namespace OctopathTraveler
{
    class TameMonsterInfo : NameValueInfo
    {
        public List<string> Types { get; private set; }
        public uint Strengh { get; private set; }
        public List<string> Skills { get; private set; }
        public string Special { get; private set; }

        public override bool CheckHeaderRow(IDictionary<string, object> row)
        {
            return base.CheckHeaderRow(row) && row.Count >= 6;
        }

        public override bool Parse(dynamic row)
        {
            if (!base.Parse((IDictionary<string, object>)row))
                return false;

            if (!uint.TryParse(row.C?.ToString(), out uint value))
                return false;

            Strengh = value;

            var types = row.D?.ToString();
            Types = !string.IsNullOrWhiteSpace(types) ? new List<string>(types.Split('/')) : new List<string>();

            var skills = row.E?.ToString();
            Skills = !string.IsNullOrWhiteSpace(skills) ? new List<string>(skills.Split('/')) : new List<string>();
            Special = row.F?.ToString();
            return true;
        }
    }
}
