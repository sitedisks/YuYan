using System;
using System.Collections.Generic;

namespace YuYan.Domain.DTO
{
    public class dtoSurvey
    {
        public int SurveryId { get; set; }
        public string Title { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; } 
        public Nullable<Guid> UserId { get; set; }

        public virtual ICollection<dtoSurveyQuestion> dtoQuestions { get; set; }
    }
}
