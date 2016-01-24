using System.Collections.Generic;
using YuYan.Domain.Database;
using YuYan.Domain.DTO;

namespace YuYan.Domain.Extensions
{
    public static class DTOConverter
    {
        public static tbSurvey ConverToTbSurvey(this dtoSurvey source, tbSurvey data = null) {

            if (data == null)
                data = new tbSurvey();

            if (source == null)
                return null;

            data.SurveyId = source.SurveryId;
            data.Title = source.Title;
            data.ShortDescription = source.ShortDesc;
            data.LongDescription = source.LongDesc;
            data.UserId = source.UserId;

            return data;
        }

        public static dtoSurvey ConvertToDtoSurvey(this tbSurvey source, dtoSurvey data = null) {
            if (data == null)
                data = new dtoSurvey();

            if (source == null)
                return null;

            data.SurveryId = source.SurveyId;
            data.Title = source.Title;
            data.ShortDesc = source.ShortDescription;
            data.LongDesc = source.LongDescription;
            data.UserId = source.UserId;

            return data;
        }

     
    }
}
