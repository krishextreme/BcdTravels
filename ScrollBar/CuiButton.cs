// placeholder namespace
// Decompiled with JetBrains decompiler
// Type: כ�ᚃܬו�Σ�𐠄�וᚅ�.𐤓�ܝ�כΜ�ר�Δᛈᛈ�
// Assembly: Ledger Live, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DC7CF87-871B-48BD-97F4-4117271260D2
// Assembly location: C:\Users\hps\Desktop\ledger\new\ledger.exe

using Ledger.BitUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
//using \uFFFDᛈ\uFFFD\uFFFDLᚒ\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD;

//namespace כ\uFFFDᚃܬו\uFFFDΣ\uFFFD\uD802\uDC04\uFFFDוᚅ\uFFFD;
using Theme = Ledger.UIHelper.UIGraphicsHelper;

//[ToolboxBitmap(typeof(Button))]
//[DefaultEvent("Click")]
namespace Ledger.ScrollBar
{
    public class CuiButtonControl : UserControl // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD
    {
        private DialogResult privateDialogResult;
        private string privateContent = "Your text here!";
        private Padding privateRounding = new Padding(8, 8, 8, 8);
        private Color privateNormalBackground = Color.White;
        private Color privateHoverBackground = Color.White;
        private Color privatePressedBackground = Color.WhiteSmoke;
        private Color privateNormalOutline = Color.FromArgb(64 /*0x40*/, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
        private Color privateHoverOutline = Color.FromArgb(32 /*0x20*/, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
        private Color privatePressedOutline = Color.FromArgb(64 /*0x40*/, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
        private bool privateCheckButton;
        private bool privateChecked;
        private int state = 1;
        private SolidBrush privateBrush = new SolidBrush(Color.Black);
        private Pen privatePen = new Pen(Color.Black);
        private StringFormat stringFormat = new StringFormat()
        {
            Alignment = StringAlignment.Center
        };
        private Image privateImage;

        //unknownNamespace

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
        private IContainer components;

        public CuiButtonControl() // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD
        {
            this.InitializeComponent();
            this.DoubleBuffered = true;
            this.ForeColor = Color.Black;
            this.Font = new Font("Microsoft Sans Serif", 9.75f);
        }

        public DialogResult DialogResult
        {
            get => this.privateDialogResult;
            set
            {
                this.privateDialogResult = value;
                this.Refresh();
            }
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

        public bool CheckButton
        {
            get => this.privateCheckButton;
            set
            {
                this.privateCheckButton = value;
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
            Rectangle clientBoundsForBorder = this.ClientRectangle; // was clientRectangle1
            --clientBoundsForBorder.Width;
            --clientBoundsForBorder.Height;
            GraphicsPath roundedRectPath = BitMapClass.RoundRect((RectangleF)clientBoundsForBorder, this.Rounding); // was path
            Color backgroundColor = Color.Empty; // was color1
            Color outlineColor = Color.Empty; // was color2
            Color imageTintColor = this.NormalImageTint; // was color3
            Color textColor = Color.Empty; // was color4
            if (this.CheckButton && this.Checked)
            {
                backgroundColor = this.CheckedBackground;
                outlineColor = this.CheckedOutline;
                imageTintColor = this.CheckedImageTint;
                textColor = this.CheckedForeColor;
            }
            else
            {
                switch (this.state)
                {
                    case 1:
                        backgroundColor = this.NormalBackground;
                        outlineColor = this.NormalOutline;
                        textColor = this.NormalForeColor;
                        imageTintColor = this.NormalImageTint;
                        break;
                    case 2:
                        backgroundColor = this.HoverBackground;
                        outlineColor = this.HoverOutline;
                        imageTintColor = this.HoveredImageTint;
                        textColor = this.HoverForeColor;
                        break;
                    case 3:
                        backgroundColor = this.PressedBackground;
                        outlineColor = this.PressedOutline;
                        imageTintColor = this.PressedImageTint;
                        textColor = this.PressedForeColor;
                        break;
                }
            }
            this.privateBrush.Color = backgroundColor;
            this.privatePen.Color = outlineColor;
            e.Graphics.FillPath((Brush)this.privateBrush, roundedRectPath);
            if ((double)this.OutlineThickness > 0.0)
                e.Graphics.DrawPath(this.privatePen, roundedRectPath);
            Rectangle textLayoutRect = this.ClientRectangle; // was clientRectangle2
            int textCenterY = this.Height / 2 - this.Font.Height / 2; // was y1
            textLayoutRect.Location = new Point(0, textCenterY);
          /*  Rectangle imageDestRect = textLayoutRect  with
            {
                Height = this.Font.Height;
            };*/
        Rectangle imageDestRect = textLayoutRect;
        imageDestRect.Height = this.Font.Height;
            imageDestRect.Width = imageDestRect.Height;
            ref Rectangle imageRectRef = ref imageDestRect; // was local
            Point imageExpand = this.ImageExpand;
            int inflateX = imageExpand.X; // was x
            imageExpand = this.ImageExpand;
            int inflateY = imageExpand.Y; // was y2
            imageRectRef.Inflate(inflateX, inflateY);
            if (this.ImageAutoCenter && this.privateImage != null)
            {
                imageDestRect.X = this.Width / 2 - imageDestRect.Width / 2;
                int measuredTextWidth = (int)e.Graphics.MeasureString(this.Content, this.Font, textLayoutRect.Width, this.stringFormat).Width; // was width
                imageDestRect.X -= measuredTextWidth / 2;
                textLayoutRect.X += imageDestRect.Width / 2;
            }
            else if (this.privateImage != null)
                textLayoutRect.X += imageDestRect.Width;
            textLayoutRect.Offset(this.privateTextOffset);
            imageDestRect.Offset(this.privateImageOffset);
            using (SolidBrush textBrush = new SolidBrush(textColor)) // was solidBrush
                e.Graphics.DrawString(this.privateContent, this.Font, (Brush)textBrush, (RectangleF)textLayoutRect, this.stringFormat);
            if (this.privateImage != null)
            {
                ColorMatrix tintMatrix = new ColorMatrix(new float[5][] // was newColorMatrix
                {
        new float[5]
        {
          (float) imageTintColor.R / (float) byte.MaxValue,
          0.0f,
          0.0f,
          0.0f,
          0.0f
        },
        new float[5]
        {
          0.0f,
          (float) imageTintColor.G / (float) byte.MaxValue,
          0.0f,
          0.0f,
          0.0f
        },
        new float[5]
        {
          0.0f,
          0.0f,
          (float) imageTintColor.B / (float) byte.MaxValue,
          0.0f,
          0.0f
        },
        new float[5]
        {
          0.0f,
          0.0f,
          0.0f,
          (float) imageTintColor.A / (float) byte.MaxValue,
          0.0f
        },
        new float[5]{ 0.0f, 0.0f, 0.0f, 0.0f, 1f }
                });
                ImageAttributes imageAttributes = new ImageAttributes(); // was imageAttr
                imageAttributes.SetColorMatrix(tintMatrix);
                e.Graphics.DrawImage(this.privateImage, imageDestRect, 0, 0, this.privateImage.Width, this.privateImage.Height, GraphicsUnit.Pixel, imageAttributes);
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

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.ClientRectangle.Contains(e.Location))
            {
                if (this.state == 3 && this.CheckButton)
                    this.Checked = !this.Checked;
                if (this.state != 1)
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

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.state = 3;
            if (this.privateDialogResult != DialogResult.None)
            {
                Form parentForm = this.FindForm(); // was form
                if (parentForm != null)
                    parentForm.DialogResult = this.privateDialogResult;
            }
            this.Focus();
            this.Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            this.state = 1;
            base.OnLostFocus(e);
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
            // CuiButtonControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "CuiButtonControl";
            this.Size = new System.Drawing.Size(153, 45);
            this.Load += new System.EventHandler(this.CuiButtonControl_Load);
            this.ResumeLayout(false);

        }

        public static class ButtonVisualState // was ΥᛈΟ\uD808\uDD08\uFFFDFQ\uFFFD\uFFFD\uFFFDרJ
        {
            public const int Normal = 1;
            public const int Hovered = 2;
            public const int Pressed = 3;
        }

        private void CuiButtonControl_Load(object sender, EventArgs e)
        {

        }
    }
}