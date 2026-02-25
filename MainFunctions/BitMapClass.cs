
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Ledger.BitUI.BitMapClass;

namespace Ledger.BitUI
{
    public static class BitMapClass
    {
        public static int arrowThicknessHardcoded = 2;

        public static int[] GetRefreshRates()
        {
            return Native.GetRefreshRates();
        }

        public static int GetHighestRefreshRate()
        {
            return Native.GetRefreshRate();
        }

        public static GraphicsPath RoundHexagon(Rectangle bounds, float rounding)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            rounding = Math.Min(rounding, (float)Math.Min(bounds.Width, bounds.Height) / 4f);

            PointF[] pointFArray = new PointF[6];
            float width = (float)bounds.Width;
            float height = (float)bounds.Height;

            pointFArray[0] = new PointF((float)bounds.X + width / 2f, (float)bounds.Y);
            pointFArray[1] = new PointF((float)bounds.X + width, (float)bounds.Y + height / 4f);
            pointFArray[2] = new PointF((float)bounds.X + width, (float)bounds.Y + (float)(3.0 * (double)height / 4.0));
            pointFArray[3] = new PointF((float)bounds.X + width / 2f, (float)bounds.Y + height);
            pointFArray[4] = new PointF((float)bounds.X, (float)bounds.Y + (float)(3.0 * (double)height / 4.0));
            pointFArray[5] = new PointF((float)bounds.X, (float)bounds.Y + height / 4f);

            for (int index = 0; index < pointFArray.Length; ++index)
            {
                PointF pointF1 = pointFArray[index];
                PointF pointF2 = pointFArray[(index - 1 + pointFArray.Length) % pointFArray.Length];
                PointF pointF3 = pointFArray[(index + 1) % pointFArray.Length];

                PointF pointF4 = Normalize(new PointF(pointF2.X - pointF1.X, pointF2.Y - pointF1.Y));
                PointF pointF5 = Normalize(new PointF(pointF3.X - pointF1.X, pointF3.Y - pointF1.Y));

                PointF pt1 = new PointF(pointF1.X + pointF4.X * rounding, pointF1.Y + pointF4.Y * rounding);
                PointF pt4 = new PointF(pointF1.X + pointF5.X * rounding, pointF1.Y + pointF5.Y * rounding);

                PointF pt2 = new PointF(pointF1.X + pointF4.X * (rounding / 2f), pointF1.Y + pointF4.Y * (rounding / 2f));
                PointF pt3 = new PointF(pointF1.X + pointF5.X * (rounding / 2f), pointF1.Y + pointF5.Y * (rounding / 2f));

                if (index == 0)
                    graphicsPath.StartFigure();

                graphicsPath.AddBezier(pt1, pt2, pt3, pt4);
            }

            graphicsPath.CloseFigure();
            return graphicsPath;
        }

        public static PointF Normalize(PointF point)
        {
            float num = (float)Math.Sqrt((double)point.X * (double)point.X + (double)point.Y * (double)point.Y);
            return new PointF(point.X / num, point.Y / num);
        }

        public static GraphicsPath RoundRect(int x, int y, int width, int height, int borderRadius)
        {
            return RoundRect((RectangleF)new Rectangle(x, y, width, height), new Padding(borderRadius));
        }

        public static GraphicsPath RoundRect(Rectangle rectangle, int borderRadius)
        {
            return RoundRect((RectangleF)rectangle, new Padding(borderRadius));
        }

        public static GraphicsPath RoundRect(RectangleF rectangle, int borderRadius)
        {
            return RoundRect(rectangle, new Padding(borderRadius));
        }

