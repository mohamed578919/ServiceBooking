namespace ServiceBooking.DTOs
{
    public class CreateApplicationDto
    {
        public string ProviderUserName { get; set; }
        public string PhoneNumber { get; set; }
        public string ProposalText { get; set; }
        public decimal ProposedPrice { get; set; }
        public string Note { get; set; }

        public int ServiceRequestId { get; set; }
    }
}
