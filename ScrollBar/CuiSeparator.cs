// Deobfuscated / cleaned version of the decompiled control.
// Original type: כ… .ᚎᚏᚱ…
//
// What it is:
// - A simple separator line control that can be horizontal or vertical.
// - Uses ForeColor for the line color.
// - Thickness is a float (default 0.5f) and margin insets the line from the edges.

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(SplitContainer))]
    public class CuiSeparator : UserControl
    {
        private float _thickness = 0.5f;
        private bool _isVertical;
        private int _separatorMargin = 8;

        private IContainer components;

        public CuiSeparator()
        {
            InitializeComponent();

            // Default separator color (semi-transparent gray) from decompile.
            ForeColor = Color.FromArgb(128, 128, 128, 128);
        }

        public float Thickness
        {
            get => _thickness;
            set { _thickness = value; Invalidate(); }
        }

        public bool Vertical
        {
            get => _isVertical;
            set { _isVertical = value; Invalidate(); }
        }

        public int SeparatorMargin
        {
            get => _separatorMargin;
            set { _separatorMargin = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // (Decompile doesn’t set SmoothingMode; keeping it as-is.)
             var path = new GraphicsPath();

            RectangleF rect = !_isVertical
                ? new RectangleF(
                    _separatorMargin,
                    Height / 2f,
                    Width - _separatorMargin * 2,
                    _thickness)
                : new RectangleF(
                    Width / 2f,
                    _separatorMargin,
                    _thickness,
                    Height - _separatorMargin * 2);

            path.AddRectangle(rect);

            using (var pen = new Pen(ForeColor, _thickness))
                e.Graphics.DrawPath(pen, path);

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
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            Name = "cuiSeparator";
            Size = new Size(604, 41);
            ResumeLayout(false);
        }
    }
}
