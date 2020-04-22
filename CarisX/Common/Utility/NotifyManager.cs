using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Oelco.Common.Log;
using System.Threading.Tasks;

namespace Oelco.Common.Utility
{

    /// <summary>
    /// 通知管理
    /// </summary>
    /// <remarks>
    /// イベントによる通知機能の提供を行います。
    /// このクラスはExecuteAllQueueを単一のスレッド上にて実行する場合、スレッドセーフです。
    /// </remarks>
    public class NotifyManager
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 通知先辞書
        /// </summary>
        private LockObject<Dictionary<Int32, List<Action<Object>>>> notifyDic = new LockObject<Dictionary<Int32, List<Action<Object>>>>();

        /// <summary>
        /// シグナル発生状態キュー
        /// </summary>
        private LockObject<Queue<Tuple<Int32, Object>>> notifyQueue = new LockObject<Queue<Tuple<Int32, Object>>>();

        /// <summary>
        /// キュー処理済通知イベント
        /// </summary>
        private ManualResetEvent queueProcessed = new ManualResetEvent( false );

        /// <summary>
        /// プロセスで使用されているメインフォームのインスタンス
        /// </summary>
        private System.Windows.Forms.Form mainForm = null;

        /// <summary>
        /// ミューテックス
        /// </summary>
        private Mutex mutexObject = new Mutex();

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 通知ターゲット追加
        /// </summary>
        /// <remarks>
        /// 通知種別にに対して呼び出されるハンドラを登録します。
        /// </remarks>
        /// <param name="notifyKind">通知種別</param>
        /// <param name="handlerFunc">通知関数</param>
        public void AddNotifyTarget( Int32 notifyKind, Action<Object> handlerFunc )
        {
            this.mainForm = Utility.SubFunction.GetMainForm();
            this.notifyDic.Lock();
            // キーが存在すれば追加、しなければ作成
            if ( !this.notifyDic.Get.Instance.ContainsKey( notifyKind ) )
            {
                this.notifyDic.Get.Instance.Add( notifyKind, new List<Action<Object>>() );
                //通知対象が登録されました
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("Notification object has been registered NotifyID={0} Handler={1}::{2}", notifyKind, handlerFunc.Method.ReflectedType.Name, handlerFunc.Method.Name));

            }
            this.notifyDic.Get.Instance[notifyKind].Add( handlerFunc );
            this.notifyDic.UnLock();
        }

        /// <summary>
        /// 通知ターゲット削除
        /// </summary>
        /// <remarks>
        /// 通知種別に対して、呼び出されるハンドラを削除します。
        /// </remarks>
        /// <param name="notifyKind">通知種別</param>
        /// <param name="handlerFunc">通知関数</param>
        public void RemoveNotifyTarget( Int32 notifyKind, Action<Object> handlerFunc )
        {
            // 指定要素を取り除く
            this.notifyDic.Lock();
            if ( this.notifyDic.Get.Instance.ContainsKey( notifyKind ) )
            {
                if ( this.notifyDic.Get.Instance[notifyKind].Contains( handlerFunc ) )
                {
                    this.notifyDic.Get.Instance[notifyKind].Remove( handlerFunc );
                    //通知対象が解除されました
                    Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("Notification target has been removed NotifyID={0} Handler={1}::{2}", notifyKind, handlerFunc.Method.ReflectedType.Name, handlerFunc.Method.Name));

                    // 空になった場合、リストを削除する。
                    if ( this.notifyDic.Get.Instance[notifyKind].Count == 0 )
                    {
                        this.notifyDic.Get.Instance.Remove( notifyKind );
                    }
                }
            }
            this.notifyDic.UnLock();
        }

        /// <summary>
        /// シグナル追加
        /// </summary>
        /// <remarks>
        /// 指定の通知を発生状態とします。
        /// </remarks>
        /// <param name="notifyKind">通知種別</param>
        /// <param name="value">受け渡し値</param>
        public void PushSignalQueue( Int32 notifyKind, Object value = null )
        {
            this.notifyQueue.Lock();
            this.notifyQueue.Get.Instance.Enqueue( new Tuple<Int32, Object>( notifyKind, value ) );
            Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("Notification occurred NotifyID={0}", notifyKind));
            this.notifyQueue.UnLock();

        }

        /// <summary>
        /// シグナル通知
        /// </summary>
        /// <remarks>
        /// 指定の通知に関連付けられたハンドラを実行します。
        /// この関数呼び出しを行う場合、関連付けられたハンドラ内部でコードが停止するケースがある場合(ShowDialog等)、
        /// この関数を抜けることは出来ません。
        /// </remarks>
        /// <param name="notifyKind">通知種別</param>
        /// <param name="value">受け渡し値</param>
        public void RaiseSignalQueue( Int32 notifyKind, Object value = null )
        {
            this.notifyQueue.Lock();
            //this.queueProcessed.Reset();
            this.notifyQueue.Get.Instance.Enqueue( new Tuple<Int32, Object>( notifyKind, value ) );
            //通知を発生させました
            Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, String.Format("Notification initiate NotifyID={0}", notifyKind ) );
            this.notifyQueue.UnLock();

            //if ( this.notifyQueue.Get.Instance.Count == 0 )
            //{
            //    return;
            //}

            //Action queueExecutor = () =>
            //{
            //    // キュー内容を全て処理
            //    while ( this.notifyQueue.Get.Instance.Count != 0 )
            //    {
            //        this.notifyQueue.Lock();
            //        Tuple<Int32, Object> queue = this.notifyQueue.Get.Instance.Dequeue();
            //        this.notifyQueue.UnLock();
            //        if ( this.notifyDic.Get.Instance.ContainsKey( queue.Item1 ) )
            //        {
            //            // 通知の実施
            //            foreach ( Action<Object> handler in this.notifyDic.Get.Instance[queue.Item1] )
            //            {
            //                Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, String.Format( "通知内容に登録されたハンドラを実行します 通知ID={0} ハンドラ={1}::{2}", queue.Item1, handler.Method.ReflectedType.Name, handler.Method.Name ) );
            //                handler( queue.Item2 );
            //            }
            //        }
            //    }
            //    //this.queueProcessed.Set();
            //};

            //Task notifyTask = new Task( () =>
            //{
            //    if ( this.mainForm != null &&
            //         this.mainForm.IsHandleCreated &&
            //        !this.mainForm.IsDisposed &&
            //         this.mainForm.InvokeRequired )
            //    {
            //        //this.queueProcessed.WaitOne();
            //        this.mainForm.Invoke( queueExecutor );
            //        //lock ( this.mainForm )
            //        //{
            //        //    this.mainForm.Invoke( (Action)( () => this.ExecuteAllQueue() ) );
            //        //}
            //    }
            //    else
            //    {
            //        queueExecutor();
            //    }
            //} );
            //notifyTask.Start();

            this.ExecuteAllQueue();
            ////if ( this.mainForm != null &&
            ////    this.mainForm.IsHandleCreated &&
            ////    !this.mainForm.IsDisposed && 
            ////    this.mainForm.InvokeRequired )
            ////{
            ////    lock ( this.mainForm )
            ////    {
            ////        this.mainForm.Invoke( (Action)( () => this.ExecuteAllQueue() ) );
            ////    }
            ////}
            ////else
            ////{
            ////    this.ExecuteAllQueue();
            //}
        }


        /// <summary>
        /// 通知実行
        /// </summary>
        /// <remarks>
        /// 発生状態の通知に関連付けられたハンドラを、全て実行します。
        /// この関数はメインスレッド以外から呼び出さないで下さい。
        /// </remarks>
        public void ExecuteAllQueue()
        {
            //this.mutexObject.WaitOne();
            if ( this.notifyQueue.Get.Instance.Count == 0 )
            {
                return;
            }
            Action queueExecutor = () =>
            {
                // キュー内容を全て処理
                while ( this.notifyQueue.Get.Instance.Count != 0 )
                {
                    this.notifyQueue.Lock();
                    Tuple<Int32, Object> queue = this.notifyQueue.Get.Instance.Dequeue();
                    this.notifyQueue.UnLock();
                    if ( this.notifyDic.Get.Instance.ContainsKey( queue.Item1 ) )
                    {
                        // 通知の実施
                        foreach ( Action<Object> handler in this.notifyDic.Get.Instance[queue.Item1] )
                        {
                            //通知内容に登録されたハンドラを実行します
                            Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("run a handler that is registered with the contents of the notification NotifyID={0} Handler={1}::{2}", queue.Item1, handler.Method.ReflectedType.Name, handler.Method.Name));
                            handler( queue.Item2 );
                        }
                    }
                }
                //this.queueProcessed.Set();
            };
            if ( this.mainForm != null &&
                   this.mainForm.IsHandleCreated &&
                  !this.mainForm.IsDisposed &&
                   this.mainForm.InvokeRequired )
            {
                //this.queueProcessed.WaitOne();
                this.mainForm.Invoke( queueExecutor );
                //lock ( this.mainForm )
                //{
                //    this.mainForm.Invoke( (Action)( () => this.ExecuteAllQueue() ) );
                //}
            }
            else
            {
                queueExecutor();
            }
        }

        #endregion

    }
}
