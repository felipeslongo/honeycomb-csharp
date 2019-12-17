namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    /// Implementation of <see cref="LifecycleObservable"/>
    /// that allows direct manipulation of its state.
    /// 
    /// Do not expose this instance to your clients.
    /// </summary>
    public class MutableLifecycleObservable : LifecycleObservable
    {
        public void NotifyLoadView() => InvokeLoadView();

        public void NotifyLoadViewIfNeeded() => InvokeLoadViewIfNeeded();

        public void NotifyViewDidLoad() => InvokeViewDidLoad();

        public void NotifyViewWillAppear() => InvokeViewWillAppear();

        public void NotifyViewWillLayoutSubviews() => InvokeViewWillLayoutSubviews();

        public void NotifyViewDidLayoutSubviews() => InvokeViewDidLayoutSubviews();

        public void NotifyViewDidAppear() => InvokeViewDidAppear();

        public void NotifyViewWillDisappear() => InvokeViewWillDisappear();

        public void NotifyViewDidDisappear() => InvokeViewDidDisappear();
    }
}
