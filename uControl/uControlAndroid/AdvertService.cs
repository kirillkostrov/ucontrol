using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Bluetooth.LE;
using Android.Bluetooth;
using Android.Util;
using Java.Util;

namespace uControlAndroid
{
    public class AdvertService : Service
    {
        private static bool DBG = false;
        private static string TAG = "QQQ";
        private AdvertiseCallback mAdvertiseCallback;
        private BluetoothAdapter mBluetoothAdapter;
        private BluetoothLeAdvertiser mBluetoothLeAdvertiser;
        private BroadcastReceiver mReceiver;
        private String mSavedName;

        class Receiver : BroadcastReceiver
        {
            AdvertService adService;
            public Receiver(AdvertService service)
            {
                adService = service;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                if ("android.bluetooth.adapter.action.STATE_CHANGED".Equals(intent.Action))
                {
                    int state = intent.GetIntExtra("android.bluetooth.adapter.extra.STATE", -1);
                    if (state == 10 || state == 13)
                    {
                        Log.Error(AdvertService.TAG, "Bluetooth disabled during advertising, stop");
                        adService.StopSelf();
                    }
                }
            }
        }

        public class MouseAdvertiseCallback : AdvertiseCallback
        {
            AdvertService adService;
            public MouseAdvertiseCallback(AdvertService service)
            {
                adService = service;
            }

            public void onStartFailure(int errorCode)
            {
                Log.Error(AdvertService.TAG, "Unable to Start LE Advertisement");
                adService.StopSelf();
                
            }

            public void onStartSuccess(AdvertiseSettings settingsInEffect)
            {
            }
        }

        public AdvertService()
        {
            mReceiver = new Receiver(this);
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate() 
        {
            base.OnCreate();
            IntentFilter filter = new IntentFilter();
            filter.AddAction("android.bluetooth.adapter.action.STATE_CHANGED");
            RegisterReceiver(mReceiver, filter);
            mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (mBluetoothAdapter == null)
            {
                Log.Error(TAG, "Unable to retrieve Bluetooth Adapter");
                StopSelf();
                return;
            }
            mBluetoothLeAdvertiser = mBluetoothAdapter.BluetoothLeAdvertiser;
            if (mBluetoothLeAdvertiser == null)
            {
                Log.Error(TAG, "Unable to retrieve Bluetooth Advertiser");
                StopSelf();
            }
            else if (!startAdvertising())
            {
                StopSelf();
            }
        }

        public override void OnDestroy()
        {
            stopAdvertising();
            try
            {
                UnregisterReceiver(this.mReceiver);
            }
            catch (Exception e)
            {
                Log.Error(TAG, "Error unregistering receiver");
            }
            base.OnDestroy();
        }

        private void enableBluetooth(bool enabled)
        {
            if (this.mBluetoothAdapter == null)
            {
                Log.Error(TAG, "No Bluetooth Adapter");
            }
            else if (enabled && !this.mBluetoothAdapter.IsEnabled)
            {
                this.mBluetoothAdapter.Enable();
            }
            else if (!enabled && this.mBluetoothAdapter.IsEnabled)
            {
                this.mBluetoothAdapter.Disable();
            }
        }

        private bool startAdvertising()
        {
            AdvertiseData.Builder dataBuilder = new AdvertiseData.Builder();
            AdvertiseSettings.Builder settingBuilder = new AdvertiseSettings.Builder();
            if (this.mBluetoothAdapter == null)
            {
                Log.Error(TAG, "No Bluetooth Adapter");
                return false;
            }
            else if (this.mBluetoothLeAdvertiser == null)
            {
                Log.Error(TAG, "No Bluetooth Advertiser");
                return false;
            }
            else if (!this.mBluetoothAdapter.IsMultipleAdvertisementSupported)
            {
                Log.Error(TAG, "BLE Multiple Advertisement Not Supported");
                return false;
            }
            else if (this.mAdvertiseCallback != null)
            {
                Log.Error(TAG, "BLE advertising already ongoing");
                return true;
            }
            else
            {
                this.mSavedName = this.mBluetoothAdapter.Name;
                this.mBluetoothAdapter.SetName("MyHID");
                settingBuilder.SetAdvertiseMode(AdvertiseMode.Balanced);
                settingBuilder.SetTimeout(0);
                AdvertiseSettings settings = settingBuilder.Build();
                settingBuilder.SetConnectable(true);
                UUID hidServiceUUID = UUID.FromString("00001812-0000-1000-8000-00805f9b34fb");
                dataBuilder.SetIncludeDeviceName(true);
                dataBuilder.SetIncludeTxPowerLevel(true);
                dataBuilder.AddServiceUuid(new ParcelUuid(hidServiceUUID));
                AdvertiseData data = dataBuilder.Build();
                this.mAdvertiseCallback = new MouseAdvertiseCallback(this);
                if (this.mAdvertiseCallback == null)
                {
                    Log.Error(TAG, "Unable to alloc advertise callback");
                    return false;
                }
                this.mBluetoothLeAdvertiser.StartAdvertising(settings, data, this.mAdvertiseCallback);
                return true;
            }
        }

        private void stopAdvertising()
        {
            if (this.mBluetoothAdapter != null)
            {
                if (this.mSavedName != null)
                {
                    this.mBluetoothAdapter.SetName(this.mSavedName);
                }
                if (this.mBluetoothLeAdvertiser == null)
                {
                    Log.Error(TAG, "No Bluetooth Advertiser");
                }
                else if (this.mAdvertiseCallback == null)
                {
                    Log.Error(TAG, "No Advertise Callback");
                }
                else if (this.mBluetoothAdapter.IsEnabled)
                {
                    this.mBluetoothLeAdvertiser.StopAdvertising(this.mAdvertiseCallback);
                    this.mAdvertiseCallback = null;
                }
            }
        }


    }
}