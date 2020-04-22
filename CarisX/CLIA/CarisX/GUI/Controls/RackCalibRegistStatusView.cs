using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.Misc;
using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;

namespace Oelco.CarisX.GUI.Controls
{
    /// <summary>
    /// ラック状態ビュー(キャリブレータ登録用)
    /// </summary>
    public partial class RackCalibRegistStatusView : UserControl
    {
        #region [定数定義]

        /// <summary>
        /// ポジション使用中
        /// </summary>
        public const String POSITION_ACTIVE = "Active";

        /// <summary>
        /// ポジション空
        /// </summary>
        public const String POSITION_UNACTIVE = "Unactive";

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 定性(PC、NC)/定量(濃度値)項目表示
        /// </summary>
        List<UltraLabel> rackPosString = new List<UltraLabel>();

        /// <summary>
        /// ラックポジション状態
        /// </summary>
        ProtocolRegistStatus[] positionStatus = new ProtocolRegistStatus[CarisXConst.RACK_POS_COUNT * 2];

        /// <summary>
        /// 使用状態のポジションの外観設定規定値
        /// </summary>
        private Infragistics.Win.Appearance ActiveAppearance = new Infragistics.Win.Appearance( POSITION_ACTIVE )
        {
            BackColor = Color.Transparent,
            ForeColor = Color.White,
            ImageBackground = Oelco.CarisX.Properties.Resources.Image_ControlRackPos_Blue,
            TextHAlign = HAlign.Center,
            TextVAlign = VAlign.Middle
        };

        /// <summary>
        /// 未使用状態のポジションの外観設定規定値
        /// </summary>
        private Infragistics.Win.Appearance UnactiveAppearance = new Infragistics.Win.Appearance( POSITION_UNACTIVE )
        {
            BackColor = Color.Transparent,
            ForeColor = Color.White,
            ImageBackground = Oelco.CarisX.Properties.Resources.Image_ControlRackPos_Gray,
            TextHAlign = HAlign.Center,
            TextVAlign = VAlign.Middle
        };
        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackCalibRegistStatusView()
        {
            InitializeComponent();

            this.rackPosString.Add( this.lblRack1Pos1Dsp );
            this.rackPosString.Add( this.lblRack1Pos2Dsp );
            this.rackPosString.Add( this.lblRack1Pos3Dsp );
            this.rackPosString.Add( this.lblRack1Pos4Dsp );
            this.rackPosString.Add( this.lblRack1Pos5Dsp );
            this.rackPosString.Add( this.lblRack2Pos6Dsp );
            this.rackPosString.Add( this.lblRack2Pos7Dsp );
            this.rackPosString.Add( this.lblRack2Pos8Dsp );
            this.rackPosString.Add( this.lblRack2Pos9Dsp );
            this.rackPosString.Add( this.lblRack2Pos10Dsp );

            this.lblRack1Id.Text = Oelco.CarisX.Properties.Resources.STRING_RACKCALIBREGISTSTATUSVIEW_000;
            this.lblRack1IdDsp.Text = Oelco.CarisX.Properties.Resources.STRING_RACKCALIBREGISTSTATUSVIEW_001;
            this.lblRack2Id.Text = Oelco.CarisX.Properties.Resources.STRING_RACKCALIBREGISTSTATUSVIEW_002;
            this.lblRack2IdDsp.Text = Oelco.CarisX.Properties.Resources.STRING_RACKCALIBREGISTSTATUSVIEW_003;
            this.lblRack1Pos1Dsp.Text = Oelco.CarisX.Properties.Resources.STRING_RACKCALIBREGISTSTATUSVIEW_004;
            this.lblRack1Pos2Dsp.Text = Oelco.CarisX.Properties.Resources.STRING_RACKCALIBREGISTSTATUSVIEW_005;
            this.lblRack1Pos3Dsp.Text = Oelco.CarisX.Properties.Resources.STRING_RACKCALIBREGISTSTATUSVIEW_006;
            this.lblRack1Pos4Dsp.Text = Oelco.CarisX.Properties.Resources.STRING_RACKCALIBREGISTSTATUSVIEW_007;
            this.lblRack1Pos5Dsp.Text = Oelco.CarisX.Properties.Resources.STRING_RACKCALIBREGISTSTATUSVIEW_008;
            this.lblRack2Pos6Dsp.Text = Oelco.CarisX.Properties.Resources.STRING_RACKCALIBREGISTSTATUSVIEW_009;
            this.lblRack2Pos7Dsp.Text = Oelco.CarisX.Properties.Resources.STRING_RACKCALIBREGISTSTATUSVIEW_010;
            this.lblRack2Pos8Dsp.Text = Oelco.CarisX.Properties.Resources.STRING_RACKCALIBREGISTSTATUSVIEW_011;
            this.lblRack2Pos9Dsp.Text = Oelco.CarisX.Properties.Resources.STRING_RACKCALIBREGISTSTATUSVIEW_012;
            this.lblRack2Pos10Dsp.Text = Oelco.CarisX.Properties.Resources.STRING_RACKCALIBREGISTSTATUSVIEW_013;
            
            // すべてのラックポジションの外観設定コレクションを追加
            this.AllPositionAppearances.Add( POSITION_UNACTIVE );
            this.AllPositionAppearances.Add( POSITION_ACTIVE );
            

            // 全ポジションの外観設定の変更時のイベント登録
            this.AllPositionAppearances.SubObjectPropChanged += ( propChange ) =>
            {
                foreach ( UltraLabel label in this.rackPosString )
                {
                    label.Appearance = this.rackPosString[0].Appearance;
                }
            };

            this.Clear();
        }

        

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラックID(ラック上段)の取得、設定
        /// </summary>
        [DefaultValue( "" )]
        public String Rack1Id
        {
            get
            {

                return this.lblRack1IdDsp.Text;
            }
            set
            {
                this.lblRack1IdDsp.Text = value;

                // ラックの表示切替
                Boolean visible = !String.IsNullOrEmpty( value );
                this.pnlRack1.Visible = visible;
                this.lblRack1Id.Visible = visible;
                this.lblRack1Id.Visible = visible;
                this.lblRack1IdDsp.Visible = visible;
            }
        }

