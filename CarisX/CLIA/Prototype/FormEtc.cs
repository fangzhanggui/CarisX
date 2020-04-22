using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.Serialization.Formatters.Soap;
using Oelco.Common.Utility;
using System.IO;
using System.Text.RegularExpressions;
using Oelco.Common.Parameter;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using System.Threading;
using System.Threading.Tasks;

namespace Prototype
{
    public partial class FormEtc : Form
    {
        public FormEtc()
        {
            InitializeComponent();

            //Invoke( ((Action)(() =>
            //{
            //} )));


            #region リフレクション動作確認
            A abInst = TestAnalyser.Analyse( "AB" );
            A acInst = TestAnalyser.Analyse( "AC" );
            String a = abInst.G();
            String b = acInst.G();
            #endregion

            #region コレクション多様性確認
            List<A> aList = new List<A>();
            aList.Add( new AB() );
            aList.Add( new AC() );
            String item0 = aList.First().G();
            String item1 = aList.Last().G();
            #endregion 
            System.Threading.Mutex mx = new System.Threading.Mutex();
            Boolean mxb = mx.WaitOne( 0 );
            mxb = mx.WaitOne( 0 );
        }

        #region ファイナライザ実行確認
        private List<ClsFin> lstFin = new List<ClsFin>();
        private Int32 counter = 0;
        private void button1_Click( object sender, EventArgs e )
        {
            lstFin.Add( new ClsFin( (++counter).ToString()) );
        }

        private void button2_Click( object sender, EventArgs e )
        {
            lstFin.Clear();
        }

        private void button3_Click( object sender, EventArgs e )
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        #endregion

        #region 非同期Dlgテスト
        System.Threading.ManualResetEvent delender = new System.Threading.ManualResetEvent( false );
        Oelco.Common.Utility.LockObject<Dictionary<String, System.Threading.Mutex>> delDic = new Oelco.Common.Utility.LockObject<Dictionary<String, System.Threading.Mutex>>();
        private void button4_Click( object sender, EventArgs e )
        {
            delDic.Lock();
            if ( !delDic.Get.Instance.ContainsKey( "A" ) )
            {
                delDic.Get.Instance.Add( "A", new System.Threading.Mutex() );
            }
            delDic.UnLock();

            delender.Reset();
            Func<String, String> A = new Func<String, String>( delA );
            A.BeginInvoke( "A", delFin,  A );
        }
        private void button5_Click( object sender, EventArgs e )
        {
            delDic.Lock();
            if ( !delDic.Get.Instance.ContainsKey( "B" ) )
            {
                delDic.Get.Instance.Add( "B", new System.Threading.Mutex() );
            }
            delDic.UnLock();
            delender.Reset();
            Func<String, String> B = new Func<String, String>( delB );
            B.BeginInvoke( "B", delFin, B );

        }
        private void button6_Click( object sender, EventArgs e )
        {
            delender.Set();
        }
        protected String delA( String id )
        {

            delDic.Lock();
            System.Threading.Mutex mtx = delDic.Get.Instance[id];
            delDic.UnLock();

            mtx.WaitOne();
            delender.WaitOne( 1000 * 3 );
            mtx.ReleaseMutex();
            return id;
        }
        protected String delB( String id )
        {
            // ここでException発生させると、EndInvoke呼び出し時にthrowしたExceptionが発生する。
            //throw new Exception();

            delDic.Lock();
            System.Threading.Mutex mtx = delDic.Get.Instance[id];
            delDic.UnLock();
            
            mtx.WaitOne();
            delender.WaitOne(1000*5);
            mtx.ReleaseMutex();

            return id;
        }
        protected void delFin( IAsyncResult ar )
        {
            Func<String, String> de = (Func<String, String>)ar.AsyncState;
            String strRes = (String)de.EndInvoke( ar );
            System.Diagnostics.Debug.WriteLine( String.Format("{0} is end",strRes ));
            Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, String.Format("{0} is end",strRes) );
        }
        #endregion

        #region シリアライズテスト
        private void button7_Click( object sender, EventArgs e )
        {
            SeriTesTes tes = new SeriTesTes();
            tes.strA = "aaa";
            tes.Seria();
        }

        private void button8_Click( object sender, EventArgs e )
        {
            SeriTes tes = new SeriTesTes();
            tes.DeSeria();
        }

