using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgrokRedirector
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMemoryCache cache)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {                
                var redirect = "redirect";

                endpoints.MapGet("/", async context =>
                {
                    if (!string.IsNullOrEmpty(context.Request.Query["r"]) && !string.IsNullOrEmpty(context.Request.Query["apikey"]))
                    {
                        if(context.Request.Query["apikey"].ToString() != "ngrokredirector123")
                            await context.Response.WriteAsync("Forbidden.");
                        else
                        {
                            var r = context.Request.Query["r"].ToString();
                            if (!r.StartsWith("http")) r = "https://" + r;
                            var entryOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove);
                            cache.Set(redirect, r, entryOptions);
                            await context.Response.WriteAsync("ngrok.bernardgabon.com is now redirected to " + r);
                        }
                    }
                    else if (cache.Get(redirect) != null)
                        context.Response.Redirect(cache.Get(redirect).ToString());                    
                    else
                        await context.Response.WriteAsync("Append ?apikey=yourkey&r=forwardingURL to set the redirect. Append /c to clear.");
                });

                endpoints.MapGet("/c", async context =>
                {
                    cache.Remove(redirect);
                    await context.Response.WriteAsync("Redirect has been cleared.");
                });
            });
        }
    }
}
