using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Ledger.Animations
{
    public static class BlurHelper
    {
        /// <summary>
        /// Applies a box blur effect to the bitmap
        /// </summary>
        /// <param name="bitmap">The bitmap to blur (passed by reference)</param>
        /// <param name="blurAmount">The amount of blur to apply (higher = more blur)</param>
        public static void Apply(ref Bitmap bitmap, float blurAmount)
        {
            if (bitmap == null || blurAmount <= 0)
                return;

            // Convert blur amount to radius (integer)
            int radius = Math.Max(1, (int)Math.Ceiling(blurAmount));

            // Apply box blur multiple times for better quality
            int iterations = Math.Max(1, (int)(blurAmount / 2));

            for (int i = 0; i < iterations; i++)
            {
                bitmap = BoxBlur(bitmap, radius);
            }
        }

        /// <summary>
        /// Performs a box blur on the bitmap
        /// </summary>
        private static Bitmap BoxBlur(Bitmap source, int radius)
        {
            if (source == null)
                return null;

            int width = source.Width;
            int height = source.Height;

            // Lock the bitmap's bits for fast access
            BitmapData sourceData = source.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * height];
            byte[] resultBuffer = new byte[sourceData.Stride * height];

            // Copy bitmap data to buffer
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            // Horizontal blur
            HorizontalBlur(pixelBuffer, resultBuffer, width, height, sourceData.Stride, radius);

            // Vertical blur (using result of horizontal blur)
            VerticalBlur(resultBuffer, pixelBuffer, width, height, sourceData.Stride, radius);

            // Copy result back to bitmap
            Marshal.Copy(pixelBuffer, 0, sourceData.Scan0, pixelBuffer.Length);
            source.UnlockBits(sourceData);

            return source;
        }

        /// <summary>
        /// Applies horizontal blur pass
        /// </summary>
        private static void HorizontalBlur(byte[] source, byte[] destination, int width, int height, int stride, int radius)
        {
            int diameter = radius * 2 + 1;

            for (int y = 0; y < height; y++)
            {
                int lineOffset = y * stride;

                for (int x = 0; x < width; x++)
                {
                    int sumB = 0, sumG = 0, sumR = 0, sumA = 0;
                    int count = 0;

                    // Calculate the blur window
                    for (int kx = -radius; kx <= radius; kx++)
                    {
                        int px = x + kx;

                        // Clamp to image boundaries
                        if (px < 0) px = 0;
                        if (px >= width) px = width - 1;

                        int pixelOffset = lineOffset + (px * 4);

                        sumB += source[pixelOffset];
                        sumG += source[pixelOffset + 1];
                        sumR += source[pixelOffset + 2];
                        sumA += source[pixelOffset + 3];
                        count++;
                    }

                    int destOffset = lineOffset + (x * 4);
                    destination[destOffset] = (byte)(sumB / count);
                    destination[destOffset + 1] = (byte)(sumG / count);
                    destination[destOffset + 2] = (byte)(sumR / count);
                    destination[destOffset + 3] = (byte)(sumA / count);
                }
            }
        }

        /// <summary>
        /// Applies vertical blur pass
        /// </summary>
        private static void VerticalBlur(byte[] source, byte[] destination, int width, int height, int stride, int radius)
        {
            int diameter = radius * 2 + 1;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int sumB = 0, sumG = 0, sumR = 0, sumA = 0;
                    int count = 0;

                    // Calculate the blur window
                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        int py = y + ky;

                        // Clamp to image boundaries
                        if (py < 0) py = 0;
                        if (py >= height) py = height - 1;

                        int pixelOffset = (py * stride) + (x * 4);

                        sumB += source[pixelOffset];
                        sumG += source[pixelOffset + 1];
                        sumR += source[pixelOffset + 2];
                        sumA += source[pixelOffset + 3];
                        count++;
                    }

                    int destOffset = (y * stride) + (x * 4);
                    destination[destOffset] = (byte)(sumB / count);
                    destination[destOffset + 1] = (byte)(sumG / count);
                    destination[destOffset + 2] = (byte)(sumR / count);
                    destination[destOffset + 3] = (byte)(sumA / count);
                }
            }
        }

        /// <summary>
        /// Alternative: Fast Gaussian-like blur using accumulation
        /// This is faster but slightly less accurate
        /// </summary>
        public static void ApplyFast(ref Bitmap bitmap, float blurAmount)
        {
            if (bitmap == null || blurAmount <= 0)
                return;

            int radius = Math.Max(1, (int)Math.Ceiling(blurAmount));
            bitmap = FastBoxBlur(bitmap, radius);
        }

        private static Bitmap FastBoxBlur(Bitmap source, int radius)
        {
            if (source == null)
                return null;

            int width = source.Width;
            int height = source.Height;

            BitmapData sourceData = source.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            int pixelCount = sourceData.Stride * height;
            byte[] pixels = new byte[pixelCount];
            byte[] result = new byte[pixelCount];

            Marshal.Copy(sourceData.Scan0, pixels, 0, pixelCount);

            // Single pass blur with larger kernel
            int diameter = radius * 2 + 1;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int sumB = 0, sumG = 0, sumR = 0, sumA = 0;
                    int count = 0;

                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        for (int kx = -radius; kx <= radius; kx++)
                        {
                            int px = Math.Max(0, Math.Min(width - 1, x + kx));
                            int py = Math.Max(0, Math.Min(height - 1, y + ky));

                            int offset = (py * sourceData.Stride) + (px * 4);

                            sumB += pixels[offset];
                            sumG += pixels[offset + 1];
                            sumR += pixels[offset + 2];
                            sumA += pixels[offset + 3];
                            count++;
                        }
                    }

                    int destOffset = (y * sourceData.Stride) + (x * 4);
                    result[destOffset] = (byte)(sumB / count);
                    result[destOffset + 1] = (byte)(sumG / count);
                    result[destOffset + 2] = (byte)(sumR / count);
                    result[destOffset + 3] = (byte)(sumA / count);
                }
            }

            Marshal.Copy(result, 0, sourceData.Scan0, pixelCount);
            source.UnlockBits(sourceData);

            return source;
        }
    }
}
