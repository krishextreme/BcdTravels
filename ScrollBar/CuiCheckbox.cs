
using Ledger.BitUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;


namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(CheckBox))]
    [DefaultEvent(nameof(CheckedChanged))]
    public class CuiCheckbox : Control
    {
        private bool _checked;

        private Color _checkedBackColor = Theme.PrimaryColor;
        private Color _uncheckedBackColor = Color.Empty;

        private bool _outlineStyle = true; // present in original; not used by original paint logic
        private Color _uncheckedOutlineColor = Color.Gray;
        private Color _checkedOutlineColor = Theme.PrimaryColor;
        private float _outlineThickness = 1f;

        private string _text;

        private int _cornerRadius = 5;

        // In original code this is public + reset each paint.
        public Point SymbolsOffset = new Point(0, 1);

        private Color _uncheckedSymbolColor = Color.Empty;
        private Color _checkedSymbolColor = Color.White;

        private bool _showSymbols = true;

        private IContainer components;

        public CuiCheckbox()
        {
            InitializeComponent();

            Size = new Size(90, 16);
            MinimumSize = new Size(16, 16);
            Cursor = Cursors.Hand;
            DoubleBuffered = true;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            Content = Name;
        }

        public event EventHandler CheckedChanged;

        [Description("Whether the switch is on or off.")]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (value == _checked)
                    return;

                _checked = value;
                Invalidate();
                CheckedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Description("The checked foreground.")]
        public Color CheckedForeground
        {
            get => _checkedBackColor;
            set { _checkedBackColor = value; Invalidate(); }
        }

        [Description("The unchecked foreground.")]
        public Color UncheckedForeground
        {
            get => _uncheckedBackColor;
            set { _uncheckedBackColor = value; Invalidate(); }
        }

        [Description("The style of the outline.")]
        public bool OutlineStyle
        {
            get => _outlineStyle;
            set { _outlineStyle = value; Invalidate(); }
        }

        [Description("The color of the outline.")]
        public Color UncheckedOutlineColor
        {
            get => _uncheckedOutlineColor;
            set { _uncheckedOutlineColor = value; Invalidate(); }
        }

        [Description("The color of the checked outline.")]
        public Color CheckedOutlineColor
        {
            get => _checkedOutlineColor;
            set { _checkedOutlineColor = value; Invalidate(); }
        }

        [Description("The thickness of the outline.")]
        public float OutlineThickness
        {
            get => _outlineThickness;
            set { _outlineThickness = value; Invalidate(); }
        }

        public string Content
        {
            get => _text;
            set { _text = value; Refresh(); }
        }

        private float SymbolStrokeWidth
        {
            get
            {
                // Matches decompiled formula.
                return (float)((Math.Min(Width, Height) / 20.0 + ((double)OutlineThickness + 1.0)) * 0.5);
            }
        }

        public int Rounding
        {
            get => _cornerRadius;
            set { _cornerRadius = value; Invalidate(); }
        }

        [Description("The color of the symbol when NOT checked.")]
        public Color UncheckedSymbolColor
        {
            get => _uncheckedSymbolColor;
            set { _uncheckedSymbolColor = value; Invalidate(); }
        }

        [Description("The color of the symbol when checked.")]
        public Color CheckedSymbolColor
        {
            get => _checkedSymbolColor;
            set { _checkedSymbolColor = value; Invalidate(); }
        }

        public bool ShowSymbols
        {
            get => _showSymbols;
            set { _showSymbols = value; Invalidate(); }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            Checked = !Checked;
            base.OnMouseClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            SymbolsOffset = new Point(0, 1);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF rectangle = new RectangleF((float)((double)this.OutlineThickness * 0.5 + 0.60000002384185791), (float)((double)this.OutlineThickness * 0.5 + 0.60000002384185791), (float)((double)this.Height - (double)this.OutlineThickness - 1.2000000476837158), (float)((double)this.Height - (double)this.OutlineThickness - 1.2000000476837158));
            using (GraphicsPath path1 =BitMapClass.RoundRect(rectangle, (int)((double)this.Rounding - (double)this.OutlineThickness - 0.60000002384185791)))
    {
                using (GraphicsPath path2 = BitMapClass.RoundRect(rectangle, this.Rounding))
      {
                    float num = (float)(this.Height - (int)((double)this.OutlineThickness * 2.0));
                    RectangleF area = new RectangleF(rectangle.X + this.OutlineThickness, rectangle.Y + this.OutlineThickness, (float)((double)num - (double)this.OutlineThickness - 1.2000000476837158), (float)((double)num - (double)this.OutlineThickness - 1.2000000476837158));
                    area.Inflate(-1f, -1f);
                    if (this.Checked)
                    {
                        using (SolidBrush solidBrush = new SolidBrush(this.CheckedForeground))
                            e.Graphics.FillPath((Brush)solidBrush, path1);
                        using (Pen pen = new Pen(this.CheckedOutlineColor, this.OutlineThickness))
                            e.Graphics.DrawPath(pen, path2);
                    }
                    else
                    {
                        using (SolidBrush solidBrush = new SolidBrush(this.UncheckedForeground))
                            e.Graphics.FillPath((Brush)solidBrush, path1);
                        using (Pen pen = new Pen(this.UncheckedOutlineColor, this.OutlineThickness))
                            e.Graphics.DrawPath(pen, path2);
                    }
                    area.Inflate(0.4f, 0.4f);
                    RectangleF layoutRectangle = area;
                    if (this.ShowSymbols)
                    {
                        if (this.Checked)
                        {
                            area.Offset(0.5f, -0.33f);
                            area.Inflate(0.25f, 0.25f);
                            using (var pen = new Pen(CheckedSymbolColor, SymbolStrokeWidth))
                            {
                                using (GraphicsPath path3 = BitMapClass.Checkmark(area, SymbolsOffset))
              {
                                    pen.StartCap = LineCap.Round;
                                    pen.EndCap = LineCap.Round;
                                    e.Graphics.DrawPath(pen, path3);
                                }
                            }
                        }
                        else
                        {
                            RectangleF rect = area;
                            rect.Inflate((float)-(int)((double)this.Height / 6.1999998092651367), (float)-(int)((double)this.Height / 6.1999998092651367));
                            rect.Offset(0.0f, -2.2f);
                            using (Pen pen = new Pen(this.UncheckedSymbolColor, SymbolStrokeWidth))
                            {
                                using (GraphicsPath path4 = BitMapClass.Crossmark(rect, SymbolsOffset))
              {
                                    pen.StartCap = LineCap.Round;
                                    pen.EndCap = LineCap.Round;
                                    e.Graphics.DrawPath(pen, path4);
                                }
                            }
                        }
                    }
                    using (StringFormat format = new StringFormat()
                    {
                        LineAlignment = StringAlignment.Center
                    })
                    {
                        using (SolidBrush solidBrush = new SolidBrush(this.ForeColor))
                        {
                            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                            layoutRectangle.Offset((float)((double)layoutRectangle.Width + 4.0 + (double)this.OutlineThickness * 1.5), (float)-((double)this.OutlineThickness * 1.5));
                            layoutRectangle.Width = (float)this.Width;
                            layoutRectangle.Height = (float)this.Height;
                            e.Graphics.DrawString(this.Content, this.Font, (Brush)solidBrush, layoutRectangle, format);
                        }
                    }
                }
            }
            base.OnPaint(e);
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    // Original resets this every paint.
        //    SymbolsOffset = new Point(0, 1);

        //    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        //    // Box rect is basically Height x Height with a small subpixel inset based on outline thickness.
        //    float inset = (float)((double)OutlineThickness * 0.5 + 0.60000002384185791);
        //    float boxSize = (float)(Height - (double)OutlineThickness - 1.2000000476837158);

        //    RectangleF boxRect = new RectangleF(inset, inset, boxSize, boxSize);

        //    // Inner fill path uses slightly smaller corner radius (radius - thickness - 0.6).
        //    int innerRadius = (int)(Rounding - (double)OutlineThickness - 0.60000002384185791);

        //    using (GraphicsPath fillPath = BitMapClass.RoundRect(Rectangle.Round(boxRect), innerRadius))
        //    using (GraphicsPath outlinePath = BitMapClass.RoundRect(Rectangle.Round(boxRect), Rounding))
        //    {
        //        // Symbol area inside the box
        //        float num = Height - (int)((double)OutlineThickness * 2.0);
        //        RectangleF symbolArea = new RectangleF(
        //            boxRect.X + OutlineThickness,
        //            boxRect.Y + OutlineThickness,
        //            (float)((double)num - (double)OutlineThickness - 1.2000000476837158),
        //            (float)((double)num - (double)OutlineThickness - 1.2000000476837158));

        //        symbolArea.Inflate(-1f, -1f);

        //        if (Checked)
        //        {
        //            using (var b = new SolidBrush(CheckedForeground))
        //                e.Graphics.FillPath(b, fillPath);

        //            using (var p = new Pen(CheckedOutlineColor, OutlineThickness))
        //                e.Graphics.DrawPath(p, outlinePath);
        //        }
        //        else
        //        {
        //            using (var b = new SolidBrush(UncheckedForeground))
        //                e.Graphics.FillPath(b, fillPath);

        //            using (var p = new Pen(UncheckedOutlineColor, OutlineThickness))
        //                e.Graphics.DrawPath(p, outlinePath);
        //        }

        //        symbolArea.Inflate(0.4f, 0.4f);
        //        RectangleF textLayoutRect = symbolArea;

        //        if (ShowSymbols)
        //        {
        //            if (Checked)
        //            {
        //                // Checkmark tweaks
        //                symbolArea.Offset(0.5f, -0.33f);
        //                symbolArea.Inflate(0.25f, 0.25f);

        //                using (var p = new Pen(CheckedSymbolColor, SymbolStrokeWidth))
        //                {
        //                    p.StartCap = LineCap.Round;
        //                    p.EndCap = LineCap.Round;

        //                    using (GraphicsPath mark = BitMapClass.Checkmark(Rectangle.Round(symbolArea), SymbolsOffset))
        //                        e.Graphics.DrawPath(p, mark);
        //                }
        //            }
        //            else
        //            {
        //                // Crossmark tweaks
        //                RectangleF crossRect = symbolArea;
        //                float shrink = -(int)(Height / 6.1999998092651367);
        //                crossRect.Inflate(shrink, shrink);
        //                crossRect.Offset(0.0f, -2.2f);

        //                using (var p = new Pen(UncheckedSymbolColor, SymbolStrokeWidth))
        //                {
        //                    p.StartCap = LineCap.Round;
        //                    p.EndCap = LineCap.Round;

        //                    using (GraphicsPath mark = BitMapClass.Crossmark(Rectangle.Round(crossRect), SymbolsOffset))
        //                        e.Graphics.DrawPath(p, mark);
        //                }
        //            }
        //        }

        //        // Draw the label to the right of the box
        //        using (var format = new StringFormat { LineAlignment = StringAlignment.Center })
        //        using (var textBrush = new SolidBrush(ForeColor))
        //        {
        //            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

        //            textLayoutRect.Offset(
        //                (float)((double)textLayoutRect.Width + 4.0 + (double)OutlineThickness * 1.5),
        //                (float)-((double)OutlineThickness * 1.5));

        //            textLayoutRect.Width = Width;
        //            textLayoutRect.Height = Height;

        //            e.Graphics.DrawString(Content, Font, textBrush, textLayoutRect, format);
        //        }
        //    }

        //    base.OnPaint(e);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Name = "cuiCheckbox";
            Size = new Size(28, 28);
            ResumeLayout(false);
        }
    }
}
