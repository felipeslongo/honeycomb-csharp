using System;
using System.Linq.Expressions;
using System.Reflection;

namespace HoneyComb.Reflection
{
    public static class Property
    {
        public const string MessageExpressionLambdaDoesNotReturnAProperty = "The expression lambda does not represent a MemberExpression that returns whose member is a PropertyInfo";
        public const string MessagePropertyHasNoSetter = "The property passed to the bind method is readonly, therefore it has no setter method to bind the value to.";

        public static Action<TValue> GetSetter<Target, TValue>(Target target, Expression<Func<Target, TValue>> propertyLambda)
        {
            var expr = propertyLambda.Body as MemberExpression;
            if (expr == null)
                throw new ArgumentException(MessageExpressionLambdaDoesNotReturnAProperty, nameof(propertyLambda));

            var prop = expr.Member as PropertyInfo;
            if (prop == null)
                throw new ArgumentException(MessageExpressionLambdaDoesNotReturnAProperty, nameof(propertyLambda));

            if (prop.CanWrite == false)
                throw new ArgumentException(MessagePropertyHasNoSetter, nameof(propertyLambda));


            return valueToBeSet => prop.SetValue(target, valueToBeSet, null);
        }
    }
}
