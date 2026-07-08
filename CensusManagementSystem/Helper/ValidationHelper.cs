using System.Text.RegularExpressions;
using System.Windows;

namespace CensusManagementSystem.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsEmpty(string value) => string.IsNullOrWhiteSpace(value);

        public static bool IsValidCNIC(string cnic)
        {
            if (IsEmpty(cnic)) return false;
            // Pakistani CNIC format: XXXXX-XXXXXXX-X (13 digits with dashes) or 13 digits
            string pattern = @"^\d{5}-?\d{7}-?\d{1}$";
            return Regex.IsMatch(cnic, pattern);
        }

        public static bool IsNumeric(string value)
        {
            if (IsEmpty(value)) return false;
            return int.TryParse(value, out _);
        }

        public static bool IsValidAge(int age) => age >= 0 && age <= 150;

        public static bool ShowValidationError(string message)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        public static bool ValidateRequiredFields(params (string Value, string FieldName)[] fields)
        {
            foreach (var field in fields)
            {
                if (IsEmpty(field.Value))
                {
                    ShowValidationError($"'{field.FieldName}' is required and cannot be empty.");
                    return false;
                }
            }
            return true;
        }

        public static bool ValidateCNIC(string cnic)
        {
            if (!IsValidCNIC(cnic))
            {
                ShowValidationError("Invalid CNIC format. Expected format: XXXXX-XXXXXXX-X or 13 digits.");
                return false;
            }
            return true;
        }

        public static bool ValidateAge(int age)
        {
            if (!IsValidAge(age))
            {
                ShowValidationError("Age must be between 0 and 150.");
                return false;
            }
            return true;
        }
    }
}
