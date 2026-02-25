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
    public class LineChartControl : UserControl
    {
        private float[] privateDataPoints = new float[7] { 100f, 90f, 80f, 75f, 70f, 65f, 60f };
        private bool usePercent = true;
        private float privateMaxValue = 100f;
        private int chartPadding = 40;
        private bool showPopup;
        private PointF popupLocation;
        private string popupText;
        private bool privateGradientBackground = true;
        private Color privatePointColor = UIGraphicsHelper.PrimaryColor;
        private Color privateAxisColor = Color.Gray;
        private string[] xLabelsLong = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        private string[] xLabelsShort = new string[7] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
        private string[] privateCustomXAxis = new string[0];
        private Color privateChartLineColor = UIGraphicsHelper.PrimaryColor;
        private Color privateDayColor = Color.DarkGray;
        private bool privateShortDates = true;
        private bool privateAutoMaxValue = true;
        private bool privateUseBezier;
        private IContainer components;

        public LineChartControl()
        {
            InitializeComponent();
            MouseMove += OnMouseMove;
            DoubleBuffered = true;
            Font = new Font("Microsoft Yahei UI", 8.25f);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Refresh();
        }

        public bool hasDuplicate(int[] nums)
        {
            Array.Sort(nums);
            for (int i = 1; i < nums.Length; i++)
                if (nums[i] == nums[i - 1])
                    return true;
            return false;
        }

        [Browsable(true)]
        [Category("CuoreUI Chart Colors")]
        [Description("Whether the background under the lines should be a gradient.")]
        public bool GradientBackground
        {
            get => privateGradientBackground;
            set { privateGradientBackground = value; Refresh(); }
        }

        [Browsable(true)]
        [Category("CuoreUI Chart Colors")]
        [Description("Color of the circular points.")]
        public Color PointColor
        {
            get => privatePointColor;
            set { privatePointColor = value; Refresh(); }
        }

        [Browsable(true)]
        [Category("CuoreUI Chart Colors")]
        [Description("Color of the axis (x and y lines).")]
        public Color AxisColor
        {
            get => privateAxisColor;
            set { privateAxisColor = value; Refresh(); }
        }

        [Browsable(true)]
        [Category("CuoreUI Chart Data")]
        [Description("The data points that will be plotted on the line chart.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public float[] DataPoints
        {
            get => privateDataPoints;
            set
            {
                privateDataPoints = value;
                if (privateAutoMaxValue)
                    privateMaxValue = privateDataPoints.Max();
                Invalidate();
            }
        }

        [Browsable(true)]
        [Category("CuoreUI Chart Data")]
        [Description("The X axis values, if you don't want to use the weekdays.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string[] CustomXAxis
        {
            get => privateCustomXAxis;
            set { privateCustomXAxis = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("CuoreUI Chart Layout")]
        [Description("The padding around the chart area.")]
        public int ChartPadding
        {
            get => chartPadding;
            set { chartPadding = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("CuoreUI Chart Layout")]
        [Description("If true, the Y-axis will show percentages. If false, it will show absolute values.")]
        public bool UsePercent
        {
            get => usePercent;
            set { usePercent = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("CuoreUI Chart Layout")]
        [Description("The maximum value for the Y-axis.")]
        public float MaxValue
        {
            get => privateMaxValue;
            set { AutoMaxValue = false; privateMaxValue = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("CuoreUI Chart Colors")]
        [Description("Color of the line that connects points.")]
        public Color ChartLineColor
        {
            get => privateChartLineColor;
            set { privateChartLineColor = value; Refresh(); }
        }

        [Browsable(true)]
        [Category("CuoreUI Chart Colors")]
        [Description("Color of the day label.")]
        public Color DayColor
        {
            get => privateDayColor;
            set { privateDayColor = value; Refresh(); }
        }

        [Browsable(true)]
        [Category("CuoreUI Chart Data")]
        [Description("Whether the dates should be minified.")]
        public bool ShortDates
        {
            get => privateShortDates;
            set { privateShortDates = value; Refresh(); }
        }

        [Browsable(true)]
        [Category("CuoreUI Chart Layout")]
        [Description("Whether the MaxValue should update automatically to fit chart data.")]
        public bool AutoMaxValue
        {
            get => privateAutoMaxValue;
            set
            {
                privateAutoMaxValue = value;
                if (value) privateMaxValue = privateDataPoints.Max();
                Invalidate();
            }
        }

        [Browsable(true)]
        [Category("CuoreUI Chart Layout")]
        [Description("Smoothens the lines by using Bezier curves instead.")]
        public bool UseBezier
        {
            get => privateUseBezier;
            set { privateUseBezier = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int width = Width - chartPadding * 2;
            int height = Height - chartPadding * 2;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            Pen axisPen = new Pen(AxisColor, 1f);
            Pen gridPen = new Pen(Color.FromArgb(64, AxisColor), 1f) { DashStyle = DashStyle.Dash };
            Pen linePen = new Pen(ChartLineColor, 2f);

            // Draw axes
            g.DrawLine(axisPen, chartPadding, chartPadding, chartPadding, height + chartPadding);
            g.DrawLine(axisPen, chartPadding, height + chartPadding, width + chartPadding, height + chartPadding);

            // Draw horizontal grid lines
            for (int i = 1; i <= 5; i++)
            {
                float y = chartPadding + height - i * height / 5f;
                g.DrawLine(gridPen, chartPadding, y, width + chartPadding, y);
            }

            float scale = 100f / privateMaxValue;

            if (privateDataPoints.Length > 1)
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    float xStart = chartPadding;
                    float yStart = chartPadding + height - privateDataPoints[0] * scale / 100f * height;
                    path.StartFigure();
                    path.AddLine(xStart, chartPadding + height, xStart, yStart);

                    for (int i = 0; i < privateDataPoints.Length - 1; i++)
                    {
                        float x1 = chartPadding + i * width / (privateDataPoints.Length - 1);
                        float y1 = chartPadding + height - privateDataPoints[i] * scale / 100f * height;
                        float x2 = chartPadding + (i + 1) * width / (privateDataPoints.Length - 1);
                        float y2 = chartPadding + height - privateDataPoints[i + 1] * scale / 100f * height;

                        if (UseBezier)
                        {
                            float cx1 = x1 + (x2 - x1) / 3f;
                            float cy1 = y1;
                            float cx2 = x2 - (x2 - x1) / 3f;
                            float cy2 = y2;
                            g.DrawBezier(linePen, x1, y1, cx1, cy1, cx2, cy2, x2, y2);
                            path.AddBezier(x1, y1, cx1, cy1, cx2, cy2, x2, y2);
                        }
                        else
                        {
                            g.DrawLine(linePen, x1, y1, x2, y2);
                            path.AddLine(x1, y1, x2, y2);
                        }
                    }

                    float xEnd = chartPadding + width;
                    float yEnd = chartPadding + height - privateDataPoints.Last() * scale / 100f * height;
                    path.AddLine(xEnd, yEnd, xEnd, chartPadding + height);
                    path.CloseFigure();

                    // Gradient background
                    if (GradientBackground)
                    {
                        Color c = ChartLineColor;
                        using (LinearGradientBrush brush = new LinearGradientBrush(
                                   new Point(0, 0), new Point(0, Height),
                                   Color.FromArgb(64, ChartLineColor), Color.Transparent))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                    else
                    {
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(32, ChartLineColor)))
                            g.FillPath(brush, path);
                    }

                    // Draw points
                    using (SolidBrush brush = new SolidBrush(PointColor))
                        for (int i = 0; i < privateDataPoints.Length; i++)
                        {
                            float x = chartPadding + i * width / (privateDataPoints.Length - 1);
                            float y = chartPadding + height - privateDataPoints[i] * scale / 100f * height;
                            g.FillEllipse(brush, x - 4f, y - 4f, 8f, 8f);
                        }

                    // Draw popup if active
                    if (showPopup)
                    {
                        SizeF sizeF = g.MeasureString(popupText, Font);
                        RectangleF rect = new RectangleF(
                            popupLocation.X - sizeF.Width / 2f,
                            popupLocation.Y - sizeF.Height - 10f,
                            sizeF.Width, sizeF.Height);

                        using (GraphicsPath pathPopup = UIGraphicsHelper.RoundRect(rect, (int)(rect.Height / 4f)))
                        {
                            using (SolidBrush brush = new SolidBrush(PopupBackground))
                                g.FillPath(brush, pathPopup);
                            g.DrawString(popupText, Font, new SolidBrush(PopupText), rect.X + 1.5f, rect.Y + 1f);
                        }
                    }
                }
            }

            axisPen.Dispose();
            gridPen.Dispose();
            linePen.Dispose();

            DrawLabels(g, chartPadding, width, height);
            base.OnPaint(e);
        }

        private Color PopupBackground => BackColor.R + BackColor.G + BackColor.B > 128 ? Color.FromArgb(64, Color.Black) : Color.FromArgb(64, Color.White);
        private Color PopupText => BackColor.R + BackColor.G + BackColor.B > 128 ? Color.White : Color.Black;

        private void DrawLabels(Graphics g, int padding, int width, int height)
        {
            Brush brush = new SolidBrush(DayColor);
            for (int i = 0; i <= 5; i++)
            {
                float y = padding + height - i * height / 5f;
                string text = usePercent ? $"{Math.Round(i * 20 * privateMaxValue / 100.0, 2)}%" : $"{Math.Round(i * privateMaxValue / 5.0, 2)}";
                g.DrawString(text, Font, brush, padding - 35, y - 7f);
            }

            if (privateCustomXAxis.Length != 0)
            {
                for (int i = 0; i < privateCustomXAxis.Length; i++)
                {
                    float x = padding + i * width / (privateCustomXAxis.Length - 1);
                    g.DrawString(privateCustomXAxis[i], Font, brush, x - 20f, padding + height + 5f);
                }
            }
            else
            {
                for (int i = 0; i < xLabelsLong.Length; i++)
                {
                    float x = padding + i * width / (xLabelsLong.Length - 1);
                    string label = ShortDates ? xLabelsShort[i] : xLabelsLong[i];
                    g.DrawString(label, Font, brush, x - 20f, padding + height + 5f);
                }
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            int width = Width - chartPadding * 2;
            int height = Height - chartPadding * 2;
            float scale = 100f / privateMaxValue;

            showPopup = false;
            for (int i = 0; i < privateDataPoints.Length; i++)
            {
                float x = chartPadding + i * width / (privateDataPoints.Length - 1);
                float y = chartPadding + height - privateDataPoints[i] * scale / 100f * height;

                if (Math.Abs(e.X - x) < 10 && Math.Abs(e.Y - y) < 10)
                {
                    showPopup = true;
                    if (!popupLocation.Equals(new PointF(x, y)))
                    {
                        popupLocation = new PointF(x, y);
                        popupText = privateDataPoints[i] + (usePercent ? "%" : "");
                        InvalidatePopupRegion();
                    }
                    break;
                }
                else
                {
                    InvalidatePopupRegion();
                    showPopup = false;
                }
            }
        }

        private void InvalidatePopupRegion()
        {
            using (Graphics g = CreateGraphics())
            {
                SizeF size = g.MeasureString(popupText, Font);
                RectangleF rect = new RectangleF(popupLocation.X - size.Width / 2f, popupLocation.Y - size.Height - 10f, size.Width, size.Height + 1f);
                Invalidate(new Region(UIGraphicsHelper.RoundRect(rect, (int)(rect.Height / 4f))));
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
            SuspendLayout();
            // 
            // LineChartControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Margin = new Padding(4, 5, 4, 5);
            Name = "LineChartControl";
            Size = new Size(660, 454);
            Load += LineChartControl_Load;
            ResumeLayout(false);
        }

        private void LineChartControl_Load(object sender, EventArgs e)
        {

        }
    }

    //internal static class UIGraphicsHelper
    //{
    //    public static Color PrimaryColor = Color.DeepSkyBlue;

    //    public static GraphicsPath RoundRect(RectangleF rect, int radius)
    //    {
    //        GraphicsPath path = new GraphicsPath();
    //        path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
    //        path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
    //        path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
    //        path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
    //        path.CloseFigure();
    //        return path;
    //    }
    //}
}
