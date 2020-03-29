namespace remikub.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using remikub.Domain;

    public class HttpExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        ///     Logs and handles exception translation to a <see cref="List{ErrorResult}"/> to be returned to the client.
        /// </summary>
        /// <param name="context"> The context on which the exception occured. </param>
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is RemikubException)
            {
                var ex = (RemikubException)context.Exception;

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(new ErrorContent(ex.Code.ToString(), ex.Details)) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }

        public class ErrorContent
        {
            public ErrorContent(string code, object? details)
            {
                Code = code;
                Details = details;
            }

            public string Code { get; }
            public object? Details { get; }
        }
    }
}