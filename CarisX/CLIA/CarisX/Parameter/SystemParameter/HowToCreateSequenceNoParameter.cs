using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// シーケンス番号発番方法設定
    /// </summary>
	public class HowToCreateSequenceNoParameter : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// 開始シーケンス番号（一般検体）　最小値設定
        /// </summary>
        public const Int32 START_SEQ_NO_PAT_MIN = 1;
        /// <summary>
        /// 開始シーケンス番号（一般検体）　最大値設定
        /// </summary>
        public const Int32 START_SEQ_NO_PAT_MAX = 9999;
        /// <summary>
        /// 開始シーケンス番号（優先検体）　最小値設定
        /// </summary>
        public const Int32 START_SEQ_NO_STAT_MIN = 1;
        /// <summary>
        /// 開始シーケンス番号（優先検体）　最大値設定
        /// </summary>
        public const Int32 START_SEQ_NO_STAT_MAX = 9999;
        /// <summary>
        /// 開始シーケンス番号（精度管理検体）　最小値設定
        /// </summary>
        public const Int32 START_SEQ_NO_CTRL_MIN = 1;
        /// <summary>
        /// 開始シーケンス番号（精度管理検体）　最大値設定
        /// </summary>
        public const Int32 START_SEQ_NO_CTRL_MAX = 9999;
        /// <summary>
        /// 開始シーケンス番号（キャリブレータ）　最小値設定
        /// </summary>
        public const Int32 START_SEQ_NO_CALIB_MIN = 1;
        /// <summary>
        /// 開始シーケンス番号（キャリブレータ）　最大値設定
        /// </summary>
        public const Int32 START_SEQ_NO_CALIB_MAX = 9999;

        /// <summary>
        /// 開始シーケンス番号（一般検体）デフォルト値設定
        /// </summary>
        public const Int32 START_SEQ_NO_PAT_DEFAULT = START_SEQ_NO_PAT_MIN;
        /// <summary>
        /// 開始シーケンス番号（優先検体）デフォルト値設定
        /// </summary>
        public const Int32 START_SEQ_NO_STAT_DEFAULT = 7001;
        /// <summary>
        /// 開始シーケンス番号（精度管理検体）デフォルト値設定
        /// </summary>
        public const Int32 START_SEQ_NO_CTRL_DEFAULT = 8001;
        /// <summary>
        /// 開始シーケンス番号（キャリブレータ）デフォルト値設定
        /// </summary>
        public const Int32 START_SEQ_NO_CALIB_DEFAULT = 9001;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 開始シーケンス番号（一般検体）
        /// </summary>
        private Int32 startSeqNoPat = START_SEQ_NO_PAT_DEFAULT;

        /// <summary>
        /// 開始シーケンス番号（優先検体）
        /// </summary>
        private Int32 startSeqNoStat = START_SEQ_NO_STAT_DEFAULT;

        /// <summary>
        /// 開始シーケンス番号（精度管理検体）
        /// </summary>
        private Int32 startSeqNoCtrl = START_SEQ_NO_CTRL_DEFAULT;

        /// <summary>
        /// 開始シーケンス番号（キャリブレータ検体）
        /// </summary>
        private Int32 startSeqNoCalib = START_SEQ_NO_CALIB_DEFAULT;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 開始シーケンス番号（一般検体）の設定／取得
        /// </summary>
        public Int32 StartSeqNoPat
        {
            get
            {
                return this.startSeqNoPat;
            }
            set
            {
                this.startSeqNoPat = value;
            }
        }

        /// <summary>
        /// 開始シーケンス番号（優先検体）の設定／取得
        /// </summary>
        public Int32 StartSeqNoStat
        {
            get
            {
                return this.startSeqNoStat;
            }
            set
            {
                this.startSeqNoStat = value;
            }
        }

        /// <summary>
        /// 開始シーケンス番号（精度管理検体）の設定／取得
        /// </summary>
        public Int32 StartSeqNoCtrl
        {
            get
            {
                return this.startSeqNoCtrl;
            }
            set
            {
                this.startSeqNoCtrl = value;
            }
        }

        /// <summary>
        /// 開始シーケンス番号（キャリブレータ）の設定／取得
        /// </summary>
        public Int32 StartSeqNoCalib
        {
            get
            {
                return this.startSeqNoCalib;
            }
            set
            {
                this.startSeqNoCalib = value;
            }
        }

        #endregion
	}
	 
}
 
