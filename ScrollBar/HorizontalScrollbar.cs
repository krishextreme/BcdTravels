// Decompiled with JetBrains decompiler (renamed for readability)
// Original type: כ….\uFFFD\uFFFD\uD800\uDD0A\uFFFD\uFFFDΣܗ\uFFFD\uFFFDܕ\uFFFD
//
// Purpose:
// - A custom owner-drawn horizontal scrollbar that "binds" to a ScrollableControl's HorizontalScroll.
// - It positions itself at the bottom of the target control and mirrors scroll/resize changes.
// - It also hooks the target's window messages (via a NativeWindow) to update thumb position.

using Ledger.BitUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


// External dependencies (obfuscated in your sample):
// - Theme.PrimaryColor + Theme.TranslucentPrimaryColor
// - BitMapClass.RoundRect(Rectangle rect, int radius)
using Theme = Ledger.UIHelper.UIGraphicsHelper;



namespace Ledger.ScrollBar // placeholder namespace
{
    [ToolboxBitmap(typeof(HScrollBar))]
    public class HorizontalScrollbar : Control
    {
        private int _thumbWidth;
        private ScrollableControl _target;

        private TargetMessageHook _messageHook;

        private bool _isThumbHovered;
        private bool _isThumbPressed;

        private int _thumbX;
        private int _mouseDownX;
        private int _thumbXOnMouseDown;

        private int _cornerRadius = 5;

        private Color _trackColor = Color.Transparent;
        private Color _thumbColor = Theme.PrimaryColor;
        private Color _thumbHoverColor = Theme.TranslucentPrimaryColor;
        private Color _thumbPressedColor = Theme.TranslucentPrimaryColor;

        private IContainer components;

        public HorizontalScrollbar()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);

