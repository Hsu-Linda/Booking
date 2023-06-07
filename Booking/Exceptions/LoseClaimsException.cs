namespace Booking.Exceptions
{
    public class LoseClaimsException:Exception
    {
        public LoseClaimsException() { }
        public LoseClaimsException(string message) : base(message) { }
        public LoseClaimsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
