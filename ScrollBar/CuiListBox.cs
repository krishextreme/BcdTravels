

using Ledger.BitUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;



namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(ListBox))]
    public class CuiListBox : ListBox
    {
        private int _rounding = 8;
        private int _itemRounding = 8;

        private Color _backgroundColor = Color.Empty;

        private Color _itemHoverBackColor = Color.FromArgb(64, 128, 128, 128);
        private Color _itemHoverForeColor = Color.Gray;

        private Color _itemForeColor = Color.DimGray;

        private Color _itemSelectedBackColor = Theme.PrimaryColor;
        private Color _itemSelectedForeColor = Color.FromArgb(11, 11, 12);

        private int _hoveredIndex = -1;

        private const int WM_VSCROLL = 0x0115;   // 277
        private const int WM_MOUSEWHEEL = 0x020A; // 522 (decompile called it MSCROLL)

        private IContainer components;

        public CuiListBox()
        {
            InitializeComponent();

            DoubleBuffered = true;

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);

            DrawMode = DrawMode.OwnerDrawFixed;
            BorderStyle = BorderStyle.None;

            ItemHeight = 34;
            ForeColor = Color.FromArgb(84, 84, 84);
            SelectionMode = SelectionMode.One;

            //Theme.FrameDrawn += RefreshTimer_Tick;

            Font = new Font("Microsoft YaHei UI", 9f, FontStyle.Regular);
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            if (!DoubleBuffered)
                return;

            SuspendLayout();
            Refresh();
            ResumeLayout(true);
        }

        public int Rounding
        {
            get => _rounding;
            set
            {
                if (value <= 0)
                    throw new Exception("Rounding cannot be less than 1");

                if (value > ClientRectangle.Height / 2)
                {
                    _rounding = ClientRectangle.Height / 2;
                    Rounding = _rounding; // preserve decompiled recursion behavior
                }
                else
                {
                    _rounding = value;
                }

                Invalidate();
            }
        }

        public int ItemRounding
        {
            get => _itemRounding;
            set
            {
                if (value <= 0)
                    throw new Exception("ItemRounding cannot be greater than half of Item Height");

                if (value > ItemHeight / 2)
                {
                    _itemRounding = ItemHeight / 2 + 1;
                    ItemRounding = _itemRounding; // preserve decompiled recursion behavior
                }
                else
                {
                    _itemRounding = value;
                }

                Invalidate();
            }
        }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set { _backgroundColor = value; Invalidate(); }
        }

        public Color ItemHoverBackgroundColor
        {
            get => _itemHoverBackColor;
            set { _itemHoverBackColor = value; Invalidate(); }
        }

        public Color ItemHoverForegroundColor
        {
            get => _itemHoverForeColor;
            set { _itemHoverForeColor = value; Invalidate(); }
        }

        public Color ForegroundColor
        {
            get => _itemForeColor;
            set { _itemForeColor = value; Invalidate(); }
        }

        public Color ItemSelectedBackgroundColor
        {
            get => _itemSelectedBackColor;
            set { _itemSelectedBackColor = value; Invalidate(); }
        }

        public Color SelectedForegroundColor
        {
            get => _itemSelectedForeColor;
            set { _itemSelectedForeColor = value; Invalidate(); }
        }

        public int HoveredIndex => _hoveredIndex;

        protected override void OnPaint(PaintEventArgs e)
        {
            SelectionMode = SelectionMode.One;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            Rectangle client = ClientRectangle;

            // “Overscan” fill to hide edges (matches decompile)
            Rectangle backFill = client;
            backFill.Inflate(5, 5);
            backFill.Offset(-1, -1);

            client.Width--;
            client.Height--;

            using (Brush baseBrush = new SolidBrush(BackColor))
                g.FillRectangle(baseBrush, backFill);

            using (GraphicsPath containerPath = BitMapClass.RoundRect(client, Rounding))
            using (Brush containerBrush = new SolidBrush(BackgroundColor))
                g.FillPath(containerBrush, containerPath);

            for (int i = 0; i < Items.Count; i++)
            {
                Rectangle itemRect = GetItemRectangle(i);
                itemRect.Inflate(-4, -2);
                itemRect.Offset(0, 2);

                int textY = itemRect.Y + ItemHeight / 2 - Font.Height + 4;

                 GraphicsPath itemPath = BitMapClass.RoundRect(itemRect, ItemRounding);

                bool isSelected = SelectedIndex == i;
                bool isHovered = HoveredIndex == i;

                Color back = isSelected
                    ? ItemSelectedBackgroundColor
                    : isHovered ? ItemHoverBackgroundColor : BackgroundColor;

                Color fore = isSelected
                    ? SelectedForegroundColor
                    : isHovered ? ItemHoverForegroundColor : ForegroundColor;

                using (Brush b = new SolidBrush(back))
                    g.FillPath(b, itemPath);

                string text = Items[i]?.ToString() ?? string.Empty;
                using (Brush tb = new SolidBrush(fore))
                    g.DrawString(text, Font, tb, itemRect.X + 6f, textY);
            }

            base.OnPaint(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int index = IndexFromPoint(e.Location);
            if (index >= 0 && index < Items.Count)
            {
                SelectedIndex = index;

                SuspendLayout();
                Refresh();
                ResumeLayout(true);
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _hoveredIndex = IndexFromPoint(e.Location);

            if (e.Button == MouseButtons.Left)
                OnMouseDown(e);

            base.OnMouseMove(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            SuspendLayout();
            Refresh();
            ResumeLayout(true);

            base.OnSelectedIndexChanged(e);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg != WM_VSCROLL && m.Msg != WM_MOUSEWHEEL)
                return;

            SuspendLayout();
            Refresh();
            ResumeLayout(true);
        }

        private void CuiListBox_MouseLeave(object sender, EventArgs e)
        {
            // Decompiled used ClientRectangle.Contains(Cursor.Position) which is client-coordinates vs screen.
            // Preserving exact logic would keep that bug; here's the corrected equivalent:
            Point clientCursor = PointToClient(Cursor.Position);
            if (ClientRectangle.Contains(clientCursor))
                return;

            _hoveredIndex = -1;
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Theme.FrameDrawn -= RefreshTimer_Tick;
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            MouseLeave += new EventHandler(CuiListBox_MouseLeave);
            ResumeLayout(false);
        }
    }
}
