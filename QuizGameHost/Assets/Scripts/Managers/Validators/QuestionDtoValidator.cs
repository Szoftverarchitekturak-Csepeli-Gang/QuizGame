using System.Collections.Generic;
using Assets.SharedAssets.Networking.Data;

namespace Assets.SharedAssets.Networking.Validators
{
    public static class QuestionDtoValidator
    {
        public static List<string> Validate(QuestionDto questionDto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(questionDto.text))
            {
                errors.Add("Text is required!");
            }
            else if (questionDto.text.Length < 5)
            {
                errors.Add("Text must be at least 5 characters long!");
            }

            if (string.IsNullOrWhiteSpace(questionDto.optionA))
            {
                errors.Add("OptionA is required!");
            }
            else if (questionDto.optionA.Length < 1) 
            {
                errors.Add("OptionA must be at least 1 character long!");
            }

            if (string.IsNullOrWhiteSpace(questionDto.optionB))
            {
                errors.Add("OptionB is required!");
            }
            else if (questionDto.optionB.Length < 1)
            {
                errors.Add("OptionB must be at least 1 character long!");
            }

            if (string.IsNullOrWhiteSpace(questionDto.optionC))
            {
                errors.Add("OptionC is required!");
            }
            else if (questionDto.optionC.Length < 1)
            {
                errors.Add("OptionC must be at least 1 character long!");
            }

            if (string.IsNullOrWhiteSpace(questionDto.optionD))
            {
                errors.Add("OptionD is required!");
            }
            else if (questionDto.optionD.Length < 1)
            {
                errors.Add("OptionD must be at least 1 character long!");
            }

            if (questionDto.correctAnswer < 1 || questionDto.correctAnswer > 4)
            {
                errors.Add("CorrectAnswer is between 1-4!");
            }

            return errors;
        }
    }
}