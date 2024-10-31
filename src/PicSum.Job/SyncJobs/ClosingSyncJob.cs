using PicSum.Job.SyncLogics;
using SWF.Core.Job;

namespace PicSum.Job.SyncJobs
{
    /// <summary>
    /// 終了同期ジョブ
    /// </summary>
    public sealed class ClosingSyncJob
        : AbstractSyncJob
    {
        public async Task Execute()
        {
            var logic = new ClosingSyncLogic();
            await logic.Execute();
        }
    }
}
