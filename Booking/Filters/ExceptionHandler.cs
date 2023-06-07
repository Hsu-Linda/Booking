using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Booking.Exceptions;
using Booking.Dtos;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace Booking.Filters
{
    public class ExceptionHandler : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is SqlException)
            {
                var apiError = new ResponseDto
                {
                    status = 500,
                    message = context.Exception.Message
                };
                context.Result = new JsonResult(apiError) { StatusCode = 500 };
            }
            if (context.Exception is InvalidCastException)
            {
                var apiError = new ResponseDto
                {
                    status = 500,
                    message = context.Exception.Message
                };
                context.Result = new JsonResult(apiError) { StatusCode = 500 };
            }
            if (context.Exception is ActivitySettingException)
            {

                var apiError = new ResponseDto
                {
                    status = 400,
                    message = context.Exception.Message,
                    title = context.Exception.GetType().Name

                };
                context.Result = new JsonResult(apiError) { StatusCode = 400 };
            }

            if (context.Exception is LoseClaimsException)
            {
                var apiError = new ResponseDto
                {
                    status = 401,
                    message = context.Exception.Message,
                    title = context.Exception.GetType().Name

                };
                context.Result = new JsonResult(apiError) { StatusCode = 401 };
            }
            if (context.Exception is TicketTypeSettingException)
            {
                var apiError = new ResponseDto
                {
                    status = 400,
                    message = context.Exception.Message,
                    title = context.Exception.GetType().Name

                };
                context.Result = new JsonResult(apiError) { StatusCode = 400 };
            }
            if (context.Exception is OrderOperationException)
            {
                var apiError = new ResponseDto
                {
                    status = 400,
                    message = context.Exception.Message,
                    title = context.Exception.GetType().Name

                };
                context.Result = new JsonResult(apiError) { StatusCode = 400 };
            }
        }
    }
}
