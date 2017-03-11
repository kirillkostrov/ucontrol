using System;
using Android.Bluetooth;
using Java.Util;

namespace uControlAndroid
{
    public class BtGattDeviceInformationService : BluetoothGattService
    {
        private static UUID DeviceInformationUUID = UUID.FromString("0000180a-0000-1000-8000-00805f9b34fb");
        private static byte[] PNP_ID = new byte[] { (byte)0, unchecked((byte)-122), Byte.MinValue, (byte)1, (byte)0, (byte)1, (byte)0 };
        private static UUID PnPIDUUID = UUID.FromString("00002A50-0000-1000-8000-00805f9b34fb");
        private static BluetoothGattCharacteristic mPnPID;

        public BtGattDeviceInformationService() : base(DeviceInformationUUID, 0)
        {
            mPnPID = new BluetoothGattCharacteristic(PnPIDUUID, GattProperty.Read, GattPermission.Read);
            mPnPID.SetValue(PNP_ID);
            AddCharacteristic(mPnPID);
        }
    }
}