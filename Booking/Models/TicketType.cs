using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class TicketType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public short Activity { get; set; }

    public short Price { get; set; }

    public string? Description { get; set; }

    public short Total { get; set; }

    public short Sales { get; set; }

    public bool Active { get; set; }

    public DateTime LastModified { get; set; }
}
