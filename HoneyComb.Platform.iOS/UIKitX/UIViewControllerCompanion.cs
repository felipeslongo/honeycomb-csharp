using HoneyComb.Platform.iOS.Lifecycles;
using UIKit;

namespace HoneyComb.Platform.iOS.UIKitX
{
    /// <summary>
    /// Companion object that provides some
    /// utilities to an UIViewController
    /// </summary>
    public class UIViewControllerCompanion
    {
        public UIViewControllerCompanion(UIViewController controller)
        {
            var lifecycleObservable = LifecycleService.GetMutableObservable(controller);
            MutableLifecycleOwners = new MutableLifecycleOwners(lifecycleObservable);
        }

        /// <summary>
        /// Both iOS and HoneyComb lifecycle owners.
        /// Pass this instance to other objects.
        /// </summary>
        public LifecycleOwners LifecycleOwners => MutableLifecycleOwners;

        /// <summary>
        /// Both iOS and HoneyComb lifecycle owners that let you mutate it.
        /// Use it if you favor composition instead of inheritance as lifecycle callback notificator.
        /// Dont't let this instance be touched by objects other than your UIViewController (the true Lifecycle owner).
        /// </summary>
        public MutableLifecycleOwners MutableLifecycleOwners { get; }
    }

    /// <summary>
    /// Companion object that provides some
    /// utilities to an UIViewController
    /// </summary>
    /// <typeparam name="TViewModel">Type of Activity ViewModel</typeparam>
    public sealed class UIViewControllerCompanion<TViewModel> : UIViewControllerCompanion
    {
        public UIViewControllerCompanion(UIViewController controller, TViewModel viewModel) : base(controller)
        {
            ViewModel = viewModel;
        }

        public TViewModel ViewModel { get; }
    }
}
