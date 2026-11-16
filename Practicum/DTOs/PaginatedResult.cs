namespace Practicum.DTOs
{
    public class PaginatedResult <T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; } = new(); 

        public int Page { get; set; }
        public int PageSize { get; set; }   
    }
}
