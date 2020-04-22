using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinListView;
using Oelco.Common.GUI;
using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Parameter;
using Oelco.Common.Utility;

namespace Oelco.CarisX.GUI.Controls
{
    /// <summary>
    /// 絞り込みパネル
    /// </summary>
    public partial class SearchInfoPanelBase : UserControl, ISearchInfo
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// OKボタンクリックイベントハンドラ
        /// </summary>
        public event EventHandler OkClick;

        /// <summary>
        /// Cancelボタンクリックイベントハンドラ
        /// </summary>
        public event EventHandler CancelClick;

        /// <summary>
        /// Closeボタンクリックイベントハンドラ
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
        public SearchInfoPanelBase()
        {
            InitializeComponent();

            // 測定日時(開始日、終了日)の初期化
            this.btnMeasuringTimeFrom.Text = DateTime.Today.ToShortDateString();
            this.btnMeasuringTimeFrom.Tag = DateTime.Today;
            this.btnMeasuringTimeTo.Text = DateTime.Today.ToShortDateString();
            this.btnMeasuringTimeTo.Tag = DateTime.Today.Add( TimeSpan.FromDays( 1 ) - TimeSpan.FromSeconds( 1 ) ); // 23:59:59

            // パネル既定ボタン
            this.btnOk.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;

            // 分析項目選択ボタン
            this.btnSelectAnalyte.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_000;

            // 各種チェックボックス
            this.chkConcentration.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_001;
            this.chkRackId.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_002;
            this.chkSequenceNo.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_003;
            this.chkMeasuringTime.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_004;

            // 各ハイフン
            this.lblHyphen1.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
            this.lblHyphen2.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
            this.lblHyphen3.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;
            this.lblHyphen4.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;

            // リマーク
            this.lblRemark.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_005;
            this.chkRemarkTemperatureError.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_006;
            this.chkRemarkCalibrationError.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_007;
            this.chkRemarkExpirationDataError.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_008;
            this.chkRemarkDataWarning.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_009;
            this.chkRemarkDataEdited.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_010;
            this.chkRemarkOnLine.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_011;

            // モジュール
            this.chkModule1.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_019;
            this.chkModule2.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_020;
            this.chkModule3.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_021;
            this.chkModule4.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_022;

        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 選択中の濃度の取得、設定
        /// </summary>
        [BrowsableAttribute( false )]
        Tuple<Boolean, Double, Double> ISearchInfo.ConcentrationSelect
        {
            get
            {
                if ( this.chkConcentration.Checked )
                {
                    return new Tuple<Boolean, Double, Double>( this.chkConcentration.Checked, Double.Parse( this.numConcentrationFrom.Text ), Double.Parse( this.numConcentrationTo.Text ) );
                }
                else
                {
                    return new Tuple<Boolean, Double, Double>( this.chkConcentration.Checked, 0, 0 );
                }
            }
            set
            {
                this.chkConcentration.Checked = value.Item1;
                this.numConcentrationFrom.Text = value.Item2.ToString();
                this.numConcentrationTo.Text = value.Item3.ToString();
            }
        }

        /// <summary>
        /// 選択中のラックIDの取得、設定
        /// </summary>
        [BrowsableAttribute( false )]
        Tuple<Boolean, CarisXIDString, CarisXIDString> ISearchInfo.RackIdSelect
        {
            get
            {
                if ( this.chkRackId.Checked )
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
                else
                {
                    return new Tuple<Boolean, CarisXIDString, CarisXIDString>( this.chkRackId.Checked, null, null );
                }
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
        Tuple<Boolean, Int32, Int32> ISearchInfo.SequenceNoSelect
        {
            get
            {
                if ( this.chkSequenceNo.Checked )
                {
                    return new Tuple<Boolean, Int32, Int32>( this.chkSequenceNo.Checked, Int32.Parse( this.numSequenceNoFrom.Text ), Int32.Parse( this.numSequenceNoTo.Text ) );
                }
                else
                {
                    return new Tuple<Boolean, Int32, Int32>( this.chkSequenceNo.Checked, 0, 0 );
                }
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
        Tuple<Boolean, DateTime, DateTime> ISearchInfo.MeasuringTimeSelect
        {
            get
            {
                if ( this.chkMeasuringTime.Checked )
                {
                    return new Tuple<Boolean, DateTime, DateTime>( this.chkMeasuringTime.Checked, (DateTime)this.btnMeasuringTimeFrom.Tag, (DateTime)this.btnMeasuringTimeTo.Tag );
                }
                else
                {
                    return new Tuple<Boolean, DateTime, DateTime>( this.chkMeasuringTime.Checked, DateTime.MinValue, DateTime.MinValue );
                }
            }
            set
            {
                this.chkMeasuringTime.Checked = value.Item1;
                this.btnMeasuringTimeFrom.Tag = value.Item2;
                this.btnMeasuringTimeFrom.Text = value.Item2.ToShortDateString();
                this.btnMeasuringTimeTo.Tag = value.Item3;
                this.btnMeasuringTimeTo.Text = value.Item3.ToShortDateString();
            }
        }

        /// <summary>
        /// 選択中の分析項目の取得
        /// </summary>
        [BrowsableAttribute( false )]
        List<String> ISearchInfo.AnalyteSelect
        {
            get
            {
                List<String> analyteSelect = new List<String>();
                foreach ( UltraListViewItem item in this.lvwSelectedAnalytes.Items )
                {
                    analyteSelect.Add( item.Text );
                }

                return analyteSelect;
            }
        }

        /// <summary>
        /// 選択中のリマークの取得、設定
        /// </summary>
        [BrowsableAttribute( false )]
        Remark.RemarkCategory ISearchInfo.RemarkSelect
        {
            get
            {
                Remark.RemarkCategory category = 0;

                // 温度エラー
                category |= ( this.chkRemarkTemperatureError.Checked ) ? Remark.RemarkCategory.TemperatureError : 0;

                // 検量線エラー
                category |= ( this.chkRemarkCalibrationError.Checked ) ? Remark.RemarkCategory.CalibrationError : 0;

                // 有効期限エラー
                category |= ( this.chkRemarkExpirationDataError.Checked ) ? Remark.RemarkCategory.ExpirationDataError : 0;

                // データ警告
                category |= ( this.chkRemarkDataWarning.Checked ) ? Remark.RemarkCategory.DataWarning : 0;

                // データ編集
                category |= ( this.chkRemarkDataEdited.Checked ) ? Remark.RemarkCategory.DataEdited : 0;
                
                // オンライン
                category |= ( this.chkRemarkOnLine.Checked ) ? Remark.RemarkCategory.OnLine : 0;

                return category;
            }
            set
            {
                // 温度エラー
                this.chkRemarkTemperatureError.Checked = ( value & Remark.RemarkCategory.TemperatureError ) != 0;
              
                // 検量線エラー
                this.chkRemarkCalibrationError.Checked = ( value & Remark.RemarkCategory.CalibrationError ) != 0;

                // 有効期限エラー
                this.chkRemarkExpirationDataError.Checked = ( value & Remark.RemarkCategory.ExpirationDataError ) != 0;

                // データ警告
                this.chkRemarkDataWarning.Checked = ( value & Remark.RemarkCategory.DataWarning ) != 0;

                // データ編集
                this.chkRemarkDataEdited.Checked = ( value & Remark.RemarkCategory.DataEdited ) != 0;

                // オンライン
                this.chkRemarkOnLine.Checked = ( value & Remark.RemarkCategory.OnLine ) != 0;
            }
        }

        /// <summary>
        /// 選択中のモジュールの取得、設定
        /// </summary>
        [BrowsableAttribute(false)]
        ModuleCategory ISearchInfo.ModuleSelect
        {
            get
            {
                ModuleCategory category = 0;

                // モジュール１
                category |= (this.chkModule1.Checked) ? ModuleCategory.Module1 : 0;

                // モジュール２
                category |= (this.chkModule2.Checked) ? ModuleCategory.Module2 : 0;

                // モジュール３
                category |= (this.chkModule3.Checked) ? ModuleCategory.Module3 : 0;

                // モジュール４
                category |= (this.chkModule4.Checked) ? ModuleCategory.Module4 : 0;

                return category;
            }
            set
            {
                // モジュール１
                this.chkModule1.Checked = (value & ModuleCategory.Module1) != 0;

                // モジュール２
                this.chkModule2.Checked = (value & ModuleCategory.Module2) != 0;

                // モジュール３
                this.chkModule3.Checked = (value & ModuleCategory.Module3) != 0;

                // モジュール４
                this.chkModule4.Checked = (value & ModuleCategory.Module4) != 0;
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

        #endregion

        /// <summary>
        /// add by marxsu 样本筛选”数据修改“权限设置
        /// </summary>
        public bool RemarkDataEditedEnable
        {
            get
            {
                return this.chkRemarkDataEdited.Visible;
            }
            set
            {
                this.chkRemarkDataEdited.Visible = value;
                if (value == false)
                {
                    this.chkRemarkOnLine.Location = new Point(this.chkRemarkDataEdited.Location.X, this.chkRemarkOnLine.Location.Y);
                }
                else
                {
                    this.chkRemarkOnLine.Location = new Point(this.chkRemarkDataEdited.Location.X + this.chkRemarkDataEdited.Size.Width, this.chkRemarkOnLine.Location.Y);
                }

            }
        }

        #region [privateメソッド]

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// OKボタンクリックイベントを実行します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click( object sender, EventArgs e )
        {
            if ( this.OkClick != null )
            {
                this.OkClick( sender, e );
            }
        }

        /// <summary>
        /// キャンセルボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// キャンセルボタンクリックイベントを実行します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnCancel_Click( object sender, EventArgs e )
        {
            if ( this.CancelClick != null )
            {
                this.CancelClick( sender, e );
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
        /// 分析項目選択ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 分析項目選択ダイアログを表示し、選択された分析項目をリストに追加します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnSelectAnalyte_Click( object sender, EventArgs e )
        {
            using ( DlgProtocolSelect protocolSelect = new DlgProtocolSelect(false) )
            {
                protocolSelect.RoutineTableMode = false;
                var selectedProtocolIndex = this.lvwSelectedAnalytes.Items.OfType<UltraListViewItem>().Select( ( item ) => Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName( item.Text ).ProtocolIndex );
                protocolSelect.SelectedProtocolIndexs.AddRange( selectedProtocolIndex );
                if ( protocolSelect.ShowDialog() == DialogResult.OK )
                {
                    this.lvwSelectedAnalytes.Items.Clear();
                    protocolSelect.SelectedProtocolIndexs.ForEach( ( index ) =>
                    {
                        MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( index );
                        if ( protocol != null )
                        {
                            this.lvwSelectedAnalytes.Items.Add( index.ToString(), protocol.ProtocolName );
                        }
                    } );
                }
            }
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
