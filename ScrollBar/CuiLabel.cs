// Deobfuscated / cleaned version of:
//   כ… .ܣ…  (UserControl "imLabel")
//
// What it is:
// - A lightweight label control that draws text with configurable horizontal/vertical alignment.
// - Uses AntiAlias + TextRenderingHint.AntiAlias.
//
// Important decompile quirk:
// - The original Content getter returns Regex.Escape(text) and setter Regex.Unescape(value).
//   That is *not* typical for a label and will surprise callers.
//   I preserved that behavior as an optional mode (EscapeContent).
//   Default is normal label behavior (Content returns the raw text).

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Ledger.BitUI;


namespace Ledger.ScrollBar
{
    [ToolboxBitmap(typeof(Label))]
    public class CuiLabel : UserControl
    {
        private string _text = "Your text here!";
        private StringAlignment _horizontalAlignment = StringAlignment.Center;
        private StringAlignment _verticalAlignment = StringAlignment.Center;

        /// <summary>
        /// Preserves the original weird behavior:
        /// - get => Regex.Escape(_text)
        /// - set => _text = Regex.Unescape(value)
        /// </summary>
        private bool _escapeContent = false;

        private IContainer components;

        public CuiLabel()
        {
            InitializeComponent();

            DoubleBuffered = true;
            AutoScaleMode = AutoScaleMode.None;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        [Description("Text content to draw.")]
        public string Content
        {
            get => _escapeContent ? Regex.Escape(_text) : _text;
            set
            {
                _text = _escapeContent ? Regex.Unescape(value ?? string.Empty) : value ?? string.Empty;
                Invalidate();
            }
        }

        [Description("If true, Content getter escapes and setter unescapes (original decompiled behavior).")]
        [DefaultValue(false)]
        public bool EscapeContent
        {
            get => _escapeContent;
            set
            {
                if (_escapeContent == value) return;
                _escapeContent = value;
                Invalidate();
            }
        }

        [DefaultValue(StringAlignment.Center)]
        public StringAlignment HorizontalAlignment
        {
            get => _horizontalAlignment;
            set
            {
                _horizontalAlignment = value;
                Invalidate();
            }
        }

        [DefaultValue(StringAlignment.Center)]
        public StringAlignment VerticalAlignment
        {
            get => _verticalAlignment;
            set
            {
                _verticalAlignment = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

             var format = new StringFormat
            {
                Alignment = HorizontalAlignment,
                LineAlignment = VerticalAlignment
            };

             var brush = new SolidBrush(ForeColor);
            e.Graphics.DrawString(_text, Font, brush, (RectangleF)ClientRectangle, format);

            base.OnPaint(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CuiLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CuiLabel";
            this.Size = new System.Drawing.Size(280, 58);
            this.Load += new System.EventHandler(this.CuiLabel_Load);
            this.ResumeLayout(false);

        }

        private void CuiLabel_Load(object sender, EventArgs e)
        {

        }
    }
}
