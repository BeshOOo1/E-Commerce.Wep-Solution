using AutoMapper;
using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Service.Abstracion;
using E_Commerce.Service.Specifications;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var Brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDTO>>(Brands);
        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductAsync(ProductQueryParams queryParams)
        {
            var Repo = _unitOfWork.GetRepository<Product, int>();
            var Spec = new ProductWithTypeAndBrandSpecification(queryParams);
            var Products = await Repo.GetAllAsync(Spec);
            var DataToResult = _mapper.Map<IEnumerable<ProductDTO>>(Products);
            var CountOfReturnedData = DataToResult.Count();
            var CountSpec = new ProductCountSpecification(queryParams);
            var CountOfAllProducts = await Repo.CountAsync(CountSpec); 
            return new PaginatedResult<ProductDTO>(queryParams.PageIndex, CountOfReturnedData, CountOfAllProducts, DataToResult);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var Types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDTO>>(Types);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var Spec = new ProductWithTypeAndBrandSpecification(id);

            var Product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(id);
            return _mapper.Map<ProductDTO>(Product);
        }

    }
}
