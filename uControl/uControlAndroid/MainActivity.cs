using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using uControlAndroid;
using Andrule.Views;
using uControlAndroid.Services;
using uControlAndroid.Entities;
using uControlAndroid.Common;

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

            GamePadServiceTest();
        }

        private void GamePadServiceTest()
        {
            GamePadService = new GamePadService();

            var gamePadList = GamePadService.GetGamePadList();

            GamePadService.CreateOrUpdateGamePad(new ListItem { Name = "asas" });
            gamePadList = GamePadService.GetGamePadList();

            var newControl = new Control
            {
                GamePadId = gamePadList[0].Id,
                ControlType = ControlType.Accelerometer,
                Height = 11,
                Weight = 22,
                Id = 0,
                XPos = 100,
                YPos = 200
            };

            newControl.Id = GamePadService.CreateOrUpdateControl(newControl);

            var contr = GamePadService.GetGamePadControls(newControl.GamePadId);

            GamePadService.DeleteControl(newControl.GamePadId, newControl.Id);

            contr = GamePadService.GetGamePadControls(newControl.GamePadId);

            GamePadService.DeleteGamePad(gamePadList[0].Id);

            gamePadList = GamePadService.GetGamePadList();

            GamePadService.CreateOrUpdateGamePad(new ListItem { Name = "asas1" });

            gamePadList = GamePadService.GetGamePadList();
        }
    }
}