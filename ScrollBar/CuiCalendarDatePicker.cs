using Ledger.ComboxAndDatePicker;
using Ledger.FileGenerator;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Web.Configuration;
using System.Windows.Forms;

namespace Ledger.ScrollBar
{
    // DatePicker theme enum
   

    public class CuiCalendarDatePicker : UserControl
    {
        public enum DatePickerUiTheme
        {
            Dark,
            Light
        }
        private UITheme privateTheme = UITheme.Light;
        private bool privateEnableThemeChangeButton = true;

        // was \uFFFD\uFFFDᚨΕ\uFFFDܢܒ\uD802\uDD12\uFFFD\uFFFD unicode
        private DatePickerForm _PickerForm;

        public bool isDialogVisible;
        private int privateRounding = 8;
        private Color privateBackgroundColor = Color.FromArgb(32, 128, 128, 128);
        private Color privateHoverBackground = Color.FromArgb(50, 128, 128, 128);
        private Color privatePressedBackground = Color.FromArgb(80, 128, 128, 128);
        private Color privateNormalOutline = Color.FromArgb(150, 128, 128, 128);
        private Color privateHoverOutline = Color.FromArgb(180, 128, 128, 128);
        private Color privatePressedOutline = Color.FromArgb(210, 128, 128, 128);
        private float privateOutlineThickness = 1.5f;
        private bool privateShowIcon = true;

        //new Resources 
        private Image privateIcon = (Image)Ledger.FileGenerator.Resources.Calendar;
        private Color privateImageTint = Color.Gray;
        private DateTime privateValue = DateTime.Now.Date;

        private StringFormat stringFormat = new StringFormat()
        {
            Alignment = StringAlignment.Center
        };

        private bool isHovered;
        private bool isPressed;
        private IContainer components;

        public CuiCalendarDatePicker()
        {
            this.InitializeComponent();
            this.ForeColor = Color.Gray;
            this.Font = new Font(this.Font.FontFamily, 9.75f);
            this.Size = new Size(153, 45);
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
        }

        public UITheme Theme
        {
            get => this.privateTheme;
            set => this.privateTheme = value;
        }

        [Description("Lets the USER toggle the theme between Light and Dark with a button.")]
        public bool EnableThemeChangeButton
        {
            get => this.privateEnableThemeChangeButton;
            set
            {
                this.privateEnableThemeChangeButton = value;
                this._PickerForm?.ToggleThemeSwitchButton(value);
            }
        }

        public void ShowDialog()
        {
            //Compiler generated
            //if (this.isDialogVisible)
            //    return;

            //this.isDialogVisible = true;

            //_PickerForm = new DatePickerForm(this.Value);
            //_PickerForm.Theme = DatePickerUiTheme;
            //_PickerForm?.ToggleThemeSwitchButton(this.privateEnableThemeChangeButton);
            //_PickerForm.Show();

            //_PickerForm.Location =
            //  this.PointToScreen(this.Location)
            //  + new Size(this.Width / 2, _PickerForm.cuiFormRounder1.Rounding * 2)
            //  - new Size(_PickerForm.Width, 0);

            //_PickerForm.FormClosing += PickerForm_FormClosing;
        }

        private void PickerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.isDialogVisible = false;
        }

        protected override void OnClick(EventArgs e)
        {
            this.Focus();
            ShowDialog();
            base.OnClick(e);
        }

        public int Rounding
        {
            get => this.privateRounding;
            set
            {
                this.privateRounding = value;
                this.Refresh();
            }
        }

        public Color NormalBackground
        {
            get => this.privateBackgroundColor;
            set
            {
                this.privateBackgroundColor = value;
                this.Refresh();
            }
        }

        public Color HoverBackground
        {
            get => this.privateHoverBackground;
            set
            {
                this.privateHoverBackground = value;
                this.Refresh();
            }
        }

        public Color PressedBackground
        {
            get => this.privatePressedBackground;
            set
            {
                this.privatePressedBackground = value;
                this.Refresh();
            }
        }

        public Color NormalOutline
        {
            get => this.privateNormalOutline;
            set
            {
                this.privateNormalOutline = value;
                this.Refresh();
            }
        }

        public Color HoverOutline
        {
            get => this.privateHoverOutline;
            set
            {
                this.privateHoverOutline = value;
                this.Refresh();
            }
        }

        public Color PressedOutline
        {
            get => this.privatePressedOutline;
            set
            {
                this.privatePressedOutline = value;
                this.Refresh();
            }
        }

        public float OutlineThickness
        {
            get => this.privateOutlineThickness;
            set
            {
                this.privateOutlineThickness = Math.Max(0.0f, value);
                this.Refresh();
            }
        }

        public bool ShowIcon
        {
            get => this.privateShowIcon;
            set
            {
                this.privateShowIcon = value;
                this.Refresh();
            }
        }

        public Image Icon
        {
            get => this.privateIcon;
            set
            {
                this.privateIcon = value;
                this.Refresh();
            }
        }

        public Color IconTint
        {
            get => this.privateImageTint;
            set
            {
                this.privateImageTint = value;
                this.Refresh();
            }
        }

        public DateTime Value
        {
            get => this.privateValue;
            set
            {
                this.privateValue = new DateTime(value.Year, value.Month, value.Day);
                EventHandler dateChanged = this.DateChanged;
                if (dateChanged != null)
                    dateChanged(this, EventArgs.Empty);
                this.Refresh();
            }
        }

        public event EventHandler DateChanged;

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Name = "cuiCalendarDatePicker";
            this.Size = new Size(153, 45);
            this.ResumeLayout(false);
        }
    }
}
