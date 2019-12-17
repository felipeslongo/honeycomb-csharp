using Foundation;
using System;
using UIKit;

namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    /// Lifecycle-Aware UIViewController.
    /// Use it to remove boilerplate code
    /// needed to invoke events from each state.
    /// </summary>
    public abstract class LifecycleUIViewController : UIViewController
    {
        private MutableLifecycleObservable lifecycleObservable = new MutableLifecycleObservable();

        public LifecycleUIViewController()
        {
        }

        public LifecycleUIViewController(NSCoder coder) : base(coder)
        {
        }

        public LifecycleUIViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected LifecycleUIViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal LifecycleUIViewController(IntPtr handle) : base(handle)
        {
        }

        public LifecycleObservable LifecycleObservable => lifecycleObservable;

        public override void LoadView()
        {
            base.LoadView();
            lifecycleObservable.NotifyLoadView();
        }

        public override void LoadViewIfNeeded()
        {
            base.LoadViewIfNeeded();
            lifecycleObservable.NotifyLoadViewIfNeeded();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            lifecycleObservable.NotifyViewDidLoad();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            lifecycleObservable.NotifyViewWillAppear();
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            lifecycleObservable.NotifyViewWillLayoutSubviews();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            lifecycleObservable.NotifyViewDidLayoutSubviews();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            lifecycleObservable.NotifyViewDidAppear();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            lifecycleObservable.NotifyViewWillDisappear();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            lifecycleObservable.NotifyViewDidDisappear();
        }
    }
}
