using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VinculacionBackend.CustomDataNotations
{
    public class ValidDomainAttribute:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            var email=value.ToString();
            Regex validDomain = new Regex(@"^[a-zA-Z0-9]+([._]?[a-zA-Z0-9])+((@unitec\.edu)|(@unitec\.edu\.hn))$");
            return validDomain.Match(email).Success;
        }

    }
}
