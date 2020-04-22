using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Globalization;

namespace Oelco.Common.Const
{
    public enum FourPType : int
    {
        strEmpty = 0,
        str1X,
        str1X2,
        str1Y,
        str1Y2,
        str1Y_K
    }

    public struct FourPTypeStruct
    {
        public FourPType PType;
        public double ValueK;
    }

    /// <summary>
    /// 汎用定数
    /// </summary>
    /// <remarks>
    /// 全システムで共有可能な定数を定義します。
    /// </remarks>
    public class GlobalConst
    {
        // TODO:不使用・変更分随時整理する
        #region 計算処理向け移植分
        
        /// <summary>
        /// ＩＮＩファイル名定義
        /// </summary>
        public const String INIFILE_NAME = "NS-X.ini";

        /// <summary>
        /// リアルタイム測定結果
        /// </summary>
        public const String REAL_MEAS_DB_NAME = "RealMeas.db";
        /// <summary>
        /// 予約ＤＢファイル名
        /// </summary>
        public const String MEAS_DIRECT_DB_NAME = "MeasDirectV2.db";
        //測定結果ＤＢファイル名定義
        /// <summary>
        /// 検体測定結果
        /// </summary>
        public const String PAT_RESULT_DB_NAME = "PatResultV2.db";		// 検体測定結果
        /// <summary>
        /// 標準液測定結果
        /// </summary>
        public const String STD_RESULT_DB_NAME = "StdResultV2.db";		// 標準液測定結果
        /// <summary>
        /// ｺﾝﾄﾛｰﾙ測定結果
        /// </summary>
        public const String CTL_RESULT_DB_NAME = "CtlResultV2.db";		// ｺﾝﾄﾛｰﾙ測定結果
        // ステータスＤＢファイル定義
        /// <summary>
        /// 検体ステータス
        /// </summary>
        public const String PAT_STATUS_DB_NAME = "PatStatusV2.db";		// 検体ステータス
        /// <summary>
        /// 標準液ステータス
        /// </summary>
        public const String STD_STATUS_DB_NAME = "StdStatusV2.db";		// 標準液ステータス
        /// <summary>
        /// ｺﾝﾄﾛｰﾙステータス
        /// </summary>
        public const String CTL_STATUS_DB_NAME = "CtlStatusV2.db";		// ｺﾝﾄﾛｰﾙステータス
        // タイムコースＤＢファイル名定義
        /// <summary>
        /// ﾀｲﾑｺｰｽﾃﾞｰﾀ
        /// </summary>
        public const String TIMECS_DB_NAME = "TimeCs.db";			// ﾀｲﾑｺｰｽﾃﾞｰﾀ
        // 登録ＤＢファイル名定義
        /// <summary>
        /// 再検査登録
        /// </summary>
        public const String RETEST_DB_NAME = "ReTestV2.db";			// 再検査登録
        /// <summary>
        /// 再検査予約データ
        /// </summary>
        public const String RESERV_DB_NAME = "ReserveV2.db";		// 再検査予約データ
        /// <summary>
        /// 希釈登録
        /// </summary>
        public const String DILUTION_DB_NAME = "Dilution.db";       // 希釈登録
        /// <summary>
        /// 標準液登録
        /// </summary>
        public const String STANDARD_DB_NAME = "StandardV2.db";		// 標準液登録
        /// <summary>
        /// コントロール登録
        /// </summary>
        public const String CONTROL_DB_NAME = "Control.db";		// コントロール登録
        /// <summary>
        /// 検量線ＤＢ名
        /// </summary>
        public const String STDLINE_DB_NAME = "Stdline.db";		// 検量線ＤＢ名
        /// <summary>
        /// 精度管理ＤＢ名
        /// </summary>
        public const String QC_DB_NAME = "QcV2.db";				// 精度管理ＤＢ名
        // 2004.07.06 BB 追加
        /// <summary>
        /// 630データDB名
        /// </summary>
        public const String RESULT_630_DB_NAME = "Result630Data.db";	// 630データDB名
        /// <summary>
        /// 陽性率データ
        /// </summary>
        public const String POSITIVE_DB_NAME = "Positive.db";		// 陽性率データ


