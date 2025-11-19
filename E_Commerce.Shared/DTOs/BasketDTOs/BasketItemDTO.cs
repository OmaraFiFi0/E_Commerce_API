using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.BasketDTOs
{
    public record BasketItemDTO (
        int Id,
        string? ProductName ,
        string? PictureUrl,
        [Range(0 , double.MaxValue)]
        decimal Price ,
        [Range(0,100)]
        int Quantity
        );
   
}
