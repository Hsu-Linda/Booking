using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Order
{
    public int Id { get; set; }

    public int Member { get; set; }

    public DateTime Trading { get; set; }

    public byte Status { get; set; }
}