        // テーブルファイル名定義
        /// <summary>
        /// システムパラメータ
        /// </summary>
        public const String SYSPARA_TBL_NAME = "SysParam.txt";		// システムパラメータ
        /// <summary>
        /// システムパラメータ初期値
        /// </summary>
        public const String SYSPARAINIT_TBL_NAME = "SysParamInit.txt";	// システムパラメータ初期値
        /// <summary>
        /// 分析パラメータ
        /// </summary>
        public const String ANAPARA_TBL_NAME = "AnaParam.txt";		// 分析パラメータ
        /// <summary>
        /// タイトル表示順パラメータ
        /// </summary>
        public const String TITLEORDER_TBL_NAME = "TitleOrder.txt";	// タイトル表示順パラメータ
        /// <summary>
        /// 障害対策パラメータ
        /// </summary>
        public const String SENSOR_TBL_NAME = "Sensor.txt";		// 障害対策パラメータ
        /// <summary>
        /// 消耗品状態パラメータ
        /// </summary>
        public const String SPAREPARTS_TBL_NAME = "SpareParts.txt";	// 消耗品状態パラメータ
        /// <summary>
        /// 試薬残量情報パラメータ
        /// </summary>
        public const String REAGREMAIN_TBL_NAME = "ReagRemain.txt";	// 試薬残量情報パラメータ
        /// <summary>
        /// ラック状況パラメータ
        /// </summary>
        public const String RACKINFO_TBL_NAME = "RackInfo.txt";		// ラック状況パラメータ
        /// <summary>
        /// 判定閾値パラメータ
        /// </summary>
        public const String JUDGEPARAM_TBL_NAME = "JudgeParam.txt";	// 判定閾値パラメータ

        /// <summary>
        /// 検量線データ
        /// </summary>
        public const String CALIB_TBL_NAME = "Calib.txt";         // 検量線データ
        /// <summary>
        /// リファレンスデータ
        /// </summary>
        public const String REFERENCE_TBL_NAME = "Reference.txt";		// リファレンスデータ
        /// <summary>
        /// ダークデータ
        /// </summary>
        public const String DARK_TBL_NAME = "Dark.txt";			// ダークデータ

        /// <summary>
        /// エラー情報
        /// </summary>
        public const String ERROR_TBL_NAME = "NSX.err";			// エラー情報

        //// 履歴テーブル
        /// <summary>
        /// エラー履歴
        /// </summary>
        public const String ERROR_LOG_NAME = "Error.log";			// エラー履歴
        /// <summary>
        /// 操作履歴
        /// </summary>
        public const String OPERATION_LOG_NAME = "Operation.log";		// 操作履歴
        /// <summary>
        /// パラメータ変更履歴
        /// </summary>
        public const String PARAM_CHANGE_LOG_NAME = "ParamChange.log";	// パラメータ変更履歴
        /// <summary>
        /// オンライン履歴
        /// </summary>
        public const String ONLINE_LOG_NAME = "Online.log";		// オンライン履歴
        /// <summary>
        /// サンプル停止要因ログ
        /// </summary>
        public const String CAUTION_LOG_NAME = "Caution.log";		// サンプル停止要因ログ

        //// 履歴最大保存数
        /// <summary>
        /// エラー履歴
        /// </summary>
        public const Int32 ERROR_LOG_MAX = 1000;   	        // エラー履歴 2005/1/20 256->1000に変更
        /// <summary>
        /// 操作履歴
        /// </summary>
        public const Int32 OPERATION_LOG_MAX = 5000;		    // 操作履歴
        /// <summary>
        /// パラメータ変更履歴
        /// </summary>
        public const Int32 PARAM_CHANGE_LOG_MAX = 5000;	        // パラメータ変更履歴
        /// <summary>
        /// /オンライン履歴
        /// </summary>
        public const Int32 ONLINE_LOG_MAX = 5000;	        // オンライン履歴

        /// <summary>
        /// デバッグ履歴
        /// </summary>
        public const Int32 DEBUG_LOG_MAX = 1000;    		// デバッグ履歴

        // サンプル種別
        /// <summary>
        /// 検体
        /// </summary>
        public const Int32 SAMPLE_PAT = 1;					// 検体
        /// <summary>
        /// 緊急検体
        /// </summary>
        public const Int32 SAMPLE_EMG = 2;					// 緊急検体
        /// <summary>
        /// 標準液
        /// </summary>
        public const Int32 SAMPLE_STD = 3;					// 標準液
        /// <summary>
        /// コントロール
        /// </summary>
        public const Int32 SAMPLE_CTL = 4;					// コントロール
        /// <summary>
        /// Mラック検体
        /// </summary>
        public const Int32 SAMPLE_M = 5;                   //　Mラック検体

