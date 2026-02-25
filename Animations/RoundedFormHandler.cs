using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using Ledger.RoundedFormOverLlay;
using Ledger.BitUI;
namespace Ledger.Animations
{
    // The overlay form that draws the rounded border
    public class RoundedFormObj : Form
    {
        private Color backgroundColor;
        private Color outlineColor;
        private int rounding;
        private Bitmap cachedBitmap;
        public bool InvalidateNextDrawCall;
        public bool initialized;
        public Form TargetForm { get; set; }

        public int Rounding
        {
            get => rounding;
            set
            {
                rounding = value;
                Invalidate();
            }
        }

        public RoundedFormObj(Color backColor, Color outline, ref int roundingValue)
        {
            backgroundColor = backColor;
            outlineColor = outline;
            rounding = roundingValue;

            // Form configuration
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.Magenta;
            TransparencyKey = Color.Magenta;
            TopMost = true;
            Opacity = 1.0;

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            initialized = true;
        }

        public void UpdateBitmap(Bitmap bitmap = null)
        {
            cachedBitmap?.Dispose();
            cachedBitmap = bitmap;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (TargetForm == null || TargetForm.IsDisposed)
                return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Magenta);

            // Draw the cached bitmap if available (for enhanced corners)
            if (cachedBitmap != null)
            {
                g.DrawImage(cachedBitmap, 0, 0);
            }

            // Draw the outline
            if (outlineColor.A > 0)
            {
                using (GraphicsPath path = CreateRoundedRectanglePath(
                    new Rectangle(0, 0, Width - 1, Height - 1), rounding))
                {
                    using (Pen pen = new Pen(outlineColor, 1f))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }

            // Apply opacity from target form
            if (Tag is double opacity)
            {
                Opacity = opacity;
            }

            InvalidateNextDrawCall = false;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Don't paint background to avoid flicker
        }

        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            // Top-left corner
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            // Top-right corner
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            // Bottom-right corner
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            // Bottom-left corner
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);

