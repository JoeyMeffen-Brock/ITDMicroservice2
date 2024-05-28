using System.ComponentModel.DataAnnotations;

namespace BrockSolutions.ITDService.Options.Validation
{
    // Add custom "ValidateObject" attribute to ensure that nested objects are also validated for attributes
    // https://stackoverflow.com/questions/2493800/how-can-i-tell-the-data-annotations-validator-to-also-validate-complex-child-pro
    internal class ValidateObjectAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(value, null, null);

            Validator.TryValidateObject(value, context, results, true);

            if (results.Count != 0)
            {
                var compositeResults = new CompositeValidationResult(
                    // Overall error message for the given validation context
                    string.Format("Validation for {0} failed.", validationContext.DisplayName)
                );
                results.ForEach(x => compositeResults.AddResult(x));

                return compositeResults;
            }

            return ValidationResult.Success;
        }
    }

    internal class CompositeValidationResult : ValidationResult
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        public IEnumerable<ValidationResult> Results
        {
            get
            {
                return _results;
            }
        }

        public CompositeValidationResult(string errorMessage) : base(errorMessage) { }
        public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames) { }
        protected CompositeValidationResult(ValidationResult validationResult) : base(validationResult) { }

        public void AddResult(ValidationResult validationResult)
        {
            _results.Add(validationResult);
        }
    }
}
