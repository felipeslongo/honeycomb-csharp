using Android.Support.V7.App;
using HoneyComb.Platform.Android.Lifecycle;

namespace HoneyComb.Platform.Android.AppCompat.App
{
    /// <summary>
    /// Companion object that provides some
    /// utilities to an Activity
    /// </summary>
    public sealed class ActivityCompanion
    {
        public ActivityCompanion(AppCompatActivity activity)
        {
            LifecycleOwners = new LifecycleOwners(activity);
        }

        public LifecycleOwners LifecycleOwners { get; }
    }
}
