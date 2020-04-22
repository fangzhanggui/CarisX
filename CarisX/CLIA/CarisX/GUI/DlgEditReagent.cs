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
using Oelco.CarisX.Comm;
using Oelco.CarisX.Const;
using Oelco.CarisX.DB;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Parameter;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 試薬編集ダイアログクラス
    /// </summary>
    public partial class DlgEditReagent : DlgCarisXBase
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 試薬種別
        /// </summary>
        private ReagentKind reagentKind;

        /// <summary>
        /// 設置ポート番号
        /// </summary>
        private Int32 portNo = 1; // 希釈液などは1固定(設置ポート番号の指定しない)にする

        /// <summary>
        /// 容量
        /// </summary>
        private Int32 capacity = 1;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgEditReagent()
        {
            InitializeComponent();
        }
    
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬種別の取得、設定
        /// </summary>
        public ReagentKind Kind
        {
            get
            {
                return this.reagentKind;
            }
            set
            {
                this.reagentKind = value;
            }
        }

        /// <summary>
        /// 設置ポート番号の取得、設定
        /// </summary>
        public Int32 PortNo
        {
            get
            {
                return this.portNo;
            }
            set
            {
                this.portNo = value;
                this.lblNo.Text = this.portNo.ToString();
            }
        }

        /// <summary>
        /// 残量の取得、設定
        /// </summary>
        public Int32 Remain
        {
            get
            {
                return (Int32)this.numRemainTest.Value;
            }
            set
            {
                this.numRemainTest.Value = value;
            }
        }

        /// <summary>
        /// ロット番号の取得、設定
        /// </summary>
        public String LotNumber
        {
            get
            {
                return this.txtLotNo.Text;
            }
            set
            {
                this.txtLotNo.Text = value;
            }
        }

        /// <summary>
        /// シリアル番号の取得、設定
        /// </summary>
        public Int32 SerialNumber
        {
            get
            {
                return (Int32)this.numSerialNo.Value;
            }
            set
            {
                this.numSerialNo.Value = value;
            }
        }

        /// <summary>
        /// 容量の取得、設定
        /// </summary>
        public Int32 Capacity
        {
            get
            {
                return this.capacity;
            }
            set
            {
                this.capacity = value;
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
            switch (this.reagentKind)
            {
            // プレトリガ・トリガ
            case ReagentKind.Pretrigger:
            case ReagentKind.Trigger:
                this.lblRemainTestUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDIT_REAGENT_LBL_test;// 単位のラベル設定
                // プレトリガ・トリガの場合は容量1：0～200mL、容量2：0～300mL ⇒ テスト数に変換
                this.numRemainTest.MinValue = CarisXSubFunction.GetDispRemainCount(this.reagentKind, CarisXConst.REMAIN_PRETRIGGER_TRIGGER_MIN);
                if (this.capacity == 1)
                {
                    this.numRemainTest.MaxValue = CarisXSubFunction.GetDispRemainCount(this.reagentKind, CarisXConst.REMAIN_PRETRIGGER_TRIGGER_MAX_1);
                }
                else
                {
                    this.numRemainTest.MaxValue = CarisXSubFunction.GetDispRemainCount(this.reagentKind, CarisXConst.REMAIN_PRETRIGGER_TRIGGER_MAX_2);
                }

                //トリガ、プレトリガ-Assay中は変更できないようにする
                if (Singleton<Oelco.CarisX.Status.SystemStatus>.Instance.Status == Status.SystemStatusKind.Assay)
                {
                    this.btnOk.Enabled = false;
                }
                else
                {
                    this.btnOk.Enabled = true;
                }
                break;

            // 希釈液
            case ReagentKind.Diluent:
                // 希釈液の場合はNo表示しない
                this.lblTitleNo.Visible = false;
                this.lblNo.Visible = false;
                this.lblRemainTestUnit.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDIT_REAGENT_LBL_ml;// 単位のラベル設定
                // 希釈液の場合は容量1：0～200mL、容量2：0～300mL
                this.numRemainTest.MinValue = CarisXConst.REMAIN_DILUENT_MIN;
                if (this.capacity == 1)
                {
                    this.numRemainTest.MaxValue = CarisXConst.REMAIN_DILUENT_MAX_1 / 1000;//(μL⇒mL)
                }
                else
                {
                    this.numRemainTest.MaxValue = CarisXConst.REMAIN_DILUENT_MAX_2 / 1000;//(μL⇒mL)
                }
                break;
            
            // 分注チップ、反応容器
            case ReagentKind.SamplingTip:
            case ReagentKind.Cell:
                // 分注チップ、反応容器の場合はLotNo、SerialNoは表示しない
                this.lblTitleLotNo.Visible = false;
                this.txtLotNo.Visible = false;
                this.lblTitleSerialNo.Visible = false;
                this.numSerialNo.Visible = false;
                this.lblRemainTestUnit.Text = String.Empty;// 単位のラベル設定
                // 分注チップ、反応容器の場合は0～96(97以上入らないようにする）
                this.numRemainTest.MinValue = CarisXConst.REMAIN_SAMPLINGTIP_CELL_MIN;
                this.numRemainTest.MaxValue = CarisXConst.REMAIN_SAMPLINGTIP_CELL_MAX;
                break;
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
            // タイトル
            if (this.Caption == String.Empty)
            {
                this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_EDITREAGENT_000;
            }

            // パネル既定ボタン
            this.btnOk.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;

            // 項目タイトル
            this.lblTitleNo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDIT_REAGENT_LBL_001;
            this.lblTitleRemainTest.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDIT_REAGENT_LBL_002;
            this.lblTitleLotNo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDIT_REAGENT_LBL_003;
            this.lblTitleSerialNo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_EDIT_REAGENT_LBL_004;
        }

        #endregion

        #region [privateメソッド]
        /// <summary>
        /// ロット番号キーダウンイベント
        /// </summary>
        /// <remarks>
        /// 数字以外入力できないようにする
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLotNo_KeyDown(object sender, KeyEventArgs e)
        {
            bool keyPressCk = true;

            if ((!e.Shift && '0' <= e.KeyValue && e.KeyValue <= '9')                    // 0～9 (キーボード)　※記号が入らないようにShiftキーが押されていない場合のみ許可
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
            this.Close();
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

        #endregion
    }
}
