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
using Oelco.CarisX.Status;
using Oelco.Common.Comm;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// アナライザ加温待ちダイアログクラス
    /// </summary>
    public partial class DlgWaitAnalyzerTemperature : DlgCarisXBase
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 更新停止フラグ
        /// </summary>
        Boolean freezeUpdate = false;

        /// <summary>
        /// ステータス有効フラグ
        /// </summary>
        private Boolean isEnableState = false;

        /// <summary>
        /// ラベル色テーブル
        /// </summary>
        private readonly Dictionary<Boolean, Color> colorTable = new Dictionary<Boolean, Color>()
        {
            {true,Color.Red},       // 範囲外判定true時の色
            {false,Color.Black}     // 範囲外判定false時の色
        };

        /// <summary>
        /// 温度ステータステーブル
        /// </summary>
        private readonly List<Dictionary<Control, Boolean>> tempStateTables = new List<Dictionary<Control, Boolean>>();

        /// <summary>
        /// 画面表示対象のモジュール
        /// </summary>
        private Int32 displayModuleIndex = (Int32)ModuleIndex.Module1;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgWaitAnalyzerTemperature()
        {
            InitializeComponent();

            // 画面が生成される際、温度更新通知対象に登録する。
            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.UpdateTemperature, this.onTemperatureUpdate );
            // 画面が生成される際、温度問合せタイマ動作開始を要求する
            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.SetAskTemperatureTimer, true );

            // 温度範囲状態テーブル初期化
            for (int i = 0; i < Enum.GetValues(typeof(ModuleIndex)).Length; i++)
            {
                tempStateTables.Add(new Dictionary<Control, bool>());
                tempStateTables[i][this.lblReactionTableValue] = true;
                tempStateTables[i][this.lblBFTableValue] = true;
                tempStateTables[i][this.lblBF1Value] = true;
                tempStateTables[i][this.lblBF2Value] = true;
                tempStateTables[i][this.lblR1Value] = true;
                tempStateTables[i][this.lblR2Value] = true;
                tempStateTables[i][this.lblChemilumiValue] = true;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 表示必要性
        /// </summary>
        /// <remarks>
        /// 現在このダイアログを表示する必要があるかどうかを取得します。
        /// </remarks>
        /// <returns>true:要 false:不要</returns>
        public Boolean IsNeedShow()
        {           

            // スレーブへの温度問合せが完了するまで待機を行う。
            // この関数を抜けず待機を行う。
            // （メインスレッドを止めてしまうと通信関連イベントが動作しない為、DoEventsを呼び出す）
            Int32 waitCount = 0;
            Int32 sleepTime = 10;
            while ( !this.isEnableState )
            {
                System.Threading.Thread.Sleep( sleepTime );
                Application.DoEvents();

                waitCount += sleepTime;
                if ( waitCount >= CarisXConst.SEQUENCE_WAIT_TIME )
                {
                    // タイムアウト（エラー表示はシーケンススレッドから通知される）
                    break;
                }
            }

            // 温度範囲外(true)がある場合、要表示
            Boolean needShow = this.tempStateTables.Any(v => v.Values.Any(x => x));
            if (needShow && !IsVisible)
            {
                //表示する場合、表示対象のインデックスを変更する
                displayModuleIndex = this.tempStateTables.FindIndex(v => v.Values.Any(x => x));
                //表示対象モジュールのボタンを選択状態にする
                btnModule1.CurrentState = (displayModuleIndex == (Int32)ModuleIndex.Module1);
                btnModule2.CurrentState = (displayModuleIndex == (Int32)ModuleIndex.Module2);
                btnModule3.CurrentState = (displayModuleIndex == (Int32)ModuleIndex.Module3);
                btnModule4.CurrentState = (displayModuleIndex == (Int32)ModuleIndex.Module4);
            }
            return needShow;
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

            //接続台数によって画面の表示・非表示を変更
            //モジュール接続台数が１の場合はボタンはすべて表示しない。２以上の場合は対応するボタンを表示する
            btnModule1.Visible = (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected >= (Int32)RackModuleIndex.Module2);
            btnModule2.Visible = (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected >= (Int32)RackModuleIndex.Module2);
            btnModule3.Visible = (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected >= (Int32)RackModuleIndex.Module3);
            btnModule4.Visible = (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected >= (Int32)RackModuleIndex.Module4);

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
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;

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

            // モジュール１～４
            this.btnModule1.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_021;
            this.btnModule2.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_022;
            this.btnModule3.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_023;
            this.btnModule4.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONANALYZERTEMPERATURE_024;
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
            // キャンセル確認を行ってダイアログを閉じる。
            // 確認ダイアログ表示中は温度更新をストップする。
            this.freezeUpdate = true;
            var cancelResult = DlgMessage.Show( CarisX.Properties.Resources.STRING_DLG_MSG_213, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel );
            if ( cancelResult == System.Windows.Forms.DialogResult.OK )
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            this.freezeUpdate = false;
        }

        /// <summary>
        /// 温度表示更新イベントハンドラ
        /// </summary>
        /// <param name="value">不使用</param>
        protected void onTemperatureUpdate( Object value )
        {
            // 温度範囲状態更新
            this.updateStateTable();

            // 温度表示を更新
            this.updateTempDisplay();
        }

        /// <summary>
        /// 温度表示更新
        /// </summary>
        /// <remarks>
        /// 画面表示内容をシステムパラメータの温度データから取得し更新を行います。
        /// </remarks>
        protected void updateTempDisplay()
        {
            if ( !this.freezeUpdate )
            {
                // 表示更新

                // 取得したデータを反映
                //適正値の設定がゼロの項目は画面表示を「---」にする
                var tempData = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter;
                var moduleTemp = Singleton<PublicMemory>.Instance.moduleTemperature[displayModuleIndex];

                this.lblReactionTableValue.Text = getTempValue(tempData.TempReactionTable, moduleTemp.TempReactionTable);
                this.lblBFTableValue.Text = getTempValue(tempData.TempBFTable, moduleTemp.TempBFTable);
                this.lblBF1Value.Text = getTempValue(tempData.TempBF1PreHeat, moduleTemp.TempBF1PreHeat);
                this.lblBF2Value.Text = getTempValue(tempData.TempBF2PreHeat, moduleTemp.TempBF2PreHeat);
                this.lblR1Value.Text = getTempValue(tempData.TempR1ProbePreHeat, moduleTemp.TempR1ProbePreHeat);
                this.lblR2Value.Text = getTempValue(tempData.TempR2ProbePreHeat, moduleTemp.TempR2ProbePreHeat);
                this.lblChemilumiValue.Text = getTempValue(tempData.TempChemiLightMeas, moduleTemp.TempChemiLightMeas);

                this.lblReagentStorageValue.Text = String.Format("{0:0.0}", moduleTemp.TempReagentCoolingBox);
                this.lblRoomValue.Text = String.Format("{0:0.0}", moduleTemp.TempRoom);
                this.lblDeviceValue.Text = String.Format("{0:0.0}", moduleTemp.TempDevice);

                // 範囲外温度検出字、文字を赤表示する
                // ( 設定値-範囲値 ) が現在値を上回る場合、範囲外となる。
                this.lblReactionTableValue.Appearance.ForeColor = this.colorTable[this.tempStateTables[displayModuleIndex][this.lblReactionTableValue]];
                this.lblBFTableValue.Appearance.ForeColor = this.colorTable[this.tempStateTables[displayModuleIndex][this.lblBFTableValue]];
                this.lblBF1Value.Appearance.ForeColor = this.colorTable[this.tempStateTables[displayModuleIndex][this.lblBF1Value]];
                this.lblBF2Value.Appearance.ForeColor = this.colorTable[this.tempStateTables[displayModuleIndex][this.lblBF2Value]];
                this.lblR1Value.Appearance.ForeColor = this.colorTable[this.tempStateTables[displayModuleIndex][this.lblR1Value]];
                this.lblR2Value.Appearance.ForeColor = this.colorTable[this.tempStateTables[displayModuleIndex][this.lblR2Value]];
                this.lblChemilumiValue.Appearance.ForeColor = this.colorTable[this.tempStateTables[displayModuleIndex][this.lblChemilumiValue]];

                //状況によってコントロールの活性・非活性、ボタンの文字色を変更
                foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
                {
                    var moduleForeColor = this.colorTable[this.tempStateTables[moduleindex].Any(v => v.Value == true)];

                    switch (moduleindex)
                    {
                        case (Int32)ModuleIndex.Module1:
                            this.btnModule1.NormalAppearance.ForeColor = moduleForeColor;
                            this.btnModule1.ToggleAppearance.ForeColor = moduleForeColor;
                            this.btnModule1.Appearance.ForeColor = moduleForeColor;
                            // this.btnModule1.Enabled = (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                            this.btnModule1.Enabled = ((Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                                                      && (Singleton<Status.SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] != Status.SystemStatusKind.MotorError));
                            break;
                        case (Int32)ModuleIndex.Module2:
                            this.btnModule2.NormalAppearance.ForeColor = moduleForeColor;
                            this.btnModule2.ToggleAppearance.ForeColor = moduleForeColor;
                            this.btnModule2.Appearance.ForeColor = moduleForeColor;
                            // this.btnModule2.Enabled = (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                            this.btnModule2.Enabled = ((Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                                                      && (Singleton<Status.SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] != Status.SystemStatusKind.MotorError));
                            break;
                        case (Int32)ModuleIndex.Module3:
                            this.btnModule3.NormalAppearance.ForeColor = moduleForeColor;
                            this.btnModule3.ToggleAppearance.ForeColor = moduleForeColor;
                            this.btnModule3.Appearance.ForeColor = moduleForeColor;
                            // this.btnModule3.Enabled = (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                            this.btnModule3.Enabled = ((Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                                                          && (Singleton<Status.SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] != Status.SystemStatusKind.MotorError));
                            break;
                        case (Int32)ModuleIndex.Module4:
                            this.btnModule4.NormalAppearance.ForeColor = moduleForeColor;
                            this.btnModule4.ToggleAppearance.ForeColor = moduleForeColor;
                            this.btnModule4.Appearance.ForeColor = moduleForeColor;
                            // this.btnModule4.Enabled = (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                            this.btnModule4.Enabled = ((Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                                                          && (Singleton<Status.SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] != Status.SystemStatusKind.MotorError));
                            break;
                    }
                }

                // 表示前に動作した場合、閉じる動作は行わない。
                if ( this.IsVisible )
                {
                    // 範囲外を全項目に対して含まない場合、自動的に画面を閉じる
                    if ( !this.IsNeedShow() )
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Close();
                    }
                }

                // この更新処理を一度でも通るとステータスの判定が可能になる。
                this.isEnableState = true;
            }
        }

        /// <summary>
        /// 温度値を取得する
        /// </summary>
        /// <param name="paramTemp">システムパラメータに定義されている温度</param>
        /// <param name="moduleTemp">モジュールから取得した温度</param>
        /// <returns>表示する温度値</returns>
        /// <remarks>
        /// システムパラメータに定義されている温度が0の場合、「---」と表示する
        /// </remarks>
        private String getTempValue(Double paramTemp, Double moduleTemp)
        {
            return paramTemp == 0 ? Properties.Resources.STRING_DLG_WAITANALYZERTEMPERATURE_000 : String.Format("{0:0.0}", moduleTemp);
        }

        /// <summary>
        /// インスタンス破棄
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(Boolean disposing)
        {
            // 画面が閉じられる際、温度問合せタイマ動作停止を要求する
            Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SetAskTemperatureTimer, false);

            // 画面が閉じられる際、温度更新通知対象からこの画面を外す。
            Singleton<NotifyManager>.Instance.RemoveNotifyTarget((Int32)NotifyKind.UpdateTemperature, this.onTemperatureUpdate);

            base.Dispose(disposing);
        }

        /// <summary>
        /// ステータス更新
        /// </summary>
        /// <remarks>
        /// 各監視対象温度による状況を更新します。
        /// </remarks>
        private void updateStateTable()
        {
            // 三好氏の提案によれば、BF1とBF2の温度調整範囲は（1° - > 2°）に拡張され、R1とR2dの温度調整範囲は（1° - > 3°）に拡張されています。
            // 温度範囲状態を現在値から設定する。
            var tempData = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.TemperatureParameter;

            foreach (Int32 moduleindex in Enum.GetValues(typeof(ModuleIndex)))
            {
                Temperature moduleTemp = Singleton<PublicMemory>.Instance.moduleTemperature[moduleindex];
                var tempStateTable = this.tempStateTables[moduleindex];

                //各温度毎に適正値の設定がゼロの場合は測定結果を無視し、適正値が設定されている場合は対象温度
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                {
                    // モーターエラーのスレーブは処理を行わない
                    if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleindex)] == Status.SystemStatusKind.MotorError)
                    {
                        //接続されていない場合はOK扱いにする
                        tempStateTable[this.lblReactionTableValue] = false;
                        tempStateTable[this.lblBFTableValue] = false;
                        tempStateTable[this.lblBF1Value] = false;
                        tempStateTable[this.lblBF2Value] = false;
                        tempStateTable[this.lblR1Value] = false;
                        tempStateTable[this.lblR2Value] = false;
                        tempStateTable[this.lblChemilumiValue] = false;
                    }
                    else
                    {
                        tempStateTable[this.lblReactionTableValue] = judgeTempState(tempData.TempReactionTable, moduleTemp.TempReactionTable, CarisXConst.ANALYZER_TEMP_NORMAL_RANGE1);
                        tempStateTable[this.lblBFTableValue] = judgeTempState(tempData.TempBFTable, moduleTemp.TempBFTable, CarisXConst.ANALYZER_TEMP_NORMAL_RANGE1);
                        tempStateTable[this.lblBF1Value] = judgeTempState(tempData.TempBF1PreHeat, moduleTemp.TempBF1PreHeat, CarisXConst.ANALYZER_TEMP_NORMAL_RANGE1);
                        tempStateTable[this.lblBF2Value] = judgeTempState(tempData.TempBF2PreHeat, moduleTemp.TempBF2PreHeat, CarisXConst.ANALYZER_TEMP_NORMAL_RANGE1_5);
                        tempStateTable[this.lblR1Value] = judgeTempState(tempData.TempR1ProbePreHeat, moduleTemp.TempR1ProbePreHeat, CarisXConst.ANALYZER_TEMP_NORMAL_RANGE5);
                        tempStateTable[this.lblR2Value] = judgeTempState(tempData.TempR2ProbePreHeat, moduleTemp.TempR2ProbePreHeat, CarisXConst.ANALYZER_TEMP_NORMAL_RANGE5);
                        tempStateTable[this.lblChemilumiValue] = judgeTempState(tempData.TempChemiLightMeas, moduleTemp.TempChemiLightMeas, CarisXConst.ANALYZER_TEMP_NORMAL_RANGE1);
                    }
                }
                else
                {
                    //接続されていない場合はOK扱いにする
                    tempStateTable[this.lblReactionTableValue] = false;
                    tempStateTable[this.lblBFTableValue] = false;
                    tempStateTable[this.lblBF1Value] = false;
                    tempStateTable[this.lblBF2Value] = false;
                    tempStateTable[this.lblR1Value] = false;
                    tempStateTable[this.lblR2Value] = false;
                    tempStateTable[this.lblChemilumiValue] = false;
                }
            }
        }

        /// <summary>
        /// 温度が適正値になっているかどうかを判定する
        /// </summary>
        /// <param name="paramTemp">システムパラメータに定義されている温度</param>
        /// <param name="moduleTemp">モジュールから取得した温度</param>
        /// <param name="tempRange">温度の有効範囲</param>
        /// <returns>true:適正値になっていない、false:適正値になっている</returns>
        private Boolean judgeTempState(Double paramTemp, Double moduleTemp, Double tempRange)
        {
            return paramTemp == 0 ? false : ((paramTemp - tempRange) > moduleTemp);
        }

        /// <summary>
        /// モジュール１クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModule1_Click(object sender, EventArgs e)
        {
            btnModule1.CurrentState = true;
            btnModule2.CurrentState = false;
            btnModule3.CurrentState = false;
            btnModule4.CurrentState = false;

            displayModuleIndex = (Int32)ModuleIndex.Module1;
            updateTempDisplay();
        }

        /// <summary>
        /// モジュール２クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModule2_Click(object sender, EventArgs e)
        {
            btnModule1.CurrentState = false;
            btnModule2.CurrentState = true;
            btnModule3.CurrentState = false;
            btnModule4.CurrentState = false;

            displayModuleIndex = (Int32)ModuleIndex.Module2;
            updateTempDisplay();
        }

        /// <summary>
        /// モジュール３クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModule3_Click(object sender, EventArgs e)
        {
            btnModule1.CurrentState = false;
            btnModule2.CurrentState = false;
            btnModule3.CurrentState = true;
            btnModule4.CurrentState = false;

            displayModuleIndex = (Int32)ModuleIndex.Module3;
            updateTempDisplay();
        }

        /// <summary>
        /// モジュール４クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModule4_Click(object sender, EventArgs e)
        {
            btnModule1.CurrentState = false;
            btnModule2.CurrentState = false;
            btnModule3.CurrentState = false;
            btnModule4.CurrentState = true;

            displayModuleIndex = (Int32)ModuleIndex.Module4;
            updateTempDisplay();
        }

        #endregion
    }

}