        //public const Int32	SAMPLE_STD	=			4;					// 標準液
        //public const Int32	SAMPLE_CTL	=			3;					// コントロール
        // MIYAZAKI <--

        // 検体区分定義
        //#define	PAT_TYPE			'1'					// 検体
        //#define	STD_TYPE			'2'					// 標準液
        //#define	CTL_TYPE			'3'					// ｺﾝﾄﾛｰﾙ

        // ラックＩＤ区分
        /// <summary>
        /// 検体ﾗｯｸ
        /// </summary>
        public const String PAT_RACK_TYPE = " ";				// 検体ﾗｯｸ
        /// <summary>
        /// 標準液ﾗｯｸ
        /// </summary>
        public const String STD_RACK_TYPE = "S";				// 標準液ﾗｯｸ
        /// <summary>
        /// ｺﾝﾄﾛｰﾙﾗｯｸ
        /// </summary>
        public const String CTL_RACK_TYPE = "S";				// ｺﾝﾄﾛｰﾙﾗｯｸ

        // 最大値
        /// <summary>
        /// ｼｰｹﾝｽ番号
        /// </summary>
        public const Int32 MAX_SEQ_NO = 9999;                // ｼｰｹﾝｽ番号
        /// <summary>
        /// ﾕﾆｰｸ番号
        /// </summary>
        public const Int32 MAX_UNIQUE_NO = 999999;              // ﾕﾆｰｸ番号
        /// <summary>
        /// ﾘﾌﾟﾘｹｰｼｮﾝ番号
        /// </summary>
        public const Int32 MAX_REP_NUM = 2;                   // ﾘﾌﾟﾘｹｰｼｮﾝ番号

        //---------------------------------------------------------------------------
        /// <summary>
        /// タイムコース最大データ数
        /// </summary>
        public const Int32 MAX_TIMECOURSE_COUNT = 10;				//タイムコース最大データ数
        //---------------------------------------------------------------------------


        // 各測定データの制限値
        /// <summary>
        /// 検体ﾚｺｰﾄﾞ数
        /// </summary>
        public const Int32 MAX_PAT_RECORD = 10000;				// 検体ﾚｺｰﾄﾞ数
        /// <summary>
        /// １０００レコード
        /// </summary>
        public const Int32 MAX_STD_RECORD = 1000;                // １０００レコード
        /// <summary>
        /// １０００レコード
        /// </summary>
        public const Int32 MAX_CTL_RECORD = 1000;                // １０００レコード


        // 装置ステータス
        /// <summary>
        /// 無接続
        /// </summary>
        public const Int32 DEVSTAT_NONE = 0;   // 無接続
        /// <summary>
        /// スタートアップ
        /// </summary>
        public const Int32 DEVSTAT_STARTUP = 1;   // スタートアップ
        /// <summary>
        /// 待機
        /// </summary>
        public const Int32 DEVSTAT_WAIT = 2;   // 待機
        /// <summary>
        /// アッセイ中
        /// </summary>
        public const Int32 DEVSTAT_ASSAY = 3;   // アッセイ中
        /// <summary>
        /// サンプル停止
        /// </summary>
        public const Int32 DEVSTAT_STOP = 4;   // サンプル停止
        /// <summary>
        /// 終了
        /// </summary>
        public const Int32 DEVSTAT_END = 5;   // 終了
        /// <summary>
        /// 一時停止
        /// </summary>
        public const Int32 DEVSTAT_PAUSE = 6;   // 一時停止


        // 各ｼｰｹﾝｽ番号初期値
        /// <summary>
        /// 検体ｼｰｹﾝｽ番号
        /// </summary>
        public const Int32 INIT_PAT_SEQ_NO = 1;                // 検体ｼｰｹﾝｽ番号
        /// <summary>
        /// 緊急検体ｼｰｹﾝｽ番号
        /// </summary>
        public const Int32 INIT_STAT_SEQ_NO = 2001;                // 緊急検体ｼｰｹﾝｽ番号
        /// <summary>
        /// ｷｬﾘﾌﾞﾚｰｼｮﾝｼｰｹﾝｽ番号
        /// </summary>
        public const Int32 INIT_CALIB_SEQ_NO = 4001;                // ｷｬﾘﾌﾞﾚｰｼｮﾝｼｰｹﾝｽ番号
        /// <summary>
        /// ｺﾝﾄﾛｰﾙｼｰｹﾝｽ番号
        /// </summary>
        public const Int32 INIT_QC_SEQ_NO = 3001;                // ｺﾝﾄﾛｰﾙｼｰｹﾝｽ番号

