using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Oelco.Common.Utility;
using System.Text.RegularExpressions;

using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using System.Globalization;

using Oelco.CarisX.DB;
using Oelco.Common.Print;
using Oelco.CarisX.Print;
using Oelco.CarisX.Const;

using Oelco.CarisX.Utility;
using Oelco.CarisX.Log;
using Oelco.Common.DB;
using System.Xml.Serialization;
using System.IO;
using Oelco.Common.Log;
using System.Net;

namespace Prototype
{
    public partial class クラス利用サンプル等 : Form
    {
        class DataTestCls
        {
            public Int32 a;
            public Int32 b;
           public  DataTestCls( Int32 a, Int32 b )
            {
                this.a = a;
                this.b = b;
            }
        }
        private String ConvertUnicodeSql( String sql )
        {
            const String UNICODE_SYMBOL = "N";
            Int32 unicodeSymbolLength = UNICODE_SYMBOL.Length;

            // SQL文の"Value()"部にあるシングルクォーテーションで囲まれた値に対してNを付加する。（Unicode対応）
            Regex replaceSentenceRegex = new Regex( @"^insert +into +*? +values *\(.*?\)", RegexOptions.IgnoreCase );
            Regex replaceTargetRegex = new Regex(@"(, *?')|(\( *')");
            String result = sql;

            if ( replaceSentenceRegex.IsMatch( sql ) )
            {
                var replaceMatches = replaceSentenceRegex.Matches( result );
                Int32 indexAdjustCount = 0; // 付与位置補正

                foreach ( Match replaceSentence in replaceMatches )
                {
                    if ( replaceTargetRegex.IsMatch( replaceSentence.Value ) )
                    {

                        // 文字の付与により増加する文字列長に対して、付与前のヒット位置インデックスからの補正を行う。
                        Int32 shiftCount = 0;
                        var replacedSentence = replaceTargetRegex.Replace( replaceSentence.Value, ( replaceTarget ) =>
                        {
                            shiftCount += unicodeSymbolLength;
                            return replaceTarget.Value.Insert( replaceTarget.Value.Length - 1, UNICODE_SYMBOL );
                        } );

                        result = replaceSentenceRegex.Replace( result, replacedSentence, 1, replaceSentence.Index + indexAdjustCount );
                        indexAdjustCount += shiftCount;
                    }
                }
            }

            return result;
        }

        public クラス利用サンプル等()
        {
#if false // 実験ｺｰﾄﾞ
            String tekitou1 = ConvertUnicodeSql( "values('ADbc',de,'cef','deg') select a from b values('abc',de,'cef','deg')" );
            String tekitou2 = ConvertUnicodeSql( "select values('abc','de','cef','deg') a from b values('abc','de','cef','deg')" );
            String tekitou3 = ConvertUnicodeSql( "select a from b values(abc,de,cef,deg)" );

            List<DataTestCls> tes = new List<DataTestCls>();
            tes.Add( new DataTestCls( 1, 2 ) );
            tes.Add( new DataTestCls( 1, 3 ) );
            tes.Add( new DataTestCls( 1, 4 ) );
            Boolean conf1 = tes.IsConflict( ( v ) => v.b );
            tes.Add( new DataTestCls( 1, 2 ) );
            Boolean conf2 = tes.IsConflict( ( v ) => v.b );
            Boolean conf3 = tes.IsConflict( ( v ) => v.a );





            Singleton<SequencialSampleNo>.Instance.Initialize();


            Singleton<SequencialSampleNo>.Instance.CreateNumber();
            Singleton<SequencialSampleNo>.Instance.CreateNumber();

            Singleton<SequencialSampleNo>.Instance.StartCount = 100;
            Singleton<SequencialSampleNo>.Instance.EndCount = 102;
            Singleton<SequencialSampleNo>.Instance.ResetNumber();


            Singleton<SequencialSampleNo>.Instance.CreateNumber();
            Singleton<SequencialSampleNo>.Instance.CreateNumber();
            Singleton<SequencialSampleNo>.Instance.CreateNumber();
            Singleton<SequencialSampleNo>.Instance.CreateNumber();
            Singleton<SequencialSampleNo>.Instance.CreateNumber();



            List<DataTestCls> data = new List<DataTestCls>();
            data.Add( new DataTestCls( 1, 1 ) );
            data.Add( new DataTestCls( 1, 2 ) );
            data.Add( new DataTestCls( 2, 3 ) );
            data.Add( new DataTestCls( 3, 4 ) );
            var selected = from v in data
                           group v by v.a;
            foreach ( var v in selected )
            {
                v.Count();
            }

            IPAddress pa = IPAddress.Parse( "192.168.1.0" );
            var addressBytes = pa.GetAddressBytes();
            Int64 val = 0;
            for(int i = 0; i < addressBytes.Count() ; i++ )
            {
                val <<= 8;
                val += addressBytes[addressBytes.Count()-i-1];
            }

            pa = new IPAddress( val );
            //String st3;
            //String st = null;
            //String st2 = String.Empty;
            //if ( st == st2 )
            //{

            //}
            //var f = (st3 == st2);

            var fo = FontManager.PrintFont;

            CultureInfo clt = System.Threading.Thread.CurrentThread.CurrentCulture;

            //System.Threading.Thread.CurrentThread
            String str = clt.TwoLetterISOLanguageName;
            CultureInfo clt2 = new CultureInfo( "ga" );

            Dictionary<Int32, String> inst = new Dictionary<Int32, String>();
            inst[25] = "fjkadjopiure98yuapre98uga0f9d8v63498yveoirhys puraw@pvojg5epuhbnseme;vsec,tomreyi";
            //DataTable dt = new DataTable();
            //DataRow dr = new DataRow();
            //dr["abc"] = "string";
            //dt.Rows.Add( dr );

            //String dr2 = (String)dt.Rows[0]["abc"];

            ////this.dica[1] = 2;
            ////this.Dica[2] = 3;
            //StringInt32 stin = "  50";
            ////stin += 3;
            //String stinStr = stin.String;
            //stin.Value += 50;
            //String stinStr2 = stin.String;
            //stin.String = "000998";
            //stinStr2 = stin.String;
            //stin.Value += 2;
            //stinStr2 = stin.String;

#endif
            InitializeComponent();
        }

