using System.ComponentModel.DataAnnotations;

namespace Booking.Dtos
{
    public class AddTicketTypeRequestDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required, Range(1, short.MaxValue)]
        public short Activity { get; set; }

        [Required]
        public short Price { get; set; }

        public string? Description { get; set; }

        [Required, Range(1, short.MaxValue)]
        public short Total { get; set; }
    }
    public class UpdateTicketTypeRequestDto
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public short Price { get; set; }

        public string? Description { get; set; }

        [Required, Range(1, short.MaxValue)]
        public short Total { get; set; }
    }

    public class UpdateLimitTicketTypeRequestDto
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required, Range(1, short.MaxValue)]
        public short Total { get; set; }
    }
}
