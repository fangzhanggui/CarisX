using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// システム構成各種設定用基底ダイアログ
    /// </summary>
    public class DlgCarisXBaseSys : DlgCarisXBase
    {
        #region [プロパティ]

        /// <summary>
        /// 使用・未使用の設定・取得
        /// </summary>
        public virtual Boolean UseConfig
        {
            get;
            set;
        }

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgCarisXBaseSys()
        {
        }

        #endregion

    }
}