        /// <summary>
        /// ラックID(ラック下段)の取得、設定
        /// </summary>
        [DefaultValue( "" )]
        public String Rack2Id
        {
            get
            {
                return this.lblRack2IdDsp.Text;
            }
            set
            {
                this.lblRack2IdDsp.Text = value;

                // ラックの表示切替
                Boolean visible = !String.IsNullOrEmpty( value );
                this.pnlRack2.Visible = visible;
                this.lblRack2Id.Visible = visible;
                this.lblRack2Id.Visible = visible;
                this.lblRack2IdDsp.Visible = visible;
            }
        }

        /// <summary>
        /// ラックポジション値の取得、設定
        /// </summary>
        [RefreshProperties( RefreshProperties.Repaint )]
        protected String this[Int32 i]
        {
            get
            {
                return this.rackPosString[i].Text;
            }
            set
            {
                this.rackPosString[i].Text = value;
                if ( !String.IsNullOrEmpty( value ) )
                {
                    if ( this.positionStatus[i] != ProtocolRegistStatus.Empty )
                    {
                        if ( this.positionStatus[i] == ProtocolRegistStatus.Uncertain )
                        {
                            this.rackPosString[i].Appearance = this.PositionActiveAppearance;
                        }
                        else
                        {
                            this.rackPosString[i].Appearance = this.PositionUnactiveAppearance;
                        }
                    }
                }
                else
                {
                    this.rackPosString[i].Appearance = this.PositionUnactiveAppearance;
                }
            }
        }

        /// <summary>
        /// ラックポジション外観設定の取得、設定
        /// </summary>
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
        [RefreshProperties( RefreshProperties.Repaint )]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AppearanceBase AllPositionApeeranace
        {

            get
            {
                return this.rackPosString[0].Appearance;
            }
            set
            {
                foreach ( UltraLabel label in this.rackPosString )
                {
                    label.Appearance = value;
                }
            }
        }

