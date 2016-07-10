using System.ComponentModel.DataAnnotations.Schema;

namespace YuYan.Domain.Database
{
    [Table("ImageType")]
    public class tbImageType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageGroup { get; set; }
        public string FileFolder { get; set; }
    }
}
