using System;
using System.Collections.Generic;
using System.Text;

namespace ValidationUtilities
{
    public interface IGenericValidationUtil<T> where T : class
    {
        public int GetMaxLen(string property);
        public int GetMinLen(string property);
    }
}
