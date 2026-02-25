using Ledger.Animations;
using Ledger.BitUI;
using Ledger.ScrollBar;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

using Ledger.Animations;



namespace Ledger.ComboxAndDatePicker
{
    // was UE\uD802\uDC31\uFFFD\uFFFDΦ\uFFFDᚏΛ\uFFFD\uFFFDטᛈ unicode
    public class ComboBoxDropDown : Form
    {
        // was \uFFFDΝΕ\uFFFDמ\uFFFD\uFFFD\uD800\uDF0Fܙ\uFFFDᚎܗᚏ unicode
        public CuiComboBox caller;

        public Cursor ButtonCursor = Cursors.Arrow;
        private string[] privateItems = new string[0];
        public int SelectedIndex;
        public string SelectedItem = string.Empty;
        private Padding privateRounding = new Padding(8);
        private Color privateNormalBackground = UiAnimationGlobals.PrimaryColor;
    private Color privateHoverBackground = Color.FromArgb(200, 123, 104, 238);
        private Color privatePressedBackground = UiAnimationGlobals.PrimaryColor;
    private Color privateNormalOutline = Color.Empty;
        private Color privateHoverOutline = Color.Empty;
        private Color privatePressedOutline = Color.Empty;
        private Color privateNormalForeColor = Color.White;
        private Color privateHoverForeColor = Color.White;
        private Color privatePressedForeColor = Color.White;
        private IContainer components;

        // was \uFFFD\uFFFDᚌᛃZ\uFFFDܡ\uFFFD\uFFFD\uFFFD unicode
        public CuiFormRounder cuiFormRounder1;

        public string[] Items
        {
            get => this.privateItems;
            set
            {
                this.privateItems = value;
                this.parseItems();
            }
        }

        public Color NoItemsForeColor { get; set; } = Color.Gray;

        private void parseItems()
        {
            // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD unicode
            CuiComboBoxDropDownItemButton[] controls = new CuiComboBoxDropDownItemButton[this.Items.Length];

            int index = 0;
            int num1 = 0;
            int num2 = this.Items.Length - 1;
            int num3 = 0;
            int num4 = 0;

            foreach (string str in this.Items)
            {
                // was UE\uD802\uDC31...\uFFFDᛚ\uFFFDᚎ\uFFFDLᛚ\uFFFDGד unicode (compiler-generated closure)
                ParseItemsClickClosure parseItemsClickClosure = new ParseItemsClickClosure(); // was ᛚᚎLᛚGד unicode
                parseItemsClickClosure.owner = this; // was <>4__this
                parseItemsClickClosure.item = str;   // was item

                if (parseItemsClickClosure.item.Trim() == string.Empty)
                {
                    --num2;
                }
                else
                {
                    // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD ܝכΜרΔᛈᛈ1 unicode
                    CuiComboBoxDropDownItemButton itemButton = new CuiComboBoxDropDownItemButton();

                    itemButton.Name = parseItemsClickClosure.item;

                    if (this.caller == null)
                        itemButton.Width = this.Width + this.cuiFormRounder1.Rounding * 2;
                    else
                        itemButton.Width = this.Width - 4;

                    itemButton.Cursor = this.ButtonCursor;
                    itemButton.Content = parseItemsClickClosure.item;
                    itemButton.Location = new Point(1, 1 + index * itemButton.Height);

                    num3 = itemButton.Height;
                    num4 = itemButton.Width;

                    // was \uFFFD\uFFFDᚌᛃZ\uFFFDܡ\uFFFD\uFFFD\uFFFD cuiFormRounder1 local (kept name) unicode type
                    CuiFormRounder cuiFormRounder1 = this.cuiFormRounder1;

                    Padding rounding = this.Rounding;
                    int all1 = rounding.All;
                    cuiFormRounder1.Rounding = all1;

                    if (index == num1)
                    {
                        // was \uD802\uDD13... ܝכΜרΔᛈᛈ2 unicode
                        CuiComboBoxDropDownItemButton firstButton = itemButton;

                        rounding = this.Rounding;
                        int all2 = rounding.All;
                        rounding = this.Rounding;
                        int all3 = rounding.All;

                        Padding padding = new Padding(all2, all3, 0, 0);
                        firstButton.Rounding = padding;
                    }
                    else if (index == num2)
                    {
                        // was \uD802\uDD13... ܝכΜרΔᛈᛈ3 unicode
                        CuiComboBoxDropDownItemButton lastButton = itemButton;

                        rounding = this.Rounding;
                        int all4 = rounding.All;
                        rounding = this.Rounding;
                        int all5 = rounding.All;

                        Padding padding = new Padding(0, 0, all4, all5);
                        lastButton.Rounding = padding;
                    }
                    else
                    {
                        itemButton.Rounding = new Padding(0);
                    }

                    if (index == num1 && index == num2)
                        itemButton.Rounding = this.Rounding;

                    itemButton.Click += new EventHandler(parseItemsClickClosure.OnItemClicked); // was <parseItems>b__0
                    controls[index] = itemButton;
                    ++index;
                }
            }

            this.SuspendLayout();
            this.Opacity = 0.0;

            if (this.caller == null)
                this.Size = new Size(this.Width, 3 + index * num3);
            else
                this.Size = new Size(this.Width, 1 + index * num3);

            this.Controls.Clear();

            if (index < 1)
            {
                // was ܣ\uFFFDᚍ\uFFFDΑCᚒQ\uFFFDרW\uFFFD unicode
                CuiLabel noItemsLabel = new CuiLabel(); // was ܣᚍΑcᚒQרW unicode

                noItemsLabel.Content = this.NoSelectionDropdownText;
                noItemsLabel.ForeColor = this.NoItemsForeColor;
                noItemsLabel.Font = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point);
                noItemsLabel.Width = this.Width;

                this.Controls.Add((Control)noItemsLabel);
                this.Visible = true;
                this.Opacity = 1.0;
                this.ResumeLayout();

                noItemsLabel.Location = new Point(0, this.Height / 2 - 2 - this.Font.Height / 2);
                noItemsLabel.HorizontalAlignment = StringAlignment.Center;
            }
            else
            {
                this.Controls.AddRange((Control[])controls);
                this.Visible = true;
                this.Opacity = 1.0;
                this.ResumeLayout();
            }

