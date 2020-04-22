using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Oelco.Common.GUI;
using Oelco.CarisX.Parameter;
using Oelco.Common.Parameter;
using Oelco.Common.Utility;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 分析項目選択ダイアログ
    /// </summary>
    public partial class DlgProtocolSelect : DlgCarisXBase
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 分析項目上限選択数
        /// </summary>
        private Int32 selectMaxLimit;

        /// <summary>
        /// 分析項目下限選択数
        /// </summary>
        private Int32 selectMinLimit;

        /// <summary>
        /// 現在選択中の分析項目インデックス
        /// </summary>
        private List<Int32> selectedProtocolIndexs = new List<Int32>();

        /// <summary>
        /// 測定テーブルモード
        /// </summary>
        private bool routineTableMode = true;

        /// <summary>
        /// 指定分析項目
        /// </summary>
        private IEnumerable<Int32> measureProtocolNo = null;

        /// <summary>
        /// 判断是否是多页的情况
        /// </summary>
        private Boolean bIsMutiPages = false;

        /// <summary>
        /// 项目的页编码（一页为120项目）
        /// </summary>
        private int ProtocolPageIndex = 1;

        /// <summary>
        /// 项目的最大页数，初始值为1
        /// </summary>
        private int MaxNumberOfProtocolPages = 1;

        /// <summary>
        /// 一页显示的项目数，为120；
        /// </summary>
        private const int MaxNumberOfOnePage = 120;

        /// <summary>
        /// 实现显示的项目数
        /// </summary>
        private List<MeasureProtocol> protocols = null;

        private bool enabledFlag = false;
        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="enabledFlag">活性/非活性切り替えフラグ</param>
        /// <param name="selectMaxLimit">分析項目上限選択数(0:制限無し)</param>
        /// <param name="selectMinLimit">分析項目下限選択数(0:制限無し) 上限以上の場合、上限と同値指定と同様</param>
        /// <param name="measureProtocolNo">初期選択分析項目番号</param>
        public DlgProtocolSelect( bool enabledFlag, Int32 selectMaxLimit = 0, Int32 selectMinLimit = 0, IEnumerable<Int32> measureProtocolNo = null )
        {
            InitializeComponent();

            // 選択数上限数設定
            if ( selectMaxLimit > 0 )
            {
                this.selectMaxLimit = selectMaxLimit;
                this.lblLimitMessage.Text = String.Format( Oelco.CarisX.Properties.Resources.STRING_DLG_PROTOCOLSELECT_001, this.selectMaxLimit.ToString() );
            }
            else
            {
                this.selectMaxLimit = 0;
                this.lblLimitMessage.Visible = false;
                this.Height = this.Height - this.lblLimitMessage.Height;
            }

            // 選択下限数設定
            if ( selectMinLimit > 0 )
            {
                if ( selectMinLimit <= selectMaxLimit )
                {
                    this.selectMinLimit = selectMinLimit;
                }
                else
                {
                    this.selectMinLimit = selectMaxLimit;
                }
            }
            else
            {
                this.selectMinLimit = 0;
            }
            
            this.measureProtocolNo = measureProtocolNo;
            this.enabledFlag = enabledFlag;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 分析項目上限選択数の取得
        /// </summary>
        public Int32 SelectMaxLimit
        {
            get
            {
                return this.selectMaxLimit;
            }
        }

        /// <summary>
        /// 分析項目下限選択数の取得
        /// </summary>
        public Int32 SelectMinLimit
        {
            get
            {
                return this.selectMinLimit;
            }
        }

        /// <summary>
        /// 選択の分析項目番号の取得
        /// </summary>
        public List<Int32> SelectedProtocolIndexs
        {
            get
            {
                return this.selectedProtocolIndexs;
            }
        }

        /// <summary>
        /// 測定テーブルモードの取得、設定
        /// </summary>
        public Boolean RoutineTableMode
        {
            get
            {
                return this.routineTableMode;
            }
            set
            {
                this.routineTableMode = value;
            }
        }

        #endregion
        
        #region [protectedメソッド]

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
        /// リソースの初期化
        /// </summary>
        /// <remarks>
        /// リソースを初期化します
        /// </remarks>
        protected override void initializeResource()
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_PROTOCOLSELECT_000;

            // ボタン
            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnClose.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_002;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// フォーム読み込みイベント
        /// </summary>
        /// <remarks>
        /// 全てのボタンの選択状態を初期化し、分析項目の読み込みを行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void DlgProtocolSelect_Load( object sender, EventArgs e )
        {
            // 全てのボタンの選択状態を初期化
            foreach ( Control ctrl in this.pnlProtocol.ClientArea.Controls )
            {
                CustomUStateButton btn = ctrl as CustomUStateButton;
                if ( btn != null )
                {
                    if ( btn.CurrentState )
                    {
                        btn.CurrentState = false;
                    }
                }
            }

            // 分析項目の読み込み
            this.loadProtocol();

            //// 選択中の分析項目の初期化
            //this.selectedProtocolIndexs.Clear();

            if (this.enabledFlag == true)
            {
                // 分析項目ボタンの活性/非活性を変更 
                this.protocolIndexToButtonDicEnabled();
            }
        }

        /// <summary>
        /// 分析項目選択ボタンステータス変更イベント
        /// </summary>
        /// <remarks>
        /// 分析項目の選択ボタンステータスを変更します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnProtocol_StateChanged( object sender, CustomUStateButton.ChangeStateEventArgs e )
        {
            CustomUStateButton btnProtocol = ( (CustomUStateButton)sender );

            if ( e.AfterState )
            {
                if ( !this.selectedProtocolIndexs.Contains( (Int32)btnProtocol.Tag ) )
                {
                    this.selectedProtocolIndexs.Add( (Int32)btnProtocol.Tag );

                    // 分析項目上限選択数があり、超過した場合
                    if ( this.selectMaxLimit > 0
                        && this.selectedProtocolIndexs.Count > this.selectMaxLimit )
                    {
                        // 最古の選択を解除
                        Int32 protocolIndex = this.selectedProtocolIndexs.First();
                        this.pnlProtocol.ClientArea.Controls.OfType<CustomUStateButton>().Single( ( btn ) => btn.Visible && (Int32)btn.Tag == protocolIndex ).CurrentState = false;
                    }
                }
            }
            else
            {
                if ( this.SelectedProtocolIndexs.Count > this.selectMinLimit )
                {
                    this.selectedProtocolIndexs.Remove( (Int32)btnProtocol.Tag );
                }
                else
                {
                    e.AfterState = true;
                }
            }
        }

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にOKを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOK_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
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
        /// 分析項目の読み込みとボタン設定
        /// </summary>
        /// <remarks>
        /// 分析項目の読み込みボタン設定を行います
        /// </remarks>
        private void loadProtocol()
        {          

            if ( this.measureProtocolNo == null )
            {
                // 分析項目のモード仕分け
                if ( this.RoutineTableMode )
                {
                    protocols = Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList;
                }
                else
                {
                    protocols = Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList;
                    //判断是否翻页
                    if (protocols.Count> MaxNumberOfOnePage)
                    {
                        bIsMutiPages = true;
                    }
                    
                }
            }
            else
            {
                protocols = Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList.Where( ( protocol ) => this.measureProtocolNo.Contains( protocol.ProtocolNo ) ).ToList();
            }

            //判断是否是多页的选择
            if (bIsMutiPages)
            {
                CaculateTheMaxPages();
                SetPreAndPostVisibilty(true);
                InitMutiPages();
            } 
            else
            {
                SetPreAndPostVisibilty(false);
                InitNoMutiPages();
            }
           
        }

        private void SetPreAndPostVisibilty(Boolean IsVisible)
        {
            btnPre.Visible = IsVisible;
            btnPost.Visible = IsVisible;
        }

        private void InitMutiPages()
        {
            updateAnalyteTableButtonText();    
        }
        private void InitNoMutiPages()
        {
            // 分析項目選択ボタンの設定
            Action<CustomUStateButton, Int32> setting = ( btn, index ) =>
            {
                if ( protocols.Count > index && protocols[index] != null )
                {
                    // 分析項目ボタンのTagにProtocolIndexを持たせる
                    btn.Text = protocols[index].ProtocolName;
                    btn.Tag = protocols[index].ProtocolIndex;
                    if ( this.selectMinLimit > 0 && this.selectedProtocolIndexs.Count() < this.selectMinLimit )
                    {
                        this.selectedProtocolIndexs.Add( protocols[index].ProtocolIndex );
                    }
                    btn.CurrentState = this.selectedProtocolIndexs.Contains( protocols[index].ProtocolIndex );
                    btn.Visible = true;

                    // ウィンドウサイズを調整
                    if ( this.pnlProtocol.Height != btn.Location.Y + btn.Height )
                    {
                        this.lblLimitMessage.Location = new Point( this.lblLimitMessage.Location.X, this.lblLimitMessage.Location.Y - ( this.pnlProtocol.Height - ( btn.Location.Y + btn.Height ) ) );
                        this.Height -= ( this.pnlProtocol.Height - ( btn.Location.Y + btn.Height ) );
                        this.pnlProtocol.Height = btn.Location.Y + btn.Height;
                    }
                }
                else
                {
                    // 分析項目が設定されないボタンは非表示
                    btn.Visible = false;
                }
            };

            // 各種分析項目選択ボタンの設定
            Int32 idx = 0;
            foreach ( CustomUStateButton btn in this.pnlProtocol.ClientArea.Controls )
            {
                setting( btn, idx++ );
            }
        }

        #endregion
        private void btnPre_Click(object sender, EventArgs e)
        {
            // UpdateTheCurrentPageSelectPrococol();
            ProtocolPageIndex--;
            if (ProtocolPageIndex == 0)
            {
                ProtocolPageIndex = MaxNumberOfProtocolPages;
            }
            updateAnalyteTableButtonText();
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            //  UpdateTheCurrentPageSelectPrococol();
            ProtocolPageIndex++;
            if (ProtocolPageIndex == (MaxNumberOfProtocolPages + 1))
            {
                ProtocolPageIndex = 1;
            }
            updateAnalyteTableButtonText();
        }
        
        /// <summary>
        /// 计算项目页的大小
        /// </summary>
        private void CaculateTheMaxPages()
        {
            int nProtocolCount = Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList.Count;
            MaxNumberOfProtocolPages = nProtocolCount / MaxNumberOfOnePage;
            if (nProtocolCount % MaxNumberOfOnePage != 0)
            {
                MaxNumberOfProtocolPages++;
            }
        }

        private void updateAnalyteTableButtonText()
        {
            try
            {
                Int32 idx = MaxNumberOfOnePage * (ProtocolPageIndex - 1);
                foreach (CustomUStateButton btn in this.pnlProtocol.ClientArea.Controls)
                {
                    if (protocols.Count > idx && protocols[idx] != null)
                    {
                        // 分析項目ボタンのTagにProtocolIndexを持たせる
                        btn.Text = protocols[idx].ProtocolName;
                        btn.Tag = protocols[idx].ProtocolIndex;
                        if (this.selectMinLimit > 0 && this.selectedProtocolIndexs.Count() < this.selectMinLimit)
                        {
                            this.selectedProtocolIndexs.Add(protocols[idx].ProtocolIndex);
                        }
                        btn.CurrentState = this.selectedProtocolIndexs.Contains(protocols[idx].ProtocolIndex);
                        btn.Visible = true;
                    }
                    else
                    {
                        // 分析項目が設定されないボタンは非表示
                        btn.Visible = false;
                    }
                    idx++;
                }
            }
            catch (System.Exception ex)
            {
              //  Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("An exception occurred in updateAnalyteTableButtonText : {0}", ex.StackTrace));
                int exresult = ex.HResult;
            }

        }

        /// <summary>
        /// 分析項目選択ボタンの活性/非活性処理
        /// </summary>
        /// <remarks>
        /// 分析項目の急診と、モジュールの急診使用有無によって分析項目選択ボタンの活性/非活性を切り替える
        /// </remarks>
        private void protocolIndexToButtonDicEnabled()
        {
            // プロトコルが読み込まれていない場合は、分析項目を読み込む
            if (protocols == null)
            {
                // 分析項目の読み込み
                this.loadProtocol();
            }

            // trueなら分析項目ボタンを非活性にする可能性あり
            // falseならすべての分析項目ボタンを活性にする
            bool enabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();


            Int32 index = 0;
            foreach ( CustomUStateButton btn in this.pnlProtocol.ClientArea.Controls )
            {
                if (protocols.Count > index && protocols[index] != null)
                {
                    if (enabledFlag)
                    {
                        if (protocols [index].UseEmergencyMode == true)
                        {
                            btn.Enabled = false;
                        }
                        else
                        {
                            btn.Enabled = true;
                        }
                    }
                    else
                    {
                        btn.Enabled = true;
                    }
                   
                }
                index++;
            }
        }
    }
}
