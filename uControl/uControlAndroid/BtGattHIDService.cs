using System;
using Android.Bluetooth;
using Java.Util;

namespace uControlAndroid
{
    public class BtGattHIDService : BluetoothGattService
    {
        private static UUID BootMouseInputReportUUID;
        private static UUID ConfigUUID;
        private static bool DBG = false;
        private static byte[] DESCRIPTOR_CONFIG_NONE;
        private static byte[] DESCRIPTOR_CONFIG_NOTIFY;
        private static UUID HIDInfoUUID;
        private static UUID HIDServiceUUID = UUID.FromString("00001812-0000-1000-8000-00805f9b34fb");
        private static byte[] HID_INFO;
        private static byte MASK_NOTIFICATION = (byte)1;
        private static byte[] MOUSE_REPORT_MAP_DESCRIPTOR;
        private static int PROTOCOL_MODE_BOOT = 0;
        private static int PROTOCOL_MODE_DEFAULT = 1;
        private static int PROTOCOL_MODE_REPORT = 1;
        private static UUID ProtocolModeUUID;
        private static byte[] REPORT_REF_MOUSE;
        private static UUID ReportMapUUID;
        private static UUID ReportRefUUID;
        private static UUID ReportUUID;
        private static string TAG = "BluetoothGattHIDService";
        private static BluetoothGattCharacteristic mBootMouseInputReport;
        private static BluetoothGattDescriptor mBootMouseInputReportConfig;
        private static BootMouseReport mBootMouseReport;
        private static BluetoothGattCharacteristic mHIDInformation;
        private static BluetoothGattCharacteristic mMouseInputReport;
        private static BluetoothGattDescriptor mMouseInputReportConfig;
        private static BluetoothGattDescriptor mMouseInputReportRef;
        private static BluetoothGattCharacteristic mProtocolMode;
        private static BluetoothGattCharacteristic mReportMap;

        public BtGattHIDService() : base(HIDServiceUUID, GattServiceType.Primary)
        {
            HIDServiceUUID = UUID.FromString("00001812-0000-1000-8000-00805f9b34fb");
            ProtocolModeUUID = UUID.FromString("00002A4E-0000-1000-8000-00805f9b34fb");
            BootMouseInputReportUUID = UUID.FromString("00002A33-0000-1000-8000-00805f9b34fb");
            ConfigUUID = UUID.FromString("00002902-0000-1000-8000-00805f9b34fb");
            HIDInfoUUID = UUID.FromString("00002A4a-0000-1000-8000-00805f9b34fb");
            ReportMapUUID = UUID.FromString("00002A4b-0000-1000-8000-00805f9b34fb");
            ReportUUID = UUID.FromString("00002A4d-0000-1000-8000-00805f9b34fb");
            ReportRefUUID = UUID.FromString("00002908-0000-1000-8000-00805f9b34fb");
            DBG = false;
            mBootMouseReport = new BootMouseReport();

            mProtocolMode = new BluetoothGattCharacteristic(ProtocolModeUUID, GattProperty.WriteNoResponse | GattProperty.Read, GattPermission.Write | GattPermission.Read);
            mBootMouseInputReport = new BluetoothGattCharacteristic(BootMouseInputReportUUID, GattProperty.Notify | GattProperty.Read, GattPermission.Read);
            mBootMouseInputReportConfig = new BluetoothGattDescriptor(ConfigUUID, GattDescriptorPermission.Write | GattDescriptorPermission.Read);
            mHIDInformation = new BluetoothGattCharacteristic(HIDInfoUUID, GattProperty.Read, GattPermission.Read);
            mReportMap = new BluetoothGattCharacteristic(ReportMapUUID, GattProperty.Read, GattPermission.Read);
            mMouseInputReport = new BluetoothGattCharacteristic(ReportUUID, GattProperty.Notify | GattProperty.Read, GattPermission.Read);
            mMouseInputReportConfig = new BluetoothGattDescriptor(ConfigUUID, GattDescriptorPermission.Write | GattDescriptorPermission.Read);
            mMouseInputReportRef = new BluetoothGattDescriptor(ReportRefUUID, GattDescriptorPermission.Read);
            MOUSE_REPORT_MAP_DESCRIPTOR = new byte[] { (byte)5, MASK_NOTIFICATION, (byte)9, (byte)2, unchecked((byte)-95), MASK_NOTIFICATION, (byte)9, MASK_NOTIFICATION, unchecked((byte)-95), (byte)0, (byte)5, (byte)9, (byte)25, MASK_NOTIFICATION, (byte)41, (byte)3, (byte)21, (byte)0, (byte)37, MASK_NOTIFICATION, unchecked((byte)-107), (byte)3, (byte)117, MASK_NOTIFICATION, unchecked((byte)-127), (byte)2, unchecked((byte)-107), MASK_NOTIFICATION, (byte)117, (byte)5, unchecked((byte)-127), (byte)3, (byte)5, MASK_NOTIFICATION, (byte)9, (byte)48, (byte)9, (byte)49, (byte)21, unchecked((byte)-127), (byte)37, Byte.MaxValue, (byte)117, (byte)8, unchecked((byte)-107), (byte)2, unchecked((byte)-127), (byte)6, unchecked((byte)-64), unchecked((byte)-64) };
            REPORT_REF_MOUSE = new byte[] { (byte)0, MASK_NOTIFICATION };
            HID_INFO = new byte[] { (byte)19, (byte)2, (byte)0, (byte)2 };
            DESCRIPTOR_CONFIG_NONE = new byte[] { (byte)0, (byte)0 };
            DESCRIPTOR_CONFIG_NOTIFY = new byte[] { MASK_NOTIFICATION, (byte)0 };

        }

