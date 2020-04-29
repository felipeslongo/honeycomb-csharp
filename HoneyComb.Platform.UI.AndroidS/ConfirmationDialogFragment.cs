using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using HoneyComb.Platform.Android.Fragments.App;
using System;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace HoneyComb.Platform.UI.AndroidS
{
    public sealed class ConfirmationDialogFragment : AppCompatDialogFragment
    {
        private FragmentCompanion<ConfirmationAndroidViewModel>? companion;
        private Func<ConfirmationAndroidViewModel> viewModelFactory;

        /// <summary>
        ///     Empty constructor to be used by the Android Framework Lifecycle only.
        /// </summary>
        public ConfirmationDialogFragment()
        {
            viewModelFactory = () =>
                throw new InvalidOperationException($"ViewModel not found for {nameof(ConfirmationDialogFragment)}");
        }

        public ConfirmationDialogFragment(ConfirmationAndroidViewModel viewModel)
        {
            viewModelFactory = () => viewModel;
        }

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
            if (companion!.DestroyedForRecreation.IsBeing)
                return;
            await companion!.ViewModel.ViewModelCore.NotifyCancellationAsync();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            companion!.DestroyedForRecreation.IsBeing.NotifyOnSaveInstanceState(outState);
        }
    }
}
