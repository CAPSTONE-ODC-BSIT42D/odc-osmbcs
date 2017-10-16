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
            
            if(str!=null)
            {
                if (str.Equals("IsEmpty"))
                {
                    if (value is null)
                    {
                        return new ValidationResult(false, "*This field must be filled.");
                    }
                    return ValidationResult.ValidResult;
                }
                else if (str.Equals("Mobile"))
                {
                    if (!(value is null))
                    {
                        if (!regex.IsValidMobileNumber(value.ToString()))
                        {
                            return new ValidationResult(false, "*Please follow the correct format for Mobile number. (Ex. (+63)-998-791 9482 or 0998-791-9482, with or without dashes as spaces(vice versa)");
                        }
                    }
                    else
                        return new ValidationResult(false, "*This field must be filled.");

                    return ValidationResult.ValidResult;


                }
                else if (str.Equals("Phone"))
                {
                    if (!(value is null))
                    {
                        if (!regex.IsValidPhoneNumber(value.ToString()))
                        {
                            return new ValidationResult(false, "*Please follow the correct format for Phone Bumber. (Ex. 123-4567, (02)123-4567, with or without dashes as spaces(vice versa))");
                        }
                    }
                    else
                        return new ValidationResult(false, "*This field must be filled.");
                    
                    return ValidationResult.ValidResult;

                }
                else if (str.Equals("Email"))
                {
                    if (!(value is null))
                    {
                        if (!regex.IsValidEmail(value.ToString()))
                        {
                            return new ValidationResult(false, "*The E-mail is not a valid email.");
                        }
                    }
                    else
                        return new ValidationResult(false, "*This field must be filled.");
                    


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
            else
            {
                return ValidationResult.ValidResult;
            }
            
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
            else
            {
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
            
        }
        

        public bool IsValidPhoneNumber(string strIn)
        {
            invalid = false;
            try
            {
                return Regex.IsMatch(strIn, @"((?:\()?(.?\d{2,3})?(?:\))?(?:-|\s*))?(?<Number>\d{3}(?:-|\s*)\d{4})",
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
                return Regex.IsMatch(strIn, @"((?:\()?(.?\d{2,3})?(?:\))?(?:-|\s*))?(?<Number>\d{3,4}(?:-|\s*)\d{3}(?:-|\s*)\d{4})",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
