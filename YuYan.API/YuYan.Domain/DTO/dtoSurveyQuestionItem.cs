using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuYan.Domain.DTO
{
    public class dtoSurveyQuestionItem: dtoTbase
    {
        public int QuestionItemId { get; set; }
        public int QuestionId { get; set; }
        public string ItemDescription { get; set; }
        public int ItemOrder { get; set; }
        public int? Score { get; set; }
        public int? GotoQuestionId { get; set; }
        public Guid? ImageId { get; set; }
    }
}
