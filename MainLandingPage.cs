// Install Microsoft.Web.WebView2 via NuGet and ensure WebView2 runtime is present on target machines
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace Ledger.MainClassFolder
{
    public partial class MainLandingPage : Form
    {
        #region Declaration 

        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private VideoView _videoView;
        private Media _currentMedia;

        // replaced standard ProgressBar with custom panel-based bars
        private Panel[] _progressBg = new Panel[3];
        private Panel[] _progressFill = new Panel[3];
        private double[] _progressPercent = new double[3];

        private string[] _videoFiles;
        private int _currentIndex = 0;

        // UI elements for top center
        private Panel _topContainer;
        private FlowLayoutPanel _centerFlow; // holds logo + title
        private PictureBox _logoBox;
        private Label _titleLabel;
        private FlowLayoutPanel _progressFlow;

        // UI synchronization context
        private SynchronizationContext _uiContext;

        // store logo path for reload
        private string _logoPath;

        #endregion

        #region Constructor 

        public MainLandingPage()
        {
            InitializeComponent();

            // Ensure native LibVLC folder is on PATH so libvlc.dll can be found.
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string runtimesNative = Path.Combine(baseDir, "runtimes", "win-x64", "native");
                string repoPackages = Path.Combine(baseDir, "..", "..", "packages", "VideoLAN.LibVLC.Windows.3.0.23", "runtimes", "win-x64", "native");
                string[] candidates = { runtimesNative, repoPackages, baseDir };
                string nativeDir = candidates.FirstOrDefault(Directory.Exists);
                if (!string.IsNullOrEmpty(nativeDir))
                {
                    var path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
                    if (!path.Split(Path.PathSeparator).Contains(nativeDir, StringComparer.OrdinalIgnoreCase))
                    {
                        Environment.SetEnvironmentVariable("PATH", nativeDir + Path.PathSeparator + path);
                    }
                }
            }
            catch { }

            try { Core.Initialize(); } catch { }

            // Build UI and defer LibVLC init until Load
            BuildUi();
            this.Load += MainLandingPage_Load;
            this.Resize += (s, e) => CenterTopControls();
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.Black;
        }

        private void MainLandingPage_Load(object sender, EventArgs e)
        {
            _uiContext = SynchronizationContext.Current ?? new WindowsFormsSynchronizationContext();

            // Video file names (relative to exe). Update names/paths if needed.
            var fileNames = new[]
            {
                @"Resources\Videos\ledgerWalletBuySell-1.webm",
                @"Resources\Videos\ledgerWalletThousandsCrypto2.webm",
                @"Resources\Videos\ledgerWalletSecureWallet3.webm"
            };

            _videoFiles = fileNames.Select(f => GetVideoPath(f)).ToArray();

            var missing = _videoFiles.Where(p => !File.Exists(p)).ToArray();
            if (missing.Length > 0)
            {
                MessageBox.Show(
                    "One or more video files are missing. Expected locations (example):" + Environment.NewLine +
                    string.Join(Environment.NewLine, _videoFiles) + Environment.NewLine + Environment.NewLine +
                    "Add the files to the project and set 'Copy to Output Directory = Copy if newer', or place them next to the EXE.",
                    "Video files missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                InitializeLibVlc();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize playback: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region UI + VLC initialization

        private void BuildUi()
        {
            // Top container for logo/title/progress
            _topContainer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 140,
                BackColor = Color.Transparent
            };
            Controls.Add(_topContainer);
            _topContainer.BringToFront();

            // center flow holds logo then title (logo before "WALLET")
            _centerFlow = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };
            _topContainer.Controls.Add(_centerFlow);

            // Logo
            _logoBox = new PictureBox
            {
                Size = new Size(48, 48),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Margin = new Padding(6, 10, 6, 6)
            };
            _centerFlow.Controls.Add(_logoBox);

            // Title next to logo (so logo is before WALLET)
            _titleLabel = new Label
            {
                Text = "WALLET",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                AutoSize = true,
                BackColor = Color.Transparent,
                Margin = new Padding(6, 18, 6, 6)
            };
            _centerFlow.Controls.Add(_titleLabel);

            // Progress flow panel (below centerFlow)
            _progressFlow = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 0),
                Padding = new Padding(0)
            };
            _topContainer.Controls.Add(_progressFlow);

            // create three custom thin progress bars (shorter length)
            for (int i = 0; i < 3; i++)
            {
                var bg = new Panel
                {
                    Width = 160,            // less length
                    Height = 12,
                    // slightly translucent background
                    BackColor = Color.FromArgb(80, 0, 0, 0),
                    Margin = new Padding(8, 6, 8, 6),
                    BorderStyle = BorderStyle.None
                };

                var fill = new Panel
                {
                    Width = 0,
                    Height = bg.Height,
                    // semi-transparent fill color so you see the video behind
                    BackColor = Color.FromArgb(160, 0, 180, 130)
                };

                bg.Controls.Add(fill);

                _progressBg[i] = bg;
                _progressFill[i] = fill;
                _progressPercent[i] = 0.0;

                _progressFlow.Controls.Add(bg);
            }

            // VideoView background
            _videoView = new VideoView
            {
                Name = "videoView",
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.Black
            };
            Controls.Add(_videoView);
            _videoView.SendToBack();

            CenterTopControls();
        }

        private void CenterTopControls()
        {
            if (_topContainer == null) return;

            // position centerFlow centered horizontally
            _centerFlow.Left = Math.Max(8, (_topContainer.ClientSize.Width - _centerFlow.Width) / 2);
            _centerFlow.Top = 8;

            // position progressFlow below the centerFlow and centered
            _progressFlow.Left = Math.Max(8, (_topContainer.ClientSize.Width - _progressFlow.Width) / 2);
            _progressFlow.Top = _centerFlow.Bottom + 6;

            // when widths change (resize) recompute fill widths from saved percent
            for (int i = 0; i < 3; i++)
            {
                if (_progressBg[i] != null && _progressFill[i] != null)
                {
                    var newWidth = (int)Math.Round(_progressBg[i].Width * _progressPercent[i]);
                    _progressFill[i].Width = Math.Max(0, Math.Min(_progressBg[i].Width, newWidth));
                }
            }
        }

        private void InitializeLibVlc()
        {
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);
            _videoView.MediaPlayer = _mediaPlayer;

            _mediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
            _mediaPlayer.EndReached += MediaPlayer_EndReached;
            _mediaPlayer.EncounteredError += (s, e) =>
            {
                PostToUi(() =>
                    MessageBox.Show("LibVLC encountered a playback error.", "Playback error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                );
            };

            _currentIndex = 0;
            // load logo path once (used for reload on sequence restart)
            _logoPath = GetVideoPath(@"Resources\logo.png");
            LoadLogoIfExists();

            PlayVideoAtIndex(_currentIndex);
        }

        #endregion

        #region Playback control and events

        private void PlayVideoAtIndex(int index)
        {
            if (index < 0 || _videoFiles == null || index >= _videoFiles.Length) return;

            // reset percent and fill width for the index
            _progressPercent[index] = 0.0;
            PostToUi(() =>
            {
                try { _progressFill[index].Width = 0; } catch { }
            });

            // reload logo each time to satisfy "image reload in same sequence"
            PostToUi(() => LoadLogoIfExists());

            try
            {
                _mediaPlayer?.Stop();
                _currentMedia?.Dispose();
                _currentMedia = null;
            }
            catch { }

            var path = _videoFiles[index];
            if (!File.Exists(path))
            {
                PostToUi(() => MessageBox.Show($"Video file not found: {path}", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error));
                return;
            }

            _currentMedia = new Media(_libVLC, path, FromType.FromPath);
            _mediaPlayer.Play(_currentMedia);
        }

        private void MediaPlayer_PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            var percent = Math.Max(0.0, Math.Min(1.0, e.Position)); // 0..1
            _progressPercent[_currentIndex] = percent;

            PostToUi(() =>
            {
                try
                {
                    if (_currentIndex >= 0 && _currentIndex < _progressFill.Length)
                    {
                        var bg = _progressBg[_currentIndex];
                        var fill = _progressFill[_currentIndex];
                        if (bg != null && fill != null)
                        {
                            // smooth fill based on position — PositionChanged fires frequently
                            fill.Width = (int)Math.Round(bg.Width * percent);
                        }
                    }
                }
                catch { }
            });
        }

        private void MediaPlayer_EndReached(object sender, EventArgs e)
        {
            PostToUi(() =>
            {
                try
                {
                    if (_currentIndex >= 0 && _currentIndex < _progressFill.Length)
                    {
                        _progressFill[_currentIndex].Width = _progressBg[_currentIndex].Width;
                        _progressPercent[_currentIndex] = 1.0;
                    }
                }
                catch { }

                _currentIndex++;

                if (_currentIndex >= _videoFiles.Length)
                {
                    // finished full sequence -> reset progress and restart immediately (no delay)
                    ResetAllProgress();
                    _currentIndex = 0;

                    // reload logo image explicitly
                    LoadLogoIfExists();

                    // restart immediately
                    PlayVideoAtIndex(_currentIndex);
                    return;
                }

                // play next video immediately (no delay)
                PlayVideoAtIndex(_currentIndex);
            });
        }

        #endregion

        #region Helpers

        private void ResetAllProgress()
        {
            for (int i = 0; i < _progressPercent.Length; i++)
            {
                _progressPercent[i] = 0.0;
            }

            PostToUi(() =>
            {
                for (int i = 0; i < _progressFill.Length; i++)
                {
                    try { if (_progressFill[i] != null) _progressFill[i].Width = 0; } catch { }
                }
            });
        }

        private void LoadLogoIfExists()
        {
            try
            {
                if (string.IsNullOrEmpty(_logoPath)) return;

                if (!File.Exists(_logoPath)) return;

                // Dispose previous image safely, then reload from file to force reload.
                var prev = _logoBox.Image;
                _logoBox.Image = null;
                try { prev?.Dispose(); } catch { }

                // load fresh image
                _logoBox.Image = Image.FromFile(_logoPath);
            }
            catch
            {
                // ignore load errors
            }
        }

        private void PostToUi(Action action)
        {
            if (action == null) return;
            if (_uiContext != null)
            {
                _uiContext.Post(_ => action(), null);
                return;
            }

            if (IsHandleCreated)
            {
                try { BeginInvoke(action); } catch { }
            }
            else
            {
                this.Load += (_, __) => { try { action(); } catch { } };
            }
        }

        private string GetVideoPath(string fileName)
        {
            string appFolder = Application.StartupPath;
            string candidate = Path.Combine(appFolder, fileName);
            if (File.Exists(candidate)) return candidate;

            string contentCandidate = Path.Combine(appFolder, "Content", fileName);
            if (File.Exists(contentCandidate)) return contentCandidate;

            string dir = appFolder;
            for (int i = 0; i < 6; i++)
            {
                dir = Path.GetDirectoryName(dir);
                if (string.IsNullOrEmpty(dir)) break;
                var p = Path.Combine(dir, fileName);
                if (File.Exists(p)) return p;
            }

            return candidate;
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (_mediaPlayer != null)
                    {
                        _mediaPlayer.PositionChanged -= MediaPlayer_PositionChanged;
                        _mediaPlayer.EndReached -= MediaPlayer_EndReached;
                    }

                    _mediaPlayer?.Stop();
                    _currentMedia?.Dispose();
                    _videoView?.Dispose();
                    _mediaPlayer?.Dispose();
                    _libVLC?.Dispose();

                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
                catch
                {
                    // swallow disposal exceptions
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
