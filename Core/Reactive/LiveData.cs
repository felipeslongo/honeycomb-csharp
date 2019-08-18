using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Reactive
{
    /// <summary>
    /// LiveData is a data holder class that can be observed.
    /// Use it to create observable properties in C# with only getter
    /// </summary>
    /// <remarks>
    /// Inspired by the LiveData class in Android Architecture Components
    /// </remarks>
    /// <typeparam name="T">Wrapped type</typeparam>
    /// <see cref="https://developer.android.com/topic/libraries/architecture/livedata"/>
    /// <see cref="https://developer.android.com/reference/androidx/lifecycle/LiveData.html"/>
    public abstract class LiveData<T>
    {
        public const string MessageExpressionLambdaDoesNotReturnAProperty = "The expression lambda does not represent a MemberExpression that returns whose member is a PropertyInfo";
        public const string MessagePropertyHasNoSetter = "The property passed to the bind method is readonly, therefore it has no setter method to bind the value to.";

        private T _value;

        // ReSharper disable once MemberCanBeProtected.Global
        public LiveData(T value)
        {
            _value = value;
        }

        // ReSharper disable once MemberCanBeProtected.Global
        public LiveData() : this(default)
        {
        }

        public event EventHandler<EventArgs> PropertyChanged;

        /// <summary>
        /// Returns the current value.
        /// </summary>
        // ReSharper disable once MemberCanBeProtected.Global
        public virtual T Value
        {
            get => _value;
            protected set => SetValue(value);
        }

        public void BindProperty<Target>(Target target, Expression<Func<Target, T>> propertyLambda)
        {
            var expr = propertyLambda.Body as MemberExpression;
            if(expr == null)
                throw new ArgumentException(MessageExpressionLambdaDoesNotReturnAProperty, nameof(propertyLambda));

            var prop = expr.Member as PropertyInfo;
            if (prop == null)
                throw new ArgumentException(MessageExpressionLambdaDoesNotReturnAProperty, nameof(propertyLambda));

            if (prop.CanWrite == false)
                throw new ArgumentException(MessagePropertyHasNoSetter, nameof(propertyLambda));

            prop.SetValue(target, Value, null);
            PropertyChanged += (sender, args) => prop.SetValue(target, Value, null);
        }

        public static implicit operator T(LiveData<T> liveData) => liveData.Value;

        protected virtual void OnPropertyChanged() => PropertyChanged?.Invoke(this, EventArgs.Empty);

        private void SetValue(T value)
        {
            if (value.Equals(_value))
                return;
            _value = value;
            OnPropertyChanged();
        }
    }
}