using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO.Ports;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Oelco.Common.Const;
using System.Drawing;
using Oelco.Common.Log;
using System.Collections;
using System.Security.AccessControl;
using System.Runtime.InteropServices;


namespace Oelco.Common.Utility
{
     
    /// <summary>
    /// 補助関数群クラス
    /// </summary>
    public static class SubFunction
    {
        private const Int32 WM_SYSCOMMAND = 274;
        private const UInt32 SC_CLOSE = 61536;
      //  private const Int32 SWP_NOSIZE = 0x1;
       // private const Int32 WM_SHOWWINDOW = 0x0018;
    

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int RegisterWindowMessage(string lpString);

		// 単体試験時、既存コード内にリソースの画像データを使用したインスタンス同士の比較処理が発見された為、
		// 対応を追加
		/// <summary>
		/// ImageData⇒バイト配列変換
		/// </summary>
		/// <param name="img">ImageData</param>
		/// <returns>バイト配列</returns>
		public static byte[] ConvertImageToBmpBytes( Image img )
		{
			// 入力引数の異常時のエラー処理
			if ( img == null )
			{
				return null;
			}

			// 返却用バイト型配列
			byte[] ImageBytes;


			// メモリストリームの生成
			using ( System.IO.MemoryStream ms = new System.IO.MemoryStream() )
			{
				// Image画像を、bmp形式でストリームに保存
				img.Save( ms, System.Drawing.Imaging.ImageFormat.Bmp );

				// ストリームのデーターをバイト型配列に変換
				ImageBytes = ms.ToArray();


				// ストリームのクローズ
				ms.Close();
			}

			return ImageBytes;
		}

		/// <summary>
		/// MD5ハッシュ取得
		/// </summary>
		/// <param name="data">画像データ</param>
		/// <returns>MD5ハッシュコード</returns>
		static public string GetMD5HashFromImageData( Image data )
		{
			return GetMD5Hash( ConvertImageToBmpBytes( data ) );
		}

		/// <summary>
		/// MD5ハッシュ取得
		/// </summary>
		/// <param name="data">対象データ</param>
		/// <returns>MD5ハッシュコード</returns>
		static public string GetMD5Hash( Byte[] data )
		{
			if ( data == null )
			{
				return String.Empty;
			}

			byte[] bs = null;

			//MD5CryptoServiceProviderオブジェクトを作成
			using ( System.Security.Cryptography.MD5CryptoServiceProvider md5 =
				new System.Security.Cryptography.MD5CryptoServiceProvider() )
			{
				//ハッシュ値を計算する
				bs = md5.ComputeHash( data );
			}

			//byte型配列を16進数の文字列に変換
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			foreach ( byte b in bs )
			{
				result.Append( b.ToString( "x2" ) );
			}

			return result.ToString();
		}

		/// <summary>
		/// 日付パース処理(YYMMDD形式）
		/// </summary>
		/// <remarks>
		/// 指定の文字列を、YYMMDDの形式でDateTime.TryParseExactを使ってパースします。西暦の上位2桁は現在日時から明示的に補間を行います。
		/// </remarks>
		/// <param name="srcStr">日付文字列(YYMMDD形式)</param>
		/// <param name="dstDateTime">日付設定対象</param>
		/// <returns>True:成功 False:失敗</returns>
		static public Boolean DateTimeTryParseExactForYYMMDD(String srcStr, out DateTime dstDateTime )
		{
			String yearTop = DateTime.Now.ToString( "yyyy" ).Remove( 2 );
			String yearDateTimeFormat = yearTop + "yyMMdd";

			return DateTime.TryParseExact( yearTop + srcStr, yearDateTimeFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out dstDateTime );
		}

        /// <summary>
        /// 日付パース処理(YYMMDDHHMMSS形式）
        /// </summary>
        /// <remarks>
        /// 指定の文字列を、YYMMDDの形式でDateTime.TryParseExactを使ってパースします。西暦の上位2桁は現在日時から明示的に補間を行います。
        /// </remarks>
        /// <param name="srcStr">日付文字列(YYMMDD形式)</param>
        /// <param name="dstDateTime">日付設定対象</param>
        /// <returns>True:成功 False:失敗</returns>
        static public Boolean DateTimeTryParseExactForYYMMDDHHMMSS( String srcStr, out DateTime dstDateTime )
        {
            String yearTop = DateTime.Now.ToString("yyyy").Remove(2);
            String yearDateTimeFormat = yearTop + "yyMMddHHmmss";

            return DateTime.TryParseExact(yearTop + srcStr, yearDateTimeFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out dstDateTime);
        }

