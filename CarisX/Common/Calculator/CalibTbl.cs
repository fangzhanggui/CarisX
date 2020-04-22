using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;
using Oelco.Common.Const;

namespace Oelco.Common.Calculator
{
#if false
    public enum axis_type : int
    {
        axLin = 0,
        axLog = 1
    };
    public class UCalibTable
    {

        public const Int32 MAX_CALIB_LOT = 3;
        public const Int32 MAX_CALIB_DATA = 5;

        public const String CALIB_TABLE_SECTION = "プロトコル{0:00}";
        public const String CALIB_MASTER_NAME = "マスターカーブ";
        public const String CALIB_TABLE_NAME = "ロット{0:0}-{1:0}";
        public const String CALIB_ABS_NAME = "閾値{0:0}-{1:0}";
        public const String CALIB_MEAS_SECTION = "前回測定検量線";
        public const String CALIB_MEAS_KEY_PROTO = "プロトコル";
        public const String CALIB_MEAS_KEY_LOT = "ロット";

        public String FileName;				// システムパラメータテーブルファイル名
        //public IniFile myIni;

        // ユーザー宣言
        // ヘッダ部
        public String LotNo;					// ロットNo.
        public String aDate;					// 日付 yyyy/mm/dd
        public String aTime;					// 時間 hh/nn
        public String dateTime;				// 日付時間 'yyyy/mm/dd hh/nn'

        // 情報部
        public Int32 SeqNo;					// SeqNo,
        public String RackID;					// ラックID
        public Int32 RackPos;				// ラック位置
        public String StdID;                  // 標準液ID

        public axis_type XAxis;				// X軸Log/Liner)
        public axis_type YAxis;				// Y軸Log/Liner)Int32

        public fitcurve_type FittingCurve;		// フィッティングカーブ

        // データ部
        public Int32 Count;					// データポイント数
        public double[] x = new double[10];		// x  //Conc
        public double[] y = new double[10];		// y  //Qcode
        public Int64[] UniqNo = new Int64[10]; // ユニーク番号

        // 表示用データ
        public Int32 RepCount;       // リプリケーション数
        public Int32[] RepNo = new Int32[10 * 5];    // リプリケーション番号
        public double[] Conc = new double[10 * 5];     // 濃度
        public double[] Qcode = new double[10 * 5];    // 吸光度
        public double[] QcodeAve = new double[10 * 5]; // 平均値(採用値)

        public UCalibTable CalibTable;


        public Int32 MeasProtoNo;
        public String MeasLotNo;

        public double Abs0_3Min;           //0-3分吸光度差

        //
        //  機能     : オブジェクトの初期化
        //
        //  返り値   : なし
        public UCalibTable()
        {
            FileName = SubFunction.GetApplicationDirectory();
            FileName += GlobalConst.CALIB_TBL_NAME;

            // TODO:INIファイル置換え
            //myIni = new IniFile( FileName );
            //if ( myIni == null )
            //{
            //    return;
            //}
                        
            this.addLfTableIfNone();
            this.addSaTableIfNone();
            //
        }
        //---------------------------------------------------------------------------

        //
        //  機能     : オブジェクトの削除時処理
        //
        //  返り値   : なし
        ~UCalibTable()
        {
        }



        //ラクトフェリン検量線テーブルを追加する。（もしなければ） 2010/9/1追加
        void addLfTableIfNone()
        {
            // TODO:LF検量線テーブル追加、要不要判断する。
            //もしラクトフェリンの検量線テーブルが存在していなければテンプレートを追加する。
            //if ( !this.readMaster( GlobalConst.PROTO_LF_NSX ) )
            //{
            //    String name;

            //    name = SubFunction.GetApplicationDirectory();
            //    name += GlobalConst.CALIB_TBL_NAME;


            //    using ( System.IO.StreamWriter file = new System.IO.StreamWriter( name, true, Encoding.GetEncoding( "shift-jis" ) ) )
            //    {
            //        file.WriteLine( "\r\n\r\n" );
            //        file.WriteLine( "[プロトコル09]\r\n" );
            //        file.WriteLine( "マスターカーブ=1999/12/01,10:11,,0,,0,,0,0,3,7,0.000000,0.000000,0,5.000000,25.000000,0,10.000000,50.000000,0,20.000000,100.000000,0,40.000000,150.000000,0,80.000000,200.000000,0,160.000000,250.000000,0,1,1,0.000000,0.000000,0.000000,1,5.000000,25.000000,25.000000,1,10.000000,50.000000,50.000000,1,20.000000,100.000000,50.000000,1,40.000000,150.000000,150.000000,1,80.000000,200.000000,200.000000,1,160.000000,250.000000,250.000000\r\n" );
            //        file.WriteLine( "ロット1-1=\r\n" );
            //        file.WriteLine( "ロット1-2=\r\n" );
            //        file.WriteLine( "ロット1-3=\r\n" );
            //        file.WriteLine( "ロット1-4=\r\n" );
            //        file.WriteLine( "ロット1-5=\r\n" );
            //        file.WriteLine( "ロット2-1=\r\n" );
            //        file.WriteLine( "ロット2-2=\r\n" );
            //        file.WriteLine( "ロット2-3=\r\n" );
            //        file.WriteLine( "ロット2-4=\r\n" );
            //        file.WriteLine( "ロット2-5=\r\n" );
            //        file.WriteLine( "ロット3-1=\r\n" );
            //        file.WriteLine( "ロット3-2=\r\n" );
            //        file.WriteLine( "ロット3-3=\r\n" );
            //        file.WriteLine( "ロット3-4=\r\n" );
            //        file.WriteLine( "ロット3-5=\r\n" );
            //        file.WriteLine( "\r\n" );
            //    }


            //}
        }


