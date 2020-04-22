using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;
using Oelco.Common.Log;
using Oelco.Common.Utility;

namespace Oelco.CarisX.Comm
{
    //【IssuesNo:16】类方法优化调整
    public class CarisXCommIoTManager
    {
        private IoTHub iotHub;

        /// <summary>
        /// 【IssuesNo:16】默认Iot状态为未连接
        /// </summary>
        private IoTStatusKind ioTStatus = IoTStatusKind.NoLink;

        /// <summary>
        /// 【IssuesNo:16】IoT连接状态取得
        /// </summary>
        public IoTStatusKind IoTStatus
        {
            get
            {
                return this.ioTStatus;
            }
            set
            {
                this.ioTStatus = value;
            }
        }

        /// <summary>
        /// Connnect
        /// </summary>
        /// <param name="iotHubConnectKey"></param>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public async void ConnectIoT(string iotHubConnectKey, string deviceID)
        {
            iotHub = new IoTHub(iotHubConnectKey, deviceID);
            var result = await iotHub.ConnectIotHub();
            if(result.Status == ResponseStatus.Success)
            {
                this.ioTStatus = IoTStatusKind.OnLine;
            }
            else
            {
                this.ioTStatus = IoTStatusKind.NoLink;
            }

            Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, result.Message);
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.IoTStatus, this.ioTStatus);

        }

        /// <summary>
        /// DisConnect
        /// </summary>
        /// <returns></returns>
        public async void DisConnectIoT()
        {
            if (iotHub != null)
            {
                var result =  await iotHub.DisconnectIotHub();
                if(result.Status == ResponseStatus.Success)
                {
                    iotHub = null;
                    this.ioTStatus = IoTStatusKind.NoLink;
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.IoTStatus, this.ioTStatus);
                }               

                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, result.Message);            
            }
        }

        /// <summary>
        /// SendMessage
        /// </summary>
        public async void SendMessage(string message)
        {
            if (iotHub != null)
            {
                var result = await iotHub.SendMessagetoDevice(message);
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, result.Message);
            }
        }

        /// <summary>
        /// Send Files to IoT 
        /// </summary>
        /// <param name="path">本地文件路径</param>
        public async void SendFiles(string path)
        {
            if (iotHub != null)
            {
                var result = await iotHub.SendToBlobAsync(path);
                if (result.Status == ResponseStatus.Success && CarisXSubFunction.CheckFileStatus(path))
                {
                    File.Delete(path);
                }

                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, result.Message);              
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

        /// <summary>
        /// 用于发送信息的客户端
        /// </summary>
        private DeviceClient deviceClient;

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
        public async Task<ResponseResult> ConnectIotHub()
        {
            //【IssuesNo:15】去除查询和遍历设备，用设备号直连可以提升连接速度
            RegistryManager registryManager = null;
            String dbgMsgHead = String.Format("[[Investigation log]]IoTManager::{0} ", MethodBase.GetCurrentMethod().DeclaringType.Name);
            String dbgMsg = string.Empty;
            try
            {
                registryManager = RegistryManager.CreateFromConnectionString(iotConnectionKey);
                var device = await registryManager.GetDeviceAsync(deviceID);

                if (device != null && device.Id.Equals(deviceID))
                {
                    deviceClient = DeviceClient.CreateFromConnectionString(CreateDeviceConnectionString(device));
                }
                else
                {
                    dbgMsg = dbgMsgHead + string.Format(" Success = False ; Date = {0} ; Detail = {1}", DateTime.Now, "Device not exit!");
                    return new ResponseResult(dbgMsg, ResponseStatus.Failure);
                }            
            }
            catch(Exception ex)
            {
                dbgMsg = dbgMsgHead + string.Format(" Success = False ; Date = {0} ; Detail = {1}", DateTime.Now, ex.Message + "\n" + ex.StackTrace);
                return new ResponseResult(dbgMsg, ResponseStatus.Failure);
            }
            finally
            {
                if (registryManager != null)
                {
                    await registryManager.CloseAsync();
                }
            }

            dbgMsg = dbgMsgHead + String.Format(" Success = True ; Date = {0}", DateTime.Now);
            return new ResponseResult(dbgMsg, ResponseStatus.Success);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult> DisconnectIotHub()
        {
            String dbgMsgHead = String.Format("[[Investigation log]]IoTManager::{0} ", MethodBase.GetCurrentMethod().DeclaringType.Name);
            String dbgMsg = string.Empty;
            try
            {
                if (deviceClient != null)
                {
                    await deviceClient.CloseAsync();
                }
            }
            catch(Exception ex)
            {
                dbgMsg = dbgMsgHead + string.Format(" Success = False ; Date = {0} ; Detail = {1}", DateTime.Now, ex.Message + "\n" + ex.StackTrace);
                return new ResponseResult(dbgMsg, ResponseStatus.Failure);
            }

            dbgMsg = dbgMsgHead + String.Format(" Success = True ; Date = {0}", DateTime.Now);
            return new ResponseResult(dbgMsg, ResponseStatus.Success);
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="msg"></param>
        public async Task<ResponseResult> SendMessagetoDevice(string msg)
        {
            String dbgMsgHead = String.Format("[[Investigation log]]IoTManager::{0} ", MethodBase.GetCurrentMethod().DeclaringType.Name);
            String dbgMsg = string.Empty;

            try
            {
                if (deviceClient != null)
                {
                    var message = new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(msg));
                    await deviceClient.SendEventAsync(message);
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                dbgMsg = dbgMsgHead + string.Format(" Success = False ; Date = {0} ; Msg = {1} ; Detail = {2} ; ", DateTime.Now, msg, ex.Message + "\n" + ex.StackTrace);
                return new ResponseResult(dbgMsg, ResponseStatus.Failure);
            }

            dbgMsg = dbgMsgHead + String.Format(" Success = True ; Date = {0} ; Msg = {1}", DateTime.Now,msg);
            return new ResponseResult(dbgMsg, ResponseStatus.Success);

        }

        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<ResponseResult> SendToBlobAsync(string path)
        {
            String dbgMsgHead = String.Format("[[Investigation log]]IoTManager::{0} ", MethodBase.GetCurrentMethod().DeclaringType.Name);
            String dbgMsg = string.Empty;

            try
            {
                if (deviceClient != null)
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
                        dbgMsg = dbgMsgHead + string.Format(" Success = False ; Date = {0} ; Detail = {1} not exist ", DateTime.Now, Path.GetFileNameWithoutExtension(path));
                        return new ResponseResult(dbgMsg, ResponseStatus.Failure);
                    }
                }
            }
            catch (Exception ex)
            {
                dbgMsg = dbgMsgHead + string.Format(" Success = False ; Date = {0} ; Path = {1} ; Detail = {2}", DateTime.Now, Path.GetFileNameWithoutExtension(path), ex.Message + "\n" + ex.StackTrace);
                return new ResponseResult(dbgMsg, ResponseStatus.Failure);
            }

            dbgMsg = dbgMsgHead + String.Format(" Success = True ; Date = {0} ; Path = {1}", DateTime.Now,path);
            return new ResponseResult(dbgMsg, ResponseStatus.Success);
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

    /// <summary>
    /// 【IssuesNo:16】新增IoT连接状态，在全局中管理连接状态
    /// </summary>
    public enum IoTStatusKind
    {
        /// <summary>
        /// 未连接
        /// </summary>
        NoLink,
        /// <summary>
        /// 在线
        /// </summary>
        OnLine
    }
}
