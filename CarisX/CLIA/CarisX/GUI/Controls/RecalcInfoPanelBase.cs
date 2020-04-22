using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.CarisX.Parameter;
using Oelco.Common.Utility;
using Oelco.CarisX.Utility;
using Oelco.CarisX.DB;

namespace Oelco.CarisX.GUI.Controls
{
    /// <summary>
    /// 再計算パネル
    /// </summary>
    public partial class RecalcInfoPanelBase : UserControl, IRecalcInfo, IRecalcRefiner
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// OKボタンクリックイベントハンドラ
        /// </summary>
        public event EventHandler OkClick;

        /// <summary>
        /// クローズボタンクリックイベントハンドラ
        /// </summary>
        public event EventHandler CloseClick;

        /// <summary>
        /// 絞り込みラックID(from)
        /// </summary>
        private CarisXIDString fromRackId;

        /// <summary>
        /// 絞り込みラックID(to)
        /// </summary>
        private CarisXIDString toRackId;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RecalcInfoPanelBase()
        {
            InitializeComponent();

            this.lblSelectionCalibrationCurve.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELBASE_000;
            this.lblFilterCondition.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELBASE_001;
            this.lblRemark.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELBASE_002;
            this.btnSelectAnalyte.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELBASE_003;
            this.btnSelectAllAnalyte.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELBASE_004;
            this.lblTitleAnalytes.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELBASE_005;
            this.lblTitleReagentLotNo.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELBASE_006;
            this.lblTitleCalibrationCurve.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELBASE_007;
            this.chkRackId.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELBASE_008;
            this.chkSequenceNo.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELBASE_009;
            this.chkMeasuringTime.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELBASE_010;
            this.chkRemarkCalibrationError.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELBASE_011;
            this.chkRemarkExpirationDataError.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELBASE_012;
            this.lblHyphen1.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
            this.lblHyphen2.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
            this.lblHyphen3.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
            this.btnOk.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;

            // 測定日時(開始日、終了日)の初期化
            this.btnMeasuringTimeFrom.Text = DateTime.Today.ToShortDateString();
            this.btnMeasuringTimeFrom.Tag = DateTime.Today;
            this.btnMeasuringTimeTo.Text = DateTime.Today.ToShortDateString();
            this.btnMeasuringTimeTo.Tag = DateTime.Today.Add( TimeSpan.FromDays( 1 ) - TimeSpan.FromSeconds( 1 ) ); // 23:59:59

            this.chkRemarkCalibrationError.Tag = Remark.RemarkBit.CalibError;
            this.chkRemarkExpirationDataError.Tag =
                Remark.RemarkBit.CalibExpirationDateError |
                Remark.RemarkBit.DilutionExpirationDateError |
                Remark.RemarkBit.PreTriggerExpirationDateError |
                Remark.RemarkBit.TriggerExpirationDateError |
                Remark.RemarkBit.ReagentExpirationDateError;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 選択中のラックIDの取得、設定
        /// </summary>
        [BrowsableAttribute( false )]
        Tuple<Boolean, CarisXIDString, CarisXIDString> IRecalcRefiner.RackIdSelect
        {
            get
            {
                if ( this.fromRackId == null || this.fromRackId.DispValueString != this.numRackIdFrom.Text )
                {
                    this.fromRackId = (CarisXIDString)( this.lblRackIdPrefix1.Text + this.numRackIdFrom.Text );
                }
                if ( this.toRackId == null || this.toRackId.DispValueString != this.numRackIdTo.Text )
                {
                    this.toRackId = (CarisXIDString)( this.lblRackIdPrefix2.Text + this.numRackIdTo.Text );
                }
                return new Tuple<Boolean, CarisXIDString, CarisXIDString>( this.chkRackId.Checked, this.fromRackId, this.toRackId );
            }
            set
            {
                this.chkRackId.Checked = value.Item1;
                this.numRackIdFrom.Value = value.Item2.Value;
                this.numRackIdTo.Value = value.Item3.Value;
            }
        }

        /// <summary>
        /// 選択中のシーケンス番号の取得、設定
        /// </summary>
        [BrowsableAttribute( false )]
        Tuple<Boolean, Int32, Int32> IRecalcRefiner.SequenceNoSelect
        {
            get
            {
                Int32 from = 0;
                Int32 to = 0;
                if ( this.chkSequenceNo.Checked & !( Int32.TryParse( this.numSequenceNoFrom.Text, out from ) & Int32.TryParse( this.numSequenceNoTo.Text, out to ) ) )
                {
                    // TODO:エラー
                }
                return new Tuple<Boolean, Int32, Int32>( this.chkSequenceNo.Checked, from, to );
            }
            set
            {
                this.chkSequenceNo.Checked = value.Item1;
                this.numSequenceNoFrom.Value = value.Item2;
                this.numSequenceNoTo.Value = value.Item3;
            }
        }

        /// <summary>
        /// 選択中の測定日付の取得、設定
        /// </summary>
        [BrowsableAttribute( false )]
        Tuple<Boolean, DateTime, DateTime> IRecalcRefiner.MeasuringTimeSelect
        {
            get
            {
                return new Tuple<Boolean, DateTime, DateTime>( this.chkMeasuringTime.Checked, (DateTime)( this.btnMeasuringTimeFrom.Tag ), (DateTime)( this.btnMeasuringTimeTo.Tag ) );
            }
            set
            {
                this.chkMeasuringTime.Checked = value.Item1;
                this.btnMeasuringTimeFrom.Tag = value.Item2;
                this.btnMeasuringTimeTo.Tag = value.Item3;
            }
        }

        /// <summary>
        /// 選択中のリマークの取得、設定
        /// </summary>
        [BrowsableAttribute( false )]
        Remark.RemarkCategory IRecalcRefiner.RemarkSelect
        {
            get
            {
                Remark.RemarkCategory category = 0;

                // 検量線エラー
                category |= ( this.chkRemarkCalibrationError.Checked ) ? Remark.RemarkCategory.CalibrationError : 0;

                // 有効期限エラー
                category |= ( this.chkRemarkExpirationDataError.Checked ) ? Remark.RemarkCategory.ExpirationDataError : 0;

                return category;
            }
            set
            {
                // 検量線エラー
                this.chkRemarkCalibrationError.Checked = ( value & Remark.RemarkCategory.CalibrationError ) != 0;

                // 有効期限エラー
                this.chkRemarkExpirationDataError.Checked = ( value & Remark.RemarkCategory.ExpirationDataError ) != 0;
            }
        }

        /// <summary>
        /// 選択中の分析項目の取得、設定
        /// </summary>
        [BrowsableAttribute( false )]
        List<Int32> IRecalcInfo.AnalyteSelect
        {
            get
            {
                if ( this.txtAnalytes.Tag == null )
                {
                    this.txtAnalytes.Tag = new List<MeasureProtocol>();
                }
                return ( (List<MeasureProtocol>)this.txtAnalytes.Tag).Select((protocol)=>protocol.ProtocolIndex).ToList();
            }
            set
            {
                var checkProtocol = value.All((valueIndex)=>Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList.Select(( protocol ) => protocol.ProtocolIndex).Contains(valueIndex));
                if ( checkProtocol )
                {
                    if ( value.Count > 1 )
                    {
                        this.txtAnalytes.Tag = Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList.Where( ( protocol ) => value.Contains( protocol.ProtocolIndex ) ).ToList();
                        this.txtAnalytes.Text = Properties.Resources.STRING_RECALCPANEL_001;
                    }
                    else if ( value.Count == 1 )
                    {
                        this.txtAnalytes.Tag = Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList.Where( ( protocol ) => value.Contains( protocol.ProtocolIndex ) ).ToList();
                        this.txtAnalytes.Text = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( value[0] ).ProtocolName;
                    }
                }
            }
        }

        /// <summary>
        /// 選択中の試薬ロット番号の取得、設定
        /// </summary>
        [BrowsableAttribute( false )]
        String IRecalcInfo.ReagentLotNoSelect
        {
            get
            {
                if ( this.cmbReagentLotNo.SelectedItem != null )
                {
                    return this.cmbReagentLotNo.SelectedItem.DisplayText;
                }
                return String.Empty;
            }
            set
            {
                this.cmbReagentLotNo.SelectedItem.DisplayText = value;
            }
        }

        /// <summary>
        /// 選択中の検量線の取得、設定
        /// </summary>
        [BrowsableAttribute( false )]
        DateTime IRecalcInfo.CalibrationCurveApprovalDate
        {
            get
            {
                if ( this.cmbCalibrationCurve.Value != null )
                {
                    return (DateTime)this.cmbCalibrationCurve.Value;
                }
                return DateTime.Today;
            }
            set
            {
                this.cmbCalibrationCurve.Value = value;
            }
        }

        /// <summary>
        /// 背景イメージの取得、設定
        /// </summary>
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        public new Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        /// <summary>
        /// 全分析項目を選択中かどうかを取得
        /// </summary>
        protected Boolean isSelectAllProtocol
        {
            get
            {
                return this.txtAnalytes.Text == Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANEL_000;
            }
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 分析項目、試薬ロット、検量線の選択が有効の場合OKボタンクリックイベントを実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOk_Click( object sender, EventArgs e )
        {
            if ( this.OkClick != null )
            {
                if ( this.isSelectAllProtocol )
                {
                    this.OkClick( sender, e );
                }
                else if ( ( (IRecalcInfo)this ).AnalyteSelect.Count > 0 )
                {
                    if ( !String.IsNullOrEmpty( ( (IRecalcInfo)this ).ReagentLotNoSelect ) )
                    {
                        if ( this.cmbCalibrationCurve.Value != null )
                        {
                            this.OkClick( sender, e );
                        }
                        else
                        {
                            // 検量線未選択
                            DlgMessage.Show( Oelco.CarisX.Properties.Resources.STRING_DLG_MSG_034, string.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.Confirm );
                        }
                    }
                    else
                    {
                        // 試薬ロット未選択
                        DlgMessage.Show( Oelco.CarisX.Properties.Resources.STRING_DLG_MSG_010, string.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.Confirm );
                    }
                }
                else
                {
                    // 分析項目未選択
                    DlgMessage.Show( Oelco.CarisX.Properties.Resources.STRING_DLG_MSG_105, string.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.Confirm );
                }
            }
        }

        /// <summary>
        /// 閉じるボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 閉じるボタンクリックイベントを実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnClose_Click( object sender, EventArgs e )
        {
            if ( this.CloseClick != null )
            {
                this.CloseClick( sender, e );
            }
        }

        /// <summary>
        /// 全て選択ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 全て選択ボタンクリックイベントを実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnSelectAllAnalyte_Click( object sender, EventArgs e )
        {
            this.cmbReagentLotNo.SelectedIndex = -1;
            this.cmbCalibrationCurve.SelectedIndex = -1;
            this.txtAnalytes.Tag = Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList;
            this.txtAnalytes.Text = CarisX.Properties.Resources.STRING_RECALCINFOPANEL_000;
        }

        /// <summary>
        /// 分析項目選択ボタン
        /// </summary>
        /// <remarks>
        /// 分析項目選択ダイアログを表示して分析項目を選択します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnSelectAnalyte_Click( object sender, EventArgs e )
        {
            // 既定の分析項目がある場合はダイアログで選択状態にする
            IEnumerable<Int32> protocolNoList = ( this.txtAnalytes.Tag as MeasureProtocol != null ) ? new[] { ( this.txtAnalytes.Tag as MeasureProtocol ).ProtocolNo } : null;

            using ( DlgProtocolSelect protocolSelect = new DlgProtocolSelect( false, 1, measureProtocolNo:protocolNoList ) )
            {
                if ( protocolSelect.ShowDialog() == DialogResult.OK && protocolSelect.SelectedProtocolIndexs.Count > 0)
                {
                    MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( protocolSelect.SelectedProtocolIndexs.First() );
                    this.cmbReagentLotNo.SelectedIndex = -1;
                    this.cmbCalibrationCurve.SelectedIndex = -1;
                    this.txtAnalytes.Tag = new List<MeasureProtocol>(){protocol};
                    this.txtAnalytes.Text = protocol.ProtocolName;
                }
            }
        }

        /// <summary>
        /// 分析項目文字列変更イベント
        /// </summary>
        /// <remarks>
        /// 分析項目文字列を変更します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void txtAnalytes_TextChanged( object sender, EventArgs e )
        {
            // 全分析項目選択
            this.btnOk.Enabled = this.isSelectAllProtocol;
            this.cmbReagentLotNo.Enabled = !this.isSelectAllProtocol;
            this.cmbCalibrationCurve.Enabled = !this.isSelectAllProtocol;
            if ( this.isSelectAllProtocol )
            {
                this.cmbReagentLotNo.DataSource = null;
                this.cmbCalibrationCurve.DataSource = null;
                this.cmbReagentLotNo.SelectedIndex = -1;
            }
            else
            {
                this.cmbReagentLotNo.DataSource = Singleton<CalibrationCurveDB>.Instance.GetData( ( (List<MeasureProtocol>)this.txtAnalytes.Tag )
                    .First().ProtocolIndex ).SelectMany( ( reagentLot ) => reagentLot.Value.SelectMany( ( curve ) => curve.Value.Select( ( data ) => data.GetReagentLotNo() ) ) ).Distinct().ToList();
                this.cmbReagentLotNo.DisplayMember = "ReagentLotNo";
                this.cmbReagentLotNo.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 試薬ロット番号の選択変更イベント
        /// </summary>
        /// <remarks>
        /// 試薬ロット番号の選択を変更します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void cmbReagentLotNo_SelectionChanged( object sender, EventArgs e )
        {
            this.cmbCalibrationCurve.Enabled = !this.isSelectAllProtocol;
            if ( this.isSelectAllProtocol )
            {
                this.cmbCalibrationCurve.DataSource = null;
                this.cmbCalibrationCurve.SelectedIndex = -1;
            }
            else
            {
                if ( this.cmbReagentLotNo.SelectedIndex >= 0 )
                {
                    var curveDataList = Singleton<CalibrationCurveDB>.Instance.GetData(((List<MeasureProtocol>)this.txtAnalytes.Tag).First().ProtocolIndex
                                                                                       , this.cmbReagentLotNo.SelectedItem.DisplayText);

                    var curveData = curveDataList.Where( ( data ) => {
                                                                        DateTime date;
                                                                        return DateTime.TryParse( data.Key, out date );
                                                                     });

                    this.cmbCalibrationCurve.DataSource = curveData.Select( ( data ) => data.Value.First().GetApprovalDateTime() ).ToList();
                    this.cmbCalibrationCurve.SelectedIndex = 0;
                }
                else
                {
                    this.cmbCalibrationCurve.DataSource = null;
                }
            }
        }

        /// <summary>
        /// 検量線の選択変更イベント
        /// </summary>
        /// <remarks>
        /// 全分析項目選択中でなければOKボタン有効に設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void cmbCalibrationCurve_SelectionChanged( object sender, EventArgs e )
        {
            this.btnOk.Enabled = !this.isSelectAllProtocol;
        }

        /// <summary>
        /// 日付(開始日)ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 日付選択ダイアログを表示し日付(開始日)を設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnMeasuringTimeFrom_Click( object sender, EventArgs e )
        {
            // 日付選択ダイアログの呼び出し
            DateTime date;
            DialogResult result = DlgDateSelect.Show( String.Empty, out date, (DateTime)this.btnMeasuringTimeFrom.Tag );
            if ( DialogResult.OK == result )
            {
                this.btnMeasuringTimeFrom.Text = date.ToShortDateString();
                this.btnMeasuringTimeFrom.Tag = date;
            }
        }

        /// <summary>
        /// 日付(終了日)ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 日付選択ダイアログを表示し日付(終了日)を設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnMeasuringTimeTo_Click( object sender, EventArgs e )
        {
            // 日付選択ダイアログの呼び出し
            DateTime date;
            DialogResult result = DlgDateSelect.Show( String.Empty, out date, (DateTime)this.btnMeasuringTimeTo.Tag );
            if ( DialogResult.OK == result )
            {
                date = date.Add( TimeSpan.FromDays( 1 ) - TimeSpan.FromSeconds( 1 ) );
                this.btnMeasuringTimeTo.Text = date.ToShortDateString();
                this.btnMeasuringTimeTo.Tag = date;
            }
        }

        #endregion
    }
}
