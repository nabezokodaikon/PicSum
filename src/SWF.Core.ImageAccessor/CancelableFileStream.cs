namespace SWF.Core.ImageAccessor
{
    internal sealed partial class CancelableFileStream
        : FileStream
    {
        private static readonly int FILE_READ_BUFFER_SIZE = 80 * 1024;

        private readonly Action cancelCheckAction;

        public CancelableFileStream(string path, Action cancelCheckAction)
            : base(path, FileMode.Open, FileAccess.Read, FileShare.Read, FILE_READ_BUFFER_SIZE)
        {
            ArgumentNullException.ThrowIfNull(cancelCheckAction, nameof(cancelCheckAction));

            this.cancelCheckAction = cancelCheckAction;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                this.cancelCheckAction();
            }
            catch
            {
                Console.WriteLine($"------------------------------------------------");
                return 0;
            }

            return base.Read(buffer, offset, count);
        }
    }
}
