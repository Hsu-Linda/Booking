using AutoMapper;
using Booking.Dtos;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly BookingContext _bookingContext;
        private readonly IMapper _iMapper;
        private readonly IMemoryCache _iMemoryCache;

        public OrderController(
            BookingContext bookingContext,
            IMapper iMapper,
            IMemoryCache iMemoryCache
            )
        {
            _bookingContext = bookingContext;
            _iMapper = iMapper;
            _iMemoryCache = iMemoryCache;
        }

        [HttpGet("{id}"), Authorize(Roles ="member")]
        public ActionResult<Order> GetOrderDetail(Guid id)
        {
            //var result = _bookingContext.Orders
            //                .Where(o => o.OrderId == id)
            //                .Include(o => o.Tickets)
            //                .SingleOrDefault();

            //if (result == null) 
            //{ 
            //    return NotFound(); 
            //}
            //if (result.Member != int.Parse(User.FindFirstValue("memberID")))
            //{
            //    return NotFound("沒有權限");
            //}
            
            return  Ok() ;
        }

        //[HttpPost("buyTicketProcess/{activityID}")]
        //public ActionResult<string> BuyTicketProcess(int activityID)
        //{
        //    bool remainValue = _iMemoryCache.TryGetValue(activityID, out var ticket);
        //    //有緩存
        //    if(remainValue)
        //        return Ok(ticket);
        //    //沒緩存
        //    var result = _bookingContext.TicketTypes
        //                    .Where(ttype => ttype.ActivityId == activityID)
        //                    .Where(ttype => ttype.NumOfRemaining >0)
        //                    .Select(ttype => _iMapper.Map<TicketTypeAccount>(ttype))
        //                    .ToList();
        //    _iMemoryCache.Set(activityID, result);
        //    return Ok(result);
        //}

        //[HttpPost("createOrder"), Authorize(Roles ="member")]
        //public ActionResult<string> PlacAnOrder([FromBody] List<TicketTypeAccount> ticketsRequest)
        //{
        //    //判斷request是否都有activityID  因為抓緩存靠activityID
        //    if (ticketsRequest.Any(tr => 0 == tr.ActivityID )) return BadRequest("error");


        //    foreach (var ticket in ticketsRequest)
        //    {
        //        //拿緩存
        //        bool memoryCache = _iMemoryCache.TryGetValue(ticket.ActivityID, out List<TicketTypeAccount>? memoryValue);
        //        if (!memoryCache)
        //        {
        //            //沒緩存
        //            var result = _bookingContext.TicketTypes
        //                            .Where(ttype => ttype.ActivityId == ticket.ActivityID)
        //                            .Select(ttype => _iMapper.Map<TicketTypeAccount>(ttype))
        //                            .ToList();
        //            _iMemoryCache.Set(ticket.ActivityID, result);
        //            memoryValue = (List<TicketTypeAccount>?)_iMemoryCache.Get(ticket.ActivityID);
        //        }


        //        //找出對應票種的緩存值  將緩存值-1
        //        TicketTypeAccount? tTypeMemory = memoryValue?.Find(m => m.TicketType.Equals(ticket.TicketType));
        //        if (tTypeMemory == null || tTypeMemory.NumOfRemaining < 1)
        //        {
        //            var a = JsonConvert.SerializeObject(memoryCache);
        //            return BadRequest("票量不夠"+a);
        //        }


        //        tTypeMemory.NumOfRemaining -= ticket.NumOfBuy;
        //    }

        //    ////拿到使用者ID memberID
        //    string memberID = User.FindFirstValue("memberID");

        //    ////創建臨時訂單  存入緩存中
        //    CreateOrderDto tempOrder = new CreateOrderDto()
        //    {
        //        OrderId = Guid.NewGuid(),
        //        Member = int.Parse(memberID),
        //        Tickets = ticketsRequest
        //    };

        //    _iMemoryCache.Set(memberID + "//" + tempOrder.OrderId, tempOrder);

        //    Console.WriteLine("-----------------------------");
        //    Console.WriteLine("訂單-----------------------------");
        //    Console.WriteLine(JsonConvert.SerializeObject(_iMemoryCache.Get(memberID + "//" + tempOrder.OrderId)));
        //    Console.WriteLine("-----------------------------"); 
        //    Console.WriteLine("-----------------------------");


        //    Console.WriteLine("-----------------------------");
        //    Console.WriteLine("活動剩餘數量緩存-----------------------------");
        //    Console.WriteLine(JsonConvert.SerializeObject(_iMemoryCache.Get(3)));
        //    Console.WriteLine("-----------------------------");
        //    Console.WriteLine("-----------------------------");

        //    return Ok(tempOrder);
        //}

        //    [HttpPost("pay"), Authorize(Roles = "member")]
        //    public ActionResult<string> Pay([FromForm] bool isPaymentSuccessful, [FromForm] Guid orderID)
        //    {
        //        if (isPaymentSuccessful)
        //        {
        //            string memberID = User.FindFirstValue("memberID");
        //            var tempOrder = (CreateOrderDto?)_iMemoryCache.Get(memberID + "//" + orderID);

        //            if (tempOrder == null) return BadRequest("沒有該order序號" + orderID);
        //            _bookingContext.Orders.Add(new Order
        //            {
        //                Member = int.Parse(memberID),
        //                OrderId = orderID,
        //                TradingDateTime = DateTime.Now
        //            });


        //            foreach (var t in tempOrder.Tickets)
        //            {
        //                _bookingContext.TicketTypes.Find(t.TicketType).NumOfRemaining--;

        //                _bookingContext.Tickets.Add(new Ticket
        //                {
        //                    TicketId = Guid.NewGuid(),
        //                    TicketType = t.TicketType,
        //                    State = 1,
        //                    OrderId = orderID,
        //                    MemberId = int.Parse(memberID)
        //                });
        //            }

        //            _bookingContext.SaveChanges();
        //            return Ok("訂票成功" + orderID);
        //        }
        //        return BadRequest("尚未付款成功" + isPaymentSuccessful + orderID);
        //    }
    }
}
