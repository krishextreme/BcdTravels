// Decompiled with JetBrains decompiler
// Type: BlurEffectComponent
// Assembly: Ledger Live, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DC7CF87-871B-48BD-97F4-4117271260D2
// Assembly location: C:\Users\hps\Desktop\for\ghidra files\ledger\ledger.exe

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;


namespace Ledger.Animations
{
    [ToolboxBitmap(typeof(Panel))]
    public class BlurEffectComponent : Component
    {
        private Bitmap cachedBitmap;
        private Control targetControl;
        private float blurAmount = 1.5f;

        public BlurEffectComponent(IContainer container)
        {
            container.Add(this);
        }

        public Control TargetControl
        {
            get => targetControl;
            set
            {
                if (TargetControl is Form || value is Form || value == null)
                {
                    targetControl = null;
                    cachedBitmap?.Dispose();
                    cachedBitmap = null;

                    if (!Debugger.IsAttached && !DesignMode || value == null)
                        return;

                    MessageBox.Show("Cannot set TargetControl to type Form in this BlurEffectComponent instance.\nBlurring the whole form would be too expensive for WinForms, sorry.", "BlurEffect");
                }
                else
                {
                    if (targetControl != null)
                    {
                        targetControl.Paint -= new PaintEventHandler(TargetControl_Paint);
                        targetControl.Invalidated -= new InvalidateEventHandler(TargetControl_Invalidated);
                    }

                    value.Parent?.Refresh();
                    targetControl = value;

                    if (targetControl != null)
                    {
                        targetControl.Paint += new PaintEventHandler(TargetControl_Paint);
                        targetControl.Invalidated += new InvalidateEventHandler(TargetControl_Invalidated);
                    }

                    cachedBitmap?.Dispose();
                    cachedBitmap = null;
                    targetControl?.Invalidate();
                }
            }
        }

        public float BlurAmount
        {
            get => blurAmount;
            set
            {
                if (value > 0.0f)
                    blurAmount = value;

                cachedBitmap?.Dispose();
                cachedBitmap = null;
                targetControl?.Refresh();
            }
        }

        private void TargetControl_Invalidated(object sender, InvalidateEventArgs e)
        {
            cachedBitmap?.Dispose();
            cachedBitmap = null;
        }

        private void TargetControl_Paint(object sender, PaintEventArgs e)
        {
            if (cachedBitmap == null || cachedBitmap.Width != targetControl.Width || cachedBitmap.Height != targetControl.Height)
            {
                cachedBitmap?.Dispose();
                cachedBitmap = new Bitmap(targetControl.Width, targetControl.Height);
                using (Graphics graphics = Graphics.FromImage(cachedBitmap))
                {
                    graphics.SmoothingMode = SmoothingMode.None;
                    graphics.PixelOffsetMode = PixelOffsetMode.None;
                    graphics.InterpolationMode = InterpolationMode.Low;
                    graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;

                    targetControl.DrawToBitmap(cachedBitmap, new Rectangle(0, 0, targetControl.Width, targetControl.Height));
                    // Assuming an external blur method exists
                    BlurHelper.Apply(ref cachedBitmap, BlurAmount);
                }
            }

            e.Graphics.DrawImage(cachedBitmap, targetControl.ClientRectangle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                cachedBitmap?.Dispose();
                if (targetControl != null)
                {
                    targetControl.Paint -= new PaintEventHandler(TargetControl_Paint);
                    targetControl.Invalidated -= new InvalidateEventHandler(TargetControl_Invalidated);
                    targetControl.Invalidate();
                    targetControl = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
