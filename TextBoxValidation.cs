using System;
using System.Windows.Controls;
using System.Globalization;
using System.Text.RegularExpressions;
namespace prototype2
{
    public class TextBoxValidation : ValidationRule
    {
        public string TextBoxType { get; set; }
        RegexUtilities regex = new RegexUtilities();
        public override ValidationResult Validate (object value, System.Globalization.CultureInfo cultureInfo)
        {
            var str = TextBoxType as string;

            if (str.Equals("String"))
            {
                if (value.ToString().Length == 0)
                    return new ValidationResult(false, "*This field must be filled.");
                return ValidationResult.ValidResult;
            }
            else if (str.Equals("Mobile"))
            {
                if (!regex.IsValidMobileNumber(value.ToString()))
                {
                    return new ValidationResult(false, "*Please follow the correct format for Mobile number. (Ex. (+63)-998-791 9482 or 0998-791-9482, with or without dashes as spaces(vice versa)");
                }
                return ValidationResult.ValidResult;
                

            }
            else if (str.Equals("Phone"))
            {
                if (!regex.IsValidPhoneNumber(value.ToString()))
                {
                    return new ValidationResult(false, "*Please follow the correct format for Phone Bumber. (Ex. 123-4567, (02)123-4567, with or without dashes as spaces(vice versa))");
                }
                return ValidationResult.ValidResult;

            }
            else if (str.Equals("Email"))
            {
                if (!regex.IsValidEmail(value.ToString()))
                {
                    return new ValidationResult(false, "*The E-mail is not a valid email.");
                }
                

            }
            else if (str.Equals("ComboBox"))
            {
                if (value is null)
                    return new ValidationResult(false, "Selection is invalid.");
                else
                    return new ValidationResult(true, null);

            }

            return ValidationResult.ValidResult;
        }
    }
    
public class RegexUtilities
    {
        bool invalid = false;

        public bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                 strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        public bool IsValidPhoneNumber(string strIn)
        {
            invalid = false;
            try
            {
                return Regex.IsMatch(strIn,@"\((?<AreaCode>\d{3})\)\s*(?<Number>\d{3}(?:-|\s*)\d{4})",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public bool IsValidMobileNumber(string strIn)
        {
            invalid = false;
            try
            {
                return Regex.IsMatch(strIn, @"(\(?(?<AreaCode>.\d{2,3})\)?(?:-|\s*))?(?<Number>\d{3,4}(?:-|\s*)\d{3}(?:-|\s*)\d{4})",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
