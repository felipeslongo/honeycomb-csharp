using System;
using Android.Graphics.Drawables;
using Android.Widget;
using HoneyComb.Core.Lifecycle;

namespace HoneyComb.LiveDataNet.Platform.Android.DataBinding
{
    public static class LiveDataBindTextViewDrawableTop
    {
        /// <summary>
        ///     Bind an <see cref="Drawable" /> resource to an <see cref="TextView" /> Drawable top member.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{Int32}" /> with an <see cref="Drawable" /> resource.</param>
        /// <param name="textView">Target</param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindTextViewDrawableTop(this LiveData<int> liveData, TextView textView)
        {
            return liveData.BindMethod(drawableId =>
                textView.SetCompoundDrawablesWithIntrinsicBounds(0, drawableId, 0, 0));
        }

        /// <summary>
        ///     Bind an <see cref="Drawable" /> resource to an <see cref="TextView" /> Drawable top member.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{Int32}" /> with an <see cref="Drawable" /> resource.</param>
        /// <param name="lifecycleOwner">HoneyComb LifecycleOwner</param>
        /// <param name="textView">Target</param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindTextViewDrawableTop(this LiveData<int> liveData, ILifecycleOwner lifecycleOwner,
            TextView textView)
        {
            return liveData.BindMethod(lifecycleOwner, drawableId =>
                textView.SetCompoundDrawablesWithIntrinsicBounds(0, drawableId, 0, 0));
        }
    }
}