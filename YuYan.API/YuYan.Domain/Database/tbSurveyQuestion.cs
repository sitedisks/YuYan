using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YuYan.Domain.Database
{
    [Table("SurveyQuestion")]
    public class tbSurveyQuestion: tbTbase
    {
        [Key, Column("Id")]
        public int QuestionId { get; set; }
        public int SurveyId { get; set; }
        public string Question { get; set; }
        public int QuestionOrder { get; set; }
        public int QuestionType { get; set; }

        [ForeignKey("SurveyId")]
        public virtual tbSurvey tbSuvery { get; set; }
        public virtual ICollection<tbSurveyQuestionItem> tbSurveyQuestionItems { get; set; }
        
    }
}
