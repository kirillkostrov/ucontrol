using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andrule.Views;
using Android.App;
using Android.Hardware;
using Android.OS;
using Andrule.Accelerometer;
using Andrule.Network;
using Andrule.UIDetails;
using Android.Content;
using Android.Widget;
using Java.Net;
using SocketException = System.Net.Sockets.SocketException;
using uControlAndroid;

namespace Andrule.Views
{
    [Activity(Label = "Andrule",
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,
              Theme = "@android:style/Theme.NoTitleBar")]
    public class WheelActivity : Activity
    {
        private AccelerometerListener _accelerometerListener;

		private int progressBrake;
		private int progressRun;

        private bool[] _buttonStates;

        private const bool invertThrottleAxis = true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WheelLayout);
            
			progressBrake = 0;
			progressRun = 0;

            var sensorManager = (SensorManager)GetSystemService(SensorService);
            _accelerometerListener = new AccelerometerListener(this, sensorManager, OnUpdateAccelerometer);

			var _seekBarBrake = FindViewById<SeekBar> (Resource.Id.seekBarBrake);
			_seekBarBrake.ProgressChanged += seekBarBrakeProgressChanged;
			var _seekBarRun = FindViewById<SeekBar> (Resource.Id.seekBarRun);
			_seekBarRun.ProgressChanged += seekBarRunProgressChanged;

            _buttonStates = new bool[32];

            Button button01 = FindViewById<Button>(Resource.Id.Button01);
            button01.Click += delegate { _buttonStates[0] = true; };

            Button button02 = FindViewById<Button>(Resource.Id.Button02);
            button02.Click += delegate { _buttonStates[1] = true; };

            Button button03 = FindViewById<Button>(Resource.Id.Button03);
            button03.Click += delegate { _buttonStates[2] = true; };

            Button button04 = FindViewById<Button>(Resource.Id.Button04);
            button04.Click += delegate { _buttonStates[3] = true; };
        }

        public bool OnUpdateAccelerometer(Accelerometer.Accelerometer accelerometer)
        {
            try
            {
                NetWorkHelper.Send(string.Format("^{0}|{1}|{2}|{3}$", accelerometer.Rotation, progressBrake, progressRun, GetButtonStatecPacked()));
                ClearButtonStates();
            }
            catch (SocketException ex)
            {
                UIHelper.ShowMessage("Sending error:" + ex.Message, this);
                //NetWorkHelper.Reconnect();
            }
            catch (Java.Net.SocketException ex)
            {
                UIHelper.ShowMessage("Sending error:" + ex.Message, this);
                //NetWorkHelper.Reconnect();
            }
            catch (Exception ex)
            {
                UIHelper.ShowMessage("Sending error:" + ex.Message, this);
            }
            
            return NetWorkHelper.IsConnected;
        }

        private uint GetButtonStatecPacked()
        {
            uint result = 0;
            for (var i = 0; i < 32; i++)
            {
                if (_buttonStates[i])
                {
                    result |= (uint)(2 ^ i);
                }
            }
            return result;
        }

        private void ClearButtonStates()
        {
            for (var i = 0; i < 32; i++)
            {
                _buttonStates[i] = false;
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            _accelerometerListener.Start();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _accelerometerListener.Stop();
        }

		private void seekBarBrakeProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e){
			if (e.FromUser)
			{
				progressBrake = e.Progress * 1000;
			}
		}

		private void seekBarRunProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e){
			if (e.FromUser)
			{
                progressRun = invertThrottleAxis ? 32000 - e.Progress * 1000 : e.Progress * 1000;
            }
		}
    }
}