        public static GraphicsPath RoundRect(RectangleF rectangle, Padding borderRadius)
        {
            // Equivalent to the decompiler's compiler-generated state holder.
            RoundRectState state;
            state.path = new GraphicsPath();

            float diameter1 = (float)(borderRadius.Top * 2);
            AddArc(rectangle.X, rectangle.Y, diameter1, 180f, 90f, ref state);

            float diameter2 = (float)(borderRadius.Left * 2);
            AddArc(rectangle.Right - diameter2, rectangle.Y, diameter2, 270f, 90f, ref state);

            float diameter3 = (float)(borderRadius.Bottom * 2);
            AddArc(rectangle.Right - diameter3, rectangle.Bottom - diameter3, diameter3, 0.0f, 90f, ref state);

            float diameter4 = (float)(borderRadius.Right * 2);
            AddArc(rectangle.X, rectangle.Bottom - diameter4, diameter4, 90f, 90f, ref state);

            state.path.CloseFigure();
            return state.path;
        }

        // This replaces the decompiler's: <RoundRect>g__AddArc|7_0(...)
        // Behavior: add an arc and then a line (standard rounded-rect building pattern).
        private static void AddArc(float x, float y, float diameter, float startAngle, float sweepAngle, ref RoundRectState state)
        {
            if (diameter <= 0f)
            {
                // Degenerate corner: treat as a point/line join (no arc).
                state.path.AddLine(x, y, x, y);
                return;
            }

            state.path.AddArc(x, y, diameter, diameter, startAngle, sweepAngle);
        }

        private struct RoundRectState
        {
            public GraphicsPath path;
        }

