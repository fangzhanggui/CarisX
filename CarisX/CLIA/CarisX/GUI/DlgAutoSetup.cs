using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Parameter;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 自動立ち上げ待機画面クラス
    /// </summary>
    public partial class DlgAutoSetup : Oelco.CarisX.GUI.DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgAutoSetup()
        {
            InitializeComponent();
        }
        #endregion

        #region [protectedメソッド]
        /// <summary>
        /// コンポーネントの初期化
        /// </summary>
        /// <remarks>
        /// コンポーネントを初期化します
        /// </remarks>
        protected override void initializeFormComponent()
        {
            // TODO:立ち上げ日時表示
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            base.setCulture();

            // 自動立ち上げ時間を取得する。
            string startTime = 
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.AutoStartupHour.ToString( "00" ) + ":" +
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AutoStartupTimerParameter.AutoStartupMinute.ToString( "00" );

            //SlaveCommCommand_0404 cmd0404 = new SlaveCommCommand_0404();
            //string startTime = cmd0404.HourForAutoStartUp.ToString( "00" ) + ":" + cmd0004.MinForAutoStartUp.ToString( "00" );

            // タイトルを設定する。
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLGAUTOSETUP_000;

            // ラベル文言を設定する。
            lblMessage.Text = Oelco.CarisX.Properties.Resources.STRING_DLGAUTOSETUP_001;            
            lblStartUpTime.Text = Oelco.CarisX.Properties.Resources.STRING_DLGAUTOSETUP_002 + startTime;
            btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
        }

        #endregion

        #region [privateメソッド]
        /// <summary>
        /// キャンセルボタン押下イベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して、画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnCancel_Click( object sender, EventArgs e )
        {
            // DialogResultにCancelを設定
            this.DialogResult = DialogResult.Cancel;

            // 自画面を閉じる
            this.Close();
        }
        #endregion
        
    }
}
