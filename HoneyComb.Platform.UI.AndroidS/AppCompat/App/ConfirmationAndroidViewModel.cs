using Android.Arch.Lifecycle;
using HoneyComb.UI.Dialogs;

namespace HoneyComb.Platform.UI.AndroidS.AppCompat.App
{
    /// <summary>
    ///     Android ViewModel for a Confirmation Alert Dialog workflow. 
    /// </summary>
    public sealed class ConfirmationAndroidViewModel : ViewModel
    {
        public ConfirmationViewModel ViewModelCore { get; } = new ConfirmationViewModel();
    }
}