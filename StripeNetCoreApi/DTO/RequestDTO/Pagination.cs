using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DTO.RequestDTO
{
    public class Pagination
    {
        private int _pagesize = 20;
        private int _page = 1;

        [DefaultValue(0)]
        public int PageSize { get { return _pagesize; } set { this._pagesize = value; } }
        [DefaultValue(0)]
        public int Page { get { return _page; } set { this._page = value; } }
    }
}
