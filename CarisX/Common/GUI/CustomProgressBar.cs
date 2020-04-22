using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using Infragistics.Win.UltraWinProgressBar;
using Oelco.Common.GUI;
using Oelco.Common.Utility;
using Oelco.Common.Log;

namespace Oelco.Common.GUI
{
    /// <summary>
    /// プログレスバー(閾値による色変化)
    /// </summary>
    public partial class CustomProgressBar : UltraProgressBar
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// プログレスバー色設定
        /// </summary>
        protected ColorStatus barColorSetting = new ColorStatus();

        /// <summary>
        /// プログレスバーイメージ設定
        /// </summary>
        protected ImageStatus barImageSetting = new ImageStatus();

        /// <summary>
        /// プログレスバー種別定義
        /// </summary>
        public enum ProgressBarType
        {
            /// <summary>
            /// 通常（色指定）
            /// </summary>
            Default,

            /// <summary>
            /// イメージ（画像設定）
            /// </summary>
            Image
        };

        #endregion

        #region [プロパティ]

        /// <summary>
        /// プログレスバー種別
        /// </summary>
        private ProgressBarType barType = ProgressBarType.Default;

        /// <summary>
        /// プログレスバー種別の取得、設定
        /// </summary>
        [Category("表示")]
        [Description("プログレスバー種別をImageに設定することで画像を適用します。")]
        public ProgressBarType BarType
        {
            get
            {
                return this.barType;
            }
            set
            {
                this.barType = value;
            }
        }

        /// <summary>
        /// プログレスバー色の取得、設定
        /// </summary>
        public ColorStatus BarColorSetting
        {
            get
            {
                return barColorSetting;
            }
        }

        /// <summary>
        /// プログレスバーのイメージの取得、設定
        /// </summary>
        public ImageStatus BarImageSetting
        {
            get
            {
                return barImageSetting;
            }
        }


        /// <summary>
        /// 現在値の取得、設定
        /// </summary>
        [RefreshProperties( RefreshProperties.Repaint )]
        public new Int32 Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                if (value > base.Maximum)
                {
                    //設定したい値が最大値を超えている場合、最大値を設定する
                    base.Value = base.Maximum;
                }
                else
                {
                    //最大値以下の場合はそのまま設定
                    base.Value = value;
                }
            }
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// 値変更時のイベントハンドラ
        /// </summary>
        /// <remarks>
        /// 値変更時のプログレスバー色を更新します。
        /// </remarks>
        /// <param name="e">イベントデータ</param>
        protected override void OnValueChanged( EventArgs e )
        {
            switch(this.barType)
            {
                case ProgressBarType.Image:
                    this.FillAppearance.ImageBackground = this.barImageSetting.GetImage(this.Value);
                    break;
                default:
                    this.FillAppearance.BackColor = this.barColorSetting.GetColor(this.Value);
                    break;
            }
            base.OnValueChanged( e );
        }
        #endregion

    }

    /// <summary>
    /// イメージ状態
    /// </summary>
    /// <remarks>
    /// 値の範囲とイメージのセットを保持します。
    /// </remarks>
    public class ImageStatus
    {
        #region [定数定義]

        /// <summary>
        /// デフォルトイメージ
        /// </summary>
        private Image DEFAULT_IMAGE = Oelco.Common.Properties.Resources.Image_Indicator_WhiteLarge;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 値域リスト
        /// </summary>
        SortedList<Int64, Image> rangeList = new SortedList<Int64, Image>();

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 色範囲設定追加
        /// </summary>
        /// <remarks>
        /// 値の下限と色を設定します、
        /// 設定された下限＜＝次に設定されている下限 で、指定色が有効となります。
        /// </remarks>
        /// <param name="rangeMin">色範囲下限</param>
        /// <param name="image">設定イメージ</param>
        public void AddImageRangePair(Int64 rangeMin, Image image)
        {
            // 重複値でなければリスト追加する。
            if (!this.rangeList.ContainsKey(rangeMin))
            {
                this.rangeList.Add(rangeMin, image);
            }
        }

        /// <summary>
        /// 指定値該当色取得
        /// </summary>
        /// <remarks>
        /// 指定値に該当する色を保持リストから検索し、取得します。
        /// 該当する色が無い場合、デフォルト色が取得されます。
        /// </remarks>
        /// <param name="value">指定値</param>
        /// <returns>指定値該当色</returns>
        public Image GetImage(Int64 value)
        {
            Image image = DEFAULT_IMAGE;

            // 保持値域リストから条件検索、
            // 指定値より小さな値のリストを取得し、
            // 存在する場合は該当リストの最後尾を使用する。
            // "検索結果要素 <= 指定値 < 検索結果除外要素"で取れるようにする。
            IEnumerable<Image> selected = from valPair in this.rangeList
                                          where valPair.Key <= value
                                          orderby valPair.Key ascending
                                          select valPair.Value;
            if (selected.Count() != 0)
            {
                image = selected.Last();
            }

            return image;
        }

        #endregion

    }
}
