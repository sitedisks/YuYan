using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YuYan.Domain.Database
{
    [Table("Image")]
    public class tbImage : tbTbase
    {
        [Key, Column("Id")]
        public Guid ImageId { get; set; }
        public int ImageType { get; set; }
        [Column("UploaderUserId")]
        public Guid UserId { get; set; }
        public string FileName { get; set; }
        public string Uri { get; set; }
        public int RefId { get; set; }

        [ForeignKey("ImageType")]
        public virtual tbImageType tbImageType { get; set; }
    }
}
