// Decompiled with JetBrains decompiler
// Type: �ᛈ��Lᚒ�����.���ᚹ���Y��
// Assembly: Ledger Live, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DC7CF87-871B-48BD-97F4-4117271260D2
// Assembly location: C:\Users\hps\Desktop\for\ghidra files\ledger\ledger.exe
//
// Rules applied:
// - Renamed obfuscated identifiers to meaningful names
// - No functionality removed/changed

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Ledger.BitUI
{
    [ToolboxBitmap(typeof(Form))]
    public class NativeFormDragComponent : Component
    {
        private Form targetForm;

        private const int WM_NCLBUTTONDOWN = 161;
        private const int HTCAPTION = 2;

        public NativeFormDragComponent(IContainer container)
        {
            container.Add((IComponent)this);
        }

        public Form TargetForm
        {
            get => this.targetForm;
            set
            {
                if (this.targetForm != null)
                    this.targetForm.MouseMove -= new MouseEventHandler(this.OnMouseMove);

                this.targetForm = value;

                if (this.targetForm == null)
                    return;

                this.targetForm.MouseMove += new MouseEventHandler(this.OnMouseMove);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            NativeFormDragComponent.ReleaseCapture();
            NativeFormDragComponent.SendMessage(this.TargetForm.Handle, 161, 2, 0);
        }
    }
}