        //// プライム
        public const Int32 PRIME_WASH = 1;					// 
        public const Int32 PRIME_WATER = 2;
        public const Int32 PRIME_ALL = 3;

        //2004/8/18 追加
        public const Int32 PRIME_QUICK = 1;
        public const Int32 PRIME_NORMAL = 2;
        public const Int32 PRIME_FULL = 3;
        //

        //// データ種別の定義
        /// <summary>
        /// --
        /// </summary>
        public const Int32 DATA_KIND_NONE = 0;					// --
        /// <summary>
        /// 検体
        /// </summary>
        public const Int32 DATA_KIND_PAT = 1;					// 検体
        /// <summary>
        /// 標準液
        /// </summary>
        public const Int32 DATA_KIND_STD = 2;					// 標準液
        /// <summary>
        /// コントロール
        /// </summary>
        public const Int32 DATA_KIND_CTL = 3;					// コントロール


        //// 廃液ボトル交換の定義
        /// <summary>
        /// --
        /// </summary>
        public const Int32 DRAIN_BTL_NONE = 0;					// --
        /// <summary>
        /// 廃液動作停止
        /// </summary>
        public const Int32 DRAIN_BTL_STOP = 1;					// 廃液動作停止
        /// <summary>
        /// 廃液動作再開
        /// </summary>
        public const Int32 DRAIN_BTL_RESTART = 2;					// 廃液動作再開


        //// 試薬の追加定義
        /// <summary>
        /// --
        /// </summary>
        public const Int32 SET_REAG_NONE = 0;					// --
        /// <summary>
        /// 確認
        /// </summary>
        public const Int32 SET_REAG_CONF = 1;					// 確認
        /// <summary>
        /// 開始要求
        /// </summary>
        public const Int32 SET_REAG_REQ = 2;					// 開始要求
        /// <summary>
        /// 開始
        /// </summary>
        public const Int32 SET_REAG_START = 3;					// 開始

        //// プロトコル定義
        //2005/6/27 変更
        /// <summary>
        /// プロトコルMAX
        /// </summary>
        public const Int32 MAX_PROTO = 10;					// プロトコルMAX  2008/1/31 8=9 に変更 2009/11/24 9=>10に変更
        /// <summary>
        /// なし(不明)
        /// </summary>
        public const Int32 PROTO_NONE = 0;					// なし(不明)
        /// <summary>
        /// HB(A)
        /// </summary>
        public const Int32 PROTO_HB_NSX = 1;					// HB(A)
        /// <summary>
        /// HB(N)
        /// </summary>
        public const Int32 PROTO_HB_NS1000 = 2;					// HB(N)
        /// <summary>
        /// HB(GC)
        /// </summary>
        public const Int32 PROTO_HB_COLBLD = 3;					// HB(GC)
        /// <summary>
        /// TF(A)
        /// </summary>
        public const Int32 PROTO_TF_NSX = 4;					// TF(A)
        /// <summary>
        /// TF(N)
        /// </summary>
        public const Int32 PROTO_TF_NS1000 = 5;					// TF(N)
        /// <summary>
        /// TF(GC)
        /// </summary>
        public const Int32 PROTO_TF_COLBLD = 6;					// TF(GC)
        /// <summary>
        /// DI
        /// </summary>
        public const Int32 PROTO_DI = 7;					// DI
        /// <summary>
        /// CC
        /// </summary>
        public const Int32 PROTO_CC = 8;					// CC
        /// <summary>
        /// TF(A)
        /// </summary>
        public const Int32 PROTO_LF_NSX = 9;					// TF(A) 2008/1/31 追加
        /// <summary>
        /// Sa-Hb
        /// </summary>
        public const Int32 PROTO_HB_SA = 10;                  // Sa-Hb 2009/11/24 追加

        /// <summary>
        /// DIの総合結果
        /// </summary>
        public const Int32 PROTO_DI_RESULT = 51;                  //DIの総合結果

