using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Threading;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using Oelco.Common.Utility;


namespace Oelco.Common.GUI
{
    /// <summary>
    /// フェード効果フォーム
    /// </summary>
    public class FormTransitionBase : FormBase
    {
        #region 透明時黒描画対策(API版)

        //private Boolean IsOpaqueLayered = true;
        //public new Double Opacity
        //{
        //    get
        //    {
        //        if ( IsOpaqueLayered )
        //        {
        //            return 1;
        //        }
        //        if ( base.Opacity == 1 )
        //        {
        //            //return base.Opacity;
        //            return 2;
        //        }
        //        return base.Opacity;
        //    }
        //    set
        //    {
        //        Byte alpha = (Byte)( 255 * value );
        //        if ( alpha == 255 )
        //        {
        //            // SetLayeredWindowAttributes( new System.Runtime.InteropServices.HandleRef( this, this.Handle ), 0, alpha, LWA_ALPHA );
        //            SetLayeredWindowAttributes( this.Handle, 0, alpha, LWA_ALPHA );
        //            //Boolean blnRes = SetLayeredWindowAttributes( this.Handle, 0, alpha, LWA_ALPHA );
        //            IsOpaqueLayered = true;
        //            //base.Opacity = value;
        //            //base.Opacity = value;
        //        }
        //        else
        //        {
        //            //Boolean blnRes = SetLayeredWindowAttributes( this.Handle, 0, alpha, LWA_ALPHA );
        //            IsOpaqueLayered = false;
        //            base.Opacity = value;
        //        }
        //    }
        //}
        //private const UInt32 LWA_ALPHA = 2;
        //[DllImport( "user32" )]
        //[return: MarshalAs( UnmanagedType.Bool )]
        //static extern Boolean SetLayeredWindowAttributes( IntPtr hwnd, UInt32 crKey, Byte bAlpha, UInt32 dwFlags );

        #endregion

        #region [定数定義]

        /// <summary>
        /// 表示種別
        /// </summary>
        protected enum ShowKind
        {
            /// <summary>
            /// 表示
            /// </summary>
            Show,
            /// <summary>
            /// 非表示
            /// </summary>
            Hide,
            /// <summary>
            /// 閉じる
            /// </summary>
            Close

        }

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 表示種別
        /// </summary>
        protected ShowKind m_ShowKind = ShowKind.Show;

        /// <summary>
        /// 表示効果タイマ
        /// </summary>
        protected Timer m_ShowTimer = new Timer();

        /// <summary>
        /// 画像表示効果タイマ
        /// </summary>
        protected Timer m_ShowTimerPict = new Timer();

        /// <summary>
        /// 初期化済みフラグ
        /// </summary>
        protected Boolean m_blnInitialized = false;

        /// <summary>
        /// フェード表示完了イベント
        /// </summary>
        public event EventHandler OnFadeShown;

        //		protected System.Threading.ManualResetEvent m_evClose = new System.Threading.ManualResetEvent( false );

        #region 透明時黒描画対策(SS版)

        /// <summary>
        /// 画像レイヤ
        /// </summary>
        protected FormPictLayer m_pctLayer;// = new FormPictLayer();

        /// <summary>
        /// 画像レイヤ使用フラグ
        /// </summary>
        protected Boolean m_blnUsePictLayer = false;

        #endregion

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormTransitionBase()
        {
            //base.Opacity = 0;
            //this.Opacity = 0;
            //this.DoubleBuffered = true; // 黒いチラつき防止
            m_ShowTimer.Tick += ShowTimerTick;
            m_ShowTimerPict.Tick += ShowTimerPictTick;

            this.StartPosition = FormStartPosition.Manual;
        }


        /// <summary>
        /// 解放処理
        /// </summary>
        /// <remarks>
        /// インスタンス解放時の処理を行います。
        /// </remarks>
        /// <param name="disposing"></param>
        protected override void Dispose(Boolean disposing)
        {

            if (disposing)
            {
                m_ShowTimer.Tick -= ShowTimerTick;
                m_ShowTimerPict.Tick -= ShowTimerPictTick;
                m_ShowTimer.Dispose();
                m_ShowTimerPict.Dispose();
                if (m_pctLayer != null)
                {
                    m_pctLayer.Dispose();
                }

            }

            base.Dispose(disposing);

        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 表示状態 取得
        /// </summary>
        public Boolean IsVisible
        {
            get
            {
#if false // TODO:表示高速化対応後はこちらの判定に移行する
                
                return this.Location.X < this.OutsideX;
#else
                return this.Opacity >= 1.0d && this.Visible;
#endif
            }
        }

        /// <summary>
        /// ダイアログ結果の取得、設定
        /// </summary>
        public new DialogResult DialogResult
        {
            get;
            set;
        }

        /// <summary>
        /// コントロール作成必要情報の取得
        /// </summary>
        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                const Int32 WS_EX_TOOLWINDOW = 0x00000080;

                // ExStyle に WS_EX_TOOLWINDOW ビットを立てる事で、
                // ALT+TABのメニューで非表示となる。
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | WS_EX_TOOLWINDOW;

                return cp;
            }
        }

