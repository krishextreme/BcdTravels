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
        private void InitializePage2()
        {
            this.Page2 = new System.Windows.Forms.Panel();
            this.cuiButton10 = new CuiButtonControl();
            this.nanoSbtn = new CuiButtonControl();
            this.Staxbtn = new CuiButtonControl();
            this.Flexbtn = new CuiButtonControl();
            this.nanoSplusbtn = new CuiButtonControl();
            this.nanoXbtn = new CuiButtonControl();

            this.Page2.SuspendLayout();

            // 
            // Page2
            // 
            this.Page2.BackgroundImage = Properties.Resources.Screenshot_2025_04_25_204108;
            this.Page2.BackgroundImageLayout = ImageLayout.Stretch;
            this.Page2.Controls.Add(this.cuiButton10);
            this.Page2.Controls.Add(this.nanoSbtn);
            this.Page2.Controls.Add(this.Staxbtn);
            this.Page2.Controls.Add(this.Flexbtn);
            this.Page2.Controls.Add(this.nanoSplusbtn);
            this.Page2.Controls.Add(this.nanoXbtn);
            this.Page2.Dock = DockStyle.Fill;
            this.Page2.Location = new Point(0, 0);
            this.Page2.Margin = new Padding(4);
            this.Page2.Name = "Page2";
            this.Page2.Size = new Size(1604, 875);
            this.Page2.TabIndex = 3;
            this.Page2.Visible = false;

            // 
            // cuiButton10 - "Previous" back button
            // 
            this.cuiButton10.BackColor = Color.Transparent;
            this.cuiButton10.CheckButton = false;
            this.cuiButton10.Checked = false;
            this.cuiButton10.CheckedBackground = Color.Transparent;
            this.cuiButton10.CheckedForeColor = Color.White;
            this.cuiButton10.CheckedImageTint = Color.White;
            this.cuiButton10.CheckedOutline = Color.DarkGray;
            this.cuiButton10.Content = "  Previous";
            this.cuiButton10.Cursor = Cursors.Hand;
            this.cuiButton10.DialogResult = DialogResult.None;
            this.cuiButton10.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            this.cuiButton10.ForeColor = Color.White;
            this.cuiButton10.HoverBackground = Color.FromArgb(26, 27, 28);
            this.cuiButton10.HoveredImageTint = Color.White;
            this.cuiButton10.HoverForeColor = Color.White;
            this.cuiButton10.HoverOutline = Color.DarkGray;
            this.cuiButton10.Image = Properties.Resources.Left_Arrow;
            this.cuiButton10.ImageAutoCenter = true;
            this.cuiButton10.ImageExpand = new Point(3, 3);
            this.cuiButton10.ImageOffset = new Point(0, 0);
            this.cuiButton10.Location = new Point(36, 30);
            this.cuiButton10.Margin = new Padding(4);
            this.cuiButton10.Name = "cuiButton10";
            this.cuiButton10.NormalBackground = Color.Transparent;
            this.cuiButton10.NormalForeColor = Color.White;
            this.cuiButton10.NormalImageTint = Color.White;
            this.cuiButton10.NormalOutline = Color.FromArgb(19, 20, 21);
            this.cuiButton10.OutlineThickness = 1.6F;
            this.cuiButton10.PressedBackground = Color.Transparent;
            this.cuiButton10.PressedForeColor = Color.White;
            this.cuiButton10.PressedImageTint = Color.White;
            this.cuiButton10.PressedOutline = Color.FromArgb(224, 224, 224);
            this.cuiButton10.Rounding = new Padding(18);
            this.cuiButton10.Size = new Size(141, 46);
            this.cuiButton10.TabIndex = 8;
            this.cuiButton10.TextAlignment = StringAlignment.Center;
            this.cuiButton10.TextOffset = new Point(0, 0);
            this.cuiButton10.Click += new EventHandler(this.cuiButton10_Click);

            // 
            // nanoXbtn - Nano X clickable area
            // 
            this.nanoXbtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            this.nanoXbtn.BackColor = Color.Transparent;
            this.nanoXbtn.CheckButton = false;
            this.nanoXbtn.Checked = false;
            this.nanoXbtn.CheckedBackground = Color.Transparent;
            this.nanoXbtn.CheckedForeColor = Color.Transparent;
            this.nanoXbtn.CheckedImageTint = Color.Transparent;
            this.nanoXbtn.CheckedOutline = Color.DarkGray;
            this.nanoXbtn.Content = "";
            this.nanoXbtn.Cursor = Cursors.Hand;
            this.nanoXbtn.DialogResult = DialogResult.None;
            this.nanoXbtn.Font = new Font("Microsoft Sans Serif", 9.75F);
            this.nanoXbtn.ForeColor = Color.Transparent;
            this.nanoXbtn.HoverBackground = Color.Transparent;
            this.nanoXbtn.HoveredImageTint = Color.Transparent;
            this.nanoXbtn.HoverForeColor = Color.Transparent;
            this.nanoXbtn.HoverOutline = Color.DimGray;
            this.nanoXbtn.Image = null;
            this.nanoXbtn.ImageAutoCenter = true;
            this.nanoXbtn.ImageExpand = new Point(0, 0);
            this.nanoXbtn.ImageOffset = new Point(0, 0);
            this.nanoXbtn.Location = new Point(1429, -25);
            this.nanoXbtn.Margin = new Padding(4);
            this.nanoXbtn.Name = "nanoXbtn";
            this.nanoXbtn.NormalBackground = Color.Transparent;
            this.nanoXbtn.NormalForeColor = Color.Transparent;
            this.nanoXbtn.NormalImageTint = Color.Transparent;
            this.nanoXbtn.NormalOutline = Color.Transparent;
            this.nanoXbtn.OutlineThickness = 1F;
            this.nanoXbtn.PressedBackground = Color.Transparent;
            this.nanoXbtn.PressedForeColor = Color.Transparent;
            this.nanoXbtn.PressedImageTint = Color.Transparent;
            this.nanoXbtn.PressedOutline = Color.DimGray;
            this.nanoXbtn.Rounding = new Padding(0);
            this.nanoXbtn.Size = new Size(417, 918);
            this.nanoXbtn.TabIndex = 0;
            this.nanoXbtn.TextAlignment = StringAlignment.Center;
            this.nanoXbtn.TextOffset = new Point(0, 0);
            this.nanoXbtn.Load += new EventHandler(this.nanoXbtn_Load);
            this.nanoXbtn.Click += new EventHandler(this.nanoXbtn_Click);

            // 
            // nanoSplusbtn - Nano S Plus clickable area
            // 
            this.nanoSplusbtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            this.nanoSplusbtn.BackColor = Color.Transparent;
            this.nanoSplusbtn.CheckButton = false;
            this.nanoSplusbtn.Checked = false;
            this.nanoSplusbtn.CheckedBackground = Color.Transparent;
            this.nanoSplusbtn.CheckedForeColor = Color.Transparent;
            this.nanoSplusbtn.CheckedImageTint = Color.Transparent;
            this.nanoSplusbtn.CheckedOutline = Color.DarkGray;
            this.nanoSplusbtn.Content = "";
            this.nanoSplusbtn.Cursor = Cursors.Hand;
            this.nanoSplusbtn.DialogResult = DialogResult.None;
            this.nanoSplusbtn.Font = new Font("Microsoft Sans Serif", 9.75F);
            this.nanoSplusbtn.ForeColor = Color.Transparent;
            this.nanoSplusbtn.HoverBackground = Color.Transparent;
            this.nanoSplusbtn.HoveredImageTint = Color.Transparent;
            this.nanoSplusbtn.HoverForeColor = Color.Transparent;
            this.nanoSplusbtn.HoverOutline = Color.DimGray;
            this.nanoSplusbtn.Image = null;
            this.nanoSplusbtn.ImageAutoCenter = true;
            this.nanoSplusbtn.ImageExpand = new Point(0, 0);
            this.nanoSplusbtn.ImageOffset = new Point(0, 0);
            this.nanoSplusbtn.Location = new Point(1012, -25);
            this.nanoSplusbtn.Margin = new Padding(4);
            this.nanoSplusbtn.Name = "nanoSplusbtn";
            this.nanoSplusbtn.NormalBackground = Color.Transparent;
            this.nanoSplusbtn.NormalForeColor = Color.Transparent;
            this.nanoSplusbtn.NormalImageTint = Color.Transparent;
            this.nanoSplusbtn.NormalOutline = Color.Transparent;
            this.nanoSplusbtn.OutlineThickness = 1F;
            this.nanoSplusbtn.PressedBackground = Color.Transparent;
            this.nanoSplusbtn.PressedForeColor = Color.Transparent;
            this.nanoSplusbtn.PressedImageTint = Color.Transparent;
            this.nanoSplusbtn.PressedOutline = Color.DimGray;
            this.nanoSplusbtn.Rounding = new Padding(0);
            this.nanoSplusbtn.Size = new Size(417, 918);
            this.nanoSplusbtn.TabIndex = 2;
            this.nanoSplusbtn.TextAlignment = StringAlignment.Center;
            this.nanoSplusbtn.TextOffset = new Point(0, 0);
            this.nanoSplusbtn.Click += new EventHandler(this.nanoSplusbtn_Click);

            // 
            // Flexbtn - Flex clickable area
            // 
            this.Flexbtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            this.Flexbtn.BackColor = Color.Transparent;
            this.Flexbtn.CheckButton = false;
            this.Flexbtn.Checked = false;
            this.Flexbtn.CheckedBackground = Color.Transparent;
            this.Flexbtn.CheckedForeColor = Color.Transparent;
            this.Flexbtn.CheckedImageTint = Color.Transparent;
            this.Flexbtn.CheckedOutline = Color.DarkGray;
            this.Flexbtn.Content = "";
            this.Flexbtn.Cursor = Cursors.Hand;
            this.Flexbtn.DialogResult = DialogResult.None;
            this.Flexbtn.Font = new Font("Microsoft Sans Serif", 9.75F);
            this.Flexbtn.ForeColor = Color.Transparent;
            this.Flexbtn.HoverBackground = Color.Transparent;
            this.Flexbtn.HoveredImageTint = Color.Transparent;
            this.Flexbtn.HoverForeColor = Color.Transparent;
            this.Flexbtn.HoverOutline = Color.DimGray;
            this.Flexbtn.Image = null;
            this.Flexbtn.ImageAutoCenter = true;
            this.Flexbtn.ImageExpand = new Point(0, 0);
            this.Flexbtn.ImageOffset = new Point(0, 0);
            this.Flexbtn.Location = new Point(524, -47);
            this.Flexbtn.Margin = new Padding(4);
            this.Flexbtn.Name = "Flexbtn";
            this.Flexbtn.NormalBackground = Color.Transparent;
            this.Flexbtn.NormalForeColor = Color.Transparent;
            this.Flexbtn.NormalImageTint = Color.Transparent;
            this.Flexbtn.NormalOutline = Color.Transparent;
            this.Flexbtn.OutlineThickness = 1F;
            this.Flexbtn.PressedBackground = Color.Transparent;
            this.Flexbtn.PressedForeColor = Color.Transparent;
            this.Flexbtn.PressedImageTint = Color.Transparent;
            this.Flexbtn.PressedOutline = Color.DimGray;
            this.Flexbtn.Rounding = new Padding(0);
            this.Flexbtn.Size = new Size(417, 918);
            this.Flexbtn.TabIndex = 3;
            this.Flexbtn.TextAlignment = StringAlignment.Center;
            this.Flexbtn.TextOffset = new Point(0, 0);
            this.Flexbtn.Click += new EventHandler(this.Flexbtn_Click);

            // 
            // Staxbtn - Stax clickable area
            // 
            this.Staxbtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            this.Staxbtn.BackColor = Color.Transparent;
            this.Staxbtn.CheckButton = false;
            this.Staxbtn.Checked = false;
            this.Staxbtn.CheckedBackground = Color.Transparent;
            this.Staxbtn.CheckedForeColor = Color.Transparent;
            this.Staxbtn.CheckedImageTint = Color.Transparent;
            this.Staxbtn.CheckedOutline = Color.DarkGray;
            this.Staxbtn.Content = "";
            this.Staxbtn.Cursor = Cursors.Hand;
            this.Staxbtn.DialogResult = DialogResult.None;
            this.Staxbtn.Font = new Font("Microsoft Sans Serif", 9.75F);
            this.Staxbtn.ForeColor = Color.Transparent;
            this.Staxbtn.HoverBackground = Color.Transparent;
            this.Staxbtn.HoveredImageTint = Color.Transparent;
            this.Staxbtn.HoverForeColor = Color.Transparent;
            this.Staxbtn.HoverOutline = Color.DimGray;
            this.Staxbtn.Image = null;
            this.Staxbtn.ImageAutoCenter = true;
            this.Staxbtn.ImageExpand = new Point(0, 0);
            this.Staxbtn.ImageOffset = new Point(0, 0);
            this.Staxbtn.Location = new Point(-240, -25);
            this.Staxbtn.Margin = new Padding(4);
            this.Staxbtn.Name = "Staxbtn";
            this.Staxbtn.NormalBackground = Color.Transparent;
            this.Staxbtn.NormalForeColor = Color.Transparent;
            this.Staxbtn.NormalImageTint = Color.Transparent;
            this.Staxbtn.NormalOutline = Color.Transparent;
            this.Staxbtn.OutlineThickness = 1F;
            this.Staxbtn.PressedBackground = Color.Transparent;
            this.Staxbtn.PressedForeColor = Color.Transparent;
            this.Staxbtn.PressedImageTint = Color.Transparent;
            this.Staxbtn.PressedOutline = Color.DimGray;
            this.Staxbtn.Rounding = new Padding(0);
            this.Staxbtn.Size = new Size(284, 918);
            this.Staxbtn.TabIndex = 4;
            this.Staxbtn.TextAlignment = StringAlignment.Center;
            this.Staxbtn.TextOffset = new Point(0, 0);
            this.Staxbtn.Click += new EventHandler(this.Staxbtn_Click);

            // 
            // nanoSbtn - Nano S clickable area
            // 
            this.nanoSbtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            this.nanoSbtn.BackColor = Color.Transparent;
            this.nanoSbtn.CheckButton = false;
            this.nanoSbtn.Checked = false;
            this.nanoSbtn.CheckedBackground = Color.Transparent;
            this.nanoSbtn.CheckedForeColor = Color.Transparent;
            this.nanoSbtn.CheckedImageTint = Color.Transparent;
            this.nanoSbtn.CheckedOutline = Color.DarkGray;
            this.nanoSbtn.Content = "";
            this.nanoSbtn.Cursor = Cursors.Hand;
            this.nanoSbtn.DialogResult = DialogResult.None;
            this.nanoSbtn.Font = new Font("Microsoft Sans Serif", 9.75F);
            this.nanoSbtn.ForeColor = Color.Transparent;
            this.nanoSbtn.HoverBackground = Color.Transparent;
            this.nanoSbtn.HoveredImageTint = Color.Transparent;
            this.nanoSbtn.HoverForeColor = Color.Transparent;
            this.nanoSbtn.HoverOutline = Color.DimGray;
            this.nanoSbtn.Image = null;
            this.nanoSbtn.ImageAutoCenter = true;
            this.nanoSbtn.ImageExpand = new Point(0, 0);
            this.nanoSbtn.ImageOffset = new Point(0, 0);
            this.nanoSbtn.Location = new Point(595, -25);
            this.nanoSbtn.Margin = new Padding(4);
            this.nanoSbtn.Name = "nanoSbtn";
            this.nanoSbtn.NormalBackground = Color.Transparent;
            this.nanoSbtn.NormalForeColor = Color.Transparent;
            this.nanoSbtn.NormalImageTint = Color.Transparent;
            this.nanoSbtn.NormalOutline = Color.Transparent;
            this.nanoSbtn.OutlineThickness = 1F;
            this.nanoSbtn.PressedBackground = Color.Transparent;
            this.nanoSbtn.PressedForeColor = Color.Transparent;
            this.nanoSbtn.PressedImageTint = Color.Transparent;
            this.nanoSbtn.PressedOutline = Color.DimGray;
            this.nanoSbtn.Rounding = new Padding(0);
            this.nanoSbtn.Size = new Size(417, 918);
            this.nanoSbtn.TabIndex = 5;
            this.nanoSbtn.TextAlignment = StringAlignment.Center;
            this.nanoSbtn.TextOffset = new Point(0, 0);
            this.nanoSbtn.Click += new EventHandler(this.nanoSbtn_Click);

            this.Page2.ResumeLayout(false);
        }

        // --- Page 2 Event Handlers ---

        private void cuiButton10_Click(object sender, EventArgs e)
        {
            this.Page4.Visible = false;
            this.Page3.Visible = false;
            this.Page2.Visible = false;
            this.page1.Visible = true;
        }

        private void nanoXbtn_Click(object sender, EventArgs e)
        {
            this.Page2.Visible = false;
            this.page1.Visible = false;
            this.Page4.Visible = false;
            this.Page3.Visible = true;
            this.FirstTime.Text = "YOUR NANO X?";
            this.SetUp.Text = "SETUP A NEW NANO X";
        }

        private void nanoSplusbtn_Click(object sender, EventArgs e)
        {
            this.Page2.Visible = false;
            this.page1.Visible = false;
            this.Page4.Visible = false;
            this.Page3.Visible = true;
            this.FirstTime.Text = "YOUR NANO S PLUS?";
            this.SetUp.Text = "SETUP A NEW NANO S PLUS";
        }

        private void Flexbtn_Click(object sender, EventArgs e)
        {
            this.Page2.Visible = false;
            this.page1.Visible = false;
            this.Page4.Visible = false;
            this.Page3.Visible = true;
            this.FirstTime.Text = "YOUR FLEX?";
            this.SetUp.Text = "SETUP A FLEX";
        }

        private void Staxbtn_Click(object sender, EventArgs e)
        {
            this.Page2.Visible = false;
            this.page1.Visible = false;
            this.Page4.Visible = false;
            this.Page3.Visible = true;
            this.FirstTime.Text = "YOUR STAX?";
            this.SetUp.Text = "SETUP A NEW STAX";
        }

        private void nanoSbtn_Click(object sender, EventArgs e)
        {
            this.Page2.Visible = false;
            this.page1.Visible = false;
            this.Page4.Visible = false;
            this.Page3.Visible = true;
            this.FirstTime.Text = "YOUR NANO S?";
            this.SetUp.Text = "SETUP A NEW NANO S";
        }

        private void nanoXbtn_Load(object sender, EventArgs e)
        {
            // Reserved for future use
        }

    
    }
}