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
using Geosnap.ApiClient.Interfaces;
using Geosnap.Android.Activities;
using Phoenix;
using Newtonsoft.Json.Linq;
using Android.Views.InputMethods;

namespace Geosnap.Android.Fragments
{
    public class LoginFragment : Fragment
    {
        private readonly IGeosnapClient _client;

        public LoginFragment(IGeosnapClient client)
        {
            _client = client;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Login, container, false);

            view.FindViewById<Button>(Resource.Id.login_button).Click += HandleLogin;
            view.FindViewById<Button>(Resource.Id.register_button).Click += HandleRegister;
            view.FindViewById<Button>(Resource.Id.reset_button).Click += HandleReset;
            view.FindViewById<EditText>(Resource.Id.password_textfield).EditorAction += HandleEditorAction;
            
            return view;
        }

        async void HandleLogin(object sender, EventArgs e)
        {
            var username = View.FindViewById<EditText>(Resource.Id.username_textfield).Text;
            var password = View.FindViewById<EditText>(Resource.Id.password_textfield).Text;

            var result = await _client.Login(username, password);
            result.Fold(response =>
            {
                GeosnapApplication.Authorization = response.Authorization;
                InitSocket(response.Authorization);

                var bundle = new Bundle();
                bundle.PutString("authorization", response.Authorization);
                bundle.PutInt("userId", response.Id);
                var intent = new Intent(View.Context, typeof(MainActivity));

                StartActivity(intent, bundle);

            }, error => Activity.RunOnUiThread(() => Toast.MakeText(View.Context, Resource.String.Error_Login, ToastLength.Long).Show()));
        }

        void HandleRegister(object sender, EventArgs e)
        {
            ;
        }

        void HandleReset(object sender, EventArgs e)
        {
            ;
        }

        void HandleEditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            e.Handled = false;
            if (e.ActionId == ImeAction.Done)
            {
                View.FindViewById<Button>(Resource.Id.login_button).CallOnClick();
                e.Handled = true;
            }
        }

        public void InitSocket(string authorization)
        {
            var socketOptions = new SocketOptions
            {
                Params = new JObject { { "jwt", authorization } },
                LogCallback = (kind, message, data) => Log.Info(Resources.GetString(Resource.String.Geosnap_Log_Tag), $"{kind} - {message}")
            };
            var socket = new Socket(Resources.GetString(Resource.String.Geosnap_Api_SocketEndpoint), socketOptions);
            socket.Connect();
            GeosnapApplication.Socket = socket;
        }
    }
}