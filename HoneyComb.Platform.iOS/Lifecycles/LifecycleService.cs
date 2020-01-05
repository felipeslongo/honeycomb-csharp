using HoneyComb.Core.Lifecycles;
using HoneyComb.Platform.iOS.Lifecycles.Usecases;
using UIKit;

namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    /// Provides utilities and helpers related
    /// to iOS Lifecycle Components.
    /// </summary>
    public static class LifecycleService
    {
        /// <summary>
        /// Gets an <see cref="AppLifecycleObservable"/> instance.
        /// </summary>
        /// <returns>Instance</returns>
        public static AppLifecycleObservable GetAppLifecycleObservable() => AppLifecycleObservable.Create();

        public static LifecycleDisposable GetDisposable(LifecycleObservable lifecycleObservable) =>
            new LifecycleDisposable(lifecycleObservable);

        public static ILifecycleOwner GetHoneyCombLifecycleOwner(LifecycleObservable lifecycleObservable) =>
            new LifecycleOwnerHoneyComb(lifecycleObservable);

        /// <summary>
        /// Determine if it is being dismissed or removed as a child view controller.
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static bool IsBeingDismissedOrRemoved(UIViewController controller) =>
            IsUIViewControllerBeingDismissedOrPoppedOfFromTheNavigationControllUsecase.Execute(controller);

        internal static MutableLifecycleObservable GetMutableObservable(UIViewController controller) =>
            new MutableLifecycleObservable();
    }
}