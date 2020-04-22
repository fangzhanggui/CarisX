using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.AppStyling;
using Infragistics.Win.Design;
using Infragistics.Win.Misc;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

using Infragistics.Shared.Serialization;

namespace Oelco.Common.GUI
{
    /// <summary>
    /// 状態保持ボタン
    /// </summary>
    public class CustomUStateButton : CustomUButton
    {
        #region [定数定義]

        /// <summary>
        /// 状態数
        /// </summary>
        private const Int32 CONST_STATE_CNT = 2; // ステータスは2種
       
        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 状態に変化があった場合発生するイベント
        /// </summary>
        public event EventHandler<ChangeStateEventArgs> StateChanged;

        /// <summary>
        /// 状態
        /// </summary>
        private Boolean m_state = false;

        /// <summary>
        /// 通常時の外観設定
        /// </summary>
        protected AppearanceBase m_normalAppearance = new Infragistics.Win.Appearance( "false" );

        /// <summary>
        /// 状態変化時の外観設定
        /// </summary>
        protected AppearanceBase m_toggleAppearance = new Infragistics.Win.Appearance( "true" );

        /// <summary>
        /// 状態変化イベント有効フラグ
        /// </summary>
        private Boolean stateChangeEventEnable = true;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CustomUStateButton()
        {
            //CurrentState = m_state;
            this.Appearance = this.m_normalAppearance;
        }

        #endregion
        
        #region [プロパティ]
        
        // 概要:
        //     コントロールのデフォルト外観を取得または設定します。
        /// <summary>
        /// 通常時の外観設定の取得、設定
        /// </summary>
        [Browsable( true )]
        public AppearanceBase NormalAppearance
        {
            get
            {
                return this.m_normalAppearance;
            }
            set
            {
                this.m_normalAppearance = value;
            }
        }

        /// <summary>
        /// 状態変化時の外観設定の取得、設定
        /// </summary>
        [Browsable( true )]
        public AppearanceBase ToggleAppearance
        {
            get
            {
                return this.m_toggleAppearance;
            }
            set
            {
                this.m_toggleAppearance = value;
                refleshAppearance();
            }
        }

        /// <summary>
        /// ユーザーが作成したAppearanceオブジェクトのコレクションを返します。
        /// </summary>
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        [Browsable( false )]
        public new AppearancesCollection Appearances
        {
            get
            {
                return base.Appearances;
            }
        }

        //[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        //[Browsable( false )]
        //public new AppearanceBase Appearance
        //{
        //    get
        //    {
        //        return base.Appearance;
        //    }
        //    protected set
        //    {
        //        base.Appearance = value;
        //    }
        //}

        /// <summary>
        /// 状態変化イベント有効フラグの取得、設定
        /// </summary>
        public Boolean StateChangeEventEnable
        {
            get
            {
                return this.stateChangeEventEnable;
            }
            set
            {
                this.stateChangeEventEnable = value;
            }
        }

        /// <summary>
        /// 現在状態を取得/設定します。
        /// </summary>
        /// <remarks>
        /// 設定の際には状態変化イベントが発生します。
        /// </remarks>
        public Boolean CurrentState
        {
            get
            {
                return m_state;
            }
            set
            {
                Boolean blnChangeOccur = m_state != value;
                Boolean before = m_state;

                m_state = value;

                // 状態変化イベント発生
                if (blnChangeOccur)
                {
                    if ( this.stateChangeEventEnable )
                    {
                        ChangeStateEventArgs args = new ChangeStateEventArgs( before, m_state );
                        this.OnStateChanged( args );
                        m_state = args.AfterState;
                    }
                }
                
                // 状態に合わせて外観設定する。
                refleshAppearance();
           }
        }
        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// 状態変化イベント
        /// </summary>
        /// <remarks>
        /// このイベントはボタンの状態が変化した後に発生します。
        /// </remarks>
        /// <param name="before">変化前状態</param>
        /// <param name="after">変化後状態</param>
        protected virtual void OnStateChanged( ChangeStateEventArgs args )
        {
            if (StateChanged != null)
            {
                StateChanged( this, args );
            }
        }

        /// <summary>
        /// 外観設定更新
        /// </summary>
        /// <remarks>
        /// 外観設定更新を行います。
        /// </remarks>
        protected virtual void refleshAppearance()
        {
            // 状態に合わせて外観設定する。
            this.Appearance = ( m_state ) ? ToggleAppearance : NormalAppearance;
        }

        /// <summary>
        /// クリック
        /// </summary>
        /// <remarks>
        /// クリックイベント時処理を行います。
        /// </remarks>
        /// <param name="e"></param>
        protected override void OnClick( EventArgs e )
        {
            // 状態変化
            this.CurrentState = !this.CurrentState;
            base.OnClick( e );
        }

        #endregion

        #region [内部クラス]
        
        /// <summary>
        /// 状態変化イベントデータ
        /// </summary>
        public class ChangeStateEventArgs : System.EventArgs
        {
            #region [コンストラクタ/デストラクタ]

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="before">変化前ステータス</param>
            /// <param name="after">変化後ステータス</param>
            public ChangeStateEventArgs( Boolean before, Boolean after )
            {
                this.BeforeState = before;
                this.AfterState = after;
            }

            #endregion

            #region [プロパティ]
            
            /// <summary>
            /// 変化後ステータス
            /// </summary>
            public Boolean AfterState
            {
                get;
                set;
            }
            /// <summary>
            /// 変化前ステータス
            /// </summary>
            public Boolean BeforeState
            {
                get;
                set;
            }

            #endregion

        }

        #endregion
    }
}
