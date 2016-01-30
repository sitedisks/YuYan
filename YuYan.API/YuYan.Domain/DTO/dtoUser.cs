using System;

namespace YuYan.Domain.DTO
{
    public class dtoUser: dtoTbase
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string IPAddress { get; set; }
        public string  Password { get; set; }
    }
}
