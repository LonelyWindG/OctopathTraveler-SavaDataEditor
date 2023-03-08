using System.Windows;
using System.Windows.Input;

namespace OctopathTraveler
{
    /// <summary>
    /// AboutWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void LabelHP_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenURL("http://turtleinsect.php.xdomain.jp");
        }

        private void LabelModifier1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenURL("https://github.com/SUDALV92/OctopathTraveler-TreasureChests-");
        }

        private void LabelModifier2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenURL("https://github.com/LonelyWindG/OctopathTraveler-SavaDataEditor");
        }

        private void LabelDataSource1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenURL("https://docs.google.com/spreadsheets/d/14Kz5mTAYdxqdgjbkbotAMGC2aoiJBbrBUiLeh8Pwu0Q");
        }

        private void LabelDataSource2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenURL("https://docs.google.com/spreadsheets/d/1O1OYHmLNsUcak5dByXbmEFDaxIbp-mDSHGC6j92P5ho");
        }

        private static void OpenURL(string url)
        {
            System.Diagnostics.Process.Start("explorer.exe", url);
        }
    }
}
