using ApartmentFinishingServices.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApartmentFinishingServices.Repository.Data
{
    public class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext dbcontext)
        {
            if (dbcontext.Categories.Count() == 0)
            {
                var categoriesData = File.ReadAllText("../ApartmentFinishingServices.Repository/Data/DataSeed/categories.json");
                var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);
                if (categories?.Count > 0)
                {
                    foreach (var category in categories)
                    {
                        dbcontext.Set<Category>().Add(category);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }
            if (dbcontext.Services.Count() == 0)
            {
                var servicesData = File.ReadAllText("../ApartmentFinishingServices.Repository/Data/DataSeed/services.json");
                var services = JsonSerializer.Deserialize<List<Services>>(servicesData);
                if (services?.Count > 0)
                {
                    foreach (var service in services)
                    {
                        dbcontext.Set<Services>().Add(service);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }
            if (dbcontext.Cities.Count() == 0)
            {
                var citiesData = File.ReadAllText("../ApartmentFinishingServices.Repository/Data/DataSeed/city.json");
                var cities = JsonSerializer.Deserialize<List<City>>(citiesData);
                if (cities?.Count > 0)
                {
                    foreach (var city in cities)
                    {
                        dbcontext.Set<City>().Add(city);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }

        }

    }
}
