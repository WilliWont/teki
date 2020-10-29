using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ValidationUtilities
{
    public class GenericValidationUtil<T> : IGenericValidationUtil<T> where T : class
    {
        public int GetMaxLen(string property)
        {
            return typeof(T).GetProperty(property).
                GetCustomAttributes(typeof(StringLengthAttribute), false).
                Cast<StringLengthAttribute>().SingleOrDefault().MaximumLength;
        }

        public int GetMinLen(string property)
        {
            return typeof(T).GetProperty(property).
                GetCustomAttributes(typeof(StringLengthAttribute), false).
                Cast<StringLengthAttribute>().SingleOrDefault().MinimumLength;
        }
    }
}
