using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using HoneyComb.LiveDataNet;
using HoneyComb.Platform.Android.AppCompat.App;
using HoneyComb.LiveDataNet.Platform.Android.DataBinding;
using HoneyComb.UI.Dialogs;
using HoneyComb.Platform.UI.AndroidS.AppCompat.App;
using HoneyComb.UI;

namespace HoneyComb.Application.AndroidApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private ActivityCompanion<MainActivityAndroidViewModel> companion;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            companion = new ActivityCompanion<MainActivityAndroidViewModel>(this);
            companion.DestroyedForRecreation.HasBeen.NotifySavedInstanceState(savedInstanceState);
            var viewModel = companion.ViewModel;
            OnRestoreInstanceStateFromOnCreate();            

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            viewModel.ViewModelCore.Text.BindTextViewText(companion.LifecycleOwners.HoneyComb, FindViewById<TextView>(Resource.Id.content_main_textview));
            FindViewById<EditText>(Resource.Id.content_main_edittext).AfterTextChanged += (_, args) =>
            {
                viewModel.ViewModelCore.NotifyTextChanged(args.Editable.ToString());
            };
            
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
            viewModel.ViewModelCore.Snackbar.SubscribeToExecuteIfUnhandled(companion.LifecycleOwners.HoneyComb, snackbar =>
            {
                Snackbar.Make(fab, snackbar, Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            });

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            _ = new ConfirmationDialogView(this, companion.LifecycleOwners.HoneyComb, viewModel.ConfirmationViewModel);

            void OnRestoreInstanceStateFromOnCreate()
            {
                if (savedInstanceState is null)
                    return;

                if (ConfirmationDialogView.GetViewModelFromSavedInstanceState(SupportFragmentManager) is { } restoredViewModel)
                    viewModel.RestoreViewModel(restoredViewModel);
            }
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private async void FabOnClick(object sender, EventArgs eventArgs)
        {
            var viewModel = companion.ViewModel;
            var result = await viewModel.ConfirmationViewModel.ViewModelCore.ShowAsync(new ConfirmationViewState(
                    new IntentId("QuerSerMorto"),
                    "Quer ser morto ?",
                    "Se voce for morto não vai ter como reviver"
                ));
            if(result)
                viewModel.ViewModelCore.NotifyActionButtonClicked();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_camera)
            {
                // Handle the camera action
            }
            else if (id == Resource.Id.nav_gallery)
            {

            }
            else if (id == Resource.Id.nav_slideshow)
            {

            }
            else if (id == Resource.Id.nav_manage)
            {

            }
            else if (id == Resource.Id.nav_share)
            {

            }
            else if (id == Resource.Id.nav_send)
            {

            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnSaveInstanceState(Bundle savedInstanceState)
        {
            base.OnSaveInstanceState(savedInstanceState);
            companion.DestroyedForRecreation.IsBeing.NotifyOnSaveInstanceState(savedInstanceState);
        }
    }
}

