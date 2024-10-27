using PicSum.Job.Jobs;
using PicSum.Job.Parameters;
using SWF.Core.Job;
using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace PicSum.UIComponent.Contents.Common
{
    [SupportedOSPlatform("windows")]
    public sealed partial class CommonJobs
        : IDisposable
    {
        public static CommonJobs Instance = new CommonJobs();

        private bool disposed = false;
        private OneWayJob<ImageFileCacheJob, ListParameter<string>> imageFileCacheJob = null;
        private OneWayJob<BookmarkAddJob, ValueParameter<string>> addBookmarkJob = null;
        private OneWayJob<SingleFileExportJob, SingleFileExportParameter> singleFileExportJob = null;
        private OneWayJob<DirectoryStateUpdateJob, DirectoryStateParameter> directoryStateUpdateJob = null;
        private OneWayJob<DirectoryViewHistoryAddJob, ValueParameter<string>> directoryHistoryaddJob = null;
        private OneWayJob<BookmarkDeleteJob, ListParameter<string>> bookmarkDeleteJob = null;
        private OneWayJob<DirectoryViewCounterDeleteJob, ListParameter<string>> directoryViewCounterDeleteJob = null;
        private OneWayJob<FileRatingUpdateJob, FileRatingUpdateParameter> fileRatingUpdateJob = null;
        private OneWayJob<FileTagDeleteJob, UpdateFileTagParameter> fileTagDeleteJob = null;

        private CommonJobs()
        {

        }

        ~CommonJobs()
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
                this.imageFileCacheJob?.Dispose();
                this.addBookmarkJob?.Dispose();
                this.singleFileExportJob?.Dispose();
                this.directoryStateUpdateJob?.Dispose();
                this.directoryHistoryaddJob?.Dispose();
                this.bookmarkDeleteJob?.Dispose();
                this.directoryViewCounterDeleteJob?.Dispose();
                this.fileRatingUpdateJob?.Dispose();
                this.fileTagDeleteJob?.Dispose();
            }

            this.disposed = true;
        }

        public void StartImageFileCacheJob(Control sender, ListParameter<string> parameter)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));

            if (this.imageFileCacheJob != null)
            {
                this.imageFileCacheJob.Dispose();
            }

            this.imageFileCacheJob = new();
            this.imageFileCacheJob.StartJob(sender, parameter);
        }

        public void StartBookmarkAddJob(Control sender, ValueParameter<string> parameter)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));

            if (this.addBookmarkJob != null)
            {
                this.addBookmarkJob.Dispose();
            }

            this.addBookmarkJob = new();
            this.addBookmarkJob.StartJob(sender, parameter);
        }

        public void StartSingleFileExportJob(Control sender, SingleFileExportParameter parameter)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));

            if (this.singleFileExportJob != null)
            {
                this.singleFileExportJob.Dispose();
            }

            this.singleFileExportJob = new();
            this.singleFileExportJob.StartJob(sender, parameter);
        }

        public void StartDirectoryStateUpdateJob(Control sender, DirectoryStateParameter parameter)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));

            if (this.directoryStateUpdateJob != null)
            {
                this.directoryStateUpdateJob.Dispose();
            }

            this.directoryStateUpdateJob = new();
            this.directoryStateUpdateJob.StartJob(sender, parameter);
        }

        public void StartDirectoryViewHistoryAddJob(Control sender, ValueParameter<string> parameter)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));

            if (this.directoryHistoryaddJob != null)
            {
                this.directoryHistoryaddJob.Dispose();
            }

            this.directoryHistoryaddJob = new();
            this.directoryHistoryaddJob.StartJob(sender, parameter);
        }

        public void StartBookmarkDeleteJob(Control sender, ListParameter<string> parameter)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));

            if (this.bookmarkDeleteJob != null)
            {
                this.bookmarkDeleteJob.Dispose();
            }

            this.bookmarkDeleteJob = new();
            this.bookmarkDeleteJob.StartJob(sender, parameter);
        }

        public void StartDirectoryViewCounterDeleteJob(Control sender, ListParameter<string> parameter)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));

            if (this.directoryViewCounterDeleteJob != null)
            {
                this.directoryViewCounterDeleteJob.Dispose();
            }

            this.directoryViewCounterDeleteJob = new();
            this.directoryViewCounterDeleteJob.StartJob(sender, parameter);
        }

        public void StartFileRatingUpdateJob(Control sender, FileRatingUpdateParameter parameter)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));

            if (this.fileRatingUpdateJob != null)
            {
                this.fileRatingUpdateJob.Dispose();
            }

            this.fileRatingUpdateJob = new();
            this.fileRatingUpdateJob.StartJob(sender, parameter);
        }

        public void StartFileTagDeleteJob(Control sender, UpdateFileTagParameter parameter)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));

            if (this.fileTagDeleteJob != null)
            {
                this.fileTagDeleteJob.Dispose();
            }

            this.fileTagDeleteJob = new();
            this.fileTagDeleteJob.StartJob(sender, parameter);
        }
    }
}
