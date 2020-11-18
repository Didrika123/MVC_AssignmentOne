using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AssignmentOne
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection(); //when redirecting when using authentication this is needed or auth will be lost?
            app.UseStaticFiles();
            /*app.UseDefaultFiles();*/
            app.UseRouting();
            app.UseSession();
           // app.UseAuthentication();// forauthentication

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute( //Note: it doesnt seem to like mixing these custom routes and specified attribute routes in controller since it overwrites the default routing pattern?
                    name: "MySpecialRule", 
                    pattern: "lolol/{action=FeverCheck}",
                    defaults: new { controller = "Odd"}
                    );
                endpoints.MapControllerRoute(
                    name: "feverCheck", 
                    pattern: "/FeverCheck",
                    defaults: new { controller = "Odd", action = "FeverCheck" }
                    );
                endpoints.MapDefaultControllerRoute();
                /*endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });*/
            });
        }
    }
}
