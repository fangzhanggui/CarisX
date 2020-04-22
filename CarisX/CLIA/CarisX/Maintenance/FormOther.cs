using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.CarisX.Parameter;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.Common.Comm;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Const;
using Oelco.CarisX.Properties;
using Oelco.CarisX.DB;

namespace Oelco.CarisX.Maintenance
{
    /// <summary>
    /// その他
    /// </summary>
    public partial class FormOther : FormUnitBase
    {
        IMaintenanceUnitStart unitStart = new UnitStart();
        volatile bool ResponseFlg;
        public CommonFunction ComFunc = new CommonFunction();

        public FormOther()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.None;
            ParametersAllFalse();
            setCulture();
            setRadioButtonValue();
            ComFunc.SetControlSettings(this);

            lbxtestSequenceListBox.SelectedIndex = 0;

            // 設定ファイルの値をセット
            int moduleConfigFlag = 0;       //初期値は持たない
            this.txtModuleConfigFlag.Text = moduleConfigFlag.ToString( "X4" );
            String convFlag = Convert.ToString( moduleConfigFlag, 2 ).PadLeft( 16, '0' );
            this.lblModuleConfigFlag2.Text = String.Format( "[{0}] ({1},{2},{3},{4})", moduleConfigFlag.ToString( "D5" )
                                                                                     , convFlag.Substring( 0, 4 )
                                                                                     , convFlag.Substring( 4, 4 )
                                                                                     , convFlag.Substring( 8, 4 )
                                                                                     , convFlag.Substring( 12, 4 ) );
        }

