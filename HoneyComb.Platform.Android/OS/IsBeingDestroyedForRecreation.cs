using Android.App;
using Android.OS;

namespace HoneyComb.Platform.Android.OS
{
    /// <summary>
    ///     Utility to help standardize the use case of detecting
    ///     if an Activity or Fragment is being destroyed
    ///     by the OS for recreation.
    /// </summary>
    public sealed class IsBeingDestroyedForRecreation
    {
        public bool Value { get; private set; }

        public static implicit operator bool(IsBeingDestroyedForRecreation @this) => @this.Value;

        /// <summary>
        ///     Call this on <see cref="Activity.OnSaveInstanceState(Bundle)"/>
        ///     or in <see cref="Fragment.OnSaveInstanceState(Bundle)"/>
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public void NotifyOnSaveInstanceState(Bundle savedInstanceState) => Value = true;
    }
}
