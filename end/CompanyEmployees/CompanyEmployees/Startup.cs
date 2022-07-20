using CompanyEmployees.Extensions;
using CompanyEmployees.Middlewares;
using Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using System.IO;

namespace CompanyEmployees
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{			
			services.ConfigureSqlContext(Configuration);
			services.ConfigureRepository();
			services.AddAutoMapper(typeof(Startup));
			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env
			
			)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
			defaultFilesOptions.DefaultFileNames.Clear();
			defaultFilesOptions.DefaultFileNames.Add("test.html");

			app.UseDefaultFiles(defaultFilesOptions);
			app.UseStaticFiles();
		
			app.UseRouting();
			app.UseAuthorization();
			app.UseMiddleware<RequestResponseLoggingMiddleware>();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
