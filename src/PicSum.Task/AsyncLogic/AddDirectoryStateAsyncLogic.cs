using PicSum.Core.Data.DatabaseAccessor;
using PicSum.Core.Task.AsyncTaskV2;
using PicSum.Data.DatabaseAccessor.Connection;
using PicSum.Data.DatabaseAccessor.Sql;
using PicSum.Task.Parameter;
using System;

namespace PicSum.Task.AsyncLogic
{
    /// <summary>
    /// フォルダ状態テーブルに登録します。
    /// </summary>
    internal sealed class AddDirectoryStateAsyncLogic
        : AbstractAsyncLogic
    {
        public AddDirectoryStateAsyncLogic(IAsyncTask task)
            : base(task)
        {

        }

        public bool Execute(DirectoryStateParameter directoryState)
        {
            if (directoryState == null)
            {
                throw new ArgumentNullException(nameof(directoryState));
            }

            if (directoryState.DirectoryPath == null)
            {
                throw new ArgumentException("フォルダパスがNULLです。", nameof(directoryState));
            }

            if (directoryState.SelectedFilePath == null)
            {
                throw new ArgumentException("選択ファイルパスがNULLです。", nameof(directoryState));
            }

            CreationDirectoryStateSql sql;
            if (string.IsNullOrEmpty(directoryState.SelectedFilePath))
            {
                sql = new CreationDirectoryStateSql(directoryState.DirectoryPath, (int)directoryState.SortTypeID, directoryState.IsAscending);
            }
            else
            {
                sql = new CreationDirectoryStateSql(directoryState.DirectoryPath, (int)directoryState.SortTypeID, directoryState.IsAscending, directoryState.SelectedFilePath);
            }

            return DatabaseManager<FileInfoConnection>.Update(sql);
        }
    }
}
