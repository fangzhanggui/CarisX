using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Oelco.CarisX.Log;
using Oelco.Common.Utility;
using Oelco.CarisX.Const;
using System.Threading;
using Oelco.CarisX.Utility;
using System.Runtime.InteropServices;
using System.Reflection;
using Oelco.CarisX.GUI;
using Oelco.CarisX.Parameter;
using Oelco.Common.Parameter;

namespace Oelco.CarisX
{
    /// <summary>
    /// アプリケーションメインクラス
    /// </summary>
    /// <remarks>
    /// アプリケーションはこのクラスから開始されます。
    /// </remarks>
    public static class Program
    {
        #region [外部関数]
        #endregion

        #region [定数定義]

        #endregion

        #region [クラス変数定義]
        
        #endregion

        #region [インスタンス変数定義]
        // アプリケーション固定名
        private static string strAppConstName = @"Global\CarisX";
        // 多重起動を禁止するミューテックス
        private static Mutex mutexObject;
        #endregion

        #region [コンストラクタ/デストラクタ]

        #endregion

        #region [プロパティ]

        #endregion

        #region [publicメソッド]

        #endregion

        #region [protectedメソッド]

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 多重起動チェック
            bool startApp = true;   
            try
            {
                // ミューテックスを生成する
                mutexObject = new Mutex( false, strAppConstName );
            }
            catch ( Exception )
            {                
                startApp = false;               
            }
            // ミューテックスを取得する
            if (!mutexObject.WaitOne( 0, false ))           
            {
                startApp = false;                
            }           
            // 多重起動エラーの場合、エラーメッセージを表示し、アプリ終了
            if ( !startApp )
            {               
                Oelco.CarisX.GUI.DlgMessage.Show( CarisX.Properties.Resources.STRING_COMMON_018, String.Empty, CarisX.Properties.Resources.STRING_DLG_TITLE_001, Oelco.CarisX.GUI.MessageDialogButtons.OK ); 
                return;
            }
        
            Application.EnableVisualStyles();
            Application.AddMessageFilter(new MessageFilter());
            Application.SetCompatibleTextRenderingDefault( false );
            Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Load();
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo == CompanyLogoParameter.CompanyLogoKind.LogoDefault)
            {
                DlgCheckCompanyLogo checkCompanyLogo = new DlgCheckCompanyLogo();
                checkCompanyLogo.ShowDialog();
            }
           
            // ログクラス初期化//日志条数从100000=〉50000（较少日志文件大小）
            Singleton<CarisXLogManager>.Instance.Initialize( CarisXConst.PathDebug,"debuglog", 50000, 10, 100, true, new TimeSpan(30,0,0,0));

            // 共通クラス内部ログ出力設定
            Singleton<CarisXLogManager>.Instance.SetInstance();
//            Marshal.GetHINSTANCE
#if !DEBUG
            // ThreadExceptionイベント・ハンドラを登録する
            Application.ThreadException += threadExceptionEventHandler;
            // UnhandledExceptionイベント・ハンドラを登録する
            Thread.GetDomain().UnhandledException += unhandledExceptionHandler;

