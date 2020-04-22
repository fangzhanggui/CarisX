using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

//using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.Misc;

//using Windows7.Multitouch.WinForms;

using Oelco.Common.Utility;

// TODO:コメント不十分

namespace Oelco.Common.GUI
{
    /// <summary>
    /// ジェスチャパネル
    /// </summary>
    /// <remarks>
    /// UltraPanelを継承し、ジェスチャによる入力を受け付けるパネルです。
    /// </remarks>
    public class GesturePanel : UltraPanel
    {
        #region [定数定義]

        /// <summary>
        /// デフォルトフリック入力判定閾値(px/sec)
        /// </summary>
        private const Int16 CONST_FLICK_THRESHOLD_DEFAULT = 1500;

        /// <summary>
        /// フリック方向
        /// </summary>
        public enum FlickDirection
        {
            /// <summary>
            /// なし
            /// </summary>
            None,
            /// <summary>
            /// 上方向
            /// </summary>
            Up,
            /// <summary>
            /// 下方向
            /// </summary>
            Down,
            /// <summary>
            /// 左方向
            /// </summary>
            Left,
            /// <summary>
            /// 右方向
            /// </summary>
            Right
        };

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 有効座標でのタッチフラグ
        /// </summary>
        Boolean mblnEnablePosTouch = false;

        /// <summary>
        /// タッチ受付エリア位置
        /// </summary>
        Point mptTouchClientLoc;

        /// <summary>
        /// スクロール有効フラグ
        /// </summary>
        private Boolean mblnEnableScroll = false;


        /// <summary>
        /// ジェスチャ開始点からの移動量
        /// </summary>
        private Point mptBeginPnlCltLoc;
        /// <summary>
        /// 最新位置
        /// </summary>
        private Point mptLastLoc;
        ///// <summary>
        ///// タッチ有効フラグ
        ///// </summary>
        //private Boolean mblnCanTouch = false;
        /// <summary>
        /// ジェスチャ有効フラグ
        /// </summary>
        private Boolean mblnCanGesture = false;
        /// <summary>
        /// 制限領域有効フラグ
        /// </summary>
        private Boolean mblnLimitRect = false;


        /// <summary>
        /// フリック利用フラグ
        /// </summary>
        private Boolean mblnUseFlick = false;

        /// <summary>
        /// フリック判定座標保持値
        /// </summary>
        private Point mptFlickJudgeTmp;
        /// <summary>
        /// フリック中フラグ
        /// </summary>
        private Boolean mblnFlickNow = false;
        /// <summary>
        /// フリック入力判定閾値(px/sec)
        /// </summary>
        private Int16 mFlickThreshold = CONST_FLICK_THRESHOLD_DEFAULT;
        /// <summary>
        /// パンジェスチャ開始時刻
        /// </summary>
        private DateTime mdtmPanStartTime;
        /// <summary>
        /// フリック入力判定タイムオーバーフラグ
        /// </summary>
        private Boolean mblnFlickTimeOver; // フリック判定タイムオーバー
        /// <summary>
        /// 現在時刻
        /// </summary>
        private DateTime mdtmNowTmp; // 現在時刻



        /// <summary>
        /// ジェスチャ開始座標
        /// </summary>
        private Point mptGesBeginLoc;

        /// <summary>
        /// スクロール代行オブジェクト
        /// </summary>
        private ScrollProxy mdlgScrollProxy = null;


        /// <summary>
        /// スクロール代行オブジェクト利用フラグ
        /// </summary>
        private Boolean mblnUseProxy = false;


        #region イベント

        /// <summary>
        /// ジェスチャ開始イベント
        /// </summary>
        public event EventHandler<GestureEventArgs> GestureBegin;

        /// <summary>
        /// ジェスチャ終了
        /// </summary>
        public event EventHandler<GestureEventArgs> GestureEnd;

        /// <summary>
        /// ズームジェスチャ
        /// </summary>
        public event EventHandler<GestureEventArgs> GestureZoom;

        /// <summary>
        /// パンジェスチャ
        /// </summary>
        public event EventHandler<GestureEventArgs> GesturePan;

        /// <summary>
        /// 回転ジェスチャ
        /// </summary>
        public event EventHandler<GestureEventArgs> GestureRotate;

        /// <summary>
        /// 2指タップジェスチャ
        /// </summary>
        public event EventHandler<GestureEventArgs> GestureTwoFingerTap;

