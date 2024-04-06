using PicSum.Core.Base.Conf;
using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace PicSum.UIComponent.AddressBar
{
    [SupportedOSPlatform("windows")]
    internal sealed class DirectoryDrawItem
        : DrawItemBase, IDisposable
    {
        #region インスタンス変数

        private DirectoryEntity directory = null;

        #endregion

        #region プロパティ

        public DirectoryEntity Directory
        {
            get
            {
                return this.directory;
            }
            set
            {
                this.directory = value;
            }
        }

        #endregion

        #region コンストラクタ

        public DirectoryDrawItem()
        {

        }

        #endregion

        #region メソッド

        public new void Dispose()
        {
            base.Dispose();
        }

        public override void Draw(Graphics g)
        {
            if (g == null)
            {
                throw new ArgumentNullException(nameof(g));
            }

            if (this.directory == null)
            {
                return;
            }

            var rect = this.GetRectangle();

            if (base.IsMouseDown)
            {
                g.FillRectangle(base.Palette.MouseDownBrush, rect);
            }
            else if (base.IsMousePoint)
            {
                g.FillRectangle(base.Palette.MousePointBrush, rect);
                g.DrawRectangle(base.Palette.MousePointPen, rect);
            }

            g.DrawString(this.directory.DirectoryName, base.Palette.TextFont, base.Palette.TextBrush, rect, base.Palette.TextFormat);
        }

        public override void OnMouseDown(MouseEventArgs e)
        {

        }

        public override void OnMouseClick(MouseEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.Button == MouseButtons.Left)
            {
                this.OnSelectedDirectory(new SelectedDirectoryEventArgs(ContentsOpenType.OverlapTab, this.directory.DirectoryPath));
            }
            else if (e.Button == MouseButtons.Middle)
            {
                this.OnSelectedDirectory(new SelectedDirectoryEventArgs(ContentsOpenType.AddTab, this.directory.DirectoryPath));
            }
        }

        #endregion
    }
}
