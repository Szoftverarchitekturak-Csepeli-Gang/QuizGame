using System.Collections.Generic;
using Assets.Scripts.Networking.Data;

namespace Assets.SharedAssets.Networking.Validators
{
    public static class QuestionDtoValidator
    {
        public static List<string> Validate(QuestionDto questionDto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(questionDto.Text))
            {
                errors.Add("Text is required!");
            }
            else if (questionDto.Text.Length < 5)
            {
                errors.Add("Text must be at least 5 characters long!");
            }

            if (string.IsNullOrWhiteSpace(questionDto.OptionA))
            {
                errors.Add("OptionA is required!");
            }
            else if (questionDto.OptionA.Length < 1) 
            {
                errors.Add("OptionA must be at least 1 character long!");
            }

            if (string.IsNullOrWhiteSpace(questionDto.OptionB))
            {
                errors.Add("OptionB is required!");
            }
            else if (questionDto.OptionB.Length < 1)
            {
                errors.Add("OptionB must be at least 1 character long!");
            }

            if (string.IsNullOrWhiteSpace(questionDto.OptionC))
            {
                errors.Add("OptionC is required!");
            }
            else if (questionDto.OptionC.Length < 1)
            {
                errors.Add("OptionC must be at least 1 character long!");
            }

            if (string.IsNullOrWhiteSpace(questionDto.OptionD))
            {
                errors.Add("OptionD is required!");
            }
            else if (questionDto.OptionD.Length < 1)
            {
                errors.Add("OptionD must be at least 1 character long!");
            }

            if (questionDto.CorrectAnswer < 1 || questionDto.CorrectAnswer > 4)
            {
                errors.Add("CorrectAnswer is between 1-4!");
            }

            return errors;
        }
    }
}