﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using PicSum.Core.Base.Conf;
using PicSum.Core.Data.FileAccessor;
using PicSum.Core.Task.AsyncTask;
using SWF.Common;

namespace PicSum.Task.AsyncLogic
{
    /// <summary>
    /// 画像ファイル読込ロジック
    /// </summary>
    internal class ReadImageFileAsyncLogic : AsyncLogicBase
    {
        public ReadImageFileAsyncLogic(AsyncFacadeBase facade) : base(facade) { }

        public Image CreateImage(string filePath, Bitmap srcImg, ImageSizeMode sizeMode, Size drawSize)
        {
            if (srcImg == null)
            {
                throw new ArgumentNullException("srcImg");
            }

            if (sizeMode == ImageSizeMode.Original ||
                sizeMode == ImageSizeMode.FitOnlyBigImage && srcImg.Width <= drawSize.Width && srcImg.Height <= drawSize.Height)
            {
                Console.WriteLine("Clone");
                var sw = Stopwatch.StartNew();
                var destImg = (Image)srcImg.Clone();
                sw.Stop();
                Console.WriteLine("[{0}] Clone", sw.ElapsedMilliseconds);
                return destImg;
            }
            else
            {
                var scale = Math.Min(drawSize.Width / (double)srcImg.Width, drawSize.Height / (double)srcImg.Height);
                var destImg = ImageUtil.ResizeImage(srcImg, scale);
                if (destImg == null)
                {
                    throw new ImageException(filePath);
                }

                return destImg;
            }
        }

        public Image CreateThumbnail(Image srcImg, int thumbSize, ImageSizeMode sizeMode)
        {
            if (srcImg == null)
            {
                throw new ArgumentNullException("srcImg");
            }

            if (thumbSize < 0)
            {
                throw new ArgumentOutOfRangeException("thumbSize");
            }

            Console.WriteLine("CreateThumbnail");
            var sw = Stopwatch.StartNew();

            var scale = Math.Min(thumbSize / (double)srcImg.Width, thumbSize / (double)srcImg.Height);
            var w = (int)(srcImg.Width * scale);
            var h = (int)(srcImg.Height * scale);

            var destImg = new Bitmap(w, h);
            using (var g = Graphics.FromImage(destImg))
            {
                g.InterpolationMode = InterpolationMode.Low;
                g.SmoothingMode = SmoothingMode.HighSpeed;
                if (sizeMode == ImageSizeMode.Original)
                {
                    using (var thumb = srcImg.GetThumbnailImage(w, h, () => false, IntPtr.Zero))
                    {
                        g.DrawImage(thumb, 0, 0, w, h);
                    }
                }
                else
                {
                    g.FillRectangle(Brushes.Yellow, new Rectangle(0, 0, w, h));
                }
            }

            sw.Stop();
            Console.WriteLine("[{0}] CreateThumbnail", sw.ElapsedMilliseconds);

            return destImg;
        }
    }
}
