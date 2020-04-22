using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.GUI.Controls
{
    /// <summary>
    /// 検体測定データ画面用再計算パネル
    /// </summary>
    public partial class RecalcInfoPanelSpecimenResult : RecalcInfoPanelBase, IRecalcInfoSpecimenResult
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RecalcInfoPanelSpecimenResult()
        {
            InitializeComponent();

            this.chkPatientId.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELSPECIMENRESULT_000;
            this.chkSpecimenMaterialType.Text = Oelco.CarisX.Properties.Resources.STRING_RECALCINFOPANELSPECIMENRESULT_001;
            
            // 検体物質種別
            this.cmbSpecimenType.DisplayMember = "Key";
            this.cmbSpecimenType.ValueMember = "Value";
            this.cmbSpecimenType.DataSource = Enum.GetValues( typeof( SpecimenMaterialType ) ).OfType<SpecimenMaterialType>().ToDictionary( ( judgementType ) => judgementType.ToTypeString() ).ToList();
            this.cmbSpecimenType.SelectedIndex = 0;
        }

        #endregion

        #region [プロパティ]

        // TODO:実装
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
        /// 選択中のサンプル種別の取得、設定
        /// </summary>
        [BrowsableAttribute( false )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
        public Tuple<Boolean, SpecimenMaterialType> SpecimenMaterialTypeSelect
        {
            get
            {
                SpecimenMaterialType specimenMaterialType = SpecimenMaterialType.BloodSerumAndPlasma;
                if ( this.cmbSpecimenType.Value is JudgementType )
                {
                    specimenMaterialType = (SpecimenMaterialType)this.cmbSpecimenType.Value;
                }
                return new Tuple<Boolean, SpecimenMaterialType>( this.chkSpecimenMaterialType.Checked, specimenMaterialType );
            }
            set
            {
                this.chkSpecimenMaterialType.Checked = value.Item1;
            }
        }
        #endregion

    }
}
