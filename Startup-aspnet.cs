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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IMemoryCache cache)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                var redirectHost = "ngrok.bernardgabon.com";
                var redirect = "redirect";
                var secret = "YOUR_API_KEY";

                endpoints.MapGet("/", async context =>
                {
                    if (!string.IsNullOrEmpty(context.Request.Query["apikey"]))
                    {
                        if(context.Request.Query["apikey"].ToString() != secret)
                            await context.Response.WriteAsync("Forbidden.");
                        else if(!string.IsNullOrEmpty(context.Request.Query["r"]))
                        {
                            var r = context.Request.Query["r"].ToString();
                            if (!r.StartsWith("http")) r = "https://" + r;
                            var entryOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove);
                            cache.Set(redirect, r, entryOptions);
                            await context.Response.WriteAsync(redirectHost + " is now redirected to " + r);
                        }
                        else
                        {
                            cache.Remove(redirect);
                            await context.Response.WriteAsync("Redirect has been cleared.");
                        }
                    }
                    else if (cache.Get(redirect) != null)
                        context.Response.Redirect(cache.Get(redirect).ToString());                    
                    else
                        await context.Response.WriteAsync("Append ?apikey=yourkey&r=forwardingURL to set new redirect.");
                });
            });
        }
    }
}
