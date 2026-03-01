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
        int _selectedIndex = -1;
        DeviceCard[] _cards;
        Label _title;

        public DeviceSelectionPage()
        {
            Text = "Ledger Wallet";
            WindowState = FormWindowState.Maximized;
            BackColor = Color.FromArgb(18, 18, 18);
            DoubleBuffered = true;
            MinimumSize = new Size(900, 600);

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
                card.CardClicked += (s, e) => SelectDevice(idx);
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
            try
            {
                string path = ResolveImagePath(_deviceImageKeys[index] + ".png");
                if (path != null)
                    return Image.FromFile(path);

                Debug.WriteLine($"[DeviceSelectionPage] Image not found: {_deviceImageKeys[index]}.png");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DeviceSelectionPage] Error loading {_deviceImageKeys[index]}: {ex.Message}");
            }

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
            if (_selectedIndex == index)
                _selectedIndex = -1;
            else
                _selectedIndex = index;

            for (int i = 0; i < _cards.Length; i++)
            {
                _cards[i].IsSelected = (i == _selectedIndex);
            }
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
            bool _isSelected;
            bool _hovered;
            RoundedButton _selectBtn;

            public event EventHandler CardClicked;

            public bool IsSelected
            {
                get { return _isSelected; }
                set
                {
                    _isSelected = value;
                    if (_selectBtn != null)
                        _selectBtn.Visible = value;
                    Invalidate();
                }
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
                // Forward mouse events from the button back to the card so hover stays active
                _selectBtn.MouseEnter += (s, e) => { _hovered = true; Invalidate(); };
                _selectBtn.MouseLeave += (s, e) =>
                {
                    // Only clear hover if the mouse truly left the card bounds
                    Point pt = PointToClient(Cursor.Position);
                    if (!ClientRectangle.Contains(pt))
                    {
                        _hovered = false;
                        Invalidate();
                    }
                };
                Controls.Add(_selectBtn);
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

            protected override void OnClick(EventArgs e)
            {
                base.OnClick(e);
                CardClicked?.Invoke(this, EventArgs.Empty);
            }

            protected override void OnMouseEnter(EventArgs e)
            {
                base.OnMouseEnter(e);
                _hovered = true;
                Invalidate();
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                base.OnMouseLeave(e);
                // Only clear hover if the mouse truly left the card (not entering a child)
                Point pt = PointToClient(Cursor.Position);
                if (!ClientRectangle.Contains(pt))
                {
                    _hovered = false;
                    Invalidate();
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                // Full-height column background
                if (_isSelected)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(32, 32, 32)))
                        g.FillRectangle(brush, ClientRectangle);

                    using (var pen = new Pen(Color.FromArgb(50, 50, 50), 1f))
                    {
                        g.DrawLine(pen, 0, 0, 0, Height);
                        g.DrawLine(pen, Width - 1, 0, Width - 1, Height);
                    }
                }
                else if (_hovered)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(25, 25, 25)))
                        g.FillRectangle(brush, ClientRectangle);
                }

                // Image area
                int imgAreaTop = 40;
                int imgAreaHeight = (int)(Height * 0.45);

                if (_deviceImage != null)
                {
                    int maxW = Width - 40;
                    int maxH = imgAreaHeight - 20;

                    float scale = Math.Min((float)maxW / _deviceImage.Width,
                                           (float)maxH / _deviceImage.Height);
                    int drawW = (int)(_deviceImage.Width * scale);
                    int drawH = (int)(_deviceImage.Height * scale);
                    int imgX = (Width - drawW) / 2;
                    int imgY = imgAreaTop + (imgAreaHeight - drawH) / 2;

                    g.DrawImage(_deviceImage, imgX, imgY, drawW, drawH);
                }
                else
                {
                    int phW = 80, phH = 120;
                    int phX = (Width - phW) / 2;
                    int phY = imgAreaTop + (imgAreaHeight - phH) / 2;
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