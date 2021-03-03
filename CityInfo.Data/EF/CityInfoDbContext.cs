using CityInfo.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Data.EF
{
	public class CityInfoDbContext : DbContext
	{
		public CityInfoDbContext(DbContextOptions<CityInfoDbContext> options) : base(options) {
		}
		public DbSet<City> City { get; set; }
	}
}
