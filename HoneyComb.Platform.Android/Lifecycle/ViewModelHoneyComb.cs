using Android.Arch.Lifecycle;
using System.Threading;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    /// Android ViewModel inherited class
    /// that comes with utilities to help
    /// reduce boilerplate code common to
    /// all ViewModels in an app.
    ///
    /// Its usage is completely optional.
    /// </summary>
    /// <remarks>
    /// Credits, Authors and Inspirations:
    ///     https://medium.com/androiddevelopers/easy-coroutines-in-android-viewmodelscope-25bffb605471
    ///     https://android.googlesource.com/platform/frameworks/support/+/refs/heads/androidx-master-dev/lifecycle/lifecycle-viewmodel/src/main/java/androidx/lifecycle/ViewModel.java?autodive=0%2F%2F%2F%2F%2F
    /// </remarks>
    public abstract class ViewModelHoneyComb : ViewModel
    {
        public ViewModelScope ViewModelScope => new ViewModelScope();

        protected override void OnCleared()
        {
            base.OnCleared();
            ViewModelScope.NotifyOnCleared();
        }
    }
}
