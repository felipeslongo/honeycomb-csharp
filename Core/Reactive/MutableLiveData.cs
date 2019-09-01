using System;

namespace Core.Reactive
{
    /// <summary>
    /// <see cref="LiveData{T}"/> which publicly exposes mutability
    /// </summary>
    /// <see cref="https://developer.android.com/reference/androidx/lifecycle/MutableLiveData.html"/>
    public class MutableLiveData<T> : LiveData<T>, IDisposable
    {
        public MutableLiveData(T value) : base(value)
        {
        }

        public MutableLiveData() : base()
        {
        }

        public new T Value 
        { 
            get => base.Value;
            set => base.Value = value;
        }

        public new void PostValue(T value) => base.PostValue(value);

        public new void Dispose() => base.Dispose();
    }
}