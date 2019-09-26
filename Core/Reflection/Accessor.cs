using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Reflection
{
    /// <summary>
    /// I wrote a wrapper using the ExpressionTree variant and c#7 (if somebody is interested):
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="https://stackoverflow.com/a/43498938/8099185"/>
    public class Accessor<T>
    {
        private Action<T> Setter;
        private Func<T> Getter;

        public Accessor(Expression<Func<T>> memberLambda)
        {
            var memberExpression = memberLambda.Body as MemberExpression;
            if(memberExpression == null)
                throw new ArgumentException(Property.MessageExpressionLambdaDoesNotReturnAProperty, nameof(memberLambda));

            var instanceExpression = memberExpression.Expression;
            var parameter = Expression.Parameter(typeof(T));

            if (memberExpression.Member is PropertyInfo propertyInfo)
            {
                if (propertyInfo.CanWrite == false)
                    throw new ArgumentException(Property.MessagePropertyHasNoSetter, nameof(memberLambda));

                Setter = Expression.Lambda<Action<T>>(Expression.Call(instanceExpression, propertyInfo.GetSetMethod(), parameter), parameter).Compile();
                Getter = Expression.Lambda<Func<T>>(Expression.Call(instanceExpression, propertyInfo.GetGetMethod())).Compile();
            }
            else if (memberExpression.Member is FieldInfo fieldInfo)
            {
                Setter = Expression.Lambda<Action<T>>(Expression.Assign(memberExpression, parameter), parameter).Compile();
                Getter = Expression.Lambda<Func<T>>(Expression.Field(instanceExpression, fieldInfo)).Compile();
            }

        }

        public void Set(T value) => Setter(value);

        public T Get() => Getter();
    }
}
