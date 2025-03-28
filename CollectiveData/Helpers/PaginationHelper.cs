using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CollectiveData.Helpers
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            
            // Ensure pageIndex is at least 1
            pageIndex = Math.Max(1, pageIndex);
            
            // Ensure pageSize is reasonable
            pageSize = Math.Clamp(pageSize, 1, 100);
            
            var items = await source.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();
                
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }

    public class PaginationParameters
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 10;
        private int _pageIndex = 1;
        
        public int PageIndex 
        { 
            get => _pageIndex; 
            set => _pageIndex = (value < 1) ? 1 : value; 
        }
        
        public int PageSize 
        { 
            get => _pageSize; 
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : (value < 1) ? 1 : value; 
        }
        
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
        public string? SearchTerm { get; set; }
    }
}
