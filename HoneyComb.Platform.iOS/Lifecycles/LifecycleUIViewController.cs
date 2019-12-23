using Foundation;
using HoneyComb.Platform.iOS.UIKitX;
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
        public LifecycleUIViewController()
        {
            Companion = new UIViewControllerCompanion(this);
        }

        public LifecycleUIViewController(NSCoder coder) : base(coder)
        {
            Companion = new UIViewControllerCompanion(this);
        }

        public LifecycleUIViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            Companion = new UIViewControllerCompanion(this);
        }

        protected LifecycleUIViewController(NSObjectFlag t) : base(t)
        {
            Companion = new UIViewControllerCompanion(this);
        }

        protected internal LifecycleUIViewController(IntPtr handle) : base(handle)
        {
            Companion = new UIViewControllerCompanion(this);
        }

        public UIViewControllerCompanion Companion { get; private set; }

        public override void LoadView()
        {
            base.LoadView();
            Companion.MutableLifecycleOwners.MutableObservable.NotifyLoadView();
        }

        public override void LoadViewIfNeeded()
        {
            base.LoadViewIfNeeded();
            Companion.MutableLifecycleOwners.MutableObservable.NotifyLoadViewIfNeeded();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Companion.MutableLifecycleOwners.MutableObservable.NotifyViewDidLoad();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            Companion.MutableLifecycleOwners.MutableObservable.NotifyViewWillAppear();
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            Companion.MutableLifecycleOwners.MutableObservable.NotifyViewWillLayoutSubviews();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            Companion.MutableLifecycleOwners.MutableObservable.NotifyViewDidLayoutSubviews();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            Companion.MutableLifecycleOwners.MutableObservable.NotifyViewDidAppear();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            Companion.MutableLifecycleOwners.MutableObservable.NotifyViewWillDisappear(this);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            Companion.MutableLifecycleOwners.MutableObservable.NotifyViewDidDisappear();
        }
    }
}
