using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Reactive
{
    /// <summary>
    /// Transformations for a LiveData class.
    /// 
    /// You can use transformation methods to carry information 
    /// across the observer's lifecycle. 
    /// The transformations aren't calculated 
    /// unless an observer is observing the returned LiveData object.
    /// 
    /// Because the transformations are calculated lazily, 
    /// lifecycle-related behavior is implicitly passed down 
    /// without requiring additional explicit calls or dependencies.
    /// 
    /// Inspired by Android
    /// </summary>
    /// <seealso cref="https://developer.android.com/reference/android/arch/lifecycle/Transformations"/>
    public static class Transformations
    {
        /// <summary>
        /// Applies the given function on the main thread to each value emitted by source LiveData 
        /// and returns LiveData, which emits resulting values.
        /// 
        /// The given function func will be executed on the main thread.
        /// 
        /// Suppose that you have a LiveData, named userLiveData, that contains user data
        /// and you need to display the user name, created by concatenating the first and the last name of the user.
        /// You can define a function that handles the name creation,
        /// that will be applied to every value emitted by useLiveData.
        /// </summary>
        /// <typeparam name="TIn">source LiveData type</typeparam>
        /// <typeparam name="TOut">returned LiveData type</typeparam>
        /// <param name="source"> a LiveData to listen to</param>
        /// <param name="func">a function to apply</param>
        /// <returns>a LiveData which emits resulting values</returns>
        /// <seealso cref="https://developer.android.com/reference/android/arch/lifecycle/Transformations.html#map(android.arch.lifecycle.LiveData<X>,%20android.arch.core.util.Function<X,%20Y>)"/>
        public static LiveData<TOut> Map<TIn, TOut>(LiveData<TIn> source, Func<TIn, TOut> func)
        {
            var liveData = new MutableLiveData<TOut>();
            _ = source.BindMethod(sourceValue => liveData.Value = func(sourceValue));
            return liveData;
        }
    }
}
