using AutoMapper;
using Booking.Dtos;
using Booking.Exceptions;
using Booking.Models;
using Booking.Services;
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
        private readonly IMapper _iMapper;
        private readonly TicketService _ticketService;
        public TicketController(
            IMapper mapper,
            TicketService ticketService)
        {
            _iMapper = mapper;
            _ticketService = ticketService;
        }

        [HttpGet("allTickets"), Authorize(Roles = "member")]
        public ActionResult GetAllTickets()
        {
            if (!Byte.TryParse(User.FindFirstValue("memberID"), out byte memberID))
            {
                throw new LoseClaimsException();
            }
            return Ok(_ticketService.GetByMemberID(memberID));
        }
    }
}
