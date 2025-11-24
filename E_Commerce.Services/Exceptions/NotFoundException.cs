using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Exceptions
{
    public abstract class NotFoundException(string message):Exception(message)
    {   }
    public sealed class ProductNotFoundException(int id)
        : NotFoundException($"Product With Id:{id} Is NotFound")
    { }
    public sealed class BasketNotFoundException (string basketid)
        :NotFoundException($"Basket With Id:{basketid} Is NotFound")
    { }
   
}
