using System;
using System.Collections.Generic;

namespace Booking.Models;

public partial class Company
{
    public short CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[] Salt { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public bool? Deleted { get; set; }
}
