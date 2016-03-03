using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YuYan.Domain.Database
{
    [Table("SurveyClient")]
    public class tbSurveyClient
    {
        public tbSurveyClient()
        {
            tbClientAnswers = new HashSet<tbSurveyClientAnswer>();
        }

        [Key, Column("Id")]
        public long ClientId { get; set; }
        public string Email { get; set; }
        public string IPAddress { get; set; }
        public int SurveyId { get; set; }
        public int TotalScore { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<tbSurveyClientAnswer> tbClientAnswers { get; set; }
    }
}
