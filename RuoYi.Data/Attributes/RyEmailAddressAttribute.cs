using System.ComponentModel.DataAnnotations;

namespace RuoYi.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RyEmailAddressAttribute : DataTypeAttribute
    {
        private static bool EnableFullDomainLiterals { get; } =
            AppContext.TryGetSwitch("System.Net.AllowFullDomainLiterals", out bool enable) ? enable : false;

        public RyEmailAddressAttribute() : base(DataType.EmailAddress)
        {
        }

        public override bool IsValid(object? value)
        {
            if (value == null || (value is string && value.ToString() == ""))
            {
                return true;
            }

            if (!(value is string valueAsString))
            {
                return false;
            }

            if (!EnableFullDomainLiterals && (valueAsString.Contains('\r') || valueAsString.Contains('\n')))
            {
                return false;
            }

            // only return true if there is only 1 '@' character
            // and it is neither the first nor the last character
            int index = valueAsString.IndexOf('@');

            return
                index > 0 &&
                index != valueAsString.Length - 1 &&
                index == valueAsString.LastIndexOf('@');
        }
    }
}
