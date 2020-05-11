using HoneyComb.LiveDataNet;
using HoneyComb.UI.Dialogs;

namespace HoneyComb.Application.AndroidApp
{
    public class MainActivityViewModel
    {
        private MutableLiveEvent<string> snackbar = new MutableLiveEvent<string>("Initial Snackbar State");
        private MutableLiveData<string> text = new MutableLiveData<string>("Initial Text");

        public MainActivityViewModel()
        {
            InitConfirmationViewModel();
        }

        public ConfirmationViewModel ConfirmationViewModel { get; private set; } = new ConfirmationViewModel();
        public LiveEvent<string> Snackbar => snackbar;
        public LiveData<string> Text => text;

        public void NotifyActionButtonClicked() => snackbar.Invoke("Replace with your own action");
        public void NotifyTextChanged(string text) => this.text.Value = text;

        public void RestoreViewModel(ConfirmationViewModel restoredViewModel)
        {
            if (ReferenceEquals(restoredViewModel, ConfirmationViewModel))
                return;

            ConfirmationViewModel.Dispose();
            ConfirmationViewModel = restoredViewModel;
            InitConfirmationViewModel();
        }

        private void InitConfirmationViewModel()
        {
            ConfirmationViewModel.OnReceivedUserIntentOnRestoration.SubscribeToExecuteIfUnhandled(userIntent =>
            {
                if (userIntent.Confirmed is false)
                    return;

                snackbar.Invoke($"User Intent after restoration process with IntentId of {userIntent.ViewState.IntentId}");
            });
        }
    }
}