        //

        //測定プロトコル名の定義
        /// <summary>
        /// 便中ヘモグロビン(A)
        /// </summary>
        public const String STR_HB_NSX = "便中ヘモグロビン(A)";
        /// <summary>
        /// 便中ヘモグロビン(N)
        /// </summary>
        public const String STR_HB_NS1000 = "便中ヘモグロビン(N)";
        /// <summary>
        /// 便中ヘモグロビン(GC)
        /// </summary>
        public const String STR_HB_SAIKETU = "便中ヘモグロビン(GC)";
        /// <summary>
        /// 便中トランスフェリン(A)
        /// </summary>
        public const String STR_TF_NSX = "便中トランスフェリン(A)";
        /// <summary>
        /// 便中トランスフェリン(N)
        /// </summary>
        public const String STR_TF_NS1000 = "便中トランスフェリン(N)";
        /// <summary>
        /// 便中トランスフェリン(GC)
        /// </summary>
        public const String STR_TF_SAIKETU = "便中トランスフェリン(GC)";
        /// <summary>
        /// 尿中ジアセチルスペルミン
        /// </summary>
        public const String STR_DI = "尿中ジアセチルスペルミン";
        /// <summary>
        /// 尿中クレアチニン
        /// </summary>
        public const String STR_CC = "尿中クレアチニン";
        /// <summary>
        /// 便中ラクトフェリン
        /// </summary>
        public const String STR_LF_NSX = "便中ラクトフェリン";  //2008/1/31 追加
        /// <summary>
        /// 唾液中ヘモグロビン
        /// </summary>
        public const String STR_HB_SA = "唾液中ヘモグロビン";    //2009/11/24 追加



        /// <summary>
        /// カットオフのMAX
        /// </summary>
        public const Int32 MAX_CUTOFF = 3;					// カットオフのMAX

        //// 架設ユニット
        /// <summary>
        /// 架設ユニットなし
        /// </summary>
        public const Int32 UNIT_NONE = 0;					// なし
        /// <summary>
        /// 150検体用架設ユニット
        /// </summary>
        public const Int32 UNIT_150 = 1;					// 150検体用架設ユニット
        /// <summary>
        /// 300検体用架設ユニット
        /// </summary>
        public const Int32 UNIT_300 = 2;					// 300検体用架設ユニット

        //// 電子メール
        /// <summary>
        /// 電子メール LAN
        /// </summary>
        public const Int32 EMAIL_COM_LAN = 1;					// LAN
        /// <summary>
        /// 電子メール 電話(モデム)
        /// </summary>
        public const Int32 EMAIL_COM_TEL = 2;					// 電話(モデム)

        // 
        // ラックステータス
        // 
        /// <summary>
        /// ラックステータス 空
        /// </summary>
        public const Int32 RACK_STATUS_EMPTY = 0;	// 空
        /// <summary>
        /// ラックステータス 処理待ち
        /// </summary>
        public const Int32 RACK_STATUS_WAIT = 1;	// 処理待ち
        /// <summary>
        /// ラックステータス 処理中
        /// </summary>
        public const Int32 RACK_STATUS_PROC = 2;	// 処理中
        /// <summary>
        /// ラックステータス 処理済
        /// </summary>
        public const Int32 RACK_STATUS_END = 3;	// 処理済
        /// <summary>
        /// ラックステータス エラー
        /// </summary>
        public const Int32 RACK_STATUS_ERROR = 4;	// エラー

        //残量の注意・警告レベル
        //2004/11/19 変更
#if false
            public const Int32 REST_ATTENTION_LINE	=         60;      //残量注意レベル(%)
            public const Int32 REST_WARNING_LINE	=           30;      //残量警告レベル(%)
#else
        /// <summary>
        /// 残量注意レベル(%)
        /// </summary>
        public const Int32 REST_ATTENTION_LINE = 40;      //残量注意レベル(%)
        /// <summary>
        /// 残量警告レベル(%)
        /// </summary>
        public const Int32 REST_WARNING_LINE = 20;      //残量警告レベル(%)
#endif

