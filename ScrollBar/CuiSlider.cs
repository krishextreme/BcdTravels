// Deobfuscated / cleaned version of the decompiled control.
// Original type: כ… .ᚊגܐᚲΑ…
//
// What it is:
// - A custom horizontal slider (track + circular thumb) implemented as a UserControl.
// - Value is clamped via MinValue/MaxValue and updated by dragging.
// - Renders a rounded track and an ellipse thumb with an outline.
//
// Quirks preserved from the decompile (important if you're matching behavior):
// - Value is forcibly truncated to an integer: _value = (float)(int)value;
// - MinValue/MaxValue setters refuse changes that would exclude the current Value.
// - Thumb positioning uses a "half-normalized" progress factor; it’s effectively $$2*abs(progress)$$.
// - First UpdateThumbRectangle() call depends on ThumbRectangle.Width (initially 0), then stabilizes.
// - Track ends are inset by half the thumb size so the thumb stays within bounds visually.
//
// External dependencies kept as aliases/placeholders:
// - Theme.PrimaryColor
// - BitMapClass.RoundRect(RectangleF, int)

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
    public class CuiSlider : UserControl
    {
        private float _value = 100f;
        private float _minValue;
        private float _maxValue = 100f;

        private Color _trackColor = Color.FromArgb(64, 128, 128, 128);
        private Color _thumbColor = Theme.PrimaryColor;

        private RectangleF _thumbRect = RectangleF.Empty;
        private int _thumbOutlineThickness = 3;

        private IContainer components;

        public CuiSlider()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public event EventHandler ValueChanged;

        /// <summary>
        /// Normalized progress in $$[0,1]$$ (unless Min==Max, where it returns 0).
        /// </summary>
        public double GetProgress()
        {
            return (double)MaxValue == (double)MinValue
                ? 0.0
                : ((double)Value - (double)MinValue) / ((double)MaxValue - (double)MinValue);
        }

        /// <summary>
        /// Decompiled: $$2*abs(progress)$$ (name reflects behavior, not intent).
        /// </summary>
        private double GetProgressHalfNormalized()
        {
            double x = -GetProgress();
            if (x < 0.0)
                x = -x;
            return x * 2.0;
        }

        public float Value
        {
            get => _value;
            set
            {
                if ((double)value < _minValue || (double)value > _maxValue)
                    return;

                bool changed = (double)value != _value;

                // Decompiled truncation (kept): slider is effectively integer-stepped.
                _value = (int)value;

                UpdateThumbRectangle();
                Refresh();

                if (!changed)
                    return;

                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public float MinValue
        {
            get => _minValue;
            set
            {
                // Refuse invalid range or excluding current value (decompiled behavior).
                if ((double)value >= _maxValue || (double)value > _value)
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
                // Refuse invalid range or excluding current value (decompiled behavior).
                if ((double)value <= _minValue || (double)value < _value)
                    return;

                _maxValue = value;
                Refresh();
            }
        }

        public Color TrackColor
        {
            get => _trackColor;
            set { _trackColor = value; Refresh(); }
        }

        public Color ThumbColor
        {
            get => _thumbColor;
            set { _thumbColor = value; Refresh(); }
        }

        public int ThumbOutlineThickness
        {
            get => _thumbOutlineThickness;
            set { _thumbOutlineThickness = value; Refresh(); }
        }

        private void UpdateThumbRectangle()
        {
            float thumbSize = (float)(Height / 8.0 * 5.0); // 5/8 of height
            float halfThumb = thumbSize / 2f;

            double p2 = GetProgressHalfNormalized();

            _thumbRect = new RectangleF(
                (float)(Width * GetProgress()
                        - (double)_thumbRect.Width / 2.0 * p2
                        - 1.0 * p2),
                (float)(Height / 2 - (double)halfThumb - 1.0),
                thumbSize,
                thumbSize);
        }

        private void UpdateThumbRectangle(out float halfThumb)
        {
            float thumbSize = (float)(Height / 8.0 * 5.0);
            float ht = thumbSize / 2f;

            double p2 = GetProgressHalfNormalized();

            _thumbRect = new RectangleF(
                (float)(Width * GetProgress()
                        - (double)_thumbRect.Width / 2.0 * p2
                        - 1.0 * p2),
                (float)(Height / 2 - (double)ht - 1.0),
                thumbSize,
                thumbSize);

            halfThumb = ht;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Track rect: thin bar centered vertically.
            RectangleF trackRect = new RectangleF(
                0f,
                0f,
                Width - 1,
                Height / 8 + 0.5f);

            trackRect.Y = (float)(Height / 2 - (double)trackRect.Height / 2.0 - 0.5);

            UpdateThumbRectangle(out float halfThumb);

            // Inset track so rounded ends don't overlap thumb bounds visually.
            trackRect.Inflate(-halfThumb, 0f);
            using (GraphicsPath trackPath =
                   BitMapClass.RoundRect(Rectangle.Round(trackRect), (int)(((double)trackRect.Height + 0.5) / 2.0)))
            using (SolidBrush trackBrush = new SolidBrush(TrackColor))
            {
                e.Graphics.FillPath(trackBrush, trackPath);
            }

            using (Pen outlinePen = new Pen(BackColor, ThumbOutlineThickness))
            using (SolidBrush thumbBrush = new SolidBrush(ThumbColor))
            {
                // Decompiled uses DrawRectangles with a single RectangleF (kept).
                e.Graphics.DrawRectangles(outlinePen, new[] { _thumbRect });
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

            // Decompiled uses Cursor.Position -> PointToClient and forwards as MouseMove with Left button.
            Point p = PointToClient(Cursor.Position);
            OnMouseMove(new MouseEventArgs(MouseButtons.Left, 1, p.X, p.Y, 0));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button != MouseButtons.Left)
                return;

            float thumbWidth = _thumbRect.Width;

            Value =
                MinValue +
                Clamp((e.X - thumbWidth / 2f) / (Width - thumbWidth), 0.0f, 1f)
                * (MaxValue - MinValue);
        }

        public static float Clamp(float value, float min, float max)
        {
            if ((double)value < (double)min)
                return min;

            return (double)value > (double)max ? max : value;
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
            Name = "cuiSlider";
            Size = new Size(228, 24);
            ResumeLayout(false);
        }
    }
}
