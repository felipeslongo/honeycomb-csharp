using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using HoneyComb.Platform.Android.Fragments.App;
using System;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace HoneyComb.Platform.UI.AndroidS.AppCompat.App
{
    /// <summary>
    ///     DialogFragment for a Confirmation Alert Dialog workflow.
    /// </summary>
    public sealed class ConfirmationDialogFragment : AppCompatDialogFragment
    {
        private FragmentCompanion<ConfirmationAndroidViewModel>? companion;
        private Func<ConfirmationAndroidViewModel> viewModelFactory;

        /// <summary>
        ///     Empty constructor to be used by the Android Framework Lifecycle only.
        /// </summary>
        public ConfirmationDialogFragment()
        {
            viewModelFactory = () => new ConfirmationAndroidViewModel();
        }

        public ConfirmationDialogFragment(ConfirmationAndroidViewModel viewModel)
        {
            viewModelFactory = () => viewModel;
        }

        public ConfirmationAndroidViewModel? ViewModel => companion?.ViewModel;

        public override async void OnCancel(IDialogInterface dialog)
        {
            base.OnCancel(dialog);
            await companion!.ViewModel.ViewModelCore.NotifyCancellationAsync();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            companion = new FragmentCompanion<ConfirmationAndroidViewModel>(this, viewModelFactory);
            companion.DestroyedForRecreation.HasBeen.NotifySavedInstanceState(savedInstanceState);
            companion.ViewModel.OnRestoreInstanceState(savedInstanceState);
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var vm = companion!.ViewModel.ViewModelCore;

            return new AlertDialog.Builder(Context)
                .SetTitle(vm.Title)
                .SetMessage(vm.Message)
                .SetPositiveButton(vm.Confirm, OnPositiveButtonClicked)
                .SetNegativeButton(vm.Cancel, OnNegativeButtonClicked)
                .Create();

            async void OnPositiveButtonClicked(object sender, DialogClickEventArgs e) =>
                await companion!.ViewModel.ViewModelCore.NotifyConfirmationAsync();

            async void OnNegativeButtonClicked(object sender, DialogClickEventArgs e) =>
                await companion!.ViewModel.ViewModelCore.NotifyCancellationAsync();
        }

        public override async void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            if (companion!.DestroyedForRecreation.IsBeing)//TODO: Use the ViewModel to see if OnCleared has been called...
                return;
            await companion!.ViewModel.ViewModelCore.NotifyCancellationAsync();
        }

        public override void OnSaveInstanceState(Bundle savedInstanceState)
        {
            base.OnSaveInstanceState(savedInstanceState);
            companion!.ViewModel.OnSaveInstanceState(savedInstanceState);
            companion!.DestroyedForRecreation.IsBeing.NotifyOnSaveInstanceState(savedInstanceState);
        }
    }
}
