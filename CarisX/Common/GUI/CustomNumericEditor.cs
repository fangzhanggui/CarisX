using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Shared;

namespace Oelco.Common.GUI
{
    /// <summary>
    /// カスタムNumericEditor
    /// </summary>
    public partial class CustomNumericEditor : UltraNumericEditor
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CustomNumericEditor()
        {
            InitializeComponent();
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// 初期化完了通知
        /// </summary>
        /// <remarks>
        /// カスタムコントロール共通のプロパティ設定を行います。
        /// MaskInputが「-nnn.nn」形式の場合、「{double:-3.2:c}」形式に変換します。
        /// </remarks>
        protected override void OnEndInit()
        {
            // デザインモードの時は以降の処理を行わない。
            if ( this.DesignMode)
            {
                return;
            }
            
            // IMEモードを無効にする
            this.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ImeModeBase = System.Windows.Forms.ImeMode.Disable;

            // Tabキー押下時の移動設定
            this.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;

            // MaskInputプロパティの編集
            switch ( this.NumericType )
            {
            case NumericType.Integer:
                // integerは小数移動がないので変更不要
                break;
            case NumericType.Double:
            //case NumericType.Decimal:
                //MaskInputが「nnn.nn」形式で設定されている場合「{double:3.2:c}」形式に変える
                if ( !String.IsNullOrEmpty( this.MaskInput ) && this.MaskInput.Contains( "n" ) )
                {
                    String wkStr = String.Empty;
                    //if (this.MaskInput.Contains("{LOC}"))
                    //    wkStr = "{LOC}";
                    if ( this.MaskInput.Contains( "-" ) )
                        wkStr += "-";
                    if ( this.MaskInput.Contains( "+" ) )
                        wkStr += "+";

                    String wk = this.MaskInput.Replace( "{LOC}", "" );
                    if ( wkStr.Length > 0 )
                    {
                        wk = wk.Replace( wkStr, "" );
                    }

                    String[] mask = wk.Split( '.' );
                    if ( mask.Length == 2 )
                    {
                        this.MaskInput = "{" + String.Format( @"double:{0}{1}.{2}:c", wkStr, mask[0].Length.ToString(), mask[1].Length.ToString() ) + "}";
                    }
                }
                else if ( String.IsNullOrEmpty( this.MaskInput ) )
                {
                    // DoubleのDefaultのMaskInputを「{Double:}」形式で設定する
                    this.MaskInput = "{double:9.2:c}";
                }

                break;
            //case NumericType.Decimal:
            //    // Decimalは[{decimal:}]形式の値を設定できないので変更不可
            //    break;
            }
                        

            base.OnEndInit();

        }

        /// <summary>
        /// フォーカス取得時処理
        /// </summary>
        /// <remarks>
        /// フォーカス取得時処理を行います。
        /// </remarks>
        /// <param name="e"></param>
        protected override void OnEnter( EventArgs e )
        {           
            base.OnEnter( e );

            // Textを全選択状態にする
            this.SelectAll();
        }       

        /// <summary>
        /// KeyDownイベント処理
        /// </summary>
        /// <remarks>
        /// KeyDownイベント処理を行います。
        /// </remarks>
        /// <param name="e"></param>
        protected override void OnKeyDown( KeyEventArgs e )
        {
            //if ( ( ( e.Modifiers & Keys.Control ) == Keys.Control ) && e.KeyCode == Keys.V )
            //{
            //    // ペーストにより全角数値を入力しようとした際の防御策
            //    IDataObject data = Clipboard.GetDataObject();
            //    String str = (String)data.GetData( DataFormats.Text );
            //    Decimal wk;
            //    if ( !Decimal.TryParse( str, out wk ) )
            //    {
            //        // クリップボードの中身が半角数字以外はキー入力を無効にする
            //        e.SuppressKeyPress = true;
            //        return;
            //    }
            //}

            if ( this.SelectedText.Length > 0 )
            {
                if ( ( e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus ) && ( this.MaskInput.Contains( "-" ) || this.MaskInput.Contains( "+" ) ) && !this.Editor.CurrentEditText.Contains( "-" ) )
                {
                    // 選択部分を削除
                    SendKeys.Send( "{DEL}" );
                    // 押下したマイナスキーが反応しなくなるのでもう一度送信
                    SendKeys.Send( "{SUBTRACT}" );
                }
                else if ( ( e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus ) && ( !this.Editor.CurrentEditText.Contains( "+" ) && this.Editor.CurrentEditText.Contains( "-" ) ) )
                {
                    // 選択部分を削除
                    SendKeys.Send( "{DEL}" );
                    // 押下したプラスキーが反応しなくなるのでもう一度送信
                    SendKeys.Send( "{ADD}" );
                }
            }
            base.OnKeyDown( e );

            // 確定後の値を退避
            //beforeValue = this.Value;
        }


        ///// <summary>
        ///// MouseDownイベント処理
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnMouseDown( MouseEventArgs e )
        //{
        //    if ( e.Button == System.Windows.Forms.MouseButtons.Right )
        //    {
        //        // クリップボードの中身に数値以外(全角数字)があれば、消す
        //        // コンテキストメニューで「貼り付け」をされた時の対応。
        //        // ※ WndProcなどで防御出来ないので応急措置
        //        IDataObject data = Clipboard.GetDataObject();
        //        String str = (String)data.GetData( DataFormats.Text );
        //        Decimal wk;
        //        if ( !Decimal.TryParse( str, out wk ) )
        //        {
        //            Clipboard.Clear();
        //        }
        //    }
        //    base.OnMouseDown( e );
        //}


        //protected override void OnValueChanged( EventArgs args )
        //{
        //    base.OnValueChanged( args );

        //    String str = this.Editor.CurrentEditText;

        //    byte[] byte_data = System.Text.Encoding.GetEncoding( 932 ).GetBytes( str );
        //    if ( byte_data.Length != str.Length )
        //    {
        //        this.Value = beforeValue;
        //    }
        //    // 確定後の値を退避
        //    beforeValue = this.Value;
        //}


        //public new void Paste()
        //{
        //    IDataObject data = Clipboard.GetDataObject();
        //    String str = (String)data.GetData( DataFormats.Text );
        //    Decimal wk;
        //    if ( !Decimal.TryParse( str, out wk ) )
        //    {

        //        return;
        //    }
        //}

        //protected override void WndProc( ref Message m )
        //{
        //    const Int32 WM_PASTE = 0x0302;

        //    if ( m.Msg == WM_PASTE )
        //    {
        //        IDataObject data = Clipboard.GetDataObject();
        //        String str = (String)data.GetData( DataFormats.Text );
        //        Decimal wk;
        //        if ( !Decimal.TryParse( str, out wk ) )
        //        {
        //            // クリップボードの中身が半角数字以外はキー入力を無効にする
        //            return;
        //        }     
        //    }

        //    base.WndProc( ref m );
        //}

        #endregion

    }
}
