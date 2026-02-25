// Cleaned + deobfuscated version of the decompiled control.
// Original type: כ… .ܫ…
//
// What it is:
// - A vertical “task stepper / progress” control (like TaskStepperProgress, but rotated).
// - Steps are stacked vertically; labels are drawn to the right of the step boxes.
// - Completed steps are filled with CompletedColor (+ optional checkmark).
// - Current step is outlined with CompletedColor and uses CurrentTaskForeColor for its label.
// - Future steps are filled with TrackColor.
//
// External dependency (kept as aliases/placeholders):
// - Theme.PrimaryColor
// - BitMapClass.RoundRect(Rectangle, int)
// - GraphicsUtil.Checkmark(Rectangle)

using Ledger.BitUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;


namespace Ledger.ScrollBar
{
    public class VerticalTaskStepperProgress : Control
    {
        private string[] _tasks =
        {
            "Task1",
            "Task2",
            "Task3",
            "Task4"
        };

        private string _longestTaskText = "Task1";

        // In the decompile these were public fields; exposed as properties here.
        private int _tasksProgress = 2;
        private int _lineThickness = 4;

        private bool _showSymbols = true;

        private Color _completedColor = Theme.PrimaryColor;
        private Color _currentTaskForeColor = Color.FromArgb(128, 128, 128);
        private Color _taskForeColor = Color.FromArgb(128, 128, 128);
        private Color _trackColor = Color.FromArgb(64, 128, 128, 128);

        private int _rounding = 10;
        private bool _autoRounding = true;

        private IContainer components;

        public VerticalTaskStepperProgress()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.AllPaintingInWmPaint, true);

