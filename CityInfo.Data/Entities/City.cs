using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.Data.Entities
{
	public class City
	{
		public City() {
			EstablishedOn = DateTime.Now;
		}

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is required."), MaxLength(50, ErrorMessage = "City name cannot exceed 50 characters.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Description is required."), MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
		public string Description { get; set; }

		[Required(ErrorMessage = "State is required"), MaxLength(50, ErrorMessage = "State name cannot exceed 50 charectors.")]
		public string State { get; set; }

		[Required(ErrorMessage = ""), MaxLength(50, ErrorMessage = "Country name cannot exceed 50 characters.")]
		public string Country { get; set; }

		[Range(1, 5, ErrorMessage = "Rating can be minimum 1 and maximum 5.")]
		public int Rating { get; set; }

		[Required(ErrorMessage = "CreatedOn is required.")]
		public DateTime EstablishedOn { get; set; }

		[Required(ErrorMessage = "Population is required.")]
		public long Population { get; set; }

		[Required(ErrorMessage = "Country Code 2 is required."), MaxLength(2, ErrorMessage = "Only 2 digits")]
		public string CountryCode2 { get; set; }

		[Required(ErrorMessage = "Country Code 3 is required."), MaxLength(2, ErrorMessage = "Only 3 digits")]
		public string CountryCode3 { get; set; }

		[Required(ErrorMessage = "Currency is required.")]
		public string Currency { get; set; }
	}
}
