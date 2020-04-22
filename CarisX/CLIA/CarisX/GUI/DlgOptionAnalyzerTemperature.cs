using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Const;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// アナライザ温度ダイアログクラス
    /// </summary>
    public partial class DlgOptionAnalyzerTemperature : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgOptionAnalyzerTemperature()
        {
            InitializeComponent();
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// リソースの初期化
        /// </summary>
        /// <remarks>
        /// リソースを初期化します
        /// </remarks>
        protected override void initializeResource()
        {
        }

        /// <summary>
        /// コンポーネントの初期化
        /// </summary>
        /// <remarks>
        /// コンポーネントを初期化します
        /// </remarks>
        protected override void initializeFormComponent()
        {
            this.lblReactionTableValue.Text = null;
            this.lblBFTableValue.Text = null;
            this.lblBF1Value.Text = null;
            this.lblBF2Value.Text = null;
            this.lblR1Value.Text = null;
            this.lblR2Value.Text = null;
            this.lblChemilumiValue.Text = null;

            this.lblReagentStorageValue.Text = null;
            this.lblRoomValue.Text = null;
            this.lblDeviceValue.Text = null;

            // 温度表示を更新
            this.updateTempDisplay();
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            // パネル既定ボタン
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_002;

            // ダイアログタイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_000;

            // 反応テーブル温度
            this.lblReactionTable.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_001;
            this.lblReactionTableUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_002;
            // BFテーブル温度
            this.lblBFTable.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_003;
            this.lblBFTableUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_004;
            // B/F1温度
            this.lblBF1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_005;
            this.lblBF1Unit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_006;
            // B/F2温度
            this.lblBF2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_007;
            this.lblBF2Unit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_008;
            // R1温度
            this.lblR1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_009;
            this.lblR1Unit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_010;
            // R2温度
            this.lblR2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_011;
            this.lblR2Unit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_012;
            // 化学発光測光部温度
            this.lblChemilumi.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_013;
            this.lblChemilumiUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_014;

            // 試薬保冷庫温度
            this.lblReagentStorage.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_015;
            this.lblReagentStorageUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_016;
            // 室温
            this.lblRoom.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_017;
            this.lblRoomUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_018;
            // 装置内温度
            this.lblDevice.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_019;
            this.lblDeviceUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_020;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// Cancelボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        protected virtual void btnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// FormClosingイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DlgOptionAnalyzerTemperature_FormClosing( object sender, FormClosingEventArgs e )
        {
            // 画面が閉じられる際、温度問合せタイマ動作停止を要求する
            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.SetAskTemperatureTimer, false );

            // 画面が閉じられる際、温度更新通知対象からこの画面を外す。
            Singleton<NotifyManager>.Instance.RemoveNotifyTarget( (Int32)NotifyKind.UpdateTemperature, this.onTemperatureUpdate );
        }

        /// <summary>
        /// 画面表示完了イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DlgOptionAnalyzerTemperature_Shown( object sender, EventArgs e )
        {
            // 画面が表示される際、温度更新通知対象に登録する。
            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.UpdateTemperature, this.onTemperatureUpdate );

            // 画面が表示される際、温度問合せタイマ動作開始を要求する
            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.SetAskTemperatureTimer, true );

        }

        /// <summary>
        /// 温度表示更新イベントハンドラ
        /// </summary>
        /// <param name="value">不使用</param>
        protected void onTemperatureUpdate( Object value )
        {
            // 温度表示を更新
            this.updateTempDisplay();
        }

        /// <summary>
        /// 温度表示更新
        /// </summary>
        /// <remarks>
        /// 画面表示内容をシステムパラメータの温度データから取得し更新を行います。
        /// </remarks>
        protected virtual void updateTempDisplay()
        {
            // 取得したデータを反映
            Temperature moduleTemp = Singleton<PublicMemory>.Instance.moduleTemperature[(Int32)Singleton<PublicMemory>.Instance.moduleIndex];

            this.lblReactionTableValue.Text = String.Format("{0:0.0}", moduleTemp.TempReactionTable);
            this.lblBFTableValue.Text = String.Format("{0:0.0}", moduleTemp.TempBFTable);
            this.lblBF1Value.Text = String.Format("{0:0.0}", moduleTemp.TempBF1PreHeat);
            this.lblBF2Value.Text = String.Format("{0:0.0}", moduleTemp.TempBF2PreHeat);
            this.lblR1Value.Text = String.Format("{0:0.0}", moduleTemp.TempR1ProbePreHeat);
            this.lblR2Value.Text = String.Format("{0:0.0}", moduleTemp.TempR2ProbePreHeat);
            this.lblChemilumiValue.Text = String.Format("{0:0.0}", moduleTemp.TempChemiLightMeas);

            this.lblReagentStorageValue.Text = String.Format("{0:0.0}", moduleTemp.TempReagentCoolingBox);
            this.lblRoomValue.Text = String.Format("{0:0.0}", moduleTemp.TempRoom);
            this.lblDeviceValue.Text = String.Format("{0:0.0}", moduleTemp.TempDevice);
        }

        #endregion
    }

}
