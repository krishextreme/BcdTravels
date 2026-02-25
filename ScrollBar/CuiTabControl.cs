using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;

using Ledger.BitUI;
namespace Ledger.ScrollBar
{
    // Custom tab page class
    public class CustomTabPage : TabPage
    {
        public CustomTabPage()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Custom painting logic if needed
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
        }
    }

    [ToolboxBitmap(typeof(TabControl))]
    public class CuiTabControl : TabControl
    {
        private Color _backgroundColor = Color.Empty;
        private int _rounding = 8;

        private Color _unselectedTabBackColor = Color.FromArgb(32, 128, 128, 128);
        private Color _selectedTabBackColor = Theme.PrimaryColor;
        private Color _hoveredTabBackColor = Color.FromArgb(64, 128, 128, 128);

        private Color _unselectedTabTextColor = Color.Gray;
        private Color _selectedTabTextColor = Color.White;
        private Color _hoveredTabTextColor = Color.FromArgb(64, 64, 64);

        private Color _deletionTabBackColor = Color.Crimson;
        private Color _deletionIconColor = Color.White;

        private Color _addButtonBackColor = Color.FromArgb(128, 0, 0, 0);
        private Color _addButtonColor = Color.White;

        private bool _showAddTabButton = true;

        private int _hoveredTabIndex = -1;
        private Rectangle _addTabButtonRect = Rectangle.Empty;

        /// <summary>
        /// If >= 0, the tab at this index is in "armed for deletion" mode (shows an X).
        /// </summary>
        public int TabSelectedToDeletion { get; set; } = -1;

        private IContainer components;

        public CuiTabControl()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            Appearance = TabAppearance.Buttons;
            DrawMode = TabDrawMode.OwnerDrawFixed;
            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(126, 42);

            ControlAdded += TabsChanged;
            ControlRemoved += TabsChanged;
        }

        private void TabsChanged(object sender, ControlEventArgs e) => Invalidate();

        public bool AllowNoTabs { get; set; }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set { _backgroundColor = value; Invalidate(); }
        }

        public int Rounding
        {
            get => _rounding;
            set { _rounding = value; Invalidate(); }
        }

        public Color UnselectedTabBackColor
        {
            get => _unselectedTabBackColor;
            set { _unselectedTabBackColor = value; Invalidate(); }
        }

        public Color SelectedTabBackColor
        {
            get => _selectedTabBackColor;
            set { _selectedTabBackColor = value; Invalidate(); }
        }

        public Color HoveredTabBackColor
        {
            get => _hoveredTabBackColor;
            set { _hoveredTabBackColor = value; Invalidate(); }
        }

        public Color UnselectedTabTextColor
        {
            get => _unselectedTabTextColor;
            set { _unselectedTabTextColor = value; Invalidate(); }
        }

        public Color SelectedTabTextColor
        {
            get => _selectedTabTextColor;
            set
            {
                _selectedTabTextColor = value;
                Invalidate();
            }
        }

        public Color HoveredTabTextColor
        {
            get => _hoveredTabTextColor;
            set { _hoveredTabTextColor = value; Invalidate(); }
        }

        public Color DeletionTabBackgroundColor
        {
            get => _deletionTabBackColor;
            set { _deletionTabBackColor = value; Invalidate(); }
        }

        public Color DeletionColor
        {
            get => _deletionIconColor;
            set { _deletionIconColor = value; Invalidate(); }
        }

        public Color AddButtonBackgroundColor
        {
            get => _addButtonBackColor;
            set { _addButtonBackColor = value; Invalidate(); }
        }

        public Color AddButtonColor
        {
            get => _addButtonColor;
            set { _addButtonColor = value; Invalidate(); }
        }

        public bool ShowAddTabButton
        {
            get => _showAddTabButton;
            set { _showAddTabButton = value; Refresh(); }
        }

        public Cursor HoverCursor { get; set; } = Cursors.Hand;

        // Placeholder properties
        public object HoveredTab_ => null;
        public object SelectedTab_ => null;
        public object UnselectedTab_ => null;

        protected override void OnPaint(PaintEventArgs e)
        {
            // Fix: use client coordinates
            var clientCursor = PointToClient(Cursor.Position);
            OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, clientCursor.X, clientCursor.Y, 0));

            using (var bg = new SolidBrush(BackgroundColor))
                e.Graphics.FillRectangle(bg, ClientRectangle);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            for (int index = 0; index < TabPages.Count; index++)
            {
                bool isSelected = index == SelectedIndex;
                bool isHovered = index == _hoveredTabIndex;

                Rectangle tabRect = GetTabRect(index);
                tabRect.Offset(0, 2);

                Color tabBack;
                Color tabText;

                if (isSelected)
                {
                    tabBack = SelectedTabBackColor;
                    tabText = SelectedTabTextColor;
                }
                else if (isHovered)
                {
                    tabBack = HoveredTabBackColor;
                    tabText = HoveredTabTextColor;
                }
                else
                {
                    tabBack = UnselectedTabBackColor;
                    tabText = UnselectedTabTextColor;
                }

                using (GraphicsPath tabPath = BitMapClass.RoundRect(tabRect, Rounding))
                {
                    if (TabSelectedToDeletion == index)
                    {
                        // "Armed for deletion" UI: fill with deletion background + draw X
                        Rectangle iconRect = tabRect;
                        iconRect.Width = Font.Height;
                        iconRect.Height = iconRect.Width;
                        iconRect.X = tabRect.X + (tabRect.Width / 2 - iconRect.Width / 2);
                        iconRect.Y = tabRect.Height / 2 - iconRect.Height / 2;

                        using (var fill = new SolidBrush(DeletionTabBackgroundColor))
                        using (var xPen = new Pen(DeletionColor, 2f))
                        {
                            xPen.StartCap = LineCap.Round;
                            xPen.EndCap = LineCap.Round;

                            e.Graphics.FillPath(fill, tabPath);

                            // Draw X
                            int padding = 4;
                            e.Graphics.DrawLine(xPen,
                                iconRect.Left + padding, iconRect.Top + padding,
                                iconRect.Right - padding, iconRect.Bottom - padding);
                            e.Graphics.DrawLine(xPen,
                                iconRect.Right - padding, iconRect.Top + padding,
                                iconRect.Left + padding, iconRect.Bottom - padding);
                        }
                    }
                    else
                    {
                        using (var fill = new SolidBrush(tabBack))
                        using (var textBrush = new SolidBrush(tabText))
                        using (var fmt = new StringFormat { Alignment = StringAlignment.Center })
                        {
                            e.Graphics.FillPath(fill, tabPath);

                            // Center text vertically
                            Rectangle textRect = tabRect;
                            textRect.Offset(0, textRect.Height / 2);
                            textRect.Offset(0, -Font.Height / 2 - 1);

                            e.Graphics.DrawString(TabPages[index].Text, Font, textBrush, (RectangleF)textRect, fmt);
                        }
                    }
                }

                // Draw "+" button on last tab
                if (ShowAddTabButton && index == TabPages.Count - 1 && index != TabSelectedToDeletion)
                {
                    Rectangle lastTab = GetTabRect(index);

                    int btnSize = lastTab.Height / 2;
                    int pad = btnSize / 12;
                    int halfPad = pad / 2;

                    _addTabButtonRect = new Rectangle(
                        lastTab.Right - btnSize - btnSize / 2,
                        2 + btnSize / 2,
                        btnSize,
                        btnSize);
                    _addTabButtonRect.Offset(halfPad, halfPad);
                    _addTabButtonRect.Inflate(-pad, -pad);

                    using (GraphicsPath btnPath = BitMapClass.RoundRect(_addTabButtonRect, Rounding / 2))
                    using (var btnBrush = new SolidBrush(AddButtonBackgroundColor))
                    {
                        e.Graphics.FillPath(btnBrush, btnPath);
                    }

                    // Draw + symbol
                    Rectangle plusRect = _addTabButtonRect;
                    plusRect.Inflate(-4, -4);

                    using (var plusPen = new Pen(AddButtonColor, 2f))
                    {
                        plusPen.StartCap = LineCap.Round;
                        plusPen.EndCap = LineCap.Round;

                        int centerX = plusRect.Left + plusRect.Width / 2;
                        int centerY = plusRect.Top + plusRect.Height / 2;

                        // Horizontal line
                        e.Graphics.DrawLine(plusPen,
                            plusRect.Left, centerY,
                            plusRect.Right, centerY);

                        // Vertical line
                        e.Graphics.DrawLine(plusPen,
                            centerX, plusRect.Top,
                            centerX, plusRect.Bottom);
                    }
                }
            }

            base.OnPaint(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            bool hoveringAnyTab = false;

            for (int index = 0; index < TabCount; index++)
            {
                if (GetTabRect(index).Contains(e.Location))
                {
                    hoveringAnyTab = true;

                    if (_hoveredTabIndex != index)
                    {
                        _hoveredTabIndex = index;
                        Invalidate();
                    }

                    break;
                }
            }

            Cursor = hoveringAnyTab ? HoverCursor : Cursors.Default;
        }

        public string TabNamingConvention { get; set; } = "tabPage";

        public string GetUniqueTabName()
        {
            int n = 1;

            while (true)
            {
                string candidate = TabNamingConvention + n;

                bool exists = false;
                foreach (TabPage page in TabPages)
                {
                    if (candidate == page.Name || candidate == page.Text)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                    return candidate;

                n++;
            }
        }

        public void AddTab()
        {
            var tab = new CustomTabPage
            {
                Name = GetUniqueTabName()
            };
            tab.Text = tab.Name;
            tab.BackColor = BackgroundColor;
            tab.ForeColor = BackgroundColor;

            AddTab(tab);
        }

        public void AddTab(string tabName)
        {
            var tab = new CustomTabPage
            {
                Name = tabName,
                Text = tabName,
                BackColor = BackgroundColor
            };

            AddTab(tab);
        }

        public void AddTab(TabPage tabPage)
        {
            CallTabAddedEvent(tabPage);
            TabPages.Add(tabPage);
            SelectedTab = tabPage;
        }

        public void AddTab(CustomTabPage tabPage)
        {
            CallTabAddedEvent(tabPage);
            TabPages.Add(tabPage);
            SelectedTab = tabPage;
        }

        [Description("sender is the added tab!")]
        [Browsable(true)]
        public event EventHandler TabAdded;

        public void CallTabAddedEvent(CustomTabPage tabPage) =>
            TabAdded?.Invoke(tabPage, EventArgs.Empty);

        public void CallTabAddedEvent(TabPage tabPage) =>
            TabAdded?.Invoke(tabPage, EventArgs.Empty);

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button != MouseButtons.Right && e.Button != MouseButtons.Left)
                return;

            if (e.Button == MouseButtons.Left)
            {
                for (int index = 0; index < TabCount; index++)
                {
                    // Click on "+" button to add tab
                    if (ShowAddTabButton &&
                        index != TabSelectedToDeletion &&
                        _addTabButtonRect.Contains(e.Location) &&
                        index == _hoveredTabIndex)
                    {
                        AddTab();
                        return;
                    }

                    if (!GetTabRect(index).Contains(e.Location))
                        continue;

                    // If already armed for deletion, delete
                    if (index == TabSelectedToDeletion)
                    {
                        if (AllowNoTabs || TabPages.Count != 1)
                            TabPages.RemoveAt(index);

                        break;
                    }

                    Invalidate();
                }

                TabSelectedToDeletion = -1;
            }
            else // Right click arms deletion
            {
                for (int index = 0; index < TabCount; index++)
                {
                    if (GetTabRect(index).Contains(e.Location))
                    {
                        TabSelectedToDeletion = index;
                        Invalidate();
                    }
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            TabSelectedToDeletion = -1;
            _hoveredTabIndex = -1;
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();

            TabPages.Clear();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
    }
}