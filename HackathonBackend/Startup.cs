namespace HackathonBackend;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Startup
{
    public Startup(IConfiguration Configuration)
    {
        this.Configuration = Configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection Services)
    {
        Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        Services.AddEndpointsApiExplorer();
        Services.AddSwaggerGen();
        Services.AddDbContext<DestinyContext>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder App, IWebHostEnvironment Env)
    {
        // Configure the HTTP request pipeline.

        if (Env.IsDevelopment())
        {
            App.UseDeveloperExceptionPage();
        }

        App.UseSwagger();
        App.UseSwaggerUI();
        App.UseHttpsRedirection();
        App.UseRouting();
        App.UseAuthorization();
        App.UseEndpoints(Endpoints =>
        {
            Endpoints.MapControllers();
        });
    }
}