        /// <summary>
        /// コレクション内容重複チェック
        /// </summary>
        /// <remarks>
        /// 対象のコレクションに対して、そのコレクション内容の特定データでの重複確認を行います。
        /// </remarks>
        /// <typeparam name="TCollection">対象型</typeparam>
        /// <typeparam name="TKey">対象型に対して比較値を抽出する処理</typeparam>
        /// <param name="collection">コレクションデータ</param>
        /// <param name="keySelector">対象型からの比較キー抽出処理</param>
        /// <returns>True:重複あり False:重複なし</returns>
        static public Boolean IsConflict<TCollection, TKey>( this ICollection<TCollection> collection, Func<TCollection, TKey> keySelector )
        {
            Boolean conflict = false;
            var selected = from v in collection
                           select keySelector( v );
            ArrayList list = new ArrayList();
            foreach ( var data in selected )
            {
                if ( !list.Contains( data ) )
                {
                    list.Add( data );
                }
                else
                {
                    conflict = true;
                    break;
                }
                //if ( selected.s( data ) )
                //{
                //    conflict = true;
                //    break;
                //}
            }

            return conflict;
        }

        /// <summary>
        /// Int32への変換処理
        /// </summary>
        /// <remarks>
        ///　引数値をInt32に変換した結果を返します。
        /// </remarks>
        /// <param name="valueString"></param>
        /// <returns></returns>
        static public Int32 SafeParseInt32( String valueString )
        {
            Int32 result = 0;
            try
            {
                result = Int32.Parse( valueString );
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine( "{0},{1}", ex.Message, ex.StackTrace );
            }
            return result;
        }
        /// <summary>
        /// Int32への変換処理
        /// </summary>
        /// <remarks>
        /// 引数値をInt32に変換した結果を返します。
        /// </remarks>
        /// <param name="valueData"></param>
        /// <returns></returns>
        static public Int32 SafeParseInt32( Object valueData )
        {
            Int32 result = 0;
            try
            {
                result = (Int32)( valueData );
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine( "{0},{1}", ex.Message, ex.StackTrace );
            }
            return result;
        }
        /// <summary>
        /// Doubleへの変換処理
        /// </summary>
        /// <remarks>
        /// 引数値をDoubleに変換した結果を返します。
        /// </remarks>
        /// <param name="valueString"></param>
        /// <returns></returns>
        static public Double SafeParseDouble( String valueString )
        {
            Double result = 0.0d;
            try
            {
                result = Double.Parse( valueString );
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine( "{0},{1}", ex.Message, ex.StackTrace );
            }
            return result;
        }

        /// <summary>
        /// メインフォーム取得
        /// </summary>
        /// <remarks>
        /// メインフォームを返します。
        /// </remarks>
        /// <returns></returns>
        static public Form GetMainForm()
        {
            List<Form> formList = new List<Form>();
            while(formList.Count == 0)
            {
                Win32API.EnumWindows((hWnd, lParam) =>
                {
                    UInt32 processId = 0;
                    UInt32 result = Win32API.GetWindowThreadProcessId(hWnd, ref processId);

                    if (processId == (UInt32)System.Diagnostics.Process.GetCurrentProcess().Id)
                    {
                        Form form = Form.FromHandle(hWnd) as Form;
                        if (form != null)
                        {
                            formList.Add(form);
                        }
                    }

                    return 1;
                }, 0);
            }
            return formList.First();
        }
        // 浮動小数点の指定桁数切捨て文字変換
        // 標準の変換では四捨五入されてしまうため作成。
        /// <summary>
        /// 浮動小数点の指定桁数切捨て文字変換
        /// </summary>
        /// <remarks>
        /// 浮動小数点の指定桁数切捨て文字変換を行います。
        /// </remarks>
        /// <param name="value"></param>
        /// <param name="numberDigits"></param>
        /// <returns></returns>
        static public String TruncateParse( Double value, Int32 numberDigits )
        {
            if ( numberDigits < 0 )
            {
                return "";
            }

            System.Globalization.NumberFormatInfo inf = new System.Globalization.NumberFormatInfo();
            inf.NumberDecimalDigits = numberDigits;
            Double db = Math.Pow( 10, numberDigits );
            Double valTmp = (Double)( Math.Truncate( ( (Decimal)value ) * (Decimal)db ) / (Decimal)db );
            return valTmp.ToString( "F", inf );
        }

