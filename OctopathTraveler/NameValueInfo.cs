using System;
using System.Collections.Generic;

namespace OctopathTraveler
{
    class NameValueInfo : IRowParser
    {
        public uint Value { get; private set; }
        public string Name { get; private set; }

        public virtual bool CheckHeaderRow(IDictionary<string, object> row)
        {
            if (row.Count < 2 || !row.TryGetValue("A", out var id))
                return false;

            if (id is string content && (content == "#ID" || content == "ID"))
                return true;

            return false;
        }

        public virtual bool Parse(dynamic row)
        {
            var cell0 = row.A?.ToString();
            if (cell0.Length >= 3 && cell0[0] == '0' && cell0[1] == 'x')
            {
                Value = Convert.ToUInt32(cell0, 16);
            }
            else
            {
                Value = Convert.ToUInt32(cell0);
            }
            Name = row.B?.ToString();
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
