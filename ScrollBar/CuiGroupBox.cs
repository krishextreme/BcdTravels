
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Ledger.BitUI;


namespace Ledger.ScrollBar
{
    public class CuiGroupBox : Panel
    {
        private Padding _rounding = new Padding(4);
        private Color _borderColor = Color.FromArgb(64, 128, 128, 128);
        private string _caption = "cuiGroupBox";

        private IContainer components;

        public CuiGroupBox()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.AllPaintingInWmPaint,
                true);

            Content = "Group Box";
        }

        [Category("CuoreUI")]
        [Description("How round will the inside border be.")]
        public Padding Rounding
        {
            get => _rounding;
            set
            {
                _rounding = value;
                Refresh();
            }
        }

        [Category("CuoreUI")]
        [Description("The color of the inner border.")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Refresh();
            }
        }

        [Category("CuoreUI")]
        [Description("The text that appears on top left.")]
        public string Content
        {
            get => _caption;
            set
            {
                _caption = value;
                Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            // Reserve vertical space for caption by increasing top padding.
            Padding r = Rounding;
            Padding = new Padding(
                r.Left,
                r.Top + Font.Height - 2,
                r.Right,
                r.Bottom);

            // Border rect starts halfway down the caption height (matches original).
            Rectangle borderRect = new Rectangle(
                0,
                Font.Height / 2,
                Width - 1,
                Height - Font.Height / 2 - 1);

            using (var borderPen = new Pen(_borderColor))
            using (var captionBrush = new SolidBrush(ForeColor))
            using (GraphicsPath borderPath = BitMapClass.RoundRect(borderRect, 1))
            {
                g.DrawPath(borderPen, borderPath);

                // Caption background "cutout"
                Size captionSize = TextRenderer.MeasureText(Content, Font);
                Rectangle captionRect = new Rectangle(
                    8,
                    0,
                    captionSize.Width + Font.Height,
                    captionSize.Height);

                using (var backBrush = new SolidBrush(BackColor))
                    g.FillRectangle(backBrush, captionRect);

                g.DrawString(
                    Content,
                    Font,
                    captionBrush,
                    (RectangleF)captionRect,
                    new StringFormat { Alignment = StringAlignment.Center });
            }

            base.OnPaint(e);
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
        }
    }
}
