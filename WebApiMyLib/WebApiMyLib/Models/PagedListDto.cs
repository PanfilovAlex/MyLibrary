using System.Collections.Generic;

namespace WebApiMyLib.Models
{
    public class PagedListDto<T>
    {
        public List<T> Items { get; private set; }
        public int TotalCount { get; private set; }

        public PagedListDto(List<T> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }
    }
}
