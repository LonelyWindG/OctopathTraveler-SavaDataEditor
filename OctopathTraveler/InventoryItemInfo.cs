using System;
using System.Collections.Generic;
using static OctopathTraveler.Properties.Resources;

namespace OctopathTraveler
{
    class InventoryItemInfo : NameValueInfo
    {
        public string TypeAndIndex { get; private set; }

        public bool IsOwned { get; set; }

        public override bool CheckHeaderRow(IDictionary<string, object> row)
        {
            return base.CheckHeaderRow(row) && row.Count >= 4;
        }

        public override bool Parse(dynamic row)
        {
            if (!base.Parse((IDictionary<string, object>)row))
                return false;

            int type = Convert.ToInt32(row.C);
            string typeAndIndex = type switch
            {
                1 => TabItemItems,
                2 => ItemSword,
                3 => ItemLance,
                4 => ItemDagger,
                5 => ItemAxe,
                6 => ItemBow,
                7 => ItemRod,
                8 => ItemShield,
                9 => ItemHead,
                10 => ItemBody,
                11 => ItemAccessory,
                12 => ItemMaterial,
                13 => ItemValuable,
                14 => ItemKnowledge,
                _ => "TYPE" + type
            };

            TypeAndIndex = $"{typeAndIndex} - {row.D}";
            return true;
        }
    }
}
