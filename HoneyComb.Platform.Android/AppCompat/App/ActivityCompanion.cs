using Android.Arch.Lifecycle;
using Android.Support.V7.App;
using HoneyComb.Platform.Android.Lifecycle;
using HoneyComb.Platform.Android.OS;
using System;

namespace HoneyComb.Platform.Android.AppCompat.App
{
    /// <summary>
    /// Companion object that provides some
    /// utilities to an Activity
    /// </summary>
    public class ActivityCompanion
    {
        public ActivityCompanion(AppCompatActivity activity)
        {
            LifecycleOwners = new LifecycleOwners(activity);
        }

        public DestroyedForRecreation DestroyedForRecreation { get; } = new DestroyedForRecreation();

        public LifecycleOwners LifecycleOwners { get; }
    }

    /// <summary>
    /// Companion object that provides some
    /// utilities to an Activity
    /// </summary>
    /// <typeparam name="TViewModel">Type of Activity ViewModel</typeparam>
    public sealed class ActivityCompanion<TViewModel> : ActivityCompanion where TViewModel : ViewModel
    {
        public ActivityCompanion(AppCompatActivity activity, Func<TViewModel> creator = null) : base(activity)
        {
            ViewModel = activity.GetViewModel(creator);
        }

        public TViewModel ViewModel { get; }
    }
}
