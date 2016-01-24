
using System.Collections.Generic;
namespace YuYan.Domain.DTO
{
    public class dtoSurveyQuestion
    {
        public int QuestionId { get; set; }
        public int SurveryId { get; set; }
        public string Question { get; set; }
        public int QuestionOrder { get; set; }
        public int QuestionType { get; set; }

        public virtual ICollection<dtoSurveyQuestionItem> dtoItems { get; set; }
    }
}
