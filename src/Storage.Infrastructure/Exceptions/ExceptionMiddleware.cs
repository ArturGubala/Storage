﻿using Humanizer;
using Microsoft.AspNetCore.Http;
using Storage.Core.Exceptions;

namespace Storage.Infrastructure.Exceptions
{
    internal class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(exception, context);
            }
        }

        private async Task HandleExceptionAsync(Exception exception, HttpContext context)
        {
            var (statusCode, error) = exception switch
            {
                CustomBadRequestException => (StatusCodes.Status400BadRequest,
                    new Error(exception.GetType().Name.Underscore().Replace("_exception", string.Empty), exception.Message)),
                CustomNotFoundException => (StatusCodes.Status404NotFound,
                    new Error(exception.GetType().Name.Underscore().Replace("_exception", string.Empty), exception.Message)),
                _ => (StatusCodes.Status500InternalServerError, new Error("error", "There was an error."))
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(error);
        }

        private record Error(string Code, string Reason);
    }
}
