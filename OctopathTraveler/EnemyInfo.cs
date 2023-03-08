using System;
using System.Collections.Generic;
using System.Linq;

namespace OctopathTraveler
{
    class EnemyInfo : NameValueInfo
    {
        public uint WeaponWeak { get; set; }

        public uint MagicWeak { get; set; }

        public bool IsEnableSword { get; set; }

        public bool IsEnableLance { get; set; }

        public bool IsEnableDagger { get; set; }

        public bool IsEnableAxe { get; set; }

        public bool IsEnableBow { get; set; }

        public bool IsEnableRod { get; set; }

        public bool IsEnableFire { get; set; }

        public bool IsEnableIce { get; set; }

        public bool IsEnableThunder { get; set; }

        public bool IsEnableWind { get; set; }

        public bool IsEnableLight { get; set; }

        public bool IsEnableDark { get; set; }

        public override bool CheckHeaderRow(IDictionary<string, object> row)
        {
            return base.CheckHeaderRow(row) && row.Count >= 4;
        }

        public override bool Parse(dynamic row)
        {
            if (!base.Parse((IDictionary<string, object>)row))
                return false;

            if (string.IsNullOrEmpty(row.C) || string.IsNullOrEmpty(row.D))
                return false;

            SetWeak(ReadWeakNum(row.C), ReadWeakNum(row.D));
            return true;
        }

        private static uint ReadWeakNum(dynamic num)
        {
            string str = (string)num.ToString();
            if (str == "0" || str == "000000")
                return 0;

            return Convert.ToUInt32(str, 2);
        }

        private void SetWeak(uint weaponWeak, uint magicWeak)
        {
            WeaponWeak = weaponWeak;
            MagicWeak = magicWeak;
            if (weaponWeak != 0)
            {
                IsEnableSword = IsEnableWeaponWeak(0);
                IsEnableLance = IsEnableWeaponWeak(1);
                IsEnableDagger = IsEnableWeaponWeak(2);
                IsEnableAxe = IsEnableWeaponWeak(3);
                IsEnableBow = IsEnableWeaponWeak(4);
                IsEnableRod = IsEnableWeaponWeak(5);
            }

            if (magicWeak != 0)
            {
                IsEnableFire = IsEnableMagicWeak(0);
                IsEnableIce = IsEnableMagicWeak(1);
                IsEnableThunder = IsEnableMagicWeak(2);
                IsEnableWind = IsEnableMagicWeak(3);
                IsEnableLight = IsEnableMagicWeak(4);
                IsEnableDark = IsEnableMagicWeak(5);
            }
        }

        private bool IsEnableWeaponWeak(int bit)
        {
            return ((1 << bit) & WeaponWeak) != 0;
        }

        private bool IsEnableMagicWeak(int bit)
        {
            return ((1 << bit) & MagicWeak) != 0;
        }
    }
}
