using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Ledger.MainClassFolder
{
    public partial class SplashScreen : Form
    {
        readonly Timer _timer;
        readonly PictureBox _logo;
        readonly Label _walletLabel;
        readonly FlowLayoutPanel _logoFlow;

        public SplashScreen()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            BackColor = Color.Black;
            ShowInTaskbar = true;
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;
            Text = "Ledger Wallet";

            _logoFlow = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.Transparent,
                WrapContents = false
            };

            _logo = new PictureBox
            {
                Size = new Size(48, 48),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 8, 8, 0)
            };
            try { _logo.Image = Properties.Resources.Ledger; } catch { }

            _walletLabel = new Label
            {
                Text = "WALLET",
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 4, 0, 0)
            };

            _logoFlow.Controls.Add(_logo);
            _logoFlow.Controls.Add(_walletLabel);
            Controls.Add(_logoFlow);

            Resize += (s, e) => CenterLogo();

            _timer = new Timer { Interval = 2500 };
            _timer.Tick += Timer_Tick;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            CenterLogo();
            _timer.Start();
        }

        void CenterLogo()
        {
            _logoFlow.Left = (ClientSize.Width - _logoFlow.Width) / 2;
            _logoFlow.Top = (ClientSize.Height - _logoFlow.Height) / 2;
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
            Close();
        }
    }
}
