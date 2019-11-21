using Android.Content.Res;
using Android.Widget;
using HoneyComb.Platform.Android.Widget;
using System;

namespace HoneyComb.LiveDataNet.Platform.Android.DataBinding
{
    public static class LiveDataBindTextViewTextColor
    {
        /// <summary>
        /// Bind an <see cref="ColorStateList"/> resource to an <see cref="TextView.SetTextColor(ColorStateList)"/> setter.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{Int32}"/> with an <see cref="ColorStateList"/> resource.</param>
        /// <param name="textView">Target</param>
        /// <returns>Binding <see cref="IDisposable"/></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindTextViewTextColor(this LiveData<int> liveData, TextView textView) =>
            liveData.BindMethod(textView.SetTextColorStateList);
    }
}
