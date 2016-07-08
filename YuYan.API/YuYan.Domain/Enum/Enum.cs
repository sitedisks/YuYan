
namespace YuYan.Domain.Enum
{
    public enum UserRole
    {
        Admin = 1,
        User = 2,
        Anonymous = 3
    }

    public enum QuestionType
    {
        checkbox = 1,
        radio = 2
    }

    public enum ImageType
    {
        UserAvatar = 1,
        SurveyBanner = 2,
        SurveyLogo = 3,
        SurveyRef = 4,
        QuestionBanner = 5,
        QuestionRef = 6,
        QuestionItem = 7
    }
}
