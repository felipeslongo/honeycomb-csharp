using System;
using Android.Views;
using HoneyComb.Core.Lifecycles;

namespace HoneyComb.LiveDataNet.Platform.Android.DataBinding
{
    public static class LiveDataBindViewActivated
    {
        /// <summary>
        ///     Bind an <see cref="bool" /> to an <see cref="View.Activated" /> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{bool}" /> representing the Activated state.</param>
        /// <param name="textView">Target</param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindViewActivated(this LiveData<bool> liveData, View view)
        {
            return liveData.Bind(() => view.Activated);
        }

        /// <summary>
        ///     Bind an <see cref="bool" /> to an <see cref="View.Activated" /> property.
        /// </summary>
        /// <param name="liveData">An <see cref="LiveData{bool}" /> representing the Activated state.</param>
        /// <param name="textView">Target</param>
        /// <param name="lifecycleOwner">HoneyComb LifecycleOwner</param>
        /// <returns>Binding <see cref="IDisposable" /></returns>
        /// <exception cref="Exception">To be added</exception>
        public static IDisposable BindViewActivated(this LiveData<bool> liveData, ILifecycleOwner lifecycleOwner,
            View view)
        {
            return liveData.Bind(lifecycleOwner, () => view.Activated);
        }
    }
}