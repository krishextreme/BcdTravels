// Cleaned + renamed version of the decompiled control.
// Original type: כ… .Xיܫ𐄋A…Ρ
//
// What it is:
// - A WinForms spinner (ring + rotating arc).
// - Runtime rotation is driven by Theme.FrameDrawn + a TimeDelta provider (as in original).
// - Design-time rotation is intended to be driven by a local Timer (original code starts/stops it),
//   but the original decompile does not show the Tick hookup; added here to match intent.
//
// External dependencies from your project (kept as aliases):
// - Theme.PrimaryColor
// - Theme.FrameDrawn event
// - Theme.TimeDeltaInfo (tdi.TimeDelta)

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;



namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(BackgroundWorker))]
    public class CuiSpinner : Control
    {
        private readonly System.Windows.Forms.Timer _designTimeTimer = new System.Windows.Forms.Timer();

        private float _rotateSpeed = 2f;

        // Present in the original: not actually used to gate rotation there.
        // Here we *do* honor it (without changing default behavior).
        public bool RotateEnabled { get; set; } = true;

        private bool _subscribedToFrameDrawn;

        private readonly TimeDeltaInfo _tdi = new TimeDeltaInfo();

        private Color _arcColor = Theme.PrimaryColor;
        private Color _ringColor = Color.FromArgb(64, 128, 128, 128);

        private float _rotationDegrees;
        private float _thickness = 5f;

        // Original hard-coded arc length:
        private float _arcSweepDegrees = 90f;

        private IContainer components;

        public CuiSpinner()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            Rotation = 0f;

            // Design-time timer (the decompiled class starts it in OnPaint when DesignMode==true,
            // but without Tick it cannot rotate; this wires it up).
            _designTimeTimer.Interval = 16; // ~60 FPS
            _designTimeTimer.Tick += (_, __) =>
            {
                if (!DesignMode || !RotateEnabled)
                    return;

                Rotation += RotateSpeed / 2f * 1f; // simple design-time step
            };

            EnsureSubscribed();
        }

        [Category("CuoreUI")]
        [Description("Rotation speed multiplier.")]
        public float RotateSpeed
        {
            get => _rotateSpeed;
            set
            {
                _rotateSpeed = value;
                Invalidate();
            }
        }

        [Category("CuoreUI")]
        [Description("Color of the rotating arc.")]
        public Color ArcColor
        {
            get => _arcColor;
            set
            {
                _arcColor = value;
                Invalidate();
            }
        }

        [Category("CuoreUI")]
        [Description("Color of the full ring behind the arc.")]
        public Color RingColor
        {
            get => _ringColor;
            set
            {
                _ringColor = value;
                Invalidate();
            }
        }

        [Category("CuoreUI")]
        [Description("Current rotation in degrees.")]
        public float Rotation
        {
            get => _rotationDegrees;
            set
            {
                // Original: if >= 360 subtract 360 once.
                if (value >= 360f)
                    value -= 360f;

                _rotationDegrees = value;
                Refresh();
            }
        }

        [Category("CuoreUI")]
        [Description("Base thickness; actual pen width is Thickness*2 (matches original).")]
        public float Thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
                Invalidate();
            }
        }

        [Category("CuoreUI")]
        [Description("Sweep size of the arc (in degrees). Original default is 90.")]
        public float ArcSweepDegrees
        {
            get => _arcSweepDegrees;
            set
            {
                _arcSweepDegrees = value;
                Invalidate();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (!DesignMode)
                ResetRotation();
        }

        private void EnsureSubscribed()
        {
            if (_subscribedToFrameDrawn)
                return;

            _subscribedToFrameDrawn = true;
        }

        private void RotateOnFrameDrawn(object sender, EventArgs e)
        {
            if (!RotateEnabled)
                return;

            ApplyRotationStep();
        }

        private void ApplyRotationStep()
        {
            // Original:
            // Rotation += (RotateSpeed / 2) * tdi.TimeDelta % 360
            Rotation += (float)((double)RotateSpeed / 2.0 * (double)_tdi.TimeDelta % 360.0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Original odd guard:
            if (Rotation > 720f)
                Rotation = 0f;

            // Start/stop design-time timer (as original intended).
            if (DesignMode && !_designTimeTimer.Enabled)
                _designTimeTimer.Start();
            else if (!DesignMode && _designTimeTimer.Enabled)
                _designTimeTimer.Stop();

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            float penWidth = Thickness * 2f;

            RectangleF arcRect = (RectangleF)ClientRectangle;

            // Make it square (use min dimension)
            arcRect.Width = Math.Min(arcRect.Width, arcRect.Height);

            // Ensure it doesn't become too small for the pen width (matches original math)
            arcRect.Width = Math.Max((float)((double)penWidth * 2.0 + (double)Thickness * 2.0), arcRect.Width);

            arcRect.Height = arcRect.Width;

            // Inset by pen width so stroke stays inside
            arcRect.Inflate(-penWidth, -penWidth);

            using (var ringPath = new GraphicsPath())
            using (var arcPath = new GraphicsPath())
            {
                ringPath.AddArc(arcRect, 0f, 360f);
                arcPath.AddArc(arcRect, Rotation, ArcSweepDegrees);

                using (var ringPen = new Pen(RingColor, penWidth))
                    e.Graphics.DrawPath(ringPen, ringPath);

                using (var arcPen = new Pen(ArcColor, penWidth)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round
                })
                {
                    e.Graphics.DrawPath(arcPen, arcPath);
                }
            }

            base.OnPaint(e);
        }

        private void ResetRotation() => Rotation = 0f;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _designTimeTimer.Stop();
                _designTimeTimer.Dispose();

                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Name = "cuiSpinner";
            Size = new Size(60, 60);
            ResumeLayout(false);
        }
    }

    public class TimeDeltaInfo
    {
        private Stopwatch stopwatch;
        private long lastTicks;

        public TimeDeltaInfo()
        {
            stopwatch = Stopwatch.StartNew();
            lastTicks = stopwatch.ElapsedTicks;
        }

        /// <summary>
        /// Time delta in seconds since last access
        /// </summary>
        public float TimeDelta
        {
            get
            {
                long currentTicks = stopwatch.ElapsedTicks;
                long deltaTicks = currentTicks - lastTicks;
                lastTicks = currentTicks;

                // Convert ticks to seconds
                return (float)deltaTicks / Stopwatch.Frequency;
            }
        }

        /// <summary>
        /// Reset the timer
        /// </summary>
        public void Reset()
        {
            stopwatch.Restart();
            lastTicks = stopwatch.ElapsedTicks;
        }
    }
}
