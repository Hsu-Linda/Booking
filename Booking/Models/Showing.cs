using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Showing
{
    public int Id { get; set; }

    public short Activity { get; set; }

    public DateTime ShowingStart { get; set; }

    public DateTime ShowingEnd { get; set; }

    public DateTime SalesStart { get; set; }

    public DateTime SalesEnd { get; set; }

    public bool Deleted { get; set; }

    public DateTime? LastModified { get; set; }

    public virtual Activity ActivityNavigation { get; set; } = null!;

    public virtual ICollection<TicketType> TicketTypes { get; } = new List<TicketType>();
}
