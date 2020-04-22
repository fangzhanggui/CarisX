using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using Oelco.Common.Utility;

namespace Oelco.Common.GUI
{
    /// <summary>
    /// 透過フォーム
    /// </summary>
    /// <remarks>
    /// 指定パネルに表示する内容を、画像によるα値制御を行いウィンドウとして表示します。
    /// </remarks>
    public partial class FormTransparentLayer : FormTransitionBase
    {
        #region [クラス変数定義]

        /// <summary>
        /// クライアントパネルの保持する画像リソース
        /// </summary>
        private List<Tuple<Image, Point, Control>> clientPnlButtonImages;

        /// <summary>
        /// クライアントフォーム
        /// </summary>
        private FormTransparentSub m_formClientArea = null;

        /// <summary>
        /// 表示イメージ
        /// </summary>
        private Bitmap m_backBmp;

        /// <summary>
        /// 作業用 全面透過イメージ
        /// </summary>
        private Bitmap m_nullImage;

        /// <summary>
        /// 表示イメージ
        /// </summary>
        private Bitmap m_backBmpOriginal;

        /// <summary>
        /// Deactivate時Hideフラグ
        /// </summary>
        private Boolean deactivateHide = true;


        private Bitmap layer2 = null;
        private Bitmap layer3 = null;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormTransparentLayer()
        {
            InitializeComponent();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 背景画像 設定/取得
        /// </summary>
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;

                // 背景画像のストレッチは現在対応の必要無し。
                //if ( this.BackgroundImageLayout == ImageLayout.Stretch )
                //{
                //    m_backBmp = new Bitmap( this.Width, this.Height, PixelFormat.Format32bppArgb );
                //    using ( Graphics g = Graphics.FromImage( m_backBmp ) )
                //    {
                //        float widthScale = (float)this.Width / (float)value.Width;
                //        float heightScale = (float)this.Height / (float)value.Height;
                //        g.ResetTransform();
                //        g.ScaleTransform( widthScale, heightScale );
                //        g.DrawImage( value, 0, 0 );
                //    }
                //}
                //else
                //{
                m_backBmp = (Bitmap)value;
                //}
                if ( m_nullImage != null )
                {
                    m_nullImage.Dispose();
                }

                // 使用画像と同サイズの透過画像作成
                m_nullImage = new Bitmap( m_backBmp.Width, m_backBmp.Height, PixelFormat.Format32bppArgb );

                // 領域情報設定
                m_backBmpOriginal = m_backBmp.Clone() as Bitmap;

                // 表示内容更新
                selectBitmap( m_backBmp );
            }
        }

        /// <summary>
        /// ウィンドウパラメータ作成
        /// </summary>
        /// <remarks>
        /// ウィンドウの表示パラメータを作成します。
        /// この関数は.NETフレームワークから呼び出されるため、
        /// このプログラム内からの呼び出しはありませんが必要となります。
        /// </remarks>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= Win32API.WS_EX_LAYERED;
                return createParams;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// フェードイン表示
        /// </summary>
        /// <remarks>
        /// このフォームのフェードイン表示を行います。
        /// </remarks>
        /// <param name="captScreenRect">表示エリア</param>
        public override void Show( Rectangle captScreenRect )
        {
            this.Visible = false;
            //this.drawStop( this.Handle ); // 移動による描画を行わない(表示不可軽減、チラつき抑制)
            this.Location = captScreenRect.Location;
            this.m_formClientArea.Top = captScreenRect.Top;
            this.setTransparency();
            base.Show( captScreenRect );
            //this.drawStart( this.Handle );

            this.Activate();
        }

