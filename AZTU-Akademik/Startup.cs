using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace AZTU_Akademik
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
            {
                //x.LoginPath = "/Login";
                x.Events.OnRedirectToLogin = content =>
                {
                    content.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                };
                x.Cookie.HttpOnly = false;
            });

            //services.AddCors(options => options.AddDefaultPolicy(builder => builder.WithOrigins("///").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));
            services.AddCors(options => options.AddPolicy("AMASPolicy", x =>
            {
                x.AllowAnyOrigin().
                AllowAnyMethod().
                AllowAnyHeader();
            }));




            services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "AZTU-AKADEMIK" });
            });



        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = "Aztu-Akademik";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Aztu-Akademik");
            });

            app.UseRouting();
            app.UseCors("AMASPolicy");


            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Assets")),
                RequestPath = "/Assets"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
