using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuYan.Domain.DTO
{
    public class dtoSurveyQuestionItem
    {
        public int QuestionItemId { get; set; }
        public int QuestionId { get; set; }
        public string ItemDescription { get; set; }
        public int ItemOrder { get; set; }
    }
}
