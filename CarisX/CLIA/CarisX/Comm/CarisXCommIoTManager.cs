using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Oelco.Common.Log;
using Oelco.Common.Utility;

namespace Oelco.CarisX.Comm
{
    public class CarisXCommIoTManager
    {
        IoTHub iotHub;

        public CarisXCommIoTManager()
        {

        }

        /// <summary>
        /// Connnect
        /// </summary>
        /// <param name="iotHubConnectStr"></param>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public async void ConnectIoT(string iotHubConnectStr, string deviceID)
        {
            iotHub = new IoTHub(iotHubConnectStr, deviceID);

            try
            {
                await iotHub.ConnectIotHub();
            }
            catch (Exception ex)
            {
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("!!!Failed !!! CarisXCommIoTManager.ConnectIoT() Message = {0} StackTrace = {1}", ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// DisConnect
        /// </summary>
        /// <returns></returns>
        public async void DisConnectIoT()
        {
            try
            {
                if (iotHub != null)
                {
                    await iotHub.DisconnectIotHub();

                    iotHub = null;
                }
            }
            catch (Exception ex)
            {
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("!!!Failed !!! CarisXCommIoTManager.DisConnectIoT() Message = {0} StackTrace = {1}", ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// SendMessage
        /// </summary>
        public async void SendMessage(string message)
        {
            if (iotHub == null)
            {
                return;
            }

            try
            {
                await iotHub.SendMessagetoDevice(message);
            }
            catch (Exception ex)
            {
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("!!!Failed !!! CarisXCommIoTManager.SendMessage() Message = {0} StackTrace = {1}", ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// Send Files to IoT 
        /// </summary>
        /// <param name="path">本地文件路径</param>
        public async void SendFiles(string path)
        {
            if (iotHub == null)
            {
                return;
            }

            try
            {
                var sendResult = await iotHub.SendToBlobAsync(path);
                if(sendResult != null && sendResult.Status == ResponseStatus.Success && File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("!!!Failed !!! CarisXCommIoTManager.SendFiles() Detail = {0} Message = {1} StackTrace = {2}", path , ex.Message, ex.StackTrace));
            }
        }
    }

    /// <summary>
    /// Content:IoT 核心类;Add by:Fang;Date:2019-01-03
    /// </summary>
    public class IoTHub
    {
        /// <summary>
        /// 连接密钥
        /// </summary>
        private string iotConnectionKey;
        /// <summary>
        /// 设备ID
        /// </summary>
        private string deviceID;

        private RegistryManager registryManager;

        private Device selectedDevice = null;

        private DeviceClient deviceClient;

        /// <summary>
        /// 连接密钥
        /// </summary>
        public string IotConnectionKey { get => iotConnectionKey; set => iotConnectionKey = value; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceID { get => deviceID; set => deviceID = value; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sIotConnectionKey"></param>
        /// <param name="sDeviceID"></param>
        public IoTHub(string sIotConnectionKey, string sDeviceID)
        {
            this.iotConnectionKey = sIotConnectionKey;
            this.deviceID = sDeviceID;
        }

        /// <summary>
        /// 连接IoT中心
        /// </summary>
        /// <returns></returns>
        public async Task ConnectIotHub()
        {
            registryManager = RegistryManager.CreateFromConnectionString(iotConnectionKey);
            IQuery query = registryManager.CreateQuery("select * from devices", null); ;
            while (query.HasMoreResults)
            {
                IEnumerable<Twin> page = await query.GetNextAsTwinAsync();
                foreach (Twin twin in page)
                {
                    var device = await registryManager.GetDeviceAsync(twin.DeviceId);
                    if (device.Id == deviceID)
                    {
                        selectedDevice = device;
                        break;
                    }
                }
            }
            if (selectedDevice == null)
            {
                throw new Exception("Device not exit!");
            }
            deviceClient = DeviceClient.CreateFromConnectionString(CreateDeviceConnectionString(selectedDevice));

            if (registryManager != null)
            {
                await registryManager.CloseAsync();
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        public async Task DisconnectIotHub()
        {
            if (deviceClient != null)
            {
                await deviceClient.CloseAsync();
            }
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="msg"></param>
        public async Task SendMessagetoDevice(string msg)
        {
            if (deviceClient != null)
            {
                var message = new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(msg));
                await deviceClient.SendEventAsync(message);
                await Task.Delay(1000);
            }
            else
            {
                throw new Exception("Device was failed to connect to the IOT when the pragram start to run!");
            }

        }

        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<ResponseResult> SendToBlobAsync(string path)
        {
            if (deviceClient != null)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        string filename = path.Substring(path.LastIndexOf("\\") + 1);
                        using (var sourceData = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            await deviceClient.UploadToBlobAsync(filename, sourceData);
                        }
                    }
                    else
                    {
                        throw new FileNotFoundException(string.Format("{0} not exist!", path));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return new ResponseResult(path, ResponseStatus.Success);
            }

            throw new Exception("Device was failed to connect to the IOT when the pragram start to run!");

        }

        /// <summary>
        /// 生成设备连接密钥
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        private String CreateDeviceConnectionString(Device device)
        {
            StringBuilder deviceConnectionString = new StringBuilder();

            var hostName = String.Empty;
            var tokenArray = iotConnectionKey.Split(';');
            for (int i = 0; i < tokenArray.Length; i++)
            {
                var keyValueArray = tokenArray[i].Split('=');
                if (keyValueArray[0] == "HostName")
                {
                    hostName = tokenArray[i] + ';';
                    break;
                }
            }

            if (!String.IsNullOrWhiteSpace(hostName))
            {
                deviceConnectionString.Append(hostName);
                deviceConnectionString.AppendFormat("DeviceId={0}", device.Id);

                if (device.Authentication != null)
                {
                    if ((device.Authentication.SymmetricKey != null) && (device.Authentication.SymmetricKey.PrimaryKey != null))
                    {
                        deviceConnectionString.AppendFormat(";SharedAccessKey={0}", device.Authentication.SymmetricKey.PrimaryKey);
                    }
                    else
                    {
                        deviceConnectionString.AppendFormat(";x509=true");
                    }
                }
            }

            return deviceConnectionString.ToString();
        }


    }

    public enum ResponseStatus
    {
        Success,
        Failure
    }

    public class ResponseResult
    {
        public string Message { get; set; }
        public ResponseStatus Status { get; set; }
        public ResponseResult(string message, ResponseStatus status)
        {
            this.Message = message;
            this.Status = status;
        }
    }
}
