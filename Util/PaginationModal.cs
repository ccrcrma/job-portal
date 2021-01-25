using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace job_portal.Util
{
    public class PaginationModal<T> : List<T> where T : class
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; } = 1;

        public PaginationModal(List<T> items, int currentPage, int totalPage)
        {
            AddRange(items);
            CurrentPage = currentPage;
            TotalPage = totalPage;
        }

        public bool HasNextPage
        {
            get
            {
                return CurrentPage < TotalPage;
            }
        }
        public bool HasPreviousPage
        {
            get
            {
                return CurrentPage > 1;
            }
        }
        public static async Task<PaginationModal<T>> CreateAsync(IQueryable<T> queryable, int pageIndex, int pageSize)
        {
            var totalItems = await queryable.CountAsync();
            var items = await queryable
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (decimal)pageSize);
            return new PaginationModal<T>(items, pageIndex, totalPage: totalPages);
        }
    }
}