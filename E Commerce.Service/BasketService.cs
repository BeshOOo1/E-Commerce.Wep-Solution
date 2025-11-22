using AutoMapper;
using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities.BasketModule;
using E_Commerce.Service.Abstracion;
using E_Commerce.Service.Exceptions;
using E_Commerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basket)
        {
            var CustomerBasket = _mapper.Map<CustomerBasket>(basket);
            var CreatedOrUpdatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(CustomerBasket);
            return _mapper.Map<BasketDTO>(CreatedOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string basketId) => await _basketRepository.DeleteBasketAsync(basketId);

        public async Task<BasketDTO> GetBasketAsync(string basketId)
        {
            var Basket = await _basketRepository.GetBasketAsync(basketId);
            if (Basket == null)
                throw new BasketNotFoundException(basketId);
            return _mapper.Map<BasketDTO>(Basket);
        }
    }
}
