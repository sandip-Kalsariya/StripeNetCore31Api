using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeNetCoreApi.DTO.ResponseDTO
{
    public class UserProfileDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string JwtToken { get; set; }
        public string Role { get; set; }
        public int? RoleId { get; set; }
        public string Customer_id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Birthdate { get; set; }
        public string Phone_Number { get; set; }
        public string Avatar { get; set; }
    }
}
