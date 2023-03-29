using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;

namespace OctopathTraveler
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
#if !DEBUG
            Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
#endif

            for (int i = 0; i < e.Args.Length; i++)
            {
                string arg = e.Args[i];
                Trace.WriteLine($"Startup arg[{i}]: {arg}");
                if (arg.StartsWith("-language="))
                {
                    SetLanguage(arg["-language=".Length..]);
                    continue;
                }

                if (arg == "-readonlyMode")
                {
                    SaveData.IsReadonlyMode = true;
                }
            }
        }

        private static void SetLanguage(string language)
        {
            try
            {
                CultureInfo cultureInfo;
                language = language.ToLower();
                switch (language)
                {
                    case "zh-cn":
                    case "zh_cn":
                        cultureInfo = CultureInfo.GetCultureInfo("zh-CN");
                        break;
                    case "ja-jp":
                    case "ja_jp":
                        cultureInfo = CultureInfo.GetCultureInfo("ja-JP");
                        break;
                    case "en-us":
                    case "en_us":
                        cultureInfo = CultureInfo.GetCultureInfo("en-US");
                        break;
                    default:
                        if (language.StartsWith("en"))
                        {
                            cultureInfo = CultureInfo.GetCultureInfo("en-US");
                            break;
                        }
                        return;
                }
                CultureInfo.CurrentUICulture = cultureInfo;
                OctopathTraveler.Properties.Resources.Culture = cultureInfo;
            }
            catch
            {
            }
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
    }
}
