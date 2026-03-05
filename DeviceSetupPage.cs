using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Ledger.MainClassFolder
{
    public partial class DeviceSetupPage : Form
    {
        string _deviceName;
        Panel _headerPanel;
        Panel _contentPanel;
        Panel _innerPanel;
        int _scrollY;
        bool _allowClose;

        public DeviceSetupPage(string deviceName)
        {
            InitializeComponent();

            _deviceName = deviceName;

            Text = "Ledger Wallet";
            // FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.FromArgb(18, 18, 18);
            DoubleBuffered = true;
            MinimumSize = new Size(900, 600);
            AutoScaleMode = AutoScaleMode.None;

            // Fill the working area (screen minus taskbar)
            var screen = Screen.PrimaryScreen.WorkingArea;
            Location = screen.Location;
            Size = screen.Size;

            BuildUI();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_allowClose && DialogResult == DialogResult.None)
            {
                e.Cancel = true;
                return;
            }
            base.OnFormClosing(e);
        }

        void BuildUI()
        {
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(18, 18, 18)
            };

            var prevBtn = new Label
            {
                Text = "← Previous",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20),
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };
            prevBtn.Click += (s, e) => { _allowClose = true; DialogResult = DialogResult.Cancel; Close(); };
            prevBtn.MouseEnter += (s, e) => prevBtn.ForeColor = Color.FromArgb(180, 180, 180);
            prevBtn.MouseLeave += (s, e) => prevBtn.ForeColor = Color.White;
            _headerPanel.Controls.Add(prevBtn);

            var logoFlow = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.Transparent,
                WrapContents = false
            };
            var logo = new PictureBox
            {
                Size = new Size(28, 28),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 4, 4, 0)
            };
            try { logo.Image = Properties.Resources.Ledger; } catch { }
            var walletLabel = new Label
            {
                Text = "WALLET",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 2, 0, 0)
            };
            logoFlow.Controls.Add(logo);
            logoFlow.Controls.Add(walletLabel);
            _headerPanel.Controls.Add(logoFlow);

            var langLabel = new Label
            {
                Text = "English ⌵",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _headerPanel.Controls.Add(langLabel);
            _headerPanel.Resize += (s, e) =>
            {
                logoFlow.Left = (_headerPanel.Width - logoFlow.Width) / 2;
                logoFlow.Top = 15;
                langLabel.Location = new Point(_headerPanel.Width - langLabel.Width - 20, 20);
            };

            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(18, 18, 18)
            };
            _innerPanel = new Panel
            {
                BackColor = Color.FromArgb(18, 18, 18),
                Location = new Point(0, 0)
            };
            _contentPanel.Controls.Add(_innerPanel);

            // Fill first, then Top — ensures content panel sits below the header
            Controls.Add(_contentPanel);
            Controls.Add(_headerPanel);

            _scrollY = 0;
            MouseWheel += OnScroll;
            _contentPanel.MouseWheel += OnScroll;
            _innerPanel.MouseWheel += OnScroll;

            // ── Section 1: Single card ──
            var section1 = CreateSingleCardSection(
                $"FIRST TIME USING\nYOUR {_deviceName.ToUpper().Replace("LEDGER ", "")}?",
                new SetupCardInfo
                {
                    Title = $"SETUP A NEW {_deviceName.ToUpper().Replace("LEDGER ", "")}",
                    Description = "Let's start and set up your device",
                    ImageKey = "1"
                });
            _innerPanel.Controls.Add(section1);

            var sep = new Panel { BackColor = Color.FromArgb(40, 40, 40), Height = 1 };
            _innerPanel.Controls.Add(sep);

            // ── Section 2: Two cards stacked ──
            var section2 = CreateTwoCardSection(
                $"ALREADY HAVE A\nRECOVERY PHRASE?",
                new SetupCardInfo
                {
                    Title = $"CONNECT YOUR {_deviceName.ToUpper().Replace("LEDGER ", "")}",
                    Description = "Is your device already set up? Connect it to the app!",
                    ImageKey = "2"
                },
                new SetupCardInfo
                {
                    Title = "RESTORE YOUR RECOVERY\nPHRASE ON A NEW DEVICE",
                    Description = $"Use an existing recovery phrase to restore your private keys on a new {_deviceName.Replace("Ledger ", "")}!",
                    ImageKey = "3"
                });
            _innerPanel.Controls.Add(section2);

            _contentPanel.Resize += (s, e) => LayoutContent(section1, sep, section2);
        }

        void OnScroll(object sender, MouseEventArgs e)
        {
            int maxScroll = Math.Max(0, _innerPanel.Height - _contentPanel.ClientSize.Height);
            _scrollY -= e.Delta;
            _scrollY = Math.Max(0, Math.Min(_scrollY, maxScroll));
            _innerPanel.Top = -_scrollY;
        }

        void LayoutContent(Panel section1, Panel sep, Panel section2)
        {
            int w = _contentPanel.ClientSize.Width;
            int viewH = _contentPanel.ClientSize.Height;

            int section1H = (int)(viewH * 0.5);
            int section2H = viewH; // Full viewport height for 2 stacked cards
            int y = 0;

            section1.Bounds = new Rectangle(0, y, w, section1H);
            y += section1H;
            sep.Bounds = new Rectangle(0, y, w, 1);
            y += 1;
            section2.Bounds = new Rectangle(0, y, w, section2H);
            y += section2H;

            _innerPanel.Size = new Size(w, y);
            int maxScroll = Math.Max(0, y - viewH);
            _scrollY = Math.Min(_scrollY, maxScroll);
            _innerPanel.Top = -_scrollY;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            var ctrls = _innerPanel.Controls;
            if (ctrls.Count >= 3)
                LayoutContent((Panel)ctrls[0], (Panel)ctrls[1], (Panel)ctrls[2]);
        }

        // ── Section with one card ──
        Panel CreateSingleCardSection(string titleText, SetupCardInfo cardInfo)
        {
            var panel = new Panel { BackColor = Color.FromArgb(18, 18, 18) };

            var title = new Label
            {
                Text = titleText,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(80, 60)
            };
            panel.Controls.Add(title);

            var card = CreateOptionCard(cardInfo);
            card.ArrowClicked += (s, e) => OpenRecoveryPage();
            panel.Controls.Add(card);

            panel.Resize += (s, e) =>
            {
                int cardW = 280;
                int vertPad = 30;
                int cardH = panel.Height - vertPad * 2;
                int cardX = panel.Width - cardW - 100;
                card.SetBounds(cardX, vertPad, cardW, cardH);
            };

            return panel;
        }

        // ── Section with two stacked cards ──
        Panel CreateTwoCardSection(string titleText, SetupCardInfo card1Info, SetupCardInfo card2Info)
        {
            var panel = new Panel { BackColor = Color.FromArgb(18, 18, 18) };

            var title = new Label
            {
                Text = titleText,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(80, 60)
            };
            panel.Controls.Add(title);

            var card1 = CreateOptionCard(card1Info);
            var card2 = CreateOptionCard(card2Info);

            // Both arrows open RecoveryPhrasePage
            card1.ArrowClicked += (s, e) => OpenRecoveryPage();
            card2.ArrowClicked += (s, e) => OpenRecoveryPage();

            panel.Controls.Add(card1);
            panel.Controls.Add(card2);

            panel.Resize += (s, e) =>
            {
                int cardW = 280;
                int gap = 10;
                int vertPad = 30;
                int cardH = (panel.Height - vertPad * 2 - gap) / 2;
                int cardX = panel.Width - cardW - 100;

                card1.SetBounds(cardX, vertPad, cardW, cardH);
                card2.SetBounds(cardX, vertPad + cardH + gap, cardW, cardH);
            };

            return panel;
        }

        void OpenRecoveryPage()
        {
            Enabled = false;
            using (var recoveryPage = new RecoveryPhrasePage(_deviceName))
            {
                var result = recoveryPage.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    _allowClose = true;
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    Enabled = true;
                    BringToFront();
                    Activate();
                }
            }
        }

        OptionCard CreateOptionCard(SetupCardInfo info)
        {
            Image img = null;
            try
            {
                string path = ResolveImagePath(info.ImageKey + ".png");
                if (path != null)
                    img = Image.FromFile(path);
            }
            catch { }

            return new OptionCard(info.Title, info.Description, img);
        }

        string ResolveImagePath(string file)
        {
            string exe = Application.StartupPath;
            string[] candidates =
            {
                Path.Combine(exe, file),
                Path.Combine(exe, "Resources", "Images", file),
                Path.Combine(exe, "..", "..", "..", "Resources", "Images", file),
                Path.Combine(exe, "..", "..", "Resources", "Images", file)
            };
            foreach (var p in candidates)
                if (File.Exists(p)) return Path.GetFullPath(p);
            return null;
        }

        struct SetupCardInfo
        {
            public string Title;
            public string Description;
            public string ImageKey;
        }

        class OptionCard : Control
        {
            string _title, _description;
            Image _image;
            bool _hovered;
            Rectangle _arrowRect;
            bool _arrowHovered;

            public event EventHandler ArrowClicked;

            public OptionCard(string title, string description, Image image)
            {
                _title = title;
                _description = description;
                _image = image;
                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.OptimizedDoubleBuffer,
                    true);
                Cursor = Cursors.Default;
            }

            protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hovered = true; Invalidate(); }
            protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hovered = false; _arrowHovered = false; Invalidate(); }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                base.OnMouseMove(e);
                bool overArrow = _arrowRect.Contains(e.Location);
                if (overArrow != _arrowHovered)
                {
                    _arrowHovered = overArrow;
                    Cursor = _arrowHovered ? Cursors.Hand : Cursors.Default;
                    Invalidate(_arrowRect);
                }
            }

            protected override void OnClick(EventArgs e)
            {
                base.OnClick(e);
                var pos = PointToClient(Cursor.Position);
                if (_arrowRect.Contains(pos))
                    ArrowClicked?.Invoke(this, EventArgs.Empty);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.Clear(Color.FromArgb(18, 18, 18));

                Color borderColor = _hovered ? Color.FromArgb(80, 80, 80) : Color.FromArgb(45, 45, 45);
                using (var pen = new Pen(borderColor, 1f))
                    g.DrawRectangle(pen, 1, 1, Width - 3, Height - 3);

                int pad = 16;
                int left = pad + 1;
                int yPos = pad + 1;

                if (_image != null)
                {
                    int imgMaxW = Width - left * 2;
                    int imgMaxH = (int)(Height * 0.42);
                    float scale = Math.Min((float)imgMaxW / _image.Width, (float)imgMaxH / _image.Height);
                    int drawW = (int)(_image.Width * scale);
                    int drawH = (int)(_image.Height * scale);
                    g.DrawImage(_image, left + (imgMaxW - drawW) / 2, yPos, drawW, drawH);
                    yPos += drawH + 10;
                }
                else
                {
                    yPos += (int)(Height * 0.42) + 10;
                }

                using (var f = new Font("Segoe UI", 10f, FontStyle.Bold))
                {
                    var r = new Rectangle(left, yPos, Width - left * 2, 50);
                    TextRenderer.DrawText(g, _title, f, r, Color.White,
                        TextFormatFlags.Left | TextFormatFlags.WordBreak);
                    yPos += TextRenderer.MeasureText(g, _title, f, r.Size,
                        TextFormatFlags.Left | TextFormatFlags.WordBreak).Height + 4;
                }

                using (var f = new Font("Segoe UI", 8.5f))
                {
                    var r = new Rectangle(left, yPos, Width - left * 2, 40);
                    TextRenderer.DrawText(g, _description, f, r, Color.FromArgb(155, 155, 155),
                        TextFormatFlags.Left | TextFormatFlags.WordBreak);
                }

                // Filled white circle with dark arrow
                int aSize = 38;
                int aX = Width - left - aSize - 2;
                int aY = Height - left - aSize - 2;
                _arrowRect = new Rectangle(aX, aY, aSize, aSize);

                using (var path = new GraphicsPath())
                {
                    path.AddEllipse(_arrowRect);
                    Color fill = _arrowHovered ? Color.FromArgb(200, 200, 200) : Color.White;
                    using (var brush = new SolidBrush(fill))
                        g.FillPath(brush, path);
                }

                using (var f = new Font("Segoe UI", 13f))
                    TextRenderer.DrawText(g, "→", f, _arrowRect, Color.FromArgb(30, 30, 30),
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }
    }
}