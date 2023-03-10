using System;
using System.Collections.Generic;

namespace OctopathTraveler
{
    class TreasureStateInfo : NameValueInfo
    {
        public uint Summation { get; private set; }
        public uint Chest { get; private set; }
        public uint HiddenItem { get; private set; }

        public override bool CheckHeaderRow(IDictionary<string, object> row)
        {
            return base.CheckHeaderRow(row) && row.Count >= 5;
        }

        public override bool Parse(dynamic row)
        {
            if (!base.Parse((IDictionary<string, object>)row))
                return false;

            string num = (string)(row.C?.ToString());
            if (num == "0" || string.IsNullOrWhiteSpace(num))
                return false;

            Summation = Convert.ToUInt32(num);

            num = (string)(row.D?.ToString());
            Chest = string.IsNullOrWhiteSpace(num) ? 0 : Convert.ToUInt32(num);

            num = (string)(row.E?.ToString());
            HiddenItem = string.IsNullOrWhiteSpace(num) ? 0 : Convert.ToUInt32(num);
            if (Summation != Chest + HiddenItem)
            {
                string msg = $"Bad treasure state line, ID: {Value}, Name: {Name}, " +
                    $"Sumation: {Summation}, Chest: {Chest}, HidenItem: {HiddenItem}, plase check excel file";
                throw new ArgumentException(msg);
            }

            return true;
        }
    }
}
