using Android.Arch.Lifecycle;
using Android.OS;
using HoneyComb.Core.Vault;
using HoneyComb.Platform.Android.OS;
using HoneyComb.UI;
using HoneyComb.UI.Dialogs;

namespace HoneyComb.Platform.UI.AndroidS.AppCompat.App
{
    /// <summary>
    ///     Android ViewModel for a Confirmation Alert Dialog workflow.
    /// </summary>
    public sealed class ConfirmationAndroidViewModel : ViewModel, IRestorableUIState
    {
        public ConfirmationAndroidViewModel(ConfirmationViewModel viewModelCore)
        {
            ViewModelCore = viewModelCore;
        }

        public ConfirmationAndroidViewModel()
        {
        }

        public string? RestorationIdentifier
        {
            get => ViewModelCore.RestorationIdentifier;
            set => ViewModelCore.RestorationIdentifier = value;
        }

        public ConfirmationViewModel ViewModelCore { get; } = new ConfirmationViewModel();

        public void OnPreservation(IBundleCoder savedInstanceState) =>
            ViewModelCore.OnPreservation(savedInstanceState);

        public void OnRestoration(IBundleCoder savedInstanceState) =>
            ViewModelCore.OnRestoration(savedInstanceState);

        public void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            if (savedInstanceState is null)
                return;

            OnRestoration(new BundleCoderForAndroid(savedInstanceState));
        }

        public void OnSaveInstanceState(Bundle savedInstanceState) =>
                    OnPreservation(new BundleCoderForAndroid(savedInstanceState));
    }
}