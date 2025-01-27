using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationAttribute.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,AllowMultiple =false)]
    public class ValidationLength : Attribute
    {   
        public int LowLength { get; set; } 
        public int HighLength { get; set; }

        public ValidationLength(int lowLength, int highLength)
        {
            LowLength = lowLength;
            HighLength = highLength;
        }


    }
}
