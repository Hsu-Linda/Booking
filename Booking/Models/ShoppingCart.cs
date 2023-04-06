using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class ShoppingCart
{
    public int ShoppingCartId { get; set; }

    public int Member { get; set; }

    public int Activity { get; set; }

    public int TicketType { get; set; }

    public int Showing { get; set; }

    public DateTime AddedDateTime { get; set; }

    public bool? Deleted { get; set; }
}
