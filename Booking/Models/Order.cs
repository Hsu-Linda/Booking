using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Order
{
    public int Id { get; set; }

    public short Member { get; set; }

    public DateTime Trading { get; set; }

    public byte Status { get; set; }

    public string? Items { get; set; }
}
