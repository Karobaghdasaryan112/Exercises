using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationAttribute.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,AllowMultiple = false)]
    public class PasswordValidation : Attribute
    {
        public int StartLength {  get; set; }
        public int EndLength { get; set; }
        public PasswordValidation(int startLength,int endLength) 
        {
            StartLength = startLength;
            EndLength = endLength;
        }
    }
}
