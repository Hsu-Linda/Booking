namespace Booking.Dtos
{
    public class UpdateTicketTypeRequestDto
    {
        public string TicketTypeName { get; set; } = null!;

        public int Price { get; set; }

        public string? Description { get; set; }

        public int? NumOfTotal { get; set; }

        public int? NumOfRemaining { get; set; }

    }
}