        //唾液中ヘモグロビン検量線テーブルを追加する。（もしなければ） 
        void addSaTableIfNone()
        {

            // TODO:SA検量線テーブル追加、要不要判断する。
            ////もし唾液中ヘモグロビンの検量線テーブルが存在していなければテンプレートを追加する。
            //if ( !this.readMaster( GlobalConst.PROTO_HB_SA ) )
            //{
            //    String name;
            //    name = SubFunction.GetApplicationDirectory();
            //    name += GlobalConst.CALIB_TBL_NAME;

            //    using ( System.IO.StreamWriter file = new System.IO.StreamWriter( name, true, Encoding.GetEncoding( "shift-jis" ) ) )
            //    {
            //        file.WriteLine( "\r\n\r\n" );
            //        file.WriteLine( "[プロトコル10]\r\n" );
            //        file.WriteLine( "マスターカーブ=1999/12/01,10:11,,0,,0,,0,0,3,7,0.000000,0.000000,0,5.000000,25.000000,0,10.000000,50.000000,0,20.000000,100.000000,0,40.000000,150.000000,0,80.000000,200.000000,0,160.000000,250.000000,0,1,1,0.000000,0.000000,0.000000,1,5.000000,25.000000,25.000000,1,10.000000,50.000000,50.000000,1,20.000000,100.000000,50.000000,1,40.000000,150.000000,150.000000,1,80.000000,200.000000,200.000000,1,160.000000,250.000000,250.000000\r\n" );
            //        file.WriteLine( "ロット1-1=\r\n" );
            //        file.WriteLine( "ロット1-2=\r\n" );
            //        file.WriteLine( "ロット1-3=\r\n" );
            //        file.WriteLine( "ロット1-4=\r\n" );
            //        file.WriteLine( "ロット1-5=\r\n" );
            //        file.WriteLine( "ロット2-1=\r\n" );
            //        file.WriteLine( "ロット2-2=\r\n" );
            //        file.WriteLine( "ロット2-3=\r\n" );
            //        file.WriteLine( "ロット2-4=\r\n" );
            //        file.WriteLine( "ロット2-5=\r\n" );
            //        file.WriteLine( "ロット3-1=\r\n" );
            //        file.WriteLine( "ロット3-2=\r\n" );
            //        file.WriteLine( "ロット3-3=\r\n" );
            //        file.WriteLine( "ロット3-4=\r\n" );
            //        file.WriteLine( "ロット3-5=\r\n" );
            //        file.WriteLine( "\r\n" );
            //    }
            //}
        }



        //---------------------------------------------------------------------------

        // 前回測定した検量線を取得
        Boolean readMeas()
        {
            // TODO:前回測定した検量線を取得(DB)

            // TODO:INIファイル置換え
            //if ( myIni == null )
            //{
            //    return false;
            ////}
            //String strSection, strKey;

            //// セクション
            //strSection = CALIB_MEAS_SECTION;

            //// プロトコル番号
            //strKey = CALIB_MEAS_KEY_PROTO;
            //MeasProtoNo = myIni.ReadInteger( strSection, strKey, 0 );

            //// ロット番号
            //strKey = CALIB_MEAS_KEY_LOT;
            //MeasLotNo = myIni.ReadString( strSection, strKey, "" );


            // 日付は最新を取得
            return true;
        }
        //---------------------------------------------------------------------------

