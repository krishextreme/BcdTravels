using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Ledger.MainClassFolder
{
    public partial class PreferencesPanel : Form
    {
        bool _analyticsEnabled;
        bool _personalizationEnabled;

        public bool AnalyticsEnabled => _analyticsEnabled;
        public bool PersonalizationEnabled => _personalizationEnabled;

        public PreferencesPanel()
        {
            InitializeComponent();

            Text = "Ledger Wallet";            
            BackColor = Color.FromArgb(30, 30, 30);
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            ShowInTaskbar = false;

            BuildContent();
        }

        void BuildContent()
        {
            int yPos = 20;

            // ── Close button ──
            var closeBtn = new Label
            {
                Text = "✕",
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                ForeColor = Color.FromArgb(180, 180, 180),
                AutoSize = true,
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            closeBtn.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
            closeBtn.MouseEnter += (s, e) => closeBtn.ForeColor = Color.White;
            closeBtn.MouseLeave += (s, e) => closeBtn.ForeColor = Color.FromArgb(180, 180, 180);
            Controls.Add(closeBtn);

            // ── Back button ──
            var backBtn = new Label
            {
                Text = "‹ Back",
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = Color.FromArgb(180, 180, 180),
                AutoSize = true,
                Location = new Point(15, yPos),
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };
            backBtn.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
            backBtn.MouseEnter += (s, e) => backBtn.ForeColor = Color.White;
            backBtn.MouseLeave += (s, e) => backBtn.ForeColor = Color.FromArgb(180, 180, 180);
            Controls.Add(backBtn);
            yPos += 40;

            // ── Title ──
            var title = new Label
            {
                Text = "Manage your preferences",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(440, 40),
                Location = new Point(30, yPos),
                BackColor = Color.Transparent
            };
            Controls.Add(title);
            yPos += 55;

            // ── Analytics toggle row ──
            var analyticsToggle = new ToggleSwitch { Checked = false };
            var analyticsRow = CreateToggleRow("Analytics", analyticsToggle, yPos);
            Controls.Add(analyticsRow);
            yPos += analyticsRow.Height + 10;

            var analyticsDesc = new Label
            {
                Text = "To measure Ledger Wallet's performance and improve both " +
                       "the app and your experience, we share usage data, including " +
                       "page visits and clicks.",
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.FromArgb(160, 160, 160),
                AutoSize = false,
                Size = new Size(440, 55),
                Location = new Point(30, yPos),
                BackColor = Color.Transparent
            };
            Controls.Add(analyticsDesc);
            yPos += 70;

            // ── Personalization toggle row ──
            var personalizationToggle = new ToggleSwitch { Checked = false };
            var personalizationRow = CreateToggleRow("Personalization", personalizationToggle, yPos);
            Controls.Add(personalizationRow);
            yPos += personalizationRow.Height + 10;

            var personalizationDesc = new Label
            {
                Text = "To receive personalized recommendations and content that " +
                       "match your preferences and to help us measure the " +
                       "performance of our marketing campaigns, we share app usage " +
                       "data, including clicks, page visits, and conversion data.",
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.FromArgb(160, 160, 160),
                AutoSize = false,
                Size = new Size(440, 70),
                Location = new Point(30, yPos),
                BackColor = Color.Transparent
            };
            Controls.Add(personalizationDesc);
            yPos += 85;

            // ── Revoke note ──
            var revokeNote = new Label
            {
                Text = "You can revoke your consent any time in the app settings.",
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                ForeColor = Color.FromArgb(130, 130, 130),
                AutoSize = false,
                Size = new Size(440, 22),
                Location = new Point(30, yPos),
                BackColor = Color.Transparent
            };
            Controls.Add(revokeNote);
            yPos += 22;

            // ── Learn more link ──
            var learnMore = new LinkLabel
            {
                Text = "Learn more about how we handle your data",
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                LinkColor = Color.FromArgb(100, 160, 255),
                ActiveLinkColor = Color.FromArgb(140, 190, 255),
                VisitedLinkColor = Color.FromArgb(100, 160, 255),
                AutoSize = true,
                Location = new Point(30, yPos),
                BackColor = Color.Transparent
            };
            learnMore.LinkClicked += (s, e) =>
                System.Diagnostics.Process.Start("https://www.ledger.com/privacy-policy");
            Controls.Add(learnMore);

            // ── Bottom separator + validate button ──
            var separator = new Panel
            {
                BackColor = Color.FromArgb(60, 60, 60),
                Dock = DockStyle.Bottom,
                Height = 1
            };
            Controls.Add(separator);

            var btnPanel = new Panel
            {
                BackColor = Color.FromArgb(30, 30, 30),
                Dock = DockStyle.Bottom,
                Height = 70
            };
            Controls.Add(btnPanel);

            var validateBtn = new RoundedButton
            {
                Text = "Validate",
                Height = 44,
                FillColor = Color.White,
                TextColor = Color.Black,
                BorderColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                Cursor = Cursors.Hand,
                CornerRadius = 22
            };
            validateBtn.Click += (s, e) =>
            {
                _analyticsEnabled = analyticsToggle.Checked;
                _personalizationEnabled = personalizationToggle.Checked;
                DialogResult = DialogResult.OK;
                Close();
            };
            btnPanel.Controls.Add(validateBtn);

            btnPanel.Resize += (s, e) =>
            {
                validateBtn.Width = btnPanel.Width - 30;
                validateBtn.Location = new Point(15, 13);
            };

            Resize += (s, e) =>
            {
                closeBtn.Location = new Point(Width - closeBtn.Width - 15, 10);
            };

            closeBtn.BringToFront();
        }

        Panel CreateToggleRow(string labelText, ToggleSwitch toggle, int y)
        {
            var row = new Panel
            {
                Location = new Point(30, y),
                Size = new Size(440, 40),
                BackColor = Color.FromArgb(42, 42, 42)
            };

            var lbl = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(12, 10),
                BackColor = Color.Transparent
            };
            row.Controls.Add(lbl);

            toggle.Location = new Point(row.Width - toggle.Width - 12, 8);
            toggle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            row.Controls.Add(toggle);

            return row;
        }

        // ── Self-contained toggle switch ──
        class ToggleSwitch : Control
        {
            bool _checked;
            bool _hovered;

            public bool Checked
            {
                get { return _checked; }
                set { _checked = value; Invalidate(); }
            }

            public event EventHandler CheckedChanged;

            public ToggleSwitch()
            {
                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.OptimizedDoubleBuffer,
                    true);
                Size = new Size(44, 24);
                Cursor = Cursors.Hand;
            }

            protected override void OnClick(EventArgs e)
            {
                base.OnClick(e);
                _checked = !_checked;
                CheckedChanged?.Invoke(this, EventArgs.Empty);
                Invalidate();
            }

            protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hovered = true; Invalidate(); }
            protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hovered = false; Invalidate(); }

            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                int trackHeight = Height;
                int trackRadius = trackHeight / 2;
                var trackRect = new Rectangle(0, 0, Width - 1, trackHeight - 1);

                Color trackColor = _checked
                    ? Color.FromArgb(100, 80, 220)
                    : Color.FromArgb(80, 80, 80);

                if (_hovered)
                    trackColor = _checked
                        ? Color.FromArgb(120, 100, 240)
                        : Color.FromArgb(100, 100, 100);

                using (var path = CreatePillPath(trackRect, trackRadius))
                using (var brush = new SolidBrush(trackColor))
                    g.FillPath(brush, path);

                int thumbDiameter = trackHeight - 6;
                int thumbX = _checked ? Width - thumbDiameter - 3 : 3;
                int thumbY = 3;

                using (var brush = new SolidBrush(Color.White))
                    g.FillEllipse(brush, thumbX, thumbY, thumbDiameter, thumbDiameter);
            }

            static GraphicsPath CreatePillPath(Rectangle rect, int radius)
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

        // ── Rounded button (matching existing project style) ──
        class RoundedButton : Control
        {
            Color _fill = Color.White;
            Color _text = Color.Black;
            Color _border = Color.White;
            int _radius = 25;
            bool _hovered;
            bool _pressed;

            public Color FillColor { get { return _fill; } set { _fill = value; Invalidate(); } }
            public Color TextColor { get { return _text; } set { _text = value; Invalidate(); } }
            public Color BorderColor { get { return _border; } set { _border = value; Invalidate(); } }
            public int CornerRadius { get { return _radius; } set { _radius = value; UpdateRegion(); Invalidate(); } }

            public RoundedButton()
            {
                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.OptimizedDoubleBuffer,
                    true);
                Size = new Size(100, 44);
            }

            void UpdateRegion()
            {
                if (Width <= 0 || Height <= 0) return;
                using (var path = CreateRoundedPath(ClientRectangle, _radius))
                    Region = new Region(path);
            }

            protected override void OnSizeChanged(EventArgs e) { base.OnSizeChanged(e); UpdateRegion(); }
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

                using (var path = CreateRoundedPath(rect, _radius))
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

            static GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
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
    }
}