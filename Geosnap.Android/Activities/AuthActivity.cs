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
using Geosnap.Android.Fragments;

namespace Geosnap.Android.Activities
{
    [Activity(Label = "Authenticate", MainLauncher = true)]
    public class AuthActivity : Activity
    {
        private readonly IGeosnapClient _client;

        public AuthActivity()
        {
            _client = GeosnapApplication.Container.GetInstance<IGeosnapClient>();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Auth);

            var mapViewFragment = new LoginFragment(_client);
            var tx = FragmentManager.BeginTransaction();
            tx.Add(Resource.Id.fragment_container, mapViewFragment);
            tx.Commit();
        }

        public void SendToMainActivity(string authorization, int userId)
        {
            var bundle = new Bundle();
            bundle.PutString("authorization", authorization);
            bundle.PutInt("userId", userId);

            StartActivity(new Intent(this, typeof(MainActivity)), bundle);
        }
    }
}