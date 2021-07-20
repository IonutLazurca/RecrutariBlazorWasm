using System;
using System.Collections.Generic;
using System.Linq;

namespace RecrutariBlazorWasm.Server.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            TotalItems = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            CurrentPage = pageNumber;
            this.AddRange(items);
        }

        public static PagedList<T> Paginate(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var totalItems = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, totalItems, pageNumber, pageSize);
        }
    }
}
