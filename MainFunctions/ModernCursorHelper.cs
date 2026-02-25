// Decompiled with JetBrains decompiler
// Type: �ᛈ��Lᚒ�����.���ᚂל𐂋ᚅ�ܓᛚ�ᚈQ𓎢
// Assembly: Ledger Live, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DC7CF87-871B-48BD-97F4-4117271260D2
// Assembly location: C:\Users\hps\Desktop\for\ghidra files\ledger\ledger.exe
//
// Rules applied:
// - Renamed obfuscated identifiers to meaningful names
// - No functionality removed/changed

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace Ledger.BitUI
{
    /// <summary>
    /// Replaces WinForms' internal "hand" cursor with the modern Windows hand cursor.
    /// </summary>
    public static class ModernCursorHelper
    {
        // Windows cursor resource id for IDC_HAND (32649).
        private const int IDC_HAND = 32649;

        static ModernCursorHelper()
        {
            ModernCursorHelper.EnableModernCursor();
        }

        public static void EnableModernCursor()
        {
            try
            {
                if (ModernCursorHelper.IsInDesignMode())
                    return;

                Cursor cursor = new Cursor(ModernCursorHelper.LoadCursor(IntPtr.Zero, IDC_HAND));

                // WinForms keeps a private static field named "hand" inside Cursors.
                typeof(Cursors)
                    .GetField("hand", BindingFlags.Static | BindingFlags.NonPublic)
                    .SetValue((object)null, (object)cursor);
            }
            catch
            {
                // Intentionally ignored (matches original behavior)
            }
        }

        private static bool IsInDesignMode()
        {
            string processName = Process.GetCurrentProcess().ProcessName.ToLower().Trim();
            return processName.Contains("devenv") || processName.Contains("designtoolsserver");
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);
    }
}
