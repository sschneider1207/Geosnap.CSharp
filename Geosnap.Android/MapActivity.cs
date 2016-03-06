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

namespace Geosnap.Android
{
    [Activity(Label = "Map")]
    public class MapActivity : Activity
    {
        private readonly IGeosnapClient _geosnapClient;

        public MapActivity()
        {
            _geosnapClient = GeosnapApplication.Container.GetInstance<IGeosnapClient>();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Map);
        }
    }
}