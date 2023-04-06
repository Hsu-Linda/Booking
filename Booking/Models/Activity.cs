using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Activity
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public byte Company { get; set; }

    public string? PhotoUrl { get; set; }

    public byte City { get; set; }

    public short District { get; set; }

    public string Address { get; set; } = null!;

    public DateTime ShowingStart { get; set; }

    public DateTime ShowingEnd { get; set; }

    public DateTime SalesStart { get; set; }

    public DateTime SalesEnd { get; set; }

    public bool Active { get; set; }

    public DateTime LastModified { get; set; }
}
