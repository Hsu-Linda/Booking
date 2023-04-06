using AutoMapper;
using Booking.Dto;
using Booking.Dtos;
using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        // 宣告的全域變數
        private readonly BookingContext _bookingContext;
        private readonly IMapper _iMapper;
        private readonly AuthService _authService;

        public CompanyController(
            BookingContext bookingContext,
            IMapper iMapper,
            AuthService authService
        )
        {
            _bookingContext = bookingContext;
            _iMapper = iMapper;
            _authService = authService;
        }

        private string GenerateSalt()
        {
            return "addSa;t";
        }

        // GET api/<CompanyController>/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            Company? companyInfo = _bookingContext.Companies.Where(c => c.CompanyId == id).SingleOrDefault();

            if (companyInfo == null)
                return NotFound("找不到");

            IEnumerable<Activity> activities = null;
            
            bool torF = int.TryParse(User.FindFirstValue("memberID"), out int memberID);

            //if (torF)
            //{
            //    var a = _bookingContext.Likes.Where(l => l.MemberId == memberID && l.CompanyId == companyInfo.CompanyId);
            //    var b = _bookingContext.Activities.Join(a, ac => ac.ActivityId, like => like.ActivityId, (ac, like) => {
            //        ActivityWithLikeDto activityWithLikeDto = _iMapper.Map<Activity, ActivityWithLikeDto>(ac);
            //        return (
            //           _iMapper.Map<Like, ActivityWithLikeDto>((Like)a, activityWithLikeDto)


            //                        );
            //    };
            //}

            //------------
            activities = _bookingContext.Activities.Where(a => a.Company == companyInfo.CompanyId);
            
            var resultObject = new { companyInfo = _iMapper.Map<QueryCompanyInfoResponseDto>(companyInfo), activities};
            
            
            //物件序列化 (有序列化 和 沒序列化有差嗎??)
            var result = JsonConvert.SerializeObject(resultObject, Formatting.Indented);

            return  Ok(result);
        }

        
        
    }
}
