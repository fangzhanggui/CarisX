using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.DB;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Common;
using Oelco.CarisX.Const;
using Infragistics.Win.Misc;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 試薬バーコード読取切り替え処理ダイアログクラス
    /// </summary>
    public partial class DlgOptionChangeReadReagentBC : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgOptionChangeReadReagentBC()
        {
            InitializeComponent();

            this.portTitleList.Add(this.gbxPort1);
            this.portTitleList.Add(this.gbxPort2);
            this.portTitleList.Add(this.gbxPort3);
            this.portTitleList.Add(this.gbxPort4);
            this.portTitleList.Add(this.gbxPort5);
            this.portTitleList.Add(this.gbxPort6);
            this.portTitleList.Add(this.gbxPort7);
            this.portTitleList.Add(this.gbxPort8);
            this.portTitleList.Add(this.gbxPort9);
            this.portTitleList.Add(this.gbxPort10);
            this.portTitleList.Add(this.gbxPort11);
            this.portTitleList.Add(this.gbxPort12);
            this.portTitleList.Add(this.gbxPort13);
            this.portTitleList.Add(this.gbxPort14);
            this.portTitleList.Add(this.gbxPort15);
            this.portTitleList.Add(this.gbxPort16);
            this.portTitleList.Add(this.gbxPort17);
            this.portTitleList.Add(this.gbxPort18);
            this.portTitleList.Add(this.gbxPort19);
            this.portTitleList.Add(this.gbxPort20);

            this.portSetList.Add(this.optPort1);
            this.portSetList.Add(this.optPort2);
            this.portSetList.Add(this.optPort3);
            this.portSetList.Add(this.optPort4);
            this.portSetList.Add(this.optPort5);
            this.portSetList.Add(this.optPort6);
            this.portSetList.Add(this.optPort7);
            this.portSetList.Add(this.optPort8);
            this.portSetList.Add(this.optPort9);
            this.portSetList.Add(this.optPort10);
            this.portSetList.Add(this.optPort11);
            this.portSetList.Add(this.optPort12);
            this.portSetList.Add(this.optPort13);
            this.portSetList.Add(this.optPort14);
            this.portSetList.Add(this.optPort15);
            this.portSetList.Add(this.optPort16);
            this.portSetList.Add(this.optPort17);
            this.portSetList.Add(this.optPort18);
            this.portSetList.Add(this.optPort19);
            this.portSetList.Add(this.optPort20);
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ポート毎のタイトルコントロールリスト
        /// </summary>
        private List<UltraGroupBox> portTitleList = new List<UltraGroupBox>();

        /// <summary>
        /// ポート毎のバーコード読取切り替えコントロールリスト
        /// </summary>
        private List<CustomUOptionSet> portSetList = new List<CustomUOptionSet>();

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// リソースの初期化
        /// </summary>
        /// <remarks>
        /// リソースを初期化します
        /// </remarks>
        protected override void initializeResource()
        {
        }

        /// <summary>
        /// コンポーネントの初期化
        /// </summary>
        /// <remarks>
        /// コンポーネントを初期化します
        /// </remarks>
        protected override void initializeFormComponent()
        {
            this.btnOk.Enabled = true;
            this.btnCancel.Enabled = true;

            // TODO:モジュール番号
            Int32 moduleIndex = (Int32)Singleton<PublicMemory>.Instance.moduleIndex;
            int[] readReagentBC = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.ReadReagentBC.GetReadReagentBC(moduleIndex);

            // コントロールに設定値を反映
            for (int portNoIndex = 0; portNoIndex < CarisXConst.REAGENT_PORT_MAX; portNoIndex++)
            {
                this.portSetList[portNoIndex].Items[(int)ReadReagentBCFlag.Read].DataValue = (int)ReadReagentBCFlag.Read;
                this.portSetList[portNoIndex].Items[(int)ReadReagentBCFlag.NotRead].DataValue = (int)ReadReagentBCFlag.NotRead;
                this.portSetList[portNoIndex].Value = (int)readReagentBC[portNoIndex];
            }
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            // パネル既定ボタン
            this.btnOk.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;

            // ダイアログタイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHANGEREADREAGENTBC_000;

            int portNoIndex = 0;
            // ポート毎のタイトル
            foreach (UltraGroupBox portTitle in this.portTitleList)
            {
                portNoIndex++;
                portTitle.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHANGEREADREAGENTBC_001 + portNoIndex.ToString();
            }

            // ポート毎の試薬バーコード読取設定
            foreach (CustomUOptionSet portSet in this.portSetList)
            {
                portSet.Items[(int)ReadReagentBCFlag.Read].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHANGEREADREAGENTBC_002;
                portSet.Items[(int)ReadReagentBCFlag.NotRead].DisplayText = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHANGEREADREAGENTBC_003;
            }
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にOKを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOk_Click( object sender, EventArgs e )
        {
            // ボタンを操作不可にする。
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            int[] setData = new int[CarisXConst.REAGENT_PORT_MAX];

            // コントロールの値を設定に反映
            for (int portNoIndex = 0; portNoIndex < CarisXConst.REAGENT_PORT_MAX; portNoIndex++)
            {
                setData[portNoIndex] = (int)this.portSetList[portNoIndex].CheckedItem.DataValue;
            }

            // TODO:モジュール番号
            Boolean result = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.ReadReagentBC.SetReadReagentBC((int)Singleton<PublicMemory>.Instance.moduleIndex, setData );
            if( result == true )
            {
                // パラメータファイルに保存
                Singleton<ParameterFilePreserve<AppSettings>>.Instance.Save();

                // 試薬保冷庫BC読み込み無効コマンド送信
                SlaveCommCommand_0493 cmd0493 = new SlaveCommCommand_0493();
                cmd0493.ReadReagBC = setData;
                Singleton<CarisXCommManager>.Instance.PushSendQueueSlave( cmd0493 );

                // 完了メッセージ表示
                DlgMessage.Show( Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHANGEREADREAGENTBC_004
                                , String.Empty
                                , Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONCHANGEREADREAGENTBC_000
                                , MessageDialogButtons.Confirm );
            }
        }

        /// <summary>
        /// Cancelボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnCancel_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        #endregion

    }
}
