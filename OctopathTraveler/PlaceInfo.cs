using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OctopathTraveler
{
    class PlaceInfo : NameValueInfo
    {
        private static readonly Regex ColorRegex = new("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");

        public string NameColor { get; private set; } = "Black";
        public string BgColor { get; private set; } = "White";

        public override bool Parse(dynamic row)
        {
            if (!base.Parse((IDictionary<string, object>)row))
                return false;

            string colors = row.C?.ToString();
            int separatorIndex = colors.IndexOf(',');
            if (separatorIndex == -1)
            {
                BgColor = ColorRegex.IsMatch(colors) ? colors : BgColor;
            }
            else
            {
                var nameColor = colors[..separatorIndex];
                var bgColor = colors[(separatorIndex + 1)..];
                NameColor = ColorRegex.IsMatch(nameColor) ? nameColor : BgColor;
                BgColor = ColorRegex.IsMatch(bgColor) ? bgColor : BgColor;
            }
            return true;
        }
    }
}
