using Booking.Models;

namespace Booking.Dtos
{
    public class CreateOrderDto
    {
        public Guid OrderId { get; set; }
        public int Member { get; set; } = 0;

        public List<TicketTypeAccount> Tickets { get; set; } = new List<TicketTypeAccount>();

    }
}
