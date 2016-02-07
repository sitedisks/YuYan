using System;

namespace YuYan.Domain.DTO
{
    public class dtoSession : dtoTbase
    {
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Expiry { get; set; }
        public string IPAddress { get; set; }
    }
}
