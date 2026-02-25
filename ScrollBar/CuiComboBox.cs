// Decompiled with JetBrains decompiler
// Type: כ�ᚃܬו�Σ�𐠄�וᚅ�.�ΝΕ�מ��𐌏ܙ�ᚎܗᚏ
// Assembly: Ledger Live, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DC7CF87-871B-48BD-97F4-4117271260D2
// Assembly location: C:\Users\hps\Desktop\ledger\new\ledger.exe

using Ledger.BitUI;
using Ledger.ColorPicker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;
using Ledger.ComboxAndDatePicker;
namespace Ledger.ScrollBar
{

    [DefaultEvent("SelectedIndexChanged")]
    [ToolboxBitmap(typeof(ComboBox))]
    public class CuiComboBox : UserControl // was \uFFFDΝΕ\uFFFDמ\uFFFD\uFFFD\uD800\uDF0Fܙ\uFFFDᚎܗᚏ unicode
    {
        private string privateSelectedItem = string.Empty;
        private string[] privateItems = new string[0];
        private DateTime lastClosed = DateTime.MinValue;
        private int timerCountdown;
        private int maxCountdown = 50;
        private Color privateBackgroundColor = Color.FromArgb((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
        private Color privateOutlineColor = Color.FromArgb(64 /*0x40*/, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
        private Color privateDropDownBackgroundColor = Color.White;
        private Color privateDropDownOutlineColor = Color.FromArgb(30, (int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
        private float privateOutlineThickness = 1f;
        public bool isBrowsingOptions;
        private string privateNoSelectionText = "None";
        private string privateNoSelectionDropdownText = "Empty";
        private Color privateExpandColor = Color.Gray;

        private ComboBoxDropDown tempdropdown; // was UE\uD802\uDC31\uFFFD\uFFFDΦ\uFFFDᚏΛ\uFFFD\uFFFDטᛈ unicode
        private IContainer components;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string[] Items
        {
            get => this.privateItems;
            set
            {
                this.privateItems = value;
                this.Invalidate();
            }
        }

        public event EventHandler SelectedIndexChanged;

        public Cursor ButtonCursor { get; set; } = Cursors.Arrow;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedItem
        {
            get => this.privateSelectedItem;
            set
            {
                value = value.Trim();
                if (((IEnumerable<string>)this.Items).Contains<string>(value) && this.privateSelectedItem != value)
                {
                    this.privateSelectedItem = value;
                    EventHandler selectedIndexChanged = this.SelectedIndexChanged;
                    if (selectedIndexChanged != null)
                        selectedIndexChanged((object)this, EventArgs.Empty);
                    this.Refresh();
                }
                else
                {
                    this.privateSelectedItem = string.Empty;
                    this.Refresh();
                }
            }
        }

        public CuiComboBox() // was \uFFFDΝΕ\uFFFDמ\uFFFD\uFFFD\uD800\uDF0Fܙ\uFFFDᚎܗᚏ unicode
        {
            this.InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.ForeColor = Color.Gray;
            Timer timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(this.Timer_Tick);
            timer.Start();
            UiAnimationGlobals.FrameDrawn += new EventHandler(this.dropdownmove);
            GlobalMouseHook.OnGlobalMouseClick += new Action(this.HandleGlobalMouseClick); // was ᚎܗ\uFFFDSᚍ\uFFFDᛇܙܟד unicode
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.Focus();
        }

        private void HandleGlobalMouseClick()
        {
            if (this.DesignMode || this.tempdropdown == null)
                return;

            Point mousePosition = Control.MousePosition;
            Rectangle bounds = this.Bounds;

            bool clickedOutsideControl = !bounds.Contains(mousePosition); // was flag1

            ComboBoxDropDown tempdropdown = this.tempdropdown; // was UE... tempdropdown
            int num;
            if (tempdropdown == null)
            {
                num = 0;
            }
            else
            {
                // ISSUE: explicit non-virtual call
                bounds = tempdropdown.Bounds;
                num = !bounds.Contains(mousePosition) ? 1 : 0;
            }

            bool clickedOutsideDropDown = num != 0; // was flag2

            if (clickedOutsideControl & clickedOutsideDropDown)
            {
                this.CloseDropDown((object)null, EventArgs.Empty);
                this.lastClosed = DateTime.Now;
                this.Refresh();
            }

            if (GlobalMouseHook.isHooked) // was ᚎܗ...isHooked
                GlobalMouseHook.Stop();     // was ᚎܗ...Stop()

            this.isBrowsingOptions = false;
            this.lastClosed = DateTime.Now;
            this.Refresh();

            if (!GlobalMouseHook.isHooked)
                return;

            GlobalMouseHook.Stop();
        }

        private void dropdownmove(object sender, EventArgs e)
        {
            if (this.tempdropdown == null)
                return;

            Point screen = this.PointToScreen(Point.Empty);
            int y = screen.Y + this.Height + 3;
            this.tempdropdown.Location = new Point(screen.X + 3, y);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.DesignMode || this.tempdropdown == null || this.tempdropdown.IsDisposed)
                return;

            Point position = Cursor.Position;
            Rectangle screen = this.RectangleToScreen(this.ClientRectangle);
            screen.Height += this.Items.Length * 45;

            if (screen.Contains(position))
                this.timerCountdown = 0;
            else
                ++this.timerCountdown;

            if (this.timerCountdown < this.maxCountdown)
                return;

            this.timerCountdown = 0;
            this.IndexChanged((object)null, EventArgs.Empty);
            this.CloseDropDown((object)this.tempdropdown, EventArgs.Empty);
        }

        public Color BackgroundColor
        {
            get => this.privateBackgroundColor;
            set
            {
                this.privateBackgroundColor = value;
                this.Invalidate();
            }
        }

        public Color OutlineColor
        {
            get => this.privateOutlineColor;
            set
            {
                this.privateOutlineColor = value;
                this.Invalidate();
            }
        }

        public Color DropDownBackgroundColor
        {
            get => this.privateDropDownBackgroundColor;
            set
            {
                this.privateDropDownBackgroundColor = value;
                this.Invalidate();
            }
        }

        public Color DropDownOutlineColor
        {
            get => this.privateDropDownOutlineColor;
            set
            {
                this.privateDropDownOutlineColor = value;
                this.Invalidate();
            }
        }

        public float OutlineThickness
        {
            get => this.privateOutlineThickness;
            set
            {
                this.privateOutlineThickness = value;
                this.Invalidate();
            }
        }

        public string NoSelectionText
        {
            get => this.privateNoSelectionText;
            set
            {
                this.privateNoSelectionText = value;
                if (this.SelectedIndex != -1)
                    return;
                this.Refresh();
            }
        }

        public string NoSelectionDropdownText
        {
            get => this.privateNoSelectionDropdownText;
            set
            {
                this.privateNoSelectionDropdownText = value;
                if (this.SelectedIndex != -1)
                    return;
                this.Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.SelectedIndex == -1 && this.privateSelectedItem != string.Empty)
            {
                this.privateSelectedItem = string.Empty;
                this.Refresh();
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            Rectangle clientRectangle1 = this.ClientRectangle;
            clientRectangle1.Inflate(-1, -1);

            GraphicsPath path1 = BitMapClass.RoundRect(clientRectangle1, this.Rounding);
            using (SolidBrush solidBrush = new SolidBrush(this.BackgroundColor))
            {
                using (Pen pen = new Pen(this.OutlineColor, this.OutlineThickness))
                {
                    e.Graphics.FillPath((Brush)solidBrush, path1);
                    e.Graphics.DrawPath(pen, path1);
                }
            }

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;

            string s = this.privateSelectedItem;
            if (this.SelectedItem == "")
                s = this.NoSelectionText;

            e.Graphics.DrawString(
              s,
              this.Font,
              (Brush)new SolidBrush(this.ForeColor),
              (PointF)new Point(this.Width / 2, this.Height / 2 - this.Font.Height / 2),
              format);

            // fixed decompiler 'with' to plain assignments, no logic change
            Rectangle clientRectangle2 = this.ClientRectangle; // was "this.ClientRectangle with { ... }"
            clientRectangle2.Width = this.Height / 2;
            clientRectangle2.X = this.ClientRectangle.Right - this.Height / 2;

            clientRectangle2.Height = clientRectangle2.Width;
            clientRectangle2.Offset(-clientRectangle2.Width / 2, clientRectangle2.Height / 2);
            clientRectangle2.Width /= 2;
            clientRectangle2.X = this.ClientRectangle.Right - this.Height / 2;
            clientRectangle2.Height = clientRectangle2.Width;
            clientRectangle2.Offset(-clientRectangle2.Width / 2, clientRectangle2.Height / 2);

            GraphicsPath path2;
            if (this.isBrowsingOptions)
            {
                clientRectangle2.Height -= 2;
                ++clientRectangle2.Y;
                path2 = BitMapClass.DownArrow(clientRectangle2);
            }
            else
            {
                clientRectangle2.Width -= 2;
                path2 = BitMapClass.LeftArrowtest(clientRectangle2);
            }

            e.Graphics.FillPath((Brush)new SolidBrush(this.ExpandArrowColor), path2);
            base.OnPaint(e);
        }

        public Color ExpandArrowColor
        {
            get => this.privateExpandColor;
            set
            {
                this.privateExpandColor = value;
                this.Invalidate();
            }
        }

        public void AddItem(string itemToAdd)
        {
            int length = this.privateItems.Length + 1;
            string[] destinationArray = new string[length];
            Array.Copy((Array)this.privateItems, (Array)destinationArray, this.privateItems.Length);
            destinationArray[length - 1] = itemToAdd;
            this.Items = destinationArray;
        }

        public void RemoveItem(string itemToRemove)
        {
            int num = Array.IndexOf<string>(this.privateItems, itemToRemove);
            if (num == -1)
                return;
            string[] destinationArray = new string[this.privateItems.Length - 1];
            Array.Copy((Array)this.privateItems, 0, (Array)destinationArray, 0, num);
            Array.Copy((Array)this.privateItems, num + 1, (Array)destinationArray, num, this.privateItems.Length - num - 1);
            this.Items = destinationArray;
        }

        public int SelectedIndex => Array.IndexOf<string>(this.privateItems, this.SelectedItem);

        private void cuiComboBox_Click(object sender, EventArgs e)
        {
            if (this.isBrowsingOptions)
            {
                this.IndexChanged((object)null, EventArgs.Empty);
            }
            else
            {
                if (this.tempdropdown != null)
                    this.tempdropdown?.Close();
                if (this.IsDisposed)
                    return;

                try
                {
                    ComboBoxDropDown ueΦᚏΛטᛈ = new ComboBoxDropDown( // was UE\uD802\uDC31\uFFFD\uFFFDΦ\uFFFDᚏΛ\uFFFD\uFFFDטᛈ unicode
                      this.Items,
                      this.Width,
                      this.DropDownBackgroundColor,
                      this.DropDownOutlineColor,
                      this,
                      this.Rounding,
                      this.ButtonCursor,
                      this.NoSelectionDropdownText);

                    ueΦᚏΛטᛈ.ButtonCursor = this.ButtonCursor;
                    ueΦᚏΛטᛈ.NormalBackground = this.ButtonNormalBackground;
                    ueΦᚏΛטᛈ.HoverBackground = this.ButtonHoverBackground;
                    ueΦᚏΛטᛈ.PressedBackground = this.ButtonPressedBackground;
                    ueΦᚏΛטᛈ.NormalOutline = this.ButtonNormalOutline;
                    ueΦᚏΛטᛈ.HoverOutline = this.ButtonHoverOutline;
                    ueΦᚏΛטᛈ.PressedOutline = this.ButtonPressedOutline;
                    ueΦᚏΛטᛈ.Rounding = new Padding(this.Rounding, this.Rounding, this.Rounding, this.Rounding);
                    ueΦᚏΛטᛈ?.updateButtons();
                    ueΦᚏΛטᛈ?.Invalidate();

                    this.isBrowsingOptions = true;
                    this.Refresh();

                    int num1 = this.Items.Length * 45;
                    Rectangle r = this.ClientRectangle;
                    r.Offset(0, r.Height);
                    r.Height = num1;
                    r = this.RectangleToScreen(r);
                    Point screen = this.PointToScreen(Point.Empty);
                    int num2 = screen.Y + this.Height + 2;
                    int num3 = screen.X + ueΦᚏΛטᛈ.cuiFormRounder1.Rounding;
                    ueΦᚏΛטᛈ.Location = new Point(num3 - 1, num2 - 1);
                    ueΦᚏΛטᛈ.Size = ueΦᚏΛטᛈ.Size + new Size(2, 2);

                    this.tempdropdown = ueΦᚏΛטᛈ;
                    this.tempdropdown.Rounding = new Padding(this.Rounding, this.Rounding, this.Rounding, this.Rounding);

                    ueΦᚏΛטᛈ.cuiFormRounder1.roundedFormObj.Location = ueΦᚏΛטᛈ.Location;
                    ueΦᚏΛטᛈ.Width = this.Width - 4;
                    ueΦᚏΛטᛈ?.cuiFormRounder1.roundedFormObj.Invalidate();
                    ueΦᚏΛטᛈ.SelectedIndexChanged += new EventHandler(this.IndexChanged);
                    ueΦᚏΛטᛈ?.cuiFormRounder1.roundedFormObj.Show();
                    ueΦᚏΛטᛈ?.Show();
                    ueΦᚏΛטᛈ?.cuiFormRounder1.TargetForm.Invalidate();

                    GlobalMouseHook.Start(); // was ᚎܗ...Start()
                }
                catch
                {
                    GlobalMouseHook.Stop(); // was ᚎܗ...Stop()
                }
            }
        }

        private void CloseDropDown(object sender, EventArgs e)
        {
            if (this.tempdropdown != null)
                this.tempdropdown?.Close();
            if (sender is ComboBoxDropDown ueΦᚏΛטᛈ) // was UE\uD802\uDC31\uFFFD\uFFFDΦ\uFFFDᚏΛ\uFFFD\uFFFDטᛈ unicode
            {
                ueΦᚏΛטᛈ?.Close();
                this.isBrowsingOptions = false;
                this.Refresh();
            }
            else
            {
                if (sender != null)
                    throw new Exception($"Invalid sender\n{sender}");
                if ((this.lastClosed - DateTime.Now).Seconds < 1)
                    return;
                this.isBrowsingOptions = false;
                this.Refresh();
            }
            GlobalMouseHook.Stop(); // was ᚎܗ...Stop()
        }

        private void IndexChanged(object sender, EventArgs e)
        {
            if (this.tempdropdown != null)
                this.tempdropdown?.Close();
            if (sender is ComboBoxDropDown ueΦᚏΛטᛈ) // was UE\uD802\uDC31\uFFFD\uFFFDΦ\uFFFDᚏΛ\uFFFD\uFFFDטᛈ unicode
            {
                this.SelectedItem = ueΦᚏΛטᛈ?.SelectedItem;
                ueΦᚏΛטᛈ?.Close();
                this.tempdropdown?.Close();
                this.isBrowsingOptions = false;
                this.Refresh();
            }
            else
            {
                if (this.tempdropdown == null)
                    throw new Exception($"Invalid sender\n{sender}");
                this.tempdropdown?.cuiFormRounder1.roundedFormObj.Close();
                this.tempdropdown.cuiFormRounder1.TargetForm = (Form)null;
                this.tempdropdown?.cuiFormRounder1.Dispose();
                this.tempdropdown?.Dispose();
                this.tempdropdown?.Close();
                this.isBrowsingOptions = false;
                this.Refresh();
                this.tempdropdown = (ComboBoxDropDown)null;
            }
        }

        public int Rounding { get; set; } = 8;

        public Color ButtonNormalBackground { get; set; } = Theme.PrimaryColor;

        public Color ButtonHoverBackground { get; set; } = Theme.TranslucentPrimaryColor;

        public Color ButtonPressedBackground { get; set; } = Theme.PrimaryColor;

        public Color ButtonNormalOutline { get; set; } = Color.Empty;

        public Color ButtonHoverOutline { get; set; } = Color.Empty;

        public Color ButtonPressedOutline { get; set; } = Color.Empty;

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
            this.DoubleBuffered = true;
            this.Name = "cuiComboBox";
            this.Size = new Size(169, 45);
            this.Click += new EventHandler(this.cuiComboBox_Click);
            this.ResumeLayout(false);
        }
    }

    // NOTE: types referenced here must exist in your project.
    // Rename targets only, not logic.
    //
    // was UE\uD802\uDC31\uFFFD\uFFFDΦ\uFFFDᚏΛ\uFFFD\uFFFDטᛈ unicode
  

    // was ᚎܗ\uFFFDSᚍ\uFFFDᛇܙܟד unicode
    public static class GlobalMouseHook
    {
        public static bool isHooked;
        public static Action OnGlobalMouseClick;
        public static void Start() { }
        public static void Stop() { }
    }

}