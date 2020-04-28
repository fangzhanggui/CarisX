using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.DB;
using Oelco.Common.Log;
using Oelco.Common.Utility;
using System.Data;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Const;
using Oelco.Common.Parameter;
using Oelco.CarisX.Log;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.DB
{
    /// <summary>
    /// 精度管理検体登録情報
    /// </summary>
    public class ControlRegistData : DataRowWrapperBase
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// 全ロット
        /// </summary>
        private const String STRING_LOTALL = "*";
        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public ControlRegistData( DataRowWrapperBase data )
            : base( data )
        {
            this.setRegisterdStatusString();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 精度管理検体登録ラックIDの取得
        /// </summary>
        public CarisXIDString RackID
        {
            get
            {
                return this.Field<String>( ControlRegistDB.STRING_RACKID );
            }
            protected set
            {
                this.SetField<String>( ControlRegistDB.STRING_RACKID, value.DispPreCharString );
            }
        }

        /// <summary>
        /// 精度管理検体登録ラックポジションの取得
        /// </summary>
        public Int32 RackPosition
        {
            get
            {
                return this.Field<Int32>( ControlRegistDB.STRING_RACKPOSITION );
            }
            protected set
            {
                this.SetField<Int32>( ControlRegistDB.STRING_RACKPOSITION, value );
            }
        }

        /// <summary>
        /// 精度管理検体ロットの取得、設定
        /// </summary>
        public String ControlLotNo
        {
            get
            {
                return this.Field<String>( ControlRegistDB.STRING_CONTROLLOTNO );
            }
            set
            {
                this.SetField<String>( ControlRegistDB.STRING_CONTROLLOTNO, value );
            }
        }

        /// <summary>
        /// 精度管理検体名の取得、設定
        /// </summary>
        public String ControlName
        {
            get
            {
                return this.Field<String>( ControlRegistDB.STRING_CONTROLNAME );
            }
            set
            {
                this.SetField<String>( ControlRegistDB.STRING_CONTROLNAME, value );
            }
        }

        /// <summary>
        /// コメントの取得、設定
        /// </summary>
        public String Comment
        {
            get
            {
                return this.Field<String>( ControlRegistDB.STRING_COMMENT );
            }
            set
            {
                this.SetField<String>( ControlRegistDB.STRING_COMMENT, value );
            }
        }

        /// <summary>
        /// 登録状態("分析項目1(試薬ロット番号)"...,"分析項目50(試薬ロット番号)")の取得
        /// </summary>
        public String RegisterdStatus
        {
            get;
            //{
            //    String measureProtocolInfo = String.Empty;

            //    var list = Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList.Select( ( measureProtocol ) =>
            //    {
            //        measureProtocolInfo = this.FieldOriginal<String>( ControlRegistDB.STRING_MEASUREPROTOCOL + ( Singleton<MeasureProtocolManager>.Instance.GetRoutineTableOrder( measureProtocol.ProtocolIndex ) ).ToString() );
            //        if ( measureProtocolInfo != null )
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            sb.Append( measureProtocol.ProtocolName );
            //            if ( measureProtocolInfo.Length > 0 )
            //            {
            //                sb.Append( "(" );
            //                if ( measureProtocolInfo == STRING_LOTALL )
            //                {
            //                    sb.Append( Oelco.CarisX.Properties.Resources.STRING_CONTROLREGIST_002 );
            //                }
            //                else
            //                {
            //                    sb.Append( measureProtocolInfo );
            //                }
            //                sb.Append( ")" );
            //            }
            //            return sb.ToString();
            //        }
            //        return null;
            //    } ).Where( ( item ) => !String.IsNullOrEmpty( item ) );

            //    return String.Join( ",", list );
            //}
            private set;
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 分析項目毎の登録情報の取得
        /// </summary>
        /// <remarks>
        /// 分析項目毎の登録情報の取得します
        /// </remarks>
        /// <returns>登録情報</returns>
        public List<RegistReagentLotInfo> GetRegisterdStatus()
        {
            List<RegistReagentLotInfo> result = new List<RegistReagentLotInfo>();
            Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList.ForEach( ( measureProtocol ) =>
            {
                // 分析項目毎の試薬ロット選択状態を取得
                var no = ( Singleton<MeasureProtocolManager>.Instance.GetRoutineTableOrder( measureProtocol.ProtocolIndex ) );
                String status = ( no > 0 && no <= CarisXConst.ROUTINE_TABLE_COUNT ) ? this.Field<String>( ControlRegistDB.STRING_MEASUREPROTOCOL + no.ToString() ) : null;

                if ( status != null )
                {
                    if ( status == String.Empty )       // 現ロット
                    {
                        result.Add( new RegistReagentLotInfo()
                        {
                            MeasureProtocolIndex = measureProtocol.ProtocolIndex,
                            SelectReagentLot = ReagentLotSelect.CurrentLot
                        } );
                    }
                    else if ( status == STRING_LOTALL ) // 全ロット
                    {
                        result.Add( new RegistReagentLotInfo()
                        {
                            MeasureProtocolIndex = measureProtocol.ProtocolIndex,
                            SelectReagentLot = ReagentLotSelect.LotAll
                        } );
                    }
                    else                                // ロット指定
                    {
                        result.Add( new RegistReagentLotInfo()
                        {
                            MeasureProtocolIndex = measureProtocol.ProtocolIndex,
                            SelectReagentLot = ReagentLotSelect.LotSpecification,
                            ReagentLotNo = status
                        } );
                    }
                }
            } );

            return result;
        }

        /// <summary>
        /// 分析項目ごとの登録情報の設定
        /// </summary>
        /// <remarks>
        /// 分析項目ごとの登録情報の設定します
        /// </remarks>
        /// <param name="setRegisterdStatus">登録情報</param>
        public void SetRegisterdStatus( List<RegistReagentLotInfo> setRegisterdStatus )
        {
            if ( setRegisterdStatus != null )
            {
                var dic = setRegisterdStatus.ToDictionary( ( status ) => status.MeasureProtocolIndex );

                Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList.ForEach( ( measureProtocol ) =>
                {
                    var routineTableOrder = Singleton<MeasureProtocolManager>.Instance.GetRoutineTableOrder( measureProtocol.ProtocolIndex );
                    if ( routineTableOrder > 0 )
                    {
                        String no = routineTableOrder.ToString();
                        if ( dic.ContainsKey( measureProtocol.ProtocolIndex ) )
                        {
                            RegistReagentLotInfo registData = dic[measureProtocol.ProtocolIndex];
                            switch ( registData.SelectReagentLot )
                            {
                            case ReagentLotSelect.CurrentLot:
                                this.SetField<String>( ControlRegistDB.STRING_MEASUREPROTOCOL + no, String.Empty );
                                break;
                            case ReagentLotSelect.LotSpecification:
                                this.SetField<String>( ControlRegistDB.STRING_MEASUREPROTOCOL + no, registData.ReagentLotNo );
                                break;
                            case ReagentLotSelect.LotAll:
                                this.SetField<String>( ControlRegistDB.STRING_MEASUREPROTOCOL + no, STRING_LOTALL );
                                break;
                            }
                        }
                        else
                        {
                            this.SetField<String>( ControlRegistDB.STRING_MEASUREPROTOCOL + no, null );
                        }
                    }
                } );

                this.setRegisterdStatusString();
            }
        }

        /// <summary>
        /// 登録状態文字列設定
        /// </summary>
        /// <remarks>
        /// 登録状態文字列設定します
        /// </remarks>
        private void setRegisterdStatusString()
        {
            String measureProtocolInfo = String.Empty;

            bool enabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();

            var list = Singleton<MeasureProtocolManager>.Instance.UseMeasureProtocolList.Select( ( measureProtocol ) =>
            {
                var routineTableOrder = Singleton<MeasureProtocolManager>.Instance.GetRoutineTableOrder( measureProtocol.ProtocolIndex );
                if ( routineTableOrder > 0 && routineTableOrder <= CarisXConst.ROUTINE_TABLE_COUNT )
                {
                    measureProtocolInfo = this.FieldOriginal<String>( ControlRegistDB.STRING_MEASUREPROTOCOL + routineTableOrder.ToString() );
                    if ( measureProtocolInfo != null )
                    {
                        StringBuilder sb = new StringBuilder();

                        // 急診使用によってグリッドから分析項目を非表示にする必要のない場合
                        if (( enabledFlag == false ) || ( measureProtocol.UseEmergencyMode == false ))
                        {
                            sb.Append(measureProtocol.ProtocolName);
                            if (measureProtocolInfo.Length > 0)
                            {
                                sb.Append("(");
                                if (measureProtocolInfo == STRING_LOTALL)
                                {
                                    sb.Append(Oelco.CarisX.Properties.Resources.STRING_CONTROLREGIST_002);
                                }
                                else
                                {
                                    sb.Append(measureProtocolInfo);
                                }
                                sb.Append(")");
                            }
                        }
                        return sb.ToString();
                    }
                }
                return null;
            } ).Where( ( item ) => !String.IsNullOrEmpty( item ) );

            this.RegisterdStatus = String.Join( ",", list );
        }

        /// <summary>
        /// 分析項目、ロット番号の登録追加
        /// </summary>
        /// <remarks>
        /// 分析項目、ロット番号の登録追加します
        /// </remarks>
        /// <param name="addRegisterdStatus"></param>
        /// <returns>true:追加成功/false:追加失敗</returns>
        public Boolean AddRegisterdStatus( RegistReagentLotInfo addRegisterdStatus )
        {
            String no = ( Singleton<MeasureProtocolManager>.Instance.GetRoutineTableOrder( addRegisterdStatus.MeasureProtocolIndex ) ).ToString();
            //if ( this.Field<String>( ControlRegistDB.STRING_MEASUREPROTOCOL + no ) != null )
            //{
            //    return false;
            //}

            switch ( addRegisterdStatus.SelectReagentLot )
            {
            case ReagentLotSelect.CurrentLot:
                this.SetField<String>( ControlRegistDB.STRING_MEASUREPROTOCOL + no, String.Empty );
                break;
            case ReagentLotSelect.LotSpecification:
                this.SetField<String>( ControlRegistDB.STRING_MEASUREPROTOCOL + no, addRegisterdStatus.ReagentLotNo );
                break;
            case ReagentLotSelect.LotAll:
                this.SetField<String>( ControlRegistDB.STRING_MEASUREPROTOCOL + no, STRING_LOTALL );
                break;
            }

            this.setRegisterdStatusString();
            return true;
        }

        /// <summary>
        /// 指定された分析項目の登録状態削除
        /// </summary>
        /// <remarks>
        /// 指定された分析項目の登録状態削除します
        /// </remarks>
        /// <param name="removeMeasureProtocolIndex">削除対象分析項目インデックス</param>
        /// <returns>true:削除成功/false:削除失敗(削除済み)</returns>
        public Boolean RemoveRegisterdStatus( Int32 removeMeasureProtocolIndex )
        {
            String no = ( Singleton<MeasureProtocolManager>.Instance.GetRoutineTableOrder( removeMeasureProtocolIndex ) ).ToString();
            if ( this.Field<String>( ControlRegistDB.STRING_MEASUREPROTOCOL + no ) == null )
            {
                return false;
            }

            this.SetField<String>( ControlRegistDB.STRING_MEASUREPROTOCOL + no, null );
            return true;
        }

        /// <summary>
        /// ラックIDの設定
        /// </summary>
        /// <remarks>
        /// ラックIDを設定します。
        /// </remarks>
        /// <param name="rackId">ラックID</param>
        public void SetRackID( CarisXIDString rackId )
        {
            this.RackID = rackId;
        }

        /// <summary>
        /// ラックポジションの設定
        /// </summary>
        /// <remarks>
        /// ラックポジションを設定します。
        /// </remarks>
        /// <param name="rackPosition">ラックポジション</param>
        public void SetRackPosition( Int32 rackPosition )
        {
            this.RackPosition = rackPosition;
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
            /// <summary>
            /// ラックID
            /// </summary>
            public const String RackID = "RackID";
            /// <summary>
            /// ラックポジション
            /// </summary>
            public const String RackPosition = "RackPosition";
            /// <summary>
            /// 精度管理検体ロット番号
            /// </summary>
            public const String ControlLotNo = "ControlLotNo";
            /// <summary>
            /// 精度管理検体名
            /// </summary>
            public const String ControlName = "ControlName";
            /// <summary>
            /// 登録状態
            /// </summary>
            public const String RegisterdStatus = "RegisterdStatus";
            /// <summary>
            /// コメント
            /// </summary>
            public const String Comment = "Comment";
        };

        /// <summary>
        /// 分析項目登録情報
        /// </summary>
        public struct RegistReagentLotInfo
        {
            /// <summary>
            /// 測定プロトコルインデックス
            /// </summary>
            public Int32 MeasureProtocolIndex;
            /// <summary>
            /// 選択試薬ロット
            /// </summary>
            public ReagentLotSelect SelectReagentLot;
            /// <summary>
            /// 試薬ロット番号
            /// </summary>
            public String ReagentLotNo;
        }
        #endregion
    }

    /// <summary>
    /// 精度管理検体登録情報DBクラス
    /// </summary>
    /// <remarks>
    /// 精度管理検体登録情報のアクセスを行うクラスです。
    /// </remarks>
    public class ControlRegistDB : DBAccessControl
    {

        #region [定数定義]

        /// <summary>
        /// ラックID(DBテーブル：controlRegist列名)
        /// </summary>
        public const String STRING_RACKID = "rackID";
        /// <summary>
        /// ラックポジション(DBテーブル：controlRegist列名)
        /// </summary>
        public const String STRING_RACKPOSITION = "rackPosition";
        /// <summary>
        /// 多重測定回数番号(DBテーブル：controlRegist列名)
        /// </summary>
        public const String STRING_RECEIPTNO = "receiptNo";
        /// <summary>
        /// 精度管理検体ロット番号(DBテーブル：controlRegist列名)
        /// </summary>
        public const String STRING_CONTROLLOTNO = "controlLotNo";
        /// <summary>
        /// 精度管理検体名(DBテーブル：controlRegist列名)
        /// </summary>
        public const String STRING_CONTROLNAME = "controlName";
        /// <summary>
        /// コメント(DBテーブル：controlRegist列名)
        /// </summary>
        public const String STRING_COMMENT = "comment";

        /// <summary>
        /// 分析項目、試薬ロット
        /// </summary>
        public const String STRING_MEASUREPROTOCOL = "measProt";


        #endregion      

        #region [プロパティ]

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT * FROM dbo.controlRegist";
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
        /// <param name="data">測定指示データ</param>
        public Boolean MeasureIndicate(int moduleId, List<AssaySchedule> assaySchedules, ref IMeasureIndicate data,ref int errorCode,ref int errorArg, ref int errorCollectionArg )
        {
            Boolean findData = false;

            CarisXIDString rackId = data.RackID;
            Int32 rackPos = data.SamplePosition;

            var result = from v in this.GetData()
                         where v.RackID.DispPreCharString == rackId.DispPreCharString && v.RackPosition == rackPos
                         select v;
            
            // 結果セットにデータ追加
            if ( result.Count() != 0 )
            {
                // 問い合わせコマンド応答用情報
                List<MeasItem> measItemList = new List<MeasItem>();

                // 画面表示用データ構造を中間データとしてデータテーブルから取得
                ControlRegistData dbData = result.First();

                data.PreDil = 1; // 手希釈倍率 精度管理検体は固定で1(定数定義へ移動する)
                data.SpecimenMaterial = SpecimenMaterialType.BloodSerumAndPlasma; // サンプル種別 精度管理検体は固定で血
                data.SampleID = dbData.ControlLotNo;

                foreach ( var registerd in dbData.GetRegisterdStatus() )
                {
                    MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( registerd.MeasureProtocolIndex );

                    switch ( registerd.SelectReagentLot )
                    {
                        case ReagentLotSelect.CurrentLot:
                            {
                                // 試薬DBクラス利用で試薬ロット取得する
                                String reagentLotNo = Singleton<ReagentDB>.Instance.GetNowReagentLotNo(protocol.ReagentCode, moduleId: moduleId);// 試薬ロット番号
                                if (String.IsNullOrEmpty(reagentLotNo))
                                {
                                    if (moduleId == CarisXConst.ALL_MODULEID)
                                    {
                                        //全モジュールをチェック時に無かった場合はエラー
                                        errorCode = 203;
                                        errorArg = protocol.ProtocolNo;
                                        errorCollectionArg = (Int32)ErrorCollectKind.ReagentError;
                                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", "[90-2]Insufficient reagent or no reagent information.");
                                        return false;
                                    }
                                    else
                                    {
                                        //特定のモジュールをチェック時は他のモジュールにあるかもしれないのでエラーにしない
                                        continue;
                                    }
                                }
                                MeasItem item = new MeasItem();
                                item.AutoDil = 1; // 後希釈倍率 精度管理検体は固定で1（定数定義へ移動する）
                                item.ProtoNo = protocol.ProtocolNo;
                                item.RepCount = protocol.RepNoForControl;
                                item.TurnNo = Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.GetTurnOrder(protocol.ProtocolName);
                                item.UniqNo = 1;
                                item.ReagentLotNo = reagentLotNo;
                                item.UseCurrentLot = true; // 現ロット使用フラグON

                                if (moduleId == CarisXConst.ALL_MODULEID)
                                {
                                    //全モジュールを対象に処理した場合

                                    item.ReagentLotNo = String.Empty;   //ロット番号が正しいかわからないので空白扱いにする
                                    measItemList.Add(item);
                                }
                                else
                                {
                                    //特定のモジュールを対象に処理した場合

                                    if (assaySchedules == null
                                        || (assaySchedules != null && assaySchedules.Count != 0 
                                            && assaySchedules.Exists(v => v.ExistsSchedule(item.ProtoNo, String.Empty, moduleId))))
                                    {
                                        //Rack No Use（分析予定がNull）、または分析予定がある場合は登録する

                                        item.UniqNo = Singleton<UniqueNo>.Instance.CreateNumber(); /* ユニーク番号生成 */
                                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【検体追加】 Unique number = {0} Rack = {1}-{2} ID = {3}", item.UniqNo, data.RackID, data.SamplePosition, data.SampleID));

                                        measItemList.Add(item);
                                    }
                                }
                                break;
                            }
                        case ReagentLotSelect.LotSpecification:
                            {
                                String reagentLotNo = Singleton<ReagentDB>.Instance.GetLotSpecificationNo(protocol.ReagentCode, registerd.ReagentLotNo, moduleId);
                                if (String.IsNullOrEmpty(reagentLotNo))
                                {
                                    if (moduleId == CarisXConst.ALL_MODULEID)
                                    {
                                        //全モジュールをチェック時に無かった場合はエラー
                                        errorCode = 204;
                                        errorArg = protocol.ProtocolNo;
                                        errorCollectionArg = (Int32)ErrorCollectKind.ReagentError;
                                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", "[90-2]Insufficient reagent or no reagent information.");
                                        return false;
                                    }
                                    else
                                    {
                                        //特定のモジュールをチェック時は他のモジュールにあるかもしれないのでエラーにしない
                                        continue;
                                    }
                                }
                                MeasItem item = new MeasItem();
                                item.AutoDil = 1; // 後希釈倍率 精度管理検体は固定で1（定数定義へ移動する）
                                item.ProtoNo = protocol.ProtocolNo;
                                item.RepCount = protocol.RepNoForControl;
                                item.TurnNo = Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.GetTurnOrder(protocol.ProtocolName);
                                item.UniqNo = 1;
                                item.ReagentLotNo = reagentLotNo;

                                if (moduleId == CarisXConst.ALL_MODULEID)
                                {
                                    //全モジュールを対象に処理した場合

                                    measItemList.Add(item);
                                }
                                else
                                {
                                    //特定のモジュールを対象に処理した場合

                                    if (assaySchedules == null
                                        || (assaySchedules != null && assaySchedules.Count != 0 
                                            && assaySchedules.Exists(v => v.ExistsSchedule(item.ProtoNo, item.ReagentLotNo, moduleId))))
                                    {
                                        //Rack No Use（分析予定がNull）、または分析予定がある場合は登録する

                                        item.UniqNo = Singleton<UniqueNo>.Instance.CreateNumber(); /* ユニーク番号生成 */
                                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【検体追加】 Unique number = {0} Rack = {1}-{2} ID = {3}", item.UniqNo, data.RackID, data.SamplePosition, data.SampleID));

                                        measItemList.Add(item);
                                    }
                                }
                                break;
                            }
                        case ReagentLotSelect.LotAll:
                            foreach ( var lotNo in HybridDataMediator.SearchAllReagentLotFromReagentDB( protocol.ReagentCode, moduleId) )
                            {
                                MeasItem reagentLotitem = new MeasItem();
                                reagentLotitem.AutoDil = 1; // 後希釈倍率 精度管理検体は固定で1（定数定義へ移動する）
                                reagentLotitem.ProtoNo = protocol.ProtocolNo;
                                reagentLotitem.RepCount = protocol.RepNoForControl;
                                reagentLotitem.TurnNo = Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.GetTurnOrder( protocol.ProtocolName );
                                reagentLotitem.UniqNo = 1;
                                reagentLotitem.ReagentLotNo = lotNo;

                                if (moduleId == CarisXConst.ALL_MODULEID)
                                {
                                    //全モジュールを対象に処理した場合

                                    measItemList.Add(reagentLotitem);
                                }
                                else
                                {
                                    //特定のモジュールを対象に処理した場合

                                    if (assaySchedules == null
                                        || (assaySchedules != null && assaySchedules.Count != 0 
                                            && assaySchedules.Exists(v => v.ExistsSchedule(reagentLotitem.ProtoNo, reagentLotitem.ReagentLotNo, moduleId))))
                                    {
                                        //Rack No Use（分析予定がNull）、または分析予定がある場合は登録する

                                        reagentLotitem.UniqNo = Singleton<UniqueNo>.Instance.CreateNumber(); /* ユニーク番号生成 */
                                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【検体追加】 Unique number = {0} Rack = {1}-{2} ID = {3}", reagentLotitem.UniqNo, data.RackID, data.SamplePosition, data.SampleID));

                                        measItemList.Add(reagentLotitem);
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
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
                errorCollectionArg = (Int32)ErrorCollectKind.RegisterError;
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", "[90-3]There is no registration information.");
            }
            return findData;
        }

        /// <summary>
        /// 精度管理検体登録情報テーブルの取得
        /// </summary>
        /// <remarks>
        /// 精度管理検体登録情報テーブルを取得します。
        /// </remarks>
        /// <returns></returns>
        public List<ControlRegistData> GetData()
        {
            List<ControlRegistData> result = new List<ControlRegistData>();

            if ( this.DataTable != null )
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    var datas = from v in dataTableList
                                let data = new ControlRegistData( v )
                                orderby data.RackID.Value ascending, data.RackPosition ascending
                                select data;
                    result = datas.ToList();
                }
                catch ( Exception ex )
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
        /// 精度管理検体登録データの設定
        /// </summary>
        /// <remarks>
        /// 精度管理検体登録データの同期を行います。
        /// </remarks>
        /// <param name="list">変更、削除操作済みデータ</param>
        public void SetData( List<ControlRegistData> list )
        {
            list.SyncDataListToDataTable( this.DataTable );
        }

        /// <summary>
        ///  精度管理検体登録データの追加
        /// </summary>
        /// <remarks>
        /// 精度管理検体登録データの追加を行います。
        /// </remarks>
        /// <param name="masterList">マスターリスト</param>
        /// <param name="rackId">ラックID</param>
        /// <param name="rackPosition">ラックポジション</param>
        /// <param name="controlLot">精度管理検体ロット</param>
        /// <param name="controlName">精度管理検体名</param>
        /// <param name="setRegisterdStatus">分析項目(試薬ロット)情報</param>
        public ControlRegistData AddData( ref List<ControlRegistData> masterList, ControlRackID rackId, Int32 rackPosition, String controlLot, String controlName, Int32 receiptNo, List<ControlRegistData.RegistReagentLotInfo> setRegisterdStatus )
        {
            if ( this.DataTable != null )
            {
                DataRow addRow = this.DataTable.Clone().NewRow();

                addRow[ControlRegistDB.STRING_RACKID] = rackId.DispPreCharString;
                addRow[ControlRegistDB.STRING_RACKPOSITION] = rackPosition;
                addRow[ControlRegistDB.STRING_CONTROLLOTNO] = controlLot;
                addRow[ControlRegistDB.STRING_CONTROLNAME] = controlName;

                ControlRegistData data = new ControlRegistData( addRow );

                data.SetRegisterdStatus( setRegisterdStatus );

                data.AddToDataList( masterList );

                return data;
            }
            return null;
        }


        /// <summary>
        /// 精度管理検体登録情報テーブル取得
        /// </summary>
        /// <remarks>
        /// 精度管理検体登録情報をDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
        }

        /// <summary>
        /// 精度管理検体登録情報テーブル書込み
        /// </summary>
        /// <remarks>
        /// 精度管理検体登録情報をDBに書き込みます。
        /// </remarks>
        public void CommitData()
        {
            this.updateBaseTable();
        }

        #endregion

    }
}