        ////色の定義
        //#define COLOR_ACTIVE_PATIENT    (TColor)0x00FFC0C0      //青(192,192,255)
        //#define COLOR_ACTIVE_STANDARD   (TColor)0x00B39CD3      //赤(211,156,179)
        //#define COLOR_ACTIVE_CONTROL    (TColor)0x00C0FFFF      //黄(255,255,192)
        //#define	COLOR_ACTIVE_POSITIVE	(TColor)0x00FFC0C0
        //#define COLOR_DILUTE_RACKID     (TColor)0x00E0E0A0      //？(160,224,224)
        //#define COLOR_DILUTE_RATE       (TColor)0x00F0CAA6      //？(166,202,240)
        //#define COLOR_EDIT_STANDARD     (TColor)0x00B39CD3      // 2004.09.15 BB 追加
        ////2004/10/4 変更
        //#if 0
        //    #define	COLOR_REMARK_PATIENT	(TColor)0x0000FFFF		// 検体：リマーク
        //    #define	COLOR_POSTV_PATIENT		(TColor)0x00E000E0		// 検体：陽性
        //#else
        //#define	COLOR_REMARK_PATIENT	(TColor)0x008BFADB		// 検体：リマーク
        ////#define	COLOR_POSTV_PATIENT		(TColor)0x00A080FF		// 検体：陽性
        //#define	COLOR_POSTV_PATIENT		(TColor)0x00FBCCFD		// 検体：陽性
        //#endif

        /// <summary>
        /// 測定結果一覧ステータス
        /// </summary>
        enum RESULT_STATUS
        {
            /// <summary>
            /// 分析中
            /// </summary>
            RESULT_STATUS_ANALYSIS = 1,     //分析中
            /// <summary>
            /// 測定終了
            /// </summary>
            RESULT_STATUS_COMPLETE,         //測定終了
            /// <summary>
            /// エラー
            /// </summary>
            RESULT_STATUS_ERROR,            //エラー
            /// <summary>
            /// 自動再検
            /// </summary>
            RESULT_STATUS_REEXAMINE,        //自動再検
            /// <summary>
            /// 予約済
            /// </summary>
            RESULT_STATUS_RESERVE           //予約済
        };

        // 
        // 試薬種別
        // 
        /// <summary>
        /// 試薬種別 R1
        /// </summary>
        public const Int32 REAG_KIND_R1 = 1;		// R1
        /// <summary>
        /// 試薬種別 R2
        /// </summary>
        public const Int32 REAG_KIND_R2 = 2;		// R2
        /// <summary>
        /// 試薬種別 希釈液
        /// </summary>
        public const Int32 REAG_KIND_DIL = 3;		// 希釈液

        // 
        // エラーコード
        // 
        /// <summary>
        /// スレーブ間通信のタイムアウト
        /// </summary>
        public const Int32 RECV_TIMEOUT = 120000;		// スレーブ間通信のタイムアウト
        /// <summary>
        /// スレーブ間通信のタイムアウト(スタートアップ)
        /// </summary>
        public const Int32 RECV_TIMEOUT_STARTUP = 999999;		// スレーブ間通信のタイムアウト(スタートアップ)
        //2004/8/24 追加
        /// <summary>
        /// 光源エラー
        /// </summary>
        public const Int32 ERR_CODE_5 = 5;           //光源エラー
        //
        
        //2004/8/4 追加
        /// <summary>
        /// スレーブからの応答がありません。
        /// </summary>
        public const Int32 ERR_CODE_31 = 31;          //スレーブからの応答がありません。
        /// <summary>
        /// ラック未登録エラー
        /// </summary>
        public const Int32 ERR_CODE_110 = 110;         //ラック未登録エラー
        /// <summary>
        /// 再検対象ラックエラー
        /// </summary>
        public const Int32 ERR_CODE_111 = 111;         //再検対象ラックエラー
        /// <summary>
        /// R1不足エラー
        /// </summary>
        public const Int32 ERR_CODE_101 = 101;         //R1不足エラー

        //2004/8/10 追加
        /// <summary>
        /// PC－ﾎｽﾄ間通信エラー
        /// </summary>
        public const Int32 ERR_CODE_17 = 17;          //PC－ﾎｽﾄ間通信エラー
        /// <summary>
        /// システムエラー
        /// </summary>
        public const Int32 ERR_CODE_24 = 24;          //システムエラー

        //2004/10/12 追加
        /// <summary>
        /// 1分後吸光度エラー
        /// </summary>
        public const Int32 ERR_CODE_12 = 12;          //1分後吸光度エラー
        /// <summary>
        /// R2分注エラー
        /// </summary>
        public const Int32 ERR_CODE_33 = 33;          //R2分注エラー

