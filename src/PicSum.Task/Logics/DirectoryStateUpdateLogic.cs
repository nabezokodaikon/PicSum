using PicSum.Core.Data.DatabaseAccessor;
using PicSum.Core.Task.AsyncTaskV2;
using PicSum.Data.DatabaseAccessor.Connection;
using PicSum.Data.DatabaseAccessor.Sql;
using PicSum.Task.Parameters;
using SWF.Common;
using System;
using System.Runtime.Versioning;

namespace PicSum.Task.Logics
{
    /// <summary>
    /// フォルダ状態テーブルを更新します。
    /// </summary>
    [SupportedOSPlatform("windows")]
    internal sealed class DirectoryStateUpdateLogic(IAsyncTask task)
        : AbstractAsyncLogic(task)
    {
        public bool Execute(DirectoryStateParameter directoryState)
        {
            ArgumentNullException.ThrowIfNull(directoryState, nameof(directoryState));

            if (directoryState.DirectoryPath == null)
            {
                throw new ArgumentException("フォルダパスがNULLです。", nameof(directoryState));
            }

            if (directoryState.SelectedFilePath == null)
            {
                throw new ArgumentException("選択ファイルパスがNULLです。", nameof(directoryState));
            }

            if (directoryState.SortTypeID == SortTypeID.Default)
            {
                throw new ArgumentException("ソートIDがデフォルトです。", nameof(directoryState));
            }

            DirectoryStateUpdateSql sql;
            if (string.IsNullOrEmpty(directoryState.SelectedFilePath))
            {
                sql = new DirectoryStateUpdateSql(directoryState.DirectoryPath, (int)directoryState.SortTypeID, directoryState.IsAscending);
            }
            else
            {
                sql = new DirectoryStateUpdateSql(directoryState.DirectoryPath, (int)directoryState.SortTypeID, directoryState.IsAscending, directoryState.SelectedFilePath);
            }

            return DatabaseManager<FileInfoConnection>.Update(sql);
        }
    }
}
