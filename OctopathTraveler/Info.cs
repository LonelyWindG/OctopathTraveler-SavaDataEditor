using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using MiniExcelLibs;

namespace OctopathTraveler
{
    class Info
    {
        private static Info mThis;
        public List<NameValueInfo> Items { get; private set; } = new List<NameValueInfo>();
        public List<NameValueInfo> CharaNames { get; private set; } = new List<NameValueInfo>();
        public List<NameValueInfo> Jobs { get; private set; } = new List<NameValueInfo>();
        public List<NameValueInfo> Equipments { get; private set; } = new List<NameValueInfo>();
        public List<NameValueInfo> Countris { get; private set; } = new List<NameValueInfo>();
        public List<NameValueInfo> Places { get; private set; } = new List<NameValueInfo>();
        public List<EnemyInfo> Enemies { get; private set; } = new List<EnemyInfo>();
        public List<TameMonsterInfo> TameMonsters { get; private set; } = new List<TameMonsterInfo>();

        private readonly Dictionary<uint, NameValueInfo> _unknownNames = new();

        private Info() { }

        public static Info Instance()
        {
            if (mThis == null)
            {
                mThis = new Info();
                mThis.Init();
            }
            return mThis;
        }

        public NameValueInfo Search<TType>(List<TType> list, uint id, bool returnIdNameIfNotExist = true)
            where TType : NameValueInfo, new()
        {
            int min = 0;
            int max = list.Count;
            for (; min < max;)
            {
                int mid = (min + max) / 2;
                if (list[mid].Value == id) return list[mid];
                else if (list[mid].Value > id) max = mid;
                else min = mid + 1;
            }
            if (!returnIdNameIfNotExist)
                return null;

            if (!_unknownNames.TryGetValue(id, out var unknownName))
            {
                unknownName = NameValueInfo.CreateUnknownName(id);
                _unknownNames.Add(id, unknownName);
            }
            return unknownName;
        }

        private void Init()
        {
            string cultureName = CultureInfo.CurrentUICulture.Name.ToLower().Replace("-", "_");

            string file = $"info_{cultureName}.xlsx";
            if (!File.Exists(file))
            {
                if (!File.Exists("info.xlsx"))
                {
                    MessageBox.Show($"Can't find [info.xlsx] or [{file}] in current folder, the names of items, characters, etc. will not be displayed", "Load names failed");
                    return;
                }
                file = "info.xlsx";
            }

            var reader = new ExcelReader(file);
            reader.AppendListAndOrderByValue("item", Items);
            reader.AppendListAndOrderByValue("character", CharaNames);
            reader.AppendListAndOrderByValue("job", Jobs);
            reader.AppendListAndOrderByValue("equipment", Equipments);
            reader.AppendListAndOrderByValue("country", Countris);
            reader.AppendListAndOrderByValue("place", Places);
            reader.AppendListAndOrderByValue("enemy_weakness", Enemies);
            reader.AppendListAndOrderByValue("tame_monster", TameMonsters);
        }

        private class ExcelReader
        {
            private readonly MemoryStream _stream;
            private readonly HashSet<string> _sheets;

            public ExcelReader(string path)
            {
                _stream = new MemoryStream(File.ReadAllBytes(path), false);
                _sheets = new HashSet<string>(MiniExcel.GetSheetNames(_stream));
            }

            public void AppendListAndOrderByValue<TInfo>(string sheet, List<TInfo> list) where TInfo : NameValueInfo, new()
            {
                AppendList(sheet, list);
                list.Sort((x, y) => x.Value.CompareTo(y.Value));
            }

            public void AppendList<TRowParser>(string sheet, List<TRowParser> list) where TRowParser : IRowParser, new()
            {
                if (!_sheets.Contains(sheet))
                    return;

                bool checkHeaderRow = true;
                var rows = MiniExcel.Query(_stream, sheetName: sheet);
                foreach (var row in rows)
                {
                    if (checkHeaderRow)
                    {
                        if (!new TRowParser().CheckHeaderRow(row))
                            break;

                        checkHeaderRow = false;
                        continue;
                    }

                    var parser = new TRowParser();
                    if (parser.Parse(row))
                    {
                        list.Add(parser);
                    }
                }
            }
        }
    }
}
