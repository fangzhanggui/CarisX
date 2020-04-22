using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Oelco.Common.DB;
using Oelco.Common.Utility;

namespace Prototype
{
    public partial class DBer : Form
    {
        public DBer()
        {
            InitializeComponent();
        }

        private void DBer_Load( object sender, EventArgs e )
        {
//            DataSet set = new DataSet();
//            SqlDataAdapter da = new SqlDataAdapter();
//            //da.Fill(
//            // DataSetからのLinq
//            var lq = from v in set.Tables[0].AsEnumerable()
//                                                  where v.Field<int>( "aaa" ) >= 0
//                                                  select v;
//            lq.First().Field<int>("aaa");

            
//            this.Invoke( (Action<int>)( (Int32 i) => i = i+1 ) );
//            this.Invoke( (Action<int>)delegate (Int32 i)
//            {
////                System.Threading.Thread.
//            });


//            set.Tables[0].Select
            Singleton<ProDB>.Instance.Initialize( "172.30.13.127", "CLIA", 1000, 1000 );
            Boolean opened = Singleton<ProDB>.Instance.Open();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            Singleton<ProDB>.Instance.LoadProTable();

        }

        private void button2_Click( object sender, EventArgs e )
        {

            Singleton<ProDB>.Instance.CommitProTable();
        }
    }

    class ProDB : Oelco.Common.DB.DBAccessControl
    {
        protected override String baseTableSelectSql
        {
            get
            {
                // TODO:UpdateCommand の動的 SQL 生成は、キーである列情報を返さない SelectCommand に対してはサポートされていません。といわれるのでこの周辺を調べる。

                //return "select dbo.analizeLog.logID from dbo.analizeLog";
//                return "select dbo.analizeLog.logID, dbo.analizeLog.writeTime, dbo.analizeLog.userID from dbo.analizeLog";
                return "select * from dbo.analizeLog";
            }
        }

        protected override Int32 TableTransactTimeout
        {
            get
            {
                return 10000;
            }
            set
            {
            }
        }

        public void LoadProTable()
        {
            this.fillBaseTable();
        }
        public void CommitProTable()
        {
            this.updateBaseTable();
        }
    }

}
