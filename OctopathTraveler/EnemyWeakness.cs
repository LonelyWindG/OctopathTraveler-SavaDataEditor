using System;
using System.ComponentModel;

namespace OctopathTraveler
{
    class EnemyWeakness : INotifyPropertyChanged
    {
        private const int WeakNumBit = 6;

        private static readonly string[] WeaponWeakProps = new string[WeakNumBit]
        {
            nameof(Sword), nameof(Lance), nameof(Dagger), nameof(Axe), nameof(Bow), nameof(Rod)
        };

        private static readonly string[] MagicWeakProps = new string[WeakNumBit]
        {
            nameof(Fire), nameof(Ice), nameof(Thunder), nameof(Wind), nameof(Light), nameof(Dark)
        };

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly uint mWeaponAddress;
        private readonly uint mMagicAddress;

        public int Num { get; }

        public EnemyInfo Info { get; }

        public EnemyWeakness(uint address, int num, EnemyInfo enermyInfo)
        {
            var gvas = new GVAS(null);
            gvas.AppendValue(SaveData.Instance().FindAddress("WeaknessOpen_", address)[0]);
            mWeaponAddress = gvas.Key("WeaknessOpen").Address;
            mMagicAddress = mWeaponAddress + 1;

            Num = num;
            Info = enermyInfo ?? throw new ArgumentNullException(nameof(enermyInfo));
        }

        public bool IsBreakAll
        {
            get
            {
                bool isBreakWeapon = SaveData.Instance().ReadNumber(mWeaponAddress, 1) == Info.WeaponWeak;
                return isBreakWeapon && SaveData.Instance().ReadNumber(mMagicAddress, 1) == Info.MagicWeak;
            }
            set
            {
                SaveData.Instance().WriteNumber(mWeaponAddress, 1, value ? Info.WeaponWeak : 0);
                SaveData.Instance().WriteNumber(mMagicAddress, 1, value ? Info.MagicWeak : 0);

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBreakAll)));
                for (int i = 0; i < WeakNumBit; i++)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(WeaponWeakProps[i]));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(MagicWeakProps[i]));
                }
            }
        }

        public bool Sword
        {
            get { return SaveData.Instance().ReadBit(mWeaponAddress, 0); }
            set
            {
                SetWeaponWeakProp(0, value);
            }
        }

        public bool Lance
        {
            get { return SaveData.Instance().ReadBit(mWeaponAddress, 1); }
            set
            {
                SetWeaponWeakProp(1, value);
            }
        }

        public bool Dagger
        {
            get { return SaveData.Instance().ReadBit(mWeaponAddress, 2); }
            set
            {
                SetWeaponWeakProp(2, value);
            }
        }

        public bool Axe
        {
            get { return SaveData.Instance().ReadBit(mWeaponAddress, 3); }
            set
            {
                SetWeaponWeakProp(3, value);
            }
        }

        public bool Bow
        {
            get { return SaveData.Instance().ReadBit(mWeaponAddress, 4); }
            set
            {
                SetWeaponWeakProp(4, value);
            }
        }

        public bool Rod
        {
            get { return SaveData.Instance().ReadBit(mWeaponAddress, 5); }
            set
            {
                SetWeaponWeakProp(5, value);
            }
        }

        public bool Fire
        {
            get { return SaveData.Instance().ReadBit(mMagicAddress, 0); }
            set
            {
                SetMagicWeakProp(0, value);
            }
        }

        public bool Ice
        {
            get { return SaveData.Instance().ReadBit(mMagicAddress, 1); }
            set
            {
                SetMagicWeakProp(1, value);
            }
        }

        public bool Thunder
        {
            get { return SaveData.Instance().ReadBit(mMagicAddress, 2); }
            set
            {
                SetMagicWeakProp(2, value);
            }
        }

        public bool Wind
        {
            get { return SaveData.Instance().ReadBit(mMagicAddress, 3); }
            set
            {
                SetMagicWeakProp(3, value);
            }
        }

        public bool Light
        {
            get { return SaveData.Instance().ReadBit(mMagicAddress, 4); }
            set
            {
                SetMagicWeakProp(4, value);
            }
        }

        public bool Dark
        {
            get { return SaveData.Instance().ReadBit(mMagicAddress, 5); }
            set
            {
                SetMagicWeakProp(5, value);
            }
        }

        private void SetWeaponWeakProp(uint bit, bool value)
        {
            SaveData.Instance().WriteBit(mWeaponAddress, bit, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(WeaponWeakProps[(int)bit]));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBreakAll)));
        }

        private void SetMagicWeakProp(uint bit, bool value)
        {
            SaveData.Instance().WriteBit(mMagicAddress, bit, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(MagicWeakProps[(int)bit]));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBreakAll)));
        }
    }
}
