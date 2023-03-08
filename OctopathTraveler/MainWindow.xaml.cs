using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace OctopathTraveler
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
            InitializeComponent();
#if !DEBUG
            Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
#endif
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
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
			if (files == null) return;
			if (!System.IO.File.Exists(files[0])) return;

			SaveData.Instance().Open(files[0]);
			Init();
			MessageBox.Show(Properties.Resources.MessageLoadSuccess);
		}

		private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e)
		{
			Load(false);
		}

		private void MenuItemFileSave_Click(object sender, RoutedEventArgs e)
		{
			Save();
		}

		private void MenuItemFileSaveAs_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			if (SaveData.Instance().SaveAs(dlg.FileName) == true) MessageBox.Show(Properties.Resources.MessageSaveSuccess);
			else MessageBox.Show(Properties.Resources.MeaageSaveFail);
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
			Save();
		}

		private void Init()
		{
			DataContext = new DataContext();
		}

		private void Load(bool force)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Open(dlg.FileName);
			Init();
			MessageBox.Show(Properties.Resources.MessageLoadSuccess);
		}

		private void Save()
		{
			if (SaveData.Instance().Save() == true) MessageBox.Show(Properties.Resources.MessageSaveSuccess);
			else MessageBox.Show(Properties.Resources.MeaageSaveFail);
		}

		private void ButtonSword_Click(object sender, RoutedEventArgs e)
		{
			Charactor chara = CharactorList.SelectedItem as Charactor;
			if(chara != null)
			{
				chara.Sword = ChoiceEquipment(chara.Sword);
			}
		}

		private void ButtonLance_Click(object sender, RoutedEventArgs e)
		{
			Charactor chara = CharactorList.SelectedItem as Charactor;
			if (chara != null)
			{
				chara.Lance = ChoiceEquipment(chara.Lance);
			}
		}

		private void ButtonDagger_Click(object sender, RoutedEventArgs e)
		{
			Charactor chara = CharactorList.SelectedItem as Charactor;
			if (chara != null)
			{
				chara.Dagger = ChoiceEquipment(chara.Dagger);
			}
		}

		private void ButtonAxe_Click(object sender, RoutedEventArgs e)
		{
			Charactor chara = CharactorList.SelectedItem as Charactor;
			if (chara != null)
			{
				chara.Axe = ChoiceEquipment(chara.Axe);
			}
		}

		private void ButtonBow_Click(object sender, RoutedEventArgs e)
		{
			Charactor chara = CharactorList.SelectedItem as Charactor;
			if (chara != null)
			{
				chara.Bow = ChoiceEquipment(chara.Bow);
			}
		}

		private void ButtonRod_Click(object sender, RoutedEventArgs e)
		{
			Charactor chara = CharactorList.SelectedItem as Charactor;
			if (chara != null)
			{
				chara.Rod = ChoiceEquipment(chara.Rod);
			}
		}

		private void ButtonShield_Click(object sender, RoutedEventArgs e)
		{
			Charactor chara = CharactorList.SelectedItem as Charactor;
			if (chara != null)
			{
				chara.Shield = ChoiceEquipment(chara.Shield);
			}
		}

		private void ButtonHead_Click(object sender, RoutedEventArgs e)
		{
			Charactor chara = CharactorList.SelectedItem as Charactor;
			if (chara != null)
			{
				chara.Head = ChoiceEquipment(chara.Head);
			}
		}

		private void ButtonBody_Click(object sender, RoutedEventArgs e)
		{
			Charactor chara = CharactorList.SelectedItem as Charactor;
			if (chara != null)
			{
				chara.Body = ChoiceEquipment(chara.Body);
			}
		}

		private void ButtonAccessory1_Click(object sender, RoutedEventArgs e)
		{
			Charactor chara = CharactorList.SelectedItem as Charactor;
			if (chara != null)
			{
				chara.Accessory1 = ChoiceEquipment(chara.Accessory1);
			}
		}

		private void ButtonAccessory2_Click(object sender, RoutedEventArgs e)
		{
			Charactor chara = CharactorList.SelectedItem as Charactor;
			if (chara != null)
			{
				chara.Accessory2 = ChoiceEquipment(chara.Accessory2);
			}
		}

		private void ButtonItem_Click(object sender, RoutedEventArgs e)
		{
			Item item = (sender as Button)?.DataContext as Item;
			if(item != null)
			{
				ItemChoiceWindow window = new ItemChoiceWindow();
				window.Type = ItemChoiceWindow.eType.item;
				window.ID = item.ID;
				window.ShowDialog();
				item.ID = window.ID;
			}
		}

		private uint ChoiceEquipment(uint id)
		{
			ItemChoiceWindow window = new ItemChoiceWindow();
			window.ID = id;
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
					int uncompletedCount = 0;

                    var builder = new StringBuilder(256);
                    foreach (var item in enermies)
                    {
                        if (item.IsBreakAll)
                        {
                            count++;
						}
						else
						{
							uncompletedCount++;
							if (uncompletedCount > 20)
							{
								if (uncompletedCount == 21)
                                    builder.AppendLine("more. . .");

								continue;
                            }
                            uint value = item.Info.Value;
							//if (value < 10)
							//{
							//	builder.Append("00");
							//}
							//else if (value < 100)
							//{
							//	builder.Append("0");
							//}
							builder.Append(value);
							builder.Append('\t');
							builder.AppendLine(item.Info.Name);
                        }
                    }

					if (uncompletedCount > 0)
                    {
						builder.Insert(0, "\n");
						builder.Insert(0, uncompletedCount);
                        builder.Insert(0, "Uncompleted : ");
                    }

                    builder.Insert(0, "\n");
					builder.Insert(0, enermies.Count);
					builder.Insert(0, "/");
					builder.Insert(0, count);
					builder.Insert(0, "Completed : ");
                    text = builder.ToString();
                }
            }

            MessageBox.Show(text, Properties.Resources.MenuDataWeakProgress);
        }
    }
}
