//----------------------------------------------------------------
// Public Class.
//          NumericEditor.cs
//	         
// Info.
//   UltraNumericエディターに以下の機能を追加して提供する
//     通常のMaxValue、MinValueとは別にMaxRangeValue、MinRangeValueプロパティを提供
//     入力範囲の検証機能（ MinRangeValue <= Value <= MaxRangeValue ）
// History
//   2011/12/01  Ver1.00.00  作成  Tomoaki Hanamachi
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinEditors;
using System.Globalization;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Oelco.Common.GUI
{
    /// <summary>
    /// NumericEditorユーザーコントロール
    /// </summary>
    /// <remarks>
    /// UltraNumericEditorを継承したカスタムコントロール
    /// 通常のMaxValue、MinValueとは別に閾値（MaxRangeValue、MinRangeValue）プロパティ
    /// を追加し、IsValidメソッドではその範囲をチェックする
    /// </remarks>
    public partial class NumericEditor : UltraNumericEditor
    {
        // Windows10対応
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean PostMessage(int hWnd, uint Msg, int wParam, int lParam);

        // Windows10対応
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);

        #region [イベント]
        
        /// <summary>
        /// 閾値変更イベントデリゲート
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void RangeChangedEventHandler( object sender, EventArgs e );

        /// <summary>
        /// 閾値変更イベント
        /// </summary>
        public event RangeChangedEventHandler RangeChanged;

        /// <summary>
        /// 閾値変更イベントの引数
        /// </summary>
        protected PropertyChangedEventArgs propChangedEventArgs;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 設定可能範囲最大値
        /// </summary>
        private Object maxRangeValue = null;
        /// <summary>
        /// 設定可能範囲最小値
        /// </summary>
        private Object minRangeValue = null;

        #endregion

        #region [コンストラクタ/デストラクタ]
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NumericEditor()
        {
            InitializeComponent();
            
            this.MaxRangeValue = this.MaxValue;
            this.MinRangeValue = this.MinValue;
            this.Nullable = true;
            this.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;

            // Windows10の場合のみ処理を行う
            System.OperatingSystem os = System.Environment.OSVersion;
            if (os.Platform == System.PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 2)
            {
                // ソフトウェアキーボードを閉じるためのイベント登録
                // ※開くほうはWinProcのクリックイベントにて実装
                this.Leave += new EventHandler(this.NumericEditor_OnLeave);
            }
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 入力閾値　最大値
        /// </summary>
        [DefaultValue( 0 )]
        [Category( "動作" )]
        [Description( "入力閾値（最大値）を指定します。" )]
        [TypeConverter( typeof( StringConverter ) )]
        public Object MaxRangeValue
        {
            get
            {
                return this.maxRangeValue;
            }
            set
            {
                this.maxRangeValue = value;
                this.OnRangeChanged();
            }
        }

        /// <summary>
        /// 入力閾値　最小値
        /// </summary>
        [DefaultValue( 0 )]
        [Category( "動作" )]
        [Description( "入力閾値（最小値）を指定します。" )]
        [TypeConverter( typeof( StringConverter ) )]
        public Object MinRangeValue
        {
            get
            {
                return this.minRangeValue;
            }
            set
            {
                this.minRangeValue = value;
                this.OnRangeChanged();
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 入力反映検証
        /// </summary>
        /// <remarks>
        /// 入力反映検証を行います。
        /// </remarks>
        /// <returns>True：有効　False：無効</returns>
        public virtual Boolean IsValid()
        {
            Boolean bRet = false;

            // 無効値を入力した直後に、フォーカスアウトしないと
            // 画面に値が表示されない現象の対策
            if ( this.Focused )
            {
                this.Value = this.Value;
            }

            if ( this.Value != DBNull.Value )
            {
                // 閾値外の値が設定されている場合はエラーとする
                switch ( this.NumericType )
                {
                case NumericType.Integer:
                    bRet = Convert.ToInt32( this.MinRangeValue ) <= Convert.ToInt32( this.Value )
                        && Convert.ToInt32( this.Value ) <= Convert.ToInt32( this.MaxRangeValue );
                    break;
                case NumericType.Double:
                    bRet = Convert.ToDouble( this.MinRangeValue ) <= Convert.ToDouble( this.Value )
                        && Convert.ToDouble( this.Value ) <= Convert.ToDouble( this.MaxRangeValue );
                    break;
                case NumericType.Decimal:
                    bRet = Convert.ToDecimal( this.MinRangeValue ) <= Convert.ToDecimal( this.Value )
                        && Convert.ToDecimal( this.Value ) <= Convert.ToDecimal( this.MaxRangeValue );
                    break;
                default:
                    bRet = false;
                    break;
                }
            }

            return bRet;
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// 閾値の変更イベントの通知
        /// </summary>
        /// <remarks>
        /// 閾値の変更イベントを発生させます。
        /// </remarks>
        protected void OnRangeChanged()
        {
            if ( RangeChanged != null )
            {
                propChangedEventArgs = new PropertyChangedEventArgs( "RangeChanged" );
                RangeChanged( this, propChangedEventArgs );
            }
        }

        /// <summary>
        /// ウィンドウイベントの通知
        /// </summary>
        /// <remarks>
        /// ウィンドウイベントを発生させます。
        /// </remarks>
        protected override void WndProc(ref Message m)
        {
            // 右クリック
            const int WM_LBUTTONDOWN = 0x201;
            const int WM_NCLBUTTONDOWN = 0xA1;

            // クリックイベントを取得
            if (((Int32)m.WParam == WM_LBUTTONDOWN) ||
                ((Int32)m.Msg == WM_LBUTTONDOWN) ||
                ((Int32)m.WParam == WM_NCLBUTTONDOWN) ||
                ((Int32)m.Msg == WM_NCLBUTTONDOWN))
            {
                // Windows10の場合のみ処理を行う
                System.OperatingSystem os = System.Environment.OSVersion;
                if (os.Platform == System.PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 2)
                {
                    // ソフトウェアキーボードを開く
                    ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe");
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.Start(startInfo);
                }
            }

            base.WndProc(ref m);
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// フォーカスが外れた通知
        /// </summary>
        /// <param name="sender">発生元オブジェクト</param>
        /// <param name="e">イベントパラメーター</param>
        private void NumericEditor_OnLeave(object sender, EventArgs e)
        {
            // Windows10の場合のみ処理を行う
            System.OperatingSystem os = System.Environment.OSVersion;
            if (os.Platform == System.PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 2)
            {
                // ソフトウェアキーボードを閉じる
                uint WM_SYSCOMMAND = 274;
                uint SC_CLOSE = 61536;
                IntPtr KeyboardWnd = FindWindow("IPTip_Main_Window", null);
                PostMessage(KeyboardWnd.ToInt32(), WM_SYSCOMMAND, (int)SC_CLOSE, 0);
            }
        }

        #endregion
    }
}
