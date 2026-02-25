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
    // PAGE 3 - Setup Choice (Is this your first time? / Setup new / Restore)
    public partial class Second2Form : Form
    {
        private void InitializePage3()
        {
            this.Page3 = new System.Windows.Forms.Panel();
            this.cuiButton9 = new CuiButtonControl();
            this.SetUp = new System.Windows.Forms.Label();
            this.FirstTime = new System.Windows.Forms.Label();
            this.cuiButton4 = new CuiButtonControl();
            this.cuiButton5 = new CuiButtonControl();
            this.cuiButton6 = new CuiButtonControl();

            this.Page3.SuspendLayout();

            // 
            // Page3
            // 
            this.Page3.BackgroundImage = Properties.Resources.Untitled_1;
            this.Page3.BackgroundImageLayout = ImageLayout.Stretch;
            this.Page3.Controls.Add(this.cuiButton9);
            this.Page3.Controls.Add(this.SetUp);
            this.Page3.Controls.Add(this.FirstTime);
            this.Page3.Controls.Add(this.cuiButton4);
            this.Page3.Controls.Add(this.cuiButton5);
            this.Page3.Controls.Add(this.cuiButton6);
            this.Page3.Dock = DockStyle.Fill;
            this.Page3.Location = new Point(0, 0);
            this.Page3.Margin = new Padding(4);
            this.Page3.Name = "Page3";
            this.Page3.Size = new Size(1604, 875);
            this.Page3.TabIndex = 4;
            this.Page3.Visible = false;

            // 
            // cuiButton9 - "Previous" back button
            // 
            this.cuiButton9.BackColor = Color.Transparent;
            this.cuiButton9.CheckButton = false;
            this.cuiButton9.Checked = false;
            this.cuiButton9.CheckedBackground = Color.Transparent;
            this.cuiButton9.CheckedForeColor = Color.White;
            this.cuiButton9.CheckedImageTint = Color.White;
            this.cuiButton9.CheckedOutline = Color.DarkGray;
            this.cuiButton9.Content = "  Previous";
            this.cuiButton9.Cursor = Cursors.Hand;
            this.cuiButton9.DialogResult = DialogResult.None;
            this.cuiButton9.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            this.cuiButton9.ForeColor = Color.White;
            this.cuiButton9.HoverBackground = Color.Transparent;
            this.cuiButton9.HoveredImageTint = Color.White;
            this.cuiButton9.HoverForeColor = Color.White;
            this.cuiButton9.HoverOutline = Color.DarkGray;
            this.cuiButton9.Image = Properties.Resources.Left_Arrow;
            this.cuiButton9.ImageAutoCenter = true;
            this.cuiButton9.ImageExpand = new Point(3, 3);
            this.cuiButton9.ImageOffset = new Point(0, 0);
            this.cuiButton9.Location = new Point(36, 28);
            this.cuiButton9.Margin = new Padding(4);
            this.cuiButton9.Name = "cuiButton9";
            this.cuiButton9.NormalBackground = Color.FromArgb(19, 20, 21);
            this.cuiButton9.NormalForeColor = Color.White;
            this.cuiButton9.NormalImageTint = Color.White;
            this.cuiButton9.NormalOutline = Color.FromArgb(19, 20, 21);
            this.cuiButton9.OutlineThickness = 1.6F;
            this.cuiButton9.PressedBackground = Color.Transparent;
            this.cuiButton9.PressedForeColor = Color.White;
            this.cuiButton9.PressedImageTint = Color.White;
            this.cuiButton9.PressedOutline = Color.FromArgb(224, 224, 224);
            this.cuiButton9.Rounding = new Padding(18);
            this.cuiButton9.Size = new Size(141, 46);
            this.cuiButton9.TabIndex = 7;
            this.cuiButton9.TextAlignment = StringAlignment.Center;
            this.cuiButton9.TextOffset = new Point(0, 0);
            this.cuiButton9.Click += new EventHandler(this.cuiButton9_Click);

            // 
            // FirstTime label - dynamic device name e.g. "YOUR NANO X?"
            // 
            this.FirstTime.AutoSize = true;
            this.FirstTime.BackColor = Color.Transparent;
            this.FirstTime.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold);
            this.FirstTime.Location = new Point(123, 124);
            this.FirstTime.Margin = new Padding(4, 0, 4, 0);
            this.FirstTime.Name = "FirstTime";
            this.FirstTime.Size = new Size(85, 29);
            this.FirstTime.TabIndex = 0;
            this.FirstTime.Text = "label1";

            // 
            // SetUp label - dynamic setup text e.g. "SETUP A NEW NANO X"
            // 
            this.SetUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.SetUp.AutoSize = true;
            this.SetUp.BackColor = Color.Transparent;
            this.SetUp.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            this.SetUp.Location = new Point(1239, 273);
            this.SetUp.Margin = new Padding(4, 0, 4, 0);
            this.SetUp.Name = "SetUp";
            this.SetUp.Size = new Size(52, 18);
            this.SetUp.TabIndex = 1;
            this.SetUp.Text = "label5";

            // 
            // cuiButton4 - First clickable option (top right area, e.g. "Setup as new")
            // 
            this.cuiButton4.Anchor = AnchorStyles.Right;
            this.cuiButton4.BackColor = Color.Transparent;
            this.cuiButton4.CheckButton = false;
            this.cuiButton4.Checked = false;
            this.cuiButton4.CheckedBackground = Color.Transparent;
            this.cuiButton4.CheckedForeColor = Color.White;
            this.cuiButton4.CheckedImageTint = Color.White;
            this.cuiButton4.CheckedOutline = Color.Olive;
            this.cuiButton4.Content = "";
            this.cuiButton4.Cursor = Cursors.Hand;
            this.cuiButton4.DialogResult = DialogResult.None;
            this.cuiButton4.Font = new Font("Microsoft Sans Serif", 9.75F);
            this.cuiButton4.ForeColor = Color.Black;
            this.cuiButton4.HoverBackground = Color.Transparent;
            this.cuiButton4.HoveredImageTint = Color.White;
            this.cuiButton4.HoverForeColor = Color.Black;
            this.cuiButton4.HoverOutline = Color.Olive;
            this.cuiButton4.Image = null;
            this.cuiButton4.ImageAutoCenter = true;
            this.cuiButton4.ImageExpand = new Point(0, 0);
            this.cuiButton4.ImageOffset = new Point(0, 0);
            this.cuiButton4.Location = new Point(1225, 13);
            this.cuiButton4.Margin = new Padding(4);
            this.cuiButton4.Name = "cuiButton4";
            this.cuiButton4.NormalBackground = Color.Transparent;
            this.cuiButton4.NormalForeColor = Color.Black;
            this.cuiButton4.NormalImageTint = Color.White;
            this.cuiButton4.NormalOutline = Color.DimGray;
            this.cuiButton4.OutlineThickness = 1F;
            this.cuiButton4.PressedBackground = Color.Transparent;
            this.cuiButton4.PressedForeColor = Color.White;
            this.cuiButton4.PressedImageTint = Color.White;
            this.cuiButton4.PressedOutline = Color.Olive;
            this.cuiButton4.Rounding = new Padding(0);
            this.cuiButton4.Size = new Size(243, 276);
            this.cuiButton4.TabIndex = 8;
            this.cuiButton4.TextAlignment = StringAlignment.Center;
            this.cuiButton4.TextOffset = new Point(0, 0);
            this.cuiButton4.Click += new EventHandler(this.cuiButton4_Click);

            // 
            // cuiButton5 - Second clickable option (middle right area, e.g. "Restore")
            // 
            this.cuiButton5.Anchor = AnchorStyles.Right;
            this.cuiButton5.BackColor = Color.Transparent;
            this.cuiButton5.CheckButton = false;
            this.cuiButton5.Checked = false;
            this.cuiButton5.CheckedBackground = Color.Transparent;
            this.cuiButton5.CheckedForeColor = Color.White;
            this.cuiButton5.CheckedImageTint = Color.White;
            this.cuiButton5.CheckedOutline = Color.Olive;
            this.cuiButton5.Content = "";
            this.cuiButton5.Cursor = Cursors.Hand;
            this.cuiButton5.DialogResult = DialogResult.None;
            this.cuiButton5.Font = new Font("Microsoft Sans Serif", 9.75F);
            this.cuiButton5.ForeColor = Color.Black;
            this.cuiButton5.HoverBackground = Color.Transparent;
            this.cuiButton5.HoveredImageTint = Color.White;
            this.cuiButton5.HoverForeColor = Color.Black;
            this.cuiButton5.HoverOutline = Color.Olive;
            this.cuiButton5.Image = null;
            this.cuiButton5.ImageAutoCenter = true;
            this.cuiButton5.ImageExpand = new Point(0, 0);
            this.cuiButton5.ImageOffset = new Point(0, 0);
            this.cuiButton5.Location = new Point(1225, 296);
            this.cuiButton5.Margin = new Padding(4);
            this.cuiButton5.Name = "cuiButton5";
            this.cuiButton5.NormalBackground = Color.Transparent;
            this.cuiButton5.NormalForeColor = Color.Black;
            this.cuiButton5.NormalImageTint = Color.White;
            this.cuiButton5.NormalOutline = Color.DimGray;
            this.cuiButton5.OutlineThickness = 1F;
            this.cuiButton5.PressedBackground = Color.Transparent;
            this.cuiButton5.PressedForeColor = Color.White;
            this.cuiButton5.PressedImageTint = Color.White;
            this.cuiButton5.PressedOutline = Color.Olive;
            this.cuiButton5.Rounding = new Padding(0);
            this.cuiButton5.Size = new Size(243, 322);
            this.cuiButton5.TabIndex = 9;
            this.cuiButton5.TextAlignment = StringAlignment.Center;
            this.cuiButton5.TextOffset = new Point(0, 0);
            this.cuiButton5.Click += new EventHandler(this.cuiButton5_Click);

            // 
            // cuiButton6 - Third clickable option (bottom right area)
            // 
            this.cuiButton6.Anchor = AnchorStyles.Right;
            this.cuiButton6.BackColor = Color.Transparent;
            this.cuiButton6.CheckButton = false;
            this.cuiButton6.Checked = false;
            this.cuiButton6.CheckedBackground = Color.Transparent;
            this.cuiButton6.CheckedForeColor = Color.White;
            this.cuiButton6.CheckedImageTint = Color.White;
            this.cuiButton6.CheckedOutline = Color.Olive;
            this.cuiButton6.Content = "";
            this.cuiButton6.Cursor = Cursors.Hand;
            this.cuiButton6.DialogResult = DialogResult.None;
            this.cuiButton6.Font = new Font("Microsoft Sans Serif", 9.75F);
            this.cuiButton6.ForeColor = Color.Black;
            this.cuiButton6.HoverBackground = Color.Transparent;
            this.cuiButton6.HoveredImageTint = Color.White;
            this.cuiButton6.HoverForeColor = Color.Black;
            this.cuiButton6.HoverOutline = Color.Olive;
            this.cuiButton6.Image = null;
            this.cuiButton6.ImageAutoCenter = true;
            this.cuiButton6.ImageExpand = new Point(0, 0);
            this.cuiButton6.ImageOffset = new Point(0, 0);
            this.cuiButton6.Location = new Point(1225, 626);
            this.cuiButton6.Margin = new Padding(4);
            this.cuiButton6.Name = "cuiButton6";
            this.cuiButton6.NormalBackground = Color.Transparent;
            this.cuiButton6.NormalForeColor = Color.Black;
            this.cuiButton6.NormalImageTint = Color.White;
            this.cuiButton6.NormalOutline = Color.DimGray;
            this.cuiButton6.OutlineThickness = 1F;
            this.cuiButton6.PressedBackground = Color.Transparent;
            this.cuiButton6.PressedForeColor = Color.White;
            this.cuiButton6.PressedImageTint = Color.White;
            this.cuiButton6.PressedOutline = Color.Olive;
            this.cuiButton6.Rounding = new Padding(0);
            this.cuiButton6.Size = new Size(243, 319);
            this.cuiButton6.TabIndex = 10;
            this.cuiButton6.TextAlignment = StringAlignment.Center;
            this.cuiButton6.TextOffset = new Point(0, 0);
            this.cuiButton6.Click += new EventHandler(this.cuiButton6_Click);

            this.Page3.ResumeLayout(false);
            this.Page3.PerformLayout();
        }

        // --- Page 3 Event Handlers ---

        private void cuiButton9_Click(object sender, EventArgs e)
        {
            this.Page4.Visible = false;
            this.Page3.Visible = false;
            this.page1.Visible = false;
            this.Page2.Visible = true;
        }

        private void cuiButton4_Click(object sender, EventArgs e)
        {
            this.Page3.Visible = false;
            this.Page2.Visible = false;
            this.page1.Visible = false;
            this.Page4.Visible = true;
        }

        private void cuiButton5_Click(object sender, EventArgs e)
        {
            this.Page3.Visible = false;
            this.Page2.Visible = false;
            this.page1.Visible = false;
            this.Page4.Visible = true;
        }

        private void cuiButton6_Click(object sender, EventArgs e)
        {
            this.Page3.Visible = false;
            this.Page2.Visible = false;
            this.page1.Visible = false;
            this.Page4.Visible = true;
        }
    }
}
