using System;

namespace HoneyComb.LiveDataNet
{
    /// <summary>
    /// https://medium.com/androiddevelopers/livedata-beyond-the-viewmodel-reactive-patterns-using-transformations-and-mediatorlivedata-fda520ba00b7
    /// </summary>
    public static class LiveData_Combine
    {
        /// <summary>
        /// Sets the value to the result of a function that is called when both `LiveData`s have data
        /// or when they receive updates after that.
        /// </summary>
        /// <typeparam name="TValueCombined"></typeparam>
        /// <typeparam name="TValueFromThis"></typeparam>
        /// <typeparam name="TValueFromOther"></typeparam>
        /// <param name="this"></param>
        /// <param name="otherLiveData"></param>
        /// <param name="onChange"></param>
        /// <returns></returns>
        public static MediatorLiveData<TValueCombined> Combine
            <TValueCombined, TValueFromThis, TValueFromOther>(
            this LiveData<TValueFromThis> @this,
            LiveData<TValueFromOther> otherLiveData,
            Func<TValueFromThis, TValueFromOther, TValueCombined> onChange
            )
        {
            var source1Emitted = false;
            var source2Emitted = false;

            var result = new MediatorLiveData<TValueCombined>();

            result.AddSource(@this, _ =>
            {
                source1Emitted = true;
                mergeFunction();
            });

            result.AddSource(otherLiveData, _ =>
            {
                source2Emitted = true;
                mergeFunction();
            });

            return result;

            void mergeFunction()
            {
                var source1Value = @this.Value;
                var source2Value = otherLiveData.Value;

                if (source1Emitted && source2Emitted)
                    result.Value = onChange(source1Value, source2Value);
            }
        }
    }
}
