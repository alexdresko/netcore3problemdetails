using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace hellangcore3
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProblemDetails(options =>
            {
                // This is the default behavior; only include exception details in a development environment.
                options.IncludeExceptionDetails = ctx => Environment.IsDevelopment();

                // This will map TeapotException to the 418 I'm a teapot status code
                options.Map<TeapotException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status418ImATeapot));

                // This will map NotImplementedException to the 501 Not Implemented status code.
                options.Map<NotImplementedException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status501NotImplemented));

                // This will map HttpRequestException to the 503 Service Unavailable status code.
                options.Map<HttpRequestException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status503ServiceUnavailable));

                // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
                // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
                options.Map<Exception>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError));
            });
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseIfElse(IsUIRequest, UIExceptionMiddleware, NonUIExceptionMiddleware);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void NonUIExceptionMiddleware(IApplicationBuilder app)
        {
            // Hellang.Middleware.ProblemDetails package
            app.UseProblemDetails();
        }

        private void UIExceptionMiddleware(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/home/error");
            }

            app.UseStatusCodePagesWithRedirects("/home/error/{0}");
        }

        private static bool IsUIRequest(HttpContext httpContext)
        {
            var requestPath = httpContext.Request.Path;
            return requestPath == "/" || !requestPath.StartsWithSegments($"/api", StringComparison.OrdinalIgnoreCase);
        }
    }

    public static class ApplicationBuilderExts
    {
        public static void UseIfElse(this IApplicationBuilder app, Func<HttpContext, bool> predicate, Action<IApplicationBuilder> ifConfiguration, Action<IApplicationBuilder> elseConfiguration)
        {
            app.UseWhen(predicate, ifConfiguration);
            app.UseWhen(ctx => !predicate(ctx), elseConfiguration);
        }
    }
}
