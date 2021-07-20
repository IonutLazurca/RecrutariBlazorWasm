namespace RecrutariBlazorWasm.Shared.Helpers
{
    public class MetaData
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        //public Metadata(int pageNumber, int totalPages, int pageSize, int totalItems)
        //{
        //    this.PageNumber = pageNumber;
        //    this.TotalPages = totalPages;
        //    this.PageSize = pageSize;
        //    this.TotalItems = totalItems;
        //}
    }
}
