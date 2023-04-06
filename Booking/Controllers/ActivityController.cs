using AutoMapper;
using Booking.Dtos;
using Booking.Filters;
using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Booking.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly BookingContext _bookingContext;
        private readonly IMapper _iMapper;
        private readonly OpenSqlService _openSql;
        private readonly IConfiguration _configuration;
        private readonly ActivityService _activityService;
        
        public ActivityController(
            BookingContext bookingContext,
            IMapper mapper,
            OpenSqlService openSql,
            IConfiguration configuration,
            ActivityService activityService)
        {
            _bookingContext = bookingContext;
            _iMapper = mapper;
            _openSql = openSql;
            _configuration = configuration;
            _activityService = activityService;
        }
        // GET: api/<ActivityController>
        // 查詢活動
        //[HttpGet]
        //[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Activity>))]
        //[ProducesResponseType((int)HttpStatusCode.NotFound, Type= typeof(string))]
        //public ActionResult<IEnumerable<Activity>> GetKeyWord( string? search)
        //{


        //    if (string.IsNullOrWhiteSpace(search))
        //        return Ok(_bookingContext.Activities);

        //    var result = _bookingContext.Activities
        //                    .Where(activity => activity.ActivityName.Contains(search));
                     
        //    if (!result.Any())
        //        return NotFound("查詢不到");
        //    return Ok(result); 
        //}

        // GET api/<ActivityController>/5
        // 查看活動詳細資訊
        //[HttpGet("{id}")] 
        //public ActionResult<Activity> Get(int id)
        //{
        //    Boolean idExist = _bookingContext.Activities.Any( a => a.ActivityId == id );
        //    if (!idExist)
        //        return NotFound("查無此 activity id：" + id + "訊息");
            
        //    var activityInfo = _bookingContext.Activities
        //                    .Include(y => y.Showings)
        //                    .Where(a => a.ActivityId == id)
        //                    .SingleOrDefault();  //SingleOrDefault 指查詢的到一筆 或 查無回傳null
        //    if (activityInfo == null)
        //        return NotFound("查無此 activity id：" + id + "訊息");

        //    return Ok(activityInfo);
        //}


        /// <summary>
        /// 收藏活動
        /// </summary>
        /// <param name="activityID"></param>
        /// <param name="likeOrUnlik"></param>
        /// <returns></returns>
        [HttpGet("save"), Authorize(Roles ="member")]
        public ActionResult<string> Save(short activityID,bool likeOrUnlik=true)
        {

            // User.FindFirstValue("memberID");  if user don't have memberID claim, then return null
            // tryParse null, empty, non-numeric will return false and the result is 0
            if (!Int16.TryParse(User.FindFirstValue("memberID"), out short memberID))   
            {
                return BadRequest("用戶id錯誤，請重新登入");
            }

            if (likeOrUnlik)
            {

                Like newLike = new Like { LikeId = 0, ActivityId = activityID, MemberId = memberID };
                _bookingContext.Likes.Add(newLike);
                _bookingContext.SaveChanges();


                return Ok(_bookingContext.Likes);
            }
            Like? likea = _bookingContext.Likes.Where(l => l.MemberId == memberID && l.ActivityId == activityID).FirstOrDefault();
            if (likea == null) { return BadRequest("查無此收藏資料"); }

            _bookingContext.Likes.Remove(likea);
            _bookingContext.SaveChanges();

            return Ok(_bookingContext.Likes);
        }

        [HttpGet("AllSave"), Authorize(Roles = "member")]
        public ActionResult<IEnumerable<Like>> AllSave ()
        {
            bool a = int.TryParse(User.FindFirstValue("memberID"), out int memberID);
            if (!a) return BadRequest("會員失敗");

            var result = _bookingContext.Likes
                            .Where(l => l.MemberId == memberID);
            
            return Ok(result);
        }

        /// <summary>
        /// 新增活動
        /// 
        /// 至少為該活動設定一個場次
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("add"), Authorize(Roles = "company")]
        public ActionResult<Activity> Post([FromBody] AddActivityRequestDto request)
        {   
            if (byte.TryParse(User.FindFirstValue("companyID"), out byte companyID))
            {
                string query = @"INSERT INTO Activity
                                VALUES (@name, @user, @photoURL, @city, @district, @address, @showingStart, @showingEnd, @salesStart, @salesEnd, 1, GETDATE());";

                Dictionary<string, Tuple<object,SqlDbType>> IParams = new Dictionary<string, Tuple<object,SqlDbType>>();
                IParams.Add("@name", new Tuple<object, SqlDbType>(request.Name, SqlDbType.NVarChar));
                IParams.Add("@user", new Tuple<object, SqlDbType>(companyID, SqlDbType.TinyInt));
                IParams.Add("@photoURL", new Tuple<object, SqlDbType>(request.PhotoUrl, SqlDbType.NVarChar));
                IParams.Add("@city", new Tuple<object, SqlDbType>(request.City,SqlDbType.TinyInt));
                IParams.Add("@district", new Tuple<object, SqlDbType>(request.District,SqlDbType.SmallInt));
                IParams.Add("@address", new Tuple<object, SqlDbType>(request.Address,SqlDbType.NVarChar));
                
                IParams.Add("@salesStart", new Tuple<object,SqlDbType>(request.SalesStart, SqlDbType.DateTime));
                IParams.Add("@salesEnd", new Tuple<object, SqlDbType>(request.SalesEnd, SqlDbType.DateTime));
                IParams.Add("@showingStart", new Tuple<object, SqlDbType>(request.ShowingStart,SqlDbType.DateTime));
                IParams.Add("@showingEnd", new Tuple<object, SqlDbType>(request.ShowingEnd, SqlDbType.DateTime));
                

                _openSql.ExecureNonQuery(query, IParams);
                Ok();
            }
            return Forbid("操作錯誤");
        }


        [ExceptionHandler]
        [HttpPost("addNew"),Authorize(Roles ="company")]
        public ActionResult<int> addnew(Activity req)
        {
            if (byte.TryParse(User.FindFirstValue("companyID"), out byte companyID))
            {
                _activityService.Add(req, companyID);
                return Ok();
                //try
                //{

                //}
                //catch (SqlException agex)
                //{
                //    return BadRequest(agex.Message);
                //}

                //if (result) return Ok();
                //return BadRequest(res);
            }
            return Forbid();
        }
        /// <summary>
        /// 
        /// badrequest
        /// 1.該筆資料已刪除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("update"),Authorize(Roles ="company")]
        public ActionResult<string> Update (UpdateActivityRequest request)
        {
            //是自己家的活動
            //當產品尚未上線 則可修改 (場次購買日期大於今天日期)
            //修改
            //暫時隱藏 = TRUE || 尚未賣出任何一張票券
            //修改
            //不可修改

            if(!Int16.TryParse(User.FindFirstValue("companyID"), out short companyID))
            {
                return Forbid("沒有權限");
            }


            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("BookingDatabase")))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("usp_update_activity", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@id", SqlDbType.SmallInt).Value = request.Id;
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = request.Name;
                    cmd.Parameters.Add("@PhotoUrl", SqlDbType.NVarChar).Value = request.PhotoUrl;
                    cmd.Parameters.Add("@city", SqlDbType.TinyInt).Value = request.City;
                    cmd.Parameters.Add("@district", SqlDbType.SmallInt).Value = request.District;
                    cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = request.Address;
                    cmd.Parameters.Add("@after", SqlDbType.Bit).Value = 0;
                    cmd.Parameters.Add("@userID", SqlDbType.SmallInt).Value = companyID;

                    SqlParameter retParameter = cmd.Parameters.Add("@return_value", SqlDbType.SmallInt);
                    retParameter.Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteNonQuery();

                    if (0 == (int)retParameter.Value)
                    {
                        return Ok("修改成功");
                    }
                    return BadRequest("修改失敗");
                }
            }

        }


        [HttpPost("updateActivity"),Authorize(Roles ="company")]
        public ActionResult<string> Updateactivity(UpdateActivityRequest request)
        {
            //提取USERID
            if (!Int16.TryParse(User.FindFirstValue("companyID"),out short companyID)) return Forbid();

            //獲得場次資訊：是否刪除、售出張數、是否已開始販售
            string query = @"SELECT TOP 1 * FROM ActivityView
                             WHERE @ID = ID and @user = Company;";

            Dictionary<string, object> IParams = new Dictionary<string, object>();
            IParams.Add("@ID",request.Id);
            IParams.Add("@user", companyID);

            object hasRows =_openSql.ExecuteQuery(query, IParams, out DataTable dt);

            //沒有此編號 或 已刪除
            if (null == hasRows || false == (bool)hasRows) return BadRequest("1：id錯誤");
            
            
            try
            {
                int day_diff = (int)dt.Rows[0]["DAYDIFF"];
                int sales = (int)dt.Rows[0]["SALES"];
                byte city = (byte)dt.Rows[0]["city"];
                DateTime showingStart = (DateTime)dt.Rows[0]["ShowingStart"];
                DateTime showingEnd = (DateTime)dt.Rows[0]["ShowingEnd"];


                if (day_diff<0 && sales>0 &&(city != request.City ||
                      showingStart < request.ShowingStart ||
                      showingEnd > request.ShowingEnd))
                    
                {
                    return BadRequest("2: 操作錯誤，不可修改");
                }

                string updateQuery =
                        @"UPDATE Activity
                        SET PhotoURL = @photoURL, District = @district, Address =@address, NAME = @name, City = @city,
                        SalesStart=@salesStart, SalesEnd =@salesEnd, ShowingStart =@showingStart, ShowingEnd=@showingEnd
                        WHERE ID = @id;";

                Dictionary<string, Tuple<object, SqlDbType>> updateParams = new Dictionary<string, Tuple<object, SqlDbType>>();
                updateParams.Add("@name", new Tuple<object, SqlDbType>(request.Name, SqlDbType.NVarChar));
                updateParams.Add("@user", new Tuple<object, SqlDbType>(companyID, SqlDbType.TinyInt));
                updateParams.Add("@photoURL", new Tuple<object, SqlDbType>(request.PhotoUrl, SqlDbType.NVarChar));
                updateParams.Add("@city", new Tuple<object, SqlDbType>(request.City, SqlDbType.TinyInt));
                updateParams.Add("@district", new Tuple<object, SqlDbType>(request.District, SqlDbType.SmallInt));
                updateParams.Add("@address", new Tuple<object, SqlDbType>(request.Address, SqlDbType.NVarChar));

                updateParams.Add("@salesStart", new Tuple<object, SqlDbType>(request.SalesStart, SqlDbType.DateTime));
                updateParams.Add("@salesEnd", new Tuple<object, SqlDbType>(request.SalesEnd, SqlDbType.DateTime));
                updateParams.Add("@showingStart", new Tuple<object, SqlDbType>(request.ShowingStart, SqlDbType.DateTime));
                updateParams.Add("@showingEnd", new Tuple<object, SqlDbType>(request.ShowingEnd, SqlDbType.DateTime));

                _openSql.ExecureNonQuery(updateQuery, updateParams);
                return Ok();
            }
            
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpPost("delete"),Authorize(Roles ="company")]
        public ActionResult<string> Delete(deleteActivityRequest request)
        {
            string query = @"if exists(SELECT * FROM ActivityView WHERE @ID= ID AND 0= SALES)
                            begin
                                update Activity
                                set Active = 0, LastModified= getdate()
                                where @id= id
                                select 0
                            end
                            else begin select 1 end;";

            Dictionary<string, Tuple<object,SqlDbType>> IParams = new Dictionary<string, Tuple<object,SqlDbType>>();
            IParams.Add("@ID", new Tuple<object, SqlDbType>(request.Id,SqlDbType.SmallInt));

            int result =_openSql.ExecuteSQLScalar(query, IParams);
            if (result == 0)
            {
                return Ok();
            }
            return BadRequest("操作錯誤");
            
        }
    }
}
