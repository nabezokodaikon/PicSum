﻿using System;
using PicSum.Core.Data.DatabaseAccessor;
using PicSum.Core.Task.AsyncTask;
using PicSum.Data.DatabaseAccessor.Connection;
using PicSum.Task.Entity;

namespace PicSum.Task.AsyncLogic
{
    /// <summary>
    /// スタートアップ非同期ロジック
    /// </summary>
    internal class StartupAsyncLogic : AsyncLogicBase
    {
        public StartupAsyncLogic(AsyncFacadeBase facade) : base(facade) { }

        public void Execute(StartupPrameterEntity param)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param");
            }

            SqlManager.SqlDirectory = param.SqlDirectoryPath;
            DatabaseManager<FileInfoConnection>.Connect(new FileInfoConnection(param.FileInfoDBFilePath));
            DatabaseManager<ThumbnailConnection>.Connect(new ThumbnailConnection(param.ThumbnailDBFilePath));
        }
    }
}
