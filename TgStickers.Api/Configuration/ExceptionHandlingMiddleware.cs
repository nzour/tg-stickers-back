using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TgStickers.Application.Exceptions;

namespace TgStickers.Api.Configuration
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings();

        static ExceptionHandlingMiddleware()
        {
            JsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                context.Response.Headers.Add("Content-Type", "application/json");

                context.Response.StatusCode = exception is AbstractHandledException
                    ? StatusCodes.Status400BadRequest
                    : StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsync(CreateErrorResponseString(exception));
            }
        }

        private static string CreateErrorResponseString(Exception ex) =>
            JsonConvert.SerializeObject(new { ErrorType = ex.GetType().Name, ex.Message }, JsonSettings);
    }
}