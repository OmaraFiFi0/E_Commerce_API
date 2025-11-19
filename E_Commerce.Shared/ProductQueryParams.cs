using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared
{
    public class ProductQueryParams
    {
        public int? brandId { get; set; }
        public int? typeId { get; set; }
        public string? search { get; set; }

        public ProductSortingOptions sort { get; set; }

        private int _pageIndex = 1; 
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = (value <= 0) ? _pageIndex : value;  }
        }
        private const int _defaultPageSize = 5;
        private  const int _maxSize = 10; 
        private int _pageSize =_defaultPageSize;

        public int PageSize
        {
            get { return _pageSize; }
            set 
            {
                if (_pageSize <= 0)
                    _pageSize = _defaultPageSize;
                else if (PageSize > 10)
                    _pageSize = _maxSize;
                else
                    _pageSize = value;
            }
        }


    }
}
