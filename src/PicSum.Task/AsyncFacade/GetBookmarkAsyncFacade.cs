using PicSum.Core.Task.AsyncTask;
using PicSum.Task.AsyncLogic;
using PicSum.Task.Entity;
using System.Runtime.Versioning;

namespace PicSum.Task.AsyncFacade
{
    [SupportedOSPlatform("windows")]
    public sealed class GetBookmarkAsyncFacade
        : TwoWayFacadeBase<ListEntity<FileShallowInfoEntity>>
    {
        public override void Execute()
        {
            var getBookmarkLogic = new GetBookmarkListAsyncLogic(this);
            var dtoList = getBookmarkLogic.Execute();

            var getInfoLogic = new GetFileShallowInfoAsyncLogic(this);
            var infoList = new ListEntity<FileShallowInfoEntity>();
            foreach (var dto in dtoList)
            {
                this.CheckCancel();

                var info = getInfoLogic.Execute(dto.FilePath, dto.RegistrationDate);
                if (info != null)
                {
                    infoList.Add(info);
                }
            }

            this.OnCallback(infoList);
        }
    }
}
