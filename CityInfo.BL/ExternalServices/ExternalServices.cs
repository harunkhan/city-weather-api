using CityInfo.Data.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace CityInfo.BL.ExternalServices
{
	public class ExternalServices : IExternalServices
	{
		private readonly IConfiguration _configuration;

		#region properties
		public string CountryURL { get => _configuration["CountryURL"].ToString(); }
		public string WeatherURL { get => _configuration["WeatherURL"].ToString(); }
		public string AppID { get => _configuration["AppId"].ToString(); }
		#endregion


		// inject the IConfiguration service and store it in a field
		public ExternalServices(IConfiguration configuration) {
			_configuration = configuration;
		}


		public CountryDetails GetCountry(string code2) {
			string url = string.Format(CountryURL, code2);
			CountryDetails country = null;

			country = CallWebService(url, country);

			return country;
		}

		public WeatherDetails GetWeather(string name) {
			string url = string.Format(WeatherURL, name, AppID);

			WeatherDetails weather = null;

			weather = CallWebService(url, weather);

			return weather;
		}

		private T CallWebService<T>(string url, T t) {
			try {
				WebRequest request = WebRequest.Create(url);
				request.Method = "GET";
				var response = request.GetResponse();

				if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK) {
					using (Stream dataStream = response.GetResponseStream()) {
						// Open the stream using a StreamReader for easy access.
						StreamReader reader = new StreamReader(dataStream);

						string data = reader.ReadToEnd();
						// Read the content.
						t = JsonSerializer.Deserialize<T>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
					}
				}
			}
			catch (Exception) {
				//Ignore Exception.
			}

			return t;
		}
	}
}
