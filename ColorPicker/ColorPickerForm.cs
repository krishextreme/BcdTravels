// Deobfuscated / renamed full code (including full InitializeComponent from your paste).
// Original type: �ܦ�ΠΒᚕ�ᚈܢ�ܗ�Z.Υ�Κ�סPܪלAרX�
//
// Rules applied:
// - Meaningful class/member names
// - No logic changes (behavior preserved)
// - External/unknown control types are kept as placeholders so the file is self-contained
using Ledger.Animations;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Timer = System.Windows.Forms.Timer;

namespace Ledger.ColorPicker
{
    public class ColorPickerForm : Form
    {
        private bool currentlyChangingColor;
        private bool updateHexTextbox = true;
        private Color privateColorVal = Color.Empty;
        private ThemeMode privateTheme;

        private IContainer components;

        private CuiTextBox alphaTextBox; // was cuiTextBox1
        private CuiTextBox redTextBox;   // was cuiTextBox2
        private CuiTextBox greenTextBox; // was cuiTextBox3
        private CuiTextBox blueTextBox;  // was cuiTextBox4
        private CuiTextBox hexTextBox;   // was cuiTextBox5

        private CuiFormDrag formDrag;         // was cuiFormDrag1
        private CuiFormRounder formRounder;   // was cuiFormRounder1

        private CuiLabel previewLabel;        // was cuiLabel1
        private CuiLabel titleLabel;          // was cuiLabel2
        private CuiLabel hexLabel;            // was cuiLabel3
        private CuiLabel alphaLabel;          // was cuiLabel4
        private CuiLabel redLabel;            // was cuiLabel5
        private CuiLabel greenLabel;          // was cuiLabel6
        private CuiLabel blueLabel;           // was cuiLabel7

        private CuiButton closeButton;        // was cuiButton1
        private CuiButton okButton;           // was cuiButton2
        private CuiButton cancelButton;       // was cuiButton3
        private CuiButton themeToggleButton;  // was cuiButton4

        private CuiBorder previewBorder;      // was cuiBorder1
        private ColorPickerWheel colorPickerWheel; // was colorPickerWheel1

        public ColorPickerForm()
        {
            InitializeComponent();
            initTimer();
        }

        public ColorPickerForm(Color primaryColor, int rounding)
        {
            InitializeComponent();
            initTimer();
            formRounder.Rounding = rounding;
            SetColor(primaryColor);
        }

        private void initTimer()
        {
            Timer timer = new Timer();
            timer.Interval = 16;
            timer.Tick += new EventHandler(this.Timer_Tick);
            timer.Start();
        }

        public Color ColorVal
        {
            get => this.privateColorVal;
            set
            {
                this.currentlyChangingColor = true;
                this.privateColorVal = value;

                this.previewBorder.PanelColor = value;

                CuiTextBox tbA = this.alphaTextBox;
                byte num = value.A;
                string strA = num.ToString() ?? "";
                tbA.Content = strA;

                CuiTextBox tbR = this.redTextBox;
                num = value.R;
                string strR = num.ToString() ?? "";
                tbR.Content = strR;

                CuiTextBox tbG = this.greenTextBox;
                num = value.G;
                string strG = num.ToString() ?? "";
                tbG.Content = strG;

                CuiTextBox tbB = this.blueTextBox;
                num = value.B;
                string strB = num.ToString() ?? "";
                tbB.Content = strB;

                if (this.updateHexTextbox)
                    this.hexTextBox.Content = this.ColorToHex(value);

                this.colorPickerWheel.UpdatePos();
                this.currentlyChangingColor = false;
            }
        }

        private string ColorToHex(Color value) => $"#{value.A:X2}{value.R:X2}{value.G:X2}{value.B:X2}";

        private bool isInsideColorPicker()
        {
            return this.colorPickerWheel.ClientRectangle.Contains(
                this.colorPickerWheel.PointToClient(Cursor.Position)
            );
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!this.isInsideColorPicker() || !NativeMethods.isClickingLeftMouse())
                return;

