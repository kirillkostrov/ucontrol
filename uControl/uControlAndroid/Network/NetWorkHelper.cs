using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Android.Content;
using Android.Util;
using Andrule.UIDetails;

namespace Andrule.Network
{
    public class NetWorkHelper : IDisposable
    {
        private static UdpClient _client;
        public static bool IsConnected { get; private set; }
        private static string currentIp { get; set; }
        private static byte ReconnectCounter { get; set; }

        public static void Connect(string ip)
        {
            try
            {
                _client = new UdpClient();
                _client.Connect(ip, 51515);
 
                currentIp = ip;
                IsConnected = true;
                ReconnectCounter = 0;
            }
            catch (SocketException e)
            {
                IsConnected = false;
                Log.Debug("Connection error: ", e.Message);
                if (ReconnectCounter > 10) throw;
                Reconnect();
                ReconnectCounter++;
            }
            catch (Exception e)
            {
                IsConnected = false;
                Log.Debug("Connection error: ", e.Message);
                throw;
            }
        }

        public static void Send(string message)
        {
            var messageInByte = Encoding.ASCII.GetBytes(message);
            try
            {
                _client?.Send(messageInByte, messageInByte.Length);
            }
            catch (Exception e)
            {
                IsConnected = false;
                Log.Debug("Sending error: ", e.Message);
                throw;
            }
        }

        public void CloseConnection()
        {
            IsConnected = false;
            _client?.Close();
        }

        public static void Reconnect()
        {
            Connect(currentIp);
        }

        public void Dispose()
        {
            IsConnected = false;
            _client?.Close();
        }
    }
}