using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ledger.ColorPicker;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Ledger.Animations;

using System.Windows.Forms.Layout;
namespace Ledger.ComboxAndDatePicker
{
    public class ComboDropDownForm : Form
    {
        private readonly string[] _items;
        private readonly ListBox _listBox;

        public event EventHandler SelectedIndexChanged;

        public string SelectedItem => _listBox.SelectedItem as string;

        // Styling properties referenced by CuiComboBox
        public Color NormalBackground { get; set; }
        public Color HoverBackground { get; set; }
        public Color PressedBackground { get; set; }

        public Color NormalOutline { get; set; }
        public Color HoverOutline { get; set; }
        public Color PressedOutline { get; set; }

        public Cursor ButtonCursor { get; set; }

        public Padding Rounding { get; set; }

        // Stub for rounded-form helper used by the decompiled code
        public CuiFormRounder cuiFormRounder1 { get; } = new CuiFormRounder();

        public ComboDropDownForm(
            string[] items,
            int width,
            Color background,
            Color outline,
            Control owner,
            int rounding,
            Cursor cursor,
            string emptyText)
        {
            _items = items ?? Array.Empty<string>();

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            BackColor = background;
            Width = width;

            _listBox = new ListBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Font = owner.Font,
                Cursor = cursor
            };

            if (_items.Length == 0)
                _listBox.Items.Add(emptyText);
            else
                _listBox.Items.AddRange(_items.Cast<object>().ToArray());

            _listBox.SelectedIndexChanged += (s, e) =>
            {
                SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
            };

            Controls.Add(_listBox);

            Height = Math.Max(1, _listBox.ItemHeight * _listBox.Items.Count);
        }

        public void updateButtons()
        {
            // No-op: required only to satisfy decompiled calls
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ComboDropDownForm
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "ComboDropDownForm";
            this.Load += new System.EventHandler(this.ComboDropDownForm_Load);
            this.ResumeLayout(false);

        }

        private void ComboDropDownForm_Load(object sender, EventArgs e)
        {

        }
    }
    //public class CuiFormRounder : Component
    //{
    //    public bool EnhanceCorners { get; set; }
    //    public Color OutlineColor { get; set; }
    //    public int Rounding { get; set; }
    //    public Form TargetForm { get; set; }
    //}

    // Minimal stub to satisfy CuiComboBox usage
    //public class CuiFormRounder : IDisposable
    //{
    //    public Form TargetForm { get; set; }
    //    public Form roundedFormObj { get; } = new Form();
    //    public int Rounding { get; set; }

    //    public void Dispose()
    //    {
    //        roundedFormObj?.Dispose();
    //    }
    //}
}
