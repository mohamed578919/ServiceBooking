namespace ServiceBooking.DTOs
{
    public class ServiceUpdateDto : ServiceCreateDto
    {
        public bool IsActive { get; set; } = true;
    }
}
