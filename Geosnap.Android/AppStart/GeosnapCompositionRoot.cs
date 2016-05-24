using Android.App;
using LightInject;
using Geosnap.ApiClient.Interfaces;
using Geosnap.ApiClient;
using Geosnap.ApiClient.Android;
using Geosnap.Android.Fragments;
using Android.Locations;

namespace Geosnap.Android.LightInject
{
    public class GeosnapCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            RegisterApiClient(serviceRegistry);
            RegisterFragments(serviceRegistry);
            serviceRegistry.RegisterFallback((type, s) => true, request => null);
        }

        private void RegisterApiClient(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IFileIO, FileIO>(new PerContainerLifetime());

            var apiEndpoint = Application.Context.GetString(Resource.String.Geosnap_Api_Endpoint);
            var apiKey = Application.Context.GetString(Resource.String.Geosnap_Api_Key);
            serviceRegistry.Register<IGeosnapClient>(factory => new GeosnapClient(apiEndpoint, apiKey, factory.GetInstance<IFileIO>()));
        }

        private void RegisterFragments(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register(factory => new LoginFragment(factory.GetInstance<IGeosnapClient>()));
            serviceRegistry.Register(factory => new RegisterFragment(factory.GetInstance<IGeosnapClient>()));
            serviceRegistry.Register<Location, MapViewFragment>((factory, location) => new MapViewFragment(location));
       }
    }
}