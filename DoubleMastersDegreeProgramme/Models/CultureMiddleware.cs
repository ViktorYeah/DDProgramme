using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DoubleMastersDegreeProgramme.Models
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var lang = context.Request.Query["lang"].ToString();
            if (!string.IsNullOrEmpty(lang))
            {
                try
                {
                    CultureInfo.CurrentCulture = new CultureInfo(lang);
                    CultureInfo.CurrentUICulture = new CultureInfo(lang);
                }
                catch (CultureNotFoundException) { }
            }
            await _next.Invoke(context);
        }
    }

    public static class CultureExtensions
    {
        public static IApplicationBuilder UseCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CultureMiddleware>();
        }
    }
}
