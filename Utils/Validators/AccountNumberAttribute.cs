namespace Hubtel.Api.Utils;

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


public class AccountNumberAttribute(string typePropertyName, string schemePropertyName) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var typeProperty = validationContext.ObjectType.GetProperty(typePropertyName);
        var schemeProperty = validationContext.ObjectType.GetProperty(schemePropertyName);

        if (typeProperty is null || schemeProperty is null)
            return new ValidationResult($"One of the properties '{typePropertyName}' or '{schemePropertyName}' is not found.");

        var typeValue = typeProperty.GetValue(validationContext.ObjectInstance, null) as string;
        var schemeValue = schemeProperty.GetValue(validationContext.ObjectInstance, null) as string;

        if (typeValue is null || schemeValue is null)
            return new ValidationResult("Type and Account Scheme fields are mandatory.");

        if (value is null || !(value is string accountNumber))
            return new ValidationResult(ErrorMessage ?? "Invalid account number format.");

        if (typeValue.Equals("momo", StringComparison.OrdinalIgnoreCase))
        {
            if (!Regex.IsMatch(accountNumber, @"^\d{10}$"))
                return new ValidationResult(ErrorMessage ?? "'Momo' account number must be exactly 10 digits");
            if (schemeValue.Equals("mtn", StringComparison.OrdinalIgnoreCase))
            {
                if (!Regex.IsMatch(accountNumber, @"^(024|025|053|054|055|059)\d{7}$"))
                {
                    return new ValidationResult("Invalid 'mtn' number.");
                }
            }
            else if (schemeValue.Equals("vodafone", StringComparison.OrdinalIgnoreCase))
            {
                if (!Regex.IsMatch(accountNumber, @"^(020|050)\d{7}$"))
                {
                    return new ValidationResult("Invalid 'vodafone' number.");
                }
            }
            else if (schemeValue.Equals("airteltigo", StringComparison.OrdinalIgnoreCase))
            {
                if (!Regex.IsMatch(accountNumber, @"^(026|056|027|057)\d{7}$"))
                {
                    return new ValidationResult("Invalid 'airteltigo' number.");
                }
            }
            else
            {
                return new ValidationResult("Invalid account scheme for 'momo' type.");
            }
        }
        else if (typeValue.Equals("card", StringComparison.OrdinalIgnoreCase))
        {
            if (!Regex.IsMatch(accountNumber, @"^\d{16}$"))
                return new ValidationResult(ErrorMessage ?? "'Card' account number must be exactly 16 digits.");

            if (schemeValue.Equals("visa", StringComparison.OrdinalIgnoreCase))
            {
                if (!accountNumber.StartsWith("4"))
                {
                    return new ValidationResult("Invalid Visa card number.");
                }
            }
            else if (schemeValue.Equals("mastercard", StringComparison.OrdinalIgnoreCase))
            {
                if (!(accountNumber.StartsWith("51") || accountNumber.StartsWith("52") ||
                      accountNumber.StartsWith("53") || accountNumber.StartsWith("54") ||
                      accountNumber.StartsWith("55") ||
                      (accountNumber.CompareTo("2221000000000000") >= 0 &&
                       accountNumber.CompareTo("2720999999999999") <= 0)))
                {
                    return new ValidationResult("Invalid Mastercard number.");
                }
            }
            else
            {
                return new ValidationResult("Invalid 'card' account scheme. Only 'visa' and 'mastercard' are accepted.");
            }
        }
        else
        {
            return new ValidationResult(ErrorMessage ?? "Invalid type provided.");
        }

        return ValidationResult.Success;
    }
}

