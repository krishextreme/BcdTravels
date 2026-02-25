
    using Ledger.BitUI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.Windows.Forms;
    using System.Windows.Forms.Layout;
    using Theme = Ledger.UIHelper.UIGraphicsHelper;

    namespace Ledger.ScrollBar
    {
        [ToolboxBitmap(typeof(Button))]
        [DefaultEvent("Click")]
        public class CuiButtonGroup : UserControl
        {
      
        private string privateContent = "Your text here!";
        private Padding privateRounding = new Padding(8, 8, 8, 8);
        private Color privateNormalBackground = Color.White;
        private Color privateHoverBackground = Color.White;
        private Color privatePressedBackground = Color.WhiteSmoke;
        private Color privateNormalOutline = Color.FromArgb(64 /*0x40*/, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
        private Color privateHoverOutline = Color.FromArgb(32 /*0x20*/, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
        private Color privatePressedOutline = Color.FromArgb(64 /*0x40*/, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
        private bool privateChecked;
        private int state = 1;
        private SolidBrush privateBrush = new SolidBrush(Color.Black);
        private Pen privatePen = new Pen(Color.Black);
        private StringFormat stringFormat = new StringFormat()
        {
            Alignment = StringAlignment.Center
        };
        private Image privateImage;
        private Color privateCheckedBackground = UiAnimationGlobals.PrimaryColor;
        private Color privateCheckedOutline = UiAnimationGlobals.PrimaryColor;
        private bool privateImageAutoCenter = true;
        private StringAlignment privateTextAlignment = StringAlignment.Center;
        private float privateOutlineThickness = 1f;
        private Point privateImageExpand = Point.Empty;
        private Color privateCheckedForeColor = Color.White;
        private Color privatePressedForeColor = Color.FromArgb(32 /*0x20*/, 32 /*0x20*/, 32 /*0x20*/);
        private Color privateHoverForeColor = Color.Black;
        private Color privateImageTint = Color.White;
        private Color privateHoveredImageTint = Color.White;
        private Color privateCheckedImageTint = Color.White;
        private Color privatePressedImageTint = Color.White;
        private Point privateImageOffset = new Point(0, 0);
        private Point privateTextOffset = new Point(0, 0);
        private int privateGroup;
        private IContainer components;

        public CuiButtonGroup() // was K\uD802\uDF0FK\uD802\uDC00\uFFFD\uFFFD\uFFFDᚑ\uFFFD\uFFFD\uFFFD\uFFFDD unicode
        {
            this.InitializeComponent();
            this.DoubleBuffered = true;
            this.ForeColor = Color.Black;
            this.Font = new Font("Microsoft Sans Serif", 9.75f);
        }

        public string Content
        {
            get => this.privateContent;
            set
            {
                this.privateContent = value;
                this.Invalidate();
            }
        }

        public Padding Rounding
        {
            get => this.privateRounding;
            set
            {
                this.privateRounding = value;
                this.Invalidate();
            }
        }

        public Color NormalBackground
        {
            get => this.privateNormalBackground;
            set
            {
                this.privateNormalBackground = value;
                this.Invalidate();
            }
        }

        public Color HoverBackground
        {
            get => this.privateHoverBackground;
            set
            {
                this.privateHoverBackground = value;
                this.Invalidate();
            }
        }

        public Color PressedBackground
        {
            get => this.privatePressedBackground;
            set
            {
                this.privatePressedBackground = value;
                this.Invalidate();
            }
        }

        public Color NormalOutline
        {
            get => this.privateNormalOutline;
            set
            {
                this.privateNormalOutline = value;
                this.Invalidate();
            }
        }

        public Color HoverOutline
        {
            get => this.privateHoverOutline;
            set
            {
                this.privateHoverOutline = value;
                this.Invalidate();
            }
        }

        public Color PressedOutline
        {
            get => this.privatePressedOutline;
            set
            {
                this.privatePressedOutline = value;
                this.Invalidate();
            }
        }

        public bool Checked
        {
            get => this.privateChecked;
            set
            {
                this.privateChecked = value;
                this.Invalidate();
            }
        }

        public Image Image
        {
            get => this.privateImage;
            set
            {
                this.privateImage = value;
                this.Invalidate();
            }
        }

        public Color CheckedBackground
        {
            get => this.privateCheckedBackground;
            set
            {
                this.privateCheckedBackground = value;
                this.Invalidate();
            }
        }

        public Color CheckedOutline
        {
            get => this.privateCheckedOutline;
            set
            {
                this.privateCheckedOutline = value;
                this.Invalidate();
            }
        }

        public bool ImageAutoCenter
        {
            get => this.privateImageAutoCenter;
            set
            {
                this.privateImageAutoCenter = value;
                this.Invalidate();
            }
        }

        public StringAlignment TextAlignment
        {
            get => this.privateTextAlignment;
            set
            {
                this.privateTextAlignment = value;
                this.Refresh();
            }
        }

        public float OutlineThickness
        {
            get => this.privateOutlineThickness;
            set
            {
                this.privateOutlineThickness = Math.Max(value, 0.0f);
                this.privatePen.Width = value;
            }
        }

        public Point ImageExpand
        {
            get => this.privateImageExpand;
            set
            {
                this.privateImageExpand = value;
                this.Invalidate();
            }
        }

        public Color CheckedForeColor
        {
            get => this.privateCheckedForeColor;
            set
            {
                this.privateCheckedForeColor = value;
                this.Refresh();
            }
        }

        public Color PressedForeColor
        {
            get => this.privatePressedForeColor;
            set
            {
                this.privatePressedForeColor = value;
                this.Refresh();
            }
        }

        public Color NormalForeColor
        {
            get => this.ForeColor;
            set => this.ForeColor = value;
        }

        public Color HoverForeColor
        {
            get => this.privateHoverForeColor;
            set
            {
                this.privateHoverForeColor = value;
                this.Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.stringFormat.Trimming = StringTrimming.None;
            this.stringFormat.FormatFlags |= StringFormatFlags.FitBlackBox | StringFormatFlags.NoWrap;
            this.stringFormat.Alignment = this.TextAlignment;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            Rectangle clientRectangle1 = this.ClientRectangle;
            --clientRectangle1.Width;
            --clientRectangle1.Height;
            Padding rounding = this.Rounding;
            int num1 = rounding.Left == 0 ? 1 : 0;
            rounding = this.Rounding;
            int num2 = rounding.Bottom == 0 ? 1 : 0;
            if ((num1 & num2) != 0)
                clientRectangle1.Inflate(-1, 0);
            GraphicsPath path = BitMapClass.RoundRect((RectangleF)clientRectangle1, this.Rounding);
            Color color1 = Color.Empty;
            Color color2 = Color.Empty;
            Color color3 = this.NormalImageTint;
            Color color4 = Color.Empty;
            if (this.Checked)
            {
                color1 = this.CheckedBackground;
                color2 = this.CheckedOutline;
                color3 = this.CheckedImageTint;
                color4 = this.CheckedForeColor;
            }
            else
            {
                switch (this.state)
                {
                    case 1:
                        color1 = this.NormalBackground;
                        color2 = this.NormalOutline;
                        color4 = this.NormalForeColor;
                        color3 = this.NormalImageTint;
                        break;
                    case 2:
                        color1 = this.HoverBackground;
                        color2 = this.HoverOutline;
                        color3 = this.HoveredImageTint;
                        color4 = this.HoverForeColor;
                        break;
                    case 3:
                        color1 = this.PressedBackground;
                        color2 = this.PressedOutline;
                        color3 = this.PressedImageTint;
                        color4 = this.PressedForeColor;
                        break;
                }
            }
            this.privateBrush.Color = color1;
            this.privatePen.Color = color2;
            e.Graphics.FillPath((Brush)this.privateBrush, path);
            if ((double)this.OutlineThickness > 0.0)
                e.Graphics.DrawPath(this.privatePen, path);
            Rectangle clientRectangle2 = this.ClientRectangle;
            int y1 = this.Height / 2 - this.Font.Height / 2;
            clientRectangle2.Location = new Point(0, y1);

            // Replaced the decompiler's invalid "with" syntax (C# 9+ record clone syntax) with equivalent copy+set.
            Rectangle destRect = clientRectangle2; // was "clientRectangle2 with { Height = this.Font.Height }"
            destRect.Height = this.Font.Height;

            destRect.Width = destRect.Height;
            ref Rectangle local = ref destRect;
            Point imageExpand = this.ImageExpand;
            int x = imageExpand.X;
            imageExpand = this.ImageExpand;
            int y2 = imageExpand.Y;
            local.Inflate(x, y2);
            if (this.ImageAutoCenter && this.privateImage != null)
            {
                destRect.X = this.Width / 2 - destRect.Width / 2;
                int width = (int)e.Graphics.MeasureString(this.Content, this.Font, clientRectangle2.Width, this.stringFormat).Width;
                destRect.X -= width / 2;
                clientRectangle2.X += destRect.Width / 2;
            }
            else if (this.privateImage != null)
                clientRectangle2.X += destRect.Width;
            clientRectangle2.Offset(this.privateTextOffset);
            destRect.Offset(this.privateImageOffset);
            using (SolidBrush solidBrush = new SolidBrush(color4))
                e.Graphics.DrawString(this.privateContent, this.Font, (Brush)solidBrush, (RectangleF)clientRectangle2, this.stringFormat);
            if (this.privateImage != null)
            {
                ColorMatrix newColorMatrix = new ColorMatrix(new float[5][]
                {
        new float[5]
        {
          (float) color3.R / (float) byte.MaxValue,
          0.0f,
          0.0f,
          0.0f,
          0.0f
        },
        new float[5]
        {
          0.0f,
          (float) color3.G / (float) byte.MaxValue,
          0.0f,
          0.0f,
          0.0f
        },
        new float[5]
        {
          0.0f,
          0.0f,
          (float) color3.B / (float) byte.MaxValue,
          0.0f,
          0.0f
        },
        new float[5]
        {
          0.0f,
          0.0f,
          0.0f,
          (float) color3.A / (float) byte.MaxValue,
          0.0f
        },
        new float[5]{ 0.0f, 0.0f, 0.0f, 0.0f, 1f }
                });
                ImageAttributes imageAttr = new ImageAttributes();
                imageAttr.SetColorMatrix(newColorMatrix);
                e.Graphics.DrawImage(this.privateImage, destRect, 0, 0, this.privateImage.Width, this.privateImage.Height, GraphicsUnit.Pixel, imageAttr);
            }
            base.OnPaint(e);
        }

        public Color NormalImageTint
        {
            get => this.privateImageTint;
            set
            {
                this.privateImageTint = value;
                this.Invalidate();
            }
        }

        public Color HoveredImageTint
        {
            get => this.privateHoveredImageTint;
            set
            {
                this.privateHoveredImageTint = value;
                this.Invalidate();
            }
        }

        public Color CheckedImageTint
        {
            get => this.privateCheckedImageTint;
            set
            {
                this.privateCheckedImageTint = value;
                this.Invalidate();
            }
        }

        public Color PressedImageTint
        {
            get => this.privatePressedImageTint;
            set
            {
                this.privatePressedImageTint = value;
                this.Invalidate();
            }
        }

        public Point ImageOffset
        {
            get => this.privateImageOffset;
            set
            {
                this.privateImageOffset = value;
                this.Invalidate();
            }
        }

        public Point TextOffset
        {
            get => this.privateTextOffset;
            set
            {
                this.privateTextOffset = value;
                this.Invalidate();
            }
        }

        [Description("The group for this and other cuiButtonGroup controls to uncheck when clicked.")]
        public int Group
        {
            get => this.privateGroup;
            set
            {
                this.privateGroup = value;
                this.Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.state = 3;
            this.Focus();
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.ClientRectangle.Contains(e.Location))
            {
                if (this.state == 3)
                {
                    try
                    {
                        foreach (Control control in (ArrangedElementCollection)this.Parent?.Controls)
                        {
                            if (control is CuiButtonGroup item && item.Group == this.Group)
                                item.Checked = item == this;
                        }
                    }
                    catch
                    {
                    }
                }
                this.state = 2;
                this.Invalidate();
            }
            else
            {
                this.state = 1;
                this.Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.state = 1;
            this.Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.state = 2;
            this.Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CuiButtonGroup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "CuiButtonGroup";
            this.Size = new System.Drawing.Size(153, 45);
            this.Load += new System.EventHandler(this.CuiButtonGroup_Load);
            this.ResumeLayout(false);

        }

        private void CuiButtonGroup_Load(object sender, EventArgs e)
        {

        }
    }
    }