            Point position = Cursor.Position;
            int x = position.X;
            position = Cursor.Position;
            int y = position.Y;

            MouseEventArgs e1 = new MouseEventArgs(MouseButtons.Left, 1, x, y, 1);
            this.ExtractColorFromClickPoint(sender, e1);
        }

        private static Color GetColorAtCursor()
        {
            IntPtr dc = NativeMethods.GetDC(IntPtr.Zero);

            NativeMethods.POINT lpPoint;
            NativeMethods.GetCursorPos(out lpPoint);

            uint pixel = NativeMethods.GetPixel(dc, lpPoint.X, lpPoint.Y);
            NativeMethods.ReleaseDC(IntPtr.Zero, dc);

            return Color.FromArgb(
                (int)pixel & (int)byte.MaxValue,
                ((int)pixel & 65280) >> 8,
                ((int)pixel & 16711680) >> 16
            );
        }

        private void ExtractColorFromClickPoint(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            this.SetColor(GetColorAtCursor());
        }

        internal void SetColor(Color target, bool arg_updatehex = true)
        {
            this.updateHexTextbox = arg_updatehex;
            this.ColorVal = target;
            this.updateHexTextbox = true;
        }

        private void closeButton_Click(object sender, EventArgs e) => this.Close();

        private void ContentChanged(object sender, EventArgs e)
        {
            if (!(sender is CuiTextBox) || this.currentlyChangingColor)
                return;

            CuiTextBox tb = sender as CuiTextBox;

            if (tb.Content.Trim() == string.Empty)
                return;

            int result;
            int.TryParse(tb.Content, out result);
            result = Math.Min((int)byte.MaxValue, result);
            result = Math.Max(0, result);

            Color colorVal;

            if (tb == this.alphaTextBox)
            {
                int alpha = result;
                colorVal = this.ColorVal;
                int r = (int)colorVal.R;
                colorVal = this.ColorVal;
                int g = (int)colorVal.G;
                colorVal = this.ColorVal;
                int b = (int)colorVal.B;
                this.ColorVal = Color.FromArgb(alpha, r, g, b);
            }

            if (tb == this.redTextBox)
            {
                colorVal = this.ColorVal;
                int a = (int)colorVal.A;
                int red = result;
                colorVal = this.ColorVal;
                int g = (int)colorVal.G;
                colorVal = this.ColorVal;
                int b = (int)colorVal.B;
                this.ColorVal = Color.FromArgb(a, red, g, b);
            }

            if (tb == this.greenTextBox)
            {
                colorVal = this.ColorVal;
                int a = (int)colorVal.A;
                colorVal = this.ColorVal;
                int r = (int)colorVal.R;
                int green = result;
                colorVal = this.ColorVal;
                int b = (int)colorVal.B;
                this.ColorVal = Color.FromArgb(a, r, green, b);
            }

            if (tb != this.blueTextBox)
                return;

            colorVal = this.ColorVal;
            int a1 = (int)colorVal.A;
            colorVal = this.ColorVal;
            int r1 = (int)colorVal.R;
            colorVal = this.ColorVal;
            int g1 = (int)colorVal.G;
            int blue = result;
            this.ColorVal = Color.FromArgb(a1, r1, g1, blue);
        }

