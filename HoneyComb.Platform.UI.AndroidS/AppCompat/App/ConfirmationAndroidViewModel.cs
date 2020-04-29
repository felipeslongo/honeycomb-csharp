using Android.Arch.Lifecycle;
using HoneyComb.UI.Dialogs;

namespace HoneyComb.Platform.UI.AndroidS.AppCompat.App
{
    public sealed class ConfirmationAndroidViewModel : ViewModel
    {
        public ConfirmationViewModel ViewModelCore { get; } = new ConfirmationViewModel();
    }
}