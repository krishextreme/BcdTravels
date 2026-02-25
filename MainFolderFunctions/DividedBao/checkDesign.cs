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
    public  class checkForm: Form
    {
        public checkForm()
        {
            InitializeComponent();
        }
        private System.Windows.Forms.Panel panel1,panel2;
        private System.Windows.Forms.Panel page1;
        private CuiButtonControl cuiButton1;
        private CuiButtonControl cuiButton2;
        private CuiButtonControl cuiButton3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem françaisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deutschToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem рyссийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem españolToolStripMenuItem;
        private System.ComponentModel.IContainer components;
        private CuiButtonControl bunifuDropdown1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel Page2;
        private System.Windows.Forms.Panel Page3;
        private System.Windows.Forms.Panel Page4;
        private System.Windows.Forms.Timer timer1;
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.bunifuDropdown1 = new Ledger.ScrollBar.CuiButtonControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cuiButton3 = new Ledger.ScrollBar.CuiButtonControl();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cuiButton2 = new Ledger.ScrollBar.CuiButtonControl();
            this.cuiButton1 = new Ledger.ScrollBar.CuiButtonControl();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.françaisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deutschToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.рyссийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.españolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.page1 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.page1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.BackgroundImage = global::Ledger.MainClassFolder.Properties.Resources.latest;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.bunifuDropdown1);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(428, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1176, 875);
            this.panel2.TabIndex = 1;
            // 
            // bunifuDropdown1
            // 
            this.bunifuDropdown1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuDropdown1.CheckButton = false;
            this.bunifuDropdown1.Checked = false;
            this.bunifuDropdown1.CheckedBackground = System.Drawing.Color.Transparent;
            this.bunifuDropdown1.CheckedForeColor = System.Drawing.Color.Black;
            this.bunifuDropdown1.CheckedImageTint = System.Drawing.Color.Black;
            this.bunifuDropdown1.CheckedOutline = System.Drawing.Color.Transparent;
            this.bunifuDropdown1.Content = "English";
            this.bunifuDropdown1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuDropdown1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.bunifuDropdown1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.bunifuDropdown1.ForeColor = System.Drawing.Color.Black;
            this.bunifuDropdown1.HoverBackground = System.Drawing.Color.Transparent;
            this.bunifuDropdown1.HoveredImageTint = System.Drawing.Color.Black;
            this.bunifuDropdown1.HoverForeColor = System.Drawing.Color.Black;
            this.bunifuDropdown1.HoverOutline = System.Drawing.Color.Transparent;
            this.bunifuDropdown1.Image = global::Ledger.MainClassFolder.Properties.Resources.Expand_Arrow;
            this.bunifuDropdown1.ImageAutoCenter = true;
            this.bunifuDropdown1.ImageExpand = new System.Drawing.Point(0, 0);
            this.bunifuDropdown1.ImageOffset = new System.Drawing.Point(0, 0);
            this.bunifuDropdown1.Location = new System.Drawing.Point(1919, 50);
            this.bunifuDropdown1.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuDropdown1.Name = "bunifuDropdown1";
            this.bunifuDropdown1.NormalBackground = System.Drawing.Color.Transparent;
            this.bunifuDropdown1.NormalForeColor = System.Drawing.Color.Black;
            this.bunifuDropdown1.NormalImageTint = System.Drawing.Color.Black;
            this.bunifuDropdown1.NormalOutline = System.Drawing.Color.Transparent;
            this.bunifuDropdown1.OutlineThickness = 1F;
            this.bunifuDropdown1.PressedBackground = System.Drawing.Color.Transparent;
            this.bunifuDropdown1.PressedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.bunifuDropdown1.PressedImageTint = System.Drawing.Color.Black;
            this.bunifuDropdown1.PressedOutline = System.Drawing.Color.Transparent;
            this.bunifuDropdown1.Rounding = new System.Windows.Forms.Padding(8);
            this.bunifuDropdown1.Size = new System.Drawing.Size(255, 55);
            this.bunifuDropdown1.TabIndex = 2;
            this.bunifuDropdown1.TextAlignment = System.Drawing.StringAlignment.Near;
            this.bunifuDropdown1.TextOffset = new System.Drawing.Point(0, 0);
            this.bunifuDropdown1.Click += new System.EventHandler(this.bunifuDropdown1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::Ledger.MainClassFolder.Properties.Resources.latest;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1176, 875);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::Ledger.MainClassFolder.Properties.Resources.Screenshot_2025_04_25_193437;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.cuiButton3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cuiButton2);
            this.panel1.Controls.Add(this.cuiButton1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(428, 875);
            this.panel1.TabIndex = 0;
            // 
            // cuiButton3
            // 
            this.cuiButton3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cuiButton3.BackColor = System.Drawing.Color.Transparent;
            this.cuiButton3.CheckButton = false;
            this.cuiButton3.Checked = false;
            this.cuiButton3.CheckedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.cuiButton3.CheckedForeColor = System.Drawing.Color.White;
            this.cuiButton3.CheckedImageTint = System.Drawing.Color.White;
            this.cuiButton3.CheckedOutline = System.Drawing.Color.DarkGray;
            this.cuiButton3.Content = "Sync with aother Ledger Live app";
            this.cuiButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cuiButton3.DialogResult = System.Windows.Forms.DialogResult.None;
            this.cuiButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.cuiButton3.ForeColor = System.Drawing.Color.White;
            this.cuiButton3.HoverBackground = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.cuiButton3.HoveredImageTint = System.Drawing.Color.White;
            this.cuiButton3.HoverForeColor = System.Drawing.Color.White;
            this.cuiButton3.HoverOutline = System.Drawing.Color.DarkGray;
            this.cuiButton3.Image = null;
            this.cuiButton3.ImageAutoCenter = true;
            this.cuiButton3.ImageExpand = new System.Drawing.Point(0, 0);
            this.cuiButton3.ImageOffset = new System.Drawing.Point(0, 0);
            this.cuiButton3.Location = new System.Drawing.Point(36, 808);
            this.cuiButton3.Margin = new System.Windows.Forms.Padding(4);
            this.cuiButton3.Name = "cuiButton3";
            this.cuiButton3.NormalBackground = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(20)))), ((int)(((byte)(21)))));
            this.cuiButton3.NormalForeColor = System.Drawing.Color.White;
            this.cuiButton3.NormalImageTint = System.Drawing.Color.White;
            this.cuiButton3.NormalOutline = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(20)))), ((int)(((byte)(21)))));
            this.cuiButton3.OutlineThickness = 1.6F;
            this.cuiButton3.PressedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.cuiButton3.PressedForeColor = System.Drawing.Color.White;
            this.cuiButton3.PressedImageTint = System.Drawing.Color.White;
            this.cuiButton3.PressedOutline = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cuiButton3.Rounding = new System.Windows.Forms.Padding(18);
            this.cuiButton3.Size = new System.Drawing.Size(357, 46);
            this.cuiButton3.TabIndex = 6;
            this.cuiButton3.TextAlignment = System.Drawing.StringAlignment.Center;
            this.cuiButton3.TextOffset = new System.Drawing.Point(0, 0);
            this.cuiButton3.Click += new System.EventHandler(this.cuiButton3_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(166)))), ((int)(((byte)(233)))));
            this.label4.Location = new System.Drawing.Point(140, 915);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Privacy Policy.\r\n";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(166)))), ((int)(((byte)(233)))));
            this.label3.Location = new System.Drawing.Point(156, 894);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Terms and Conditions ";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.DarkGray;
            this.label2.Location = new System.Drawing.Point(45, 879);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(316, 34);
            this.label2.TabIndex = 3;
            this.label2.Text = "By tapping \"Get Started\" you consent and \r\nagree to our                          " +
    "          and\r\n";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cuiButton2
            // 
            this.cuiButton2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cuiButton2.CheckButton = false;
            this.cuiButton2.Checked = false;
            this.cuiButton2.CheckedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.cuiButton2.CheckedForeColor = System.Drawing.Color.White;
            this.cuiButton2.CheckedImageTint = System.Drawing.Color.White;
            this.cuiButton2.CheckedOutline = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cuiButton2.Content = "No Device?Buy Ledger";
            this.cuiButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cuiButton2.DialogResult = System.Windows.Forms.DialogResult.None;
            this.cuiButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.cuiButton2.ForeColor = System.Drawing.Color.White;
            this.cuiButton2.HoverBackground = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.cuiButton2.HoveredImageTint = System.Drawing.Color.White;
            this.cuiButton2.HoverForeColor = System.Drawing.Color.White;
            this.cuiButton2.HoverOutline = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cuiButton2.Image = null;
            this.cuiButton2.ImageAutoCenter = true;
            this.cuiButton2.ImageExpand = new System.Drawing.Point(0, 0);
            this.cuiButton2.ImageOffset = new System.Drawing.Point(0, 0);
            this.cuiButton2.Location = new System.Drawing.Point(36, 755);
            this.cuiButton2.Margin = new System.Windows.Forms.Padding(4);
            this.cuiButton2.Name = "cuiButton2";
            this.cuiButton2.NormalBackground = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(20)))), ((int)(((byte)(21)))));
            this.cuiButton2.NormalForeColor = System.Drawing.Color.White;
            this.cuiButton2.NormalImageTint = System.Drawing.Color.White;
            this.cuiButton2.NormalOutline = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cuiButton2.OutlineThickness = 1.6F;
            this.cuiButton2.PressedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.cuiButton2.PressedForeColor = System.Drawing.Color.White;
            this.cuiButton2.PressedImageTint = System.Drawing.Color.White;
            this.cuiButton2.PressedOutline = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cuiButton2.Rounding = new System.Windows.Forms.Padding(18);
            this.cuiButton2.Size = new System.Drawing.Size(357, 49);
            this.cuiButton2.TabIndex = 1;
            this.cuiButton2.TextAlignment = System.Drawing.StringAlignment.Center;
            this.cuiButton2.TextOffset = new System.Drawing.Point(0, 0);
            this.cuiButton2.Click += new System.EventHandler(this.cuiButton2_Click);
            // 
            // cuiButton1
            // 
            this.cuiButton1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cuiButton1.CheckButton = false;
            this.cuiButton1.Checked = false;
            this.cuiButton1.CheckedBackground = System.Drawing.Color.White;
            this.cuiButton1.CheckedForeColor = System.Drawing.Color.Black;
            this.cuiButton1.CheckedImageTint = System.Drawing.Color.White;
            this.cuiButton1.CheckedOutline = System.Drawing.Color.White;
            this.cuiButton1.Content = "Get Started";
            this.cuiButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cuiButton1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.cuiButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.cuiButton1.ForeColor = System.Drawing.Color.Black;
            this.cuiButton1.HoverBackground = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cuiButton1.HoveredImageTint = System.Drawing.Color.Black;
            this.cuiButton1.HoverForeColor = System.Drawing.Color.Black;
            this.cuiButton1.HoverOutline = System.Drawing.Color.Empty;
            this.cuiButton1.Image = null;
            this.cuiButton1.ImageAutoCenter = true;
            this.cuiButton1.ImageExpand = new System.Drawing.Point(0, 0);
            this.cuiButton1.ImageOffset = new System.Drawing.Point(0, 0);
            this.cuiButton1.Location = new System.Drawing.Point(36, 702);
            this.cuiButton1.Margin = new System.Windows.Forms.Padding(4);
            this.cuiButton1.Name = "cuiButton1";
            this.cuiButton1.NormalBackground = System.Drawing.Color.White;
            this.cuiButton1.NormalForeColor = System.Drawing.Color.Black;
            this.cuiButton1.NormalImageTint = System.Drawing.Color.White;
            this.cuiButton1.NormalOutline = System.Drawing.Color.Empty;
            this.cuiButton1.OutlineThickness = 1.6F;
            this.cuiButton1.PressedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cuiButton1.PressedForeColor = System.Drawing.Color.Black;
            this.cuiButton1.PressedImageTint = System.Drawing.Color.White;
            this.cuiButton1.PressedOutline = System.Drawing.Color.Empty;
            this.cuiButton1.Rounding = new System.Windows.Forms.Padding(18);
            this.cuiButton1.Size = new System.Drawing.Size(357, 46);
            this.cuiButton1.TabIndex = 0;
            this.cuiButton1.TextAlignment = System.Drawing.StringAlignment.Center;
            this.cuiButton1.TextOffset = new System.Drawing.Point(0, 0);
            this.cuiButton1.Click += new System.EventHandler(this.cuiButton1_Click);
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Size = new System.Drawing.Size(106, 24);
            this.englishToolStripMenuItem.Text = "English";
            // 
            // françaisToolStripMenuItem
            // 
            this.françaisToolStripMenuItem.Name = "françaisToolStripMenuItem";
            this.françaisToolStripMenuItem.Size = new System.Drawing.Size(106, 24);
            this.françaisToolStripMenuItem.Text = "Français";
            // 
            // deutschToolStripMenuItem
            // 
            this.deutschToolStripMenuItem.Name = "deutschToolStripMenuItem";
            this.deutschToolStripMenuItem.Size = new System.Drawing.Size(106, 24);
            this.deutschToolStripMenuItem.Text = "Deutsch";
            // 
            // рyссийToolStripMenuItem
            // 
            this.рyссийToolStripMenuItem.Name = "рyссийToolStripMenuItem";
            this.рyссийToolStripMenuItem.Size = new System.Drawing.Size(106, 24);
            this.рyссийToolStripMenuItem.Text = "Рyссий";
            // 
            // españolToolStripMenuItem
            // 
            this.españolToolStripMenuItem.Name = "españolToolStripMenuItem";
            this.españolToolStripMenuItem.Size = new System.Drawing.Size(106, 24);
            this.españolToolStripMenuItem.Text = "Español";
            // 
            // page1
            // 
            this.page1.Controls.Add(this.panel2);
            this.page1.Controls.Add(this.panel1);
            this.page1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.page1.Location = new System.Drawing.Point(0, 0);
            this.page1.Margin = new System.Windows.Forms.Padding(4);
            this.page1.Name = "page1";
            this.page1.Size = new System.Drawing.Size(1604, 875);
            this.page1.TabIndex = 2;
            // 
            // checkForm
            // 
            this.ClientSize = new System.Drawing.Size(900, 354);
            this.Name = "checkForm";
            this.Load += new System.EventHandler(this.checkForm_Load);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.page1.ResumeLayout(false);
            this.ResumeLayout(false);

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

        private void checkForm_Load(object sender, EventArgs e)
        {

        }

      
        private void Second2Form_Load(object sender, EventArgs e)
        {

        }
    }
}
