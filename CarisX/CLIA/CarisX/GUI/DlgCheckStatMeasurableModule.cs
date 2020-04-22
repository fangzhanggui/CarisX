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
using Oelco.CarisX.DB;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Status;
using Oelco.CarisX.Const;
using System.IO;
using System.Threading;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.Common.Comm;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinEditors;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    ///STAT測定可能モジュールの確認ダイアログクラス
    /// </summary>
    public partial class DlgCheckStatMeasurableModule : DlgCarisXBase
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 検体ID
        /// </summary>
        String patientID = String.Empty;

        /// <summary>
        /// モジュールIndexリスト
        /// </summary>
        List<Int32> moduleIndexList = null;

        /// <summary>
        /// モジュールアイテムリスト
        /// </summary>
        List<Tuple<UltraLabel,UltraLabel, PictureBox>> moduleItems = new List<Tuple<UltraLabel, UltraLabel, PictureBox>>();

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgCheckStatMeasurableModule( string patientId, List<Int32> moduleIndexList = null )
        {
            this.patientID = patientId;

            this.moduleIndexList = moduleIndexList;

            InitializeComponent();

            Singleton<NotifyManager>.Instance.AddNotifyTarget( (Int32)NotifyKind.UpdateStatMeasurableModule, this.onUpdateStatMeasurableModule );
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
            this.btnClose.Enabled = true;

            moduleItems.Clear();
            moduleItems.Add( new Tuple<UltraLabel, UltraLabel, PictureBox>( this.lblModule1Name, this.lblModule1StatOk, this.pctModule1Check ) );
            moduleItems.Add( new Tuple<UltraLabel, UltraLabel, PictureBox>( this.lblModule2Name, this.lblModule2StatOk, this.pctModule2Check ) );
            moduleItems.Add( new Tuple<UltraLabel, UltraLabel, PictureBox>( this.lblModule3Name, this.lblModule3StatOk, this.pctModule3Check ) );
            moduleItems.Add( new Tuple<UltraLabel, UltraLabel, PictureBox>( this.lblModule4Name, this.lblModule4StatOk, this.pctModule4Check ) );

            // 検体ID
            this.lblPatientID.Text = Properties.Resources.STRING_DLG_CHECKSTATMEASURABLEMODULE_001 + this.patientID;

            // モジュール毎の表示活性状態を制御する
            this.setVisibleModuleStatus();

            if( this.moduleIndexList != null )
            {
                this.onUpdateStatMeasurableModule( this.moduleIndexList );
            }
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
            this.btnClose.Text = Properties.Resources.STRING_COMMON_002;

            // ダイアログタイトル
            this.Caption = Properties.Resources.STRING_DLG_CHECKSTATMEASURABLEMODULE_000;

            // 検体IDラベル
            this.lblPatientID.Text = Properties.Resources.STRING_DLG_CHECKSTATMEASURABLEMODULE_001;

            // 選択項目
            this.lblModule1Name.Text = Properties.Resources.STRING_MAIN_FRAME_039;
            this.lblModule2Name.Text = Properties.Resources.STRING_MAIN_FRAME_040;
            this.lblModule3Name.Text = Properties.Resources.STRING_MAIN_FRAME_041;
            this.lblModule4Name.Text = Properties.Resources.STRING_MAIN_FRAME_042;
            this.lblModule1StatOk.Text = String.Empty;
            this.lblModule2StatOk.Text = String.Empty;
            this.lblModule3StatOk.Text = String.Empty;
            this.lblModule4StatOk.Text = String.Empty;
        }

        /// <summary>
        /// STAT測定可能モジュール更新
        /// </summary>
        /// <param name="value"></param>
        protected void onUpdateStatMeasurableModule( Object value )
        {
            if( value != null)
            {
                // モジュールIndexが指定されているため、キャスト
                List<Int32> moduleIndexList = (List<Int32>)value;

                // スレーブ接続台数取得
                int numOfConnectedCount = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

                foreach (int moduleIndex in moduleIndexList)
                {
                    // 接続台数を超えての変更は許可しない
                    if( moduleIndex < numOfConnectedCount )
                    {
                        // STAT OK
                        this.moduleItems[moduleIndex].Item2.Text = Properties.Resources.STRING_DLG_CHECKSTATMEASURABLEMODULE_002;

                        // 既にモジュールが選択済みか確認
                        // ※一番若い番号のチェックが見つかり次第、チェックループは抜ける
                        int alreadyCheckedModuleIndex = -1;
                        for (int checkIndex = 0; checkIndex < numOfConnectedCount; checkIndex++)
                        {
                            if (this.moduleItems[checkIndex].Item3.Visible == true)
                            {
                                alreadyCheckedModuleIndex = checkIndex;
                                break;
                            }
                        }

                        if (alreadyCheckedModuleIndex >= 0)
                        {
                            // 通知のきたモジュールIndexの方が若い番号の場合
                            if (alreadyCheckedModuleIndex > moduleIndex)
                            {
                                // 既にチェックが付いている方のチェックを外し、
                                this.moduleItems[alreadyCheckedModuleIndex].Item3.Visible = false;

                                // 新しく通知のきたモジュールのチェックをONにする
                                this.moduleItems[moduleIndex].Item3.Visible = true;
                            }
                            else
                            {
                                // 何もしない
                            }
                        }
                        else
                        {
                            // 通知のきたモジュールのチェックをONにする
                            this.moduleItems[moduleIndex].Item3.Visible = true;
                        }
                    }
                }
            }
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
        private void btnClose_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Module1～4タブの活性状態制御を行う
        /// 接続されていないものは画面上操作できないようにする
        /// </summary>
        private void setVisibleModuleStatus()
        {
            // スレーブ接続台数取得
            int numOfConnectedCount = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

            // カウンター設定
            int itemCount = 0;

            // 接続台数によって表示活性状態切り替え
            foreach (var moduleItem in this.moduleItems)
            {
                // チェック状態は チェック済み＋非活性＋非表示 とする
                moduleItem.Item3.Enabled = false;
                moduleItem.Item3.Visible = false;

                if (itemCount < numOfConnectedCount)
                {
                    // 接続台数の範囲内

                    // 表示設定
                    moduleItem.Item1.Visible = true;
                    moduleItem.Item2.Visible = true;

                    // STAT状態は「---」を表示
                    moduleItem.Item2.Text = Properties.Resources.STRING_DLG_CHECKSTATMEASURABLEMODULE_003;
                }
                else
                {
                    // 接続台数の範囲外

                    // 非表示設定
                    moduleItem.Item1.Visible = false;
                    moduleItem.Item2.Visible = false;

                    // STAT状態をクリア
                    moduleItem.Item2.Text = String.Empty;
                }

                itemCount++;
            }
        }

        #endregion
    }
}
