using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Comm
{
    /// <summary>
    /// 通信コマンド
    /// </summary>
    /// <remarks>
    /// 通信コマンドのバイナリ-テキストを中継します。
    /// 各コマンドはこのクラスを継承し、個別に定義されます。
    /// </remarks>
    public class CommCommand
    {
        #region [クラス変数定義]

        /// <summary>
        /// 通信番号
        /// </summary>
        private Int32 commNo = 0;

        /// <summary>
        /// コマンド文字列
        /// </summary>
        private String commandText = String.Empty;

		/// <summary>
		/// 処理後待機イベント
		/// </summary>
		/// <remarks>
		/// このイベントを保有するコマンドを送信した際、送受信スレッドはこのイベントを待機します。
		/// </remarks>
		private System.Threading.ManualResetEvent waitEvent = null;

        #endregion

        #region [プロパティ]

		/// <summary>
		/// 処理後待機イベント
		/// </summary>
		/// <remarks>
		/// このイベントを保有するコマンドを送信した際、送受信スレッドはこのイベントを待機します。
		/// </remarks>
		[System.Xml.Serialization.XmlIgnore]
		public System.Threading.ManualResetEvent WaitEvent
		{
			get
			{
				return this.waitEvent;
			}
			set
			{
				this.waitEvent = value;
			}
		}

        /// <summary>
        /// コマンド文字列の取得、設定
        /// </summary>
        public virtual String CommandText
        {
            get
            {
                return commandText;
            }
            set
            {
                commandText = value;
            }
        }

        /// <summary>
        /// コマンド種別の取得、設定
        /// </summary>
        public virtual Int32 CommandId
        {
            // コマンド種別を整数値で返す
            get
            {
                return 0;
            }
            set
            {
                ;
            }
        }


        /// <summary>
        /// 通信番号の取得、設定
        /// </summary>
        public virtual Int32 CommNo
        {
            // 通信番号を整数値で返す
            get
            {
                return commNo;
            }
            set
            {
                commNo = value;
            }
        }
        
        // XXX:タイムアウト、応答Idは初期構想にあったが、SequenceHelperの登場により価値があまり無い？

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド文字列設定
        /// </summary>
        /// <remarks>
        /// コマンド文字列を設定します。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:コマンド文字列設定成功 False:コマンド文字列設定失敗</returns>
        public virtual Boolean SetCommandString( String commandStr )
        {
            this.commandText = commandStr;
            return true;
        }

        #endregion

    }

    ///// <summary>
    ///// コマンド種別
    ///// </summary>
    //abstract public class CommandKind
    //{
    //}
}