        /// <summary>
        /// システムパスを取得
        /// </summary>
        /// <remarks>
        /// システムパスを返します。
        /// </remarks>
        /// <returns></returns>
        static public String GetApplicationDirectory()
        {
            String appDir = System.Windows.Forms.Application.ExecutablePath;
            appDir = System.IO.Path.GetDirectoryName( appDir ) + @"\";
            return appDir;
        }

        /// <summary>
        /// 対応地域文字列取得
        /// </summary>
        /// <remarks>
        /// 現在のカルチャに対応した地域文字列を返します。
        /// </remarks>
        /// <param name="supportRegions"></param>
        /// <returns></returns>
        static public String GetRegionName( String[] supportRegions = null )
        {
#if DEBUG
            // 開発段階ではスレッドの言語によって分岐する
            switch (Thread.CurrentThread.CurrentCulture.Name)
            {
                case "zh-CN":
                    return "CN";
                default:
                    return "US";
            }
#else
            String currentRegion = RegionInfo.CurrentRegion.TwoLetterISORegionName;

            // 対応地域文字を検索し、見つからない場合は対応地域文字配列の先頭を適用する。
            if ( supportRegions != null )
            {
                var searched = supportRegions.FirstOrDefault( ( ( str ) => currentRegion == str ) );
                if ( ( searched == null ) || ( searched.Count() == 0 ))
                {
                    currentRegion = supportRegions.First();
                }
            }

            return currentRegion;
#endif
        }

        /// <summary>
        /// 文字列よりEnum値を取得
        /// </summary>
        /// <remarks>
        /// 指定の列挙型に指定の文字列と同じ名前のメンバがある場合、その値を取得します。
        /// </remarks>
        /// <typeparam name="EnumType">列挙型</typeparam>
        /// <param name="enumString">Enumのメンバ名</param>
        /// <param name="result">取得したEnum値</param>
        /// <returns>取得結果(true:成功/false:失敗)</returns>
        static public Boolean EnumTryParseFromValueText<EnumType>( String enumString, out EnumType result )
        {
            Int32 value = 0;
            Boolean isNum = Int32.TryParse( enumString, out value );
            Boolean isSuccess = false;
            result = default( EnumType );
            if ( !isNum )
            {
                // 指定した文字列が数値化できない
                return isSuccess;
            }

            // Enum定義を列挙して一致を確認する。
            try
            {
                Array enumArray = Enum.GetValues( typeof( EnumType ) );
                foreach ( var val in enumArray )
                {
                    if ( (Int32)val == value )
                    {
                        result = (EnumType)val;
                        isSuccess = true;
                    }
                }
            }
            catch ( Exception )
            {
                // Enum型以外を指定して呼び出された。
            }

            return isSuccess;
        }

        /// <summary>
        /// COMポート生成待機
        /// </summary>
        /// <remarks>
        /// 指定のCOMポートが利用可能となるまで待機します(ドライバによるComの初期化待ち)。
        /// </remarks>
        /// <param name="portName">ポート名称</param>
        /// <returns>True:待機完了 False:タイムアウト</returns>
        static public Boolean WaitForInitComPort( String portName )
        {
            const Int32 WAIT_INIT_COM_TIME = 60;
            Boolean portFind = false;
            Int32 waitCount = 0;

            while ( !portFind )
            {
                String[] portNames = SerialPort.GetPortNames();

                foreach ( String name in portNames )
                {
                    // Comが存在する(大文字と小文字を区別しない)0:同じ 非0:異なる
                    if ( 0 == String.Compare( portName, name, true ) )
                    {
                        portFind = true;
                        break;
                    }
                }

                System.Threading.Thread.Sleep( 1000 );    // 1sec待機
                waitCount++;

                // タイムアウト60sec
                if ( waitCount > WAIT_INIT_COM_TIME )
                {
                    break;
                }
            }
            // ComNameがPortよりも先に生成されるため、waitを置く
            System.Threading.Thread.Sleep( 5000 );    // 1sec待機

            return portFind;
        }

