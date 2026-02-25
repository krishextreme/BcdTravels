// Decompiled with JetBrains decompiler (renamed for readability)
// Original type: כ….\uFFFD\uFFFDWᚃב…
//
// Purpose:
// - A custom textbox UserControl with:
//   - optional rounded border (via RoundRect helper)
//   - optional underlined style (draw only bottom-half border)
//   - focus/normal background + border colors
//   - placeholder text rendered by an overlaid read-only TextBox
//   - optional left image with tint (normal vs focused)
//
// External dependencies in your project:
// - Theme.PrimaryColor (default focus border color)
// - BitMapClass.RoundRect(RectangleF rect, Padding rounding)

using Ledger.BitUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;


namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(TextBox))]
    [DefaultEvent(nameof(ContentChanged))]
    public class CuiTextBox : UserControl
    {
        private Color _backgroundColor = Color.White;
        private Color _focusBackgroundColor = Color.White;

        private Color _borderColor = Color.FromArgb(128, 128, 128, 128);
        private Color _focusBorderColor = Theme.PrimaryColor;

        private int _borderSize = 1;
        private bool _underlinedStyle = true;

        internal bool _isFocusedInternal;

        private Padding _rounding = new Padding(8, 8, 8, 8);

        private string _placeholderText = "";
        private bool _isPlaceholderVisible;

        protected string _textValue = "";

        private Size _textOffset = new Size(0, 0);

        private Image _leftImage;
        private Point _imageExpand = Point.Empty;
        private Color _normalImageTint = Color.White;
        private Color _focusImageTint = Color.White;
        private Point _imageOffset = new Point(0, 0);

        private IContainer components;

        // Present in original designer output but unused.
        private Panel panel1;

        private TextBox placeholderTextBox;
        public TextBox contentTextBox;

        private bool IsFocused
        {
            get => _isFocusedInternal;
            set
            {
                _isFocusedInternal = value;

                contentTextBox.BackColor = value ? FocusBackgroundColor : BackgroundColor;
                placeholderTextBox.BackColor = contentTextBox.BackColor;

                Refresh();
            }
        }

        public event EventHandler ContentChanged;

        public CuiTextBox()
        {
            InitializeComponent();

            base.BackColor = Color.Empty;
            ForeColor = Color.Gray;

            Multiline = false;

            Load += OnLoad;
            GotFocus += OnLoad; // matches original
        }

        private void OnLoad(object sender, EventArgs e) => IsFocused = false;

        [Category("CuoreUI")]
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;

                if (DesignMode)
                {
                    contentTextBox.BackColor = value;
                    placeholderTextBox.BackColor = value;
                }
                else
                {
                    contentTextBox.BackColor = IsFocused ? FocusBackgroundColor : value;
                    placeholderTextBox.BackColor = contentTextBox.BackColor;
                }

                Refresh();
            }
        }

        [Category("CuoreUI")]
        public Color FocusBackgroundColor
        {
            get => _focusBackgroundColor;
            set
            {
                _focusBackgroundColor = value;

                if (DesignMode)
                {
                    contentTextBox.BackColor = value;
                    placeholderTextBox.BackColor = value;
                }
                else
                {
                    // NOTE: this mirrors the decompiled logic (a bit odd, but faithful).
                    contentTextBox.BackColor = IsFocused ? FocusBackgroundColor : value;
                    placeholderTextBox.BackColor = contentTextBox.BackColor;
                }

                Refresh();
            }
        }

        [Category("CuoreUI")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        [Category("CuoreUI")]
        public Color FocusBorderColor
        {
            get => _focusBorderColor;
            set => _focusBorderColor = value;
        }

        [Category("CuoreUI")]
        private int BorderSize
        {
            get => _borderSize;
            set
            {
                if (value < 1)
                    return;

                _borderSize = value;
                Invalidate();
            }
        }

        [Category("CuoreUI")]
        public bool UnderlinedStyle
        {
            get => _underlinedStyle;
            set { _underlinedStyle = value; Invalidate(); }
        }

        [Category("CuoreUI")]
        public bool PasswordChar
        {
            get => contentTextBox.UseSystemPasswordChar;
            set => contentTextBox.UseSystemPasswordChar = value;
        }

        [Category("CuoreUI")]
        public bool Multiline
        {
            get => contentTextBox.Multiline;
            set
            {
                contentTextBox.Multiline = value;
                placeholderTextBox.Multiline = value;
            }
        }

        // Decompiler shows a private "new BackColor" that forces alpha=255.
        [Category("CuoreUI")]
        private new Color BackColor
        {
            get => base.BackColor;
            set
            {
                value = Color.FromArgb(255, value);
                base.BackColor = value;
            }
        }

        [Category("CuoreUI")]
        public override Color ForeColor
        {
            get => contentTextBox.ForeColor;
            set
            {
                base.ForeColor = value;
                contentTextBox.ForeColor = value;
                contentTextBox.Refresh();
            }
        }

        [Category("CuoreUI")]
        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                contentTextBox.Font = value;
                placeholderTextBox.Font = value;
            }
        }

        [Category("CuoreUI")]
        public string Content
        {
            get => _textValue;
            set
            {
                _textValue = value;
                contentTextBox.Text = value;
                UpdatePlaceholder();
            }
        }

        [Category("CuoreUI")]
        public Padding Rounding
        {
            get => _rounding;
            set
            {
                if (value == new Padding(0, 0, 0, 0))
                    value = new Padding(2, 2, 2, 2);

                if (value.All < 0 && value.All != -1)
                    return;

                _rounding = value;
                Invalidate();
            }
        }

        [Category("CuoreUI")]
        public Color PlaceholderColor
        {
            get => placeholderTextBox.ForeColor;
            set => placeholderTextBox.ForeColor = value;
        }

        [Category("CuoreUI")]
        public string PlaceholderText
        {
            get => _placeholderText;
            set
            {
                _placeholderText = value;
                UpdatePlaceholder();
            }
        }

        [Category("CuoreUI")]
        public Size TextOffset
        {
            get => _textOffset;
            set { _textOffset = value; Refresh(); }
        }

        [Category("CuoreUI")]
        public Image Image
        {
            get => _leftImage;
            set { _leftImage = value; Invalidate(); }
        }

        [Category("CuoreUI")]
        public Point ImageExpand
        {
            get => _imageExpand;
            set { _imageExpand = value; Invalidate(); }
        }

        [Category("CuoreUI")]
        public Color NormalImageTint
        {
            get => _normalImageTint;
            set { _normalImageTint = value; Invalidate(); }
        }

        [Category("CuoreUI")]
        public Color FocusImageTint
        {
            get => _focusImageTint;
            set { _focusImageTint = value; Invalidate(); }
        }

        [Category("CuoreUI")]
        public Point ImageOffset
        {
            get => _imageOffset;
            set { _imageOffset = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            placeholderTextBox.Visible = _isPlaceholderVisible;

            Graphics g = e.Graphics;

            // Fill control background (the UserControl surface)
            using (var back = new SolidBrush(BackColor))
                g.FillRectangle(back, ClientRectangle);

            // Compute padding so the inner textboxes are vertically aligned and leave space for image.
            Padding padding;
            if (Multiline)
            {
                int vPad = Rounding.All / 2 + Font.Height / 8;
                padding = new Padding(Font.Height, vPad, Font.Height, vPad);
            }
            else
            {
                int top = Height / 2 - Font.Height / 2;
                if (top < 0) top = -top;
                padding = new Padding(Font.Height, top, Font.Height, 0);
            }

            padding.Left += TextOffset.Width;
            padding.Right += TextOffset.Width;
            padding.Top += TextOffset.Height;
            padding.Bottom += TextOffset.Height;

            Padding = padding;

            // Draw border: rounded path if rounding > 1 (or -1), else simple line/rect.
            if (_rounding.All > 1 || _rounding.All == -1)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle outer = ClientRectangle;
                Rectangle inner = Rectangle.Inflate(outer, -BorderSize, -BorderSize);

                int eraseWidth = _borderSize > 0 ? _borderSize : 1;

                using (var innerFill = new SolidBrush(IsFocused ? FocusBackgroundColor : BackgroundColor))
                using (GraphicsPath outerPath = BitMapClass.RoundRect(outer, 1))
                using (GraphicsPath innerPath = BitMapClass.RoundRect(inner, 1))
                using (var erasePen = new Pen(BackColor, eraseWidth))
                using (var borderPen = new Pen(IsFocused ? FocusBorderColor : BorderColor, BorderSize)
                {
                    Alignment = PenAlignment.Center
                })
                {
                    e.Graphics.FillPath(innerFill, innerPath);

                    if (UnderlinedStyle)
                    {
                        // "Erase" outer and then draw only the bottom half of the border.
                        g.DrawPath(erasePen, outerPath);

                        RectangleF bounds = innerPath.GetBounds();
                        using (var region = new Region(new RectangleF(
                                   bounds.X + 1f,
                                   bounds.Y + bounds.Height / 2f,
                                   bounds.Width - 1f,
                                   (float)(bounds.Height / 2.0 + 1.0))))
                        {
                            g.SetClip(region, CombineMode.Intersect);
                            g.DrawPath(borderPen, innerPath);
                            g.ResetClip();
                        }
                    }
                    else
                    {
                        g.DrawPath(erasePen, outerPath);
                        g.DrawPath(borderPen, innerPath);
                    }
                }
            }
            else
            {
                using (var pen = new Pen(BorderColor, BorderSize) { Alignment = PenAlignment.Inset })
                {
                    Region = new Region(ClientRectangle);

                    if (IsFocused)
                        pen.Color = FocusBorderColor;

                    if (UnderlinedStyle)
                        g.DrawLine(pen, 0, Height - 1, Width, Height - 1);
                    else
                        g.DrawRectangle(pen, 0.0f, 0.0f, Width - 0.5f, Height - 0.5f);
                }
            }

            // Optional left image (tinted)
            Color tint = _isFocusedInternal ? FocusImageTint : NormalImageTint;

            Rectangle imageRect = new Rectangle(
                contentTextBox.Height,
                contentTextBox.Location.Y,
                contentTextBox.Height,
                contentTextBox.Height);

            imageRect.Inflate(ImageExpand.X, ImageExpand.Y);
            imageRect.Offset(_imageOffset);

            if (_leftImage != null)
            {
                var matrix = new ColorMatrix(new[]
                {
                    new float[] { tint.R / 255f, 0, 0, 0, 0 },
                    new float[] { 0, tint.G / 255f, 0, 0, 0 },
                    new float[] { 0, 0, tint.B / 255f, 0, 0 },
                    new float[] { 0, 0, 0, tint.A / 255f, 0 },
                    new float[] { 0, 0, 0, 0, 1f }
                });

                using (var attrs = new ImageAttributes())
                {
                    attrs.SetColorMatrix(matrix);
                    e.Graphics.DrawImage(_leftImage, imageRect, 0, 0, _leftImage.Width, _leftImage.Height, GraphicsUnit.Pixel, attrs);
                }
            }

            base.OnPaint(e);
        }

        protected void UpdatePlaceholder()
        {
            placeholderTextBox.Text = PlaceholderText;

            if (_textValue == "" && !_isFocusedInternal)
            {
                placeholderTextBox.Visible = true;
                _isPlaceholderVisible = true;
            }
            else
            {
                placeholderTextBox.Visible = false;
                _isPlaceholderVisible = false;
            }
        }

        private void ContentTextBox_TextChanged(object sender, EventArgs e)
        {
            _textValue = contentTextBox.Text;
            UpdatePlaceholder();
            ContentChanged?.Invoke(this, e);
        }

        private void ContentTextBox_Click(object sender, EventArgs e) => OnClick(e);
        private void ContentTextBox_MouseEnter(object sender, EventArgs e) => OnMouseEnter(e);
        private void ContentTextBox_MouseLeave(object sender, EventArgs e) => OnMouseLeave(e);
        private void ContentTextBox_KeyPress(object sender, KeyPressEventArgs e) => OnKeyPress(e);

        private void ContentTextBox_Enter(object sender, EventArgs e)
        {
            IsFocused = true;
            UpdatePlaceholder();
        }

        private void ContentTextBox_Leave(object sender, EventArgs e)
        {
            IsFocused = false;
            Refresh();
            UpdatePlaceholder();
        }

        private void CuiTextBox_Click(object sender, EventArgs e)
        {
            contentTextBox.Focus();
            Refresh();
        }

        private void PlaceholderTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            CuiTextBox_Click(sender, e);
        }

        private void PlaceholderTextBox_TextChanged(object sender, EventArgs e)
        {
            // Keep placeholder textbox synced to PlaceholderText if anything tries to change it.
            if (placeholderTextBox.Text != PlaceholderText)
                placeholderTextBox.Text = PlaceholderText;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            contentTextBox = new TextBox();
            panel1 = new Panel();
            placeholderTextBox = new TextBox();
            SuspendLayout();
            // 
            // contentTextBox
            // 
            contentTextBox.BorderStyle = BorderStyle.None;
            contentTextBox.Dock = DockStyle.Fill;
            contentTextBox.Location = new Point(10, 7);
            contentTextBox.Name = "contentTextBox";
            contentTextBox.Size = new Size(246, 15);
            contentTextBox.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(0, 0);
            panel1.TabIndex = 1;
            // 
            // placeholderTextBox
            // 
            placeholderTextBox.BorderStyle = BorderStyle.None;
            placeholderTextBox.Dock = DockStyle.Fill;
            placeholderTextBox.Location = new Point(10, 7);
            placeholderTextBox.Name = "placeholderTextBox";
            placeholderTextBox.ReadOnly = true;
            placeholderTextBox.Size = new Size(246, 15);
            placeholderTextBox.TabIndex = 2;
            // 
            // CuiTextBox
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.Window;
            Controls.Add(placeholderTextBox);
            Controls.Add(panel1);
            Controls.Add(contentTextBox);
            Cursor = Cursors.IBeam;
            DoubleBuffered = true;
            Font = new Font("Microsoft Sans Serif", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.FromArgb(64, 64, 64);
            Margin = new Padding(4);
            Name = "CuiTextBox";
            Padding = new Padding(10, 7, 10, 7);
            Size = new Size(266, 45);
            Load += CuiTextBox_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        private void CuiTextBox_Load(object sender, EventArgs e)
        {

        }
    }
}
