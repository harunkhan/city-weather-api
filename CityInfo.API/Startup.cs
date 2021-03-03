using CityInfo.API.Services;
using CityInfo.BL.ExternalServices;
using CityInfo.BL.UnitOfWork;
using CityInfo.Data.EF;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CityInfo.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services.AddControllers();
			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "CityInfo API", Version = "v1" });
			});
			services.AddDbContext<CityInfoDbContext>(options => {
				options.UseSqlServer(Configuration.GetConnectionString("CityDBContext"));
			});
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IExternalServices, ExternalServices>();
			services.AddScoped<ICityService, CityService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}
			app.UseSwagger();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
			});

			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "CotyInfo API V1");
			});
		}
	}
}
