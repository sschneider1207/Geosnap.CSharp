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
using Android.Locations;

namespace Geosnap.Android.Activities
{
    [Activity(Label = "Main")]
    public class MainActivity : Activity
    {
        private LocationManager _locationManager;
        private string _locationProvider;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            InitializeLocationManager();
            var location = _locationManager.GetLastKnownLocation(_locationProvider);
            var tx = FragmentManager.BeginTransaction();
            tx.Add(Resource.Id.fragment_container, GeosnapApplication.Container.GetInstance<Location, MapViewFragment>(location));
            tx.Commit();
        }

        private void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            var criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            var acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
        }

        public void transit() { }
    }
}