        [Serializable]
        public class SeriTes
        {
            public void Seria()
            {
                // 通常のXmlSerializerでは、パブリックメンバしかシリアライズ対象とされない為、
                // SOAPを使う
                // ↑メンテナンスツール側での取り回しが悪い（SOAPプロトコルで書かれたXMLなので、テキスト表示すると普通のXMLと違う）
                //   ので、XmlSerializerにする。
                var lizer = new System.Xml.Serialization.XmlSerializer(this.GetType());
                //var lizer = new SoapFormatter();// this.GetType() );
                System.IO.File.Delete( @"c:\seri.xml" );
                XmlCipher chipher = new XmlCipher( "tet" );
                 using (StreamWriter writer = new StreamWriter( new MemoryStream() ))
                {
                    lizer.Serialize( writer, this );
                    chipher.EncryptFile( writer.BaseStream, @"c:\seri.xml" );
                }


                System.IO.File.Delete( @"c:\noebcseri.xml" );
                using ( var fs = new System.IO.FileStream( @"c:\noebcseri.xml", System.IO.FileMode.CreateNew ) )
                {
                    lizer.Serialize( fs, this );
                }
            }

            public void DeSeria()
            {
                var lizer = new System.Xml.Serialization.XmlSerializer( this.GetType() );

                XmlCipher chipher = new XmlCipher( "tet" );
                Object tes = null;
                Stream reader = new MemoryStream();
                chipher.DecryptFile( @"c:\seri.xml", ref reader );

                try
                {
                    tes = lizer.Deserialize( reader );
                }
                catch ( Exception ex )
                {
                    // 形式違反
                    System.Diagnostics.Debug.WriteLine( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }
                // thisのフィールドに読込んだオブジェクトの値を代入する。
                setter(  tes );


                //var lizer = new SoapFormatter();
                //using ( var fs = new System.IO.FileStream( @"c:\seri.xml", System.IO.FileMode.Open ) )
                //{
                //    // TryCatchする
                //    Object tes = lizer.Deserialize( fs );
                //    setter( tes );
                //}
            }

            // thisに代入できないので、メンバに対してリフレクション使用で設定する。
            protected Boolean setter( Object testes )
            {
                // 同一タイプでなければ失敗
                Type tesType = testes.GetType();
                if ( this.GetType() != tesType )
                {
                    return false;
                }
                foreach ( var field in tesType.GetFields() )
                {
                    field.SetValue( this, field.GetValue( testes ) );
                }
                //foreach ( var property in tesType.GetProperties() )
                //{
                //    property.SetValue( this, property.GetValue( testes,null ),null );
                //}

                return false;
//                tes = testes;
            }
        }

        [Serializable]
        public class SeriTesTes : SeriTes
        {
            public Int32 intA = 3;
            public double dblA = Math.PI;
            public String strA = "SeriTes";
            private Int32 intVa = 99;
            public List<Int32> listVa = new List<Int32>()
            {1, 2, 3
            };

            public Int32 IntVa
            {
                get
                {
                    return intVa;
                }
                set
                {
                    intVa = value;
                }
            }
        }
        #endregion

        private void button9_Click( object sender, EventArgs e )
        {
            // 分析項目登録の"名前1(倍率1),名前2(倍率2)..."
            // 等を切り出してくる正規表現
          
            Regex reg = new Regex( ".*(x(?<value>[0-9]*))" );
            Match ma1 = reg.Match( "AAA" );
            Match ma2 = reg.Match( "BAA(x10)" );
            if ( ma1.Success )
            {
                String str1 = ma1.Value;
            }
            if ( ma2.Success )
            {
                String str2 = ma2.Groups["value"].Value;
            }
        }

        private void button10_Click( object sender, EventArgs e )
        {
            RemarkBit rmkb = (RemarkBit)0x0000000000000011;
            String[] str = rmkb.ToString().Split(new String[]{","}, StringSplitOptions.None );

        }

