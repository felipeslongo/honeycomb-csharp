using Android.Views;
using System;

namespace HoneyComb.LiveDataNet.Platform.Android.DataBinding
{
    public static class LiveDataBindViewActivated
    {
        /// <summary>
        /// Bind an <see cref="bool"/> to an <see cref="View.Activated"/> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{bool}"/> representing the Activated state.</param>
        /// <param name="textView">Target</param>
        /// <returns>Binding <see cref="IDisposable"/></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindViewActivated(this LiveData<bool> liveData, View view) =>
            liveData.Bind(() => view.Activated);
    }
}
