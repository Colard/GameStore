using System;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    abstract public class AbstractNumberComparison : ValidationAttribute
    {
        private double _limitNumber;

        public AbstractNumberComparison(double limitNumber)
        {
            _limitNumber = limitNumber;
        }

        public override bool IsValid(object value)
        {
            try
            {
                double number = Convert.ToDouble(value);
                return checkNumbers(number, _limitNumber);
            }
            catch
            {
                return false;
            }
        }

        protected abstract bool checkNumbers(double val1, double val2);
    }
}
