using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class TicketTypeRemainingView
{
    public int Id { get; set; }

    public short? Remaining { get; set; }
}
