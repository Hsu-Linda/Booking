using Booking.Models;

namespace Booking.Dtos
{
    public class ActivityWithLikeDto
    {
        public int ActivityId { get; set; }

        public string ActivityName { get; set; } = null!;

        public int Company { get; set; }

        public string? Photo { get; set; }

        public string? PhotoUrl { get; set; }

        public int City { get; set; }

        public int District { get; set; }

        public string Address { get; set; } = null!;

        public bool Like { get; set; }=false;
    
    }
}
