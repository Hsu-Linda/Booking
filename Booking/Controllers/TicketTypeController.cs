using AutoMapper;
using Booking.Dtos;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketTypeController : ControllerBase
    {
        private readonly BookingContext _bookingContext;
        private readonly IMapper _iMapper;
        public TicketTypeController(BookingContext bookingContext, IMapper imapper)
        {
            _bookingContext= bookingContext;
            _iMapper= imapper;
        }

        [HttpGet("{id}"),Authorize(Roles ="company")]
        public ActionResult<TicketType> Get(int id) 
        { 
            var result = _bookingContext.TicketTypes.Find(id);
            if (result == null) return NotFound(null);
            return Ok(result);
        }
        
        [HttpPost,Authorize(Roles ="company")]
        public ActionResult<TicketType> Post([FromBody] AddTicketTypeRequestDto tickeyTypeRequest)
        {
            var result = _iMapper.Map<TicketType>(tickeyTypeRequest);
            if(tickeyTypeRequest.Description!= null) result.Description = tickeyTypeRequest.Description;
            
            
            _bookingContext.Add(result);
            _bookingContext.SaveChanges();

            return Ok(result);
        }

        [HttpPut("{id}"), Authorize(Roles ="company")]
        public bool Put(int id, UpdateTicketTypeRequestDto ttRequest)
        {
            var update = _bookingContext.TicketTypes
                            .Find(id);
            
            if (update == null) return false;
            
            _iMapper.Map<UpdateTicketTypeRequestDto, TicketType>(ttRequest, update);
            _bookingContext.SaveChanges();
            return true;
        }


        //[HttpDelete("{id}"), Authorize(Roles ="company")]
        //public ActionResult<string> Delete(int id)
        //{
        //    var delete = _bookingContext.TicketTypes
        //                    .Where(tt => tt.TicketTypeId == id)
        //                    .Include(tt => tt.Tickets)
        //                    .SingleOrDefault();

        //    if (delete == null) return BadRequest("票種編號錯誤");
        //    if (delete.Tickets.Count > 0) return BadRequest("已有購買，不可刪除");

        //    _bookingContext.Remove(delete);
        //    _bookingContext.SaveChanges();
        //    return Ok();
        //}

    }
}
