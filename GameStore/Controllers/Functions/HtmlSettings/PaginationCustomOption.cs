using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Controllers.Functions.HtmlSettings
{
    public class PaginationCustomOption : PagedListRenderOptions
    {
        public static PagedListRenderOptions ShowOnlySomePages(int countOfPages, string first,  string last,  string prev, string next)
        {
            return new PagedListRenderOptions
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                DisplayLinkToLastPage = PagedListDisplayMode.Always,
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                DisplayLinkToNextPage = PagedListDisplayMode.Always,
                MaximumPageNumbersToDisplay = countOfPages,
                LinkToFirstPageFormat = first,
                LinkToPreviousPageFormat = prev,
                LinkToNextPageFormat = next,
                LinkToLastPageFormat = last,
            };
            
        }
    }
}