using StockManagement.Application.Features.Users.DTOs;
using System.Collections.Generic;

namespace StockManagement.Application.Features.Roles.DTOs
{
    public class RoleDto
    {
        public int Id { get; set; }                 
        public string Name { get; set; }            

       
        public List<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
