using System;

namespace HoneyComb.LiveDataNet
{
    public interface ILifecycleOwner
    {
        IDisposable Subscribe(ILifecycleObserver lifecycleObserver);
    }
}
