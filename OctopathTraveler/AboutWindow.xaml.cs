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
            System.Diagnostics.Process.Start("explorer.exe", "http://turtleinsect.php.xdomain.jp");
        }

        private void LabelModifier1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://github.com/SUDALV92/OctopathTraveler-TreasureChests-");
        }

        private void LabelModifier2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://github.com/LonelyWindG/OctopathTraveler-SavaDataEditor");
        }
    }
}
