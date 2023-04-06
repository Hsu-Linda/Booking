//using AutoMapper;
//using Azure.Core;
//using Booking.Dtos;
//using Booking.Models;
//using Booking.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Reflection.PortableExecutable;
//using System.Security.Claims;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace Booking.Controllers
//{
//    [Route("enterprise")]
//    [ApiController]
//    public class ShowingController : ControllerBase
//    {
//        private readonly BookingContext _bookingContext;
//        private readonly IMapper _iMapper;
//        private readonly IConfiguration _configuration;
//        private readonly OpenSqlService _openSql;
//        //ID不存在, Deleted是True, 是不是公司的活動, 賣出幾張票
//        private string canEdit =
//               @"  SELECT 
//                    Showing.ID,
//                    COUNT(ticket.ID) AS Sales
//                FROM Showing
//                LEFT JOIN TicketType ON Showing.ID = TicketType.SHOWING
//                LEFT JOIN Ticket ON TicketType.ID = Ticket.TicketType
//                WHERE  @showingID = Showing.ID
//                AND 0 = Showing.Deleted 
//                AND showing.Activity IN 
//                    (SELECT ID FROM Activity
//                        WHERE Company = @userid)
//                GROUP BY Showing.ID;";

//        public ShowingController(
//            BookingContext bookingContext,
//            IMapper mapper,
//            IConfiguration configuration,
//            OpenSqlService openSql)
//        {
//            _bookingContext= bookingContext;
//            _iMapper = mapper;
//            _configuration = configuration;
//            _openSql = openSql;
//        }

        
//        [HttpGet("GetShowing/{showingID}"),Authorize(Roles ="company")]
//        public ActionResult<Showing> Get(int showingID)
//        {
//            var result = _bookingContext.Showings
//                            .Where(s => s.Id == showingID)
//                            .Include(s => s.TicketTypes)
//                            .ThenInclude(t => t.Tickets)
//                            .SingleOrDefault();

//            if (result == null)
//                return NotFound();
//            return Ok(result);
//        }

//        //[HttpPost("addShowing"),Authorize(Roles ="company")]
//        //public ActionResult<Showing> Post([FromBody] AddShowingRequest request)
//        //{
//            //bool parseSuccess = Byte.TryParse(User.FindFirstValue("companyID"), out byte companyID);
//            //if (!parseSuccess)
//            //{
//            //    return Forbid();
//            //}

//            //string add = @"
//            //    IF @activity IN (SELECT Activity.ID FROM Activity WHERE @userID = Company)
//	           //     begin
//	           //     INSERT INTO Showing(Activity, ShowingStart, ShowingEnd, SalesStart, SalesEnd, Deleted, LastModified)
//	           //     VALUES(@activity,@showingStart,@showingEnd,@salesStart,@salesEnd,'0', GETDATE());
//	           //     SELECT 1;
//	           //     end
//            //    ELSE 
//	           //     SELECT 0;";
            
//            //Dictionary<string, object> IParams = new Dictionary<string, object>();
//            //try
//            //{
//            //    DateTime salesStart = DateTime.Parse(request.SalesStart);
//            //    DateTime salesEnd = DateTime.Parse(request.SalesEnd);
//            //    DateTime showingStart = DateTime.Parse(request.ShowingStart);
//            //    DateTime showingEnd = DateTime.Parse(request.ShowingEnd);

                
//            //    IParams.Add("@userid", companyID);
//            //    IParams.Add("@activity", request.Activity);
//            //    IParams.Add("@salesStart", salesStart);
//            //    IParams.Add("@salesEnd", salesEnd);
//            //    IParams.Add("@showingStart", showingStart);
//            //    IParams.Add("@showingEnd", showingEnd);
//            //}
//            //catch
//            //{
//            //    return BadRequest("操作錯誤");
//            //}

//            //int scalarReturn = _openSql.ExecuteSQLScalar(add, IParams);
//            //if ( scalarReturn == 1)
//            //{
//            //    return Ok("新增成功");
//            //}
//            //return BadRequest("操作錯誤"+scalarReturn);

//        //}

//        /// <summary>
//        /// 
//        /// 若場次被訂購 則只可更改購票開始與結束時間
//        /// 
//        /// </summary>
//        /// <param name="request"></param>
//        /// <returns></returns>
//        [HttpPost("updateShowing"), Authorize(Roles ="company")]
//        public ActionResult<string> Update(UpdateShowingRequest request)
//        {
//            bool parseSuccess = Byte.TryParse(User.FindFirstValue("companyID"), out byte companyID);
//            if (!parseSuccess)
//            {
//                return Forbid();
//            }

