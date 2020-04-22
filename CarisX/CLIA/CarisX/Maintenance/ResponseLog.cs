using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using Oelco.CarisX.Parameter;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.Common.Comm;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Const;
using System.Diagnostics;


namespace Oelco.CarisX.Maintenance
{
    public class ResponseLog
    {
        public string LogFilePath = "";
        public Int32 SequenceNo { get; set; } = 0;
        public string ModuleName { get; set; } = "";
        public string UnitName { get; set; } = "";
        public string SequenceName { get; set; } = "";
        public Int32 RepeatNo { get; set; } = 0;

        /// <summary>
        /// ログファイルを作成する
        /// </summary>
        /// <remarks>
        /// LogFilePathの内容をここで定義し、以降はLogFilePathに設定されているログファイルに対して書き込みを行う。
        /// その為、ログファイルを分割したいタイミングで必ずこのメソッドの呼び出しが必要。
        /// </remarks>
        public void CreateLog()
        {
            LogFilePath = Path.Combine(CarisXConst.PathLog, "MainteRsp", DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");

            try
            {
                using (FileStream fs = File.Open(LogFilePath, FileMode.Create))
                {
                    //ファイルを作成
                }
            }
            catch (Exception ex)
            {
                // TODO:とりあえずイベントログに出しておきます
                EventLog.WriteEntry(System.Reflection.Assembly.GetExecutingAssembly().ToString(), ex.ToString(), EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// ログに出力する内容を編集する
        /// </summary>
        private string EditLogText(string response)
        {

            List<string> logtextList = new List<string> { };

            //シーケンス名の「1: ～」の「～」の部分のみ取得する
            Int32 SequenceNameStrIdx = SequenceName.IndexOf(":");
            if (0 <= SequenceNameStrIdx)
            {
                SequenceName = SequenceName.Substring(SequenceNameStrIdx + 2);
            }

            logtextList.Add(SequenceNo.ToString());
            logtextList.Add(ModuleName);
            logtextList.Add(UnitName);
            logtextList.Add(SequenceName);
            logtextList.Add(RepeatNo.ToString());
            logtextList.Add(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            logtextList.Add(response);

            return string.Join(",", logtextList);
        }


        /// <summary>
        /// ログファイルに書き込みする
        /// </summary>
        public void WriteLog(string responce)
        {
            if ("" == LogFilePath || !File.Exists(LogFilePath))
            {
                //ログファイルパスが設定されていない、またはファイルが存在しない場合、ファイルを作成する
                CreateLog();
            }

            try
            {
                string logtext = EditLogText(responce);

                using (FileStream fs = File.Open(LogFilePath, FileMode.Append))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.WriteLine(logtext);
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO:とりあえずイベントログに出しておきます
                EventLog.WriteEntry(System.Reflection.Assembly.GetExecutingAssembly().ToString(), ex.ToString(), EventLogEntryType.Warning);
            }
        }



    }
}
