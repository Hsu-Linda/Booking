using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Company
{
    public string CompanyId { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public string CompanyPhone { get; set; } = null!;

    public string CompanyAccount { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public byte[] Password { get; set; } = null!;
}
