﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationAttribute.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,AllowMultiple =false)]
    public class EmailValidation : Attribute
    {

    }
}
