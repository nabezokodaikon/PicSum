using PicSum.UIComponent.Contents.FileList;
using SWF.UIComponent.TabOperation;
using System.Runtime.Versioning;

namespace PicSum.UIComponent.Contents.Parameter
{
    [SupportedOSPlatform("windows")]
    public sealed class BookmarkFileListPageParameter
        : IPageParameter
    {
        public const string PAGE_SOURCES = "Bookmark";

        public string Key { get; private set; }
        public string PageSources { get; private set; }
        public string SourcesKey { get; private set; }
        public string SelectedFilePath { get; set; }

        public BookmarkFileListPageParameter()
        {
            this.PageSources = BookmarkFileListPageParameter.PAGE_SOURCES;
            this.SourcesKey = string.Empty;
            this.Key = string.Format("{0}ListPage", this.PageSources);
            this.SelectedFilePath = string.Empty;
        }

        public PagePanel CreatePage()
        {
            return new BookmarkFileListPage(this);
        }
    }
}
