using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    ///     iOS Application Global Lifecycle-aware observer.
    ///     Use it as a Observable object to execute
    ///     code that is lifecycle dependent of the Application Global lifecycle.
    /// </summary>
    public sealed class AppLifecycleObservable
    {
        /// <summary>
        /// There is no reason not to be a singleton instance...
        /// </summary>
        private static readonly Lazy<AppLifecycleObservable> singleton = new Lazy<AppLifecycleObservable>(() => new AppLifecycleObservable());

        private AppLifecycleObservable()
        {
            SubscribeToUIApplicationLifecycle();
        }

        public event EventHandler? DidBecomeActive;

        public event EventHandler? WillResignActive;

        public event EventHandler? WillTerminate;

        /// <summary>
        ///     Check if the app is running in the background
        /// </summary>
        /// <remarks>https://stackoverflow.com/questions/5835806/is-there-any-way-to-check-if-ios-app-is-in-background</remarks>
        public bool IsInBackground => StateCurrent == UIApplicationState.Background ||
                                      StateCurrent == UIApplicationState.Inactive;

        /// <summary>
        ///     Check if the app is running in the foreground as Active.
        /// </summary>
        /// <remarks>https://stackoverflow.com/questions/5835806/is-there-any-way-to-check-if-ios-app-is-in-background</remarks>
        public bool IsInForeground => StateCurrent == UIApplicationState.Active;

        /// <summary>
        ///     Gets the Global Application Lifecycle <see cref="UIApplicationState" /> current state.
        /// </summary>
        public UIApplicationState StateCurrent => UIApplication.SharedApplication.ApplicationState;

        internal static AppLifecycleObservable Create() => singleton.Value;

        private void Dispose()
        {
            DidBecomeActive = null;
            WillResignActive = null;
            WillTerminate = null;
        }

        /// <summary>
        /// Subscribe to the <see cref="UIApplication"/> lifecycle related notifications.
        /// </summary>
        /// <remarks>
        ///     Credits
        ///         https://stackoverflow.com/questions/9011868/whats-the-best-way-to-detect-when-the-app-is-entering-the-background-for-my-vie
        ///         https://stackoverflow.com/questions/34744783/detect-ios-app-entering-background
        /// </remarks>
        private void SubscribeToUIApplicationLifecycle()
        {
            var subscriptionTokens = new List<IDisposable>();
            subscriptionTokens.Add(SubscribeToUIApplicationDidBecomeActive());
            subscriptionTokens.Add(SubscribeToUIApplicationWillResignActive());
            subscriptionTokens.Add(SubscribeToUIApplicationWillTerminate());

            IDisposable SubscribeToUIApplicationDidBecomeActive() =>
                NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidBecomeActiveNotification, OnUIApplicationDidBecomeActive);

            IDisposable SubscribeToUIApplicationWillResignActive() =>
            NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.WillResignActiveNotification, OnUIApplicationWillResignActive);

            IDisposable SubscribeToUIApplicationWillTerminate() =>
                NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.WillTerminateNotification, OnUIApplicationWillTerminate);

            void OnUIApplicationDidBecomeActive(NSNotification _)
            {
                DidBecomeActive?.Invoke(this, EventArgs.Empty);
            }

            void OnUIApplicationWillResignActive(NSNotification _)
            {
                WillResignActive?.Invoke(this, EventArgs.Empty);
            }

            void OnUIApplicationWillTerminate(NSNotification _)
            {
                WillTerminate?.Invoke(this, EventArgs.Empty);
                subscriptionTokens.ForEach(subscription => subscription.Dispose());
                Dispose();
            }
        }
    }
}
