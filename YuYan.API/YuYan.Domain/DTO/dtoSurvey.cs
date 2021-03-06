﻿using System;
using System.Collections.Generic;

namespace YuYan.Domain.DTO
{
    public class dtoSurvey: dtoTbase
    {
        public int SurveyId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string URLToken { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; } 
        public Nullable<Guid> UserId { get; set; }
        public int VisitCount { get; set; }
        public int CompleteCount { get; set; }

        public virtual ICollection<dtoSurveyQuestion> dtoQuestions { get; set; }
    }
}
