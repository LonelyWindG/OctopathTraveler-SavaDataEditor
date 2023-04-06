using System;
using System.Diagnostics;
using System.Windows.Data;

namespace OctopathTraveler
{
    class BasicData
    {
        private readonly uint _moneyAddress;
        private readonly uint _heroAddress;
        private readonly uint _battleCountAddress;
        private readonly uint _escapeCountAddress;
        private readonly uint _treasureCountAddress;
        private readonly uint _hiddenPointCountAddress;

        public string SaveDate { get; }

        public string PlayTime { get; }

        public uint Money
        {
            get { return SaveData.Instance().ReadNumber(_moneyAddress, 4); }
            set { Util.WriteNumber(_moneyAddress, 4, value, 0, 9999999); }
        }

        public uint Hero
        {
            get => SaveData.Instance().ReadNumber(_heroAddress, 4);
            set => Util.WriteNumber(_heroAddress, 4, value, 1, 8);
        }

        public uint BattleCount
        {
            get => SaveData.Instance().ReadNumber(_battleCountAddress, 4);
            set => Util.WriteNumber(_battleCountAddress, 4, value, 0, 9999999);
        }

        public uint EscapeCount
        {
            get => SaveData.Instance().ReadNumber(_escapeCountAddress, 4);
            set => Util.WriteNumber(_escapeCountAddress, 4, value, 0, 9999999);
        }

        public uint TreasureCount
        {
            get => SaveData.Instance().ReadNumber(_treasureCountAddress, 4);
            set => Util.WriteNumber(_treasureCountAddress, 4, value, 0, 734);
        }

        public uint HiddenPointCount
        {
            get => IsExistHiddenPointCount ? SaveData.Instance().ReadNumber(_hiddenPointCountAddress, 4) : 0;
            set
            {
                if (IsExistHiddenPointCount)
                    return;

                Util.WriteNumber(_hiddenPointCountAddress, 4, value, 0, 152);
            }
        }

        public bool IsExistHiddenPointCount { get; private set; }

        public string ItemCount { get; private set; } = "0/579";

        public BasicData()
        {
            var save = SaveData.Instance();
            var gvas = new GVAS(null);

            uint soltDataAddress = Util.FindFirstAddress("slotData", 0);
            foreach (var address in save.FindAddress("SaveDate_", soltDataAddress))
            {
                gvas.AppendValue(address, false);
            }
            SaveDate = new DateTime((int)gvas.ReadNumber("SaveDate_Year"), (int)gvas.ReadNumber("SaveDate_Month"), (int)gvas.ReadNumber("SaveDate_Day"),
                (int)gvas.ReadNumber("SaveDate_Hour"), (int)gvas.ReadNumber("SaveDate_Minute"), (int)gvas.ReadNumber("SaveDate_Second")).ToString();

            gvas.AppendValue(Util.FindFirstAddress("PlayTime", soltDataAddress));
            var playTime = new TimeSpan(gvas.ReadNumber("PlayTime") * TimeSpan.TicksPerSecond);
            PlayTime = $"{(int)playTime.TotalHours}:{playTime.Minutes:D2}:{playTime.Seconds:D2}";

            gvas.AppendValue(Util.FindFirstAddress("Money_", 0));
            _moneyAddress = gvas.Key("Money").Address;

            gvas.AppendValue(Util.FindFirstAddress("FirstSelectCharacterID", 0));
            _heroAddress = gvas.Key("FirstSelectCharacterID").Address;

            uint achievementAddress = Util.FindFirstAddress("Achievement", 0);
            gvas.AppendValue(Util.FindFirstAddress("BattleCount", achievementAddress));
            _battleCountAddress = gvas.Key("BattleCount").Address;

            gvas.AppendValue(Util.FindFirstAddress("EscapeCount", achievementAddress));
            _escapeCountAddress = gvas.Key("EscapeCount").Address;

            gvas.AppendValue(Util.FindFirstAddress("TreasureCount", achievementAddress));
            _treasureCountAddress = gvas.Key("TreasureCount").Address;

            IsExistHiddenPointCount = Util.TryFindFirstAddress("HiddenPointCount", achievementAddress, out _hiddenPointCountAddress);
            if (IsExistHiddenPointCount)
            {
                gvas.AppendValue(_hiddenPointCountAddress);
                _hiddenPointCountAddress = gvas.Key("HiddenPointCount").Address;
            }

            gvas = new GVAS(null);
            gvas.AppendValue(Util.FindFirstAddress("ItemFlag", achievementAddress));

            int itemCount = 0;
            int flagIndex = 0;
            string flagKey = "ItemFlag_" + flagIndex;
            while (gvas.HasKey(flagKey))
            {
                uint flags = gvas.ReadNumber(flagKey);
                for (int i = 0; i < 32; i++)
                {
                    if (((flags >> i) & 1) == 1)
                    {
                        itemCount++;
                    }
                }

                flagIndex++;
                flagKey = "ItemFlag_" + flagIndex;
            }

            ItemCount = itemCount + "/579";
        }
    }
}
