namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    ///     Simplified facade/wrapper around <see cref="MutableLifecycleObservable"/>
    ///     that exposes the bare minimum lifecycle notifications that you MUST
    ///     call in your UIViewController in order to comply to HoneyComb Lifecycle.
    ///     
    ///     Keep note that you will have an <see cref="LifecycleObservable"/>
    ///     instance that will not dispatch events for every single lifecycle event of iOS
    ///     if you choose to use this slim/simplified approach.
    /// </summary>
    public class MutableLifecycleObservableSimplified
    {
        private MutableLifecycleObservable mutableObservable;

        public MutableLifecycleObservableSimplified(MutableLifecycleObservable mutableObservable)
        {
            this.mutableObservable = mutableObservable;
        }

        public void NotifyViewWillAppear() => mutableObservable.NotifyViewWillAppear();
        public void NotifyViewDidAppear() => mutableObservable.NotifyViewDidAppear();
        public void NotifyViewWillDisappear(UIKit.UIViewController controller) => mutableObservable.NotifyViewWillDisappear(controller);
        public void NotifyViewDidDisappear() => mutableObservable.NotifyViewDidDisappear();
    }
}
