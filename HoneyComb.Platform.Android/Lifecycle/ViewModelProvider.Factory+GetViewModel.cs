using Android.Arch.Lifecycle;
using Android.Support.V4.App;
using Java.Lang;
using System;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    /// Provides extension methods to facilitate the
    /// construction of a ViewModel (<see cref="global::Android.Arch.Lifecycle.ViewModel"/>)
    /// using delegates as factory methods.
    /// </summary>
    /// <remarks>
    /// Credits:
    ///     https://proandroiddev.com/view-model-creation-in-android-android-architecture-components-kotlin-ce9f6b93a46b
    /// </remarks>
    public static class ViewModelProviderFactoryGetViewModel
    {
        /// <summary>
        /// Returns an existing ViewModel or creates a new one in the scope
        /// (usually, a fragment or an activity),
        /// associated with its ViewModelProvider.
        ///
        /// The created ViewModel is associated with the given scope and will be retained
        /// as long as the scope is alive(e.g. if it is an activity, until it is
        /// finished or process is killed).
        /// </summary>
        /// <typeparam name="TViewModel">Android ViewModel</typeparam>
        /// <param name="this">Fragment with scope.</param>
        /// <param name="creator">Optional delegate factory method.</param>
        /// <returns>An existing ViewModel or a new one in the scope.</returns>
        public static TViewModel GetViewModel<TViewModel>(this Fragment @this, Func<TViewModel> creator = null)
            where TViewModel : ViewModel
        {
            var javaClass = Class.FromType(typeof(TViewModel));
            var viewModel = GetViewModelFromViewModelProviders(@this, creator, javaClass);

            return (TViewModel)viewModel;
        }

        /// <summary>
        /// Returns an existing ViewModel or creates a new one in the scope
        /// (usually, a fragment or an activity),
        /// associated with its ViewModelProvider.
        ///
        /// The created ViewModel is associated with the given scope and will be retained
        /// as long as the scope is alive(e.g. if it is an activity, until it is
        /// finished or process is killed).
        /// </summary>
        /// <typeparam name="TViewModel">Android ViewModel</typeparam>
        /// <param name="this">Activity with scope.</param>
        /// <param name="creator">Optional delegate factory method.</param>
        /// <returns>An existing ViewModel or a new one in the scope.</returns>
        public static TViewModel GetViewModel<TViewModel>(this FragmentActivity @this, Func<TViewModel> creator = null)
            where TViewModel : ViewModel
        {
            var javaClass = Class.FromType(typeof(TViewModel));
            var viewModel = GetViewModelFromViewModelProviders(@this, creator, javaClass);

            return (TViewModel)viewModel;
        }

        private static object GetViewModelFromViewModelProviders<TViewModel>(Fragment fragment,
            Func<TViewModel> creator, Class javaClass)
            where TViewModel : ViewModel
        {
            if (creator is null)
                return ViewModelProviders.Of(fragment).Get(javaClass);

            return ViewModelProviders.Of(fragment, new ViewModelFactory<TViewModel>(creator)).Get(javaClass);
        }

        private static object GetViewModelFromViewModelProviders<TViewModel>(FragmentActivity activity,
            Func<TViewModel> creator, Class javaClass)
            where TViewModel : ViewModel
        {
            if (creator is null)
                return ViewModelProviders.Of(activity).Get(javaClass);

            return ViewModelProviders.Of(activity, new ViewModelFactory<TViewModel>(creator)).Get(javaClass);
        }
    }
}
