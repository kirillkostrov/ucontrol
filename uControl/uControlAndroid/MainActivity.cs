using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using uControlAndroid;
using Andrule.Views;
using uControlAndroid.Repository;
using System.Linq;
using uControlAndroid.Entities;
using uControlAndroid.Common;

namespace Andrule
{
    [Activity(Label = "Andrule", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class MainActivity : Activity
    {
        public static TabHost Tabs { get; private set; }

        private static readonly string ConnectionPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            var setupBtn = FindViewById(Resource.Id.openSetupBtn);
            setupBtn.Click += delegate {
                var intent = new Intent(this, typeof(SetupActivity));
				StartActivity(intent);
            };

            var wheelBtn = FindViewById(Resource.Id.openWheelBtn);
            wheelBtn.Click += delegate {
                var intent = new Intent(this, typeof(WheelActivity));
                StartActivity(intent);
            };
            var connectionPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            Database.InitDatabase(ConnectionPath);
            //CreateDatabase();
            //Tabs = TabHost;

            //CreateTab(typeof(SetupActivity), "setup", "SETUP");
            //CreateTab(typeof(WheelActivity), "wheel", "WHEEL");
        }

        private void CreateDatabase()
        {
            using (var db = new Database(ConnectionPath))
            {
                var gamePadId = db.Add(new GamePad { Name = "Dendi" });

                db.Add(new Control { ControlType = ControlType.ActionButton, Height = 10, Weight = 10, XPos = 0, YPos = 0, GamePadId = gamePadId + 1 });

                db.Add(new Control { ControlType = ControlType.DPad, Height = 102, Weight = 120, XPos = 20, YPos = 20, GamePadId = gamePadId });

                db.Add(new Control { ControlType = ControlType.Accelerometer, Height = 130, Weight = 30, XPos = 10, YPos = 20, GamePadId = gamePadId });

                var messages = db.GetAllItems<Control>().ToArray();
            }
        }

   //     private void CreateTab(Type activityType, string tag, string label)
   //     {
   //         var intent = new Intent(this, activityType);
   //         intent.AddFlags(ActivityFlags.NewTask);

   //         var spec = Tabs.NewTabSpec(tag);
			//spec.SetIndicator(label);
   //         spec.SetContent(intent);

   //         Tabs.AddTab(spec);
   //     }
    }
}