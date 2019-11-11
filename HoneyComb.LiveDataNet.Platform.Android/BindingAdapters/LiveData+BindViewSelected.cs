using Android.Views;
using System;

namespace HoneyComb.LiveDataNet.Platform.Android.BindingAdapters
{
    public static class LiveDataBindViewSelected
    {
        /// <summary>
        /// Bind an <see cref="bool"/> to an <see cref="View.Selected"/> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{bool}"/> representing the Selected state.</param>
        /// <param name="textView">Target</param>
        /// <returns>Binding <see cref="IDisposable"/></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindViewSelected(this LiveData<bool> liveData, View view) =>
            liveData.Bind(() => view.Selected);
    }
}
