using System;

namespace YuYan.Domain.DTO
{
    public class dtoSurveyResult: dtoTbase
    {
        public int SurveyResultId { get; set; }
        public int MinScore { get; set; }
        public int MaxScore { get; set; }
        public int SurveyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? ShowStatistics { get; set; }
    }
}
