using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace SWF.UIComponent.TabOperation
{
    /// <summary>
    /// タブのドラッグ操作を制御するクラスです。
    /// </summary>
    [SupportedOSPlatform("windows")]
    internal static class TabDragOperation
    {
        private const int DEFAULT_WIDTH_OFFSET = 8;
        private static readonly List<Form> FORM_LIST = new List<Form>();
        private static TabDragForm tabDragForm = null;
        private static TabInfo tab = null;
        private static Point fromScreenPoint = Point.Empty;
        private static int widthOffset = 0;
        private static int heightOffset = 0;
        private static bool isBegin = false;
        private static bool isMoving = false;

        private static TabDragForm TabDragForm
        {
            get
            {
                if (tabDragForm == null)
                {
                    tabDragForm = new TabDragForm();
                }

                return tabDragForm;
            }
        }

        /// <summary>
        /// フォームをリストに追加します。
        /// </summary>
        /// <param name="form"></param>
        public static void AddForm(Form form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            if (FORM_LIST.Contains(form))
            {
                throw new ArgumentException("既に追加されているフォームをリストに追加しようとしました。", nameof(form));
            }

            FORM_LIST.Insert(0, form);

            AddFormHandler(form);
        }

        /// <summary>
        /// フォームがリスト内に存在するか確認します。
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static bool ContentsForm(Form form)
        {
            return FORM_LIST.Contains(form);
        }

        /// <summary>
        /// 操作中か確認します。
        /// </summary>
        public static bool IsBegin
        {
            get
            {
                return isBegin;
            }
        }

        /// <summary>
        /// タブのドラッグ操作を試みます。
        /// </summary>
        /// <param name="tab"></param>
        public static void BeginTabDragOperation(TabInfo tab)
        {
            if (tab == null)
            {
                throw new ArgumentNullException("tab");
            }

            if (tab.Owner == null)
            {
                throw new ArgumentException("タブはどこにも所有されていません。", "tab");
            }

            if (isBegin)
            {
                throw new SystemException("既にドラッグ操作が開始されています。");
            }

            fromScreenPoint = Cursor.Position;
            var clientPoint = tab.Owner.PointToClient(fromScreenPoint);
            widthOffset = clientPoint.X - tab.DrawArea.X;
            heightOffset = clientPoint.Y - tab.DrawArea.Y;

            TabDragOperation.tab = tab;
            isBegin = true;

            TabDragForm.SetTab(tab);
        }

        /// <summary>
        /// タブのドラッグ操作を終了します。
        /// </summary>
        /// <returns>対象となったタブを返します。</returns>
        public static TabInfo EndTabDragOperation()
        {
            if (!isBegin)
            {
                throw new SystemException("ドラッグ操作が開始されていません。");
            }

            var targetTab = tab;

            isBegin = false;
            isMoving = false;
            tab = null;

            TabDragForm.Hide();
            TabDragForm.Clear();

            return targetTab;
        }

        /// <summary>
        /// タブを移動します。
        /// </summary>
        public static void MoveTab()
        {
            if (!isBegin)
            {
                throw new SystemException("ドラッグ操作が開始されていません。");
            }

            var toScreenPoint = Cursor.Position;
            var moveWidth = toScreenPoint.X - fromScreenPoint.X;
            var moveHeight = toScreenPoint.Y - fromScreenPoint.Y;

            if (!isMoving)
            {
                if ((SystemInformation.DragSize.Width < Math.Abs(moveWidth) ||
                     SystemInformation.DragSize.Height < Math.Abs(moveHeight)))
                {
                    isMoving = true;
                }
                else
                {
                    return;
                }
            }

            TabDragForm.SetLocation(widthOffset, heightOffset);

            if (tab.Owner != null)
            {
                // タブが所有されている場合。
                if (tab.Owner.GetTabsScreenRectangle().Contains(toScreenPoint))
                {
                    var clientPoint = tab.Owner.PointToClient(toScreenPoint);

                    int toX = clientPoint.X;
                    if (tab.DrawArea.Width > widthOffset)
                    {
                        toX = clientPoint.X - widthOffset;
                    }
                    else
                    {
                        toX = clientPoint.X - DEFAULT_WIDTH_OFFSET;
                    }

                    var rect = tab.Owner.GetTabsClientRectangle();
                    if (toX < rect.X)
                    {
                        tab.DrawArea.X = rect.X;
                    }
                    else if (toX + tab.DrawArea.Width > rect.Right)
                    {
                        tab.DrawArea.Right = rect.Right;
                    }
                    else
                    {
                        tab.DrawArea.X = toX;
                    }

                    tab.Owner.InvalidateHeader();
                    return;
                }
                else
                {
                    tab.Owner.RemoveTab(tab);
                    TabDragForm.Show();
                    return;
                }
            }
            else
            {
                // タブが所有されていない場合。
                foreach (var form in FORM_LIST)
                {
                    if (form.Bounds.Contains(toScreenPoint))
                    {
                        TabSwitch owner = GetTabSwitchControl(form);
                        if (owner.GetTabsScreenRectangle().Contains(toScreenPoint))
                        {
                            form.Activate();
                            var clientPoint = owner.PointToClient(toScreenPoint);
                            tab.DrawArea.X = clientPoint.X;
                            owner.AddTab(tab);
                            TabDragForm.Hide();
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 対象のタブであるか確認します。
        /// </summary>
        /// <param name="tab">比較するタブ</param>
        /// <returns></returns>
        public static bool IsTarget(TabInfo tab)
        {
            if (tab == null)
            {
                throw new ArgumentNullException(nameof(tab));
            }

            return tab.Equals(TabDragOperation.tab);
        }

        private static TabSwitch GetTabSwitchControl(Form form)
        {
            var tabSwitch = GetTabSwitchControl(form.Controls);
            if (tabSwitch != null)
            {
                return tabSwitch;
            }
            else
            {
                throw new NullReferenceException("フォーム内にタブスイッチコントロールが存在しません。");
            }
        }

        private static TabSwitch GetTabSwitchControl(Control.ControlCollection controls)
        {
            foreach (Control child in controls)
            {
                if (child is TabSwitch)
                {
                    return (TabSwitch)child;
                }
                else if (child.Controls.Count > 0)
                {
                    var result = GetTabSwitchControl(child.Controls);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        private static void AddFormHandler(Form form)
        {
            form.Activated += new EventHandler(Form_Activated);
            form.FormClosing += new FormClosingEventHandler(Form_FormClosing);
        }

        private static void RemoveFormHandler(Form form)
        {
            form.Activated -= new EventHandler(Form_Activated);
            form.FormClosing -= new FormClosingEventHandler(Form_FormClosing);
        }

        #region フォームイベント

        private static void Form_Activated(object sender, EventArgs e)
        {
            if (sender != null && sender is Form)
            {
                var form = (Form)sender;
                FORM_LIST.Remove(form);
                FORM_LIST.Insert(0, form);
            }
        }

        private static void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender != null && sender is Form)
            {
                var form = (Form)sender;
                FORM_LIST.Remove(form);
                RemoveFormHandler(form);
            }
        }

        #endregion
    }
}
