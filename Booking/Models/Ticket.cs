using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public short Activity { get; set; }

    public int TicketType { get; set; }

    public int OrderId { get; set; }

    public int Member { get; set; }

    public byte Status { get; set; }

    public DateTime LastModified { get; set; }
}
