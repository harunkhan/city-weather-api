using System;
using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Model
{
	public class City
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string State { get; set; }

		public string Country { get; set; }

		public int Rating { get; set; }

		public DateTime EstablishedOn { get; set; }

		public long Population { get; set; }

		public string CountryCode2 { get; set; }

		public string CountryCode3 { get; set; }

		public string Currency { get; set; }

		public CountryDetails CountryDetails { get; set; }

		public WeatherDetails WeatherDetails { get; set; }

		public City() {
			CountryDetails = new CountryDetails();
			WeatherDetails = new WeatherDetails();
		}
	}

	public class CityModel
	{
		[Required, MaxLength(50)]
		public string Name { get; set; }
		[Required, MaxLength(200)]
		public string Description { get; set; }
		[Required, MaxLength(50)]
		public string State { get; set; }
		[Required, MaxLength(50)]
		public string Country { get; set; }
		[Required, Range(1, 5)]
		public int Rating { get; set; }
		[Required, DataType(DataType.Date)]
		public DateTime EstablishedOn { get; set; }
		[Required]
		public long Population { get; set; }
		[Required, MaxLength(2)]
		public string CountryCode2 { get; set; }
		[Required, MaxLength(3)]
		public string CountryCode3 { get; set; }
		[Required, MaxLength(20)]
		public string Currency { get; set; }
	}
}
