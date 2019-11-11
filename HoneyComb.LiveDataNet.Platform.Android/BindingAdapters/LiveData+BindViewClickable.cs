using Android.Views;
using System;

namespace HoneyComb.LiveDataNet.Platform.Android.BindingAdapters
{
    public static class LiveDataBindViewClickable
    {
        /// <summary>
        /// Bind an <see cref="bool"/> to an <see cref="View.Clickable"/> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{bool}"/> representing the Clickable state.</param>
        /// <param name="textView">Target</param>
        /// <returns>Binding <see cref="IDisposable"/></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindViewClickable(this LiveData<bool> liveData, View view) =>
            liveData.Bind(() => view.Clickable);

        /// <summary>
        /// Bind an negated <see cref="bool"/> to an <see cref="View.Clickable"/> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{bool}"/> representing the negated Clickable state.</param>
        /// <param name="textView">Target</param>
        /// <returns>Binding <see cref="IDisposable"/></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindViewClickableNegation(this LiveData<bool> liveData, View view) =>
            liveData.BindMethod(valor => view.Clickable = !valor);
    }
}
