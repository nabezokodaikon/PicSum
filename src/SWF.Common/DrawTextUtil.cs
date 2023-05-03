﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using WinApi;

namespace SWF.Common
{
    public static class DrawTextUtil
    {
        public enum TextStyle
        {
            Normal,
            Glowing
        }

        private readonly static bool _isSupportedDrawThemeTextExWindowsVersion = (6 <= Environment.OSVersion.Version.Major);

        public static void DrawText(Graphics srcDc, string text, Font font, Rectangle bounds, Color color, TextFormatFlags flags, TextStyle style)
        {
            if (_isSupportedDrawThemeTextExWindowsVersion && isSupportedTheme())
            {
                drawGrassText(srcDc, text, font, bounds, color, flags, style);
            }
            else
            {
                drawClassicText(srcDc, text, font, bounds, color, flags);
            }
        }

        private static void drawClassicText(Graphics srcDc, string text, Font font, Rectangle bounds, Color color, TextFormatFlags flags)
        {
            TextRenderer.DrawText(srcDc, text, font, bounds, color, flags);
        }

        private static void drawGrassText(Graphics srcDc, string text, Font font, Rectangle bounds, Color color, TextFormatFlags flags, TextStyle style)
        {
            IntPtr srcHdc = srcDc.GetHdc();

            // Create a memory DC so we can work offscreen
            IntPtr memoryHdc = WinApiMembers.CreateCompatibleDC(srcHdc);

            // Create a device-independent bitmap and select it into our DC
            WinApiMembers.BITMAPINFO bi = new WinApiMembers.BITMAPINFO();
            bi.biSize = Marshal.SizeOf(bi);
            bi.biWidth = bounds.Width;
            bi.biHeight = -bounds.Height;
            bi.biPlanes = 1;
            bi.biBitCount = 32;
            bi.biCompression = 0; // BI_RGB
            IntPtr dib = WinApiMembers.CreateDIBSection(srcHdc, bi, 0, 0, IntPtr.Zero, 0);
            WinApiMembers.SelectObject(memoryHdc, dib);

            // Create and select font
            IntPtr fontHandle = font.ToHfont();
            WinApiMembers.SelectObject(memoryHdc, fontHandle);

            // Draw glowing text
            VisualStyleRenderer renderer = new VisualStyleRenderer(VisualStyleElement.Window.Caption.Active);
            WinApiMembers.DTTOPTS dttOpts = new WinApiMembers.DTTOPTS();
            dttOpts.dwSize = Marshal.SizeOf(typeof(WinApiMembers.DTTOPTS));
            dttOpts.dwFlags = getDwFlags(style);

            dttOpts.crText = ColorTranslator.ToWin32(color);
            dttOpts.iGlowSize = 8; // This is about the size Microsoft Word 2007 uses
            WinApiMembers.RECT textBounds = new WinApiMembers.RECT(0, 0, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);

            WinApiMembers.BitBlt(memoryHdc, 0, 0, bounds.Width, bounds.Height, srcHdc, bounds.Left, bounds.Top, WinApiMembers.SRCCOPY);

            WinApiMembers.DrawThemeTextEx(renderer.Handle, memoryHdc, 0, 0, text, -1, (int)flags, ref textBounds, ref dttOpts);

            // Copy to foreground
            WinApiMembers.BitBlt(srcHdc, bounds.Left, bounds.Top, bounds.Width, bounds.Height, memoryHdc, 0, 0, WinApiMembers.SRCCOPY);

            // Clean up
            WinApiMembers.DeleteObject(fontHandle);
            WinApiMembers.DeleteObject(dib);
            WinApiMembers.DeleteDC(memoryHdc);

            srcDc.ReleaseHdc(srcHdc);
        }

        private static bool isSupportedTheme()
        {
            if (VisualStyleInformation.IsSupportedByOS &&
                VisualStyleInformation.IsEnabledByUser)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool hasUxThemeDll()
        {
            IntPtr dll = WinApiMembers.LoadLibrary("UxTheme.dll");
            if (dll != IntPtr.Zero)
            {
                WinApiMembers.FreeLibrary(dll);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static int getDwFlags(TextStyle style)
        {
            if (style == TextStyle.Glowing)
            {
                return WinApiMembers.DTT_COMPOSITED |
                       WinApiMembers.DTT_GLOWSIZE |
                       WinApiMembers.DTT_TEXTCOLOR;
            }
            else
            {
                return WinApiMembers.DTT_COMPOSITED |
                       WinApiMembers.DTT_TEXTCOLOR;
            }
        }
    }
}
