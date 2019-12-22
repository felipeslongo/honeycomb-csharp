﻿using HoneyComb.Platform.iOS.Lifecycles.Usecases;
using UIKit;

namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    /// Provides utilities and helpers related 
    /// to iOS Lifecycle Components.
    /// </summary>
    public static class LifecycleService
    {
        public static LifecycleDisposable GetDisposable(LifecycleObservable lifecycleObservable) =>
            new LifecycleDisposable(lifecycleObservable);

        /// <summary>
        /// Determine if it is being dismissed or removed as a child view controller.
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static bool IsBeingDismissedOrRemoved(UIViewController controller) =>
            IsUIViewControllerBeingDismissedOrPoppedOfFromTheNavigationControllUsecase.Execute(controller);
    }
}