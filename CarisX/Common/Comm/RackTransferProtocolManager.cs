using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Comm
{
    /// <summary>
    /// ラック搬送通信手順管理
    /// </summary>
    /// <remarks>
    /// 対ラック搬送の通信手順を管理します。
    /// </remarks>
    public class RackTransferProtocolManager : CommProtocolManager
    {
        // TODO:ラック搬送通信手順管理


        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferProtocolManager()
        {
            this.initialize();
        }
        
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 対ラック搬送用切断処理
        /// </summary>
        /// <remarks>
        /// CommSocketのClose()の前に呼ぶ対ラック搬送用切断処理
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public void CloseSession()
        {
            if (this.CommObject != null)
            {
                this.CommObject.CloseSession();
            }
        }

        /// <summary>
        /// 接続
        /// </summary>
        /// <remarks>
        /// 接続を行い、対ラック搬送用通信ログの設定を行う
        /// </remarks>
        /// <param name="parameter">接続パラメータ</param>
        /// <param name="saveFilePath">通信ログファイルパス</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean ConnectRackTransfer(Object parameter, String saveFilePath)
        {
            Boolean openSuccess = base.Connect(parameter);

            if (openSuccess == true)
            {
                this.CommObject.setMonitor(true);
                this.CommObject.SaveFilePath(saveFilePath);// 通信ログファイルパス設定
            }

            return openSuccess;
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// メンバの初期化を行います。
        /// </remarks>
        protected virtual void initialize()
        {
            this.CommObject = new CommSocket();
        }

        #endregion

    }
}
