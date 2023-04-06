using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class ActivityView
{
    public short Id { get; set; }

    public byte Company { get; set; }

    public int? Daydiff { get; set; }

    public int? Sales { get; set; }

    public DateTime ShowingStart { get; set; }

    public DateTime ShowingEnd { get; set; }

    public byte City { get; set; }
}
