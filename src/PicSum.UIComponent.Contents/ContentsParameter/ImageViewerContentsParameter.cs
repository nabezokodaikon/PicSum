﻿using System;
using System.Collections.Generic;
using System.Drawing;
using SWF.UIComponent.TabOperation;

namespace PicSum.UIComponent.Contents.ContentsParameter
{
    /// <summary>
    /// 画像表示コンテンツパラメータ
    /// </summary>
    public class ImageViewerContentsParameter : IContentsParameter
    {
        private IList<string> _filePathList;
        private string _selectedFilePath;

        public IList<string> FilePathList
        {
            get
            {
                return _filePathList;
            }
        }

        public string SelectedFilePath
        {
            get
            {
                return _selectedFilePath;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _selectedFilePath = value;
            }
        }

        public string ContentsTitle { get; private set; }

        public Image ContentsIcon { get; private set; }

        public ImageViewerContentsParameter(IList<string> filePathList, string selectedFilePath)
        {
            if (filePathList == null)
            {
                throw new ArgumentNullException("filePathList");
            }

            if (selectedFilePath == null)
            {
                throw new ArgumentNullException("selectedFilePath");
            }

            _filePathList = filePathList;
            _selectedFilePath = selectedFilePath;
            this.ContentsTitle = string.Empty;
            this.ContentsIcon = null;
        }

        public ImageViewerContentsParameter(IList<string> filePathList, string selectedFilePath, string contentsTitle, Image contentsIcon)
        {
            if (filePathList == null)
            {
                throw new ArgumentNullException("filePathList");
            }

            if (selectedFilePath == null)
            {
                throw new ArgumentNullException("selectedFilePath");
            }

            _filePathList = filePathList;
            _selectedFilePath = selectedFilePath;
            this.ContentsTitle = contentsTitle ?? throw new ArgumentNullException(nameof(contentsTitle));
            this.ContentsIcon = contentsIcon ?? throw new ArgumentNullException(nameof(contentsIcon));
        }

        public ContentsPanel CreateContents()
        {
            return new ImageViewerContents.ImageViewerContents(this);
        }
    }
}
