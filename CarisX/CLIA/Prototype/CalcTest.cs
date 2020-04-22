using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.CarisX.Calculator;
using Oelco.CarisX.GUI.Controls;

namespace Prototype
{
    public partial class CalcTest : Form
    {
        public CalcTest()
        {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            RecalcInfoPanelSpecimenResult recalcInfo = new RecalcInfoPanelSpecimenResult();
            ( (IRecalcInfo)recalcInfo ).AnalyteSelect = new List<Int32>();
            ( (IRecalcInfo)recalcInfo ).AnalyteSelect.Add( 1 );
            ( (IRecalcRefiner)recalcInfo ).MeasuringTimeSelect = new Tuple<bool, DateTime, DateTime>( true, DateTime.Today, DateTime.Today );

            List<CalcData> result;
            CarisXCalculator.ReCalcSpecimen( recalcInfo,
                new List<CalcData>() { new CalcData( 1, "", 1, 1, 1, 1, 1, DateTime.Today ) },
                out result );
        }
    }
}
