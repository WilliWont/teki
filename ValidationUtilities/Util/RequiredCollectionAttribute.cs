using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationUtilities.Util
{
    // possibly deprecated
    // credits to Kyle Trauberman at:
    // https://stackoverflow.com/a/11267012/11620610
    public class RequiredCollectionAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            bool isValid = base.IsValid(value);

            if (isValid)
            {
                ICollection collection = value as ICollection;
                if (collection != null)
                {
                    isValid = collection.Count != 0;
                }
            }
            return isValid;
        }
    }
}
