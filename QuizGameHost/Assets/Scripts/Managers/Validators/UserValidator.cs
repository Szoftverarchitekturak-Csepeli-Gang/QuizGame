using System.Collections.Generic;
using Assets.SharedAssets.Networking.Data;

namespace Assets.SharedAssets.Networking.Validators
{
    public static class UserValidator{
        public static List<string> Validate(string username, string password, string confirmPassword)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(username))
            {
                errors.Add("Username cannot be empty!");
            }
            else if(string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password cannot be empty!");
            }
            else if(!password.Equals(confirmPassword))
            {
                errors.Add("Passwords don't match!");
            }
            else if (username.Length < 5)
            {
                errors.Add("Invalid data: username must be at least 5 characters long!");
            }
            else if (password.Length < 5)
            {
                errors.Add("Invalid data: password must be at least 5 characters long!");
            }

            return errors;
        }
    }
}