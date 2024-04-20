using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace API_FinalTask.Models
{
    public class PriceValidate:ValidationAttribute
    {

        public PriceValidate() { }

        public override bool IsValid(object? obj)
        {
            if (obj == null)
                return false;
            else
            {
                if (obj is double)
                {
                    double price = (double)obj;
                
                    if (price>10)
                        return true;
                    else
                    {

                        ErrorMessage = "InValid price value it must be 10 or more "; 
                        return false;
                    }
                }
                else
                    return false;
            }
           
        }
    }
}
