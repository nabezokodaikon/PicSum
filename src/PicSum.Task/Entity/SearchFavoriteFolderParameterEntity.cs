﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PicSum.Core.Task.Base;

namespace PicSum.Task.Entity
{
    public class SearchFavoriteDirectoryParameterEntity : IEntity
    {
        public bool IsOnlyDirectory { get; set; }
        public int Count { get; set; }
    }
}
