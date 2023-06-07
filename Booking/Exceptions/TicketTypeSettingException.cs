namespace Booking.Exceptions
{
    public class TicketTypeSettingException:Exception
    {
        public TicketTypeSettingException() { }
        public TicketTypeSettingException(string message) : base(message) { }
        public TicketTypeSettingException(string message, Exception inner) : base(message,inner) { }
    }
}
