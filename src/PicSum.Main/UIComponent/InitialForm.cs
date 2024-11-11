using PicSum.Job.Common;
using PicSum.Job.Parameters;
using PicSum.Main.Mng;
using PicSum.UIComponent.Contents.Parameter;
using SWF.Core.Base;
using SWF.Core.FileAccessor;
using SWF.Core.Job;
using SWF.UIComponent.Core;
using System;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows.Forms;

namespace PicSum.Main.UIComponent
{
    /// <summary>
    /// ダミーフォーム
    /// </summary>
    [SupportedOSPlatform("windows10.0.17763.0")]
    internal sealed partial class InitialForm
        : HideForm, ISender
    {
        private bool disposed = false;
        private readonly BrowserManager browserManager = new();

        public InitialForm()
        {
            this.browserManager.BrowserNothing += new(this.BrowserManager_BrowserNothing);
        }

        private void BrowserManager_BrowserNothing(object sender, EventArgs e)
        {
            this.disposed = true;
            this.Close();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            Instance<JobCaller>.Initialize(new JobCaller(SynchronizationContext.Current));

            base.OnHandleCreated(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            Instance<JobCaller>.Value.GCCollectRunJob.Value
                .Reset()
                .BeginCancel()
                .StartJob(this);

            Instance<JobCaller>.Value.PipeServerJob.Value
                .Reset()
                .Callback(_ =>
                {
                    if (this.disposed)
                    {
                        return;
                    }

                    if (!FileUtil.CanAccess(_.Value) || !FileUtil.IsImageFile(_.Value))
                    {
                        return;
                    }

                    var form = this.browserManager.GetActiveBrowser();
                    var directoryPath = FileUtil.GetParentDirectoryPath(_.Value);

                    var sortInfo = new SortInfo();
                    sortInfo.SetSortType(SortTypeID.FilePath, true);

                    var parameter = new ImageViewerPageParameter(
                        DirectoryFileListPageParameter.PAGE_SOURCES,
                        directoryPath,
                        BrowserMainPanel.GetImageFilesAction(new ImageFileGetByDirectoryParameter(_.Value)),
                        _.Value,
                        sortInfo,
                        FileUtil.GetFileName(directoryPath),
                        Instance<IFileIconCacher>.Value.SmallDirectoryIcon,
                        true,
                        true);

                    form.AddImageViewerPageTab(parameter);
                    if (form.WindowState == FormWindowState.Minimized)
                    {
                        form.RestoreWindowState();
                    }
                    form.Activate();
                })
                .BeginCancel()
                .StartJob(this);

            var form = this.browserManager.GetActiveBrowser();
            form.Show();

            base.OnLoad(e);
        }
    }
}
