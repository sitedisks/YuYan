using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace YuYan.Data.Database
{
    [Table("Survey")]
    public class tbSurvey
    {
        [Key]
        [Column("Id")]
        public int SurveyId { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
    }
}
