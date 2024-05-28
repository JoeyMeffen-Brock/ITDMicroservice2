using System.ComponentModel.DataAnnotations;

namespace BrockSolutions.ITDService.Options
{
    public class ITDServiceParameters
    {
        public const string CONFIGURATION_KEY = "ITDService";

        [Required(ErrorMessage = "Don't forget ExampleConfigurationValue!", AllowEmptyStrings = false)]
        public string ExampleConfigurationValue { get; set; } = string.Empty;

        public IDictionary<string, string>? ConnectionString { get; set; } 

        // TODO: Add configuration parameters here to match the ITDService yaml files. 

        // NOTE: Use the custom ValidationAttribute [ValidateObject] as a data annotation on sub-classes
        // as ValidationAttribute data annotation really only works on C# built in types
        // https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-7.0
    }
}
