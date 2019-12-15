using Android.Arch.Lifecycle;
using Android.Support.V4.App;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    /// Provides extension methods to facilitate the
    /// registration of a constructed ViewModel
    /// into an Activity or Fragment scope.
    ///
    /// The motivation was to make the intention of
    /// registering into the scope more clear.
    /// </summary>
    /// <remarks>
    /// Credits:
    ///     https://proandroiddev.com/view-model-creation-in-android-android-architecture-components-kotlin-ce9f6b93a46b
    /// </remarks>
    public static class ViewModelProviderRegisterViewModelScope
    {
        /// <summary>
        /// Register the ViewModel into the Fragment scope.
        /// </summary>
        /// <typeparam name="TViewModel">Android ViewModel</typeparam>
        /// <param name="this">Fragment with scope.</param>
        /// <param name="viewModel">ViewModel to be registered.</param>
        public static void RegisterViewModelScope<TViewModel>(this Fragment @this, TViewModel viewModel) where TViewModel : ViewModel =>
            @this.GetViewModel(() => viewModel);

        /// <summary>
        /// Register the ViewModel into the Activity scope.
        /// </summary>
        /// <typeparam name="TViewModel">Android ViewModel</typeparam>
        /// <param name="this">Activity with scope.</param>
        /// <param name="viewModel">ViewModel to be registered.</param>
        public static void RegisterViewModelScope<TViewModel>(this FragmentActivity @this, TViewModel viewModel) where TViewModel : ViewModel =>
            @this.GetViewModel(() => viewModel);
    }
}
