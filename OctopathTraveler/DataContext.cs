using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace OctopathTraveler
{
    class DataContext
    {
        public ObservableCollection<Member> MainParty { get; set; } = new ObservableCollection<Member>();
        public ObservableCollection<Charactor> Charactors { get; set; } = new ObservableCollection<Charactor>();
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<MissionID> MissionIDs { get; set; } = new ObservableCollection<MissionID>();
        public ObservableCollection<CountryMission> Countris { get; set; } = new ObservableCollection<CountryMission>();
        public ObservableCollection<Place> Places { get; set; } = new ObservableCollection<Place>();
        public ObservableCollection<TameMonster> TameMonsters { get; set; } = new ObservableCollection<TameMonster>();
        public ObservableCollection<EnemyWeakness> EnemyWeaknesses { get; set; } = new ObservableCollection<EnemyWeakness>();
        public ObservableCollection<TreasureState> TreasureStates { get; set; } = new ObservableCollection<TreasureState>();

        public Info Info { get; private set; } = Info.Instance();

        private readonly uint mMoneyAddress;
        private readonly uint mHeroAddress;
        public DataContext()
        {
            SaveData save = SaveData.Instance();
            foreach (var address in save.FindAddress("CharacterID_", 0))
            {
                var chara = new Charactor(address);
                if (chara.ID < 0 || chara.ID > 8) continue;
                Charactors.Add(chara);
            }

            var items = new List<Item>(Info.Items.Count);
            foreach (var address in save.FindAddress("ItemID_", 0))
            {
                var item = new Item(address);
                if (item.ID == 0)
                    continue;

                items.Add(item);
            }
            items.Sort((x, y) => x.ID.CompareTo(y.ID));
            Items = new ObservableCollection<Item>(items);

            var gvas = new GVAS(null);
            gvas.AppendValue(save.FindAddress("MainMemberID_", 0)[0]);
            for (uint i = 0; i < 4; i++)
            {
                MainParty.Add(new Member(gvas.Key("MainMemberID_" + i.ToString()).Address));
            }

            gvas = new GVAS(null);
            gvas.AppendValue(save.FindAddress("SubMissionOrder", 0)[0]);
            for (uint i = 0; i < 200; i++)
            {
                MissionIDs.Add(new MissionID(gvas.Key("SubMissionOrder_" + i.ToString())));
            }

            var missionStates = save.FindAddress("MissionState_", 0);
            var clearIndex = save.FindAddress("ClearIndex_", 0);
            if (missionStates.Count == clearIndex.Count)
            {
                for (int i = 0; i < missionStates.Count; i++)
                {
                    var stateGvas = new GVAS(null);
                    stateGvas.AppendValue(missionStates[i]);
                    var clearGvas = new GVAS(null);
                    clearGvas.AppendValue(clearIndex[i]);

                    var mission = new CountryMission() { Country = Info.Instance().Countris.Any() ? Info.Instance().Countris[i].Name : "" };
                    for (int j = 0; j < 100; j++)
                    {
                        mission.Missions.Add(new Mission(stateGvas.Key("MissionState_" + j.ToString()), clearGvas.Key("ClearIndex_" + j.ToString())));
                    }
                    Countris.Add(mission);
                }
            }

            gvas = new GVAS(null);
            gvas.AppendValue(save.FindAddress("VisitedMap", 0)[0]);
            uint id = 0;
            for (uint i = 0; i < 10; i++)
            {
                GVASData data = gvas.Key("VisitedMap_" + i.ToString());
                for (uint size = 0; size < data.Size; size++)
                {
                    for (uint bit = 0; bit < 8; bit++)
                    {
                        if (id < 12)//Ignore the first 12, as these values are all 0
                        {
                            id++;
                            continue;
                        }

                        var info = Info.Search(Info.Instance().Places, id);
                        var place = new Place(data.Address + size, bit, info?.Name ?? $"{id}(0x{id:X})({data.Address + size + bit})");
                        Places.Add(place);
                        id++;
                    }
                }
            }

            var treasures = save.FindAddress("TreasureStateArray", 0);

            //1257303 is the valid starting address for the treasure status of STEAM® save data
            //Refer to the table at https://docs.google.com/spreadsheets/d/1WGN0166crI5IbnJ4QADnLiNHrL2FUr0MVFqmWH7dBRg
            //Chinese description https://tieba.baidu.com/p/7822253075
            int diff = treasures.Count > 12 ? checked((int)treasures[12] - 1257303) : 0;
            for (int i = 0; i < treasures.Count; i++)
            {
                if (i < 12)//Ignore the first 12, as these values are all 0
                    continue;

                //gvas = new GVAS(null);
                //gvas.AppendValue(treasures[i]);
                uint tid = checked((uint)((int)treasures[i] - diff));
                var info = Info.Search(Info.Instance().TreasureStates, tid);
                if (info == null)
                    continue;

                uint treausreAddress = treasures[i] + 100;
                if (tid == 1277013)
                {
                    Trace.Write($"{i + 1}/{treasures.Count}");
                }

                //var data = gvas.Key("TreasureStateArray_" + i);
                var treasure = new TreasureState(treausreAddress, info);
                TreasureStates.Add(treasure);
            }

            gvas = new GVAS(null);
            uint tame = save.FindAddress("TameMonsterData", 0)[0];
            for (uint i = 0; i < 10; i++)
            {
                uint enemyAddress = save.FindAddress("EnemyID_", tame)[0];
                TameMonsters.Add(new TameMonster(enemyAddress));
                tame = enemyAddress + 1;
            }

            uint weaks = save.FindAddress("EnemyInfoData", 0)[0];
            Console.WriteLine(save.FindAddress("IsAnalyse_", 0).Count);
            List<uint> isAnalyseList = save.FindAddress("IsAnalyse_", 0);

            var lastEnemy = Info.Instance().Enemies.LastOrDefault();
            int maxNum = lastEnemy == null ? -1 : (int)lastEnemy.Value;
            for (int num = 0; num < Math.Min(isAnalyseList.Count, maxNum + 1); num++)
            {
                //if (num == 422) break;
                var info = Info.Search(Info.Instance().Enemies, checked((uint)num));
                if (info == null)
                    continue;

                uint i = isAnalyseList[num];
                EnemyWeaknesses.Add(new EnemyWeakness(i, num, info));
            }

            gvas = new GVAS(null);
            gvas.AppendValue(save.FindAddress("Money_", 0)[0]);
            mMoneyAddress = gvas.Key("Money").Address;
            gvas = new GVAS(null);
            gvas.AppendValue(save.FindAddress("FirstSelectCharacterID", 0)[0]);
            mHeroAddress = gvas.Key("FirstSelectCharacterID").Address;
        }

        public uint Money
        {
            get { return SaveData.Instance().ReadNumber(mMoneyAddress, 4); }
            set { Util.WriteNumber(mMoneyAddress, 4, value, 0, 9999999); }
        }

        public uint Hero
        {
            get { return SaveData.Instance().ReadNumber(mHeroAddress, 4); }
            set { SaveData.Instance().WriteNumber(mHeroAddress, 4, value); }
        }
    }
}
