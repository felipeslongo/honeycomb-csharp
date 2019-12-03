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
    /// </remarks>
    public abstract class ViewModelHoneyComb : ViewModel
    {
        private readonly CancellationTokenSource _viewModelScopeToken = new CancellationTokenSource();

        public CancellationToken ViewModelScope => _viewModelScopeToken.Token;

        protected override void OnCleared()
        {
            base.OnCleared();
            _viewModelScopeToken.Cancel();
        }
    }
}
