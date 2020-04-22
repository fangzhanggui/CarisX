using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Oelco.Common.GUI
{
    /// <summary>
    /// 点滅ボタンクラス
    /// </summary>
    public class BlinkButton : CustomUStateButton
    {
        #region [定数定義]

        ///// <summary>
        ///// 点滅段階設定デフォルト値
        ///// </summary>
        //private const Int32 CONST_BLINK_STEP = 20;

        /// <summary>
        /// 点滅タイマTick時間(全インスタンス共用）
        /// </summary>
        private const Int32 CONST_BLINK_TIMER_PACE = 10;
        #endregion

        #region [クラス変数定義]

        /// <summary>
        /// タイマーオブジェクト(全インスタンスで共用)
        /// </summary>
        static private System.Windows.Forms.Timer baseTimer;

        #endregion

        #region [インスタンス変数定義]


        /// <summary>
        /// 周期処理実行必要数
        /// </summary>
        private Int32 cycleCount = 0;
        /// <summary>
        /// 周期処理実行数
        /// </summary>
        private Int32 cycleStep = 0;
        /// <summary>
        /// 周期処理実行条件Tickカウント周期
        /// </summary>
        private Int32 blinkCycle = 0;
        /// <summary>
        /// Tick周期カウント
        /// </summary>
        private Int32 tickCounter = 0;
        /// <summary>
        /// 元画像サイズ
        /// </summary>
        private Rectangle backSize;
        /// <summary>
        /// 点滅画像サイズ
        /// </summary>
        private Rectangle blinkSize;

        /// <summary>
        /// α値設定カラーマトリクス
        /// </summary>
        private ColorMatrix alphaMatrix = new ColorMatrix();
        /// <summary>
        /// α値設定画像属性
        /// </summary>
        private ImageAttributes alphaAttribute = new ImageAttributes();

        /// <summary>
        /// 点滅後対象画像
        /// </summary>
        private Image blinkToImage;
        /// <summary>
        /// 点滅前対象画像
        /// </summary>
        private Image blinkFromImage;

        /// <summary>
        /// 点滅中背景画像
        /// </summary>
        private Image blinkingImage;

        /// <summary>
        /// 元のボタン画像
        /// </summary>
        private Image tempImage;

        ///// <summary>
        ///// 点滅段階
        ///// </summary>
        //private Int32 blinkLevel;

        /// <summary>
        /// 逆点滅フラグ
        /// </summary>
        private Boolean blinkReverse = false;

        /// <summary>
        /// 点滅中フラグ
        /// </summary>
        private Boolean nowBlink = false;

        /// <summary>
        /// 点滅所要時間
        /// </summary>
        private Int32 blinkTime = 1000;

        /// <summary>
        /// 外観設定
        /// </summary>
        private Infragistics.Win.AppearanceBase appearanceTemp;
        ///// <summary>
        ///// 点滅段階設定
        ///// </summary>
        //private Int32 miBlinkStep = CONST_BLINK_STEP;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BlinkButton()
            : base()
        {
            if ( BlinkButton.baseTimer == null )
            {
                BlinkButton.baseTimer = new System.Windows.Forms.Timer();
                BlinkButton.baseTimer.Interval = BlinkButton.CONST_BLINK_TIMER_PACE;
                BlinkButton.baseTimer.Start(); // タイマは常に動作する。
            }
            //InitializeComponent();
            BlinkButton.baseTimer.Tick += new System.EventHandler( this.baseTimerTick );
        }


        /// <summary>
        /// Dispose処理
        /// </summary>
        /// <remarks>
        /// Dispose処理を行います。
        /// </remarks>
        /// <param name="disposing"></param>
        protected override void Dispose( Boolean disposing )
        {
            if ( disposing )
            {
                BlinkButton.baseTimer.Tick -= new System.EventHandler( this.baseTimerTick );
            }
            base.Dispose( disposing );
        }

        #endregion

        #region [プロパティ]

        ///// <summary>
        ///// 点滅段階
        ///// </summary>
        //public Int32 BlinkStep
        //{
        //    get
        //    {
        //        return miBlinkStep;
        //    }
        //    set
        //    {
        //        if ( value != 0 )
        //        {
        //            miBlinkStep = value;

        //            // 点滅段階設定から、タイマのインターバル設定する。
        //            Int32 iSetStep = blinkTime / miBlinkStep;
        //            if ( iSetStep == 0 )
        //            {
        //                iSetStep = 1;
        //            }
        //            blinkLevel = 0;

        //            // タイマの全インスタンス共有により、TickのInterval設定をTick回数当たりの応答頻度に変更する。
        //            //mTimerObj.Interval = iSetStep;
        //        }
        //    }
        //}

        /// <summary>
        /// 点滅中フラグ取得
        /// </summary>
        public Boolean IsBlink
        {
            get
            {
                return this.nowBlink;
            }
        }
        #endregion
        
        #region [publicメソッド]

        /// <summary>
        /// 点滅開始
        /// </summary>
        /// <remarks>
        /// 点滅色から、点滅背景色までを、点滅時間毎に往復する。
        /// </remarks>
        /// <param name="clrBack">点滅色</param>
        /// <param name="clrBlink">点滅背景色</param>
        /// <param name="blinkTime">点滅時間</param>
        /// <param name="blinkCycle">実行周期</param>
        public void BlinkStart( Image clrBack, Image clrBlink, Int32 blinkTime, Int32 blinkCycle )
        {
            if ( this.IsBlink == false )
            {
                // 動作単位回数を計算
                // cycleCount回の動作で終了する。
                if ( blinkCycle <= 0 )
                {
                    // 0以下を禁止する。
                    blinkCycle = 1;
                }

                // 所要時間に対して、ベースのタイマTick周期*実行周期が何回必要か計算
                this.cycleCount = blinkTime / ( BlinkButton.CONST_BLINK_TIMER_PACE * blinkCycle );
                if ( this.cycleCount <= 0 )
                {
                    // 0以下を禁止する
                    this.cycleCount = 1;
                }

                // blinkCycle回に1度cycleStepがカウントアップされる。
                this.blinkCycle = blinkCycle;

                // cycleStep回目の動作、これを元に合成画像の透過率が計算される。
                this.cycleStep = 0;

                // 点滅してからTick動作が行われた回数
                // tickCounter%blinkCycle==0であれば合成画像が変わる。
                this.tickCounter = 0;

                // 点滅所要時間
                this.blinkTime = blinkTime;

                // 画像の保持
                this.tempImage = Appearance.ImageBackground;
                this.blinkFromImage = (Image)clrBack.Clone();
                this.blinkToImage = (Image)clrBlink.Clone();

                // 自身の外観をクローンに置き換える。
                this.appearanceTemp = this.Appearance;
                this.Appearance = (Infragistics.Win.AppearanceBase)this.Appearance.Clone();
                this.Appearance.ImageBackground = clrBack;

                // サイズ保持
                this.backSize = new Rectangle( 0, 0, clrBack.Size.Width, clrBack.Size.Height );
                this.blinkSize = new Rectangle( 0, 0, clrBlink.Size.Width, clrBlink.Size.Height );

                // 状態初期化
                this.blinkReverse = false;
                this.nowBlink = true;

            }
        }

        /// <summary>
        /// 点滅終了
        /// </summary>
        /// <remarks>
        /// 点滅を終了します。
        /// </remarks>
        public void BlinkEnd()
        {
            if ( this.nowBlink )
            {
                this.nowBlink = false;
                this.imageClean();
                this.Appearance = this.appearanceTemp;
                this.Appearance.ImageBackground = this.tempImage;
                this.tempImage = null;

                // 外観の更新
                base.refleshAppearance();
            }
        }

        #endregion

        /// <summary>
        /// 外観設定更新
        /// </summary>
        /// <remarks>
        /// 外観設定を更新します。
        /// </remarks>
        protected override void refleshAppearance()
        {
            // 点滅動作中は外観の更新をしない。
            // 外観の更新をしてしまうと点滅途中段階の画像と差し替えられ、点滅終了時にDisposeされてしまう。
            if ( !this.nowBlink )
            {
                base.refleshAppearance();
            }
        }

        #region [privateメソッド]

        /// <summary>
        /// 保持画像リソース開放
        /// </summary>
        /// <remarks>
        /// 保持している画像リソースを解放します。
        /// </remarks>
        private void imageClean()
        {
            if ( this.blinkingImage != null )
            {
                this.blinkingImage.Dispose();
                this.blinkingImage = null;
            }
        }

        /// <summary>
        /// 点滅タイマ処理
        /// </summary>
        /// <remarks>
        /// ベースとなるタイマのTick周期により実行される。
        /// 画像の合成・設定を行う。
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void baseTimerTick( object sender, EventArgs e )
        {
            // 点滅動作中以外は無視する。
            if ( this.nowBlink )
            {
                // 画像変化周期
                this.tickCounter++;
                if ( ( this.tickCounter % this.blinkCycle ) == 0 )
                {
                    // サイクル方向によりインクリメント/デクリメント切り替え
                    this.cycleStep += this.blinkReverse ? -1 : 1;

                    // 透過率の動作方向
                    if ( this.cycleStep >= this.cycleCount )
                    {
                        this.blinkReverse = true;
                    }
                    else if ( this.cycleStep <= 0 )
                    {
                        this.blinkReverse = false;
                    }

                    // 画像生成(あれば破棄)
                    this.Appearance.ImageBackground = null;
                    this.imageClean();

                    try
                    {
                        // 元画像をコピー
                        this.blinkingImage = new Bitmap( this.tempImage.Size.Width, this.tempImage.Size.Height, PixelFormat.Format32bppArgb );
                        using ( Graphics g = Graphics.FromImage( this.blinkingImage ) )
                        {
                            // 元画像、点滅対象画像をα値考慮で合成する。

                            // 点滅対象画像を上からかぶせる。
                            float alpha = (float)Math.Round( (Double)this.cycleStep / this.cycleCount, 2 );
                            float revAlpha = 1.00f - alpha;

                            this.alphaMatrix.Matrix00 = 1;
                            this.alphaMatrix.Matrix11 = 1;
                            this.alphaMatrix.Matrix22 = 1;
                            this.alphaMatrix.Matrix33 = revAlpha; // ここにα値への倍率をかける
                            this.alphaMatrix.Matrix44 = 1;

                            // 元画像を合成
                            this.alphaAttribute.SetColorMatrix( alphaMatrix );
                            g.DrawImage( this.blinkFromImage, this.backSize, 0f, 0f, this.blinkFromImage.Width, this.blinkFromImage.Height, GraphicsUnit.Pixel, this.alphaAttribute );

                            // 点滅対象画像を合成
                            alphaMatrix.Matrix33 = alpha;
                            this.alphaAttribute.SetColorMatrix( alphaMatrix );
                            g.DrawImage( this.blinkToImage, this.blinkSize, 0f, 0f, this.blinkToImage.Width, this.blinkToImage.Height, GraphicsUnit.Pixel, this.alphaAttribute );
                        }
                        this.Appearance.ImageBackground = this.blinkingImage;
                    }
                    catch ( Exception )
                    {
                        // 終了タイミングにより失敗の可能性
                    }
                }
            }

        }
        
        #endregion
    }
}