        //private Dictionary<Int32, Int32> dica = new Dictionary<int, int>();

        //public Dictionary<Int32, Int32> Dica const
        //{
        //    get
        //    {
        //        return dica;
        //    }
        //    set
        //    {
        //        dica = value;
        //    }
        //}


        /// <summary>
        /// 履歴ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click( object sender, EventArgs e )
        {
            using ( Form f = new HistoryTest() )
            {
                f.ShowDialog();
            }
        }

        private void button2_Click( object sender, EventArgs e )
        {
            using ( Form f = new FormEtc() )
            {
                f.ShowDialog();
            }
        }

        private void button3_Click( object sender, EventArgs e )
        {
            using ( Form f = new LayerTester() )
            {
                f.ShowDialog();
            }
        }

        private void button4_Click( object sender, EventArgs e )
        {
            using ( Form f = new AreaTest() )
            {
                f.ShowDialog();
            }
        }

        private void button5_Click( object sender, EventArgs e )
        {

            using ( Form f = new SyoKomoku() )
            {
                f.ShowDialog();
            }
        }

        private void button6_Click( object sender, EventArgs e )
        {
            using ( Form f = new Blinker() )
            {
                f.ShowDialog();
            }
        }

        private void button7_Click( object sender, EventArgs e )
        {
            using ( Black f = new Black() )
            {
                f.ShowDialog();
            }
        }

        private void button8_Click( object sender, EventArgs e )
        {
            using ( Form f = new TimeLabeler() )
            {
                f.ShowDialog();
            }
        }

        private void button9_Click( object sender, EventArgs e )
        {
            using ( Form f = new DBer() )
            {
                f.ShowDialog();
            }
        }

        private void button10_Click( object sender, EventArgs e )
        {
            using (Form f = new CustomChartTest())
            {
                f.ShowDialog();
            }
        }

        private void button11_Click( object sender, EventArgs e )
        {

            using ( Form f = new Para() )
            {
                f.ShowDialog();
            }
        }

        private void button12_Click( object sender, EventArgs e )
        {
            using (Form f = new CommonMessageDialog())
            {
                f.ShowDialog();
            }
        }

        private void button13_Click( object sender, EventArgs e )
        {
            using (Form f = new RangeSelectTest())
            {
                f.ShowDialog();
            }
        }
        
        private void button14_Click( object sender, EventArgs e )
        {
            using (Form f = new Oelco.CarisX.GUI.DlgDateSelect("",DateTime.Now))
            {
                f.ShowDialog();
            }
        }
        
