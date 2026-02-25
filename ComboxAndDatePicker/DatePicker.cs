using Ledger.Animations;
using Ledger.ColorPicker;
using Ledger.ComboxAndDatePicker;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Ledger.ComboxAndDatePicker
{
    // was \uFFFD\uFFFDᚨΕ\uFFFDܢܒ\uD802\uDD12\uFFFD\uFFFD unicode
    public class DatePickerForm : Form
        {
            private DateTime privateValue;

            // was ᚑ\uFFFD\uFFFD\uFFFDSΧ\uFFFDᛒᚎᛉGט unicode
            private CalendarYearPickerPage yearPickerControl;

            // was Ω\uFFFDL\uFFFD\uFFFD\uFFFDל\uFFFDר\uFFFDᛟ unicode
            private CalendarMonthDayPickerPage monthDayPickerControl;

            // was \uFFFD\uFFFDᚨΕ\uFFFDܢܒ\uD802\uDD12\uFFFD\uFFFD.ᚓ\uFFFDᛊ\uFFFD\uFFFDᚾ\uFFFDΣᛁ\uFFFD\uFFFD\uFFFD\uFFFDS unicode
            private DatePickerForm.UiTheme privateTheme;

            private IContainer components;

            // was \uFFFD\uFFFD\uFFFDᚹ\uFFFD\uFFFD\uFFFDY\uFFFD\uFFFD unicode
            private CuiFormDrag cuiFormDrag1;

            // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD unicode
            private CuiButton cuiButton4;

            // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD unicode
            private CuiButton cuiButton1;

            // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD unicode
            private CuiButton cuiButton3;

            // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD unicode
            private CuiButton cuiButton2;

            // was ܣ\uFFFDᚍ\uFFFDΑCᚒQ\uFFFDרW\uFFFD unicode
            private CuiLabel cuiLabel3;

            private Panel pagePanel;

            // was Z\uD800\uDC2D\uFFFD\uFFFDᚒ\uFFFDבבΚ\uFFFDΥHܣQ\uFFFDܦ unicode
            private CuiControlDrag cuiControlDrag1;

            // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD unicode
            private CuiButton cuiButton5;

            // was \uFFFD\uFFFDᚌᛃZ\uFFFDܡ\uFFFD\uFFFD\uFFFD unicode
            public CuiFormRounder cuiFormRounder1;

            public DateTime Value
            {
                get => this.privateValue;
                set
                {
                    this.privateValue = value;
                    this.cuiLabel3.Content = value.ToString("D");
                    this.yearPickerControl?.UpdateYearButtons();
                    this.monthDayPickerControl?.UpdateDayButtons();
                }
            }

            internal void ToggleThemeSwitchButton(bool value) => this.cuiButton4.Visible = value;

            // was \uFFFD\uFFFDᚨΕ\uFFFDܢܒ\uD802\uDD12\uFFFD\uFFFD(DateTime startWithDateTime) unicode
            public DatePickerForm(DateTime startWithDateTime)
            {
                this.InitializeComponent();
                this.yearPickerControl = new CalendarYearPickerPage(this);       // was ᚑ\uFFFD\uFFFD\uFFFDSΧ\uFFFDᛒᚎᛉGט unicode
                this.monthDayPickerControl = new CalendarMonthDayPickerPage(this); // was Ω\uFFFDL\uFFFD\uFFFD\uFFFDל\uFFFDר\uFFFDᛟ unicode
                this.Value = startWithDateTime;
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                this.pagePanel.Controls.Add((Control)this.monthDayPickerControl);
            }

            private void SetPage(UserControl pageControl)
            {
                this.pagePanel.Controls.Clear();
                this.pagePanel.Controls.Add((Control)pageControl);
            }

            private void cuiButton3_Click(object sender, EventArgs e)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }

            private void cuiButton2_Click(object sender, EventArgs e)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

            internal void SetYear(int year, bool returnToChoosingDay = true)
            {
                DateTime dateTime = this.Value;
                int day1 = dateTime.Day;
                int year1 = year;
                dateTime = this.Value;
                int month = dateTime.Month;
                int val2 = DateTime.DaysInMonth(year1, month);
                int day2 = Math.Min(day1, val2);
                this.Value = new DateTime(year, this.Value.Month, day2);
                if (!returnToChoosingDay)
                    return;
                this.SetPage((UserControl)this.monthDayPickerControl);
            }

            internal void SetDayMonth(int day, int month)
            {
                int day1 = Math.Min(day, DateTime.DaysInMonth(this.Value.Year, month));
                this.Value = new DateTime(this.Value.Year, month, day1);
            }

            private void cuiButton1_Click(object sender, EventArgs e)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }

            private void cuiButton5_Click(object sender, EventArgs e)
            {
                if (this.pagePanel.Controls[0] == this.monthDayPickerControl)
                    this.SetPage((UserControl)this.yearPickerControl);
                else
                    this.SetPage((UserControl)this.monthDayPickerControl);
            }

            // was \uFFFD\uFFFDᚨΕ\uFFFDܢܒ\uD802\uDD12\uFFFD\uFFFD.ᚓ\uFFFDᛊ\uFFFD\uFFFDᚾ\uFFFDΣᛁ\uFFFD\uFFFD\uFFFD\uFFFDS Theme unicode
            public DatePickerForm.UiTheme Theme
            {
                get => this.privateTheme;
                set
                {
                    this.privateTheme = value;
                    this.SuspendLayout();
                    switch (value)
                    {
                        case DatePickerForm.UiTheme.Dark:
                            this.BackColor = Color.Black;
                            foreach (Control control in (ArrangedElementCollection)this.Controls)
                            {
                                // was \uFFFD\uFFFDWᚃב\uFFFD\uD808\uDF07\uFFFD\uFFFDח\uFFFD\uFFFD unicode
                                if (control is CuiBorderedPanel wᚃבח)
                                {
                                    wᚃבח.ForeColor = SystemColors.ButtonFace;
                                    ((Control)wᚃבח).BackColor = Color.Black;
                                    wᚃבח.BorderColor = Color.FromArgb(34, 34, 34);
                                }
                                else if (control is CuiLabel ܣᚍΑcᚒQרW && ܣᚍΑcᚒQרW != this.cuiLabel3)
                                    ܣᚍΑcᚒQרW.ForeColor = Color.White;
                            }
                            this.cuiButton3.NormalOutline = Color.FromArgb(20, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
                            this.cuiButton2.NormalBackground = Color.FromArgb(20, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
                            this.cuiButton2.ForeColor = Color.White;
                            this.cuiButton1.NormalBackground = Color.FromArgb(20, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
                            this.cuiButton1.NormalImageTint = Color.White;
                            this.cuiButton1.HoveredImageTint = this.cuiButton1.NormalImageTint;
                            this.cuiButton1.PressedImageTint = this.cuiButton1.NormalImageTint;
                            this.cuiFormRounder1.OutlineColor = Color.FromArgb(30, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
                            this.cuiLabel3.ForeColor = Color.FromArgb(171, 171, 171);
                            break;

                        case DatePickerForm.UiTheme.Light:
                            this.BackColor = SystemColors.Control;
                            foreach (Control control in (ArrangedElementCollection)this.Controls)
                            {
                                // was \uFFFD\uFFFDWᚃב\uFFFD\uD808\uDF07\uFFFD\uFFFDח\uFFFD\uFFFD unicode
                                if (control is CuiBorderedPanel wᚃבח)
                                {
                                    wᚃבח.ForeColor = Color.Black;
                                    ((Control)wᚃבח).BackColor = SystemColors.Control;
                                    wᚃבח.BorderColor = Color.FromArgb(221, 221, 221);
                                }
                                else if (control is CuiLabel ܣᚍΑcᚒQרW && ܣᚍΑcᚒQרW != this.cuiLabel3)
                                    ܣᚍΑcᚒQרW.ForeColor = Color.Black;
                            }
                            this.cuiButton3.NormalOutline = Color.FromArgb(20, 0, 0, 0);
                            this.cuiButton2.NormalBackground = Color.FromArgb(20, 0, 0, 0);
                            this.cuiButton2.ForeColor = Color.Black;
                            this.cuiButton1.NormalBackground = Color.FromArgb(20, 0, 0, 0);
                            this.cuiButton1.NormalImageTint = Color.Black;
                            this.cuiButton1.HoveredImageTint = this.cuiButton1.NormalImageTint;
                            this.cuiButton1.PressedImageTint = this.cuiButton1.NormalImageTint;
                            this.cuiFormRounder1.OutlineColor = Color.FromArgb(30, 0, 0, 0);
                            this.cuiLabel3.ForeColor = Color.FromArgb(84, 84, 84);
                            break;
                    }
                    this.ResumeLayout();
                }
            }

            private void cuiButton4_Click(object sender, EventArgs e)
            {
                if (this.Theme == DatePickerForm.UiTheme.Light)
                    this.Theme = DatePickerForm.UiTheme.Dark;
                else
                    this.Theme = DatePickerForm.UiTheme.Light;
            }

            private void cuiButton2_ForeColorChanged(object sender, EventArgs e)
            {
                this.cuiButton2.HoverForeColor = this.cuiButton2.ForeColor;
                this.cuiButton2.PressedForeColor = this.cuiButton2.ForeColor;
                this.cuiButton3.ForeColor = this.cuiButton2.ForeColor;
                this.cuiButton3.HoverForeColor = this.cuiButton2.ForeColor;
                this.cuiButton3.PressedForeColor = this.cuiButton2.ForeColor;
                this.cuiButton2.NormalImageTint = this.cuiButton2.ForeColor;
                this.cuiButton3.NormalImageTint = this.cuiButton2.ForeColor;
                this.cuiButton2.HoveredImageTint = this.cuiButton2.ForeColor;
                this.cuiButton3.HoveredImageTint = this.cuiButton2.ForeColor;
                this.cuiButton2.PressedImageTint = this.cuiButton2.ForeColor;
                this.cuiButton3.PressedImageTint = this.cuiButton2.ForeColor;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing && this.components != null)
                    this.components.Dispose();
                base.Dispose(disposing);
            }

            private void InitializeComponent()
            {
                this.components = (IContainer)new System.ComponentModel.Container();
                ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DatePickerForm)); // was typeof(\uFFFD\uFFFDᚨΕ...) unicode

                this.pagePanel = new Panel();
                this.cuiButton5 = new CuiButton();  // was \uD802\uDD13... unicode
                this.cuiButton3 = new CuiButton();  // was \uD802\uDD13... unicode
                this.cuiButton2 = new CuiButton();  // was \uD802\uDD13... unicode
                this.cuiButton4 = new CuiButton();  // was \uD802\uDD13... unicode
                this.cuiButton1 = new CuiButton();  // was \uD802\uDD13... unicode
                this.cuiLabel3 = new CuiLabel();    // was ܣ\uFFFD... unicode
                this.cuiFormRounder1 = new CuiFormRounder(); // was \uFFFD\uFFFDᚌᛃZ... unicode
                this.cuiFormDrag1 = new CuiFormDrag(this.components); // was \uFFFD\uFFFD\uFFFDᚹ... unicode
                this.cuiControlDrag1 = new CuiControlDrag(this.components); // was Z\uD800\uDC2D... unicode

                this.SuspendLayout();

                this.pagePanel.BackColor = Color.Transparent;
                this.pagePanel.Location = new Point(11, 50);
                this.pagePanel.Name = "pagePanel";
                this.pagePanel.Size = new Size(376, 196);
                this.pagePanel.TabIndex = 26;

                this.cuiButton5.CheckButton = false;
                this.cuiButton5.Checked = false;
                this.cuiButton5.CheckedBackground = Color.FromArgb((int)byte.MaxValue, 106, 0);
                this.cuiButton5.CheckedForeColor = Color.Empty;
                this.cuiButton5.CheckedImageTint = Color.Empty;
                this.cuiButton5.CheckedOutline = Color.Empty;
                this.cuiButton5.Content = "";
                this.cuiButton5.Cursor = Cursors.Hand;
                this.cuiButton5.DialogResult = DialogResult.None;
                this.cuiButton5.Font = new Font("Microsoft Sans Serif", 9.75f);
                this.cuiButton5.ForeColor = Color.White;
                this.cuiButton5.HoverBackground = Color.Empty;
                this.cuiButton5.HoveredImageTint = Color.Gray;
                this.cuiButton5.HoverForeColor = Color.Empty;
                this.cuiButton5.HoverOutline = Color.Empty;
                this.cuiButton5.Image = (Image)componentResourceManager.GetObject("cuiButton5.Image");
                this.cuiButton5.ImageAutoCenter = true;
                this.cuiButton5.ImageExpand = new Point(2, 2);
                this.cuiButton5.ImageOffset = new Point(0, 0);
                this.cuiButton5.NormalImageTint = Color.Gray;
                this.cuiButton5.Location = new Point(16 /*0x10*/, 10);
                this.cuiButton5.Name = "cuiButton5";
                this.cuiButton5.NormalBackground = Color.Empty;
                this.cuiButton5.NormalOutline = Color.Empty;
                this.cuiButton5.OutlineThickness = 1.6f;
                this.cuiButton5.PressedBackground = Color.Empty;
                this.cuiButton5.PressedForeColor = Color.Empty;
                this.cuiButton5.PressedImageTint = Color.Gray;
                this.cuiButton5.PressedOutline = Color.Empty;
                this.cuiButton5.Rounding = new Padding(8);
                this.cuiButton5.Size = new Size(29, 29);
                this.cuiButton5.TabIndex = 27;
                this.cuiButton5.TextOffset = new Point(0, 0);
                this.cuiButton5.Click += new EventHandler(this.cuiButton5_Click);

                this.cuiButton3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                this.cuiButton3.CheckButton = false;
                this.cuiButton3.Checked = false;
                this.cuiButton3.CheckedBackground = Color.FromArgb((int)byte.MaxValue, 106, 0);
                this.cuiButton3.CheckedForeColor = Color.White;
                this.cuiButton3.CheckedImageTint = Color.White;
                this.cuiButton3.CheckedOutline = Color.FromArgb((int)byte.MaxValue, 106, 0);
                this.cuiButton3.Content = "Cancel";
                this.cuiButton3.DialogResult = DialogResult.None;
                this.cuiButton3.Font = new Font("Microsoft Sans Serif", 9.75f);
                this.cuiButton3.ForeColor = Color.FromArgb(200, 200, 200);
                this.cuiButton3.HoverBackground = Color.Empty;
                this.cuiButton3.HoveredImageTint = Color.FromArgb(200, 200, 200);
                this.cuiButton3.HoverForeColor = Color.White;
                this.cuiButton3.HoverOutline = Color.FromArgb(200, (int)byte.MaxValue, 106, 0);
                this.cuiButton3.Image = (Image)Resources.crossmark;
                this.cuiButton3.ImageAutoCenter = true;
                this.cuiButton3.ImageExpand = new Point(2, 2);
                this.cuiButton3.ImageOffset = new Point(-2, 0);
                this.cuiButton3.NormalImageTint = Color.FromArgb(200, 200, 200);
                this.cuiButton3.Location = new Point(202, 258);
                this.cuiButton3.Name = "cuiButton3";
                this.cuiButton3.NormalBackground = Color.Empty;
                this.cuiButton3.NormalOutline = Color.FromArgb(20, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
                this.cuiButton3.OutlineThickness = 1.6f;
                this.cuiButton3.PressedBackground = Color.FromArgb((int)byte.MaxValue, 106, 0);
                this.cuiButton3.PressedForeColor = Color.White;
                this.cuiButton3.PressedImageTint = Color.FromArgb(200, 200, 200);
                this.cuiButton3.PressedOutline = Color.Empty;
                this.cuiButton3.Rounding = new Padding(8);
                this.cuiButton3.Size = new Size(186, 43);
                this.cuiButton3.TabIndex = 24;
                this.cuiButton3.TextOffset = new Point(0, 0);
                this.cuiButton3.Click += new EventHandler(this.cuiButton3_Click);

                this.cuiButton2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                this.cuiButton2.CheckButton = false;
                this.cuiButton2.Checked = false;
                this.cuiButton2.CheckedBackground = Color.FromArgb((int)byte.MaxValue, 106, 0);
                this.cuiButton2.CheckedForeColor = Color.White;
                this.cuiButton2.CheckedImageTint = Color.White;
                this.cuiButton2.CheckedOutline = Color.FromArgb((int)byte.MaxValue, 106, 0);
                this.cuiButton2.Content = "Done";
                this.cuiButton2.DialogResult = DialogResult.None;
                this.cuiButton2.Font = new Font("Microsoft Sans Serif", 9.75f);
                this.cuiButton2.ForeColor = Color.White;
                this.cuiButton2.HoverBackground = Color.FromArgb(200, (int)byte.MaxValue, 106, 0);
                this.cuiButton2.HoveredImageTint = Color.White;
                this.cuiButton2.HoverForeColor = Color.White;
                this.cuiButton2.HoverOutline = Color.Empty;
                this.cuiButton2.Image = (Image)Resources.yes;
                this.cuiButton2.ImageAutoCenter = true;
                this.cuiButton2.ImageExpand = new Point(2, 2);
                this.cuiButton2.ImageOffset = new Point(-3, 0);
                this.cuiButton2.NormalImageTint = Color.White;
                this.cuiButton2.Location = new Point(10, 258);
                this.cuiButton2.Name = "cuiButton2";
                this.cuiButton2.NormalBackground = Color.FromArgb(20, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
                this.cuiButton2.NormalOutline = Color.Empty;
                this.cuiButton2.OutlineThickness = 1.6f;
                this.cuiButton2.PressedBackground = Color.FromArgb((int)byte.MaxValue, 106, 0);
                this.cuiButton2.PressedForeColor = Color.White;
                this.cuiButton2.PressedImageTint = Color.White;
                this.cuiButton2.PressedOutline = Color.Empty;
                this.cuiButton2.Rounding = new Padding(8);
                this.cuiButton2.Size = new Size(186, 43);
                this.cuiButton2.TabIndex = 23;
                this.cuiButton2.TextOffset = new Point(0, 0);
                this.cuiButton2.ForeColorChanged += new EventHandler(this.cuiButton2_ForeColorChanged);
                this.cuiButton2.Click += new EventHandler(this.cuiButton2_Click);

                this.cuiButton4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                this.cuiButton4.CheckButton = false;
                this.cuiButton4.Checked = false;
                this.cuiButton4.CheckedBackground = Color.Empty;
                this.cuiButton4.CheckedForeColor = Color.White;
                this.cuiButton4.CheckedImageTint = Color.White;
                this.cuiButton4.CheckedOutline = Color.Empty;
                this.cuiButton4.Content = "";
                this.cuiButton4.DialogResult = DialogResult.None;
                this.cuiButton4.Font = new Font("Microsoft Sans Serif", 9.75f);
                this.cuiButton4.ForeColor = Color.White;
                this.cuiButton4.HoverBackground = Color.Empty;
                this.cuiButton4.HoveredImageTint = Color.Gray;
                this.cuiButton4.HoverForeColor = Color.White;
                this.cuiButton4.HoverOutline = Color.Empty;
                this.cuiButton4.Image = (Image)Resources.half_moon;
                this.cuiButton4.ImageAutoCenter = true;
                this.cuiButton4.ImageExpand = new Point(2, 2);
                this.cuiButton4.ImageOffset = new Point(0, 0);
                this.cuiButton4.NormalImageTint = Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/);
                this.cuiButton4.Location = new Point(306, 1);
                this.cuiButton4.Name = "cuiButton4";
                this.cuiButton4.NormalBackground = Color.Empty;
                this.cuiButton4.NormalOutline = Color.Empty;
                this.cuiButton4.OutlineThickness = 1.6f;
                this.cuiButton4.PressedBackground = Color.Empty;
                this.cuiButton4.PressedForeColor = Color.White;
                this.cuiButton4.PressedImageTint = Color.FromArgb(96 /*0x60*/, 96 /*0x60*/, 96 /*0x60*/);
                this.cuiButton4.PressedOutline = Color.Empty;
                this.cuiButton4.Rounding = new Padding(8);
                this.cuiButton4.Size = new Size(43, 43);
                this.cuiButton4.TabIndex = 22;
                this.cuiButton4.TextOffset = new Point(0, 0);
                this.cuiButton4.Click += new EventHandler(this.cuiButton4_Click);

                this.cuiButton1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                this.cuiButton1.CheckButton = false;
                this.cuiButton1.Checked = false;
                this.cuiButton1.CheckedBackground = Color.FromArgb((int)byte.MaxValue, 106, 0);
                this.cuiButton1.CheckedForeColor = Color.White;
                this.cuiButton1.CheckedImageTint = Color.White;
                this.cuiButton1.CheckedOutline = Color.FromArgb((int)byte.MaxValue, 106, 0);
                this.cuiButton1.Content = "";
                this.cuiButton1.DialogResult = DialogResult.None;
                this.cuiButton1.Font = new Font("Microsoft Sans Serif", 9.75f);
                this.cuiButton1.ForeColor = Color.White;
                this.cuiButton1.HoverBackground = Color.FromArgb(200, 130, 130, 130);
                this.cuiButton1.HoveredImageTint = Color.White;
                this.cuiButton1.HoverForeColor = Color.White;
                this.cuiButton1.HoverOutline = Color.Empty;
                this.cuiButton1.Image = (Image)Resources.crossmark;
                this.cuiButton1.ImageAutoCenter = true;
                this.cuiButton1.ImageExpand = new Point(2, 2);
                this.cuiButton1.ImageOffset = new Point(0, 0);
                this.cuiButton1.NormalImageTint = Color.White;
                this.cuiButton1.Location = new Point(355, 1);
                this.cuiButton1.Name = "cuiButton1";
                this.cuiButton1.NormalBackground = Color.FromArgb(20, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
                this.cuiButton1.NormalOutline = Color.Empty;
                this.cuiButton1.OutlineThickness = 1.6f;
                this.cuiButton1.PressedBackground = Color.FromArgb(150, 130, 130, 130);
                this.cuiButton1.PressedForeColor = Color.White;
                this.cuiButton1.PressedImageTint = Color.White;
                this.cuiButton1.PressedOutline = Color.Empty;
                this.cuiButton1.Rounding = new Padding(8);
                this.cuiButton1.Size = new Size(43, 43);
                this.cuiButton1.TabIndex = 21;
                this.cuiButton1.TextOffset = new Point(0, 0);
                this.cuiButton1.Click += new EventHandler(this.cuiButton1_Click);

                this.cuiLabel3.Content = "yyyy,\\ xxxxxxxxx";
                this.cuiLabel3.Font = new Font("Microsoft YaHei UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)238);
                this.cuiLabel3.ForeColor = SystemColors.AppWorkspace;
                this.cuiLabel3.HorizontalAlignment = StringAlignment.Near;
                this.cuiLabel3.Location = new Point(51, 16 /*0x10*/);
                this.cuiLabel3.Name = "cuiLabel3";
                this.cuiLabel3.Size = new Size(261, 15);
                this.cuiLabel3.TabIndex = 25;

                this.cuiFormRounder1.EnhanceCorners = false;
                this.cuiFormRounder1.OutlineColor = Color.FromArgb(30, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
                this.cuiFormRounder1.Rounding = 8;
                this.cuiFormRounder1.TargetForm = (Form)this;

                this.cuiFormDrag1.TargetForm = (Form)this;
                this.cuiControlDrag1.TargetControl = (Control)this.cuiLabel3;

                this.AutoScaleDimensions = new SizeF(6f, 13f);
                this.AutoScaleMode = AutoScaleMode.Font;
                this.BackColor = Color.Black;
                this.ClientSize = new Size(400, 313);

                this.Controls.Add((Control)this.cuiButton5);
                this.Controls.Add((Control)this.pagePanel);
                this.Controls.Add((Control)this.cuiButton3);
                this.Controls.Add((Control)this.cuiButton2);
                this.Controls.Add((Control)this.cuiButton4);
                this.Controls.Add((Control)this.cuiButton1);
                this.Controls.Add((Control)this.cuiLabel3);

                this.FormBorderStyle = FormBorderStyle.None;
                this.Name = "DatePicker";
                this.Text = "DatePicker";
                this.TopMost = true;

                this.ResumeLayout(false);
            }
        public enum UiTheme
        {
            Dark,
            Light,
        }
    }
  

    // ---------- type name mappings only (so this file compiles where needed) ----------
    // You already have real implementations elsewhere; keep them there.

    // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD unicode
    //public class CuiButton : Control
    //{
    //    public bool CheckButton { get; set; }
    //    public bool Checked { get; set; }
    //    public Color CheckedBackground { get; set; }
    //    public Color CheckedForeColor { get; set; }
    //    public Color CheckedImageTint { get; set; }
    //    public Color CheckedOutline { get; set; }
    //    public string Content { get; set; }
    //    public DialogResult DialogResult { get; set; }
    //    public Color HoverBackground { get; set; }
    //    public Color HoveredImageTint { get; set; }
    //    public Color HoverForeColor { get; set; }
    //    public Color HoverOutline { get; set; }
    //    public Image Image { get; set; }
    //    public bool ImageAutoCenter { get; set; }
    //    public Point ImageExpand { get; set; }
    //    public Point ImageOffset { get; set; }
    //    public Color NormalImageTint { get; set; }
    //    public Color NormalBackground { get; set; }
    //    public Color NormalOutline { get; set; }
    //    public float OutlineThickness { get; set; }
    //    public Color PressedBackground { get; set; }
    //    public Color PressedForeColor { get; set; }
    //    public Color PressedImageTint { get; set; }
    //    public Color PressedOutline { get; set; }
    //    public Padding Rounding { get; set; }
    //    public Point TextOffset { get; set; }
    //    public Color HoverForeColor2 { get; set; }
    //    public Color PressedForeColor2 { get; set; }

    //    public Color HoverForeColor { get; set; }
    //    public Color PressedForeColor { get; set; }
    //}

    // was ܣ\uFFFDᚍ\uFFFDΑCᚒQ\uFFFDרW\uFFFD unicode
    //public class CuiLabel : Control
    //{
    //    public string Content { get; set; }
    //    public StringAlignment HorizontalAlignment { get; set; }
    //}

    // was \uFFFD\uFFFDᚌᛃZ\uFFFDܡ\uFFFD\uFFFD\uFFFD unicode
    //public class CuiFormRounder : Component
    //{
    //    public bool EnhanceCorners { get; set; }
    //    public Color OutlineColor { get; set; }
    //    public int Rounding { get; set; }
    //    public Form TargetForm { get; set; }
    //}

    // was \uFFFD\uFFFD\uFFFDᚹ\uFFFD\uFFFD\uFFFDY\uFFFD\uFFFD unicode
    public class CuiFormDrag : Component
        {
            public CuiFormDrag(IContainer container) { }
            public Form TargetForm { get; set; }
        }

        // was Z\uD800\uDC2D\uFFFD\uFFFDᚒ\uFFFDבבΚ\uFFFDΥHܣQ\uFFFDܦ unicode
        public class CuiControlDrag : Component
        {
            public CuiControlDrag(IContainer container) { }
            public Control TargetControl { get; set; }
        }

        // was \uFFFD\uFFFDWᚃב\uFFFD\uD808\uDF07\uFFFD\uFFFDח\uFFFD\uFFFD unicode
        public class CuiBorderedPanel : Control
        {
            public Color BorderColor { get; set; }
        }

        // was ᚑ\uFFFD\uFFFD\uFFFDSΧ\uFFFDᛒᚎᛉGט unicode
        public class CalendarYearPickerPage : UserControl
        {
            public CalendarYearPickerPage(DatePickerForm owner) { }
            public void UpdateYearButtons() { }
        }

        // was Ω\uFFFDL\uFFFD\uFFFD\uFFFDל\uFFFDר\uFFFDᛟ unicode
        public class CalendarMonthDayPickerPage : UserControl
        {
            public CalendarMonthDayPickerPage(DatePickerForm owner) { }
            public void UpdateDayButtons() { }
        }
    }



//public class DatePickerForm : Form
//{
//    private DateTime selectedDate;
//    private YearPickerControl yearPickerControl;
//    private MonthDayPickerControl monthDayPickerControl;
//    private ThemeOptions currentTheme;
//    private IContainer components;
//    private FormDragHandler formDragHandler;
//    private CuiButton cancelButton;
//    private CuiButton doneButton;
//    private CuiButton toggleThemeButton;
//    private CuiButton switchButton;
//    private CuiLabel dateLabel;
//    private Panel pagePanel;
//    private ControlDragHandler controlDragHandler;
//    private CuiButton toggleButton;
//    public FormRounder formRounder;

//    public DateTime Value
//    {
//        get => selectedDate;
//        set
//        {
//            selectedDate = value;
//            dateLabel.Content = value.ToString("D");
//            yearPickerControl?.UpdateYearButtons();
//            monthDayPickerControl?.UpdateDayButtons();
//        }
//    }

//    internal void ToggleThemeSwitchButton(bool value) => toggleThemeButton.Visible = value;

//    public DatePickerForm(DateTime startWithDateTime)
//    {
//        InitializeComponent();
//        yearPickerControl = new YearPickerControl(this);
//        monthDayPickerControl = new MonthDayPickerControl(this);
//        Value = startWithDateTime;
//        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
//        SetStyle(ControlStyles.UserPaint, true);
//        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
//        pagePanel.Controls.Add(monthDayPickerControl);
//    }

//    private void SetPage(UserControl pageControl)
//    {
//        pagePanel.Controls.Clear();
//        pagePanel.Controls.Add(pageControl);
//    }

//    private void cancelButton_Click(object sender, EventArgs e)
//    {
//        DialogResult = DialogResult.Cancel;
//        Close();
//    }

//    private void doneButton_Click(object sender, EventArgs e)
//    {
//        DialogResult = DialogResult.OK;
//        Close();
//    }

//    internal void SetYear(int year, bool returnToChoosingDay = true)
//    {
//        DateTime dateTime = Value;
//        int day = dateTime.Day;
//        int month = dateTime.Month;
//        int daysInMonth = DateTime.DaysInMonth(year, month);
//        int adjustedDay = Math.Min(day, daysInMonth);
//        Value = new DateTime(year, month, adjustedDay);
//        if (returnToChoosingDay)
//            this.SetPage(monthDayPickerControl);
//    }

//    internal void SetDayMonth(int day, int month)
//    {
//        int adjustedDay = Math.Min(day, DateTime.DaysInMonth(Value.Year, month));
//        Value = new DateTime(Value.Year, month, adjustedDay);
//    }

//    private void switchButton_Click(object sender, EventArgs e)
//    {
//        DialogResult = DialogResult.Cancel;
//        Close();
//    }

//    private void toggleButton_Click(object sender, EventArgs e)
//    {
//        if (pagePanel.Controls[0] == monthDayPickerControl)
//            this.SetPage(yearPickerControl);
//        else
//            this.SetPage(monthDayPickerControl);
//    }

//    public ThemeOptions Theme
//    {
//        get => currentTheme;
//        set
//        {
//            currentTheme = value;
//            SuspendLayout();
//            switch (value)
//            {
//                case ThemeOptions.Dark:
//                    BackColor = Color.Black;
//                    foreach (Control control in Controls)
//                    {
//                        if (control is ThemedControl themedControl)
//                        {
//                            themedControl.ForeColor = SystemColors.ButtonFace;
//                            themedControl.BackColor = Color.Black;
//                            themedControl.BorderColor = Color.FromArgb(34, 34, 34);
//                        }
//                        else if (control is CuiLabel customLabel && customLabel != dateLabel)
//                            customLabel.ForeColor = Color.White;
//                    }
//                    cancelButton.NormalOutline = Color.FromArgb(20, byte.MaxValue, byte.MaxValue, byte.MaxValue);
//                    doneButton.NormalBackground = Color.FromArgb(20, byte.MaxValue, byte.MaxValue, byte.MaxValue);
//                    doneButton.ForeColor = Color.White;
//                    switchButton.NormalBackground = Color.FromArgb(20, byte.MaxValue, byte.MaxValue, byte.MaxValue);
//                    switchButton.NormalImageTint = Color.White;
//                    switchButton.HoveredImageTint = switchButton.NormalImageTint;
//                    switchButton.PressedImageTint = switchButton.NormalImageTint;
//                    formRounder.OutlineColor = Color.FromArgb(30, byte.MaxValue, byte.MaxValue, byte.MaxValue);
//                    dateLabel.ForeColor = Color.FromArgb(171, 171, 171);
//                    break;

//                case ThemeOptions.Light:
//                    BackColor = SystemColors.Control;
//                    foreach (Control control in Controls)
//                    {
//                        if (control is ThemedControl themedControl)
//                        {
//                            themedControl.ForeColor = Color.Black;
//                            themedControl.BackColor = SystemColors.Control;
//                            themedControl.BorderColor = Color.FromArgb(221, 221, 221);
//                        }
//                        else if (control is CuiLabel customLabel && customLabel != dateLabel)
//                            customLabel.ForeColor = Color.Black;
//                    }
//                    cancelButton.NormalOutline = Color.FromArgb(20, 0, 0, 0);
//                    doneButton.NormalBackground = Color.FromArgb(20, 0, 0, 0);
//                    doneButton.ForeColor = Color.Black;
//                    switchButton.NormalBackground = Color.FromArgb(20, 0, 0, 0);
//                    switchButton.NormalImageTint = Color.Black;
//                    switchButton.HoveredImageTint = switchButton.NormalImageTint;
//                    switchButton.PressedImageTint = switchButton.NormalImageTint;
//                    formRounder.OutlineColor = Color.FromArgb(30, 0, 0, 0);
//                    dateLabel.ForeColor = Color.FromArgb(84, 84, 84);
//                    break;
//            }
//            ResumeLayout();
//        }
//    }

//    private void toggleThemeButton_Click(object sender, EventArgs e)
//    {
//        Theme = Theme == ThemeOptions.Light ? ThemeOptions.Dark : ThemeOptions.Light;
//    }

//    private void doneButton_ForeColorChanged(object sender, EventArgs e)
//    {
//        doneButton.HoverForeColor = doneButton.ForeColor;
//        doneButton.PressedForeColor = doneButton.ForeColor;
//        cancelButton.ForeColor = doneButton.ForeColor;
//        cancelButton.HoverForeColor = doneButton.ForeColor;
//        cancelButton.PressedForeColor = doneButton.ForeColor;
//        doneButton.NormalImageTint = doneButton.ForeColor;
//        cancelButton.NormalImageTint = doneButton.ForeColor;
//        doneButton.HoveredImageTint = doneButton.ForeColor;
//        cancelButton.HoveredImageTint = doneButton.ForeColor;
//        doneButton.PressedImageTint = doneButton.ForeColor;
//        cancelButton.PressedImageTint = doneButton.ForeColor;
//    }

//    protected override void Dispose(bool disposing)
//    {
//        if (disposing && components != null)
//            components.Dispose();
//        base.Dispose(disposing);
//    }

//    private void InitializeComponent()
//    {
//        components = new Container();
//        ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DatePickerForm));
//        pagePanel = new Panel();
//        toggleButton = new CuiButton();
//        cancelButton = new CuiButton();
//        doneButton = new CuiButton();
//        toggleThemeButton = new CuiButton();
//        switchButton = new CuiButton();
//        dateLabel = new CuiLabel();
//        formRounder = new FormRounder();
//        formDragHandler = new FormDragHandler(components);
//        controlDragHandler = new ControlDragHandler(components);
//        SuspendLayout();
//        pagePanel.BackColor = Color.Transparent;
//        pagePanel.Location = new Point(11, 50);
//        pagePanel.Name = "pagePanel";
//        pagePanel.Size = new Size(376, 196);
//        pagePanel.TabIndex = 26;

//        // Toggle Button
//        toggleButton.CheckButton = false;
//        toggleButton.Checked = false;
//        toggleButton.Content = "";
//        toggleButton.Cursor = Cursors.Hand;
//        toggleButton.Font = new Font("Microsoft Sans Serif", 9.75f);
//        toggleButton.ForeColor = Color.White;
//        toggleButton.Location = new Point(16, 10);
//        toggleButton.Name = "toggleButton";
//        toggleButton.Size = new Size(29, 29);
//        toggleButton.Click += new EventHandler(toggleButton_Click);

//        // Cancel Button
//        cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
//        cancelButton.CheckButton = false;
//        cancelButton.Checked = false;
//        cancelButton.Content = "Cancel";
//        cancelButton.Font = new Font("Microsoft Sans Serif", 9.75f);
//        cancelButton.ForeColor = Color.FromArgb(200, 200, 200);
//        cancelButton.Location = new Point(202, 258);
//        cancelButton.Name = "cancelButton";
//        cancelButton.Size = new Size(186, 43);
//        cancelButton.Click += new EventHandler(cancelButton_Click);

//        // Done Button
//        doneButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
//        doneButton.CheckButton = false;
//        doneButton.Checked = false;
//        doneButton.Content = "Done";
//        doneButton.Font = new Font("Microsoft Sans Serif", 9.75f);
//        doneButton.ForeColor = Color.White;
//        doneButton.Location = new Point(10, 258);
//        doneButton.Name = "doneButton";
//        doneButton.Size = new Size(186, 43);
//        doneButton.Click += new EventHandler(doneButton_Click);

//        // Theme Toggle Button
//        toggleThemeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
//        toggleThemeButton.CheckButton = false;
//        toggleThemeButton.Checked = false;
//        toggleThemeButton.Content = "";
//        toggleThemeButton.Font = new Font("Microsoft Sans Serif", 9.75f);
//        toggleThemeButton.ForeColor = Color.White;
//        toggleThemeButton.Location = new Point(306, 1);
//        toggleThemeButton.Name = "toggleThemeButton";
//        toggleThemeButton.Size = new Size(43, 43);
//        toggleThemeButton.Click += new EventHandler(toggleThemeButton_Click);

//        // Switch Button
//        switchButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
//        switchButton.CheckButton = false;
//        switchButton.Checked = false;
//        switchButton.Content = "";
//        switchButton.Font = new Font("Microsoft Sans Serif", 9.75f);
//        switchButton.ForeColor = Color.White;
//        switchButton.Location = new Point(355, 1);
//        switchButton.Name = "switchButton";
//        switchButton.Size = new Size(43, 43);
//        switchButton.Click += new EventHandler(switchButton_Click);

//        // Date Label
//        dateLabel.Content = "yyyy,\\ xxxxxxxxx";
//        dateLabel.Font = new Font("Microsoft YaHei UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 238);
//        dateLabel.ForeColor = SystemColors.AppWorkspace;
//        dateLabel.Location = new Point(51, 16);
//        dateLabel.Name = "dateLabel";
//        dateLabel.Size = new Size(261, 15);
//        dateLabel.TabIndex = 25;

//        // Form Settings
//        BackColor = Color.Black;
//        ClientSize = new Size(400, 313);
//        Controls.Add(toggleButton);
//        Controls.Add(pagePanel);
//        Controls.Add(cancelButton);
//        Controls.Add(doneButton);
//        Controls.Add(toggleThemeButton);
//        Controls.Add(switchButton);
//        Controls.Add(dateLabel);
//        FormBorderStyle = FormBorderStyle.None;
//        Name = "DatePicker";
//        Text = "DatePicker";
//        TopMost = true;
//        ResumeLayout(false);
//    }

//    public enum ThemeOptions
//    {
//        Dark,
//        Light,
//    }
//}