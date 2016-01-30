
using YuYan.Domain.Enum;
namespace YuYan.Domain.DTO
{
    public class dtoUserProfile: dtoUser
    {
        public UserRole UserRole { get; set; }
        public string Username { get; set; }
        public string IPAddress { get; set; }
        public string StreetNo { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
