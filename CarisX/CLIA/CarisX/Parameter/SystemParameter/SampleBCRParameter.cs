using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 検体バーコードリーダー設定
    /// </summary>
	public class SampleBCRParameter : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// 検体ID桁数　最小桁数設定
        /// </summary>
        public const Int32 RACK_ID_SAMP_DIGIT_MIN = 1;
        /// <summary>
        /// 検体ID桁数　最大桁数設定
        /// </summary>
        public const Int32 RACK_ID_SAMP_DIGIT_MAX = 16;

        /// <summary>
        /// バーコード種類設定
        /// </summary>
        public enum BarcodeKind
        {
            /// <summary>
            /// 1)ITF, NW-7, CODE39, CODE128
            /// </summary>
            Type1,
            /// <summary>
            /// 2)ITF, 2of5, CODE39, CODE128
            /// </summary>
            Type2,
            /// <summary>
            /// 3)ITF, NW-7, 2of5, CODE128
            /// </summary>
            Type3,
            /// <summary>
            /// 4)ITF, NW-7, CODE39, 2of5
            /// </summary>
            Type4
        }

        /// <summary>
        /// 検体ID固定長有無　デフォルト設定
        /// </summary>
        public const Boolean RACK_ID_SAMP_FIX_DEFAULT = false;
        /// <summary>
        /// 検体ID桁数　デフォルト設定
        /// </summary>
        public const Int32 RACK_ID_SAMP_DIGIT_DEFAULT = 12;
        /// <summary>
        /// C/Dキャラクタ転送有無　デフォルト設定
        /// </summary>
        public const Boolean CD_CHAR_TRANS_DEFAULT = true;
        /// <summary>
        /// C/Dチェック有無　デフォルト設定
        /// </summary>
        public const Boolean CD_CHECK_DEFAULT = true;
        /// <summary>
        /// ST/SP転送有無　デフォルト設定
        /// </summary>
        public const Boolean STSP_TRANS_DEFAULT = false;
        /// <summary>
        /// バーコード種類　デフォルト設定
        /// </summary>
        public const BarcodeKind BARCODE_DEFAULT = BarcodeKind.Type1;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 検体IDが固定長であるか
        /// </summary>
        private Boolean usableRackIDSampFixedLength = RACK_ID_SAMP_FIX_DEFAULT;
        /// <summary>
        /// 検体ID桁数
        /// </summary>
        private Int32 rackIDSampDigit = RACK_ID_SAMP_DIGIT_DEFAULT;
        /// <summary>
        /// C/Dキャラクタ転送
        /// </summary>
        private Boolean usableCDCharTrans = CD_CHAR_TRANS_DEFAULT;
        /// <summary>
        /// C/Dチェック
        /// </summary>
        private Boolean usableCDCheck = CD_CHECK_DEFAULT;
        /// <summary>
        /// ST/SP転送
        /// </summary>
        private Boolean usableSTSPTrans = STSP_TRANS_DEFAULT;
        /// <summary>
        /// バーコード種類
        /// </summary>
        private BarcodeKind selectBCKind = BARCODE_DEFAULT;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 検体IDの固定長有無の設定／取得
        /// </summary>
        public Boolean UsableRackIDSampFixedLength
        {
            get
            {
                return this.usableRackIDSampFixedLength;
            }
            set
            {
                this.usableRackIDSampFixedLength = value;
            }
        }

        /// <summary>
        /// 検体ID桁数の設定／取得
        /// </summary>
        public Int32 RackIDSampDigit
        {
            get
            {
                return this.rackIDSampDigit;
            }
            set
            {
                this.rackIDSampDigit = value;
            }
        }

        /// <summary>
        /// C/Dキャラクタ転送の設定／取得
        /// </summary>
        public Boolean UsableCDCharTrans
        {
            get
            {
                return this.usableCDCharTrans;
            }
            set
            {
                this.usableCDCharTrans = value;
            }
        }

        /// <summary>
        /// C/Dチェックの設定／取得
        /// </summary>
        public Boolean UsableCDCheck
        {
            get
            {
                return this.usableCDCheck;
            }
            set
            {
                this.usableCDCheck = value;
            }
        }

        /// <summary>
        /// ST/SP転送の設定／取得
        /// </summary>
        public Boolean UsableSTSPTrans
        {
            get
            {
                return this.usableSTSPTrans;
            }
            set
            {
                this.usableSTSPTrans = value;
            }
        }

        /// <summary>
        /// バーコード種類の設定／取得
        /// </summary>
        public BarcodeKind SelectBCKind
        {
            get
            {
                return this.selectBCKind;
            }
            set
            {
                this.selectBCKind = value;
            }
        }

        #endregion
	}
	 
}
 
