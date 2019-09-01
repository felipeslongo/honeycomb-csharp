using System;
using System.Reactive.Disposables;

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

        public IDisposable TwoWayBind(MutableLiveData<T> other)
        {
            var otherToThisBind = other.Bind(this);
            var thisToOtherBind = Bind(other);

            return new CompositeDisposable(otherToThisBind, thisToOtherBind);
        }
    }
}