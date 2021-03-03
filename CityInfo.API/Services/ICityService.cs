using CityInfo.API.Model;
using System.Collections.Generic;

namespace CityInfo.API.Services
{
	public interface ICityService
	{
		List<City> GetCity();
		City GetCity(string name);
		int AddCity(CityModel city);
		bool UpdateCity(int id, CityModel city);
		bool DeleteCity(int id);
	}
}
