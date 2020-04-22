using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Calculator;

namespace Oelco.CarisX.Utility
{
    /// <summary>
    /// 分析エラー情報クラス
    /// </summary>
    public class AnalysisErrorInfo
    {
        #region [クラス変数定義]

        /// <summary>
        /// 分析エラー毎の既定の分析エラー情報
        /// </summary>
        private static List<AnalysisErrorInfo> defaultProtocolErrorInfo = new List<AnalysisErrorInfo>();

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static AnalysisErrorInfo()
        {
            #region 既定エラー情報初期化
            // 非エラー
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.NoError, new Remark(), String.Empty, 0, 0, new List<String>() ) );

            // リマーク/メッセージ/詳細情報
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.Abnormal1, Remark.RemarkBit.CalcConcError, String.Empty, 36, 2, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.Abnormal2, Remark.RemarkBit.CalcConcError, String.Empty, 36, 2, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.Abnormal3, Remark.RemarkBit.CalcConcError, String.Empty, 36, 2, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.Abnormal4, Remark.RemarkBit.CalcConcError, String.Empty, 36, 2, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.Abnormal5, Remark.RemarkBit.CalcConcError, String.Empty, 36, 2, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.Abnormal6, Remark.RemarkBit.CalcConcError, String.Empty, 36, 2, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.Abnormal7, Remark.RemarkBit.CalcConcError, String.Empty, 36, 2, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.CalcConcError, Remark.RemarkBit.CalcConcError, String.Empty, 36, 2, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.CalibHighError, Remark.RemarkBit.CalibError, String.Empty, 34, 1, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.CalibLowError, Remark.RemarkBit.CalibError, String.Empty, 34, 1, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.DarkError, Remark.RemarkBit.DarkError, String.Empty, 30, 1, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.DiffError, Remark.RemarkBit.DiffError, String.Empty, 36, 1, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.DivByZero, Remark.RemarkBit.CalcConcError, String.Empty, 36, 2, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.GrayZoneError, Remark.REMARK_DEFAULT, String.Empty, 0, 0, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.HighError, Remark.REMARK_DEFAULT, String.Empty, 0, 0, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.LowError, Remark.REMARK_DEFAULT, String.Empty, 0, 0, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.NoCurveType, Remark.RemarkBit.CalibrationCurveError, String.Empty, 61, 1, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.NotConverge, Remark.RemarkBit.CalcConcError, String.Empty, 36, 2, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.NotExistMasterCurve, Remark.RemarkBit.CalcConcError, String.Empty, 36, 2, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add(new AnalysisErrorInfo(AnalysisError.NotExistMeasProto, Remark.RemarkBit.CalcConcError, String.Empty, 61, 1, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add(new AnalysisErrorInfo(AnalysisError.NotExistMeasProtoFile, Remark.RemarkBit.CalcConcError, String.Empty, 61, 1, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add(new AnalysisErrorInfo(AnalysisError.NotExistProtoNo, Remark.RemarkBit.CalcConcError, String.Empty, 61, 1, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.Other, Remark.RemarkBit.CalcConcError, String.Empty, 36, 2, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add(new AnalysisErrorInfo(AnalysisError.OverRoutineTable, Remark.REMARK_DEFAULT, String.Empty, 61, 1, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add(new AnalysisErrorInfo(AnalysisError.DynamicHighErr, Remark.RemarkBit.DynamicrangeUpperError, String.Empty, 0, 2, new List<String>()
            {
            }));
            defaultProtocolErrorInfo.Add(new AnalysisErrorInfo(AnalysisError.DynamicLowErr, Remark.RemarkBit.DynamicrangeLowerError, String.Empty, 0, 1, new List<String>()
            {
            }));
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.CalibCurveErr, Remark.RemarkBit.CalibrationCurveError, String.Empty, 0, 0, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.OutOfRangeOfControlErr, Remark.RemarkBit.ControlRangeError, String.Empty, 37, 1, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.CalibCurveApprovalErr, Remark.RemarkBit.CalibExpirationDateError, String.Empty, 0, 0, new List<String>()
            {
            } ) );
            defaultProtocolErrorInfo.Add( new AnalysisErrorInfo( AnalysisError.PhotometryError, Remark.RemarkBit.PhotometryError, String.Empty, 31, 1, new List<String>()
            {
            } ) );

            #endregion
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AnalysisErrorInfo( AnalysisError error = AnalysisError.NoError )
        {
            this.SetDefaultInfo( error );
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private AnalysisErrorInfo( AnalysisError error, Remark remark, String errorMessage, Int32 errorCode, Int32 errorArg, List<String> errorDetailInfoParam )
        {
            this.EditInfo( error, remark, errorMessage, errorCode, errorArg, errorDetailInfoParam );
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// エラーの取得
        /// </summary>
        public AnalysisError Error
        {
            get;
            private set;
        }

        /// <summary>
        /// リマークの取得
        /// </summary>
        public Remark Remark
        {
            get;
            private set;
        }

        /// <summary>
        /// エラーコメントの取得
        /// </summary>
        public String ErrorComment
        {
            get;
            private set;
        }

        /// <summary>
        /// エラーコードの取得
        /// </summary>
        public Int32 ErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// エラー引数の取得
        /// </summary>
        public Int32 ErrorArg
        {
            get;
            private set;
        }

        /// <summary>
        /// エラー詳細情報文字列引数の取得
        /// </summary>
        public List<String> ErrorDetailInfoParam
        {
            get;
            private set;
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 既定エラー情報を設定
        /// </summary>
        /// <remarks>
        /// 既定エラー情報を設定します
        /// </remarks>
        /// <param name="error"></param>
        public void SetDefaultInfo( AnalysisError error )
        {
            var Info = defaultProtocolErrorInfo.Find( ( info ) => info.Error == error );
            if ( Info != null )
            {
                var defaultInfo = Info;
                this.EditInfo( defaultInfo.Error, defaultInfo.Remark, defaultInfo.ErrorComment, defaultInfo.ErrorCode, defaultInfo.ErrorArg, defaultInfo.ErrorDetailInfoParam );
            }
        }
        
        /// <summary>
        /// 現在の設定を編集
        /// 引数がNullの場合、
        /// </summary>
        /// <remarks>
        /// 現在の設定を編集します
        /// </remarks>
        /// <param name="error">エラー</param>
        /// <param name="remark">リマーク</param>
        /// <param name="errorComment">エラーコメント</param>
        /// <param name="errorDetailInfoParam">エラー詳細文字列引数</param>
        /// <param name="errorCode">エラーコード</param>
        /// <param name="errorArg">エラー引数</param>
        public void EditInfo( Nullable<AnalysisError> error = null, Remark remark = null, String errorComment = null, Nullable<Int32> errorCode = null, Nullable<Int32> errorArg = null, List<String> errorDetailInfoParam = null )
        {
            this.Error = error ?? this.Error;
            this.ErrorComment = errorComment ?? this.ErrorComment;
            this.ErrorDetailInfoParam = ( errorDetailInfoParam != null ) ? new List<String>( errorDetailInfoParam ) : this.ErrorDetailInfoParam;
            this.ErrorCode = errorCode ?? this.ErrorCode;
            this.ErrorArg = errorArg ?? this.ErrorArg;
            this.Remark = remark;
        }

        #endregion

    }

    /// <summary>
    /// 分析エラー(ProtoError)
    /// </summary>
    public enum AnalysisError
    {
        /// <summary>
        /// 非エラー:0
        /// </summary>
        NoError = 0,
        /// <summary>
        /// 分析項目ファイルなしエラー:1
        /// </summary>
        NotExistMeasProtoFile,
        /// <summary>
        /// 分析項目なしエラー:2
        /// </summary>
        NotExistMeasProto,
        /// <summary>
        /// :3
        /// </summary>
        OverRoutineTable,
        /// <summary>
        /// 濃度正常範囲外エラー(上限超過)(CulcErr依存):4
        /// </summary>
        HighError,
        /// <summary>
        /// 濃度正常範囲外エラー(下限未満)(CulcErr依存):5
        /// </summary>
        LowError,
        /// <summary>
        /// 分析項目番号なしエラー:6
        /// </summary>
        NotExistProtoNo,
        // ↓ここから計算クラスのエラー
        /// <summary>
        /// 計算ゼロ除算発生エラー(CulcErr依存):7
        /// </summary>
        DivByZero,
        /// <summary>
        /// 検量線なしエラー(CulcErr依存):8
        /// </summary>
        NoCurveType,
        ///// <summary>
        ///// 
        ///// </summary>
        //PNRatio,
        /// <summary>
        /// Abnormal1エラー(CulcErr依存):9
        /// </summary>
        Abnormal1,
        /// <summary>
        /// Abnormal2エラー(CulcErr依存):10
        /// </summary>
        Abnormal2,
        /// <summary>
        /// Abnormal3エラー(CulcErr依存):11
        /// </summary>
        Abnormal3,
        /// <summary>
        /// Abnormal4エラー(CulcErr依存):12
        /// </summary>
        Abnormal4,
        /// <summary>
        /// Abnormal5エラー(CulcErr依存):13
        /// </summary>
        Abnormal5,
        /// <summary>
        /// Abnormal6エラー(CulcErr依存):14
        /// </summary>
        Abnormal6,
        /// <summary>
        /// Abnormal7エラー(CulcErr依存):15
        /// </summary>
        Abnormal7,
        /// <summary>
        /// 濃度算出エラー(CulcErr依存):16
        /// </summary>
        Other,
        /// <summary>
        /// ダークエラー:17
        /// </summary>
        DarkError,
        ///// <summary>
        ///// 
        ///// </summary>
        //OptHighError,
        ///// <summary>
        ///// 
        ///// </summary>
        //OptLowError,
        /// <summary>
        /// 多重測定内乖離限界エラー:18
        /// </summary>
        DiffError,
        /// <summary>
        /// 濃度算出不能エラー:19
        /// </summary>
        CalcConcError,
        /// <summary>
        /// キャリブレーションデータエラー(上限超過):20
        /// </summary>
        CalibHighError,
        /// <summary>
        /// キャリブレーションデータエラー(下限未満):21
        /// </summary>
        CalibLowError,
        /// <summary>
        /// 擬陽性(定性項目判定結果):22
        /// </summary>
        GrayZoneError,
        /// <summary>
        /// 値非収束エラー(CulcErr依存):23
        /// </summary>
        NotConverge,
        /// <summary>
        /// 基本検量線なしエラー:24
        /// </summary>
        NotExistMasterCurve,
        /// <summary>
        /// ダイナミックレンジエラー(上限超過):25
        /// </summary>
        DynamicHighErr,
        /// <summary>
        /// ダイナミックレンジエラー(下限未満):26
        /// </summary>
        DynamicLowErr,
        /// <summary>
        /// 検量線エラー:27
        /// </summary>
        CalibCurveErr,
        /// <summary>
        /// 管理値判定不能エラー:28
        /// </summary>
        JudgeControlErr,
        /// <summary>
        /// 管理値範囲外エラー(Xバー管理図):29
        /// </summary>
        OutOfRangeOfControlErr,
        /// <summary>
        /// 検量線有効期限エラー:30
        /// </summary>
        CalibCurveApprovalErr,
        /// <summary>
        /// 測光エラー:31
        /// </summary>  
        PhotometryError,
        ///// <summary>
        ///// 発光比率許容範囲外エラー:32
        ///// </summary>              
        //MeasRatio
    }

    /// <summary>
    /// 拡張エラークラス
    /// </summary>
    public static class ErrorExtension
    {
        /// <summary>
        /// 計算エラーの上位エラー変換用テーブル
        /// </summary>
        private static Dictionary<CalcuErr, AnalysisError> errorTable = new Dictionary<CalcuErr, AnalysisError>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static ErrorExtension()
        {
            #region 変換用エラーテーブル初期化
            errorTable.Add( CalcuErr.Abnormal1, AnalysisError.Abnormal1 );
            errorTable.Add( CalcuErr.Abnormal2, AnalysisError.Abnormal2 );
            errorTable.Add( CalcuErr.Abnormal3, AnalysisError.Abnormal3 );
            errorTable.Add( CalcuErr.Abnormal4, AnalysisError.Abnormal4 );
            errorTable.Add( CalcuErr.Abnormal5, AnalysisError.Abnormal5 );
            errorTable.Add( CalcuErr.Abnormal6, AnalysisError.Abnormal6 );
            errorTable.Add( CalcuErr.Abnormal7, AnalysisError.Abnormal7 );
            errorTable.Add( CalcuErr.DivByZero, AnalysisError.DivByZero );
            errorTable.Add( CalcuErr.High, AnalysisError.HighError );
            errorTable.Add( CalcuErr.Low, AnalysisError.LowError );
            errorTable.Add( CalcuErr.NoCurveType, AnalysisError.NoCurveType );
            errorTable.Add( CalcuErr.NoError, AnalysisError.NoError );
            errorTable.Add( CalcuErr.NotConverge, AnalysisError.NotConverge );
            errorTable.Add( CalcuErr.Other, AnalysisError.Other );
            //errorTable.Add( CalcuErr.PNRatio, AnalysisError.PNRatio );
            #endregion
        }

        /// <summary>
        /// 指定の計算エラー対応する上位エラー取得
        /// </summary>
        /// <remarks>
        /// 指定の計算エラー対応する上位エラー取得します
        /// </remarks>
        /// <param name="err">計算エラー</param>
        /// <returns>エラー</returns>
        public static AnalysisError ToAnalysisError( this CalcuErr err )
        {
            if ( errorTable.Keys.Contains( err ) )
            {
                return errorTable[err];
            }
            return errorTable[CalcuErr.NoError];
        }
    }
}