        public static GraphicsPath Checkmark(Rectangle area)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            Point[] points = new Point[3]
            {
                new Point(area.Left + (int)((double)area.Width * 0.25), area.Top + (int)((double)area.Height * 0.5)),
                new Point(area.Left + (int)((double)area.Width * 0.45), area.Top + (int)((double)area.Height * 0.7)),
                new Point(area.Right - (int)((double)area.Width * 0.3), area.Top + (int)((double)area.Height * 0.3))
            };
            graphicsPath.AddLines(points);
            return graphicsPath;
        }

        public static GraphicsPath Checkmark(RectangleF area)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            PointF[] points = new PointF[3]
            {
                new PointF(area.Left + (float)(int)((double)area.Width * 0.25), area.Top + (float)(int)((double)area.Height * 0.5)),
                new PointF(area.Left + (float)(int)((double)area.Width * 0.45), area.Top + (float)(int)((double)area.Height * 0.7)),
                new PointF(area.Right - (float)(int)((double)area.Width * 0.3), area.Top + (float)(int)((double)area.Height * 0.3))
            };
            graphicsPath.AddLines(points);
            return graphicsPath;
        }

        public static GraphicsPath Checkmark(RectangleF area, Point symbolsOffset)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            area.Offset((PointF)symbolsOffset);
            PointF[] points = new PointF[3]
            {
                new PointF(area.Left + (float)(int)((double)area.Width * 0.25), area.Top + (float)(int)((double)area.Height * 0.5)),
                new PointF(area.Left + (float)(int)((double)area.Width * 0.45), area.Top + (float)(int)((double)area.Height * 0.7)),
                new PointF(area.Right - (float)(int)((double)area.Width * 0.3), area.Top + (float)(int)((double)area.Height * 0.3))
            };
            graphicsPath.AddLines(points);
            return graphicsPath;
        }

        public static GraphicsPath Crossmark(Rectangle rect)
        {
            Rectangle rectangle = rect;
            int width1 = rectangle.Width;
            rectangle.Width = (int)Math.Round((double)rectangle.Width * 0.699999988079071, 0);
            rectangle.Height = rectangle.Width;

            int width2 = rectangle.Width;
            int num = width1 - width2;
            rectangle.Offset(num / 2, 1 + num / 2);

            GraphicsPath graphicsPath = new GraphicsPath();
            Point[] points1 = new Point[2]
            {
                new Point(rectangle.Left, rectangle.Top),
                new Point(rectangle.Right, rectangle.Bottom)
            };
            graphicsPath.AddLines(points1);

            GraphicsPath addingPath = new GraphicsPath();
            Point[] points2 = new Point[2]
            {
                new Point(rectangle.Left, rectangle.Bottom),
                new Point(rectangle.Right, rectangle.Top)
            };
            addingPath.AddLines(points2);

            graphicsPath.AddPath(addingPath, false);
            return graphicsPath;
        }

        public static GraphicsPath Crossmark(Rectangle rect, Point symbolsOffset)
        {
            Rectangle rectangle = rect;
            rectangle.Offset(symbolsOffset);

            int width1 = rectangle.Width;
            rectangle.Width = (int)Math.Round((double)rectangle.Width * 0.699999988079071, 0);
            rectangle.Height = rectangle.Width;

            int width2 = rectangle.Width;
            int num = width1 - width2;
            rectangle.Offset(num / 2, 1 + num / 2);

            GraphicsPath graphicsPath = new GraphicsPath();
            Point[] points1 = new Point[2]
            {
                new Point(rectangle.Left, rectangle.Top),
                new Point(rectangle.Right, rectangle.Bottom)
            };
            graphicsPath.AddLines(points1);

            GraphicsPath addingPath = new GraphicsPath();
            Point[] points2 = new Point[2]
            {
                new Point(rectangle.Left, rectangle.Bottom),
                new Point(rectangle.Right, rectangle.Top)
            };
            addingPath.AddLines(points2);

            graphicsPath.AddPath(addingPath, false);
            return graphicsPath;
        }

        public static GraphicsPath Crossmark(RectangleF rect, Point symbolsOffset)
        {
            RectangleF rectangleF = rect;
            rectangleF.Offset((PointF)symbolsOffset);

            float width1 = rectangleF.Width;
            rectangleF.Width = (float)(int)Math.Round((double)rectangleF.Width * 0.699999988079071, 0);
            rectangleF.Height = rectangleF.Width;

            float width2 = rectangleF.Width;
            float num = width1 - width2;
            rectangleF.Offset(num / 2f, (float)(1.0 + (double)num / 2.0));

            GraphicsPath graphicsPath = new GraphicsPath();
            PointF[] points1 = new PointF[2]
            {
                new PointF(rectangleF.Left, rectangleF.Top),
                new PointF(rectangleF.Right, rectangleF.Bottom)
            };
            graphicsPath.AddLines(points1);

            GraphicsPath addingPath = new GraphicsPath();
            PointF[] points2 = new PointF[2]
            {
                new PointF(rectangleF.Left, rectangleF.Bottom),
                new PointF(rectangleF.Right, rectangleF.Top)
            };
            addingPath.AddLines(points2);

            graphicsPath.AddPath(addingPath, false);
            return graphicsPath;
        }

        public static GraphicsPath Plus(Rectangle rect)
        {
            Rectangle rectangle = rect;
            int width1 = rectangle.Width;
            rectangle.Width = (int)Math.Round((double)rectangle.Width * 0.699999988079071, 0);
            rectangle.Height = rectangle.Width;

            int width2 = rectangle.Width;
            int num = width1 - width2;
            rectangle.Offset(num / 2, 1 + num / 2);

            GraphicsPath graphicsPath = new GraphicsPath();
            Point[] points1 = new Point[2]
            {
                new Point(rectangle.Left, rectangle.Top + rectangle.Height / 2),
                new Point(rectangle.Right, rectangle.Top + rectangle.Height / 2)
            };
            graphicsPath.AddLines(points1);

            GraphicsPath addingPath = new GraphicsPath();
            Point[] points2 = new Point[2]
            {
                new Point(rectangle.Left + rectangle.Width / 2, rectangle.Top),
                new Point(rectangle.Left + rectangle.Width / 2, rectangle.Bottom)
            };
            addingPath.AddLines(points2);

            graphicsPath.AddPath(addingPath, false);
            return graphicsPath;
        }

        public static GraphicsPath LeftArrow(Rectangle rect)
        {
            rect.Height = rect.Width;
            rect.Width /= 2;

            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddLine(rect.Right, rect.Top, rect.Left, rect.Bottom / 2);
            graphicsPath.AddLine(rect.Left, rect.Bottom / 2, rect.Right, rect.Bottom);

            Matrix matrix = new Matrix();
            matrix.Translate(
                (float)(3 + arrowThicknessHardcoded * 2),
                (float)(4 + (arrowThicknessHardcoded + rect.Height / 2))
            );
            matrix.Scale(1f, 0.9f);
            matrix.Scale(0.6f, 0.6f);

            graphicsPath.Transform(matrix);
            return graphicsPath;
        }

        public static GraphicsPath RightArrow(Rectangle rect)
        {
            rect.Height = rect.Width;
            rect.Width /= 2;
            rect.Offset(rect.Width -arrowThicknessHardcoded, 0);

            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddLine(rect.Left, rect.Top, rect.Right, rect.Bottom / 2);
            graphicsPath.AddLine(rect.Right, rect.Bottom / 2, rect.Left, rect.Bottom);

            Matrix matrix = new Matrix();
            matrix.Translate(
                (float)(13 + /*-*/ arrowThicknessHardcoded * 2),
                (float)(4 + (arrowThicknessHardcoded + rect.Height / 2))
            );
            matrix.Scale(1f, 0.9f);
            matrix.Scale(0.6f, 0.6f);

            graphicsPath.Transform(matrix);
            return graphicsPath;
        }

        public static GraphicsPath LeftArrowtest(Rectangle rectangle)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            Point[] points = new Point[3]
            {
                new Point(rectangle.Right, rectangle.Top),
                new Point(rectangle.Left, rectangle.Top + rectangle.Height / 2),
                new Point(rectangle.Right, rectangle.Bottom)
            };
            graphicsPath.AddPolygon(points);
            return graphicsPath;
        }

        public static GraphicsPath DownArrow(Rectangle rect)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            Point[] points = new Point[3]
            {
                new Point(rect.Left, rect.Top),
                new Point(rect.Left + rect.Width / 2, rect.Bottom),
                new Point(rect.Right, rect.Top)
            };
            graphicsPath.AddPolygon(points);
            return graphicsPath;
        }

        public static void CopyProperties(this object source, object destination)
        {
            System.Type type = source != null && destination != null
                ? destination.GetType()
                : throw new Exception("Source or/and Destination Objects are null");

            foreach (PropertyInfo property1 in source.GetType().GetProperties())
            {
                if (property1.CanRead)
                {
                    PropertyInfo property2 = type.GetProperty(property1.Name);
                    if (!(property2 == (PropertyInfo)null)
                        && property2.CanWrite
                        && (!(property2.GetSetMethod(true) != (MethodInfo)null) || !property2.GetSetMethod(true).IsPrivate)
                        && (property2.GetSetMethod().Attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope
                        && property2.PropertyType.IsAssignableFrom(property1.PropertyType))
                    {
                        property2.SetValue(destination, property1.GetValue(source, (object[])null), (object[])null);
                    }
                }
            }
        }

        public static GraphicsPath Star(
            float centerX,
            float centerY,
            float outerRadius,
            float innerRadius,
            int numPoints)
        {
            if (numPoints % 2 == 0 || numPoints < 5)
                throw new ArgumentException("Number of points must be an odd number and greater than or equal to 5.");

            GraphicsPath graphicsPath = new GraphicsPath();
            float num = 360f / (float)numPoints;
            float angleInDegrees = -90f;

            PointF[] points = new PointF[numPoints * 2];
            for (int index = 0; index < numPoints * 2; index += 2)
            {
                points[index] = PointOnCircle(centerX, centerY, outerRadius, angleInDegrees);
                points[index + 1] = PointOnCircle(centerX, centerY, innerRadius, angleInDegrees + num / 2f);
                angleInDegrees += num;
            }

            graphicsPath.AddPolygon(points);
            return graphicsPath;
        }

        private static PointF PointOnCircle(
            float centerX,
            float centerY,
            float radius,
            float angleInDegrees)
        {
            float num = (float)((double)angleInDegrees * Math.PI / 180.0);
            return new PointF(
                centerX + radius * (float)Math.Cos((double)num),
                centerY + radius * (float)Math.Sin((double)num)
            );
        }

        internal static GraphicsPath UpArrow(Rectangle rect)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            Point[] points = new Point[3]
            {
                new Point(rect.Left, rect.Bottom),
                new Point(rect.Left + rect.Width / 2, rect.Top),
                new Point(rect.Right, rect.Bottom)
            };
            graphicsPath.AddPolygon(points);
            return graphicsPath;
        }

        public static void ToggleFormVisibilityWithoutActivating(Form form, bool show)
        {
            if (form == null || form.IsDisposed)
                return;

            if (show)
            {
                if (form.Visible)
                    return;

                Native.NoActivateWindow.ShowWindow(form.Handle, 4);
            }
            else
            {
                if (!form.Visible)
                    return;

                Native.NoActivateWindow.ShowWindow(form.Handle, 0);
            }
        }

        // was ח\uFFFDTY\uD802\uDF05GZ\uFFFD\uD808\uDD0C\uFFFD\uFFFDܩ unicode
        public static class Native
        {
            public const int WM_SYSCOMMAND = 274;
            public const int SC_CLOSE = 61536;
            public const int GWL_EXSTYLE = -20;
            public const int WS_EX_TRANSPARENT = 32 /*0x20*/;
            public const int WS_EX_LAYERED = 524288 /*0x080000*/;
            public const int WS_EX_NOACTIVATE = 134217728 /*0x08000000*/;
            public static bool REFRESH_RATE_OVERRIDE = false;
            public static int SPOOFED_REFRESH_RATE = 60;

            [DllImport("user32.dll")]
            public static extern IntPtr GetDC(IntPtr hwnd);

            [DllImport("user32.dll")]
            public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

            [DllImport("gdi32.dll")]
            public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

            [DllImport("user32.dll")]
            public static extern bool GetCursorPos(
              out Native.CursorPoint lpPoint); // was ...\uFFFDᚐ\uFFFD\uFFFDΟܥΜ\uFFFD\uFFFDᚃ\uFFFD lpPoint

            [DllImport("user32.dll")]
            public static extern short GetAsyncKeyState(int vKey);

            public static bool isClickingLeftMouse()
            {
                return ((uint)Native.GetAsyncKeyState(1) & 32768U /*0x8000*/) > 0U;
            }

            [DllImport("user32.dll")]
            public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll")]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll")]
            private static extern bool EnumDisplaySettings(
              string lpszDeviceName,
              int iModeNum,
              ref Native.DisplayModeInfo lpDevMode); // was ...Ο\uFFFDKRܐ\uFFFD\uFFFDܕᚦᚈᚏס\uFFFD lpDevMode

            [DllImport("user32.dll")]
            private static extern bool EnumDisplayDevices(
              string lpDevice,
              uint iDevNum,
              ref Native.DisplayDeviceInfo lpDisplayDevice, // was ...\uFFFD\uFFFDᚇ\uFFFD\uFFFDᚍܐܓ\uFFFDᚎ\uFFFDܒΙ\uFFFD lpDisplayDevice
              uint dwFlags);

            public static int GetRefreshRate()
            {
                if (Native.REFRESH_RATE_OVERRIDE)
        return Native.SPOOFED_REFRESH_RATE;

                Native.DisplayDeviceInfo lpDisplayDevice =
                  new Native.DisplayDeviceInfo();

                lpDisplayDevice.cb = Marshal.SizeOf < Native.DisplayDeviceInfo > (lpDisplayDevice);

                Native.DisplayModeInfo lpDevMode =
                  new Native.DisplayModeInfo();

                lpDevMode.dmSize = (short)Marshal.SizeOf(typeof(Native.DisplayModeInfo));

                uint iDevNum = 0;
                int refreshRate = 1;

                for (; Native.EnumDisplayDevices((string)null, iDevNum, ref lpDisplayDevice, 0U); ++iDevNum)
                {
                    if (Native.EnumDisplaySettings(lpDisplayDevice.DeviceName, -1, ref lpDevMode))
        {
                    int displayFrequency = lpDevMode.dmDisplayFrequency;
                    if (displayFrequency > refreshRate)
                        refreshRate = displayFrequency;
                }
            }

      return refreshRate;
    }

        public static int[] GetRefreshRates()
        {
            List<int> intList = new List<int>();

            Native.DisplayDeviceInfo lpDisplayDevice =
              new Native.DisplayDeviceInfo();

            lpDisplayDevice.cb = Marshal.SizeOf < Native.DisplayDeviceInfo > (lpDisplayDevice);

            Native.DisplayModeInfo lpDevMode =
              new Native.DisplayModeInfo();

            lpDevMode.dmSize = (short)Marshal.SizeOf(typeof(Native.DisplayModeInfo));

            for (uint iDevNum = 0; Native.EnumDisplayDevices((string)null, iDevNum, ref lpDisplayDevice, 0U); ++iDevNum)
            {
                if (Native.EnumDisplaySettings(lpDisplayDevice.DeviceName, -1, ref lpDevMode))
          intList.Add(lpDevMode.dmDisplayFrequency);
        }

      return intList.ToArray();
    }

    // was \uFFFDᚐ\uFFFD\uFFFDΟܥΜ\uFFFD\uFFFDᚃ\uFFFD unicode
    public struct CursorPoint
    {
        public int X;
        public int Y;
    }

    // was Ο\uFFFDKRܐ\uFFFD\uFFFDܕᚦᚈᚏס\uFFFD unicode
    public struct DisplayModeInfo
    {
        private const int CCHDEVICENAME = 32 /*0x20*/;
        private const int CCHFORMNAME = 32 /*0x20*/;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32 /*0x20*/)]
        public string dmDeviceName;

        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;
        public int dmPositionX;
        public int dmPositionY;
        public int dmDisplayOrientation;
        public int dmDisplayFixedOutput;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32 /*0x20*/)]
        public string dmFormName;

        public short dmLogPixels;
        public int dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;
        public int dmDisplayFlags;
        public int dmDisplayFrequency;
        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;
        public int dmReserved2;
        public int dmPanningWidth;
        public int dmPanningHeight;
    }

    // was \uFFFD\uFFFDᚇ\uFFFD\uFFFDᚍܐܓ\uFFFDᚎ\uFFFDܒΙ\uFFFD unicode
    public struct DisplayDeviceInfo
    {
        public int cb;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32 /*0x20*/)]
        public string DeviceName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128 /*0x80*/)]
        public string DeviceString;

        public int StateFlags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128 /*0x80*/)]
        public string DeviceID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128 /*0x80*/)]
        public string DeviceKey;
    }

    // was \uD80C\uDF03\uFFFDק\uFFFDܠ\uFFFDו\uFFFD\uFFFDY\uFFFD unicode
    internal static class NoActivateWindow
    {
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_LAYERED = 524288 /*0x080000*/;
        internal const int LWA_ALPHA = 2;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_HIDE = 0;

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool SetLayeredWindowAttributes(
          IntPtr hwnd,
          int crKey,
          byte bAlpha,
          int dwFlags);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }

    // was \uFFFD\uFFFDΑᚑ\uFFFD\uFFFDΑ\uFFFDΘᚈ\uFFFDMᚌ unicode
    internal static class LayeredWindowBitmap
    {
        public static void SetBitmap(Bitmap bitmap, int left, int top, IntPtr handle)
        {
            Native.LayeredWindowBitmap.SetBitmap(bitmap, byte.MaxValue, left, top, handle);
        }

        public static void SetBitmap(Bitmap bitmap, byte opacity, int left, int top, IntPtr handle)
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");

            IntPtr dc = Native.LayeredWindowBitmap.LayeredWindowApi.GetDC(IntPtr.Zero);
            IntPtr compatibleDc = Native.LayeredWindowBitmap.LayeredWindowApi.CreateCompatibleDC(dc);

            IntPtr hObject1 = IntPtr.Zero;
            IntPtr hObject2 = IntPtr.Zero;

            try
            {
                hObject1 = bitmap.GetHbitmap(Color.FromArgb(0));
                hObject2 = Native.LayeredWindowBitmap.LayeredWindowApi.SelectObject(compatibleDc, hObject1);

                Native.LayeredWindowBitmap.LayeredWindowApi.SizeValue psize =
                  new Native.LayeredWindowBitmap.LayeredWindowApi.SizeValue(bitmap.Width, bitmap.Height);

                Native.LayeredWindowBitmap.LayeredWindowApi.PointValue pptDst =
                  new Native.LayeredWindowBitmap.LayeredWindowApi.PointValue(left, top);

                Native.LayeredWindowBitmap.LayeredWindowApi.PointValue pprSrc =
                  new Native.LayeredWindowBitmap.LayeredWindowApi.PointValue(0, 0);

                Native.LayeredWindowBitmap.LayeredWindowApi.BLENDFUNCTION pblend =
                  new Native.LayeredWindowBitmap.LayeredWindowApi.BLENDFUNCTION()
            {
              BlendOp = 0,
              BlendFlags = 0,
              SourceConstantAlpha = opacity,
              AlphaFormat = 1
            };

          int num = (int)Native.LayeredWindowBitmap.LayeredWindowApi.UpdateLayeredWindow(handle, dc, ref pptDst, ref psize, compatibleDc, ref pprSrc, 0, ref pblend, 2);
        }
        finally
        {
          Native.LayeredWindowBitmap.LayeredWindowApi.ReleaseDC(IntPtr.Zero, dc);

          if (hObject1 != IntPtr.Zero)
          {
            Native.LayeredWindowBitmap.LayeredWindowApi.SelectObject(compatibleDc, hObject2);
            int num = (int)Native.LayeredWindowBitmap.LayeredWindowApi.DeleteObject(hObject1);
          }

          int num1 = (int)Native.LayeredWindowBitmap.LayeredWindowApi.DeleteDC(compatibleDc);
        }
      }

      // was ח\uFFFDTY\uD802\uDF05GZ\uFFFD\uD808\uDD0C\uFFFD\uFFFDܩ (nested) unicode
      internal static class LayeredWindowApi
      {
        public const int ULW_COLORKEY = 1;
        public const int ULW_ALPHA = 2;
        public const int ULW_OPAQUE = 4;
        public const byte AC_SRC_OVER = 0;
        public const byte AC_SRC_ALPHA = 1;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern LayeredWindowApiNativeBoolResult UpdateLayeredWindow(
          IntPtr hwnd,
          IntPtr hdcDst,
          ref Native.LayeredWindowBitmap.LayeredWindowApi.PointValue pptDst,
          ref Native.LayeredWindowBitmap.LayeredWindowApi.SizeValue psize,
          IntPtr hdcSrc,
          ref Native.LayeredWindowBitmap.LayeredWindowApi.PointValue pprSrc,
          int crKey,
          ref Native.LayeredWindowBitmap.LayeredWindowApi.BLENDFUNCTION pblend,
          int dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern LayeredWindowApiNativeBoolResult DeleteDC(
          IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern LayeredWindowApiNativeBoolResult DeleteObject(
          IntPtr hObject);

        // was ܗXᚊΝ\uFFFDש\uFFFD\uFFFDחᚆ\uFFFDJ unicode
        public enum LayeredWindowApiNativeBoolResult
                    {
          False,
          True,
        }

                    // was ᚋ\uFFFDרW\uFFFDΕ\uD802\uDF07Ν\uFFFDᛟ\uFFFD unicode
                    public struct PointValue
                    {
                        public int x;
                        public int y;

                        public PointValue(int x, int y)
                        {
                            this.x = x;
                            this.y = y;
                        }
                    }

                    public struct SizeValue
                    {
                        public int cx;
                        public int cy;

                        public SizeValue(int cx, int cy)
                        {
                            this.cx = cx;
                            this.cy = cy;
                        }
                    }

                    [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct ColorsBitMapClass
        {
          public byte Blue;
          public byte Green;
          public byte Red;
          public byte Alpha;
        }

        // was ᚦ\uFFFDQ\uFFFDᚌ\uFFFDܥ\uFFFD\uFFFDW unicode
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
          public byte BlendOp;
          public byte BlendFlags;
          public byte SourceConstantAlpha;
          public byte AlphaFormat;
        }
      }
    }
  }

    }
}
