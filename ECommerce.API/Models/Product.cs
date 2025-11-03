using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Models
{
    public class Product
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage = " Name is required.")]
        [MaxLength(3,ErrorMessage ="Max Length 3 ")]
        public string Name { get; set; }
    }
}
