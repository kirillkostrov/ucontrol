
using System;

using Android.App;
using Android.Content;
using Android.OS;

namespace uControlAndroid
{
    [Service(Label = "NetworkService")]
    [IntentFilter(new String[] { "com.yourname.NetworkService" })]
    public class NetworkService : Service
    {
        IBinder binder;

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            // start your service logic here

            // Return the correct StartCommandResult for the type of service you are building
            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            binder = new NetworkServiceBinder(this);
            return binder;
        }
    }

    public class NetworkServiceBinder : Binder
    {
        readonly NetworkService service;

        public NetworkServiceBinder(NetworkService service)
        {
            this.service = service;
        }

        public NetworkService GetNetworkService()
        {
            return service;
        }
    }
}
