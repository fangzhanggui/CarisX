using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Utility;

namespace Oelco.CarisX.Utility
{
    // 履歴種別クラス。文字列化も要りそう？（もしファイルに落とすのであれば）
    // Caris,AFTでそれぞれ一つずつこのクラスと同様のものを実装する。
    public enum CarisXHistoryKind
    {
        /// <summary>
        /// 分析ステータス画面表示
        /// </summary>
        ShowAssay,
        /// <summary>
        /// キャリブレータ解析画面表示
        /// </summary>
        ShowCalibAnalysis,
        /// <summary>
        /// キャリブレータ登録画面表示
        /// </summary>
        ShowCalibRegistration,
        /// <summary>
        /// キャリブレータ測定データ画面表示
        /// </summary>
        ShowCalibResult,
        /// <summary>
        /// キャリブレータステータス画面表示
        /// </summary>
        ShowCalibStatus,
        /// <summary>
        /// 精度管理画面表示
        /// </summary>
        ShowControlQC,
        /// <summary>
        /// 精度管理検体登録画面表示
        /// </summary>
        ShowControlRegistration,
        /// <summary>
        /// 精度管理検体測定履歴データ画面表示
        /// </summary>
        ShowControlResult,
        /// <summary>
        /// 分析項目パラメータ画面表示
        /// </summary>
        ShowProtocolSetting,
        /// <summary>
        /// 試薬準備画面表示
        /// </summary>
        ShowSetReagent,
        /// <summary>
        /// 一般検体登録画面表示
        /// </summary>
        ShowSpecimenRegistration,
        /// <summary>
        /// STAT検体登録画面表示
        /// </summary>
        ShowSpecimenStatRegistration,
        /// <summary>
        /// 検体測定データ画面表示
        /// </summary>
        ShowSpecimenResult,
        /// <summary>
        /// 検体再検査画面表示
        /// </summary>
        ShowSpecimenRetest,
        /// <summary>
        /// システム分析項目パラメータ選択画面表示
        /// </summary>
        ShowSystemAnalytes,
        /// <summary>
        /// システム構成画面表示
        /// </summary>
        ShowSystemConfigration,
        /// <summary>
        /// システム履歴画面表示
        /// </summary>
        ShowSystemLog,
        /// <summary>
        /// システムオプション画面表示
        /// </summary>
        ShowSystemOption,
        /// <summary>
        /// システムユーザ管理画面表示
        /// </summary>
        ShowSystemUserControl,
        /// <summary>
        /// システムオプション画面(モジュール毎)表示
        /// </summary>
        ShowSystemModuleOption
    }
    /// <summary>
    /// 記録動作種別
    /// </summary>
    class CarisXHistoryActionKind : HistoryActionKind
    {
        /// <summary>
        /// 種別
        /// </summary>
        private CarisXHistoryKind kind;
        /// <summary>
        /// 記録動作種別辞書
        /// </summary>
        private readonly static Dictionary<CarisXHistoryKind, CarisXHistoryActionKind> thisDic = new Dictionary<CarisXHistoryKind, CarisXHistoryActionKind>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static CarisXHistoryActionKind()
        {
            var kinds = Enum.GetValues( typeof( CarisXHistoryKind ) );
            foreach ( var knd in kinds )
            {
                thisDic.Add( (CarisXHistoryKind)knd, new CarisXHistoryActionKind( (CarisXHistoryKind)knd ) );
            }
        }

        /// <summary>
        /// 種別取得
        /// </summary>
        /// <remarks>
        /// 種別取得します
        /// </remarks>
        /// <param name="knd"></param>
        /// <returns></returns>
        static public CarisXHistoryActionKind GetKind( CarisXHistoryKind knd )
        {
            return thisDic[knd];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="kind"></param>
        protected CarisXHistoryActionKind( CarisXHistoryKind kind )
        {
            this.kind = kind;
        }
        /// <summary>
        /// 動作種別の取得
        /// </summary>
        public CarisXHistoryKind ActionKind
        {
            get
            {
                return kind;
            }
            //set
            //{
            //    kind = value;
            //}
        }

    }
}
