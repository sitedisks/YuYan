using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YuYan.Domain.Database
{
    [Table("Session")]
    public class tbSession : tbTbase
    {
        [Key, Column("Id")]
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Expiry { get; set; }
        public string IPAddress { get; set; }

        [ForeignKey("UserId")]
        public tbUser tbUser { get; set; }
    }
}
