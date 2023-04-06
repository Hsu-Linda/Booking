using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;

namespace Booking.Filters
{
    public class ExceptionHandler:ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if(context.Exception is SqlException)
            {
                var apiError = new ResponseDto
                {
                    code = 400,
                    message = context.Exception.Message
                };
                context.Result = new JsonResult(apiError) { StatusCode = 400 };
            }
        }
    }
}