        // 前回測定した検量線を取得
        Boolean writeMeas( Int32 nMeasProtoNo, String aMeasLotNo )
        {
            // TODO:前回測定した検量線を設定(DB)
            // TODO:INIファイル置換え
            //if ( myIni == null )
            //{
            //    return false;
            //}
            //String strSection, strKey;

            //MeasProtoNo = nMeasProtoNo;
            //MeasLotNo = aMeasLotNo;

            //// セクション
            //strSection = CALIB_MEAS_SECTION;

            //// プロトコル番号
            //strKey = CALIB_MEAS_KEY_PROTO;
            //myIni.WriteInteger( strSection, strKey, MeasProtoNo );

            //// ロット番号
            //strKey = CALIB_MEAS_KEY_LOT;
            //myIni.WriteString( strSection, strKey, MeasLotNo );

            // 日付は最新を取得
            return true;
        }

        // ヘッダ部取得
        Boolean readHead( Int32 protoNo, Int32 lotNo, Int32 dataNo )
        {
            // TODO:INIファイル置換え
            //if ( myIni == null )
            //{
            //    return false;
            //}
            //String strSection, strKey, strText, strWk;
            //// セクション
            //strSection = String.Format( CALIB_TABLE_SECTION, protoNo );

            //if ( lotNo == -1 && dataNo == -1 )
            //{
            //    strKey = CALIB_MASTER_NAME;
            //}
            //else
            //{
            //    // キー
            //    strKey = String.Format( CALIB_TABLE_NAME, lotNo, dataNo );
            //}
            //strText = myIni.ReadString( strSection, strKey, "" );


            //if ( strText == String.Empty )
            //    return false;

            //// 日付
            //Int32 nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //aDate = strWk;
            //strText = strText.Remove( 0, nPos );

            //// 時間
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //aTime = strWk;
            //strText = strText.Remove( 0, nPos );

            //dateTime = aDate + " " + aTime;

            //// ﾛｯﾄNo.
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //LotNo = strWk;
            //strText = strText.Remove( 0, nPos );

            //// SeqNo,
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //SeqNo = CppDummy.atoi( strWk );
            //strText = strText.Remove( 0, nPos );

            //// ラックID
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //RackID = strWk;
            //strText = strText.Remove( 0, nPos );

            //// ラック位置
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //RackPos = CppDummy.atoi( strWk );
            //strText = strText.Remove( 0, nPos );

            //// 標準液ID
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    // 元がおかしい
            //    // 2004.10.22 BB MOD
            //    nPos = strText.Length + 1;
            //    //return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //StdID = strWk;
            //strText = strText.Remove(1, nPos);

            return true;
        }
        //---------------------------------------------------------------------------

        // テーブルを取得
        Boolean readMaster( Int32 protoNo )
        {

            // TODO:検量線テーブルを取得(DB)
            //if ( myIni == null )
            //{
            //    return false;
            //}
            return this.readTable( protoNo, -1, -1 );
        }
        //---------------------------------------------------------------------------


