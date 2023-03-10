using System;
using System.ComponentModel;
using System.Text;

namespace OctopathTraveler
{
    class TreasureState : INotifyPropertyChanged
    {
        private uint summation;
        private uint chest;
        private uint hiddenItem;
        private string value;

        //I don't know how to make the CheckBox control read-only, so I can only keep an empty set method
        public bool IsCollectAll { get => summation == Info.Summation; set { } }
        public bool IsCollectChest { get => chest == Info.Chest; set { } }
        public bool IsCollectHiddenItem { get => hiddenItem == Info.HiddenItem; set { } }

        public string SummationProgress => $"{summation}/{Info.Summation}";
        public string ChestProgress => $"{chest}/{Info.Chest}";
        public string HiddenItemProgress => $"{hiddenItem}/{Info.HiddenItem}";


        public uint Summation => summation;
        public uint Chest => chest;
        public uint HiddenItem => hiddenItem;
        public string Value => value;

        public TreasureStateInfo Info { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public TreasureState(uint address, TreasureStateInfo info)
        {
            //var sb = new StringBuilder();
            address += 3;//Ignore the first three, because these values are all 0

            uint showValue = 0;
            for (uint i = 0; i < info.Summation; i++)
            {
                uint readValue = SaveData.Instance().ReadNumber(address + i, 1);
                showValue = checked((showValue << 1) + readValue);
                if (readValue != 0)
                {
                    summation++;
                    if (i < info.Chest)
                        chest++;
                    else
                        hiddenItem++;

                }
            }

            value = Convert.ToString(showValue, 2).PadLeft((int)info.Summation, '0'); //sb.ToString().TrimEnd();
            Info = info;
        }
    }
}
