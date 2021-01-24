using Android.Support.V4.App;
using HoneyComb.Core.Lifecycles;
using System;
using System.Text;

namespace HoneyComb.Platform.UI.AndroidS.AppCompat.App
{
    /// <summary>
    ///     View for a Confirmation Alert Dialog workflow.
    /// </summary>
    public sealed class ConfirmationDialogView
    {
        private readonly FragmentActivity activity;
        private readonly ILifecycleOwner lifecycleOwner;
        private readonly ConfirmationAndroidViewModel viewModel;

        public ConfirmationDialogView(FragmentActivity activity,
            ILifecycleOwner lifecycleOwner,
            ConfirmationAndroidViewModel viewModel)
        {
            this.activity = activity;
            this.lifecycleOwner = lifecycleOwner;
            this.viewModel = viewModel;
            Init();
        }

        public static string FragmentTag => typeof(ConfirmationDialogFragment).FullName;

        public bool IsFragmentVisible => Fragment != null;

        private ConfirmationDialogFragment? Fragment =>
                            GetDialogFragment(activity.SupportFragmentManager);

        public static ConfirmationAndroidViewModel? GetViewModelFromSavedInstanceState(FragmentManager fragmentManager)
        {
            var fragment = GetDialogFragment(fragmentManager);
            if (fragment is null)
                return null;

            return fragment.ViewModel;
        }

        private static ConfirmationDialogFragment? GetDialogFragment(FragmentManager fragmentManager) =>
            (ConfirmationDialogFragment)fragmentManager.FindFragmentByTag(FragmentTag);

        private void DismissDialogFragment()
        {
            var currentFragment = Fragment;
            if (currentFragment is null) return;

            currentFragment.Dismiss();
        }

        private void Init()
        {
            ValidateIfFragmentViewModelIsSync();

            viewModel.ViewModelCore.Visibility.Value
                .BindMethod(lifecycleOwner, OnVisibilityChanged);
        }

        private void OnVisibilityChanged(bool isVisible)
        {
            if (isVisible)
            {
                ShowDialogFragment();
                return;
            }

            DismissDialogFragment();
        }

        private void ShowDialogFragment()
        {
            if (IsFragmentVisible)
                return;

            new ConfirmationDialogFragment(viewModel)
                .Show(activity.SupportFragmentManager, FragmentTag);
        }

        private void ValidateIfFragmentViewModelIsSync()
        {
            if (IsFragmentVisible is false)
                return;

            var fragmentViewModel = Fragment!.ViewModel!;
            if (ReferenceEquals(fragmentViewModel, viewModel) is false)
                throw new InvalidOperationException(GetMessage());

            static string GetMessage()
            {
                return new StringBuilder()
                    .AppendLine("Cannot synchronize with the current ConfirmationDialog because there is a different instance of the ViewModel than the one passed in the Constructor.")
                    .AppendLine("This can happen during Android OS Process Death where the system kills your app for memory and recreate later.")
                    .Append("In this case during the Restoration Process, you synchronize both the ConfirmationAndroidViewModel to have the same instance (you decide which one).")
                    .ToString();
            }
        }
    }
}
