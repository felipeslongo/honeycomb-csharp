using Android.Arch.Lifecycle;

namespace HoneyComb.Application.AndroidApp
{
    public class MainActivityAndroidViewModel : ViewModel
    {
        public MainActivityViewModel ViewModel { get; } = new MainActivityViewModel();
    }
}