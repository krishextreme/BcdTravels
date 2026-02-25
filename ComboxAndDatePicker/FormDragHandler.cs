using Ledger.Animations;
using Ledger.ColorPicker;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Ledger.ComboxAndDatePicker
{
    // Form drag handler component
    public class FormDragHandler : Component
    {
        private Form targetForm;
        private bool isDragging;
        private Point dragStartPoint;

        public FormDragHandler(IContainer container)
        {
            container?.Add(this);
        }

        public Form TargetForm
        {
            get => targetForm;
            set
            {
                if (targetForm != null)
                {
                    targetForm.MouseDown -= Form_MouseDown;
                    targetForm.MouseMove -= Form_MouseMove;
                    targetForm.MouseUp -= Form_MouseUp;
                }

                targetForm = value;

                if (targetForm != null)
                {
                    targetForm.MouseDown += Form_MouseDown;
                    targetForm.MouseMove += Form_MouseMove;
                    targetForm.MouseUp += Form_MouseUp;
                }
            }
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragStartPoint = e.Location;
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && targetForm != null)
            {
                Point newLocation = targetForm.Location;
                newLocation.X += e.X - dragStartPoint.X;
                newLocation.Y += e.Y - dragStartPoint.Y;
                targetForm.Location = newLocation;
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
    }

    // Control drag handler component
    public class ControlDragHandler : Component
    {
        private Control targetControl;
        private Form parentForm;
        private bool isDragging;
        private Point dragStartPoint;

        public ControlDragHandler(IContainer container)
        {
            container?.Add(this);
        }

        public Control TargetControl
        {
            get => targetControl;
            set
            {
                if (targetControl != null)
                {
                    targetControl.MouseDown -= Control_MouseDown;
                    targetControl.MouseMove -= Control_MouseMove;
                    targetControl.MouseUp -= Control_MouseUp;
                }

                targetControl = value;

                if (targetControl != null)
                {
                    parentForm = targetControl.FindForm();
                    targetControl.MouseDown += Control_MouseDown;
                    targetControl.MouseMove += Control_MouseMove;
                    targetControl.MouseUp += Control_MouseUp;
                }
            }
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && parentForm != null)
            {
                isDragging = true;
                dragStartPoint = targetControl.PointToScreen(e.Location);
            }
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && parentForm != null)
            {
                Point currentPoint = targetControl.PointToScreen(e.Location);
                Point offset = new Point(
                    currentPoint.X - dragStartPoint.X,
                    currentPoint.Y - dragStartPoint.Y
                );
                parentForm.Location = new Point(
                    parentForm.Location.X + offset.X,
                    parentForm.Location.Y + offset.Y
                );
                dragStartPoint = currentPoint;
            }
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
    }

    // Form rounder component
    public class FormRounder : Component
    {
        private Form targetForm;
        private int rounding = 8;
        private Color outlineColor = Color.FromArgb(30, 128, 128, 128);

        public Form TargetForm
        {
            get => targetForm;
            set => targetForm = value;
        }

        public int Rounding
        {
            get => rounding;
            set => rounding = value;
        }

        public Color OutlineColor
        {
            get => outlineColor;
            set => outlineColor = value;
        }
    }

    // Base class for themed controls
    public class ThemedControl : UserControl
    {
        private Color borderColor = Color.FromArgb(34, 34, 34);

        public Color BorderColor
        {
            get => borderColor;
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (borderColor.A > 0)
            {
                using (Pen pen = new Pen(borderColor))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
                }
            }
        }
    }

    // Year picker control
    public class YearPickerControl : ThemedControl
    {
        private DatePickerForm parentForm;
        private FlowLayoutPanel flowPanel;
        private const int ButtonsPerRow = 4;
        private const int YearRange = 12;

        public YearPickerControl(DatePickerForm parent)
        {
            parentForm = parent;
            InitializeControl();
        }

        private void InitializeControl()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.Transparent;

            flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                Padding = new Padding(5)
            };

            Controls.Add(flowPanel);
            UpdateYearButtons();
        }

        public void UpdateYearButtons()
        {
            flowPanel.Controls.Clear();

            int currentYear = parentForm.Value.Year;
            int startYear = currentYear - (YearRange / 2);

            for (int i = 0; i < YearRange; i++)
            {
                int year = startYear + i;
                CuiButton yearButton = CreateYearButton(year, year == currentYear);
                flowPanel.Controls.Add(yearButton);
            }
        }

        private CuiButton CreateYearButton(int year, bool isSelected)
        {
            CuiButton button = new CuiButton
            {
                Content = year.ToString(),
                Size = new Size(85, 40),
                Margin = new Padding(3),
                Font = new Font("Microsoft Sans Serif", 9.75f),
                ForeColor = isSelected ? Color.White : Color.Gray,
                NormalBackground = isSelected ? Color.FromArgb(255, 106, 0) : Color.Transparent,
                HoverBackground = Color.FromArgb(200, 255, 106, 0),
                Cursor = Cursors.Hand,
                Tag = year
            };

            button.Click += (s, e) =>
            {
                int selectedYear = (int)((CuiButton)s).Tag;
                parentForm.SetYear(selectedYear);
            };

            return button;
        }
    }

    // Month and day picker control
    public class MonthDayPickerControl : ThemedControl
    {
        private DatePickerForm parentForm;
        private Panel monthPanel;
        private Panel dayPanel;
        private Label monthLabel;

        private static readonly string[] MonthNames = {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        };

        public MonthDayPickerControl(DatePickerForm parent)
        {
            parentForm = parent;
            InitializeControl();
        }

        private void InitializeControl()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.Transparent;

            // Month label
            monthLabel = new Label
            {
                Text = parentForm.Value.ToString("MMMM yyyy"),
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold)
            };
            Controls.Add(monthLabel);

            // Month panel
            monthPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                Padding = new Padding(5)
            };
            Controls.Add(monthPanel);

            // Day panel
            dayPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(5)
            };
            Controls.Add(dayPanel);

            UpdateMonthButtons();
            UpdateDayButtons();
        }

        private void UpdateMonthButtons()
        {
            monthPanel.Controls.Clear();

            FlowLayoutPanel flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = true
            };

            int currentMonth = parentForm.Value.Month;

            for (int month = 1; month <= 12; month++)
            {
                int m = month;
                CuiButton monthButton = new CuiButton
                {
                    Content = MonthNames[month - 1],
                    Size = new Size(60, 30),
                    Margin = new Padding(2),
                    Font = new Font("Microsoft Sans Serif", 8.25f),
                    ForeColor = month == currentMonth ? Color.White : Color.Gray,
                    NormalBackground = month == currentMonth ? Color.FromArgb(255, 106, 0) : Color.Transparent,
                    HoverBackground = Color.FromArgb(200, 255, 106, 0),
                    Cursor = Cursors.Hand
                };

                monthButton.Click += (s, e) =>
                {
                    parentForm.SetDayMonth(parentForm.Value.Day, m);
                    UpdateMonthButtons();
                    UpdateDayButtons();
                };

                flowPanel.Controls.Add(monthButton);
            }

            monthPanel.Controls.Add(flowPanel);
        }

        public void UpdateDayButtons()
        {
            dayPanel.Controls.Clear();
            monthLabel.Text = parentForm.Value.ToString("MMMM yyyy");

            FlowLayoutPanel flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = true
            };

            // Add day of week headers
            string[] dayHeaders = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            foreach (string header in dayHeaders)
            {
                Label headerLabel = new Label
                {
                    Text = header,
                    Size = new Size(50, 20),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold),
                    Margin = new Padding(1)
                };
                flowPanel.Controls.Add(headerLabel);
            }

            // Get first day of month
            DateTime firstDay = new DateTime(parentForm.Value.Year, parentForm.Value.Month, 1);
            int startDayOfWeek = (int)firstDay.DayOfWeek;

            // Add empty spaces for days before month starts
            for (int i = 0; i < startDayOfWeek; i++)
            {
                flowPanel.Controls.Add(new Label { Size = new Size(50, 40), Margin = new Padding(1) });
            }

            // Add day buttons
            int daysInMonth = DateTime.DaysInMonth(parentForm.Value.Year, parentForm.Value.Month);
            int currentDay = parentForm.Value.Day;

            for (int day = 1; day <= daysInMonth; day++)
            {
                int d = day;
                CuiButton dayButton = new CuiButton
                {
                    Content = day.ToString(),
                    Size = new Size(50, 40),
                    Margin = new Padding(1),
                    Font = new Font("Microsoft Sans Serif", 9f),
                    ForeColor = day == currentDay ? Color.White : Color.Gray,
                    NormalBackground = day == currentDay ? Color.FromArgb(255, 106, 0) : Color.Transparent,
                    HoverBackground = Color.FromArgb(200, 255, 106, 0),
                    Cursor = Cursors.Hand
                };

                dayButton.Click += (s, e) =>
                {
                    parentForm.SetDayMonth(d, parentForm.Value.Month);
                    UpdateDayButtons();
                };

                flowPanel.Controls.Add(dayButton);
            }

            dayPanel.Controls.Add(flowPanel);
        }
    }

    // CuiButton - already defined in ColorPickerForm artifact
    // CuiLabel - already defined in ColorPickerForm artifact
    // Using the same implementations

    public enum ThemeOptions
    {
        Dark,
        Light,
    }
}