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
using Java.IO;

namespace Geosnap.Android
{
    [Application]
    public class GeosnapApplication : Application
    {
        private const string _installation = "INSTALLATION";
        private static Guid _appId;
        private static object _appIdLock = new object();
        public static Guid AppId
        {
            get
            {
                if(_appId != Guid.Empty) { return _appId; }

                lock (_appIdLock) {
                    if (_appId != Guid.Empty) { return _appId; }

                    using (var installation = new File(Context.FilesDir, _installation))
                    {
                        if (installation.Exists())
                        {
                            using (var f = new RandomAccessFile(installation, "r"))
                            {
                                var bytes = new byte[f.Length()];
                                f.ReadFully(bytes);
                                f.Close();
                                _appId = new Guid(bytes);
                            }
                        }
                        else
                        {
                            _appId = Guid.NewGuid();
                            using (var stream = new FileOutputStream(installation))
                            {
                                stream.Write(_appId.ToByteArray());
                                stream.Close();
                            }
                        }
                    }
                }
                return _appId;
            }
        }

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
        
        public GeosnapApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        public override void OnCreate()
        {
            base.OnCreate();

            Container.RegisterAssembly(GetType().Assembly);
        }
    }
}