            Size = new Size(58, 480);
        }

        [Description("Tasks in text separated by new lines.")]
        public string[] Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value ?? Array.Empty<string>();

                // Decompiled: Aggregate to find longest string by length.
                _longestTaskText = _tasks.Aggregate(
                    "",
                    (max, cur) => max.Length <= cur.Length ? cur : max);

                Invalidate();
            }
        }

        [Description("How many tasks are completed.")]
        public int TasksProgress
        {
            get => _tasksProgress;
            set { _tasksProgress = value; Invalidate(); }
        }

        public int LineThickness
        {
            get => _lineThickness;
            set { _lineThickness = value; Invalidate(); }
        }

        [Description("Whether to show the checkmark symbol on the completed tasks.")]
        public bool ShowSymbols
        {
            get => _showSymbols;
            set { _showSymbols = value; Invalidate(); }
        }

        [Description("The primary color of the control, the color of completed tasks and current task.")]
        public Color CompletedColor
        {
            get => _completedColor;
            set { _completedColor = value; Invalidate(); }
        }

        [Description("The color of the text of the current task.")]
        public Color CurrentTaskForeColor
        {
            get => _currentTaskForeColor;
            set { _currentTaskForeColor = value; Invalidate(); }
        }

        [Description("The color of the text for every task other than the current task.")]
        public Color TaskForeColor
        {
            get => _taskForeColor;
            set { _taskForeColor = value; Invalidate(); }
        }

        [Description("The color of the track of the uncompleted tasks.")]
        public Color TrackColor
        {
            get => _trackColor;
            set { _trackColor = value; Invalidate(); }
        }

        public int Rounding
        {
            get => _rounding;
            set { _rounding = value; Invalidate(); }
        }

        public bool AutoRounding
        {
            get => _autoRounding;
            set { _autoRounding = value; Invalidate(); }
        }

        [Description("Read-only. Returns the name of the current task (as string).")]
        public string CurrentTask
        {
            get
            {
                try { return Tasks[TasksProgress - 1]; }
                catch { return string.Empty; }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Tasks == null || Tasks.Length < 2)
                return;

            // Width reserved for labels (based on the longest task string).
            int labelWidth = (int)(e.Graphics.MeasureString(_longestTaskText, Font).Width + 0.5);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            // These variables match the decompiled math (renamed for readability).
            int availableWidthForSteps = Width - 1 - labelWidth; // num2
            int leftPadding = availableWidthForSteps / 8;        // x1
            int halfLeftPadding = leftPadding / 2;               // x2
            int stepSize = availableWidthForSteps - leftPadding; // num3

            int count = Tasks.Length;
            int stepSpacing = (Height - stepSize * 2) / (count - 1); // num4

            // Step origin (top-left of the rounded square)
            Point stepOrigin = new Point(halfLeftPadding, stepSize);

            // Label anchor point (text is vertically centered via StringFormat.LineAlignment)
            Point labelPoint = new Point(stepSize + leftPadding + 1, 0);

             var fmt = new StringFormat { LineAlignment = StringAlignment.Center };

             Brush completedBrush = new SolidBrush(CompletedColor);
             Brush trackBrush = new SolidBrush(TrackColor);

            int borderRadius = !AutoRounding ? Math.Min(stepSize / 2, _rounding) : stepSize / 2;

            for (int index = 0; index < count; index++)
            {
                // Decompiled Y positioning includes a compensation term:
                //   - index * availableWidthForSteps / Tasks.Length - leftPadding
                stepOrigin.Y = stepSize + index * stepSpacing - index * availableWidthForSteps / Tasks.Length - leftPadding;

                labelPoint.Y = stepOrigin.Y + (stepSize + 1) / 2 + 1;

                bool isCurrent = index == TasksProgress - 1;
                bool isCompleted = index < TasksProgress;

                if (isCurrent)
                {
                    // Current step: outline only
                    var rect = new Rectangle(
                        leftPadding + 1,
                        stepOrigin.Y + halfLeftPadding / 2 + 1,
                        stepSize - halfLeftPadding - 2,
                        stepSize - halfLeftPadding - 2);

                     GraphicsPath outlinePath = BitMapClass.RoundRect(
                        rect,
                        borderRadius - leftPadding / 2 - 1);

                     var outlinePen = new Pen(CompletedColor, LineThickness / 2f - 1f);
                    e.Graphics.DrawPath(outlinePen, outlinePath);

                     var textBrush = new SolidBrush(CurrentTaskForeColor);
                    e.Graphics.DrawString(Tasks[index], Font, textBrush, (PointF)labelPoint, fmt);
                }
                else if (isCompleted)
                {
                    // Completed: filled + optional checkmark
                    var rect = new Rectangle(leftPadding, stepOrigin.Y, stepSize, stepSize);

                     GraphicsPath stepPath = BitMapClass.RoundRect(rect, borderRadius);
                    e.Graphics.FillPath(completedBrush, stepPath);

                    if (ShowSymbols)
                    {
                        rect.Inflate(0, -1);
                        rect.Inflate(-(stepSize / 10), -(stepSize / 10));

                         GraphicsPath checkPath = BitMapClass.RoundRect(rect, borderRadius);
                         var symbolPen = new Pen(BackColor, stepSize / 8f)
                        {
                            StartCap = LineCap.Round,
                            EndCap = LineCap.Round
                        };

                        e.Graphics.DrawPath(symbolPen, checkPath);
                    }

                     var textBrush = new SolidBrush(TaskForeColor);
                    e.Graphics.DrawString(Tasks[index], Font, textBrush, (PointF)labelPoint, fmt);
                }
                else
                {
                    // Future: track fill
                     GraphicsPath stepPath = BitMapClass.RoundRect(
                        new Rectangle(leftPadding, stepOrigin.Y, stepSize, stepSize),
                        borderRadius);

                    e.Graphics.FillPath(trackBrush, stepPath);

                     var textBrush = new SolidBrush(TaskForeColor);
                    e.Graphics.DrawString(Tasks[index], Font, textBrush, (PointF)labelPoint, fmt);
                }

                // Connector line to next step
                if (index != count - 1)
                {
                    Color lineColor = index < TasksProgress - 1 ? CompletedColor : TrackColor;

                     var linePen = new Pen(lineColor, LineThickness / 2f)
                    {
                        StartCap = LineCap.Round,
                        EndCap = LineCap.Round
                    };

                    Point p1 = stepOrigin;
                    p1.X += (stepSize + leftPadding + 1) / 2;
                    p1.Y += leftPadding + stepSize;

                    Point p2 = p1;
                    p2.Y = p1.Y + stepSpacing - availableWidthForSteps / Tasks.Length - availableWidthForSteps - leftPadding * 2 + 1;

                    p1.Y += leftPadding;
                    p2.Y -= leftPadding;

                    e.Graphics.DrawLine(linePen, p1, p2);
                }
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
            components = new Container();
        }
    }
}
