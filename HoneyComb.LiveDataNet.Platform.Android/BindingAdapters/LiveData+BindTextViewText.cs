using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoneyComb.LiveDataNet.Platform.Android.BindingAdapters
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
