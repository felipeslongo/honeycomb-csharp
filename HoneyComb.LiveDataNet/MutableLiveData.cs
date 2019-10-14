using System;
using System.Reactive.Disposables;
using System.Threading;

namespace HoneyComb.LiveDataNet
{
    /// <summary>
    /// <see cref="LiveData{T}"/> which publicly exposes mutability
    /// </summary>
    /// <see cref="https://developer.android.com/reference/androidx/lifecycle/MutableLiveData.html"/>
    public class MutableLiveData<T> : LiveData<T>
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

        public new SynchronizationContext? SynchronizationContext
        {
            get => base.SynchronizationContext;
            set => base.SynchronizationContext = value;
        }

        /// <summary><inheritdoc cref="LiveData.PostValue"/></summary>
        public new void PostValue(T value) => base.PostValue(value);

        public IDisposable TwoWayBind(MutableLiveData<T> other)
        {
            var otherToThisBind = other.Bind(this);
            var thisToOtherBind = Bind(other);

            return new CompositeDisposable(otherToThisBind, thisToOtherBind);
        }
    }
}
