using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YuYan.Domain.Database
{
    [Table("SurveyResult")]
    public class tbSurveyResult: tbTbase
    {
        [Key, Column("Id")]
        public int SurveyResultId { get; set; }
        public int MinScore { get; set; }
        public int MaxScore { get; set; }
        public int SurveyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
