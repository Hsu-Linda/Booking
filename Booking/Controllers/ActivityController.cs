using AutoMapper;
using Booking.Dtos;
using Booking.Exceptions;
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
    [ExceptionHandler]
    public class ActivityController : ControllerBase
    {
        private readonly ActivityService _activityService;
        private readonly AuthService _authService;

        public ActivityController(
            ActivityService activityService,
            AuthService authService)
        {
            _activityService = activityService;
            _authService = authService;
        }

        [HttpGet("getAll"), Authorize(Roles = "company")]
        public ActionResult<List<Activity>> GetAll()
        {
            if (!Byte.TryParse(User.FindFirstValue("companyID"), out byte companyID))
            {
                throw new LoseClaimsException();
            }
            return Ok(_activityService.GetAll(companyID));
        }

        [HttpGet("search")]
        public ActionResult<List<Activity>> Search(SearchCondition condition)
        {
            if (condition == null)
            {
                return new List<Activity>();
            }
            IEnumerable<ActivityInfoResponse> list = _activityService.SearchActivities(condition);
            if (list.FirstOrDefault() == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpGet("getFromCompany/{id}"), Authorize(Roles = "company")]
        public ActionResult<Activity> GetFromCompany(short id)
        {
            if (!Byte.TryParse(User.FindFirstValue("companyID"), out byte companyID))
            {
                throw new LoseClaimsException();
            }
            return Ok(_activityService.GetInfoFromCompany(id, companyID));
        }

        [HttpGet("get/{id}")]
        public ActionResult<ActivityInfoResponse> GetInfo(short id)
        {
            ActivityInfoResponse? result = _activityService.GetInfo(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }

        [HttpPost("add"), Authorize(Roles = "company")]
        public ActionResult Add(AddActivityRequestDto activity)
        {
            if (!Byte.TryParse(User.FindFirstValue("companyID"), out byte companyID))
            {
                throw new LoseClaimsException();
            }
            
            if(_activityService.Add(activity, companyID, out byte errorCode))
            {
                return Ok();
            };

            if(1 == errorCode)
            {
                return BadRequest("活動名稱不可相同");
            };
            
            return BadRequest();
        }

        [HttpPost("update"), Authorize(Roles = "company")]
        public ActionResult<string> Update(UpdateActivityRequest activity)
        {
            if (!Byte.TryParse(User.FindFirstValue("companyID"), out byte companyID))
            {
                throw new LoseClaimsException();
            }
            _activityService.Update(activity, companyID);
            return Ok();
        }

        [HttpPost("delete"), Authorize(Roles = "company")]
        public ActionResult Delete(deleteActivityRequest activity)
        {
            if (!Byte.TryParse(User.FindFirstValue("companyID"), out byte companyID))
            {
                throw new LoseClaimsException();
            }
            _activityService.Delete(activity.Id, companyID);
            return Ok();
        }
    }
}
