using Booking.Dto;
using Booking.Dtos;
using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly BookingContext _bookingContext;

        public AuthController(
            AuthService authService,
            BookingContext bookingContext
        )
        {
            _authService = authService;
            _bookingContext = bookingContext;
        }

        [HttpPost("registerMember")]
        public ActionResult<string> RegisterMember(AddMemberRequestDto registerRequest)
        {
            if (_authService.FindMemberAccountExist(registerRequest.Email, out Member? member)
                ) return BadRequest("email已註冊過帳號");

            _authService.RegisterMember(registerRequest);
            return Ok("註冊成功");
        }

        [HttpPost("loginMember")]
        public ActionResult<string> LoginMember(QueryLoginRequestDto loginRequest)
        {
            if (!_authService.FindMemberAccountExist(loginRequest.Email, out Member? memberFromDB))
                return BadRequest("您尚未註冊");

            if (!_authService.VerifyMemberLogin(loginRequest, memberFromDB))
                return BadRequest("帳號密碼錯誤");

            string token = _authService.CreateToken(member: memberFromDB, role: "member", company: null);
            return Ok(token);
        }

        [HttpPost("registerCompany")]
        public ActionResult<Company> RegisterCompany(AddCompanyRequestDto companyRequest)
        {
            if (_authService.FindCompanyAccountExist(companyRequest.Email, out Company? company)
                ) return BadRequest("email已註冊過帳號");

            _authService.RegisterCompany(companyRequest);
            return Ok("註冊成功");
        }


        [HttpPost("loginCompany")]
        public ActionResult<string> LoginCompany(QueryLoginRequestDto loginRequest)
        {
            if (!_authService.FindCompanyAccountExist(loginRequest.Email, out Company? companyFromDB))
                return BadRequest("您尚未註冊");

            if (!_authService.VerifyCompanyLogin(loginRequest, companyFromDB))
                return BadRequest("帳號密碼錯誤");

            string token = _authService.CreateToken(member: null, role: "company", company: companyFromDB);
            return Ok(token);
        }
    }
}
