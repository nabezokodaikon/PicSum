using PicSum.Job.Entities;

namespace PicSum.Job.Common
{
    public sealed partial class ClipFiles
        : IClipFiles
    {
        private bool disposed = false;
        private readonly Lock LOCK = new();
        private readonly Dictionary<string, ClipFileEntity> dic = [];

        public ClipFiles()
        {

        }

        ~ClipFiles()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {

            }

            this.disposed = true;
        }

        public void AddFiles(string[] files)
        {
            ArgumentNullException.ThrowIfNull(files, nameof(files));

            lock (this.LOCK)
            {
                foreach (var file in files)
                {
                    this.dic.Remove(file);
                    this.dic.Add(file, new ClipFileEntity(file));
                }
            }
        }

        public void RemoveFiles(string[] files)
        {
            ArgumentNullException.ThrowIfNull(files, nameof(files));

            lock (this.LOCK)
            {
                foreach (var file in files)
                {
                    this.dic.Remove(file);
                }
            }
        }

        public ClipFileEntity[] GetFiles()
        {
            lock (this.LOCK)
            {
                return [.. this.dic.Values];
            }
        }
    }
}
