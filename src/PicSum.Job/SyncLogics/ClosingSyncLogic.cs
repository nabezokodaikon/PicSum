using SWF.Core.DatabaseAccessor;
using PicSum.Core.Job.AsyncJob;
using PicSum.DatabaseAccessor.Connection;

namespace PicSum.Job.SyncLogics
{
    /// <summary>
    /// 終了同期ロジック
    /// </summary>
    internal sealed class ClosingSyncLogic
        : AbstractSyncLogic
    {
        public void Execute()
        {
            DatabaseManager<FileInfoConnection>.Close();
            DatabaseManager<ThumbnailConnection>.Close();
        }
    }
}
