using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using portal.Domain;
using portal.Security.Identity;
using portal.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace portal.UnitTest
{
    public class PortalServiceWebApplicationFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var portalDBContextService = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<PortalDbContext>));
                if (portalDBContextService != null)
                    services.Remove(portalDBContextService);

                services.AddDbContext<PortalDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryPortalDB");
                });

                var securityDBContext = services.SingleOrDefault(
                  d => d.ServiceType ==
                      typeof(DbContextOptions<SecurityDbContext>));
                if (portalDBContextService != null)
                    services.Remove(securityDBContext);

                services.AddDbContext<SecurityDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemorySecurityDB");
                });

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    using (var appContext = scope.ServiceProvider.GetRequiredService<PortalDbContext>())
                    {
                        try
                        {
                            appContext.Database.EnsureCreated();
                        }
                        catch (Exception ex)
                        {
                            //Log errors or do anything you think it's needed
                            throw;
                        }
                    }
                    using (var appContext = scope.ServiceProvider.GetRequiredService<SecurityDbContext>())
                    {
                        try
                        {
                            appContext.Database.EnsureCreated();
                        }
                        catch (Exception ex)
                        {
                            //Log errors or do anything you think it's needed
                            throw;
                        }
                    }
                }
            });
        }
    }
}