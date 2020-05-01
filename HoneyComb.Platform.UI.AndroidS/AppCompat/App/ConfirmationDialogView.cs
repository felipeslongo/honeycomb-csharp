using Android.Support.V4.App;
using HoneyComb.Core.Lifecycles;

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

        public string FragmentTag => typeof(ConfirmationDialogFragment).FullName;

        public bool IsFragmentVisible => GetDialogFragment() != null;

        private void DismissDialogFragment()
        {
            var currentFragment = GetDialogFragment();
            if (currentFragment is null) return;

            currentFragment.Dismiss();
        }

        private ConfirmationDialogFragment? GetDialogFragment()
        {
            var fragmentManager = activity.SupportFragmentManager;
            var currentFragment = (ConfirmationDialogFragment)fragmentManager.FindFragmentByTag(FragmentTag);
            return currentFragment;
        }

        private void Init()
        {
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
    }
}
