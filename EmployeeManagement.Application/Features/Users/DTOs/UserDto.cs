using System;

namespace StockManagement.Application.Features.Users.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }                    
        public string Username { get; set; }           
        public int RoleId { get; set; }                 
    }
}