            this.Invalidate();
        }

        public void updateButtons()
        {
            foreach (Control control in (ArrangedElementCollection)this.Controls)
            {
                if (control is CuiComboBoxDropDownItemButton itemButton) // was \uD802\uDD13... ܝכΜרΔᛈᛈ unicode
                {
                    itemButton.NormalBackground = this.NormalBackground;
                    itemButton.HoverBackground = this.HoverBackground;
                    itemButton.PressedBackground = this.PressedBackground;
                    itemButton.NormalOutline = this.NormalOutline;
                    itemButton.HoverOutline = this.HoverOutline;
                    itemButton.PressedOutline = this.PressedOutline;
                    itemButton.ForeColor = this.NormalForeColor;
                    itemButton.HoverForeColor = this.HoverForeColor;
                    itemButton.PressedForeColor = this.PressedForeColor;
                }
            }
        }

        public event EventHandler SelectedIndexChanged;

        internal void GoTo(Point position) => this.Location = position;

        public void SetWidth(int userWidth) => this.Width = userWidth - this.cuiFormRounder1.Rounding * 2;

        public ComboBoxDropDown(int x, int y) // was UE\uD802\uDC31... unicode
        {
            this.InitializeComponent();
            this.Location = new Point(x, y);
        }

        public string NoSelectionDropdownText { get; set; } = "Empty";

        public ComboBoxDropDown(
          string[] userItems,
          int userWidth,
          Color bg,
          Color outline,
         CuiComboBox userCaller, // was \uFFFDΝΕ\uFFFDמ... unicode
          int roundingArg,
          Cursor cursorForButtons,
          string textWhenNothingSelected,
          bool visible = true)
        {
            this.InitializeComponent();
            this.ButtonCursor = cursorForButtons;
            this.NoSelectionDropdownText = textWhenNothingSelected;
            this.Rounding = new Padding(roundingArg, roundingArg, roundingArg, roundingArg);
            this.cuiFormRounder1.Rounding = this.Rounding.All;
            this.Width = userWidth - 3;
            this.cuiFormRounder1.OutlineColor = outline;
            this.BackColor = Color.FromArgb((int)byte.MaxValue, (int)bg.R, (int)bg.G, (int)bg.B);
            this.caller = userCaller;
            this.Items = userItems;
            if (this.caller == null || !visible)
            {
                this.Opacity = 0.0;
                this.Width -= 4;
            }
            this.updateButtons();
        }

        public ComboBoxDropDown() // was UE\uD802\uDC31... unicode
        {
            this.InitializeComponent();
            this.ShowInTaskbar = false;
        }

        public Padding Rounding
        {
            get => this.privateRounding;
            set
            {
                this.privateRounding = value;
                this.Invalidate();
            }
        }

        public Color NormalBackground
        {
            get => this.privateNormalBackground;
            set
            {
                this.privateNormalBackground = value;
                this.Invalidate();
            }
        }

