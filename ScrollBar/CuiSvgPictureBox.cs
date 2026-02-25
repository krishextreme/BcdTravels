
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Svg;

namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(PictureBox))]
    public class CuiSvgPictureBox : Control
    {
        private string[] _svgCode = new[] { "" };

        private Color _overrideStroke = Color.Empty;
        private Color _overrideFill = Color.Empty;

        private Bitmap _bitmap;

        // Helps avoid overlapping async writes/reads.
        private readonly SemaphoreSlim _saveLock = new SemaphoreSlim(1, 1);

        private IContainer components;

        public CuiSvgPictureBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
        }

        private string GetSvgPath()
        {
            string dir = Path.Combine(Environment.CurrentDirectory, "CuoreUI");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return Path.Combine(dir, $"{Name}_cuore.svg");
        }

        /// <summary>
        /// Source SVG lines (as provided by the caller).
        /// Setting this triggers re-save with current OverrideStroke/OverrideFill.
        /// </summary>
        public string[] SvgCode
        {
            get => _svgCode;
            set
            {
                _svgCode = value ?? Array.Empty<string>();

                // Decompiled behavior: re-assigning properties forces SaveSVG twice.
                OverrideStroke = OverrideStroke;
                OverrideFill = OverrideFill;
            }
        }

        /// <summary>
        /// The final SVG lines that are being used/saved (after overrides applied).
        /// Decompiled: private set; initialized to array with one empty string.
        /// </summary>
        public string[] RunningSvgCode { get; private set; } = new[] { "" };

        private static string ColorToHex(Color color) =>
            $"#{color.R:x2}{color.G:x2}{color.B:x2}";

        public Color OverrideStroke
        {
            get => _overrideStroke;
            set
            {
                // Decompiled behavior: Transparent/Empty means "use BackColor"
                if (value == Color.Transparent || value == Color.Empty)
                    value = BackColor;

                _overrideStroke = value;

                // Decompiled calls SaveSVG(true,false)
                _ = SaveSvgAsync(alternativeStroke: true, alternativeFill: false);
            }
        }

        public Color OverrideFill
        {
            get => _overrideFill;
            set
            {
                if (value == Color.Transparent || value == Color.Empty)
                    value = BackColor;

                _overrideFill = value;

                // Decompiled calls SaveSVG(false,true)
                _ = SaveSvgAsync(alternativeStroke: false, alternativeFill: true);
            }
        }

        /// <summary>
        /// Reconstructed SaveSVG. The original was async-void via a compiler state machine.
        /// This implementation:
        /// - Combines SvgCode -> text
        /// - Optionally replaces stroke="..." and/or fill="..." attributes
        /// - Writes the result to disk (GetSvgPath)
        /// - Updates RunningSvgCode and invalidates to repaint
        /// </summary>
        private async Task SaveSvgAsync(bool alternativeStroke, bool alternativeFill)
        {
            // Keep behavior close to async-void: swallow exceptions (like many async-void handlers do)
            try
            {
                await _saveLock.WaitAsync().ConfigureAwait(false);

                string svgText = string.Join(Environment.NewLine, _svgCode ?? Array.Empty<string>());

                // If nothing to save, still update RunningSvgCode and return.
                if (string.IsNullOrWhiteSpace(svgText))
                {
                    RunningSvgCode = new[] { "" };
                    return;
                }

                if (alternativeStroke)
                {
                    string strokeHex = ColorToHex(_overrideStroke);
                    svgText = ReplaceSvgAttribute(svgText, "stroke", strokeHex);
                }

                if (alternativeFill)
                {
                    string fillHex = ColorToHex(_overrideFill);
                    svgText = ReplaceSvgAttribute(svgText, "fill", fillHex);
                }

                RunningSvgCode = svgText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                string path = GetSvgPath();

                // Write asynchronously, UTF-8 without BOM is typical for SVG text.
                byte[] bytes = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false).GetBytes(svgText);
                using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, useAsync: true))
                {
                    await fs.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
                }

                // Trigger repaint on UI thread
                if (!IsDisposed && IsHandleCreated)
                {
                    try
                    {
                        BeginInvoke((MethodInvoker)(() =>
                        {
                            Invalidate();
                        }));
                    }
                    catch
                    {
                        // Decompiled async-void behavior: swallow UI race exceptions
                    }
                }
            }
            catch
            {
                // Intentionally swallowed (matches async-void semantics)
            }
            finally
            {
                if (_saveLock.CurrentCount == 0)
                    _saveLock.Release();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (RunningSvgCode == null || RunningSvgCode.Length == 0)
                return;

            string path = GetSvgPath();
            if (!File.Exists(path))
                return;

            try
            {
                SvgDocument doc = SvgDocument.Open(path);

                doc.Width = Width;
                doc.Height = Height;

                _bitmap?.Dispose();
                _bitmap = doc.Draw();

                if (_bitmap != null)
                {
                    e.Graphics.SmoothingMode =
                        System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.InterpolationMode =
                        System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    e.Graphics.DrawImage(_bitmap, ClientRectangle);
                }
            }
            catch
            {
                // Decompiled code ignored SVG parse/draw errors
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _bitmap?.Dispose();
                _saveLock?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            Size = new Size(100, 100);
        }

        /// <summary>
        /// Replaces stroke="..." or fill="..." attributes in SVG text.
        /// Matches the broad regex behavior seen in the decompile.
        /// </summary>
        private static string ReplaceSvgAttribute(string svg, string attribute, string hexColor)
        {
            string pattern = $@"{attribute}\s*=\s*[""'][^""']*[""']";
            string replacement = $"{attribute}=\"{hexColor}\"";

            return Regex.Replace(svg, pattern, replacement, RegexOptions.IgnoreCase);
        }
    }
}
