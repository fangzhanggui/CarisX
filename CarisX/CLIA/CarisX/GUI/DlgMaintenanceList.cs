using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using Oelco.Common.GUI;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using Oelco.Common.Utility;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Const;
using Oelco.Common.Parameter;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// メンテナンス日誌リストダイアログクラス
    /// </summary>
    public partial class DlgMaintenanceList : DlgCarisXBase
    {
        #region [定数定義]

        /// <summary>
        /// 行番号列キー
        /// </summary>
        private const String STRING_CHECKNO = "No";
        /// <summary>
        /// メンテナンス日誌項目名列キー
        /// </summary>
        private const String STRING_CHECKITEM = "CheckItem";
        /// <summary>
        /// メンテナンス日誌種別列キー
        /// </summary>
        private const String STRING_CHECKKIND = "Kind";
        /// <summary>
        /// Module1列キー
        /// </summary>
        private const String STRING_CHECKMODULE1 = "Module1";
        /// <summary>
        /// Module2列キー
        /// </summary>
        private const String STRING_CHECKMODULE2 = "Module2";
        /// <summary>
        /// Module3列キー
        /// </summary>
        private const String STRING_CHECKMODULE3 = "Module3";
        /// <summary>
        /// Module4列キー
        /// </summary>
        private const String STRING_CHECKMODULE4 = "Module4";
        /// <summary>
        /// メンテナンス日誌番号列キー
        /// </summary>
        private const String STRING_MAINTENANCEJOURNALNO = "MaintenanceJournalNo";
        /// <summary>
        /// メンテナンス日誌ユニット番号キター
        /// </summary>
        private const String STRING_UNITNO = "UnitNo";
        /// <summary>
        ///  メンテナンス日誌種別
        /// </summary>
        private MaintenanceJournalType mainteJournalType = MaintenanceJournalType.User;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">メンテナンス日誌種別</param>
        public DlgMaintenanceList(MaintenanceJournalType type)
        {
            // 種別の設定
            mainteJournalType = type;

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
            // グリッド表示順
            this.grdMaintenanceJournalList.SetGridColumnOrder(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.MaintenanceListSettings.GridColOrder);
            // グリッド列幅
            this.grdMaintenanceJournalList.SetGridColmnWidth(Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.MaintenanceListSettings.GridColWidth);
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            // ダイアログタイトル
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_000;
            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;

            // グリッドヘッダの設定
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKNO].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_001;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKKIND].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_008;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKITEM].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_002;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE1].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_003;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE2].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_004;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE3].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_005;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE4].Header.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_006;


            // 編集をできないようにする設定
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKNO].CellActivation = Activation.NoEdit;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKKIND].CellActivation = Activation.NoEdit;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKITEM].CellActivation = Activation.NoEdit;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE1].CellActivation = Activation.NoEdit;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE2].CellActivation = Activation.NoEdit;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE3].CellActivation = Activation.NoEdit;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE4].CellActivation = Activation.NoEdit;

            // 項目名を左寄せに設定
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKITEM].CellAppearance.TextHAlign = HAlign.Left;

            // No.列の表示スタイルをボタンスタイルに設定
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE1].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE1].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE2].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE2].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE3].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE3].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE4].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE4].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;

            // 非表示の設定
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_MAINTENANCEJOURNALNO].Hidden = true;
            this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_UNITNO].Hidden = true;
            Int32 moduleNumConnected = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;
            if (moduleNumConnected <= 3)
            {
                this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE4].Hidden = true;
            }
            if (moduleNumConnected <= 2)
            {
                this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE3].Hidden = true;
            }
            if (moduleNumConnected == 1)
            {
                this.grdMaintenanceJournalList.DisplayLayout.Bands[0].Columns[STRING_CHECKMODULE2].Hidden = true;
            }

        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// フォーム読み込みイベント
        /// </summary>
        /// <remarks>
        /// 画面の表示状態を初期化します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void DlgMaintenanceList_Load(object sender, EventArgs e)
        {
            // グリッドの値を取得します
            List<MaintenanceJournalListData> maintenanceJournalInfoManager = Singleton<MaintenanceJournalInfoManager>.Instance.GetMaintenanceJournalListDatas();
            
            this.grdMaintenanceJournalList.DataSource = maintenanceJournalInfoManager;
            Singleton<MaintenanceJournalInfoManager>.Instance.CreateIndex(this.mainteJournalType); 
        }

        /// <summary>
        /// OKボタンクリックイベント
        /// </summary>
        /// ユーザー用：xmlを保存し、csvを出力し、画面を閉じます。
        /// サービスマン用：チェック状態を保持して画面を閉じます。
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            MaintenanceJournalInfoManager maintenanceJournalInfoManager = Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance;
            // OK処理を実行します
            maintenanceJournalInfoManager.OkExecute(this.grdMaintenanceJournalList);

            // この画面を閉じます
            this.Close();
        }

        /// <summary>
        /// Exit処理を実行
        /// </summary>
        /// <remarks>
        /// メンテナンス画面にてExitを押下した時の処理を行います。
        /// </remarks>
        public void ServicemanExit()
        {
            MaintenanceJournalInfoManager maintenanceJournalInfoManager = Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance;
            // EXIT処理を実行します。
            maintenanceJournalInfoManager.ServicemanExitExecute(this.grdMaintenanceJournalList);
        }

        /// <summary>
        /// 行ダブルクリック
        /// </summary>
        /// <remarks>
        /// 詳細画面を表示します。
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdMaintenanceList_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            try
            {
                using (DlgMaintenanceDetail dlg = new DlgMaintenanceDetail())
                {
                    // インデックスを取得
                    int doubleClickIndex = e.Row.Index;
                    // メンテナンス日誌番号を取得
                    string doubleClickMaintenanceJournalNo = grdMaintenanceJournalList.Rows[doubleClickIndex].Cells[STRING_MAINTENANCEJOURNALNO].Value.ToString();
                    // 種別を取得
                    string doubleClickMaintenanceJournalKind = grdMaintenanceJournalList.Rows[doubleClickIndex].Cells[STRING_CHECKKIND].Value.ToString();
                    // ユニットを取得
                    string doubtClickMaintenanceJournalUnit = grdMaintenanceJournalList.Rows[doubleClickIndex].Cells[STRING_UNITNO].Value.ToString();

                    // メンテナンス日誌詳細画面に遷移
                    dlg.ShowMaintenanceDetail(doubleClickMaintenanceJournalNo, doubleClickMaintenanceJournalKind, doubtClickMaintenanceJournalUnit);
                }
            }
            catch (Exception ex)
            {
                // 詳細画面の表示に失敗しました。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("Detail screen display failed :{0}", ex.StackTrace));
                // 画面を閉じます。
                this.Close();
            }
        }

        /// <summary>
        /// フォームClosingイベント
        /// </summary>
        /// <remarks>
        /// グリッド表示順UI設定、グリッド列幅UI設定を保存します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void DlgMaintenanceList_FormClosing(object sender, FormClosingEventArgs e)
        {
            // グリッド表示順UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.MaintenanceListSettings.GridColOrder = this.grdMaintenanceJournalList.GetGridColumnOrder();
            // グリッド列幅UI設定保存
            Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.MaintenanceListSettings.GridColWidth = this.grdMaintenanceJournalList.GetGridColmnWidth();
        }

        /// <summary>
        /// フォームClickCellButtonイベント
        /// </summary>
        /// <remarks>
        /// グリッド表示のmodule列をクリックした時、チェックボタンの表示文字列を変更します。
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void grdMaintenanceList_ClickCellButton(object sender, CellEventArgs e)
        {
            // onとoffを切り替えます
            if (grdMaintenanceJournalList.Rows[e.Cell.Row.Index].Cells[e.Cell.Column.Index].Text == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_014)
            {
                // on（"○"が表示されます。)
                grdMaintenanceJournalList.Rows[e.Cell.Row.Index].Cells[e.Cell.Column.Index].Value = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013;
            }
            else
            {
                // off（""が表示されます。)
                grdMaintenanceJournalList.Rows[e.Cell.Row.Index].Cells[e.Cell.Column.Index].Value = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_014;
            }
        }
    }

    #endregion
}
