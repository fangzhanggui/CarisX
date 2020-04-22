using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.CarisX.Const;
using Oelco.CarisX.Properties;
using Oelco.CarisX.Log;
using Oelco.Common.Utility;
using Oelco.Common.Log;
using Oelco.CarisX.Utility;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// STAT検査依頼問合せダイアログクラス
    /// </summary>
    public partial class DlgAskWorkSheatSTAT : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgAskWorkSheatSTAT()
        {
            InitializeComponent();
        }
    
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 検体IDの取得、設定
        /// </summary>
        public String SampleID
        {
            get
            {
                return (String)this.txtSampleID.Value;
            }
            set
            {
                this.txtSampleID.Value = value;
            }
        }
        /// <summary>
        /// カップ種別の取得、設定
        /// </summary>
        public SpecimenCupKind CupKind
        {
            get
            {
                return (SpecimenCupKind)this.cmbCupKind.Value;
            }
            set
            {
                this.cmbCupKind.Value = value;
            }
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
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.Caption = Resources.STRING_DLG_ASKWORKSHEATSTAT_000;            
            this.lblSubTitle.Text = Resources.STRING_DLG_ASKWORKSHEATSTAT_001;

            lblSampleID.Text = Resources.STRING_DLG_ASKWORKSHEATSTAT_002;
            lblNotes.Text = Resources.STRING_DLG_ASKWORKSHEATSTAT_007;

            lblCupKind.Text = Resources.STRING_DLG_ASKWORKSHEATSTAT_003;
            cmbCupKind.Items.Clear();
            cmbCupKind.Items.Add(SpecimenCupKind.Cup, Resources.STRING_DLG_ASKWORKSHEATSTAT_004);
            cmbCupKind.Items.Add(SpecimenCupKind.Tube, Resources.STRING_DLG_ASKWORKSHEATSTAT_005);
            cmbCupKind.Items.Add(SpecimenCupKind.CupOnTube, Resources.STRING_DLG_ASKWORKSHEATSTAT_006);
            cmbCupKind.SelectedIndex = 0;

            this.btnOk.Text =  Resources.STRING_COMMON_001;
            this.btnCancel.Text = Resources.STRING_COMMON_003;
        }

        #endregion

        /// <summary>
        /// OKボタン押下イベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にOKを設定して、画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOk_Click( object sender, EventArgs e )
        {
            if (checkInputContent())
            {
                //入力内容のチェックがOKの場合
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// キャンセルボタン押下イベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して、画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 入力内容のチェック
        /// </summary>
        /// <remarks>
        /// 入力内容のチェックを行います
        /// </remarks>
        private Boolean checkInputContent()
        {
            try
            {
                // 未入力チェック
                if (txtSampleID.Text.Trim() == String.Empty)
                {
                    //入力内容がエラーです
                    DlgMessage.Show(Resources.STRING_DLG_MSG_159, String.Empty, Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty
                    , ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// サンプルID入力テキスト入力変更変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSampleID_TectChanged( object sender, EventArgs e )
        {
            // カーソル位置を記憶
            Int32 selset = txtSampleID.SelectionStart;

            String formatCheck = CarisXSubFunction.IsValidForPatientID(txtSampleID.Text);

            // 検体IDに使用できない文字があった場合
            Boolean formatError = formatCheck.Equals(txtSampleID.Text);

            if (!formatError)
            {
                // サンプルID入力テキストの検体IDをチェック後の検体IDに変更
                txtSampleID.Text = formatCheck;

                // カーソル位置を変更
                txtSampleID.SelectionStart = selset - 1;
            }

        }
    }
}
