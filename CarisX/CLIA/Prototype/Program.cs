using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Prototype
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
			DateTime dtt;
			Boolean success = DateTime.TryParseExact( "20130831", "yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, out dtt );
			success = DateTime.TryParseExact( "20130832", "yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, out dtt );

			DateTime dt;
			DateTime.TryParseExact( "130820", "yyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, out dt );
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );

			//Int32 tes = Test;
			//var f = new クラス利用サンプル等();
            //var f = new GraphTest();
            //Application.Run( f );

            var ed = new LayerTester();
            Application.Run(ed);
        }

		[Obsolete("このプロパティは処理を行いません")]
		static Int32 Test
		{
			get;
			set;
		}
    }
}
