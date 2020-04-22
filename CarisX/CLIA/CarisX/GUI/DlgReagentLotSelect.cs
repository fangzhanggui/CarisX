using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.DB;
using Oelco.Common.Utility;
using Infragistics.Win.UltraWinListView;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 試薬ロット選択ダイアログ
    /// </summary>
    public partial class DlgReagentLotSelect : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgReagentLotSelect()
        {
            InitializeComponent();
        }

        #endregion

        #region [プロパティ]
        /// <summary>
        /// 試薬ロット番号の取得
        /// </summary>
        public String ReagentLotNo
        {
            get
            {
                if ( this.lvwReagentLotSelect.SelectedItems.Count > 0 )
                {
                    return this.lvwReagentLotSelect.SelectedItems[0].Key;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 分析項目の設定
        /// </summary>
        /// <remarks>
        /// 分析項目の設定を行います
        /// </remarks>
        /// <param name="measureProtocolIndex">分析項目インデックス</param>
        /// <returns></returns>
        public void SetMeasureProtocol( Int32 measureProtocolIndex )
        {
            // 試薬ロット番号の取得
            String[] lotNo = Singleton<ReagentDB>.Instance.GetReagentLotNo(Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(measureProtocolIndex).ReagentCode);
            
            foreach(String no in lotNo)
            {
                this.lvwReagentLotSelect.Items.Add( no, no );
            }
            if ( this.lvwReagentLotSelect.Items.Count > 0 )
            {
                this.lvwReagentLotSelect.Items[0].Activate();
            }
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
            this.Caption = Oelco.CarisX.Properties.Resources.STRING_DLG_REAGENTLOTSELECT_000;

            // ボタン
            this.btnOK.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_001;
            this.btnClose.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_002;
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
        private void btnOK_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 閉じるボタンクリックイベント
        /// </summary>
        /// <remarks>
        /// ダイアログ結果にキャンセルを設定して画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnClose_Click( object sender, EventArgs e )
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 試薬ロット番号リストItemActivating処理
        /// </summary>
        /// <remarks>
        /// 試薬ロット番号リスト選択変更前アイテムの色設定クリア処理を行います
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvwReagentLotSelect_ItemActivating( object sender, ItemActivatingEventArgs e )
        {
            // 選択変更前アイテムの色設定クリア処理
            if ( this.lvwReagentLotSelect.SelectedItems.Count > 0 )
            {
                this.lvwReagentLotSelect.SelectedItems[0].Appearance.BackColor = this.lvwReagentLotSelect.Appearance.BackColor;
                this.lvwReagentLotSelect.SelectedItems[0].Appearance.ForeColor = this.lvwReagentLotSelect.Appearance.ForeColor; 
            }            
        }

        /// <summary>
        /// 試薬ロット番号リストItemActivated処理
        /// </summary>
        /// <remarks>
        /// 試薬ロット番号リスト選択アイテムの色設定を行います
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvwReagentLotSelect_ItemActivated( object sender, ItemActivatedEventArgs e )
        {
            // SelectedItems再設定
            this.lvwReagentLotSelect.SelectedItems.Clear();
            this.lvwReagentLotSelect.SelectedItems.Add( this.lvwReagentLotSelect.ActiveItem );
            // 選択アイテムの色設定
            this.lvwReagentLotSelect.SelectedItems[0].Appearance.BackColor = this.lvwReagentLotSelect.ItemSettings.SelectedAppearance.BackColor;
            this.lvwReagentLotSelect.SelectedItems[0].Appearance.ForeColor = this.lvwReagentLotSelect.ItemSettings.SelectedAppearance.ForeColor;
        }
        #endregion

        
        


    }
}
