using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Geosnap.ApiClient.Interfaces;
using System.Threading.Tasks;
using LightInject;
using Phoenix;
using Newtonsoft.Json.Linq;
using Android.Util;
using Android.Views.InputMethods;

namespace Geosnap.Android
{
    [Activity(Label = "Login", MainLauncher = true )]
    public class LoginActivity : Activity
    {
        private readonly IGeosnapClient _geosnapClient;
        
        public LoginActivity()
        {
            _geosnapClient = GeosnapApplication.Container.GetInstance<IGeosnapClient>();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.Login);

            FindViewById<Button>(Resource.Id.login_button).Click += HandleLogin;
            FindViewById<Button>(Resource.Id.register_button).Click += HandleRegister;
            FindViewById<Button>(Resource.Id.reset_button).Click += HandleReset;
            FindViewById<EditText>(Resource.Id.password_textfield).EditorAction += HandleEditorAction;
            
        }

        async void HandleLogin(object sender, EventArgs e)
        {
            var username = FindViewById<EditText>(Resource.Id.username_textfield).Text;
            var password = FindViewById<EditText>(Resource.Id.password_textfield).Text;

            var result = await _geosnapClient.Login(username, password);
            result.Fold(response =>
            {
                GeosnapApplication.Authorization = response.Authorization;
                InitChannels(response.Authorization, response.Id);

            }, error => RunOnUiThread(() => Toast.MakeText(this, Resource.String.Error_Login, ToastLength.Long).Show()));
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
            if(e.ActionId == ImeAction.Done)
            {
                FindViewById<Button>(Resource.Id.login_button).CallOnClick();
                e.Handled = true;
            }
        }

        public void InitChannels(string authorization, int userId)
        {
            var socketOptions = new SocketOptions
            {
                Params = new JObject { { "jwt", authorization } },
                LogCallback = (kind, message, data) => Log.Info(Resources.GetString(Resource.String.Geosnap_Log_Tag), $"{kind} - {message}")
            };
            var socket = new Socket(Resources.GetString(Resource.String.Geosnap_Api_SocketEndpoint), socketOptions);
            socket.Connect();
            GeosnapApplication.Socket = socket;

            StartActivity(typeof(MapActivity));
            //var userChannel = socket.Channel($"users:{userId}", null);
            //userChannel.Join()
            //    .Receive("ok", (_) =>
            //    {
            //        GeosnapApplication.UserChannel = userChannel;
            //        Log.Info(Resources.GetString(Resource.String.Geosnap_Log_Tag), "Successfully joined user channel.");                    
            //    })
            //    .Receive("error", (e) => Log.Error(Resources.GetString(Resource.String.Geosnap_Log_Tag), $"Unable to join user channel, '{e.ToString()}'"))
            //    .Receive("timeout", (_) => Log.Error(Resources.GetString(Resource.String.Geosnap_Log_Tag), "Timeout while trying to join user channel"));  
        }
    }
}