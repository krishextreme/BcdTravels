// Cleaned + deobfuscated version of the decompiled control.
// Original type: כ… .�ܡ…
//
// What it is:
// - A horizontal multi-step “task progress / stepper” control.
// - Draws N rounded squares (steps) with labels under them.
// - Completed steps are filled with CompletedColor (+ optional checkmark symbol).
// - Current step is outlined with CompletedColor and uses CurrentTaskForeColor for its label.
// - Future steps are filled with TrackColor.
//
// External dependency (kept as alias):
// - BitMapClass.RoundRect(Rectangle, int)
// - GraphicsUtil.Checkmark(Rectangle)
// - Theme.PrimaryColor

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
    public class TaskStepperProgress : Control
    {
        private string[] _tasks =
        {
            "Task1",
            "Task2",
            "Task3",
            "Task4"
        };

        // NOTE: These were public fields in the decompile; converted to properties for sanity,
        // while preserving default values/behavior.
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

        public TaskStepperProgress()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.AllPaintingInWmPaint, true);

            Size = new Size(480, 36);
        }

        [Description("Tasks in text separated by new lines.")]
        public string[] Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value ?? Array.Empty<string>();
                Invalidate();
            }
        }

        [Description("How many tasks are completed (1-based for CurrentTask logic).")]
        public int TasksProgress
        {
            get => _tasksProgress;
            set
            {
                _tasksProgress = value;
                Invalidate();
            }
        }

        public int LineThickness
        {
            get => _lineThickness;
            set
            {
                _lineThickness = value;
                Invalidate();
            }
        }

        [Description("Whether to show the checkmark symbol on the completed tasks.")]
        public bool ShowSymbols
        {
            get => _showSymbols;
            set
            {
                _showSymbols = value;
                Invalidate();
            }
        }

        [Description("The primary color of the control, the color of completed tasks and current task.")]
        public Color CompletedColor
        {
            get => _completedColor;
            set
            {
                _completedColor = value;
                Invalidate();
            }
        }

        [Description("The color of the text of the current task.")]
        public Color CurrentTaskForeColor
        {
            get => _currentTaskForeColor;
            set
            {
                _currentTaskForeColor = value;
                Invalidate();
            }
        }

        [Description("The color of the text for every task other than the current task.")]
        public Color TaskForeColor
        {
            get => _taskForeColor;
            set
            {
                _taskForeColor = value;
                Invalidate();
            }
        }

        [Description("The color of the track of the uncompleted tasks.")]
        public Color TrackColor
        {
            get => _trackColor;
            set
            {
                _trackColor = value;
                Invalidate();
            }
        }

        public int Rounding
        {
            get => _rounding;
            set
            {
                _rounding = value;
                Invalidate();
            }
        }

        public bool AutoRounding
        {
            get => _autoRounding;
            set
            {
                _autoRounding = value;
                Invalidate();
            }
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

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            // These variables match the original math (renamed for readability).
            int availableHeight = Height - 1 - Font.Height; // space reserved for the step circles/squares
            int topPadding = availableHeight / 8;           // y1
            int halfTopPadding = topPadding / 2;            // y2
            int stepSize = availableHeight - topPadding;    // num2

            int count = Tasks.Length;

            // Horizontal spacing between steps (based on the decompile formula).
            int stepSpacing = (Width - stepSize * 2) / (count - 1);

            // point1: top-left of step square-ish area (with weird adjustments)
            Point stepOrigin = new Point(stepSize, halfTopPadding);

            // point2: label draw point (centered by StringFormat)
            Point labelPoint = new Point(0, stepSize + topPadding + 1);

             var centerFormat = new StringFormat { Alignment = StringAlignment.Center };

             Brush completedBrush = new SolidBrush(CompletedColor);
            Brush trackBrush = new SolidBrush(TrackColor);

            int borderRadius = !AutoRounding
                ? Math.Min(stepSize / 2, _rounding)
                : stepSize / 2;

            for (int index = 0; index < count; index++)
            {
                // Decompiled X positioning: includes a compensation term "- index * availableHeight / Tasks.Length - topPadding"
                stepOrigin.X = stepSize + index * stepSpacing - index * availableHeight / Tasks.Length - topPadding;

                labelPoint.X = stepOrigin.X + (stepSize + 1) / 2;

                bool isCurrent = index == TasksProgress - 1;
                bool isCompleted = index < TasksProgress;

                if (isCurrent)
                {
                    // Current step: outline only
                    var rect = new Rectangle(
                        stepOrigin.X + halfTopPadding / 2 + 1,
                        topPadding + 1,
                        stepSize - halfTopPadding - 2,
                        stepSize - halfTopPadding - 2);

                     GraphicsPath outlinePath = BitMapClass.RoundRect(
                        rect,
                        borderRadius - topPadding / 2 - 1);

                    var outlinePen = new Pen(CompletedColor, LineThickness / 2f - 1f);
                    e.Graphics.DrawPath(outlinePen, outlinePath);

                     var textBrush = new SolidBrush(CurrentTaskForeColor);
                    e.Graphics.DrawString(Tasks[index], Font, textBrush, (PointF)labelPoint, centerFormat);
                }
                else if (isCompleted)
                {
                    // Completed step: filled + optional checkmark
                    var rect = new Rectangle(stepOrigin.X, topPadding, stepSize, stepSize);

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
                    e.Graphics.DrawString(Tasks[index], Font, textBrush, (PointF)labelPoint, centerFormat);
                }
                else
                {
                    // Future step: track fill
                     GraphicsPath stepPath = BitMapClass.RoundRect(
                        new Rectangle(stepOrigin.X, topPadding, stepSize, stepSize),
                        borderRadius);

                    e.Graphics.FillPath(trackBrush, stepPath);

                     var textBrush = new SolidBrush(TaskForeColor);
                    e.Graphics.DrawString(Tasks[index], Font, textBrush, (PointF)labelPoint, centerFormat);
                }

                // Draw connector line to next step
                if (index != count - 1)
                {
                    Color lineColor = index < TasksProgress - 1 ? CompletedColor : TrackColor;

                     var linePen = new Pen(lineColor, LineThickness / 2f)
                    {
                        StartCap = LineCap.Round,
                        EndCap = LineCap.Round
                    };

                    Point p1 = stepOrigin;
                    p1.Y += (stepSize + topPadding + 1) / 2;
                    p1.X += topPadding + stepSize;

                    // Decompiled uses C# 9 "with" on Point (struct record-like syntax in decompiler output)
                    Point p2 = p1;
                    p2.X = p1.X + stepSpacing - availableHeight / Tasks.Length - availableHeight - topPadding * 2 + 1;

                    p1.X += topPadding;
                    p2.X -= topPadding;

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
