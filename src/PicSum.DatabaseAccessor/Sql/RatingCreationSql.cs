using SWF.Core.DatabaseAccessor;
using System.Runtime.Versioning;

namespace PicSum.DatabaseAccessor.Sql
{
    /// <summary>
    /// 評価T作成
    /// </summary>
    [SupportedOSPlatform("windows10.0.17763.0")]
    public sealed class RatingCreationSql
        : SqlBase
    {
        const string SQL_TEXT =
@"
INSERT INTO t_rating (
     file_id
    ,rating
    ,registration_date
)
SELECT mf.file_id
      ,:rating
      ,:registration_date
  FROM m_file mf
 WHERE mf.file_path = :file_path
";

        public RatingCreationSql(string filePath, int rating, DateTime registrationDate)
            : base(SQL_TEXT)
        {
            ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

            base.ParameterList.AddRange([
                SqlParameterUtil.CreateParameter("file_path", filePath),
                SqlParameterUtil.CreateParameter("rating", rating),
                SqlParameterUtil.CreateParameter("registration_date", registrationDate)
            ]);
        }
    }
}
