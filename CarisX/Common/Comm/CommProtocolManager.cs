using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Utility;
using Oelco.Common.Log;

namespace Oelco.Common.Comm
{
    /// <summary>
    /// コマンド送信結果
    /// </summary>
    /// <param name="retCode"></param>
    /// <param name="cmd"></param>
    public delegate void CommSendResult( Int32 retCode, CommCommand cmd );

    /// <summary>
    /// 通信プロトコル管理
    /// </summary>
    /// <remarks>
    /// 通信インスタンス及び手順の管理を行います。
    /// </remarks>
    public class CommProtocolManager
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 通信インターフェース
        /// </summary>
        private IComm commObject = null;

        /// <summary>
        /// コマンド解析インターフェース
        /// </summary>
        private ICommCommandAnalyser commandAnalyser = null;

        /// <summary>
        /// 通信番号
        /// </summary>
        private Int32 commNo = 0;

        /// <summary>
        /// コマンド送信結果変数
        /// </summary>
        protected CommSendResult dlgCommSendResult;

		/// <summary>
		/// 待機イベント
		/// </summary>
		/// <remarks>
		/// 設定されたイベントが存在する場合、通信処理を停止すべき状態であることを利用側から判断を行うことができます。
		/// </remarks>
		private System.Threading.ManualResetEvent sleepEvent = null;

		/// <summary>
		/// 待機イベント排他オブジェクト
		/// </summary>
		private System.Threading.Mutex sleepEventMutex = new System.Threading.Mutex();

		/// <summary>
		/// 待機イベント利用数
		/// </summary>
		/// <remarks>
		/// 設定されたイベントが使用されている数です、
		/// 待機イベントを利用し、解除された際、この値が0である場合に待機イベントをこのインスタンスから削除します。
		/// </remarks>
		protected Int32 sleepEventUseCount = 0;

        #endregion

        #region [プロパティ]

		/// <summary>
		/// 待機イベント
		/// </summary>
		/// <remarks>
		/// 設定されたイベントが存在する場合、通信処理を停止します。
		/// 設定を行う際、既に設定されたイベントが解除されるまで、このプロパティを呼び出したスレッドはブロックされます。
		/// </remarks>
		public System.Threading.ManualResetEvent SleepEvent
		{
			get
			{
				return this.sleepEvent;
			}
			set
			{
				while ( this.sleepEvent != null )
				{
					System.Threading.Thread.Sleep( 10 );
				}
				this.sleepEvent = value;
			}
		}

        /// <summary>
        /// 接続状態の取得
        /// </summary>
        public ConnectionStatus CommStatus
        {
            get
            {
                ConnectionStatus result = ConnectionStatus.Error;
                if ( this.commObject != null )
                {
                    // インターフェース経由でパラメータ設定
                    result = this.commObject.ConnectionStatus;
                }
                return result;
            }
        }

        /// <summary>
        /// ログの取得
        /// </summary>
        public List<String> Log
        {
            get;
            protected set;
        }

        /// <summary>
        /// 通信インターフェース 取得/設定
        /// </summary>
        public IComm CommObject
        {
            get
            {
                return this.commObject;
            }
            set
            {
                this.commObject = value;
            }
        }

        /// <summary>
        /// コマンド解析インターフェース 取得/設定
        /// </summary>
        public ICommCommandAnalyser CommandAnalyser
        {
            get
            {
                return commandAnalyser;
            }
            set
            {
                commandAnalyser = value;
            }
        }
        
        /// <summary>
        /// 通信番号 取得/設定
        /// </summary>
        public Int32 CommNo
        {
            get
            {
                return commNo;
            }
            set
            {
                commNo = value;
            }
        }

        #endregion

        #region [publicメソッド]


		/// <summary>
		/// イベント待機
		/// </summary>
		/// <remarks>
		/// 待機イベントが設定されている場合、この関数を呼び出したスレッドはイベントのシグナルを待機します。
		/// </remarks>
		public virtual void WaitEventEnd()
		{
			if ( this.sleepEvent != null )
			{
				this.sleepEventUseCount++;
				this.sleepEventMutex.WaitOne();
				if ( this.sleepEvent == null )
				{
					return;
				}
				this.sleepEvent.WaitOne();
				this.sleepEventMutex.ReleaseMutex();
				this.sleepEventUseCount--;

				if ( this.sleepEventUseCount == 0 )
				{
					this.sleepEventMutex.WaitOne();
					this.sleepEvent = null;
					this.sleepEventMutex.ReleaseMutex();
					this.sleepEventUseCount = 0;
				}
			}
		}

