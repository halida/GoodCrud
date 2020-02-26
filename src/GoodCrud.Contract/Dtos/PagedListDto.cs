using System.Collections.Generic;

namespace GoodCrud.Contract.Dtos
{
    public class PagedListDto<T>
    {
        public List<T> List { get; set; }
        public PagedListMetaData MetaData { get; set; }
    }

    // opens the sets
    public class PagedListMetaData
    {
        public int PageCount { get; set; }
        public int TotalItemCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public bool IsFirstPage { get; set; }
        public bool IsLastPage { get; set; }
        public int FirstItemOnPage { get; set; }
        public int LastItemOnPage { get; set; }

    }
}
