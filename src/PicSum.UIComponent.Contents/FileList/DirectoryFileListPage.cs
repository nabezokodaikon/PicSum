using PicSum.Job.Entities;
using PicSum.Job.Jobs;
using PicSum.Job.Parameters;
using PicSum.Job.Results;
using PicSum.UIComponent.Contents.Common;
using PicSum.UIComponent.Contents.Parameter;
using SWF.Core.Base;
using SWF.Core.FileAccessor;
using SWF.Core.ImageAccessor;
using SWF.Core.Job;
using SWF.UIComponent.TabOperation;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace PicSum.UIComponent.Contents.FileList
{
    /// <summary>
    /// フォルダファイルリストコンテンツ
    /// </summary>
    [SupportedOSPlatform("windows")]
    internal sealed partial class DirectoryFileListPage
        : AbstractFileListPage
    {
        private static Action<Control> ImageFilesGetAction(ImageViewerPageParameter param)
        {
            return sender =>
            {
                using (var job = new TwoWayJob<FilesGetByDirectoryJob, ValueParameter<string>, DirectoryGetResult>())
                {
                    job
                    .Callback(e =>
                    {
                        var imageFiles = e.FileInfoList
                            .Where(fileInfo => fileInfo.IsImageFile);
                        var sortImageFiles = GetSortFiles(imageFiles, param.SortInfo)
                            .Select(fileInfo => fileInfo.FilePath)
                            .ToArray();

                        if (!FileUtil.IsImageFile(param.SelectedFilePath))
                        {
                            throw new SWFException($"画像ファイルが選択されていません。'{param.SelectedFilePath}'");
                        }

                        var eventArgs = new GetImageFilesEventArgs(
                            sortImageFiles, param.SelectedFilePath, param.PageTitle, param.PageIcon);
                        param.OnGetImageFiles(eventArgs);
                    });

                    job.StartJob(sender, new ValueParameter<string>(param.SourcesKey));
                    job.WaitJobComplete();
                }
            };
        }

        private bool disposed = false;
        private readonly DirectoryFileListPageParameter parameter = null;

        public DirectoryFileListPage(DirectoryFileListPageParameter param)
            : base(param)
        {
            this.parameter = param;

            this.Title = FileUtil.GetFileName(this.parameter.DirectoryPath);

            if (FileUtil.IsSystemRoot(this.parameter.DirectoryPath))
            {
                this.Icon = FileIconCash.SmallMyComputerIcon;
            }
            else if (FileUtil.IsDrive(this.parameter.DirectoryPath))
            {
                this.Icon = FileIconCash.GetSmallDriveIcon(this.parameter.DirectoryPath);
            }
            else
            {
                this.Icon = FileIconCash.SmallDirectoryIcon;
            }

            this.IsRemoveFromListMenuItemVisible = false;
            this.IsMoveControlVisible = !string.IsNullOrEmpty(this.parameter.DirectoryPath);
            base.sortFileRgistrationDateToolStripButton.Enabled = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            using (var job = new TwoWayJob<FilesGetByDirectoryJob, ValueParameter<string>, DirectoryGetResult>())
            {
                var param = new ValueParameter<string>(this.parameter.DirectoryPath);

                job.Callback(_ =>
                {
                    if (this.disposed)
                    {
                        return;
                    }

                    this.SearchJob_Callback(_);
                });

                job.StartJob(this, param);
                job.WaitJobComplete();
            }

            base.OnLoad(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.SaveCurrentDirectoryState();
            }

            this.disposed = true;

            base.Dispose(disposing);
        }

        protected override void OnDrawTabPage(DrawTabEventArgs e)
        {
            e.Graphics.DrawImage(this.Icon, e.IconRectangle);
            DrawTextUtil.DrawText(
                e.Graphics, this.Title, e.Font,
                new Rectangle(
                    (int)e.TextRectangle.X,
                    (int)e.TextRectangle.Y,
                    (int)e.TextRectangle.Width,
                    (int)e.TextRectangle.Height),
                e.TitleColor, e.TitleFormatFlags);
        }

        protected override void OnBackgroundMouseClick(MouseEventArgs e)
        {
            if (!FileUtil.IsSystemRoot(this.parameter.DirectoryPath))
            {
                base.OnSelectedFileChanged(new SelectedFileChangeEventArgs(this.parameter.DirectoryPath));
            }
        }

        protected override void OnRemoveFile(System.Collections.Generic.IList<string> filePathList)
        {
            // 処理無し。
        }

        protected override void OnMovePreviewButtonClick(EventArgs e)
        {
            if (FileUtil.IsSystemRoot(this.parameter.DirectoryPath))
            {
                return;
            }

            using (var job = new TwoWayJob<NextDirectoryGetJob, NextDirectoryGetParameter<string>, ValueResult<string>>())
            {
                var param = new NextDirectoryGetParameter<string>
                {
                    CurrentParameter = new ValueEntity<string>(this.parameter.DirectoryPath),
                    IsNext = false,
                };

                job.Callback(_ =>
                {
                    if (this.disposed)
                    {
                        return;
                    }

                    this.GetNextDirectoryProcess_Callback(_);
                });

                job.StartJob(this, param);
                job.WaitJobComplete();
            }
        }

        protected override void OnMoveNextButtonClick(EventArgs e)
        {
            if (FileUtil.IsSystemRoot(this.parameter.DirectoryPath))
            {
                return;
            }

            using (var job = new TwoWayJob<NextDirectoryGetJob, NextDirectoryGetParameter<string>, ValueResult<string>>())
            {
                var param = new NextDirectoryGetParameter<string>
                {
                    IsNext = true,
                    CurrentParameter = new ValueEntity<string>(this.parameter.DirectoryPath)
                };

                job.Callback(_ =>
                {
                    if (this.disposed)
                    {
                        return;
                    }

                    this.GetNextDirectoryProcess_Callback(_);
                });

                job.StartJob(this, param);
                job.WaitJobComplete();
            }
        }

        protected override Action<Control> GetImageFilesGetAction(ImageViewerPageParameter param)
        {
            return ImageFilesGetAction(param);
        }

        protected override void FileContextMenu_Opening(object sender, CancelEventArgs e)
        {
            var filePathList = this.GetSelectedFiles();
            if (filePathList.Count > 0)
            {
                this.IsDirectoryActiveTabOpenMenuItemVisible = true;
                this.SetContextMenuFiles(filePathList);
            }
            else if (!FileUtil.IsSystemRoot(this.parameter.DirectoryPath))
            {
                this.IsDirectoryActiveTabOpenMenuItemVisible = false;
                this.SetContextMenuFiles(this.parameter.DirectoryPath);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void SaveCurrentDirectoryState()
        {
            var param = new DirectoryStateParameter
            {
                DirectoryPath = this.parameter.DirectoryPath
            };

            if (base.SortTypeID == SortTypeID.Default)
            {
                param.SortTypeID = SortTypeID.FileName;
                param.IsAscending = true;
            }
            else
            {
                param.SortTypeID = base.SortTypeID;
                param.IsAscending = base.IsAscending;
            }

            param.SelectedFilePath = base.SelectedFilePath;

            CommonJobs.Instance.StartDirectoryStateUpdateJob(this, param);
        }

        private void SearchJob_Callback(DirectoryGetResult e)
        {
            if (!string.IsNullOrEmpty(this.parameter.SelectedFilePath))
            {
                if (e.DirectoryState.Equals(DirectoryStateParameter.EMPTY))
                {
                    base.SetFiles(e.FileInfoList, this.parameter.SelectedFilePath, SortTypeID.FilePath, true);
                }
                else
                {
                    base.SetFiles(e.FileInfoList, this.parameter.SelectedFilePath, e.DirectoryState.SortTypeID, e.DirectoryState.IsAscending);
                }
            }
            else
            {
                if (e.DirectoryState.Equals(DirectoryStateParameter.EMPTY))
                {
                    base.SetFiles(e.FileInfoList, string.Empty, SortTypeID.FilePath, true);

                }
                else
                {
                    base.SetFiles(e.FileInfoList, e.DirectoryState.SelectedFilePath, e.DirectoryState.SortTypeID, e.DirectoryState.IsAscending);
                    if (e.FileInfoList.Count < 1)
                    {
                        base.OnSelectedFileChanged(new SelectedFileChangeEventArgs(e.DirectoryState.DirectoryPath));
                    }
                }
            }

            var param = new ValueParameter<string>(e.DirectoryPath);
            CommonJobs.Instance.StartDirectoryViewHistoryAddJob(this, param);
        }

        private void GetNextDirectoryProcess_Callback(ValueResult<string> e)
        {
            var param = new DirectoryFileListPageParameter(e.Value);
            this.OnOpenPage(new BrowserPageEventArgs(PageOpenType.OverlapTab, param));
        }

    }
}
