using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Oelco.CarisX.Const;

namespace Oelco.CarisX.GUI.Controls
{
    /// <summary>
    /// 拡大率切替コントロールクラス
    /// </summary>
    public partial class ZoomPanel : UserControl
    {

        #region [インスタンス変数定義]
        
        /// <summary>
        /// ズーム率設定イベントハンドラ
        /// </summary>
        public event Action<Int32> SetZoom;
        
        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ZoomPanel()
        {
            InitializeComponent();

            // 規定値設定
            this.ZoomStep = CarisXConst.GRID_ZOOM_STEP;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ズーム変動率(規定値：10%) 設定/取得
        /// </summary>
        [DefaultValue(10)]
        public Int32 ZoomStep
        {
            get;
            set;
        }

        /// <summary>
        /// ズーム率 設定/取得
        /// </summary>
        public Int32 Zoom
        {
            get
            {
                return Int32.Parse( this.lblZoomPercent.Text );
            }
            set
            {
                this.lblZoomPercent.Text = value.ToString();
            }
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// ズームインボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ズーム変動率の設定値分、現在のズーム率から拡大します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnZoomIn_Click( object sender, EventArgs e )
        {
            if ( this.Zoom + this.ZoomStep <= CarisXConst.GRID_ZOOM_MAX )
            {
                this.lblZoomPercent.Text = ( this.Zoom + this.ZoomStep ).ToString();
            }
        }

        /// <summary>
        /// ズームアウトボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ズーム変動率の設定値分、現在のズーム率から縮小します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnZoomOut_Click( object sender, EventArgs e )
        {
            if ( this.Zoom - this.ZoomStep >= CarisXConst.GRID_ZOOM_MIN )
            {
                this.lblZoomPercent.Text = ( this.Zoom - this.ZoomStep ).ToString();
            }
        }

        /// <summary>
        /// 拡大率パーセント値変更イベント
        /// </summary>
        /// <remarks>
        /// 指定変動率にズーム率を変更します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void lblZoomPercent_TextChanged(object sender, EventArgs e)
        {
            if (this.SetZoom != null)
            {
                this.SetZoom(Int32.Parse(this.lblZoomPercent.Text));
            }
        }
        #endregion

    }
}
