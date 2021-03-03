using CityInfo.API;
using CityInfo.API.Controllers;
using CityInfo.API.Services;
using CityInfo.BL.ExternalServices;
using CityInfo.BL.UnitOfWork;
using CityInfo.Data.EF;
using CityInfo.Data.Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CityInfo.Test
{
	[TestClass]
	public class CityUnitTest
	{
		private DependencyResolverHelpercs _serviceProvider;
		private ICityService cityService;
		private IUnitOfWork unitOfWork;
		private IExternalServices externalServices;
		private IConfiguration configuration;
		private CityInfoDbContext dbContext;

		public CityUnitTest() {
			InitContext();
			var webHost = WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
			_serviceProvider = new DependencyResolverHelpercs(webHost);
		}

		public void InitContext() {
			var builder = new DbContextOptionsBuilder<CityInfoDbContext>().UseInMemoryDatabase("CityDb");

			var context = new CityInfoDbContext(builder.Options);
			var city = new City { Country = "US", CountryCode2 = "US", CountryCode3 = "USA", Currency = "Doller", Description = "Fox name is nick.", EstablishedOn = new DateTime(1779, 01, 01), Id = 1, Name = "Dallas", Population = 987654321, Rating = 3, State = "Oklahoma" };
			var obj = context.City.Find(1);
			if (obj == null) {
				context.City.Add(city);
			}
			context.SaveChanges();
			dbContext = context;
		}

		[TestInitialize]
		public void Setup() {
			configuration = _serviceProvider.GetService<IConfiguration>();
			externalServices = new ExternalServices(configuration);
			unitOfWork = new UnitOfWork(dbContext);
			cityService = new CityService(unitOfWork, externalServices);
		}

		[TestMethod]
		public void TestGetAll() {
			CityController controller = new CityController(cityService);

			var response = controller.Get() as OkObjectResult;

			Assert.IsNotNull(response);
			Assert.AreEqual(response.StatusCode, 200);
			Assert.IsInstanceOfType(response.Value, typeof(List<API.Model.City>));
		}

		[TestMethod]
		public void TestGetSingle() {
			CityController controller = new CityController(cityService);

			var response = controller.Get("Dallas") as OkObjectResult;

			Assert.IsNotNull(response);
			Assert.AreEqual(response.StatusCode, 200);
			Assert.IsInstanceOfType(response.Value, typeof(API.Model.City));
		}
	}

	public class DependencyResolverHelpercs
	{
		private readonly IWebHost _webHost;

		/// <inheritdoc />
		public DependencyResolverHelpercs(IWebHost WebHost) => _webHost = WebHost;

		public T GetService<T>() {
			using (var serviceScope = _webHost.Services.CreateScope()) {
				var services = serviceScope.ServiceProvider;
				try {
					var scopedService = services.GetRequiredService<T>();
					return scopedService;
				}
				catch (Exception e) {
					Console.WriteLine(e);
					throw;
				}
			};
		}
	}
}
