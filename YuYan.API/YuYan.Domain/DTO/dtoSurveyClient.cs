
using System.Collections.Generic;
namespace YuYan.Domain.DTO
{
    public class dtoSurveyClient
    {
        public long ClientId { get; set; }
        public string Email { get; set; }
        public string IPAddress { get; set; }
        public int SurveyId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public virtual ICollection<dtoSurveyClientAnswer> dtoClientAnswers { get; set; }
    }
}
