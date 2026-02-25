
using Ledger.BitUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;


namespace Ledger.ScrollBar
{
    public class CuiGradientPanel : UserControl
    {
        private Color _fillColor1 = Theme.PrimaryColor;
        private Color _fillColor2 = Color.Transparent;

        private Color _outlineColor1 = Theme.PrimaryColor;
        private Color _outlineColor2 = Theme.PrimaryColor;

        private float _outlineThickness = 1f;

        // Padding used as per-corner rounding radii (Left/Top/Right/Bottom).
        private Padding _rounding = new Padding(8, 8, 8, 8);

        private float _gradientAngle;

        private IContainer components;

        public CuiGradientPanel()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Refresh();
        }

        public Color PanelColor1
        {
            get => _fillColor1;
            set { _fillColor1 = value; Invalidate(); }
        }

        public Color PanelColor2
        {
            get => _fillColor2;
            set { _fillColor2 = value; Invalidate(); }
        }

        public Color PanelOutlineColor1
        {
            get => _outlineColor1;
            set { _outlineColor1 = value; Invalidate(); }
        }

        public Color PanelOutlineColor2
        {
            get => _outlineColor2;
            set { _outlineColor2 = value; Invalidate(); }
        }

        public float OutlineThickness
        {
            get => _outlineThickness;
            set { _outlineThickness = value; Invalidate(); }
        }

        public Padding Rounding
        {
            get => _rounding;
            set { _rounding = value; Invalidate(); }
        }

        public float GradientAngle
        {
            get => _gradientAngle;
            set
            {
                _gradientAngle = value % 360f;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = ClientRectangle;
            rect.Inflate(-1, -1);

             GraphicsPath path = BitMapClass.RoundRect(rect, 1);

            // Fill
            using (var fillBrush = new LinearGradientBrush(rect, _fillColor1, _fillColor2, _gradientAngle, true))
            {
                e.Graphics.FillPath(fillBrush, path);

                // This “erase” pass matches the decompiled intent: draw the path with BackColor,
                // typically to clean edges against parent background.
                 var edgePen = new Pen(BackColor);
                e.Graphics.DrawPath(edgePen, path);
            }

            // Outline
            using (var outlineBrush = new LinearGradientBrush(rect, _outlineColor1, _outlineColor2, _gradientAngle, true))
            using (var outlinePen = new Pen(outlineBrush, _outlineThickness))
            {
                e.Graphics.DrawPath(outlinePen, path);
            }

            base.OnPaint(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CuiGradientPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "CuiGradientPanel";
            this.Load += new System.EventHandler(this.CuiGradientPanel_Load);
            this.ResumeLayout(false);

        }

        private void CuiGradientPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
