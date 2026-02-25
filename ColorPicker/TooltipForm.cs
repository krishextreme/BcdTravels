// Deobfuscated / cleaned version of the tooltip window.
// Original type: �ܦ�ΠΒᚕ�ᚈܢ�ܗ�Z.�Κ�ג��ܣᚋΝ�E�C

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Ledger.ColorPicker
{
    public sealed class TooltipForm : Form
    {
        private IContainer components;

        // External custom label type (keep as-is unless you paste its definition)
        private readonly CuiLabel _label;

        public TooltipForm()
        {
            _label = new CuiLabel();
            InitializeComponent();

            TextChanged += TooltipForm_TextChanged;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                // Decompiled literal: 0x08080020
                // Typically includes layered + transparent + topmost related flags.
                cp.ExStyle |= 0x08080020;

                return cp;
            }
        }

        private void TooltipForm_TextChanged(object sender, EventArgs e)
        {
            _label.Content = Text;

            // Decompiled code used CreateGraphics() without disposing; fixed.
            using (Graphics g = CreateGraphics())
            {
                Size textSize = g.MeasureString(Text, _label.Font).ToSize();
                Size = new Size(textSize.Width + 2 + _label.Font.Height, textSize.Height * 2);
            }
        }

        private void TooltipForm_Resize(object sender, EventArgs e)
        {
            _label.Location = new Point(0, _label.Font.Height / 2);
            _label.Width = Width;
            _label.Height = Height;
        }

        private void TooltipForm_ForeColorChanged(object sender, EventArgs e)
        {
            _label.ForeColor = ForeColor;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            _label.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _label.Content = "Tooltip\\ Text";
            _label.Font = new Font("Microsoft YaHei UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 238);
            _label.ForeColor = SystemColors.ButtonFace;
            _label.HorizontalAlignment = StringAlignment.Center;
            _label.Location = new Point(0, 0);
            _label.Name = "tooltipLabel";
            _label.Size = new Size(80, 20);
            _label.TabIndex = 0;

            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;

            BackColor = Color.Black;
            ClientSize = new Size(80, 20);
            Controls.Add(_label);

            ForeColor = SystemColors.ButtonFace;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;

            Name = "TooltipForm";
            Opacity = 0.75;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "TooltipForm";
            TopMost = true;

            ForeColorChanged += TooltipForm_ForeColorChanged;
            Resize += TooltipForm_Resize;

            ResumeLayout(false);
        }
    }
}
