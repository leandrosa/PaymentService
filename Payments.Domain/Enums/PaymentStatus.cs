namespace Payments.Domain.Enums
{
    public enum PaymentStatus
    {
        Uprocessed = 0,
        Pending = 1,
        Success = 2,
        Complete = 3,
        Cancelled = 4,
        Rejected = 5
    }
}
