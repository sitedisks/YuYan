using System;

namespace YuYan.Domain.DTO
{
    public class dtoSurveyShare
    {
        public long SurveyShareId { get; set; }
        public int SurveyId { get; set; }
        public string IPAddress { get; set; }
        public DateTime VisitedDate { get; set; }
    }
}
