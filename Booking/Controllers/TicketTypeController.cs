using AutoMapper;
using Booking.Dtos;
using Booking.Exceptions;
using Booking.Filters;
using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExceptionHandler]
    public class TicketTypeController : ControllerBase
    {
        private readonly TicketTypeService _ticketTypeService;
        public TicketTypeController(TicketTypeService ticketTypeService)
        {
            _ticketTypeService = ticketTypeService;
        }

        [HttpGet("get/{activityID}"), Authorize(Roles = "company")]
        public ActionResult<List<TicketType>> Get(short activityID)
        {
            if (!Byte.TryParse(User.FindFirstValue("companyID"), out byte companyID))
            {
                throw new LoseClaimsException();
            }
            return Ok(_ticketTypeService.GetAll(activityID, companyID));
        }

        [HttpPost("add"), Authorize(Roles = "company")]
        public ActionResult Add([FromBody] AddTicketTypeRequestDto tickeyTypeRequest)
        {
            if (!Byte.TryParse(User.FindFirstValue("companyID"), out byte companyID))
            {
                throw new LoseClaimsException();
            }
            _ticketTypeService.Add(tickeyTypeRequest, companyID);
            return Ok();

        }

        [HttpPost("update"), Authorize(Roles = "company")]
        public ActionResult Update(UpdateTicketTypeRequestDto ttRequest)
        {
            if (!Byte.TryParse(User.FindFirstValue("companyID"), out byte companyID))
            {
                throw new LoseClaimsException();
            }
            _ticketTypeService.Update(ttRequest, companyID);
            return Ok();
        }


        [HttpPost("delete/{id}"), Authorize(Roles = "company")]
        public ActionResult Delete(int id)
        {
            if (!Byte.TryParse(User.FindFirstValue("companyID"), out byte companyID))
            {
                throw new LoseClaimsException();
            }
            _ticketTypeService.Delete(id, companyID);
            return Ok();
        }

    }
}
