using Booking.Models;

namespace Booking.Dtos
{
    public class QueryCompanyInfoResponseDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;

        public string? CompanyPhone { get; set; }
    }
}