        /// <summary>
        /// 数値切り捨て処理
        /// </summary>
        /// <remarks>
        /// 指定の精度の数値に切り捨てた値を返します。
        /// </remarks>
        /// <param name="value">丸め対象の倍精度浮動小数点数。</param>
        /// <param name="digits">戻り値の有効桁数の精度。</param>
        /// <returns>iDigits に等しい精度の数値に切り捨てられた数値。</returns>
        static public Double ToRoundDown( Double value, Int32 digits )
        {
            Double coef = System.Math.Pow( 10, digits );

            return value > 0 ? System.Math.Floor( value * coef ) / coef :
                                System.Math.Ceiling( value * coef ) / coef;
        }

        /// <summary>
        /// アプリケーション初期化処理
        /// </summary>
        /// <remarks>
        /// アプリケーションの初期化処理を行います。
        /// </remarks>
        public static void ApplicationInitialize()
        {
            // タスクバーの表示非表示切り替え
            Int32 hWnd = Win32API.FindWindow( "Shell_TrayWnd", null );
            Int32 hWndButton = Win32API.FindWindow( "Button", null );

            Action<Boolean> hideTaskBar = ( flg ) =>
            {
                if ( hWnd != 0 )
                {

                    Win32API.ShowWindow( hWnd, flg ? 0 : 5 );

                    if ( hWndButton != 0 )
                    {
                        Win32API.ShowWindow( hWndButton, flg ? 0 : 5 );
                    }
                }
            };

            //Application.ThreadException += ( sender, e ) => hideTaskBar( false );
            Thread.GetDomain().ProcessExit += ( sender, e ) => hideTaskBar( false );
            //Thread.GetDomain().UnhandledException += ( sender, e ) => hideTaskBar( false );

#if !DEBUG_CARIS
            hideTaskBar( true );
#endif
        }

        /// <summary>
        /// ファイル保存ダイアログの表示
        /// </summary>
        /// <remarks>
        /// ファイル保存ダイアログを表示します。
        /// </remarks>
        /// <param name="selectFileName">指定されたファイル名(フルパス)</param>
        /// <param name="initFileName"></param>
        /// <param name="summaryName"></param>
        /// <returns></returns>
        public static DialogResult ShowSaveCSVFileDialog( out String selectFileName, OutputFileKind fileKind, String initDirectory, String initFileName, String summaryName )
        {
            selectFileName = null;

            // 初期表示ファイル名を指定
            if ( initFileName != null )
            {
                Singleton<SaveFileDialog>.Instance.FileName = initFileName;
            }
            else
            {
                Singleton<SaveFileDialog>.Instance.FileName = "*";
            }

            // 初期表示ディレクトリを指定
            Singleton<SaveFileDialog>.Instance.InitialDirectory = initDirectory;

            // [ファイルの種類]に表示される選択肢を指定
            Singleton<SaveFileDialog>.Instance.Filter = String.Join( "|", ( fileKind.ToTypeString().Select( ( kind ) => String.Format( @"(*{0})|*{0}", kind ) ) ) );

            // タイトルを設定する
            Singleton<SaveFileDialog>.Instance.Title = summaryName + " " + Common.Properties.Resources.STRING_DLG_SAVEFILE_001;

            DialogResult result = Singleton<SaveFileDialog>.Instance.ShowDialog();
            if ( result == DialogResult.OK )
            {
                selectFileName = Singleton<SaveFileDialog>.Instance.FileName;
            }

            return result;
        }

