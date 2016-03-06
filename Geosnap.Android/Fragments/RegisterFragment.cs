using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;
using Geosnap.ApiClient.Interfaces;
using DeviceInfo.Plugin;
using Android.Systems;
using Android.Provider;

namespace Geosnap.Android.Fragments
{
    public class RegisterFragment : Fragment
    {
        private readonly IGeosnapClient _client;

        public RegisterFragment(IGeosnapClient client)
        {
            _client = client;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Register, container, false);
            
            view.FindViewById<Button>(Resource.Id.create_button).Click += HandleCreate;
            view.FindViewById<EditText>(Resource.Id.email_textfield).EditorAction += HandleEditorAction;
            return view;
        }

        async void HandleCreate(object sender, EventArgs e)
        {
            var username = View.FindViewById<EditText>(Resource.Id.username_textfield).Text;
            var password = View.FindViewById<EditText>(Resource.Id.password_textfield).Text;
            var email = View.FindViewById<EditText>(Resource.Id.email_textfield).Text;
            var uuid = GeosnapApplication.AppId;

            var result = await _client.Register(username, password, email, uuid);

            result.Fold(_ =>
            {
                Activity.RunOnUiThread(() => Toast.MakeText(View.Context, Resource.String.Register_SuccessToast, ToastLength.Long).Show());
                FragmentManager.PopBackStackImmediate();
            }, error =>
            {
                Activity.RunOnUiThread(() => Toast.MakeText(View.Context, Resource.String.Error_CreateAccount, ToastLength.Long).Show());
            });
        }

        void HandleEditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            e.Handled = false;
            if (e.ActionId == ImeAction.Done)
            {
                View.FindViewById<Button>(Resource.Id.create_button).CallOnClick();
                e.Handled = true;
            }
        }
    }
}