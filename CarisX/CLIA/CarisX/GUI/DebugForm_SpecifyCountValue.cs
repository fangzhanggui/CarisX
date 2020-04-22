using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.CarisX.Utility;
using Oelco.Common.Utility;

namespace Oelco.CarisX.GUI
{
    /// <summary>
    /// デバッグ用カウント値操作フォーム
    /// </summary>
    public partial class DebugForm_SpecifyCountValue : Form
    {
        /// <summary>
        /// カウント値データリストの取得
        /// </summary>
        public static IList<SpecifyCountValueData> Items
        {
            get
            {
                return Singleton<List<SpecifyCountValueData>>.Instance;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DebugForm_SpecifyCountValue()
        {
            InitializeComponent();
            this.dataGridView1.DataSource = new BindingList<SpecifyCountValueData>( DebugForm_SpecifyCountValue.Items );
            this.dataGridView1.DataError += ( obj, e ) =>
            {
                e.Cancel = true;
            };

            //2103/7/17 検量線テスト oelco_test
           //Oelco.CarisX.Calculator.CarisXCalculator.Curvetest();

        }

        #region [内部クラス]

        /// <summary>
        /// カウント値データ
        /// </summary>
        public class SpecifyCountValueData
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public SpecifyCountValueData()
            {
                RackID = "0001";
                RackPosition = 1;
                ReplicationNo = 1;
                CountValue = 1;
            }

            /// <summary>
            /// ラックID
            /// </summary>
            public String RackID
            {
                get;
                set;
            }

            /// <summary>
            /// ラックポジション
            /// </summary>
            public Int32 RackPosition
            {
                get;
                set;
            }

            /// <summary>
            /// 多重測定回数番号
            /// </summary>
            public Int32 ReplicationNo
            {
                get;
                set;
            }

            /// <summary>
            /// カウント値
            /// </summary>
            public Int32 CountValue
            {
                get;
                set;
            }
        }

        #endregion
    }
}
