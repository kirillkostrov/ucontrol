using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using uControlAndroid;
using Andrule.Views;
using uControlAndroid.Services;

namespace Andrule
{
    [Activity(Label = "Andrule", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class MainActivity : Activity
    {
        public static TabHost Tabs { get; private set; }

        private GamePadService GamePadService { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            var setupBtn = FindViewById(Resource.Id.openSetupBtn);
            setupBtn.Click += delegate
            {
                var intent = new Intent(this, typeof(SetupActivity));
                StartActivity(intent);
            };

            var wheelBtn = FindViewById(Resource.Id.openWheelBtn);
            wheelBtn.Click += delegate
            {
                var intent = new Intent(this, typeof(WheelActivity));
                StartActivity(intent);
            };
        }        

    }
}