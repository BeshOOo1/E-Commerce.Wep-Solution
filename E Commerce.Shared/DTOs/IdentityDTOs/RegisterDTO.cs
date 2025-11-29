using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.IdentityDTOs
{
    public record RegisterDTO(string Email, string DisplayName, string UserName, string Passward, [Phone] string PhoneNumber);
}