//            Dictionary<string, object> IParams = new Dictionary<string, object>();
//            IParams.Add("@userid", companyID);
//            IParams.Add("@showingID", request.ID);

//            bool hasRows = _openSql.ExecuteQuery(canEdit, IParams, out DataTable dt);
//            if (!hasRows) return BadRequest("操作錯誤");

//            int sales = (int)(dt.Rows[0].ItemArray[1] ?? 0);

//            // 場次被訂購 可以延長場次
//            if(sales > 0)
//            {
//                try
//                {
//                    DateTime salesStart = DateTime.Parse(request.SalesStart);
//                    DateTime salesEnd = DateTime.Parse(request.SalesEnd);

//                    IParams.Add("@salesStart", salesStart);
//                    IParams.Add("@salesEnd", salesEnd);
//                }
//                catch
//                {
//                    return BadRequest("操作錯誤ss");
//                }

//                string update =@"Update Showing
//                Set SalesStart = @salesStart, SalesEnd = @salesEnd, LastModified = GETDATE()
//                WHERE @showingID = Showing.ID;";

//                _openSql.ExecureNonQuery(update, IParams);

//                return Ok("訂購時間修改成功");
//            }

//            //場次未被訂購 可以修改場次時間

//            try
//            {
//                DateTime salesStart = DateTime.Parse(request.SalesStart);
//                DateTime salesEnd = DateTime.Parse(request.SalesEnd);
//                DateTime showingStart = DateTime.Parse(request.ShowingStart);
//                DateTime showingEnd = DateTime.Parse(request.ShowingEnd);

//                IParams.Add("@salesStart", salesStart);
//                IParams.Add("@salesEnd", salesEnd);
//                IParams.Add("@showingStart", showingStart);
//                IParams.Add("@showingEnd", showingEnd);
//            }
//            catch
//            {
//                return BadRequest("操作錯誤");
//            }

//            string updateAll = @"Update Showing
//                                Set SalesStart = @salesStart, 
//                                SalesEnd = @salesEnd, 
//                                ShowingStart = @showingStart,
//                                ShowingEnd = @showingEnd,
//                                LastModified = GETDATE()
//                                WHERE @showingID = Showing.ID";
//            _openSql.ExecureNonQuery(updateAll, IParams);
//            return Ok("修改成功");
//        }


//        [HttpPost("test"),Authorize]
//        public ActionResult<string> Test()
//        {
//            // if don't have companyID then user.findFirstValue return null
//            // null,space,empty return false and 0 from Byte.TryParse 
//            bool byteSuccessful = Byte.TryParse(User.FindFirstValue("companyID"), out byte result);  
//            if (byteSuccessful) {
//                return Ok(result);
//            }
//            return BadRequest(result);
//        }

//        /// <summary>
//        /// 1.透過ID刪除場次
//        /// 2.只能刪除自己公司的活動場次  (判斷roles是company 且 companyID是該活動場次主辦)
//        /// 3.已經被刪除的不可被重複刪除
//        /// 4.有被訂購的場次不可以刪除
//        /// 
//        /// return badrequest 操作錯誤
//        /// 1.場次不存在
//        /// 2.沒有場次修改權限
//        /// 3.已經被刪除
//        /// 
//        /// badrequest 被訂購場次不可刪除
//        /// 
//        /// </summary>
//        /// <param name="request">
//        /// {
//        ///     ID: 要求修改的場次序號
//        /// }
//        /// </param>
//        /// <returns></returns>
//        [HttpPost("deleteShowing"), Authorize(Roles ="company")]
//        public ActionResult<string> Delete(DeleteShowingRequest request)
//        {
//            bool parseSuccesss = Byte.TryParse(User.FindFirstValue("companyID"), out byte companyID);
//            if (!parseSuccesss) 
//            {
//                return Forbid();
//            }
            

//            //sql參數
//            Dictionary<string, object> IParams = new Dictionary<string, object>();
//            IParams.Add("@userid", companyID);
//            IParams.Add("@showingID", request.ID);

//            bool drHasRows = _openSql.ExecuteQuery(canEdit, IParams, out DataTable dt);

//            if (drHasRows)
//            {
//                int sales = (int)(dt.Rows[0].ItemArray[1] ?? -1);
                
//                if (sales == 0)
//                {
//                    string updateQuery =
//                    @"UPDATE Showing
//                    SET Deleted = 1, LastModified = GETDATE()
//                    WHERE @showingID = Showing.ID
//                    and 0 = Deleted;";

//                    _openSql.ExecureNonQuery(updateQuery, IParams);

//                    return Ok("已刪除");
//                }
//                return BadRequest("被訂購場次不可刪除");
//            }
//            return BadRequest("操作錯誤");
//        }
//    }
//}
