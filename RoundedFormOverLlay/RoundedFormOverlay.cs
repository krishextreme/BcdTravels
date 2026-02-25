
using Ledger.BitUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Ledger.RoundedFormOverLlay
{
    public class RoundedFormOverlay : Form
    {
        private Color _backgroundColor = Color.White;
        private Color _borderColor = Color.White;

        private Bitmap _backBufferBitmap;
        private Graphics _backBufferGraphics;

        private bool _invalidateNextDrawCall;
        private Image _backgroundImageOfTargetForm;

        public int Rounding = 8;

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x080000;      // 524288
        private const int WS_EX_TRANSPARENT = 0x20;      // 32

        private Bitmap _targetFormBitmap;
        public Form TargetForm;

        private TextureBrush _targetTextureBrush;
        private bool _stop;

        private IContainer components;
        public bool initialized;

        public bool InvalidateNextDrawCall
        {
            get => _invalidateNextDrawCall;
            set
            {
                if (!value)
                    return;

                DrawForm(null, EventArgs.Empty);
                _invalidateNextDrawCall = false;
            }
        }

        public Image BackgroundImageOfTargetForm
        {
            get => _backgroundImageOfTargetForm;
            set
            {
                _backgroundImageOfTargetForm?.Dispose();
                _backgroundImageOfTargetForm = value;
                DrawForm(null, EventArgs.Empty);
            }
        }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                Invalidate();
            }
        }

        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        public RoundedFormOverlay(Color initBackgroundColor, Color initBorderColor, ref int roundValue, bool show = true)
        {
            // Original code does this before InitializeComponent().
            BitMapClass.Native.LayeredWindowBitmap.SetBitmap(new Bitmap(1, 1), 0, Width, Height, Handle);

            Visible = show;
            InitializeComponent();
            SetStyles();

            BackgroundColor = initBackgroundColor;
            BorderColor = initBorderColor;
            Rounding = roundValue;
        }

        private void SetStyles()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            UpdateStyles();
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;                 // WS_EX_TOOLWINDOW (commonly)
                cp.ExStyle |= WS_EX_LAYERED;
                cp.ExStyle |= WS_EX_LAYERED | WS_EX_TRANSPARENT;
                return cp;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            InitializeWindowFix();
        }

        public void DrawForm(object sender, EventArgs e)
        {
            if (_stop)
                return;

            try
            {
                // Same as: this.privateBackgroundColor = this.TargetForm?.BackColor.Value;
                _backgroundColor = TargetForm?.BackColor ?? _backgroundColor;
            }
            catch
            {
                // swallow (matches decompiled)
            }

            SuspendLayout();
            try
            {
                if (_backBufferBitmap != null && !(_backBufferBitmap.Size != Size))
                {
                    Color backgroundColor = BackgroundColor;
                    Color? targetBackColor = TargetForm?.BackColor;

                    if ((targetBackColor.HasValue ? backgroundColor != targetBackColor.GetValueOrDefault() ? 1 : 0 : 1) == 0
                        && !InvalidateNextDrawCall)
                    {
                        _backBufferGraphics.Clear(Color.Transparent);
                        goto Render;
                    }
                }

                InvalidateNextDrawCall = false;

                _backBufferBitmap?.Dispose();
                _backBufferGraphics?.Dispose();

                _backBufferBitmap = new Bitmap(Width, Height);
                _backBufferGraphics = Graphics.FromImage(_backBufferBitmap);

            Render:
                Rectangle outerRect = new Rectangle(0, 0, Width - 2, Height - 2);
                GraphicsPath borderPath = GraphicsUtil.RoundRect(outerRect, Rounding);

                Rectangle innerRect = outerRect;
                innerRect.Offset(1, 1);
                innerRect.Inflate(-1, -1);

                Rectangle innerRect2 = innerRect;
                innerRect2.Offset(-1, -1);

                // Decompiled code creates this and ignores it (kept as-is).
                GraphicsUtil.RoundRect(innerRect, Rounding);

                GraphicsPath fillPath = GraphicsUtil.RoundRect(innerRect2, Rounding);

                using (Pen borderPen = new Pen(BorderColor))
                {
                    _backBufferGraphics.SmoothingMode = SmoothingMode.AntiAlias;

                    if (Rounding > 0)
                    {
                        using (Pen bgPen = new Pen(BackgroundColor, 1f))
                        using (SolidBrush bgBrush = new SolidBrush(BackgroundColor))
                        {
                            _backBufferGraphics.FillPath(bgBrush, fillPath);
                            _backBufferGraphics.DrawPath(bgPen, fillPath);
                        }

                        if (_targetTextureBrush != null)
                        {
                            using (Pen texturePen = new Pen(_targetTextureBrush, 1f))
                                _backBufferGraphics.DrawPath(texturePen, fillPath);
                        }
                    }

                    _backBufferGraphics.DrawPath(borderPen, borderPath);
                    _backBufferGraphics.SmoothingMode = SmoothingMode.None;
                }

                // Preserved original behavior (unsafe if Tag is not a double).
                byte alpha = (byte)((double)Tag * byte.MaxValue);

                BitMapClass.Native.LayeredWindowBitmap.SetBitmap(
                    initialized ? _backBufferBitmap : null,
                    initialized ? alpha : (byte)0,
                    Left,
                    Top,
                    Handle);
            }
            finally
            {
                ResumeLayout();
            }
        }

        private void InitializeWindowFix()
        {
            Location = new Point(-Width + 1, -Height + 1);

            // Remove WS_EX_LAYERED and WS_EX_TRANSPARENT (same bit-math as decompiled: & -524289 & -33).
            int exStyle = (int)GetWindowLong(Handle, GWL_EXSTYLE);
            exStyle = exStyle & ~WS_EX_LAYERED & ~WS_EX_TRANSPARENT;
            SetWindowLong(Handle, GWL_EXSTYLE, exStyle);

            initialized = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!DesignMode)
            {
                // Decompiled uses an external wrapper around Get/SetWindowLong; direct call is equivalent.
                int exStyle = (int)GetWindowLong(Handle, GWL_EXSTYLE);
                SetWindowLong(Handle, GWL_EXSTYLE, exStyle | WS_EX_LAYERED);
            }

            DrawForm(this, EventArgs.Empty);
        }

        protected override void OnResize(EventArgs e) => Region = null;

        private void RoundedForm_PaddingChanged(object sender, EventArgs e)
        {
            DrawForm(this, e);
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            DrawForm(this, e);
        }

        internal void Stop()
        {
            _stop = true;

            _backBufferBitmap?.Dispose();
            _backBufferGraphics?.Dispose();

            PaddingChanged -= new EventHandler(RoundedForm_PaddingChanged);
            Dispose();
        }

        internal void UpdBitmap()
        {
            _targetFormBitmap?.Dispose();
            _targetTextureBrush?.Image?.Dispose();
            _targetTextureBrush?.Dispose();
            _targetTextureBrush = null;
        }

        internal void UpdBitmap(Bitmap experimentalBitmap)
        {
            if (_targetFormBitmap != experimentalBitmap)
            {
                _targetFormBitmap?.Dispose();
                _targetFormBitmap = experimentalBitmap;
            }

            _targetTextureBrush?.Dispose();
            _targetTextureBrush = new TextureBrush(_targetFormBitmap)
            {
                WrapMode = WrapMode.Clamp
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RoundedFormOverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(75, 57);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "RoundedFormOverlay";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "RoundedForm";
            this.Load += new System.EventHandler(this.RoundedFormOverlay_Load);
            this.PaddingChanged += new System.EventHandler(this.RoundedForm_PaddingChanged);
            this.ResumeLayout(false);

        }

        private void RoundedFormOverlay_Load(object sender, EventArgs e)
        {

        }
    }

    // ---- Placeholders for missing external definitions referenced by the obfuscated assembly ----

    internal static class GraphicsUtil
    {
        public static GraphicsPath RoundRect(Rectangle bounds, int radius)
        {
            // Placeholder only. Replace with the real implementation from your project/binary.
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                path.CloseFigure();
                return path;
            }

            int d = radius * 2;
            Rectangle arc = new Rectangle(bounds.Location, new Size(d, d));

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - d;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - d;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Creates a star-shaped GraphicsPath.
        /// </summary>
        /// <param name="centerX">Star center X</param>
        /// <param name="centerY">Star center Y</param>
        /// <param name="outerRadius">Outer radius (tip of star)</param>
        /// <param name="innerRadius">Inner radius (indent)</param>
        /// <param name="points">Number of star points (usually 5)</param>
        public static GraphicsPath Star(
            float centerX,
            float centerY,
            float outerRadius,
            float innerRadius,
            int points)
        {
            GraphicsPath path = new GraphicsPath();

            if (points < 2)
                return path;

            PointF[] vertices = new PointF[points * 2];
            double angleStep = Math.PI / points;
            double angle = -Math.PI / 2; // start at top

            for (int i = 0; i < vertices.Length; i++)
            {
                float radius = (i % 2 == 0) ? outerRadius : innerRadius;

                vertices[i] = new PointF(
                    centerX + (float)(Math.Cos(angle) * radius),
                    centerY + (float)(Math.Sin(angle) * radius)
                );

                angle += angleStep;
            }

            path.AddPolygon(vertices);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Creates a left-pointing arrow inside the given rectangle.
        /// </summary>
        public static GraphicsPath LeftArrowtest(Rectangle arrowRect)
        {
            GraphicsPath path = new GraphicsPath();

            int x = arrowRect.X;
            int y = arrowRect.Y;
            int w = arrowRect.Width;
            int h = arrowRect.Height;

            PointF[] points =
            {
            new PointF(x + w,     y),           // top-right
            new PointF(x,         y + h / 2f),   // center-left (arrow tip)
            new PointF(x + w,     y + h),        // bottom-right
            new PointF(x + w * 0.7f, y + h),     // bottom-inner
            new PointF(x + w * 0.7f, y),         // top-inner
        };

            path.AddPolygon(points);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Creates a down-pointing arrow inside the given rectangle.
        /// </summary>
        public static GraphicsPath DownArrow(Rectangle arrowRect)
        {
            GraphicsPath path = new GraphicsPath();

            int x = arrowRect.X;
            int y = arrowRect.Y;
            int w = arrowRect.Width;
            int h = arrowRect.Height;

            PointF[] points =
            {
            new PointF(x,         y),            // top-left
            new PointF(x + w,     y),            // top-right
            new PointF(x + w,     y + h * 0.7f), // right-inner
            new PointF(x + w / 2f,y + h),        // bottom-center (arrow tip)
            new PointF(x,         y + h * 0.7f), // left-inner
        };

            path.AddPolygon(points);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Creates a checkmark (✔) GraphicsPath inside the given rectangle.
        /// </summary>
        /// <param name="bounds">Area where the checkmark is drawn</param>
        /// <param name="offset">X/Y inset for symbol positioning</param>
        public static GraphicsPath Checkmark(Rectangle bounds, Point offset)
        {
            GraphicsPath path = new GraphicsPath();

            Rectangle r = new Rectangle(
                bounds.X + offset.X,
                bounds.Y + offset.Y,
                bounds.Width - offset.X * 2,
                bounds.Height - offset.Y * 2
            );

            float x = r.X;
            float y = r.Y;
            float w = r.Width;
            float h = r.Height;

            // Proportional checkmark geometry
            PointF p1 = new PointF(x + w * 0.15f, y + h * 0.55f); // left
            PointF p2 = new PointF(x + w * 0.40f, y + h * 0.80f); // bottom
            PointF p3 = new PointF(x + w * 0.85f, y + h * 0.20f); // right

            path.AddLines(new[] { p1, p2, p3 });
            return path;
        }

        /// <summary>
        /// Creates a crossmark (✕) GraphicsPath inside the given rectangle.
        /// </summary>
        /// <param name="bounds">Area where the cross is drawn</param>
        /// <param name="offset">X/Y inset for symbol positioning</param>
        public static GraphicsPath Crossmark(Rectangle bounds, Point offset)
        {
            GraphicsPath path = new GraphicsPath();

            Rectangle r = new Rectangle(
                bounds.X + offset.X,
                bounds.Y + offset.Y,
                bounds.Width - offset.X * 2,
                bounds.Height - offset.Y * 2
            );

            float x = r.X;
            float y = r.Y;
            float w = r.Width;
            float h = r.Height;

            // First diagonal \
            path.StartFigure();
            path.AddLine(
                x + w * 0.20f, y + h * 0.20f,
                x + w * 0.80f, y + h * 0.80f
            );

            // Second diagonal /
            path.StartFigure();
            path.AddLine(
                x + w * 0.80f, y + h * 0.20f,
                x + w * 0.20f, y + h * 0.80f
            );

            return path;
        }
    }

    //internal static class LayeredWindowRenderer
    //{
    //    public static void SetBitmap(Bitmap bitmap, byte alpha, int x, int y, uint hwnd)
    //    {
    //        // Placeholder for:
    //        // Mᚺ... .ח... .???.SetBitmap(...)
    //        //
    //        // Usually wraps UpdateLayeredWindow and BLENDFUNCTION for per-pixel alpha.
    //    }
    //}
}
