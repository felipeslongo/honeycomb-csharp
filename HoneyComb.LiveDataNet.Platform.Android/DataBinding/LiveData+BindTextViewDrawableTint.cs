using System;
using Android.Widget;
using HoneyComb.Core.Lifecycles;
using HoneyComb.Platform.Android.Widget;

namespace HoneyComb.LiveDataNet.Platform.Android.DataBinding
{
    public static class LiveDataBindTextViewDrawableTint
    {
        /// <summary>
        ///     Bind a Color to an <see cref="TextView.CompoundDrawableTintList" /> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{Int32}" /> with a color state list resource.</param>
        /// <param name="textView">Target</param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindTextViewDrawableTint(this LiveData<int> liveData, TextView textView)
        {
            return liveData.BindMethod(textView.SetCompoundDrawableTintList);
        }

        /// <summary>
        ///     Bind a Color to an <see cref="TextView.CompoundDrawableTintList" /> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{Int32}" /> with a color state list resource.</param>
        /// <param name="lifecycleOwner">HoneyComb LifecycleOwner</param>
        /// <param name="textView">Target</param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindTextViewDrawableTint(this LiveData<int> liveData, ILifecycleOwner lifecycleOwner,
            TextView textView)
        {
            return liveData.BindMethod(lifecycleOwner, textView.SetCompoundDrawableTintList);
        }
    }
}