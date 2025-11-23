using System.Collections.Generic;
using Assets.SharedAssets.Networking.Data;

namespace Assets.SharedAssets.Networking.Validators
{
    public static class QuestionBankDtoValidator{
        public static List<string> Validate(QuestionBankDto questionBankDto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(questionBankDto.Title))
            {
                errors.Add("Invalid data: title cannot be empty!");
            }
            else if (questionBankDto.Title.Length < 5)
            {
                errors.Add("Invalid data: title must be at least 5 characters long.");
            }

            if (questionBankDto.OwnerId < 0)
            {
                errors.Add("Invalid data: owner id cannot be negative!!");
            }

            return errors;
        }
    }
}