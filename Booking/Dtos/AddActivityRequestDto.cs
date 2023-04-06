using Booking.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Booking.Dtos
{
    public class AddActivityRequestDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required(AllowEmptyStrings =true)]
        public string? PhotoUrl { get; set; }

        [Required,Range(1,byte.MaxValue)]
        public byte City { get; set; }
        
        [Required,Range(1,short.MaxValue)]
        public short District { get; set; }
        
        [Required]
        public string Address { get; set; } = null!;

        [Required, Range(typeof(DateTime), "1/1/2000", "12/31/9999")]
        public DateTime ShowingStart { get; set; }

        [Required, Range(typeof(DateTime), "1/1/2000", "12/31/9999")]
        public DateTime ShowingEnd { get; set; }

        [Required, Range(typeof(DateTime), "1/1/2000", "12/31/9999")]
        public DateTime SalesStart { get; set; }

        [Required, Range(typeof(DateTime), "1/1/2000", "12/31/9999")]
        public DateTime SalesEnd { get; set; }

    }

    public class UpdateActivityRequest
    {
        [Required, Range(1, short.MaxValue)]
        public short Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required(AllowEmptyStrings =true)]
        public string? PhotoUrl { get; set; }

        [Required,Range(1,byte.MaxValue)]
        public byte City { get; set; }

        [Required,Range(1,short.MaxValue)]
        public short District { get; set; }

        [Required]
        public string Address { get; set; } = null!;
        
        [Required,Range(typeof(DateTime),"1/1/2000","12/31/9999")]
        public DateTime ShowingStart { get; set; }

        [Required, Range(typeof(DateTime),"1/1/2000","12/31/9999")]
        public DateTime ShowingEnd { get; set; }

        [Required, Range(typeof(DateTime),"1/1/2000","12/31/9999")]
        public DateTime SalesStart { get; set; }

        [Required,Range(typeof(DateTime),"1/1/2000","12/31/9999")]
        public DateTime SalesEnd { get; set; }
    }

    public class deleteActivityRequest
    {
        [Required, Range(1, short.MaxValue)]
        public short Id { get; set; }
    }
}
