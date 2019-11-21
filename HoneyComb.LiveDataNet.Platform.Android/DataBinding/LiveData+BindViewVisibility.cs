using Android.Views;
using System;

namespace HoneyComb.LiveDataNet.Platform.Android.DataBinding
{
    public static class LiveDataBindViewVisibility
    {
        /// <summary>
        /// Bind an <see cref="bool"/> to an <see cref="View.Visibility"/> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{bool}"/> representing the Visibility state.</param>
        /// <param name="view">Target</param>
        /// <param name="falseViewState">State used when <paramref name="liveData"/> is false. Default to <see cref="ViewStates.Gone"/>.</param>
        /// <returns>Binding <see cref="IDisposable"/></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindViewVisibility(this LiveData<bool> liveData, View view, ViewStates falseViewState = ViewStates.Gone) =>
            liveData.BindMethod(visibility => view.Visibility = ConvertBooleanToViewState(visibility, falseViewState));

        private static ViewStates ConvertBooleanToViewState(bool visibility, ViewStates falseViewState) =>
            visibility ? ViewStates.Visible : falseViewState;
    }
}
