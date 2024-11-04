using PicSum.Job.Common;
using PicSum.Job.Entities;
using SWF.Core.FileAccessor;
using SWF.Core.ImageAccessor;
using SWF.Core.Job;
using System.Runtime.Versioning;

namespace PicSum.Job.Logics
{
    /// <summary>
    /// ファイルの浅い情報を取得します。
    /// </summary>
    [SupportedOSPlatform("windows")]
    internal sealed class FileShallowInfoGetLogic(AbstractAsyncJob job)
        : AbstractAsyncLogic(job)
    {
        private const int THUMBNAIL_SIZE = 248;

        public FileShallowInfoEntity Execute(string filePath, bool isGetThumbnail)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

            var info = new FileShallowInfoEntity
            {
                RgistrationDate = null
            };

            if (FileUtil.IsSystemRoot(filePath))
            {
                info.FilePath = filePath;
                info.FileName = FileUtil.GetFileName(filePath);
                info.IsFile = false;
                info.IsImageFile = false;
                info.UpdateDate = null;
                info.SmallIcon = FileIconCacher.Instance.SmallPCIcon;
                info.ExtraLargeIcon = FileIconCacher.Instance.LargePCIcon;
                info.JumboIcon = FileIconCacher.Instance.LargePCIcon;
            }
            else if (FileUtil.IsDrive(filePath))
            {
                info.FilePath = filePath;
                info.FileName = FileUtil.GetFileName(filePath);
                info.IsFile = false;
                info.IsImageFile = false;
                info.UpdateDate = FileUtil.GetUpdateDate(filePath);
                info.SmallIcon = FileIconCacher.Instance.GetSmallDriveIcon(info.FilePath);
                info.ExtraLargeIcon = FileIconCacher.Instance.GetExtraLargeDriveIcon(info.FilePath);
                info.JumboIcon = FileIconCacher.Instance.GetJumboDriveIcon(info.FilePath);
            }
            else if (FileUtil.IsFile(filePath))
            {
                info.FilePath = filePath;
                info.FileName = FileUtil.GetFileName(filePath);
                info.IsFile = true;
                info.IsImageFile = FileUtil.IsImageFile(filePath);
                info.UpdateDate = FileUtil.GetUpdateDate(filePath);
                info.SmallIcon = FileIconCacher.Instance.GetSmallFileIcon(info.FilePath);
                info.ExtraLargeIcon = FileIconCacher.Instance.GetExtraLargeFileIcon(info.FilePath);
                info.JumboIcon = FileIconCacher.Instance.GetJumboFileIcon(info.FilePath);

                if (isGetThumbnail)
                {
                    var thumbnailBuffer = ThumbnailCacher.Instance.GetOnlyCache(filePath, THUMBNAIL_SIZE, THUMBNAIL_SIZE);
                    if (thumbnailBuffer != ThumbnailCacheEntity.EMPTY)
                    {
                        if (thumbnailBuffer.ThumbnailBuffer == null)
                        {
                            throw new NullReferenceException("サムネイルのバッファがNullです。");
                        }

                        info.ThumbnailImage = ThumbnailUtil.ToImage(thumbnailBuffer.ThumbnailBuffer);
                        info.ThumbnailWidth = THUMBNAIL_SIZE;
                        info.ThumbnailHeight = THUMBNAIL_SIZE;
                        info.SourceWidth = thumbnailBuffer.SourceWidth;
                        info.SourceHeight = thumbnailBuffer.SourceHeight;
                    }
                }
            }
            else if (FileUtil.IsDirectory(filePath))
            {
                info.FilePath = filePath;
                info.FileName = FileUtil.GetFileName(filePath);
                info.IsFile = false;
                info.IsImageFile = false;
                info.UpdateDate = FileUtil.GetUpdateDate(filePath);
                info.SmallIcon = FileIconCacher.Instance.SmallDirectoryIcon;
                info.ExtraLargeIcon = FileIconCacher.Instance.ExtraLargeDirectoryIcon;
                info.JumboIcon = FileIconCacher.Instance.JumboDirectoryIcon;

                if (isGetThumbnail)
                {
                    var thumbnailBuffer = ThumbnailCacher.Instance.GetOnlyCache(filePath, THUMBNAIL_SIZE, THUMBNAIL_SIZE);
                    if (thumbnailBuffer != ThumbnailCacheEntity.EMPTY)
                    {
                        if (thumbnailBuffer.ThumbnailBuffer == null)
                        {
                            throw new NullReferenceException("サムネイルのバッファがNullです。");
                        }

                        info.ThumbnailImage = ThumbnailUtil.ToImage(thumbnailBuffer.ThumbnailBuffer);
                        info.ThumbnailWidth = THUMBNAIL_SIZE;
                        info.ThumbnailHeight = THUMBNAIL_SIZE;
                        info.SourceWidth = thumbnailBuffer.SourceWidth;
                        info.SourceHeight = thumbnailBuffer.SourceHeight;
                    }
                }
            }
            else
            {
                throw new ArgumentException($"不正なファイルパスです。'{filePath}'", nameof(filePath));
            }

            return info;
        }

        public FileShallowInfoEntity Execute(string filePath, DateTime registrationDate)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

            var info = this.Execute(filePath, true);
            info.RgistrationDate = registrationDate;

            return info;
        }
    }
}
