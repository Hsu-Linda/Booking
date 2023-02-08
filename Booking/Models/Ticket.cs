using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Ticket
{
    public string TicketId { get; set; } = null!;

    public string Member { get; set; } = null!;

    public string Activity { get; set; } = null!;

    public string OrderTime { get; set; } = null!;

    public int State { get; set; }
}