        /// <summary>
        /// ファイルを開くダイアログの表示
        /// </summary>
        /// <remarks>
        /// ファイルを開くダイアログを表示します。
        /// </remarks>
        /// <param name="selectFileName">指定されたファイル名(フルパス)</param>
        /// <param name="initFileName"></param>
        /// <param name="summaryName"></param>
        /// <returns></returns>
        public static DialogResult ShowOpenFileDialog( out String selectFileName, OutputFileKind fileKind, String initDirectory, String initFileName, String summaryName )
        {
            selectFileName = null;

            // 初期表示ファイル名を指定
            if ( initFileName != null )
            {
                Singleton<OpenFileDialog>.Instance.FileName = initFileName;
            }
            else
            {
                Singleton<OpenFileDialog>.Instance.FileName = "*";
            }

            // 初期表示ディレクトリを指定
            Singleton<SaveFileDialog>.Instance.InitialDirectory = initDirectory;

            // [ファイルの種類]に表示される選択肢を指定
            Singleton<OpenFileDialog>.Instance.Filter = String.Join( "|", ( fileKind.ToTypeString().Select( ( kind ) => String.Format( @"(*{0})|*{0}", kind ) ) ) );

            // タイトルを設定する
            Singleton<OpenFileDialog>.Instance.Title = summaryName + " " + Common.Properties.Resources.STRING_DLG_SAVEFILE_001;

            DialogResult result = Singleton<OpenFileDialog>.Instance.ShowDialog();
            if ( result == DialogResult.OK )
            {
                selectFileName = Singleton<OpenFileDialog>.Instance.FileName;
            }

            return result;
        }

#if DEBUG_CARIS

        static CultureInfo cultureInfo;

        /// <summary>
        /// デバッグ用
        /// </summary>
        public static Action DebugStartJapanese = () =>
        {
            cultureInfo = System.Threading.Thread.CurrentThread.CurrentUICulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.GetCultureInfo( "ja-JP" ).Clone();
        };

        /// <summary>
        /// デバッグ用
        /// </summary>
        public static Action DebugEndJapanese = () =>
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
        };
#endif

        /// <summary>
        /// コントロールのイメージを取得する
        /// </summary>
        /// <remarks>
        /// コントロールのイメージを返します。
        /// </remarks>
        /// <param name="ctrl">キャプチャするコントロール</param>
        /// <returns>取得できたイメージ</returns>
        public static Bitmap CaptureControl( Control ctrl )
        {
            Bitmap img = new Bitmap( ctrl.Width, ctrl.Height );
            Graphics memg = Graphics.FromImage( img );
            IntPtr dc = memg.GetHdc();
            Win32API.PrintWindow( ctrl.Handle, dc, 0 );
            memg.ReleaseHdc( dc );
            memg.Dispose();
            return img;
        }

        /// <summary>
        /// 標準偏差算出
        /// </summary>
        /// <remarks>
        /// 標準偏差を算出します。
        /// </remarks>
        /// <param name="data">全標本</param>
        /// <returns></returns>
        public static Double? GetSD( this IEnumerable<Double?> data, Boolean roundOffAveInt = false )
        {
            try
            {
                return data.Where( ( item ) => item.HasValue ).Cast<Double>().GetSD( roundOffAveInt );
            }
            catch ( Exception )
            {
                return null;
            }
        }

        /// <summary>
        /// 標準偏差算出
        /// </summary>
        /// <remarks>
        /// 標準偏差を算出します。
        /// </remarks>
        /// <param name="data">全標本</param>
        /// <returns>標準偏差値</returns>
        public static Double GetSD( this IEnumerable<Double> data, Boolean roundOffAveInt = false )
        {
            Double average;
            return data.GetSD( out average, roundOffAveInt );
        }

