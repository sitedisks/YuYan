using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YuYan.Domain.Database
{
    [Table("SurveyShare")]
    public class tbSurveyShare
    {
        [Key, Column("Id")]
        public long SurveyShareId { get; set; }
        public int SurveyId { get; set; }
        public string IPAddress { get; set; }
        public DateTime VisitedDate { get; set; }
    }
}
