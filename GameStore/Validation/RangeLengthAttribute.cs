using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameStore.Validation
{
    public class RangeLengthAttribute : StringLengthAttribute
    {
        public RangeLengthAttribute(int maximumLength) : base(maximumLength)
        {
        }

        public override bool IsValid(object value)
        {
            string val = Convert.ToString(value);
            if (val.Length < base.MinimumLength)
                base.ErrorMessage = "Мінімальна довжина рядка " + base.MinimumLength;
            if (val.Length > base.MaximumLength)
                base.ErrorMessage = "Максимальна довжина рядка " + base.MaximumLength;
            return base.IsValid(value);
        }
    }
}