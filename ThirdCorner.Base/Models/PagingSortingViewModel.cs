using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThirdCorner.Base.Models
{
    public class PagingSortingViewModel
    {
        public int PageCount;
        public int PageIndex;
        public string SortColumn;
        public string SortDirection;

        public bool ShowNext
        {
            get { return PageIndex < PageCount - 1 && PageCount > 1; }
        }

        public bool ShowBack
        {
            get { return PageIndex > 0; }
        }

        
    }
}