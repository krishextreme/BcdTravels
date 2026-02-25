// Deobfuscated / cleaned version of the decompiled control.
// Original type: כ… .ᚃ…
//
// What it is:
// - A rounded Panel that fills with PanelColor and draws an outline with PanelOutlineColor.
// - Corner rounding is per-corner via Padding (Left/Top/Right/Bottom).
//
// External dependencies kept as aliases/placeholders:
// - Theme.PrimaryColor
// - BitMapClass.RoundRect(RectangleF, Padding)

using Ledger.BitUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;


namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(Panel))]
    public class CuiBorderPanel : Panel
    {
        private Color _panelColor = Theme.PrimaryColor;
        private Color _outlineColor = Theme.PrimaryColor;
        private float _outlineThickness = 1f;

        // Padding used as per-corner rounding radii.
        private Padding _cornerRadii = new Padding(8, 8, 8, 8);

        private IContainer components;

        public CuiBorderPanel()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Refresh();
        }

        public Color PanelColor
        {
            get => _panelColor;
            set { _panelColor = value; Invalidate(); }
        }

        public Color PanelOutlineColor
        {
            get => _outlineColor;
            set { _outlineColor = value; Invalidate(); }
        }

        public float OutlineThickness
        {
            get => _outlineThickness;
            set { _outlineThickness = value; Invalidate(); }
        }

        public Padding Rounding
        {
            get => _cornerRadii;
            set { _cornerRadii = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = ClientRectangle;
            rect.Width--;
            rect.Height--;

             GraphicsPath path = BitMapClass.RoundRect(rect, 2);

            using (SolidBrush fill = new SolidBrush(PanelColor))
            using (Pen outline = new Pen(PanelOutlineColor, OutlineThickness))
            {
                e.Graphics.FillPath(fill, path);
                e.Graphics.DrawPath(outline, path);
            }

            base.OnPaint(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Name = "cuiBorder";
            Size = new Size(346, 150);
            ResumeLayout(false);
        }
    }
}
