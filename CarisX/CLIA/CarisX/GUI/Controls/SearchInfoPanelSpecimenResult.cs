using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.GUI.Controls
{
    /// <summary>
    /// 検体測定データ画面絞込みパネル
    /// </summary>
    public partial class SearchInfoPanelSpecimenResult : SearchInfoPanelBase, ISearchInfoSpecimenResult
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SearchInfoPanelSpecimenResult()
        {
            InitializeComponent();

            // 各種チェックボックス
            this.chkPatientId.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_013;
            this.chkJudgement.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_014;
            this.chkSpecimenMaterialType.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_015;
            this.chkComment.Text = Oelco.CarisX.Properties.Resources.STRING_SEARCHINFO_016;

            // 判定
            this.optJudgement.DisplayMember = "Key";
            this.optJudgement.ValueMember = "Value";
            this.optJudgement.DataSource = Enum.GetValues( typeof( JudgementType ) ).OfType<JudgementType>().ToDictionary( ( judgementType ) => judgementType.ToTypeString() ).ToList();
            this.optJudgement.CheckedIndex = 0;

            // 検体物質種別
            this.cmbSpecimenType.DisplayMember = "Key";
            this.cmbSpecimenType.ValueMember = "Value";
            this.cmbSpecimenType.DataSource = Enum.GetValues( typeof( SpecimenMaterialType ) ).OfType<SpecimenMaterialType>().ToDictionary( ( specimenMaterialType ) => specimenMaterialType.ToTypeString() ).ToList();
            this.cmbSpecimenType.SelectedIndex = 0;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 選択中の測定種別の取得、設定
        /// </summary>
        [BrowsableAttribute( false )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
        public Tuple<Boolean, JudgementType> JudgementSelect
        {
            get
            {
                return new Tuple<Boolean, JudgementType>( this.chkJudgement.Checked, (JudgementType)this.optJudgement.Value );
            }
            set
            {
                this.chkJudgement.Checked = value.Item1;
                this.optJudgement.Value = (Int32)value.Item2;
            }
        }

        /// <summary>
        /// 選択中の検体IDの取得、設定
        /// </summary>
        [BrowsableAttribute( false )]
        public Tuple<Boolean, String> PatientIdSelect
        {
            get
            {
                return new Tuple<Boolean, String>( this.chkPatientId.Checked, this.txtPatientId.Text );
            }
            set
            {
                this.chkPatientId.Checked = value.Item1;
                this.txtPatientId.Text = value.Item2;
            }
        }

        /// <summary>
        /// 選択中の検体種別の取得、設定
        /// </summary>
        [BrowsableAttribute( false )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
        public Tuple<Boolean, SpecimenMaterialType> SpecimenMaterialTypeSelect
        {
            get
            {
                SpecimenMaterialType cmbSpecimenTypeValue = SpecimenMaterialType.BloodSerumAndPlasma;
                if ( this.cmbSpecimenType.Value is SpecimenMaterialType )
                {
                    cmbSpecimenTypeValue = (SpecimenMaterialType)this.cmbSpecimenType.Value;
                }
                return new Tuple<Boolean, SpecimenMaterialType>( this.chkSpecimenMaterialType.Checked, cmbSpecimenTypeValue );
            }
            set
            {
                this.chkSpecimenMaterialType.Checked = value.Item1;
                this.cmbSpecimenType.SelectedText = ( (SpecimenMaterialType)value.Item2 ).ToTypeString();
            }
        }

        /// <summary>
        /// 選択中のコメントの取得、設定
        /// </summary>
        [BrowsableAttribute( false )]
        public Tuple<Boolean, String> CommentSelect
        {
            get
            {
                return new Tuple<Boolean, String>( this.chkComment.Checked, this.txtComment.Text );
            }
            set
            {
                this.chkComment.Checked = value.Item1;
                this.txtComment.Text = value.Item2;
            }
        }

        #endregion

    }
}
