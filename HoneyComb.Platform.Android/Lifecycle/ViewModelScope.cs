using System.Threading;
using Android.Arch.Lifecycle;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    /// <see cref="CancellationTokenSource"/> tied to a <see cref="ViewModel"/>.
    /// This scope will be canceled when ViewModel will be cleared, i.e <see cref="ViewModel.OnCleared"/> is called.
    /// 
    /// This scope is bound to the main UI Thread.
    /// 
    /// Its important to call <see cref="NotifyOnCleared"/> on your <see cref="ViewModel.OnCleared"/>.
    /// </summary>
    /// Credits, Authors and Inspirations:
    ///     https://medium.com/androiddevelopers/easy-coroutines-in-android-viewmodelscope-25bffb605471
    ///     https://android.googlesource.com/platform/frameworks/support/+/refs/heads/androidx-master-dev/lifecycle/lifecycle-viewmodel-ktx/src/main/java/androidx/lifecycle/ViewModel.kt
    /// </remarks>
    public sealed class ViewModelScope
    {
        private readonly CancellationTokenSource _scoppedTokenSource = new CancellationTokenSource();

        public CancellationToken Token => _scoppedTokenSource.Token;

        public void NotifyOnCleared()
        {
            _scoppedTokenSource.Cancel();
        }
    }
}
