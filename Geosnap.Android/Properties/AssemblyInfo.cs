using System.Reflection;
using System.Runtime.InteropServices;
using Android.App;
using LightInject;
using Android;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Geosnap.Android")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Geosnap.Android")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]


[assembly: MetaData("com.google.android.maps.v2.API_KEY", Value = "AIzaSyCUGYbtrXF80rOaRxKDNvuOaMwxtZZT5OM")]
[assembly: CompositionRootType(typeof(Geosnap.Android.LightInject.GeosnapCompositionRoot))]

// Permissions
[assembly: UsesPermission(Manifest.Permission.Internet)]
[assembly: UsesPermission(Manifest.Permission.AccessNetworkState)]
[assembly: UsesPermission(Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission(Manifest.Permission.AccessFineLocation)]
[assembly: UsesPermission(Manifest.Permission.AccessMockLocation)]
[assembly: UsesPermission(Manifest.Permission.WriteExternalStorage)]