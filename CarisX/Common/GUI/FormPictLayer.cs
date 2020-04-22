using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading.Tasks;


// TODO:コメント不十分

namespace Oelco.Common.GUI
{
    /// <summary>
    /// 画像表示
    /// </summary>
    public partial class FormPictLayer : Form//, IMoveObject
    {
        #region [インスタンス変数定義]

        //// 表示完了イベント
        //System.Threading.ManualResetEvent m_shownEvent = new System.Threading.ManualResetEvent( false );

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 画像コントロールの取得
        /// </summary>
        public Infragistics.Win.UltraWinEditors.UltraPictureBox PictureControl
        {
            get
            {
                return this.ultraPictureBox1;
            }
        }

        /// <summary>
        /// 表示画像イメージの取得、設定
        /// </summary>
        public Bitmap ShowImage
        {
            get
            {
                return (Bitmap)( this.ultraPictureBox1.Image );
            }
            set
            {
                this.ultraPictureBox1.Image = value;
                this.ultraPictureBox1.Refresh();
            }
        }

        /// <summary>
        /// コントロール作成必要情報の取得
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                const Int32 WS_EX_TOOLWINDOW = 0x00000080;

                // ExStyle に WS_EX_TOOLWINDOW ビットを立てる
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | WS_EX_TOOLWINDOW;

                return cp;
            }
        }

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormPictLayer()
        {
            InitializeComponent();
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 画像位置設定
        /// </summary>
        /// <remarks>
        /// 画像の位置設定を行います。
        /// </remarks>
        /// <param name="loc">画像位置(座標)</param>
        public void SetLocation( Point loc )
        {
            this.ultraPictureBox1.Location = loc;
        }

        /// <summary>
        /// 画像リソースの解放
        /// </summary>
        /// <remarks>
        /// 画像リソースの解放を行います。
        /// </remarks>
        public void CleanResource()
        {
            if ( this.ultraPictureBox1.Image != null )
            {
                Bitmap bmp = (Bitmap)this.ultraPictureBox1.Image;
                bmp.Dispose();
                this.ultraPictureBox1.Image = null;
            }
        }

        //public System.Threading.ManualResetEvent ShownEvent
        //{
        //    get
        //    {
        //        return m_shownEvent;
        //    }
        //}

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// Form読み込みイベント
        /// </summary>
        /// <remarks>
        /// Form読み込みイベント発生時の処理を行います。
        /// </remarks>
        /// <param name="e">イベントデータ</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );
        }

        #endregion

        #region [privateメソッド]

        //private void FormPictLayer_Shown( object sender, EventArgs e )
        //{
        //    //FormTransparentLayer.drawStart( this );
        //    m_shownEvent.Set();
        //}


        //private void FormPictLayer_Resize( object sender, EventArgs e )
        //{

        //    //this.ultraPictureBox1.Size = this.Size;
        //}

        #endregion
    }
}
