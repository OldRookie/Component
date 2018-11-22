using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Component.Infrastructure
{
    public class ValidationResultInfo
    {
        public bool IsValid { get { return ValidationResults.Count == 0; } }
        public Dictionary<string, List<ValidationResult>> ValidationResults { get; set; }
    }

    public class ValidationBaseInfo
    {
        public ValidationBaseInfo()
        {
            ErrorMessages = new List<string>();
        }
        public bool IsValid { get { return ErrorMessages.Count == 0; } }
        public List<string> ErrorMessages { get; set; }
    }

    public class ValidationResultBaseInfo
    {
        public ValidationResultBaseInfo()
        {
            Messages = new List<string>();
        }

        public bool IsValid
        {
            get
            {
                return Messages.Count == 0;

            }
        }

        public List<string> Messages { get; set; }
    }
}