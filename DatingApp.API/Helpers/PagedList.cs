using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Helpers
{
    public class PagedList<T>: List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public PagedList(IList<T> items, int count, int pageNumber, int PageSize )
        {
            this.TotalCount=count;
            this.PageSize=PageSize;
            this.CurrentPage = pageNumber;
            this.TotalPages = (int) Math.Ceiling(count / (double) PageSize);
            this.AddRange(items);
        }
        
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,
            int pageNumber, int PageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * PageSize)
                                    .Take(PageSize)
                                    .ToListAsync(); // todo check the query
            return new PagedList<T>(items, count, pageNumber, PageSize);        
        }
        
    }
}