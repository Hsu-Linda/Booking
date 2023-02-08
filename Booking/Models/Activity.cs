using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Activity
{
    public string ActivityId { get; set; } = null!;

    public string ActivityName { get; set; } = null!;

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public string? Photo { get; set; }

    public string? PhotoUrl { get; set; }

    public string City { get; set; } = null!;

    public string District { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Company { get; set; } = null!;
}