        // テーブルを取得
        Boolean readTable( Int32 protoNo, Int32 lotNo, Int32 dataNo )
        {
            // TODO:検量線テーブルを取得(DB)
            //if ( myIni == null )
            //{
            //    return false;
            //}
            //String strSection, strKey, strText, strWk;

            //// セクション
            //strSection = String.Format( CALIB_TABLE_SECTION, protoNo );

            //// キー
            //if ( lotNo == -1 || dataNo == -1 )
            //{
            //    strKey = CALIB_MASTER_NAME;
            //}
            //else
            //{
            //    strKey = String.Format( CALIB_TABLE_NAME, lotNo, dataNo );
            //}

            //strText = myIni.ReadString( strSection, strKey, "" );

            //if ( strText == String.Empty )
            //    return false;

            ////// ヘッダ部 ////
            //// 日付
            //Int32 nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //aDate = strWk;
            //strText = strText.Remove( 0, nPos );

            //// 時間
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //aTime = strWk;
            //strText = strText.Remove( 0, nPos );

            //dateTime = aDate + " " + aTime;

            //// ﾛｯﾄNo.
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //LotNo = strWk;
            //strText = strText.Remove( 0, nPos );

            //// SeqNo,
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //SeqNo = CppDummy.atoi( strWk );
            //strText = strText.Remove( 0, nPos );

            //// ラックID
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //RackID = strWk;
            //strText = strText.Remove( 0, nPos );

            //// ラック位置
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //RackPos = CppDummy.atoi( strWk );
            //strText = strText.Remove( 0, nPos );

            //// 標準液ID
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //StdID = strWk;
            //strText = strText.Remove( 0, nPos );

            ////// 情報部部 ////
            //// X軸
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //XAxis = (axis_type)CppDummy.atoi( strWk );
            //strText = strText.Remove( 0, nPos );

            //// Y軸
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //YAxis = (axis_type)CppDummy.atoi( strWk );
            //strText = strText.Remove( 0, nPos );

            //// フィッティングカーブ
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //FittingCurve = (fitcurve_type)CppDummy.atoi( strWk );
            //strText = strText.Remove( 0, nPos );

            //if ( FittingCurve != fitcurve_type.fcSpline &&
            //    FittingCurve != fitcurve_type.fc4Parameter )
            //{
            //    // デフォルト
            //    FittingCurve = fitcurve_type.fc4Parameter; //fcSpline;
            //}

            ////// データ部 ////
            //// データ数
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    return false;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //Count = CppDummy.atoi( strWk );
            //strText = strText.Remove( 0, nPos );

            //for ( Int32 i = 0; i < Count; i++ )
            //{
            //    // X値
            //    nPos = strText.IndexOf( "," ) + 1;
            //    if ( nPos == 0 )
            //    {
            //        return false;
            //    }
            //    strWk = strText.Substring( 0, nPos - 1 );
            //    x[i] = CppDummy.atof( strWk );
            //    strText = strText.Remove( 0, nPos );
            //    // Y値
            //    nPos = strText.IndexOf( "," ) + 1;
            //    if ( nPos == 0 )
            //    {
            //        strWk = strText;
            //    }
            //    else
            //    {
            //        strWk = strText.Substring( 0, nPos - 1 );
            //    }
            //    y[i] = CppDummy.atof( strWk );
            //    strText = strText.Remove( 0, nPos );
            //    // ユニーク番号
            //    nPos = strText.IndexOf( "," ) + 1;
            //    if ( nPos == 0 )
            //    {
            //        strWk = strText;
            //    }
            //    else
            //    {
            //        strWk = strText.Substring( 0, nPos - 1 );
            //    }
            //    UniqNo[i] = CppDummy.atoi( strWk );
            //    strText = strText.Remove( 0, nPos );
            //}
            //// リプリケーション数
            //nPos = strText.IndexOf( "," ) + 1;
            //if ( nPos == 0 )
            //{
            //    // データが無い場合は、リプリケーション１回のデータとする
            //    RepCount = 1;
            //    for ( Int32 i = 0; i < Count; i++ )
            //    {
            //        RepNo[i] = 1;
            //        Conc[i] = x[i];
            //        Qcode[i] = y[i];
            //        QcodeAve[i] = Qcode[i];
            //    }
            //    return true;
            //}
            //strWk = strText.Substring( 0, nPos - 1 );
            //RepCount = CppDummy.atoi( strWk );
            //strText = strText.Remove( 0, nPos );

            //for ( Int32 i = 0; i < Count * RepCount; i++ )
            //{
            //    // リプリケーション番号
            //    nPos = strText.IndexOf( "," ) + 1;
            //    if ( nPos == 0 )
            //    {
            //        return false;
            //    }
            //    strWk = strText.Substring( 0, nPos - 1 );
            //    RepNo[i] = CppDummy.atoi( strWk );
            //    strText = strText.Remove( 0, nPos );
            //    // 濃度
            //    nPos = strText.IndexOf( "," ) + 1;
            //    if ( nPos == 0 )
            //    {
            //        strWk = strText;
            //    }
            //    else
            //    {
            //        strWk = strText.Substring( 0, nPos - 1 );
            //    }
            //    Conc[i] = CppDummy.atof( strWk );
            //    strText = strText.Remove( 0, nPos );
            //    // 吸光度差
            //    nPos = strText.IndexOf( "," ) + 1;
            //    if ( nPos == 0 )
            //    {
            //        strWk = strText;
            //    }
            //    else
            //    {
            //        strWk = strText.Substring( 0, nPos - 1 );
            //    }
            //    Qcode[i] = CppDummy.atof( strWk );
            //    strText = strText.Remove( 0, nPos );
            //    // 吸光度差採用値
            //    nPos = strText.IndexOf( "," ) + 1;
            //    if ( nPos == 0 )
            //    {
            //        strWk = strText;
            //    }
            //    else
            //    {
            //        strWk = strText.Substring( 0, nPos - 1 );
            //    }
            //    QcodeAve[i] = CppDummy.atof( strWk );
            //    strText = strText.Remove( 0, nPos );

            //}

            return true;
        }
        //---------------------------------------------------------------------------


         // 0-3分吸光度差をセット
        Boolean write0_3Abs( Int32 protoNo, Int32 lotNo, Int32 dataNo )
        {
            // TODO:0-3分吸光度差設定 共通化対応を考慮する必要あり。
            //if ( myIni == null )
            //{
            //    return false;
            //}
            //String strSection, strKey;

            //// セクション
            //strSection = String.Format( "プロトコル{0:00}", protoNo );
            //// キー
            //strKey = String.Format( CALIB_ABS_NAME, lotNo, dataNo );
            //myIni.WriteDouble( strSection, strKey, this.Abs0_3Min );

            return true;
        }