            _thumbWidth = 50;
            MinimumSize = new Size(50, 20);
        }

        public int ThumbWidth
        {
            get => _thumbWidth;
            set
            {
                _thumbWidth = value;
                Invalidate();
            }
        }

        /// <summary>
        /// The scrollable control whose HorizontalScroll this scrollbar controls.
        /// </summary>
        public ScrollableControl TargetControl
        {
            get => _target;
            set
            {
                if (_target == value)
                    return;

                if (_target != null)
                {
                    _target.Scroll -= Target_Scroll;
                    _target.Resize -= Target_Resize;
                    _messageHook?.ReleaseHandle();
                }

                if (Dock != DockStyle.None)
                    Dock = DockStyle.None;

                _target = value;

                if (_target == null)
                    return;

                _target.Scroll += Target_Scroll;
                _target.Resize += Target_Resize;

                _messageHook = new TargetMessageHook(this);
                _messageHook.AssignHandle(_target.Handle);

                BindToTarget();
            }
        }

        public int Rounding
        {
            get => _cornerRadius;
            set
            {
                _cornerRadius = value;
                Invalidate();
            }
        }

        public Color Background
        {
            get => _trackColor;
            set
            {
                _trackColor = value;
                Invalidate();
            }
        }

        public Color ThumbColor
        {
            get => _thumbColor;
            set
            {
                _thumbColor = value;
                Invalidate();
            }
        }

        public Color HoveredThumbColor
        {
            get => _thumbHoverColor;
            set
            {
                _thumbHoverColor = value;
                Invalidate();
            }
        }

        public Color PressedThumbColor
        {
            get => _thumbPressedColor;
            set
            {
                _thumbPressedColor = value;
                Invalidate();
            }
        }

        private void BindToTarget()
        {
            if (_target == null)
                return;

            Width = _target.Width;
            Location = new Point(_target.Left, _target.Bottom - Height);
            BringToFront();

            UpdateThumbFromTarget();

            // Decompiler logic: if we're not already parented directly to a Form, add to the Form.
            if (Parent is Form)
                return;

            FindForm()?.Controls.Add(this);
        }

        private void Target_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateThumbFromTarget();
            Refresh();
        }

        private void Target_Resize(object sender, EventArgs e)
        {
            BindToTarget();
            Refresh();
        }

        private void UpdateThumbFromTarget()
        {
            if (_target == null)
                return;

            // denominator is (Maximum - LargeChange + 1) as per WinForms scrolling conventions
            int denom = _target.HorizontalScroll.Maximum - _target.HorizontalScroll.LargeChange + 1;
            if (denom <= 0)
            {
                _thumbX = 0;
                Invalidate();
                return;
            }

            float t = (float)_target.HorizontalScroll.Value / denom;
            _thumbX = (int)((Width - _thumbWidth) * t);

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (_target != null)
            {
                var desired = new Point(_target.Left, _target.Bottom - Height);
                if (Location != desired)
                    Location = desired;

                _target.AutoScroll = true;
                Width = _target.Width;
            }

            e.Graphics.Clear(BackColor);

            var trackRect = new Rectangle(0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            using (GraphicsPath trackPath = BitMapClass.RoundRect(trackRect, Rounding))
            using (SolidBrush trackBrush = new SolidBrush(Background))
            {
                e.Graphics.FillPath(trackBrush, trackPath);
            }

            var thumbRect = new Rectangle(_thumbX, 0, _thumbWidth - 1, Height - 1);
            using (GraphicsPath thumbPath = BitMapClass.RoundRect(thumbRect, Rounding))
            using (SolidBrush thumbBrush = new SolidBrush(
                       _isThumbPressed ? PressedThumbColor :
                       _isThumbHovered ? HoveredThumbColor :
                       ThumbColor))
            {
                e.Graphics.FillPath(thumbBrush, thumbPath);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left)
                return;

            if (!new Rectangle(_thumbX, 0, _thumbWidth, Height).Contains(e.Location))
                return;

            _isThumbPressed = true;
            Capture = true;

            _mouseDownX = e.X;
            _thumbXOnMouseDown = _thumbX;

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button != MouseButtons.Left)
                return;

            _isThumbPressed = false;
            Capture = false;

            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isThumbPressed)
            {
                int newThumbX = Math.Max(0, Math.Min(Width - _thumbWidth, _thumbXOnMouseDown + (e.X - _mouseDownX)));

                float t = Width - _thumbWidth <= 0 ? 0f : (float)newThumbX / (Width - _thumbWidth);

                if (_target != null)
                {
                    int denom = _target.HorizontalScroll.Maximum - _target.HorizontalScroll.LargeChange + 1;
                    if (denom > 0)
                        _target.HorizontalScroll.Value = (int)(denom * t);
                }

                _thumbX = newThumbX;
                Refresh();
                return;
            }

            bool isOverThumb = new Rectangle(_thumbX, 0, _thumbWidth, Height).Contains(e.Location);
            if (isOverThumb == _isThumbHovered)
                return;

            _isThumbHovered = isOverThumb;
            Invalidate();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                // preserve decompiled: createParams.Style |= 1;
                // 0x00000001 == WS_BORDER
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x00000001;
                return cp;
            }
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
            Name = "cuiScrollbarHorizontal";
            Size = new Size(258, 21);
            ResumeLayout(false);
        }

        /// <summary>
        /// Hooks messages from the target control to keep the thumb in sync with scroll changes.
        /// </summary>
        private sealed class TargetMessageHook : NativeWindow
        {
            private readonly HorizontalScrollbar _scrollbar;

            public TargetMessageHook(HorizontalScrollbar scrollbar)
            {
                _scrollbar = scrollbar;
            }

            protected override void WndProc(ref Message m)
            {
                // 0x0114 = WM_HSCROLL, 0x020A = WM_MOUSEWHEEL
                if (m.Msg == 0x0114 || m.Msg == 0x020A)
                    _scrollbar.UpdateThumbFromTarget();

                base.WndProc(ref m);
                _scrollbar.Refresh();
            }
        }
    }
}
