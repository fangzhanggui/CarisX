using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prototype
{
    public partial class CustomChartTest : Form
    {
        public CustomChartTest()
        {
            InitializeComponent();

            this.customChart1.DataSource = getColumnData();
            this.customChart1.DataBind();

            this.customChart1.LineChart.Thickness = 10;
            this.customChart1.ColorModel.AlphaLevel = (byte)255;

            Oelco.Common.GUI.YAxisZone yAxisZone = new Oelco.Common.GUI.YAxisZone();
            yAxisZone.Value1 = 10;
            yAxisZone.Value2 = 300;

            Oelco.Common.GUI.YAxisLine yAxisLine = new Oelco.Common.GUI.YAxisLine();
            
            yAxisLine.Value = 50;
            
            this.customChart1.YAxisZoneItems.Add( yAxisZone );
            this.customChart1.YAxisLineItems.Add( yAxisLine );
        }

        /// <summary>
        /// データ取得時
        /// </summary>
        /// <returns></returns>
        private DataTable getColumnData()
        {
            DataTable mydata = new DataTable();
            // 列と列名を定義します。
            mydata.Columns.Add( "Series Labels", typeof( String ) );
            mydata.Columns.Add( "Column A", typeof( Int32 ) );
            mydata.Columns.Add( "Column B", typeof( Int32 ) );
            mydata.Columns.Add( "Column C", typeof( Int32 ) );
            mydata.Columns.Add( "Column D", typeof( Int32 ) );
            mydata.Columns.Add( "Column E", typeof( Int32 ) );
            mydata.Columns.Add( "Column F", typeof( Int32 ) );
            mydata.Columns.Add( "Column G", typeof( Int32 ) );
            mydata.Columns.Add( "Column H", typeof( Int32 ) );
            mydata.Columns.Add( "Column I", typeof( Int32 ) );
            mydata.Columns.Add( "Column J", typeof( Int32 ) );
            mydata.Columns.Add( "Column K", typeof( Int32 ) );
            mydata.Columns.Add( "Column L", typeof( Int32 ) );
            mydata.Columns.Add( "Column M", typeof( Int32 ) );
            mydata.Columns.Add( "Column N", typeof( Int32 ) );
            mydata.Columns.Add( "Column O", typeof( Int32 ) );
            mydata.Columns.Add( "Column P", typeof( Int32 ) );

            // データの行を追加します。
            mydata.Rows.Add();
            mydata.Rows[0]["Series Labels"] = (object)"Series A";
            mydata.Rows[0]["Column A"] = 15;
            mydata.Rows[0]["Column B"] = 15;
            mydata.Rows[0]["Column C"] = 15;
            mydata.Rows[0]["Column D"] = 15;
            mydata.Rows[0]["Column E"] = 15;
            mydata.Rows[0]["Column F"] = 15;
            mydata.Rows[0]["Column G"] = 15;
            mydata.Rows[0]["Column H"] = 15;
            mydata.Rows[0]["Column I"] = 15;
            mydata.Rows[0]["Column J"] = 15;
            mydata.Rows[0]["Column K"] = 15;
            mydata.Rows[0]["Column L"] = 15;
            mydata.Rows[0]["Column M"] = 15;
            mydata.Rows[0]["Column N"] = 15;
            mydata.Rows[0]["Column O"] = 15;
            mydata.Rows[0]["Column P"] = 15;

            mydata.Rows.Add( new Object[] { "Series B", 20, 6, 6, 5, 20, 6, 6, 5, 20, 6, 6, 5, 20, 6, 6, 5 } );
            mydata.Rows.Add( new Object[] { "Series C", 5, 8, 8, 20, 5, 8, 8, 20, 5, 8, 8, 20, 5, 8, 8, 20 } );
            mydata.Rows.Add( new Object[] { "Series D", 7, 20, 7, 7, 7, 20, 7, 7, 7, 20, 7, 7, 7, 20, 7, 7 } );
            return mydata;
        }
    }
}
