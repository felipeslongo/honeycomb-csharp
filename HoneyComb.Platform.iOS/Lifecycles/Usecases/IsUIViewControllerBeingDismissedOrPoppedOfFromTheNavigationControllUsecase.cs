using UIKit;

namespace HoneyComb.Platform.iOS.Lifecycles.Usecases
{
    /// <summary>
    ///     Determine if it is being dismissed or removed as a child view controller.
    /// </summary>
    /// <remarks>
    ///     Credits
    ///         -https://stackoverflow.com/questions/1816614/viewwilldisappear-determine-whether-view-controller-is-being-popped-or-is-showi
    ///         -https://stackoverflow.com/questions/21915249/how-to-detect-if-view-controller-is-being-popped-of-from-the-navigation-controll
    ///         -https://stackoverflow.com/questions/10248412/isbeingdismissed-not-set-in-viewwilldisappea
    /// </remarks>
    internal static class IsUIViewControllerBeingDismissedOrPoppedOfFromTheNavigationControllUsecase
    {
        public static bool Execute(UIViewController controller) =>
            controller.IsBeingDismissed ||
            controller.IsMovingFromParentViewController ||
            (controller.NavigationController?.IsBeingDismissed ?? false);
    }
}