            path.CloseFigure();
            return path;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80000; // WS_EX_LAYERED
                cp.ExStyle |= 0x20; // WS_EX_TRANSPARENT
                return cp;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                cachedBitmap?.Dispose();
            }
            base.Dispose(disposing);
        }
        public Color OutlineColor
        {
            get => this.outlineColor; set
            {
                outlineColor = value;
            }

        }
    }


  // was \uFFFD\uFFFDᚌᛃZ\uFFFDܡ\uFFFD\uFFFD\uFFFD unicode
  public class CuiFormRounder : Component
    {
        // was \uFFFDתܩᛋܒΨΙWסᚾ\uFFFDܕ\uFFFDΘ unicode
        public RoundedFormOverlay roundedFormObj;

        private Form privateTargetForm;
        private bool targetFormActivating;
        private bool shouldCloseDown;
        private bool wasFormClosingCalled;
        private int privateRounding = 8;
        private Color privateOutlineColor = Color.FromArgb(32 /*0x20*/, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
        private const uint SWP_NOSIZE = 1;
        private const uint SWP_NOMOVE = 2;
        private const uint SWP_NOACTIVATE = 16 /*0x10*/;
        private static readonly IntPtr HWND_TOP = new IntPtr(0);
        private bool privateEnhanceCorners;
        private FormWindowState lastState;
        private IContainer components;

        public Form TargetForm
        {
            get => this.privateTargetForm;
            set
            {
                if (value == null && this.privateTargetForm != null && !this.privateTargetForm.Disposing)
                    this.privateTargetForm.Region = (Region)null;
                this.privateTargetForm = value;
                if (value == null)
                {
                    this.roundedFormObj?.UpdBitmap();
                    this.roundedFormObj?.Hide();
                }
                else
                {
                    this.TargetForm.Load += new EventHandler(this.TargetForm_Load);
                    this.TargetForm.Resize += new EventHandler(this.TargetForm_Resize);
                    this.TargetForm.LocationChanged += new EventHandler(this.TargetForm_LocationChanged);
                    this.TargetForm.FormClosing += new FormClosingEventHandler(this.TargetForm_FormClosing);
                    this.TargetForm.VisibleChanged += new EventHandler(this.TargetForm_VisibleChanged);
                    this.TargetForm.BackColorChanged += new EventHandler(this.TargetForm_BackColorChanged);
                    this.TargetForm.Activated += new EventHandler(this.TargetForm_Activated);
                    this.TargetForm.HandleCreated += new EventHandler(this.TargetForm_HandleCreated);
                    this.TargetForm.ResizeEnd += (EventHandler)((e, s) => this.UpdateExperimentalBitmap());
                    if (this.roundedFormObj == null || this.roundedFormObj.IsDisposed || this.DesignMode)
                        return;
                    this.UpdateRoundedFormRegion();
                    this.TargetForm_LocationChanged((object)this, EventArgs.Empty);
                    this.TargetForm_Resize((object)this, EventArgs.Empty);
                    if (this.EnhanceCorners)
                        this.UpdateExperimentalBitmap();
                    this.roundedFormObj?.Show();
                }
            }
        }

        private void TargetForm_HandleCreated(object sender, EventArgs e)
        {
            if (this.DesignMode || this.TargetForm == null)
                return;
            int pvAttribute = 1;
            CuiFormRounder.DwmSetWindowAttribute(this.TargetForm.Handle, -3, ref pvAttribute, 4);
        }

        private void TargetForm_Activated(object sender, EventArgs e)
        {
            if (this.shouldCloseDown || this.targetFormActivating)
                return;
            this.targetFormActivating = true;
            this.FakeForm_Activated(sender, e);
        }

        private void TargetForm_BackColorChanged(object sender, EventArgs e)
        {
            int num = this.shouldCloseDown ? 1 : 0;
        }

        private void TargetForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.shouldCloseDown || this.DesignMode || this.roundedFormObj == null || this.wasFormClosingCalled || this.roundedFormObj.IsDisposed)
                return;
            this.roundedFormObj.Visible = this.TargetForm.Visible;
            this.roundedFormObj.Tag = (object)this.TargetForm.Opacity;
            this.roundedFormObj.InvalidateNextDrawCall = true;
        }

        private void TargetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.shouldCloseDown = true;
            if (this.wasFormClosingCalled || this.TargetForm == null)
                return;
            this.wasFormClosingCalled = true;
            this.TargetForm.Load -= new EventHandler(this.TargetForm_Load);
            this.TargetForm.Resize -= new EventHandler(this.TargetForm_Resize);
            this.TargetForm.LocationChanged -= new EventHandler(this.TargetForm_LocationChanged);
            this.TargetForm.FormClosing -= new FormClosingEventHandler(this.TargetForm_FormClosing);
            this.TargetForm.VisibleChanged -= new EventHandler(this.TargetForm_VisibleChanged);
            this.TargetForm.BackColorChanged -= new EventHandler(this.TargetForm_BackColorChanged);
            this.TargetForm.Controls.Clear();
            this.TryCloseForm(this.TargetForm);
            this.TryCloseForm((Form)this.roundedFormObj);
        }

        private void TryCloseForm(Form f)
        {
            if (f == null || f.IsDisposed || !f.IsHandleCreated)
                return;
            BitMapClass.Native.SendMessage(f.Handle, 16 /*0x10*/, IntPtr.Zero, IntPtr.Zero);
        }

        public void FakeForm_Activated(object sender, EventArgs e)
        {
            if (this.DesignMode || this.shouldCloseDown || this.wasFormClosingCalled || this.DesignMode || this.TargetForm == null || this.TargetForm.IsDisposed)
                return;
            if (this.roundedFormObj != null && !this.roundedFormObj.IsDisposed)
            {
                this.roundedFormObj.Tag = (object)this.TargetForm.Opacity;
                this.roundedFormObj.InvalidateNextDrawCall = true;
                if (this.roundedFormObj.WindowState != FormWindowState.Minimized && !this.wasFormClosingCalled && !this.shouldCloseDown)
                {
                    if (this.TargetForm.WindowState != FormWindowState.Minimized)
                    {
                        CuiFormRounder.SetWindowPos(this.roundedFormObj.Handle, CuiFormRounder.HWND_TOP, 0, 0, 0, 0, 19U);
                        CuiFormRounder.SetWindowPos(this.TargetForm.Handle, CuiFormRounder.HWND_TOP, 0, 0, 0, 0, 19U);
                    }
                    this.TargetForm.BringToFront();
                }
            }
            this.targetFormActivating = false;
        }

        private static Point PointSubtract(Point p1, Point p2) => new Point(p1.X - p2.X, p1.Y - p2.Y);

        public int Rounding
        {
            get => this.privateRounding;
            set
            {
                this.privateRounding = value;
                if (this.shouldCloseDown || this.roundedFormObj == null || this.roundedFormObj.IsDisposed)
                    return;
                this.roundedFormObj.Rounding = value;
                if (this.TargetForm == null)
                    return;
                this.UpdateTargetFormRegion();
                this.UpdateRoundedFormRegion();
                this.roundedFormObj?.Invalidate();
            }
        }

        private void TargetForm_LocationChanged(object sender, EventArgs e)
        {
            //// ISSUE: variable of a compiler-generated type
            //CuiFormRounder.\uFFFDܒᚎΓ\uFFFDΟ\uFFFD\uD800\uDD04\uFFFDᛇᚠ\uFFFD stateMachine;
            //// ISSUE: reference to a compiler-generated field
            //stateMachine.\u003C\u003Et__builder = AsyncVoidMethodBuilder.Create();
            //// ISSUE: reference to a compiler-generated field
            //stateMachine.\u003C\u003E4__this = this;
            //// ISSUE: reference to a compiler-generated field
            //stateMachine.\u003C\u003E1__state = -1;
            //// ISSUE: reference to a compiler-generated field
            //stateMachine.\u003C\u003Et__builder.Start < CuiFormRounder.\uFFFDܒᚎΓ\uFFFDΟ\uFFFD\uD800\uDD04\uFFFDᛇᚠ\uFFFD> (ref stateMachine);
        }

        public Color OutlineColor
        {
            get => this.privateOutlineColor;
            set
            {
                this.privateOutlineColor = value;
                if (this.shouldCloseDown || this.roundedFormObj == null || this.roundedFormObj.IsDisposed)
                    return;
                this.roundedFormObj?.Invalidate();
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(
          IntPtr hWnd,
          IntPtr hWndInsertAfter,
          int X,
          int Y,
          int cx,
          int cy,
          uint uFlags);

        private void UpdateTargetFormRegion()
        {
            this.TargetForm.Region = Region.FromHrgn(CuiFormRounder.CreateRoundRectRgn(0, 0, this.TargetForm.Width, this.TargetForm.Height, (int)((double)this.Rounding * 2.0), (int)((double)this.Rounding * 2.0)));
        }

        public void UpdateRoundedFormRegion()
        {
            if (this.DesignMode)
            {
                Form targetForm = this.TargetForm;
                if ((targetForm != null ? (targetForm.Opacity != 1.0 ? 1 : 0) : 1) == 0)
                {
                    this.roundedFormObj.Region = new Region(this.roundedFormObj.ClientRectangle);
                    return;
                }
            }
            if (this.roundedFormObj == null || this.roundedFormObj == null || this.roundedFormObj.IsDisposed)
                return;
            Region region1 = new Region(this.roundedFormObj.ClientRectangle);
            Region region2 = this.TargetForm.Region.Clone();
            region2.Translate(1, 1);
            region1.Exclude(region2);
            this.roundedFormObj.Region = region1;
        }

        [DllImport("Gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(
          int nLeftRect,
          int nTopRect,
          int nRightRect,
          int nBottomRect,
          int nWidthEllipse,
          int nHeightEllipse);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(
          IntPtr hwnd,
          int dwAttribute,
          ref int pvAttribute,
          int cbAttribute);

        private void TargetForm_Load(object sender, EventArgs e)
        {
            this.FakeForm_Activated(sender, e);
            this.TargetForm.FormBorderStyle = FormBorderStyle.None;

            // was \uFFFDתܩᛋܒΨΙWסᚾ\uFFFDܕ\uFFFDΘ(...) unicode
            this.roundedFormObj = new RoundedFormOverlay(this.TargetForm.BackColor, this.OutlineColor, ref this.privateRounding);

            this.roundedFormObj.TargetForm = this.TargetForm;
            this.roundedFormObj.Tag = (object)this.TargetForm.Opacity;
            this.TargetForm.Region = Region.FromHrgn(CuiFormRounder.CreateRoundRectRgn(0, 0, this.TargetForm.Width, this.TargetForm.Height, (int)((double)this.Rounding * 2.0), (int)((double)this.Rounding * 2.0)));
            this.roundedFormObj?.Show();
            this.roundedFormObj.Activated += new EventHandler(this.FakeForm_Activated);
            this.TargetForm_Resize((object)this, EventArgs.Empty);
            this.TargetForm_LocationChanged((object)this, EventArgs.Empty);
            this.TargetForm.FormBorderStyle = FormBorderStyle.None;
            if (this.TargetForm.ShowInTaskbar)
            {
                this.TargetForm.BringToFront();
                this.TargetForm.Activate();
                this.TargetForm.Focus();
                this.TargetForm.Location = this.TargetForm.Location;
                if (!this.DesignMode && this.TargetForm != null)
                {
                    int pvAttribute = 0;
                    CuiFormRounder.DwmSetWindowAttribute(this.TargetForm.Handle, -3, ref pvAttribute, 4);
                }
            }
            UiAnimationGlobals.TenFramesDrawn += (EventHandler)((_, __) =>
            {
                if (this.roundedFormObj != null && !this.shouldCloseDown)
                {
                    if (!this.roundedFormObj.IsDisposed)
                    {
                        try
                        {
                            this.roundedFormObj.Tag = (object)this.TargetForm.Opacity;
                            this.roundedFormObj.InvalidateNextDrawCall = true;
                        }
                        catch
                        {
                        }
                        if (this.TargetForm == null || this.TargetForm.IsDisposed || this.TargetForm.TopMost == this.roundedFormObj.TopMost)
                            return;
                        this.roundedFormObj.TopMost = this.TargetForm.TopMost;
                        return;
                    }
                }
                if (!this.roundedFormObj.initialized)
                    return;
                this.Dispose();
            });
        }

        [Description("EXPERIMENTAL! Uses a bitmap approach to smoothen out the insides of the form, so that there isn't a 1px border the color of TargetForm.BackColor around the TargetForm")]
        public bool EnhanceCorners
        {
            get => this.privateEnhanceCorners;
            set
            {
                this.privateEnhanceCorners = value;
                this.UpdateExperimentalBitmap();
            }
        }

        private Bitmap experimentalBitmap { get; set; }

        private void UpdateExperimentalBitmap()
        {
            if (this.DesignMode || this.TargetForm == null || this.TargetForm.IsDisposed || this.shouldCloseDown || this.roundedFormObj != null || this.roundedFormObj.IsDisposed || !this.roundedFormObj.initialized)
                return;
            if (!this.EnhanceCorners || this.Rounding == 0)
            {
                this.experimentalBitmap?.Dispose();
                this.roundedFormObj?.UpdBitmap();
            }
            else
            {
                Bitmap bitmap = new Bitmap(Math.Max(this.TargetForm.Width + 1, 1), Math.Max(this.TargetForm.Height + 1, 1));
                using (Graphics graphics = Graphics.FromImage((Image)bitmap))
                {
                    graphics.Clear(Color.Transparent);
                    this.TargetForm?.DrawToBitmap(bitmap, new Rectangle(0, 0, this.TargetForm.Width, this.TargetForm.Height));
                }
                this.experimentalBitmap?.Dispose();
                this.experimentalBitmap = bitmap;
                this.roundedFormObj?.UpdBitmap(bitmap);
            }
        }

        private void TargetForm_Resize(object sender, EventArgs e)
        {
            if (this.DesignMode || this.TargetForm == null || this.TargetForm.IsDisposed || this.shouldCloseDown || this.roundedFormObj == null || this.roundedFormObj.IsDisposed || !this.roundedFormObj.initialized)
                return;
            if (this.TargetForm.WindowState != this.lastState)
            {
                this.lastState = this.TargetForm.WindowState;
                this.roundedFormObj.WindowState = this.TargetForm.WindowState;
            }
            if (this.TargetForm.WindowState != FormWindowState.Minimized)
            {
                this.roundedFormObj.Size = Size.Add(this.TargetForm.Size, new Size(2, 2));
                this.UpdateRoundedFormRegion();
                this.UpdateExperimentalBitmap();
                this.roundedFormObj.InvalidateNextDrawCall = true;
                if (this.TargetForm.WindowState == FormWindowState.Normal)
                    this.TargetForm.Region = Region.FromHrgn(CuiFormRounder.CreateRoundRectRgn(0, 0, this.TargetForm.Width, this.TargetForm.Height, (int)((double)this.Rounding * 2.0), (int)((double)this.Rounding * 2.0)));
                else
                    this.TargetForm.Region = (Region)null;
            }
            this.targetFormActivating = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() => this.components = (IContainer)new System.ComponentModel.Container();
    }

   
    }