        /// <summary>
        /// 全ての画面の幅の合計
        /// </summary>
        protected Int32 OutsideX
        {
            get
            {
                //return 1200;
                Int32 xPos = 0;
                foreach (var scr in Screen.AllScreens)
                {
                    xPos += scr.WorkingArea.Width;
                }

                return xPos;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コントロール最前面移動
        /// </summary>
        /// <remarks>
        /// コントロールを z オーダーの最前面へ移動します。
        /// </remarks>
        public new void BringToFront()
        {
            base.BringToFront();

            if (this.m_pctLayer != null)
            {
                this.m_pctLayer.BringToFront();
            }
        }

        /// <summary>
        /// フェードインモーダル表示
        /// </summary>
        /// <remarks>
        /// フェードインモーダル表示を行います。
        /// </remarks>
        /// <param name="captScreenRect"></param>
        /// <returns></returns>
        public virtual DialogResult ShowDialog(Rectangle captScreenRect)
        {
            //m_evClose.Reset();


            if (m_pctLayer == null)
            {
                m_pctLayer = new FormPictLayer();
                m_pctLayer.Left = this.OutsideX;
                m_pctLayer.Show();
                m_pctLayer.Opacity = 0.01d;
                //                m_pctLayer.Visible = false;
            }

            // Bitmapオブジェクトにスクリーン・キャプチャ
            Bitmap myBmp = new Bitmap(captScreenRect.Width,
                      captScreenRect.Height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(myBmp))
            {
                g.CopyFromScreen(captScreenRect.X, captScreenRect.Y, 0, 0,
                captScreenRect.Size, CopyPixelOperation.SourceCopy);
            }

            m_pctLayer.CleanResource(); // もし画像を保持していれば消す
            m_pctLayer.Size = this.Size;
            m_pctLayer.StartPosition = FormStartPosition.Manual;
            m_pctLayer.ShowImage = myBmp;
            m_pctLayer.Opacity = 1.0;
            //m_pctLayer.Visible = true;



            m_blnUsePictLayer = true;
            //m_evClose.Reset();
            //this.Opacity = 1.0;
            //if ( m_blnInitialized )
            //{
            //this.Visible = true;
            m_ShowKind = ShowKind.Show;
            this.m_ShowTimerPict.Interval = 10;
            this.m_ShowTimerPict.Enabled = true;
            //}
            //else
            //{
            m_pctLayer.Location = captScreenRect.Location;
            m_pctLayer.TopMost = true;
            //m_pctLayer.Show();
            m_pctLayer.Activate();
            //while ( true )
            //{
            Application.DoEvents();
            System.Threading.Thread.Sleep(30);
            //}
            //}

            DialogResult result = base.ShowDialog();

            m_pctLayer.Hide();


            return result;
            //return base.ShowDialog();

        }

        /// <summary>
        /// フェードイン表示
        /// </summary>
        /// <remarks>
        /// 表示対象領域の画像をキャプチャし、全面に表示させた後、透明度を落としていく事によるフェード表示を行います。
        /// </remarks>
        /// <param name="captScreenRect">表示対象領域</param>
        public virtual void Show(Rectangle captScreenRect)
        {
            this.m_ShowTimer.Enabled = false;

            if (m_pctLayer == null)
            {
                m_pctLayer = new FormPictLayer();
                m_pctLayer.MaximumSize = m_pctLayer.MinimumSize = captScreenRect.Size;
                m_pctLayer.Size = captScreenRect.Size;
                m_pctLayer.StartPosition = FormStartPosition.Manual;
                m_pctLayer.Left = this.OutsideX;
                m_pctLayer.Opacity = 0d;
                //m_pctLayer.Visible = false;
                m_pctLayer.Show();
            }


            // Bitmapオブジェクトにスクリーン・キャプチャ
            Bitmap myBmp = new Bitmap(captScreenRect.Width,
                      captScreenRect.Height, PixelFormat.Format32bppArgb);

            // ここの処理は、windowsのロック画面になった場合等に、キャプチャする領域が失効して例外発生する。
            using (Graphics g = Graphics.FromImage(myBmp))
            {
                try
                {
                    g.CopyFromScreen(captScreenRect.X, captScreenRect.Y, 0, 0,
                    captScreenRect.Size, CopyPixelOperation.SourceCopy);
                }
                catch (Exception ex)
                {
                    //クライアント領域の描画内容取得に失敗しました。
                    Singleton<Log.LogManager>.Instance.WriteCommonLog(Log.LogKind.DebugLog, String.Format("failed to get the rendered content of the client area:{0}", ex.StackTrace));
                }
            }

            m_pctLayer.CleanResource(); // もし画像を保持していれば消す
            m_pctLayer.MaximumSize = m_pctLayer.MinimumSize = captScreenRect.Size;
            m_pctLayer.Size = captScreenRect.Size;
            //m_pctLayer.StartPosition = FormStartPosition.Manual;
            m_pctLayer.ShowImage = myBmp;
            m_pctLayer.Opacity = 1d;
            //m_pctLayer.Opacity = 1.0;
            //m_pctLayer.Visible = true;

            m_pctLayer.TopMost = true;
            m_pctLayer.Activate();


            m_blnUsePictLayer = true;
            //m_pctLayer.Show();
            m_pctLayer.Location = captScreenRect.Location;

#if false
            for ( Int32 i = 0; i < 30; i++ )
            {
                Application.DoEvents();
            }
#endif
            //System.Threading.Thread.Sleep( 30 ); // 少し待つ

            //m_evClose.Reset();
            //this.Opacity = 1.0;
            //if ( m_blnInitialized )
            //{
            //    this.Visible = true;
            //    m_ShowKind = ShowKind.Show;
            //    this.m_ShowTimerPict.Interval = 10;
            //    this.m_ShowTimerPict.Enabled = true;
            //}
            //else
            //{
            // base.Show();
            //this.drawStop( this.Handle );
            //this.drawStop( this.m_pctLayer.Handle );
            this.StartPosition = FormStartPosition.Manual;
            this.Show();
            this.Left += this.OutsideX;
            this.Opacity = 1d;
#if false
            for ( Int32 i = 0; i < 30; i++ )
            {
                Application.DoEvents();
            }
#endif
            this.Left -= this.OutsideX;

            //this.drawStart( this.m_pctLayer.Handle );

            if (m_blnInitialized)
            {
                //this.Visible = true;
                m_ShowKind = ShowKind.Show;
                this.m_ShowTimerPict.Interval = 10;
                this.m_ShowTimerPict.Enabled = true;
            }
            //}

            //m_pctLayer.BringToFront();
            //m_pctLayer.Activate();
            // 画面のSS取ってそれを全体表示し、SSの透明度を上げていく方法(未実装)

            //using ( Bitmap bmp = new Bitmap( Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height ) )
            //{
            //    using ( Graphics g = Graphics.FromImage( bmp ) )
            //    {
            //        g.CopyFromScreen( new Point( 0, 0 ), new Point( 0, 0 ), bmp.Size );
            //    }
            //    bmp.Save( @"c:\prt.bmp" );
            //}			

#if false // 旧手法		
			m_evClose.Reset();
			this.Opacity = 1.0;
			if ( m_blnInitialized )
			{
				this.Visible = true;
				m_ShowKind = ShowKind.Show;
				this.m_ShowTimer.Interval = 10;
				this.m_ShowTimer.Enabled = true;
			}
			else
			{
				base.Show();
			}
#endif

        }

        /// <summary>
        /// フェードイン表示
        /// </summary>
        /// <remarks>
        /// フォーム自身の透明度設定を利用したフェード表示を行います。
        /// </remarks>
        public new virtual void Show()
        {

            //m_evClose.Reset();
            //			this.Opacity = 1.0;
            //this.Opacity = 0;
            if (this.m_blnInitialized)
            {
                this.Visible = true;
                this.m_ShowKind = ShowKind.Show;
                if (!this.m_blnUsePictLayer)
                {
                    this.m_ShowTimer.Interval = 10;
                    this.m_ShowTimer.Enabled = true;
                }
            }
            else
            {
                base.Show();
            }
        }

        /// <summary>
        /// 通常表示
        /// </summary>
        /// <remarks>
        /// 通常表示を行います。
        /// </remarks>
        public virtual void ShowNormal()
        {
            //this.Opacity = 1;
            //m_evClose.Reset();
            if (this.m_blnInitialized)
            {
                this.Visible = true;
            }
            else
            {
                base.Show();
            }
        }

        /// <summary>
        /// モーダル表示
        /// </summary>
        /// <remarks>
        /// モーダル表示を行います。
        /// </remarks>
        /// <returns></returns>
        public new virtual DialogResult ShowDialog()
        {
            //m_evClose.Reset();

            this.Opacity = 0;

            if (m_blnInitialized)
            {
                this.Visible = true;
                m_ShowKind = ShowKind.Show;
                this.m_ShowTimer.Interval = 10;
                this.m_ShowTimer.Enabled = true;
            }

            return base.ShowDialog();

        }

        /// <summary>
        /// フェードアウト効果付ウィンドウクローズ
        /// </summary>
        /// <remarks>
        /// フェードアウト効果付ウィンドウクローズを行います。
        /// </remarks>
        public new virtual void Close()
        {
            foreach (Form form in this.OwnedForms)
            {
                if (form is FormTransitionBase)
                {
                    form.Close();
                }
            }

            m_ShowKind = ShowKind.Close;
            this.m_ShowTimer.Interval = 10;
            this.m_ShowTimer.Enabled = true;

            // クローズ完了するまでこの関数をブロック
            // @@ 未実装
            // @@ Formのタイマはこの画面スレッドを止めると動けない。
            // @@ 別の実装にする必要あり、若しくはSystem.Windows.Forms.Application.DoEvents()を使う
            //while ( !m_evClose.WaitOne( 100 ) )
            //{
            //    System.Threading.Thread.Yield();
            //}



            //this.FormClose();
        }

        /// <summary>
        /// 画面外へ移動
        /// </summary>
        /// <remarks>
        /// 自フォームを画面外の座標へ移動します。
        /// </remarks>
        public virtual void MoveOutSide()
        {
            this.Location = new Point(this.Location.X + this.OutsideX, this.Location.Y);
        }

        /// <summary>
        /// フェードアウト効果付ウィンドウ非表示処理
        /// </summary>
        /// <remarks>
        /// フェードアウト効果付ウィンドウ非表示処理を行います。
        /// </remarks>
        public new virtual void Hide()
        {
            if (this.Opacity != 0)
            {
                m_ShowKind = ShowKind.Hide;
                this.m_ShowTimer.Interval = 10;
                this.m_ShowTimer.Enabled = true;
            }
        }

        /// <summary>
        /// フェードアウト効果付ウィンドウ非表示処理
        /// </summary>
        /// <remarks>
        /// フェードアウト効果付ウィンドウ非表示処理を行います。
        /// </remarks>
        /// <param name="captScreenRect">対象領域</param>
        public virtual void Hide(Rectangle captScreenRect)
        {
            this.m_ShowTimer.Enabled = false;
            this.BringToFront();
            if (m_pctLayer == null)
            {
                m_pctLayer = new FormPictLayer();
                //m_pctLayer.Visible = false;
                m_pctLayer.MaximumSize = m_pctLayer.MinimumSize = captScreenRect.Size;
                m_pctLayer.Size = captScreenRect.Size;
                m_pctLayer.StartPosition = FormStartPosition.Manual;
                m_pctLayer.Left = this.OutsideX;
                m_pctLayer.Show();
                m_pctLayer.Opacity = 0.01d;
            }

            // Bitmapオブジェクトにスクリーン・キャプチャ
            Bitmap myBmp = new Bitmap(captScreenRect.Width,
                      captScreenRect.Height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(myBmp))
            {
                g.CopyFromScreen(captScreenRect.X, captScreenRect.Y, 0, 0,
                captScreenRect.Size, CopyPixelOperation.SourceCopy);
            }

            m_pctLayer.CleanResource(); // もし画像を保持していれば消す
            m_pctLayer.Size = this.Size;
            //m_pctLayer.StartPosition = FormStartPosition.Manual;
            m_pctLayer.ShowImage = myBmp;
            m_pctLayer.Visible = true;

            m_pctLayer.TopMost = true;

            m_blnUsePictLayer = true;
            //this.Opacity = 0.0;


            m_ShowKind = ShowKind.Hide;
            this.m_ShowTimerPict.Interval = 10;
            this.m_ShowTimerPict.Enabled = true;

            this.Visible = false;
            m_pctLayer.Opacity = 1.0;
            m_pctLayer.Activate();
            m_pctLayer.Location = captScreenRect.Location;
            m_pctLayer.Refresh();
            //for ( Int32 i = 0; i < 30; i++ )
            //{
            //    Application.DoEvents();
            //}
            //m_pctLayer.Show();
        }

        /// <summary>
        /// フォームクローズ
        /// </summary>
        /// <remarks>
        /// フォームクローズ処理を行います。
        /// </remarks>
        public virtual void FormClose()
        {
            base.Close();
        }

        #endregion

        #region [protectedメソッド]

        ///// <summary>
        ///// 描画停止
        ///// </summary>
        ///// <remarks>
        ///// 対象ハンドルにメッセージを送り、描画の更新を停止します。
        ///// </remarks>
        ///// <param name="tgtHandle">対象フォーム</param>
        //protected void drawStop( IntPtr tgtHandle )
        //{
        //    Win32API.SendMessage( tgtHandle, Win32API.WM_SETREDRAW, (IntPtr)0, (IntPtr)0 );
        //}

        ///// <summary>
        ///// 描画開始
        ///// </summary>
        ///// <remarks>
        ///// 対象ハンドルにメッセージを送り、描画の更新を開始します。
        ///// </remarks>
        ///// <param name="tgtHandle">対象フォーム</param>
        //protected void drawStart( IntPtr tgtHandle )
        //{
        //    Win32API.SendMessage( tgtHandle, Win32API.WM_SETREDRAW, (IntPtr)1, (IntPtr)0 );
        //}

        /// <summary>
        /// Shownイベント処理
        /// </summary>
        /// <remarks>
        /// Shownイベント発生時の処理を行います。
        /// </remarks>
        /// <param name="e">イベント情報(未使用)</param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            //this.drawStart( this );

            m_ShowKind = ShowKind.Show;

            if (m_blnUsePictLayer)
            {
                this.m_ShowTimerPict.Interval = 10;
                this.m_ShowTimerPict.Enabled = true;
            }
            else
            {
                this.m_ShowTimer.Interval = 10;
                this.m_ShowTimer.Enabled = true;
            }
            m_blnInitialized = true;


            //// 非常に遅い
            //for ( Int32 i = 0; i <= 100; i++ )
            //{
            //    this.Opacity = i/100.0;
            //    this.Refresh();
            //    //System.Threading.Thread.Sleep( 10 );
            //}
        }

        /// <summary>
        /// 画像レイヤ表示タイマ
        /// </summary>
        /// <remarks>
        /// 画像レイヤ表示タイマを起動します。
        /// </remarks>
        /// <param name="sender">イベント発生オブジェクト（不使用）</param>
        /// <param name="e">イベント情報（不使用）</param>
        protected virtual void ShowTimerPictTick(Object sender, EventArgs e)
        {
            //			Double dblNextOpacity = 0.99;
            Double dblNextOpacity = 1.0;

            switch (m_ShowKind)
            {
                case ShowKind.Show:
                    //dblNextOpacity = ( m_pctLayer.Opacity + 0.01 ) * 1.2;
                    dblNextOpacity -= (1.0 - (this.m_pctLayer.Opacity - 0.01)) * 1.35;
                    if (dblNextOpacity <= 0.0)
                    {
                        //dblNextOpacity = 0.99f;
                        dblNextOpacity = 0.0f;
                        this.m_ShowTimerPict.Enabled = false;
                        this.m_blnUsePictLayer = false;
                        this.m_pctLayer.CleanResource(); // もし画像を保持していれば消す
                        this.m_pctLayer.Left = this.OutsideX;

                        // フェード表示完了イベント発生
                        if (this.OnFadeShown != null)
                        {
                            this.Refresh();
                            this.OnFadeShown(this, new EventArgs());
                        }
                    }
                    break;
                case ShowKind.Close:
                    dblNextOpacity -= (1.0 - (this.m_pctLayer.Opacity - 0.01)) * 1.35;
                    if (dblNextOpacity <= 0.0)
                    {
                        dblNextOpacity = 0.0;
                        this.m_ShowTimerPict.Enabled = false;
                        this.m_blnUsePictLayer = false;
                        //m_evClose.Set(); // @@ 未実装
                        this.m_pctLayer.Close();
                        this.FormClose();
                        this.m_pctLayer.CleanResource(); // もし画像を保持していれば消す
                    }
                    break;
                case ShowKind.Hide:
                    dblNextOpacity -= (1.0 - (this.m_pctLayer.Opacity - 0.01)) * 1.35;
                    if (dblNextOpacity <= 0.0)
                    {
                        dblNextOpacity = 0.0;
                        this.m_ShowTimerPict.Enabled = false;
                        this.m_blnUsePictLayer = false;
                        //m_pctLayer.Visible = false;
                        this.m_pctLayer.CleanResource(); // もし画像を保持していれば消す
                        this.m_pctLayer.Left = this.OutsideX;
                    }
                    break;
                default:
                    break;
            }

            this.m_pctLayer.Opacity = dblNextOpacity;
        }

        /// <summary>
        /// 画面表示タイマ
        /// </summary>
        /// <remarks>
        /// 画面表示タイマを起動します。
        /// </remarks>
        /// <param name="sender">イベント発生オブジェクト（不使用）</param>
        /// <param name="e">イベント情報（不使用）</param>
        protected virtual void ShowTimerTick(Object sender, EventArgs e)
        {
            Double dblNextOpacity = 1.0f;

            switch (m_ShowKind)
            {
                case ShowKind.Show:
                    dblNextOpacity = (this.Opacity + 0.01) * 1.2;
                    if (dblNextOpacity >= 1.0)
                    {
                        //dblNextOpacity = 0.99f;
                        dblNextOpacity = 1.0f;
                        this.m_ShowTimer.Enabled = false;

                        if (this.OnFadeShown != null)
                        {
                            for (Int32 i = 0; i < 1000; i++)
                            {
                                System.Windows.Forms.Application.DoEvents();
                            }

                            this.OnFadeShown(this, new EventArgs());
                        }
                    }
                    break;
                case ShowKind.Close:
                    dblNextOpacity = dblNextOpacity - (1.0 - (this.Opacity - 0.01)) * 1.2;
                    if (dblNextOpacity <= 0.0)
                    {
                        dblNextOpacity = 0.0;
                        this.m_ShowTimer.Enabled = false;
                        //m_evClose.Set(); // @@ 未実装
                        base.DialogResult = this.DialogResult;
                        this.FormClose();
                        //this.FormClose();
                        if (m_pctLayer != null)
                        {
                            m_pctLayer.Dispose();
                        }
                    }
                    break;
                case ShowKind.Hide:

                    dblNextOpacity = dblNextOpacity - (1.0 - (this.Opacity - 0.01)) * 1.2;
                    if (dblNextOpacity <= 0.0)
                    {
                        dblNextOpacity = 0.0;
                        this.m_ShowTimer.Enabled = false;
                        //m_pctLayer.Visible = false;
                    }
                    break;
                default:
                    break;
            }

            this.Opacity = dblNextOpacity;
        }

        //protected virtual void ShowTimerTick( Object myObject, EventArgs myEventArgs )
        //{
        //    Double dblNextOpacity = 0.99; 

        //    switch ( m_ShowKind )
        //    {
        //    case ShowKind.Show:
        //        dblNextOpacity = ( this.Opacity + 0.01 ) * 1.2;
        //        if ( dblNextOpacity >= 1.0 )
        //        {
        //            //dblNextOpacity = 0.99f;
        //            dblNextOpacity = 1.0f;
        //            this.m_ShowTimer.Enabled = false;
        //        }
        //        break;
        //    case ShowKind.Close:
        //        dblNextOpacity = dblNextOpacity - ( 1.0 - ( this.Opacity - 0.01 ) ) * 1.2;
        //        if ( dblNextOpacity <= 0.0 )
        //        {
        //            dblNextOpacity = 0.0;
        //            this.m_ShowTimer.Enabled = false;
        //            m_evClose.Set(); // @@ 未実装
        //            this.FormClose();
        //            //this.FormClose();
        //        }
        //        break;
        //    case ShowKind.Hide:

        //        dblNextOpacity = dblNextOpacity - ( 1.0 - ( this.Opacity - 0.01 ) ) * 1.2;
        //        if ( dblNextOpacity <= 0.0 )
        //        {
        //            dblNextOpacity = 0.0;
        //            this.m_ShowTimer.Enabled = false;
        //            this.Visible = false;
        //        }
        //        break;
        //    default:
        //        break;
        //    }

        //    this.Opacity = dblNextOpacity;
        //}

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// コンポーネント初期化
        /// </summary>
        /// <remarks>
        /// コンポーネントの初期化を行います。
        /// </remarks>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormTransitionBase
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "FormTransitionBase";
            this.ResumeLayout(false);
        }

        #endregion

    }
}