        /// <summary>
        /// 標準偏差算出
        /// </summary>
        /// <remarks>
        /// 標準偏差を算出します。
        /// </remarks>
        /// <param name="data">全標本</param>
        /// <param name="average">平均</param>
        /// <param name="averageInteger">平均の整数化(四捨五入)</param>
        /// <returns>標準偏差値</returns>
        public static Double GetSD( this IEnumerable<Double> data, out Double average, Boolean roundOffAveInt = false )
        {
            average = 0;
            if ( data != null )
            {
                Decimal numOfUseData = data.Count();
                // change by marxsu
                //if ( numOfUseData > 0 )
                if (numOfUseData > 1)
                {
                    try
                    {
                        var ave = roundOffAveInt ? Math.Round( data.Average(), MidpointRounding.AwayFromZero ) : data.Average();
                        var dataSum = data.Sum( ( item ) => Math.Pow( ( (Double)( (Decimal)item - (Decimal)ave ) ), 2 ) );
                        average = ave;

                        return Math.Sqrt( (Double)( (Decimal)dataSum / (Decimal)( numOfUseData - 1 ) ) );
                    }
                    catch ( Exception  ex )
                    {
                        //Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, "SubFunction.GetSD Exception!" );
                        Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("SubFunction.GetSD Exception!Message = {0} StackTrace = {1}", ex.Message, ex.StackTrace));
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 変動係数算出
        /// </summary>
        /// <remarks>
        /// 変動係数を算出します。
        /// </remarks>
        /// <param name="data">全標本</param>
        /// <param name="percent">百分率で取得</param>
        /// <returns>変動係数</returns>
        public static Double GetCV( this IEnumerable<Double> data, Boolean percent = false, Boolean roundOffAveInt = false )
        {
            Double sd;
            return GetCV( data, out sd, percent, roundOffAveInt );
        }

        /// <summary>
        /// 変動係数算出
        /// </summary>
        /// <remarks>
        /// 変動係数を算出します。
        /// </remarks>
        /// <param name="data">全標本</param>
        /// <param name="sd">標準偏差値</param>
        /// <param name="percent">百分率で取得</param>
        /// <returns>変動係数</returns>
        public static Double GetCV( this IEnumerable<Double> data, out Double sd, Boolean percent = false, Boolean roundOffAveInt = false )
        {
            Double average;
            return GetCV( data, out sd, out average, percent, roundOffAveInt );
        }

        /// <summary>
        /// 変動係数算出
        /// </summary>
        /// <remarks>
        /// 変動係数を算出します。
        /// </remarks>
        /// <param name="data">全標本</param>
        /// <param name="sd">標準偏差値</param>
        /// <param name="average">平均(四捨五入)</param>
        /// <param name="percent">百分率で取得</param>
        /// <returns>変動係数</returns>
        public static Double GetCV( this IEnumerable<Double> data, out Double sd, out Double average, Boolean percent = false, Boolean roundOffAveInt = false )
        {
            sd = GetSD( data, out average, roundOffAveInt );
            return GetCV( sd, average, percent );
        }

        

        /// <summary>
        /// 変動係数算出
        /// </summary>
        /// <remarks>
        /// 変動係数を算出します。
        /// </remarks>
        /// <param name="sd">標準偏差</param>
        /// <param name="average">平均値</param>
        /// <param name="percent">変動係数の百分率取得</param>
        /// <returns>変動係数</returns>
        public static Double GetCV( Double sd, Double average, Boolean percent = false )
        {
            if ( sd >= 0 && average > 0 )
            {
                var buff = (Decimal)sd * ( ( percent ) ? 100m : 1m );
                return (Double)( buff / (Decimal)average );
            }
            return 0;
        }

        /// <summary>
        /// 四捨五入文字列変換
        /// </summary>
        /// <remarks>
        /// 引数値を四捨五入した値を文字列で返します。
        /// </remarks>
        /// <param name="value">四捨五入対象値</param>
        /// <param name="digits">少数桁数</param>
        public static String ToRoundOffParse( Double value, Int32 digits )
        {
            return Math.Round( value, digits, MidpointRounding.AwayFromZero ).ToString( "F", new System.Globalization.NumberFormatInfo()
            {
                NumberDecimalDigits = digits
            } );
        }        

        /// <summary>
        /// TabletTipレジストリ更新
        /// </summary>
        /// <remarks>
        /// TabletPC入力パネルの使用/不使用を設定する為、レジストリを更新します。
        /// 引数値とレジストリ値に相違がある場合のみ更新します。
        /// </remarks>
        /// <param name="enable">TabletPC入力パネル使用/不使用</param>
        /// <returns></returns>
        public static Boolean UpdateTabletTipRegistry(String keyName , Boolean enable)
        {
            Boolean rtn = true;

            // Windows7以外の場合は処理をしない
            System.OperatingSystem os = System.Environment.OSVersion;
            if ( (os.Platform == System.PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 1)
              || (os.Platform == System.PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 2) )
            {
                // We should get the write Rights
                string user = Environment.UserDomainName + "\\" + Environment.UserName;
                System.Security.AccessControl.RegistrySecurity rs = new System.Security.AccessControl.RegistrySecurity();
                rs.AddAccessRule(new RegistryAccessRule(user,
                   RegistryRights.WriteKey,
                   InheritanceFlags.None,
                   PropagationFlags.None,
                   AccessControlType.Allow));
                // 操作するレジストリ・キーの名前
                String rKeyName = GlobalConst.TABLETTIP_KEYNAME;

                // 設定処理を行う対象となるレジストリの値の名前
                String rSetValueName = keyName;

                // 設定する値のデータ
                Int32 newValue = Convert.ToInt32(enable);  // REG_DWORD型

                // レジストリの設定と削除
                try
                {
                  
                   // Microsoft.Win32.RegistryKey rKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(rKeyName);
                   // if (rKey ==null)
                    //{
                         // レジストリ・キーを新規作成して開く
                        // rKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey( rKeyName );
                       Microsoft.Win32.RegistryKey rKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(rKeyName, Microsoft.Win32.RegistryKeyPermissionCheck.Default, rs);
                    //}
                   

                    // レジストリの値を取得する。
                    var val = rKey.GetValue( rSetValueName );
                    // レジストリの値が設定値と違っていたらレジストリを書き換える
                    if (val == null || !val.Equals(newValue) )
                    {
                        rKey.SetValue( rSetValueName, newValue );

                        // 開いたレジストリを閉じる
                        rKey.Close();
                        

                        // サービス再起動
                        RebootTabletInputService(enable);
                    }

                  //rs.AddAccessRule(new RegistryAccessRule(user,
                  //RegistryRights.WriteKey,
                  //InheritanceFlags.None,
                  //PropagationFlags.None,
                  //AccessControlType.Deny));
                  //rKey.SetAccessControl(rs);
                }
                catch ( Exception ex )
                {
                    Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, ex.Message + " " + ex.StackTrace );
                }
            }

            return rtn;
        }

        /// <summary>
        /// 开启软件盘
        /// </summary>
        private static void StartTabletProcess()
        {
            dynamic file = "C:\\Program Files\\Common Files\\microsoft shared\\ink\\TabTip.exe";
            //if (!System.IO.File.Exists(file))
            //    return -1;
            System.Diagnostics.Process.Start(file);
            //  return SetUnDock();
            System.Threading.Thread.Sleep(6500);
            IntPtr TouchWnd = new IntPtr(0);
            TouchWnd = FindWindow("IPTip_Main_Window", null);//
            if (TouchWnd != null)
            {
               // PostMessage(TouchWnd, WM_SHOWWINDOW, 1, 0);
                PostMessage(TouchWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
               // SetWindowPos(TouchWnd, 1, 0, 0, 0, 0, SWP_NOSIZE);
               // PostMessage(h,wm_hide,0,); 
                
            }           
        }

        /// <summary>
        /// 是否打开软件盘
        /// </summary>
        /// <param name="bShow"></param>
        public static void StartTablet(Boolean bShow)
        {
            if (!bShow)
            {
                return;
            }
            //the Tablet is running
            IntPtr TouchhWnd = new IntPtr(0);
            TouchhWnd = FindWindow("IPTip_Main_Window", null);
            if (TouchhWnd != IntPtr.Zero)
            {
                return;
            }

            StartTabletProcess();
        }

        /// <summary>
        /// TabletInputService再起動
        /// </summary>
        /// <remarks>
        /// TabletInputServiceサービスの再起動を行います。
        /// </remarks>
        private static void RebootTabletInputService(Boolean bShowTablet)
        {
            try
            {
                System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController( GlobalConst.TABLET_SERVICE_NAME );

                // 停止できるか調べる
                if (!bShowTablet && sc.CanStop)
                {
                    //サービスを停止する
                    sc.Stop();
                    //サービスが停止するまで待機する
                    sc.WaitForStatus( System.ServiceProcess.ServiceControllerStatus.Stopped );
                }
                else if (bShowTablet && sc.Status == System.ServiceProcess.ServiceControllerStatus.Stopped )
                {
                    //サービスが停止していれば、開始する
                    sc.Start();
                    //サービスが開始されるまで待機する
                    sc.WaitForStatus( System.ServiceProcess.ServiceControllerStatus.Running );

                    Thread threadStartTabletInput = new Thread(new ThreadStart(StartTabletProcess));
                    threadStartTabletInput.Start();                
                  
                }
                sc.Close();
                sc.Dispose();
            }
            catch ( Exception ex )
            {
                Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, ex.Message + " " + ex.StackTrace );
            }
        }
    }
}
