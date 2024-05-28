using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BrockSolutions.ITDService.Options.Validation
{
    public static class OptionsValidator<TOptions> where TOptions : class
    {
        // Static variables for each type of options
        private static bool _isValid;
        private static TOptions? _lastValidated;

        /// <summary>
        /// A method which validates TOptions. This validates Data Annotations by default. 
        /// The SystemFileWatcher triggers OnChange multiple times for a single change, so this method
        /// accounts for that by only validating if the values of the options have changed since last validation.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="optionsToValidate"></param>
        /// <returns>Whether the incoming options are valid</returns>
        public static bool Validate(NLog.ILogger logger, TOptions optionsToValidate)
        {
            if (optionsToValidate == null)
            {
                _isValid = false;
                return _isValid;
            }
            if (_lastValidated == null)
            {
                _lastValidated = optionsToValidate;
            }
            // TODO: Check if values of the options have changed using an equality comparer
            // (default equal doesn't work here because file hashes are different)
            else if (!JsonEquals(_lastValidated, optionsToValidate))
            {
                _lastValidated = optionsToValidate;
            }
            else
            {
                return _isValid;
            }

            logger.Info("Validating configuration for {ConfigurationType}", typeof(TOptions));

            // Validation Failures MUST be logged before returning false. 
            // OptionsMonitor consumes validation exceptions so that invalid or inaccessible configurations can be recoverable.
            // This also means that execution will not stop for invalid Options unless they are being called on a page or in startup. 
            _isValid = DataAnnotationValidation(logger, optionsToValidate);

            return _isValid;
        }

        /// <summary>
        /// Validate the options using DataAnnotations in the options class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="optionsToValidate"></param>
        /// <returns></returns>
        private static bool DataAnnotationValidation(NLog.ILogger logger, TOptions optionsToValidate)
        {
            bool isValid;
            var validationContext = new ValidationContext(optionsToValidate);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(
                optionsToValidate,
                validationContext,
                validationResults,
                validateAllProperties: true
            );

            if (validationResults.Count != 0)
            {
                FormatAndLogError(logger, validationContext, validationResults);
                isValid = false;
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }

        private static void FormatAndLogError(NLog.ILogger logger, ValidationContext validationContext, List<ValidationResult> validationResults)
        {
            var failureMessages = validationResults.SelectMany(x =>
            {
                return x is CompositeValidationResult compositeValidationResult
                    ? compositeValidationResult.Results?.Select(s => s.ErrorMessage)
                        ?? Enumerable.Empty<string>()
                    : new[] { x.ErrorMessage };
            });

            // This exception doesn't get thrown because it would be caught in the OptionsMonitor middleware
            // It's helpful for formatting and logging the failureMessages though.
            var ex = new OptionsValidationException(
                nameof(TOptions),
                typeof(TOptions),
                failureMessages
            );

            var failedValidationMembers = validationResults.SelectMany(x =>
            {
                return x is CompositeValidationResult compositeValidationResult
                    ? compositeValidationResult.Results?.SelectMany(s => s.MemberNames)
                        ?? Enumerable.Empty<string>()
                    : x.MemberNames;
            });

            logger.Error(
                ex,
                "Validation failed for {validationContext} members: {FailedMembers}.",
                validationContext.DisplayName,
                string.Join("; ", failedValidationMembers)
            );
        }

        // TODO: Substitute uses of this!
        // This is computationally heavy compared to overriding Equals on the classes to be validated...
        // but it's also an easy way to test equality of public properties of two objects.
        private static bool JsonEquals(TOptions obj1, TOptions obj2)
        {
            if (obj1 == null && obj2 == null)
            {
                return true;
            }
            if (obj1 == null ^ obj2 == null)
            {
                return false;
            }
            return JsonConvert.SerializeObject(obj1).Equals(JsonConvert.SerializeObject(obj2));
        }
    }
}
