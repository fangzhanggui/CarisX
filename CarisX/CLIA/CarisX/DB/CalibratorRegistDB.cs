using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.DB;
using Oelco.Common.Utility;
using Oelco.Common.Log;
using System.Data;
using Oelco.Common.Calculator;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Const;
using Oelco.Common.Parameter;
using Oelco.CarisX.Log;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.DB
{
    /// <summary>
    /// キャリブレータ登録情報
    /// </summary>
    public class CalibratorRegistData : DataRowWrapperBase
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public CalibratorRegistData( DataRowWrapperBase data )
            : base( data )
        {
        }

        #endregion

        #region [プロパティ]
        
        /// <summary>
        /// 分析項目インデックスの取得、設定
        /// </summary>
        protected Int32 MeasrureProtocolIndex
        {
            get
            {
                return this.Field<Int32>( CalibratorRegistDB.STRING_MEASUREPROTOCOLINDEX );
            }
            set
            {
                this.SetField<Int32>( CalibratorRegistDB.STRING_MEASUREPROTOCOLINDEX, value );
            }
        }

        /// <summary>
        /// キャリブレータ登録ラックIDの取得
        /// </summary>
        public CarisXIDString RackID
        {
            get
            {
                return this.Field<String>( CalibratorRegistDB.STRING_RACKID );
            }
        }

        /// <summary>
        /// キャリブレータ登録ラックポジションの取得
        /// </summary>
        public Int32 RackPosition
        {
            get
            {
                return this.Field<Int32>( CalibratorRegistDB.STRING_RACKPOSITION );
            }
        }

        /// <summary>
        /// 分析項目の取得
        /// </summary>
        public String Analytes
        {
            get
            {
                return Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( this.MeasrureProtocolIndex ).ProtocolName;
            }
        }
        
        /// <summary>
        /// ロット選択状態の取得
        /// </summary>
        public String LotSelection
        {
            get
            {
                return ( String.IsNullOrEmpty( this.ReagentLotNo ) ) ? Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_008 : Oelco.CarisX.Properties.Resources.STRING_CALIBREGIST_009;
            }
        }

        /// <summary>
        /// 試薬ロット番号の取得、設定
        /// </summary>
        public String ReagentLotNo
        {
            get
            {
                return this.Field<String>( CalibratorRegistDB.STRING_REAGENT_LOTNO );
            }
        }

        /// <summary>
        /// 濃度値の取得
        /// </summary>
        public String Concentration
        {
            get
            {
                return this.Field<String>( CalibratorRegistDB.STRING_CONCENTRATION );
            }
        }

        /// <summary>
        /// 濃度単位の取得、設定
        /// </summary>
        public String ConcentrationUnit
        {
            //get;
            //set;
            get
            {
                MeasureProtocol protocl = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(this.MeasrureProtocolIndex);
                return protocl.ConcUnit;
            }
        }

        /// <summary>
        /// キャリブロット番号の取得、設定
        /// </summary>
        public String CalibLotNo
        {
            get
            {
                return this.Field<String>( CalibratorRegistDB.STRING_CALIB_LOTNO );
            }
            protected set
            {
                this.SetField<String>( CalibratorRegistDB.STRING_CALIB_LOTNO, value );
            }
        }

        /// <summary>
        /// 選択分析モジュール番号の取得、設定
        /// </summary>
        public Int32 RegisteredModules
        {
            get
            {
                return this.Field<Int32>(CalibratorRegistDB.STRING_REGISTERED_MODULES);
            }
            set
            {
                this.SetField<Int32>(CalibratorRegistDB.STRING_REGISTERED_MODULES, value);
            }
        }

        /// <summary>
        /// 選択分析モジュール番号の取得、設定
        /// </summary>
        public String RegisteredModulesString
        {
            get
            {
                // 選択分析モジュール番号を取得
                int selectedModuleNo = this.GetRegisteredModules();

                // 連結用文字列リストを生成
                List<String> tempJoinList = new List<String>();

                // 選択状態のモジュールを確認し、文字列リストへ追加
                if ((selectedModuleNo & (int)ModuleCategory.Module1) != 0)
                {
                    tempJoinList.Add(String.Format("{0}", (int)RackModuleIndex.Module1));
                }
                if ((selectedModuleNo & (int)ModuleCategory.Module2) != 0)
                {
                    tempJoinList.Add(String.Format("{0}", (int)RackModuleIndex.Module2));
                }
                if ((selectedModuleNo & (int)ModuleCategory.Module3) != 0)
                {
                    tempJoinList.Add(String.Format("{0}", (int)RackModuleIndex.Module3));
                }
                if ((selectedModuleNo & (int)ModuleCategory.Module4) != 0)
                {
                    tempJoinList.Add(String.Format("{0}", (int)RackModuleIndex.Module4));
                }

                // 連結用文字列リストをカンマで連結
                String result = String.Join(Oelco.CarisX.Properties.Resources.STRING_COMMON_006, tempJoinList);

                return result;
            }
            set
            {
                // 処理なし
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 分析項目インデックスの取得
        /// </summary>
        /// <remarks>
        /// 分析項目インデックスを取得します。
        /// </remarks>
        /// <returns></returns>
        public Int32 GetMeasureProtocolIndex()
        {
            return this.MeasrureProtocolIndex;
        }

        /// <summary>
        /// キャリブレータ登録開始ラックIDの取得
        /// </summary>
        /// <remarks>
        /// キャリブレータ登録開始ラックIDを取得します。
        /// </remarks>
        /// <returns></returns>
        public CarisXIDString GetStartRackID()
        {
            return this.Field<String>( CalibratorRegistDB.STRING_START_RACKID );
        }

        /// <summary>
        /// キャリブレータ登録開始ラックポジションの取得
        /// </summary>
        /// <remarks>
        /// キャリブレータ登録開始ラックポジションを取得します。
        /// </remarks>
        /// <returns></returns>
        public Int32 GetStartRackPosition()
        {
            return this.Field<Int32>( CalibratorRegistDB.STRING_START_RACKPOSITION );
        }

        /// <summary>
        /// 選択分析モジュール番号の取得
        /// </summary>
        /// <returns>選択分析モジュール番号</returns>
        public Int32 GetRegisteredModules()
        {
            return this.RegisteredModules;
        }

        /// <summary>
        /// 選択分析モジュール番号の取得
        /// </summary>
        /// <returns>選択分析モジュール番号</returns>
        public List<RackModuleIndex> GetRegisteredModuleNoList( int moduleNo )
        {
            // 選択分析モジュール番号を取得
            int registeredModules = this.GetRegisteredModules();

            // 連結用文字列リストを生成
            List<RackModuleIndex> resultList = new List<RackModuleIndex>();

            // 選択状態のモジュールを確認し、文字列リストへ追加
            if ((registeredModules & (int)ModuleCategory.Module1) != 0)
            {
                // 全モジュールが対象または指定モジュール番号と一致する場合
                if ((moduleNo == CarisXConst.ALL_MODULEID)
                    || (moduleNo == (int)RackModuleIndex.Module1))
                {
                    resultList.Add(RackModuleIndex.Module1);
                }
            }
            if ((registeredModules & (int)ModuleCategory.Module2) != 0)
            {
                // 全モジュールが対象または指定モジュール番号と一致する場合
                if ((moduleNo == CarisXConst.ALL_MODULEID)
                    || (moduleNo == (int)RackModuleIndex.Module2))
                {
                    resultList.Add(RackModuleIndex.Module2);
                }
            }
            if ((registeredModules & (int)ModuleCategory.Module3) != 0)
            {
                // 全モジュールが対象または指定モジュール番号と一致する場合
                if ((moduleNo == CarisXConst.ALL_MODULEID)
                    || (moduleNo == (int)RackModuleIndex.Module3))
                {
                    resultList.Add(RackModuleIndex.Module3);
                }
            }
            if ((registeredModules & (int)ModuleCategory.Module4) != 0)
            {
                // 全モジュールが対象または指定モジュール番号と一致する場合
                if ((moduleNo == CarisXConst.ALL_MODULEID)
                    || (moduleNo == (int)RackModuleIndex.Module4))
                {
                    resultList.Add(RackModuleIndex.Module4);
                }
            }

            return resultList;
        }

        #endregion

        #region [内部クラス]

        /// <summary>
        /// 列キー
        /// </summary>
        /// <remarks>
        /// グリッド向け列キー設定用クラスです。
        /// </remarks>
        public struct DataKeys
        {
            public const String RackID = "RackID";
            public const String RackPosition = "RackPosition";
            public const String Analytes = "Analytes";
            public const String Concentration = "Concentration";
            public const String LotSelection = "LotSelection";
            public const String ReagentLotNo = "ReagentLotNo";
            public const String ConcentrationUnit = "ConcentrationUnit";
            public const String CalibLotNo = "CalibLotNo";
            public const String RegisteredModules = "RegisteredModules";
            public const String RegisteredModulesString = "RegisteredModulesString";
        };

        #endregion
    }

    /// <summary>
    /// キャリブレータ登録情報DBクラス
    /// </summary>
    /// <remarks>
    /// キャリブレータ登録情報のアクセスを行うクラスです。
    /// </remarks>
    public class CalibratorRegistDB : DBAccessControl
    {
        #region [定数定義]
        
        /// <summary>
        /// ラックID(DBテーブル：CalibratorRegist列名)
        /// </summary>
        public const String STRING_RACKID = "rackId";

        /// <summary>
        /// ラックポジション(DBテーブル：CalibratorRegist列名)
        /// </summary>
        public const String STRING_RACKPOSITION = "rackPosition";
        
        /// <summary>
        /// 開始ラックID(DBテーブル：CalibratorRegist列名)
        /// </summary>
        public const String STRING_START_RACKID = "startRackId";

        /// <summary>
        /// 開始ラックポジション(DBテーブル：CalibratorRegist列名)
        /// </summary>
        public const String STRING_START_RACKPOSITION = "startRackPosition";
        
        /// <summary>
        /// 分析項目インデックス(DBテーブル：CalibratorRegist列名)
        /// </summary>
        public const String STRING_MEASUREPROTOCOLINDEX = "measureProtocolIndex";

        /// <summary>
        /// 試薬ロット番号(DBテーブル：CalibratorRegist列名)
        /// </summary>
        public const String STRING_REAGENT_LOTNO = "reagentLotNo";

        /// <summary>
        /// 濃度値(DBテーブル：CalibratorRegist列名)
        /// </summary>
        public const String STRING_CONCENTRATION = "concentration";
                
        /// <summary>
        /// 登録日時(DBテーブル：CalibratorRegist列名)
        /// </summary>
        public const String STRING_REGIST_DATETIME = "registDateTime";

        /// <summary>
        /// キャリブロット番号(DBテーブル：CalibratorRegist列名)
        /// </summary>
        public const String STRING_CALIB_LOTNO = "calibLotNo";
        
        /// <summary>
        /// カップボリューム(DBテーブル：CalibratorRegist列名)
        /// </summary>
        public const String STRING_CUPVOLUME = "cupVolume";

        /// <summary>
        /// チューブボリューム(DBテーブル：CalibratorRegist列名)
        /// </summary>
        public const String STRING_TUBEVOLUME = "tubeVolume";

        /// <summary>
        /// 選択分析モジュール番号(DBテーブル：CalibratorRegist列名)
        /// </summary>
        public const String STRING_REGISTERED_MODULES = "RegisteredModules";

        #endregion

        #region [プロパティ]

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT * FROM dbo.calibratorRegist";
            }
        }

        #endregion
        
        #region [publicメソッド]
        
        /// <summary>
        /// 測定指示
        /// </summary>
        /// <remarks>
        /// スレーブからの測定指示データ問合せ応答データの検索を行います。
        /// 検索に必要な情報を保持するオブジェクトと、結果を格納するオブジェクトは同一です。
        /// </remarks>
        /// <param name="moduleId">モジュール番号</param>
        /// <param name="assaySchedules">分析スケジュールリスト</param>
        /// <param name="data">測定指示データ</param>
        /// <param name="errorArg">エラー引数</param>
        /// <param name="preModuleNoList">仮モジュール番号リスト</param>
        /// <returns></returns>
        public Boolean MeasureIndicate(int moduleId
                                     , List<AssaySchedule> assaySchedules
                                     , ref IMeasureIndicate data
                                     , ref int errorArg
                                     , ref List<RackModuleIndex> preModuleNoList )
        {
            Boolean findData = false;

            //String searchId = data.SampleID;
            CarisXIDString rackId = data.RackID;
            Int32 rackPos = data.SamplePosition;

            var result = from v in this.GetData( moduleId )
                         where v.RackID.DispPreCharString == rackId.DispPreCharString && v.RackPosition == rackPos
                         select v;

            // 結果セットにデータ追加
            if ( result.Count() != 0 )
            {
                // 問い合わせコマンド応答用情報
                List<MeasItem> measItemList = new List<MeasItem>();

                // 画面表示用データ構造を中間データとしてデータテーブルから取得
                var dbData = result.First();
                preModuleNoList = dbData.GetRegisteredModuleNoList(moduleId);
                data.SampleID = dbData.CalibLotNo;
                data.PreDil = 1; // 手希釈倍率 キャリブレータは固定で1(定数定義へ移動する)
                data.SpecimenMaterial = SpecimenMaterialType.BloodSerumAndPlasma; // サンプル種別 キャリブレータは固定で血

                MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( dbData.GetMeasureProtocolIndex() );

                MeasItem item = new MeasItem();
                item.AutoDil = 1; // 後希釈倍率 キャリブレータは固定で1（定数定義へ移動する）
                item.ProtoNo = protocol.ProtocolNo;
                item.RepCount = protocol.RepNoForCalib;
                item.TurnNo = Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.GetTurnOrder(protocol.ProtocolName);
                item.UniqNo = 1;
                item.ReagentLotNo = result.First().ReagentLotNo;// 指定の試薬ロット番号

                // 試薬ロットが取得できる場合のみ、データを登録する
                if (Singleton<ReagentDB>.Instance.GetLotSpecificationNo(protocol.ReagentCode, item.ReagentLotNo, moduleId: moduleId) != String.Empty)
                {
                    if (moduleId == CarisXConst.ALL_MODULEID)
                    {
                        //全モジュールを対象に処理した場合

                        measItemList.Add(item);
                    }
                    else
                    {
                        //特定のモジュールを対象に処理した場合

                        if (assaySchedules == null
                            || (assaySchedules != null && assaySchedules.Count != 0 && assaySchedules.Exists(v => v.ExistsSchedule(item.ProtoNo, item.ReagentLotNo))))
                        {
                            //Rack No Use（分析予定がNull）、または分析予定がある場合は登録する

                            item.UniqNo = Singleton<UniqueNo>.Instance.CreateNumber(); /* ユニーク番号生成 */
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("[Add Sample] UniqNo = {0} Rack = {1}-{2} ID = {3}", item.UniqNo, data.RackID, data.SamplePosition, data.SampleID));

                            measItemList.Add(item);
                        }
                    }
                }
                // 試薬残量不足、試薬登録がない場合
                else
                {
                    errorArg = (Int32)ErrorCollectKind.ReagentError;
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", "[90-2]Insufficient reagent or no reagent information.");
                }

                if (measItemList.Count != 0)
                {
                    // 測定項目を応答データに設定
                    data.MeasItemCount = measItemList.Count;
                    data.MeasItemArray = measItemList.ToArray();
                    findData = true; // 検索結果あり
                }
            }
            // 登録情報がない場合
            else
            {
                errorArg = (Int32)ErrorCollectKind.RegisterError;
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", "[90-3]There is no registration information.");
            }
            return findData;
        }
        /// <summary>
        /// キャリブレータ登録情報テーブルの取得
        /// </summary>
        /// <remarks>
        /// キャリブレータ登録情報テーブルを取得します。
        /// </remarks>
        /// <returns></returns>
        public List<CalibratorRegistData> GetData(Int32 moduleId = CarisXConst.ALL_MODULEID)
        {
            List<CalibratorRegistData> result = new List<CalibratorRegistData>();

            if (this.DataTable != null)
            {
                try
                {
                    var datas = from v in this.DataTable.Copy().AsEnumerable()
                                let data = new CalibratorRegistData( v )
                                orderby data.RackID.Value ascending, data.RackPosition ascending
                                select data;

                    // 全モジュール対象の場合
                    if (moduleId == CarisXConst.ALL_MODULEID)
                    {
                        // 範囲登録
                        result.AddRange(datas);
                    }
                    else
                    {
                        // 指定登録
                        foreach (var addData in datas)
                        {
                            // 取得したリストのカウントが0より大きい => 対象モジュールが含まれている
                            if( addData.GetRegisteredModuleNoList(moduleId).Count() > 0 )
                            {
                                result.Add(addData);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    //Singleton<LogManager>.Instance.Error( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace );
                }
            }

            return result;
        }

        /// <summary>
        /// キャリブレータ登録データの設定
        /// </summary>
        /// <remarks>
        /// キャリブレータ登録データの同期を行います。
        /// </remarks>
        /// <param name="list">変更、削除操作済みデータ</param>
        public void SetData( List<CalibratorRegistData> list )
        {
            list.SyncDataListToDataTable( this.DataTable );
        }

        /// <summary>
        /// 全てのキャリブレータ登録情報の削除
        /// </summary>
        /// <remarks>
        /// 全てのキャリブレータ登録情報の削除を行います。
        /// </remarks>
        /// <returns></returns>
        public Boolean DeleteAll()
        {
            Boolean result = true;
            if ( this.DataTable != null )
            {
                foreach ( DataRow dat in this.DataTable.Rows )
                {
                    dat.Delete();
                }
            }
            return result;
        }

        /// <summary>
        /// マスターリストへの新規データの追加
        /// </summary>
        /// <remarks>
        /// マスターリストへの新規データの追加します
        /// </remarks>
        /// <param name="rackId">ラックID</param>
        /// <param name="rackPosition">ラックポジション</param>
        /// <param name="startRackId">開始ラックID</param>
        /// <param name="startRackPosition">開始ラックポジション</param>
        /// <param name="measureProtocolIndex">分析項目インデックス</param>
        /// <param name="reagentLotNo">試薬ロット番号</param>
        /// <param name="concentration">濃度</param>
        /// <param name="registDateTime">登録日時</param>
        /// <param name="calibLotNo">キャリブーレータロット番号</param>
        /// <param name="registeredModules">登録分析モジュール番号</param>
        /// <returns></returns>
        public CalibratorRegistData AddData( CalibRackID rackId, Int32 rackPosition, CalibRackID startRackId, Int32 startRackPosition, Int32 measureProtocolIndex, String reagentLotNo, String concentration, DateTime registDateTime, String calibLotNo, Int32 registeredModules )
        {
            if ( this.DataTable != null )
            {
                DataRow addRow = this.DataTable.Clone().NewRow();

                addRow[STRING_RACKID] = rackId.DispPreCharString;
                addRow[STRING_RACKPOSITION] = rackPosition;
                addRow[STRING_START_RACKID] = startRackId.DispPreCharString;
                addRow[STRING_START_RACKPOSITION] = startRackPosition;
                addRow[STRING_MEASUREPROTOCOLINDEX] = measureProtocolIndex;
                addRow[STRING_REAGENT_LOTNO] = reagentLotNo;
                addRow[STRING_CONCENTRATION] = concentration;
                addRow[STRING_REGIST_DATETIME] = registDateTime;
                addRow[STRING_CALIB_LOTNO] = calibLotNo;
                addRow[STRING_REGISTERED_MODULES] = registeredModules;

                CalibratorRegistData data = new CalibratorRegistData( addRow );
                new List<CalibratorRegistData> { data }.SyncDataListToDataTable( this.DataTable );
                return data;
            }
            return null;
        }

        /// <summary>
        /// DBのカラム追加
        /// <remarks>
        /// 今後、カラムの型変換を行う際は中のクエリを変更するような処理に変える
        /// </remarks>
        /// </summary>
        public void AddColumn()
        {
            // NULL不許可（デフォルト=1）で登録分析モジュール番号カラムを追加
            String addColumnSql = String.Format("alter table dbo.calibratorRegist add {0} int not null default '1'", CalibratorRegistDB.STRING_REGISTERED_MODULES);

            //　クエリ実行
            this.ExecuteSql(addColumnSql);
        }

        /// <summary>
        /// キャリブレータ登録情報テーブル取得
        /// </summary>
        /// <remarks>
        /// キャリブレータ登録情報をDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
        }

        /// <summary>
        /// キャリブレータ登録情報テーブル書込み
        /// </summary>
        /// <remarks>
        /// キャリブレータ登録情報をDBに書き込みます。
        /// </remarks>
        public void CommitData()
        {
            this.updateBaseTable();
        }

        #endregion
    }
}
