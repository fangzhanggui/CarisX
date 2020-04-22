using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Oelco.Common.GUI
{
    /// <summary>
    /// 各子画面の基底クラス
    /// </summary>
    public partial class FormChildBase : FormTransitionBase
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 画面タイトル（画面グループ＋画面種別）
        /// </summary>
        public string dispTitle = "";

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormChildBase()
        {
            InitializeComponent();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 親フォームの取得、設定
        /// </summary>
        static public Form BaseForm
        {
            get;
            set;
        }

        /// <summary>
        /// 子フォームの取得、設定
        /// </summary>
        static public Point SubFormLocation
        {
            get;
            set;
        }

        /// <summary>
        /// 編集状態を設定／取得します
        /// </summary>
        static public Boolean IsEdit
        {
            set;
            get;
        }

        #endregion

        #region [publicメソッド]
        
        /// <summary>
        /// 各種データの更新
        /// </summary>
        /// <remarks>
        /// 各種データの更新します
        /// </remarks>
        public virtual void RefleshData()
        {
            // TODO:実装＆XMLコメント作成
        }

        /// <summary>
        /// フォーム表示
        /// </summary>
        /// <remarks>
        /// フォームの表示を行います。
        /// </remarks>
        public override void Show()
        {
            // 親フォームでない場合(子フォームの場合のみ)
            this.Location = FormChildBase.SubFormLocation;
            this.Owner = FormChildBase.BaseForm;

            base.Show();
        }

        #endregion
        
    }
}
