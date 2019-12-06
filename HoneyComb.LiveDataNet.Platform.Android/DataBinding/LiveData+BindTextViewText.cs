using Android.Widget;
using System;

namespace HoneyComb.LiveDataNet.Platform.Android.DataBinding
{
    public static class LiveDataBindTextViewText
    {
        /// <summary>
        /// Bind an <see cref="string"/> to an <see cref="TextView.Text"/> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{string}"/>.</param>
        /// <param name="textView">Target</param>
        /// <returns>Binding <see cref="IDisposable"/></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindTextViewText(this LiveData<string> liveData, TextView textView) =>
            liveData.Bind(() => textView.Text);
    }
}
