using Booking.Models;

namespace Booking.Dtos
{
    public class AddTicketTypeRequestDto
    {
        public string TicketTypeName { get; set; } = null!;

        public int ShowingId { get; set; }

        public int Price { get; set; }

        public string? Description { get; set; }

        public int? NumOfTotal { get; set; }

        public int? NumOfRemaining { get; set; }
        
        public int ActivityId { get; set; }

    }
}
