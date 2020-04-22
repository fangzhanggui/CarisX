using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// IoT設定
    /// </summary>
    public class IoTParameter : AttachmentParameter
    {
        /// <summary>
        /// 机种
        /// </summary>
        public static short MODEL_ID = 1;

        /// <summary>
        /// 仪器编号	
        /// </summary>
        public static long MACHINE_SERIAL_NUMBER = 1;

        /// <summary>
        /// IoT连接密钥
        /// </summary>
        public static string IOT_CONNECTIONSTR = "Please input the connection key.";

        /// <summary>
        /// 是否上传测试数据
        /// </summary>
        public static bool UPLOAD_MEASUREMENT_DATA = true;

        /// <summary>
        /// 是否上传错误信息
        /// </summary>
        public static bool UPLOAD_ERROR_COMMAND = true;

        /// <summary>
        /// 是否上传使用日期
        /// </summary>
        public static bool UPLOAD_DUE_DATE = true;

        /// <summary>
        /// 是否使用文件上传（分析终止）
        /// </summary>
        public static bool UPLOAD_LOGFILE_AFTER_ANALYSISING = true;

        /// <summary>
        /// 交货日期
        /// </summary>
        public static DateTime DELIVERY_DATE = DateTime.MaxValue;

        private short modelId = MODEL_ID;

        private long machineSerialNumber = MACHINE_SERIAL_NUMBER;

        private string iotConnectionStr = IOT_CONNECTIONSTR;

        private bool uploadMeasurementData = UPLOAD_MEASUREMENT_DATA;

        private bool uploadErrorCommand = UPLOAD_ERROR_COMMAND;

        private bool uploadDueDate = UPLOAD_DUE_DATE;

        private bool uploadLogfileAftAnalys = UPLOAD_LOGFILE_AFTER_ANALYSISING;

        private DateTime delivery_date = DELIVERY_DATE;

        /// <summary>
        /// 机种
        /// </summary>
        public short ModelId { get => modelId; set => modelId = value; }

        /// <summary>
        ///  仪器编号	
        /// </summary>
        public long MachineSerialNumber { get => machineSerialNumber; set => machineSerialNumber = value; }

        /// <summary>
        /// IoT连接密钥
        /// </summary>
        public string IoTConnectionStr { get => iotConnectionStr; set => iotConnectionStr = value; }

        /// <summary>
        /// 是否上传测试数据
        /// </summary>
        public bool UploadMeasurementData { get => uploadMeasurementData; set => uploadMeasurementData = value; }

        /// <summary>
        /// 是否上传错误信息
        /// </summary>
        public bool UploadErrorCommand { get => uploadErrorCommand; set => uploadErrorCommand = value; }

        /// <summary>
        /// 是否上传使用日期
        /// </summary>
        public bool UploadDueDate { get => uploadDueDate; set => uploadDueDate = value; }

        /// <summary>
        /// 是否使用文件上传（分析终止）
        /// </summary>
        public bool UploadLogfileAftAnalys { get => uploadLogfileAftAnalys; set => uploadLogfileAftAnalys = value; }

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime Delivery_date { get => delivery_date; set => delivery_date = value; }
    }
}
