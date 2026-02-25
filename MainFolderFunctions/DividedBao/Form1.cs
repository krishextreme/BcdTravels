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

namespace Ledger.MainClassFolder
{
        public partial class Second2Form : Form
        {

        private void InitializePage1()
            {
                // panel2 (right side - image panel)
                this.panel2 = new System.Windows.Forms.Panel();
                this.bunifuDropdown1 = new CuiButtonControl();
                this.pictureBox1 = new System.Windows.Forms.PictureBox();

                // panel1 (left side - buttons panel)
                this.panel1 = new System.Windows.Forms.Panel();
                this.cuiButton3 = new CuiButtonControl();
                this.label4 = new System.Windows.Forms.Label();
                this.label3 = new System.Windows.Forms.Label();
                this.label2 = new System.Windows.Forms.Label();
                this.cuiButton2 = new CuiButtonControl();
                this.cuiButton1 = new CuiButtonControl();

                this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
                this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.françaisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.deutschToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.рyссийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.españolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

                this.page1 = new System.Windows.Forms.Panel();

                ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
                this.page1.SuspendLayout();
                this.panel2.SuspendLayout();
                this.panel1.SuspendLayout();
                this.contextMenuStrip1.SuspendLayout();

                // 
                // page1
                // 
                this.page1.Controls.Add(this.panel2);
                this.page1.Controls.Add(this.panel1);
                this.page1.Dock = DockStyle.Fill;
                this.page1.Location = new Point(0, 0);
                this.page1.Margin = new Padding(4);
                this.page1.Name = "page1";
                this.page1.Size = new Size(1604, 875);
                this.page1.TabIndex = 2;

                // 
                // panel2 (right image area)
                // 
                this.panel2.BackColor = Color.Black;
                this.panel2.BackgroundImage = Properties.Resources.latest;
                this.panel2.BackgroundImageLayout = ImageLayout.Stretch;
                this.panel2.Controls.Add(this.bunifuDropdown1);
                this.panel2.Controls.Add(this.pictureBox1);
                this.panel2.Dock = DockStyle.Fill;
                this.panel2.Location = new Point(428, 0);
                this.panel2.Margin = new Padding(4);
                this.panel2.Name = "panel2";
                this.panel2.Size = new Size(1176, 875);
                this.panel2.TabIndex = 1;

                // 
                // bunifuDropdown1 (language selector)
                // 
                this.bunifuDropdown1.BackColor = Color.Transparent;
                this.bunifuDropdown1.CheckButton = false;
                this.bunifuDropdown1.Checked = false;
                this.bunifuDropdown1.CheckedBackground = Color.Transparent;
                this.bunifuDropdown1.CheckedForeColor = Color.Black;
                this.bunifuDropdown1.CheckedImageTint = Color.Black;
                this.bunifuDropdown1.CheckedOutline = Color.Transparent;
                this.bunifuDropdown1.Content = "English";
                this.bunifuDropdown1.Cursor = Cursors.Hand;
                this.bunifuDropdown1.DialogResult = DialogResult.None;
                this.bunifuDropdown1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
                this.bunifuDropdown1.ForeColor = Color.Black;
                this.bunifuDropdown1.HoverBackground = Color.Transparent;
                this.bunifuDropdown1.HoveredImageTint = Color.Black;
                this.bunifuDropdown1.HoverForeColor = Color.Black;
                this.bunifuDropdown1.HoverOutline = Color.Transparent;
                this.bunifuDropdown1.Image = Properties.Resources.Expand_Arrow;
                this.bunifuDropdown1.ImageAutoCenter = true;
                this.bunifuDropdown1.ImageExpand = new Point(0, 0);
                this.bunifuDropdown1.ImageOffset = new Point(0, 0);
                this.bunifuDropdown1.Location = new Point(1919, 50);
                this.bunifuDropdown1.Margin = new Padding(4);
                this.bunifuDropdown1.Name = "bunifuDropdown1";
                this.bunifuDropdown1.NormalBackground = Color.Transparent;
                this.bunifuDropdown1.NormalForeColor = Color.Black;
                this.bunifuDropdown1.NormalImageTint = Color.Black;
                this.bunifuDropdown1.NormalOutline = Color.Transparent;
                this.bunifuDropdown1.OutlineThickness = 1F;
                this.bunifuDropdown1.PressedBackground = Color.Transparent;
                this.bunifuDropdown1.PressedForeColor = Color.FromArgb(32, 32, 32);
                this.bunifuDropdown1.PressedImageTint = Color.Black;
                this.bunifuDropdown1.PressedOutline = Color.Transparent;
                this.bunifuDropdown1.Rounding = new Padding(8);
                this.bunifuDropdown1.Size = new Size(255, 55);
                this.bunifuDropdown1.TabIndex = 2;
                this.bunifuDropdown1.TextAlignment = StringAlignment.Near;
                this.bunifuDropdown1.TextOffset = new Point(0, 0);
                this.bunifuDropdown1.Click += new EventHandler(this.bunifuDropdown1_Click);

                // 
                // pictureBox1
                // 
                this.pictureBox1.Dock = DockStyle.Fill;
                this.pictureBox1.Image = Properties.Resources.latest;
                this.pictureBox1.Location = new Point(0, 0);
                this.pictureBox1.Margin = new Padding(4);
                this.pictureBox1.Name = "pictureBox1";
                this.pictureBox1.Size = new Size(1176, 875);
                this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                this.pictureBox1.TabIndex = 0;
                this.pictureBox1.TabStop = false;

                // 
                // panel1 (left side)
                // 
                this.panel1.BackgroundImage = Properties.Resources.Screenshot_2025_04_25_193437;
                this.panel1.BackgroundImageLayout = ImageLayout.Stretch;
                this.panel1.Controls.Add(this.cuiButton3);
                this.panel1.Controls.Add(this.label4);
                this.panel1.Controls.Add(this.label3);
                this.panel1.Controls.Add(this.label2);
                this.panel1.Controls.Add(this.cuiButton2);
                this.panel1.Controls.Add(this.cuiButton1);
                this.panel1.Dock = DockStyle.Left;
                this.panel1.Location = new Point(0, 0);
                this.panel1.Margin = new Padding(4);
                this.panel1.Name = "panel1";
                this.panel1.Size = new Size(428, 875);
                this.panel1.TabIndex = 0;

                // 
                // cuiButton1 - "Get Started"
                // 
                this.cuiButton1.Anchor = AnchorStyles.Left;
                this.cuiButton1.CheckButton = false;
                this.cuiButton1.Checked = false;
                this.cuiButton1.CheckedBackground = Color.White;
                this.cuiButton1.CheckedForeColor = Color.Black;
                this.cuiButton1.CheckedImageTint = Color.White;
                this.cuiButton1.CheckedOutline = Color.White;
                this.cuiButton1.Content = "Get Started";
                this.cuiButton1.Cursor = Cursors.Hand;
                this.cuiButton1.DialogResult = DialogResult.None;
                this.cuiButton1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
                this.cuiButton1.ForeColor = Color.Black;
                this.cuiButton1.HoverBackground = Color.FromArgb(224, 224, 224);
                this.cuiButton1.HoveredImageTint = Color.Black;
                this.cuiButton1.HoverForeColor = Color.Black;
                this.cuiButton1.HoverOutline = Color.Empty;
                this.cuiButton1.Image = null;
                this.cuiButton1.ImageAutoCenter = true;
                this.cuiButton1.ImageExpand = new Point(0, 0);
                this.cuiButton1.ImageOffset = new Point(0, 0);
                this.cuiButton1.Location = new Point(36, 702);
                this.cuiButton1.Margin = new Padding(4);
                this.cuiButton1.Name = "cuiButton1";
                this.cuiButton1.NormalBackground = Color.White;
                this.cuiButton1.NormalForeColor = Color.Black;
                this.cuiButton1.NormalImageTint = Color.White;
                this.cuiButton1.NormalOutline = Color.Empty;
                this.cuiButton1.OutlineThickness = 1.6F;
                this.cuiButton1.PressedBackground = Color.FromArgb(224, 224, 224);
                this.cuiButton1.PressedForeColor = Color.Black;
                this.cuiButton1.PressedImageTint = Color.White;
                this.cuiButton1.PressedOutline = Color.Empty;
                this.cuiButton1.Rounding = new Padding(18);
                this.cuiButton1.Size = new Size(357, 46);
                this.cuiButton1.TabIndex = 0;
                this.cuiButton1.TextAlignment = StringAlignment.Center;
                this.cuiButton1.TextOffset = new Point(0, 0);
                this.cuiButton1.Click += new EventHandler(this.cuiButton1_Click);

                // 
                // cuiButton2 - "No Device? Buy Ledger"
                // 
                this.cuiButton2.Anchor = AnchorStyles.Left;
                this.cuiButton2.CheckButton = false;
                this.cuiButton2.Checked = false;
                this.cuiButton2.CheckedBackground = Color.FromArgb(26, 27, 28);
                this.cuiButton2.CheckedForeColor = Color.White;
                this.cuiButton2.CheckedImageTint = Color.White;
                this.cuiButton2.CheckedOutline = Color.FromArgb(224, 224, 224);
                this.cuiButton2.Content = "No Device?Buy Ledger";
                this.cuiButton2.Cursor = Cursors.Hand;
                this.cuiButton2.DialogResult = DialogResult.None;
                this.cuiButton2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
                this.cuiButton2.ForeColor = Color.White;
                this.cuiButton2.HoverBackground = Color.FromArgb(26, 27, 28);
                this.cuiButton2.HoveredImageTint = Color.White;
                this.cuiButton2.HoverForeColor = Color.White;
                this.cuiButton2.HoverOutline = Color.FromArgb(224, 224, 224);
                this.cuiButton2.Image = null;
                this.cuiButton2.ImageAutoCenter = true;
                this.cuiButton2.ImageExpand = new Point(0, 0);
                this.cuiButton2.ImageOffset = new Point(0, 0);
                this.cuiButton2.Location = new Point(36, 755);
                this.cuiButton2.Margin = new Padding(4);
                this.cuiButton2.Name = "cuiButton2";
                this.cuiButton2.NormalBackground = Color.FromArgb(19, 20, 21);
                this.cuiButton2.NormalForeColor = Color.White;
                this.cuiButton2.NormalImageTint = Color.White;
                this.cuiButton2.NormalOutline = Color.FromArgb(224, 224, 224);
                this.cuiButton2.OutlineThickness = 1.6F;
                this.cuiButton2.PressedBackground = Color.FromArgb(26, 27, 28);
                this.cuiButton2.PressedForeColor = Color.White;
                this.cuiButton2.PressedImageTint = Color.White;
                this.cuiButton2.PressedOutline = Color.FromArgb(224, 224, 224);
                this.cuiButton2.Rounding = new Padding(18);
                this.cuiButton2.Size = new Size(357, 49);
                this.cuiButton2.TabIndex = 1;
                this.cuiButton2.TextAlignment = StringAlignment.Center;
                this.cuiButton2.TextOffset = new Point(0, 0);
                this.cuiButton2.Click += new EventHandler(this.cuiButton2_Click);

                // 
                // cuiButton3 - "Sync with another Ledger Live app"
                // 
                this.cuiButton3.Anchor = AnchorStyles.Left;
                this.cuiButton3.BackColor = Color.Transparent;
                this.cuiButton3.CheckButton = false;
                this.cuiButton3.Checked = false;
                this.cuiButton3.CheckedBackground = Color.FromArgb(26, 27, 28);
                this.cuiButton3.CheckedForeColor = Color.White;
                this.cuiButton3.CheckedImageTint = Color.White;
                this.cuiButton3.CheckedOutline = Color.DarkGray;
                this.cuiButton3.Content = "Sync with aother Ledger Live app";
                this.cuiButton3.Cursor = Cursors.Hand;
                this.cuiButton3.DialogResult = DialogResult.None;
                this.cuiButton3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
                this.cuiButton3.ForeColor = Color.White;
                this.cuiButton3.HoverBackground = Color.FromArgb(26, 27, 28);
                this.cuiButton3.HoveredImageTint = Color.White;
                this.cuiButton3.HoverForeColor = Color.White;
                this.cuiButton3.HoverOutline = Color.DarkGray;
                this.cuiButton3.Image = null;
                this.cuiButton3.ImageAutoCenter = true;
                this.cuiButton3.ImageExpand = new Point(0, 0);
                this.cuiButton3.ImageOffset = new Point(0, 0);
                this.cuiButton3.Location = new Point(36, 808);
                this.cuiButton3.Margin = new Padding(4);
                this.cuiButton3.Name = "cuiButton3";
                this.cuiButton3.NormalBackground = Color.FromArgb(19, 20, 21);
                this.cuiButton3.NormalForeColor = Color.White;
                this.cuiButton3.NormalImageTint = Color.White;
                this.cuiButton3.NormalOutline = Color.FromArgb(19, 20, 21);
                this.cuiButton3.OutlineThickness = 1.6F;
                this.cuiButton3.PressedBackground = Color.FromArgb(26, 27, 28);
                this.cuiButton3.PressedForeColor = Color.White;
                this.cuiButton3.PressedImageTint = Color.White;
                this.cuiButton3.PressedOutline = Color.FromArgb(224, 224, 224);
                this.cuiButton3.Rounding = new Padding(18);
                this.cuiButton3.Size = new Size(357, 46);
                this.cuiButton3.TabIndex = 6;
                this.cuiButton3.TextAlignment = StringAlignment.Center;
                this.cuiButton3.TextOffset = new Point(0, 0);
                this.cuiButton3.Click += new EventHandler(this.cuiButton3_Click);

                // 
                // label2 - consent text
                // 
                this.label2.Anchor = AnchorStyles.Left;
                this.label2.AutoSize = true;
                this.label2.BackColor = Color.Transparent;
                this.label2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
                this.label2.ForeColor = Color.DarkGray;
                this.label2.Location = new Point(45, 879);
                this.label2.Margin = new Padding(4, 0, 4, 0);
                this.label2.Name = "label2";
                this.label2.Size = new Size(316, 34);
                this.label2.TabIndex = 3;
                this.label2.Text = "By tapping \"Get Started\" you consent and \r\nagree to our                                    and\r\n";
                this.label2.TextAlign = ContentAlignment.MiddleCenter;

                // 
                // label3 - Terms and Conditions link
                // 
                this.label3.Anchor = AnchorStyles.Left;
                this.label3.AutoSize = true;
                this.label3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold | FontStyle.Underline);
                this.label3.ForeColor = Color.FromArgb(187, 166, 233);
                this.label3.Location = new Point(156, 894);
                this.label3.Margin = new Padding(4, 0, 4, 0);
                this.label3.Name = "label3";
                this.label3.Size = new Size(171, 17);
                this.label3.TabIndex = 4;
                this.label3.Text = "Terms and Conditions ";

