using Android.Support.V4.App;
using HoneyComb.Platform.Android.Lifecycle;

namespace HoneyComb.Platform.Android.Fragments.App
{
    /// <summary>
    ///     Companion object that provides some
    ///     utilities to an Fragment
    /// </summary>
    public sealed class FragmentCompanion
    {
        public FragmentCompanion(Fragment fragment)
        {
            LifecycleOwners = new LifecycleOwners(fragment);
        }

        public LifecycleOwners LifecycleOwners { get; }
    }
}
