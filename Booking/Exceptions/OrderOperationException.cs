namespace Booking.Exceptions
{
    public class OrderOperationException : Exception
    {
        public OrderOperationException() { }
        public OrderOperationException(string message) : base(message) { }
        public OrderOperationException(string message,Exception inner) : base(message, inner) { }
    }
}
