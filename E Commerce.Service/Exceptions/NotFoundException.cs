using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service.Exceptions
{
    public class NotFoundException(string message) : Exception(message)
    {

    }
    public sealed class ProductNotFoundException(int Id) : NotFoundException($"Product with Id {Id} is not Found")
    {

    }
    public sealed class BasketNotFoundException(string Id) : NotFoundException($"Basket with Id {Id} is not Found")
    {

    }
}
