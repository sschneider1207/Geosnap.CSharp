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
using LightInject;
using Geosnap.ApiClient.Interfaces;
using Geosnap.ApiClient;
using Android.Content.Res;
using Geosnap.ApiClient.Android;
using Phoenix;

namespace Geosnap.Android
{
    [Application]
    public class GeosnapApplication : Application
    {
        public static ServiceContainer Container { get; } = new ServiceContainer();
               
        public static string Authorization
        {
            get
            {
                return Container.GetInstance<string>(AuthorizationKey);
            }
            set
            {
                Container.RegisterInstance(value, AuthorizationKey);
            }
        }
        private const string AuthorizationKey = "authorization";

        public static Socket Socket
        {
            get
            {
                return Container.GetInstance<Socket>(SocketKey);
            }
            set
            {
                Container.RegisterInstance(value, SocketKey);
            }
        }
        private const string SocketKey = "socket";

        public static Channel UserChannel
        {
            get
            {
                return Container.GetInstance<Channel>(UserChannelKey);
            }
            set
            {
                Container.RegisterInstance(value, UserChannelKey);
            }
        }
        private const string UserChannelKey = "userChannel";

        public GeosnapApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        public override void OnCreate()
        {
            RegisterServices();

            base.OnCreate();
        }

        private void RegisterServices()
        {
            Container.Register<IFileIO, FileIO>(new PerContainerLifetime());

            var apiEndpoint = Resources.GetString(Resource.String.Geosnap_Api_Endpoint);
            var apiKey = Resources.GetString(Resource.String.Geosnap_Api_Key);
            Container.Register<IGeosnapClient>(factory => new GeosnapClient(apiEndpoint, apiKey, factory.GetInstance<IFileIO>()));

            Container.RegisterFallback((type, s) => true, request => null);      
        }

        
    }
}