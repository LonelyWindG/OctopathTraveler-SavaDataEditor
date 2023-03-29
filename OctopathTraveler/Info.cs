using MiniExcelLibs;
using OctopathTraveler.Properties;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace OctopathTraveler
{
    class Info
    {
        public static string LoadedInfoFile { get; private set; }

        private static Info mThis;
        public List<NameValueInfo> Items { get; private set; } = new List<NameValueInfo>();
        public List<InventoryItemInfo> ItemInventory { get; private set; } = new List<InventoryItemInfo>();
        public List<NameValueInfo> CharaNames { get; private set; } = new List<NameValueInfo>();
        public List<NameValueInfo> Jobs { get; private set; } = new List<NameValueInfo>();
        public List<NameValueInfo> Equipments { get; private set; } = new List<NameValueInfo>();
        public List<NameValueInfo> Countris { get; private set; } = new List<NameValueInfo>();
        public List<PlaceInfo> Places { get; private set; } = new List<PlaceInfo>();
        public List<EnemyInfo> Enemies { get; private set; } = new List<EnemyInfo>();
        public List<TameMonsterInfo> TameMonsters { get; private set; } = new List<TameMonsterInfo>();
        public List<TreasureStateInfo> TreasureStates { get; private set; } = new List<TreasureStateInfo>();

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

        public static string GetNameOrID2Hex<TInfo>(List<TInfo> list, uint id) where TInfo : NameValueInfo, new()
        {
            return id == 0 ? "0" : Search(list, id)?.Name ?? $"{id}(0x{id:X})";
        }

        public static TType Search<TType>(List<TType> list, uint id) where TType : NameValueInfo, new()
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
            return null;
        }

        private void Init()
        {
            var culture = Resources.Culture ?? CultureInfo.CurrentUICulture;
            byte[] excel = LoadExcel(false, culture);
            excel ??= LoadExcel(false, null);
            excel ??= LoadExcel(true, culture);
            excel ??= LoadExcel(true, null);

            if (excel == null || excel.Length <= 0)
            {
                LoadedInfoFile = "Info Excel Path: NotFound";
                return;
            }

            var reader = new ExcelReader(excel);
            reader.AppendListAndOrderByValue("item", Items);
            reader.AppendList("item_inventory", ItemInventory);
            reader.AppendListAndOrderByValue("character", CharaNames);
            reader.AppendListAndOrderByValue("job", Jobs);
            reader.AppendListAndOrderByValue("equipment", Equipments);
            reader.AppendListAndOrderByValue("country", Countris);
            reader.AppendListAndOrderByValue("place", Places);
            reader.AppendListAndOrderByValue("enemy_weakness", Enemies);
            reader.AppendListAndOrderByValue("tame_monster", TameMonsters);
            reader.AppendListAndOrderByValue("treasure_states", TreasureStates);
        }

        private static byte[] LoadExcel(bool embedded, CultureInfo culture)
        {
            bool isSpecificCulture = culture != null && !culture.Name.StartsWith("en", System.StringComparison.OrdinalIgnoreCase);
            if (embedded)
            {
                var resourceSet = Resources.ResourceManager.GetResourceSet(isSpecificCulture ? culture : CultureInfo.InvariantCulture, true, false);
                LoadedInfoFile = "Info Excel Path: Embedded Resource, Language: en";
                return resourceSet == null ? null : resourceSet.GetObject("InfoExcel") as byte[];
            }
            else if (isSpecificCulture)
            {
                string file = $"info_{culture.Name.ToLower().Replace("-", "_")}.xlsx";
                if (File.Exists(file))
                {
                    LoadedInfoFile = "Info Excel Path: " + file.Replace("_", "__");
                    return File.ReadAllBytes(file);
                }
            }
            else if (File.Exists("info.xlsx"))
            {
                LoadedInfoFile = "Info Excel Path: info.xlsx";
                return File.ReadAllBytes("info.xlsx");
            }
            return null;
        }

        public static bool TryGetEmbeddedInfoExcel(out (string, byte[])[] excels)
        {
            var culture = Resources.Culture ?? CultureInfo.CurrentUICulture;
            var invariantExcel = Resources.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, false)?.GetObject("InfoExcel") as byte[];
            if (culture.Name.StartsWith("en", System.StringComparison.OrdinalIgnoreCase))
            {
                excels = new[] { ("info.xlsx", invariantExcel) };
                return true;
            }
            var cultureExcel = Resources.ResourceManager.GetResourceSet(culture, true, false)?.GetObject("InfoExcel") as byte[];
            if (invariantExcel != null && cultureExcel != null)
            {
                excels = new[]
                {
                    ($"info_{culture.Name.ToLower().Replace("-", "_")}.xlsx", cultureExcel),
                    ("info.xlsx", invariantExcel)
                };
                return true;
            }

            if (invariantExcel == null && cultureExcel == null)
            {
                excels = null;
                return false;
            }

            if (cultureExcel != null)
            {
                excels = new[] { ($"info_{culture.Name.ToLower().Replace("-", "_")}.xlsx", cultureExcel) };
                return true;
            }
            excels = new[] { ($"info.xlsx", cultureExcel) };
            return true;
        }

        private class ExcelReader
        {
            private readonly MemoryStream _stream;
            private readonly HashSet<string> _sheets;

            public ExcelReader(byte[] excel)
            {
                _stream = new MemoryStream(excel, false);
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
