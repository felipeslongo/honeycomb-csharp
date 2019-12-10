using System;
using Android.Views;
using HoneyComb.Core.Lifecycles;

namespace HoneyComb.LiveDataNet.Platform.Android.DataBinding
{
    public static class LiveDataBindViewVisibility
    {
        /// <summary>
        ///     Bind an <see cref="ViewStates" /> to an <see cref="View.Visibility" /> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{ViewStates}" /> representing the Visibility state.</param>
        /// <param name="view">Target</param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindViewVisibility(this LiveData<ViewStates> liveData, View view)
        {
            return liveData.BindMethod(viewState => view.Visibility = viewState);
        }

        /// <summary>
        ///     Bind an <see cref="ViewStates" /> to an <see cref="View.Visibility" /> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{ViewStates}" /> representing the Visibility state.</param>
        /// <param name="lifecycleOwner">HoneyComb LifecycleOwner</param>
        /// <param name="view">Target</param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindViewVisibility(this LiveData<ViewStates> liveData, ILifecycleOwner lifecycleOwner,
            View view)
        {
            return liveData.BindMethod(lifecycleOwner, viewState => view.Visibility = viewState);
        }

        /// <summary>
        ///     Bind an <see cref="bool" /> to an <see cref="View.Visibility" /> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{bool}" /> representing the Visibility state.</param>
        /// <param name="view">Target</param>
        /// <param name="falseViewState">
        ///     State used when <paramref name="liveData" /> is false. Default to
        ///     <see cref="ViewStates.Gone" />.
        /// </param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindViewVisibility(this LiveData<bool> liveData, View view,
            ViewStates falseViewState = ViewStates.Gone)
        {
            return liveData.BindMethod(visibility =>
                view.Visibility = ConvertBooleanToViewState(visibility, falseViewState));
        }

        /// <summary>
        ///     Bind an <see cref="bool" /> to an <see cref="View.Visibility" /> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{bool}" /> representing the Visibility state.</param>
        /// <param name="lifecycleOwner">HoneyComb LifecycleOwner</param>
        /// <param name="view">Target</param>
        /// <param name="falseViewState">
        ///     State used when <paramref name="liveData" /> is false. Default to
        ///     <see cref="ViewStates.Gone" />.
        /// </param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindViewVisibility(this LiveData<bool> liveData, ILifecycleOwner lifecycleOwner,
            View view, ViewStates falseViewState = ViewStates.Gone)
        {
            return liveData.BindMethod(lifecycleOwner,
                visibility => view.Visibility = ConvertBooleanToViewState(visibility, falseViewState));
        }

        private static ViewStates ConvertBooleanToViewState(bool visibility, ViewStates falseViewState)
        {
            return visibility ? ViewStates.Visible : falseViewState;
        }
    }
}
