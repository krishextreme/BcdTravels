
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
//using otherClass \uFFFDܙ\uFFFDܗ\uFFFDΝᛟ\uFFFD\uFFFD\uFFFD\uFFFD;
using Timer = System.Windows.Forms.Timer;

namespace Ledger.MainClassFolder
{

    public class AlertPopupForm : Form // was: \uFFFDΑ\uFFFD\uD802\uDC04\uFFFD\uFFFD\uFFFDT\uFFFDC
    {
        private int slideStep;                    // was: interval
        private IContainer components;
        private Timer autoCloseTimer;             // was: timeout
        private Timer slideInTimer;               // was: show
        private Timer fadeOutTimer;               // was: closealert
        private Label messageLabel;               // was: message
        private ImageList imageList1;
        private PictureBox iconPictureBox;        // was: icon
      
        public AlertPopupForm(                   // was: \uFFFDΑ\uFFFD\uD802\uDC04\uFFFD\uFFFD\uFFFDT\uFFFDC
             string messageText,                    // was: _message
             AlertType alertType)                   // was: type / obfuscated enum
           {
               this.InitializeComponent();
               this.messageLabel.Text = messageText;
               if (this.imageList1.Images.Count < 4)
                   throw new InvalidOperationException("ImageList must contain 4 images");
               switch (alertType)
               {
                   case AlertType.success:
                       this.BackColor = Color.FromArgb(26, 27, 28);
                       this.iconPictureBox.Image = this.imageList1.Images[0];
                       break;
                   case AlertType.info:
                       this.BackColor = Color.FromArgb(26, 27, 28);
                       this.iconPictureBox.Image = this.imageList1.Images[1];
                       break;
                   case AlertType.warning:
                       this.BackColor = Color.FromArgb(26, 27, 28);
                       this.iconPictureBox.Image = this.imageList1.Images[2];
                       break;
                   case AlertType.error:
                       this.BackColor = Color.FromArgb(26, 27, 28);
                       this.iconPictureBox.Image = this.imageList1.Images[3];
                       break;
               }
           }
   
        public static void Show(
          string message,
          AlertType type)
        {
                       new AlertPopupForm(message, type).Show();
        }

        private void alert_Load(object sender, EventArgs e)
        {
            this.Top = -1 * this.Height;
            this.Left = Screen.PrimaryScreen.Bounds.Width - this.Width - 60;
            this.slideInTimer.Start();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e) => this.fadeOutTimer.Start();

        private void timeout_Tick(object sender, EventArgs e) => this.fadeOutTimer.Start();

        private void show_Tick(object sender, EventArgs e)
        {
            if (this.Top < 60)
            {
                this.Top += this.slideStep;
                this.slideStep += 2;
            }
            else
                this.slideInTimer.Stop();
        }

        private void close_Tick(object sender, EventArgs e)
        {
            if (this.Opacity > 0.0)
                this.Opacity -= 0.2;
            else
                this.Close();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlertPopupForm));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.autoCloseTimer = new System.Windows.Forms.Timer(this.components);
            this.slideInTimer = new System.Windows.Forms.Timer(this.components);
            this.fadeOutTimer = new System.Windows.Forms.Timer(this.components);
            this.messageLabel = new System.Windows.Forms.Label();
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // autoCloseTimer
            // 
            this.autoCloseTimer.Enabled = true;
            this.autoCloseTimer.Interval = 5000;
            this.autoCloseTimer.Tick += new System.EventHandler(this.timeout_Tick);
            // 
            // slideInTimer
            // 
            this.slideInTimer.Interval = 1;
            this.slideInTimer.Tick += new System.EventHandler(this.show_Tick);
            // 
            // fadeOutTimer
            // 
            this.fadeOutTimer.Tick += new System.EventHandler(this.close_Tick);
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.BackColor = System.Drawing.Color.Transparent;
            this.messageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageLabel.ForeColor = System.Drawing.Color.White;
            this.messageLabel.Location = new System.Drawing.Point(168, 74);
            this.messageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(168, 20);
            this.messageLabel.TabIndex = 259;
            this.messageLabel.Text = "Success message ";
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox.Image = global::Ledger.MainClassFolder.Properties.Resources.Ledger;
            this.iconPictureBox.Location = new System.Drawing.Point(48, 74);
            this.iconPictureBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(112, 87);
            this.iconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconPictureBox.TabIndex = 258;
            this.iconPictureBox.TabStop = false;
            this.iconPictureBox.Click += new System.EventHandler(this.iconPictureBox_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "success.PNG");
            this.imageList1.Images.SetKeyName(1, "warning.jpg");
            this.imageList1.Images.SetKeyName(2, "error.jpg");
            this.imageList1.Images.SetKeyName(3, "info.PNG");
            // 
            // AlertPopupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(27)))), ((int)(((byte)(28)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(680, 303);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.iconPictureBox);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AlertPopupForm";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Error";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.alert_Load);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public enum AlertType // was: \uFFFDܠ\uFFFD\uFFFD\uFFFD\uFFFDע\uFFFDᚏΥ\uFFFDג\uFFFD
        {
            success, // was: success
            info,    // was: info
            warning, // was: warning
            error,   // was: error
        }

        private void iconPictureBox_Click(object sender, EventArgs e)
        {

        }
    }

}