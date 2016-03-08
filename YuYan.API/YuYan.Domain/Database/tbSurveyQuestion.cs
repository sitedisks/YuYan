using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YuYan.Domain.Enum;

namespace YuYan.Domain.Database
{
    [Table("SurveyQuestion")]
    public class tbSurveyQuestion: tbTbase
    {
        public tbSurveyQuestion() {
            tbSurveyQuestionItems = new HashSet<tbSurveyQuestionItem>();
        }
        [Key, Column("Id")]
        public int QuestionId { get; set; }
        public int SurveyId { get; set; }
        public string Question { get; set; }
        public int QuestionOrder { get; set; }
        public QuestionType QuestionType { get; set; }

        [ForeignKey("SurveyId")]
        public virtual tbSurvey tbSurvey { get; set; }
        public virtual ICollection<tbSurveyQuestionItem> tbSurveyQuestionItems { get; set; }
        
    }
}