        // 0-3分吸光度差を取得
        //double read0_3Abs( Int32 protoNo, Int32 lotNo, Int32 dataNo )
        //{
        //    // TODO:0-3分吸光度差取得 共通化対応を考慮する必要あり。
        //    //if ( myIni == null )
        //    //{
        //    //    return 0.0;
        //    //}
        //    //String strSection, strKey;

        //    //// セクション
        //    //strSection = String.Format( "プロトコル{0:00}", protoNo );
        //    //// キー
        //    //strKey = String.Format( CALIB_ABS_NAME, lotNo, dataNo );
        //    //double abs0_3 = myIni.ReadDouble( strSection, strKey, 9999.0 );
        //    return abs0_3;
        //}



        // テーブルを取得
        Boolean writeTable( Int32 protoNo, Int32 lotNo, Int32 dataNo )
        {
            // TODO:検量線テーブル設定 共通化対応を考慮する必要あり。
            //if ( myIni == null )
            //{
            //    return false;
            //}
            //String strSection, strKey, strText, strWk;

            //// セクション
            //strSection = String.Format( "プロトコル{0:00}", protoNo );
            //// キー
            //if ( lotNo == -1 || dataNo == -1 )
            //{
            //    strKey = CALIB_MASTER_NAME;
            //}
            //else
            //{
            //    strKey = String.Format( CALIB_TABLE_NAME, lotNo, dataNo );
            //}
            ////strKey = String.Format( ロット{0:0}-{1:0}", lotNo, dataNo);

            //strText = "";

            ////// ヘッダ部 ////
            //// 日付
            //strText += aDate;
            //strText += ",";

            //// 時間
            //strText += aTime;
            //strText += ",";

            //// ﾛｯﾄNo.
            //strText += LotNo;
            //strText += ",";

            ////// 情報部 ////
            //// シーケンス番号
            //strWk = String.Format( "{0:0}", SeqNo );
            //strText += strWk;
            //strText += ",";

            //// ラックID
            //strText += RackID;
            //strText += ",";

            //// ラック位置
            //strWk = String.Format( "{0:0}", RackPos );
            //strText += strWk;
            //strText += ",";

            //// 標準液ID
            //strText += StdID;
            //strText += ",";

            //// X軸
            //strWk = String.Format( "{0:0}", XAxis );
            //strText += strWk;
            //strText += ",";

            //// Y軸
            //strWk = String.Format( "{0:0}", YAxis );
            //strText += strWk;
            //strText += ",";

            //// フィッティングカーブ
            //strWk = String.Format( "{0:0}", FittingCurve );
            //strText += strWk;
            //strText += ",";

            ////// データ部 ////
            //// データ数
            //strWk = String.Format( "{0:0}", Count );
            //strText += strWk;

            //// データ値
            //for ( Int32 i = 0; i < Count; i++ )
            //{
            //    // X,Y値
            //    strWk = String.Format( "{0:0.0},{1:0.0},{2:0}", x[i], y[i], UniqNo[i] );
            //    strText += strWk;
            //}

            //// リプリケーション数
            //strWk = String.Format( "{0:0}", RepCount );
            //strText += strWk;

            //// データ値
            //for ( Int32 i = 0; i < Count * RepCount; i++ )
            //{
            //    // X,Y値
            //    strWk = String.Format( "{0:0},{1:0.0},{2:0.0},{3:0.0}", RepNo[i], Conc[i], Qcode[i], QcodeAve[i] );
            //    strText += strWk;
            //}


            //myIni.WriteString( strSection, strKey, strText );

            return true;
        }
        //---------------------------------------------------------------------------

        // 削除
        Boolean delTable( Int32 protoNo, Int32 lotNo, Int32 dataNo )
        {
            // TODO:検量線テーブル削除
            //if ( myIni == null )
            //{
            //    return false;
            //}
            //String strSection, strKey;

            //// セクション
            //strSection = String.Format( "プロトコル{0:00}", protoNo );
            //// キー
            //strKey = String.Format( "{0:0}-{1:0}", lotNo, dataNo );

            //myIni.WriteString( strSection, strKey, "" );

            return true;
        }
        //---------------------------------------------------------------------------


        //---------------------------------------------------------------------------
        // ロット番号列挙
        //Boolean enumLotNo( Int32 protoNo, List<String> arLotNo )
        //{
        //    arLotNo.Clear();
        //    arLotNo.Add( "MST" );
        //    for ( Int32 i = 1; i <= MAX_CALIB_LOT; i++ )
        //    {
        //        for ( Int32 j = 1; j <= MAX_CALIB_DATA; j++ )
        //        {
        //            if ( this.readHead( protoNo, i, j ) )
        //            {
        //                arLotNo.Add( this.LotNo );
        //                break;
        //            }
        //        }
        //    }
        //    return true;
        //}
        //---------------------------------------------------------------------------

