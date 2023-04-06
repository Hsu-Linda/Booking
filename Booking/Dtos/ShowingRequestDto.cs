namespace Booking.Dtos
{
    public class AddShowingRequest
    {
        public int Activity { get; set; }

        public string ShowingStart { get; set; }

        public string ShowingEnd { get; set; }

        public string SalesStart { get; set; }

        public string SalesEnd { get; set; }
    }


    public class UpdateShowingRequest
    {
        public int ID { get; set; }

        public string ShowingStart { get; set; }

        public string ShowingEnd { get; set; }

        public string SalesStart { get; set; }

        public string SalesEnd { get; set; }

    }

    
    public class DeleteShowingRequest
    {
        public int ID { get; set; }
    }
}
