using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.AuthenticationDTOs
{
    public record AddressDTO(
    string FirstName,
    string LastName,
    string City,
    string Country,
    string Street
);

}