        // 日付時間列挙
        //Boolean enumDate( Int32 protoNo, String aLotNo, List<String> arDate )
        //{
        //    arDate.Clear();
        //    if ( aLotNo == "MST" )
        //    {
        //        if ( this.readHead( protoNo, -1, -1 ) )
        //        {
        //            arDate.Add( this.dateTime );
        //        }
        //    }
        //    else
        //    {
        //        List<String> pList = new List<String>();
        //        for ( Int32 i = 1; i <= MAX_CALIB_LOT; i++ )
        //        {
        //            for ( Int32 j = 1; j <= MAX_CALIB_DATA; j++ )
        //            {
        //                if ( this.readHead( protoNo, i, j ) )
        //                {
        //                    if ( this.LotNo == aLotNo )
        //                    {
        //                        pList.Add( this.dateTime );
        //                    }
        //                }
        //            }
        //        }
        //        pList.Sort();

        //        for ( Int32 i = 0; i < pList.Count; i++ )
        //        {
        //            Int32 idx = pList.Count - 1 - i;
        //            arDate.Add( pList[idx] );
        //        }
        //        pList = null;

        //    }
        //    return true;
        //}
        //---------------------------------------------------------------------------


        // 読み込み
        //public Boolean getData( Int32 nProtoNo, String sLotNo, String sDateTime )
        //{
        //    if ( sLotNo == "MST" )
        //    {
        //        this.readTable( nProtoNo, -1, -1 );
        //        return true;
        //    }

        //    // ロットMAX=3
        //    for ( Int32 lot = 1; lot <= MAX_CALIB_LOT; lot++ )
        //    {
        //        for ( Int32 n = 1; n <= MAX_CALIB_DATA; n++ )
        //        {

        //            if ( this.readHead( nProtoNo, lot, n ) )
        //            {
        //                // ロットを探す
        //                if ( this.LotNo == sLotNo )
        //                {
        //                    // 日付チェック
        //                    if ( sDateTime == this.dateTime )
        //                    {
        //                        return this.readTable( nProtoNo, lot, n );
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}
        //---------------------------------------------------------------------------

        // 登録
        //public Boolean setData( Int32 nProtoNo, String sLotNo, String sStdID )
        //{
        //    if ( myIni == null )
        //    {
        //        return false;
        //    }

        //    //	Int32 lotIdx = 1;

        //    DateTime dt = System.DateTime.Now;
        //    String sDate = dt.ToString( "yyyy/MM/dd" );//dt.FormatString( "yyyy/mm/dd" );
        //    String sTime = dt.ToString( "hh:nn" );//FormatString( "hh:nn" );
        //    Int32 nSeqNo = SeqNo;					// SeqNo,
        //    String sRackID = RackID;					// ラックID
        //    Int32 nRackPos = RackPos;				// ラック位置

        //    if ( !this.checkData() )
        //    {
        //        return false;
        //    }
        //    Boolean bLotCheck = true;

        //RetryFunc:

        //    // ロットMAX=3
        //    for ( Int32 i = 1; i <= MAX_CALIB_LOT; i++ )
        //    {

        //        if ( this.readHead( nProtoNo, i, 1 ) )
        //        {
        //            // ロットを探す
        //            if ( this.LotNo == sLotNo )
        //            {
        //                /////////
        //                // 追加
        //                /////////

        //                // 空きを探す
        //                for ( Int32 j = 1; j <= MAX_CALIB_DATA; j++ )
        //                {
        //                    if ( !this.readHead( nProtoNo, i, j ) )
        //                    {
        //                        StdID = sStdID;
        //                        aDate = sDate;
        //                        aTime = sTime;
        //                        SeqNo = nSeqNo;
        //                        RackID = sRackID;
        //                        RackPos = nRackPos;
        //                        StdID = sStdID;
        //                        this.writeTable( nProtoNo, i, j );
        //                        this.write0_3Abs( nProtoNo, i, j );
        //                        return true;
        //                    }
        //                }

        //                ////////////////////////////////////////
        //                // ここまできたら最古のデータを書き換え
        //                ////////////////////////////////////////

        //                this.readHead( nProtoNo, i, 1 );

        //                String tDateTime = this.dateTime;
        //                Int32 oldIdx = 1;
        //                for ( Int32 j = 2; j <= MAX_CALIB_DATA; j++ )
        //                {
        //                    if ( this.readHead( nProtoNo, i, j ) )
        //                    {
        //                        if ( String.CompareOrdinal( tDateTime, this.dateTime ) > 0 )
        //                        {
        //                            oldIdx = j;
        //                            tDateTime = this.dateTime;
        //                        }
        //                    }
        //                }

