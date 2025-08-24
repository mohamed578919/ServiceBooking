namespace ServiceBooking.DTOs
{
    public class ProviderSearchQuery
    {
        public int? CategoryId { get; set; }
        public string? Keyword { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
