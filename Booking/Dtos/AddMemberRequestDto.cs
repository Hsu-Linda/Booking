using Booking.Models;

namespace Booking.Dtos
{
    public class AddMemberRequestDto
    {

        public string MemberName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
