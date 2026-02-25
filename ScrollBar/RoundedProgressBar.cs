using Ledger.BitUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;

// External dependency (obfuscated in the sample):
// - Theme.PrimaryColor (your code uses: ג...PrimaryColor)
// - BitMapClass.RoundRect(Rectangle rect, int radius) (your code uses: M...RoundRect)
//using GraphicsUtil = graphicClass;

namespace Ledger.ScrollBar // placeholder namespace
{
    /// <summary>
    /// A simple rounded progress-bar style control that draws at 2x resolution for smoother edges.
    /// </summary>
    [ToolboxBitmap(typeof(ProgressBar))]
    public class RoundedProgressBar : UserControl
    {
        private int _value = 50;
        private int _maxValue = 100;
        private bool _flipped;

        private Color _backgroundColor = Color.FromArgb(64, 128, 128, 128);
        private Color _foregroundColor = Theme.PrimaryColor;

        private int _cornerRadius = 8;

        private IContainer components;

        public RoundedProgressBar()
        {
            InitializeComponent();
            DoubleBuffered = true;
            AutoScaleMode = AutoScaleMode.None;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        [Category("Behavior")]
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                Invalidate();
            }
        }

        [Category("Behavior")]
        public int MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool Flipped
        {
            get => _flipped;
            set
            {
                _flipped = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color Background
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color Foreground
        {
            get => _foregroundColor;
            set
            {
                _foregroundColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Corner radius in pixels (clamped to half the control height).
        /// </summary>
        [Category("Appearance")]
        public int Rounding
        {
            get => _cornerRadius;
            set
            {
                // The decompiled code effectively clamps to Height/2, but did it via recursion.
                // This is the same behavior, just expressed safely.
                int maxAllowed = ClientRectangle.Height / 2;
                _cornerRadius = value > maxAllowed ? maxAllowed : value;

                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Paint onto a 2x bitmap to improve anti-aliased edges, then scale down.
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;

            int w2 = ClientSize.Width * 2;
            int h2 = ClientSize.Height * 2;

            using (Bitmap backBuffer = new Bitmap(w2, h2))
            using (Graphics g = Graphics.FromImage(backBuffer))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Clip to rounded outer rectangle
                using (GraphicsPath clipPath = BitMapClass.RoundRect(new Rectangle(0, 0, w2, h2), Rounding * 2))
                {
                    g.SetClip(clipPath);

                    // Guard against division by zero; original code assumes MaxValue != 0
                    float progress = MaxValue == 0 ? 0f : (float)Value / MaxValue;

                    // Original math: ClientRectangle.Width * progress * 2.0
                    float filledWidth = (float)(ClientRectangle.Width * progress * 2.0);

                    // Filled portion (foreground)
                    RectangleF filledRect = new RectangleF(
                        0f,
                        0f,
                        filledWidth,
                        ClientRectangle.Height * 2 + 1);

                    // Unfilled portion (background) - matches the decompiled "x/width" shaping
                    RectangleF emptyRect = new RectangleF(
                        filledWidth - Rounding - filledWidth / 4f,
                        0f,
                        ClientRectangle.Width * 2 - filledWidth + Rounding * 2 + filledWidth / 4f,
                        ClientRectangle.Height * 2);

                    using (SolidBrush bg = new SolidBrush(Background))
                        g.FillRectangle(bg, emptyRect);

                    Rectangle filledRectInt = Rectangle.Round(filledRect);

                    using (GraphicsPath filledPath = BitMapClass.RoundRect(filledRectInt, Rounding * 2))
                    using (SolidBrush fg = new SolidBrush(Foreground))
                        g.FillPath(fg, filledPath);
                }

                if (Flipped)
                    backBuffer.RotateFlip(RotateFlipType.RotateNoneFlipX);

                e.Graphics.DrawImage(backBuffer, ClientRectangle);
            }

            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            AutoScaleMode = AutoScaleMode.Font;
        }
    }
}
