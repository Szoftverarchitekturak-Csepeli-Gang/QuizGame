using Assets.Scripts.Networking.Data;
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
                dto.optionA ?? "",
                dto.optionB ?? "",
                dto.optionC ?? "",
                dto.optionD ?? ""
            };

            return new Question(dto.text, answers, dto.correctAnswer);
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
                text = model.QuestionText,
                optionA = model.Answers.Length > 0 ? model.Answers[0] : "",
                optionB = model.Answers.Length > 1 ? model.Answers[1] : "",
                optionC = model.Answers.Length > 2 ? model.Answers[2] : "",
                optionD = model.Answers.Length > 3 ? model.Answers[3] : "",
                correctAnswer = model.CorrectAnswerIdx
            };
        }
    }
}
