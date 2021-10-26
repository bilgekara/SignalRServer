using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRServer.Business;
using SignalRServer.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer
{
    public class Startup
    {
        /* CORS Politikalarý : Sunucularýmýza bilinmedik clientlardan gelen istekleri 
         *  tarayýcýlar tarafýndan direk engelleyen bir güvenlik önlemi
         *  Biz bu güvenlik önlemini bilinçli olarak düþürüyoruz
         */
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
           {
               options.AddPolicy("CorsPolicy", builder => builder
                .WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

           });
            
            //services.AddCors(options => options.AddDefaultPolicy(policy =>
            //policy.WithOrigins("http://localhost:4200")
            //.AllowAnyMethod()
            //.AllowAnyHeader()
            //.AllowCredentials()
            //.SetIsOriginAllowed(origin => true)
            //));
            ////ben gelen bütün metodlara izin veriyorum
            //// belirli bir politika oluþturuyoruz
            //// politikayý kullanýyoruz

            //services.AddTransient<MyBusiness>();

            services.AddSignalR();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        
        {
            //Debug.WriteLine("debug calisti");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                /* þu endpoint þu hub'a baðlansýn
                 * client'ýn subscrýbe olabileceði hub'ýn endpointini bildirmemiz gerekiyor
                 * bundan sonra uygulamada myhub endpointine istek,baðlantý talebi 
                 * geliyosa buradaki hub tarafýndan karþýlanacaktýr
                 */

                endpoints.MapControllers();
                endpoints.MapHub<MyHub>("/myhub");

            });
        }
    }
}