        /// <summary>
        /// クライアントパネル設定
        /// </summary>
        /// <remarks>
        /// 表示を行うパネルを設定します。
        /// ここで設定されたパネルは、本来の親コントロールから関連付けを変更され、
        /// FormTransparentSubにより管理されます。
        /// </remarks>
        /// <param name="clientPanel">クライアントパネル</param>
        public void SetClientPanel( Infragistics.Win.Misc.UltraPanel clientPanel )
        {
            if ( m_formClientArea != null )
            {
                m_formClientArea.Dispose();
            }

            // フォーム設定
            // このフォームと、クライアントフォームをクライアントパネルの情報を元に設定します。
            m_formClientArea = new FormTransparentSub();
            m_formClientArea.ShowInTaskbar = false;                     // タスクバーに表示させない
            m_formClientArea.ControlBox = false;                        // コントロールボックスを表示しない
            m_formClientArea.FormBorderStyle = FormBorderStyle.None;    // 枠線スタイル＝枠線なし
            this.Size = clientPanel.Size;                               // サイズの設定
            m_formClientArea.Size = this.ClientSize;                    // Form2のサイズはForm1のクライアント領域のサイズ
            m_formClientArea.StartPosition = FormStartPosition.Manual;  // Form2の初期位置はLocationで指定
            this.StartPosition = FormStartPosition.Manual;
            this.MaximumSize = this.MinimumSize = this.ClientSize;
            this.Location = new Point( 0, 0 );
            this.Left += this.OutsideX;
            //Screen.PrimaryScreen.Bounds.Width;
            m_formClientArea.Location = this.Location;                  // Form2の位置をForm1のクライアント領域にセット

            // 透過色設定
            // このフォーム上で透過扱いとなる色を設定します。
            //Color excludeColor = Color.FromArgb(253, 253, 253);
            Color excludeColor = Color.FromArgb(173, 173, 173);
            m_formClientArea.BackColor = excludeColor;

            // 背景イメージ生成
            // クライアントパネルの背景をこのフォームに設定します。
            // クライアントパネルの背景は削除され、透明色が設定されます。
            Image imgResized = new Bitmap( clientPanel.Appearance.ImageBackground, clientPanel.Size );
            this.BackgroundImage = imgResized;
            clientPanel.Appearance.ImageBackground = null;
            clientPanel.BackColor = Color.Transparent;
            //clientPanel.Visible = true;


            // 透過色の設定
            m_formClientArea.TransparencyKey = excludeColor;

            // クライアントパネルのParent設定
            clientPanel.Parent = m_formClientArea;
            clientPanel.Dock = DockStyle.Fill;

            // クライアントフォームのオーナー設定
            this.AddOwnedForm( m_formClientArea );

            // 画像作成
            // クライアントパネルの保持するボタン画像を全て抜き出し、
            // 保持します。
            // 透過イメージ表示の際に使用されます。
            this.clientPnlButtonImages = this.spoilAllUltraButtonImage( this.m_formClientArea );

            // 初回表示を画面領域外で行います。
            // この表示を行っておく事により、初回表示時のチラつきを抑制する。
            Rectangle preView = clientPanel.DisplayRectangle;
            preView.X += this.OutsideX;
            this.preShow( preView );
        }