        /// <summary>
        /// ロールオーバージェスチャ
        /// </summary>
        public event EventHandler<GestureEventArgs> GestureRollover;

        /// <summary>
        /// フリックジェスチャ
        /// </summary>
        public event EventHandler<FlickEventArgs> GestureFlick;

        #endregion

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GesturePanel()
            : base()
        {
            // RegisterTouchWindowで登録しなければ、デフォルトはGestureメッセージを受け取るようになっている。

            // タッチが有効かどうか確認する
            //try
            //{
            //    if ( RegisterTouchWindow( this.Handle, 0 ) )
            //    {
            //        mblnCanTouch = true;
            //        UnregisterTouchWindow( this.Handle, 0 );
            //    }
            //    else
            //    {
            //        mblnCanTouch = false;
            //    }
            //}
            //catch ( Exception )
            //{
            //    mblnCanTouch = false;
            //}

            // ジェスチャが有効かどうか確認する
            //try
            //{
            //    if ( RegisterGestureHandlerWindow( this.Handle, 0 ) )
            //    {
            //        mblnCanGesture = true;
            //    }
            //    else
            //    {
            //        mblnCanGesture = false;
            //    }
            //}
            //catch ( Exception ex)
            //{
            //    MessageBox.Show( ex.Message );
            //    mblnCanGesture = false;
            //}

            // タッチ受付エリア、初期はパネルサイズと同じ
            //msizTouchClientSize = this.Size;
        }

