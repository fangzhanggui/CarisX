using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;

namespace Oelco.CarisX.GUI.Controls
{
    /// <summary>
    /// 絞込み条件インターフェース
    /// </summary>
    public interface ISearchInfo
    {
        #region [プロパティ]

        /// <summary>
        /// 選択中の濃度(範囲指定/開始値、終了値)の取得、設定
        /// </summary>
        Tuple<Boolean, Double, Double> ConcentrationSelect
        {
            get;
            set;
        }

        /// <summary>
        /// 選択中のラックID(範囲指定/開始ID、終了ID)の取得、設定
        /// </summary>
        Tuple<Boolean, CarisXIDString, CarisXIDString> RackIdSelect
        {
            get;
            set;
        }

        /// <summary>
        /// 選択中のシーケンス番号(範囲指定/開始番号、終了番号)の取得、設定
        /// </summary>
        Tuple<Boolean, Int32, Int32> SequenceNoSelect
        {
            get;
            set;
        }

        /// <summary>
        /// 選択中の測定日付(範囲指定/開始日、終了日)の取得、設定
        /// </summary>
        Tuple<Boolean, DateTime, DateTime> MeasuringTimeSelect
        {
            get;
            set;
        }

        /// <summary>
        /// 選択中の分析項目の取得、設定
        /// </summary>
        List<String> AnalyteSelect
        {
            get;
        }

        /// <summary>
        /// 選択中のリマークの取得、設定
        /// </summary>
        Remark.RemarkCategory RemarkSelect
        {
            get;
            set;
        }

        /// <summary>
        /// 選択中のモジュールの取得、設定
        /// </summary>
        ModuleCategory ModuleSelect
        {
            get;
            set;
        }

        #endregion
    }

    /// <summary>
    /// 検体測定データ画面絞込み用インターフェース
    /// </summary>
    public interface ISearchInfoSpecimenResult : ISearchInfo
    {
        #region [プロパティ]

        /// <summary>
        /// 選択中の判定種別の取得、設定
        /// </summary>
        Tuple<Boolean, JudgementType> JudgementSelect
        {
            get;
            set;
        }

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

        /// <summary>
        /// 選択中のコメントの取得、設定
        /// </summary>
        Tuple<Boolean, String> CommentSelect
        {
            get;
            set;
        }

        #endregion
    }

    /// <summary>
    /// キャリブレータ測定データ画面絞込み用インターフェース
    /// </summary>
    public interface ISearchInfoCalibratorResult : ISearchInfo
    {
        #region [プロパティ]

        /// <summary>
        /// 選択中のキャリブレータロットの取得、設定
        /// </summary>
        Tuple<Boolean, String> CalibratorLotSelect
        {
            get;
            set;
        }

        #endregion
    }

    /// <summary>
    /// 精度管理検体測定データ画面絞り込み用インターフェース
    /// </summary>
    public interface ISearchInfoControlResult : ISearchInfo
    {
        #region [プロパティ]

        /// <summary>
        /// 選択中の精度管理検体名の取得、設定
        /// </summary>
        Tuple<Boolean, String> ControlNameSelect
        {
            get;
            set;
        }

        /// <summary>
        /// 選択中の精度管理検体ロットの取得、設定
        /// </summary>
        Tuple<Boolean, String> ControlLotSelect
        {
            get;
            set;
        }

        /// <summary>
        /// 選択中のコメントの取得、設定
        /// </summary>
        Tuple<Boolean, String> CommentSelect
        {
            get;
            set;
        }

        #endregion
    }

    /// <summary>
    /// 履歴絞り込み条件インターフェース
    /// </summary>
    public interface ISearchLogInfo
    {
        #region [プロパティ]

        /// <summary>
        /// 書き込み時刻の取得、設定
        /// </summary>
        Tuple<Boolean, DateTime, DateTime> WriteTimeSelect
        {
            get;
            set;
        }

        /// <summary>
        /// ユーザーIDの取得、設定
        /// </summary>
        Tuple<Boolean, String> UserIDSelect
        {
            get;
            set;
        }

        #endregion
    }

    /// <summary>
    /// エラー履歴絞り込み条件インターフェース
    /// </summary>
    public interface ISearchLogInfoErrorLog : ISearchLogInfo
    {
        #region [プロパティ]

        /// <summary>
        /// エラーコードの取得、設定
        /// </summary>
        Tuple<Boolean, String> ErrorCodeSelect
        {
            get;
            set;
        }

        /// <summary>
        /// エラー引数の取得、設定
        /// </summary>
        Tuple<Boolean, String> ErrorArgSelect
        {
            get;
            set;
        }

        /// <summary>
        /// エラーレベルの取得、設定
        /// </summary>
        Tuple<Boolean, String> ErrorLevelSelect
        {
            get;
            set;
        }

        /// <summary>
        /// エラーのコメントの取得、設定
        /// </summary>
        Tuple<Boolean, String> ErrorContentSelect
        {
            get;
            set;
        }

        /// <summary>
        /// 選択モジュールの取得、設定
        /// </summary>
        ErrorFilteringCategory ModuleSelect
        {
            get;
            set;
        }

        /// <summary>
        /// エラーの発生回数の取得、設定
        /// </summary>
        Tuple<Boolean, Int32, Int32> SumSelect
        {
            get;
            set;
        }

        #endregion
    }

}