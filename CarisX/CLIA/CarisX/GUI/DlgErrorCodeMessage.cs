using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Oelco.Common.Utility;
using System.IO;
using System.Text.RegularExpressions;
using Oelco.Common.Parameter;
using Oelco.CarisX.Const;
using Oelco.CarisX.Parameter.ErrorCodeData;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.Utility;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// エラー検知ダイアログ
    /// </summary>
    public partial class DlgErrorCodeMessage : DlgCarisXBase
    {
        #region [変数]
        private Int32 ModuleIndex;
        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgErrorCodeMessage()
        {
            InitializeComponent();
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// パラメータ表示処理
        /// </summary>
        /// <remarks>
        /// 呼び出し元で指定されたコード、引数からエラー内容、エラー説明を取得し表示する処理。
        /// 追加メッセージに文字が入力されている時は、エラー説明に追加する。
        /// </remarks>
        /// <param name="code">コード</param>
        /// <param name="argments">引数</param>
        /// <param name="message">追加のメッセージ</param>
        /// <param name="moduleName">分析モジュール名</param>
        public void ShowErrorMessage( String code, String arguments, String message, String moduleName, Int32 moduleId)
        {
            // パラメータ表示処理
            Singleton<ParameterFilePreserve<ErrorCodeDataManager>>.Instance.Load();
            ErrorCodeData errorData = Singleton<ParameterFilePreserve<ErrorCodeDataManager>>.Instance.Param.GetCodeData( code, arguments );
            if ( errorData == null )
            {
                // 定義されていないエラーコードが指定された。
                return;
            }

            ModuleIndex = CarisXSubFunction.ModuleIDToModuleIndex(moduleId);

            // 追加メッセージが入力されていた場合の追加処理
            if ( message != String.Empty )
            {
                errorData.Message += message;
            }

            // タイトル・メッセージ・画像パス
            //this.lblErrorCodeNo.Text = String.Format( "{0}-{1}", code, arguments );
            this.lblErrorCodeNo.Text = code + Oelco.CarisX.Properties.Resources.STRING_COMMON_000 + arguments;
            this.lblErrorContents.Text = errorData.Title;
            this.lblErrorExplanation.Text = errorData.Message;
            this.lblModuleNo.Text = moduleName;
            String imagePath = errorData.GetFullImagePath();

            // 画像ファイルのチェック
            try
            {
                this.pbxErrorPicture.Image = Image.FromFile( imagePath );
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine( ex.Message );
                Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
            }
            this.ShowDialog();
        }



        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            // タイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_MSG_150;

            // ラベル
            this.lblErrorCode.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_ERRORCODEMESSAGE_001;
            this.lblErrorExplanationTitle.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_ERRORCODEMESSAGE_002;
            this.lblErrorPictureTitle.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_ERRORCODEMESSAGE_004;

            // ボタン
            this.btnMute.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_ERRORCODEMESSAGE_003;
            this.btnClose.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_002;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// 消音処理
        /// </summary>
        /// <remarks>
        /// エラーが発生した時のエラー音を消す処理。
        /// </remarks>
        /// <param name="sender">消音ボタン</param>
        /// <param name="e">押下イベント</param>
        private void btnMute_Click( object sender, EventArgs e )
        {
            // スレーブにブザー消去送信
            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.SendBuzzerCancel, ModuleIndex);
        }

        /// <summary>
        /// 閉じる処理
        /// </summary>
        /// <remarks>
        /// エラーコード画面を閉じる処理
        /// </remarks>
        /// <param name="sender">閉じるボタン</param>
        /// <param name="e">押下イベント</param>
        private void btnClose_Click( object sender, EventArgs e )
        {
            // スレーブにブザー消去送信
            Singleton<NotifyManager>.Instance.PushSignalQueue( (Int32)NotifyKind.SendBuzzerCancel, ModuleIndex);

            this.Close();
        }

        #endregion

    }
}
