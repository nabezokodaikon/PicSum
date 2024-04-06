using System;

namespace SWF.Common
{
    /// <summary>
    /// 画像ユーティリティ例外クラス。
    /// </summary>
    public sealed class ImageUtilException
        : SWFException
    {
        public ImageUtilException(string message, Exception exception)
            : base(message, exception)
        {

        }
    }
}
