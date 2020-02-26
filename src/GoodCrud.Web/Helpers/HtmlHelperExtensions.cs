using GoodCrud.Contract.Dtos;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Mvc.Core;
using X.PagedList.Mvc.Bootstrap4.NetCore;
using Microsoft.AspNetCore.Mvc.Routing;


namespace GoodCrud.Web.Helpers
{
    public static class HtmlHelperExtensions
    {

        public static IHtmlContent ListView<T>(this IHtmlHelper htmlHelper, string viewName, PagedListDto<T> pagedList)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext);
            var paginationContent = htmlHelper.PagedListPager(
                pagedList.MetaData, page =>
                {
                    var request = htmlHelper.ViewContext.HttpContext.Request;
                    var uri = Common.GetUpdatedUri(request, "page", page.ToString());
                    return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(uri, "page", page.ToString());
                }, Bootstrap4PagedListRenderOptions.ClassicPlusFirstAndLast);


            var listContent = htmlHelper.Partial(viewName, pagedList.List);

            var content = new HtmlContentBuilder()
                .AppendHtml(paginationContent)
                .AppendHtml(listContent)
                .AppendHtml(paginationContent);
            return content;
        }
    }
}
