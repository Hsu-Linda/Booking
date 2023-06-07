using Booking.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Booking.Dtos
{
    public class ActivityInfoResponse
    {
        public string Name { get; set; } = null!;

        public short Company { get; set; }

        public string? PhotoUrl { get; set; }

        public byte City { get; set; }

        public short District { get; set; }

        public string Address { get; set; } = null!;

        public DateTime ShowingStart { get; set; }

        public DateTime ShowingEnd { get; set; }

        public DateTime SalesStart { get; set; }

        public DateTime SalesEnd { get; set; }

    }
    public class AddActivityRequestDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required(AllowEmptyStrings = true)]
        public string? PhotoUrl { get; set; }

        [Required, Range(1, byte.MaxValue)]
        public byte City { get; set; }

        [Required, Range(1, short.MaxValue)]
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

    public class SearchCondition
    {
        public short? companyID { get; set; }
        public string? keyWord { get; set; }
    };

    public class UpdateActivityRequest
    {
        [Required, Range(1, short.MaxValue)]
        public short Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required(AllowEmptyStrings = true)]
        public string? PhotoUrl { get; set; }

        [Required, Range(1, byte.MaxValue)]
        public byte City { get; set; }

        [Required, Range(1, short.MaxValue)]
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
        public bool limit { get; set; } = false;

    }

    public class UpdateLimitActivityRequest
    {
        [Required, Range(1, short.MaxValue)]
        public short Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required(AllowEmptyStrings = true)]
        public string? PhotoUrl { get; set; }

        [Required, Range(1, byte.MaxValue)]
        public byte City { get; set; }

        [Required, Range(1, short.MaxValue)]
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

    public class deleteActivityRequest
    {
        [Required, Range(1, short.MaxValue)]
        public short Id { get; set; }
    }


    public class ActivityWithLikeDto
    {
        public int ActivityId { get; set; }

        public string ActivityName { get; set; } = null!;

        public int Company { get; set; }

        public string? Photo { get; set; }

        public string? PhotoUrl { get; set; }

        public int City { get; set; }

        public int District { get; set; }

        public string Address { get; set; } = null!;

        public bool Like { get; set; } = false;

    }
}
