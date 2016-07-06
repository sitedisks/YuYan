using System;
using YuYan.Domain.Enum;

namespace YuYan.Domain.DTO
{
    public class dtoImage
    {
        public Guid ImageId { get; set; }
        public ImageType ImageType { get; set; }
        public Guid UserId { get; set; }
        public string FileName { get; set; }
        public string Uri { get; set; }
        public int RefId { get; set; }
    }
}
