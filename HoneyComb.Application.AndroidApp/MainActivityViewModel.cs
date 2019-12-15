using HoneyComb.LiveDataNet;

namespace HoneyComb.Application.AndroidApp
{
    public class MainActivityViewModel
    {
        private MutableLiveEvent<string> snackbar = new MutableLiveEvent<string>();
        private MutableLiveData<string> text = new MutableLiveData<string>("Initial Text");

        public LiveEvent<string> Snackbar => snackbar;
        public LiveData<string> Text => text;

        public void NotifyActionButtonClicked() => snackbar.Invoke("Replace with your own action");
        public void NotifyTextChanged(string text) => this.text.Value = text;
    }
}