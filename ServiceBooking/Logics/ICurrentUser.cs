namespace ServiceBooking.Logics
{
    public interface ICurrentUser
    {
        string UserId { get; }
        bool IsInRole(string role);
        string? GetUserId();
    }
}
