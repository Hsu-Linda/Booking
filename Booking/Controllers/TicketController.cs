using AutoMapper;
using Booking.Dtos;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly BookingContext _bookingContext;
        private readonly IMapper _iMapper;
        public TicketController(BookingContext bookingContext, IMapper mapper )
        {
            _bookingContext = bookingContext;
            _iMapper = mapper;
        }

       // [HttpGet,Authorize]
       //public ActionResult<IEnumerable<Ticket>> Get() {

       //     int memberId = int.Parse(User.FindFirstValue("memberID"));
       //     IEnumerable<Ticket> tickets = _bookingContext.Tickets.Where(t => t.MemberId == memberId);
       //     if (tickets.Any()) 
       //         return Ok(tickets);
       //     return NotFound(null);
       // }
        

        // GET api/<TickeyController>/5
        //[HttpGet("{id}")]
        //public ActionResult<Ticket> Get(Guid id)
        //{
        //    var result = _bookingContext.Tickets.Where(t => t.TicketId.Equals(id)).SingleOrDefault();
        //    if(result==null)
        //        return NotFound("找不到編號為"+ id + " 的票券");
        //    return Ok(result);
        //}
    }
}
