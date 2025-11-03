using ECommerce.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // GET : BaseURL/api/Product/{id}
        [HttpGet("{Id}")]
        public ActionResult<Product> GetElement(int Id)
        {
            if (Id == 1)
                return NotFound();
            return new Product() { Id = Id  , Name = "Mobile"};
        }
        [HttpGet]
        // GET: BaseURL/api/Product
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return new List<Product>();
        }
        [HttpPost]
        // POST : BaseURL/api/Product
        public ActionResult<Product> AddProduct(Product item)
        {
            return item;
        }
        //[HttpPost]
        // POST : BaseURL/api/Product ××
        [HttpPut]
        public ActionResult<Product> UpdateProduct(Product item)
        {
            return item;
        }
        [HttpDelete]
        // Delete : BaseURL/api/Product
        public ActionResult<Product> DeleteProduct(Product item)
        {
            return item;
        }
    }
}
