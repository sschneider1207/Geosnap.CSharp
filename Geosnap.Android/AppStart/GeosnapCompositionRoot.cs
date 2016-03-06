using Android.App;
using LightInject;
using Geosnap.ApiClient.Interfaces;
using Geosnap.ApiClient;
using Geosnap.ApiClient.Android;

namespace Geosnap.Android.LightInject
{
    public class GeosnapCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IFileIO, FileIO>(new PerContainerLifetime());

            var apiEndpoint = Application.Context.GetString(Resource.String.Geosnap_Api_Endpoint);
            var apiKey = Application.Context.GetString(Resource.String.Geosnap_Api_Key);
            serviceRegistry.Register<IGeosnapClient>(factory => new GeosnapClient(apiEndpoint, apiKey, factory.GetInstance<IFileIO>()));

            serviceRegistry.RegisterFallback((type, s) => true, request => null);
        }
    }
}