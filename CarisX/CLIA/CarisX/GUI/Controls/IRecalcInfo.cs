using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.GUI.Controls
{
    /// <summary>
    /// 再計算条件インターフェース
    /// </summary>
    public interface IRecalcInfo
    {
        #region [プロパティ]

        /// <summary>
        /// 選択中の分析項目の取得、設定
        /// </summary>
        List<Int32> AnalyteSelect
        {
            get;
            set;
        }

        /// <summary>
        /// 選択中の試薬ロット番号の取得、設定
        /// </summary>
        String ReagentLotNoSelect
        {
            get;
            set;
        }

        /// <summary>
        /// 選択中の検量線の取得、設定
        /// </summary>
        DateTime CalibrationCurveApprovalDate
        {
            get;
            set;
        }
        #endregion
    }

    /// <summary>
    /// 再計算絞込み条件インターフェース
    /// </summary>
    public interface IRecalcRefiner
    {
        /// <summary>
        /// 選択中のラックIDの取得、設定
        /// </summary>
        Tuple<Boolean, CarisXIDString, CarisXIDString> RackIdSelect
        {
            get;
            set;
        }

        /// <summary>
        /// 選択中のシーケンス番号の取得、設定
        /// </summary>
        Tuple<Boolean, Int32, Int32> SequenceNoSelect
        {
            get;
            set;
        }


        /// <summary>
        /// 選択中の測定日付の取得、設定
        /// </summary>
        Tuple<Boolean, DateTime, DateTime> MeasuringTimeSelect
        {
            get;
            set;
        }

        /// <summary>
        /// 選択中のリマークの取得、設定
        /// </summary>
        Remark.RemarkCategory RemarkSelect
        {
            get;
            set;
        }
    }
    
    /// <summary>
    /// 検体測定データ画面再計算用インターフェース
    /// </summary>
    public interface IRecalcInfoSpecimenResult : IRecalcInfo, IRecalcRefiner
    {
        #region [プロパティ]

        /// <summary>
        /// 選択中の検体IDの取得、設定
        /// </summary>
        Tuple<Boolean, String> PatientIdSelect
        {
            get;
            set;
        }

        /// <summary>
        /// 選択中の検体種別の取得、設定
        /// </summary>
        Tuple<Boolean, SpecimenMaterialType> SpecimenMaterialTypeSelect
        {
            get;
            set;
        }

        #endregion
    }

    /// <summary>
    /// 精度管理検体測定データ画面再計算用インターフェース
    /// </summary>
    public interface IRecalcInfoControlResult : IRecalcInfo, IRecalcRefiner
    {
        #region [プロパティ]

        /// <summary>
        /// 選択中の精度管理検体ロットの取得、設定
        /// </summary>
        Tuple<Boolean, String> ControlLotNoSelect
        {
            get;
            set;
        }

        /// <summary>
        /// 選択中の精度管理検体名の取得、設定
        /// </summary>
        Tuple<Boolean, String> ControlNameSelect
        {
            get;
            set;
        } 
        #endregion
    }
}
