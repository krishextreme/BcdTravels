using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ledger.Animations
{
    // Singleton manager for the tooltip window
    public static class TooltipManager
    {
        private static TooltipForm tooltipWindow;

        public static TooltipForm TooltipWindow
        {
            get
            {
                if (tooltipWindow == null || tooltipWindow.IsDisposed)
                {
                    tooltipWindow = new TooltipForm();
                }
                return tooltipWindow;
            }
        }

        public static void ShowTooltip(string text, Point location, Color foreColor, Color backColor)
        {
            TooltipWindow.ShowTooltip(text, location, foreColor, backColor);
        }

        public static void HideTooltip()
        {
            TooltipWindow?.HideTooltip();
        }
    }

    // The actual tooltip form
    public class TooltipForm : Form
    {
        private Label contentLabel;
        private System.Windows.Forms.Timer hideTimer;
        private const int TooltipPadding = 8;
        private const int TooltipRounding = 6;

        public TooltipForm()
        {
            InitializeTooltipForm();
        }

        private void InitializeTooltipForm()
        {
            // Form properties
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            TopMost = true;
            BackColor = Color.FromArgb(64, 64, 64);
            ForeColor = Color.White;

            // Make the form semi-transparent
            Opacity = 0.95;

            // Set up double buffering
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);

            // Content label
            contentLabel = new Label
            {
                AutoSize = true,
                Padding = new Padding(TooltipPadding),
                BackColor = Color.Transparent,
                ForeColor = this.ForeColor,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };

            Controls.Add(contentLabel);

            // Hide timer
            hideTimer = new System.Windows.Forms.Timer();
            hideTimer.Interval = 3000; // Hide after 3 seconds
            hideTimer.Tick += (s, e) =>
            {
                HideTooltip();
            };

            // Initially hidden
            Visible = false;
        }

        public void ShowTooltip(string text, Point location, Color foreColor, Color backColor)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            contentLabel.Text = text;
            contentLabel.ForeColor = foreColor;
            BackColor = backColor;

            // Calculate size based on content
            Size textSize = TextRenderer.MeasureText(text, contentLabel.Font);
            Size = new Size(
                textSize.Width + (TooltipPadding * 2),
                textSize.Height + (TooltipPadding * 2)
            );

            // Position the tooltip
            Location = AdjustLocationToScreen(location);

            // Create rounded region
            Region = CreateRoundedRegion(new Rectangle(0, 0, Width, Height), TooltipRounding);

            // Show and start hide timer
            Show();
            hideTimer.Stop();
            hideTimer.Start();
        }

        public void HideTooltip()
        {
            hideTimer.Stop();
            Hide();
        }

        private Point AdjustLocationToScreen(Point location)
        {
            // Offset tooltip from cursor
            Point adjustedLocation = new Point(location.X + 15, location.Y + 15);

            // Get screen bounds
            Screen screen = Screen.FromPoint(location);
            Rectangle screenBounds = screen.WorkingArea;

            // Adjust if tooltip goes off screen
            if (adjustedLocation.X + Width > screenBounds.Right)
                adjustedLocation.X = screenBounds.Right - Width;

            if (adjustedLocation.Y + Height > screenBounds.Bottom)
                adjustedLocation.Y = location.Y - Height - 5;

            if (adjustedLocation.X < screenBounds.Left)
                adjustedLocation.X = screenBounds.Left;

            if (adjustedLocation.Y < screenBounds.Top)
                adjustedLocation.Y = screenBounds.Top;

            return adjustedLocation;
        }

        private Region CreateRoundedRegion(Rectangle rect, int radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                int diameter = radius * 2;

                path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
                path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
                path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();

                return new Region(path);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw rounded background
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = CreateRoundedRectanglePath(ClientRectangle, TooltipRounding))
            {
                using (SolidBrush brush = new SolidBrush(BackColor))
                {
                    g.FillPath(brush, path);
                }

                // Optional: Draw border
                using (Pen pen = new Pen(Color.FromArgb(80, 80, 80), 1))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            rect.Width -= 1;
            rect.Height -= 1;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
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
                return cp;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                hideTimer?.Dispose();
                contentLabel?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    // The tooltip component
    public class TooltipComponent : Component
    {
        private Control targetControl;
        private string content = "Tooltip Text";
        private Color foreColor = Color.White;
        private Color backColor = Color.FromArgb(64, 64, 64);
        private IContainer components;

        private TooltipForm tooltipForm => TooltipManager.TooltipWindow;

        public TooltipComponent() => InitializeComponent();

        public TooltipComponent(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        public Control TargetControl
        {
            get => targetControl;
            set
            {
                // Unsubscribe from old control
                if (targetControl != null)
                {
                    targetControl.MouseHover -= MouseHover;
                    targetControl.MouseLeave -= MouseLeave;
                }

                targetControl = value;

                // Subscribe to new control
                if (targetControl != null)
                {
                    targetControl.MouseHover += MouseHover;
                    targetControl.MouseLeave += MouseLeave;
                }
            }
        }

        [Description("The text to display in the tooltip.")]
        public string Content
        {
            get => content;
            set => content = value;
        }

        [Description("The foreground color of the tooltip text.")]
        public Color ForeColor
        {
            get => foreColor;
            set => foreColor = value;
        }

        [Description("The background color of the tooltip.")]
        public Color BackColor
        {
            get => backColor;
            set => backColor = value;
        }

        private void MouseHover(object sender, EventArgs e)
        {
            TooltipStateMachine stateMachine = new TooltipStateMachine();
            stateMachine.Builder = AsyncVoidMethodBuilder.Create();
            stateMachine.Instance = this;
            stateMachine.State = -1;
            stateMachine.Builder.Start(ref stateMachine);
        }

        private void MouseLeave(object sender, EventArgs e)
        {
            TooltipManager.HideTooltip();
        }

        // The actual tooltip display logic
        private async void ShowTooltipAsync()
        {
            if (targetControl == null || targetControl.IsDisposed)
                return;

            // Small delay to avoid showing tooltip on quick mouse movements
            await Task.Delay(500);

            // Check if mouse is still over the control
            if (targetControl == null || targetControl.IsDisposed)
                return;

            Point cursorPosition = Cursor.Position;
            Point controlPosition = targetControl.PointToClient(cursorPosition);

            // Only show if cursor is still within control bounds
            if (targetControl.ClientRectangle.Contains(controlPosition))
            {
                TooltipManager.ShowTooltip(content, cursorPosition, foreColor, backColor);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();

                // Unsubscribe from events
                if (targetControl != null)
                {
                    targetControl.MouseHover -= MouseHover;
                    targetControl.MouseLeave -= MouseLeave;
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() => components = new Container();

        // State machine for async tooltip display
        private struct TooltipStateMachine : IAsyncStateMachine
        {
            public int State;
            public AsyncVoidMethodBuilder Builder;
            public TooltipComponent Instance;

            public void MoveNext()
            {
                try
                {
                    if (State == -1)
                    {
                        Instance.ShowTooltipAsync();
                    }
                }
                catch (Exception ex)
                {
                    Builder.SetException(ex);
                    return;
                }

                Builder.SetResult();
            }

            public void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                Builder.SetStateMachine(stateMachine);
            }
        }
    }
}