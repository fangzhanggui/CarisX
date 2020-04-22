using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 洗浄、分注設定
    /// </summary>
	public class WashDispVolParameter : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// 洗浄液　最小値設定（μL）
        /// </summary>
        public const Int32 DISP_VOLUME_WASH_MIN = 0;
        /// <summary>
        /// 洗浄液　最大値設定（μL）
        /// </summary>
        public const Int32 DISP_VOLUME_WASH_MAX = 999;
        /// <summary>
        /// プレトリガ　最小値設定（μL）
        /// </summary>
        public const Int32 DISP_VOLUME_PRE_TRIG_MIN = 0;
        /// <summary>
        /// プレトリガ　最大値設定（μL）
        /// </summary>
        public const Int32 DISP_VOLUME_PRE_TRIG_MAX = 999;
        /// <summary>
        /// トリガ　最小値設定（μL）
        /// </summary>
        public const Int32 DISP_VOLUME_TRIG_MIN = 0;
        /// <summary>
        /// トリガ　最大値設定（μL）
        /// </summary>
        public const Int32 DISP_VOLUME_TRIG_MAX = 999;

        /// <summary>
        /// 洗浄液　デフォルト値設定（μL）
        /// </summary>
        public const Int32 DISP_VOLUME_WASH_DEFAULT = 700;
        /// <summary>
        /// プレトリガ　デフォルト値設定（μL）
        /// </summary>
        public const Int32 DISP_VOLUME_PRE_TRIG_DEFAULT = DISP_VOLUME_PRE_TRIG_MIN;
        /// <summary>
        /// トリガ　デフォルト値設定（μL）
        /// </summary>
        public const Int32 DISP_VOLUME_TRIG_DEFAULT = DISP_VOLUME_TRIG_MIN;


        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 洗浄液分注量（μL）
        /// </summary>
        private Int32 dispVolWash = DISP_VOLUME_WASH_DEFAULT;
        /// <summary>
        /// プレトリガ分注量（μL）
        /// </summary>
        private Int32 dispVolPreTrig = DISP_VOLUME_PRE_TRIG_DEFAULT;
        /// <summary>
        /// トリガ分注量（μL）
        /// </summary>
        private Int32 dispVolTrig = DISP_VOLUME_TRIG_DEFAULT;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 洗浄液分注量の設定／取得
        /// </summary>
        public Int32 DispVolWash
        {
            get
            {
                return this.dispVolWash;
            }
            set
            {
                this.dispVolWash = value;
            }
        }
        /// <summary>
        /// プレトリガ分注量の設定／取得
        /// </summary>
        public Int32 DispVolPreTrig
        {
            get
            {
                return this.dispVolPreTrig;
            }
            set
            {
                this.dispVolPreTrig = value;
            }
        }
        /// <summary>
        /// トリガ分注量の設定／取得
        /// </summary>
        public Int32 DispVolTrig
        {
            get
            {
                return this.dispVolTrig;
            }
            set
            {
                this.dispVolTrig = value;
            }
        }

        #endregion
	}
	 
}
 
