using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// 範囲選択ダイアログクラス
    /// </summary>
    public partial class DlgTargetSelectRange : DlgCarisXBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DlgTargetSelectRange()
        {
            InitializeComponent();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 出力範囲の取得、設定
        /// </summary>
        protected TargetRange TargetRange
        {
            get;
            set;
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 範囲選択ダイアログの表示
        /// </summary>
        /// <remarks>
        /// 範囲選択ダイアログの表示を行います
        /// </remarks>
        /// <returns></returns>
        public static TargetRange Show( String caption, String all = null, String specification = null, TargetRange range = TargetRange.All )
        {
            using ( DlgTargetSelectRange dlgOutputSelectRange = new DlgTargetSelectRange() )
            {
                dlgOutputSelectRange.customUOptionSet.Value = (Int32)range;
                dlgOutputSelectRange.Caption = caption ?? Oelco.CarisX.Properties.Resources.STRING_DLG_TARGETSELECTRENGE_000;
                dlgOutputSelectRange.customUOptionSet.Items[0].DisplayText = all ?? Oelco.CarisX.Properties.Resources.STRING_DLG_TARGETSELECTRENGE_001;
                dlgOutputSelectRange.customUOptionSet.Items[1].DisplayText = specification ?? Oelco.CarisX.Properties.Resources.STRING_DLG_TARGETSELECTRENGE_002;
                dlgOutputSelectRange.customUOptionSet.Items[0].CheckState = CheckState.Checked;
                dlgOutputSelectRange.ShowDialog();

                return dlgOutputSelectRange.TargetRange;
            }
        }

        /// <summary>
        /// 範囲選択ダイアログの表示
        /// </summary>
        /// <remarks>
        /// 範囲選択ダイアログの新規表示を行います
        /// </remarks>
        /// <returns></returns>
        public new static TargetRange Show()
        {
            return DlgTargetSelectRange.Show( null, null, null );
        }

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
        }

        /// <summary>
        /// カルチャによるリソースの設定
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに従ってコンポーネントにリソースの設定を行います
        /// </remarks>
        protected override void setCulture()
        {
            this.btnExecute.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_007;
            this.btnCancel.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_003;
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// 実行ボタン押下イベント
        /// </summary>
        /// <remarks>
        /// 範囲指定を取得して、画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnExecute_Click( object sender, EventArgs e )
        {
            if (this.customUOptionSet.Value != null)
            {
                if (( (Int32)GUI.TargetRange.All ) == ( (Int32)this.customUOptionSet.Value ))
                {
                    this.TargetRange = GUI.TargetRange.All;
                }
                else if (( (Int32)GUI.TargetRange.Specification ) == ( (Int32)this.customUOptionSet.Value ))
                {
                    this.TargetRange = GUI.TargetRange.Specification;
                }
            }

            this.Close();
        }

        /// <summary>
        /// キャンセルボタン押下イベント
        /// </summary>
        /// <remarks>
        /// 画面を終了します
        /// </remarks>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void btnCancel_Click( object sender, EventArgs e )
        {
            this.Close();
        }

        #endregion

    }

    /// <summary>
    /// 範囲選択
    /// </summary>
    public enum TargetRange : int
    {
        /// <summary>
        /// 指定なし
        /// </summary>
        None = 0,

        /// <summary>
        /// 全て
        /// </summary>
        All = 1,
        
        /// <summary>
        /// 範囲指定
        /// </summary>
        Specification = 2
    }
}
