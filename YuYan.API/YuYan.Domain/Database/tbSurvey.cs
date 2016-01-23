using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YuYan.Domain.Database
{
    [Table("Survey")]
    public class tbSurvey: tbTbase
    {
        [Key, Column("Id")]
        public int SurveyId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        [Column("UpdatedByUserId")]
        public Guid UserId { get; set; }

    }
}
