using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;
using Java.Util;

namespace uControlAndroid
{
    public class BtGattBatteryService : BluetoothGattService
    {
        private static UUID BatteryLevelConfigUUID = UUID.FromString("00002902-0000-1000-8000-00805f9b34fb");
        private static UUID BatteryLevelUUID = UUID.FromString("00002a19-0000-1000-8000-00805f9b34fb");
        private static UUID BatteryServiceUUID = UUID.FromString("0000180f-0000-1000-8000-00805f9b34fb");
        private static byte MASK_NOTIFICATION = (byte)1;
        private static BluetoothGattCharacteristic mBatteryLevel;
        private static BluetoothGattDescriptor mBatteryLevelConfig;

        public BtGattBatteryService() : base(BatteryServiceUUID, GattServiceType.Primary)
        {
            mBatteryLevel = new BluetoothGattCharacteristic(BatteryLevelUUID, GattProperty.Read | GattProperty.Read, GattPermission.Read);
            mBatteryLevelConfig = new BluetoothGattDescriptor(BatteryLevelConfigUUID, GattDescriptorPermission.Read | GattDescriptorPermission.Write);
        }


    }

}
}