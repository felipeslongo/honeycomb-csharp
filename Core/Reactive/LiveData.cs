using Core.Reflection;
using Core.Threading;
using System;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

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
    public abstract class LiveData<T> : IObservable<EventArgs>
    {
        public const string MessageExpressionLambdaDoesNotReturnAProperty = "The expression lambda does not represent a MemberExpression that returns whose member is a PropertyInfo";
        public const string MessagePropertyHasNoSetter = "The property passed to the bind method is readonly, therefore it has no setter method to bind the value to.";

        private T _value;
        private IObservable<EventArgs> _asObservable;

        // ReSharper disable once MemberCanBeProtected.Global
        public LiveData(T value)
        {
            _value = value;

            _asObservable = Observable.FromEventPattern<EventArgs>(
                eventHandler => PropertyChanged += eventHandler,
                eventHandler => PropertyChanged -= eventHandler
                ).Select(eventPattern => eventPattern.EventArgs);
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

        public IDisposable BindProperty<Target>(Target target, Expression<Func<Target, T>> propertyLambda)
        {
            var propertySetter = Property.GetSetter(target, propertyLambda);
            propertySetter(Value);

            EventHandler<EventArgs> eventHandler = (sender, args) => propertySetter(Value);
            PropertyChanged += eventHandler;

            return Disposable.Create(() => PropertyChanged -= eventHandler);
        }    

        public IDisposable BindField<Target>(Target target, Expression<Func<Target, T>> fieldLambda)
        {
            var fieldSetter = Field.GetSetter(target, fieldLambda);
            fieldSetter(Value);

            EventHandler<EventArgs> eventHandler = (sender, args) => fieldSetter(Value);
            PropertyChanged += eventHandler;

            return Disposable.Create(() => PropertyChanged -= eventHandler);
        }

        public Bind Bind(Expression<Func<T>> propertyLambda)
        {
            var accessor = new Accessor<T>(propertyLambda);
            accessor.Set(Value);

            EventHandler<EventArgs> eventHandler = (sender, args) => accessor.Set(Value);
            var bind = new Bind(eventHandler);

            PropertyChanged += bind.OnLiveDataPropertyChanged;
            bind.ConfigureUnbind(() => PropertyChanged -= bind.OnLiveDataPropertyChanged);

            return bind;
        }

        public Bind BindMethod(Action<T> method)
        {
            EventHandler<EventArgs> eventHandler = (sender, args) => method(Value);
            var bind = new Bind(eventHandler);

            PropertyChanged += bind.OnLiveDataPropertyChanged;
            bind.ConfigureUnbind(() => PropertyChanged -= bind.OnLiveDataPropertyChanged);

            return bind;
        }

        public Bind BindMethod(Action method)
        {
            EventHandler<EventArgs> eventHandler = (sender, args) => method();
            var bind = new Bind(eventHandler);

            PropertyChanged += bind.OnLiveDataPropertyChanged;
            bind.ConfigureUnbind(() => PropertyChanged -= bind.OnLiveDataPropertyChanged);

            return bind;
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

        public IDisposable Subscribe(IObserver<EventArgs> observer) => _asObservable.Subscribe(observer);
    }
}