        /// <summary>
        /// パラメータ設定
        /// </summary>
        /// <remarks>
        /// 保持する通信インターフェースを通して、
        /// 接続に関するパラメータを設定します。
        /// </remarks>
        /// <param name="parameter">接続パラメータ</param>
        public virtual void SetParameter( Object parameter )
        {
            if ( this.commObject != null )
            {
                // インターフェース経由でパラメータ設定
                this.commObject.SetConnectParam( parameter );
            }
        }

        /// <summary>
        /// 接続
        /// </summary>
        /// <remarks>
        /// 保持する通信インターフェースを通して、
        /// 接続を行います。
        /// </remarks>
        /// <param name="parameter">接続パラメータ</param>
        /// <returns>True:成功 False:失敗</returns>
        public virtual Boolean Connect( Object parameter )
        {
            Boolean openSuccess = false;

            if ( this.commObject != null )
            {
                // インターフェース経由でパラメータ設定
                this.commObject.SetConnectParam( parameter );

                // 接続試行
                openSuccess = this.commObject.Open();
            }

            return openSuccess;
        }

        /// <summary>
        /// 切断
        /// </summary>
        /// <remarks>
        /// 保持する通信インターフェースを通して、
        /// 切断を行います。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public virtual Boolean DisConnect()
        {
            Boolean closeSuccess = false;

            if ( this.commObject != null )
            {
                closeSuccess = this.commObject.Close();
            }

            return closeSuccess;
        }

        /// <summary>
        /// コマンド送信結果コールバック登録
        /// </summary>
        /// <remarks>
        /// コマンド送信結果をメンバに設定します。
        /// </remarks>
        /// <param name="commSendResult">送信結果結果デリゲート</param>
        public virtual void SetCommSendResultCallBack( CommSendResult commSendResult )
        {
            this.dlgCommSendResult = commSendResult;
        }

        /// <summary>
        /// コマンド送信結果コールバック解除
        /// </summary>
        /// <remarks>
        /// コマンド送信結果をクリアします。
        /// </remarks>
        public virtual void ClearCommSendResultCallBack()
        {
            this.dlgCommSendResult = null;
        }

