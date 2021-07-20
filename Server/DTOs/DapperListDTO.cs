using System.Collections.Generic;

namespace RecrutariBlazorWasm.Server.DTOs
{
    public class DapperQueryDTO<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }

        public DapperQueryDTO(IEnumerable<T> items, int totalItems)
        {
            this.Items = items;
            this.TotalItems = totalItems;
        }
    }
}
