using System;
using System.Linq;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CurrentIp.Storage.DependencyInjection;
using CurrentIp.Web.Providers;

namespace CurrentIp.Web {
  public class Startup {
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment) {
      _environment = environment;
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) {
      services.AddHealthChecks();
      if (_environment.EnvironmentName == "IntegrationTests") {
        services.AddDistributedMemoryCache();
      }
      else if (Configuration.GetSection("REDIS").Exists()) {
        services.AddStackExchangeRedisCache(options => { options.Configuration = Configuration.GetSection("REDIS").Value; });
      }
      else {
        services.AddDistributedMemoryCache();
      }

      services.AddTransient<IRdpFileProvider, RdpFileProvider>();
      services
        .AddPageStorage()
        .AddControllers()
        .AddFluentValidation(opt => opt.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseEndpoints(endpoints => {
        endpoints.MapHealthChecks("/api/health");
        endpoints.MapControllers();
      });
    }
  }
}