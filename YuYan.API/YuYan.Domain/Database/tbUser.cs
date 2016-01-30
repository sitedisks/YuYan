using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YuYan.Domain.Enum;

namespace YuYan.Domain.Database
{
    [Table("User")]
    public class tbUser: tbTbase
    {
        public tbUser() {
            tbSessions = new HashSet<tbSession>();
            tbSurveys = new HashSet<tbSurvey>();
        }
        [Key, Column("Id")]
        public Guid UserId { get; set; }
        [Column("UserRoleId")]
        public UserRole UserRole { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string IPAddress { get; set; }
        public string StreetNo { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        #region relationship
        public virtual ICollection<tbSession> tbSessions { get; set; }
        public virtual ICollection<tbSurvey> tbSurveys { get; set; }
        #endregion

    }
}