        private void provision()
        {
            setProtocolMode(PROTOCOL_MODE_REPORT);
            AddCharacteristic(mProtocolMode);
            mBootMouseInputReportConfig.SetValue(DESCRIPTOR_CONFIG_NONE);
            mBootMouseInputReport.AddDescriptor(mBootMouseInputReportConfig);
            mBootMouseInputReport.SetValue(mBootMouseReport.getRawValue());
            AddCharacteristic(mBootMouseInputReport);
            mHIDInformation.SetValue(HID_INFO);
            AddCharacteristic(mHIDInformation);
            mReportMap.SetValue(MOUSE_REPORT_MAP_DESCRIPTOR);
            AddCharacteristic(mReportMap);
            mBootMouseInputReportConfig.SetValue(DESCRIPTOR_CONFIG_NOTIFY);
            mMouseInputReport.AddDescriptor(mMouseInputReportConfig);
            mMouseInputReportRef.SetValue(REPORT_REF_MOUSE);
            mMouseInputReport.AddDescriptor(mMouseInputReportRef);
            mMouseInputReport.SetValue(mBootMouseReport.getRawValue());
            AddCharacteristic(mMouseInputReport);

        }

        private void setProtocolMode(int protocolMode)
        {
            byte[] value = new byte[(byte)PROTOCOL_MODE_REPORT];
            value[PROTOCOL_MODE_BOOT] = (byte)protocolMode;
            mProtocolMode.SetValue(value);
        }

        public void setXYDisplacement(int x, int y)
        {
            mBootMouseReport.setXDisplacement(x);
            mBootMouseReport.setYDisplacement(y);
            mBootMouseInputReport.SetValue(mBootMouseReport.getRawValue());
            mMouseInputReport.SetValue(mBootMouseReport.getRawValue());
        }

        public void setButton(int id, bool value)
        {
            switch (id)
            {
                case 0:
                    mBootMouseReport.setButton1(value);
                    break;
                case 1:
                    mBootMouseReport.setButton2(value);
                    break;
                case 2:
                    mBootMouseReport.setButton3(value);
                    break;
            }
        }

        private bool isNotifEnabled(BluetoothGattDescriptor d)
        {
            if (d.GetValue() != null && (d.GetValue()[PROTOCOL_MODE_BOOT] & PROTOCOL_MODE_REPORT) == PROTOCOL_MODE_REPORT)
            {
                return true;
            }
            return false;
        }

        public BluetoothGattCharacteristic getNotification()
        {
            int mode = mProtocolMode.GetIntValue(GattFormat.Uint8, PROTOCOL_MODE_BOOT).IntValue();
            mMouseInputReport.SetValue(mBootMouseReport.getRawValue());
            if (mode == 0 && isNotifEnabled(mBootMouseInputReportConfig))
            {
                return mBootMouseInputReport;
            }
            if (mode == PROTOCOL_MODE_REPORT && isNotifEnabled(mMouseInputReportConfig))
            {
                return mMouseInputReport;
            }
            return null;
        }


    }
}