        private void button15_Click( object sender, EventArgs e )
        {
            using (Form f = new ReagentDialog())
            {
                f.ShowDialog();
            }
        }

        private void button16_Click( object sender, EventArgs e )
        {
            // Proto雛形
            foreach ( var v in Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList )
            {
                //v.ConcsOfEach.Add( 0 );
                //v.ConcsOfEach.Add( 0 );
                //v.CountRangesOfEach.Add( new MeasureProtocol.ItemRange() );
                //v.CountRangesOfEach.Add( new MeasureProtocol.ItemRange() );
                //v.AdjustStatus.Add( new MeasureProtocol.ItemAdjustPoint() );
                //v.AdjustStatus.Add( new MeasureProtocol.ItemAdjustPoint() );
            }
            Singleton<MeasureProtocolManager>.Instance.SaveAllMeasureProtocol();

        }
        private void button18_Click( object sender, EventArgs e )
        {
            // Proto読込
            Singleton<MeasureProtocolManager>.Instance.LoadAllMeasureProtocol();
        }

        private void button17_Click( object sender, EventArgs e )
        {
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Save();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            ErrorCodeDialog errCode = new ErrorCodeDialog();
            errCode.Show();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            ///************************ START ******************************/

            // SpecimenRegistrationGridViewDataSet型のサンプルデータを作成
            // テスト用データ
            // 実際はSpecimenRegistrationGridViewDataSetよりデータを取得
            SpecimenRegistrationPrint prt = new SpecimenRegistrationPrint();

            Singleton<MeasureProtocolManager>.Instance.LoadAllMeasureProtocol();

            List<SpecimenRegistrationGridViewDataSet> grdData = new List<SpecimenRegistrationGridViewDataSet>();

            // リスト指定
            List<Tuple<Int32?, Int32?>> reg = new List<Tuple<Int32?, Int32?>>();
            Tuple<Int32?, Int32?> regItem1 = new Tuple<Int32?, Int32?>(1, 10);
            Tuple<Int32?, Int32?> regItem2 = new Tuple<Int32?, Int32?>(2, 20);
            Tuple<Int32?, Int32?> regItem3 = new Tuple<Int32?, Int32?>(3, 30);
            Tuple<Int32?, Int32?> regItem4 = new Tuple<Int32?, Int32?>(4, 40);
            Tuple<Int32?, Int32?> regItem5 = new Tuple<Int32?, Int32?>(5, 50);

            //list.Add(new Tuple<Int32?, Int32?>(1,1));
            reg.Add(regItem1);
            reg.Add(regItem2);
            reg.Add(regItem3);
            reg.Add(regItem4);
            reg.Add(regItem5);

            SpecimenRegistrationGridViewDataSet row1 = new SpecimenRegistrationGridViewDataSet();
            row1.RackID = "1";
            row1.RackPosition = 1;
            row1.ReceiptNumber = 12;
            row1.PatientID = "ABCDEFGHIJKLMNOP";
            row1.SpecimenType = SpecimenMaterialType.BloodSerumAndPlasma;
            row1.Registered = reg;
            row1.ManualDil = 1;
            row1.Comment = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZAB";
            grdData.Add(row1);

            SpecimenRegistrationGridViewDataSet row2 = new SpecimenRegistrationGridViewDataSet();
            row2.RackID = "2";
            row2.RackPosition = 2;
            row2.ReceiptNumber = 23;
            row2.PatientID = "2222";
            row2.SpecimenType = SpecimenMaterialType.BloodSerumAndPlasma;
            row2.Registered = reg;
            row2.ManualDil = 1;
            row2.Comment = "";
            grdData.Add(row2);

            SpecimenRegistrationGridViewDataSet row3 = new SpecimenRegistrationGridViewDataSet();
            row3.RackID = "3";
            row3.RackPosition = 3;
            row3.ReceiptNumber = 34;
            row3.PatientID = "3333";
            row3.SpecimenType = SpecimenMaterialType.Urine;
            row3.Registered = reg;
            row3.ManualDil = 1;
            row3.Comment = "";
            grdData.Add(row3);

            SpecimenRegistrationGridViewDataSet row4 = new SpecimenRegistrationGridViewDataSet();
            row4.RackID = "4";
            row4.RackPosition = 4;
            row4.ReceiptNumber = 45;
            row4.PatientID = "4444";
            row4.SpecimenType = SpecimenMaterialType.BloodSerumAndPlasma;
            row4.Registered = reg;
            row4.ManualDil = 1;
            row4.Comment = "";
            grdData.Add(row4);

            // 印刷
            Boolean ret = prt.Print(grdData);
            if (ret)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("成功しました。"));
                Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "成功しました。" );
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(String.Format("失敗しました。"));
                Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "失敗しました。" );
            }

            /************************ END ********************************/
        }

        private void button21_Click( object sender, EventArgs e )
        {
            SubFunction.ApplicationInitialize();
        }

        private void button22_Click( object sender, EventArgs e )
        {
            //Singleton<SQLServerDBAccess>.Instance.Initialize( Environment.MachineName, "CLIA", 1000, 1000 );
            //Singleton<SQLServerDBAccess>.Instance.Open();
            //var optvalue = new CarisXLogInfoErrorLogExtention();
            //Singleton<CarisXLogManager>.Instance.Initialize( "", "LogInfo", 10000, 100, 20, false );
            //Singleton<CarisXLogManager>.Instance.Write( Oelco.Common.Log.LogKind.ErrorHist, "USER", optvalue, "コンテンツー！！" );
        }

        private void button23_Click( object sender, EventArgs e )
        {
            Singleton<ProgressBarTest>.Instance.Show();
        }

        private void button24_Click( object sender, EventArgs e )
        {
            using ( var main = new Oelco.CarisX.GUI.FormMainFrame() )
            {
                main.Show();
            }
        }

        private void button25_Click( object sender, EventArgs e )
        {
            //string res;
            //var result = Oelco.Common.Utility.SubFunction.ShowSaveCSVFileDialog( out res, Oelco.Common.Const.OutputFileKind.CSV, "output", "出力" );
            //result = Oelco.Common.Utility.SubFunction.ShowSaveCSVFileDialog( out res, Oelco.Common.Const.OutputFileKind.CSV | Oelco.Common.Const.OutputFileKind.XML, "output", "出力" );
        }

        private void button26_Click( object sender, EventArgs e )
        {
            SerializableTimeSpan span = new SerializableTimeSpan();
            span = new TimeSpan( 123, 0, 0 );
            Int64 va = span.Value;

            XmlSerializer lizer = new XmlSerializer( typeof( SerializableTimeSpan ) );

            using ( StreamWriter writer = new StreamWriter( new FileStream( @"c:\timespa.xml", FileMode.CreateNew ) ) )
            {
                lizer.Serialize( writer, span );
            }
            using ( StreamReader sr = new StreamReader( new FileStream( @"c:\timespa.xml", FileMode.Open, FileAccess.Read ) ) )
            {
                SerializableTimeSpan loadedObjClone = (SerializableTimeSpan)lizer.Deserialize( sr );
            }
        }

        private void button27_Click( object sender, EventArgs e )
        {
            using ( var dlg = new GraphTest() )
            {
                dlg.ShowDialog();
            }
        }

        private void button28_Click( object sender, EventArgs e )
        {
            using ( var dlg = new FormNumericEditor() )
            {
                dlg.ShowDialog();
            }
        }

        private void クラス利用サンプル等_Load( object sender, EventArgs e )
        {

        }

        private void button29_Click( object sender, EventArgs e )
        {
            // 割り込みテスト
            using ( var dlg = new TimeInterrupter() )
            {
                dlg.ShowDialog();
            }
        }

        List<Form> fList = new List<Form>();
        private void button30_Click( object sender, EventArgs e )
        {
            var dlg = new FormSoftKeyBord();            
            dlg.flg = false;
            dlg.Show();
            fList.Add( dlg );
            
            
        }

        private void button31_Click( object sender, EventArgs e )
        {
            foreach ( var dlg in from v in fList
                                 where !v.Visible
                                 select v )
            {
                dlg.Dispose();
            }
            fList.Clear();
            GC.Collect();
        }

    }

    //public class OwnedDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    //{
    //    private Object owner = null;
    //    public OwnedDictionary( Object owner )
    //    {
    //        this.owner = owner;
    //    }
    //    public new void Add( TKey key, TValue value )
    //    {
    //        if ( owner != null )
    //        {

    //        }
    //    }
    //    public new void Clear();
    //    public new bool Remove( TKey key );
    //}
}
