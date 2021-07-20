using RecrutariBlazorWasm.Shared.Helpers;
using System.Collections.Generic;

namespace RecrutariBlazorWasm.Client.Helpers
{
    public class PagingResponse<T>
    {
        public List<T> Items { get; set; }
        public MetaData Metadata { get; set; }
    }
}