        //                StdID = sStdID;
        //                aDate = sDate;
        //                aTime = sTime;
        //                SeqNo = nSeqNo;
        //                RackID = sRackID;
        //                RackPos = nRackPos;
        //                StdID = sStdID;
        //                this.writeTable( nProtoNo, i, oldIdx );

        //                this.write0_3Abs( nProtoNo, i, oldIdx );

        //                return true;
        //            }
        //        }
        //    }

        //    /////////////////////
        //    // ここにきたら新規
        //    /////////////////////
        //    // ロットMAX=3
        //    for ( Int32 i = 1; i <= MAX_CALIB_LOT; i++ )
        //    {
        //        // 空きを探す
        //        if ( !this.readHead( nProtoNo, i, 1 ) )
        //        {
        //            LotNo = sLotNo;
        //            StdID = sStdID;
        //            aDate = sDate;
        //            aTime = sTime;
        //            SeqNo = nSeqNo;
        //            RackID = sRackID;
        //            RackPos = nRackPos;
        //            StdID = sStdID;
        //            this.writeTable( nProtoNo, i, 1 );

        //            this.write0_3Abs( nProtoNo, i, 1 );

        //            return true;
        //        }
        //    }

        //    if ( bLotCheck )
        //    {
        //        bLotCheck = false;

        //        // 古いロットを削除
        //        this.delOldLot( nProtoNo );

        //        goto RetryFunc;
        //    }


        //    return false;
        //}
        //---------------------------------------------------------------------------

        // 削除
        //public Boolean delData( Int32 nProtoNo, String aLotNo, String aDateTime )
        //{
        //    // ロットMAX=3
        //    for ( Int32 lot = 1; lot <= MAX_CALIB_LOT; lot++ )
        //    {

        //        for ( Int32 i = 1; i <= MAX_CALIB_DATA; i++ )
        //        { // データMAX=5
        //            if ( this.readHead( nProtoNo, lot, i ) )
        //            {

        //                // 一致？
        //                if ( aLotNo == this.LotNo &&
        //                    aDateTime == this.dateTime )
        //                {
        //                    this.delTable( nProtoNo, lot, i );
        //                    //this.delDatabase( SeqNo, aDate, RackID, RackPos );
        //                    //2008/6/6 追加
        //                    if ( i == 1 )
        //                    {
        //                        for ( Int32 j = 2; j <= MAX_CALIB_DATA; j++ )
        //                        {
        //                            if ( this.readTable( nProtoNo, lot, j ) )
        //                            {
        //                                this.writeTable( nProtoNo, lot, 1 );
        //                                this.delTable( nProtoNo, lot, j );
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    //                            

        //                    return true;
        //                }
        //            }
        //        }

        //    }
        //    return false;
        //}
        //---------------------------------------------------------------------------

        // 取得
        //Boolean getData( Int32 nProtoNo, String aLotNo )
        //{
        //    // ロットMAX=3
        //    for ( Int32 lot = 1; lot <= MAX_CALIB_LOT; lot++ )
        //    {

        //        // 最新日付を探す
        //        for ( Int32 n = 1; n <= MAX_CALIB_DATA; n++ )
        //        { // データMAX=5

        //            if ( this.readHead( nProtoNo, lot, n ) )
        //            {

        //                // ロットを探す
        //                if ( this.LotNo != aLotNo )
        //                {
        //                    break;
        //                }

        //                // 最新データを探す
        //                String aDateTime = this.dateTime;
        //                Int32 idx = n;

        //                // 最新日付を探す
        //                for ( Int32 i = 1; i <= MAX_CALIB_DATA; i++ )
        //                { // データMAX=5
        //                    if ( this.readHead( nProtoNo, lot, i ) )
        //                    {
        //                        if ( String.CompareOrdinal( aDateTime, this.dateTime ) < 0 )
        //                        {
        //                            aDateTime = this.dateTime;
        //                            idx = i;
        //                        }
        //                    }
        //                }
        //                return this.readTable( nProtoNo, lot, idx );
        //            }
        //        }
        //    }
        //    // マスターカーブ読み込み
        //    return this.readMaster( nProtoNo );
        //}



        // 検量線のロット、データ番号を取得
        //public Boolean getDataNo( ref Int32 lotNo, ref Int32 dataNo, Int32 nProtoNo, String aLotNo )
        //{
        //    // ロットMAX=3
        //    for ( Int32 lot = 1; lot <= MAX_CALIB_LOT; lot++ )
        //    {

