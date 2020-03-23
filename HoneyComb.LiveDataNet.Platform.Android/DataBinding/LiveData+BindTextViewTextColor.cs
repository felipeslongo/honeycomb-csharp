using System;
using Android.Content.Res;
using Android.Widget;
using HoneyComb.Core.Lifecycles;
using HoneyComb.Platform.Android.Widget;

namespace HoneyComb.LiveDataNet.Platform.Android.DataBinding
{
    public static class LiveDataBindTextViewTextColor
    {
        /// <summary>
        ///     Bind an <see cref="ColorStateList" /> resource to an <see cref="TextView.SetTextColor(ColorStateList)" /> setter.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{Int32}" /> with an <see cref="ColorStateList" /> resource.</param>
        /// <param name="textView">Target</param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindTextViewTextColor(this LiveData<int> liveData, TextView textView)
        {
#pragma warning disable 8622
            return liveData.BindMethod(textView.SetTextColorStateList);
#pragma warning restore 8622
        }

        /// <summary>
        ///     Bind an <see cref="ColorStateList" /> resource to an <see cref="TextView.SetTextColor(ColorStateList)" /> setter.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{Int32}" /> with an <see cref="ColorStateList" /> resource.</param>
        /// <param name="lifecycleOwner">HoneyComb LifecycleOwner</param>
        /// <param name="textView">Target</param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindTextViewTextColor(this LiveData<int> liveData, ILifecycleOwner lifecycleOwner,
            TextView textView)
        {
#pragma warning disable 8622
            return liveData.BindMethod(lifecycleOwner, textView.SetTextColorStateList);
#pragma warning restore 8622
        }
    }
}
