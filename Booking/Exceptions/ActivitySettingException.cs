namespace Booking.Exceptions
{
    public class ActivitySettingException : Exception
    {
        public ActivitySettingException()
        {

        }
        public ActivitySettingException(string message):base(message)
        {
        }

        public ActivitySettingException(string message, Exception inner) : base(message, inner)
        {

        }

    }
}
