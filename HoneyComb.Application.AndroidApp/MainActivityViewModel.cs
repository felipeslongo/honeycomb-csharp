using HoneyComb.LiveDataNet;

namespace HoneyComb.Application.AndroidApp
{
    public class MainActivityViewModel
    {
        private MutableLiveData<string> text = new MutableLiveData<string>("Initial Text");

        public LiveData<string> Text => text;

        public void NotifyTextChanged(string text) => this.text.Value = text;
    }
}