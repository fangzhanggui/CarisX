using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Oelco.Common.GUI
{
    /// <summary>
    /// 機能拡張ラベル
    /// </summary>
    /// <remarks>
    /// InfragisticsのUltraLabelコントロールに対して機能を拡張したクラスです。
    /// </remarks>
    public class CustomLabel : Infragistics.Win.Misc.UltraLabel
    {
        #region [定数定義]

        /// <summary>
        /// タイマ周期(1000ms)
        /// </summary>
        private const Int32 CONST_TIMER_TICK_INTERVAL = 1000;

        /// <summary>
        /// 秒(残時間"分"表示切上げ用)
        /// </summary>
        private const Int32 REMAIN_TIME_CEILING_SECOUNDS = 59; // 59秒

        #endregion
        
        #region [インスタンス変数定義]

        /// <summary>
        /// タイマー終了イベント
        /// </summary>
        public event EventHandler TimeOver;

        /// <summary>
        /// 残り時間
        /// </summary>
        private TimeSpan remainTime;

        /// <summary>
        /// 残り時間表示テキストフォーマット(MSDN参照)
        /// </summary>
        private String timeFormatString = @"mm";

        /// <summary>
        /// タイマー強制終了フラグ
        /// </summary>
        private Boolean isAborted = false;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CustomLabel()
        {
            InitializeComponent();

            this.countDownTimer.Enabled = false;
            this.countDownTimer.Interval = CONST_TIMER_TICK_INTERVAL;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// タイマー強制終了フラグ
        /// </summary>
        public Boolean IsAborted
        {
            get
            {
                return isAborted;
            }
        }

        /// <summary>
        /// 残り時間表示テキストフォーマット 設定/取得
        /// </summary>
        public String TimeFormatString
        {
            get
            {
                return this.timeFormatString;
            }
            set
            {
                this.timeFormatString = value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// カウントダウン開始
        /// </summary>
        /// <remarks>
        /// 設定した時刻のカウントダウン表示を開始します。
        /// </remarks>
        /// <param name="span">カウント時間</param>
        public void StartCountDown( TimeSpan span )
        {
            this.isAborted = false;
            this.remainTime = span;
            this.refleshText();
            this.countDownTimer.Start();
        }

        /// <summary>
        /// カウントダウン中断
        /// </summary>
        /// <remarks>
        /// 現在表示中のカウントダウン表示削除します。
        /// </remarks>
        /// <param name="span">カウント時間</param>
        public void AbortCountDown()
        {
            if ( this.countDownTimer.Enabled )
            {
                this.isAborted = true;
                this.countDownTimer.Stop();
                this.remainTime = TimeSpan.FromSeconds(0);
                this.onTimeOver();
            }

        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// タイマ終了イベント通知
        /// </summary>
        /// <remarks>
        /// 呼び出された時、タイマ終了イベントを通知します。
        /// </remarks>
        protected void onTimeOver()
        {
            if ( TimeOver != null )
            {
                TimeOver( this, new EventArgs() );
            }
        }
        
        /// <summary>
        /// テキスト更新
        /// </summary>
        /// <remarks>
        /// 現在の残り時間を表示更新します。
        /// </remarks>
        protected void refleshText()
        {
            try
            {
                this.Text = this.remainTime.Add( TimeSpan.FromSeconds( CustomLabel.REMAIN_TIME_CEILING_SECOUNDS ) ).ToString( this.timeFormatString );
            }
            catch ( Exception )
            {
                // 時刻フォーマット書式異常
            }
        }

        /// <summary>
        /// Tickイベントハンドラ
        /// </summary>
        /// <remarks>
        /// タイマの周期イベントハンドラです。
        /// </remarks>
        /// <param name="sender">イベント通知元</param>
        /// <param name="e">イベント情報</param>
        protected void timerTick( object sender, EventArgs e )
        {
            this.remainTime -= TimeSpan.FromMilliseconds( CONST_TIMER_TICK_INTERVAL );
            if ( this.remainTime.Ticks <= 0 )
            {
                this.remainTime = TimeSpan.FromTicks( 0 );
                this.countDownTimer.Stop();
                onTimeOver();
            }

            this.refleshText();
        }

        #endregion
        
        #region Windows Form Designer generated code
   
        /// <summary>
        /// カウントダウンタイマ
        /// </summary>
        private System.Windows.Forms.Timer countDownTimer;

        /// <summary>
        /// コンテナ
        /// </summary>
        private System.ComponentModel.IContainer components;

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.countDownTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.countDownTimer.Tick += new System.EventHandler(this.timerTick);
            this.ResumeLayout(false);

        }

        #endregion

    }

}
