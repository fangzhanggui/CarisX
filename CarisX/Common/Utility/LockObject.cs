//----------------------------------------------------------------
// Public Class. 		
//	  LockObject
// Info.			
//   排他機能を提供する。
// History			
//   2011/09/01　　Ver1.00.00　　内部処理作成	M.T
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Oelco.Common.Utility
{
    /// <summary>
    /// 排他オブジェクトクラス
    /// </summary>
    /// <typeparam name="TypeT">排他機能適用型</typeparam>
    /// <example>
    /// LockObject&lt;Int32&gt; intLockObject = new LockObject&lt;Int32&gt;();
    /// intLockObject.Lock();               // 排他
    /// intLockObject.Get.Instance = 100;   // 操作
    /// intLockObject.UnLock();             // 排他解除
    /// </example>
    public class LockObject<TypeT> where TypeT: new()
    {
        /// <summary>
        /// 参照中継クラス
        /// </summary>
        /// <remarks>
        /// 参照型と値型、両方で利用可能なように参照を中継するクラス。
        /// </remarks>
        public class ReferenceRelay
        {
            #region [インスタンス変数定義]

            /// <summary>
            /// 対象オブジェクトインスタンス
            /// </summary>
            private TypeT instance = default( TypeT );

            #endregion

            #region [コンストラクタ/デストラクタ]

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="setInstance">対象インスタンス</param>
            public ReferenceRelay( ref TypeT setInstance )
            {
                instance = setInstance;
            }

            #endregion

            #region [プロパティ]

            /// <summary>
            /// 対象インスタンス取得/設定
            /// </summary>
            public TypeT Instance
            {
                get
                {
                    return instance;
                }
                set
                {
                    instance = value;
                }
            }

            #endregion

        }

        #region "インスタンス変数定義"

        /// <summary>
        /// 排他制御オブジェクト
        /// </summary>
        private Mutex mMutexObject = new Mutex( false, null );

        /// <summary>
        /// 参照中継オブジェクト
        /// </summary>
        protected ReferenceRelay relay = null;

        #endregion

        #region "コンストラクタ"

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// オブジェクトの初期化を行います。
        /// </remarks>
        public  LockObject()
        {
            TypeT tmp = new TypeT();
            relay = new ReferenceRelay( ref tmp );
        }

        #endregion

        #region "プロパティ"

        /// <summary>
        /// オブジェクト取得
        /// </summary>
        public ReferenceRelay Get
        {
            get
            {
                return this.relay;
            }
        }

        #endregion

        #region "Publicメソッド"


        /// <summary>
        /// ロック処理
        /// </summary>
        /// <remarks>
        /// オブジェクトのロックを行います
        /// </remarks>
        public void Lock()
        {
            mMutexObject.WaitOne();
        }

        /// <summary>
        /// アンロック処理
        /// </summary>
        /// <remarks>
        /// オブジェクトのアンロックを行います
        /// </remarks>
        public void UnLock()
        {
            mMutexObject.ReleaseMutex();

        }


        /// <summary>
        /// オブジェクト設定
        /// </summary>
        /// <remarks>
        /// 対象オブジェクトの設定を行います
        /// </remarks>
        /// <param name="rObj">対象オブジェクト</param>
        public void SetObject( ref TypeT rObj )
        {
            TypeT tmp = rObj;
            this.relay = new ReferenceRelay( ref tmp );
        }

        #endregion

    }
}