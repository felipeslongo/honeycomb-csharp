using HoneyComb.Reflection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace HoneyComb.LiveDataNet
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
    public abstract class LiveData<T> : IObservable<EventArgs>, IDisposable
    {
        public const string MessageExpressionLambdaDoesNotReturnAProperty = "The expression lambda does not represent a MemberExpression that returns whose member is a PropertyInfo";
        public const string MessagePropertyHasNoSetter = "The property passed to the bind method is readonly, therefore it has no setter method to bind the value to.";

        private T _value;
        private readonly Lazy<IObservable<EventArgs>> _asObservable;

        // ReSharper disable once MemberCanBeProtected.Global
        public LiveData(T value)
        {
            _value = value;

            _asObservable = new Lazy<IObservable<EventArgs>>(() => Observable.FromEventPattern<EventArgs>(
                eventHandler => PropertyChanged += eventHandler,
                eventHandler => PropertyChanged -= eventHandler
                ).Select(eventPattern => eventPattern.EventArgs));
        }

        // ReSharper disable once MemberCanBeProtected.Global
        public LiveData() : this(default!)
        {
        }

        public event EventHandler<EventArgs>? PropertyChanged;

        /// <summary>
        /// Returns the current value.
        /// </summary>
        // ReSharper disable once MemberCanBeProtected.Global
        public T Value
        {
            get => _value;
            protected set => SetValue(value);
        }

        /// <summary>
        /// <see cref="SynchronizationContext"/> used in <see cref="PostValue(T)"/>.
        /// Default value is null/>
        /// </summary>
        public SynchronizationContext? SynchronizationContext { get; protected set; }

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

        public IDisposable Bind(MutableLiveData<T> otherLiveData) =>
            BindEventHandler((_, __) => otherLiveData.Value = Value);

        public IDisposable Bind(Expression<Func<T>> propertyLambda)
        {
            var accessor = new Accessor<T>(propertyLambda);
            return BindEventHandler((_, __) => accessor.Set(Value));
        }

        public IDisposable Bind(ILifecycleOwner lifecycleOwner, Expression<Func<T>> propertyLambda)
        {
            var accessor = new Accessor<T>(propertyLambda);
            return BindEventHandler(lifecycleOwner, (_, __) => accessor.Set(Value));
        }

        public IDisposable BindMethod(Action<T> method) =>
            BindEventHandler((_, __) => method(Value));

        public IDisposable BindMethod(ILifecycleOwner lifecycleOwner, Action<T> method) =>
            BindEventHandler(lifecycleOwner, (_, __) => method(Value));

        public IDisposable BindMethod(Action method) =>
            BindEventHandler((_, __) => method());

        public IDisposable BindMethod(ILifecycleOwner lifecycleOwner, Action method) =>
            BindEventHandler(lifecycleOwner, (_, __) => method());

        public IDisposable BindEventHandler(EventHandler<EventArgs> eventHandler)
        {
            eventHandler(this, EventArgs.Empty);
            PropertyChanged += eventHandler;

            return Disposable.Create(() => PropertyChanged -= eventHandler);
        }

        public IDisposable BindEventHandler(ILifecycleOwner lifecycleOwner, EventHandler<EventArgs> eventHandler)
        {
            var wrapper = new LifecycleBoundObserver(eventHandler);
#pragma warning disable IDE0067 // Dispose objects before losing scope
            var lifecycleSubscription = lifecycleOwner.Lifecycle.Subscribe(wrapper);
#pragma warning restore IDE0067 // Dispose objects before losing scope
            PropertyChanged += wrapper.Invoke;
            wrapper.Subscription = Disposable.Create(() =>
            {
                PropertyChanged -= wrapper.Invoke;
                lifecycleSubscription.Dispose();
            });

            return wrapper;
        }

        public static implicit operator T(LiveData<T> liveData) => liveData.Value;

        protected virtual void OnPropertyChanged() => PropertyChanged?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Sets the value. If there are active observers, the value will be dispatched to them.
        /// This method must be called from the main thread. If you need set a value from a background thread, you can use postValue(Object)
        /// </summary>
        /// <param name="value"></param>
        /// <seealso cref="https://developer.android.com/reference/android/arch/lifecycle/MutableLiveData#setvalue"/>
        private void SetValue(T value)
        {
            if (IsValueUnchanged(value))
                return;
            _value = value;
            OnPropertyChanged();
        }

        /// <summary>
        /// Posts a task asynchronously to the current <see cref="SynchronizationContext"/> to set the given value.
        /// If there are active observers, the value will be dispatched to them in that context.
        /// If context is null then <see cref="SetValue(T)"/> is called synchronously instead.
        /// </summary>
        /// <param name="value"></param>
        /// <seealso cref="https://developer.android.com/reference/android/arch/lifecycle/MutableLiveData#postvalue"/>
        protected void PostValue(T value)
        {
            if (IsValueUnchanged(value))
                return;
            _value = value;

            if (SynchronizationContext == null)
            {
                OnPropertyChanged();
                return;
            }

            SynchronizationContext.Post(_ => OnPropertyChanged(), null);
        }

        private bool IsValueUnchanged(T value) => EqualityComparer<T>.Default.Equals(value, _value);

        public IDisposable Subscribe(IObserver<EventArgs> observer) => _asObservable.Value.Subscribe(observer);

        public virtual void Dispose() => PropertyChanged = null;
    }
}
