using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.Misc;

using Oelco.Common.GUI;
using Oelco.Common.Utility;
using Oelco.CarisX.Parameter;
using Oelco.Common.Parameter;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.GUI.Controls
{
    /// <summary>
    /// 分析項目設定パネル
    /// </summary>
    public partial class AnalysisSettingPanel : UserControl
    {

        #region [定数定義]

        /// <summary>
        /// 分析項目ボタン数
        /// </summary>
        public const Int32 PROTOCOL_BTN_COUNT = 50;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 分析項目選択状態変更データクラス
        /// </summary>
        public class AnalisisSettingPanelSelectChangingData
        {

            ///
            /// 是否计算ROMA值
            ///

            Boolean isCalcRoma = false;

            public Boolean IsCalcRoma
            {
                get
                {
                    return isCalcRoma;
                }
                set
                {
                    isCalcRoma = value;
                }
            }
            /// <summary>
            /// 次選択状態
            /// </summary>
            Boolean nextState = false;

            /// <summary>
            /// 次選択状態の取得、設定
            /// </summary>
            public Boolean NextState
            {
                get
                {
                    return nextState;
                }
                set
                {
                    nextState = value;
                }
            }
            /// <summary>
            /// キャンセル
            /// </summary>
            Boolean cancel = false;

            /// <summary>
            /// キャンセルの取得、設定
            /// </summary>
            public Boolean Cancel
            {
                get
                {
                    return cancel;
                }
                set
                {
                    cancel = value;
                }
            }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="nextState"></param>
            /// <param name="cancel"></param>
            public AnalisisSettingPanelSelectChangingData( Boolean nextState, Boolean cancel,Boolean calcRoma = false )
            {
                this.nextState = nextState;
                this.cancel = cancel;
                this.isCalcRoma = calcRoma;
            }
        }

        /// <summary>
        /// 分析項目選択状態変更時イベントハンドラ
        /// </summary>
        public event Action<Int32, Boolean> ProtocolCheckChanged;
        /// <summary>
        /// 分析項目選択状態変更時イベントハンドラ
        /// </summary>
        public event Action<Int32, AnalisisSettingPanelSelectChangingData> ProtocolCheckChanging;

        /// <summary>
        /// 閉じるボタンクリックイベントハンドラ
        /// </summary>
        public event Action Closed;

        /// <summary>
        /// 分析項目選択ボタンリスト
        /// </summary>
        private Dictionary<CustomUStateButton, Int32> protocolButtonInfo = new Dictionary<CustomUStateButton, Int32>();
        /// <summary>
        /// プロトコルインデックス-ボタン辞書リスト
        /// </summary>
		private Dictionary<Int32, CustomUStateButton> protocolIndexToButtonDic = new Dictionary<Int32, CustomUStateButton>();
        /// <summary>
        /// プロトコルインデックス-設定値リスト
        /// </summary>
		private Dictionary<Int32, Dictionary<String, Int32>> protocolValueDic = new Dictionary<Int32, Dictionary<String, Int32>>();

        const string protocolValueItem1 = "DefaultDilutionRatio";
        const string protocolValueItem2 = "DefaultMeasTimes";
        const string protocolValueItem3 = "DilutionRatio";
        const string protocolValueItem4 = "MeasTimes";

        #endregion
        /// <summary>
        /// 稀釈倍率デフォルト値
        /// </summary>
        private const Int32 DIL_DEFAULT =1;
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AnalysisSettingPanel()
        {
            InitializeComponent();
            this.ProtocolNameList = new List<String>();

            // ボタンリスト作成（順序を合わせるため）
            this.protocolButtonInfo.Add( this.btnMeasProto1, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto2, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto3, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto4, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto5, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto6, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto7, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto8, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto9, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto10, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto11, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto12, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto13, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto14, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto15, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto16, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto17, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto18, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto19, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto20, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto21, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto22, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto23, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto24, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto25, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto26, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto27, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto28, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto29, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto30, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto31, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto32, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto33, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto34, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto35, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto36, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto37, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto38, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto39, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto40, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto41, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto42, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto43, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto44, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto45, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto46, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto47, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto48, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto49, 0 );
            this.protocolButtonInfo.Add( this.btnMeasProto50, 0 );

            this.lblTitleAutoDilutionRatio.Text = Oelco.CarisX.Properties.Resources.STRING_ANALYSISSETTINGPANEL_000;
            this.chkIsEditRegistInfo.Text = Oelco.CarisX.Properties.Resources.STRING_ANALYSISSETTINGPANEL_057;
           
            this.clearProtocolButtons();
        }

        /// <summary>
        /// ボタン初期化処理
        /// </summary>
        /// <remarks>
        /// ボタン部の初期化を行います。
        /// </remarks>
        private void clearProtocolButtons()
        {
            // ボタン部初期化
            foreach ( KeyValuePair<CustomUStateButton, Int32> buttonInfo in this.protocolButtonInfo )
            {
                buttonInfo.Key.Enabled = false;
                buttonInfo.Key.Text = String.Empty;
            }
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// プロトコル名リストの取得
        /// </summary>
        public List<String> ProtocolNameList
        {
            get;
            protected set;
        }
        /// <summary>
        /// 希釈率の取得
        /// </summary>
        public Int32 Dilution
        {
            get
            {
                return (Int32)( this.numDilutionRatio.Value );
            }
        }

        /// <summary>
        /// 測定回数の取得
        /// </summary>
        public Int32 MeasTimes
        {
            get
            {
                return (Int32)(this.numMeasTimes.Value);
            }
        }

        /// <summary>
        /// 登録情報編集モード切替用チェックボックス表示設定プロパティ
        /// </summary>
        public Boolean ChkIsEditRegistInfoVisible
        {
            get
            {
                return this.chkIsEditRegistInfo.Visible;
            }
            set
            {
                this.chkIsEditRegistInfo.Visible = value;
            }
        }

        /// <summary>
        /// Closeボタン表示設定プロパティ
        /// </summary>
        public Boolean BtnCloseVisible
		{
			get 
			{
				return this.btnClose.Visible;
			}
			set
			{
				this.btnClose.Visible = value;
			}
		}

		/// <summary>
		/// 分析項目選択ボタンリスト
		/// </summary>
		public Dictionary<Int32, CustomUStateButton> ProtocolIndexToButtonDic
		{
			get 
			{ 
				return this.protocolIndexToButtonDic; 
			}
		}

        #endregion

        /// <summary>
        /// 固定数
        /// </summary>
        Int32 lockCount = 0;

        #region [publicメソッド]

        /// <summary>
        /// 選択数固定
        /// </summary>
        /// <remarks>
        /// 指定した数以下の選択状態にならないよう設定します。
        /// 0を設定する事で無制限となります。
        /// </remarks>
        /// <param name="selectCount">固定数</param>
        public void SetMustSelection( Int32 selectCount )
        {
            this.lockCount = selectCount;
        }

        /// <summary>
        /// 分析項目状態設定
        /// </summary>
        /// <remarks>
        /// 分析項目状態の設定を行います。
        /// </remarks>
        /// <param name="protoIndexList">分析項目状態(分析項目インデックスのリスト)</param>
        /// <param name="useEmergencyModeBtnChanged">急診使用によってボタンの設定変更フラグ(分析項目インデックスのリスト)</param>
        public void SetProtocolSettingState( List<Tuple<Int32, Int32, Int32>> protoIndexDilList, bool useEmergencyModeBtnChanged = false )
        {
            bool enabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();

            Dictionary<Int32, CustomUStateButton> tempDic = new Dictionary<Int32,CustomUStateButton>();
            List<Int32> removeList = new List<Int32>();

            // プロトコルインデックス-ボタン辞書を作業用領域にコピー
            foreach ( var val in this.protocolIndexToButtonDic )
            {
                tempDic.Add(val.Key,val.Value);
            }

            // 分析項目インデックスのリスト内容が設定されている箇所を選択状態とする。
            // 作業用領域から、選択状態とした項目を削除する。
            foreach ( var protoIndexDil in protoIndexDilList )
            {
                if ( tempDic.ContainsKey( protoIndexDil.Item1 ) )
                {
                    var protocal = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(protoIndexDil.Item1);
                    
                    // 全スレーブの急診使用無しの場合、急診使用無しの分析項目は変更処理を行わない
                    if ((enabledFlag == true) && ( protocal.UseEmergencyMode == true) && ( useEmergencyModeBtnChanged == true))
                    {
                        continue;
                    }

                    tempDic[protoIndexDil.Item1].CurrentState = true;
                    
                    //表示する名称の編集
                    tempDic[protoIndexDil.Item1].Text = this.createDilString( tempDic[protoIndexDil.Item1], protoIndexDil.Item2, protoIndexDil.Item3);
                    this.protocolValueDic[protoIndexDil.Item1][protocolValueItem3] = protoIndexDil.Item2;
                    this.protocolValueDic[protoIndexDil.Item1][protocolValueItem4] = protoIndexDil.Item3;

                    removeList.Add( protoIndexDil.Item1 );
                }
            }
            foreach ( Int32 protoIndex in removeList )
            {
                tempDic.Remove( protoIndex );
            }

            // 作業用領域の内容に対して非選択状態を設定する。
            foreach ( var val in tempDic )
            {
                val.Value.CurrentState = false;
                val.Value.Text = (String)val.Value.Tag;
            }

            foreach (Int32 tempDicKeys in tempDic.Keys)
            {
                this.protocolValueDic[tempDicKeys][protocolValueItem3] = this.protocolValueDic[tempDicKeys][protocolValueItem1];
                this.protocolValueDic[tempDicKeys][protocolValueItem4] = this.protocolValueDic[tempDicKeys][protocolValueItem2];
            }

            //// 分析項目選択種別を設定
            //this.materialType = materialType;

        }

        // Getは不要か
        //public void GetProtocolSettingState()
        //{

        //}

		/// <summary>
		///  ProtocolCheckChangingイベントを発生させる
		/// </summary>
		/// <param name="protoIndex"></param>
		/// <param name="data"></param>
		public void OnProtocolCheckChanging(Int32 protoIndex, ref AnalisisSettingPanelSelectChangingData data)
		{
			this.ProtocolCheckChanging(protoIndex, data);
		}

		/// <summary>
		/// ProtocolCheckChangedを発生させる
		/// </summary>
		/// <param name="protoIndex"></param>
		/// <param name="select"></param>
		public void OnProtocolCheckChanged(Int32 protoIndex, bool select)
		{
			this.ProtocolCheckChanged(protoIndex, select);
		}

        /// <summary>
        /// 検体情報を再読込
        /// </summary>
        /// <remarks>
        /// 検体情報を再読込します
        /// </remarks>
        public void ReLoadAnalyteInformation()
        {
            // ボタン情報を初期化
            this.initializeProtocolButtonInfo();

            // ボタン状態を更新
            this.refleshProtocolItems();
        }
		
        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <remarks>
        /// 分析項目選択ボタンの初期化を行います。
        /// </remarks>
        /// <param name="e">イベント情報</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            // 分析項目名称を取得
            //this.ProtocolNameList.Capacity = PROTOCOL_BTN_COUNT;
            //var rls = from Control a in this.ultraPanel2.ClientArea.Controls.GetEnumerator() where 
            //foreach ( Control v in this.ultraPanel2.ClientArea.Controls )
            //{
            //    System.Diagnostics.Debug.WriteLine( v.Name );
            //}

            // ボタン情報を初期化
            this.initializeProtocolButtonInfo();

            // ボタン状態を更新
            this.refleshProtocolItems();

            /// AnalysisSettingPanelを継承しているGroupSelectAnalysisSettingPanelと、GroupSelectAnalysisSettingPanelを使用するFormを
            /// デザインモードで動かすと表示できなくなる現象が発生した。
            /// 原因はデザインモード時に継承元のOnLoadイベントが呼ばれてしまい、その際にインスタンスがないにもかかわらずインスタンスの初期化を行ってしまうため動かなくなった。
            /// 対処として、デザインモード時にはインスタンスに関する処理を行わないようにする必要がある。
            // デザインモード時で動いている場合はここで処理を終える。
            if (this.DesignMode) return;

            // FormSystemAnalytes画面では、分析項目ボタンの活性/非活性変更を行わない
            if (!Singleton<FormSystemAnalytes>.Instance.Visible)
            {
                // ボタンの活性/非活性を変更
                protocolIndexToButtonDicEnabled();
            }
           
        }
        
        /// <summary>
        /// ボタン情報を初期化
        /// </summary>
        /// <remarks>
        /// ボタン情報を初期化します
        /// </remarks>
        protected virtual void initializeProtocolButtonInfo()
        {
            this.ProtocolNameList.Clear();
            Int32 count = 0;
            Dictionary<CustomUStateButton,  Int32>.Enumerator buttonInfoEnumerator = this.protocolButtonInfo.GetEnumerator();
            this.protocolIndexToButtonDic.Clear();
            this.protocolValueDic.Clear();
            foreach ( MeasureProtocol protocol in Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList )
            {
                // ルーチンテーブルで選択されている分析項目を追加
                if ( protocol.DisplayProtocol )
                {
                    count++;
                    buttonInfoEnumerator.MoveNext();

                    if ( count > PROTOCOL_BTN_COUNT )
                    {
                        // ボタン数を越える場合ここで抜ける
                        break;
                    }
                    this.ProtocolNameList.Add( protocol.ProtocolName );

                    // Indexがかぶる場合の処理（設定ミスによる）
                    if ( !this.protocolIndexToButtonDic.ContainsKey( protocol.ProtocolIndex ) )
                    {
                        this.protocolIndexToButtonDic.Add( protocol.ProtocolIndex, buttonInfoEnumerator.Current.Key );
                    }

                    //分析項目毎の情報を初期化する
                    if (!this.protocolValueDic.ContainsKey(protocol.ProtocolIndex))
                    {
                        Dictionary<string, int> protocolValue = new Dictionary<string, int>
                        {
                            {protocolValueItem1,1 },
                            {protocolValueItem2,protocol.RepNoForSample },
                            {protocolValueItem3,1 },
                            {protocolValueItem4,1 },
                        }
                        ;
                        this.protocolValueDic.Add(protocol.ProtocolIndex, protocolValue);
                    }
                }
            }
            foreach ( var indexData in this.protocolIndexToButtonDic )
            {
                this.protocolButtonInfo[indexData.Value] = indexData.Key;
            }
        }

        /// <summary>
        /// 分析項目情報更新
        /// </summary>
        /// <remarks>
        /// 分析項目情報更新します
        /// </remarks>
		protected virtual void refleshProtocolItems()
        {
            // 先にクリア（項目減った場合もある為）
            foreach ( var btnInf in this.protocolButtonInfo )
            {
                btnInf.Key.Visible = false;
                btnInf.Key.Enabled = false;
                btnInf.Key.Text = String.Empty;
                btnInf.Key.Tag = String.Empty;
            }

            // 有効なボタンを設定           
            Dictionary<CustomUStateButton, Int32>.Enumerator buttonInfoEnumerator = this.protocolButtonInfo.GetEnumerator();
            buttonInfoEnumerator.MoveNext();
            foreach ( var name in this.ProtocolNameList )
            {
                buttonInfoEnumerator.Current.Key.Visible = true;
                buttonInfoEnumerator.Current.Key.Enabled = true;
                buttonInfoEnumerator.Current.Key.Text = name;
                buttonInfoEnumerator.Current.Key.Tag = name; // デフォルト（倍率表示無し）名称
                

                // 選択状態のクリア
                buttonInfoEnumerator.Current.Key.CurrentState = false;
                if ( !buttonInfoEnumerator.MoveNext() )
                {
                    // 不整合（コーディングミスにより発生の可能性）
                    break;
                }
            }
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// 閉じるボタンイベント
        /// </summary>
        /// <remarks>
        /// 画面終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnMeasProtocolPanelClose_Click( object sender, EventArgs e )
        {
            if (this.Closed != null)
            {
                this.Closed();
            }
        }

        /// <summary>
        /// 分析項目ボタンクリック
        /// </summary>
        /// <remarks>
        /// 分析項目の選択変更処理を行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnMeasProto_Click( object sender, EventArgs e )
        {
            CustomUStateButton button = (CustomUStateButton)sender;
            DialogResult result;
            Int32 protocolIndex = this.protocolButtonInfo[(CustomUStateButton)sender];

            // 変更前イベント通知
            AnalisisSettingPanelSelectChangingData item = new AnalisisSettingPanelSelectChangingData(((CustomUStateButton)sender).CurrentState, false);
            this.ProtocolCheckChanging(this.protocolButtonInfo[(CustomUStateButton)sender], item);
            if (item.Cancel)
            {
                ((CustomUStateButton)sender).CurrentState = !((CustomUStateButton)sender).CurrentState;
                return;
            }

            if (chkIsEditRegistInfo.Checked)
            {
                //分析項目の登録情報を編集する場合

                this.numDilutionRatio.Value = this.protocolValueDic[protocolIndex][protocolValueItem3];
                this.numMeasTimes.Value = this.protocolValueDic[protocolIndex][protocolValueItem4];

                using (DlgEditRegistInfo dlg = new DlgEditRegistInfo(protocolIndex, (int)this.numDilutionRatio.Value, (int)this.numMeasTimes.Value))
                {
                    result = dlg.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        //分析項目の詳細を指定したので、必ず選択状態にする
                        this.numDilutionRatio.Value = dlg.DilutionRatio;
                        this.numMeasTimes.Value = dlg.MeasTimes;
                        this.protocolValueDic[protocolIndex][protocolValueItem3] = dlg.DilutionRatio;
                        this.protocolValueDic[protocolIndex][protocolValueItem4] = dlg.MeasTimes;
                        ((CustomUStateButton)sender).CurrentState = true;
                    }
                    else
                    {
                        //分析項目の詳細指定をキャンセルしたので、状態は現状維持にする
                        ((CustomUStateButton)sender).CurrentState = !((CustomUStateButton)sender).CurrentState;
                    }
                }
            }
            else
            {
                //分析項目の登録情報を編集しない場合
                this.numDilutionRatio.Value = this.protocolValueDic[protocolIndex][protocolValueItem1];
                this.numMeasTimes.Value = this.protocolValueDic[protocolIndex][protocolValueItem2];
                this.protocolValueDic[protocolIndex][protocolValueItem3] = this.protocolValueDic[protocolIndex][protocolValueItem1];
                this.protocolValueDic[protocolIndex][protocolValueItem4] = this.protocolValueDic[protocolIndex][protocolValueItem2];

                result = DialogResult.OK;
            }

            if (result == DialogResult.OK)
            {
                // 選択下限数処理
                Int32 toggleCount = (from v in this.protocolButtonInfo
                                     where v.Key.CurrentState == true
                                     select v).Count();
                if ((this.lockCount >= 0) && (toggleCount < this.lockCount))
                {
                    ((CustomUStateButton)sender).CurrentState = true;
                    //return;
                }

                // 自動希釈倍率設定
                button.Text = this.createDilString(button, this.Dilution, this.MeasTimes);

                // 選択状態設定
                this.ProtocolCheckChanged(this.protocolButtonInfo[(CustomUStateButton)sender], ((CustomUStateButton)sender).CurrentState);
            }
        }

        /// <summary>
        /// 濃度付きボタンテキスト生成
        /// </summary>
        /// <remarks>
        /// 濃度付きボタンテキスト生成します
        /// </remarks>
        /// <param name="button"></param>
        /// <returns></returns>
        private String createDilString( CustomUStateButton button, Int32 dil = 1, Int32 meastimes = 1)
        {
            String nameWithDil= (String)button.Tag;
            String dilString = string.Empty;
            String meastimesString = string.Empty;

            if ( button.CurrentState == true )
            {
                // 自動稀釈がX1の場合は名称変化しない
                if ( dil != 1 )
                {
                    dilString = String.Format( "{0}{1}",
                                                Oelco.CarisX.Properties.Resources.STRING_ANALYSISSETTINGPANEL_055,
                                                dil);
                }

                meastimesString = "[" + meastimes.ToString() + "]";

                nameWithDil += "\n" + dilString + meastimesString;
            }

            return nameWithDil;
        }

        /// <summary>
        /// 分析項目ボタン活性/非活性
        /// </summary>
        /// /// <remarks>
        /// 分析項目ボタンを急診使用有無によって活性/非活性にする
        /// </remarks>
        private void protocolIndexToButtonDicEnabled()
        {
            // trueなら分析項目ボタンを非活性にする可能性あり
            // falseならすべての分析項目ボタンを活性にする
            bool enabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();

            if (ProtocolIndexToButtonDic.Count == 0)
            {
                initializeProtocolButtonInfo();
            }

            foreach (var btn in ProtocolIndexToButtonDic)
            {
                // ボタンのキーと対応する分析項目のプロトコルを取得
                var protocal = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(btn.Key);

                if (enabledFlag)
                {
                    // 分析項目の急診使用がありの場合非活性にする
                    if (protocal.UseEmergencyMode == true)
                    {
                        btn.Value.Enabled = false;
                    }
                    else
                    {
                        btn.Value.Enabled = true;
                    }
                }
                else
                {
                    btn.Value.Enabled = true;
                }
            }
        }

        #endregion

    }
}
