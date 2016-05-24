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
using Android.Preferences;

namespace Geosnap.Android.Activities
{
    [Activity(Label = "Authenticate", MainLauncher = true)]
    public class AuthActivity : Activity
    {
        private const string AuthorizationCacheKey = "Geosnap.Authorization";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            string authorization;
            int userId;
            if(TryGetAuthorization(out authorization, out userId))
            {
                SendToMainActivity(authorization, userId, false);
            }

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Auth);

            var tx = FragmentManager.BeginTransaction();
            tx.Add(Resource.Id.fragment_container, GeosnapApplication.Container.GetInstance<LoginFragment>());
            tx.Commit();
        }

        public void SendToMainActivity(string authorization, int userId, bool remember)
        {
            if (remember)
            {
                CacheAuthorization(authorization, userId);
            }
            var bundle = new Bundle();
            bundle.PutString("authorization", authorization);
            bundle.PutInt("userId", userId);

            StartActivity(new Intent(this, typeof(MainActivity)), bundle);
        }

        public void SwapToRegisterFragment()
        {
            var tx = FragmentManager.BeginTransaction();
            tx.Replace(Resource.Id.fragment_container, GeosnapApplication.Container.GetInstance<RegisterFragment>());
            tx.AddToBackStack(null);
            tx.Commit();
        }

        private bool TryGetAuthorization(out string authorization, out int userId)
        {
            authorization = null;
            userId = 0;

            var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            string val = prefs.GetString(AuthorizationCacheKey, null);
            if (string.IsNullOrWhiteSpace(val))
            {
                return false;
            }

            var pieces = val.Split(new[] { ':' }, 2);
            if (pieces.Length != 2)
            {
                return false;
            }

            if (!int.TryParse(pieces[0], out userId))
            {
                return false;
            }

            authorization = pieces[1];
            return true;
        }

        private void CacheAuthorization(string authorization, int userId)
        {
            var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            var editor = prefs.Edit();
            var val = $"{userId}:{authorization}";
            editor.PutString(AuthorizationCacheKey, val);
            editor.Apply();
        }
    }
}