// Cleaned + deobfuscated version of the decompiled control.
// Original type: כ… .ΝSᚑᚒ…
//
// What it is:
// - A circular knob / dial control.
// - Dragging the mouse around the circle maps angle (0..360) to Value (MinValue..MaxValue).
// - Draws a circular track, and optionally:
//   - a thumb indicator (small circle)
//   - an arc indicator (filled arc portion)
//   - or both (Combined)
// - Optionally draws the numeric Value in the center.
//
// External dependency kept as alias (default thumb color):

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;

namespace Ledger.ScrollBar
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(TrackBar))]
    [DefaultEvent(nameof(ValueChanged))]
    public class CuiCircleKnob : UserControl
    {
        private bool _isDragging;

        private Color _trackColor = Color.FromArgb(64, 128, 128, 128);

        private float _halfTrackThickness;
        private int _trackThickness = 2;

        private Color _thumbColor = Theme.PrimaryColor;

        private float _value = 100f;
        private float _minValue = 0f;
        private float _maxValue = 360f;

        private bool _showValueText = true;

        private KnobVisualStyle _knobStyle = KnobVisualStyle.Combined;

        private RectangleF _thumbRect = RectangleF.Empty;

        private IContainer components;

        public CuiCircleKnob()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            ForeColor = Color.FromArgb(128, 128, 128, 128);

            // Ensure derived geometry is correct on first paint.
            TrackThickness = _trackThickness;
            UpdateThumbRectangle();
        }

        // --------------------
        // Interaction
        // --------------------
        protected override void OnMouseDown(MouseEventArgs e)
        {
            _isDragging = true;
            UpdateValueFromMouse(e.Location);
            Focus();
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_isDragging)
                UpdateValueFromMouse(e.Location);

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isDragging = false;
            base.OnMouseUp(e);
        }

        private void UpdateValueFromMouse(Point mousePosition)
        {
            float cx = Width / 2f;
            float cy = Height / 2f;

            float dx = mousePosition.X - cx;

            // Decompiled mapping:
            // angle = atan2(y - cy, x - cx) * 180/pi + 90
            float angleDegrees = (float)(Math.Atan2(mousePosition.Y - cy, dx) * (180.0 / Math.PI) + 90.0);
            if (angleDegrees < 0f)
                angleDegrees += 360f;

            Value = MinValue + (float)(angleDegrees / 360.0 * (MaxValue - MinValue));
        }

        // --------------------
        // Public API
        // --------------------
        [Category("CuoreUI")]
        public Color TrackColor
        {
            get => _trackColor;
            set { _trackColor = value; Refresh(); }
        }

        [Category("CuoreUI")]
        public int TrackThickness
        {
            get => _trackThickness;
            set
            {
                _trackThickness = value;
                _halfTrackThickness = value / 2f;
                UpdateThumbRectangle();
                Refresh();
            }
        }

        [Category("CuoreUI")]
        public Color ThumbColor
        {
            get => _thumbColor;
            set { _thumbColor = value; Refresh(); }
        }

        public event EventHandler ValueChanged;

        [Category("CuoreUI")]
        public float Value
        {
            get => _value;
            set
            {
                if (value < _minValue || value > _maxValue)
                    return;

                bool changed = value != _value;

                // Decompiled behavior: value is truncated to an integer.
                _value = (int)value;

                UpdateThumbRectangle();
                Refresh();

                if (changed)
                    ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Category("CuoreUI")]
        public float MinValue
        {
            get => _minValue;
            set
            {
                if (value >= _maxValue || value > _value)
                    return;

                _minValue = value;
                UpdateThumbRectangle();
                Refresh();
            }
        }

        [Category("CuoreUI")]
        public float MaxValue
        {
            get => _maxValue;
            set
            {
                if (value <= _minValue || value < _value)
                    return;

                _maxValue = value;
                UpdateThumbRectangle();
                Refresh();
            }
        }

        [Category("CuoreUI")]
        public bool ShowValueText
        {
            get => _showValueText;
            set { _showValueText = value; Refresh(); }
        }

        [Category("CuoreUI")]
        public KnobVisualStyle KnobStyle
        {
            get => _knobStyle;
            set { _knobStyle = value; Refresh(); }
        }

        // --------------------
        // Layout / geometry
        // --------------------
        protected override void OnResize(EventArgs e)
        {
            UpdateThumbRectangle();

            // Decompiled behavior: forces square control
            Width = Height;

            base.OnResize(e);
        }

        private void UpdateThumbRectangle()
        {
            float cx = Width / 2f;
            float cy = Height / 2f;

            float radius =
                Math.Min(Width, Height) / 2f
                - _halfTrackThickness
                - Height / 8f;

            // Decompiled angle:
            // radians = ((Value - Min)/(Max - Min) * 360 - 90) * pi/180
            double radians =
                ((Value - MinValue) / (MaxValue - MinValue) * 360.0 - 90.0) * (Math.PI / 180.0);

            float knobRadius = Height / 8f;       // thumb circle radius
            float knobDiameter = Height / 4f;     // thumb circle diameter

            _thumbRect = new RectangleF(
                cx + (int)(radius * Math.Cos(radians)) - knobRadius,
                cy + (int)(radius * Math.Sin(radians)) - knobRadius,
                knobDiameter,
                knobDiameter);
        }

        // --------------------
        // Painting
        // --------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            // The decompiler showed a compiler-generated closure struct and two local functions:
            //  - <OnPaint>g__DrawThumbStyle|46_0
            //  - <OnPaint>g__DrawArcStyle|46_1
            //
            // This rewrite inlines them as private methods.

            RectangleF trackRect = (RectangleF)ClientRectangle;
            trackRect.Inflate(-1f, -1f);
            trackRect.Inflate(-_halfTrackThickness, -_halfTrackThickness);
            trackRect.Inflate(-(Height / 8f), -(Height / 8f));

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            using (var trackPen = new Pen(TrackColor, _trackThickness))
                e.Graphics.DrawEllipse(trackPen, trackRect);

            switch (KnobStyle)
            {
                case KnobVisualStyle.Thumb:
                    DrawThumbStyle(e.Graphics);
                    break;

                case KnobVisualStyle.Arc:
                    DrawArcStyle(e.Graphics, trackRect);
                    break;

                case KnobVisualStyle.Combined:
                    DrawArcStyle(e.Graphics, trackRect);
                    DrawThumbStyle(e.Graphics);
                    break;
            }

            if (ShowValueText)
            {
                using (var format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                })
                using (var brush = new SolidBrush(ForeColor))
                {
                    e.Graphics.DrawString(Value.ToString(), Font, brush, trackRect, format);
                }
            }

            base.OnPaint(e);
        }

        private void DrawThumbStyle(Graphics g)
        {
            using (var brush = new SolidBrush(ThumbColor))
            {
                g.FillEllipse(brush, _thumbRect);
            }
        }

        private void DrawArcStyle(Graphics g, RectangleF trackRect)
        {
            // Arc shows current value as sweep from "top" (12 o'clock) clockwise.
            // This matches the -90 shift used in UpdateThumbRectangle.
            float sweep = 0f;

            if (MaxValue != MinValue)
                sweep = (Value - MinValue) / (MaxValue - MinValue) * 360f;

            // Use a rounded-cap pen to look like a modern knob indicator.
            using (var pen = new Pen(ThumbColor, _trackThickness)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            })
            {
                g.DrawArc(pen, trackRect, -90f, sweep);
            }
        }

        // --------------------
        // Boilerplate
        // --------------------
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
            // CuiCircleKnob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CuiCircleKnob";
            this.Size = new System.Drawing.Size(96, 89);
            this.Load += new System.EventHandler(this.CuiCircleKnob_Load);
            this.ResumeLayout(false);

        }

        public enum KnobVisualStyle
        {
            Thumb,
            Arc,
            Combined
        }

        private void CuiCircleKnob_Load(object sender, EventArgs e)
        {

        }
    }
}
