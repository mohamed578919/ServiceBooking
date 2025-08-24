using Microsoft.EntityFrameworkCore;

namespace ServiceBooking.Models
{
    public class PaymentTransaction
    {
        public int Id { get; set; }
        public string PayPalOrderId { get; set; }
        public string Status { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string PayerEmail { get; set; }


        //===============================================================
        public int ClientId { get; set; }

        // Navigation Property للعميل
        public Client Client { get; set; }


        //===================================
        // One-to-One مع Request
        public int RequestId { get; set; }
        public Request Request { get; set; } = default!;
    }
}
