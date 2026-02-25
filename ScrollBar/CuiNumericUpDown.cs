// Cleaned + deobfuscated version of the decompiled control.
// Original type: כ… .מ𐀰…
//
// What it is:
// - A tiny 2-button numeric up/down control (top half increments, bottom half decrements).
// - Buttons have 3 visual states (Normal/Hovered/Pressed) stored as bytes.
// - Exposes Value/MinValue/MaxValue/StepSize and raises ValueChanged.
//
// Notes vs decompile:
// - The decompiler references a compiler-generated local function: <OnPaint>g__DrawButton|72_0
//   but its body is not shown in your snippet.
// - The code below reconstructs that function based on the fields and the way it’s called.
//   If you paste the missing local-function body (or the nested compiler-generated type),
//   I can match the original drawing logic exactly (colors, corner rounding per half, arrow path, etc.).

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;


namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(NumericUpDown))]
    [DefaultEvent(nameof(ValueChanged))]
    public class CuiNumericUpDown : UserControl
    {
        private Padding _rounding = new Padding(4, 4, 4, 4);

        private Color _normalBackground = Theme.PrimaryColor;
        private Color _hoverBackground = Theme.TranslucentPrimaryColor;
        private Color _pressedBackground = Theme.PrimaryColor;

        private Color _normalOutline = Color.Empty;
        private Color _hoverOutline = Color.Empty;
        private Color _pressedOutline = Color.Empty;

        private Color _normalArrowColor = Color.FromArgb(128, 255, 255, 255);
        private Color _hoverArrowColor = Color.FromArgb(255, 255, 255, 255);
        private Color _pressedArrowColor = Color.FromArgb(128, 255, 255, 255);

        // Decompiled: top button is btn1State, bottom button is btn2State.
        private byte _topButtonState = (byte)ButtonVisualState.Normal;
        private byte _bottomButtonState = (byte)ButtonVisualState.Normal;

        private float _value = 50f;
        private float _minValue = 0f;
        private float _maxValue = 100f;
        private float _stepSize = 5f;

        private IContainer components;

        public CuiNumericUpDown()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        // --------------------
        // Public API
        // --------------------
        public Padding Rounding
        {
            get => _rounding;
            set { _rounding = value; Invalidate(); }
        }

        public Color NormalBackground
        {
            get => _normalBackground;
            set { _normalBackground = value; Invalidate(); }
        }

        public Color HoverBackground
        {
            get => _hoverBackground;
            set { _hoverBackground = value; Invalidate(); }
        }

        public Color PressedBackground
        {
            get => _pressedBackground;
            set { _pressedBackground = value; Invalidate(); }
        }

        public Color NormalOutline
        {
            get => _normalOutline;
            set { _normalOutline = value; Invalidate(); }
        }

        public Color HoverOutline
        {
            get => _hoverOutline;
            set { _hoverOutline = value; Invalidate(); }
        }

        public Color PressedOutline
        {
            get => _pressedOutline;
            set { _pressedOutline = value; Invalidate(); }
        }

        public Color NormalArrowColor
        {
            get => _normalArrowColor;
            set { _normalArrowColor = value; Invalidate(); }
        }

        public Color HoverArrowColor
        {
            get => _hoverArrowColor;
            set { _hoverArrowColor = value; Invalidate(); }
        }

        public Color PressedArrowColor
        {
            get => _pressedArrowColor;
            set { _pressedArrowColor = value; Invalidate(); }
        }

        public float Value
        {
            get => _value;
            set
            {
                value = Math.Min(_maxValue, Math.Max(value, _minValue));
                bool changed = value != _value;

                _value = value;
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

        public float StepSize
        {
            get => _stepSize;
            set { _stepSize = value; Refresh(); }
        }

        public event EventHandler ValueChanged;

        // --------------------
        // Internal state properties (decompiled behavior)
        // --------------------
        private byte TopButtonState
        {
            get => _topButtonState;
            set
            {
                if (_topButtonState == value) return;
                _topButtonState = value;
                Refresh();
            }
        }

        private byte BottomButtonState
        {
            get => _bottomButtonState;
            set
            {
                if (_bottomButtonState == value) return;
                _bottomButtonState = value;
                Refresh();
            }
        }

        // --------------------
        // Input handling (matches the decompile)
        // --------------------
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Decompiled uses PointToClient(Cursor.Position) rather than e.Location.
            if (PointToClient(Cursor.Position).Y > Height / 2)
            {
                TopButtonState = (byte)ButtonVisualState.Normal;
                BottomButtonState = (byte)ButtonVisualState.Pressed;
                Value -= StepSize;
            }
            else
            {
                TopButtonState = (byte)ButtonVisualState.Pressed;
                BottomButtonState = (byte)ButtonVisualState.Normal;
                Value += StepSize;
            }

            // Decompiled: calls OnClick((EventArgs)e)
            OnClick(e);
            Focus();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (PointToClient(Cursor.Position).Y > Height / 2)
            {
                TopButtonState = (byte)ButtonVisualState.Normal;
                BottomButtonState = (byte)ButtonVisualState.Hovered;
            }
            else
            {
                TopButtonState = (byte)ButtonVisualState.Hovered;
                BottomButtonState = (byte)ButtonVisualState.Normal;
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            TopButtonState = (byte)ButtonVisualState.Normal;
            BottomButtonState = (byte)ButtonVisualState.Normal;
            base.OnMouseLeave(e);
        }

        // --------------------
        // Painting
        // --------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int halfHeight = Height / 2;

            // Decompiled: new Rectangle(0, 0, Width - 2, halfHeight)
            Rectangle btnRect = new Rectangle(0, 0, Width - 2, halfHeight);

            byte currentBtn = 0; // 0 = top, 1 = bottom

            DrawButton(e.Graphics, btnRect, (ButtonVisualState)TopButtonState, currentBtn);

            // Decompiled: Offset(0, halfHeight - 1)
            btnRect.Offset(0, halfHeight - 1);

            // Decompiled: for odd heights draw a 1px separator line in NormalBackground.
            if (Height % 2 == 1)
            {
                btnRect.Y += 1;
                using (var pen = new Pen(NormalBackground))
                {
                    e.Graphics.DrawLine(
                        pen,
                        new PointF(btnRect.Location.X + 0.5f, btnRect.Location.Y),
                        new PointF(Width - 2.5f, btnRect.Location.Y));
                }
            }

            currentBtn++;
            DrawButton(e.Graphics, btnRect, (ButtonVisualState)BottomButtonState, currentBtn);

            base.OnPaint(e);
        }

        /// <summary>
        /// Reconstructed equivalent of: <OnPaint>g__DrawButton|72_0(int state, ref closure)
        /// </summary>
        private void DrawButton(Graphics g, Rectangle rect, ButtonVisualState state, byte whichButton)
        {
            // whichButton: 0 = top, 1 = bottom
            bool isTop = whichButton == 0;

            Color bg;
            Color outline;
            Color arrow;

            switch (state)
            {
                case ButtonVisualState.Hovered:
                    bg = HoverBackground;
                    outline = HoverOutline;
                    arrow = HoverArrowColor;
                    break;

                case ButtonVisualState.Pressed:
                    bg = PressedBackground;
                    outline = PressedOutline;
                    arrow = PressedArrowColor;
                    break;

                default:
                    bg = NormalBackground;
                    outline = NormalOutline;
                    arrow = NormalArrowColor;
                    break;
            }

            // Only round the outer corners (top corners for the top button, bottom corners for bottom button).
            // This matches typical 2-segment rounded controls and aligns with the existence of a per-corner Padding.
            Padding r = Rounding;
            var perCorner = isTop
                ? new Padding(r.Left, r.Top, r.Right, 0)
                : new Padding(0, 0, r.Right, r.Bottom);

            using (GraphicsPath path = CreateRoundRectPath(rect, perCorner))
            using (var fill = new SolidBrush(bg))
            {
                g.FillPath(fill, path);

                if (outline != Color.Empty)
                {
                    using (var pen = new Pen(outline, 1f))
                        g.DrawPath(pen, path);
                }
            }

            // Arrow glyph (simple triangle). Exact arrow geometry may differ from the original.
            Rectangle arrowRect = rect;
            arrowRect.Inflate(-(rect.Width / 4), -(rect.Height / 4));

            using (var arrowBrush = new SolidBrush(arrow))
            using (GraphicsPath arrowPath = isTop ? CreateUpArrowPath(arrowRect) : CreateDownArrowPath(arrowRect))
            {
                g.FillPath(arrowBrush, arrowPath);
            }
        }

        // --------------------
        // Small drawing helpers
        // --------------------
        private static GraphicsPath CreateRoundRectPath(Rectangle rect, Padding radius)
        {
            int tl = Math.Max(0, radius.Left);
            int tr = Math.Max(0, radius.Top);
            int br = Math.Max(0, radius.Right);
            int bl = Math.Max(0, radius.Bottom);

            var path = new GraphicsPath();
            path.StartFigure();

            // Top-left
            if (tl > 0)
                path.AddArc(rect.X, rect.Y, tl * 2, tl * 2, 180, 90);
            else
                path.AddLine(rect.X, rect.Y, rect.X, rect.Y);

            // Top edge + top-right
            if (tr > 0)
                path.AddArc(rect.Right - tr * 2, rect.Y, tr * 2, tr * 2, 270, 90);
            else
                path.AddLine(rect.Right, rect.Y, rect.Right, rect.Y);

            // Right edge + bottom-right
            if (br > 0)
                path.AddArc(rect.Right - br * 2, rect.Bottom - br * 2, br * 2, br * 2, 0, 90);
            else
                path.AddLine(rect.Right, rect.Bottom, rect.Right, rect.Bottom);

            // Bottom edge + bottom-left
            if (bl > 0)
                path.AddArc(rect.X, rect.Bottom - bl * 2, bl * 2, bl * 2, 90, 90);
            else
                path.AddLine(rect.X, rect.Bottom, rect.X, rect.Bottom);

            path.CloseFigure();
            return path;
        }

        private static GraphicsPath CreateUpArrowPath(Rectangle r)
        {
            var path = new GraphicsPath();
            PointF p1 = new PointF(r.Left + r.Width / 2f, r.Top);
            PointF p2 = new PointF(r.Right, r.Bottom);
            PointF p3 = new PointF(r.Left, r.Bottom);
            path.AddPolygon(new[] { p1, p2, p3 });
            return path;
        }

        private static GraphicsPath CreateDownArrowPath(Rectangle r)
        {
            var path = new GraphicsPath();
            PointF p1 = new PointF(r.Left, r.Top);
            PointF p2 = new PointF(r.Right, r.Top);
            PointF p3 = new PointF(r.Left + r.Width / 2f, r.Bottom);
            path.AddPolygon(new[] { p1, p2, p3 });
            return path;
        }

        // --------------------
        // Designer / dispose
        // --------------------
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
            Name = "cuiNumericUpDown";
            Size = new Size(22, 40);
            ResumeLayout(false);
        }

        public enum ButtonVisualState : byte
        {
            Normal = 1,
            Hovered = 2,
            Pressed = 3
        }

        // Decompiled had a nested static class with these constants; enum is cleaner.
        public static class States
        {
            public const int Normal = 1;
            public const int Hovered = 2;
            public const int Pressed = 3;
        }
    }
}
