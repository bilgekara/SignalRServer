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
        /* CORS Politikalar� : Sunucular�m�za bilinmedik clientlardan gelen istekleri 
         *  taray�c�lar taraf�ndan direk engelleyen bir g�venlik �nlemi
         *  Biz bu g�venlik �nlemini bilin�li olarak d���r�yoruz
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
            ////ben gelen b�t�n metodlara izin veriyorum
            //// belirli bir politika olu�turuyoruz
            //// politikay� kullan�yoruz

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
                /* �u endpoint �u hub'a ba�lans�n
                 * client'�n subscr�be olabilece�i hub'�n endpointini bildirmemiz gerekiyor
                 * bundan sonra uygulamada myhub endpointine istek,ba�lant� talebi 
                 * geliyosa buradaki hub taraf�ndan kar��lanacakt�r
                 */

                endpoints.MapControllers();
                endpoints.MapHub<MyHub>("/myhub");

            });
        }
    }
}
