using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Utility;
using Oelco.CarisX.DB;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Common
{
    /// <summary>
    /// リアルタイムデータ更新代理クラス
    /// </summary>
    /// <remarks>
    /// ユーザ操作以外でのデータ更新による再読込み・通知処理を行います。
    /// </remarks>tes
    static class RealtimeDataAgent
    {
        #region [publicメソッド]
        /// <summary>
        /// 検体情報取得
        /// </summary>
        /// <remarks>
        /// 検体情報取得します
        /// </remarks>
        static public void LoadSampleData()
        {
            Singleton<SpecimenGeneralDB>.Instance.LoadDB();
            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.RealtimeData, RealtimeDataKind.SampleRegist );
        }

        /// <summary>
        /// 検体情報取得
        /// </summary>
        /// <remarks>
        /// 検体情報取得します
        /// </remarks>
        static public void LoadStatData()
        {
            Singleton<SpecimenStatDB>.Instance.LoadDB();
            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.RealtimeData, RealtimeDataKind.StatRegist );
        }
        /// <summary>
        /// 分析中情報取得
        /// </summary>
        /// <remarks>
        /// 分析中情報取得します
        /// </remarks>
        static public void LoadAssayData()
        {
            Singleton<SpecimenAssayDB>.Instance.LoadDB();
            Singleton<CalibratorAssayDB>.Instance.LoadDB();
            Singleton<ControlAssayDB>.Instance.LoadDB();
            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.RealtimeData, RealtimeDataKind.AssayData );
        }

        /// <summary>
        /// 試薬テーブル取得
        /// </summary>
        /// <remarks>
        /// 試薬テーブル取得します
        /// </remarks>
        static public void LoadReagentRemainData()
        {
            Singleton<ReagentDB>.Instance.LoadDB();

            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.RealtimeData, RealtimeDataKind.ReagentData );
        }

        /// <summary>
        /// 再検査データ取得
        /// </summary>
        /// <remarks>
        /// 再検査データ取得します
        /// </remarks>
        static public void LoadReMeasureSampleData()
        {
            Singleton<SpecimenReMeasureDB>.Instance.LoadDB();
            Singleton<SpecimenStatReMeasureDB>.Instance.LoadDB();
            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.RealtimeData, RealtimeDataKind.SampleRetest );
        }

        /// <summary>
        /// 検体測定結果情報テーブル取得
        /// </summary>
        /// <remarks>
        /// 検体測定結果情報テーブル取得します
        /// </remarks>
        static public void LoadSpecimenResultData()
        {
            Singleton<SpecimenResultDB>.Instance.LoadDB();
            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.RealtimeData, RealtimeDataKind.SampleResult );
        }

        /// <summary>
        /// 精度管理精度検体測定結果情報テーブル取得
        /// </summary>
        /// <remarks>
        /// 精度管理精度検体測定結果情報テーブル取得します
        /// </remarks>
        static public void LoadControlResultData()
        {
            Singleton<ControlResultDB>.Instance.LoadDB();
            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.RealtimeData, RealtimeDataKind.ControlResult );
        }

        /// <summary>
        /// キャリブレータ測定結果情報取得
        /// </summary>
        /// <remarks>
        /// キャリブレータ測定結果情報を取得します
        /// </remarks>
        static public void LoadCalibratoResultData()
        {
            Singleton<CalibratorResultDB>.Instance.LoadDB();
            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.RealtimeData, RealtimeDataKind.CalibResult );
        }
        #endregion
        
    }
}
