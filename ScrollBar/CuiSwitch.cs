// Cleaned + deobfuscated version of the decompiled control.
// Original type: כ… .ܙ…
//
// What it is:
// - A WinForms “switch/toggle” control (rounded track + circular thumb).
// - Click toggles Checked.
// - Thumb animates between left/right over ~350ms.
// - Optionally draws a checkmark/cross symbol inside the thumb.
//
// External dependency (kept as alias):
// - BitMapClass.RoundRect(Rectangle, int)
// - GraphicsUtil.Checkmark(Rectangle)
// - GraphicsUtil.Crossmark(Rectangle)
// - Theme.PrimaryColor

using Ledger.BitUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;


namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(ProgressBar))]
    [DefaultEvent(nameof(CheckedChanged))]
    public class CuiSwitch : UserControl
    {
        // Decompiled fields (renamed)
        private double _elapsedMs;
        private const int AnimationDurationMs = 350;

        private double _xDistance = 2.0;     // used by animation
        private float _startX;               // animation start thumbX
        private bool _animationFinished = true;
        private int _animationsInQueue;

        private bool _checked;

        private Color _checkedBackground = Color.FromArgb(64, Theme.PrimaryColor);
        private Color _uncheckedBackground = Color.FromArgb(32, 128, 128, 128);

        private Color _checkedForeground = Color.Empty;
        private Color _uncheckedForeground = Color.Empty;

        private Color _uncheckedOutlineColor = Color.Empty;
        private Color _checkedOutlineColor = Color.Empty;

        private Color _uncheckedSymbolColor = Color.Gray;
        private Color _checkedSymbolColor = Theme.PrimaryColor;

        private float _outlineThickness = 1f;

        private int _thumbX = 2;
        private bool _animating;
        private float _targetX = 2f;

        private RectangleF _thumbRect;

        private bool _showSymbols = true;
        private Size _thumbSizeModifier = new Size(0, 0);

        private CancellationTokenSource _animCts;

        private IContainer components;

        public CuiSwitch()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            Size = new Size(48, 24);
            Cursor = Cursors.Hand;

            if (DesignMode)
                _checkedBackground = BackColor;

            ForeColor = Color.FromArgb(171, 171, 171);
        }

        public event EventHandler CheckedChanged;

        [Description("Whether the switch is on or off.")]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (!_animating)
                {
                    _checked = value;
                    CheckedChanged?.Invoke(this, EventArgs.Empty);

                    _ = AnimateThumbLocationAsync();
                }

                Invalidate();
            }
        }

        [Description("The rounded background for the CHECKED switch.")]
        public Color CheckedBackground
        {
            get => _checkedBackground;
            set { _checkedBackground = value; Invalidate(); }
        }

        [Description("The rounded background for the UNCHECKED switch.")]
        public Color UncheckedBackground
        {
            get => _uncheckedBackground;
            set { _uncheckedBackground = value; Invalidate(); }
        }

        [Description("The checked foreground (thumb fill).")]
        public Color CheckedForeground
        {
            get => _checkedForeground;
            set { _checkedForeground = value; Invalidate(); }
        }

        [Description("The unchecked foreground (thumb fill).")]
        public Color UncheckedForeground
        {
            get => _uncheckedForeground;
            set { _uncheckedForeground = value; Invalidate(); }
        }

        [Description("The color of the outline (unchecked).")]
        public Color UncheckedOutlineColor
        {
            get => _uncheckedOutlineColor;
            set { _uncheckedOutlineColor = value; Invalidate(); }
        }

        [Description("The color of the outline (checked).")]
        public Color CheckedOutlineColor
        {
            get => _checkedOutlineColor;
            set { _checkedOutlineColor = value; Invalidate(); }
        }

        [Description("The symbol color when unchecked.")]
        public Color UncheckedSymbolColor
        {
            get => _uncheckedSymbolColor;
            set { _uncheckedSymbolColor = value; Invalidate(); }
        }

        [Description("The symbol color when checked.")]
        public Color CheckedSymbolColor
        {
            get => _checkedSymbolColor;
            set { _checkedSymbolColor = value; Invalidate(); }
        }

        [Description("The thickness of the outline.")]
        public float OutlineThickness
        {
            get => _outlineThickness;
            set { _outlineThickness = value; Invalidate(); }
        }

        public bool ShowSymbols
        {
            get => _showSymbols;
            set { _showSymbols = value; Invalidate(); }
        }

        public Size ThumbSizeModifier
        {
            get => _thumbSizeModifier;
            set { _thumbSizeModifier = value; Refresh(); }
        }

        /// <summary>
        /// Replaces the compiler-generated async state machine with a readable implementation.
        /// The original binary animates thumbX over ~350ms.
        /// </summary>
        public async Task AnimateThumbLocationAsync()
        {
            // Queue behavior (there were fields for it in the decompile).
            _animationsInQueue++;

            // Cancel any existing animation and start fresh (practical reconstruction).
            _animCts?.Cancel();
            _animCts?.Dispose();
            _animCts = new CancellationTokenSource();
            CancellationToken ct = _animCts.Token;

            try
            {
                _animating = true;
                _animationFinished = false;

                // Compute start/target positions consistent with OnPaint logic.
                // Left position (unchecked):
                //   Height/2 - thumbSize/2 + OutlineThickness/2 - 2
                // Right position (checked):
                //   Width - 3.5 - (Height - 7) - OutlineThickness/2
                float leftX = ComputeLeftThumbX();
                float rightX = ComputeRightThumbX();

                _startX = _thumbX;
                _targetX = Checked ? rightX : leftX;

                _xDistance = Math.Abs(_targetX - _startX);
                _elapsedMs = 0;

                const int frameMs = 15; // ~66fps, close enough for WinForms
                var sw = System.Diagnostics.Stopwatch.StartNew();

                while (_elapsedMs < AnimationDurationMs)
                {
                    ct.ThrowIfCancellationRequested();

                    _elapsedMs = sw.Elapsed.TotalMilliseconds;
                    double t = Math.Min(1.0, _elapsedMs / AnimationDurationMs);

                    // Simple ease-out (not guaranteed identical, but matches intent).
                    // t' = 1 - (1 - t)^2
                    double eased = 1.0 - (1.0 - t) * (1.0 - t);

                    float x = (float)(_startX + (_targetX - _startX) * eased);
                    _thumbX = (int)Math.Round(x);

                    Invalidate();
                    await Task.Delay(frameMs, ct).ConfigureAwait(true);
                }

                _thumbX = (int)Math.Round(_targetX);
                Invalidate();
            }
            catch (OperationCanceledException)
            {
                // swallow (replacement for "queue"/emergency behavior)
            }
            finally
            {
                _animationsInQueue = Math.Max(0, _animationsInQueue - 1);
                _animating = false;
                _animationFinished = true;
            }
        }

        /// <summary>
        /// Decompiled had an async-void EmergencySetLocation(int duration).
        /// Here it simply jumps quickly to the target (or animates with a shorter duration).
        /// </summary>
        private async void EmergencySetLocation(int durationMs)
        {
            int saved = AnimationDurationMs;
            // Minimal faithful behavior: just snap if duration is <= 0
            if (durationMs <= 0)
            {
                _thumbX = (int)Math.Round(Checked ? ComputeRightThumbX() : ComputeLeftThumbX());
                Invalidate();
                return;
            }

            // Quick mini-animation.
            float from = _thumbX;
            float to = Checked ? ComputeRightThumbX() : ComputeLeftThumbX();

            _animating = true;
            var sw = System.Diagnostics.Stopwatch.StartNew();
            while (sw.Elapsed.TotalMilliseconds < durationMs)
            {
                double t = Math.Min(1.0, sw.Elapsed.TotalMilliseconds / durationMs);
                float x = from + (to - from) * (float)t;
                _thumbX = (int)Math.Round(x);
                Invalidate();
                await Task.Delay(15).ConfigureAwait(true);
            }

            _thumbX = (int)Math.Round(to);
            _animating = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Decompiled: if not animating, recompute thumbX directly from Checked.
            if (!_animating)
            {
                _thumbX = !Checked
                    ? (int)(Height / 2.0 - ThumbRectangleInt.Height / 2.0 + OutlineThickness / 2.0 - 2.0)
                    : (int)(Width - 3.5 - (Height - 7) - OutlineThickness / 2.0);
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int halfHMinus1;
            try { halfHMinus1 = Height / 2 - 1; }
            catch { halfHMinus1 = 1; }

            Rectangle trackRect = ClientRectangle;
            trackRect.Inflate(-1, -1);
            trackRect.Inflate(-(int)OutlineThickness, -(int)OutlineThickness);

            int radius = halfHMinus1 - (int)OutlineThickness;

            using (GraphicsPath trackPath = BitMapClass.RoundRect(trackRect, radius))
            {
                using (var trackBrush = new SolidBrush(Checked ? CheckedBackground : UncheckedBackground))
                    e.Graphics.FillPath(trackBrush, trackPath);

                int thumbSize = Height - 7;
                _thumbRect = new RectangleF(_thumbX, 3f, thumbSize, thumbSize);

                _thumbRect.Offset(0.5f, 0.5f);
                _thumbRect.Inflate(-(int)OutlineThickness, -(int)OutlineThickness);
                _thumbRect.Inflate((SizeF)ThumbSizeModifier);

                Rectangle symbolRect = ThumbRectangleInt;
                symbolRect.Offset(1, 0);
                symbolRect.Height = symbolRect.Width;

                using (var thumbBrush = new SolidBrush(Checked ? CheckedForeground : UncheckedForeground))
                    e.Graphics.FillEllipse(thumbBrush, _thumbRect);

                using (var outlinePen = new Pen(Checked ? CheckedOutlineColor : UncheckedOutlineColor, OutlineThickness))
                    e.Graphics.DrawPath(outlinePen, trackPath);

                using (var symbolPen = new Pen(UncheckedSymbolColor, Height / 10f))
                {
                    symbolPen.StartCap = LineCap.Round;
                    symbolPen.EndCap = LineCap.Round;

                    if (ShowSymbols)
                    {
                        if (Checked)
                        {
                            symbolPen.Color = CheckedSymbolColor;
                            symbolRect.Offset(0, 1);
                            e.Graphics.DrawPath(symbolPen, BitMapClass.RoundRect(symbolRect, 1));
                        }
                        else
                        {
                            int inflate = (int)(Height / 6.1999998092651367);
                            symbolRect.Inflate(-inflate, -inflate);
                            e.Graphics.DrawPath(symbolPen, BitMapClass.RoundRect(symbolRect, 1));
                        }
                    }
                }
            }

            base.OnPaint(e);
        }

        private Rectangle ThumbRectangleInt =>
            new Rectangle((int)_thumbRect.X, (int)_thumbRect.Y, (int)_thumbRect.Width, (int)_thumbRect.Height);

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            _thumbX = 3;
            if (Width <= 0)
                return;

            _thumbX = Math.Min(Width - 5 - (Height - 7), 3);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (_animating)
                return;

            Checked = !Checked;
        }

        private void cuiSwitch_Load(object sender, EventArgs e) => Invalidate();

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
        }

        private float ComputeLeftThumbX()
        {
            // Uses current thumb size (Height-7) approximated; symbol rect is derived from thumb rect anyway.
            int thumbSize = Height - 7;
            return (float)(Height / 2.0 - thumbSize / 2.0 + OutlineThickness / 2.0 - 2.0);
        }

        private float ComputeRightThumbX()
        {
            return (float)(Width - 3.5 - (Height - 7) - OutlineThickness / 2.0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _animCts?.Cancel();
                _animCts?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.None;
            Name = "cuiSwitch";
            Size = new Size(50, 24);
            Load += new EventHandler(cuiSwitch_Load);
            ResumeLayout(false);
        }
    }
}
