using Assets.SharedAssets.Networking.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                dto.optionA ?? "",
                dto.optionB ?? "",
                dto.optionC ?? "",
                dto.optionD ?? ""
            };

            return new Question(dto.id, dto.text, answers, dto.correctAnswer-1);
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
                id = model.Id,
                text = model.QuestionText,
                optionA = model.Answers.Length > 0 ? model.Answers[0] : "",
                optionB = model.Answers.Length > 1 ? model.Answers[1] : "",
                optionC = model.Answers.Length > 2 ? model.Answers[2] : "",
                optionD = model.Answers.Length > 3 ? model.Answers[3] : "",
                correctAnswer = model.CorrectAnswerIdx+1
            };
        }

        public static List<QuestionDto> ToDtoList(this List<Question> models)
        {
            return models?.Select(ToDto).ToList() ?? new List<QuestionDto>();
        }
    }
}