        /// <summary>
        /// リマークビット定義
        /// </summary>
        [Flags]
        public enum RemarkBit : long
        {
            // TODO:英名でリマークの定義を行う
            /// <summary>
            /// Ｍ試薬吸引エラー 
            /// </summary>
            MReagentSuckingUpError = 0x0000000000000001,
            /// <summary>
            /// Ｒ１試薬吸引エラー
            /// </summary>
            R1ReagentSuckingUpError = 0x0000000000000002,
            /// <summary>
            /// Ｒ２試薬吸引エラー
            /// </summary>
            R2ReagentSuckingUpError = 0x0000000000000004,
            /// <summary>
            /// 前処理液吸引エラー
            /// </summary>
            PreProcessLiquidSuckingUpError = 0x0000000000000008,
            /// <summary>
            /// サンプルなしエラー
            /// </summary>
            NoSampleError = 0x0000000000000010,
            /// <summary>
            /// サンプル詰まりエラー
            /// </summary>
            SampleStoppedUpError = 0x0000000000000020,
            /// <summary>
            /// サンプル空吸エラー
            /// </summary>
            NotSampleSuckingUpError = 0x0000000000000040,
            /// <summary>
            /// サンプル分注エラー
            /// </summary>
            SampleDispenseError = 0x0000000000000080,
            /// <summary>
            /// 希釈液なしエラー
            /// </summary>
            NoDilutionError = 0x0000000000000100,
            /// <summary>
            /// 洗浄不良エラー
            /// </summary>
            WashingFailureError = 0x0000000000000200,
            /// <summary>
            /// 光学系エラー
            /// </summary>
            DetectorError = 0x0000000000000400,
            /// <summary>
            /// サンプル分注チップ装着エラー
            /// </summary>
            SampleDispenseTipSetError = 0x0000000000000800,
            /// <summary>
            /// サンプル分注チップ廃棄エラー
            /// </summary>
            SampleDispenseTipDisposalError = 0x0000000000001000,
            /// <summary>
            /// ダークエラー
            /// </summary>
            DarkError = 0x0000000000002000,
            /// <summary>
            /// 測光エラー
            /// </summary>
            PhotometryError = 0x0000000000004000,
            /// <summary>
            /// 免疫反応槽部温度エラー
            /// </summary>
            TempOfImmunoreactionError = 0x0000000000008000,
            /// <summary>
            /// B/F1プレヒート部温度エラー
            /// </summary>
            TempOfBF1PreHeatError = 0x0000000000010000,
            /// <summary>
            /// B/F2プレヒート部温度エラー
            /// </summary>
            TempOfBF2PreHeatError = 0x0000000000020000,
            /// <summary>
            /// R1プローブプレヒート部温度エラー
            /// </summary>
            TempOfR1ProbePreHeatError = 0x0000000000040000,
            /// <summary>
            /// R2プローブプレヒート部温度エラー
            /// </summary>
            TempOfR2ProbePreHeatError = 0x0000000000080000,
            /// <summary>
            /// プレトリガプレヒート部温度エラー
            /// </summary>
            TempOfPreTriggerPreHeatError = 0x0000000000100000,
            /// <summary>
            /// トリガプレヒート部温度エラー
            /// </summary>
            TempOfTriggerPreHeatError = 0x0000000000200000,
            /// <summary>
            /// フォトマル部温度エラー
            /// </summary>
            TempOfDetectorError = 0x0000000000400000,
            /// <summary>
            /// 試薬保冷庫温度エラー
            /// </summary>
            TempOfReagentCoolerError = 0x0000000008000000,
            /// <summary>
            /// 希釈液保冷庫温度エラー
            /// </summary>
            TempOfDilutionCoolerError = 0x0000000001000000,
            /// <summary>
            /// サイクルタイムオーバー
            /// </summary>
            CycleTimeOverError = 0x0000000002000000,
            /// <summary>
            /// 未測定（ラック強制排出による）
            /// </summary>
            NoMeasuredError = 0x0000000004000000,
            /// <summary>
            /// 反応容器搬送エラー
            /// </summary>
            ReactionVesselCarryError = 0x0000000008000000,
            /// <summary>
            /// 試薬ノズル洗浄エラー
            /// </summary>
            ReagentNozzleWashingError = 0x0000000010000000,
            /// <summary>
            /// 予約
            /// </summary>

