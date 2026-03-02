using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Ledger.MainClassFolder
{
    public partial class MainLandingPage : Form
    {
        LibVLC _lib;
        MediaPlayer _player;
        Media _media;
        VideoView _video;

        OverlayForm _overlayForm;

        string[] _videos;
        int _index;

        SynchronizationContext _ui;

        readonly string[] _titles =
        {
            "A wallet that protects and puts you in control",
            "Send,receive,swap and stake thousands of crypto",
            "Verify all your transactions with peace of mind"
        };

        public MainLandingPage()
        {
            InitializeComponent();

            WindowState = FormWindowState.Maximized;
            BackColor = Color.Black;

            try { Core.Initialize(); } catch { }

            _video = new VideoView { Dock = DockStyle.Fill };
            Controls.Add(_video);

            Load += MainLandingPage_Load;
            Move += (s, e) => SyncOverlay();
            Resize += (s, e) => SyncOverlay();
            FormClosed += (s, e) => _overlayForm?.Close();
        }

        void MainLandingPage_Load(object sender, EventArgs e)
        {
            _ui = SynchronizationContext.Current;

            _overlayForm = new OverlayForm();
            _overlayForm.Owner = this;
            _overlayForm.Show();
            SyncOverlay();

            _videos = new[]
            {
                "ledgerWalletBuySell-1.webm",
                "ledgerWalletThousandsCrypto2.webm",
                "ledgerWalletSecureWallet3.webm"
            }
            .Select(ResolveVideoPath)
            .ToArray();

            if (_videos.Any(v => string.IsNullOrEmpty(v)))
            {
                MessageBox.Show(
                    "One or more videos not found.\nSearched in:\n" +
                    Path.Combine(Application.StartupPath, "Resources", "Videos"),
                    "Missing videos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                InitVLC();
            }
        }

        void SyncOverlay()
        {
            if (_overlayForm == null || !_overlayForm.Visible) return;

            var rect = RectangleToScreen(ClientRectangle);
            _overlayForm.Bounds = rect;
        }

        string ResolveVideoPath(string file)
        {
            string exe = Application.StartupPath;

            string[] candidates =
            {
                Path.Combine(exe, file),
                Path.Combine(exe, "Resources", "Videos", file),
                Path.Combine(exe, "..", "..", "..", "Resources", "Videos", file),
                Path.Combine(exe, "..", "..", "Resources", "Videos", file)
            };

            foreach (var p in candidates)
                if (File.Exists(p))
                    return Path.GetFullPath(p);

            return null;
        }

        #region VLC

        void InitVLC()
        {
            _lib = new LibVLC("--no-audio");
            _player = new MediaPlayer(_lib);
            _video.MediaPlayer = _player;

            _player.PositionChanged += (s, e) =>
                Post(() => _overlayForm?.SetProgress(_index, e.Position));

            _player.EndReached += (s, e) =>
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Post(() =>
                    {
                        _overlayForm?.SetProgress(_index, 1);
                        _index++;

                        if (_index >= _videos.Length)
                        {
                            _index = 0;
                            _overlayForm?.ResetProgress();
                        }

                        Play(_index);
                    });
                });
            };

            Play(0);
        }

        void Play(int i)
        {
            if (_videos == null || i >= _videos.Length) return;
            if (string.IsNullOrEmpty(_videos[i]) || !File.Exists(_videos[i])) return;

            if (_media != null) _media.Dispose();

            _media = new Media(_lib, _videos[i], FromType.FromPath);
            _player.Play(_media);

            if (i < _titles.Length)
                _overlayForm?.SetHeadline(_titles[i]);
        }

        void Post(Action a)
        {
            if (_ui != null)
                _ui.Post(_ => a(), null);
        }

        #endregion

        #region Overlay Form

        class OverlayForm : Form
        {
            static readonly Color KeyColor = Color.FromArgb(1, 1, 1);

            FlowLayoutPanel _logoFlow;
            PictureBox _logo;
            Label _wallet;

            SegmentedProgress _progress;
            Label _headline;
            LinkLabel _footer;

            RoundedButton _start;
            RoundedButton _buy;
            RoundedButton _sync;

            DimPanel _dimOverlay;
            ConsentPanel _consentPanel;
            System.Windows.Forms.Timer _slideTimer;
            int _slideTarget;
            int _slideStep = 30;

            public OverlayForm()
            {
                FormBorderStyle = FormBorderStyle.None;
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.Manual;
                BackColor = KeyColor;
                TransparencyKey = KeyColor;
                DoubleBuffered = true;

                BuildUI();

                Resize += (s, e) => LayoutUI();
            }

            void BuildUI()
            {
                _logoFlow = new FlowLayoutPanel
                {
                    AutoSize = true,
                    FlowDirection = FlowDirection.LeftToRight,
                    BackColor = KeyColor,
                    WrapContents = false
                };
                Controls.Add(_logoFlow);

                _logo = new PictureBox
                {
                    Size = new Size(34, 34),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = KeyColor,
                    Margin = new Padding(0, 5, 4, 0)
                };

                try { _logo.Image = Properties.Resources.Ledger; } catch { }

                _wallet = new Label
                {
                    Text = "WALLET",
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 18, FontStyle.Bold),
                    AutoSize = true,
                    BackColor = KeyColor,
                    Margin = new Padding(0, 3, 0, 0)
                };

                _logoFlow.Controls.Add(_logo);
                _logoFlow.Controls.Add(_wallet);

                _progress = new SegmentedProgress { BackColor = KeyColor };
                Controls.Add(_progress);

                _headline = new Label
                {
                    Text = "",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(200, 200, 200),
                    AutoSize = true,
                    BackColor = KeyColor
                };
                Controls.Add(_headline);

                _start = new RoundedButton
                {
                    Text = "Get started",
                    Width = 280,
                    Height = 50,
                    FillColor = Color.White,
                    TextColor = Color.Black,
                    BorderColor = Color.White,
                    Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                    Cursor = Cursors.Hand,
                    CornerRadius = 25
                };
                _start.Click += (s, e) => ShowConsentPanel();
                Controls.Add(_start);

                _buy = new RoundedButton
                {
                    Text = "No device? Buy a Ledger",
                    Width = 280,
                    Height = 50,
                    FillColor = Color.FromArgb(35, 35, 35),
                    TextColor = Color.White,
                    BorderColor = Color.FromArgb(80, 80, 80),
                    Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                    Cursor = Cursors.Hand,
                    CornerRadius = 25
                };
                Controls.Add(_buy);

                _sync = new RoundedButton
                {
                    Text = "Sync with another Ledger Wallet app",
                    Width = 340,
                    Height = 44,
                    FillColor = KeyColor,
                    TextColor = Color.White,
                    BorderColor = KeyColor,
                    HoverFillColor = Color.FromArgb(50, 50, 50),
                    HoverBorderColor = Color.FromArgb(80, 80, 80),
                    Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    CornerRadius = 22
                };
                Controls.Add(_sync);

                _footer = new LinkLabel
                {
                    Text = "By continuing, you agree to Ledger's Terms & Conditions and Privacy Policy. Ledger provides no financial advice.\n" +
                           "Swap and staking services are provided by third parties.",
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                    LinkColor = Color.White,
                    ActiveLinkColor = Color.LightGray,
                    VisitedLinkColor = Color.White,
                    BackColor = KeyColor,
                    AutoSize = false,
                    TextAlign = ContentAlignment.TopCenter,
                    Size = new Size(780, 55)
                };

                int tcStart = _footer.Text.IndexOf("Terms & Conditions");
                _footer.Links.Add(tcStart, "Terms & Conditions".Length, "https://www.ledger.com/terms-and-conditions");

                int ppStart = _footer.Text.IndexOf("Privacy Policy");
                _footer.Links.Add(ppStart, "Privacy Policy".Length, "https://www.ledger.com/privacy-policy");

                _footer.LinkClicked += (s, ev) =>
                {
                    if (ev.Link.LinkData is string url)
                        System.Diagnostics.Process.Start(url);
                };

                Controls.Add(_footer);

                _dimOverlay = new DimPanel
                {
                    Visible = false,
                    Cursor = Cursors.Hand
                };
                _dimOverlay.Click += (s, e) => HideConsentPanelOnly();
                Controls.Add(_dimOverlay);
                _dimOverlay.BringToFront();

                _consentPanel = new ConsentPanel();
                _consentPanel.CloseRequested += (s, e) => HideConsentPanel();
                Controls.Add(_consentPanel);
                _consentPanel.BringToFront();

                _slideTimer = new System.Windows.Forms.Timer { Interval = 12 };
                _slideTimer.Tick += SlideTimer_Tick;
            }

            void ShowConsentPanel()
            {
                if (_consentPanel.Visible && _consentPanel.Left < ClientSize.Width)
                    return;

                _dimOverlay.Bounds = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
                _dimOverlay.Visible = true;
                _dimOverlay.BringToFront();

                int panelWidth = Math.Min(520, ClientSize.Width);
                _consentPanel.Size = new Size(panelWidth, ClientSize.Height);
                _consentPanel.Left = ClientSize.Width;
                _consentPanel.Top = 0;
                _consentPanel.Visible = true;
                _consentPanel.BringToFront();

                _slideTarget = ClientSize.Width - panelWidth;
                _slideTimer.Start();
            }

            void HideConsentPanel()
            {
                _slideTarget = ClientSize.Width;
                _navigateAfterClose = true;
                _slideTimer.Start();
            }

            void HideConsentPanelOnly()
            {
                _slideTarget = ClientSize.Width;
                _navigateAfterClose = false;
                _slideTimer.Start();
            }

            bool _navigateAfterClose;

            void SlideTimer_Tick(object sender, EventArgs e)
            {
                int current = _consentPanel.Left;
                int diff = _slideTarget - current;

                if (Math.Abs(diff) <= _slideStep)
                {
                    _consentPanel.Left = _slideTarget;
                    _slideTimer.Stop();

                    if (_slideTarget >= ClientSize.Width)
                    {
                        _consentPanel.Visible = false;
                        _dimOverlay.Visible = false;

                        if (_navigateAfterClose)
                        {
                            _navigateAfterClose = false;
                            OpenDeviceSelection();
                        }
                    }
                }
                else
                {
                    _consentPanel.Left += diff > 0 ? _slideStep : -_slideStep;
                }
            }

            void OpenDeviceSelection()
            {
                var mainForm = Owner as MainLandingPage;
                if (mainForm == null) return;

                mainForm.Hide();
                Visible = false;

                var devicePage = new DeviceSelectionPage();
                var result = devicePage.ShowDialog();

                if (result == DialogResult.Cancel)
                {
                    // User clicked "Previous" — come back to landing page
                    mainForm.Show();
                    Visible = true;
                }
            }

            void LayoutUI()
            {
                if (!IsHandleCreated || ClientSize.Width == 0) return;

                int cx = ClientSize.Width / 2;
                int bottom = ClientSize.Height;

                _logoFlow.Left = cx - _logoFlow.Width / 2;
                _logoFlow.Top = 14;

                _progress.Left = cx - _progress.Width / 2;
                _progress.Top = _logoFlow.Bottom + 8;

                _headline.Left = cx - _headline.Width / 2;
                _headline.Top = _progress.Bottom + 14;

                int buttonY = bottom - 180;
                _start.Location = new Point(cx - _start.Width - 10, buttonY);
                _buy.Location = new Point(cx + 10, buttonY);

                _sync.Left = cx - _sync.Width / 2;
                _sync.Top = buttonY + _start.Height + 14;

                _footer.Left = cx - _footer.Width / 2;
                _footer.Top = bottom - 70;

                if (_consentPanel.Visible)
                {
                    _dimOverlay.Bounds = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);

                    int panelWidth = Math.Min(520, ClientSize.Width);
                    _consentPanel.Size = new Size(panelWidth, ClientSize.Height);
                    _slideTarget = ClientSize.Width - panelWidth;
                    _consentPanel.Left = _slideTarget;
                    _consentPanel.Top = 0;
                }
            }

            public void SetProgress(int index, double value)
            {
                _progress.Set(index, value);
            }

            public void ResetProgress()
            {
                _progress.ResetAll();
            }

            public void SetHeadline(string text)
            {
                _headline.Text = text;
                LayoutUI();
            }
        }

        #endregion

        #region Dim Overlay

        /// <summary>
        /// A control that paints a semi-transparent dark fill.
        /// Cannot use BackColor alpha on standard Panel, so we owner-draw it.
        /// </summary>
        class DimPanel : Control
        {
            public DimPanel()
            {
                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.OptimizedDoubleBuffer,
                    true);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                using (var brush = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
                {
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
            }
        }

        #endregion

        #region Consent Panel

        class ConsentPanel : Panel
        {
            public event EventHandler CloseRequested;

            public ConsentPanel()
            {
                Visible = false;
                BackColor = Color.FromArgb(30, 30, 30);
                AutoScroll = true;
                Padding = new Padding(30, 20, 30, 20);

                BuildContent();
            }

            void BuildContent()
            {
                int yPos = 20;

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
                closeBtn.Click += (s, e) => CloseRequested?.Invoke(this, EventArgs.Empty);
                closeBtn.MouseEnter += (s, e) => closeBtn.ForeColor = Color.White;
                closeBtn.MouseLeave += (s, e) => closeBtn.ForeColor = Color.FromArgb(180, 180, 180);
                Controls.Add(closeBtn);

                var title = new Label
                {
                    Text = "Help us improve and personalize your experience",
                    Font = new Font("Segoe UI", 16, FontStyle.Bold),
                    ForeColor = Color.White,
                    AutoSize = false,
                    Size = new Size(440, 60),
                    Location = new Point(30, yPos),
                    BackColor = Color.Transparent
                };
                Controls.Add(title);
                yPos += 70;

                var subtitle = new Label
                {
                    Text = "Sharing your Ledger Wallet data helps us understand your preferences and show content that's relevant to you.",
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                    ForeColor = Color.FromArgb(180, 180, 180),
                    AutoSize = false,
                    Size = new Size(440, 50),
                    Location = new Point(30, yPos),
                    BackColor = Color.Transparent
                };
                Controls.Add(subtitle);
                yPos += 60;

                var consentIntro = new Label
                {
                    Text = "With your consent, Ledger will be able to collect data about how you use Ledger Wallet (including page visits and clicks):",
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    ForeColor = Color.White,
                    AutoSize = false,
                    Size = new Size(440, 50),
                    Location = new Point(30, yPos),
                    BackColor = Color.Transparent
                };
                Controls.Add(consentIntro);
                yPos += 60;

                string[] greenItems =
                {
                    "To measure the performance of Ledger Wallet and enhance both the app and your experience (Analytics).",
                    "To provide you with personalized recommendations and content tailored to your preferences, as well as to help us measure the effectiveness of our marketing campaigns (Personalization)."
                };

                foreach (var item in greenItems)
                {
                    var row = CreateBulletRow("✔", Color.FromArgb(76, 175, 80), item, yPos);
                    Controls.Add(row.Item1);
                    Controls.Add(row.Item2);
                    yPos += row.Item2.Height + 12;
                }

                yPos += 10;

                var neverCollect = new Label
                {
                    Text = "Ledger will never collect information regarding:",
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    ForeColor = Color.White,
                    AutoSize = false,
                    Size = new Size(440, 25),
                    Location = new Point(30, yPos),
                    BackColor = Color.Transparent
                };
                Controls.Add(neverCollect);
                yPos += 35;

                string[] redItems =
                {
                    "Your assets.",
                    "Your portfolio.",
                    "Your personal identifying information."
                };

                foreach (var item in redItems)
                {
                    var row = CreateBulletRow("✕", Color.FromArgb(220, 60, 60), item, yPos);
                    Controls.Add(row.Item1);
                    Controls.Add(row.Item2);
                    yPos += row.Item2.Height + 10;
                }

                yPos += 10;

                var revokeNote = new Label
                {
                    Text = "You can revoke your consent any time in the app settings.",
                    Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                    ForeColor = Color.FromArgb(150, 150, 150),
                    AutoSize = false,
                    Size = new Size(440, 22),
                    Location = new Point(30, yPos),
                    BackColor = Color.Transparent
                };
                Controls.Add(revokeNote);
                yPos += 22;

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
                yPos += 50;

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

                var manageBtn = new Label
                {
                    Text = "Manage your preferences",
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(130, 120, 220),
                    AutoSize = true,
                    Location = new Point(15, 25),
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand
                };
                manageBtn.MouseEnter += (s, e) =>
                {
                    manageBtn.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold | FontStyle.Underline);
                };
                manageBtn.MouseLeave += (s, e) =>
                {
                    manageBtn.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                };

                manageBtn.Click += (s, e) =>
                {
                    // Convert the consent panel's position to screen coordinates
                    var parent = this.Parent as Form;
                    Point screenPos;
                    if (parent != null)
                        screenPos = parent.PointToScreen(this.Location);
                    else
                        screenPos = this.PointToScreen(Point.Empty);

                    var prefs = new PreferencesPanel
                    {
                        Size = this.Size,
                        Location = screenPos
                    };
                    this.Visible = false;
                    var result = prefs.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        CloseRequested?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        this.Visible = true;
                    }
                };

                btnPanel.Controls.Add(manageBtn);

                var acceptBtn = new RoundedButton
                {
                    Text = "Accept all",
                    Width = 120,
                    Height = 40,
                    FillColor = Color.FromArgb(100, 80, 220),
                    TextColor = Color.White,
                    BorderColor = Color.FromArgb(100, 80, 220),
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    CornerRadius = 20,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnPanel.Controls.Add(acceptBtn);

                var refuseBtn = new RoundedButton
                {
                    Text = "Refuse all",
                    Width = 120,
                    Height = 40,
                    FillColor = Color.FromArgb(45, 45, 45),
                    TextColor = Color.White,
                    BorderColor = Color.FromArgb(100, 100, 100),
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                    Cursor = Cursors.Hand,
                    CornerRadius = 20,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                refuseBtn.Click += (s, e) => CloseRequested?.Invoke(this, EventArgs.Empty);
                btnPanel.Controls.Add(refuseBtn);
                acceptBtn.Click += (s, e) => CloseRequested?.Invoke(this, EventArgs.Empty);

                btnPanel.Resize += (s, e) =>
                {
                    acceptBtn.Location = new Point(btnPanel.Width - acceptBtn.Width - 15, 15);
                    refuseBtn.Location = new Point(acceptBtn.Left - refuseBtn.Width - 10, 15);
                };

                Resize += (s, e) =>
                {
                    closeBtn.Location = new Point(Width - closeBtn.Width - 15, 10);
                };

                closeBtn.BringToFront();
            }

            Tuple<Label, Label> CreateBulletRow(string icon, Color iconColor, string text, int y)
            {
                var iconLabel = new Label
                {
                    Text = icon,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    ForeColor = iconColor,
                    AutoSize = true,
                    Location = new Point(30, y),
                    BackColor = Color.Transparent
                };

                int textHeight = text.Length > 80 ? 55 : 30;
                var textLabel = new Label
                {
                    Text = text,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                    ForeColor = Color.White,
                    AutoSize = false,
                    Size = new Size(410, textHeight),
                    Location = new Point(55, y),
                    BackColor = Color.Transparent
                };

                return Tuple.Create(iconLabel, textLabel);
            }
        }

        #endregion

        #region Custom Controls

        class RoundedButton : Control
        {
            Color _fill = Color.White;
            Color _text = Color.Black;
            Color _border = Color.White;
            Color? _hoverFill = null;
            Color? _hoverBorder = null;
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

            public Color? HoverFillColor
            {
                get { return _hoverFill; }
                set { _hoverFill = value; Invalidate(); }
            }

            public Color? HoverBorderColor
            {
                get { return _hoverBorder; }
                set { _hoverBorder = value; Invalidate(); }
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

                Size = new Size(260, 50);
            }

            void UpdateRegion()
            {
                using (var path = CreateRoundedPath(ClientRectangle, _radius))
                {
                    Region = new Region(path);
                }
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);
                UpdateRegion();
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
                _hovered = false;
                _pressed = false;
                Invalidate();
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);
                _pressed = true;
                Invalidate();
            }

            protected override void OnMouseUp(MouseEventArgs e)
            {
                base.OnMouseUp(e);
                _pressed = false;
                Invalidate();
            }

            static Color AdjustBrightness(Color c, int amt)
            {
                int r = Math.Max(0, Math.Min(255, c.R + amt));
                int g = Math.Max(0, Math.Min(255, c.G + amt));
                int b = Math.Max(0, Math.Min(255, c.B + amt));
                return Color.FromArgb(c.A, r, g, b);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                var rect = ClientRectangle;
                rect.Width -= 1;
                rect.Height -= 1;

                using (var path = CreateRoundedPath(rect, _radius))
                {
                    Color fill;
                    Color border;

                    if (_pressed)
                    {
                        fill = _hoverFill.HasValue
                            ? AdjustBrightness(_hoverFill.Value, 15)
                            : AdjustBrightness(_fill, _fill.GetBrightness() > 0.5f ? -50 : 30);
                        border = _hoverBorder ?? _border;
                    }
                    else if (_hovered)
                    {
                        fill = _hoverFill ?? AdjustBrightness(_fill, _fill.GetBrightness() > 0.5f ? -25 : 20);
                        border = _hoverBorder ?? _border;
                    }
                    else
                    {
                        fill = _fill;
                        border = _border;
                    }

                    using (var brush = new SolidBrush(fill))
                    {
                        e.Graphics.FillPath(brush, path);
                    }

                    using (var pen = new Pen(border, 1.5f))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }

                TextRenderer.DrawText(
                    e.Graphics,
                    Text,
                    Font,
                    rect,
                    _text,
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

        class SegmentedProgress : Control
        {
            double[] p = new double[3];

            public SegmentedProgress()
            {
                DoubleBuffered = true;
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                Size = new Size(500, 4);
            }

            public void Set(int i, double v)
            {
                if (i < 0 || i >= p.Length) return;
                p[i] = Math.Max(0, Math.Min(1, v));
                Invalidate();
            }

            public void ResetAll()
            {
                for (int i = 0; i < p.Length; i++) p[i] = 0;
                Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                int totalWidth = Width;
                int gap = 8;
                int segWidth = (totalWidth - gap * (p.Length - 1)) / p.Length;

                for (int i = 0; i < p.Length; i++)
                {
                    int x = i * (segWidth + gap);

                    using (var bg = new SolidBrush(Color.FromArgb(70, 255, 255, 255)))
                    {
                        e.Graphics.FillRectangle(bg, x, 0, segWidth, Height);
                    }

                    int fillWidth = (int)(segWidth * p[i]);
                    if (fillWidth > 0)
                    {
                        e.Graphics.FillRectangle(Brushes.White, x, 0, fillWidth, Height);
                    }
                }
            }
        }

        #endregion
    }
}