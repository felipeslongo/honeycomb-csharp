using System;
using System.Linq.Expressions;
using System.Reflection;

namespace HoneyComb.Reflection
{
    public static class Field
    {
        public const string MessageExpressionLambdaDoesNotReturnAField = "The expression lambda does not represent a MemberExpression that returns whose member is a FieldInfo";
        public const string MessageFieldIsReadonly = "The field is ReadOnly and cannot be changed";

        public static Action<TValue> GetSetter<Target, TValue>(Target target, Expression<Func<Target, TValue>> fieldLambda)
        {
            var expr = fieldLambda.Body as MemberExpression;
            if (expr == null)
                throw new ArgumentException(MessageExpressionLambdaDoesNotReturnAField, nameof(fieldLambda));

            var prop = expr.Member as FieldInfo;
            if (prop == null)
                throw new ArgumentException(MessageExpressionLambdaDoesNotReturnAField, nameof(fieldLambda));

            if (prop.IsInitOnly)
                throw new ArgumentException(MessageFieldIsReadonly, nameof(fieldLambda));

            return valueToBeSet => prop.SetValue(target, valueToBeSet);
        }
    }
}
