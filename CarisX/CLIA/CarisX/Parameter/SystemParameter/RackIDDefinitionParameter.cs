using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// ラックID割り当て
    /// </summary>
	public class RackIDDefinitionParameter : AttachmentParameter
	{
        #region [定数定義]

        /// <summary>
        /// 検体ラック[ラックID分析]　最小値設定
        /// </summary>
        public const Int32 RACK_ID_SAMP_MIN = 1;
        /// <summary>
        /// 検体ラック[ラックID分析]　最大値設定
        /// </summary>
        public const Int32 RACK_ID_SAMP_MAX = 3999;
        /// <summary>
        /// 検体ラック[検体ID分析]　最小値設定
        /// </summary>
        public const Int32 RACK_ID_SAMP_MODE_SAMPLE_MIN = 1;
        /// <summary>
        /// 検体ラック[検体ID分析]　最大値設定
        /// </summary>
        public const Int32 RACK_ID_SAMP_MODE_SAMPLE_MAX = 3999;
        /// <summary>
        /// 精度管理検体ラック　最小値設定
        /// </summary>
        public const Int32 RACK_ID_CTRL_MIN = 1;
        /// <summary>
        /// 精度管理検体ラック　最大値設定
        /// </summary>
        public const Int32 RACK_ID_CTRL_MAX = 999;
        /// <summary>
        /// キャリブレータラック　最小値設定
        /// </summary>
        public const Int32 RACK_ID_CALIB_MIN = 1;
        /// <summary>
        /// キャリブレータラック　最大値設定
        /// </summary>
        public const Int32 RACK_ID_CALIB_MAX = 999;

        /// <summary>
        /// 検体ラック[ラックID分析]（最小）　デフォルト値設定
        /// </summary>
        public const Int32 RACK_ID_SAMP_LOWER_DEFAULT = RACK_ID_SAMP_MIN;
        /// <summary>
        /// 検体ラック[ラックID分析]（最大）　デフォルト値設定
        /// </summary>
        public const Int32 RACK_ID_SAMP_UPPER_DEFAULT = 100;
        /// <summary>
        /// 検体ラック[検体ID分析]（最小）　デフォルト値設定
        /// </summary>
        public const Int32 RACK_ID_SAMP_LOWER_MODE_SAMPLE_DEFAULT = RACK_ID_SAMP_MODE_SAMPLE_MIN;
        /// <summary>
        /// 検体ラック[検体ID分析]（最大）　デフォルト値設定
        /// </summary>
        public const Int32 RACK_ID_SAMP_UPPER_MODE_SAMPLE_DEFAULT = 100;
        /// <summary>
        /// 精度管理検体ラック（最小）　デフォルト値設定
        /// </summary>
        public const Int32 RACK_ID_CTRL_LOWER_DEFAULT = RACK_ID_CTRL_MIN;
        /// <summary>
        /// 精度管理検体ラック（最大）　デフォルト値設定
        /// </summary>
        public const Int32 RACK_ID_CTRL_UPPER_DEFAULT = 50;
        /// <summary>
        /// キャリブレータラック（最小）　デフォルト値設定
        /// </summary>
        public const Int32 RACK_ID_CALIB_LOWER_DEFAULT = RACK_ID_CALIB_MIN;
        /// <summary>
        /// キャリブレータラック（最大）　デフォルト値設定
        /// </summary>
        public const Int32 RACK_ID_CALIB_UPPER_DEFAULT = 50;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 検体ラック[ラックID分析]　最小値
        /// </summary>
        private Int32 minRackIDSamp = RACK_ID_SAMP_LOWER_DEFAULT;
        /// <summary>
        /// 検体ラック[ラックID分析]　最大値
        /// </summary>
        private Int32 maxRackIDSamp = RACK_ID_SAMP_UPPER_DEFAULT;
        /// <summary>
        /// 検体ラック[検体ID分析]　最小値
        /// </summary>
        private Int32 minRackIDSampModeSample = RACK_ID_SAMP_LOWER_MODE_SAMPLE_DEFAULT;
        /// <summary>
        /// 検体ラック[検体ID分析]　最大値
        /// </summary>
        private Int32 maxRackIDSampModeSample = RACK_ID_SAMP_UPPER_MODE_SAMPLE_DEFAULT;
        /// <summary>
        /// 精度管理検体ラック　最小値
        /// </summary>
        private Int32 minRackIDCtrl = RACK_ID_CTRL_LOWER_DEFAULT;
        /// <summary>
        /// 精度管理検体ラック 　最大値
        /// </summary>
        private Int32 maxRackIDCtrl = RACK_ID_CTRL_UPPER_DEFAULT;
        /// <summary>
        /// キャリブレータラック　最小値
        /// </summary>
        private Int32 minRackIDCalib = RACK_ID_CALIB_LOWER_DEFAULT;
        /// <summary>
        /// キャリブレータラック　最大値
        /// </summary>
        private Int32 maxRackIDCalib = RACK_ID_CALIB_UPPER_DEFAULT;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 検体ラック[ラックID分析]（最小値）の設定／取得Sample rack [rack ID analysis] setting (minimum value) / acquisition
        /// </summary>
        public Int32 MinRackIDSamp
        {
            get
            {
                return this.minRackIDSamp;
            }
            set
            {
                this.minRackIDSamp = value;
            }
        }

        /// <summary>
        /// 検体ラック[ラックID分析]（最大値）の設定／取得Sample rack [rack ID analysis]
        /// </summary>
        public Int32 MaxRackIDSamp
        {
            get
            {
                return this.maxRackIDSamp;
            }
            set
            {
                this.maxRackIDSamp = value;
            }
        }

        /// <summary>
        /// 検体ラック[検体ID分析]（最小値）の設定／取得Sample rack [specimen ID analysis] setting (minimum value) / acquisition
        /// </summary>
        public Int32 MinRackIDSampModeSample
        {
            get
            {
                return this.minRackIDSampModeSample;
            }
            set
            {
                this.minRackIDSampModeSample = value;
            }
        }

        /// <summary>
        /// 検体ラック[検体ID分析]（最大値）の設定／取得
        /// </summary>
        public Int32 MaxRackIDSampModeSample
        {
            get
            {
                return this.maxRackIDSampModeSample;
            }
            set
            {
                this.maxRackIDSampModeSample = value;
            }
        }
        /// <summary>
        /// 精度管理検体ラック（最小値）の設定／取得Setting of quality control sample rack (minimum value) / acquisition
        /// </summary>
        public Int32 MinRackIDCtrl
        {
            get
            {
                return this.minRackIDCtrl;
            }
            set
            {
                this.minRackIDCtrl = value;
            }
        }

        /// <summary>
        /// 精度管理検体ラック（最大値）の設定／取得
        /// </summary>
        public Int32 MaxRackIDCtrl
        {
            get
            {
                return this.maxRackIDCtrl;
            }
            set
            {
                this.maxRackIDCtrl = value;
            }
        }

        /// <summary>
        /// キャリブレータラック（最小値）の設定／取得
        /// </summary>
        public Int32 MinRackIDCalib
        {
            get
            {
                return this.minRackIDCalib;
            }
            set
            {
                this.minRackIDCalib = value;
            }
        }

        /// <summary>
        /// キャリブレータラック（最大値）の設定／取得
        /// </summary>
        public Int32 MaxRackIDCalib
        {
            get
            {
                return this.maxRackIDCalib;
            }
            set
            {
                this.maxRackIDCalib = value;
            }
        }

        #endregion
	}
	 
}
 
