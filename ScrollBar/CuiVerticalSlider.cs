// Cleaned + renamed version of the decompiled control.
// Original type: כ… .צ…
//
// What it is:
// - A vertical slider (track + circular thumb) that maps mouse Y to Value.
// - Value is effectively integer-stepped because setter does: _value = (float)(int)value;
// - Supports "UpsideDown" (invert direction).
//
// External dependencies (left as aliases):
// - Theme.PrimaryColor
// - BitMapClass.RoundRect(...)

using Ledger.BitUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;


namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(TrackBar))]
    [DefaultEvent(nameof(ValueChanged))]
    public class CuiVerticalSlider : UserControl
    {
        private bool _upsideDown;

        private float _value = 100f;
        private float _minValue = 0f;
        private float _maxValue = 100f;

        private Color _trackColor = Color.FromArgb(64, 128, 128, 128);
        private Color _thumbColor = Theme.PrimaryColor;

        private RectangleF _thumbRect = RectangleF.Empty;
        private int _thumbOutlineThickness = 3;

        private IContainer components;

        public CuiVerticalSlider()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public bool UpsideDown
        {
            get => _upsideDown;
            set
            {
                _upsideDown = value;
                Refresh();
            }
        }

        public event EventHandler ValueChanged;

        public float Value
        {
            get => _value;
            set
            {
                if (value < _minValue || value > _maxValue)
                    return;

                bool changed = value != _value;

                // Decompiled truncates to int (so slider is integer-stepped even though float)
                _value = (int)value;

                UpdateThumbRectangle();
                Refresh();

                if (changed)
                    ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public float MinValue
        {
            get => _minValue;
            set
            {
                if (value >= _maxValue || value > _value)
                    return;

                _minValue = value;
                Refresh();
            }
        }

        public float MaxValue
        {
            get => _maxValue;
            set
            {
                if (value <= _minValue || value < _value)
                    return;

                _maxValue = value;
                Refresh();
            }
        }

        public Color TrackColor
        {
            get => _trackColor;
            set
            {
                _trackColor = value;
                Refresh();
            }
        }

        public Color ThumbColor
        {
            get => _thumbColor;
            set
            {
                _thumbColor = value;
                Refresh();
            }
        }

        public int ThumbOutlineThickness
        {
            get => _thumbOutlineThickness;
            set
            {
                _thumbOutlineThickness = value;
                Refresh();
            }
        }

        public double GetProgress()
        {
            return (double)MaxValue == (double)MinValue
                ? 0.0
                : ((double)Value - (double)MinValue) / ((double)MaxValue - (double)MinValue);
        }

        // Decompiled oddity: returns abs(-progress) * 2 => progress*2 in [0,2]
        // Used in thumb Y calculation.
        private double GetProgressHalfNormalized()
        {
            double x = -GetProgress();
            if (x < 0.0)
                x = -x;
            return x * 2.0;
        }

        private void UpdateThumbRectangle()
        {
            float thumbSize = (float)(Width / 8.0 * 5.0);
            float halfThumb = thumbSize / 2f;

            double p2 = GetProgressHalfNormalized();

            _thumbRect = new RectangleF(
                (float)(Width / 2 - (double)halfThumb - 1.0),
                (float)(Height * GetProgress()
                        - (double)_thumbRect.Height / 2.0 * p2
                        - 1.0 * p2),
                thumbSize,
                thumbSize);

            if (UpsideDown)
            {
                _thumbRect.Y = (float)(Height - (double)_thumbRect.Y - (double)_thumbRect.Height - 2.0);
            }
        }

        private void UpdateThumbRectangle(out float halfThumb)
        {
            float thumbSize = (float)(Width / 8.0 * 5.0);
            float half = thumbSize / 2f;

            double p2 = GetProgressHalfNormalized();

            _thumbRect = new RectangleF(
                (float)(Width / 2 - (double)half - 1.0),
                (float)(Height * GetProgress()
                        - (double)_thumbRect.Height / 2.0 * p2
                        - 1.0 * p2),
                thumbSize,
                thumbSize);

            if (UpsideDown)
            {
                _thumbRect.Y = (float)(Height - (double)_thumbRect.Y - (double)_thumbRect.Height - 2.0);
            }

            halfThumb = half;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Track rect: thin rounded vertical bar centered in control
            RectangleF trackRect = new RectangleF(
                0f,
                0f,
                Width / 8 + 0.5f,
                Height - 1);

            trackRect.X = (float)(Width / 2 - (double)trackRect.Width / 2.0 - 0.5);

            UpdateThumbRectangle(out float halfThumb);
            trackRect.Inflate(0f, -halfThumb);
            Rectangle rectangle = Rectangle.Ceiling(trackRect);
            using (GraphicsPath trackPath = BitMapClass.RoundRect(rectangle, (int)(((double)trackRect.Width + 0.5) / 2.0)))
            using (var trackBrush = new SolidBrush(TrackColor))
            {
                e.Graphics.FillPath(trackBrush, trackPath);
            }

            // Thumb: ellipse with "outline" created by drawing the thumb rect using a pen of BackColor
            using (var outlinePen = new Pen(BackColor, ThumbOutlineThickness))
            using (var thumbBrush = new SolidBrush(ThumbColor))
            {
                e.Graphics.DrawRectangles(outlinePen, new[] { _thumbRect }); // decompiled odd choice
                e.Graphics.FillEllipse(thumbBrush, _thumbRect);
            }

            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            UpdateThumbRectangle();
            base.OnResize(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();

            // Decompiled uses Cursor.Position rather than e.Location.
            Point p = PointToClient(Cursor.Position);
            OnMouseMove(new MouseEventArgs(MouseButtons.Left, 1, p.X, p.Y, 0));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button != MouseButtons.Left)
                return;

            float thumbH = _thumbRect.Height;

            float t = Clamp(
                (e.Y - thumbH / 2f) / (Height - thumbH),
                0.0f,
                1f);

            if (UpsideDown)
                t = 1f - t;

            Value = MinValue + t * (MaxValue - MinValue);
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            return value > max ? max : value;
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
            Name = "cuiSliderVertical";
            Size = new Size(24, 228);
            ResumeLayout(false);
        }
    }
}
