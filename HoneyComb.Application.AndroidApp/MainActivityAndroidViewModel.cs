using Android.Arch.Lifecycle;
using HoneyComb.Platform.UI.AndroidS.AppCompat.App;
using System;

namespace HoneyComb.Application.AndroidApp
{
    public class MainActivityAndroidViewModel : ViewModel
    {
        public MainActivityAndroidViewModel()
        {
            ConfirmationViewModel = new ConfirmationAndroidViewModel(ViewModelCore.ConfirmationViewModel);
        }

        public MainActivityViewModel ViewModelCore { get; } = new MainActivityViewModel();
        
        public ConfirmationAndroidViewModel ConfirmationViewModel { get; private set; }

        public void RestoreViewModel(ConfirmationAndroidViewModel restoredViewModel)
        {
            if (ReferenceEquals(restoredViewModel, ConfirmationViewModel))
                return;

            ConfirmationViewModel = restoredViewModel;
            ViewModelCore.RestoreViewModel(ConfirmationViewModel.ViewModelCore);
        }
    }
}