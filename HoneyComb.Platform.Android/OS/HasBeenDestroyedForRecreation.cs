using Android.App;
using Android.OS;

namespace HoneyComb.Platform.Android.OS
{
    /// <summary>
    ///     Utility to help standardize the use case of detecting
    ///     if an Activity or Fragment has been destroyed
    ///     by the OS for recreation.
    /// </summary>
    public sealed class HasBeenDestroyedForRecreation
    {
        public bool Value { get; private set; }

        public static implicit operator bool(HasBeenDestroyedForRecreation @this) => @this.Value;

        /// <summary>
        ///     Call this on <see cref="Activity.OnRestoreInstanceState(Bundle)"/>
        ///     or in <see cref="Fragment.OnViewStateRestored(Bundle)"/>
        ///     or in any lifecycle method that has an SavedInstanceState bundle as parameter,
        ///     like OnCreate
        /// </summary>
        public void NotifySavedInstanceState(Bundle savedInstanceState) =>
            Value = savedInstanceState != null;
    }
}
