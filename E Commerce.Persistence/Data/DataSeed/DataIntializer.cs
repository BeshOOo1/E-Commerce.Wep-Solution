using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Commerce.Persistence.Data.DataSeed
{
    public class DataIntializer : IDataIntializer
    {
        private readonly StoreDbConext _dbConext;

        public DataIntializer(StoreDbConext dbConext)
        {
            _dbConext = dbConext;
        }
        public async Task IntializeAsync()
        {
            try
            {
                var HasProducts = await _dbConext.Products.AnyAsync();
                var HasProductBrands = await _dbConext.ProductBrands.AnyAsync();
                var HasProductTypes = await _dbConext.ProductTypes.AnyAsync();

                if (HasProducts && HasProductBrands && HasProductTypes) return;

                if (!HasProductTypes)
                {
                    await SeedDataFromJsonAsync<ProductType, int>("types.json", _dbConext.ProductTypes);
                }

                if (!HasProductBrands)
                {
                   await SeedDataFromJsonAsync<ProductBrand, int>("brands.json", _dbConext.ProductBrands);
                }

                 await _dbConext.SaveChangesAsync();
                if(!HasProducts)
                {
                   await SeedDataFromJsonAsync<Product, int>("products.json", _dbConext.Products);
                }
                await _dbConext.SaveChangesAsync();

            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Data Seed is Faild {ex}");
            }
        }

        private async Task SeedDataFromJsonAsync<T ,TKey>(string FileName,DbSet<T> dbset) where T :BaseEntity<TKey>
        {
            // D:\BackEnd.Net\Course\API\Project_API\E Commerce.Wep Solution\E Commerce.Persistence\Data\DataSeed\JSONFiles\brands.json

            var FilePath = @"..\E Commerce.Persistence\Data\DataSeed\JSONFiles\" + FileName;

            if (!File.Exists(FilePath)) throw new FileNotFoundException($"File {FileName} is not Exist");

            try
            {
                using var DataStreams = File.OpenRead(FilePath);

                var Data = await JsonSerializer.DeserializeAsync<List<T>>(DataStreams, new JsonSerializerOptions 
                {
                    PropertyNameCaseInsensitive = true
                });

                if(Data != null)
                {
                    dbset.AddRange(Data);
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Error while Reading Json File : {ex}");
                return;
            }


        }
    }
}
