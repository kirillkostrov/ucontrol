using Android.App;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Java.Util;

namespace uControlAndroid
{
    [Activity(Label = "uControl", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        public class ControlReport
        {
            bool button { get; set; }

            public bool isButton()
            {
                return this.button;
            }
            public void setButton(bool state)
            {
                this.button = state;
            }
        }

        int count = 1;

        BluetoothGattServer btGattServer { get; set; }

        static readonly UUID HIDServiceUUID = UUID.FromString("00001812-0000-1000-8000-00805f9b34fb");
        static readonly UUID BootMouseInputReportUUID = UUID.FromString("00002A33-0000-1000-8000-00805f9b34fb");
        static readonly UUID ConfigUUID = UUID.FromString("00002902-0000-1000-8000-00805f9b34fb");
        static readonly UUID HIDInfoUUID = UUID.FromString("00002A4a-0000-1000-8000-00805f9b34fb");
        static readonly UUID ReportMapUUID = UUID.FromString("00002A4b-0000-1000-8000-00805f9b34fb");
        static readonly UUID ReportUUID = UUID.FromString("00002A4d-0000-1000-8000-00805f9b34fb");
        static readonly UUID ReportRefUUID = UUID.FromString("00002908-0000-1000-8000-00805f9b34fb");

        ControlReport controlReport = new ControlReport();



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var service = new BluetoothGattService(HIDServiceUUID, GattServiceType.Primary);
            service.AddCharacteristic(new BluetoothGattCharacteristic())
            btGattServer.AddService();

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate { button.Text = $"{count++} clicks!"; };
        }
    }
}

