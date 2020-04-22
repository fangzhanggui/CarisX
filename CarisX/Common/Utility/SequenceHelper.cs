using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Reflection;

namespace Oelco.Common.Utility
{
    // シーケンスデリゲート別名 引数、戻り値にシーケンス名称。
    using SequenceDelegate = Func<String,Object[],String>;

    /// <summary>
    /// シーケンス補助
    /// </summary>
    /// <remarks>
    /// 応答を持つ動作に対して、
    /// 呼び出し側からの動作は1関数のコールで行えるよう定義を行います。
    /// </remarks>
    public class SequenceHelper
    {

        #region [定数定義]

        /// <summary>
        /// 重複シーケンス終了待ち時間デフォルト(30秒)
        /// </summary>
        protected const Int32 DEFAULT_SEQUENCE_WAIT_TIME = 1000 * 30;

        #endregion

        #region [インスタンス変数定義]
        
        /// <summary>
        /// 動作中シーケンス辞書
        /// </summary>
        private LockObject<Dictionary<String, Tuple<Semaphore, SimpleCounter>>> activeSequenceDic = new LockObject<Dictionary<String, Tuple<Semaphore, SimpleCounter>>>();
        //private LockObject<Dictionary<String, Mutex>> activeSequenceDic = new LockObject<Dictionary<String, Mutex>>();

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 動作中シーケンス辞書 取得
        /// </summary>
        protected LockObject<Dictionary<String, Tuple<Semaphore,SimpleCounter>>> ActiveSequenceDic
        {
            get
            {
                return activeSequenceDic;
            }
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// 動作中シーケンス設定
        /// </summary>
        /// <remarks>
        /// 指定のシーケンス名称を、動作中シーケンスに設定します。
        /// この関数は必ず動作登録済みのシーケンス名称を指定して呼び出されます。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <param name="waitTimeout">待機タイムアウト</param>
        /// <returns>True:成功 False:タイムアウト</returns>
        protected Boolean setActiveSequence( String sequenceName, Int32 waitTimeout )
        {
            this.activeSequenceDic.Lock();
            Tuple<Semaphore, SimpleCounter> semaphore = this.activeSequenceDic.Get.Instance[sequenceName];
            this.activeSequenceDic.UnLock();

            // スレッドはセマフォ保持状態になる(タイムアウト待ち)
            Boolean successGetMutex = semaphore.Item1.WaitOne( waitTimeout );
            if (successGetMutex)
            {
                //Mutexを取得できた場合のみカウンターをインクリメントする
                semaphore.Item2.Increment();
            }

            return successGetMutex;
        }



        /// <summary>
        /// シーケンス関数動作開始
        /// </summary>
        /// <remarks>
        /// 指定したシーケンス関数を、指定のシーケンス名称として開始します。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <param name="sequence">シーケンス関数</param>
        protected void startSequence( String sequenceName, Object[] param, SequenceDelegate sequence )
        {
            // 実行中シーケンス辞書追加
            this.activeSequenceDic.Lock();
            if ( !this.activeSequenceDic.Get.Instance.ContainsKey( sequenceName ) )
            {
                this.activeSequenceDic.Get.Instance.Add( sequenceName, new Tuple<Semaphore, SimpleCounter>( new Semaphore( 1, 1 ), new SimpleCounter() ) );
            }
            this.activeSequenceDic.UnLock();

            // シーケンス関数の非同期実行を開始する。
            sequence.BeginInvoke( sequenceName, param, this.finallySequence, sequence );
        }

        /// <summary>
        /// シーケンス関数動作完了コールバック
        /// </summary>
        /// <remarks>
        /// シーケンス関数の非同期実行が終了した際に呼び出されます。
        /// シーケンス関数動作の完了処理を行います。
        /// </remarks>
        /// <param name="asyncResult">非同期動作情報</param>
        protected void finallySequence( IAsyncResult asyncResult )
        {
            // シーケンスデリゲート情報取得
            SequenceDelegate de = (SequenceDelegate)asyncResult.AsyncState;

            // nullチェック
            if (de == null)
            {
                return;
            }
            try
            {
                String sequenceName = (String)de.EndInvoke( asyncResult );
                // 実行中シーケンスから外す
                this.reSetActiveSequence( sequenceName );
            }
            catch ( Exception ex )
            {
                // シーケンス関数内でキャッチされない例外が発生した場合ここにくる。
                // 正常ケースではエラー時含めここへ来る事は無いので、要対応
                //シーケンス内部でキャッチされない例外が発生しました。
                System.Diagnostics.Debug.WriteLine(String.Format("【Critical Error】Exceptions that are not caught in the internal sequence occurred{0}:{1}", ex.Message, ex.StackTrace));
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Log.LogKind.DebugLog, String.Format("【Critical Error】Exceptions that are not caught in the internal sequence occurred。{0}:{1}", ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region [privateメソッド]
        
        /// <summary>
        /// 動作中シーケンス解除
        /// </summary>
        /// <remarks>
        /// SequenceHelperが内部的にコールします。
        /// 指定のシーケンス名称を、動作中シーケンスから解除します。
        /// 指定のシーケンス名称が動作中ではない場合、この関数は失敗します。
        /// この関数は必ず動作登録済みのシーケンス名称を指定して呼び出されます。
        /// </remarks>
        /// <param name="sequenceName">シーケンス名称</param>
        /// <returns>True:成功 False:失敗</returns>
        private Boolean reSetActiveSequence( String sequenceName )
        {
            //同時実行数の上限で実行をキャンセルしている場合、以降の処理をしない
            if (sequenceName == "CANCEL")
            {
                return true;
            }

            this.activeSequenceDic.Lock();
            Tuple<Semaphore, SimpleCounter> semaphore = this.activeSequenceDic.Get.Instance[sequenceName];
            this.activeSequenceDic.UnLock();

            // セマフォを開放
            Boolean successReleaseMutex = true;
            if ( semaphore.Item2.Number != 0 )
            {
                semaphore.Item1.Release();
                semaphore.Item2.Decrement();
            }

            //// 呼び出しスレッドがミューテックスを保持していた場合開放
            //Boolean successReleaseMutex = false;
            //if ( semaphore.WaitOne( 0 ) )
            //{
            //    semaphore.Release();
            //    successReleaseMutex = true;
            //}

            return successReleaseMutex;
        }

        #endregion

        #region [内部クラス]

        /// <summary>
        /// カウントクラス
        /// </summary>
        /// <remarks>
        /// 単純な値の操作を提供します。
        /// </remarks>
        protected class SimpleCounter
        {
            #region [インスタンス変数定義]

            /// <summary>
            /// 保持値
            /// </summary>
            Int32 number = 0;

            #endregion

            #region [プロパティ]

            /// <summary>
            /// 保持値 取得/設定
            /// </summary>
            public Int32 Number
            {
                get
                {
                    return number;
                }
                set
                {
                    number = value;
                }
            }

            #endregion

            #region [publicメソッド]

            /// <summary>
            /// インクリメント
            /// </summary>
            /// <remarks>
            /// 保持値をインクリメントします。
            /// </remarks>
            public void Increment()
            {
                number++;
            }

            /// <summary>
            /// デクリメント
            /// </summary>
            /// <remarks>
            /// 保持地をデクリメントします。
            /// </remarks>
            public void Decrement()
            {
                number--;
            }

            #endregion

        }

        #endregion
    }
}
