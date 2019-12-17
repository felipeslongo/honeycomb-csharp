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
        protected void NotifyLoadView() => InvokeLoadView();

        protected void NotifyLoadViewIfNeeded() => InvokeLoadViewIfNeeded();

        protected void NotifyViewDidLoad() => InvokeViewDidLoad();

        protected void NotifyViewWillAppear() => InvokeViewWillAppear();

        protected void NotifyViewWillLayoutSubviews() => InvokeViewWillLayoutSubviews();

        protected void NotifyViewDidLayoutSubviews() => InvokeViewDidLayoutSubviews();

        protected void NotifyViewDidAppear() => InvokeViewDidAppear();

        protected void NotifyViewWillDisappear() => InvokeViewWillDisappear();

        protected void NotifyViewDidDisappear() => InvokeViewDidDisappear();
    }
}
