using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;


namespace Ledger.UIHelper
{
    public class PieChartControl : UserControl
    {
        private string[] _dataPoints = new string[5]
        {
            "data1_100",
            "data2_90",
            "data3_50",
            "data4_50",
            "data5_300"
        };

        private List<float> privateDataValuePoints = new List<float>();
        private List<string> privateDataNamePoints = new List<string>();

        private Color _segmentColor =
            Color.FromArgb(64, UIGraphicsHelper.PrimaryColor);

        private Color _segmentBorderColor =
            UIGraphicsHelper.PrimaryColor;

        private int privateChartPadding = 30;
        private bool privateShowPopup = true;

        private float privateTotalValue;

        private bool showPopup;
        private PointF popupLocation;
        private string popupText;
        private bool mouseIn;

        private IContainer components;

        public PieChartControl()
        {
            InitializeComponent();
            MouseMove += OnMouseMove;
            DoubleBuffered = true;
            ForeColor = Color.White;
            Font = new Font("Microsoft Yahei UI", 8.25f);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public string[] DataPoints
        {
            get => _dataPoints;
            set
            {
                var values = new List<float>();
                var names = new List<string>();

                foreach (string item in value)
                {
                    string[] parts = item.Split('_');
                    names.Add(parts[0]);
                    values.Add(Convert.ToSingle(parts[1]));
                }

                privateDataValuePoints.Clear();
                privateDataNamePoints.Clear();

                privateDataValuePoints.AddRange(values);
                privateDataNamePoints.AddRange(names);

                _dataPoints = value;
                GC.Collect();
                Invalidate();
            }
        }

        public Color SegmentColor
        {
            get => _segmentColor;
            set
            {
                _segmentColor = value;
                Invalidate();
            }
        }

        public Color SegmentBorderColor
        {
            get => _segmentBorderColor;
            set
            {
                _segmentBorderColor = value;
                Invalidate();
            }
        }

        private Color GradientEndColor =>
            Color.FromArgb(48, ChartBorderColor);

        private Color PopupBackground
        {
            get
            {
                int sum = BackColor.R + BackColor.G + BackColor.B;
                return sum > 128
                    ? Color.FromArgb(64, Color.Black)
                    : Color.FromArgb(64, Color.White);
            }
        }

        private Color PopupText
        {
            get
            {
                int sum = BackColor.R + BackColor.G + BackColor.B;
                return sum > 128 ? Color.White : Color.Black;
            }
        }

        public int ChartPadding
        {
            get => privateChartPadding;
            set
            {
                privateChartPadding = value;
                MinimumSize = new Size(value * 4, value * 4);
                Invalidate();
            }
        }

        public bool ShowPopup
        {
            get => privateShowPopup;
            set
            {
                privateShowPopup = value;
                Invalidate();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Refresh();
        }

        public Color SliceBorderColor { get; set; } =
            Color.FromArgb(64, 255, 255, 255);

        public Color ChartBorderColor { get; set; } =
            UIGraphicsHelper.PrimaryColor;

        public float SliceBorderThickness { get; set; } = 1f;
        public float ChartBorderThickness { get; set; } = 1.6f;

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            int diameter = Math.Min(Width, Height) - 2 * ChartPadding;
            int centerX = Width / 2;
            int centerY = Height / 2;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(centerX - diameter / 2, centerY - diameter / 2, diameter, diameter);
                using (PathGradientBrush brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = Color.Transparent;
                    brush.SurroundColors = new[] { GradientEndColor };
                    brush.FocusScales = new PointF(0.25f, 0.25f);
                    g.FillEllipse(brush, centerX - diameter / 2, centerY - diameter / 2, diameter, diameter);
                }
            }

            privateTotalValue = privateDataValuePoints.Sum();
            float startAngle = 0f;

            GraphicsPath piePath = new GraphicsPath();

            try
            {
                for (int i = 0; i < DataPoints.Length; i++)
                {
                    float sweep =
                        privateDataValuePoints[i] / privateTotalValue * 360f;

                    piePath.AddPie(
                        centerX - diameter / 2,
                        centerY - diameter / 2,
                        diameter,
                        diameter,
                        startAngle,
                        sweep);

                    float mid = startAngle + sweep / 2f;
                    float labelX =
                        centerX + diameter / 4f * (float)Math.Cos(mid * Math.PI / 180);
                    float labelY =
                        centerY + diameter / 4f * (float)Math.Sin(mid * Math.PI / 180);

                    string label = privateDataNamePoints[i];
                    SizeF size = g.MeasureString(label, Font);

                    g.DrawString(
                        label,
                        Font,
                        new SolidBrush(ForeColor),
                        labelX - size.Width / 2,
                        labelY - size.Height / 2);

                    startAngle += sweep;
                }
            }
            catch
            {
                ChartPadding = ChartPadding;
                DataPoints = DataPoints;
                return;
            }

            using (Pen pen = new Pen(SliceBorderColor, SliceBorderThickness))
            {
                pen.DashStyle = DashStyle.Dash;
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                g.DrawPath(pen, piePath);
            }

            using (Pen pen = new Pen(ChartBorderColor, ChartBorderThickness))
            {
                g.DrawEllipse(
                    pen,
                    centerX - diameter / 2,
                    centerY - diameter / 2,
                    diameter,
                    diameter);
            }

            if (showPopup && ShowPopup && mouseIn)
            {
                SizeF size = g.MeasureString(popupText, Font);
                RectangleF rect = new RectangleF(
                    popupLocation.X - size.Width / 2,
                    popupLocation.Y - size.Height - 10,
                    size.Width,
                    size.Height);

                GraphicsPath popupPath =
                    UIGraphicsHelper.RoundRect(rect, (int)(rect.Height / 4));

                g.FillPath(new SolidBrush(PopupBackground), popupPath);
                g.DrawString(popupText, Font, new SolidBrush(PopupText), rect);
            }

            base.OnPaint(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            mouseIn = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouseIn = false;
            Refresh();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            int cx = Width / 2;
            int cy = Height / 2;

            float currentAngle = 0f;
            bool hit = false;

            for (int i = 0; i < DataPoints.Length; i++)
            {
                float sweep =
                    privateDataValuePoints[i] / privateTotalValue * 360f;

                float angle =
                    (float)(Math.Atan2(cy - e.Y, cx - e.X) * 180 / Math.PI + 180);

                if (angle >= currentAngle && angle <= currentAngle + sweep)
                {
                    showPopup = true;
                    popupLocation = new PointF(e.X, e.Y);
                    popupText =
                        $"{privateDataNamePoints[i]} ({privateDataValuePoints[i] / privateTotalValue * 100:0.##}%)";

                    Invalidate();
                    hit = true;
                    break;
                }

                currentAngle += sweep;
            }

            if (!hit)
            {
                showPopup = false;
                Invalidate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PieChartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PieChartControl";
            this.Size = new System.Drawing.Size(200, 185);
            this.Load += new System.EventHandler(this.PieChartControl_Load_1);
            this.ResumeLayout(false);

        }

        private void PieChartControl_Load(object sender, EventArgs e)
        {

        }

        private void PieChartControl_Load_1(object sender, EventArgs e)
        {

        }
    }

    /// <summary>
    /// Placeholder for missing external obfuscated helpers
    /// </summary>
    internal static class UIGraphicsHelper
    {
        public static Color PrimaryColor = Color.DeepSkyBlue;
        public static Color TranslucentPrimaryColor =>
            Color.FromArgb(102, PrimaryColor);

        public static GraphicsPath RoundRect(RectangleF rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

}
