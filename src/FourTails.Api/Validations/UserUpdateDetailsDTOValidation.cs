using FluentValidation;
using FourTails.DTOs.Payload.User;
using System.Text.RegularExpressions;

namespace FourTails.Api.Validations;

public class UserUpdateDetailsDTOValidation : AbstractValidator<UserUpdateDetailsDTO>
{
    public UserUpdateDetailsDTOValidation()
    {
        RuleFor(user => user.Id).Custom((str, context) => 
        {
            if (str.ToString().Length == 0)
            {
                context.AddFailure("Id field should not be empty.");
            }
        });
        RuleFor(user => user.Age).Custom((number, context) => 
        {
            if (number.ToString().Length == 0)
            {
                context.AddFailure("Age field should not be empty.");
            }
            else if (number < 18)
            {
                context.AddFailure("User must be at least 18 years old.");
            }
        });
        RuleFor(user => user.Address).Custom((str, context) => 
        {
            if (str == null || str.Length == 0)
            {
                context.AddFailure("Address field should not be empty.");
            }
            else if (!Regex.Match(str, @"^[A-Za-z0-9]+(?:\s[A-Za-z0-9'.,]+)+$", RegexOptions.IgnoreCase).Success)
            {
                context.AddFailure("Address should start with number and follow with 'abc' characters.");
            } 
            else if (str.Length > 200)
            {
                context.AddFailure("Address should not exceed maximum of 200 characters.");
            }
        });
        RuleFor(user => user.FirstName).Custom((str, context) => 
        {
            if (str == null || str.Length == 0)
            {
                context.AddFailure("FirstName sfield should not be empty.");
            }
            else if (!Regex.Match(str, @"^[\p{L} \.'\-]+$", RegexOptions.IgnoreCase).Success)
            {
                context.AddFailure("FirstName should only consist of 'abc' characters and/or numbers.");
            } 
            else if (str.Length > 50)
            {
                context.AddFailure("FirstName should not exceed maximum of 50 characters.");
            }
        });
        RuleFor(user => user.LastName).Custom((str, context) => 
        {
            if (str == null || str.Length == 0)
            {
                context.AddFailure("LastName field should not be empty.");
            }
            else if (!Regex.Match(str, @"^[\p{L} \.'\-]+$", RegexOptions.IgnoreCase).Success)
            {
                context.AddFailure("LastName should only consist of 'abc' characters and/or numbers.");
            } 
            else if (str.Length > 50)
            {
                context.AddFailure("LastName should not exceed maximum of 50 characters.");
            }
        });
    }
}