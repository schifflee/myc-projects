using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyKTV_Management_Studio
{
    public class Pagination<T>:List<T>
    {
        public int DataCount { get; set; } //总记录数
        public int PageCount { get; set; } //总页数
        public int PageIndex { get; set; } //当前页码
        public int PageSize { get; set; } //每页显示记录数

        public bool HasPreviousPage
        {
            get { return PageIndex>1; }
        }
        public bool HasNextPage
        {
            get { return PageIndex<this.PageCount; }
        }

        public Pagination(List<T> dataList, int pageSize, int pageIndex)
        {
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.DataCount = dataList.Count;
            this.PageCount = (int)Math.Ceiling((decimal)this.DataCount / pageSize);
            this.AddRange(dataList.Skip((pageIndex - 1) * pageSize).Take(pageSize));
        }
    }
}
