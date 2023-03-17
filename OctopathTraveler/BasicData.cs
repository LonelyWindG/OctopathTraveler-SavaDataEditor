using System;
using System.ComponentModel;

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
            get => SaveData.Instance().ReadNumber(_hiddenPointCountAddress, 4);
            set => Util.WriteNumber(_hiddenPointCountAddress, 4, value, 0, 152);
        }

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
            PlayTime = $"{(int)playTime.TotalHours}:{playTime.Minutes}:{playTime.Seconds}";

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

            gvas.AppendValue(Util.FindFirstAddress("HiddenPointCount", achievementAddress));
            _hiddenPointCountAddress = gvas.Key("HiddenPointCount").Address;
        }
    }
}