        private void setCulture()
        {
            //Tab
            tabUnit.Tabs[0].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_000;//Test
            tabUnit.Tabs[1].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_001;//Configuration
            tabUnit.Tabs[2].Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_002;//Motor Parameters

            //Testタブ
            gbxtestSequence.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_003;//Sequence
            object SequenceList = new SequenceItem[]
            {
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_004, No=(int)OtherSequence.WarningBeep},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_005, No=(int)OtherSequence.AllLEDTest},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_025, No=(int)OtherSequence.SoftWareTEST1},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_026, No=(int)OtherSequence.SoftWareTEST2},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_006, No=(int)OtherSequence.CPLDWrite},
                new SequenceItem{Name=Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_007, No=(int)OtherSequence.CPLDRead},
            };
            lbxtestSequenceListBox = ComFunc.setSequenceListBox(lbxtestSequenceListBox, SequenceList);

            gbxtestParameters.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_008;
            lbltestRepeatFrequency.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_009;
            lbltestNumber.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_010;
            lbltestRepeatInterval.Text = Resources_Maintenance.STRING_MAINTENANCE_OTHER_027;
            lbltestRepeatIntervalUnit.Text = Resources_Maintenance.STRING_MAINTENANCE_OTHER_028;
            lbltestWarningBeep.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_012;
            rbttestBeepON.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_013;
            rbttestBeepOFF.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_014;
            lbltestMelody.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_020;
            lbltestVolume.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_021;
            lbltestInterval.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_015;
            lbltestIntervalUnit.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_016;
            lbltestReadLength.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_017;
            lbltestAddress.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_018;
            lbltestAddress0x.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_029;
            lbltestData.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_019;
            lbltestData0x.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_029;
            lbltestData1.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_022;
            lbltestData2.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_023;
            lbltestData3.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_024;

            gbxtestResponce.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_011;

            //Configurationタブ
            btnClearReagentHistory.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_030;
            gbxModuleConfigFlag.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_031;
            lblTitleModuleConfigFlag.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_032;
            lblModuleConfigFlag0x.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_029;
            btnResetModuleConfigFlag.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_033;
            btnModifyModuleConfigFlag.Text = Properties.Resources_Maintenance.STRING_MAINTENANCE_OTHER_034;

            //MotorParameterタブ

        }

        /// <summary>
        /// ラジオボタンの選択値を設定
        /// </summary>
        private void setRadioButtonValue()
        {
            rbttestBeepON.Tag = (int)OtherRadioValue.BeepON;
            rbttestBeepOFF.Tag = (int)OtherRadioValue.BeepOFF;
        }

        #region [Addressの入力制御]
        /// <summary>
        /// アドレスに16進数で入力できる値のみ入力可能にする
        /// </summary>
        private void txttestAddress_KeyDown(object sender, KeyEventArgs e)
        {

            bool keyPressCk = true;

            if (('A' <= e.KeyValue && e.KeyValue <= 'F')                                // A～F
             || (!e.Shift && '0' <= e.KeyValue && e.KeyValue <= '9')                    // 0～9 (キーボード)　※記号が入らないようにShiftキーが押されていない場合のみ許可
             || ((int)Keys.NumPad0 <= e.KeyValue && e.KeyValue <= (int)Keys.NumPad9)    // 1～9(テンキー) 
             || ((int)Keys.Left == e.KeyValue || (int)Keys.Right == e.KeyValue)         // ←→
             || ((int)Keys.Home == e.KeyValue || (int)Keys.End == e.KeyValue)           // Home、End
             || ((int)Keys.Back == e.KeyValue || (int)Keys.Delete == e.KeyValue)        // BackSpace、Del
             || ((int)Keys.Tab == e.KeyValue)                                           // Tab
             )
            {
                keyPressCk = false;
            }

            // [KeyDown]後の[KeyPress]イベント
            e.SuppressKeyPress = keyPressCk; // true = 無効 / false = 許可

        }

        /// <summary>
        /// アドレスに16進数で入力できる値のみ入力可能にする
        /// </summary>
        private void txttestAddress_Validating(object sender, CancelEventArgs e)
        {
            //入力されていない場合はチェックしない
            if (txttestAddress.Text.Length == 0)
            {
                return;
            }

            //画面の16進数を10進数に変更
            int address = int.Parse(txttestAddress.Text, System.Globalization.NumberStyles.HexNumber);
            //0x000～0x1FF（0～511）の範囲外の場合はエラー
            if (!(0 <= address && address <= 511))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// アドレスが3桁の16進数で表示されるようにする
        /// </summary>
        private void txttestAddress_Validated(object sender, EventArgs e)
        {
            //未入力の場合は"0"を設定
            if (txttestAddress.Text.Length == 0) txttestAddress.Text = "0";

            //画面の16進数を10進数に変更
            int address = int.Parse(txttestAddress.Text, System.Globalization.NumberStyles.HexNumber);
            //3桁の16進数にして再設定
            txttestAddress.Text = address.ToString("X3");
        }
        #endregion

        #region [Dataの入力制御]
        /// <summary>
        /// Dataに16進数で入力できる値のみ入力可能にする
        /// </summary>
        private void txttestData_KeyDown(object sender, KeyEventArgs e)
        {

            bool keyPressCk = true;

            if (('A' <= e.KeyValue && e.KeyValue <= 'F')                                // A～F
             || (!e.Shift && '0' <= e.KeyValue && e.KeyValue <= '9')                    // 0～9 (キーボード)　※記号が入らないようにShiftキーが押されていない場合のみ許可
             || ((int)Keys.NumPad0 <= e.KeyValue && e.KeyValue <= (int)Keys.NumPad9)    // 1～9(テンキー) 
             || ((int)Keys.Left == e.KeyValue || (int)Keys.Right == e.KeyValue)         // ←→
             || ((int)Keys.Home == e.KeyValue || (int)Keys.End == e.KeyValue)           // Home、End
             || ((int)Keys.Back == e.KeyValue || (int)Keys.Delete == e.KeyValue)        // BackSpace、Del
             || ((int)Keys.Tab == e.KeyValue)                                           // Tab
             )
            {
                keyPressCk = false;
            }

            // [KeyDown]後の[KeyPress]イベント
            e.SuppressKeyPress = keyPressCk; // true = 無効 / false = 許可

        }

        /// <summary>
        /// Dataに0x00～0xFF以外が入力されている場合はエラー
        /// </summary>
        private void txttestData_Validating(object sender, CancelEventArgs e)
        {
            //入力されていない場合はチェックしない
            if (txttestData.Text.Length == 0)
            {
                return;
            }

            //画面の16進数を10進数に変更
            int address = int.Parse(txttestData.Text, System.Globalization.NumberStyles.HexNumber);
            //0x00～0xFF（0～255）の範囲外の場合はエラー
            if (!(0 <= address && address <= 255))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Dataが2桁の16進数で表示されるようにする
        /// </summary>
        private void txttestData_Validated(object sender, EventArgs e)
        {
            //未入力の場合は"0"を設定
            if (txttestData.Text.Length == 0) txttestData.Text = "0";

            //画面の16進数を10進数に変更
            int address = int.Parse(txttestData.Text, System.Globalization.NumberStyles.HexNumber);
            //3桁の16進数にして再設定
            txttestData.Text = address.ToString("X2");
        }
        #endregion

        #region [ModuleConfigFlagの入力制御]
        /// <summary>
        /// ModuleConfigFlagに16進数で入力できる値のみ入力可能にする
        /// </summary>
        private void txtModuleConfigFlag_KeyDown( object sender, KeyEventArgs e )
        {
            bool keyPressCk = true;

            if (( 'A' <= e.KeyValue && e.KeyValue <= 'F' )                                // A～F
             || ( !e.Shift && '0' <= e.KeyValue && e.KeyValue <= '9' )                    // 0～9 (キーボード)　※記号が入らないようにShiftキーが押されていない場合のみ許可
             || ( (int)Keys.NumPad0 <= e.KeyValue && e.KeyValue <= (int)Keys.NumPad9 )    // 1～9(テンキー) 
             || ( (int)Keys.Left == e.KeyValue || (int)Keys.Right == e.KeyValue )         // ←→
             || ( (int)Keys.Home == e.KeyValue || (int)Keys.End == e.KeyValue )           // Home、End
             || ( (int)Keys.Back == e.KeyValue || (int)Keys.Delete == e.KeyValue )        // BackSpace、Del
             || ( (int)Keys.Tab == e.KeyValue )                                           // Tab
             )
            {
                keyPressCk = false;
            }

            // [KeyDown]後の[KeyPress]イベント
            e.SuppressKeyPress = keyPressCk; // true = 無効 / false = 許可
        }

        /// <summary>
        /// ModuleConfigFlagに0x00～0xFFFF以外が入力されている場合はエラー
        /// </summary>
        private void txtModuleConfigFlag_Validating( object sender, CancelEventArgs e )
        {
            // 入力されていない場合はチェックしない
            if (this.txtModuleConfigFlag.Text.Length == 0)
            {
                return;
            }

            // 画面の16進数を10進数に変更
            int moduleConfigFlag = int.Parse( this.txtModuleConfigFlag.Text, System.Globalization.NumberStyles.HexNumber );
            // 0x00～0xFFFF（0～65535‬）の範囲外の場合はエラー
            if (!( 0 <= moduleConfigFlag && moduleConfigFlag <= 65535 ))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// ModuleConfigFlagが4桁の16進数で表示されるようにする
        /// </summary>
        private void txtModuleConfigFlag_Validated( object sender, EventArgs e )
        {
            // 未入力の場合は"0"を設定
            if (this.txtModuleConfigFlag.Text.Length == 0)
            {
                this.txtModuleConfigFlag.Text = "0";
            }

            // 画面の16進数を10進数に変更
            int moduleConfigFlag = int.Parse( this.txtModuleConfigFlag.Text, System.Globalization.NumberStyles.HexNumber );

            // 4桁の16進数にして再設定
            this.txtModuleConfigFlag.Text = moduleConfigFlag.ToString( "X4" );
        }
        #endregion

/// <summary>
/// ユニットをスタートします。
/// </summary>
        public override void UnitStart()
        {
            //入力値チェック
            if (!CheckControls(tabUnit.Tabs[0].TabPage))
            {
                DlgMaintenance msg = new DlgMaintenance(Resources_Maintenance.DlgSeqChkMsg, false);
                msg.ShowDialog();
                DispEnable(true);
                return;
            }
            //フォームのバリデーションを行う
            if (!this.Validate()){
                DlgMaintenance msg = new DlgMaintenance(Resources_Maintenance.DlgSeqChkMsg, false);
                msg.ShowDialog();
                DispEnable(true);
                return;
            }
            txttestResponce.Text = "";
            Singleton<CarisXSequenceHelperManager>.Instance.Maintenance.ExecuteSeqence(UnitStartPrc);
        }

        /// <summary>
        /// ユニットスタート処理をおこないます。
        /// </summary>
        private String UnitStartPrc(String str, object[] param)
        {
            Func<object> getfuncno = () => { return lbxtestSequenceListBox.SelectedValue; };
            int FuncNo = (int)Invoke(getfuncno);

            Func<string> getfuncname = () => { return lbxtestSequenceListBox.Text; };
            string FuncName = (string)Invoke(getfuncname);

            SlaveCommCommand_0439 StartComm = new SlaveCommCommand_0439();
            StartComm.UnitNo = (int)UnitNoList.Other;
            StartComm.FuncNo = FuncNo;

            switch (FuncNo)
            {
                case (int)OtherSequence.WarningBeep:
                    StartComm.Arg1 = ComFunc.getSelectedRadioButtonValue(gbxtestWarningBeep);
                    StartComm.Arg2 = (int)numtestMelody.Value;
                    StartComm.Arg3 = (int)numtestVolume.Value;
                    break;
                case (int)OtherSequence.AllLEDTest:
                    StartComm.Arg1 = (int)numtestInterval.Value;
                    break;
                case (int)OtherSequence.SoftWareTEST1:
                case (int)OtherSequence.SoftWareTEST2:
                    StartComm.Arg1 = (int)numtestData1.Value;
                    StartComm.Arg2 = (int)numtestData2.Value;
                    StartComm.Arg3 = (int)numtestData3.Value;
                    break;
                case (int)OtherSequence.CPLDWrite:
                    //StartComm.Arg1には何も設定せず飛ばす
                    StartComm.Arg2 = int.Parse(txttestAddress.Text, System.Globalization.NumberStyles.HexNumber);
                    StartComm.Arg3 = int.Parse(txttestData.Text, System.Globalization.NumberStyles.HexNumber);
                    break;
                case (int)OtherSequence.CPLDRead:
                    StartComm.Arg1 = (int)numtestReadLength.Value;
                    StartComm.Arg2 = int.Parse(txttestAddress.Text, System.Globalization.NumberStyles.HexNumber);
                    break;
            }

            //レスポンスがある機能の場合のみ、レスポンスのログファイルを作成
            if (ComFunc.chkExistsResponse(MaintenanceMainNavi.Other, FuncNo))
            {
                Singleton<ResponseLog>.Instance.CreateLog();
                Singleton<ResponseLog>.Instance.SequenceNo = FuncNo;
                Singleton<ResponseLog>.Instance.SequenceName = FuncName;
            }

            Func<int> getRepeatInterval = () => { return (int)numtestRepeatInterval.Value; };
            int repeatInteerval = (int)Invoke(getRepeatInterval);

            Func<int> repeatFrequency = () => { return (int)numtestRepeatFrequency.Value; };
            for (int i = 0; i < (int)Invoke(repeatFrequency); i++)
            {
                // 繰り返しインターバルに設定されているミリ秒分待機（初回以外かつ中断フラグオフ）
                if (i != 0 && !AbortFlg) System.Threading.Thread.Sleep(repeatInteerval);

                // ポーズ処理
                while (PauseFlg)
                {
                    if (AbortFlg) break;
                }

                // 中断処理
                if (AbortFlg)
                {
                    AbortFlg = false;
                    break;
                }

                // 現在の実行回数を画面に反映
                Action getNumber = () => { lbltestNumberDsp.Text = (i + 1).ToString(); }; Invoke(getNumber);

                // 受信待ちフラグを設定
                ResponseFlg = true;

                // コマンド送信
                unitStart.Start(StartComm);

                // 応答コマンド受信待ち
                while (ResponseFlg)
                {
                    if (AbortFlg)
                    {
                        // 中断要求により受信待ち解除
                        break;
                    }
                }
            }

            AbortFlg = false;

            //メイン画面のナビゲータバーをEnableコマンドバーをデフォルトに戻す。
            Action Disp = () => { DispEnable(true); }; Invoke(Disp);

            return str;
        }

        /// <summary>
        /// 受信データ処理をおこないます。
        /// </summary>
        public override void SetResponse(CommCommandEventArgs comm)
        {
            switch (comm.Command.CommandId)
            {
                case (int)CommandKind.Command1439:
                    //ユニットテスト
                    string Result = comm.Command.CommandText.Remove(0, 20).Trim();

                    if (ComFunc.chkExistsResponse(MaintenanceMainNavi.Other, (int)lbxtestSequenceListBox.SelectedValue))
                    {
                        Singleton<ResponseLog>.Instance.RepeatNo = int.Parse(lbltestNumberDsp.Text);
                        Singleton<ResponseLog>.Instance.WriteLog(Result);
                        Result = (txttestResponce.Text + Result + System.Environment.NewLine);      //画面の内容にレスポンスの内容を追記する形にする
                    }

                    txttestResponce.Text = Result;
                    txttestResponce.SelectionStart = txttestResponce.Text.Length;
                    txttestResponce.Focus();
                    txttestResponce.ScrollToCaret();

                    ResponseFlg = false;
                    break;
                case (int)CommandKind.Command1412:
                    //ポーズ、再開、停止のいずれか
                    switch (LastestPauseCtl)
                    {
                        case CommandControlParameter.Abort:
                            //停止処理時
                            PauseFlg = false;
                            AbortFlg = true;

                            this.Enabled = true;
                            break;
                        case CommandControlParameter.Pause:
                            //ポーズ処理時
                            //レスポンスを受けてする処理なし
                            break;
                        case CommandControlParameter.Restart:
                            //再開処理時
                            PauseFlg = false;
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// パラメータをすべて非活性にする
        /// </summary>
        private void ParametersAllFalse()
        {
            gbxtestWarningBeep.Enabled = false;
            numtestInterval.Enabled = false;
            numtestReadLength.Enabled = false;
            txttestAddress.Enabled = false;
            txttestData.Enabled = false;
            numtestData1.Enabled = false;
            numtestData2.Enabled = false;
            numtestData3.Enabled = false;
            numtestMelody.Enabled = false;
            numtestVolume.Enabled = false;
        }

        /// <summary>
        /// 選択された機能番号に応じてパラメータを活性化する
        /// </summary>
        private void lbxtestSequenceListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParametersAllFalse();
            switch (lbxtestSequenceListBox.SelectedValue)
            {
                case (int)OtherSequence.WarningBeep:
                    gbxtestWarningBeep.Enabled = true;
                    numtestMelody.Enabled = true;
                    numtestVolume.Enabled = true;
                    break;
                case (int)OtherSequence.AllLEDTest:
                    numtestInterval.Enabled = true;
                    break;
                case (int)OtherSequence.SoftWareTEST1:
                case (int)OtherSequence.SoftWareTEST2:
                    numtestData1.Enabled = true;
                    numtestData2.Enabled = true;
                    numtestData3.Enabled = true;
                    break;
                case (int)OtherSequence.CPLDWrite:
                    txttestAddress.Enabled = true;
                    txttestData.Enabled = true;
                    break;
                case (int)OtherSequence.CPLDRead:
                    numtestReadLength.Enabled = true;
                    txttestAddress.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// タブが切り替えられたときにメンテナンスメイン画面のコマンドバーの表示切り替えをおこないます。
        /// </summary>
        private void TabControl_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            ToolbarsControl();
            SetUnitMode(e.Tab.Index);
        }

        /// <summary>
        /// メンテナンスメイン画面のコマンドバーの表示切り替えをおこないます。
        /// </summary>
        public override void ToolbarsControl(int toolBarEnablekind = -1)
        {
            //デリゲートでCall
            ToolbarsDisp(tabUnit.SelectedTab.Index, toolBarEnablekind);
        }

        /// <summary>
        /// Maintenanc画面のEnable/Disableを切り替えます。
        /// </summary>
        private void DispEnable(bool enable)
        {
            this.Enabled = enable;
            MainToolbarsDispEnable(enable);
        }

        /// <summary>
        /// 試薬残量履歴クリア
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearReagentHistory_Click(object sender, EventArgs e)
        {
            DlgMaintenance dlg = new DlgMaintenance(Resources_Maintenance.DlgClearReagentHistory, true);
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                // 試薬残量履歴テーブルをクリア
                Singleton<ReagentHistoryDB>.Instance.DeleteAll();

                // 残量クリアコマンド送信
                // 各モジュールへソフトウエア識別コマンドを送信
                foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
                {
                    // オンラインとなっているモジュールへ送信
                    if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                    {
                        SlaveCommCommand_0475 cmd0475 = new SlaveCommCommand_0475();
                        Singleton<CarisXCommManager>.Instance.PushSendQueueSlave((int)moduleindex, cmd0475);
                    }
                }
            }
        }

        /// <summary>
        /// モジュール構成フラグリセット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetModuleConfigFlag_Click( object sender, EventArgs e )
        {
            // 設定ファイルの値に戻す
            int moduleConfigFlag = 0;
            this.txtModuleConfigFlag.Text = moduleConfigFlag.ToString( "X4" );
            String convFlag = Convert.ToString( moduleConfigFlag, 2 ).PadLeft( 16, '0' );
            this.lblModuleConfigFlag2.Text = String.Format( "[{0}] ({1},{2},{3},{4})", moduleConfigFlag.ToString( "D5" )
                                                                                     , convFlag.Substring( 0, 4 )
                                                                                     , convFlag.Substring( 4, 4 )
                                                                                     , convFlag.Substring( 8, 4 )
                                                                                     , convFlag.Substring( 12, 4 ) );
        }

        /// <summary>
        /// モジュール構成フラグ変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyModuleConfigFlag_Click( object sender, EventArgs e )
        {
            // 画面の16進数を10進数に変更
            int moduleConfigFlag = int.Parse( this.txtModuleConfigFlag.Text, System.Globalization.NumberStyles.HexNumber );

            // 2進数表記を設定
            String convFlag = Convert.ToString( moduleConfigFlag, 2 ).PadLeft( 16, '0' );
            this.lblModuleConfigFlag2.Text = String.Format( "[{0}] ({1},{2},{3},{4})", moduleConfigFlag.ToString( "D5" )
                                                                                     , convFlag.Substring( 0, 4 )
                                                                                     , convFlag.Substring( 4, 4 )
                                                                                     , convFlag.Substring( 8, 4 )
                                                                                     , convFlag.Substring( 12, 4 ) );
        }

        /// <summary>
        /// モジュール構成フラグ送信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendModuleConfigFlag_Click( object sender, EventArgs e )
        {
            // 画面の16進数を10進数に変更
            int moduleConfigFlag = int.Parse( this.txtModuleConfigFlag.Text, System.Globalization.NumberStyles.HexNumber );
            // 2進数表記を設定
            String convFlag = Convert.ToString( moduleConfigFlag, 2 ).PadLeft( 16, '0' );
            this.lblModuleConfigFlag2.Text = String.Format( "[{0}] ({1},{2},{3},{4})", moduleConfigFlag.ToString( "D5" )
                                                                                     , convFlag.Substring( 0, 4 )
                                                                                     , convFlag.Substring( 4, 4 )
                                                                                     , convFlag.Substring( 8, 4 )
                                                                                     , convFlag.Substring( 12, 4 ) );

            // ユニットテストコマンドで値を送信（選択中モジュールに対してのみ）
            SlaveCommCommand_0439 cmd0439 = new SlaveCommCommand_0439();
            cmd0439.UnitNo = (int)UnitNoList.Other;
            cmd0439.FuncNo = (int)OtherSequence.ModuleConfig;
            cmd0439.Arg1 = moduleConfigFlag;

            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(cmd0439);
        }
    }
}
