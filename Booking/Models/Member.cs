using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Member
{
    public short MemberId { get; set; }

    public string MemberName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[] Salt { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public bool? Deleted { get; set; }
}