        //2004/11/16追加
        /// <summary>
        /// 検体或いはR１分注エラー
        /// </summary>
        public const Int32 ERR_CODE_35 = 35;          //検体或いはR１分注エラー

        //2005/8/1 追加
        /// <summary>
        /// ワークシートフォーマットエラー
        /// </summary>
        public const Int32 ERR_CODE_38 = 38;          //ワークシートフォーマットエラー
        //2005/12/8 追加
        /// <summary>
        /// ワークシートタイムアウトエラー
        /// </summary>
        public const Int32 ERR_CODE_116 = 116;         //ワークシートタイムアウトエラー

        //2006/2/1 追加 スレーブVer2がインストールされたかどうかのフラグファイル
        //public const String FILE_NAME_VER2_INSTALLED	= "Slave(Ver2)Installed";

        //2008/3/24 追加 スレーブVer3がインストールされたかどうかのフラグファイル
        //public const String FILE_NAME_VER3_INSTALLED	= "Slave(Ver3)Installed";

        //2010/1/29 追加 スレーブVer4がインストールされたかどうかのフラグファイル
        /// <summary>
        /// スレーブVer4がインストールされたかどうかのフラグファイル
        /// </summary>
        public const String FILE_NAME_VER4_INSTALLED = "Slave(Ver4)Installed";
        #endregion

        /// <summary>
        /// 出力ファイル拡張子[CSV]
        /// </summary>
        public const String OUTPUT_FILEKIND_CSV = @".csv";
        /// <summary>
        /// 出力ファイル拡張子[XML]
        /// </summary>
        public const String OUTPUT_FILEKIND_XML = @".xml";
        /// <summary>
        /// ファイル拡張子[XLS]
        /// </summary>
        public const String FILEKIND_XLS = @".xls";

        /// <summary>
        /// チャート表示文字列色
        /// </summary>
        static public readonly Color CHART_INVALIDATED_TEXT_COLOR = Color.FromArgb( 0, 0, 0 );    // 黒

        /// <summary>
        /// 日付表示用フォーマット文字列の取得
        /// </summary>
        static public String DISPLAY_DATETIME_FORMAT
        {
            get
            {
                return DateTimeFormatInfo.CurrentInfo.ShortDatePattern + " " + DateTimeFormatInfo.CurrentInfo.LongTimePattern;
            }
        }

        /// <summary>
        /// TabletPCレジストリキー名
        /// </summary>
        public const String TABLETTIP_KEYNAME = @"Software\Microsoft\TabletTip\1.7";

        //public const String TABLETTIP_SET_VALUE_NAME = "ShowIPTipTouchTarget";

        /// <summary>
        /// TabletPCサービス名
        /// </summary>
        public const String TABLET_SERVICE_NAME = "TabletInputService";
    }



    /// <summary>
    /// 出力用ファイル種別
    /// </summary>
    [Flags]
    public enum OutputFileKind : int
    {
        /// <summary>
        /// XMLファイル
        /// </summary>
        XML = 0x01,
        /// <summary>
        /// CSVファイル
        /// </summary>
        CSV = 0x02,
        /// <summary>
        /// XLSファイル
        /// </summary>
        XLS = 0x04
    }
    /// <summary>
    /// Enum用の拡張メソッドクラス
    /// </summary>
    public static class EnumExtention
    {
        /// <summary>
        /// 列挙値に関連する文字列の取得
        /// </summary>
        /// <remarks>
        /// 列挙値に関連する文字列を返します。
        /// </remarks>
        /// <param name="type">列挙値</param>
        /// <returns>失敗:Count() == 0</returns>
        public static List<String> ToTypeString( this OutputFileKind type )
        {
            List<String> result = new List<string>();

            foreach ( OutputFileKind kind in Enum.GetValues( typeof( OutputFileKind ) ) )
            {
                switch ( type & kind )
                {
                case OutputFileKind.XML:
                    result.Add( GlobalConst.OUTPUT_FILEKIND_XML );
                    break;
                case OutputFileKind.CSV:
                    result.Add( GlobalConst.OUTPUT_FILEKIND_CSV );
                    break;
                case OutputFileKind.XLS:
                    result.Add( GlobalConst.FILEKIND_XLS );
                    break;
                }
            }
            return result;
        }
    }
}
