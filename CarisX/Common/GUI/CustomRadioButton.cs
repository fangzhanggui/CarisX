using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;

namespace Oelco.Common.GUI
{
    /// <summary>
    /// CarisX既定カスタムデザインUltraOptionSet
    /// </summary>
    public class CustomURadioButton : UltraRadioButton
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// GlyphInfo既定値
        /// </summary>
        private GlyphInfoBase glyphInfoDefault = new RadioButtonImageGlyphInfo( Oelco.Common.Properties.Resources.Image_OptionSetGlyphInfo, "カスタム ラジオ ボタン グリフ" );

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CustomURadioButton()
        {
            this.HandleCreated += new EventHandler( CustomRadioButton_HandleCreated );
            this.BorderStyle = UIElementBorderStyle.None;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラジオ ボタンが描画される方法を決定します。
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new GlyphInfoBase GlyphInfo
        {
            get
            {
                if (base.GlyphInfo != this.glyphInfoDefault && base.GlyphInfo == null)
                    base.GlyphInfo = glyphInfoDefault;
                return base.GlyphInfo;
            }
            set
            {
                base.GlyphInfo = value;
            }
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// ハンドル生成イベント
        /// </summary>
        /// <remarks>
        /// ハンドル生成イベント処理を行います。
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        void CustomRadioButton_HandleCreated( object sender, EventArgs e )
        {
            // GlyphInfoの初期化(既定)
            this.GlyphInfo = glyphInfoDefault;
        }

        /// <summary>
        /// プロパティシリアル化必要有無の取得
        /// </summary>
        /// <remarks>
        /// Infragistics.Win.UltraWinEditors.UltraOptionSet.GlyphInfo プロパティがデフォルト値に設定されているかどうかを示すブール値を返します。
        /// </remarks>
        /// <returns>プロパティのシリアル化が必要かどうかを示すブール値</returns>
        private new Boolean ShouldSerializeGlyphInfo()
        {
            return this.GlyphInfo != glyphInfoDefault;
        }

        /// <summary>
        /// GlyphInfoプロパティのリセット
        /// </summary>
        /// <remarks>
        /// Infragistics.Win.UltraWinEditors.UltraOptionSet.GlyphInfo プロパティをデフォルト値にリセットします。
        /// </remarks>
        private new void ResetGlyphInfo()
        {
            this.GlyphInfo = glyphInfoDefault;
        }

        #endregion

    }
}
