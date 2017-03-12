
using System;

using Android.App;
using Android.Content;
using Android.OS;
using Andrule.Network;
using Andrule.UIDetails;
using Android.Util;
using Andrule.Views;

namespace uControlAndroid
{
    [Service(Label = "NetworkService")]
    [IntentFilter(new String[] { "com.yourname.NetworkService" })]
    public class NetworkService : Service
    {
        IBinder binder;
        public NetWorkHelper netWorkHelper;
        public bool IsConnected;
        public bool IsCreated;

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            netWorkHelper = new NetWorkHelper();
            var ip = intent.GetStringExtra("ip");

            try {
                NetWorkHelper.Connect(ip);
            } 
            catch (Exception ex)
			{
                Log.Debug("NetworkService", "Connection error:" + ex.Message);
			}

			if (NetWorkHelper.IsConnected)
			{
                NetWorkHelper.Send("I connected with you!");
			}

            // Return the correct StartCommandResult for the type of service you are building
            return StartCommandResult.NotSticky;
        }

		public override void OnCreate()
		{
			base.OnCreate();
            IsCreated = true;
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
