using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Like
{
    public int LikeId { get; set; }

    public short MemberId { get; set; }

    public short ActivityId { get; set; }

    public byte CompanyId { get; set; }

    public bool? Deleted { get; set; }
}
