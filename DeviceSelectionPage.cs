using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Ledger.MainClassFolder
{
    public partial class DeviceSelectionPage : Form
    {
        readonly string[] _deviceNames =
        {
            "Ledger Stax",
            "Ledger Flex",
            "Ledger Nano Gen5",
            "Ledger Nano S",
            "Ledger Nano S Plus",
            "Ledger Nano X"
        };

        readonly string[] _deviceImageKeys =
        {
            "LedgerStax",
            "LedgerFlex",
            "LedgerNanoGen5",
            "LedgerNanoS",
            "LedgerNanoSPlus",
            "LedgerNanoX"
        };

        Panel _headerPanel;
        Panel _contentPanel;
        DeviceCard[] _cards;
        Label _title;

        static Image[] _imageCache;

        public DeviceSelectionPage()
        {
            InitializeComponent();
            Text = "Ledger Wallet";
            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.FromArgb(18, 18, 18);
            DoubleBuffered = true;
            MinimumSize = new Size(900, 600);
            AutoScaleMode = AutoScaleMode.None;

            var screen = Screen.PrimaryScreen.WorkingArea;
            Location = screen.Location;
            Size = screen.Size;

            BuildUI();
            Resize += (s, e) => LayoutCards();
        }

        void BuildUI()
        {
            // ── Header ──
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(18, 18, 18)
            };

            var prevBtn = new Label
            {
                Text = "← Previous",
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20),
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };
            prevBtn.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
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
            try
            {
                // Try loading Ledger logo from disk first, then fall back to embedded resource
                string logoPath = ResolveImagePath("Ledger.png");
                if (logoPath != null)
                    logo.Image = Image.FromFile(logoPath);
                else
                    logo.Image = Properties.Resources.Ledger;
            }
            catch { }

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
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
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

            // ── Content area ──
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(18, 18, 18),
                AutoScroll = true
            };

            _title = new Label
            {
                Text = "WHAT'S YOUR LEDGER?",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent
            };
            _contentPanel.Controls.Add(_title);

            _cards = new DeviceCard[_deviceNames.Length];
            for (int i = 0; i < _deviceNames.Length; i++)
            {
                int idx = i;
                var card = new DeviceCard(_deviceNames[i], TryLoadDeviceImage(i));
                card.CardClicked += (s, e) =>
                {
                    Hide();
                    var setupPage = new DeviceSetupPage(_deviceNames[idx]);
                    var result = setupPage.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        Show();
                    }
                    else
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                };
                _contentPanel.Controls.Add(card);
                _cards[i] = card;
            }

            // Add content panel FIRST, then header panel
            // WinForms docking fills in reverse order — last added docks first
            Controls.Add(_contentPanel);
            Controls.Add(_headerPanel);

            // Ensure header stays on top of the Z-order
            _headerPanel.BringToFront();
        }

        Image TryLoadDeviceImage(int index)
        {
            if (_imageCache == null)
                _imageCache = new Image[_deviceImageKeys.Length];

            if (_imageCache[index] != null)
                return _imageCache[index];

            try
            {
                string path = ResolveImagePath(_deviceImageKeys[index] + ".png");
                if (path != null)
                {
                    // Load into memory so the file isn't locked
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        _imageCache[index] = Image.FromStream(fs);
                    return _imageCache[index];
                }
            }
            catch { }

            return null;
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
                if (File.Exists(p))
                    return Path.GetFullPath(p);

            return null;
        }

        void SelectDevice(int index)
        {
            // No selection state — directly handle the device choice here
            // For now, just a placeholder for future navigation
        }

        void LayoutCards()
        {
            if (_cards == null || _cards.Length == 0) return;

            int areaWidth = _contentPanel.ClientSize.Width;
            int areaHeight = _contentPanel.ClientSize.Height;

            // Center title
            if (_title != null)
            {
                _title.Left = (areaWidth - _title.Width) / 2;
                _title.Top = 20;
            }

            // Each card is a vertical column that spans most of the content height
            int columnCount = _cards.Length;
            int columnWidth = areaWidth / columnCount;
            int columnTop = (_title != null) ? _title.Bottom + 20 : 80;
            int columnHeight = areaHeight - columnTop;

            for (int i = 0; i < _cards.Length; i++)
            {
                _cards[i].Size = new Size(columnWidth, columnHeight);
                _cards[i].Location = new Point(i * columnWidth, columnTop);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            LayoutCards();
        }

        // ── Device Card Control ──
        class DeviceCard : Control
        {
            string _deviceName;
            Image _deviceImage;
            bool _hovered;
            RoundedButton _selectBtn;

            // Hover animation — simplified
            Timer _animTimer;
            int _animFrame;
            int _imgOffsetY;
            static readonly int[] _offsets = PrecalcOffsets();
            const int AnimAmplitude = 8;

            public event EventHandler CardClicked;

            static int[] PrecalcOffsets()
            {
                // Pre-calculate 60 frames of sine wave offsets
                var arr = new int[60];
                for (int i = 0; i < 60; i++)
                    arr[i] = (int)(Math.Sin(i * 0.12) * AnimAmplitude);
                return arr;
            }

            public DeviceCard(string name, Image image)
            {
                _deviceName = name;
                _deviceImage = image;

                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.OptimizedDoubleBuffer,
                    true);

                Cursor = Cursors.Hand;

                _selectBtn = new RoundedButton
                {
                    Text = "Select",
                    Width = 100,
                    Height = 36,
                    FillColor = Color.White,
                    TextColor = Color.Black,
                    BorderColor = Color.White,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                    Cursor = Cursors.Hand,
                    CornerRadius = 18,
                    Visible = false
                };
                _selectBtn.Click += (s, e) => CardClicked?.Invoke(this, EventArgs.Empty);
                _selectBtn.MouseEnter += (s, e) => { SetHovered(true); };
                _selectBtn.MouseLeave += (s, e) =>
                {
                    Point pt = PointToClient(Cursor.Position);
                    if (!ClientRectangle.Contains(pt))
                        SetHovered(false);
                };
                Controls.Add(_selectBtn);

                // Slower interval = fewer redraws per second
                _animTimer = new Timer { Interval = 50 };
                _animTimer.Tick += AnimTimer_Tick;
            }

            void SetHovered(bool hovered)
            {
                if (_hovered == hovered) return;
                _hovered = hovered;

                if (_hovered)
                {
                    _animFrame = 0;
                    _selectBtn.Visible = true;
                    _animTimer.Start();
                }
                else
                {
                    _animTimer.Stop();
                    _imgOffsetY = 0;
                    _selectBtn.Visible = false;
                }

                Invalidate();
            }

            void AnimTimer_Tick(object sender, EventArgs e)
            {
                _animFrame = (_animFrame + 1) % _offsets.Length;
                int newOffset = _offsets[_animFrame];
                if (newOffset == _imgOffsetY) return; // Skip repaint if nothing changed
                _imgOffsetY = newOffset;
                // Only invalidate the image area, not the whole control
                int imgAreaTop = 40;
                int imgAreaHeight = (int)(Height * 0.45);
                Invalidate(new Rectangle(0, imgAreaTop - AnimAmplitude, Width, imgAreaHeight + AnimAmplitude * 2));
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);
                if (_selectBtn == null) return;

                int imgAreaTop = 40;
                int imgAreaHeight = (int)(Height * 0.45);
                int nameY = imgAreaTop + imgAreaHeight + 10;
                int btnY = nameY + 40;

                _selectBtn.Location = new Point(
                    (Width - _selectBtn.Width) / 2,
                    btnY);
            }

            protected override void OnMouseEnter(EventArgs e)
            {
                base.OnMouseEnter(e);
                SetHovered(true);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                base.OnMouseLeave(e);
                Point pt = PointToClient(Cursor.Position);
                if (!ClientRectangle.Contains(pt))
                    SetHovered(false);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.InterpolationMode = InterpolationMode.Low;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                // Background only on hover
                if (_hovered)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(28, 28, 28)))
                        g.FillRectangle(brush, ClientRectangle);

                    using (var pen = new Pen(Color.FromArgb(50, 50, 50), 1f))
                    {
                        g.DrawLine(pen, 0, 0, 0, Height);
                        g.DrawLine(pen, Width - 1, 0, Width - 1, Height);
                    }
                }

                // Image area
                int imgAreaTop = 40;
                int imgAreaHeight = (int)(Height * 0.45);

                if (_deviceImage != null)
                {
                    int maxW = Width - 80;
                    int maxH = imgAreaHeight - 60;

                    float scale = Math.Min((float)maxW / _deviceImage.Width,
                                           (float)maxH / _deviceImage.Height);
                    scale = Math.Min(scale, 0.6f);

                    int drawW = (int)(_deviceImage.Width * scale);
                    int drawH = (int)(_deviceImage.Height * scale);
                    int imgX = (Width - drawW) / 2;
                    int imgY = imgAreaTop + (imgAreaHeight - drawH) / 2 + _imgOffsetY;

                    g.DrawImage(_deviceImage, imgX, imgY, drawW, drawH);
                }
                else
                {
                    int phW = 60, phH = 90;
                    int phX = (Width - phW) / 2;
                    int phY = imgAreaTop + (imgAreaHeight - phH) / 2 + _imgOffsetY;
                    using (var pen = new Pen(Color.FromArgb(60, 60, 60), 1f))
                        g.DrawRectangle(pen, phX, phY, phW, phH);
                }

                // Device name
                int nameY = imgAreaTop + imgAreaHeight + 10;
                var nameRect = new Rectangle(5, nameY, Width - 10, 30);
                TextRenderer.DrawText(g, _deviceName,
                    new Font("Segoe UI", 11f, FontStyle.Regular),
                    nameRect, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _animTimer?.Stop();
                    _animTimer?.Dispose();
                }
                base.Dispose(disposing);
            }

            internal static GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
            {
                int d = radius * 2;
                var path = new GraphicsPath();
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                return path;
            }
        }

        // ── RoundedButton ──
        class RoundedButton : Control
        {
            Color _fill = Color.White;
            Color _text = Color.Black;
            Color _border = Color.White;
            int _radius = 25;
            bool _hovered;
            bool _pressed;

            public Color FillColor
            {
                get { return _fill; }
                set { _fill = value; Invalidate(); }
            }

            public Color TextColor
            {
                get { return _text; }
                set { _text = value; Invalidate(); }
            }

            public Color BorderColor
            {
                get { return _border; }
                set { _border = value; Invalidate(); }
            }

            public int CornerRadius
            {
                get { return _radius; }
                set { _radius = value; UpdateRegion(); Invalidate(); }
            }

            public RoundedButton()
            {
                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.OptimizedDoubleBuffer,
                    true);
                Size = new Size(100, 36);
            }

            void UpdateRegion()
            {
                if (Width <= 0 || Height <= 0) return;
                using (var path = DeviceCard.CreateRoundedPath(ClientRectangle, _radius))
                    Region = new Region(path);
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);
                UpdateRegion();
            }

            protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hovered = true; Invalidate(); }
            protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hovered = false; _pressed = false; Invalidate(); }
            protected override void OnMouseDown(MouseEventArgs e) { base.OnMouseDown(e); _pressed = true; Invalidate(); }
            protected override void OnMouseUp(MouseEventArgs e) { base.OnMouseUp(e); _pressed = false; Invalidate(); }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = ClientRectangle;
                rect.Width -= 1;
                rect.Height -= 1;

                using (var path = DeviceCard.CreateRoundedPath(rect, _radius))
                {
                    Color fill = _fill;
                    if (_pressed) fill = Color.FromArgb(200, 200, 200);
                    else if (_hovered) fill = Color.FromArgb(230, 230, 230);

                    using (var brush = new SolidBrush(fill))
                        e.Graphics.FillPath(brush, path);
                    using (var pen = new Pen(_border, 1.5f))
                        e.Graphics.DrawPath(pen, path);
                }

                TextRenderer.DrawText(e.Graphics, Text, Font, rect, _text,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }
    }
}