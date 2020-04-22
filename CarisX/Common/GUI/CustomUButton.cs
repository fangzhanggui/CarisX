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
	/// カスタムボタンクラス
	/// </summary>
	/// <remarks>
	/// UltraBugttonに対し、フリックによるボタン押下時の外観調整や、
	/// 点滅表示機能を追加したクラスです。
	/// </remarks>
    public class CustomUButton : Infragistics.Win.Misc.UltraButton
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CustomUButton()
            : base()
        {
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
            }
            base.Dispose( disposing );
        }

        #endregion
                
        #region [protectedメソッド]

        /// <summary>
        /// MouseDownイベント
        /// </summary>
        /// <remarks>
        /// タッチによるボタン押下時の外観変化対応
        /// 通常のマウスクリックによるイベント順序はDown->Click->Up、Down後にボタンからマウスを外して押上の場合Down->Up
        /// </remarks>
        protected override void OnMouseDown( System.Windows.Forms.MouseEventArgs e )
        {
            //DebugLogger.Writeline( DateTime.Now.ToString("dd HH:mm:ss.fffffff") + " OnMouseDown" );
            //			System.Windows.Forms.MessageBox.Show( "OnMouseDown" );
            //mMouseDownCalled = true;
            base.OnMouseDown( e );
            this.Refresh();
        }

        /// <summary>
        /// MouseUpイベント
        /// </summary>
        /// <remarks>
        /// タッチによるボタン押下時の外観変化対応
        /// Sleepを入れることで、OnMouseDownで変化した外観の描画を見えるようにする。
        /// </remarks>
        protected override void OnMouseUp( System.Windows.Forms.MouseEventArgs e )
        {
            System.Threading.Thread.Sleep( 20 );
            //DebugLogger.Writeline( DateTime.Now.ToString( "dd HH:mm:ss.fffffff" ) + " OnMouseUp" );
            //System.Windows.Forms.MessageBox.Show( "OnMouseUp" );
            //mMouseDownCalled = false;
            base.OnMouseUp( e );
            this.Refresh();
        }

        #endregion

    }	
}
