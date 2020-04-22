using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.GUI.Controls
{
    public partial class SearchLogInfoPanelErrorLog : SearchLogInfoPanelBase, ISearchLogInfoErrorLog
    {
        public SearchLogInfoPanelErrorLog()
        {
            InitializeComponent();

            // 各種チェックボックス
            this.chkErrorCode.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_025;
            this.chkErrorArg.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_026;
            this.chkErrorLevel.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_027;
            this.chkErrorTitle.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_028;

            // ラック搬送
            this.chkRackTransfer.Text = Oelco.CarisX.Properties.Resources.STRING_DLG_OPTIONVERSIONUP_003;

            // モジュール
            this.chkModule1.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_019;
            this.chkModule2.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_020;
            this.chkModule3.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_021;
            this.chkModule4.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_022;

            this.chkDPR.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_029;

            // 各ハイフン
            this.lblHyphen2.Text = Oelco.CarisX.Properties.Resources.STRING_COMMON_000;

            // 初期値
            this.optErrorLevel.CheckedIndex = 0;
        }

        #region [プロパティ]

        /// <summary>
        /// エラーコードの取得、設定
        /// </summary>
        [BrowsableAttribute(false)]
        Tuple<bool, string> ISearchLogInfoErrorLog.ErrorCodeSelect
        {
            get
            {
                // エラーコードボタンのチェック状態と入力されたエラーコードを返す
                return new Tuple<Boolean, String>(this.chkErrorCode.Checked, this.numErrorCode.Text);
            }
            set
            {
                // 各種値の設定
                this.chkErrorCode.Checked = value.Item1;
                this.numErrorCode.Text = value.Item2;
            }
        }

        /// <summary>
        /// エラー引数の取得、設定
        /// </summary>
        [BrowsableAttribute(false)]
        Tuple<bool, string> ISearchLogInfoErrorLog.ErrorArgSelect
        {
            get
            {
                // エラー引数ボタンのチェック状態と入力されたエラー引数を返す
                return new Tuple<Boolean, String>(this.chkErrorArg.Checked, this.numErrorArg.Text);
            }
            set
            {
                // 各種値の設定
                this.chkErrorArg.Checked = value.Item1;
                this.numErrorArg.Text = value.Item2;
            }
        }

        /// <summary>
        /// エラーレベルの取得、設定
        /// </summary>
        [BrowsableAttribute(false)]
        Tuple<bool, string> ISearchLogInfoErrorLog.ErrorLevelSelect
        {
            get
            {
                String errorLevel = "";
                switch (this.optErrorLevel.Value)
                {
                    case 0:
                        errorLevel = Oelco.CarisX.Properties.Resources.STRING_ERRORLEVEL_1;
                        break;
                    case 1:
                        errorLevel = Oelco.CarisX.Properties.Resources.STRING_ERRORLEVEL_2;
                        break;
                    case 2:
                        errorLevel = Oelco.CarisX.Properties.Resources.STRING_ERRORLEVEL_3;
                        break;
                }

                // エラーレベルボタンのチェック状態と選択されてエラーレベルを返す
                return new Tuple<Boolean, String>(this.chkErrorLevel.Checked, errorLevel);
            }
            set
            {
                // 各種値の設定
                this.chkErrorLevel.Checked = value.Item1;
                this.optErrorLevel.Value = value.Item2;
            }
        }

        /// <summary>
        /// エラー履歴のコメントの取得、設定
        /// </summary>
        [BrowsableAttribute(false)]
        Tuple<bool, string> ISearchLogInfoErrorLog.ErrorContentSelect
        {
            get
            {
                // エラー履歴コメントボタンのチェック状態と入力された文字を返す
                return new Tuple<Boolean, String>(this.chkErrorTitle.Checked, this.txtErrorComment.Text);
            }
            set
            {
                // 各種値の設定
                this.chkErrorTitle.Checked = value.Item1;
                this.txtErrorComment.Text = value.Item2;
            }
        }

        /// <summary>
        /// 選択中のモジュールの取得、設定
        /// </summary>
        [BrowsableAttribute(false)]
        ErrorFilteringCategory ISearchLogInfoErrorLog.ModuleSelect
        {
            get
            {
                ErrorFilteringCategory category = 0;

                // モジュール１
                category |= ( this.chkModule1.Checked ) ? ErrorFilteringCategory.Module1 : 0;

                // モジュール２
                category |= ( this.chkModule2.Checked ) ? ErrorFilteringCategory.Module2 : 0;

                // モジュール３
                category |= ( this.chkModule3.Checked ) ? ErrorFilteringCategory.Module3 : 0;

                // モジュール４
                category |= ( this.chkModule4.Checked ) ? ErrorFilteringCategory.Module4 : 0;

                // ラック搬送
                category |= ( this.chkRackTransfer.Checked ) ? ErrorFilteringCategory.RackTransfer : 0;

                // DPR
                category |= ( this.chkDPR.Checked ) ? ErrorFilteringCategory.DPR : 0;

                return category;
            }
            set
            {
                // モジュール１
                this.chkModule1.Checked = ( value & ErrorFilteringCategory.Module1 ) != 0;

                // モジュール２
                this.chkModule2.Checked = ( value & ErrorFilteringCategory.Module2 ) != 0;

                // モジュール３
                this.chkModule3.Checked = ( value & ErrorFilteringCategory.Module3 ) != 0;

                // モジュール４
                this.chkModule4.Checked = ( value & ErrorFilteringCategory.Module4 ) != 0;

                // ラック搬送
                this.chkRackTransfer.Checked = ( value & ErrorFilteringCategory.RackTransfer ) != 0;

                // DPR
                this.chkRackTransfer.Checked = ( value & ErrorFilteringCategory.DPR ) != 0;
            }
        }

        /// <summary>
        ///  エラーの発生回数の取得、設定
        /// </summary>
        [BrowsableAttribute(false)]
        Tuple<Boolean, Int32, Int32> ISearchLogInfoErrorLog.SumSelect
        {
            get
            {
                Int32 sumFrom = Int32.Parse(this.numSumFrom.Text);
                Int32 sumTo = Int32.Parse(this.numSumTo.Text);
                return new Tuple<bool, Int32, Int32>(this.chkSum.Checked, sumFrom, sumTo);
            }
            set
            {
                this.chkSum.Checked = value.Item1;
                this.numSumFrom.Text = value.Item2.ToString();
                this.numSumTo.Text = value.Item3.ToString();
            }
        }

        #endregion
    }
}
