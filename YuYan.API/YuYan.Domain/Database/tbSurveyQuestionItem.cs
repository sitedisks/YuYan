using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YuYan.Domain.Database
{
    [Table("SurveyQuestionItem")]
    public class tbSurveyQuestionItem: tbTbase
    {
        [Key, Column("Id")]
        public int QuestionItemId { get; set; }
        public int QuestionId { get; set; }
        public string ItemDescription { get; set; }
        public int ItemOrder { get; set; }
        public Nullable<int> Score { get; set; }
        public Nullable<int> GotoQuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public virtual tbSurveyQuestion tbSurveyQuestion { get; set; }
    }
}