        /// <summary>
        /// 開放処理
        /// </summary>
        /// <remarks>
        /// コントロール解放時の処理を行います。
        /// </remarks>
        protected override void Dispose( Boolean disposing )
        {
            if ( disposing )
            {
                //UnregisterGestureHandlerWindow( this.Handle, 0 );
                //UnregisterTouchWindow( this.Handle, 0 );
            }
            base.Dispose( disposing );
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// タッチ受付エリア位置
        /// </summary>
        public Point TouchClientLoc
        {
            get
            {
                return mptTouchClientLoc;
            }
            set
            {
                mptTouchClientLoc = value;
            }
        }

        /// <summary>
        /// タッチ受付エリアサイズ
        /// </summary>
        Size msizTouchClientSize;

        public Size TouchClientSize
        {
            get
            {
                return msizTouchClientSize;
            }
            set
            {
                msizTouchClientSize = value;
            }
        }

        /// <summary>
        /// ジェスチャスクロール有効フラグ
        /// </summary>
        public Boolean EnableGestureScroll
        {
            get
            {
                return mblnEnableScroll;
            }
            set
            {
                mblnEnableScroll = value;
            }
        }

        /// <summary>
        /// フリック利用フラグ
        /// </summary>
        public Boolean UseFlick
        {
            get
            {
                return mblnUseFlick;
            }
            set
            {
                mblnUseFlick = value;
            }
        }

        /// <summary>
        /// フリック入力判定閾値(px/sec)
        /// </summary>
        public Int16 FlickThreshold
        {
            get
            {
                return mFlickThreshold;
            }
            set
            {
                mFlickThreshold = value;
            }
        }

        /// <summary>
        /// ジェスチャ判定領域使用フラグ
        /// </summary>
        public Boolean UseTouchClientSize
        {
            get
            {
                return mblnLimitRect;
            }
            set
            {
                mblnLimitRect = value;
            }
        }

        /// <summary>
        /// スクロール代理オブジェクト
        /// </summary>
        public ScrollProxy ScrollProxy
        {
            get
            {
                return mdlgScrollProxy;
            }
            set
            {
                mdlgScrollProxy = value;
            }
        }

        /// <summary>
        /// スクロール代行オブジェクト利用フラグ
        /// </summary>
        /// <remarks>
        /// Trueであれば、ScrollProxyを使用します。
        /// </remarks>
        public Boolean UseProxy
        {
            get
            {
                return mblnUseProxy;
            }
            set
            {
                mblnUseProxy = value;
            }
        }

        /// <summary>
        /// ジェスチャ開始座標
        /// </summary>
        public Point GesBeginLoc
        {
            get
            {
                return mptGesBeginLoc;
            }
        }

        /// <summary>
        /// ジェスチャ有効フラグ
        /// </summary>
        /// <remarks>
        /// Trueの時、ジェスチャによる入力を受付ます。
        /// </remarks>
        public Boolean EnableGesture
        {
            get
            {
                return mblnCanGesture;
            }
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// ウィンドウプロシージャ
        /// </summary>
        /// <remarks>
        /// Windwosメッセージを処理します。</br>
        /// (ジェスチャ処理の拡張を含む)
        /// </remarks>
        /// <param name="m">メッセージ</param>
        protected override void WndProc( ref Message m )
        {
            switch ( m.Msg )
            {

            case Win32API.WM_GESTURE:
                DecodeGesture( ref m );
                break;

            case Win32API.WM_GESTURENOTIFY:
                // WM_GESTURENOTIFYメッセージが発生した時、
                // SetGestureConfigを行う。

                // シングルタッチのPanメッセージ取得設定
                Win32API.GESTURECONFIG[] gescon = 
				{
					new Win32API.GESTURECONFIG( 4,0x16,0 )
				};

                //gescon.dwID = 4;			
                //gescon[0].dwID		= 4;
                //gescon[0].dwWant	= 6;
                //gescon[0].dwBlock	= 0;


                //gescon[1].dwID		= 7;
                //gescon[1].dwWant	= 1;
                //gescon[1].dwBlock	= 0;
                if ( !Win32API.SetGestureConfig( this.Handle, 0, (uint)gescon.Length, gescon, (uint)( Marshal.SizeOf( new Win32API.GESTURECONFIG() ) ) ) )
                {
                    MessageBox.Show( "GesNotErr!" );
                }
                break;
            default:
                break;
            }

            base.WndProc( ref m );
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// スクロール処理
        /// </summary>
        /// <remarks>
        /// スクロール時処理を行います。
        /// </remarks>
        /// <param name="ptLoc">ジェスチャ発生座標</param>
        private void scrollFunction( Point ptLoc )
        {
            // X方向スクロール
            Int32 xMove = ( ptLoc.X - this.GesBeginLoc.X );// + ( this.GesBeginLoc.X - this.mptBeginPnlCltLoc.X );
            // Y方向スクロール
            Int32 yMove = ( ptLoc.Y - this.GesBeginLoc.Y );// + ( this.GesBeginLoc.Y - this.mptBeginPnlCltLoc.Y );

            Point ptNewLoc = new Point();
            ptNewLoc.Y = this.mptBeginPnlCltLoc.Y + ( -yMove );
            ptNewLoc.X = this.mptBeginPnlCltLoc.X + ( -xMove );

            if ( this.UseProxy )
            {
                // 代理アクセス有効であれば、実行する。
                if ( this.mdlgScrollProxy.SetXValue != null )
                {
                    // 最大最小超過をチェックして位置設定を行う
                    if ( ( this.mdlgScrollProxy.GetXMinValue != null ) && ( ptNewLoc.X < this.mdlgScrollProxy.GetXMinValue() ) )
                    {
                        this.mdlgScrollProxy.SetXValue( this.mdlgScrollProxy.GetXMinValue() );
                    }
                    else if ( ( this.mdlgScrollProxy.GetXMaxValue != null ) && ( ptNewLoc.X > this.mdlgScrollProxy.GetXMaxValue() ) )
                    {
                        this.mdlgScrollProxy.SetXValue( this.mdlgScrollProxy.GetXMaxValue() );
                    }
                    else
                    {
                        this.mdlgScrollProxy.SetXValue( ptNewLoc.X );
                    }
                }

                if ( this.mdlgScrollProxy.SetYValue != null )
                {
                    // 最大最小超過をチェックして位置設定を行う
                    if ( ( this.mdlgScrollProxy.GetYMinValue != null ) && ( ptNewLoc.Y < this.mdlgScrollProxy.GetYMinValue() ) )
                    {
                        this.mdlgScrollProxy.SetYValue( this.mdlgScrollProxy.GetYMinValue() );
                    }
                    else if ( ( this.mdlgScrollProxy.GetYMaxValue != null ) && ( ptNewLoc.Y > this.mdlgScrollProxy.GetYMaxValue() ) )
                    {
                        this.mdlgScrollProxy.SetYValue( this.mdlgScrollProxy.GetYMaxValue() );
                    }
                    else
                    {
                        this.mdlgScrollProxy.SetYValue( ptNewLoc.Y );
                    }
                }
            }
            else
            {
                //String str = String.Format( "SCROLL!" );
                //Form1.stList.Items.Insert( 0, str );

                // パネルのスクロール処理
                // 最大最小超過をチェックして位置設定を行う
                if ( ptNewLoc.X < this.HorizontalScrollProperties.Minimum )
                {
                    this.HorizontalScrollProperties.Value = this.HorizontalScrollProperties.Minimum;
                }
                else if ( ptNewLoc.X > this.HorizontalScrollProperties.Maximum )
                {
                    this.HorizontalScrollProperties.Value = this.HorizontalScrollProperties.Maximum;
                }
                else
                {
                    this.HorizontalScrollProperties.Value = ptNewLoc.X;
                }

                // 最大最小超過をチェックして位置設定を行う
                if ( ptNewLoc.Y < this.VerticalScrollProperties.Minimum )
                {
                    this.VerticalScrollProperties.Value = this.VerticalScrollProperties.Minimum;
                }
                else if ( ptNewLoc.Y > this.VerticalScrollProperties.Maximum )
                {
                    this.VerticalScrollProperties.Value = this.VerticalScrollProperties.Maximum;
                }
                else
                {
                    this.VerticalScrollProperties.Value = ptNewLoc.Y;
                }

                //String str = String.Format( "Pan ({0},{1}) now({2},{3})", ptLoc.X, ptLoc.Y, ptNewLoc.X, ptNewLoc.Y );
                //Form1.stList.Items.Insert( 0, str );
            }

            this.mptLastLoc = ptLoc;

        }

        /// <summary>
        /// ジェスチャ開始位置記憶
        /// </summary>
        /// <remarks>
        /// ジェスチャ開始位置の設定を行います。
        /// </remarks>
        private void setBeginLoc()
        {
            if ( this.UseProxy )
            {
                if ( this.mdlgScrollProxy != null )
                {
                    // 代理アクセス(登録されていなければ動作しない)
                    if ( this.mdlgScrollProxy.GetXValue != null )
                    {
                        this.mptBeginPnlCltLoc.X = this.mdlgScrollProxy.GetXValue();
                    }
                    if ( this.mdlgScrollProxy.GetYValue != null )
                    {
                        this.mptBeginPnlCltLoc.Y = this.mdlgScrollProxy.GetYValue();
                    }
                }
            }
            else
            {
                this.mptBeginPnlCltLoc.X = this.HorizontalScrollProperties.Value;
                this.mptBeginPnlCltLoc.Y = this.VerticalScrollProperties.Value;
            }

        }

        /// <summary>
        /// フリック判定
        /// </summary>
        /// <remarks>
        /// フリック判定を行います。
        /// </remarks>
        /// <param name="gi">ジェスチャ情報</param>
        /// <returns>フリック方向</returns>
        private FlickDirection FlickCheck( Win32API.GESTUREINFO gi )
        {
            FlickDirection flick = FlickDirection.None;
            //Int16 accelerationX = (Int16)( 0x0000ffff & ( gi.ullArguments >> 32 ) ) ;
            //Int16 accelerationY = (Int16)( gi.ullArguments >> 48 );
            Int64 lNowTick = mdtmNowTmp.Ticks;//DateTime.Now.Ticks;
            const Double dblTick1Sec = 10000000; // 1秒相当のTick
            Double dblTick100mSec = TimeSpan.FromMilliseconds( 100 ).Ticks;

            // 時間差分
            Double dblTickDiff = lNowTick - mdtmPanStartTime.Ticks;

            //if ( mdtmPanStartTime.Ticks != lNowTick )

            // 判定時間100ms
            // この時間内に判定できなければタイムアウトと見なす
            if ( dblTickDiff > dblTick100mSec )
            {
                // 移動量
                Int32 xMove = ( gi.ptsLocation.x - mptFlickJudgeTmp.X );
                Int32 yMove = ( gi.ptsLocation.y - mptFlickJudgeTmp.Y );

                // 移動距離
                Double dblMoveDist = Math.Sqrt( Math.Pow( xMove, 2 ) + Math.Pow( yMove, 2 ) );

                // 移動速度（px/s)
                Double dblMoveSpd = dblMoveDist / ( dblTickDiff / dblTick1Sec );

                // 閾値超過判定
                if ( dblMoveSpd > FlickThreshold )
                {
#if DEBUGGG
					String str = String.Format( "フリック成立 速度={0} PanSt={1}, Now={2}, Diff={3} Dist ={4}", dblMoveSpd.ToString(), mdtmPanStartTime.Ticks, lNowTick, dblTickDiff, dblMoveDist );
					Form1.stList.Items.Insert( 0, str );
#endif



                    // フリック判定ON
                    // フリック方向判定
                    // 最も成分の強い方向を識別する。
                    if ( Math.Abs( xMove ) > Math.Abs( yMove ) )
                    {
                        // X方向フリック
                        if ( xMove > 0 )
                        {
                            flick = FlickDirection.Right;
                        }
                        else
                        {
                            flick = FlickDirection.Left;
                        }
                    }
                    else
                    {
                        // Y方向フリック
                        if ( yMove > 0 )
                        {
                            flick = FlickDirection.Down;
                        }
                        else
                        {
                            flick = FlickDirection.Up;
                        }
                    }

                }
                else
                {
#if DEBUGGG
					String str = String.Format( "フリックタイムアウト" );
					Form1.stList.Items.Insert( 0, str );
#endif

                    // 閾値超過しない場合、判定はタイムオーバー
                    mblnFlickTimeOver = true;
                }

            }
            return flick;
        }

        /// <summary>
        /// ジェスチャ情報読込み
        /// </summary>
        /// <remarks>
        /// ジェスチャ情報読込を行います。
        /// </remarks>
        /// <param name="m">メッセージ</param>
        private void DecodeGesture( ref Message m )
        {
            Win32API.GESTUREINFO gi = new Win32API.GESTUREINFO();
            gi.cbSize = Marshal.SizeOf( typeof( Win32API.GESTUREINFO ) );

            if ( Win32API.GetGestureInfo( m.LParam, ref gi ) )
            {
                try
                {
                    mdtmNowTmp = DateTime.Now;
#if DEBUGGG					
					String str = String.Format( "DecodeGesture Time = {5} Flags={6} GesID={0} 加速度={1}\t{2}　\t座標XY={3:00000}\t{4:00000}", gi.dwID, ( (Int16)( gi.ullArguments >> 48 ) ).ToString( "d6" ), ( (Int16)( 0x0000ffff & ( gi.ullArguments >> 32 ) ) ).ToString( "d6" ), gi.ptsLocation.x, gi.ptsLocation.y, /*DateTime.Now.ToString("dd HH:mm:ss.fffffff")*/ mdtmNowTmp.Ticks.ToString( "0000000000" ), gi.dwFlags );
					Form1.stList.Items.Insert( 0, str );
#endif

                    // ジェスチャはGID_BEGINで始まり、GID_ENDで終了する。
                    // PAN等通知されるメッセージの種類は、WM_GESTURENOTIFYが通知された際に設定している。

                    // ジェスチャイベント位置
                    Point gestureLocation = new Point( gi.ptsLocation.x, gi.ptsLocation.y );
                    switch ( gi.dwID )
                    {
                    case Win32API.GID_BEGIN:

                        System.Diagnostics.Debug.WriteLine( "GID_BEGIN" );

                        // 反応領域設定
                        // mblnLimitRect=Trueならこの領域内でのみ判定を行う。
                        Point ptCli = this.PointToClient( gestureLocation );
                        if ( mblnLimitRect )
                        {
                            mblnEnablePosTouch = (
                                ( ( ptCli.X - this.mptTouchClientLoc.X ) >= 0 ) &&
                                ( ( ptCli.X - this.mptTouchClientLoc.X ) <= ( this.msizTouchClientSize.Width ) ) &&
                                ( ( ptCli.Y - this.mptTouchClientLoc.Y ) >= 0 ) &&
                                ( ( ptCli.Y - this.mptTouchClientLoc.Y ) <= ( this.msizTouchClientSize.Height ) )
                                );
                        }
                        else
                        {
                            mblnEnablePosTouch = true;
                        }

                        //String str = String.Format( "Ges({0},{1}) Clilo({2},{3}) Size({4},{5})", ptCli.X, ptCli.Y, mptTouchClientLoc.X, mptTouchClientLoc.Y, msizTouchClientSize.Width, msizTouchClientSize.Height );
                        //Form1.stList.Items.Insert( 0, str );

                        if ( mblnEnablePosTouch )
                        {

                            // 開始位置初期化
                            mptBeginPnlCltLoc.X = 0;
                            mptBeginPnlCltLoc.Y = 0;

                            // フリック状態初期化
                            mblnFlickNow = false;
                            mblnFlickTimeOver = UseFlick ? false : true; // フリック入力使用しない設定なら最初からタイムオーバー

                            // 開始位置記憶
                            mptGesBeginLoc = gestureLocation;
                            setBeginLoc();
                            //mptLastLoc = mptGesBeginLoc;
                            if ( GestureBegin != null )
                            {
                                GestureBegin( this, new GestureEventArgs( gestureLocation ) );
                            }
                        }

                        break;
                    case Win32API.GID_END:
                        System.Diagnostics.Debug.WriteLine( "GID_END" );
                        if ( GestureEnd != null )
                        {
                            GestureEnd( this, new GestureEventArgs( gestureLocation ) );
                        }
                        break;
                    case Win32API.GID_ZOOM:
                        System.Diagnostics.Debug.WriteLine( "GID_ZOOM" );
                        if ( GestureZoom != null )
                        {
                            GestureZoom( this, new GestureEventArgs( gestureLocation ) );
                        }
                        break;
                    case Win32API.GID_PAN:
                        System.Diagnostics.Debug.WriteLine( "GID_PAN" );

                        // フリックは一度のPANにつき一回のみ発生させる。
                        if ( mblnEnablePosTouch && ( !mblnFlickNow ) && ( !mblnFlickTimeOver ) )
                        {
                            // 初回に比較用時刻取得
                            if ( gi.dwFlags == Win32API.GF_BEGIN )
                            {
                                mdtmPanStartTime = DateTime.Now;
                                mptFlickJudgeTmp.X = gi.ptsLocation.x;
                                mptFlickJudgeTmp.Y = gi.ptsLocation.y;
                            }

                            FlickDirection flick = FlickCheck( gi );
                            // フリック判定
                            if ( flick != FlickDirection.None )
                            {
                                // フリック確定
                                mblnFlickNow = true;
                                if ( GestureFlick != null )
                                {
                                    GestureFlick( this, new FlickEventArgs( gestureLocation, flick ) );
                                }
                            }
                        }

                        // スクロール許可&タッチ許可位置&フリック入力中ではない&
                        if ( mblnEnableScroll && mblnEnablePosTouch && ( !mblnFlickNow ) && mblnFlickTimeOver )
                        {
                            scrollFunction( gestureLocation );
                        }

                        if ( ( GesturePan != null ) && mblnFlickTimeOver )
                        {
                            GesturePan( this, new GestureEventArgs( gestureLocation ) );
                        }

                        break;
                    case Win32API.GID_ROTATE:
                        System.Diagnostics.Debug.WriteLine( "GID_ROTATE" );
                        if ( GestureRotate != null )
                        {
                            GestureRotate( this, new GestureEventArgs( gestureLocation ) );
                        }
                        break;
                    case Win32API.GID_TWOFINGERTAP:
                        System.Diagnostics.Debug.WriteLine( "GID_TWOFINGERTAP" );
                        if ( GestureTwoFingerTap != null )
                        {
                            GestureTwoFingerTap( this, new GestureEventArgs( gestureLocation ) );
                        }
                        break;
                    case Win32API.GID_ROLLOVER:
                        System.Diagnostics.Debug.WriteLine( "GID_ROLLOVER" );
                        if ( GestureRollover != null )
                        {
                            GestureRollover( this, new GestureEventArgs( gestureLocation ) );
                        }
                        break;
                    default:
                        // 不明なジェスチャコマンド
                        break;
                    }
                }
                finally
                {
                    Win32API.CloseGestureInfoHandle( m.LParam );
                }
            }
        }

        #endregion

    }

    /// <summary>
    /// ジェスチャイベントデータ
    /// </summary>
    public class GestureEventArgs : System.EventArgs
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 座標
        /// </summary>
        private Point location;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="location">座標</param>
        public GestureEventArgs( Point location )
        {
            this.location = location;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 座標の取得
        /// </summary>
        public Point Location
        {
            get
            {
                return this.location;
            }
        }

        #endregion

    }

    /// <summary>
    /// フリックイベントデータ
    /// </summary>
    public class FlickEventArgs : GestureEventArgs
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// フリック方向
        /// </summary>
        private GesturePanel.FlickDirection direction;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="location">座標</param>
        /// <param name="direction">フリック方向</param>
        public FlickEventArgs( Point location, GesturePanel.FlickDirection direction )
            : base( location )
        {
            this.direction = direction;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// フリック方向
        /// </summary>
        public GesturePanel.FlickDirection Direction
        {
            get
            {
                return direction;
            }
        }

        #endregion

    }
}