        /// <summary>
        /// 送信
        /// </summary>
        /// <remarks>
        /// コマンドデータをテキスト化し、送信します。
        /// </remarks>
        /// <param name="command">送信コマンド</param>
        /// <returns>True:成功 False:失敗</returns>
        public virtual Boolean Send( CommCommand command )
        {
            Boolean sendSuccess = false;

            if ( this.commObject != null )
            {
                Int32 ret = 0;
                try
                {
                    // コマンドID取得
                    ret = this.commObject.SendText( command.CommandId.ToString( "d4" ) + command.CommandText );
                }
                catch ( Exception ex )
                {
                    Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, String.Format( "*Command Send Error![{3}] Type = {0} Message = {1} StackTrace = {2}", command == null ? "unknown" : command.GetType().Name, ex.Message, ex.StackTrace, CommNo ) );
                    ret = -1;   // 例外発生時は送信異常（-1）とする
                }
                if ( ret > 0 )
                {
                    sendSuccess = true;
                }

                // TODO:「０」：未接続、「-1」：送信異常、「-2」タイムアウト異常、「-3」：スレッド終了、「-4」パラメータ異常 の場合、エラー表示する。
                // TODO:ユーザープログラムのみを起動した場合は、コマンド送信時エラーが発生してもエラー表示は行なわない。
                //（G1200では接続確認コマンドが送信できた場合のみ、装置が起動していると認識し、エラー表示をするように行なっていたと思います。）
                // →今回の装置の起動状態は通信Libの接続状態を見ることで判定
                Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, String.Format( "*{3}Command Sended[{4}] : Type = {0} success = {1} detail = {2}", command == null ? "unknown" : command.GetType().Name, sendSuccess.ToString(), ret.ToString(), DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss" ), CommNo ) );
                System.Diagnostics.Debug.WriteLine( String.Format( "*{3}Command Sended[{4}] : Type = {0} success = {1} detail = {2}", command == null ? "unknown" : command.GetType().Name, sendSuccess.ToString(), ret.ToString(), DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss" ), CommNo) );
                this.commandReflection( command );

                // コマンド送信結果コールバック呼び出し
                if ( dlgCommSendResult != null )
                {
                    dlgCommSendResult( ret, command );
                }
            }

            return sendSuccess;
        }

        /// <summary>
        /// 受信
        /// </summary>
        /// <remarks>
        /// 受信したテキストデータをコマンドデータとして受け取ります。
        /// </remarks>
        /// <param name="command">コマンドデータ取得先</param>
        /// <returns>True:成功 False:失敗</returns>
        public virtual Boolean Receive( out CommCommand command )
        {
            Boolean recvSuccess = false;
            command = null;

            if ( this.commObject != null )
            {
                String strRecvBuff;
                Int32 ret = this.commObject.RecvText( out strRecvBuff );
                if ( ret > 0 )
                {
                    recvSuccess = true;
                }

                // 解析する
                if ( recvSuccess )
                {

                    command = this.commandAnalyser.AnalyseCommand( strRecvBuff );

                    // コマンド解析失敗なら受信失敗扱い。
                    if ( command == null )
                    {
                        // TODO:コマンド受信時に、該当のコマンドが存在しない場合はエラー表示する。
                        Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, String.Format( "*{1}Command analyse failed[{2}] : String = {0}", strRecvBuff, DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss" ), CommNo) );
                        recvSuccess = false;
                    }
                    else
                    {
                        Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, String.Format( "*{1}Command Received[{3}] : Type = {0} Text = {2}", command == null ? "unknown" : command.GetType().Name, DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss" ), strRecvBuff, CommNo) );
                        System.Diagnostics.Debug.WriteLine( String.Format( "*{1}Command Received[{3}] : Type = {0} Text = {2}", command == null ? "unknown" : command.GetType().Name, DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss" ), strRecvBuff, CommNo) );
                    }
                    this.commandReflection( command );
                }
            }

            return recvSuccess;
        }

        ///// <summary>
        ///// デバッグ用 インスタンス状態解析
        ///// </summary>
        ///// <remarks>
        ///// オブジェクトの内容を詳細に解析し、出力を行います。
        ///// </remarks>
        ///// <param name="instance">解析対象</param>
        //public void InstanceAnalyser( Object instance )
        //{
        //    //TestClassクラスのTypeオブジェクトを取得する
        //    Type t = instance.GetType();

        //    //メソッドの一覧を取得する
        //    System.Reflection.MethodInfo[] methods = t.GetMethods(
        //        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic |
        //        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static );

        //    foreach ( System.Reflection.MethodInfo m in methods )
        //    {
        //        //特別な名前のメソッドは表示しない
        //        if ( m.IsSpecialName )
        //        {
        //            continue;
        //        }

        //        //アクセシビリティを表示
        //        //ここではIs...プロパティを使っているが、
        //        //Attributesプロパティを調べても同じ
        //        if ( m.IsPublic )
        //            Console.Write( "public " );
        //        if ( m.IsPrivate )
        //            Console.Write( "private " );
        //        if ( m.IsAssembly )
        //            Console.Write( "internal " );
        //        if ( m.IsFamily )
        //            Console.Write( "protected " );
        //        if ( m.IsFamilyOrAssembly )
        //            Console.Write( "internal protected " );

        //        //その他修飾子を表示
        //        if ( m.IsStatic )
        //            Console.Write( "static " );
        //        if ( m.IsAbstract )
        //            Console.Write( "abstract " );
        //        else if ( m.IsVirtual )
        //            Console.Write( "virtual " );

        //        //戻り値を表示
        //        if ( m.ReturnType == typeof( void ) )
        //            Console.Write( "void " );
        //        else
        //            Console.Write( m.ReturnType.ToString() + " " );

        //        //メソッド名を表示
        //        Console.Write( m.Name );

        //        //パラメータを表示
        //        System.Reflection.ParameterInfo[] prms = m.GetParameters();
        //        Console.Write( "(" );
        //        for ( Int32 i = 0; i < prms.Length; i++ )
        //        {
        //            System.Reflection.ParameterInfo p = prms[i];
        //            Console.Write( p.ParameterType.ToString() + " " + p.Name );
        //            if ( prms.Length - 1 > i )
        //                Console.Write( ", " );
        //        }
        //        Console.Write( ")" );

        //        System.Reflection.FieldInfo[] fields = t.GetFields( System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance );
        //        foreach ( var field in fields )
        //        {
        //            field.GetValue( instance );
        //        }

        //    }
        //}

        /// <summary>
        /// デバッグ用関数
        /// </summary>
        /// <remarks>
        /// コマンドデータを文字列化し、出力します。
        /// </remarks>
        /// <param name="cmd">コマンドデータ</param>
        protected void commandReflection( CommCommand cmd )
        {
#if DEBUG
            if ( cmd == null )
            {
                return;
            }
            try
            {
                Int32 NEST_LIMIT = 5;
                System.Diagnostics.Debug.WriteLine( String.Format("-----[{0}]-------------------↓commandDetail↓---------------------------", CommNo ) );
                Type pType = cmd.GetType();
                String fieldName = String.Empty;
                foreach ( var v in pType.GetProperties() )
                {
                    Int32 nestRest = NEST_LIMIT;
                    fieldName = v.Name;
                    Object obj = null;
                    obj = v.GetValue( cmd, null );
                    Action<Object> actA = null;
                    actA = ( o ) =>
                    {
                        if ( nestRest-- < 0 )
                        {
                            // 深い階層を持つオブジェクトに対する再帰でのスタックオーバーフロー対策
                            return;
                        }

                        if ( o != null )
                        {
                            Type propType = o.GetType();
                            // var fields = propType.GetFields( System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic );
                            var fields = propType.GetFields( System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic );
                            if ( fields.Count() == 0 )
                            {
                                if ( propType.IsArray )
                                {

                                    var count = propType.GetProperty( "Length" );
                                    var getvalue = propType.GetMethod( "GetValue", new Type[] { typeof( Int64 ) } );
                                    try
                                    {
                                        Int32 aryCount = (Int32)count.GetValue( o, null );

                                        for ( Int32 i = 0; i < aryCount; i++ )
                                        {
                                            System.Diagnostics.Debug.WriteLine( "{0}:[{1}]", propType.Name, i );
                                            var value = getvalue.Invoke( o, new object[]
                                                {
                                                    i
                                                } );
                                            actA( value );
                                        }
                                    }
                                    catch ( Exception ex )
                                    {
                                        System.Diagnostics.Debug.WriteLine( String.Format( "{0}の解析に失敗:{1} {2}", propType.Name, ex.Message, ex.StackTrace ) );
                                    }
                                }
                                else if ( !propType.IsPrimitive && propType.Name != "String" && propType.IsClass )
                                {
                                    var innarFields = propType.GetFields( System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance );

                                    foreach ( var innarField in innarFields )
                                    {
                                        var fieldVal = innarField.GetValue( o );
                                        fieldName = innarField.Name;
                                        actA( fieldVal );
                                    }
                                    var props = propType.GetProperties();
                                    foreach ( var prop in props )
                                    {
                                        Type prooType = prop.GetType();
                                        try
                                        {
                                            var propVal = prop.GetValue( o, null );
                                            actA( propVal );
                                        }
                                        catch ( Exception ex )
                                        {
                                            System.Diagnostics.Debug.WriteLine( String.Format( "{0}の解析に失敗:{1} {2}", propType.Name, ex.Message, ex.StackTrace ) );
                                        }
                                    }
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine( "{0}={1}", fieldName, o );
                                }
                            }
                            else
                            {
                                foreach ( System.Reflection.FieldInfo field in fields )
                                {
                                    if ( field.GetType() == typeof( string ) )
                                    {
                                        System.Diagnostics.Debug.WriteLine( "{0}={1}", propType.Name, o );
                                    }
                                    Object obj2 = null;

                                    obj2 = field.GetValue( o );
                                    actA( obj2 );
                                }
                            }
                        }
                        //if ( o != null )
                        //{
                        //    Type propType = o.GetType();
                        //    Int32 propCnt = propType.GetProperties().Count();
                        //    if ( propCnt == 0 )
                        //    {
                        //        System.Diagnostics.Debug.WriteLine( "{0}={1}", propType.Name, o );
                        //    }
                        //    else
                        //    {
                        //        foreach ( var vv in propType.GetProperties() )
                        //        {
                        //            if ( vv.GetType() == typeof(string))
                        //            {
                        //                System.Diagnostics.Debug.WriteLine( "{0}={1}", propType.Name, o );
                        //            }
                        //            Object obj2 = null;

                        //            obj2 = vv.GetValue( o, null );
                        //            actA( obj2 );
                        //        }
                        //    }
                        //}
                    };
                    actA( obj );
                    //Action<Object> actB = null;
                    //Action<Object> actA = ( o ) =>
                    //{
                    //    if ( o != null )
                    //    {
                    //        Type propType = o.GetType();
                    //        foreach ( var vv in propType.GetProperties() )
                    //        {
                    //            Object obj2 = null;
                    //            vv.GetValue( obj2,null);
                    //            actB( obj2 );
                    //        }
                    //    }
                    //};
                    //actB = actA;
                    //actA( obj );              
                }
                System.Diagnostics.Debug.WriteLine( String.Format("-----[{0}]-------------------↑commandDetail↑---------------------------", CommNo) );
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine( "コマンド詳細解析に失敗:{0} {1}", ex.Message, ex.StackTrace );
            }
            //foreach ( var v in pType.GetMembers() )
            //{
            //    Type cType = v.DeclaringType;
            //    //v.
            //    //cType.GetMembers( v. );
            //}
#endif
        }

        ///// <summary>
        ///// 接続状態取得
        ///// </summary>
        //ConnectionStatus ConenctionStatus
        //{
        //    get;
        //}
        //public virtual ConnectionStatus IsConnect()
        //{
        //}

        #endregion
    }
}
