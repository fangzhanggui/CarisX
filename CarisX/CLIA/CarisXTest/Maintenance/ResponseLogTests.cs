using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oelco.CarisX.Maintenance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Oelco.CarisX.Maintenance.Tests
{
    [TestClass()]
    public class ResponseLogTests
    {
        const string eventsource = "CarisX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        string filepath;
        string datetimenow;
        Boolean existsEventLog;

        [TestMethod()]
        public void CreateLogTest()
        {
            ResponseLog reslog = new ResponseLog();

            datetimenow = DateTime.Now.ToString("yyyyMMddHHmmss");
            filepath = Path.Combine(Application.StartupPath, datetimenow + ".log");

            reslog.CreateLog();

            //ログファイルのパスが正しいこと
            Assert.AreEqual(filepath, reslog.LogFilePath);

            //ログファイルが存在すること
            Assert.AreEqual(true, File.Exists(reslog.LogFilePath));
            
            //ログファイルを既にオープンした状態で処理を実行する
            using (FileStream fs = File.Open(filepath, FileMode.Create))
            {
                datetimenow = DateTime.Now.ToString("yyyyMMddHHmmss");
                filepath = Path.Combine(Application.StartupPath, datetimenow + ".log");

                reslog.CreateLog();

                existsEventLog = false;
                //ログが書き込まれているかチェック
                if (System.Diagnostics.EventLog.Exists("Application"))
                {
                    //EventLogオブジェクトを作成する
                    System.Diagnostics.EventLog log = new System.Diagnostics.EventLog("Application");

                    //ログエントリをすべて取得する
                    foreach (System.Diagnostics.EventLogEntry entry in log.Entries)
                    {
                        if (eventsource == entry.Source && double.Parse(datetimenow) <= double.Parse(entry.TimeWritten.ToString("yyyyMMddHHmmss")))
                        {
                            //CarisXから書き込みされたイベントが存在すればフラグ立てる
                            existsEventLog = true;
                        }
                    }

                    //閉じる
                    log.Close();
                }
            }

            Assert.AreEqual(true, existsEventLog);

            //１秒待機
            System.Threading.Thread.Sleep(1000);
        }

        [TestMethod()]
        public void EditLogTextTest()
        {
            ResponseLog reslog = new ResponseLog();
            var pbObj = new PrivateObject(reslog);

            reslog.SequenceNo = 1;
            reslog.ModuleName = "2";
            reslog.UnitName = "3";
            reslog.SequenceName = "0: 4";
            reslog.RepeatNo = 5;

            Assert.AreEqual("1,2,3,4,5," + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ",test", pbObj.Invoke("EditLogText", "test"));

        }

        [TestMethod()]
        public void WriteLogTest()
        {
            ResponseLog reslog = new ResponseLog();

            reslog.SequenceNo = 1;
            reslog.ModuleName = "2";
            reslog.UnitName = "3";
            reslog.SequenceName = "0: 4";
            reslog.RepeatNo = 5;

            //ログファイルパスが未設定の状態
            reslog.WriteLog("test");

            //ファイルできた？
            datetimenow = DateTime.Now.ToString("yyyyMMddHHmmss");
            filepath = Path.Combine(Application.StartupPath, datetimenow + ".log");
            //ログファイルが存在すること
            Assert.AreEqual(true, File.Exists(reslog.LogFilePath));
            //ファイルに書き込みした内容が正しいか
            using (FileStream fs = File.Open(reslog.LogFilePath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    Assert.AreEqual("1,2,3,4,5," + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ",test", sr.ReadLine());
                }
            }


            //１秒待機
            System.Threading.Thread.Sleep(1000);


            //存在しないファイルパスを設定する
            reslog.LogFilePath = "C:\test.log";

            //ログ出力
            reslog.WriteLog("test");

            //ファイルできた？
            datetimenow = DateTime.Now.ToString("yyyyMMddHHmmss");
            filepath = Path.Combine(Application.StartupPath, datetimenow + ".log");
            //ファイルパスが変わった？
            Assert.AreEqual(filepath, reslog.LogFilePath);
            //ログファイルが存在すること
            Assert.AreEqual(true, File.Exists(reslog.LogFilePath));


            //１秒待機
            System.Threading.Thread.Sleep(1000);


            //ログファイルを既にオープンした状態で処理を実行する
            using (FileStream fs = File.Open(reslog.LogFilePath, FileMode.Append))
            {
                datetimenow = DateTime.Now.ToString("yyyyMMddHHmmss");

                reslog.WriteLog("test");

                existsEventLog = false;
                //ログが書き込まれているかチェック
                if (System.Diagnostics.EventLog.Exists("Application"))
                {
                    //EventLogオブジェクトを作成する
                    System.Diagnostics.EventLog log = new System.Diagnostics.EventLog("Application");

                    //ログエントリをすべて取得する
                    foreach (System.Diagnostics.EventLogEntry entry in log.Entries)
                    {
                        if (eventsource == entry.Source && double.Parse(datetimenow) <= double.Parse(entry.TimeWritten.ToString("yyyyMMddHHmmss")))
                        {
                            //CarisXから書き込みされたイベントが存在すればフラグ立てる
                            existsEventLog = true;
                        }
                    }

                    //閉じる
                    log.Close();
                }
            }

            Assert.AreEqual(true, existsEventLog);

            //１秒待機
            System.Threading.Thread.Sleep(1000);
        }
    }
}