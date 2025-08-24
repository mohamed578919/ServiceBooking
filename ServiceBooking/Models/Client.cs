using Microsoft.VisualBasic;

namespace ServiceBooking.Models
{
    public class Client 
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

        public virtual ICollection<Request> Requests { get; set; } = new HashSet<Request>();
        public virtual ICollection<Complaint>? Complaints { get; set; } = new HashSet<Complaint>();
        public virtual ICollection<PaymentTransaction>? PaymentTransactions { get; set; } = new HashSet<PaymentTransaction>();





    }
}
