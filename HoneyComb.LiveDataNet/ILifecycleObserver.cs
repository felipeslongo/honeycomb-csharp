namespace HoneyComb.LiveDataNet
{
    public interface ILifecycleObserver
    {
        void OnActive();
        void OnInactive();
        void OnDisposed();
    }
}
