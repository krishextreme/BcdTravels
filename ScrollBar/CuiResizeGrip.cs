// Cleaned + deobfuscated version of the decompiled control.
// Original type: כ… .ܒW…
//
// What it is:
// - A bottom-right resize grip UserControl that resizes a target Form by dragging.
// - Draws a 2x3 (or 3x3 minus one) “square-dot” grip texture in the corner.
// - Uses a Timer + GetAsyncKeyState(VK_LBUTTON) to track drag while mouse is held.
//
// Notable quirks preserved from the decompile:
// - Resizing is done by: TargetForm.Size = TargetForm.Size - delta
//   where delta = (last.X - current.X, last.Y - current.Y). This effectively grows when you drag down/right.
// - Drag loop is timer-driven rather than pure mouse events.
// - TextureOffset exists but (in your snippet) wasn’t applied; I *apply* it in the grip drawing helper,
//   because it is clearly intended. If you want byte-for-byte behavior, tell me and I’ll remove it.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;
using Timer = System.Windows.Forms.Timer;


namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(Form))]
    public class CuiResizeGrip : UserControl
    {
        private static readonly Point UninitializedPoint = new Point(-1, -1);

        private Point _lastMousePoint = UninitializedPoint;

        private Form _targetForm;

        private Color _gripColor = Color.Gray;
        private bool _gripTexture = true;

        private readonly Timer _dragTimer = new Timer();

        private const int VK_LBUTTON = 1;

        private Size _textureOffset = new Size(-2, -2);
        private int _gripSize = 2; // "half size" in decompile; squares are size*2
        private bool _skipBottomRightSquare;

        private IContainer components;

        public CuiResizeGrip()
        {
            InitializeComponent();

            Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            _dragTimer.Tick += DragTimer_Tick;

            Size = new Size(24, 24);
            Cursor = Cursors.SizeNWSE;

            // Decompiled creates a second timer just to periodically update dragTimer.Interval
            // based on highest refresh rate.
            var refreshRateTimer = new Timer();
            refreshRateTimer.Interval = 1000;
            refreshRateTimer.Start();
            refreshRateTimer.Tick += (s, e) =>
                _dragTimer.Interval = 1024;
        }

        public Form TargetForm
        {
            get => _targetForm;
            set => _targetForm = value;
        }

        public Color GripColor
        {
            get => _gripColor;
            set { _gripColor = value; Refresh(); }
        }

        public bool GripTexture
        {
            get => _gripTexture;
            set { _gripTexture = value; Refresh(); }
        }

        public Size TextureOffset
        {
            get => _textureOffset;
            set { _textureOffset = value; Refresh(); }
        }

        public int GripSize
        {
            get => _gripSize;
            set { _gripSize = value; Refresh(); }
        }

        public bool SkipBottomRightSquare
        {
            get => _skipBottomRightSquare;
            set { _skipBottomRightSquare = value; Refresh(); }
        }

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            Focus();

            _lastMousePoint = UninitializedPoint;

            _dragTimer.Interval = 1024;
            _dragTimer.Stop();
            _dragTimer.Start();
        }

        private void DragTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (TargetForm == null)
                    return;

                // If left mouse is no longer down, stop dragging.
                if (((uint)GetAsyncKeyState(VK_LBUTTON) & 0x8000U) == 0U)
                {
                    _dragTimer.Stop();
                    return;
                }

                if (_lastMousePoint == UninitializedPoint)
                    _lastMousePoint = Cursor.Position;

                Point current = Cursor.Position;
                Point delta = GetDelta(current, _lastMousePoint);
                _lastMousePoint = current;

                // Decompile: Size.Subtract(TargetForm.Size, (Size)delta)
                TargetForm.Size = Size.Subtract(TargetForm.Size, (Size)delta);
            }
            catch (NullReferenceException)
            {
                // decompiled code swallowed this
            }
        }

        private static Point GetDelta(Point current, Point last)
        {
            // decompile: p2 - p1 (cast through double for no reason)
            return new Point((int)(double)(last.X - current.X), (int)(double)(last.Y - current.Y));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (GripTexture)
            {
                using (var brush = new SolidBrush(GripColor))
                using (GraphicsPath gp = CreateSquareGripPath(GripSize))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.None;
                    e.Graphics.FillPath(brush, gp);
                }
            }

            if (TargetForm != null)
            {
                // Keep this grip pinned to the bottom-right of the target form
                Location = new Point(TargetForm.Size.Width - Width, TargetForm.Size.Height - Height);
            }

            base.OnPaint(e);
        }

        /// <summary>
        /// Equivalent intent of SquareGripPath(int size) + its local function CreateAddRect.
        /// In the decompile, "size" is stored as halfSize, then doubled.
        /// It adds 6 squares (or 5 if SkipBottomRightSquare).
        /// </summary>
        private GraphicsPath CreateSquareGripPath(int halfSize)
        {
            int size = halfSize * 2; // decompile: size *= 2
            var gp = new GraphicsPath();

            // Layout (bottom-right corner), in "size" steps:
            // (x,y) = (W-size, H-size) is the bottom-right square.
            //
            // The decompile adds:
            // - (W-size,   H-size)        [optional]
            // - (W-size,   H-2*size)
            // - (W-size,   H-3*size)
            // - (W-2*size, H-size)
            // - (W-3*size, H-size)
            // - (W-2*size, H-2*size)

            if (!SkipBottomRightSquare)
                CreateAddRect(gp, Width - size, Height - size, halfSize);

            CreateAddRect(gp, Width - size, Height - size * 2, halfSize);
            CreateAddRect(gp, Width - size, Height - size * 3, halfSize);

            CreateAddRect(gp, Width - size * 2, Height - size, halfSize);
            CreateAddRect(gp, Width - size * 3, Height - size, halfSize);

            CreateAddRect(gp, Width - size * 2, Height - size * 2, halfSize);

            return gp;
        }

        /// <summary>
        /// Reconstructed local function:
        /// <SquareGripPath>g__CreateAddRect|24_0(int x, int y, ref closure)
        ///
        /// The closure contains:
        /// - halfSize
        /// - gp
        ///
        /// The presence of TextureOffset suggests the rect is nudged (default -2,-2).
        /// </summary>
        private void CreateAddRect(GraphicsPath gp, int x, int y, int halfSize)
        {
            int size = halfSize * 2;

            // Apply the (intended) texture offset so the dots sit a bit inward.
            x += TextureOffset.Width;
            y += TextureOffset.Height;

            gp.AddRectangle(new Rectangle(x, y, size, size));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                // NOTE: original code didn’t dispose timers explicitly; WinForms timers are components,
                // but these were created manually. Disposing is safer.
                _dragTimer?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // CuiResizeGrip
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Margin = new Padding(4, 3, 4, 3);
            Name = "CuiResizeGrip";
            Size = new Size(409, 233);
            ResumeLayout(false);
        }
    }
}
