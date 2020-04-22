using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.CarisX.Const;
using Oelco.CarisX.Parameter.AnalyteGroup;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 分析項目管理クラス
    /// </summary>
	public class MeasureProtocolManager// : ParameterFilePreserve
    {

        #region [インスタンス変数定義]

        /// <summary>
        /// 分析項目
        /// </summary>
		private List<ParameterFilePreserve< MeasureProtocol >> measureProtocol = new List<ParameterFilePreserve< MeasureProtocol >>();

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MeasureProtocolManager()
        {
            // 分析項目の殻を作っておく
            for ( Int32 i = 0; i < PROTOCOL_MAX_COUNT; i++ )
            {
                ParameterFilePreserve<MeasureProtocol> protocol = new ParameterFilePreserve<MeasureProtocol>();

                // 分析項目番号を作成
                protocol.Param.ProtocolIndex = i+1;
                this.measureProtocol.Add( protocol );
            }
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 分析項目の取得
        /// </summary>
        public List<MeasureProtocol> MeasureProtocolList
        {
            get
            {
                // 全ての分析項目設定を抽出する。
                IEnumerable<MeasureProtocol> searched = from v in this.measureProtocol
                                                        where ( v.Param.IsError() != true )
                                                        select v.Param;
                List<MeasureProtocol> ret = new List<MeasureProtocol>();
                ret.AddRange( searched );
                return ret;
            }
        }

        /// <summary>
        /// 使用設定分析項目の取得
        /// </summary>
        public List<MeasureProtocol> UseMeasureProtocolList
        {
            get
            {
                // 使用設定となっている分析項目設定を抽出する。
                IEnumerable<MeasureProtocol> searched = from v in this.measureProtocol
                                                      where (( v.Param.DisplayProtocol == true )
                                                          && ( v.Param.IsError() != true ))
                                                      select v.Param;
                List<MeasureProtocol> ret = new List<MeasureProtocol>();
                ret.AddRange( searched );
                return ret;
            }
        }
        #endregion

        #region [publicメソッド]
        /// <summary>
        /// 全分析項目保存
        /// </summary>
        /// <remarks>
        /// 全分析項目保存します
        /// </remarks>
        /// <returns></returns>
        public Boolean SaveAllMeasureProtocol()
        {
            Boolean result = true;

            // 分析項目をファイルへ全て書き込む
            foreach ( var protocol in this.measureProtocol )
            {
                if ( protocol.SaveEncryption() == false )
                {
                    // 読込に失敗
                    result = false;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 保存单个项目的文件
        /// </summary>
        /// <param name="protocolNo"></param>
        /// <returns></returns>
        public Boolean SaveMeasureProtocol(int protocolNo)
        {
            Boolean result = true;
            foreach (var protocol in this.measureProtocol)
            {
                if (protocol.Param.ProtocolNo == protocolNo)
                {
                    if (protocol.SaveEncryption() == false)
                    {
                        // 読込に失敗
                        result = false;                         
                    }
                    break;
                }                
            }     
            return result;
        }

        /// <summary>
        /// 全分析項目読込
        /// </summary>
        /// <remarks>
        /// 全分析項目読込します
        /// </remarks>
        /// <returns></returns>
        public Boolean LoadAllMeasureProtocol()
        {
            Boolean result = true;
            // 分析項目をファイルから全て読み込む
            foreach( var protocol in this.measureProtocol )
            {
                if ( protocol.LoadEncryption() == false )
                {
                    // 分析項目読取失敗
                    System.Diagnostics.Debug.WriteLine(String.Format("Load failure. ProtocolIndex={0}", protocol.Param.ProtocolIndex));
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                     String.Format("Load failure. ProtocolIndex={0}", protocol.Param.ProtocolIndex));
                    // 読込に失敗
                    result = false;
                }

                if (result == false)
                {
                    // 読込に失敗なのでここで終わり
                    break;
                }
                else
                {
                    // 旧アッセイシーケンスから新アッセイシーケンスとそれに伴った希釈倍率に変換
                    protocol.Param.ConvertAssaySequenceAndDilutionRatio();
                }
            }
            //if ( result != false )
            //{
                // ProtocolIndex重複チェック 重篤なエラーが発生する可能性がある為、ログに出しておく
                var checker =   from vv in
                                    from v in this.measureProtocol
                                    where v.Param.IsError() == false
                                    group v.Param by v.Param.ProtocolIndex
                                where vv.Count() >= 2
                                select vv;
                if ( checker.Count() != 0 )
                {
                    foreach ( var prot in checker )
                    {
                        System.Diagnostics.Debug.WriteLine( String.Format( "ProtocolIndex={0}が{2}件重複します。Path:{1}", prot.First().ProtocolIndex, prot.First().SavePath,prot.Count() ) );
                        Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty,
                                                                         String.Format( "ProtocolIndex={0}が{2}件重複します。Path:{1}", prot.First().ProtocolIndex, prot.First().SavePath, prot.Count() ) );
                    }
                }
 
 
                this.protocolIndexToRoutineTableOrder.Clear();
                this.routineTableOrderToProtocolIndex.Clear();
                Int32 routineTableOrder = 1; // 1Origin
                
                foreach ( var protocol in this.UseMeasureProtocolList )
                {
                    // 分析項目インデックス <-> ルーチンテーブル順序辞書設定
                    this.protocolIndexToRoutineTableOrder[protocol.ProtocolIndex] = routineTableOrder;
                    this.routineTableOrderToProtocolIndex[routineTableOrder] = protocol.ProtocolIndex;
                    routineTableOrder++;
                }
            //}
            return result;
        }

        /// <summary>
        /// 分析項目設定取得
        /// </summary>
        /// <remarks>
        /// 分析項目設定情報を分析項目名称から取得します。
        /// </remarks>
        /// <param name="name">分析項目名称</param>
        /// <returns>分析項目情報</returns>
        public MeasureProtocol GetMeasureProtocolFromName(String name )
        {
            MeasureProtocol protocol = null;

            // 測定項目名称から測定項目設定を検索
            IEnumerable<MeasureProtocol> searchResult = from p in this.measureProtocol
                                                        where p.Param.ProtocolName == name
                                                        select p.Param;

            // 検索結果を取得
            if ( searchResult.Count() != 0 )
            {
                protocol = searchResult.First();
            }

            return protocol;
        }

        /// <summary>
        /// 分析項目設定取得
        /// </summary>
        /// <remarks>
        /// 分析項目設定情報を分析項目インデックスから取得します。
        /// </remarks>
        /// <param name="name">分析項目インデックス</param>
        /// <returns>分析項目情報</returns>
        public MeasureProtocol GetMeasureProtocolFromProtocolIndex( Int32 protocolIndex)
        {
            MeasureProtocol protocol = null;

            // 分析項目インデックスから測定項目設定を検索
            IEnumerable<MeasureProtocol> searchResult = from p in this.measureProtocol
                                                        where p.Param.ProtocolIndex == protocolIndex
                                                        select p.Param;

            // 検索結果を取得
            if (searchResult.Count() != 0)
            {
                protocol = searchResult.First();
            }

            return protocol;
        }
        /// <summary>
        /// 分析項目設定取得
        /// </summary>
        /// <remarks>
        /// 分析項目設定情報を分析項目番号から取得します。
        /// </remarks>
        /// <param name="name">分析項目番号</param>
        /// <returns>分析項目情報</returns>
        public MeasureProtocol GetMeasureProtocolFromProtocolNo( Int32 protocolNo )
        {
            MeasureProtocol protocol = null;

            // 分析項目番号から測定項目設定を検索
            IEnumerable<MeasureProtocol> searchResult = from p in this.measureProtocol
                                                        where p.Param.ProtocolNo == protocolNo
                                                        select p.Param;

            // 検索結果を取得
            if ( searchResult.Count() != 0 )
            {
                protocol = searchResult.First();
            }

            return protocol;
        }
        /// <summary>
        /// デフォルト分析項目設定取得
        /// </summary>
        /// <remarks>
        /// 分析項目設定情報を分析項目番号から取得します。
        /// </remarks>
        /// <param name="name">分析項目番号</param>
        /// <returns>分析項目情報</returns>
        public MeasureProtocol GetDefaultMeasureProtocolFromProtocolNo( Int32 protocolNo )
        {
            MeasureProtocol serchResult = this.GetMeasureProtocolFromProtocolNo( protocolNo );

            if ( serchResult != null )
            {
                // 分析項目番号から測定項目設定を検索
                ParameterFilePreserve<MeasureProtocol> protocol = new ParameterFilePreserve<MeasureProtocol>();
                // プロトコルインデックスに引数値を設定する
                protocol.Param.ProtocolIndex = serchResult.ProtocolIndex;
                // 読込パスをDefaultにする
                protocol.Param.SetSaveProtocolPathDefault();
                // プロトコル読込
                if ( protocol.LoadEncryption() )
                {
                    return protocol.Param;
                }
            }           

            return null;
        }

        /// <summary>
        /// 分析項目インデックス <-> ルーチンテーブル順序
        /// </summary>
        private Dictionary<Int32,Int32> protocolIndexToRoutineTableOrder = new Dictionary<Int32,Int32>();
        /// <summary>
        /// ルーチンテーブル順序 <-> 分析項目インデックス
        /// </summary>
        private Dictionary<Int32,Int32> routineTableOrderToProtocolIndex = new Dictionary<Int32,Int32>();

        /// <summary>
        /// 分析項目インデックス <-> ルーチンテーブル順序取得
        /// </summary>
        /// <remarks>
        /// 分析項目インデックス <-> ルーチンテーブル順序取得します
        /// </remarks>
        /// <param name="routineTableOrder"></param>
        /// <returns></returns>
        public Int32 GetProtocolIndexFromRoutineTableOrder( Int32 routineTableOrder )
        {
            Int32 protocolIndex = 0;
            
            if ( this.routineTableOrderToProtocolIndex.ContainsKey( routineTableOrder ) )
            {
                protocolIndex = this.routineTableOrderToProtocolIndex[routineTableOrder];
            }
            return protocolIndex;
            
        }

        /// <summary>
        /// ルーチンテーブル番号の取得
        /// </summary>
        /// <remarks>
        /// ルーチンテーブル番号の取得します
        /// </remarks>
        /// <param name="protocolIndex">分析項目インデックス</param>
        /// <returns>0:取得失敗</returns>
        public Int32 GetRoutineTableOrder( Int32 protocolIndex )
        {
            Int32 routineTableOrder = 0;
            if ( this.protocolIndexToRoutineTableOrder.ContainsKey( protocolIndex ) )
            {
                routineTableOrder = this.protocolIndexToRoutineTableOrder[protocolIndex];
            }
            return routineTableOrder;
        }        

        /// <summary>
        /// 分析項目使用フラグ設定
        /// </summary>
        /// <remarks>
        /// 分析項目使用フラグを設定します。
        /// </remarks>
        /// <param name="name">分析項目名称</param>
        /// <param name="displayProtocol">分析項目使用フラグ</param>
        public void SetDisplayProtocol(String name, Boolean displayProtocol)
        {
            // 測定項目名称から測定項目設定を検索
            IEnumerable<MeasureProtocol> searchResult = from p in this.measureProtocol
                                                        where p.Param.ProtocolName == name
                                                        select p.Param;
            // 検索結果を取得し、分析項目使用フラグを設定
            foreach (var protocol in searchResult)
            {
                if (name == "")
                {
                    // 分析項目名称が空の場合は、使用の設定できるのもおかしいので常にfalse
                    protocol.DisplayProtocol = false;
                }
                else
                {
                    protocol.DisplayProtocol = displayProtocol;
                }
            }

            //if (searchResult.Count() != 0)
            //{
            //    MeasureProtocol protocol = searchResult.First();
            //    protocol.DisplayProtocol = displayProtocol;
            //}
        }
       


        //public String AskProtocolNameFromProtocolNo
        #endregion

        /// <summary>
        /// 分析項目最大数
        /// </summary>
        private const Int32 PROTOCOL_MAX_COUNT = 200; // TODO:後でこの定義は上位へ移動する

        #region [ProtocolConvert関連]

        /// <summary>
        /// インポート対象分析項目インデックス
        /// </summary>
        private List<Int32> importProtocolIndex;

        /// <summary>
        /// インポートする分析項目
        /// </summary>
        private List<MeasureProtocol> importProtocol = new List<MeasureProtocol>();


		/// <summary>
		/// プロトコルコンバータで出力された分析項目を読み込む
		/// </summary>
		/// <returns>true:読込み成功、false:読込み失敗</returns>
		public Boolean LoadExportProtocol(List<Int32> protIdx, out List<MeasureProtocol> outProtocol)
		{
			this.importProtocolIndex = protIdx;

			Boolean rtn = loadExportProtocol();
			outProtocol = this.importProtocol;
			return rtn;
		}		

		/// <summary>
		/// 分析項目インポート処理
		/// </summary>
		/// <returns></returns>
		public Boolean ImportMeasProtoParameter()
		{
			Boolean rtn = true;

			if (!(importProtocolToDefault()      // Defaultフォルダにインポート
				&& importProtocolToProtocol()     // Protocolフォルダにインポート
				&& updateMeasureProtocolInfo()))   // MeasureProtocolInfo更新
			{
				rtn = false;
			}

			return rtn;
		}

        /// <summary>
        /// 出力済み分析プロトコルの読込
        /// </summary>
        /// <remarks>
        /// 出力済み分析プロトコルの読込します
        /// </remarks>
        /// <returns></returns>
		public Boolean loadExportProtocol()
        {
            Boolean rtn = true;

            foreach ( var index in importProtocolIndex )
            {
                ParameterFilePreserve<MeasureProtocol> protocol = new ParameterFilePreserve<MeasureProtocol>();
                protocol.Param.ProtocolIndex = index;
                protocol.Param.SetSaveProtocolPath( CarisXConst.ProtoConvExportDir );
                if ( protocol.LoadEncryption() )
                {
                    // 旧アッセイシーケンスから新アッセイシーケンスとそれに伴った希釈倍率に変換
                    protocol.Param.ConvertAssaySequenceAndDilutionRatio();

                    this.importProtocol.Add( protocol.Param );
                }
                else
                {
                    // 1件でも正常に読み込めなかったらエラー
                    return false;
                }
            }
            return rtn;
        }
        
        /// <summary>
        /// Defaultに分析項目をインポート
        /// </summary>
        /// <remarks>
        /// Defaultに分析項目をインポートします
        /// </remarks>
        /// <param name="inpPath"></param>
        /// <param name="outPath"></param>
        /// <returns></returns>
        public Boolean importProtocolToDefault()
        {
            Boolean rtn = true;
            List<ParameterFilePreserve<MeasureProtocol>> defaultProtocol = new List<ParameterFilePreserve<MeasureProtocol>>();

            if ( this.importProtocol != null )
            {
                // プロトコルコンバータで出力された分析項目をDafalut分析項目に反映する
                foreach ( var prot in this.importProtocol )
                {
					// 旧のプロトコルが存在する場合は、上書き禁止項目だけ旧の設定値を新にセットする
					// (Defaultフォルダ内の値ではなく、現在の分析項目が保存されているフォルダ内の値を設定する)
					// 反映前の値を取得する
					MeasureProtocol oldProt = this.GetMeasureProtocolFromProtocolIndex(prot.ProtocolIndex);
					if (oldProt != null)
					{
						prot.Enable = oldProt.Enable;
						prot.DisplayProtocol = oldProt.DisplayProtocol;
					}

                    ParameterFilePreserve<MeasureProtocol> protocol = SetProtocolData( prot );
                    // 保存先をDefaultフォルダに設定する
                    protocol.Param.SetSaveProtocolPathDefault();                   
                    // 分析項目を保存
                    protocol.SaveEncryption();
                }
            }
            else
            {               
                return false;
            }           

            return rtn;
        }

        /// <summary>
        /// Protocolフォルダに分析項目をインポート
        /// </summary>
        /// <remarks>
        /// Protocolフォルダに分析項目をインポートします
        /// </remarks>
        /// 
        public Boolean importProtocolToProtocol()
        {
            Boolean rtn = true;

            // 出力された分析項目の内容を反映する
            foreach ( var prot in this.importProtocol )
            {
                // 反映前の値を取得する
                MeasureProtocol oldProt = this.GetMeasureProtocolFromProtocolIndex( prot.ProtocolIndex );
                // 旧のプロトコルが存在する場合は、上書き禁止項目だけ旧の設定値を新にセットする
                if ( oldProt != null )
                {
                    prot.Enable = oldProt.Enable;
                    prot.DisplayProtocol = oldProt.DisplayProtocol;
                    //prot.RepNoForSample = oldProt.RepNoForSample;
                    //prot.RepNoForControl = oldProt.RepNoForControl;
                    //prot.GainOfCorrelation = oldProt.GainOfCorrelation;
                    //prot.OffsetOfCorrelation = oldProt.OffsetOfCorrelation;
                    //prot.ValidityOfCurve = oldProt.ValidityOfCurve;
                    //prot.AutoReTest = oldProt.AutoReTest;
                }
                ParameterFilePreserve<MeasureProtocol> protocol = SetProtocolData( prot );
                protocol.SaveEncryption();
            }
            //// XMLに保存する
            //this.SaveAllMeasureProtocol();
            return rtn;          
        }

        /// <summary>
        /// 分析項目に値を設定する
        /// </summary>
        /// <remarks>
        /// 分析項目に値を設定します
        /// </remarks>
        /// <param name="prot"></param>
        /// <returns></returns>
        private ParameterFilePreserve<MeasureProtocol> SetProtocolData( MeasureProtocol prot )
        {
            ParameterFilePreserve<MeasureProtocol> protocol = new ParameterFilePreserve<MeasureProtocol>();
            // 項目に値を設定
            protocol.Param.RepNoForSample = prot.RepNoForSample;
            protocol.Param.RepNoForControl = prot.RepNoForControl;
            protocol.Param.RepNoForCalib = prot.RepNoForCalib;
            protocol.Param.ValidityOfCurve = prot.ValidityOfCurve;
            protocol.Param.PosiLine = prot.PosiLine;
            protocol.Param.NegaLine = prot.NegaLine;
            protocol.Param.ProtocolNo = prot.ProtocolNo;
            protocol.Param.ProtocolIndex = prot.ProtocolIndex;
            protocol.Param.ProtocolName = prot.ProtocolName;
            protocol.Param.ReagentName = prot.ReagentName;
            protocol.Param.AssaySequence = prot.AssaySequence;
            protocol.Param.ProtocolDilutionRatio = prot.ProtocolDilutionRatio;
            protocol.Param.PreProcessSequence = prot.PreProcessSequence;
            protocol.Param.SampleKind = prot.SampleKind;
            protocol.Param.UseAfterDil = prot.UseAfterDil;
            protocol.Param.UseAutoReTest = prot.UseAutoReTest;
            protocol.Param.AutoDilutionReTest = prot.AutoDilutionReTest;
            protocol.Param.AutoDilutionReTestRatio = prot.AutoDilutionReTestRatio;
            protocol.Param.AutoReTest = prot.AutoReTest;
            protocol.Param.UseManualDil = prot.UseManualDil;
            protocol.Param.ReagentCode = prot.ReagentCode;
            protocol.Param.SmpDispenseVolume = prot.SmpDispenseVolume;
            protocol.Param.MReagDispenseVolume = prot.MReagDispenseVolume;
            protocol.Param.R1DispenseVolume = prot.R1DispenseVolume;
            protocol.Param.R2DispenseVolume = prot.R2DispenseVolume;
            protocol.Param.PreProsess1DispenseVolume = prot.PreProsess1DispenseVolume;
            protocol.Param.PreProsess2DispenseVolume = prot.PreProsess2DispenseVolume;
            protocol.Param.CalibType = prot.CalibType;
            protocol.Param.GainOfCorrelation = prot.GainOfCorrelation;
            protocol.Param.OffsetOfCorrelation = prot.OffsetOfCorrelation;
            protocol.Param.CalibMethod = prot.CalibMethod;
            protocol.Param.NumOfMeasPointInCalib = prot.NumOfMeasPointInCalib;
            protocol.Param.ConcsOfEach = prot.ConcsOfEach;
            protocol.Param.CalibMeasPointOfEach = prot.CalibMeasPointOfEach;
            protocol.Param.CountRangesOfEach = prot.CountRangesOfEach;
            protocol.Param.ConcUnit = prot.ConcUnit;
            protocol.Param.LengthAfterDemPoint = prot.LengthAfterDemPoint;
            protocol.Param.ConcDynamicRange = prot.ConcDynamicRange;
            protocol.Param.MulMeasDevLimitCV = prot.MulMeasDevLimitCV;
            protocol.Param.DisplayProtocol = prot.DisplayProtocol;
            protocol.Param.UseAfterDilAtCalcu = prot.UseAfterDilAtCalcu;
            protocol.Param.UseManualDilAtCalcu = prot.UseManualDilAtCalcu;
            protocol.Param.Coef_A = prot.Coef_A;
            protocol.Param.Coef_B = prot.Coef_B;
            protocol.Param.Coef_C = prot.Coef_C;
            protocol.Param.Coef_D = prot.Coef_D;
            protocol.Param.Coef_E = prot.Coef_E;
			protocol.Param.DayOfReagentValid = prot.DayOfReagentValid;
			
            protocol.Param.RetestRange.UseLow = prot.RetestRange.UseLow;
            protocol.Param.RetestRange.UseMiddle = prot.RetestRange.UseMiddle;
            protocol.Param.RetestRange.UseHigh = prot.RetestRange.UseHigh;
            //每个都可以设置动态ＣＶ
            protocol.Param.UseCVIndependence = prot.UseCVIndependence;
            protocol.Param.CVofEachPoint = prot.CVofEachPoint;
            

            //4参数设置加权
            protocol.Param.FourPrameterMethodKValue = prot.FourPrameterMethodKValue;
            protocol.Param.FourPrameterMethodType = prot.FourPrameterMethodType;

            //校准品、质控品是否稀释
            protocol.Param.DiluCalibOrControl = prot.DiluCalibOrControl;

            //是否是IGRA项目
            protocol.Param.IsIGRA = prot.IsIGRA;

            protocol.Param.TurnOrder = prot.TurnOrder;

            return protocol;
        }

        /// <summary>
        /// MeasureProtocolInfo更新
        /// </summary>
        /// <remarks>
        /// 分析項目情報更新します
        /// </remarks>
        private Boolean updateMeasureProtocolInfo()
        {
            List<String> info = new List<String>();
            
            foreach ( var prot in this.importProtocol )
            {
                info.Add( prot.ProtocolName );
            }

            Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.SetTurnOrder( info );
            Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.InitTurnOrder();
            Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.SaveEncryption();

            return true;
        }
        
        #endregion

    }
	 
}
 