            // 右クリックを無効化する
            mouseHookHandle = setHook(); // マウスフック設定
#endif
            mainWindow = new Oelco.CarisX.GUI.FormMainFrame();
            try
            {
                Application.Run(mainWindow);
            }
            catch (Exception ex)
            {
                // ログ出力
                Singleton<CarisXLogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("An exception occurred in ApplicationRun : {0}", ex.StackTrace));
            }
            finally
            {
                removeHook();
                // ミューテックスを解放する
                mutexObject.ReleaseMutex();
                // ミューテックスを破棄する
                mutexObject.Close();
            }
        }
        static private Form mainWindow = null;

        private static Oelco.Common.Utility.Win32API.LowLevelMouseProc mouseProc = MouseHookProc;

        static public IntPtr setHook()
        {
            
            IntPtr hHook = Win32API.SetWindowsHookEx( Win32API.HookType.WH_MOUSE_LL,
                mouseProc,
                // Marshal.GetHINSTANCEのものは、デバッガ上から実行した場合、正確な値がとれない為APIを利用する。
                Win32API.GetModuleHandle(null),//Marshal.GetHINSTANCE( Assembly.GetExecutingAssembly().GetModules()[0] ),
                0 );
            if ( hHook == IntPtr.Zero )
            {
                int errorCode = Marshal.GetLastWin32Error();
            }

//            IntPtr hHook = Win32API.SetWindowsHookEx( Win32API.HookType.WH_MOUSE_LL, MouseHookProc, Marshal.GetHINSTANCE( Assembly.GetExecutingAssembly().GetModules()[0] ), 0 );
            return hHook;
        }
        static public void removeHook()
        {
            if ( mouseHookHandle != IntPtr.Zero )
            {
                Win32API.UnhookWindowsHookEx( mouseHookHandle );
            }
        }
        static IntPtr MouseHookProc( int code, Oelco.Common.Utility.Win32API.WindowsMessages wParam, ref Oelco.Common.Utility.Win32API.MSLLHOOKSTRUCT lParam )
        {
            // 右クリック無効化
            // TODO:メインフレームの表示座標内に入っていたら無効化する
            switch ( wParam )
            {
            case Win32API.WindowsMessages.RBUTTONDOWN:
            case Win32API.WindowsMessages.RBUTTONDBLCLK:
            case Win32API.WindowsMessages.RBUTTONUP:
                {

                    System.Drawing.Rectangle clientRect = mainWindow.RectangleToScreen( mainWindow.ClientRectangle );
                    if ( ( Form.ActiveForm != null)  && ( lParam.point.X >= clientRect.Left && lParam.point.X <= clientRect.Right ) &&
                        ( lParam.point.Y >= clientRect.Top && lParam.point.Y <= clientRect.Bottom ) )
                    {
                        return (IntPtr)1;
                    }
                    break;
                }
            }
            return Win32API.CallNextHookEx( mouseHookHandle, code, (IntPtr)wParam, ref lParam );
        }
        static IntPtr mouseHookHandle = IntPtr.Zero;
//        static IntPtr MouseHookProc( int code, IntPtr wParam, ref IntPtr lParam )
//        {
//            // TODO:右クリックを無効化する WindowHandleから所属プロセス見て自分自身なら無効にする
//            return Win32API.CallNextHookEx( mouseHookHandle, code, wParam, lParam );
////            return 0;
//        }

#if !DEBUG
        static void threadExceptionEventHandler(object sender, ThreadExceptionEventArgs e)
        {
            Singleton<CarisXLogManager>.Instance.Write(Oelco.Common.Log.LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                String.Format("ended exception in the main thread is not caught。 Message={0} StackTrace={1} ", e.Exception.Message, e.Exception.StackTrace));
            //modified by zhangdong 2013_4_12 begin ,cause if have some excetions happened ,the LogManager should not be unloaded
            //Singleton<CarisXLogManager>.Instance.Dispose();
            //modified by zhangdong 2013_4_12 end
        }
        public static void unhandledExceptionHandler( object sender, UnhandledExceptionEventArgs e )
        {
            Exception ex = e.ExceptionObject as Exception;
            if ( ex != null )
            {
                Singleton<CarisXLogManager>.Instance.Write( Oelco.Common.Log.LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    String.Format("Uncaught exception occurred。 Message={0} StackTrace={1}", ex.Message, ex.StackTrace));
            }
            else
            {
                Singleton<CarisXLogManager>.Instance.Write( Oelco.Common.Log.LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID,
                    String.Format("Uncaught exception occurred。 Message=UNKNOWN"));
            }
        }
#endif
#endregion
   
    public class MessageFilter : IMessageFilter 
    {
       
        public bool PreFilterMessage(ref Message m) 
        {
            if (m.Msg == 0x0101 && m.WParam.ToInt32() == 0x0070) 
            {

                CarisX.Const.HelpDocument.openHelpDocumentPage();
            }
            return false;
        }
      }
    }
}
