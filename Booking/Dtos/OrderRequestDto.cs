using Booking.Models;
using System.ComponentModel.DataAnnotations;

namespace Booking.Dtos
{
    public class OrderRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int ticketTypeID { get; set; }

        [Required, Range(1, short.MaxValue)]
        public short purchaseQuantity { get; set; }
    }
    public class AddOrderRequestDto
    {
        [Required]
        public List<OrderRequest> Items { get; set; }
    }

    public class tickets
    {
        [Required, Range(1, int.MaxValue)]
        public int ticetID { get; set; }

        [Required, Range(1, short.MaxValue)]
        public short activityID { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int ticketTypeID { get; set; }
    }

    public partial class OrderDetailResponseDto
    {
        public int Id { get; set; }

        public short Member { get; set; }

        public DateTime Trading { get; set; }

        public byte Status { get; set; }

        public List<Ticket> Items { get; set; }
    }

}