        //        // 最新日付を探す
        //        for ( Int32 n = 1; n <= MAX_CALIB_DATA; n++ )
        //        { // データMAX=5

        //            if ( this.readHead( nProtoNo, lot, n ) )
        //            {

        //                // ロットを探す
        //                if ( this.LotNo != aLotNo )
        //                {
        //                    break;
        //                }

        //                // 最新データを探す
        //                String aDateTime = this.dateTime;
        //                Int32 idx = n;

        //                // 最新日付を探す
        //                for ( Int32 i = 1; i <= MAX_CALIB_DATA; i++ )
        //                { // データMAX=5
        //                    if ( this.readHead( nProtoNo, lot, i ) )
        //                    {
        //                        if ( String.CompareOrdinal( aDateTime, this.dateTime ) < 0 )
        //                        {
        //                            aDateTime = this.dateTime;
        //                            idx = i;
        //                        }
        //                    }
        //                }
        //                lotNo = lot;
        //                dataNo = idx;
        //                return true;
        //            }
        //        }
        //    }
        //    // マスターカーブ読み込み
        //    return false;
        //}

        //---------------------------------------------------------------------------

        // 更新
        //Boolean updateData( Int32 nProtoNo, String aLotNo, String aDateTime )
        //{
        //    if ( aLotNo == "MST" )
        //    {
        //        if ( this.readHead( nProtoNo, -1, -1 ) )
        //        {

        //            // 更新処理
        //            this.writeTable( nProtoNo, -1, -1 );
        //            return true;
        //        }
        //    }
        //    else
        //    {

        //        // ロットを探す
        //        for ( Int32 i = 1; i <= MAX_CALIB_LOT; i++ )
        //        {
        //            // 日付を探す
        //            for ( Int32 j = 1; j <= MAX_CALIB_DATA; j++ )
        //            {
        //                if ( this.readHead( nProtoNo, i, j ) )
        //                {

        //                    // ロット番号が一致
        //                    if ( aLotNo == this.LotNo &&
        //                        aDateTime == this.dateTime )
        //                    {

        //                        // 更新処理
        //                        this.writeTable( nProtoNo, i, j );
        //                        //this.updateDatabase( SeqNo, aDate, RackID, RackPos );
        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}


        //Boolean checkData()
        //{
        //    Boolean bError = true;
        //    if ( Count != 0 )
        //    {
        //        for ( Int32 i = 0; i < Count - 1; i++ )
        //        {
        //            // データの差があれば!
        //            if ( x[i] != x[i + 1] )
        //            {
        //                bError = false;
        //            }
        //        }
        //    }
        //    return !bError;
        //}



        // 指定したプロトコル、ロット番号の検量線が存在するかどうか。
        //public Boolean checkIfExistData( Int32 nProtoNo, String aLotNo )
        //{
        //    // ロットMAX=3
        //    for ( Int32 lot = 1; lot <= MAX_CALIB_LOT; lot++ )
        //    {
        //        for ( Int32 no = 1; no <= MAX_CALIB_DATA; ++no )
        //        {
        //            if ( this.readHead( nProtoNo, lot, no ) )
        //            {
        //                // ロットを探す
        //                if ( this.LotNo == aLotNo )
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}
        //---------------------------------------------------------------------------


        //double getMaxConc()
        //{
        //    return x[Count - 1];
        //}
        //------------


        // 2005.03.10 BB Miyazaki [] -.

        // 古いロットを削除する
        //void delOldLot( Int32 nProtoNo )
        //{
        //    String aDateTime = "";
        //    Int32 nLot = 0;

        //    // ロットMAX=3
        //    for ( Int32 lot = 1; lot <= MAX_CALIB_LOT; lot++ )
        //    {

        //        // 最古日付を探す
        //        for ( Int32 n = 1; n <= MAX_CALIB_DATA; n++ )
        //        { // データMAX=5

        //            if ( this.readHead( nProtoNo, lot, n ) )
        //            {

        //                if ( aDateTime == String.Empty )
        //                {
        //                    aDateTime = this.dateTime;
        //                    //2005/3/28 追加
        //                    nLot = lot;
        //                    //
        //                }

        //                if ( String.CompareOrdinal( aDateTime, this.dateTime ) > 0 )
        //                {
        //                    aDateTime = this.dateTime;
        //                    nLot = lot;
        //                }
        //            }
        //        }
        //    }

        //    // 削除
        //    for ( Int32 n = 1; n <= MAX_CALIB_DATA; n++ )
        //    { // データMAX=5
        //        if ( this.readHead( nProtoNo, nLot, n ) )
        //        {
        //            this.delTable( nProtoNo, nLot, n );

        //        }
        //    }
        //}



    }
#endif
}
