using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Widget;
using Andrule.Network;
using Andrule.UIDetails;
using uControlAndroid;
using System.Threading;

namespace Andrule.Views
{
    [Activity]
    public class SetupActivity : Activity
    {
        private ISharedPreferences preferences;
        private EditText editIpText;
        private Button connectButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SetupLayout);
			preferences = GetPreferences(FileCreationMode.Private);

			editIpText = FindViewById<EditText>(Resource.Id.editIpText);
			connectButton = FindViewById<Button>(Resource.Id.connectButton);
            connectButton.Click += GetIpAndConnect;

            var ipAddress = preferences.GetString("ipAddress", string.Empty);
            editIpText.Text = ipAddress;
            var netWorkState = NetWorkHelper.IsConnected;
        }

        private void GetIpAndConnect(object sender, EventArgs e)
        {
            try
            {
                var ipAddress = editIpText.Text;

                var editor = preferences.Edit();
                editor.PutString("ipAddress", ipAddress);
                editor.Commit();

                var intent = new Intent(this, typeof(NetworkService));
                intent.PutExtra("ip", ipAddress);
                StartService(intent);
                Thread.Sleep(3000);
            }
            catch(Exception ex){
                UIHelper.ShowMessage(ex.Message, this);
            }

            if (NetWorkHelper.IsConnected) {
				connectButton.Click -= GetIpAndConnect;
				connectButton.Text = "Stop";
			    connectButton.Click += CloseConnection;
			}
        }

        private void CloseConnection(object sender, EventArgs e)
        {
            StopService(new Intent(this, typeof(NetworkService)));
            connectButton.Click += GetIpAndConnect;
            connectButton.Click -= CloseConnection;
            connectButton.Text = "Connect";
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}