        /// <summary>
        /// Deactivate時Hide動作設定
        /// </summary>
        /// <remarks>
        /// Deactivate時Hide動作設定を行います。
        /// </remarks>
        /// <param name="hideWhenDeactivate">Hideフラグ</param>
        public void SetDeactivateHide( Boolean hideWhenDeactivate )
        {
            this.deactivateHide = hideWhenDeactivate;
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// メッセージプロシージャ
        /// </summary>
        /// <remarks>
        /// メッセージの内、マウスに関連するメッセージのみ通知を行います。
        /// </remarks>
        /// <param name="m">メッセージ内容</param>
        protected override void WndProc( ref Message m )
        {
            // マウス関連のメッセージを通知する。
            this.m_formClientArea.ReflectMouseMessage( ref m );
            base.WndProc( ref m );
        }

        /// <summary>
        /// Closingイベントオーバーライド
        /// </summary>
        /// <remarks>
        /// このフォームのクローズイベントに対して、
        /// クライアントフォームを連動させます。
        /// </remarks>
        /// <param name="e">イベント情報</param>
        protected override void OnClosing( CancelEventArgs e )
        {
            if ( this.m_formClientArea != null )
            {
                this.m_formClientArea.Close();
            }
            base.OnClosing( e );
        }

        /// <summary>
        /// Shownイベントオーバーライド
        /// </summary>
        /// <remarks>
        /// 表示完了時、クライアントフォームの表示を行います。
        /// </remarks>
        /// <param name="e">イベント情報</param>
        protected override void OnShown( EventArgs e )
        {
            base.OnShown( e );
            this.m_formClientArea.Show();
        }

        /// <summary>
        /// 画像設定
        /// </summary>
        /// <remarks>
        /// このフォームへの表示内容設定を行います。
        /// フォームの透過率はピクセル単位で全て表示画像のα値依存となります。
        /// この画像設定が行われたフォームに対して、Opacityによる透過率の制御は出来ません。
        /// </remarks>
        /// <param name="bitmap">表示内容</param>
        protected void selectBitmap( Bitmap bitmap )
        {
            // デバイスコンテキスト取得
            IntPtr screenDc = Win32API.GetDC( IntPtr.Zero );
            IntPtr memDc = Win32API.CreateCompatibleDC( screenDc );
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr hOldBitmap = IntPtr.Zero;

            try
            {
                // ビットマップハンドル取得
                hBitmap = bitmap.GetHbitmap( Color.FromArgb( 0 ) );
                hOldBitmap = Win32API.SelectObject( memDc, hBitmap );

                // レイヤードウィンドウ設定
                Win32API.Size newSize = new Win32API.Size( bitmap.Width, bitmap.Height );		// 画像にウィンドウサイズを合わせる
                Win32API.Point sourceLocation = new Win32API.Point( 0, 0 );
                Win32API.Point newLocation = new Win32API.Point( this.Left, this.Top );		// ウィンドウ位置設定
                Win32API.BLENDFUNCTION blend = new Win32API.BLENDFUNCTION();
                blend.BlendOp = Win32API.AC_SRC_OVER;							// 固定　AC_SRC_OVER
                blend.BlendFlags = 0;											// 固定　0
                blend.SourceConstantAlpha = 230;								// 255でピクセル毎にアルファ値変動
                blend.AlphaFormat = Win32API.AC_SRC_ALPHA;						// 固定　AC_SRC_ALPHA

                // 設定
                Win32API.UpdateLayeredWindow( Handle, screenDc, ref newLocation, ref newSize,
                    memDc, ref sourceLocation, 0, ref blend, Win32API.ULW_ALPHA );
            }
            catch ( Exception )
            {
            }
            finally
            {
                // デバイスコンテキスト開放
                Win32API.ReleaseDC( IntPtr.Zero, screenDc );
                // ハンドル開放
                if ( hBitmap != IntPtr.Zero )
                {
                    Win32API.SelectObject( memDc, hOldBitmap );
                    Win32API.DeleteObject( hBitmap );
                }
                Win32API.DeleteDC( memDc );
            }
        }



        /// <summary>
        /// Deactivateイベントオーバーライド
        /// </summary>
        /// <remarks>
        /// Deactivate時、このフォームを表示領域外へ移動します。
        /// </remarks>
        /// <param name="e">イベント情報</param>
        protected override void OnDeactivate( EventArgs e )
        {
            if ( this.deactivateHide )
            {
                this.MoveOutSide();
            }

            base.OnDeactivate( e );
        }

        /// <summary>
        /// ボタン画像抽出
        /// </summary>
        /// <remarks>
        /// 対象フォームに存在するUltraButtonから、表示画像と座標を抽出します。
        /// </remarks>
        /// <param name="target">対象フォーム</param>
        /// <returns>表示画像・座標</returns>
        protected List<Tuple<Image, Point, Control>> spoilAllUltraButtonImage( Form target )
        {
            // ここでの処理はtargetに対して直接ボタンコントロールは存在しない前提とする。
            List<Tuple<Image, Point, Control>> result = new List<Tuple<Image, Point, Control>>();
            foreach ( Control ctl in target.Controls )
            {
                result.AddRange( spoilAllUltraButtonImage( ctl ) );
            }

            // 結果の座標は全てスクリーン座標になっているので、Formのクライアント座標にしておく。
            // ※これを利用して描画する画像はフォームのサイズと必ず同一の為。

            //for ( Int32 iLoop = 0; iLoop < result.Count; iLoop++ )
            //{
            //    result[iLoop] = new Tuple<Image, Point>( result[iLoop].Item1, target.PointToClient( result[iLoop].Item2 ) ); 
            //}

            //result.ForEach( (Action<Tuple<Image, Point>>)( ( Tuple<Image, Point> t ) => t = new Tuple<Image,Point>(t.Item1, target.PointToClient( t.Item2 ) ) ) );

            return result;
        }

        /// <summary>
        /// ボタン画像抽出
        /// </summary>
        /// <remarks>
        /// 対象コントロールに存在するUltraButtonから、ボタン画像と座標を抽出します。
        /// コントロールの走査は再帰的に行われます。
        /// </remarks>
        /// <param name="target">対象コントロール</param>
        /// <returns>表示画像・座標</returns>
        protected List<Tuple<Image, Point, Control>> spoilAllUltraButtonImage( Control target )
        {
            List<Tuple<Image, Point, Control>> result = new List<Tuple<Image, Point, Control>>();
            foreach ( Control ctl in target.Controls )
            {
                result.AddRange( spoilAllUltraButtonImage( ctl ) );
                if ( ctl is Infragistics.Win.Misc.UltraButton )
                {
                    Infragistics.Win.Misc.UltraButton ultraButton = ctl as Infragistics.Win.Misc.UltraButton;
                    result.Add( new Tuple<Image, Point, Control>( ultraButton.Appearance.ImageBackground, ctl.Location, ultraButton ) );
                    ultraButton.Appearance.ImageBackground = null; // 本来保持しているものは削除する。
                    //ultraButton.Appearance.ImageBackground = Oelco.Common.Properties.Resources.Image_Transparent;
                }

            }
            return result;
        }

        /// <summary>
        /// UltraButtonコントロールの表示状態保存
        /// </summary>
        /// <remarks>
        /// 全てのUltraButtonコントロールの表示状態を記憶します。
        /// </remarks>
        /// <param name="target"></param>
        /// <returns></returns>
        protected Dictionary<Control, Boolean> saveAllUltraButtonVisible( Form target )
        {
            // 全てのUltraButtonコントロールの表示状態を記憶する
            Dictionary<Control, Boolean> dataDic = new Dictionary<Control, Boolean>();
            Action<Control, Dictionary<Control, Boolean>> getVisible = null;
            getVisible = ( Control tgt, Dictionary<Control, Boolean> data ) =>
            {
                foreach ( Control ctl in tgt.Controls )
                {
                    getVisible( ctl, data );
                    if ( ctl is Infragistics.Win.Misc.UltraButton )
                    {
                        dataDic[ctl] = ctl.Visible;
                    }
                }
            };
            foreach ( Control ctl in target.Controls )
            {
                getVisible( ctl, dataDic );
            }

            return dataDic;
        }

        /// <summary>
        /// 全UltraButtonコントロールの表示状態設定
        /// </summary>
        /// <remarks>
        /// 全てのUltraButtonコントロールの表示状態を設定する
        /// </remarks>
        /// <param name="target">対象フォーム</param>
        /// <param name="dataDic">コントロールと表示状態対応リスト</param>
        protected void restoreAllUltrButtonVisible( Form target, Dictionary<Control, Boolean> dataDic )
        {
            // 全てのUltraButtonコントロールの表示状態を設定する
            Action<Control, Dictionary<Control, Boolean>> setVisible = null;
            setVisible = ( Control tgt, Dictionary<Control, Boolean> data ) =>
            {
                foreach ( Control ctl in tgt.Controls )
                {
                    setVisible( ctl, data );
                    if ( ctl is Infragistics.Win.Misc.UltraButton )
                    {
                        ctl.Visible = dataDic.ContainsKey( ctl ) ? dataDic[ctl] : false;
                    }
                }
            };
            foreach ( Control ctl in target.Controls )
            {
                setVisible( ctl, dataDic );
            }
        }

        /// <summary>
        /// 全UltraButtonコントロールの表示状態設定
        /// </summary>
        /// <remarks>
        /// 全てのUltraButtonコントロールの表示状態を設定する
        /// </remarks>
        /// <param name="target"></param>
        /// <param name="visible"></param>
        protected void allUltraButtonVisible( Form target, Boolean visible )
        {
            // ここでの処理はtargetに対して直接ボタンコントロールは存在しない前提とする。
            //List<Tuple<Image, Point>> result = new List<Tuple<Image, Point>>();
            foreach ( Control ctl in target.Controls )
            {
                this.allUltraButtonVisible( ctl, visible );
            }
        }

        /// <summary>
        /// UltraButtonコントロールの表示状態設定
        /// </summary>
        /// <remarks>
        /// UltraButtonコントロールの表示状態を設定します。
        /// </remarks>
        /// <param name="target"></param>
        /// <param name="visible"></param>
        protected void allUltraButtonVisible( Control target, Boolean visible )
        {
            foreach ( Control ctl in target.Controls )
            {
                this.allUltraButtonVisible( ctl, visible );
                if ( ctl is Infragistics.Win.Misc.UltraButton )
                {
                    ctl.Visible = visible;
                }
            }
        }

        /// <summary>
        /// 透過情報設定
        /// </summary>
        /// <remarks>
        /// 透過フォームの表示に必要な情報を生成・設定します。
        /// </remarks>
        protected void setTransparency()
        {
            //m_realPos = this.Location;

            // デスクトップ外に表示を行う
            m_formClientArea.Left = this.Location.X + this.OutsideX;

            // 透過表示を使用する
            this.selectBitmap( this.m_nullImage );
            //this.ShowNormal();

            // 結合bmpを作成
            // 透過フォームは三層の画像を合成し表示する。
            // 1層:クライアントパネルの背景画像
            // 2層:クライアントパネルのボタン画像
            // 3層:クライアントパネルのボタン画像以外（ボタンの枠等、スタイルによる影響を受ける）
            // 1-2層に対しては画像のα値が適用され、透過される。
            // 3層はα値が適用されず、本来表示する内容と同様の表示を行う。
            if ((layer2 == null) || (layer3 == null))
            {
                layer2 = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                layer3 = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                // 表示することで画面の表示内容を取得可能となる。
                m_formClientArea.Show();

                // 画像レイヤ作成
                Dictionary<Control, Boolean> visibleInfo = this.saveAllUltraButtonVisible(this.m_formClientArea);
                layer2 = Utility.Transparency.GetAlphaImage(this.m_formClientArea.Size, this.clientPnlButtonImages, visibleInfo);

                // 画像以外のレイヤ作成
                // ** ボタンの表示領域がラベルに掛かっている場合、ボタンの表示内容が全て透過となっている場合でも、
                //    ラベルの上に表示領域がかかり、テキストが切れてしまう為、一時的にボタンを非表示とする。
                //    非表示のままにすると、クリックが利かなくなる為即表示する。
                this.allUltraButtonVisible(this.m_formClientArea, false);
                layer3 = Utility.Transparency.GetAlphaImage(this.m_formClientArea);
                //this.allUltraButtonVisible( this.m_formClientArea, true );
                this.restoreAllUltrButtonVisible(this.m_formClientArea, visibleInfo);
            }

            // 画像レイヤ
            {
                // 画像合成
                this.m_backBmp = (Bitmap)m_backBmpOriginal.Clone(); // 再表示の際に、合成後画像に対して再合成が行われるのを抑止
                using ( Graphics g = Graphics.FromImage( this.m_backBmp ) )
                {
                    g.DrawImage( layer2, 0, 0 );
                    g.DrawImage( layer3, 0, 0 );
                }

                // 表示タイマ設定
                m_ShowKind = ShowKind.Show;
                this.m_ShowTimerPict.Interval = 10;
                this.m_ShowTimerPict.Enabled = true;

                // 表示内容設定
                this.selectBitmap( this.m_backBmp );
                m_ShowTimer.Enabled = false;
            }
        }

        /// <summary>
        /// 事前表示
        /// </summary>
        /// <remarks>
        /// 利用者による表示が行われる前に呼び出されます。
        /// 表示内容を生成しておき、実表示の際に負荷を軽減します。
        /// </remarks>
        /// <param name="captScreenRect">表示領域</param>
        protected void preShow( Rectangle captScreenRect )
        {
            this.Visible = false;
            this.Location = captScreenRect.Location;
            this.m_formClientArea.Top = captScreenRect.Top;
            this.setTransparency();

            base.Show( captScreenRect );
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
        }

        #endregion

        #region [内部クラス]

        /// <summary>
        /// クライアントフォーム
        /// </summary>
        /// <remarks>
        /// FormTransparentLayerに利用される内部クラスです。
        /// </remarks>
        protected partial class FormTransparentSub : Form
        {
            #region [publicメソッド]

            /// <summary>
            /// マウスメッセージ通知
            /// </summary>
            /// <remarks>
            /// 透過フォームに対するマウス操作を、自身に適用します。
            /// 表示座標の差分に対して調整を行います。
            /// </remarks>
            /// <param name="m">メッセージ内容</param>
            public void ReflectMouseMessage( ref Message m )
            {
                const Int32 WM_LBUTTONUP = 0x0202;

                switch ( m.Msg )
                {
                //case WM_LBUTTONDOWN:
                case WM_LBUTTONUP:

                    Int32 x = (Int32)m.LParam & 0xFFFF;
                    Int32 y = ( (Int32)m.LParam >> 16 ) & 0xFFFF;

                    // 親フォームの座標が送られてくるので変換する
                    Point pOwner = this.Owner.PointToScreen( new Point( x, y ) );
                    Point pAdjust = new Point();
                    pAdjust.X = this.Location.X - this.Owner.Location.X;
                    pAdjust.Y = 0; // Y座標は補正しない

                    // 保持するコントロール全て確認
                    foreach ( Control ctl in this.Controls )
                    {
                        if ( ctl.Controls.Count != 0 )
                        {
                            this.reflectMouseMsg( ctl.Controls, pOwner, pAdjust, ref m );
                        }
                    }
                    break;
                }
            }

            #endregion

            #region [protectedメソッド]

            /// <summary>
            /// マウスメッセージ通知
            /// </summary>
            /// <remarks>
            /// 透過フォームに対するマウス操作を、自身に適用します。
            /// 表示座標の差分に対して調整を行います。
            /// </remarks>
            /// <param name="ctlCol">コントロールコレクション</param>
            /// <param name="pPoint">通知座標</param>
            /// <param name="pAdjust">通知座標補正値</param>
            /// <param name="m">メッセージ内容</param>
            protected void reflectMouseMsg( Control.ControlCollection ctlCol, Point pPoint, Point pAdjust, ref Message m )
            {
                foreach ( Control ctl in ctlCol )
                {
                    if ( ctl.Controls.Count != 0 )
                    {
                        this.reflectMouseMsg( ctl.Controls, pPoint, pAdjust, ref m );
                    }

                    Rectangle rect = ctl.RectangleToScreen( ctl.ClientRectangle );
                    rect.Location = new Point( rect.Location.X - pAdjust.X, rect.Location.Y - pAdjust.Y ); // ロケーションを補正
                    if ( rect.IntersectsWith( new Rectangle( pPoint.X, pPoint.Y, 1, 1 ) ) )
                    {
                        if ( ctl is Infragistics.Win.Misc.UltraButton )
                        {
                            //★クリックメッセージのシミュレートでは、クライアントフォームのボタン該当位置のリージョンが削除されている為通知不可
                            //Win32API.PostMessage( ctl.Handle, 0x0201, (IntPtr)0x0001, (IntPtr)0 );
                            //Win32API.PostMessage( ctl.Handle, 0x0202, (IntPtr)0x0000, (IntPtr)0 );

                            ( ctl as Infragistics.Win.Misc.UltraButton ).PerformClick();
                        }
                        //else if ( ctl is Infragistics.Win.UltraWinEditors.UltraPictureBox )
                        //{
                        //    //WM_LBUTTONDOWN              0x0201
                        //    //WM_LBUTTONUP                0x0202
                        //    //MK_LBUTTON                  0x0001
                        //    //WM_MOUSEACTIVATE            0x0021
                        //    //WM_NCHITTEST                0x0084
                        //    //HTCLIENT                    1
                        //    System.Diagnostics.Process[] prc = System.Diagnostics.Process.GetProcesses();
                        //    IntPtr mainWindowHandle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
                        //    //IntPtr mainWindowHandle =  System.Diagnostics.Process.GetProcesses().First().MainWindowHandle;

                        //    //Int32 result = Win32API.SendMessage( ctl.Handle, 0x0021, (IntPtr)mainWindowHandle, (IntPtr)( ( 0x0201 << 16 ) | ( 0x0001 ) ) );
                        //    //Int32 loc = ( ctl.Location.X << 16 | ctl.Location.Y );
                        //    //Int32 loc = ( 23 << 16 | 33 );
                        //    //Win32API.PostMessage( ctl.Handle, 0x0201, (IntPtr)0x0001, (IntPtr)loc );
                        //    //Win32API.PostMessage( ctl.Handle, 0x0202, (IntPtr)0x0000, (IntPtr)loc );
                        //    Win32API.PostMessage( ctl.Handle, 0x0201, (IntPtr)0x0001, (IntPtr)0 );
                        //    Win32API.PostMessage( ctl.Handle, 0x0202, (IntPtr)0x0000, (IntPtr)0 );
                        //}
                    }
                }
            }


            /// <summary>
            /// Paintイベントオーバーライド
            /// </summary>
            /// <remarks>
            /// Paintイベント時の描画内容に対して、アンチエイリアスを無効化します。
            /// </remarks>
            /// <param name="e">イベント情報</param>
            protected override void OnPaint( PaintEventArgs e )
            {
                // 描画のアンチエイリアス無効化
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                base.OnPaint( e );
            }

            //protected override void OnPaintBackground( PaintEventArgs e )
            //{
            //    // 背景描画のアンチエイリアス無効化
            //    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            //    e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            //    base.OnPaintBackground( e );
            //}

            #endregion

        }

        #endregion

    }
}
