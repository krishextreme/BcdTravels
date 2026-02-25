
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Ledger.BitUI
{
    /// <summary>
    /// Simple blur implementations operating on 24bpp RGB bitmaps via LockBits + pointer math.
    /// </summary>
    public static class BitmapBlurEffects
    {
        private static float Clamp(float value, float min, float max)
        {
            if ((double)value < (double)min)
                return min;
            return (double)value > (double)max ? max : value;
        }

        /// <summary>
        /// Separable Gaussian blur (horizontal pass into temp buffer, then vertical pass into source).
        /// </summary>
        public static class GaussianBlur
        {
            public static unsafe void Apply(ref Bitmap bitmap, float radius)
            {
                if ((double)radius < 0.10000000149011612)
                    return;

                BitmapData bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadWrite,
                    PixelFormat.Format24bppRgb
                );

                int stride = bitmapData.Stride;
                IntPtr scan0 = bitmapData.Scan0;
                int width = bitmap.Width;
                int height = bitmap.Height;

                byte* src = (byte*)scan0.ToPointer();

                float[] kernel = GaussianBlur.CreateGaussianKernel(radius);
                int half = kernel.Length / 2;

                // temp buffer for first pass
                byte* temp = stackalloc byte[height * stride];

                // Horizontal pass
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        float b = 0.0f;
                        float g = 0.0f;
                        float r = 0.0f;
                        float wsum = 0.0f;

                        for (int k = -half; k <= half; ++k)
                        {
                            int sx = x + k;
                            if (sx >= 0 && sx < width)
                            {
                                byte* p = src + y * stride + sx * 3;
                                float w = kernel[half + k];

                                b += (float)(*p) * w;
                                g += (float)p[1] * w;
                                r += (float)p[2] * w;
                                wsum += w;
                            }
                        }

                        byte* d = temp + y * stride + x * 3;
                        *d = (byte)BitmapBlurEffects.Clamp(b / wsum, 0.0f, (float)byte.MaxValue);
                        d[1] = (byte)BitmapBlurEffects.Clamp(g / wsum, 0.0f, (float)byte.MaxValue);
                        d[2] = (byte)BitmapBlurEffects.Clamp(r / wsum, 0.0f, (float)byte.MaxValue);
                    }
                }

                // Vertical pass
                for (int x = 0; x < width; ++x)
                {
                    for (int y = 0; y < height; ++y)
                    {
                        float b = 0.0f;
                        float g = 0.0f;
                        float r = 0.0f;
                        float wsum = 0.0f;

                        for (int k = -half; k <= half; ++k)
                        {
                            int sy = y + k;
                            if (sy >= 0 && sy < height)
                            {
                                byte* p = temp + sy * stride + x * 3;
                                float w = kernel[half + k];

                                b += (float)(*p) * w;
                                g += (float)p[1] * w;
                                r += (float)p[2] * w;
                                wsum += w;
                            }
                        }

                        byte* d = src + y * stride + x * 3;
                        *d = (byte)BitmapBlurEffects.Clamp(b / wsum, 0.0f, (float)byte.MaxValue);
                        d[1] = (byte)BitmapBlurEffects.Clamp(g / wsum, 0.0f, (float)byte.MaxValue);
                        d[2] = (byte)BitmapBlurEffects.Clamp(r / wsum, 0.0f, (float)byte.MaxValue);
                    }
                }

                bitmap.UnlockBits(bitmapData);
            }

            private static float[] CreateGaussianKernel(float radius)
            {
                int length = (int)(Math.Ceiling((double)radius) * 2.0) + 1;
                float[] kernel = new float[length];

                float sigma = radius / 2f;
                float a = 1f / ((float)Math.Sqrt(2.0 * Math.PI) * sigma);
                float twoSigmaSq = 2f * sigma * sigma;

                for (int i = 0; i < length; ++i)
                {
                    int x = i - length / 2;
                    kernel[i] = a * (float)Math.Exp((double)(-x * x) / (double)twoSigmaSq);
                }

                return kernel;
            }
        }

        /// <summary>
        /// Separable blur using a quadratic (parabolic) kernel (horizontal pass into temp, then vertical).
        /// </summary>
        public static class QuadraticKernelBlur
        {
            public static unsafe void Apply(ref Bitmap bitmap, float radius)
            {
                if ((double)radius < 0.10000000149011612)
                    return;

                BitmapData bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadWrite,
                    PixelFormat.Format24bppRgb
                );

                int stride = bitmapData.Stride;
                IntPtr scan0 = bitmapData.Scan0;
                int width = bitmap.Width;
                int height = bitmap.Height;

                byte* src = (byte*)scan0.ToPointer();

                float[] kernel = QuadraticKernelBlur.CreateQuadraticKernel(radius);
                int half = kernel.Length / 2;

                byte* temp = stackalloc byte[height * stride];

                // Horizontal pass
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        float b = 0.0f;
                        float g = 0.0f;
                        float r = 0.0f;
                        float wsum = 0.0f;

                        for (int k = -half; k <= half; ++k)
                        {
                            int sx = x + k;
                            if (sx >= 0 && sx < width)
                            {
                                byte* p = src + y * stride + sx * 3;
                                float w = kernel[half + k];

                                b += (float)(*p) * w;
                                g += (float)p[1] * w;
                                r += (float)p[2] * w;
                                wsum += w;
                            }
                        }

                        byte* d = temp + y * stride + x * 3;
                        *d = (byte)BitmapBlurEffects.Clamp(b / wsum, 0.0f, (float)byte.MaxValue);
                        d[1] = (byte)BitmapBlurEffects.Clamp(g / wsum, 0.0f, (float)byte.MaxValue);
                        d[2] = (byte)BitmapBlurEffects.Clamp(r / wsum, 0.0f, (float)byte.MaxValue);
                    }
                }

                // Vertical pass
                for (int x = 0; x < width; ++x)
                {
                    for (int y = 0; y < height; ++y)
                    {
                        float b = 0.0f;
                        float g = 0.0f;
                        float r = 0.0f;
                        float wsum = 0.0f;

                        for (int k = -half; k <= half; ++k)
                        {
                            int sy = y + k;
                            if (sy >= 0 && sy < height)
                            {
                                byte* p = temp + sy * stride + x * 3;
                                float w = kernel[half + k];

                                b += (float)(*p) * w;
                                g += (float)p[1] * w;
                                r += (float)p[2] * w;
                                wsum += w;
                            }
                        }

                        byte* d = src + y * stride + x * 3;
                        *d = (byte)BitmapBlurEffects.Clamp(b / wsum, 0.0f, (float)byte.MaxValue);
                        d[1] = (byte)BitmapBlurEffects.Clamp(g / wsum, 0.0f, (float)byte.MaxValue);
                        d[2] = (byte)BitmapBlurEffects.Clamp(r / wsum, 0.0f, (float)byte.MaxValue);
                    }
                }

                bitmap.UnlockBits(bitmapData);
            }

            private static float[] CreateQuadraticKernel(float radius)
            {
                int length = (int)(Math.Ceiling((double)radius) * 2.0) + 1;
                float[] kernel = new float[length];

                float radiusSq = radius * radius;

                for (int i = 0; i < length; ++i)
                {
                    float x = (float)i - radius;
                    kernel[i] = (float)(1.0 - (double)x * (double)x / (double)radiusSq);

                    if ((double)kernel[i] < 0.0)
                        kernel[i] = 0.0f;
                }

                return kernel;
            }
        }
    }
}
