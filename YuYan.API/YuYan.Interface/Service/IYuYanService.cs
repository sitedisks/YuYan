using System.Threading.Tasks;
using YuYan.Domain.DTO;

namespace YuYan.Interface.Service
{
    public interface IYuYanService
    {
        Task<dtoSurvey> GetSurveyBySurveyId(int surveyId);

    }
}