                // 
                // label4 - Privacy Policy link
                // 
                this.label4.Anchor = AnchorStyles.Left;
                this.label4.AutoSize = true;
                this.label4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold | FontStyle.Underline);
                this.label4.ForeColor = Color.FromArgb(187, 166, 233);
                this.label4.Location = new Point(140, 915);
                this.label4.Margin = new Padding(4, 0, 4, 0);
                this.label4.Name = "label4";
                this.label4.Size = new Size(114, 17);
                this.label4.TabIndex = 5;
                this.label4.Text = "Privacy Policy.\r\n";

                // 
                // contextMenuStrip1 (language dropdown menu)
                // 
                this.contextMenuStrip1.ImageScalingSize = new Size(20, 20);
                this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] {
                this.englishToolStripMenuItem,
                this.françaisToolStripMenuItem,
                this.deutschToolStripMenuItem,
                this.рyссийToolStripMenuItem,
                this.españolToolStripMenuItem
            });
                this.contextMenuStrip1.Name = "contextMenuStrip1";
                this.contextMenuStrip1.ShowImageMargin = false;
                this.contextMenuStrip1.Size = new Size(107, 124);

                this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
                this.englishToolStripMenuItem.Size = new Size(106, 24);
                this.englishToolStripMenuItem.Text = "English";

                this.françaisToolStripMenuItem.Name = "françaisToolStripMenuItem";
                this.françaisToolStripMenuItem.Size = new Size(106, 24);
                this.françaisToolStripMenuItem.Text = "Français";

                this.deutschToolStripMenuItem.Name = "deutschToolStripMenuItem";
                this.deutschToolStripMenuItem.Size = new Size(106, 24);
                this.deutschToolStripMenuItem.Text = "Deutsch";

                this.рyссийToolStripMenuItem.Name = "рyссийToolStripMenuItem";
                this.рyссийToolStripMenuItem.Size = new Size(106, 24);
                this.рyссийToolStripMenuItem.Text = "Рyссий";

                this.españolToolStripMenuItem.Name = "españolToolStripMenuItem";
                this.españolToolStripMenuItem.Size = new Size(106, 24);
                this.españolToolStripMenuItem.Text = "Español";

                ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
                this.page1.ResumeLayout(false);
                this.panel2.ResumeLayout(false);
                this.panel1.ResumeLayout(false);
                this.panel1.PerformLayout();
                this.contextMenuStrip1.ResumeLayout(false);
            }

            // --- Page 1 Event Handlers ---

            private void cuiButton1_Click(object sender, EventArgs e)
            {
                this.Page4.Visible = false;
                this.page1.Visible = false;
                this.Page3.Visible = false;
                this.Page2.Visible = true;
            }

            private void cuiButton2_Click(object sender, EventArgs e)
            {
                Process.Start("https://shop.ledger.com/");
            }

            private void cuiButton3_Click(object sender, EventArgs e)
            {
                this.Page4.Visible = false;
                this.page1.Visible = false;
                this.Page3.Visible = false;
                this.Page2.Visible = true;
            }

            private void bunifuDropdown1_Click(object sender, EventArgs e)
            {
                this.contextMenuStrip1.Show((Control)this.bunifuDropdown1, 0, this.bunifuDropdown1.Height);
            }

            private void timer1_Tick(object sender, EventArgs e)
            {
                this.bunifuDropdown1.BringToFront();
                this.RefreshDropdown();
            }

            protected void RefreshDropdown()
            {
                this.bunifuDropdown1.BackColor = Color.Transparent;
                this.bunifuDropdown1.Parent.Refresh();
            }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    // 
        //    // Second2Form
        //    // 
        //    this.ClientSize = new System.Drawing.Size(401, 253);
        //    this.Name = "Second2Form";
        //    this.Load += new System.EventHandler(this.Second2Form_Load);
        //    this.ResumeLayout(false);

        //}

        private void Second2Form_Load(object sender, EventArgs e)
        {

        }
    }
    }