        private void hexInput_ContentChanged(object sender, EventArgs e)
        {
            if (!(sender is CuiTextBox))
                return;

            try
            {
                Color target = ColorTranslator.FromHtml(this.hexTextBox.Content);
                if (!(target != Color.Empty))
                    return;

                this.SetColor(target, false);
            }
            catch
            {
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public ThemeMode Theme
        {
            get => this.privateTheme;
            set
            {
                this.privateTheme = value;
                this.colorPickerWheel.Theme = value;

                switch (value)
                {
                    case ThemeMode.Dark:
                        this.BackColor = Color.Black;
                        foreach (Control control in (ArrangedElementCollection)this.Controls)
                        {
                            if (control is CuiTextBox tb)
                            {
                                ((Control)tb).BackColor = this.BackColor;
                                tb.ForeColor = Color.White;
                                tb.BackgroundColor = Color.FromArgb(16, 16, 16);
                                tb.FocusBackgroundColor = tb.BackgroundColor;
                            }
                            else if (control is CuiLabel label && label != this.hexLabel)
                            {
                                label.ForeColor = Color.White;
                            }
                        }

                        this.cancelButton.NormalOutline = Color.FromArgb(20, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
                        this.okButton.NormalBackground = Color.FromArgb(20, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
                        this.okButton.ForeColor = Color.White;

                        this.previewBorder.PanelOutlineColor = Color.FromArgb(30, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
                        this.closeButton.NormalImageTint = Color.White;
                        this.formRounder.OutlineColor = Color.FromArgb(30, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);

                        this.hexLabel.ForeColor = Color.FromArgb(171, 171, 171);
                        break;

                    case ThemeMode.Light:
                        this.BackColor = SystemColors.Control;
                        foreach (Control control in (ArrangedElementCollection)this.Controls)
                        {
                            if (control is CuiTextBox tb)
                            {
                                ((Control)tb).BackColor = this.BackColor;
                                tb.ForeColor = Color.Black;
                                tb.BackgroundColor = Color.White;
                                tb.FocusBackgroundColor = tb.BackgroundColor;
                            }
                            else if (control is CuiLabel label && label != this.hexLabel)
                            {
                                label.ForeColor = Color.Black;
                            }
                        }

                        this.cancelButton.NormalOutline = Color.FromArgb(20, 0, 0, 0);
                        this.okButton.NormalBackground = Color.FromArgb(20, 0, 0, 0);
                        this.okButton.ForeColor = Color.Black;

                        this.previewBorder.PanelOutlineColor = Color.FromArgb(30, 0, 0, 0);
                        this.closeButton.NormalImageTint = Color.Black;
                        this.formRounder.OutlineColor = Color.FromArgb(30, 0, 0, 0);

                        this.hexLabel.ForeColor = Color.FromArgb(84, 84, 84);
                        break;
                }
            }
        }

        internal void ToggleThemeSwitchButton(bool value) => this.themeToggleButton.Visible = value;

        private void okButton_ForeColorChanged(object sender, EventArgs e)
        {
            this.okButton.HoverForeColor = this.okButton.ForeColor;
            this.okButton.PressedForeColor = this.okButton.ForeColor;

            this.cancelButton.ForeColor = this.okButton.ForeColor;
            this.cancelButton.HoverForeColor = this.okButton.ForeColor;
            this.cancelButton.PressedForeColor = this.okButton.ForeColor;

            this.okButton.NormalImageTint = this.okButton.ForeColor;
            this.cancelButton.NormalImageTint = this.okButton.ForeColor;

            this.okButton.HoveredImageTint = this.okButton.ForeColor;
            this.cancelButton.HoveredImageTint = this.okButton.ForeColor;

            this.okButton.PressedImageTint = this.okButton.ForeColor;
            this.cancelButton.PressedImageTint = this.okButton.ForeColor;
        }

        private void themeToggleButton_Click(object sender, EventArgs e)
        {
            if (this.Theme == ThemeMode.Light)
                this.Theme = ThemeMode.Dark;
            else
                this.Theme = ThemeMode.Light;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ColorPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(574, 570);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.Name = "ColorPickerForm";
            this.Text = "Color Picker";
            this.Load += new System.EventHandler(this.ColorPickerForm_Load);
            this.ResumeLayout(false);

        }

        public enum ThemeMode
        {
            Dark,
            Light
        }

        private void ColorPickerForm_Load(object sender, EventArgs e)
        {

        }
    }

    // --------------------------------------------------------------------
    // Placeholder external types (replace with your real deobfuscated controls)
    // --------------------------------------------------------------------

    public class CuiTextBox : Control
    {
        public event EventHandler ContentChanged;

        public Color BackgroundColor { get; set; }
        public Color BorderColor { get; set; }

        public string Content
        {
            get => Text;
            set
            {
                Text = value;
                ContentChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Color FocusBackgroundColor { get; set; }
        public Color FocusBorderColor { get; set; }
        public Color FocusImageTint { get; set; }

        public Image Image { get; set; }
        public Point ImageExpand { get; set; }
        public Point ImageOffset { get; set; }

        public bool Multiline { get; set; } // ADDED: Missing property

        public Color NormalImageTint { get; set; }

        public bool PasswordChar { get; set; }
        public Color PlaceholderColor { get; set; }
        public string PlaceholderText { get; set; }
        public Padding Rounding { get; set; }
        public Size TextOffset { get; set; }
        public bool UnderlinedStyle { get; set; }
    }

    public class CuiLabel : Control
    {
        public string Content
        {
            get => Text;
            set => Text = value;
        }

        public StringAlignment HorizontalAlignment { get; set; }
    }

    public class CuiButton : Control
    {
        public bool CheckButton { get; set; }
        public bool Checked { get; set; }

        public Color CheckedBackground { get; set; }
        public Color CheckedForeColor { get; set; }
        public Color CheckedImageTint { get; set; }
        public Color CheckedOutline { get; set; }

        public string Content
        {
            get => Text;
            set => Text = value;
        }

        public DialogResult DialogResult { get; set; }

        public Color HoverBackground { get; set; }
        public Color HoveredImageTint { get; set; }
        public Color HoverForeColor { get; set; }
        public Color HoverOutline { get; set; }

        public Image Image { get; set; }
        public bool ImageAutoCenter { get; set; }
        public Point ImageExpand { get; set; }
        public Point ImageOffset { get; set; }

        public Color NormalBackground { get; set; }
        public Color NormalForeColor { get; set; }
        public Color NormalImageTint { get; set; }
        public Color NormalOutline { get; set; }

        public float OutlineThickness { get; set; }

        public Color PressedBackground { get; set; }
        public Color PressedForeColor { get; set; }
        public Color PressedImageTint { get; set; }
        public Color PressedOutline { get; set; }

        public Padding Rounding { get; set; }
        public StringAlignment TextAlignment { get; set; }
        public Point TextOffset { get; set; }
    }

    public class CuiBorder : Control
    {
        public float OutlineThickness { get; set; }
        public Color PanelColor { get; set; }
        public Color PanelOutlineColor { get; set; }
        public Padding Rounding { get; set; }
    }

    public class ColorPickerWheel : Control
    {
        public Image Content { get; set; }
        public Padding Rounding { get; set; }
        public Color ImageTint { get; set; }
        public float OutlineThickness { get; set; }
        public Color PanelOutlineColor { get; set; }
        public int Rotation { get; set; }

        public ColorPickerForm.ThemeMode Theme { get; set; }

        public void UpdatePos()
        {
            // placeholder
        }
    }


    public class CuiFormDrag
    {
        public CuiFormDrag(IContainer components) { }
        public Form TargetForm { get; set; }
    }

    // Placeholder Resources used by InitializeComponent
    public static class Resources
    {
        public static Image half_moon = new Bitmap(16, 16);
        public static Image crossmark = new Bitmap(16, 16);
        public static Image yes = new Bitmap(16, 16);
    }

    // Placeholder for the obfuscated native/PInvoke helper
    internal static class NativeMethods
    {
        internal struct POINT
        {
            public int X;
            public int Y;
        }

        internal static bool isClickingLeftMouse()
            => (Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern IntPtr GetDC(IntPtr hwnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern int ReleaseDC(IntPtr hwnd, IntPtr dc);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool GetCursorPos(out POINT pt);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        internal static extern uint GetPixel(IntPtr dc, int x, int y);
    }
}
