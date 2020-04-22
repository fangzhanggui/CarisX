using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using Oelco.Common.Utility;

namespace Oelco.Common.GUI
{
    /// <summary>
    /// フォームの基底クラス
    /// </summary>
    /// <remarks>
    /// 全てのフォームで継承されます。
    /// .NetのFormクラスへの処理を拡張します。
    /// </remarks>
    public partial class FormBase : Form
    {
        #region [クラス変数定義]

        /// <summary>
        /// サイズ変更の許可
        /// </summary>
        private Boolean allowResize = false;

        /// <summary>
        /// ユーザーレベル変更時イベントハンドラ
        /// </summary>
        private static event Action userLevelChanged;

        /// <summary>
        /// 全フォーム内コントロール状態切り替えイベントハンドラ
        /// </summary>
        private static event Func<Boolean, List<Tuple<Control, Boolean>>> allControlEnableChange;

        /// <summary>
        /// 全フォーム保持コントロール有効状態設定による変動情報
        /// </summary>
        protected static Dictionary<Object, List<Tuple<Control, Boolean>>> lastCalledAllEnableChangeStatus = null;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormBase()
        {
            InitializeComponent();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// サイズ変更の許可
        /// </summary>
        protected Boolean AllowResize
        {
            get
            {
                return this.allowResize;
            }
            set
            {
                this.allowResize = value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 全フォーム保持コントロール有効状態設定
        /// </summary>
        /// <remarks>
        /// 現在有効な全FormBase継承インスタンスに対して、保持する全コントロールの有効状態を設定します。
        /// </remarks>
        /// <param name="enable">設定状態</param>
        public static void AllFormItemEnableChange( Boolean enable )
        {
            if ( FormBase.allControlEnableChange != null )
            {
                lastCalledAllEnableChangeStatus = new Dictionary<object, List<Tuple<Control, Boolean>>>();
                foreach ( Func<Boolean, List<Tuple<Control, Boolean>>> delSingle in FormBase.allControlEnableChange.GetInvocationList() )
                {
                    lastCalledAllEnableChangeStatus.Add( delSingle.Target, delSingle( enable ) );
                }
            }
        }

        /// <summary>
        /// 全フォーム保持コントロール有効状態復元
        /// </summary>
        /// <remarks>
        /// 直近の全フォーム保持コントロール有効状態設定呼び出しにより行われた変更を元に戻します。
        /// この関数は、呼び出されると使用した復元用の情報を破棄します。
        /// </remarks>
        public static void AllFormItemEnableStatusRestore()
        {
            if (lastCalledAllEnableChangeStatus != null )
            {
                foreach ( var restoreInf in lastCalledAllEnableChangeStatus )
                {
                    foreach ( var pair in restoreInf.Value )
                    {
                        pair.Item1.Enabled = pair.Item2;
                    }
                }
                lastCalledAllEnableChangeStatus = null;
            }
        }

        /// <summary>
        /// ユーザーレベル変更時イベント呼び出し
        /// </summary>
        /// <remarks>
        /// ユーザレベル変更時イベントハンドらを呼び出します。
        /// </remarks>
        public static void UserLevelChanged()
        {
            if ( FormBase.userLevelChanged != null )
            {
                FormBase.userLevelChanged();
            }
        }

        /// <summary>
        /// 有効状態設定
        /// </summary>
        /// <remarks>
        /// コントロールコレクション内容全てに有効状態を設定します。
        /// </remarks>
        /// <param name="enable">有効状態</param>
        /// <param name="controlCollection">対象コントロールコレクション</param>
        /// <returns>変更前状態</returns>
        private List<Tuple<Control, Boolean>> statusChange( Boolean enable, Control.ControlCollection controlCollection )
        {
            List<Tuple<Control, Boolean>> result = new List<Tuple<Control, bool>>();

            foreach ( Control col in controlCollection )
            {
                // 可視のものに対してのみ処理を行う。
                if ( col.Visible )
                {
                    if ( enable )
                    {
                        // 保持コントロールの状態を先にリスト追加する。
                        // パネルやグループボックス等は、親コントロールを無効にすると子コントロール全てに無効を設定する為。
                        result = result.Union( this.statusChange( enable, col.Controls ) ).ToList();
                    }
                    result.Add( new Tuple<Control, Boolean>( col, col.Enabled ) );
                    col.Enabled = enable;
                }
            }

            return result;
        }

        /// <summary>
        /// 有効状態設定
        /// </summary>
        /// <remarks>
        /// このフォームが保持するコントロール全てに有効状態を設定します。
        /// </remarks>
        /// <param name="enable">有効状態</param>
        /// <returns>変更前状態</returns>
        protected virtual List<Tuple<Control, Boolean>> statusChangeAll( Boolean enable )
        {
            List<Tuple<Control, bool>> result = new List<Tuple<Control, bool>>();

            // 可視のものに対してのみ処理を行う。
            if ( this.Visible )
            {
                result = this.statusChange( enable, this.Controls );
            }
            return result;
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// ウィンドウプロシージャオーバーライド
        /// </summary>
        /// <remarks>
        /// ウィンドウプロシージャのメッセージ処理を行います。
        /// </remarks>
        /// <param name="m">メッセージ情報</param>
        protected override void WndProc( ref Message m )
        {
            const int WM_NCHITTEST = 0x84;
            //const int WM_SYSCOMMAND = 0x112;
            //const int SC_MOVE = 0xf010;
            //const int SC_SIZE = 0xf000;
            const Int32 WM_SYSCOMMAND = 0x112;
            const Int32 SC_SIZE = 0xf000;
            /// <summary>ウィンドウ境界の左端の線上にあります。</summary>
            const int HTLEFT = 10;
            /// <summary>ウィンドウの右境界線にあります。</summary>
            const int HTRIGHT = 11;
            /// <summary>ウィンドウ境界の上端の線上にあります。</summary>
            const int HTTOP = 12;
            /// <summary>ウィンドウ境界線の左上隅にあります。</summary>
            const int HTTOPLEFT = 13;
            /// <summary>ウィンドウ境界線の右上隅にあります。</summary>
            const int HTTOPRIGHT = 14;
            /// <summary>ウィンドウの下端の境界線にあります。</summary>
            const int HTBOTTOM = 15;
            /// <summary>ウィンドウ境界の左下隅にあります。</summary>
            const int HTBOTTOMLEFT = 16;
            /// <summary>ウィンドウ境界の右下隅にあります。</summary>
            const int HTBOTTOMRIGHT = 17;
            /// <summary>サイズ変更境界を保持しないウィンドウの境界内にあります。</summary>
            const int HTBORDER = 18;

            // WM_SYSCOMMAND (SC_SIZE) を無視することでフォームをサイズ変更できないようにする
            if ( !this.allowResize )
            {
                // 境界線上でマウス カーソルが変更されないようにする
                if ( m.Msg == WM_NCHITTEST )
                {
                    base.WndProc( ref m );

                    switch ( m.Result.ToInt32() )
                    {
                    case HTLEFT:
                    case HTRIGHT:
                    case HTTOP:
                    case HTTOPLEFT:
                    case HTTOPRIGHT:
                    case HTBOTTOM:
                    case HTBOTTOMLEFT:
                    case HTBOTTOMRIGHT:
                        m.Result = new IntPtr( HTBORDER );
                        break;
                    }

                    return;
                }
                else if ( m.Msg == WM_SYSCOMMAND )
                {
                    Int32 wparam = m.WParam.ToInt32() & 0xfff0;
                    if ( wparam == SC_SIZE )
                    {
                        return;
                    }
                }
            }

            base.WndProc( ref m );
        }

        /// <summary>
        /// リソースの初期化
        /// </summary>
        /// <remarks>
        /// リソースの初期化を行います。
        /// </remarks>
        private void initializeResourceBase()
        {
            // 派生先で実装
            this.initializeResource();
        }

        /// <summary>
        /// コンポーネントの初期化
        /// </summary>
        /// <remarks>
        /// コンポーネントの初期化を行います。
        /// </remarks>
        private void initializeFormComponentBase()
        {
            // 派生先で実装
            this.initializeFormComponent();
        }

        /// <summary>
        /// 言語設定
        /// </summary>
        /// <remarks>
        /// 言語設定を行います。派生先で実装します。
        /// </remarks>
        private void setCultureBase()
        {
            // 派生先で実装
            this.setCulture();
        }

        /// <summary>
        /// リソースの初期化
        /// </summary>
        /// <remarks>
        /// リソースの初期化を行います。
        /// </remarks>
        protected virtual void initializeResource()
        {
            //このクラスはinitializeResourceが実装されていません
            System.Diagnostics.Debug.WriteLine(String.Format("This class does not implement the initializeResource。({0})", this.GetType().Name));
        }

        /// <summary>
        /// コンポーネントの初期化
        /// </summary>
        /// <remarks>
        /// コンポーネントの初期化を行います。
        /// </remarks>
        protected virtual void initializeFormComponent()
        {
            //このクラスはinitializeFormComponentが実装されていません
            System.Diagnostics.Debug.WriteLine(String.Format("This class does not implement the initializeFormComponent。({0})", this.GetType().Name));
        }

        /// <summary>
        /// 言語設定
        /// </summary>
        /// <remarks>
        /// 言語設定を行います。
        /// </remarks>
        protected virtual void setCulture()
        {
            //このクラスはsetCultureが実装されていません
            System.Diagnostics.Debug.WriteLine(String.Format("This class does not implement the setCulture。({0})", this.GetType().Name));
        }

        /// <summary>
        /// ユーザレベル設定
        /// </summary>
        /// <remarks>
        /// ユーザレベル設定を行います。
        /// </remarks>
        //protected virtual void setUser()
        //{
        //    System.Diagnostics.Debug.WriteLine( String.Format( "このクラスはsetUserが実装されていません。({0})", this.GetType().Name ) );
        //}

        /// <summary>
        /// ユーザレベル設定
        /// </summary>
        /// <remarks>
        /// ユーザレベル設定を行います。
        /// </remarks>
        /// <param name="value"></param>
        protected virtual void setUser( Object value )
        {
            //このクラスはsetUserが実装されていません
            System.Diagnostics.Debug.WriteLine(String.Format("This class does not implement the setUser。({0})", this.GetType().Name));
        }

        /// <summary>
        /// Loadイベント発生
        /// </summary>
        /// <remarks>
        /// Loadイベントを発生させます。
        /// </remarks>
        /// <param name="e">イベントデータ</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            // 言語設定
            this.setCultureBase();

            // 各初期化
            this.initializeFormComponentBase();
            this.initializeResourceBase();

            // ユーザーレベル変更イベント追加
            FormBase.userLevelChanged += this.onUserLevelChanged;
            // 全コントロール状態切り替えイベント追加
            FormBase.allControlEnableChange += this.statusChangeAll;
        }

        /// <summary>
        /// OnClosingイベントオーバーライド
        /// </summary>
        /// <remarks>
        /// このクラスが固有で保持するリソースの開放処理を行います。
        /// </remarks>
        /// <param name="e"></param>
        protected override void OnClosing( CancelEventArgs e )
        {
            // ユーザーレベル変更イベント解除
            FormBase.userLevelChanged -= this.onUserLevelChanged;

            // 全コントロール状態切り替えイベント解除
            FormBase.allControlEnableChange -= this.statusChangeAll;

            base.OnClosing( e );
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// ユーザー切り替え時初期化実施フラグ
        /// </summary>
        private Boolean initializeOnUserChange = true;

        /// <summary>
        /// ユーザー切り替え時初期化実施フラグの取得、設定
        /// </summary>
        protected Boolean InitializeOnUserChange
        {
            get
            {
                return this.initializeOnUserChange;
            }
            set
            {
                this.initializeOnUserChange = value;
            }
        }

        /// <summary>
        /// ユーザーレベル変更時イベント
        /// </summary>
        private void onUserLevelChanged()
        {
            if ( this.initializeOnUserChange )
            {
                // 各初期化
                this.initializeFormComponent();
                this.initializeResource();
            }

            this.setUser( null );
        }

        #endregion

    }
}
