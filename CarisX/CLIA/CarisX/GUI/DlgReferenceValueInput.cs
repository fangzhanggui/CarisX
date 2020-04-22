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
using Oelco.CarisX.Parameter;
using Oelco.Common.Parameter;
using Oelco.CarisX.DB;
using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 管理値入力ダイアログ
    /// </summary>
    public partial class DlgReferenceValueInput : DlgCarisXBase
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// 現在選択中のボタン
        /// </summary>
        private CustomUStateButton selectingButton;

        /// <summary>
        /// 精度管理情報
        /// </summary>
        ControlQCData currentControlQCData = null;
        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgReferenceValueInput()
        {
            InitializeComponent();

            // プロトコルボタンの分析項目関連付け
            List<CustomUStateButton> buttons = ( from btn in this.pnlProtocol.ClientArea.Controls.OfType<CustomUStateButton>()
                                                 orderby btn.Location.Y, btn.Location.X
                                                 select btn ).ToList();
            for ( Int32 i = 0; i < buttons.Count; i++ )
            {
                if ( i < Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList.Count )
                {
                    buttons[i].Text = Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList[i].ProtocolName;
                    buttons[i].Tag = Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList[i].ProtocolIndex;
                }
                else
                {
                    buttons[i].Visible = false;
                }
            }

            this.IsEditMode = true;
            this.btnSave.Enabled = false;
            this.btnDelete.Enabled = false;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 選択中の分析項目のインデックスの取得、設定
        /// </summary>
        private Int32 SelectedMeasureProtocolIndex
        {
            get
            {
                if ( this.selectingButton != null )
                {
                    return (Int32)this.selectingButton.Tag;
                }
                return -1;
            }
            set
            {
                this.selectingButton = this.pnlProtocol.ClientArea.Controls.OfType<CustomUStateButton>().FirstOrDefault( ( btn ) => (Int32)btn.Tag == value );
            }
        }

        /// <summary>
        /// 編集モードかどうかを取得、設定
        /// </summary>
        private Boolean IsEditMode
        {
            get
            {
                return (Boolean)this.optEditMode.Value;
            }
            set
            {
                this.optEditMode.Value = value;
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
            // ダイアログタイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_REFERENCEVALUEINPUT_000;

            // グループボックス名
            this.gbxEditMode.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_REFERENCEVALUEINPUT_001;
            this.gbxReferenceValue.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_REFERENCEVALUEINPUT_002;

            // 編集モード選択項目
            this.optEditMode.Items[0].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_REFERENCEVALUEINPUT_003;
            this.optEditMode.Items[1].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_REFERENCEVALUEINPUT_004;

            // 精度管理検体名、ロット番号
            this.lblTitleControlName.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_REFERENCEVALUEINPUT_005;
            this.lblTitleControlLotNo.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_REFERENCEVALUEINPUT_006;

            // 管理図名
            this.lblTitleInterDayChart.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_REFERENCEVALUEINPUT_007;
            this.lblTitleRControlChart.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_REFERENCEVALUEINPUT_008;

            // 管理値名
            this.lblTitleInterDayMean.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_REFERENCEVALUEINPUT_009;
            this.lblTitleInterDayConcentrationWidth.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_REFERENCEVALUEINPUT_010;
            this.lblTitleControlR.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_REFERENCEVALUEINPUT_011;

            // ボタン
            this.btnDelete.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_010;
            this.btnSave.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_011;
            this.btnClose.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_002;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// 分析項目ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 分析項目を設定します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnProtocol_Click( object sender, EventArgs e )
        {
            var btn = (CustomUStateButton)sender;
            // 分析項目指定
            if ( sender != this.selectingButton && ( btn ).CurrentState )
            {
                this.selectingButton = btn;
                String maskInput = CarisXSubFunction.GetConcNumericEditorMaskInput( (Int32)btn.Tag );
                // = null;
                //var integerCount = CarisXConst.CONC_REAL_NUMBER_DIGITS - Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( (Int32)btn.Tag ).LengthAfterDemPoint;
                //for ( int i = 0; i < CarisXConst.CONC_REAL_NUMBER_DIGITS; i++ )
                //{
                //    maskInput += ( ( integerCount == i ) ? "." : String.Empty ) + "n";
                //}

                //maskInput

                this.numInterDayMean.MaskInput = maskInput;
                this.numInterDayConcentrationWidth.MaskInput = maskInput;
                this.numControlR.MaskInput = maskInput;

                this.numInterDayMean.Enabled = true;
                this.numInterDayConcentrationWidth.Enabled = true;
                this.numControlR.Enabled = true;
            }
            else
            {
                this.selectingButton = null;

                this.numInterDayMean.MaskInput = null;
                this.numInterDayConcentrationWidth.MaskInput = null;
                this.numControlR.MaskInput = null;

                this.numInterDayMean.Enabled = false;
                this.numInterDayConcentrationWidth.Enabled = false;
                this.numControlR.Enabled = false;
            }

            this.numInterDayMean.Value = 0;
            this.numInterDayConcentrationWidth.Value = 0;
            this.numControlR.Value = 0;

            this.loadControlQCInfo();
            this.loadControlQCData();
        }

        /// <summary>
        /// 分析項目に対応する
        /// </summary>
        /// <remarks>
        /// 分析項目を設定します
        /// </remarks>
        private void loadControlQCInfo()
        {
            // 選択された分析項目の精度管理情報を取得
            var controlQCList = Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.ControlQCList.Where( ( data ) => data.MeasureProtocolIndex == SelectedMeasureProtocolIndex );
            if ( controlQCList != null && controlQCList.Count() > 0 )
            {
                // 精度管理検体名(昇順)
                var index = this.cmbControlName.SelectedIndex;
                this.cmbControlName.DataSource = controlQCList.Select( ( data ) => data.ControlName ).OrderBy( ( str ) => str ).Distinct().ToList();
                this.cmbControlName.DataBind();

                //// 精度管理検体ロット(降順)
                //this.cmbControlLotNo.DataSource = controlQCList.Select( ( data ) => data.ControlLotNo ).OrderByDescending( ( str ) => str ).Distinct().ToList();
                //this.cmbControlLotNo.DataBind();

                // 初期表示設定
                this.cmbControlName.SelectedIndex = ( index > 0 && index < this.cmbControlName.Items.Count ) ? index : 0;
                //this.cmbControlLotNo.SelectedIndex = 0;

                this.btnDelete.Enabled = true;
            }
            else
            {
                // 精度管理検体名(昇順)
                this.cmbControlName.DataSource = null;

                // 精度管理検体ロット(降順)
                this.cmbControlLotNo.DataSource = null;

                this.btnSave.Enabled = false;
            }
        }

        /// <summary>
        /// 編集モード選択変更イベント
        /// </summary>
        /// <remarks>
        /// 編集モード切替します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void optEditMode_ValueChanged( object sender, EventArgs e )
        {
            // 編集モード切替
            this.cmbControlLotNo.Enabled = this.IsEditMode;
            this.cmbControlName.Enabled = this.IsEditMode;
            //this.btnDelete.Enabled = this.IsEditMode;
            this.txtControlLotNo.Enabled = !this.IsEditMode;
            this.txtControlName.Enabled = !this.IsEditMode;
            this.loadControlQCInfo();
            this.setControlLotNo();
            this.loadControlQCData();
        }

        /// <summary>
        /// 保存ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 精度管理情報を保存します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnSave_Click( object sender, EventArgs e )
        {
            this.saveControlQCData();
        }

        /// <summary>
        /// 削除ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 精度管理情報を削除します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnDelete_Click( object sender, EventArgs e )
        {
            this.removeControlQCData();
        }

        /// <summary>
        /// 閉じるボタンクリックイベント
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
        /// フォーム読み込みイベント
        /// </summary>
        /// <remarks>
        /// パラメータ読み込みします
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void DlgReferenceValueInput_Load( object sender, EventArgs e )
        {
            Singleton<ParameterFilePreserve<ControlQC>>.Instance.Load();

            this.numInterDayMean.Enabled = false;
            this.numInterDayConcentrationWidth.Enabled = false;
            this.numControlR.Enabled = false;
        }

        /// <summary>
        /// 精度管理検体名の選択変更イベント
        /// </summary>
        /// <remarks>
        /// 精度管理ロットNoの設定を行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void cmbControlName_SelectionChanged( object sender, EventArgs e )
        {
            this.setControlLotNo();
        }

        /// <summary>
        /// 精度管理検体ロット番号の選択変更イベント
        /// </summary>
        /// <remarks>
        /// 管理情報の読込を行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void cmbControlLotNo_SelectionChanged( object sender, EventArgs e )
        {
            this.loadControlQCData();

            //if ( !String.IsNullOrEmpty( cmbControlLotNo.Text) )
            //{
            //    var controlQCList = Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.ControlQCList.Where( ( data ) => data.MeasureProtocolIndex == SelectedMeasureProtocolIndex && data.ControlLotNo == cmbControlLotNo.Text );
            //    // 精度管理検体名(昇順)
            //    this.cmbControlName.DataSource = controlQCList.Select( ( data ) => data.ControlName ).OrderBy( ( str ) => str ).Distinct().ToList();
            //    this.cmbControlName.DataBind();
            //}
        }

        /// <summary>
        /// 精度管理ロットNoを設定
        /// </summary>
        /// <remarks>
        /// 精度管理ロットNoの設定を行います
        /// </remarks>
        private void setControlLotNo()
        {
            if ( !String.IsNullOrEmpty( cmbControlName.Text ) )
            {
                this.cmbControlLotNo.SelectedIndex = -1;
                var controlQCList = Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.ControlQCList.Where( ( data ) => data.MeasureProtocolIndex == SelectedMeasureProtocolIndex && data.ControlName == cmbControlName.Text );
                // 精度管理検体ロット(降順)
                this.cmbControlLotNo.DataSource = controlQCList.Select( ( data ) => data.ControlLotNo ).OrderByDescending( ( str ) => str ).Distinct().ToList();
                this.cmbControlLotNo.DataBind();
                this.cmbControlLotNo.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 管理情報の読み込み
        /// </summary>
        /// <remarks>
        /// 精度管理情報の読込を行います
        /// </remarks>
        private void loadControlQCData()
        {
            GetControlQCData();

            if ( this.currentControlQCData != null )
            {
                // 管理表示設定
                this.numInterDayMean.Value = this.currentControlQCData.Mean ?? 0;
                this.numInterDayConcentrationWidth.Value = this.currentControlQCData.ConcentrationWidth ?? 0;
                this.numControlR.Value = this.currentControlQCData.ControlR ?? 0;
            }
        }

        /// <summary>
        /// 精度管理情報の取得
        /// </summary>
        /// <remarks>
        /// 精度管理情報の取得を行います
        /// </remarks>
        private void GetControlQCData()
        {
            if ( Singleton<ParameterFilePreserve<ControlQC>>.Instance.Load() )
            {
                this.currentControlQCData = GetEditData();
                if ( this.currentControlQCData != null )
                {
                    this.btnDelete.Enabled = true;
                }
                else
                {
                    this.btnDelete.Enabled = false;
                }
            }
            this.currentControlQCData = this.currentControlQCData ?? new Func<ControlQCData>( () =>
            {
                var data = new ControlQCData();
                data.MeasureProtocolIndex = this.SelectedMeasureProtocolIndex;
                if ( !this.IsEditMode && !String.IsNullOrWhiteSpace( this.txtControlLotNo.Text ) && !String.IsNullOrWhiteSpace( this.txtControlName.Text ) )
                {
                    data.ControlName = this.txtControlName.Text;
                    data.ControlLotNo = this.txtControlLotNo.Text;
                    return data;
                }
                else
                {
                    return null;
                }
            } )();

            this.btnSave.Enabled = ( this.currentControlQCData != null );
        }

        /// <summary>
        /// 精度管理検体情報データ取得
        /// </summary>
        /// <remarks>
        /// 分析項目から精度管理情報の取得を行います
        /// </remarks>
        /// <returns></returns>
        private ControlQCData GetEditData()
        {
            Func<ControlQCData, Boolean> match = ( data ) =>
            {
                String lotNo;
                String controlName;
                if ( this.IsEditMode )
                {
                    lotNo = this.cmbControlLotNo.Text;
                    controlName = this.cmbControlName.Text;
                }
                else
                {
                    lotNo = this.txtControlLotNo.Text;
                    controlName = this.txtControlName.Text;
                }
                return data.MeasureProtocolIndex == this.SelectedMeasureProtocolIndex
                    && data.ControlLotNo == lotNo
                && data.ControlName == controlName;
            };
            return Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.ControlQCList.LastOrDefault( match );
        }

        /// <summary>
        /// 現在の精度管理情報を保存
        /// </summary>
        /// <remarks>
        /// 精度管理情報の保存を行います
        /// </remarks>
        private void saveControlQCData()
        {
            if ( this.SelectedMeasureProtocolIndex <= 0 )
            {
                // エラー(分析項目未選択)
                DlgMessage.Show( Properties.Resources.STRING_DLG_MSG_105, String.Empty, Properties.Resources.STRING_DLG_TITLE_002, MessageDialogButtons.OK );
                return;
            }
            this.GetControlQCData();
            this.currentControlQCData.Mean = (Double)this.numInterDayMean.Value;
            this.currentControlQCData.ConcentrationWidth = (Double)this.numInterDayConcentrationWidth.Value;
            this.currentControlQCData.ControlR = (Double)this.numControlR.Value;

            if ( !Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.ControlQCList.Contains( this.currentControlQCData ) )
            {
                Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.ControlQCList.Add( this.currentControlQCData );
            }
            Singleton<ParameterFilePreserve<ControlQC>>.Instance.Save();

            DlgMessage.Show( Oelco.CarisX.Properties.Resources.STRING_DLG_MSG_137, String.Empty, Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm );
        }

        /// <summary>
        /// 現在の編集対象の精度管理情報を削除
        /// </summary>
        /// <remarks>
        /// 精度管理情報の削除を行います
        /// </remarks>
        private void removeControlQCData()
        {
            if ( this.IsEditMode )
            {
                var removeData = Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.ControlQCList.FirstOrDefault( ( data ) =>
                {
                    return data.MeasureProtocolIndex == this.currentControlQCData.MeasureProtocolIndex && data.ControlName == this.currentControlQCData.ControlName && data.ControlLotNo == this.currentControlQCData.ControlLotNo;
                } );

                if ( removeData != null )
                {
                    Singleton<ParameterFilePreserve<ControlQC>>.Instance.Param.ControlQCList.Remove( removeData );
                }
                else
                {
                    // TODO:エラー削除対象なし
                }
                Singleton<ParameterFilePreserve<ControlQC>>.Instance.Save();
                this.loadControlQCInfo();
                this.setControlLotNo();
                this.loadControlQCData();
            }
            else
            {
                // TODO:エラー
            }
        }

        /// <summary>
        /// 分析項目選択ボタンステータス変更イベント
        /// </summary>
        /// <remarks>
        /// 分析項目選択ボタンステータスの変更を行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnProtocol_StateChanged( object sender, CustomUStateButton.ChangeStateEventArgs e )
        {
            CustomUStateButton btnProtocol = ( (CustomUStateButton)sender );

            if ( e.AfterState )
            {
                // 分析項目上限選択数を超過した場合
                var selectButtons = this.pnlProtocol.ClientArea.Controls.OfType<CustomUStateButton>().Where( ( ctrl ) => ( (CustomUStateButton)ctrl ).CurrentState );
                if ( selectButtons.Count() > 1 )
                {
                    // 他の選択を解除
                    foreach ( var button in selectButtons.Where( ( btn ) => btn != btnProtocol ) )
                    {
                        button.CurrentState = false;
                    }
                }
            }
            else
            {
                IEnumerable<Control> selectButtons = this.pnlProtocol.Controls.OfType<Control>().Where( ( ctrl ) => ctrl as CustomUStateButton != null && ( (CustomUStateButton)ctrl ).CurrentState );
            }
        }

        /// <summary>
        /// 精度管理検体ロット番号・検体名の変更イベント
        /// </summary>
        /// <remarks>
        /// 精度管理検体ロット番号・検体名の変更を行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void txtControlNameAndLot_TextChanged( object sender, EventArgs e )
        {
            // 新規登録時は精度管理検体名、精度管理検体ロット番号が両入力済時のみ保存可
            if ( !this.IsEditMode )
            {
                if ( String.IsNullOrEmpty( txtControlName.Text ) || String.IsNullOrEmpty( txtControlLotNo.Text ) )
                {
                    btnSave.Enabled = false;
                }
                else
                {
                    btnSave.Enabled = true;
                }
            }
        }

        #endregion
    }
}
