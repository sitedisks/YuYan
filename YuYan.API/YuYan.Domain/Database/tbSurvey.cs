using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YuYan.Domain.Database
{
    [Table("Survey")]
    public class tbSurvey : tbTbase
    {
        public tbSurvey()
        {
            tbSurveyQuestions = new HashSet<tbSurveyQuestion>();
        }
        [Key, Column("Id")]
        public int SurveyId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        [Column("UpdatedByUserId")]
        public Nullable<Guid> UserId { get; set; }

        public virtual ICollection<tbSurveyQuestion> tbSurveyQuestions { get; set; }

    }
}