        /// <summary>
        /// ラックポジション外観設定コレクションの取得
        /// </summary>
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
        [RefreshProperties( RefreshProperties.Repaint )]
        public AppearancesCollection AllPositionAppearances
        {
            get
            {
                return this.rackPosString[0].Appearances;
            }
        }

        /// <summary>
        /// 登録候補時の外観設定
        /// </summary>
        [AmbientValue( typeof( AppearanceBase ), "" )]
        public AppearanceBase PositionActiveAppearance
        {
            get
            {
                return this.AllPositionAppearances.Contains( POSITION_ACTIVE ) ? this.AllPositionAppearances[POSITION_ACTIVE] : this.ActiveAppearance;
            }
            set
            {
                if ( !this.AllPositionAppearances.Contains( POSITION_ACTIVE ) )
                {
                    this.AllPositionAppearances.Add( value );
                }
            }
        }

        /// <summary>
        /// 空、登録済み時の外観設定
        /// </summary>
        [AmbientValue( typeof( AppearanceBase ), "" )]
        public AppearanceBase PositionUnactiveAppearance
        {
            get
            {
                return this.AllPositionAppearances.Contains( POSITION_UNACTIVE ) ? this.AllPositionAppearances[POSITION_UNACTIVE] : this.UnactiveAppearance;
            }
            set
            {
                if ( !this.AllPositionAppearances.Contains( POSITION_UNACTIVE ) )
                {
                    this.AllPositionAppearances.Add( value );
                }
            }
        }

        /// <summary>
        /// 要素の数の取得
        /// </summary>
        public Int32 Length
        {
            get
            {
                return this.rackPosString.Count;
            }
        }

