using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Member
{
    public string MemberId { get; set; } = null!;

    public string MemberName { get; set; } = null!;

    public string? Email { get; set; }

    public string Account { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public byte[] Password { get; set; } = null!;
}
