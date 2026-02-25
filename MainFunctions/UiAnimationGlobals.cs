
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Ledger.BitUI
{
    public static class UiAnimationGlobals
    {
        public static readonly Color PrimaryColor = Color.FromArgb((int)byte.MaxValue, 106, 0);

        private static Timer refreshRateTimer;
        private static Timer refreshRateRefresher = new Timer();
        private static byte frameCounter = 0;

        public static Color TranslucentPrimaryColor
        {
            get => Color.FromArgb(192 /*0xC0*/, UiAnimationGlobals.PrimaryColor);
        }

        public static event EventHandler FrameDrawn;

        public static event EventHandler TenFramesDrawn;

        static UiAnimationGlobals()
        {
            // External (unknown) type call preserved
            CursorUtil.EnableModernCursor();

            Process.GetCurrentProcess().Exited += (EventHandler)((e, s) => AnimationLoopController.Stop());

            UiAnimationGlobals.refreshRateTimer = new Timer();
            UiAnimationGlobals.SetTimerRefreshRate();
        }

        public static int[] GetRefreshRates()
        {
            return UiGraphicsUtil.GetRefreshRates();
        }

        public static int GetHighestRefreshRate()
        {
            return Math.Min(Math.Max(1, UiGraphicsUtil.GetHighestRefreshRate()), 1000);
        }

        public static float LazyTimeDelta
        {
            get => 1000f / (float)UiAnimationGlobals.GetHighestRefreshRate();
        }

        public static int LazyInt32TimeDelta
        {
            get => 1000 / UiAnimationGlobals.GetHighestRefreshRate();
        }

        private static void SetTimerRefreshRate()
        {
            UiAnimationGlobals.refreshRateTimer.Interval = 1000 / UiAnimationGlobals.GetHighestRefreshRate();

            if (!UiAnimationGlobals.refreshRateTimer.Enabled)
            {
                UiAnimationGlobals.refreshRateTimer.Start();
                UiAnimationGlobals.refreshRateTimer.Tick += (EventHandler)((sender, args) =>
                {
                    EventHandler frameDrawn = UiAnimationGlobals.FrameDrawn;
                    if (frameDrawn != null)
                        frameDrawn((object)null, EventArgs.Empty);

                    ++UiAnimationGlobals.frameCounter;

                    if (UiAnimationGlobals.frameCounter < (byte)10)
                        return;

                    UiAnimationGlobals.frameCounter = (byte)0;

                    EventHandler tenFramesDrawn = UiAnimationGlobals.TenFramesDrawn;
                    if (tenFramesDrawn == null)
                        return;

                    tenFramesDrawn((object)null, EventArgs.Empty);
                });
            }

            if (UiAnimationGlobals.refreshRateRefresher.Enabled)
                return;

            UiAnimationGlobals.refreshRateRefresher.Interval = 1000;
            UiAnimationGlobals.refreshRateRefresher.Start();
            UiAnimationGlobals.refreshRateRefresher.Tick += (EventHandler)((sender, args) =>
                UiAnimationGlobals.refreshRateTimer.Interval = 1000 / UiAnimationGlobals.GetHighestRefreshRate());
        }

        public class TimeDeltaStopwatch
        {
            private Stopwatch stopwatch = Stopwatch.StartNew();
            private long lastElapsedTicks;

            public float TimeDelta
            {
                get
                {
                    long elapsedTicks = this.stopwatch.ElapsedTicks;
                    long num = elapsedTicks - this.lastElapsedTicks;
                    this.lastElapsedTicks = elapsedTicks;
                    return (float)num / (float)Stopwatch.Frequency * 100f;
                }
            }
        }

        public static class ImageEffects
        {
            public static Bitmap TintBitmap(Bitmap originalBitmap, Color tintColor)
            {
                if (originalBitmap == null)
                    return (Bitmap)null;

                Bitmap bitmap = new Bitmap(originalBitmap.Width, originalBitmap.Height);
                using (Graphics graphics = Graphics.FromImage((Image)bitmap))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    ColorMatrix newColorMatrix = new ColorMatrix(new float[5][]
                    {
                        new float[5]
                        {
                            (float) tintColor.R / (float) byte.MaxValue,
                            0.0f,
                            0.0f,
                            0.0f,
                            0.0f
                        },
                        new float[5]
                        {
                            0.0f,
                            (float) tintColor.G / (float) byte.MaxValue,
                            0.0f,
                            0.0f,
                            0.0f
                        },
                        new float[5]
                        {
                            0.0f,
                            0.0f,
                            (float) tintColor.B / (float) byte.MaxValue,
                            0.0f,
                            0.0f
                        },
                        new float[5]
                        {
                            0.0f,
                            0.0f,
                            0.0f,
                            (float) tintColor.A / (float) byte.MaxValue,
                            0.0f
                        },
                        new float[5] { 0.0f, 0.0f, 0.0f, 0.0f, 1f }
                    });

                    ImageAttributes imageAttr = new ImageAttributes();
                    imageAttr.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    graphics.DrawImage(
                        (Image)originalBitmap,
                        new Rectangle(0, 0, originalBitmap.Width, originalBitmap.Height),
                        0,
                        0,
                        originalBitmap.Width,
                        originalBitmap.Height,
                        GraphicsUnit.Pixel,
                        imageAttr
                    );
                }

                return bitmap;
            }

            public static class Blur
            {
                public static class GaussianBlur
                {
                    public static void Apply(ref Bitmap bitmap, float radius)
                    {
                        // External (unknown) type call preserved
                        ExternalBlurLibrary.GaussianBlur.Apply(ref bitmap, radius);
                    }
                }

                public static class BoxBlur
                {
                    public static void Apply(ref Bitmap bitmap, float radius)
                    {
                        // External (unknown) type call preserved
                        ExternalBlurLibrary.BoxBlur.Apply(ref bitmap, radius);
                    }
                }
            }
        }

        public enum EasingType
        {
            QuadIn,
            QuadOut,
            QuadInOut,
            QuartIn,
            QuartOut,
            QuartInOut,
            QuintIn,
            QuintOut,
            QuintInOut,
            ExpoIn,
            ExpoOut,
            ExpoInOut,
            BackIn,
            BackOut,
            BackInOut,
            SextIn,
            SextOut,
            SextInOut,
        }

        public static class Easing
        {
            public static double FromEasingType(
                UiAnimationGlobals.EasingType easingType,
                double time,
                double duration = 1.0,
                double backOvershoot = 1.70158)
            {
                switch (easingType)
                {
                    case UiAnimationGlobals.EasingType.QuadIn:
                        return UiAnimationGlobals.Easing.Quadratic.In(time, duration);

                    case UiAnimationGlobals.EasingType.QuadOut:
                        return UiAnimationGlobals.Easing.Quadratic.Out(time, duration);

                    case UiAnimationGlobals.EasingType.QuadInOut:
                        return UiAnimationGlobals.Easing.Quadratic.InOut(time, duration);

                    case UiAnimationGlobals.EasingType.QuartIn:
                        return UiAnimationGlobals.Easing.Quartic.In(time, duration);

                    case UiAnimationGlobals.EasingType.QuartOut:
                        return UiAnimationGlobals.Easing.Quartic.Out(time, duration);

                    case UiAnimationGlobals.EasingType.QuartInOut:
                        return UiAnimationGlobals.Easing.Quartic.InOut(time, duration);

                    case UiAnimationGlobals.EasingType.QuintIn:
                        return UiAnimationGlobals.Easing.Quintic.In(time, duration);

                    case UiAnimationGlobals.EasingType.QuintOut:
                        return UiAnimationGlobals.Easing.Quintic.Out(time, duration);

                    case UiAnimationGlobals.EasingType.QuintInOut:
                        return UiAnimationGlobals.Easing.Quintic.InOut(time, duration);

                    case UiAnimationGlobals.EasingType.ExpoIn:
                        return UiAnimationGlobals.Easing.Exponential.In(time, duration);

                    case UiAnimationGlobals.EasingType.ExpoOut:
                        return UiAnimationGlobals.Easing.Exponential.Out(time, duration);

                    case UiAnimationGlobals.EasingType.ExpoInOut:
                        return UiAnimationGlobals.Easing.Exponential.InOut(time, duration);

                    case UiAnimationGlobals.EasingType.BackIn:
                        return UiAnimationGlobals.Easing.Back.In(time, duration, backOvershoot);

                    case UiAnimationGlobals.EasingType.BackOut:
                        return UiAnimationGlobals.Easing.Back.Out(time, duration, backOvershoot);

                    case UiAnimationGlobals.EasingType.BackInOut:
                        return UiAnimationGlobals.Easing.Back.InOut(time, duration, backOvershoot);

                    case UiAnimationGlobals.EasingType.SextIn:
                        return UiAnimationGlobals.Easing.Sextic.In(time, duration);

                    case UiAnimationGlobals.EasingType.SextOut:
                        return UiAnimationGlobals.Easing.Sextic.Out(time, duration);

                    case UiAnimationGlobals.EasingType.SextInOut:
                        return UiAnimationGlobals.Easing.Sextic.InOut(time, duration);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(easingType), (object)easingType, (string)null);
                }
            }

            public static class Quadratic
            {
                public static double In(double time, double duration = 1.0) => time * time / duration;

                public static double Out(double time, double duration = 1.0)
                {
                    return time * (2.0 - time) / duration;
                }

                public static double InOut(double time, double duration = 1.0)
                {
                    return time < 0.5
                        ? 2.0 * time * time / duration
                        : ((4.0 - 2.0 * time) * time - 1.0) / duration;
                }
            }

            public static class Quartic
            {
                public static double In(double time, double duration = 1.0)
                {
                    return time * time * time * time / duration;
                }

                public static double Out(double time, double duration = 1.0)
                {
                    return (1.0 - Math.Pow(1.0 - time, 4.0)) / duration;
                }

                public static double InOut(double time, double duration = 1.0)
                {
                    return time < 0.5
                        ? 8.0 * time * time * time * time / duration
                        : (1.0 - Math.Pow(-2.0 * time + 2.0, 4.0) / 2.0) / duration;
                }
            }

            public static class Quintic
            {
                public static double In(double time, double duration = 1.0)
                {
                    return time * time * time * time * time / duration;
                }

                public static double Out(double time, double duration = 1.0)
                {
                    return (1.0 - Math.Pow(1.0 - time, 5.0)) / duration;
                }

                public static double InOut(double time, double duration = 1.0)
                {
                    return time < 0.5
                        ? 16.0 * time * time * time * time * time / duration
                        : (1.0 - Math.Pow(-2.0 * time + 2.0, 5.0) / 2.0) / duration;
                }
            }

            public static class Sextic
            {
                public static double In(double time, double duration = 1.0) => Math.Pow(time, 6.0) / duration;

                public static double Out(double time, double duration = 1.0)
                {
                    return (1.0 - Math.Pow(1.0 - time, 6.0)) / duration;
                }

                public static double InOut(double time, double duration = 1.0)
                {
                    return time < 0.5
                        ? 32.0 * Math.Pow(time, 6.0) / duration
                        : (1.0 - Math.Pow(-2.0 * time + 2.0, 6.0) / 2.0) / duration;
                }
            }

            public static class Exponential
            {
                public static double In(double time, double duration = 1.0)
                {
                    return (time == 0.0 ? 0.0 : Math.Pow(2.0, 10.0 * time - 10.0)) / duration;
                }

                public static double Out(double time, double duration = 1.0)
                {
                    return (time == 1.0 ? 1.0 : 1.0 - Math.Pow(2.0, -10.0 * time)) / duration;
                }

                public static double InOut(double time, double duration = 1.0)
                {
                    if (time == 0.0)
                        return 0.0;

                    if (time == 1.0)
                        return 1.0;

                    return time < 0.5
                        ? Math.Pow(2.0, 20.0 * time - 10.0) / 2.0 / duration
                        : (2.0 - Math.Pow(2.0, -20.0 * time + 10.0)) / 2.0 / duration;
                }
            }

            public static class Back
            {
                public static double c1 = 1.70158;
                private static double c2 = UiAnimationGlobals.Easing.Back.c1 * 1.525;
                private static double c3 = UiAnimationGlobals.Easing.Back.c1 + 1.0;

                private static void RecalculateOvershoot(double backOvershoot)
                {
                    if (UiAnimationGlobals.Easing.Back.c1 == backOvershoot)
                        return;

                    UiAnimationGlobals.Easing.Back.c1 = backOvershoot;
                    UiAnimationGlobals.Easing.Back.c2 = UiAnimationGlobals.Easing.Back.c1 * 1.525;
                    UiAnimationGlobals.Easing.Back.c3 = UiAnimationGlobals.Easing.Back.c1 + 1.0;
                }

                public static double In(double time, double duration = 1.0, double backOvershoot = 1.70158)
                {
                    UiAnimationGlobals.Easing.Back.RecalculateOvershoot(backOvershoot);

                    return (UiAnimationGlobals.Easing.Back.c3 * time * time * time
                            - UiAnimationGlobals.Easing.Back.c1 * time * time) / duration;
                }

                public static double Out(double time, double duration = 1.0, double backOvershoot = 1.70158)
                {
                    UiAnimationGlobals.Easing.Back.RecalculateOvershoot(backOvershoot);

                    return (1.0
                            + UiAnimationGlobals.Easing.Back.c3 * Math.Pow(time - 1.0, 3.0)
                            + UiAnimationGlobals.Easing.Back.c1 * Math.Pow(time - 1.0, 2.0)) / duration;
                }

                public static double InOut(double time, double duration = 1.0, double backOvershoot = 1.70158)
                {
                    UiAnimationGlobals.Easing.Back.RecalculateOvershoot(backOvershoot);

                    return time < 0.5
                        ? Math.Pow(2.0 * time, 2.0)
                          * ((UiAnimationGlobals.Easing.Back.c2 + 1.0) * 2.0 * time - UiAnimationGlobals.Easing.Back.c2)
                          / 2.0
                          / duration
                        : (Math.Pow(2.0 * time - 2.0, 2.0)
                           * ((UiAnimationGlobals.Easing.Back.c2 + 1.0) * (time * 2.0 - 2.0) + UiAnimationGlobals.Easing.Back.c2)
                           + 2.0)
                          / 2.0
                          / duration;
                }
            }
        }
    }

    // --------------------------------------------------------------------
    // External/unknown types referenced by the decompiled code:
    // These are placeholders so this file can compile as-is.
    // Replace them with your real implementations/namespaces.
    // --------------------------------------------------------------------

    internal static class CursorUtil // was: \uFFFD\uFFFD\uFFFDᚂל\uD800\uDC8Bᚅ\uFFFDܓᛚ\uFFFDᚈQ\uD80C\uDFA2
    {
        public static void EnableModernCursor()
        {
            // placeholder
        }
    }

    internal static class AnimationLoopController // was: ᚎܗ\uFFFDSᚍ\uFFFDᛇܙܟד
    {
        public static void Stop()
        {
            // placeholder
        }
    }

    internal static class UiGraphicsUtil // was: Mᚺ\uFFFDΦ\uFFFDᛉ\uFFFDΨܚΤ\uFFFD\uFFFDᛜΙᚈ\uFFFD
    {
        public static int[] GetRefreshRates() => Array.Empty<int>();
        public static int GetHighestRefreshRate() => 60;
    }

    internal static class ExternalBlurLibrary // was: \uFFFD\uFFFDᚌ\uFFFDܝΓ\uFFFD\uFFFDᚌᚐ
    {
        internal static class GaussianBlur // was: ΒᛒU...
        {
            public static void Apply(ref Bitmap bitmap, float radius)
            {
                // placeholder
            }
        }

        internal static class BoxBlur // was: ΑܦA...
        {
            public static void Apply(ref Bitmap bitmap, float radius)
            {
                // placeholder
            }
        }
    }
}
