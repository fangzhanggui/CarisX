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
        /// 仪器编号	
        /// </summary>
        public static long MACHINE_SERIAL_NUMBER = 1;

        /// <summary>
        /// IoT连接密钥【IssuesNo:16】连接密钥变量名称调整
        /// </summary>
        public static string IOT_CONNECTIONKEY = "Please input the connection key.";

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

		/// <summary>
        /// 【IssuesNo:16】区分单元1所对应的云端设备ID
        /// </summary>
        private long slave1No = MACHINE_SERIAL_NUMBER;

		/// <summary>
        /// 【IssuesNo:16】区分单元2所对应的云端设备ID
        /// </summary>
        private long slave2No = MACHINE_SERIAL_NUMBER;

		/// <summary>
        /// 【IssuesNo:16】区分单元3所对应的云端设备ID
        /// </summary>
        private long slave3No = MACHINE_SERIAL_NUMBER;

		/// <summary>
        /// 【IssuesNo:16】区分单元4所对应的云端设备ID
        /// </summary>
        private long slave4No = MACHINE_SERIAL_NUMBER;

		/// <summary>
        /// IoT连接密钥【IssuesNo:16】连接密钥变量名称调整
        /// </summary>
        private string iotConnectionkey = IOT_CONNECTIONKEY;

        private bool uploadMeasurementData = UPLOAD_MEASUREMENT_DATA;

        private bool uploadErrorCommand = UPLOAD_ERROR_COMMAND;

        private bool uploadDueDate = UPLOAD_DUE_DATE;

        private bool uploadLogfileAftAnalys = UPLOAD_LOGFILE_AFTER_ANALYSISING;

        private DateTime delivery_date = DELIVERY_DATE;

		/// <summary>
        /// 【IssuesNo:16】区分单元1所对应的云端设备ID
        /// </summary>
        public long Slave1No { get => slave1No; set => slave1No = value; }

		/// <summary>
        /// 【IssuesNo:16】区分单元2所对应的云端设备ID
        /// </summary>
        public long Slave2No { get => slave2No; set => slave2No = value; }

		/// <summary>
        /// 【IssuesNo:16】区分单元3所对应的云端设备ID
        /// </summary>
        public long Slave3No { get => slave3No; set => slave3No = value; }

		/// <summary>
        /// 【IssuesNo:16】区分单元4所对应的云端设备ID
        /// </summary>
        public long Slave4No { get => slave4No; set => slave4No = value; }

		/// <summary>
        /// IoT连接密钥【IssuesNo:16】连接密钥变量名称调整
        /// </summary>
        public string IoTConnectionKey { get => iotConnectionkey; set => iotConnectionkey = value; }

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
