using GameStore.Validation;
using System;

namespace GameStore.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NumberMax : AbstractNumberComparison
    {
        public NumberMax(double limitNumber) : base(limitNumber) {}

        protected override bool checkNumbers(double val1, double val2) 
        {
            return val1 <= val2;
        }
    }
}
