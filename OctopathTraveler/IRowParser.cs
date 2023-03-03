using System.Collections.Generic;

namespace OctopathTraveler
{
    interface IRowParser
    {
        bool CheckHeaderRow(IDictionary<string, object> row);

        bool Parse(dynamic row);
    }
}
