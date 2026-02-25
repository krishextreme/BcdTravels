
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Ledger.BitUI
{

    public static class GlobalMouseClickHook
    {
        private const int WH_MOUSE_LL = 14;
        private const int WM_LBUTTONDOWN = 513;

        private static LowLevelMouseProc hookProc = new LowLevelMouseProc(GlobalMouseClickHook.HookCallback);
        private static IntPtr hookHandle = IntPtr.Zero;

        public static bool IsHooked = false;

        /// <summary>
        /// Called when a global left mouse click (WM_LBUTTONDOWN) is observed.
        /// Note: original code assumes this is non-null and will throw otherwise.
        /// </summary>
        public static Action OnGlobalMouseClick;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(
            int idHook,
            LowLevelMouseProc lpfn,
            IntPtr hMod,
            uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public static void Start()
        {
            GlobalMouseClickHook.hookHandle = GlobalMouseClickHook.SetHook(GlobalMouseClickHook.hookProc);
            GlobalMouseClickHook.IsHooked = true;
        }

        public static void Stop()
        {
            GlobalMouseClickHook.UnhookWindowsHookEx(GlobalMouseClickHook.hookHandle);
            GlobalMouseClickHook.IsHooked = false;
            GC.Collect();
        }

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process currentProcess = Process.GetCurrentProcess())
            using (ProcessModule mainModule = currentProcess.MainModule)
            {
                return GlobalMouseClickHook.SetWindowsHookEx(
                    WH_MOUSE_LL,
                    proc,
                    GlobalMouseClickHook.GetModuleHandle(mainModule.ModuleName),
                    0U
                );
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_LBUTTONDOWN)
                GlobalMouseClickHook.OnGlobalMouseClick();

            return GlobalMouseClickHook.CallNextHookEx(
                GlobalMouseClickHook.hookHandle,
                nCode,
                wParam,
                lParam
            );
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
    }
}
