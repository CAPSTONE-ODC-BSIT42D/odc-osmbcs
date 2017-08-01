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
            else if (str.Equals("Number"))
            {
                try
                {
                    Decimal number = Decimal.Parse(value.ToString());
                    if (value.ToString().Length < 7)
                    {
                        return new ValidationResult(false, "*Please follow the correct format for Phone number. (Ex. 123-4567)");
                    }
                    else if (value.ToString().Length > 11)
                    {
                        return new ValidationResult(false, "*Please follow the correct format for Mobile number. (Ex. 0912-345-6789)");
                    }
                }
                catch (Exception e)
                {
                    return new ValidationResult(false, "*Characters and symbols are not accepted.");
                }
                return ValidationResult.ValidResult;

            }
            else if (str.Equals("Mobile"))
            {
                try
                {
                    Decimal number = Decimal.Parse(value.ToString());
                    if (value.ToString().Length > 11)
                    {
                        return new ValidationResult(false, "*Please follow the correct format for Mobile number. (Ex. 0912-345-6789)");
                    }
                }
                catch (Exception e)
                {
                    return new ValidationResult(false, "*Characters and symbols are not accepted.");
                }
                return ValidationResult.ValidResult;

            }
            else if (str.Equals("Phone"))
            {
                try
                {
                    Decimal number = Decimal.Parse(value.ToString());
                    if (value.ToString().Length > 7)
                    {
                        return new ValidationResult(false, "*Please follow the correct format for Phone Bumber. (Ex. 123-4567)");
                    }
                }
                catch (Exception e)
                {
                    return new ValidationResult(false, "*Characters and symbols are not accepted.");
                }
                return ValidationResult.ValidResult;

            }
            else if (str.Equals("Email"))
            {
                if (!regex.IsValidEmail(value.ToString()))
                {
                    return new ValidationResult(false, "*Please use valid format for E-mail.");
                }
                return ValidationResult.ValidResult;

            }
            else if (str.Equals("ComboBox"))
            {
                if (value is null)
                    return new ValidationResult(false, "Please choose a Contact Type.");
                else
                    return new ValidationResult(true, null);

            }
            //if (value == null)
            //    return new ValidationResult(false, "Value cannot be empty.");
            //else
            //{
                

            //}

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
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
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
    }
}
