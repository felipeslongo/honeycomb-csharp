using Foundation;
using UIKit;

namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    ///     iOS Application Global Lifecycle-aware observer.
    ///     Use it as a Observable object to execute
    ///     code that is lifecycle dependent of the App lifecycle.
    /// </summary>
    public sealed class AppLifecycleObservable
    {
        public AppLifecycleObservable()
        {
            NSNotificationCenter.DefaultCenter.AddObserver(,);
        }
        
        /// <summary>
        ///     Gets the Global Application Lifecycle <see cref="UIApplicationState" /> current state.
        /// </summary>
        public UIApplicationState StateCurrent => UIApplication.SharedApplication.ApplicationState;

        /// <summary>
        ///     Check if the app is running in the background
        /// </summary>
        /// <remarks>https://stackoverflow.com/questions/5835806/is-there-any-way-to-check-if-ios-app-is-in-background</remarks>
        public bool IsInBackground => StateCurrent == UIApplicationState.Background ||
                                      StateCurrent == UIApplicationState.Inactive;
    }
}
