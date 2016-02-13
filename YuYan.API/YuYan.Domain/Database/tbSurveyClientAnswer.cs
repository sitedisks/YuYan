using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YuYan.Domain.Database
{
    [Table("SurveyClientAnswer")]
    public class tbSurveyClientAnswer
    {
        [Key,Column("Id")]
        public long AnswerId { get; set; }
        [Column("SurveyClientId")]
        public long ClientId { get; set; }
        public int QuestionId { get; set; }
        public int QuestionItemId { get; set; }
        public bool IsChecked { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("ClientId")]
        public virtual tbSurveyClient tbSurveyClient { get; set; }
    }
}
