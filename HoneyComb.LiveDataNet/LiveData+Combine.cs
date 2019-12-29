using System;

namespace HoneyComb.LiveDataNet
{
    /// <summary>
    /// Have to think if i will publish this or not.
    /// https://medium.com/androiddevelopers/livedata-beyond-the-viewmodel-reactive-patterns-using-transformations-and-mediatorlivedata-fda520ba00b7
    /// </summary>
    internal static class LiveData_Combine
    {
        public static MediatorLiveData<TValueCombined> Combine<TValueCombined, TValueFromThis, TValueFromOther>(
            this LiveData<TValueFromThis> @this, 
            LiveData<TValueFromOther> otherLiveData,
            Func<TValueFromThis, TValueFromOther, TValueCombined> onChange
            )
        {
            var source1Emitted = false;
            var source2Emitted = false;

            var result = new MediatorLiveData<TValueCombined>();

            Action mergeFunction = () =>
            {
                var source1Value = @this.Value;
                var source2Value = otherLiveData.Value;

                if (source1Emitted && source2Emitted)
                    result.Value = onChange(source1Value, source2Value);
            };

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
        }
    }
}
