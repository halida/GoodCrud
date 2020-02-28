using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Mvc.Core;
using X.PagedList.Mvc.Bootstrap4.NetCore;
using Microsoft.AspNetCore.Mvc.Routing;
using AutoMapper;
using GoodCrud.Application.Contract.Dtos;

namespace GoodCrud.Web.Helpers
{
    public static class HtmlHelperExtensions
    {

        public static IHtmlContent ListView<T>(this IHtmlHelper htmlHelper, string viewName, PagedListDto<T> pagedList)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext);
            var mapper = (IMapper)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IMapper));
            var metaData = mapper.Map<X.PagedList.PagedListMetaData>(pagedList.MetaData);

            var listContent = htmlHelper.Partial(viewName, pagedList.List);
            if (metaData.TotalItemCount <= 0)
            {
                return new HtmlString("<div class='p-4 text-center' style='background: #fafafa'>Empty</div>");
            }

            var paginationContent = htmlHelper.PagedListPager(
                metaData, page =>
                {
                    var request = htmlHelper.ViewContext.HttpContext.Request;
                    var uri = Common.GetUpdatedUri(request, "page", page.ToString());
                    return Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(uri, "page", page.ToString());
                }, Bootstrap4PagedListRenderOptions.ClassicPlusFirstAndLast);

            var content = new HtmlContentBuilder()
                .AppendHtml(paginationContent)
                .AppendHtml(listContent)
                .AppendHtml(paginationContent);
            return content;
        }
    }
}
