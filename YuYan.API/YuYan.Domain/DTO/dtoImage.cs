using System;
using YuYan.Domain.Enum;

namespace YuYan.Domain.DTO
{
    public class dtoImage
    {
        public Guid ImageId { get; set; }
        public ImageType ImageType { get; set; }
        public int RefId { get; set; }
    }
}
