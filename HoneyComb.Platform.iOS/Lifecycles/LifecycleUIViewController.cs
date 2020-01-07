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
        public LifecycleUIViewController() : base()
        {
            Companion = CreateCompanion();
        }

        public LifecycleUIViewController(NSCoder coder) : base(coder)
        {
            Companion = CreateCompanion();
        }

        public LifecycleUIViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            Companion = CreateCompanion();
        }

        protected LifecycleUIViewController(NSObjectFlag t) : base(t)
        {
            Companion = CreateCompanion();
        }

        protected internal LifecycleUIViewController(IntPtr handle) : base(handle)
        {
            Companion = CreateCompanion();
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

        protected virtual UIViewControllerCompanion CreateCompanion() =>
            new UIViewControllerCompanion(this);
    }

    /// <summary>
    /// Lifecycle-Aware UIViewController.
    /// Use it to remove boilerplate code
    /// needed to invoke events from each state.
    /// 
    /// With a ViewModel generic Companion.
    /// </summary>
    public abstract class LifecycleUIViewController<TViewModel> : LifecycleUIViewController
    {
        public LifecycleUIViewController() : base()
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

        public new UIViewControllerCompanion<TViewModel> Companion => (UIViewControllerCompanion<TViewModel>)base.Companion;

        protected override UIViewControllerCompanion CreateCompanion() =>
            new UIViewControllerCompanion<TViewModel>(this, CreateViewModel());

        protected abstract TViewModel CreateViewModel();
    }
}
