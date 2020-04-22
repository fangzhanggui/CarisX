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
using Oelco.CarisX.Const;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 試薬バーコード入力処理ダイアログクラス
    /// </summary>
    public partial class DlgOptionInputReagentBC : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgOptionInputReagentBC()
        {
            InitializeComponent();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 表示更新フラグ
        /// </summary>
        private Boolean isUpdateEnableControl = true;

        /// <summary>
        /// M試薬情報
        /// </summary>
        ReagentData MReagInfo = null;

        /// <summary>
        /// R1試薬/前処理液1情報
        /// </summary>
        ReagentData R1P1ReagInfo = null;

        /// <summary>
        /// R2試薬/前処理液2情報
        /// </summary>
        ReagentData R2P2ReagInfo = null;

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
            // ポート番号の初期値を設定
            this.numPortNo.Value = 1;

            // 試薬バーコード情報設定
            this.setReagentBCInfo( (int)this.numPortNo.Value );

            // 表示更新
            this.updateEnableControls();
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
            this.btnOk.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;

            // ダイアログタイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_000;

            this.lblPortNo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_001;

            this.gbxCommonReagentBCInfo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_002;
            this.lblMakerCode.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_006;
            this.lblReagentCode.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_007;
            this.chkPreprocessLiquid.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_008;
            this.lblLotNo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_009;
            this.lblExpirationDate.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_010;

            this.gbxMReagentBCInfo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_003;
            this.lblMReagSerialNo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_011;
            this.lblMReagCapacity.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_012;
            this.lblMReagCapacityUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_013;
            this.gbxMReagBottleVolume.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_014;
            this.optMReagRemainSelect.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_015;
            this.optMReagRemainSelect.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_016;
            this.lblMReagRemainVolumeUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_017;

            this.gbxRReagentBCInfo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_004;
            this.lblRReagSerialNo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_011;
            this.lblRReagCapacity.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_012;
            this.lblRReagCapacityUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_013;
            this.gbxRReagBottleVolume.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_014;
            this.optRReagRemainSelect.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_015;
            this.optRReagRemainSelect.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_016;
            this.lblR1ReagRemainVolume.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_018;
            this.lblR2ReagRemainVolume.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_019;
            this.lblR1ReagRemainVolumeUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_017;
            this.lblR2ReagRemainVolumeUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_017;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にOKを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOk_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            // ポート番号取得
            int portNo = 0;
            Int32.TryParse( this.numPortNo.Value.ToString(), out portNo );

            // 残量セットコマンドインスタンス生成
            SlaveCommCommand_0434 cmd0434 = new SlaveCommCommand_0434();

            // インターフェースの実装クラスをrefで渡せない為、ここで作業用にインターフェース型へ移し変える。内容の設定される実体はコマンドクラス
            IRemainAmountInfoSet remainAmountSet = cmd0434;

            // 最新の試薬残量情報取得
            Singleton<ReagentDB>.Instance.GetReagentRemain( ref remainAmountSet, CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex));

            // 前処理液の有無
            Boolean isPreprocessLiquid = false;
            if (( this.chkPreprocessLiquid.Visible == true ) && ( this.chkPreprocessLiquid.Checked == true ))
            {
                isPreprocessLiquid = true;
            }

            // 分析項目パラメータ取得
            MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo( (int)this.numReagentCode.Value );

            // M試薬の残量情報を更新
            int MPortNoIdx = ( ( portNo * 3 ) - 1 );
            remainAmountSet.ReagentRemainTable[MPortNoIdx] = this.getMReagentRemainTable( protocol, isPreprocessLiquid );

            // R1試薬/P1前処理液の残量情報を更新
            int R1P1PortNoIdx = ( ( ( portNo * 3 ) - 2 ) - 1 );
            remainAmountSet.ReagentRemainTable[R1P1PortNoIdx] = this.getR1P1ReagentRemainTable( protocol, isPreprocessLiquid );

            // R2試薬/P2前処理液の残量情報を更新
            int R2P2PortNoIdx = ( ( ( portNo * 3 ) - 1 ) - 1 );
            remainAmountSet.ReagentRemainTable[R2P2PortNoIdx] = this.getR2P2ReagentRemainTable( protocol, isPreprocessLiquid );

            // 試薬情報更新タイムスタンプ設定
            remainAmountSet.TimeStamp = DateTime.Now;

            // 試薬情報更新
            CarisXSubFunction.SetReagentRemain( remainAmountSet, true, CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex), false);

            // 残量セットコマンド送信
            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave( cmd0434 );

            // 完了メッセージ表示
            DlgMessage.Show( Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_022
                            , String.Empty
                            , Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_000
                            , MessageDialogButtons.Confirm );

            // 試薬情報設定
            this.setReagentBCInfo( portNo );

            // 表示更新
            this.updateEnableControls();
        }

        /// <summary>
        /// Cancelボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// ポート番号切り替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numPortNo_ValueChanged( object sender, EventArgs e )
        {
            int portNo = 0;

            if (this.numPortNo.Value != null)
            {
                Int32.TryParse( this.numPortNo.Value.ToString(), out portNo );
            }

            // 試薬情報設定
            this.setReagentBCInfo( portNo );

            // 表示更新
            this.updateEnableControls();
        }

        /// <summary>
        /// 試薬コード編集イベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numReagentCode_ValueChanged( object sender, EventArgs e )
        {
            // 表示更新
            this.updateEnableControls();
        }

        /// <summary>
        /// 前処理液チェック切り替えイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPreprocessLiquid_CheckedChanged( object sender, EventArgs e )
        {
            // 表示更新
            this.updateEnableControls();
        }

        /// <summary>
        /// 有効期限ボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExpirationDate_Click( object sender, EventArgs e )
        {
            // 日付選択ダイアログの呼び出し
            DateTime date;
            DateTime defaultDate = DateTime.Now;
            DateTime.TryParse( this.btnExpirationDate.Text, out defaultDate );
            DialogResult result = DlgDateSelect.Show( String.Empty, out date, defaultDate );
            if (DialogResult.OK == result)
            {
                this.btnExpirationDate.Text = date.ToShortDateString();
                this.btnExpirationDate.Tag = date;
            }
        }
        /// <summary>
        /// R1/R2試薬残量設定切り替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optRReagRemainSelect_ValueChanged( object sender, EventArgs e )
        {
            // 表示更新
            this.updateEnableControls();
        }

        /// <summary>
        /// M試薬残量設定切り替えイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void optMReagRemainSelect_ValueChanged( object sender, EventArgs e )
        {
            // 表示更新
            this.updateEnableControls();
        }

        /// <summary>
        /// 試薬バーコード情報設定
        /// </summary>
        /// <param name="portNo"></param>
        private void setReagentBCInfo( int portNo )
        {
            // 表示更新を停止
            this.isUpdateEnableControl = false;

            // 試薬情報の初期化
            this.MReagInfo = null;
            this.R1P1ReagInfo = null;
            this.R2P2ReagInfo = null;

            // モジュールINDEXの取得
            Int32 moduleId = CarisXSubFunction.ModuleIndexToModuleId(Singleton<PublicMemory>.Instance.moduleIndex);

            // M試薬リスト取得
            var reagentList = Singleton<ReagentDB>.Instance.GetData(ReagentKind.Reagent, moduleId).Where<ReagentData>(x => (x.PortNo == (portNo * 3)));
            if (reagentList != null && reagentList.Count() > 0)
            {
                MReagInfo = reagentList.ToList<ReagentData>().First();
                if (( MReagInfo.ReagentCode != 0 ) && ( MReagInfo.ReagentType == (int)ReagentType.M ))
                {
                    // 共通部の設定
                    this.numReagentCode.Value = MReagInfo.ReagentCode;              // 試薬コード
                    this.cmbMakerCode.Value = MReagInfo.MakerCode;                  // メーカーコード
                    this.txtLotNo.Text = MReagInfo.LotNo;                           // ロット番号
                    this.btnExpirationDate.Text = ( (DateTime)MReagInfo.ExpirationDate ).ToShortDateString();  // 有効期限
                    this.btnExpirationDate.Tag = MReagInfo.ExpirationDate.ToString();
                    this.numMReagSerialNo.Value = MReagInfo.SerialNo;               // シリアル番号
                    this.cmbMReagCapacity.Value = MReagInfo.Capacity;               // 容量
                    this.optMReagRemainSelect.Value = false;                        // 残量選択
                    this.numMReagRemainVolume.Value = MReagInfo.Remain;             // M試薬の残量設定
                }
                else
                {
                    MReagInfo = null;
                }
            }

            // R1試薬リスト取得
            reagentList = Singleton<ReagentDB>.Instance.GetData(ReagentKind.Reagent, moduleId).Where<ReagentData>(x => (x.PortNo == ((portNo * 3) - 2)));
            if (reagentList != null && reagentList.Count() > 0)
            {
                R1P1ReagInfo = reagentList.ToList<ReagentData>().First();
                if (R1P1ReagInfo.ReagentCode != 0 && ( R1P1ReagInfo.ReagentType != (int)ReagentType.M ))
                {
                    if (R1P1ReagInfo.ReagentType == (int)ReagentType.R1R2)
                    {
                        // R1/R2試薬
                        this.chkPreprocessLiquid.Checked = false;
                    }
                    else
                    {
                        // 前処理液1/ 前処理液2
                        this.chkPreprocessLiquid.Checked = true;
                    }

                    // 共通部の設定
                    if (MReagInfo == null)
                    {
                        this.numReagentCode.Value = R1P1ReagInfo.ReagentCode;           // 試薬コード
                        this.cmbMakerCode.Value = R1P1ReagInfo.MakerCode;               // メーカーコード
                        this.txtLotNo.Text = R1P1ReagInfo.LotNo;                        // ロット番号
                        this.btnExpirationDate.Text = ( (DateTime)R1P1ReagInfo.ExpirationDate ).ToShortDateString();  // 有効期限
                        this.btnExpirationDate.Tag = R1P1ReagInfo.ExpirationDate.ToString();
                    }
                    this.numRReagSerialNo.Value = R1P1ReagInfo.SerialNo;            // シリアル番号
                    this.cmbRReagCapacity.Value = R1P1ReagInfo.Capacity;            // 容量
                    this.optRReagRemainSelect.Value = false;                        // 残量選択
                    this.numR1ReagRemainVolume.Value = R1P1ReagInfo.Remain;         // R1試薬またはP1前処理液の残量設定
                }
                else
                {
                    R1P1ReagInfo = null;
                }
            }

            // R2試薬リスト取得
            reagentList = Singleton<ReagentDB>.Instance.GetData(ReagentKind.Reagent, moduleId).Where<ReagentData>(x => (x.PortNo == ((portNo * 3) - 1)));
            if (reagentList != null && reagentList.Count() > 0)
            {
                R2P2ReagInfo = reagentList.ToList<ReagentData>().First();
                if (R2P2ReagInfo.ReagentCode != 0 && ( R2P2ReagInfo.ReagentType != (int)ReagentType.M ))
                {
                    // 共通部の設定
                    if (MReagInfo == null && R1P1ReagInfo == null)
                    {
                        this.numReagentCode.Value = R2P2ReagInfo.ReagentCode;       // 試薬コード
                        this.cmbMakerCode.Value = R2P2ReagInfo.MakerCode;           // メーカーコード
                        this.txtLotNo.Text = R2P2ReagInfo.LotNo;                    // ロット番号
                        this.btnExpirationDate.Text = ( (DateTime)R2P2ReagInfo.ExpirationDate ).ToShortDateString();  // 有効期限
                        this.btnExpirationDate.Tag = R2P2ReagInfo.ExpirationDate.ToString();
                    }

                    if (R1P1ReagInfo == null)
                    {
                        this.numRReagSerialNo.Value = R2P2ReagInfo.SerialNo;        // シリアル番号
                        this.cmbRReagCapacity.Value = R2P2ReagInfo.Capacity;        // 容量
                        this.optRReagRemainSelect.Value = false;                    // 残量選択

                        if (R2P2ReagInfo.ReagentType == (int)ReagentType.R1R2)
                        {
                            // R1/R2試薬
                            this.chkPreprocessLiquid.Checked = false;
                        }
                        else
                        {
                            // 前処理液1/ 前処理液2
                            this.chkPreprocessLiquid.Checked = true;
                        }
                    }

                    this.numR2ReagRemainVolume.Value = R2P2ReagInfo.Remain;         // R2試薬またはP2前処理液の残量設定
                }
                else
                {
                    R2P2ReagInfo = null;
                }
            }

            // 試薬コードが読み取れなかった場合
            if (MReagInfo == null && R1P1ReagInfo == null && R2P2ReagInfo == null)
            {
                this.numReagentCode.Value = null;                  // 試薬コード
                this.cmbMakerCode.SelectedIndex = 0;               // メーカーコード
                this.txtLotNo.Text = String.Empty;                 // ロット番号
                this.btnExpirationDate.Text = DateTime.Today.ToShortDateString();  // 有効期限
                this.btnExpirationDate.Tag = DateTime.Today.ToString();
            }

            // 表示更新停止解除
            this.isUpdateEnableControl = true;
        }

        /// <summary>
        /// コントロール有効無効更新
        /// </summary>
        private void updateEnableControls()
        {
            // 表示更新の停止チェック
            if (this.isUpdateEnableControl == false)
            {
                return;
            }

            // ポート番号および試薬コードがnull
            if (this.numPortNo.Value == null || this.numReagentCode.Value == null)
            {
                // 共通部以外は無効
                this.gbxMReagentBCInfo.Visible = false;
                this.gbxRReagentBCInfo.Visible = false;
                this.chkPreprocessLiquid.Visible = false;
                this.btnOk.Enabled = false;
                return;
            }

            // ポート番号および試薬コードが認識できない値
            int portNo = 0;
            Int32.TryParse( this.numPortNo.Value.ToString(), out portNo );
            int reagCode = 0;
            Int32.TryParse( this.numReagentCode.Value.ToString(), out reagCode );
            MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo( reagCode );
            if (portNo == 0 || reagCode == 0 || protocol == null)
            {
                // 共通部以外は無効
                this.gbxMReagentBCInfo.Visible = false;
                this.gbxRReagentBCInfo.Visible = false;
                this.chkPreprocessLiquid.Visible = false;
                this.btnOk.Enabled = false;
                return;
            }

            // ポート番号および試薬コードが認識できた
            this.btnOk.Enabled = true;
            this.gbxMReagentBCInfo.Visible = true;
            this.gbxRReagentBCInfo.Visible = true;

            // 分析項目設定の前処理液シーケンスの有無
            if (protocol.PreProcessSequence == MeasureProtocol.PreProcessSequenceKind.None)
            {
                this.chkPreprocessLiquid.Visible = false;

                // R1/R2試薬表記に変更
                this.gbxMReagentBCInfo.Visible = true;
                this.gbxRReagentBCInfo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_004;
                this.lblR1ReagRemainVolume.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_018;
                this.lblR2ReagRemainVolume.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_019;
            }
            else
            {
                this.chkPreprocessLiquid.Visible = true;

                // 前処理液有無のチェックが付いている場合
                if (this.chkPreprocessLiquid.Checked == true)
                {
                    // P1/P2前処理液表記に変更
                    this.gbxMReagentBCInfo.Visible = false;
                    this.gbxRReagentBCInfo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_005;
                    this.lblR1ReagRemainVolume.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_020;
                    this.lblR2ReagRemainVolume.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_021;
                }
                else
                {
                    // R1/R2試薬表記に変更
                    this.gbxMReagentBCInfo.Visible = true;
                    this.gbxRReagentBCInfo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_004;
                    this.lblR1ReagRemainVolume.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_018;
                    this.lblR2ReagRemainVolume.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONINPUTREAGENTBC_019;
                }
            }

            // R試薬の残量選択状態取得
            Boolean isSelected = (Boolean)this.optRReagRemainSelect.Value;
            if (isSelected == true)
            {
                this.numR1ReagRemainVolume.Enabled = true;
                this.numR2ReagRemainVolume.Enabled = true;
            }
            else
            {
                this.numR1ReagRemainVolume.Enabled = false;
                this.numR2ReagRemainVolume.Enabled = false;
            }

            // M試薬の残量選択状態取得
            isSelected = (Boolean)this.optMReagRemainSelect.Value;
            if (isSelected == true)
            {
                this.numMReagRemainVolume.Enabled = true;
            }
            else
            {
                this.numMReagRemainVolume.Enabled = false;
            }
        }

        /// <summary>
        /// M試薬残量情報を取得
        /// </summary>
        /// <returns></returns>
        private ReagentRemainTable getMReagentRemainTable( MeasureProtocol protocol, Boolean isPreprocessLiquid )
        {
            // 試薬残量情報インスタンス生成
            ReagentRemainTable setReagentRemain = new ReagentRemainTable();

            // 前処理液の有無チェック
            if (isPreprocessLiquid == false)
            {
                // 試薬種別
                setReagentRemain.ReagType = (int)ReagentType.M;

                // 試薬種別詳細
                setReagentRemain.ReagTypeDetail = ReagentTypeDetail.M;

                // メーカーコード
                setReagentRemain.MakerCode = this.cmbMakerCode.Text;

                // 試薬コード
                Int32.TryParse( this.numReagentCode.Text, out setReagentRemain.ReagCode );

                // ボトル容量
                Int32.TryParse( this.cmbMReagCapacity.Text, out setReagentRemain.Capacity );

                // シリアル番号
                Int32.TryParse( this.numMReagSerialNo.Text, out setReagentRemain.RemainingAmount.SerialNumber );

                // ロット番号
                setReagentRemain.RemainingAmount.LotNumber = this.txtLotNo.Text;

                // 有効期限
                DateTime.TryParse( this.btnExpirationDate.Tag.ToString(), out setReagentRemain.RemainingAmount.TermOfUse );

                // 設置日(暫定対応)
                DateTime.TryParse( DateTime.Now.ToString(), out setReagentRemain.RemainingAmount.InstallationData);

                // 残量
                if ((Boolean)this.optMReagRemainSelect.Value == false)
                {
                    // 管理されている残量から設定 => 履歴からの情報取得
                    // ※履歴に存在しない場合はボトル容量を設定
                    int historyRemain = setReagentRemain.Capacity * protocol.MReagDispenseVolume;

                    // 最新の試薬履歴取得
                    List<ReagentHistoryData> reagentHistoryDataList = Singleton<ReagentHistoryDB>.Instance.GetData();
                    if (reagentHistoryDataList != null && reagentHistoryDataList.Count > 0)
                    {
                        // 履歴情報検索
                        var reagentHistoryData = reagentHistoryDataList.Where<ReagentHistoryData>( v => ( v.ReagentCode == setReagentRemain.ReagCode )
                                                                                                        && ( v.ReagentTypeDetail == (int)setReagentRemain.ReagTypeDetail )
                                                                                                        && ( v.SerialNo == setReagentRemain.RemainingAmount.SerialNumber )
                                                                                                        && ( v.LotNo == setReagentRemain.RemainingAmount.LotNumber ) );
                        if (reagentHistoryData != null && reagentHistoryData.Count() > 0)
                        {
                            // 履歴情報にある残量を取得
                            historyRemain = ( (ReagentHistoryData)reagentHistoryData.First() ).Remain;
                        }
                    }

                    // 管理されている残量を設定
                    setReagentRemain.RemainingAmount.Remain = historyRemain;
                }
                else
                {
                    // 管理されている残量（履歴）以外の設定の場合 => 入力された残量を設定
                    Int32 remainVolume = 0;
                    Int32.TryParse( this.numMReagRemainVolume.Text, out remainVolume );
                    setReagentRemain.RemainingAmount.Remain = remainVolume;
                }
            }

            return setReagentRemain;
        }

        /// <summary>
        /// R1試薬/P1前処理液残量情報を取得
        /// </summary>
        /// <returns></returns>
        private ReagentRemainTable getR1P1ReagentRemainTable( MeasureProtocol protocol, Boolean isPreprocessLiquid )
        {
            // 試薬残量情報インスタンス生成
            ReagentRemainTable setReagentRemain = new ReagentRemainTable();

            // 試薬種別と試薬種別詳細
            if (isPreprocessLiquid == false)
            {
                // R1試薬
                setReagentRemain.ReagType = (int)ReagentType.R1R2;
                setReagentRemain.ReagTypeDetail = ReagentTypeDetail.R1;
            }
            else
            {
                // P1前処理液
                setReagentRemain.ReagType = (int)ReagentType.T1T2;
                setReagentRemain.ReagTypeDetail = ReagentTypeDetail.T1;
            }

            // メーカーコード
            setReagentRemain.MakerCode = this.cmbMakerCode.Text;

            // 試薬コード
            Int32.TryParse( this.numReagentCode.Text, out setReagentRemain.ReagCode );

            // ボトル容量
            Int32.TryParse( this.cmbRReagCapacity.Text, out setReagentRemain.Capacity );

            // シリアル番号
            Int32.TryParse( this.numRReagSerialNo.Text, out setReagentRemain.RemainingAmount.SerialNumber );

            // ロット番号
            setReagentRemain.RemainingAmount.LotNumber = this.txtLotNo.Text;

            // 有効期限
            DateTime.TryParse( this.btnExpirationDate.Tag.ToString(), out setReagentRemain.RemainingAmount.TermOfUse );

            // 設置日(暫定対応)
            DateTime.TryParse(DateTime.Now.ToString(), out setReagentRemain.RemainingAmount.InstallationData);

            // 残量
            if ((Boolean)this.optRReagRemainSelect.Value == false)
            {
                // 管理されている残量から設定 => 履歴からの情報取得
                // ※履歴に存在しない場合はボトル容量を設定
                int historyRemain = setReagentRemain.Capacity * protocol.R1DispenseVolume;
                if (isPreprocessLiquid == true)
                {
                    // P1前処理液分注量で再計算
                    historyRemain = setReagentRemain.Capacity * protocol.PreProsess1DispenseVolume;
                }

                // 最新の試薬履歴取得
                List<ReagentHistoryData> reagentHistoryDataList = Singleton<ReagentHistoryDB>.Instance.GetData();
                if (reagentHistoryDataList != null && reagentHistoryDataList.Count > 0)
                {
                    // 履歴情報検索
                    var reagentHistoryData = reagentHistoryDataList.Where<ReagentHistoryData>( v => ( v.ReagentCode == setReagentRemain.ReagCode )
                                                                                                 && ( v.ReagentTypeDetail == (int)setReagentRemain.ReagTypeDetail )
                                                                                                 && ( v.SerialNo == setReagentRemain.RemainingAmount.SerialNumber )
                                                                                                 && ( v.LotNo == setReagentRemain.RemainingAmount.LotNumber ) );
                    if (reagentHistoryData != null && reagentHistoryData.Count() > 0)
                    {
                        // 履歴情報にある残量を取得
                        historyRemain = ( (ReagentHistoryData)reagentHistoryData.First() ).Remain;
                    }
                }

                // 管理されている残量を設定
                setReagentRemain.RemainingAmount.Remain = historyRemain;
            }
            else
            {
                // 管理されている残量（履歴）以外の設定の場合 => 入力された残量を設定
                Int32 remainVolume = 0;
                Int32.TryParse( this.numR1ReagRemainVolume.Text, out remainVolume );
                setReagentRemain.RemainingAmount.Remain = remainVolume;
            }

            return setReagentRemain;
        }

        /// <summary>
        /// R2試薬/P2前処理液残量情報を取得
        /// </summary>
        /// <returns></returns>
        private ReagentRemainTable getR2P2ReagentRemainTable( MeasureProtocol protocol, Boolean isPreprocessLiquid )
        {
            // 試薬残量情報インスタンス生成
            ReagentRemainTable setReagentRemain = new ReagentRemainTable();

            // 試薬種別と試薬種別詳細
            if (isPreprocessLiquid == false)
            {
                // R2
                setReagentRemain.ReagType = (int)ReagentType.R1R2;
                setReagentRemain.ReagTypeDetail = ReagentTypeDetail.R2;
            }
            else
            {
                // P2
                setReagentRemain.ReagType = (int)ReagentType.T1T2;
                setReagentRemain.ReagTypeDetail = ReagentTypeDetail.T2;
            }
            // メーカーコード
            setReagentRemain.MakerCode = this.cmbMakerCode.Text;

            // 試薬コード
            Int32.TryParse( this.numReagentCode.Text, out setReagentRemain.ReagCode );

            // ボトル容量
            Int32.TryParse( this.cmbRReagCapacity.Text, out setReagentRemain.Capacity );

            // シリアル番号
            Int32.TryParse( this.numRReagSerialNo.Text, out setReagentRemain.RemainingAmount.SerialNumber );
            
            // ロット番号
            setReagentRemain.RemainingAmount.LotNumber = this.txtLotNo.Text;

            // 有効期限
            DateTime.TryParse( this.btnExpirationDate.Tag.ToString(), out setReagentRemain.RemainingAmount.TermOfUse );

            // 設置日(暫定対応)
            DateTime.TryParse( DateTime.Now.ToString(), out setReagentRemain.RemainingAmount.InstallationData);

            // 残量
            if ((Boolean)this.optRReagRemainSelect.Value == false)
            {
                // 管理されている残量から設定 => 履歴からの情報取得
                // ※履歴に存在しない場合はボトル容量を設定
                int historyRemain = setReagentRemain.Capacity * protocol.R2DispenseVolume;
                if (isPreprocessLiquid == true)
                {
                    // P2前処理液分注量で再計算
                    historyRemain = setReagentRemain.Capacity * protocol.PreProsess2DispenseVolume;
                }

                // 最新の試薬履歴取得
                List<ReagentHistoryData> reagentHistoryDataList = Singleton<ReagentHistoryDB>.Instance.GetData();
                if (reagentHistoryDataList != null && reagentHistoryDataList.Count > 0)
                {
                    // 履歴情報検索
                    var reagentHistoryData = reagentHistoryDataList.Where<ReagentHistoryData>( v => ( v.ReagentCode == setReagentRemain.ReagCode )
                                                                                                 && ( v.ReagentTypeDetail == (int)setReagentRemain.ReagTypeDetail )
                                                                                                 && ( v.SerialNo == setReagentRemain.RemainingAmount.SerialNumber )
                                                                                                 && ( v.LotNo == setReagentRemain.RemainingAmount.LotNumber ) );
                    if (reagentHistoryData != null && reagentHistoryData.Count() > 0)
                    {
                        // 履歴情報にある残量を取得
                        historyRemain = ( (ReagentHistoryData)reagentHistoryData.First() ).Remain;
                    }
                }

                // 管理されている残量を設定
                setReagentRemain.RemainingAmount.Remain = historyRemain;
            }
            else
            {
                // 管理されている残量（履歴）以外の設定の場合 => 入力された残量を設定
                Int32 remainVolume = 0;
                Int32.TryParse( this.numR2ReagRemainVolume.Text, out remainVolume );
                setReagentRemain.RemainingAmount.Remain = remainVolume;
            }

            return setReagentRemain;
        }

        #endregion
    }
}
