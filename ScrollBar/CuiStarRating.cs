// Cleaned + deobfuscated version of the decompiled control.
// Original type: כ… .��מV�T���ܬΡT
//
// What it is:
// - A star rating control that supports half-star increments.
// - Rating is stored as an int where:
//     0  = 0 stars
//     1  = 0.5 star
//     2  = 1 star
//     ...
//     10 = 5 stars (when StarCount == 5)
// - Mouse drag with left button updates Rating (if AllowUserInteraction is true).
//
// External dependency kept as alias:
// - Theme.PrimaryColor
// - GraphicsUtil.Star(...) -> returns GraphicsPath for a star polygon

using Ledger.BitUI;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Theme = Ledger.UIHelper.UIGraphicsHelper;


namespace YourApp.UI.Controls
{
    [ToolboxBitmap(typeof(ToolTip))]
    public class CuiStarRating : Control
    {
        private int _starCount = 5;
        private int _rating = 2; // decompile default (== 1 star when starCount=5)
        private Color _starColor = Theme.PrimaryColor;
        private int _starBorderSize = 1;
        private bool _allowUserInteraction = true;

        public CuiStarRating()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Size = new Size(150, 28);
        }

        public int StarCount
        {
            get => _starCount;
            set
            {
                _starCount = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Half-step rating. For 5 stars the typical range is 0..10.
        /// (The original code does not clamp; it just assigns and repaints.)
        /// </summary>
        public int Rating
        {
            get => _rating;
            set
            {
                _rating = value;
                Invalidate();
            }
        }

        public Color StarColor
        {
            get => _starColor;
            set
            {
                _starColor = value;
                Invalidate();
            }
        }

        public int StarBorderSize
        {
            get => _starBorderSize;
            set
            {
                _starBorderSize = value;
                Invalidate();
            }
        }

        public bool AllowUserInteraction
        {
            get => _allowUserInteraction;
            set
            {
                _allowUserInteraction = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            int starWidth = Height - 2;
            int gap = starWidth / 5;

            for (int i = 0; i < StarCount; i++)
            {
                int x = i * (starWidth + gap);

                // Used for the "half-star" masking rectangle.
                Rectangle maskRect = new Rectangle(x, 0, starWidth, Height);
                maskRect.Offset(starWidth / 2, 0);
                maskRect.Inflate(-StarBorderSize, -StarBorderSize);
                maskRect.Offset(StarBorderSize / 2, StarBorderSize / 2);

                // Star path centered at (x + starWidth/2, Height/2)
                GraphicsPath starPath = BitMapClass.Star(
                    x + starWidth / 2f,
                    Height / 2f,
                    starWidth / 2f,
                    starWidth / 3.8f,
                    5);

                using (var fillBrush = new SolidBrush(StarColor))
                {
                    // Full star if rating covers this star fully.
                    if ((i + 1) * 2 <= Rating)
                    {
                        e.Graphics.FillPath(fillBrush, starPath);
                    }
                    // Half star: fill it, then mask half with BackColor rectangle.
                    else if (i * 2 + 1 == Rating)
                    {
                        e.Graphics.FillPath(fillBrush, starPath);

                        maskRect.Inflate(StarBorderSize, StarBorderSize);
                        maskRect.Offset(-(StarBorderSize / 2), -(StarBorderSize / 2));

                        using (var back = new SolidBrush(BackColor))
                            e.Graphics.FillRectangle(back, maskRect);
                    }
                }

                using (var outlinePen = new Pen(StarColor, StarBorderSize / 2f))
                    e.Graphics.DrawPath(outlinePen, starPath);

                starPath.Dispose();
            }

            base.OnPaint(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!AllowUserInteraction || e.Button != MouseButtons.Left)
                return;

            // Decompiled computes based on fixed "5 stars" geometry.
            // It does NOT use StarCount here (likely a bug/assumption).
            int starSize = Height - 2;
            int gap = starSize / 5;

            int assumedStarCount = 5;
            int x = e.X + 5;

            if (x < 0)
            {
                Rating = 0;
            }
            else if (x > assumedStarCount * (starSize + gap))
            {
                Rating = 10;
            }
            else
            {
                int starIndex = (x - gap) / (starSize + gap);

                if (((x - gap) % (starSize + gap)) > (starSize / 2))
                    Rating = (starIndex + 1) * 2;
                else
                    Rating = starIndex * 2 + 1;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
            OnMouseMove(e); // decompiled behavior: clicking sets rating immediately
        }
    }
}
