using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Oelco.CarisX.Utility;
using Oelco.Common.Utility;
using Oelco.CarisX.Parameter;
using Oelco.Common.Const;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 分析項目パラメータ読み取りダイアログクラス
    /// </summary>
    public partial class DlgImportMeasProto : Oelco.CarisX.GUI.DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgImportMeasProto()
        {
            InitializeComponent();            
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
            this.lblDialogTitle.Text = CarisX.Properties.Resources.STRING_DLG_IMPORTMEASPROTO_000;
            this.lblparamSheet.Text = CarisX.Properties.Resources.STRING_DLG_IMPORTMEASPROTO_001;
            this.btnImport.Text = CarisX.Properties.Resources.STRING_DLG_IMPORTMEASPROTO_002;            
            this.btnOpen.Text = CarisX.Properties.Resources.STRING_DLG_IMPORTMEASPROTO_003;
            this.btnCancel.Text = CarisX.Properties.Resources.STRING_COMMON_003;
        }
        
        #endregion

        #region [privateメソッド]        
        /// <summary>
        /// 読込ボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// 分析項目パラメータシートの読込を実行します
        /// 読込終了でダイアログ結果にOKを設定して画面を終了します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click( object sender, EventArgs e )
        {
            // 分析項目パラメータシート名が入力されていない場合はないもしない
            if ( this.txtFolder.Text == "" )
            {
                return;
            }

            // 現在のマウスポインタを保持
            Cursor curCursor = this.Cursor;
            // マウスポインタを待機用に
            this.Cursor = Cursors.WaitCursor;

            System.Diagnostics.Process hProcess = new System.Diagnostics.Process();
            hProcess.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName( CarisXConst.ProtoConvExeName );
            hProcess.StartInfo.FileName = CarisXConst.ProtoConvExeName;
			hProcess.StartInfo.Arguments = string.Format(" \"/xls:{0}\" \"/out:{1}\" \"/log:{2}\"", this.txtFolder.Text, CarisXConst.ProtoConvExportDir, CarisXConst.PROT_CONV_LOG);
            hProcess.Start();

            // 終了まで待機
            hProcess.WaitForExit();

            // 不要になった時点で破棄する
            hProcess.Close();
            hProcess.Dispose();

            // マウスポインタを元に戻す
            this.Cursor = curCursor;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// キャンセルボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 開くボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ファイルを開くのダイアログを表示します
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click( object sender, EventArgs e )
        {
            String fileName = String.Empty;
            if ( CarisXSubFunction.ShowOpenFileDialog( out fileName, OutputFileKind.XLS, String.Empty, CarisX.Properties.Resources.STRING_DLG_IMPORTMEASPROTO_001, 
                Singleton<CarisXUISettingManager>.Instance.DlgImportMeasProtoSettings ) == System.Windows.Forms.DialogResult.OK )
            {
                this.txtFolder.Text = fileName;
            }
        }
        #endregion

        //通过文件名获得项目的版本号
        public String getProtocolVersion()
        {
            String protocolVersion = String.Empty;
            if (this.txtFolder.Text != String.Empty)
            {
                String fileName = System.IO.Path.GetFileNameWithoutExtension(this.txtFolder.Text);
                String[] splitFileName = fileName.Split('_');
                if (splitFileName.Length >= 2)
                {
                    protocolVersion = splitFileName[1];
                }     
            }
            return protocolVersion;
        }
    }
}
