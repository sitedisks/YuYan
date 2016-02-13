
namespace YuYan.Domain.DTO
{
    public class dtoSurveyClientAnswer
    {
        public long AnswerId { get; set; }
        public long ClientId { get; set; }
        public int QuestionId { get; set; }
        public int QuestionItemId { get; set; }
        public bool IsChecked { get; set; }

    }
}
