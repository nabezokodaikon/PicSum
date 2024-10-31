using PicSum.DatabaseAccessor.Connection;
using PicSum.Job.Common;
using PicSum.Job.Logics;
using SWF.Core.DatabaseAccessor;
using SWF.Core.FileAccessor;
using SWF.Core.ImageAccessor;
using SWF.Core.Job;

namespace PicSum.Job.SyncLogics
{
    /// <summary>
    /// 終了同期ロジック
    /// </summary>
    internal sealed class ClosingSyncLogic
        : AbstractSyncLogic
    {
        public async Task Execute()
        {
            await CommonJobs.Instance.DisposeAsync();

            UIThreadAccessor.Instance.Dispose();
            FileIconCash.DisposeStaticResources();
            ThumbnailGetLogic.DisposeStaticResouces();
            ImageFileCacheUtil.DisposeStaticResources();
            FileExportLogic.DisposeStaticResouces();

            DatabaseManager<FileInfoConnection>.Close();
            DatabaseManager<ThumbnailConnection>.Close();
        }
    }
}
