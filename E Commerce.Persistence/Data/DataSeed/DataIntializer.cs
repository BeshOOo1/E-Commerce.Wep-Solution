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

namespace E_Commerce.Persistence.Data.DataSeed
{
    public class DataIntializer : IDataIntializer
    {
        private readonly StoreDbConext _dbConext;

        public DataIntializer(StoreDbConext dbConext)
        {
            _dbConext = dbConext;
        }
        public void Intialize()
        {
            try
            {
                var HasProducts = _dbConext.Products.Any();
                var HasBrands = _dbConext.ProductBrands.Any();
                var HasTypes = _dbConext.ProductTypes.Any();

                if (HasBrands && HasTypes  && HasProducts) return;
               
                if(!HasBrands)
                {
                    SeedDataFromJson<ProductBrand, int>("brands.json", _dbConext.ProductBrands);
                }
                if(!HasTypes)
                {
                    SeedDataFromJson<ProductType, int>("types.json", _dbConext.ProductTypes);
                }
                _dbConext.SaveChanges();
                if(!HasProducts)
                {
                    SeedDataFromJson<Product, int>("products.json", _dbConext.Products);
                }
                _dbConext.SaveChanges();

            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Data Seed is Faild {ex}");
            }
        }

        private void SeedDataFromJson<T ,TKey>(string FileName,DbSet<T> dbset) where T :BaseEntity<TKey>
        {
            // D:\BackEnd.Net\Course\API\Project_API\E Commerce.Wep Solution\E Commerce.Persistence\Data\DataSeed\JSONFiles\brands.json

            var FilePath = @"..\E Commerce.Persistence\Data\DataSeed\JSONFiles\" + FileName;

            if (!File.Exists(FilePath)) throw new FileNotFoundException($"File {FileName} is not Exist");

            try
            {
                using var dataStreams = File.OpenRead(FilePath);

                var data = JsonSerializer.Deserialize<List<T>>(dataStreams, new JsonSerializerOptions 
                {
                    PropertyNameCaseInsensitive = true
                });

                if(data is not null)
                {
                    dbset.AddRange(data);
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
