using System;
using Android.Views;
using HoneyComb.Core.Lifecycles;

namespace HoneyComb.LiveDataNet.Platform.Android.DataBinding
{
    public static class LiveDataBindViewClickable
    {
        /// <summary>
        ///     Bind an <see cref="bool" /> to an <see cref="View.Clickable" /> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{bool}" /> representing the Clickable state.</param>
        /// <param name="textView">Target</param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindViewClickable(this LiveData<bool> liveData, View view)
        {
            return liveData.Bind(() => view.Clickable);
        }

        /// <summary>
        ///     Bind an <see cref="bool" /> to an <see cref="View.Clickable" /> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{bool}" /> representing the Clickable state.</param>
        /// <param name="textView">Target</param>
        /// <param name="lifecycleOwner">HoneyComb LifecycleOwner</param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindViewClickable(this LiveData<bool> liveData, ILifecycleOwner lifecycleOwner,
            View view)
        {
            return liveData.Bind(lifecycleOwner, () => view.Clickable);
        }

        /// <summary>
        ///     Bind an negated <see cref="bool" /> to an <see cref="View.Clickable" /> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{bool}" /> representing the negated Clickable state.</param>
        /// <param name="textView">Target</param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindViewClickableNegation(this LiveData<bool> liveData, View view)
        {
            return liveData.BindMethod(valor => view.Clickable = !valor);
        }

        /// <summary>
        ///     Bind an negated <see cref="bool" /> to an <see cref="View.Clickable" /> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{bool}" /> representing the negated Clickable state.</param>
        /// <param name="textView">Target</param>
        /// <param name="lifecycleOwner">HoneyComb LifecycleOwner</param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindViewClickableNegation(this LiveData<bool> liveData,
            ILifecycleOwner lifecycleOwner, View view)
        {
            return liveData.BindMethod(lifecycleOwner, valor => view.Clickable = !valor);
        }
    }
}