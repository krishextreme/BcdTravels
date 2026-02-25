using Ledger.BitUI;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(ProgressBar))]
    public class CuiFillProgressBar : Control   // ← Change to your real base if different
    {
        // If these come from base class → remove from here
        // Otherwise keep them and add [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]

        [Browsable(true), Category("Appearance")]
        public Color Background { get; set; } = Color.FromArgb(40, 40, 45);

        [Browsable(true), Category("Appearance")]
        public Color Foreground { get; set; } = Color.DodgerBlue;

        [Browsable(true), Category("Behavior")]
        public int Value { get; set; } = 0;

        [Browsable(true), Category("Behavior")]
        public int MaxValue { get; set; } = 100;

        [Browsable(true), Category("Appearance")]
        public int Rounding { get; set; } = 12;

        [Browsable(true), Category("Behavior")]
        public bool Flipped { get; set; } = false;

        public CuiFillProgressBar()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;

            if (Width <= 1 || Height <= 1 || MaxValue <= 0) return;

            // 2x supersampling
            int bmpW = Width * 2;
            int bmpH = Height * 2;

             var bitmap = new Bitmap(bmpW, bmpH);
            var g = Graphics.FromImage(bitmap);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            // Clip to rounded rectangle (2× scale)
            var clipPath = BitMapClass.RoundRect(new Rectangle(0, 0, bmpW, bmpH), Rounding * 2);
            g.SetClip(clipPath);

            float progress = (float)Value / MaxValue;
            float filledHeight2x = Height * 2 * progress;

            // Background (empty part) — drawn from bottom up
             var backBrush = new SolidBrush(Background);
            g.FillRectangle(backBrush, 0, filledHeight2x, bmpW, bmpH - filledHeight2x);

            // Foreground (filled part) — full rounded rect, but clipped anyway
            var fillBrush = new SolidBrush(Foreground);

            // We make filled area slightly wider → old decompiler artifact (helps with rounding edges)
            var filledRect = new RectangleF(
                -Width / 4f,           // little overhang left
                0,
                bmpW + Width / 2f,     // little overhang right
                filledHeight2x + Rounding * 2 // safety for rounding
            );

            var filledPath = BitMapClass.RoundRect(Rectangle.Round(filledRect), Rounding * 2);
            g.FillPath(fillBrush, filledPath);

            // ────────────────────────────────────────────────
            // Flip if requested (very common for bottom→top bars)
            if (Flipped)
            {
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
            }

            // Final blit — high quality downscale
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImage(bitmap, ClientRectangle);
        }
    }
}