            /// <summary>
            /// 検量線エラー
            /// </summary>
            CalibrationCurveError = 0x0000000100000000,
            /// <summary>
            /// 試薬有効期限エラー
            /// </summary>
            ReagentExpirationDateError = 0x0000000200000000,
            /// <summary>
            /// 希釈液有効期限エラー
            /// </summary>
            DilutionExpirationDateError = 0x0000000400000000,
            /// <summary>
            /// プレトリガ有効期限エラー
            /// </summary>
            PreTriggerExpirationDateError = 0x0000000800000000,
            /// <summary>
            /// トリガ有効期限エラー
            /// </summary>
            TriggerExpirationDateError = 0x0000001000000000,
            /// <summary>
            /// 検量線有効期限エラー
            /// </summary>
            CalibExpirationDateError = 0x0000002000000000,
            /// <summary>
            /// 多重測定時のデータ間乖離許容範囲外エラー
            /// </summary>
            DiffError = 0x0000004000000000,
            /// <summary>
            /// 濃度値算出不能エラー
            /// </summary>
            CalcConcError = 0x0000008000000000,
            /// <summary>
            /// キャリブレーション正常範囲外エラー
            /// </summary>
            CalibError = 0x0000010000000000,
            /// <summary>
            /// ダイナミックレンジ正常範囲外エラー／上限値オーバー
            /// </summary>
            DynamicrangeUpperError = 0x0000020000000000,
            /// <summary>
            /// ダイナミックレンジ正常範囲外エラー／下限値オーバー
            /// </summary>
            DynamicrangeLowerError = 0x0000040000000000,
            /// <summary>
            /// 管理値範囲外エラー（Xバー管理図）
            /// </summary>
            ControlRangeError = 0x0000080000000000,
            /// <summary>
            /// 精度管理判定不能
            /// </summary>
            ControlError = 0x0000100000000000,
            /// <summary>
            /// データ編集（手希釈倍率修正）
            /// </summary>
            EditOfManualDil = 0x0000200000000000,
            /// <summary>
            /// データ編集（再計算）
            /// </summary>
            EditOfReCalcu = 0x0000400000000000,
            /// <summary>
            /// データ編集（検量線カウント修正）
            /// </summary>
            EditOfCalibCount = 0x0000800000000000,
            /// <summary>
            /// データ編集（修正された検量線で再計算）
            /// </summary>
            EditOfReCalcuByEditCurve = 0x0001000000000000,
            /// <summary>
            /// データ編集（精度管理濃度修正）
            /// </summary>
            EditOfControlConc = 0x0002000000000000
        }

        #region[エラー画面用]

        private void customUButton2_Click( object sender, EventArgs e )
        {
            // Singleton<ParameterFilePreserve<ErrorCodeDataManager>>.Instance.Load();が呼び出されていること。
            ErrorCodeData errorData = Singleton<ParameterFilePreserve<ErrorCodeDataManager>>.Instance.Param.GetCodeData( "1", "2" );
            if ( errorData == null )
            {
                // 定義されていないエラーコードが指定された。
                return;
            }
            // タイトル・メッセージ・画像パス
            String ErroTitle = errorData.Title;
            String ErrorMessage = errorData.Message;
            String ImagePath = errorData.GetFullImagePath();

        }

        /// <summary>
        /// エラーコードデータ管理クラス
        /// </summary>
        /// <remarks>
        /// エラーコードデータを管理します。
        /// </remarks>
        public class ErrorCodeDataManager : ISavePath
        {
            /// <summary>
            /// エラーコードデータ
            /// </summary>
            private List<ErrorCodeData> codeDataList = new List<ErrorCodeData>();

            /// <summary>
            /// エラーコードデータ 設定/取得
            /// </summary>
            public List<ErrorCodeData> CodeDataList
            {
                get
                {
                    return this.codeDataList;
                }
                set
                {
                    this.codeDataList = value;
                }
            }

            /// <summary>
            /// エラーコードデータ取得
            /// </summary>
            /// <remarks>
            /// エラーコード・エラーコード引数を元にエラーコードデータを取得します。
            /// </remarks>
            /// <param name="errorCode">エラーコード</param>
            /// <param name="errorCodeArgument">エラーコード引数</param>
            /// <returns>エラーコードデータ</returns>
            public ErrorCodeData GetCodeData( String errorCode, String errorCodeArgument )
            {
                ErrorCodeData data = null;

                // エラーコード・引数を元に保持データから検索する。
                IEnumerable<ErrorCodeData> searched = from v in this.codeDataList
                                                      where v.Code == errorCode && v.Argment == errorCodeArgument
                                                      select v;
                if ( searched.Count() != 0 )
                {
                    data = searched.First();
                }

                return data;
            }

            #region ISavePath メンバー

            public String SavePath
            {
                get
                {
                    return @".\ErrorMessage.xml";
                }
            }

