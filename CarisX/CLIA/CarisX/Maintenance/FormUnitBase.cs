using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
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
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using System.Collections;


namespace Oelco.CarisX.Maintenance
{
    public partial class FormUnitBase : Form
    {
        public SetToolbarsDisp ToolbarsDisp;
        public MaintenanceMainEnable MainToolbarsDispEnable;

        IMaintenanceUnitPause unitPause = new UnitPause();
        IMaintenanceUnitRestart unitRestart = new UnitRestart();
        IMaintenanceUnitStart unitAdjust = new UnitAdjust();
        IMaintenanceUnitAbort unitAbort = new UnitStop();

        public volatile bool PauseFlg = false;
        public volatile bool AbortFlg = false;
        public volatile CommandControlParameter LastestPauseCtl = CommandControlParameter.Abort;
        public volatile bool ConfigTabUseFlg = true;
        public MaintenanceUnitMode UnitMode = MaintenanceUnitMode.Other;

        public FormUnitBase()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.Opaque, true);  //ちらつきを抑える為に背景の消去を行わない
        }

        /// <summary>
        /// ユニットをスタートします。
        /// </summary>
        public virtual void UnitStart()
        {
            //処理の共通化を行うため、枠を定義
            //処理については継承先で実装する
        }

        /// <summary>
        /// ユニットをポーズします。
        /// </summary>
        public void UnitPause(ModuleKind ModuleKind = ModuleKind.Slave)
        {
            LastestPauseCtl = CommandControlParameter.Pause;
            PauseFlg = true;
            unitPause.Pause(ModuleKind);
        }

        /// <summary>
        /// ユニットをリスタートします。
        /// </summary>
        public void UnitRestart(ModuleKind ModuleKind = ModuleKind.Slave)
        {
            LastestPauseCtl = CommandControlParameter.Restart;
            unitRestart.Restart(ModuleKind);
        }

        /// <summary>
        /// ユニットを停止します。
        /// </summary>
        public virtual bool UnitAbortexe()
        {
            //処理の共通化を行うため、枠を定義
            //処理については継承先で実装する
            UnitAbort();
            return true;
        }

        /// <summary>
        /// ユニットを停止します。
        /// </summary>
        public void UnitAbort(ModuleKind ModuleKind = ModuleKind.Slave)
        {
            LastestPauseCtl = CommandControlParameter.Abort;
            unitAbort.Abort(ModuleKind);
        }

        /// <summary>
        /// パラメータ保存
        /// </summary>
        public virtual void ParamSave()
        {
            //処理の共通化を行うため、枠を定義
            //処理については継承先で実装する
        }

        /// <summary>
        /// コンフィグパラメータを読み込みます。
        /// </summary>
        public void configLoad(ParameterFilePreserve<CarisXConfigParameter> param)
        {
            param.LoadRaw();
        }

        /// <summary>
        /// コンフィグパラメータを保存します。
        /// </summary>
        public void configSave(ParameterFilePreserve<CarisXConfigParameter> param)
        {
            param.SaveRaw();
        }


        /// <summary>
        /// モーターパラメータを読み込みます。
        /// </summary>
        public void motorLoad(ParameterFilePreserve<CarisXMotorParameter> param)
        {
            param.LoadRaw();
        }

        /// <summary>
        /// モーターパラメータを保存します。
        /// </summary>
        public void motorSave(ParameterFilePreserve<CarisXMotorParameter> param)
        {
            param.SaveRaw();
        }

        public List<CarisXCommCommand> sendParamList = new List<CarisXCommCommand>();
        /// <summary>
        /// 送信するパラメータコマンドを貯めておきます。
        /// </summary>
        protected void SpoolParam(CarisXCommCommand param)
        {
            sendParamList.Add(param);
        }

        /// <summary>
        /// パラメータコマンドを送信します。
        /// </summary>
        protected void SendParam(ModuleKind ModuleKind = ModuleKind.Slave)
        {
            List<CarisXCommCommand> temp = new List<CarisXCommCommand>();
            temp.AddRange(sendParamList);

            // データ送信
            Singleton<CarisXSequenceHelperManager>.Instance.Maintenance.MaintenanceSetParamSequence(temp, ModuleKind);

            // リストクリア
            sendParamList.Clear();
        }

        /// <summary>
        /// パラメータコマンドを送信します。
        /// </summary>
        protected void SendParam(CarisXCommCommand param, ModuleKind ModuleKind = ModuleKind.Slave )
        {
            SpoolParam(param);
            SendParam(ModuleKind);
        }

        /// <summary>
        /// センサー使用有無パラメータを保存します。
        /// </summary>
        public void sensorParamSave(ParameterFilePreserve<CarisXSensorParameter> param)
        {
            param.SaveRaw();
        }

        /// <summary>
        /// センサー使用有無パラメータを読み込みます。
        /// </summary>
        public void SensorParamLoad(ParameterFilePreserve<CarisXSensorParameter> param)
        {
            param.LoadRaw();
        }

        /// <summary>
        /// 指定されたコントロール上に存在するすべてのコントロールを取得します。
        /// </summary>
        /// <param name="top_ctrl"></param>
        /// <returns></returns>
        protected bool CheckControls(Control top_ctrl)
        {
            List<Control> controls = GetAllControls(top_ctrl);

            foreach (Control ctrl in controls)
            {
                Infragistics.Win.UltraWinEditors.UltraNumericEditor target = ctrl as Infragistics.Win.UltraWinEditors.UltraNumericEditor;
                if (target != null && target.ReadOnly == false)
                {
                    if (!IsNumeric(target.Value.ToString()))
                        return false;
                }
            }
            return true;
        }


        protected bool IsNumeric(string stTarget)
        {
            double dNullable;

            return double.TryParse(
                stTarget,
                System.Globalization.NumberStyles.Any,
                null,
                out dNullable
            );
        }


        protected List<Control> GetAllControls(Control top_ctrl)
        {
            List<Control> buf = new List<Control>();
            foreach (Control c in top_ctrl.Controls)
            {
                buf.Add(c);
                buf.AddRange(GetAllControls(c));
            }
            return buf;
        }

        /// <summary>
        /// メンテナンスメイン画面のコマンドバーの表示切り替えをおこないます。
        /// </summary>
        public virtual void ToolbarsControl(int toolBarEnablekind = -1)
        {
            //処理の共通化を行うため、枠を定義
            //処理については継承先で実装する
        }

        /// <summary>
        /// コンフィグパラメータ読み込み
        /// </summary>
        public virtual void ConfigParamLoad()
        {
            //処理の共通化を行うため、枠を定義
            //処理については継承先で実装する
        }

        /// <summary>
        /// モーターパラメータ読み込み
        /// </summary>
        public virtual void MotorParamDisp()
        {
            //処理の共通化を行うため、枠を定義
            //処理については継承先で実装する
        }

        /// <summary>
        /// 受信データ処理をおこないます。
        /// </summary>
        public virtual void SetResponse(CommCommandEventArgs comm)
        {
            //処理の共通化を行うため、枠を定義
            //処理については継承先で実装する
        }

        /// <summary>
        /// 受信データ処理をおこないます。
        /// </summary>
        public virtual void SetParameterResponse(bool SendResult)
        {
            //処理の共通化を行うため、枠を定義
            //処理については継承先で実装する
        }

        /// <summary>
        /// 指定されたコントロール上に存在editBoxの文字色を黒にする。
        /// </summary>
        /// <param name="top_ctrl"></param>
        /// <returns></returns>
        protected void EditBoxControlsBlack(Control top_ctrl)
        {
            List<Control> controls = GetAllControls(top_ctrl);

            foreach (Control ctrl in controls)
            {
                Infragistics.Win.UltraWinEditors.UltraNumericEditor target = ctrl as Infragistics.Win.UltraWinEditors.UltraNumericEditor;
                if (target != null && target.ReadOnly == false)
                {
                    target.Appearance.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

        protected void WriteLog(string text)
        {
            string file_path = Application.StartupPath + @"\MentenanceLog.txt";

            using (System.IO.FileStream fs = System.IO.File.Open(file_path, System.IO.FileMode.Append))
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fs))
                {
                    writer.WriteLine(DateTime.Now.ToString() + "," + text);
                }
            }
        }

        /// <summary>
        /// UP/DownボタンをEnable/Disableにする
        /// </summary>
        public virtual void UpDownButtonEnable(bool enable)
        {
            //処理の共通化を行うため、枠を定義
            //処理については継承先で実装する
        }

        /// <summary>
        /// モーター移動 
        /// </summary>
        /// <param name="Up">調整値を加算するかどうか</param>
        /// <param name="MotorNo">コマンドで送信するモーター番号</param>
        /// <param name="numOffset">調整対象のオフセットのコントロール</param>
        /// <param name="Pitch">調整値</param>
        protected void AdjustValue(bool Up, int MotorNo, Infragistics.Win.UltraWinEditors.UltraNumericEditor numOffset, double Pitch)
        {
            UpDownButtonEnable(false);

            SlaveCommCommand_0473 AdjustUpDownComm = new SlaveCommCommand_0473();

            //モーター調整コマンドチクチク
            AdjustUpDownComm.MotorNo = MotorNo;
            if (Up)
                AdjustUpDownComm.Distance = (double)Pitch;
            else
                AdjustUpDownComm.Distance = -(double)Pitch;

            //画面の調整対象のオフセットに調整値を加味する
            //Downの場合はDistanceにマイナス値が入っているので減算になる
            numOffset.Value = (double)numOffset.Value + AdjustUpDownComm.Distance;

            unitAdjust.Start(AdjustUpDownComm);

            //レスポンスはSetResponseで待機
        }

        /// <summary>
        /// フォームの活性状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormUnitBase_EnabledChanged(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                //フォームが活性状態になった際、アクティブコントロールの再設定を行う
                //UltraNumericEditerにフォーカスがある状態で、フォームの非活性→活性とすると、
                //なぜかフォーカスが設定されているUltraNumericEditerが操作不能に陥るため。
                Control tempcontrol = this.ActiveControl;
                this.ActiveControl = null;
                this.ActiveControl = tempcontrol;
            }
        }

        /// <summary>
        /// パラメータに渡されたタブのインデックスからモードを設定します
        /// </summary>
        /// <param name="tabindex"></param>
        protected void SetUnitMode(int tabindex)
        {
            switch (tabindex)
            {
                case (int)MaintenanceTabIndex.Test:
                    UnitMode = MaintenanceUnitMode.Test;
                    break;
                case (int)MaintenanceTabIndex.Config:
                    UnitMode = MaintenanceUnitMode.Config;
                    break;
                case (int)MaintenanceTabIndex.MParam:
                    UnitMode = MaintenanceUnitMode.MParam;
                    break;
                case (int)MaintenanceTabIndex.MAdjust:
                    UnitMode = MaintenanceUnitMode.MAdjust;
                    break;
                default:
                    UnitMode = MaintenanceUnitMode.Other;
                    break;
            }

        }
    }
}
