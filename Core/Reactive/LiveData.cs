using System;

namespace Core.Reactive
{
    /// <summary>
    /// LiveData is a data holder class that can be observed.
    /// Use it to create observable properties in C#.
    /// </summary>
    /// <remarks>
    /// Inspired by the LiveData class in Android Architecture Components
    /// </remarks>
    /// <typeparam name="T">Wrapped type</typeparam>
    /// <see cref="https://developer.android.com/topic/libraries/architecture/livedata"/>
    /// <see cref="https://developer.android.com/reference/androidx/lifecycle/LiveData.html"/>
    public class LiveData<T>
    {
        private T _value;
        private event EventHandler _propertyChanged;

        public LiveData(T value)
        {
            Value = value;
        }

        public LiveData() : this(default)
        {
        }
        
        public event EventHandler PropertyChanged
        {
            add
            {
                value(this, EventArgs.Empty);
                _propertyChanged += value;
            }
            remove => _propertyChanged -= value;
        }

        public T Value
        {
            get => _value;
            set => SetValue(value);
        }

        private void SetValue(T value)
        {
            _value = value;
            OnPropertyChanged();
        }

        public static implicit operator T(LiveData<T> liveData) => liveData.Value;

        protected virtual void OnPropertyChanged() => _propertyChanged?.Invoke(this, EventArgs.Empty);
    }
}