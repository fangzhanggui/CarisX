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
using Oelco.CarisX.DB;
using Oelco.CarisX.Const;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 消耗品詳細表示ダイアログ
    /// </summary>
    public partial class DlgConsumableDetailed : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgConsumableDetailed()
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
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            // ダイアログタイトル
            this.Caption = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_000;

            // グループボックスタイトル
            this.gbxPretrigger1.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_001;
            this.gbxPretrigger2.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_002;
            this.gbxTrigger1.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_003;
            this.gbxTrigger2.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_004;
            this.gbxDiluent.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_005;
            this.gbxSamplingTip.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_006;

            // 各種詳細項目名
            // プレトリガ1
            this.lblTitlePretrigger1LotNo.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_008;
            this.lblTitlePretrigger1Tests.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_009;
            this.lblTitlePretrigger1ExpirationDate.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_010;
            // プレトリガ2
            this.lblTitlePretrigger2LotNo.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_008;
            this.lblTitlePretrigger2Tests.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_009;
            this.lblTitlePretrigger2ExpirationDate.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_010;
            // トリガ1
            this.lblTitleTrigger1LotNo.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_008;
            this.lblTitleTrigger1Tests.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_009;
            this.lblTitleTrigger1ExpirationDate.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_010;
            // トリガ2
            this.lblTitleTrigger2LotNo.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_008;
            this.lblTitleTrigger2Tests.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_009;
            this.lblTitleTrigger2ExpirationDate.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_010;
            // 希釈液
            this.lblTitleDiluentLotNo.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_008;
            this.lblTitleDiluentTests.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_032;
            this.lblTitleDiluentExpirationDate.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_010;
            // 分注チップ
            this.lblTitleSamplingTipAllTests.Text = Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_011;
            this.lblTitleSamplingTipCase1.Text= Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_012;
            this.lblTitleSamplingTipCase2.Text= Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_013;
            this.lblTitleSamplingTipCase3.Text= Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_014;
            this.lblTitleSamplingTipCase4.Text= Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_015;
            this.lblTitleSamplingTipCase5.Text= Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_016;
            this.lblTitleSamplingTipCase6.Text= Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_017;
            this.lblTitleSamplingTipCase7.Text= Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_018;
            this.lblTitleSamplingTipCase8.Text= Properties.Resources.STRING_DLG_CONSUMABLEDETAILED_019;

            // 閉じるボタン
            this.btnClose.Text = Properties.Resources.STRING_COMMON_002;
        }

        #endregion

        #region [privateメソッド]
        
        /// <summary>
        /// フォーム読み込みイベント
        /// </summary>
        /// <remarks>
        /// 試薬種別、設置ポート番号対応フォームの読み込みを行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void DlgConsumableDetailed_Load( object sender, EventArgs e )
        {
            int selectedModuleNo = (int)Singleton<PublicMemory>.Instance.moduleIndex + 1;
            var reagentData = Singleton<ReagentDB>.Instance.GetData( null, selectedModuleNo );

            reagentData.ForEach( ( data ) =>
            {
                switch ( (ReagentKind)data.ReagentKind )
                {
                case ReagentKind.Pretrigger:
                    switch ( data.PortNo )
                    {
                    case 1:
                        this.lblPretrigger1LotNo.Text = ( String.IsNullOrEmpty( data.LotNo ) ) ? Properties.Resources.STRING_COMMON_000 : data.LotNo;
                        this.lblPretrigger1Tests.Text = CarisXSubFunction.GetDispRemainCount( (ReagentKind)data.ReagentKind, data.Remain ).ToString();
						this.lblPretrigger1ExpirationDate.Text = ( data.ExpirationDate.HasValue ) ? data.ExpirationDate.Value.ToShortDateString() : Properties.Resources.STRING_COMMON_000;
                        break;
                    case 2:
                        this.lblPretrigger2LotNo.Text = ( String.IsNullOrEmpty( data.LotNo ) ) ? Properties.Resources.STRING_COMMON_000 : data.LotNo;
                        this.lblPretrigger2Tests.Text = CarisXSubFunction.GetDispRemainCount( (ReagentKind)data.ReagentKind, data.Remain ).ToString();

						this.lblPretrigger2ExpirationDate.Text = ( data.ExpirationDate.HasValue ) ? data.ExpirationDate.Value.ToShortDateString() : Properties.Resources.STRING_COMMON_000;
                        break;
                    }
                    break;
                case ReagentKind.Trigger:
                    switch ( data.PortNo )
                    {
                    case 1:
                        this.lblTrigger1LotNo.Text = ( String.IsNullOrEmpty( data.LotNo ) ) ? Properties.Resources.STRING_COMMON_000 : data.LotNo;
                        this.lblTrigger1Tests.Text = CarisXSubFunction.GetDispRemainCount( (ReagentKind)data.ReagentKind, data.Remain ).ToString();
						this.lblTrigger1ExpirationDate.Text = ( data.ExpirationDate.HasValue ) ? data.ExpirationDate.Value.ToShortDateString() : Properties.Resources.STRING_COMMON_000;
                        break;
                    case 2:
                        this.lblTrigger2LotNo.Text = ( String.IsNullOrEmpty( data.LotNo ) ) ? Properties.Resources.STRING_COMMON_000 : data.LotNo;
                        this.lblTrigger2Tests.Text = CarisXSubFunction.GetDispRemainCount( (ReagentKind)data.ReagentKind, data.Remain ).ToString();
						this.lblTrigger2ExpirationDate.Text = ( data.ExpirationDate.HasValue ) ? data.ExpirationDate.Value.ToShortDateString() : Properties.Resources.STRING_COMMON_000;
                        break;
                    }
                    break;
                case ReagentKind.Diluent:
                    this.lblDiluentLotNo.Text = ( String.IsNullOrEmpty( data.LotNo ) ) ? Properties.Resources.STRING_COMMON_000 : data.LotNo;
                    this.lblDiluentTests.Text = CarisXSubFunction.GetDispRemainCount( (ReagentKind)data.ReagentKind, data.Remain ).ToString();
					this.lblDiluentExpirationDate.Text = ( data.ExpirationDate.HasValue ) ? data.ExpirationDate.Value.ToShortDateString() : Properties.Resources.STRING_COMMON_000;
                    break;
                case ReagentKind.SamplingTip:
                    switch ( data.PortNo )
                    {
                    case 1:
                        this.lblTipCase1.Text = data.Remain.ToString();
                        break;
                    case 2:
                        this.lblTipCase2.Text = data.Remain.ToString();
                        break;
                    case 3:
                        this.lblTipCase3.Text = data.Remain.ToString();
                        break;
                    case 4:
                        this.lblTipCase4.Text = data.Remain.ToString();
                        break;
                    case 5:
                        this.lblTipCase5.Text = data.Remain.ToString();
                        break;
                    case 6:
                        this.lblTipCase6.Text = data.Remain.ToString();
                        break;
                    case 7:
                        this.lblTipCase7.Text = data.Remain.ToString();
                        break;
                    case 8:
                        this.lblTipCase8.Text = data.Remain.ToString();
                        break;
                    }
                    this.lblSamplingTipAllTests.Tag = ((int?)this.lblSamplingTipAllTests.Tag ?? 0) + data.Remain;
                    this.lblSamplingTipAllTests.Text = ( (int)this.lblSamplingTipAllTests.Tag ).ToString();
                    break;
                }
            } );
        }

        /// <summary>
        /// ダイアログ閉じるボタン
        /// </summary>
        /// <remarks>
        /// フォームの終了を行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnClose_Click( object sender, EventArgs e )
        {
            this.Close();
        }

        #endregion
    }
}
