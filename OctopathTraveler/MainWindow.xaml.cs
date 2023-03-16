using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using static OctopathTraveler.Properties.Resources;

namespace OctopathTraveler
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
#if !DEBUG
            Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
#endif
            InitializeComponent();
            TopBar_ButtonSave.Visibility = SaveData.IsReadonlyMode ? Visibility.Collapsed : Visibility.Visible;
        }

        private static void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                if (e.Handled)
                    return;

                HandleException(e.Exception);
            }
            finally
            {
                e.Handled = true;
            }
        }

        private static void HandleException(Exception exception)
        {
            string ex = exception.ToString();
            try
            {
                File.AppendAllText("exception.log", DateTime.Now.ToString() + "\n" + ex + "\n");
            }
            catch (Exception)
            {
            }
            MessageBox.Show(ex, "Program Exception");
        }

        private void Window_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is not string[] files) return;
            if (!File.Exists(files[0])) return;

            SaveData.Instance().Open(files[0]);
            Init();
            MessageBox.Show(MessageLoadSuccess);
        }

        private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e)
        {
            Load(false);
        }

        private void MenuItemFileSave_Click(object sender, RoutedEventArgs e)
        {
            if (SaveData.IsReadonlyMode)
            {
                MessageBox.Show(MeaageSaveFail, "ReadonlyMode");
                return;
            }
            Save();
        }

        private void MenuItemFileSaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (SaveData.IsReadonlyMode)
            {
                MessageBox.Show(MeaageSaveFail, "ReadonlyMode");
                return;
            }

            var dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == false) return;

            if (SaveData.Instance().SaveAs(dlg.FileName) == true) MessageBox.Show(MessageSaveSuccess);
            else MessageBox.Show(MeaageSaveFail, SaveData.IsReadonlyMode ? "ReadonlyMode" : "");
        }

        private void MenuItemFileSaveAsJson_Click(object sender, RoutedEventArgs e)
        {
            ConvertSaveDataToJson(false);
        }

        private void MenuItemExportInfoExcel_Click(object sender, RoutedEventArgs e)
        {
            if (!Info.TryGetEmbeddedInfoExcel(out var excels))
            {
                MessageBox.Show(MeaageSaveFail);
                return;
            }

            bool isSaveAny = false;
            foreach ((var fileName, var bytes) in excels)
            {
                string ext = Path.GetExtension(fileName);
                string v = $"Excel files (*{ext})|*{ext}|All files (*.*)|*.*";
                var dialog = new SaveFileDialog
                {
                    InitialDirectory = Directory.GetCurrentDirectory(),
                    FileName = fileName,
                    Filter = v
                };
                if (dialog.ShowDialog() == false)
                    continue;

                File.WriteAllBytes(dialog.FileName, bytes);
                isSaveAny = true;
            }

            MessageBox.Show(isSaveAny ? MessageSaveSuccess : MeaageSaveFail);
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        private void ToolBarFileOpen_Click(object sender, RoutedEventArgs e)
        {
            Load(false);
        }

        private void ToolBarFileSave_Click(object sender, RoutedEventArgs e)
        {
            if (SaveData.IsReadonlyMode)
            {
                MessageBox.Show("ReadonlyMode");
                return;
            }
            Save();
        }

        private void ToolBarConvertToJson_Click(object sender, RoutedEventArgs e)
        {
            ConvertSaveDataToJson(true);
        }

        private void Init()
        {
            SetWeakFilter(0);
            SetTreasureStateFilter(0);
            DataContext = new DataContext();
            TopBar_LoadedInfoFile.Content = Info.LoadedInfoFile;
        }

        private void Load(bool force)
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == false) return;

            SaveData.Instance().Open(dlg.FileName);
            Init();
            MessageBox.Show(MessageLoadSuccess);
        }

        private void Save()
        {
            if (SaveData.Instance().Save() == true) MessageBox.Show(MessageSaveSuccess);
            else MessageBox.Show(MeaageSaveFail, SaveData.IsReadonlyMode ? "ReadonlyMode" : "");
        }

        private static void ConvertSaveDataToJson(bool convertOtherFile)
        {
            string fileName;
            if (convertOtherFile)
            {
                var openDialog = new OpenFileDialog();
                if (openDialog.ShowDialog() == false) return;
                fileName = openDialog.FileName;
            }
            else
            {
                fileName = SaveData.Instance().FileName;
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    MessageBox.Show(MeaageSaveFail);
                    return;
                }
            }

            string folder = Path.GetDirectoryName(fileName);
            var saveDialog = new SaveFileDialog
            {
                InitialDirectory = string.IsNullOrEmpty(folder) ? Directory.GetCurrentDirectory() : folder,
                FileName = Path.GetFileName(fileName) + ".json",
                Filter = "Json files (*.json)|*.json|All files (*.*)|*.*"
            };
            if (saveDialog.ShowDialog() == false)
                return;

            bool saved;
            Exception ex;

            if (convertOtherFile)
                (saved, ex) = GvasFormat.GvasConverter.Convert2JsonFile(saveDialog.FileName, File.OpenRead(fileName));
            else
                (saved, ex) = SaveData.Instance().SaveAsJson(saveDialog.FileName);

            if (ex != null)
            {
                if (saved)
                    MessageBox.Show("Only part of the data is saved as json because of the exception:\n" + ex.ToString(), MessageSaveSuccess);
                else
                    MessageBox.Show(ex.ToString(), MeaageSaveFail);

            }
            else
            {
                MessageBox.Show(saved ? MessageSaveSuccess : MeaageSaveFail);
            }
        }

        private void ButtonSword_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Sword = ChoiceEquipment(chara.Sword);
            }
        }

        private void ButtonLance_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Lance = ChoiceEquipment(chara.Lance);
            }
        }

        private void ButtonDagger_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Dagger = ChoiceEquipment(chara.Dagger);
            }
        }

        private void ButtonAxe_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Axe = ChoiceEquipment(chara.Axe);
            }
        }

        private void ButtonBow_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Bow = ChoiceEquipment(chara.Bow);
            }
        }

        private void ButtonRod_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Rod = ChoiceEquipment(chara.Rod);
            }
        }

        private void ButtonShield_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Shield = ChoiceEquipment(chara.Shield);
            }
        }

        private void ButtonHead_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Head = ChoiceEquipment(chara.Head);
            }
        }

        private void ButtonBody_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Body = ChoiceEquipment(chara.Body);
            }
        }

        private void ButtonAccessory1_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Accessory1 = ChoiceEquipment(chara.Accessory1);
            }
        }

        private void ButtonAccessory2_Click(object sender, RoutedEventArgs e)
        {
            if (CharactorList.SelectedItem is Charactor chara)
            {
                chara.Accessory2 = ChoiceEquipment(chara.Accessory2);
            }
        }

        private void ButtonItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { DataContext: Item item })
            {
                var window = new ItemChoiceWindow
                {
                    Type = ItemChoiceWindow.eType.item,
                    ID = item.ID
                };
                window.ShowDialog();
                item.ID = window.ID;
            }
        }

        private uint ChoiceEquipment(uint id)
        {
            var window = new ItemChoiceWindow
            {
                ID = id
            };
            window.ShowDialog();
            return window.ID;
        }

        private void MenuItemWeakProgress_Click(object sender, RoutedEventArgs e)
        {
            string text = "Completed : 0/0";
            if (DataContext != null)
            {
                var enermies = ((DataContext)DataContext)?.EnemyWeaknesses;
                if (enermies != null)
                {
                    int count = 0;

                    var builder = new StringBuilder(256);
                    foreach (var item in enermies)
                    {
                        if (item.IsBreakAll)
                            count++;
                    }

                    static void AppendNum(StringBuilder builder, string des, int completed, int total)
                    {
                        builder.Append(des);
                        builder.Append(" : ");
                        builder.Append(completed);
                        builder.Append('/');
                        builder.Append(total);
                        builder.AppendLine();
                    }

                    AppendNum(builder, "Completed", count, enermies.Count);
                    AppendNum(builder, "Uncompleted", enermies.Count - count, enermies.Count);
                    text = builder.ToString();
                }
            }

            MessageBox.Show(text, "Progress");
        }

        private void MenuItemWeakAll_Click(object sender, RoutedEventArgs e)
        {
            SetWeakFilter(0);
        }

        private void MenuItemWeakCompleted_Click(object sender, RoutedEventArgs e)
        {
            SetWeakFilter(1);
        }

        private void MenuItemWeakUncompleted_Click(object sender, RoutedEventArgs e)
        {
            SetWeakFilter(-1);
        }

        private void SetWeakFilter(int type)
        {
            MenuItemWeakAll.IsChecked = type == 0;
            MenuItemWeakCompleted.IsChecked = type > 0;
            MenuItemWeakUncompleted.IsChecked = type < 0;
            if (DataContext is not DataContext dataContext)
                return;

            var collectionView = CollectionViewSource.GetDefaultView(dataContext.EnemyWeaknesses);
            if (type > 0)
                collectionView.Filter = obj => ((EnemyWeakness)obj).IsBreakAll;
            else if (type < 0)
                collectionView.Filter = obj => !((EnemyWeakness)obj).IsBreakAll;
            else
                collectionView.Filter = null;
        }

        private void MenuItemTreasureStateProgress_Click(object sender, EventArgs e)
        {
            string text = "Completed : 0/0";
            if (DataContext != null)
            {
                var treasureStates = ((DataContext)DataContext)?.TreasureStates;
                if (treasureStates != null)
                {
                    uint completedChest = 0;
                    uint completedHiddenItem = 0;
                    uint totalChest = 0;
                    uint totalHiddenItem = 0;
                    uint completedCount = 0;

                    var builder = new StringBuilder(256);
                    foreach (var item in treasureStates)
                    {
                        var info = item.Info;

                        totalChest += info.Chest;
                        totalHiddenItem += info.HiddenItem;
                        completedChest += item.Chest;
                        completedHiddenItem += item.HiddenItem;
                        if (item.IsCollectAll)
                            completedCount++;
                    }

                    static void AppendNum(StringBuilder builder, string des, uint completed, uint total)
                    {
                        builder.Append(des);
                        builder.Append(" : ");
                        builder.Append(completed);
                        builder.Append('/');
                        builder.Append(total);
                        builder.AppendLine();
                    }

                    AppendNum(builder, "Completed", completedCount, (uint)treasureStates.Count);
                    AppendNum(builder, "Uncompleted", (uint)treasureStates.Count - completedCount, (uint)treasureStates.Count);
                    AppendNum(builder, TreasureStatesSummation, completedChest + completedHiddenItem, totalChest + totalHiddenItem);
                    AppendNum(builder, TreasureStatesChest, completedChest, totalChest);
                    AppendNum(builder, TreasureStatesHiddenItem, completedHiddenItem, totalHiddenItem);
                    text = builder.ToString();
                }
            }

            MessageBox.Show(text, "Progress");
        }

        private void MenuItemTreasureStateAll_Click(object sender, RoutedEventArgs e)
        {
            SetTreasureStateFilter(0);
        }

        private void MenuItemTreasureStateCompleted_Click(object sender, RoutedEventArgs e)
        {
            SetTreasureStateFilter(1);
        }

        private void MenuItemTreasureStateUncompleted_Click(object sender, RoutedEventArgs e)
        {
            SetTreasureStateFilter(-1);
        }

        private void SetTreasureStateFilter(int type)
        {
            MenuItemTreasureStateAll.IsChecked = type == 0;
            MenuItemTreasureStateCompleted.IsChecked = type > 0;
            MenuItemTreasureStateUncompleted.IsChecked = type < 0;
            if (DataContext is not DataContext dataContext)
                return;

            var collectionView = CollectionViewSource.GetDefaultView(dataContext.TreasureStates);
            if (type > 0)
                collectionView.Filter = obj => ((TreasureState)obj).IsCollectAll;
            else if (type < 0)
                collectionView.Filter = obj => !((TreasureState)obj).IsCollectAll;
            else
                collectionView.Filter = null;
        }
    }
}
