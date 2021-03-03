using CityInfo.Data.Entities;

namespace CityInfo.BL.ExternalServices
{
	public interface IExternalServices
	{
		string CountryURL { get; }
		string WeatherURL { get; }
		string AppID { get; }
		CountryDetails GetCountry(string code2);
		WeatherDetails GetWeather(string name);
	}
}
