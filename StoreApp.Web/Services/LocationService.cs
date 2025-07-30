using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using StoreApp.Data.Location;

namespace StoreApp.Web.Services
{
    public class LocationService
    {
        private readonly IWebHostEnvironment _env;

        public LocationService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<List<CityJsonModel>> GetAllCitiesAsync()
        {
            var path = Path.Combine(_env.WebRootPath, "data/locations/sehirler.json");

            if (!File.Exists(path))
                throw new FileNotFoundException("Şehir dosyası bulunamadı", path);

            var json = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<List<CityJsonModel>>(json) ?? new();
        }


        public async Task<List<DistrictJsonModel>> GetDistrictsByProvinceIdAsync(string sehir_id)
        {
            var path = Path.Combine(_env.WebRootPath, "data/locations/ilceler.json");
            var json = await File.ReadAllTextAsync(path);
            var allDistricts = JsonSerializer.Deserialize<List<DistrictJsonModel>>(json);
            return allDistricts.Where(d => d.sehir_id == sehir_id).ToList();
        }

        public async Task<List<NeighborhoodJsonModel>> GetNeighborhoodsByDistrictIdAsync(string ilce_id)
        {
            var neighborhoods = new List<NeighborhoodJsonModel>();

            for (int i = 1; i <= 4; i++)
            {
                var path = Path.Combine(_env.WebRootPath, $"data/locations/mahalleler-{i}.json");
                if (!File.Exists(path)) continue;

                var json = await File.ReadAllTextAsync(path);
                var list = JsonSerializer.Deserialize<List<NeighborhoodJsonModel>>(json);
                neighborhoods.AddRange(list.Where(m => m.ilce_id == ilce_id));
            }

            return neighborhoods;
        }
    }
}