            #endregion
        }
        /// <summary>
        /// エラーコードデータ
        /// </summary>
        /// <remarks>
        /// エラーコードデータクラス
        /// </remarks>
        public class ErrorCodeData
        {
            /// <summary>
            /// エラーコード
            /// </summary>
            public String Code = String.Empty;
            /// <summary>
            /// エラーコード引数
            /// </summary>
            public String Argment = String.Empty;
            /// <summary>
            /// エラーメッセージタイトル
            /// </summary>
            public String Title = String.Empty;
            /// <summary>
            /// エラーメッセージ
            /// </summary>
            public String Message = String.Empty;
            /// <summary>
            /// エラー画像パス
            /// </summary>
            public String ImagePath = String.Empty;

            /// <summary>
            /// エラー画像完全パス取得
            /// </summary>
            /// <remarks>
            /// エラー画像パスにフォルダ構成を適用して取得します。
            /// </remarks>
            /// <returns>エラー画像完全パス</returns>
            public String GetFullImagePath()
            {
                String returnPath = this.ImagePath;
                try
                {
                    String root = Path.GetPathRoot( this.ImagePath );
                    if ( ( root == "" ) || ( root == @"\" ) )
                    {
                        // パスがルートからでなければ、エラー画像パスを付与
                        returnPath = Oelco.CarisX.Const.CarisXConst.PathErrImage + @"\" + this.ImagePath;
                    }
                }
                catch ( Exception ex )
                {
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                    // GetPathRootで失敗したら空白
                    returnPath = String.Empty;
                }
                return returnPath;
            }
        }
        #endregion 

        private void FormEtc_Load( object sender, EventArgs e )
        {
            this.ultraPictureBox1.Image = Image.FromFile( @" C:\2.bmp" );
        }

        private void button11_Click( object sender, EventArgs e )
        {
            EtherA A = new EtherA();
            A.a = 123;
            A.b = 456;
            EtherB B = new EtherB();
            B.a = 789;
            B.b = 987;

            A.Dainyu( B );

            A.a = 123;
            A.b = 456;

            B.Dainyu( A );
            
        }

        class EtherA : EtherIF
        {

            #region EtherIF メンバー

            public Int32 a
            {
                get;
                set;
            }

            public Int32 b
            {
                get;
                set;
            }

            #endregion
        }
        class EtherB : EtherIF
        {

            #region EtherIF メンバー

            public Int32 a
            {
                get;
                set;
            }
            public Int32 b
            {
                get;
                set;
            }

            #endregion
        }

        ManualResetEvent maResa = new ManualResetEvent(false);
        ManualResetEvent maResb = new ManualResetEvent( false );
        private void button14_Click( object sender, EventArgs e )
        {            
            ( new Task( () =>
            {
                maResa.Reset();
                maResb.Reset();
                WaitHandle.WaitAny( new WaitHandle[] { maResa, maResb } );
                MessageBox.Show( "Ok" );
            } ) ).Start();
        }

        private void button13_Click( object sender, EventArgs e )
        {
            maResa.Set();
        }

        private void button12_Click( object sender, EventArgs e )
        {
            maResb.Set();
        }

    }
    public class ClsFin
    {
        String counter = "";
        public ClsFin( String counter )
        {
            this.counter = counter;
        }
        ~ClsFin()
        {
            System.Diagnostics.Debug.WriteLine( String.Format("~ClsFin({0})",this.counter) );
            Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, String.Format( "~ClsFin({0})", this.counter ) );
        }
    }

    static public class staA
    {
    }


    /// <summary>
    /// リフレクション使用のアナライザテスト
    /// </summary>
    static public class TestAnalyser
    {

        static public A Analyse( String str )
        {
            String strName = String.Empty;
            Type analysedType = null;
            A instance = null;

            if ( str == "AB" )
            {
                analysedType = typeof( AB );
            }
            else if ( str == "AC" )
            {
                analysedType = typeof( AC );
            }

            if ( analysedType != null )
            {
                instance = (A)Activator.CreateInstance( analysedType );
            }

            return instance;
        }
    }


    public class A
    {
        public virtual String G()
        {
            return "A";
        }

    }
    public class AB : A
    {
        public override String G()
        {
            return "AB";
        }

    }
    public class AC : A
    {
        public override String G()
        {
            return "AC";
        }

    }

    public interface EtherIF
    {
        Int32 a
        {
            get;
            set;
        }
        Int32 b
        {
            get;
            set;
        }
    }
    static class Extention
    {
        public static void Dainyu( this EtherIF a, EtherIF B )
        {
            a.a = B.a;
            a.b = B.b;
        }
    }

}