        /// <summary>
        /// ラックIDのタイトルの取得、設定
        /// </summary>
        public String RackIDTitle
        {
            get
            {
                return this.lblRack1Id.Text;
            }
            set
            {
                this.lblRack1Id.Text = value;
                this.lblRack2Id.Text = value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// クリア
        /// </summary>
        /// <remarks>
        /// ラックID、ポジション情報をクリアします
        /// </remarks>
        public void Clear()
        {
            // ラック名
            this.Rack1Id = null;
            this.Rack2Id = null;

            // ラックポジション値
            for ( Int32 i = 0; i < this.Length; i++ )
            {
                this[i] = null;
            }

            this.positionStatus = new ProtocolRegistStatus[CarisXConst.RACK_POS_COUNT * 2];

        }

        /// <summary>
        /// ラック情報の追加
        /// </summary>
        /// <remarks>
        /// ラック情報を追加します
        /// </remarks>
        /// <param name="rackId">ラックID</param>
        /// <param name="info">ラックポジションデータ</param>
        public void AddRackInfo( String rackId, Tuple<String, ProtocolRegistStatus>[] info )
        {
            Int32 rackPosition = String.IsNullOrEmpty( this.Rack1Id ) ? 0 : CarisXConst.RACK_POS_COUNT;
            for ( Int32 i = 0; i < info.Length; i++ )
            {
                this.positionStatus[i + rackPosition] = info[i].Item2;
                if ( info[i].Item2 == ProtocolRegistStatus.Registerd ||
                    info[i].Item2 == ProtocolRegistStatus.Uncertain )
                {
                    this[i + rackPosition] = info[i].Item1;
                }
            }

            if ( rackPosition == 0 )
            {
                this.Rack1Id = rackId;
            }
            else
            {
                this.Rack2Id = rackId;
            }
        }

        /// <summary>
        /// ラック情報の設定
        /// </summary>
        /// <remarks>
        /// ラック情報を設定します
        /// </remarks>
        /// <param name="startRackId"></param>
        /// <param name="info"></param>
        public void SetRackInfo( CarisXIDString startRackId, List<Tuple<String, ProtocolRegistStatus>>[] info )
        {
            if ( startRackId != null )
            {
                CarisXIDString rackId = startRackId.DispPreCharString;
                this.Rack1Id = rackId.DispPreCharString;
                if ( info.Count() > 1 && info[1].Count( ( data ) => data.Item2 != ProtocolRegistStatus.Empty ) > 0 )
                {
                    rackId.Value += 1;
                    this.Rack2Id = rackId.DispPreCharString;
                }
                else
                {
                    this.Rack2Id = null;
                }
            }
            else
            {
                this.Rack1Id = null;
            }

            for ( Int32 i = 0; i < ( CarisXConst.RACK_POS_COUNT * 2 ); i++ )
            {
                Tuple<String, ProtocolRegistStatus> posStatusInfo;
                if ( ( info.Count() >= 1 && i < CarisXConst.RACK_POS_COUNT ) || ( info.Count() > 1 && i >= CarisXConst.RACK_POS_COUNT ) )
                {
                    posStatusInfo = info[i / CarisXConst.RACK_POS_COUNT][i % CarisXConst.RACK_POS_COUNT];
                }
                else
                {
                    posStatusInfo = new Tuple<string, ProtocolRegistStatus>( string.Empty, ProtocolRegistStatus.Empty );
                }

                switch ( posStatusInfo.Item2 )
                {
                case ProtocolRegistStatus.Empty:
                    this.positionStatus[i] = posStatusInfo.Item2;
                    this[i] = String.Empty;
                    break;
                case ProtocolRegistStatus.Uncertain:
                case ProtocolRegistStatus.Registerd:
                    this.positionStatus[i] = posStatusInfo.Item2;
                    this[i] = posStatusInfo.Item1;
                    break;
                default:
                    break;
                }

            }
        }

        /// <summary>
        /// ラックポジション情報の設定
        /// </summary>
        /// <remarks>
        /// ラックポジション情報を設定します
        /// </remarks>
        /// <param name="rackPos">ラックポジション</param>
        /// <param name="iConc">ラックポジションの濃度値</param>
        public void SetTextConRackInfo(Int32 rackPos, String conc)
        {
            this[rackPos] = conc;
        }

        /// <summary>
        /// ラックポジション情報の取得
        /// </summary>
        /// <remarks>
        /// ラックポジション情報を取得します
        /// </remarks>
        /// <param name="rackPos">ラックポジション</param>
        /// <returns>ラックポジションの濃度値</returns>
        public string GetTextConRackInfo(Int32 rackPos)
        {
            return this[rackPos];
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// 登録候補時の外観設定を使用状態の取得
        /// </summary>
        /// <remarks>
        /// 登録候補時の外観設定を使用状態を取得します。</br>
        /// デザイナ
        /// </remarks>
        /// <returns></returns>
        public bool ShouldSerializePositionActiveAppearance()
        {
            return this.PositionActiveAppearance != this.ActiveAppearance;
        }

        /// <summary>
        /// 登録候補時の外観設定を使用状態の設定
        /// </summary>
        /// <remarks>
        /// 登録候補時の外観設定を使用状態に設定します。</br>
        /// </remarks>
        public void ResetPositionActiveAppearance()
        {
            this.PositionActiveAppearance = this.ActiveAppearance;
        }

        /// <summary>
        /// 空、登録済み時の外観設定を未使用状態の取得
        /// </summary>
        /// <remarks>
        /// 空、登録済み時の外観設定を未使用状態を取得します。</br>
        /// </remarks>
        /// <returns></returns>
        public bool ShouldSerializePositionUnactiveAppearance()
        {
            return this.PositionUnactiveAppearance != this.UnactiveAppearance;
        }

        /// <summary>
        /// 空、登録済み時の外観設定を未使用状態の設定
        /// </summary>
        /// <remarks>
        /// 空、登録済み時の外観設定を未使用状態に設定します。</br>
        /// </remarks>
        public void ResetPositionUnactiveAppearance()
        {
            this.PositionUnactiveAppearance = this.UnactiveAppearance;
        }
        #endregion

    }
}
