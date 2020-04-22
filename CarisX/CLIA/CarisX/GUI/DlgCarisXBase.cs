using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Shared;
using Oelco.Common.GUI;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// CarisXダイアログベース
    /// </summary>
    public partial class DlgCarisXBase : FormBlackOut
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// タイトル文字列
        /// </summary>
        private String caption;

        /// <summary>
        /// タイトル外観
        /// </summary>
        private AppearanceBase captionAppearance = new Infragistics.Win.Appearance();

        /// <summary>
        /// タイトル外観ベースデータ
        /// </summary>
        private AppearanceData captionAppearanceBaseData; 

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgCarisXBase()
        {
            InitializeComponent();
            base.Text = null;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CaptionAppearance.SubObjectPropChanged += new SubObjectPropChangeEventHandler( CaptionAppearance_SubObjectPropChanged );

            this.captionAppearanceBaseData.TextHAlign = HAlign.Center;
            this.captionAppearanceBaseData.TextVAlign = VAlign.Middle;
            this.captionAppearanceBaseData.BackColor = Color.LightGray;
            this.captionAppearanceBaseData.ForeColor = Color.Black;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// このコントロールに関連付けられたテキスト
        /// </summary>
        [SettingsBindable( true )]
        [BrowsableAttribute( false )]
        [DesignOnly(true)]
        public override String Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                // 設定を無効化(overrideによる)
            }
        }

        /// <summary>
        /// ダイアログタイトルに表示されるテキストの取得、設定
        /// </summary>
        [Category("表示")]
        [SettingsBindable( true )]
        public String Caption
        {
            get
            {
                return this.caption;
            }
            set
            {
                this.caption = value;

                // ダイアログのタイトル用ラベルに設定
                this.lblDialogTitle.Text = this.caption;
            }
        }

        /// <summary>
        /// ダイアログタイトルのデフォルトの外観の取得、設定
        /// </summary>
        [SettingsBindable( true )]
        public AppearanceBase CaptionAppearance
        {
            get
            {
                return this.captionAppearance;
            }
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// パネルサイズ変更イベント
        /// </summary>
        /// <remarks>
        /// パネルサイズの変更を行います
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void pnlDialogMain_SizeChanged( object sender, EventArgs e )
        {
            this.lblDialogTitle.Size = new Size( this.pnlDialogMain.Width - 50, this.lblDialogTitle.Size.Height );
            this.lblDialogTitle.Location = new Point( this.pnlDialogMain.Width / 2 - this.lblDialogTitle.Size.Width / 2, this.lblDialogTitle.Location.Y );
        }

        /// <summary>
        /// タイトル外観の変更イベント
        /// </summary>
        /// <remarks>
        /// タイトル外観の変更を行います
        /// </remarks>
        /// <param name="propChange"></param>
        private void CaptionAppearance_SubObjectPropChanged( Infragistics.Shared.PropChangeInfo propChange )
        {
            this.lblDialogTitle.Appearance = new Infragistics.Win.Appearance( ref this.captionAppearanceBaseData );
            AppearanceData data = this.captionAppearance.Data;
            AppearancePropFlags flags = this.lblDialogTitle.Appearance.NonDefaultSettings;
            this.lblDialogTitle.Appearance.MergeData( ref data, ref flags );
            this.lblDialogTitle.Appearance = new Infragistics.Win.Appearance(ref data);
        }

        #endregion
    }
}
