// Deobfuscated / cleaned version of the decompiled control.
// Original type: כ… .�ᚺᚐ…ܠ
//
// What it is:
// - A custom vertical scrollbar (drawn as a rounded track + rounded thumb) implemented as a Panel.
// - It “binds” itself to a ScrollableControl (TargetControl) and manipulates TargetControl.VerticalScroll.Value.
// - It also hooks the target's window messages to update thumb position on WM_VSCROLL / WM_MOUSEWHEEL.
//
// Notes / quirks preserved from decompile:
// - When TargetControl is set, the scrollbar forces DockStyle.None.
// - BindToTargetControl() may add this control to the owning Form if its parent isn't a Form.
// - OnPaint() contains a special-case for an unknown custom container type (placeholder alias below).
//
// External dependencies kept as aliases/placeholders:
// - Theme.PrimaryColor / Theme.TranslucentPrimaryColor
// - BitMapClass.RoundRect(Rectangle, int)
// - Unknown container type: \uFFFD\uFFFD\uD800\uDD05\uFFFDᚺZ\uFFFD\uFFFD\uFFFDܦ\uFFFD

using Ledger.BitUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;



namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(VScrollBar))]
    public class CuiVerticalScrollbar : Panel
    {
        private int _thumbHeight = 50;
        private ScrollableControl _targetControl;

        private TargetWindowMessageHook _messageHook;

        private bool _isThumbHovered;
        private bool _isThumbPressed;

        private int _thumbPosition;
        private int _initialMouseY;
        private int _initialThumbPosition;

        private int _rounding = 5;
        private Color _trackBackColor = Color.Transparent;

        private Color _thumbColor = Theme.PrimaryColor;
        private Color _hoveredThumbColor = Theme.TranslucentPrimaryColor;
        private Color _pressedThumbColor = Theme.TranslucentPrimaryColor;

        private IContainer components;

        public CuiVerticalScrollbar()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            _thumbHeight = 50;
            MinimumSize = new Size(20, 50);
        }

        public int ThumbHeight
        {
            get => _thumbHeight;
            set
            {
                _thumbHeight = Math.Max(1, value);
                Invalidate();
                UpdateThumbPosition();
            }
        }

        public ScrollableControl TargetControl
        {
            get => _targetControl;
            set
            {
                if (ReferenceEquals(_targetControl, value))
                    return;

                // Unhook old target
                if (_targetControl != null)
                {
                    _targetControl.Scroll -= TargetControl_Scroll;
                    _targetControl.Resize -= TargetControl_Resize;

                    _messageHook?.ReleaseHandle();
                    _messageHook = null;
                }

                // Decompiled forcibly undocks when binding.
                if (Dock != DockStyle.None)
                    Dock = DockStyle.None;

                _targetControl = value;

                if (_targetControl == null)
                    return;

                // Hook new target
                _targetControl.Scroll += TargetControl_Scroll;
                _targetControl.Resize += TargetControl_Resize;

                _messageHook = new TargetWindowMessageHook(this);
                _messageHook.AssignHandle(_targetControl.Handle);

                BindToTargetControl();
            }
        }

        public int Rounding
        {
            get => _rounding;
            set { _rounding = value; Invalidate(); }
        }

        public Color Background
        {
            get => _trackBackColor;
            set { _trackBackColor = value; Invalidate(); }
        }

        public Color ThumbColor
        {
            get => _thumbColor;
            set { _thumbColor = value; Invalidate(); }
        }

        public Color HoveredThumbColor
        {
            get => _hoveredThumbColor;
            set { _hoveredThumbColor = value; Invalidate(); }
        }

        public Color PressedThumbColor
        {
            get => _pressedThumbColor;
            set { _pressedThumbColor = value; Invalidate(); }
        }

        private void BindToTargetControl()
        {
            if (_targetControl == null)
                return;

            Height = _targetControl.Height;
            Location = new Point(_targetControl.Right - Width, _targetControl.Top);
            BringToFront();

            UpdateThumbPosition();

            // Decompiled behavior: if not already parented by a Form, add to owning Form.
            if (Parent is Form)
                return;

            FindForm()?.Controls.Add(this);
        }

        private void TargetControl_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateThumbPosition();
            Refresh();
        }

        private void TargetControl_Resize(object sender, EventArgs e)
        {
            BindToTargetControl();
            Refresh();
        }

        internal void UpdateThumbPosition()
        {
            if (_targetControl == null)
                return;

            int denom = _targetControl.VerticalScroll.Maximum - _targetControl.VerticalScroll.LargeChange + 1;
            if (denom <= 0)
            {
                _thumbPosition = 0;
                Invalidate();
                return;
            }

            float t = _targetControl.VerticalScroll.Value / (float)denom;
            _thumbPosition = (int)((Height - _thumbHeight) * t);

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (_targetControl != null)
            {
                var desired = new Point(_targetControl.Right - Width, _targetControl.Top);
                if (Location != desired)
                    Location = desired;

                _targetControl.AutoScroll = true;
                Height = _targetControl.Height;
            }

            e.Graphics.Clear(BackColor);

            Rectangle trackRect = new Rectangle(0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            Rectangle thumbRect = new Rectangle(0, _thumbPosition, Width - 1, _thumbHeight - 1);

            using (GraphicsPath trackPath = BitMapClass.RoundRect(trackRect, Rounding))
            using (Brush trackBrush = new SolidBrush(Background))
            {
                e.Graphics.FillPath(trackBrush, trackPath);
            }

            Color thumbFill = _isThumbPressed
                ? PressedThumbColor
                : _isThumbHovered ? HoveredThumbColor : ThumbColor;

            using (GraphicsPath thumbPath = BitMapClass.RoundRect(thumbRect, Rounding))
            using (Brush thumbBrush = new SolidBrush(thumbFill))
            {
                e.Graphics.FillPath(thumbBrush, thumbPath);
            }

            base.OnPaint(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left)
                return;

            if (!new Rectangle(0, _thumbPosition, Width, _thumbHeight).Contains(e.Location))
                return;

            _isThumbPressed = true;
            Capture = true;
            _initialMouseY = e.Y;
            _initialThumbPosition = _thumbPosition;

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

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            _isThumbPressed = false;
            Capture = false;
            _isThumbHovered = false;

            Refresh();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isThumbPressed)
            {
                int newThumbPos = Math.Max(0,
                    Math.Min(Height - _thumbHeight, _initialThumbPosition + (e.Y - _initialMouseY)));

                float t = Height - _thumbHeight <= 0 ? 0f : newThumbPos / (float)(Height - _thumbHeight);

                if (_targetControl != null)
                {
                    int denom = _targetControl.VerticalScroll.Maximum - _targetControl.VerticalScroll.LargeChange + 1;
                    if (denom > 0)
                    {
                        int newValue = (int)(denom * t);

                        // Keep within valid range to avoid ArgumentOutOfRangeException.
                        newValue = Math.Max(_targetControl.VerticalScroll.Minimum,
                            Math.Min(denom, newValue));

                        _targetControl.VerticalScroll.Value = newValue;
                    }
                }

                _thumbPosition = newThumbPos;
                Refresh();
            }
            else
            {
                bool hovered = new Rectangle(0, _thumbPosition, Width, _thumbHeight).Contains(e.Location);
                if (hovered == _isThumbHovered)
                    return;

                _isThumbHovered = hovered;
                Invalidate();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                // Decompiled: Style |= 1 (WS_BORDER).
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x00000001;
                return cp;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Ensure we unhook the target and release the native window hook.
                TargetControl = null;
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Name = "cuiScrollbar";
            Size = new Size(21, 258);
            ResumeLayout(false);
        }

        /// <summary>
        /// Hooks the TargetControl's window messages so we can update thumb position on scroll/mousewheel.
        /// </summary>
        private sealed class TargetWindowMessageHook : NativeWindow
        {
            private readonly CuiVerticalScrollbar _scrollbar;

            public TargetWindowMessageHook(CuiVerticalScrollbar scrollbar)
            {
                _scrollbar = scrollbar;
            }

            protected override void WndProc(ref Message m)
            {
                const int WM_VSCROLL = 0x0115;   // 277
                const int WM_MOUSEWHEEL = 0x020A; // 522

                if (m.Msg == WM_VSCROLL || m.Msg == WM_MOUSEWHEEL)
                    _scrollbar.UpdateThumbPosition();

                base.WndProc(ref m);
                _scrollbar.Refresh();
            }
        }
    }
}
