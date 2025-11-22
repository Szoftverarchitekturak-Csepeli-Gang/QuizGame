using Assets.SharedAssets.Networking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SharedAssets.Networking.Mappers
{
    public static class QuestionMapper
    {
        public static Question ToModel(this QuestionDto dto)
        {
            if (dto == null) return null;

            var answers = new[]
            {
                dto.OptionA ?? "",
                dto.OptionB ?? "",
                dto.OptionC ?? "",
                dto.OptionD ?? ""
            };

            return new Question(dto.Id, dto.Text, answers, dto.CorrectAnswer-1);
        }

        public static List<Question> ToModelList(this List<QuestionDto> dtos)
        {
            return dtos?.Select(ToModel).ToList() ?? new List<Question>();
        }

        public static QuestionDto ToDto(this Question model)
        {
            if (model == null) return null;

            return new QuestionDto
            {
                Id = model.Id,
                Text = model.QuestionText,
                OptionA = model.Answers.Length > 0 ? model.Answers[0] : "",
                OptionB = model.Answers.Length > 1 ? model.Answers[1] : "",
                OptionC = model.Answers.Length > 2 ? model.Answers[2] : "",
                OptionD = model.Answers.Length > 3 ? model.Answers[3] : "",
                CorrectAnswer = model.CorrectAnswerIdx+1
            };
        }

        public static List<QuestionDto> ToDtoList(this List<Question> models)
        {
            return models?.Select(ToDto).ToList() ?? new List<QuestionDto>();
        }
    }
}
