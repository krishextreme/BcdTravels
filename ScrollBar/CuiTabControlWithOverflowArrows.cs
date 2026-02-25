using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Windows.Forms.TabControl;
using System.Linq;

namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(TabControl))]
    public class CuiTabControlWithOverflowArrows : UserControl
    {
        private readonly TabControl _tabControl = new TabControl();
        private readonly Panel _overflowPanel = new Panel();

        private Color _overflowArrowsColor = Color.FromArgb(180, 180, 180);

        private Rectangle _leftArrowHitRect = Rectangle.Empty;
        private Rectangle _rightArrowHitRect = Rectangle.Empty;

        private IContainer components;

        public CuiTabControlWithOverflowArrows()
        {
            InitializeComponent();

            // Basic flat look setup
            _tabControl.Appearance = TabAppearance.FlatButtons;
            _tabControl.ItemSize = new Size(110, 34);
            _tabControl.SizeMode = TabSizeMode.Fixed;
            _tabControl.Padding = new Point(12, 6);
            _tabControl.Margin = Padding.Empty;

            Controls.Add(_tabControl);
            Controls.Add(_overflowPanel);

            // Events
            //_tabControl.ControlAdded += (_, _) => UpdateOverflowState();
            //_tabControl.ControlRemoved += (_, _) => UpdateOverflowState();
            //_tabControl.SelectedIndexChanged += (_, _) => UpdateOverflowState();
            //_tabControl.SizeChanged += (_, _) => UpdateLayout();
            _tabControl.ControlAdded += (object sender, ControlEventArgs e) => UpdateOverflowState();
            _tabControl.ControlRemoved += (object sender, ControlEventArgs e) => UpdateOverflowState();
            _tabControl.SelectedIndexChanged += (object sender, EventArgs e) => UpdateOverflowState();
            _tabControl.SizeChanged += (object sender, EventArgs e) => UpdateLayout();


            if (!DesignMode)
            {
                _overflowPanel.Paint += OverflowPanel_Paint;
                _overflowPanel.MouseClick += OverflowPanel_MouseClick;
                _overflowPanel.MouseMove += (_, e) => _overflowPanel.Cursor =
                    (_leftArrowHitRect.Contains(e.Location) || _rightArrowHitRect.Contains(e.Location))
                        ? Cursors.Hand : Cursors.Default;
            }

            UpdateLayout();
            UpdateOverflowState();
        }

        // ---------------- Proxy Properties ----------------

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TabPageCollection TabPages => _tabControl.TabPages;

        public int SelectedIndex
        {
            get => _tabControl.SelectedIndex;
            set => _tabControl.SelectedIndex = value;
        }

        public TabPage SelectedTab
        {
            get => _tabControl.SelectedTab;
            set => _tabControl.SelectedTab = value;
        }

        public Size ItemSize
        {
            get => _tabControl.ItemSize;
            set => _tabControl.ItemSize = value;
        }

        public Color BackColor
        {
            get => _tabControl.BackColor;
            set => _tabControl.BackColor = value;
        }

        // Add your other color properties here (UnselectedTabBackColor, etc.)
        // They won't do much with standard TabControl but allow easy swap later

        public Color OverflowArrowsColor
        {
            get => _overflowArrowsColor;
            set { _overflowArrowsColor = value; _overflowPanel.Invalidate(); }
        }

        // Simple AddTab overloads
        public void AddTab(string text = "New Tab")
        {
            var page = new TabPage(text);
            TabPages.Add(page);
        }

        // Optional: your unique name generator (can be useful)
        public string GetUniqueTabName(string prefix = "Tab")
        {
            int i = 1;
            while (true)
            {
                string name = $"{prefix}{i}";
                if (!TabPages.Cast<TabPage>().Any(p => p.Text == name || p.Name == name))
                    return name;
                i++;
            }
        }

        private bool IsOverfilled =>
            TabPages.Count * (_tabControl.ItemSize.Width + 8) > _tabControl.ClientSize.Width;

        private void UpdateOverflowState()
        {
            _overflowPanel.Visible = IsOverfilled && TabPages.Count > 1;
            if (_overflowPanel.Visible)
                _overflowPanel.BringToFront();
            else
                _tabControl.BringToFront();

            _overflowPanel.Invalidate();
        }

        private void UpdateLayout()
        {
            SuspendLayout();

            _tabControl.Dock = DockStyle.Fill;

            _overflowPanel.Width = 52;
            _overflowPanel.Height = _tabControl.ItemSize.Height + 8;
            _overflowPanel.Location = new Point(Width - _overflowPanel.Width - 1, 0);
            _overflowPanel.BackColor = _tabControl.BackColor;
            _overflowPanel.Cursor = Cursors.Default;

            ResumeLayout(false);
        }

        private void OverflowPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            if (_leftArrowHitRect.Contains(e.Location) && SelectedIndex > 0)
            {
                SelectedIndex--;
            }
            else if (_rightArrowHitRect.Contains(e.Location) && SelectedIndex < TabPages.Count - 1)
            {
                SelectedIndex++;
            }
        }

        private void OverflowPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = _overflowPanel.Width;
            int h = _overflowPanel.Height;

            _leftArrowHitRect = new Rectangle(0, 0, w / 2, h);
            _rightArrowHitRect = new Rectangle(w / 2, 0, w / 2, h);

            bool leftEnabled = SelectedIndex > 0;
            bool rightEnabled = SelectedIndex < TabPages.Count - 1;

             var pen = new Pen(leftEnabled ? _overflowArrowsColor : Color.Gray, 2.8f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };

            // Left arrow
            if (leftEnabled)
                g.DrawLines(pen, new[] { new Point(14, h / 2 - 9), new Point(26, h / 2), new Point(14, h / 2 + 9) });

            // Right arrow
             var penRight = new Pen(rightEnabled ? _overflowArrowsColor : Color.Gray, 2.8f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };

            if (rightEnabled)
                g.DrawLines(penRight, new[] { new Point(w - 14, h / 2 - 9), new Point(w - 26, h / 2), new Point(w - 14, h / 2 + 9) });
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateLayout();
            UpdateOverflowState();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            AutoScaleMode = AutoScaleMode.Font;
        }
    }
}