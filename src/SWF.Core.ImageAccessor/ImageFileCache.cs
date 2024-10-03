using System.Diagnostics;
using System.Runtime.Versioning;

namespace SWF.Core.ImageAccessor
{
    [SupportedOSPlatform("windows")]
    internal sealed class ImageFileCache
        : IDisposable, IEquatable<ImageFileCache>
    {
        private bool disposed = false;

        public string FilePath { get; private set; }
        public CvImage Image { get; private set; }
        public DateTime Timestamp { get; private set; }

        public ImageFileCache(string filePath, CvImage image, DateTime timestamp)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
            ArgumentNullException.ThrowIfNull(image, nameof(image));

            this.FilePath = filePath;
            this.Image = image;
            this.Timestamp = timestamp;
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.Image?.Dispose();
                this.Image = null;

                var sw = Stopwatch.StartNew();
                GC.Collect();
                sw.Stop();
                Console.WriteLine($"[{Thread.CurrentThread.Name}] GC.Collect: {sw.ElapsedMilliseconds} ms");
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ImageFileCache()
        {
            this.Dispose(false);
        }

        public ImageFileCache ShallowCopy()
        {
            if (this.FilePath == null)
            {
                throw new NullReferenceException("ファイルパスが設定されていません。");
            }

            if (this.Image == null)
            {
                throw new NullReferenceException("イメージが設定されていません。");
            }

            return new ImageFileCache(this.FilePath, this.Image.ShallowCopy(), this.Timestamp);
        }

        public ImageFileCache DeepCopy()
        {
            if (this.FilePath == null)
            {
                throw new NullReferenceException("ファイルパスが設定されていません。");
            }

            if (this.Image == null)
            {
                throw new NullReferenceException("イメージが設定されていません。");
            }

            return new ImageFileCache(this.FilePath, this.Image.DeepCopy(), this.Timestamp);
        }

        public bool Equals(ImageFileCache? other)
        {
            if (other == null)
            {
                return false;
            }

            if (other.FilePath != this.FilePath)
            {
                return false;
            }

            if (other.Timestamp != this.Timestamp)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return (this.FilePath, this.Timestamp).GetHashCode();
        }
    }
}
