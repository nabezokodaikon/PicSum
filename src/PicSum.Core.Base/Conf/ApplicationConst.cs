﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PicSum.Core.Base.Conf
{
    /// <summary>
    /// アプリケーション定数クラス
    /// </summary>
    public static class ApplicationConst
    {
        /// <summary>
        /// SQLファイルの拡張子
        /// </summary>
        public const string SqlFileExtension = ".sql";

        /// <summary>
        /// 番号付SQLパラメータの書式
        /// </summary>
        public const string NumberingSqlParameterFormat = "{0}_{1}";

        // TODO: 削除。
        /// <summary>
        /// 評価最小値
        /// </summary>
        public const int MinimumRatingValue = 1;

        // TODO: 削除。
        /// <summary>
        /// 評価最大値
        /// </summary>
        public const int MaximumRatingValue = 5;
    }

    /// <summary>
    /// ソート種別ID
    /// </summary>
    public enum SortTypeID
    {
        Default = 0,
        FileName = 1,
        FilePath = 2,
        UpdateDate = 3,
        CreateDate = 4
    }

    /// <summary>
    /// コンテンツ表示種別
    /// </summary>
    public enum ContentsOpenType
    {
        Default = 0,
        OverlapTab = 1,
        AddTab = 2,
        InsertTab = 3,
        NewWindow = 4
    }

    /// <summary>
    /// 画像表示モード
    /// </summary>
    public enum ImageDisplayMode
    {
        Single = 0,
        LeftFacing = 1,
        RightFacing = 2
    }

    /// <summary>
    /// 画像サイズモード
    /// </summary>
    public enum ImageSizeMode
    {
        Original = 0,
        FitAllImage = 1,
        FitOnlyBigImage = 2
    }
}
