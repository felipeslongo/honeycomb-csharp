using Android.Arch.Lifecycle;
using Android.Support.V4.App;
using HoneyComb.Platform.Android.Lifecycle;
using HoneyComb.Platform.Android.OS;
using System;

namespace HoneyComb.Platform.Android.Fragments.App
{
    /// <summary>
    ///     Companion object that provides some
    ///     utilities to an Fragment
    /// </summary>
    public class FragmentCompanion
    {
        public FragmentCompanion(Fragment fragment)
        {
            LifecycleOwners = new LifecycleOwners(fragment);
        }

        public DestroyedForRecreation DestroyedForRecreation { get; } = new DestroyedForRecreation();

        public LifecycleOwners LifecycleOwners { get; }
    }

    /// <summary>
    ///     Companion object that provides some
    ///     utilities to an Fragment
    /// </summary>
    /// <typeparam name="TViewModel">Type of Activity ViewModel</typeparam>
    public sealed class FragmentCompanion<TViewModel> : FragmentCompanion where TViewModel : ViewModel
    {
        public FragmentCompanion(Fragment fragment, Func<TViewModel> creator = null) : base(fragment)
        {
            ViewModel = fragment.GetViewModel(creator);
        }

        public TViewModel ViewModel { get; }
    }
}
