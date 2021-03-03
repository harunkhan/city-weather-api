using AutoMapper;
using CityInfo.BL.ExternalServices;
using CityInfo.BL.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace CityInfo.API.Services
{
	public class CityService : ICityService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IExternalServices _externalServices;

		public CityService(IUnitOfWork unitOfWork, IExternalServices externalServices) {
			_unitOfWork = unitOfWork;
			_externalServices = externalServices;
		}
		public int AddCity(Model.CityModel city) {
			int id = 0;
			using (var scope = new TransactionScope()) {
				var config = new MapperConfiguration(cfg => cfg.CreateMap<Model.CityModel, Data.Entities.City>());
				var mapper = config.CreateMapper();

				var newCity = mapper.Map<Model.CityModel, Data.Entities.City>(city);

				_unitOfWork.CityRepository.Insert(newCity);
				_unitOfWork.Save();
				scope.Complete();
				id = newCity.Id;
			}
			return id;
		}

		public bool DeleteCity(int id) {
			var success = false;
			if (id > 0) {
				using (var scope = new TransactionScope()) {
					var city = _unitOfWork.CityRepository.GetByID(id);
					if (city != null) {
						_unitOfWork.CityRepository.Delete(city);
						_unitOfWork.Save();
						scope.Complete();
						success = true;
					}
				}
			}
			return success;
		}

		public List<Model.City> GetCity() {
			List<Model.City> cities = null;

			var response = _unitOfWork.CityRepository.GetAll().ToList();
			if (response.Any()) {
				var config = new MapperConfiguration(cfg => cfg.CreateMap<Data.Entities.City, Model.City>());
				var mapper = config.CreateMapper();

				cities = mapper.Map<List<Data.Entities.City>, List<Model.City>>(response);
			}

			foreach (var city in cities) {
				city.CountryDetails = GetCountryDetails(city.CountryCode2);

				city.WeatherDetails = GetWeatherDetails(city.Name);
			}

			return cities;
		}

		public Model.City GetCity(string name) {
			Model.City city = null;

			var response = _unitOfWork.CityRepository.GetFirst(m => m.Name == name);
			if (response != null) {
				var config = new MapperConfiguration(cfg => cfg.CreateMap<Data.Entities.City, Model.City>());
				var mapper = config.CreateMapper();

				city = mapper.Map<Data.Entities.City, Model.City>(response);

				city.CountryDetails = GetCountryDetails(city.CountryCode2);

				city.WeatherDetails = GetWeatherDetails(city.Name);
			}
			return city;
		}

		public bool UpdateCity(int id, Model.CityModel city) {
			var success = false;
			if (city != null) {
				using (var scope = new TransactionScope()) {
					var dbCity = _unitOfWork.CityRepository.GetByID(id);

					if (dbCity != null) {
						//Updating City Id from Db again to avoid any issue.
						var Id = dbCity.Id;
						var config = new MapperConfiguration(cfg => cfg.CreateMap<Model.CityModel, Data.Entities.City>());
						var mapper = config.CreateMapper();

						dbCity = mapper.Map<Model.CityModel, Data.Entities.City>(city);
						dbCity.Id = Id;
					}
					_unitOfWork.CityRepository.Update(dbCity);
					_unitOfWork.Save();
					scope.Complete();
					success = true;
				}
			}
			return success;
		}

		private Model.WeatherDetails GetWeatherDetails(string name) {
			Model.WeatherDetails weather = null;

			if (!string.IsNullOrWhiteSpace(name)) {
				var weatherDetails = _externalServices.GetWeather(name);

				var config = new MapperConfiguration(cfg => {
					cfg.CreateMap<Data.Entities.Coord, Model.Coord>();
					cfg.CreateMap<Data.Entities.Weather, Model.Weather>();
					cfg.CreateMap<Data.Entities.Main, Model.Main>();
					cfg.CreateMap<Data.Entities.Wind, Model.Wind>();
					cfg.CreateMap<Data.Entities.Clouds, Model.Clouds>();
					cfg.CreateMap<Data.Entities.Sys, Model.Sys>();
					cfg.CreateMap<Data.Entities.WeatherDetails, Model.WeatherDetails>()
						.ForMember(s => s.weather, c => c.MapFrom(m => m.weather));
				});

				var mapper = config.CreateMapper();

				weather = mapper.Map<Data.Entities.WeatherDetails, Model.WeatherDetails>(weatherDetails);
			}
			return weather;
		}

		private Model.CountryDetails GetCountryDetails(string countryCode2) {
			Model.CountryDetails countryDetails = null;

			if (!string.IsNullOrWhiteSpace(countryCode2)) {
				var countries = _externalServices.GetCountry(countryCode2);

				var config = new MapperConfiguration(cfg => {
					cfg.CreateMap<Data.Entities.Currency, Model.Currency>();
					cfg.CreateMap<Data.Entities.Language, Model.Language>();
					cfg.CreateMap<Data.Entities.Translations, Model.Translations>();
					cfg.CreateMap<Data.Entities.RegionalBloc, Model.RegionalBloc>();
					cfg.CreateMap<Data.Entities.CountryDetails, Model.CountryDetails>()
						.ForMember(s => s.currencies, c => c.MapFrom(m => m.currencies))
						.ForMember(s => s.languages, c => c.MapFrom(m => m.languages))
						.ForMember(s => s.regionalBlocs, c => c.MapFrom(m => m.regionalBlocs));
				});
				var mapper = config.CreateMapper();

				countryDetails = mapper.Map<Data.Entities.CountryDetails, Model.CountryDetails>(countries);
			}
			return countryDetails;
		}
	}
}
