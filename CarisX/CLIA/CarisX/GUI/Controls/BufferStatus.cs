using Oelco.CarisX.Const;
using Oelco.Common.Utility;
using System.Drawing;
using System.Windows.Forms;

namespace Oelco.CarisX.GUI.Controls
{
    /// <summary>
    /// バッファーステータス表示コントロール
    /// </summary>
    public partial class BufferStatus : UserControl
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BufferStatus()
        {
            InitializeComponent();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ステータス
        /// </summary>
        public TotalBufferStatus Status
        {
            get
            {
                // 単体試験時、既存コード内にリソースの画像データを使用したインスタンス同士の比較処理が発見された為、
                // 対応を追加
                if (SubFunction.GetMD5HashFromImageData(this.pbxBufferIcon.DefaultImage) ==
                    SubFunction.GetMD5HashFromImageData(Properties.Resources.Image_TotalBufferStatus_Cross))
                {
                    return TotalBufferStatus.cross;
                }
                else if (SubFunction.GetMD5HashFromImageData(this.pbxBufferIcon.DefaultImage) ==
                    SubFunction.GetMD5HashFromImageData(Properties.Resources.Image_TotalBufferStatus_Exclamation))
                {
                    return TotalBufferStatus.exclamation;
                }
                else if (SubFunction.GetMD5HashFromImageData(this.pbxBufferIcon.DefaultImage) ==
                    SubFunction.GetMD5HashFromImageData(Properties.Resources.Image_TotalBufferStatus_Circle))
                {
                    return TotalBufferStatus.circle;
                }
                else
                {
                    return TotalBufferStatus.None;
                }
            }
            set
            {
                switch (value)
                {
                    case TotalBufferStatus.None:
                        this.pbxBufferIcon.DefaultImage = Properties.Resources.Image_TotalBufferStatus_None;
                        this.pbxBufferIcon.BackColor = Color.DarkGray;
                        break;
                    case TotalBufferStatus.circle:
                        this.pbxBufferIcon.DefaultImage = Properties.Resources.Image_TotalBufferStatus_Circle;
                        this.pbxBufferIcon.BackColor = Color.White;
                        break;
                    case TotalBufferStatus.exclamation:
                        this.pbxBufferIcon.DefaultImage = Properties.Resources.Image_TotalBufferStatus_Exclamation;
                        this.pbxBufferIcon.BackColor = Color.White;
                        break;
                    case TotalBufferStatus.cross:
                        this.pbxBufferIcon.DefaultImage = Properties.Resources.Image_TotalBufferStatus_Cross;
                        this.pbxBufferIcon.BackColor = Color.White;
                        break;
                }
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コントロールを初期化
        /// </summary>
        public void Initialize()
        {
            this.pbxBufferIcon.DefaultImage = Properties.Resources.Image_TotalBufferStatus_None;
            this.pbxBufferIcon.BackColor = Color.DarkGray;
        }

        #endregion

    }
}