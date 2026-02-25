// Decompiled with JetBrains decompiler (renamed for readability)
// Original type: כ… .Λא…ב
//
// Purpose:
// - PictureBox-like UserControl that:
//   - optionally tints the source image (multiplicative RGBA tint)
//   - draws it clipped to a rounded rectangle
//   - can rotate the image (by re-tinting/rebuilding the brush when Rotation changes)
//   - optionally draws an outline around the rounded rectangle
//
// External dependency in your project:
// - BitMapClass.RoundRect(RectangleF rect, Padding rounding)

using Ledger.BitUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;



namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(PictureBox))]
    public class CuiPictureBox : UserControl
    {
        // Cached, tinted bitmap and brush used for painting.
        private Image _cachedTintedImage;
        private TextureBrush _cachedBrush;

        // Source image (as assigned by user).
        private Image _image;

        private Padding _rounding = new Padding(8, 8, 8, 8);

        private Color _imageTint = Color.White;
        private int _rotationDegrees;

        private Color _outlineColor = Color.Empty;
        private float _outlineThickness = 1f;

        private IContainer components;

        public CuiPictureBox()
        {
            InitializeComponent();
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);
        }

        /// <summary>
        /// Source image to display.
        /// NOTE: This control may dispose images in some cases (see OnPaint notes).
        /// </summary>
        [Category("CuoreUI")]
        public Image Content
        {
            get => _image;
            set
            {
                if (value != null)
                {
                    _image = value;

                    // Reset caches and rebuild tinted copy
                    _cachedTintedImage = null;
                    _cachedBrush = null;

                    RebuildTintedCache();
                }
                else
                {
                    _image = null;
                }

                Invalidate();
            }
        }

        [Category("CuoreUI")]
        public Padding Rounding
        {
            get => _rounding;
            set { _rounding = value; Invalidate(); }
        }

        [Category("CuoreUI")]
        public Color ImageTint
        {
            get => _imageTint;
            set
            {
                _imageTint = value;
                RebuildTintedCache();
                Invalidate();
            }
        }

        [Category("CuoreUI")]
        public int Rotation
        {
            get => _rotationDegrees;
            set
            {
                if (_rotationDegrees == value)
                    return;

                _rotationDegrees = value % 360;

                // Decompiled behavior: forces a rebuild by reassigning Content to itself.
                Content = Content;
            }
        }

        [Category("CuoreUI")]
        public Color PanelOutlineColor
        {
            get => _outlineColor;
            set { _outlineColor = value; Invalidate(); }
        }

        [Category("CuoreUI")]
        public float OutlineThickness
        {
            get => _outlineThickness;
            set { _outlineThickness = value; Invalidate(); }
        }

        private void RebuildTintedCache()
        {
            if (_image == null)
                return;

            // The original code does not dispose previous cached images/brushes here.
            // It only disposes them in an odd branch in OnPaint (see below).
            // We keep behavior minimal and faithful: overwrite caches.
            Bitmap tinted = new Bitmap(_image.Width, _image.Height);

            using (Graphics g = Graphics.FromImage(tinted))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                Color tint = ImageTint;

                var matrix = new ColorMatrix(new[]
                {
                    new float[] { tint.R / 255f, 0, 0, 0, 0 },
                    new float[] { 0, tint.G / 255f, 0, 0, 0 },
                    new float[] { 0, 0, tint.B / 255f, 0, 0 },
                    new float[] { 0, 0, 0, tint.A / 255f, 0 },
                    new float[] { 0, 0, 0, 0, 1f }
                });

                using (var attrs = new ImageAttributes())
                {
                    attrs.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    g.DrawImage(
                        _image,
                        new Rectangle(0, 0, _image.Width, _image.Height),
                        0, 0, _image.Width, _image.Height,
                        GraphicsUnit.Pixel,
                        attrs);
                }
            }

            _cachedBrush = new TextureBrush(tinted, WrapMode.Clamp);
            _cachedTintedImage = tinted; // keep a reference like the original
        }

        private void DisposeIfPossible(ref Image target)
        {
            try
            {
                target?.Dispose();
                GC.Collect(); // decompiled code calls this (not recommended, but preserved)
            }
            catch
            {
            }
            target = null;
        }

        private void DisposeIfPossible(ref TextureBrush target)
        {
            try
            {
                if (target != null)
                {
                    target.Image?.Dispose();
                    target.Dispose();
                }

                GC.Collect(); // preserved
            }
            catch
            {
            }
            target = null;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Decompiled behavior (important):
            // If _cachedTintedImage is null, it disposes *Content* and caches.
            //
            // This is risky because Content is typically "owned" by the caller.
            // I'm preserving it because it matches your decompile, but consider removing
            // the DisposeIfPossible(ref _image) call if you want normal WinForms semantics.
            if (_cachedTintedImage == null)
            {
                DisposeIfPossible(ref _image);
                DisposeIfPossible(ref _cachedTintedImage);
                DisposeIfPossible(ref _cachedBrush);
                return;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle rect = ClientRectangle;
            rect.Inflate(-1, -1);

            using (var outlinePen = new Pen(PanelOutlineColor, OutlineThickness))
            using (GraphicsPath path = BitMapClass.RoundRect(rect, 1))
            {
                ApplyBrushTransform(); // rotation + scale

                e.Graphics.FillPath(_cachedBrush, path);
                e.Graphics.DrawPath(outlinePen, path);
            }

            base.OnPaint(e);
        }

        private void ApplyBrushTransform()
        {
            if (_cachedBrush?.Image == null)
                return;

            Size imgSize = _cachedBrush.Image.Size;

            float scaleX = (float)Width / imgSize.Width;
            float scaleY = (float)Height / imgSize.Height;

            using (var m = new Matrix())
            {
                // Original order: RotateAt, then Scale.
                m.RotateAt(Rotation, new PointF(Width / 2f, Height / 2f));
                m.Scale(scaleX, scaleY);

                _cachedBrush.Transform = m;
            }
        }

        private void CuiPictureBox_Resize(object sender, EventArgs e)
        {
            if (Content == null)
                return;

            // Decompiled behavior: force re-tint/re-cache on resize
            Content = Content;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();

                // The original Dispose only disposed components.
                // If you want to avoid leaks, you can also dispose caches here.
                // (Not doing it keeps closer to decompiled behavior.)
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            Name = "cuiPictureBox";
            Resize += new EventHandler(CuiPictureBox_Resize);
            ResumeLayout(false);
        }
    }
}
