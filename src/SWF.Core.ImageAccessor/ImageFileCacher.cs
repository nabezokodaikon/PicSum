using OpenCvSharp.Extensions;
using SWF.Core.FileAccessor;
using System.Runtime.Versioning;

namespace SWF.Core.ImageAccessor
{
    [SupportedOSPlatform("windows")]
    public static class ImageFileCacher
    {
        private const int CACHE_CAPACITY = 12;
        private static readonly List<ImageFileCacheEntity> CACHE_LIST = new(CACHE_CAPACITY);
        private static readonly Dictionary<string, ImageFileCacheEntity> CACHE_DICTIONARY = new(CACHE_CAPACITY);
        private static readonly object CACHE_LOCK = new();

        public static void DisposeStaticResources()
        {
            foreach (var cache in CACHE_LIST)
            {
                cache.Dispose();
            }
        }

        public static Size GetSize(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

            return Read(filePath, static cache =>
            {
                if (cache.Bitmap == null)
                {
                    throw new NullReferenceException("キャッシュのBitmapがNullです。");
                }

                return cache.Bitmap.Size;
            });
        }

        public static CvImage GetCvImage(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

            return Read(filePath, static cache =>
            {
                if (cache.Bitmap == null)
                {
                    throw new NullReferenceException("キャッシュのBitmapがNullです。");
                }

                return new CvImage(cache.Bitmap.ToMat(), cache.Bitmap.PixelFormat);
            });
        }

        public static void Create(string filePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

            var timestamp = FileUtil.GetUpdateDate(filePath);

            lock (CACHE_LOCK)
            {
                if (CACHE_DICTIONARY.TryGetValue(filePath, out var cache))
                {
                    if (timestamp == cache.Timestamp)
                    {
                        return;
                    }
                }

                if (cache != null)
                {
                    CACHE_LIST.Remove(cache);
                    CACHE_DICTIONARY.Remove(cache.FilePath);
                    cache.Dispose();
                }

                if (CACHE_LIST.Count > CACHE_CAPACITY)
                {
                    var removeCache = CACHE_LIST[0];
                    CACHE_LIST.Remove(removeCache);
                    CACHE_DICTIONARY.Remove(removeCache.FilePath);
                    removeCache.Dispose();
                }

                var bitmap = ImageUtil.ReadImageFile(filePath);
                ImageFileSizeCacher.Set(filePath, bitmap.Size);
                var newCache = new ImageFileCacheEntity(filePath, bitmap, timestamp);
                CACHE_DICTIONARY.Add(newCache.FilePath, newCache);
                CACHE_LIST.Add(newCache);
            }
        }

        private static T Read<T>(string filePath, Func<ImageFileCacheEntity, T> resultFunc)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

            var timestamp = FileUtil.GetUpdateDate(filePath);

            lock (CACHE_LOCK)
            {
                if (CACHE_DICTIONARY.TryGetValue(filePath, out var cache))
                {
                    if (timestamp == cache.Timestamp)
                    {
                        return resultFunc(cache);
                    }
                }

                if (cache != null)
                {
                    CACHE_LIST.Remove(cache);
                    CACHE_DICTIONARY.Remove(cache.FilePath);
                    cache.Dispose();
                }

                if (CACHE_LIST.Count > CACHE_CAPACITY)
                {
                    var removeCache = CACHE_LIST[0];
                    CACHE_LIST.Remove(removeCache);
                    CACHE_DICTIONARY.Remove(removeCache.FilePath);
                    removeCache.Dispose();
                }

                var bitmap = ImageUtil.ReadImageFile(filePath);
                ImageFileSizeCacher.Set(filePath, bitmap.Size);
                var newCache = new ImageFileCacheEntity(filePath, bitmap, timestamp);
                CACHE_DICTIONARY.Add(filePath, newCache);
                CACHE_LIST.Add(newCache);
                return resultFunc(newCache);
            }
        }
    }
}