        public Color HoverBackground
        {
            get => this.privateHoverBackground;
            set
            {
                this.privateHoverBackground = value;
                this.Invalidate();
            }
        }

        public Color PressedBackground
        {
            get => this.privatePressedBackground;
            set
            {
                this.privatePressedBackground = value;
                this.Invalidate();
            }
        }

        public Color NormalOutline
        {
            get => this.privateNormalOutline;
            set
            {
                this.privateNormalOutline = value;
                this.Invalidate();
            }
        }

        public Color HoverOutline
        {
            get => this.privateHoverOutline;
            set
            {
                this.privateHoverOutline = value;
                this.Invalidate();
            }
        }

        public Color PressedOutline
        {
            get => this.privatePressedOutline;
            set
            {
                this.privatePressedOutline = value;
                this.Invalidate();
            }
        }

        public Color NormalForeColor
        {
            get => this.privateNormalForeColor;
            set
            {
                this.privateNormalForeColor = value;
                this.Invalidate();
            }
        }

        public Color HoverForeColor
        {
            get => this.privateHoverForeColor;
            set
            {
                this.privateHoverForeColor = value;
                this.Invalidate();
            }
        }

        public Color PressedForeColor
        {
            get => this.privatePressedForeColor;
            set
            {
                this.privatePressedForeColor = value;
                this.Invalidate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // was \uFFFD\uFFFDᚌᛃZ\uFFFDܡ\uFFFD\uFFFD\uFFFD unicode
            this.cuiFormRounder1 = new CuiFormRounder();

            this.SuspendLayout();
            this.cuiFormRounder1.EnhanceCorners = false;
            this.cuiFormRounder1.OutlineColor = Color.FromArgb(30, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
            this.cuiFormRounder1.Rounding = 8;
            this.cuiFormRounder1.TargetForm = (Form)this;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoValidate = AutoValidate.Disable;
            this.BackColor = Color.White;
            this.ClientSize = new Size(0, 0);
            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ImeMode = ImeMode.Off;
            this.Location = new Point(-1000, -1000);
            this.Name = "ComboBoxDropDown";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Text = "ComboBoxDropDown";
            this.TopMost = true;
            this.ResumeLayout(false);
        }

        // was UE\uD802\uDC31\uFFFD\uFFFDΦ\uFFFDᚏΛ\uFFFD\uFFFDטᛈ.\uFFFDᛚ\uFFFDᚎ\uFFFDLᛚ\uFFFDGד unicode
        private sealed class ParseItemsClickClosure
        {
            public ComboBoxDropDown owner; // was <>4__this
            public string item;               // was item

            public void OnItemClicked(object sender, EventArgs e) // was <parseItems>b__0
            {
                owner.SelectedItem = item;
                owner.SelectedIndex = Array.IndexOf(owner.Items, item);

                EventHandler selectedIndexChanged = owner.SelectedIndexChanged;
                if (selectedIndexChanged != null)
                    selectedIndexChanged((object)owner, EventArgs.Empty);
            }
        }
    }

    // was \uFFFDΝΕ\uFFFDמ\uFFFD\uFFFD\uD800\uDF0Fܙ\uFFFDᚎܗᚏ unicode
    // NOTE: this is just the type name mapping for this file; keep your real implementation elsewhere.
  /// <summary>
  ///  public class CuiComboBox : UserControl { }
  /// </summary>

    // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD unicode
    // NOTE: type name mapping only; keep your real implementation elsewhere.
    public class CuiComboBoxDropDownItemButton : Control
    {
        public string Content { get; set; }
        public Padding Rounding { get; set; }

        public Color NormalBackground { get; set; }
        public Color HoverBackground { get; set; }
        public Color PressedBackground { get; set; }

        public Color NormalOutline { get; set; }
        public Color HoverOutline { get; set; }
        public Color PressedOutline { get; set; }

        public Color HoverForeColor { get; set; }
        public Color PressedForeColor { get; set; }
    }

    // was \uFFFD\uFFFDᚌᛃZ\uFFFDܡ\uFFFD\uFFFD\uFFFD unicode
    // NOTE: type name mapping only; keep your real implementation elsewhere.

    // was ܣ\uFFFDᚍ\uFFFDΑCᚒQ\uFFFDרW\uFFFD unicode
    // NOTE: type name mapping only; keep your real implementation elsewhere.
    //public class CuiLabel : Control
    //{
    //    public string Content { get; set; }
    //    public StringAlignment HorizontalAlignment { get; set; }
    //}
}
