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
    [Activity(Label = "Main")]
    public class MainActivity : Activity
    {
        private readonly IGeosnapClient _geosnapClient;

        public MainActivity()
        {
            _geosnapClient = GeosnapApplication.Container.GetInstance<IGeosnapClient>();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            var mapViewFragment = new MapViewFragment();
            var tx = FragmentManager.BeginTransaction();
            tx.Add(Resource.Id.fragment_container, mapViewFragment);
            tx.Commit();
        }

        public void transit() { }
    }
}