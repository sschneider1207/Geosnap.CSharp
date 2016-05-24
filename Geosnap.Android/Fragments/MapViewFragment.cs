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
using Android.Gms.Maps;
using Geosnap.Android.Activities;
using Android.Locations;
using Android.Gms.Maps.Model;

namespace Geosnap.Android.Fragments
{
    public class MapViewFragment : Fragment, IOnMapReadyCallback
    {
        private readonly Location _location;
        private GoogleMap _googleMap;

        public MapViewFragment(Location location)
        {
            _location = location;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.MapView, container, false);

            var mapFrag = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map);
            mapFrag.GetMapAsync(this);

            return view;
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _googleMap = googleMap;

            var builder = CameraPosition.InvokeBuilder();
            builder.Target(new LatLng(_location.Latitude, _location.Longitude));
            builder.Zoom(18);
            var cameraPosition = builder.Build();
            var cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            _googleMap.MoveCamera(cameraUpdate);
        }
    }
}