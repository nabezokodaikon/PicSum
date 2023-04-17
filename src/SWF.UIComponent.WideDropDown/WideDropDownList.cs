﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SWF.UIComponent.WideDropDown
{
    public sealed class WideDropDownList
        : ToolStripDropDown
    {
        #region イベント・デリゲート

        public event EventHandler<ItemMouseClickEventArgs> ItemMouseClick;

        #endregion

        private readonly SolidBrush selectedTextBrush = new SolidBrush(Color.White);
        private readonly List<string> itemList = new List<string>();

        #region パブリックプロパティ

        public Image Icon { get; set; }

        /// <summary>
        /// 項目テキスト色
        /// </summary>
        [Category("項目描画")]
        public Color ItemTextColor
        {
            get
            {
                return this.flowList.ItemTextColor;
            }
            set
            {
                this.flowList.ItemTextColor = value;
            }
        }

        /// <summary>
        /// 項目選択色
        /// </summary>
        [Category("項目描画")]
        public Color SelectedItemColor
        {
            get
            {
                return this.flowList.SelectedItemColor;
            }
            set
            {
                this.flowList.SelectedItemColor = value;
            }
        }

        /// <summary>
        /// 項目マウスポイント色
        /// </summary>
        [Category("項目描画")]
        public Color MousePointItemColor
        {
            get
            {
                return this.flowList.MousePointItemColor;
            }
            set
            {
                this.flowList.MousePointItemColor = value;
            }
        }

        /// <summary>
        /// 項目テキストフォーマット
        /// </summary>
        [Category("項目描画")]
        public StringTrimming ItemTextTrimming
        {
            get
            {
                return this.flowList.ItemTextTrimming;
            }
            set
            {
                this.flowList.ItemTextTrimming = value;
            }
        }

        /// <summary>
        /// 項目テキストフォーマット
        /// </summary>
        [Category("項目描画")]
        public StringAlignment ItemTextAlignment
        {
            get
            {
                return this.flowList.ItemTextAlignment;
            }
            set
            {
                this.flowList.ItemTextAlignment = value;
            }
        }

        /// <summary>
        /// 項目テキストフォーマット
        /// </summary>
        [Category("項目描画")]
        public StringAlignment ItemTextLineAlignment
        {
            get
            {
                return this.flowList.ItemTextLineAlignment;
            }
            set
            {
                this.flowList.ItemTextLineAlignment = value;
            }
        }

        /// <summary>
        /// 項目テキストフォーマット
        /// </summary>
        [Category("項目描画")]
        public StringFormatFlags ItemTextFormatFlags
        {
            get
            {
                return this.flowList.ItemTextFormatFlags;
            }
            set
            {
                this.flowList.ItemTextFormatFlags = value;
            }
        }

        [Browsable(false)]
        public StringFormat ItemTextFormat
        {
            get
            {
                return this.flowList.ItemTextFormat;
            }
        }

        /// <summary>
        /// 背景色
        /// </summary>
        public new Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                this.toolStripItem.BackColor = value;
                this.flowList.BackColor = value;
            }
        }

        /// <summary>
        /// サイズ
        /// </summary>
        public new Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
                this.toolStripItem.Size = value;
            }
        }

        /// <summary>
        /// スクロールバーの幅
        /// </summary>
        public int ScrollBarWidth
        {
            get
            {
                return this.flowList.ScrollBarWidth;
            }
        }

        #endregion

        #region プライベートプロパティ

        private ToolStripControlHost toolStripItem
        {
            get
            {
                return (ToolStripControlHost)this.Items[0];
            }
        }

        private FlowList.FlowList flowList
        {
            get
            {
                return (FlowList.FlowList)((ToolStripControlHost)this.Items[0]).Control;
            }
        }

        private int itemTextHeight
        {
            get
            {
                return this.FontHeight * 2;
            }
        }

        #endregion

        #region コンストラクタ

        public WideDropDownList()
        {
            this.SetStyle(ControlStyles.Selectable, false);

            this.Items.Add(new ToolStripControlHost(new FlowList.FlowList()));
            this.Padding = new Padding(2, 1, 2, 0);

            this.toolStripItem.AutoSize = false;
            this.toolStripItem.BackColor = this.BackColor;

            this.flowList.BackColor = Color.White;
            this.flowList.SelectedItemColor = Color.FromArgb(192, 48, 96, 144);
            this.flowList.MousePointItemColor = Color.FromArgb(24, 48, 96, 144);
            this.flowList.ItemTextColor = Color.FromArgb(64, 64, 64);
            this.flowList.ItemTextTrimming = StringTrimming.EllipsisCharacter;
            this.flowList.ItemTextLineAlignment = StringAlignment.Center;
            this.flowList.ItemTextAlignment = StringAlignment.Near;
            this.flowList.ItemTextFormatFlags = StringFormatFlags.NoWrap;
            this.flowList.Dock = DockStyle.Fill;
            this.flowList.IsLileList = false;
            this.flowList.ItemSpace = 0;
            this.flowList.IsMultiSelect = false;

            this.flowList.ItemMouseClick += new EventHandler<MouseEventArgs>(this.flowList_ItemMouseClick);
            this.flowList.DrawItem += new EventHandler<SWF.UIComponent.FlowList.DrawItemEventArgs>(this.flowList_DrawItem);
        }

        #endregion

        #region パブリックメソッド

        public void SetItemSize(int width, int height)
        {
            this.flowList.SetItemSize(width, height);
        }

        public void SetItems(IList<string> itemList)
        {
            if (itemList == null)
            {
                throw new ArgumentNullException(nameof(itemList));
            }

            this.flowList.BeginUpdate();

            try
            {
                this.itemList.Clear();
                this.itemList.AddRange(itemList);

                this.flowList.ClearSelectedItems();
                this.flowList.ItemCount = this.itemList.Count;
            }
            finally
            {
                this.flowList.EndUpdate();
            }
        }

        public void AddItems(IList<string> itemList)
        {
            if (itemList == null)
            {
                throw new ArgumentNullException(nameof(itemList));
            }

            this.flowList.BeginUpdate();

            try
            {
                this.itemList.AddRange(itemList);
                var tempItemList = this.itemList
                    .GroupBy(item => item)
                    .Select(item => item.First())
                    .OrderBy(item => item)
                    .ToList();
                this.itemList.Clear();
                this.itemList.AddRange(tempItemList);
                this.flowList.ItemCount = this.itemList.Count;
            }
            finally
            {
                this.flowList.EndUpdate();
            }
        }

        public void SelectItem(string item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var index = this.itemList.IndexOf(item);
            if (index < 0)
            {
                return;
            }

            this.flowList.BeginUpdate();

            try
            {
                this.flowList.SelectItem(index);
            }
            finally
            {
                this.flowList.EndUpdate();
            }
        }

        #endregion

        #region 継承メソッド

        protected override void OnOpened(EventArgs e)
        {
            this.flowList.Focus();
            base.OnOpened(e);
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            this.flowList.Invalidate();
            base.OnInvalidated(e);
        }

        #endregion

        #region プライベートメソッド

        private SolidBrush getTextBrush(SWF.UIComponent.FlowList.DrawItemEventArgs e)
        {
            if (e.IsSelected)
            {
                return selectedTextBrush;
            }
            else
            {
                return flowList.ItemTextBrush;
            }
        }

        private Rectangle getTextRectangle(SWF.UIComponent.FlowList.DrawItemEventArgs e)
        {
            return new Rectangle(e.ItemRectangle.X,
                                 e.ItemRectangle.Bottom - itemTextHeight,
                                 e.ItemRectangle.Width,
                                 itemTextHeight);
        }

        private void flowList_DrawItem(object sender, UIComponent.FlowList.DrawItemEventArgs e)
        {
            if (this.itemList.Count < 1)
            {
                return;
            }

            if (e.IsSelected)
            {
                e.Graphics.FillRectangle(this.flowList.SelectedItemBrush, e.ItemRectangle);
            }

            if (e.IsFocus)
            {
                e.Graphics.FillRectangle(this.flowList.FocusItemBrush, e.ItemRectangle);
            }

            if (e.IsMousePoint)
            {
                e.Graphics.FillRectangle(this.flowList.MousePointItemBrush, e.ItemRectangle);
            }

            var item = this.itemList[e.ItemIndex];
            e.Graphics.DrawString(item, this.Font, getTextBrush(e), getTextRectangle(e), this.flowList.ItemTextFormat);
        }

        private void flowList_ItemMouseClick(object sender, MouseEventArgs e)
        {
            this.Close();

            if (this.itemList.Count < 1)
            {
                return;
            }

            var indexs = this.flowList.GetSelectedIndexs();
            if (indexs.Count < 1)
            {
                return;
            }

            var index = indexs.First();
            if (this.itemList.Count - 1 < index)
            {
                return;
            }

            if (this.ItemMouseClick != null)
            {
                var item = this.itemList[indexs.First()];
                var args = new ItemMouseClickEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta, item);
                this.ItemMouseClick(this, args);
            }
        }

        #endregion
    }
}
