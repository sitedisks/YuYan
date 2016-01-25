
using System.Collections.Generic;
using YuYan.Domain.Enum;
namespace YuYan.Domain.DTO
{
    public class dtoSurveyQuestion : dtoTbase
    {
        public int QuestionId { get; set; }
        public int SurveryId { get; set; }
        public string Question { get; set; }
        public int QuestionOrder { get; set; }
        public QuestionType QuestionType { get; set; }

        public virtual ICollection<dtoSurveyQuestionItem> dtoItems { get; set; }
    }
}
