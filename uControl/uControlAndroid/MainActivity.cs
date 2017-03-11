using Android.App;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Android.Views;

namespace uControlAndroid
{
    [Activity(Label = "uControl", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        Button button;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            button = FindViewById<Button>(Resource.Id.myButton);
            button.Click += delegate { BLTConnect(); };
            //button.Click += delegate { button.Text = $"{count++} clicks!"; };
        }

        private void BLTConnect()
        {
			BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
			if (adapter == null)
            {
                button.Text = "adapter is not found";
            }
            if (!adapter.IsEnabled)
            {
				button.Text = "Bluetooth adapter is not enabled.";
			}
            if (adapter.IsEnabled) {
                button.Text = adapter.Name;
            }
        }
    }
}

