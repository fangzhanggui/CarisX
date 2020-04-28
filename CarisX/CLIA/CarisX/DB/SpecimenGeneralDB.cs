using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;
using Oelco.CarisX.Utility;

using Oelco.Common.Log;
using Oelco.CarisX.Const;
using Oelco.Common.DB;
using Oelco.CarisX.Parameter;
using System.Data;
using Oelco.Common.Parameter;
using Oelco.CarisX.Common;
using Oelco.CarisX.Log;

//using LinkKey = Tuple<String, Int32, String>;

namespace Oelco.CarisX.DB
{
    /// <summary>
    /// 検体グリッド表示内容
    /// </summary>
    public class SpecimenRegistrationGridViewDataSet
    {
        /// <summary>
        /// ラックID
        /// </summary>
        public CarisXIDString RackID = String.Empty;
        /// <summary>
        /// ラックポジション
        /// </summary>
        public Int32 RackPosition = 0;
        /// <summary>
        /// 受付番号
        /// </summary>
        public Int32 ReceiptNumber = 0;
        /// <summary>
        /// 検体ID
        /// </summary>
        public String PatientID = String.Empty;
        /// <summary>
        /// 検体種別
        /// </summary>
        public SpecimenMaterialType SpecimenType;
        /// <summary>
        /// 分析項目リスト（分析項目インデックス,稀釈倍率,測定回数)
        /// </summary>
        public List<Tuple<Int32?, Int32?, Int32?>> Registered = new List<Tuple<Int32?, Int32?, Int32?>>();
        /// <summary>
        /// 手稀釈倍率
        /// </summary>
        public Int32 ManualDil = 0;
        /// <summary>
        /// コメント
        /// </summary>
        public String Comment = String.Empty;

    }

    /// <summary>
    /// 検体DBアクセスクラス
    /// </summary>
    public class SpecimenGeneralDB : DBAccessControl
    {
        /// <summary>
        /// データテーブル-表示内容リンク情報
        /// </summary>
        private Dictionary<Tuple<String, Int32, String>, DataRow> dataLinkDic = new Dictionary<Tuple<String, Int32, String>, DataRow>();

        #region [プロパティ]

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT * FROM dbo.SpecimenGeneralRegist";
            }
        }

        #endregion

        #region [定数定義]

        /// <summary>
        /// 検体区分(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_SAMPLEMEASKIND = "sampleMeasKind";
        /// <summary>
        /// ラックID(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_RACKID = "rackID";
        /// <summary>
        /// ラックポジション(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_RACKPOSITION = "rackPosition";
        /// <summary>
        /// 受付番号(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_RECEIPTNO = "receiptNo";
        /// <summary>
        /// 検体ID(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_SAMPLEID = "sampleID";
        /// <summary>
        /// 検体種別(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_SPECIMENMATERIALTYPE = "specimenMaterialType";
        /// <summary>
        /// ステータス(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_STATUS = "status";
        /// <summary>
        /// 手希釈倍率(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MANUALDILUTION = "manualDilution";
        /// <summary>
        /// コメント(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_COMMENT = "comment";
        /// <summary>
        /// 分析項目（共通文字）
        /// </summary>
        public const String STRING_MEASPROT = "measProt";
        /// <summary>
        /// 分析項目1(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT1 = STRING_MEASPROT + "1";
        /// <summary>
        /// 分析項目2(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT2 = STRING_MEASPROT + "2";
        /// <summary>
        /// 分析項目3(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT3 = STRING_MEASPROT + "3";
        /// <summary>
        /// 分析項目4(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT4 = STRING_MEASPROT + "4";
        /// <summary>
        /// 分析項目5(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT5 = STRING_MEASPROT + "5";
        /// <summary>
        /// 分析項目6(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT6 = STRING_MEASPROT + "6";
        /// <summary>
        /// 分析項目7(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT7 = STRING_MEASPROT + "7";
        /// <summary>
        /// 分析項目8(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT8 = STRING_MEASPROT + "8";
        /// <summary>
        /// 分析項目9(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT9 = STRING_MEASPROT + "9";
        /// <summary>
        /// 分析項目10(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT10 = STRING_MEASPROT + "10";
        /// <summary>
        /// 分析項目11(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT11 = STRING_MEASPROT + "11";
        /// <summary>
        /// 分析項目12(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT12 = STRING_MEASPROT + "12";
        /// <summary>
        /// 分析項目13(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT13 = STRING_MEASPROT + "13";
        /// <summary>
        /// 分析項目14(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT14 = STRING_MEASPROT + "14";
        /// <summary>
        /// 分析項目15(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT15 = STRING_MEASPROT + "15";
        /// <summary>
        /// 分析項目16(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT16 = STRING_MEASPROT + "16";
        /// <summary>
        /// 分析項目17(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT17 = STRING_MEASPROT + "17";
        /// <summary>
        /// 分析項目18(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT18 = STRING_MEASPROT + "18";
        /// <summary>
        /// 分析項目19(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT19 = STRING_MEASPROT + "19";
        /// <summary>
        /// 分析項目20(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT20 = STRING_MEASPROT + "20";
        /// <summary>
        /// 分析項目21(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT21 = STRING_MEASPROT + "21";
        /// <summary>
        /// 分析項目22(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT22 = STRING_MEASPROT + "22";
        /// <summary>
        /// 分析項目23(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT23 = STRING_MEASPROT + "23";
        /// <summary>
        /// 分析項目24(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT24 = STRING_MEASPROT + "24";
        /// <summary>
        /// 分析項目25(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT25 = STRING_MEASPROT + "25";
        /// <summary>
        /// 分析項目26(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT26 = STRING_MEASPROT + "26";
        /// <summary>
        /// 分析項目27(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT27 = STRING_MEASPROT + "27";
        /// <summary>
        /// 分析項目28(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT28 = STRING_MEASPROT + "28";
        /// <summary>
        /// 分析項目29(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT29 = STRING_MEASPROT + "29";
        /// <summary>
        /// 分析項目30(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT30 = STRING_MEASPROT + "30";
        /// <summary>
        /// 分析項目31(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT31 = STRING_MEASPROT + "31";
        /// <summary>
        /// 分析項目32(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT32 = STRING_MEASPROT + "32";
        /// <summary>
        /// 分析項目33(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT33 = STRING_MEASPROT + "33";
        /// <summary>
        /// 分析項目34(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT34 = STRING_MEASPROT + "34";
        /// <summary>
        /// 分析項目35(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT35 = STRING_MEASPROT + "35";
        /// <summary>
        /// 分析項目36(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT36 = STRING_MEASPROT + "36";
        /// <summary>
        /// 分析項目37(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT37 = STRING_MEASPROT + "37";
        /// <summary>
        /// 分析項目38(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT38 = STRING_MEASPROT + "38";
        /// <summary>
        /// 分析項目39(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT39 = STRING_MEASPROT + "39";
        /// <summary>
        /// 分析項目40(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT40 = STRING_MEASPROT + "40";
        /// <summary>
        /// 分析項目41(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT41 = STRING_MEASPROT + "41";
        /// <summary>
        /// 分析項目42(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT42 = STRING_MEASPROT + "42";
        /// <summary>
        /// 分析項目43(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT43 = STRING_MEASPROT + "43";
        /// <summary>
        /// 分析項目44(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT44 = STRING_MEASPROT + "44";
        /// <summary>
        /// 分析項目45(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT45 = STRING_MEASPROT + "45";
        /// <summary>
        /// 分析項目46(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT46 = STRING_MEASPROT + "46";
        /// <summary>
        /// 分析項目47(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT47 = STRING_MEASPROT + "47";
        /// <summary>
        /// 分析項目48(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT48 = STRING_MEASPROT + "48";
        /// <summary>
        /// 分析項目49(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT49 = STRING_MEASPROT + "49";
        /// <summary>
        /// 分析項目50(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_MEASPROT50 = STRING_MEASPROT + "50";
        /// <summary>
        /// カップボリューム(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_CUPVOL = "cupVol";
        /// <summary>
        /// チューブボリューム(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_TUBEVOL = "tubeVol";
        /// <summary>
        /// 検体番号(DBテーブル：specimenGeneralRegist列名)
        /// </summary>
        public const String STRING_SAMPLENO = "sampleNo";

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 最大件数到達フラグ　取得
        /// </summary>
        public Boolean IsRecordMax
        {
            get
            {
                return this.dataLinkDic.Count >= this.RecordCountMax;
            }
        }

        /// <summary>
        /// 測定指示
        /// </summary>
        /// <remarks>
        /// スレーブからの測定指示データ問合せ応答データの検索を行います。
        /// 検索に必要な情報を保持するオブジェクトと、結果を格納するオブジェクトは同一です。
        /// </remarks>
        /// <param name="askType">問合せ法式</param>
        /// <param name="moduleId"></param>
        /// <param name="assaySchedules"></param>
        /// <param name="flgChkReagent"></param>
        /// <param name="data">測定指示データ</param>
        /// <param name="receiptNumber">受付番号</param>
        /// <param name="comment">コメント</param>
        /// <param name="errorArg">エラー引数</param>
        /// <returns>True:取得成功　False:取得失敗</returns
        public Boolean MeasureIndicate( AskTypeKind askType
                                      , int moduleId
                                      , List<AssaySchedule> assaySchedules
                                      , Boolean flgChkReagent
                                      , ref IMeasureIndicate data
                                      , out Int32 receiptNumber
                                      , out String comment
                                      , ref Int32 errorArg
                                      , SpecimenCupKind cupKind )
        {
            // 検体検索
            // →この関数ではしない  // 検体にシーケンス番号付与(→DBへ)
            // 検体に検体識別番号付与(→DBへ)
            // 分析項目毎にユニーク番号付与(→この時点ではメモリ保持のみ)

            Boolean findData = false;
            receiptNumber = 0;
            comment = String.Empty;


            String searchId = data.SampleID;
            CarisXIDString rackId = data.RackID;
            Int32 rackPos = data.SamplePosition;

            // データ検索
            IEnumerable<DataRow> result = null;

            //　コピーデータリストを取得
            var dataTableList = this.DataTable.AsEnumerable().ToList();

            // ラックID検索・サンプルID検索
            // 既に読込まれたテーブルデータより検索を行う。
            switch (askType)
            {

                case AskTypeKind.RackID:
                    result = from v in dataTableList
                             where rackId.DispPreCharString == (v[STRING_RACKID] == null ? String.Empty : v[STRING_RACKID].ToString()) &&
                             rackPos == (v[STRING_RACKPOSITION] == null ? 0 : (Int32)v[STRING_RACKPOSITION])
                             select v;

                    break;

                case AskTypeKind.SampleID:
                    result = from v in dataTableList
                             where searchId == v[STRING_SAMPLEID].ToString() 
                                || rackId.DispPreCharString == (v[STRING_RACKID] == null ? String.Empty : v[STRING_RACKID].ToString()) 
                                && rackPos == (v[STRING_RACKPOSITION] == null ? 0 : (Int32)v[STRING_RACKPOSITION])
                             select v;

                    break;
            }

            // 結果セットにデータ追加
            if (result.Count() != 0)
            {
                // 問い合わせコマンド応答用情報
                List<MeasItem> measItemList = new List<MeasItem>();
                // 画面表示用データ構造を中間データとしてデータテーブルから取得
                SpecimenRegistrationGridViewDataSet viewData = this.getDataStruct(result.First());
                data.PreDil = viewData.ManualDil;

                if (!string.IsNullOrEmpty(viewData.PatientID))
                {
                    if (viewData.PatientID.Trim().Length != 0)
                    {
                        data.SampleID = viewData.PatientID.Trim();
                    }
                }
                data.SpecimenMaterial = viewData.SpecimenType;

                foreach (Tuple<Int32?, Int32?, Int32?> registerd in viewData.Registered)
                {
                    if (registerd.Item1.HasValue && registerd.Item2.HasValue)
                    {
                        MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(registerd.Item1.Value);

                        if (protocol.IsIGRA)
                        {
                            // 既に登録済みの分析項目があれば、IGRAのみ受け付け可とするため、クリアする
                            measItemList.Clear();

                            // ポジションチェック
                            if ((data.SamplePosition == 1)
                             || (data.SamplePosition == 5))
                            {
                                // ポジション1およびポジション5は不定な登録であるため、無視する
                                break;
                            }
                            else
                            {
                                // その他のポジションの場合、カップ種別をチェック
                                if ((cupKind == SpecimenCupKind.None)
                                    ||(cupKind == SpecimenCupKind.Cup))
                                {
                                    // カップに設定
                                    data.SpecimenCup = SpecimenCupKind.Cup;

                                }
                                else
                                {
                                    // IGRAはカップ以外を受け付けないため、無効とする
                                    break;
                                }
                            }
                        }
                        else
                        {
                            // その他の分析項目の場合
                            if (cupKind == SpecimenCupKind.None)
                            {
                                // カップ無し時は無視する
                                continue;
                            }
                        }

                        MeasItem item = new MeasItem();

                        // 試薬の急診使用有の場合
                        if (protocol.UseEmergencyMode == true)
                        {
                            // 急診使用ありの場合は希釈倍率を1に固定する
                            item.AutoDil = 1;
                        }
                        // 試薬の急診使用無の場合
                        else
                        {
                            item.AutoDil = registerd.Item2.Value; // 後希釈倍率
                        }
                        item.ProtoNo = protocol.ProtocolNo;
                        item.RepCount = registerd.Item3.Value;  //測定回数
                        item.TurnNo = Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.GetTurnOrder(protocol.ProtocolName);
                        item.UniqNo = 1;

                        // 試薬DBクラス利用で試薬ロット取得する
                        if (flgChkReagent)
                        {
                            item.ReagentLotNo = Singleton<ReagentDB>.Instance.GetNowReagentLotNo(protocol.ReagentCode, moduleId: moduleId);// 試薬ロット番号                         
                            if (item.ReagentLotNo != String.Empty)
                            {
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
                                        || (assaySchedules != null && assaySchedules.Count != 0 && assaySchedules.Exists(v => v.ExistsSchedule(item.ProtoNo, String.Empty))))
                                    {
                                        //Rack No Use（分析予定がNull）、または分析予定がある場合は登録する

                                        item.UniqNo = Singleton<UniqueNo>.Instance.CreateNumber(); /* ユニーク番号生成 */
                                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【検体追加】 Unique number = {0} Rack = {1}-{2} ID = {3}", item.UniqNo, data.RackID, data.SamplePosition, data.SampleID));

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
                        }
                        else
                        {
                            item.ReagentLotNo = String.Empty;
                            measItemList.Add(item);
                        }

                        // IGRAの登録が存在している場合、他のプロトロルの登録情報は受け付けない
                        if (protocol.IsIGRA)
                        {
                            break;
                        }
                    }
                }

                if (measItemList.Count != 0)
                {
                    // 測定項目を応答データに設定
                    data.MeasItemCount = measItemList.Count;
                    data.MeasItemArray = measItemList.ToArray();
                    receiptNumber = viewData.ReceiptNumber;
                    comment = viewData.Comment;
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
        /// DB行
        /// </summary>
        /// <param name="rackId"></param>
        /// <param name="rackPos"></param>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        protected IEnumerable<Tuple<String, Int32, String>> seqrchLinkKey(CarisXIDString rackId, Int32 rackPos, String sampleId)
        {
            IEnumerable<Tuple<String, Int32, String>> searched = null;
            if (sampleId.Trim().Length != 0)
            {
                // 検体ID分析時に行われる検索
                searched = from v in this.dataLinkDic.Keys
                           where (v.Item3 == sampleId)
                           select v;
            }
            else
            {
                // ラックID分析時・検体ID分析時(ラック位置も登録のデータ)に行われる検索
                searched = from v in this.dataLinkDic.Keys
                           where (v.Item1 == rackId.DispPreCharString && v.Item2 == rackPos)
                           select v;
            }
            return searched;
        }

        /// <summary>
        /// コメント検索
        /// </summary>
        /// <param name="rackId"></param>
        /// <param name="rackPos"></param>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        public String SearchComment(CarisXIDString rackId, Int32 rackPos, String sampleId)
        {
            String comment = String.Empty;

            // 現在読込まれているデータの中から、指定項目のコメントを検索します。
            if (this.DataTable != null)
            {
                var searched = seqrchLinkKey(rackId, rackPos, sampleId);
                if (searched.Count() != 0)
                {
                    comment = this.dataLinkDic[searched.First()].Field<String>(STRING_COMMENT);
                }
            }
            return comment;
        }

        /// <summary>
        /// 受付番号検索
        /// </summary>
        /// <param name="rackId"></param>
        /// <param name="rackPos"></param>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        public Int32 SearchReceiptNumber(CarisXIDString rackId, Int32 rackPos, String sampleId)
        {
            Int32 receiptNumber = 0;

            // 現在読込まれているデータの中から、指定項目の受付番号を検索します。
            if (this.DataTable != null)
            {
                var searched = seqrchLinkKey(rackId, rackPos, sampleId);
                if (searched.Count() != 0)
                {
                    receiptNumber = this.dataLinkDic[searched.First()].Field<Int32>(STRING_RECEIPTNO);
                }
            }
            return receiptNumber;
        }

        /// <summary>
        /// 検体登録画面表示情報取得
        /// </summary>
        /// <remarks>
        /// 検体登録テーブルから取得したデータを、検体登録画面表示出力向けに取得します。
        /// </remarks>
        /// <returns>検体登録画面表示データ</returns>
        public List<SpecimenRegistrationGridViewDataSet> GetDispData()
        {
            List<SpecimenRegistrationGridViewDataSet> result = new List<SpecimenRegistrationGridViewDataSet>();

            if (this.DataTable != null)
            {
                try
                {
                    // テーブル内容全て返す
                    foreach (System.Data.DataRow row in this.DataTable.Rows)
                    {
                        SpecimenRegistrationGridViewDataSet data = getDataStruct(row);
                        result.Add(data);
                    }
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }

            return result;

        }

        /// <summary>
        /// 検体登録画面表示情報取得
        /// </summary>
        /// <remarks>
        /// 検体登録テーブルから取得したデータを、検体登録画面表示出力向けに取得します。
        /// </remarks>
        /// <returns>検体登録画面表示データ</returns>
        public List<MeasureProtocol> GetRegisterdProtocols()
        {
            List<MeasureProtocol> result = new List<MeasureProtocol>();

            if (this.DataTable != null)
            {
                try
                {
                    // テーブル内容全て返す
                    foreach (System.Data.DataRow row in this.DataTable.Rows)
                    {
                        for (Int32 i = 0; i < CarisXConst.MEAS_PROTO_REGIST_MAX; i++)
                        {
                            // 分析項目が設定されていれば設定
                            // 設定されていない場合、空データとして設定
                            Int32? measProtValue = row.Field<Int32?>(STRING_MEASPROT + (i + 1).ToString());
                            Int32 protocolIndex = 0;
                            if (measProtValue.HasValue)
                            {
                                // DBのmeasProtoXXのXXはルーチンテーブル順序と一致する。
                                protocolIndex = Singleton<MeasureProtocolManager>.Instance.GetProtocolIndexFromRoutineTableOrder(i + 1);
                                MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(protocolIndex);
                                if (measureProtocol != null)
                                {
                                    result.Add(measureProtocol);
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }

            return result;

        }

        /// <summary>
        /// 登録分析項目・自動希釈倍率取得
        /// </summary>
        /// <remarks>
        /// 検体登録テーブルから取得したデータと自動希釈倍率を取得します
        /// </remarks>
        /// <returns>登録分析項目・自動希釈倍率</returns>
        public List<Tuple<MeasureProtocol, Int32>> GetRegisterdProtocolsAndAutoDil()
        {
            List<Tuple<MeasureProtocol, Int32>> result = new List<Tuple<MeasureProtocol, Int32>>();

            if (this.DataTable != null)
            {
                try
                {
                    // テーブル内容全て返す
                    foreach (System.Data.DataRow row in this.DataTable.Rows)
                    {
                        for (Int32 i = 0; i < CarisXConst.MEAS_PROTO_REGIST_MAX; i++)
                        {
                            // 分析項目が設定されていれば設定
                            // 設定されていない場合、空データとして設定
                            Int32? measProtValue = row.Field<Int32?>(STRING_MEASPROT + (i + 1).ToString());
                            Int32 dilutionRate = 0;
                            Int32 protocolIndex = 0;
                            if (measProtValue.HasValue)
                            {
                                var disMeasProtValue = disassemblyMeasProtValue(measProtValue.Value);
                                dilutionRate = disMeasProtValue.Item1;

                                // DBのmeasProtoXXのXXはルーチンテーブル順序と一致する。
                                protocolIndex = Singleton<MeasureProtocolManager>.Instance.GetProtocolIndexFromRoutineTableOrder(i + 1);
                                MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(protocolIndex);
                                if (measureProtocol != null)
                                {
                                    result.Add(new Tuple<MeasureProtocol, Int32>(measureProtocol, dilutionRate));
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    //Singleton<LogManager>.Instance.Error( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }

            return result;

        }
        /// <summary>
        /// データ構造取得
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private SpecimenRegistrationGridViewDataSet getDataStruct(DataRow row)
        {
            SpecimenRegistrationGridViewDataSet data = new SpecimenRegistrationGridViewDataSet();
            data.RackID = (String)row[STRING_RACKID];
            data.RackPosition = (Int32)row[STRING_RACKPOSITION];
            data.ReceiptNumber = (Int32)row[STRING_RECEIPTNO];
            data.PatientID = (String)row[STRING_SAMPLEID];
            data.SpecimenType = (SpecimenMaterialType)row[STRING_SPECIMENMATERIALTYPE];
            data.Comment = (String)row[STRING_COMMENT];
            data.ManualDil = (Int32)row[STRING_MANUALDILUTION];

            for (Int32 i = 0; i < CarisXConst.MEAS_PROTO_REGIST_MAX; i++)
            {
                // 分析項目が設定されていれば設定
                // 設定されていない場合、空データとして設定
                Int32? measProtValue = row.Field<Int32?>(STRING_MEASPROT + (i + 1).ToString());
                Int32? dilutionRate = null;
                Int32? measTimes = null;
                Int32? protocolIndex = null;

                if (measProtValue.HasValue)
                {
                    //値が設定されていた場合

                    var disMeasProtValue = disassemblyMeasProtValue(measProtValue.Value);
                    dilutionRate = disMeasProtValue.Item1;
                    measTimes = disMeasProtValue.Item2;

                    // DBのmeasProtoXXのXXはルーチンテーブル順序と一致する。
                    protocolIndex = Singleton<MeasureProtocolManager>.Instance.GetProtocolIndexFromRoutineTableOrder(i + 1);
                }

                data.Registered.Add(new Tuple<Int32?, Int32?, Int32?>(protocolIndex, dilutionRate, measTimes));
            }
            return data;
        }

        /// <summary>
        /// データ削除
        /// </summary>
        /// <remarks>
        /// 登録されている検体情報を指定して削除します。
        /// </remarks>
        /// <param name="deleteKeyList">削除情報リスト(ラックID,ラックポジション,検体ID)</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean DeleteData(List<Tuple<String, Int32, String>> deleteKeyList)
        {
            Boolean result = true;
            if (this.DataTable != null)
            {
                foreach (var deleteInfo in deleteKeyList)
                {
                    IEnumerable<Tuple<string, int, string>> keyFinder = null;
                    if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode == AssayModeParameter.AssayModeKind.RackID)
                    {
                        // ラックID・Posのみで削除する。
                        keyFinder = from v in this.dataLinkDic
                                    where v.Key.Item1 == deleteInfo.Item1 && v.Key.Item2 == deleteInfo.Item2
                                    select v.Key;

                    }
                    else if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode == AssayModeParameter.AssayModeKind.SampleID)
                    {
                        // ラックID・Posがある場合と無い場合を考慮する
                        keyFinder = from v in this.dataLinkDic
                                    where (v.Key.Item3 == deleteInfo.Item3 && v.Key.Item1 == deleteInfo.Item1 && v.Key.Item2 == deleteInfo.Item2)
                                    select v.Key;
                    }
                    if (keyFinder.Count() != 0)
                    {
                        // 既存データにIGRAが登録済みかチェック
                        bool bRegistedIGRA = false;

                        DataRow row = this.dataLinkDic[keyFinder.First()];
                        for (Int32 i = 0; i < CarisXConst.MEAS_PROTO_REGIST_MAX; i++)
                        {
                            // 分析項目が設定されており、測定指示と一致すれば、空データに設定
                            String measProtoColumnName = STRING_MEASPROT + (i + 1).ToString();

                            Int32? measProtValue = row.Field<Int32?>(measProtoColumnName);

                            // 値が設定されているか確認
                            if (measProtValue.HasValue)
                            {
                                // ルーチンテーブルから該当する分析項目Indexを取得
                                Int32 protocolIndex = Singleton<MeasureProtocolManager>.Instance.GetProtocolIndexFromRoutineTableOrder(i + 1);

                                // 分析項目Index => 分析項目情報へ変換
                                MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(protocolIndex);
                                if (protocol.IsIGRA)
                                {
                                    // IGRA項目あり
                                    bRegistedIGRA = true;
                                    break;
                                }
                            }
                        }

                        // IGRA項目が削除対象の場合
                        if (bRegistedIGRA)
                        {
                            // ポジション2,3,4を全て削除する
                            List<Int32> deletePositionList = new List<Int32>(){ 2, 3, 4 };
                            foreach( Int32 deletePosition in deletePositionList )
                            {
                                Tuple<String, Int32, String> deleteLinkKey = new Tuple<String, Int32, String>
                                    (keyFinder.First().Item1, deletePosition, keyFinder.First().Item3);
                                this.deleteDataLincDic(deleteLinkKey);
                            }
                        }
                        else
                        {
                            // 対象データをデータリンクから削除
                            this.dataLinkDic[keyFinder.First()].Delete();
                        }
                    }
                }
                //this.createDataLink();
            }
            return result;
        }

        /// <summary>
        /// データ削除（測定指示問合せ用）
        /// </summary>
        /// <remarks>
        /// 登録されている検体情報を指定して削除します。
        /// </remarks>
        /// <param name="deleteKeyList">削除情報リスト(ラックID,ラックポジション,検体ID)</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean DeleteData( IMeasureIndicate measIndicateData )
        {
            Boolean result = true;
            if (this.DataTable != null)
            {
                // 削除情報取得
                Tuple<String, Int32, String> deleteInfo = new Tuple<String, Int32, String>( measIndicateData.RackID
                                                                                          , measIndicateData.SamplePosition
                                                                                          , measIndicateData.SampleID );

                // 分析方式によって取得方法切り替え
                IEnumerable<Tuple<string, int, string>> keyFinder = null;
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode == AssayModeParameter.AssayModeKind.RackID)
                {
                    // ラックID・Posのみで削除する。
                    keyFinder = from v in this.dataLinkDic
                                where v.Key.Item1 == deleteInfo.Item1 && v.Key.Item2 == deleteInfo.Item2
                                select v.Key;

                }
                else if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.AssayMode == AssayModeParameter.AssayModeKind.SampleID)
                {
                    // ラックID・Posがある場合と無い場合を考慮する
                    keyFinder = from v in this.dataLinkDic
                                where ( v.Key.Item3 == deleteInfo.Item3 ) || ( v.Key.Item1 == deleteInfo.Item1 && v.Key.Item2 == deleteInfo.Item2 )
                                select v.Key;
                }

                // 削除対象がある場合
                if (keyFinder.Count() != 0)
                {
                    int registeredCount = 0;

                    // 登録済み分析項目を書き換え
                    DataRow row = this.dataLinkDic[keyFinder.First()];
                    for (Int32 i = 0; i < CarisXConst.MEAS_PROTO_REGIST_MAX; i++)
                    {
                        // 分析項目が設定されており、測定指示と一致すれば、空データに設定
                        String measProtoColumnName = STRING_MEASPROT + ( i + 1 ).ToString();

                        Int32? measProtValue = row.Field<Int32?>( measProtoColumnName );

                        // 値が設定されているか確認
                        if (measProtValue.HasValue)
                        {
                            // 登録情報カウントをインクリメント
                            registeredCount++;

                            // ルーチンテーブルから該当する分析項目Indexを取得
                            Int32 protocolIndex = Singleton<MeasureProtocolManager>.Instance.GetProtocolIndexFromRoutineTableOrder( i + 1 );

                            // 分析項目Index => 分析項目番号へ変換
                            MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex( protocolIndex );
                           
                            // 登録情報と比較
                            foreach ( var measItem in measIndicateData.MeasItemArray )
                            {
                                // 一致する分析項目の登録があるか確認
                                if( measItem.ProtoNo == protocol.ProtocolNo)
                                {
                                    // nullに設定
                                    row.SetField<Int32?>( measProtoColumnName, null );
                                    break;
                                }
                            }
                        }
                    }

                    // 登録情報と指示問合せ
                    if(registeredCount == measIndicateData.MeasItemArray.Count())
                    {
                        this.dataLinkDic[keyFinder.First()].Delete();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// データ全削除
        /// </summary>
        /// <remarks>
        /// 登録されている検体情報を全て削除します。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean DeleteAll()
        {
            Boolean result = true;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            this.ExecuteSql("DELETE FROM dbo.SpecimenGeneralRegist");

            sw.Stop();
            // 一般検体登録DB削除時間
            System.Diagnostics.Debug.WriteLine(String.Format("SpecimenGeneralRegist delete time:{0}msec", sw.ElapsedMilliseconds));
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                , CarisXLogInfoBaseExtention.Empty, String.Format("SpecimenGeneralRegist delete time:{0}msec", sw.ElapsedMilliseconds));
            sw.Reset();
            sw.Start();
            this.LoadDB();
            sw.Stop();
            // DBロード時間
            System.Diagnostics.Debug.WriteLine(String.Format("DB load time:{0}msec", sw.ElapsedMilliseconds));
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                , CarisXLogInfoBaseExtention.Empty, String.Format("DB load time:{0}msec", sw.ElapsedMilliseconds));

            return result;
        }

        /// <summary>
        /// 問い合わせ検体情報設定
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public Boolean SetAskData(AskWorkSheetData data)
        {

            Boolean result = true;
            if (this.DataTable != null)
            {
                try
                {
                    Tuple<String, Int32, String> linkKey = null;

                    GeneralRackID rackId = (GeneralRackID)data.FromHostCommand.RackID;

                    linkKey = new Tuple<String, Int32, String>(((GeneralRackID)data.FromHostCommand.RackID).DispPreCharString, data.FromHostCommand.RackPos, data.FromHostCommand.SampleID);

                    // 追加
                    DataRow newRow = this.DataTable.NewRow();

                    newRow.BeginEdit();

                    newRow[STRING_RACKID] = ((GeneralRackID)data.FromHostCommand.RackID).DispPreCharString;
                    newRow[STRING_RACKPOSITION] = data.FromHostCommand.RackPos;
                    newRow[STRING_RECEIPTNO] = data.FromHostCommand.ReceiptNumber;
                    newRow[STRING_SAMPLEID] = data.FromHostCommand.SampleID;
                    newRow[STRING_SPECIMENMATERIALTYPE] = data.FromHostCommand.SampleMaterialType.GetDPRSpecimemMaterialType();
                    newRow[STRING_COMMENT] = data.FromHostCommand.Comment;
                    newRow[STRING_SAMPLEMEASKIND] = data.FromHostCommand.SampleType.GetDPRSampleKind();
                    newRow[STRING_SAMPLENO] = 0; // TODO:不使用Field
                    newRow[STRING_MANUALDILUTION] = data.FromHostCommand.ManualDil;

                    Int32 routineTableOrder = 0;

                    // プロトコル番号変換しつつ登録
                    Boolean noRegist = true;
                    foreach (var hostProtocolInfo in data.FromHostCommand.MeasItems)
                    {
                        MeasureProtocol protocol = Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param
                            .GetDPRProtocolFromHostProtocolNumber(hostProtocolInfo.protoNo, Singleton<MeasureProtocolManager>.Instance);

                        Int32 autoDil = hostProtocolInfo.afterDil.GetDPRAutoDilution();
                        if (autoDil != 0 && protocol != null)
                        {
                            noRegist = false;
                            // プロトコルインデックス→ルーチンテーブル順序
                            routineTableOrder = Singleton<MeasureProtocolManager>.Instance.GetRoutineTableOrder(protocol.ProtocolIndex);
                            newRow[STRING_MEASPROT + routineTableOrder] = assemblyMeasProtValue(autoDil, protocol.RepNoForSample);
                        }
                    }

                    newRow.EndEdit();
                    if (!noRegist)
                    {
                        this.DataTable.Rows.Add(newRow);
                        this.dataLinkDic[linkKey] = newRow;
                    }
                }
                catch (Exception ex)
                {
                    // 不正データ
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// データリンク削除
        /// </summary>
        /// <param name="linkKey">データリンクキー</param>
        /// <param name="data">編集データ</param>
        /// <param name="bReplaceReceiptNo">受付番号置き換えフラグ</param>
        private void deleteDataLincDic(Tuple<String, Int32, String> linkKey)
        {
            if (this.dataLinkDic != null)
            {
                // リンクキーのデータが存在する場合
                if(this.dataLinkDic.ContainsKey(linkKey))
                {
                    // 削除
                    this.dataLinkDic[linkKey].Delete();
                }
            }
        }

        /// <summary>
        /// データリンク編集
        /// </summary>
        /// <param name="linkKey">データリンクキー</param>
        /// <param name="data">編集データ</param>
        /// <param name="bReplaceReceiptNo">受付番号置き換えフラグ</param>
        private void modifyDataLinkDic(Tuple<String, Int32, String> linkKey, SpecimenRegistrationGridViewDataSet data, bool bReplaceReceiptNo = false)
        {
            // 更新
            this.dataLinkDic[linkKey].BeginEdit();
            this.dataLinkDic[linkKey][STRING_SAMPLEID] = data.PatientID;
            this.dataLinkDic[linkKey][STRING_SPECIMENMATERIALTYPE] = data.SpecimenType;
            this.dataLinkDic[linkKey][STRING_COMMENT] = data.Comment;
            this.dataLinkDic[linkKey][STRING_MANUALDILUTION] = data.ManualDil;

            // 受付番号を置き換える場合
            if(bReplaceReceiptNo)
            {
                this.dataLinkDic[linkKey][STRING_RECEIPTNO] = data.ReceiptNumber;
            }

            Int32 routineTableOrder = 0;

            // 先に全てのmeasProt内容をクリアする。
            for (int tableOrder = 1; tableOrder <= CarisXConst.ROUTINE_TABLE_COUNT; tableOrder++)
            {
                this.dataLinkDic[linkKey][STRING_MEASPROT + tableOrder.ToString()] = DBNull.Value;
            }

            foreach (Tuple<Int32?, Int32?, Int32?> dat in data.Registered)
            {
                if (dat.Item1.HasValue)
                {
                    // プロトコルインデックス→ルーチンテーブル順序
                    routineTableOrder = Singleton<MeasureProtocolManager>.Instance.GetRoutineTableOrder(dat.Item1.Value);
                    this.dataLinkDic[linkKey][STRING_MEASPROT + routineTableOrder.ToString()] = assemblyMeasProtValue(dat.Item2.Value, dat.Item3.Value);
                }
            }

            this.dataLinkDic[linkKey].EndEdit();
        }

        /// <summary>
        /// データリンク新規追加
        /// </summary>
        /// <param name="linkKey"></param>
        /// <param name="data"></param>
        /// <param name="bCreateReceiptNo"></param>
        private void createNewDataLinkDic(Tuple<String, Int32, String> linkKey, SpecimenRegistrationGridViewDataSet data, bool bCreateReceiptNo = true)
        {
            // *受付番号作成*
            Int32 receiptNo = 0;
            if (bCreateReceiptNo)
            {
                receiptNo = Singleton<ReceiptNo>.Instance.CreateNumber();
                data.ReceiptNumber = receiptNo;
            }
            else
            {
                receiptNo = data.ReceiptNumber;
            }

            // 追加
            DataRow newRow = this.DataTable.NewRow();

            newRow.BeginEdit();
            newRow[STRING_RACKID] = data.RackID.DispPreCharString;
            newRow[STRING_RACKPOSITION] = linkKey.Item2;
            newRow[STRING_RECEIPTNO] = receiptNo;
            newRow[STRING_SAMPLEID] = data.PatientID;
            newRow[STRING_SPECIMENMATERIALTYPE] = data.SpecimenType;
            newRow[STRING_COMMENT] = data.Comment;
            newRow[STRING_SAMPLEMEASKIND] = data.RackID.GetSampleKind();
            newRow[STRING_SAMPLENO] = 0; // TODO:不使用Field
            newRow[STRING_MANUALDILUTION] = data.ManualDil;

            Int32 routineTableOrder = 0;
            foreach (Tuple<Int32?, Int32?, Int32?> dat in data.Registered)
            {
                if (dat.Item1.HasValue)
                {
                    MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(dat.Item1.Value);
                    // プロトコルインデックス→ルーチンテーブル順序
                    routineTableOrder = Singleton<MeasureProtocolManager>.Instance.GetRoutineTableOrder(dat.Item1.Value);
                    newRow[STRING_MEASPROT + routineTableOrder] = assemblyMeasProtValue(dat.Item2, dat.Item3);
                }
            }

            newRow.EndEdit();
            this.DataTable.Rows.Add(newRow);
            this.dataLinkDic[linkKey] = newRow;
        }

        /// <summary>
        /// データ設定Data set
        /// </summary>
        /// <param name="dataList">設定データリスト</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean SetDispData(List<SpecimenRegistrationGridViewDataSet> dataList, out String errorMessage)
        {
            errorMessage = String.Empty;

            Boolean result = true;
            if (this.DataTable != null)
            {
                try
                {
                    Tuple<String, Int32, String> linkKey = null;

                    // テーブル内容を更新する
                    foreach (SpecimenRegistrationGridViewDataSet data in dataList)
                    {
                        // データリンクキー（ラックID-ポジション-検体ID）を生成
                        linkKey = new Tuple<String, Int32, String>(data.RackID.DispPreCharString, data.RackPosition, data.PatientID);

                        bool bContainIGRA = false;

                        // 分析項目登録にIGRA項目がないかチェック
                        for (int i = 0; i < data.Registered.Count; i++)
                        {
                            if (data.Registered[i].Item1 != null)
                            {
                                MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(data.Registered[i].Item1.Value);
                                if (protocol.IsIGRA)
                                {
                                    // IGRA項目あり
                                    bContainIGRA = true;
                                    break;
                                }
                            }
                        }

                        // 既存データにIGRAが登録済みかチェック
                        bool bRegistedIGRA = false;

                        if (this.dataLinkDic.ContainsKey(linkKey))
                        {
                            DataRow row = this.dataLinkDic[linkKey];
                            for (Int32 i = 0; i < CarisXConst.MEAS_PROTO_REGIST_MAX; i++)
                            {
                                // 分析項目が設定されており、測定指示と一致すれば、空データに設定
                                String measProtoColumnName = STRING_MEASPROT + (i + 1).ToString();

                                Int32? measProtValue = row.Field<Int32?>(measProtoColumnName);

                                // 値が設定されているか確認
                                if (measProtValue.HasValue)
                                {
                                    // ルーチンテーブルから該当する分析項目Indexを取得
                                    Int32 protocolIndex = Singleton<MeasureProtocolManager>.Instance.GetProtocolIndexFromRoutineTableOrder(i + 1);

                                    // 分析項目Index => 分析項目情報へ変換
                                    MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(protocolIndex);
                                    if (protocol.IsIGRA)
                                    {
                                        // IGRA項目あり
                                        bRegistedIGRA = true;
                                        break;
                                    }
                                }
                            }
                        }

                        // これから登録する項目がIGRAで、IGRAがまだ登録されていない
                        if (bContainIGRA && !bRegistedIGRA)
                        {
                            // IGRAの新規登録の場合

                            // ラックポジション2以外は受け付けない
                            if( data.RackPosition != 2)
                            {
                                // ポジション指定異常のエラーメッセージ出力
                                errorMessage = String.Format(CarisX.Properties.Resources.STRING_DLG_MSG_262, data.RackID);
                            }
                        }
                        // これから登録する項目はIGRA以外で、既にIGRAが登録済み
                        else if (!bContainIGRA && bRegistedIGRA)
                        {
                            // IGRAを別項目で上書きしようとしている

                            // 上書き不可のエラーメッセージ出力
                            errorMessage = String.Format(CarisX.Properties.Resources.STRING_DLG_MSG_263, data.RackID);
                        }
                        // これから登録する項目はIGRA以外で、IGRA以外が登録済みまたは登録なし
                        else if (!bContainIGRA && !bRegistedIGRA)
                        {
                            // ポジション2を確認
                            Tuple<String, Int32, String> checkLinkKey = new Tuple<String, Int32, String>
                                (data.RackID.DispPreCharString, 2, data.PatientID);

                            if (this.dataLinkDic.ContainsKey(checkLinkKey))
                            {
                                DataRow row = this.dataLinkDic[checkLinkKey];
                                for (Int32 i = 0; i < CarisXConst.MEAS_PROTO_REGIST_MAX; i++)
                                {
                                    // 分析項目が設定されており、測定指示と一致すれば、空データに設定
                                    String measProtoColumnName = STRING_MEASPROT + (i + 1).ToString();

                                    Int32? measProtValue = row.Field<Int32?>(measProtoColumnName);

                                    // 値が設定されているか確認
                                    if (measProtValue.HasValue)
                                    {
                                        // ルーチンテーブルから該当する分析項目Indexを取得
                                        Int32 protocolIndex = Singleton<MeasureProtocolManager>.Instance.GetProtocolIndexFromRoutineTableOrder(i + 1);

                                        // 分析項目Index => 分析項目情報へ変換
                                        MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(protocolIndex);
                                        if (protocol.IsIGRA)
                                        {
                                            // IGRA項目あり
                                            // 登録不可のエラーメッセージ出力
                                            errorMessage = String.Format(CarisX.Properties.Resources.STRING_DLG_MSG_263, data.RackID);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        // IGRAを登録しているにも関わらず、再登録
                        else
                        {
                            // IGRAのデータ内容を更新
                            // 希釈倍率の更新？
                        }

                        // 何らかのエラーが発生している場合
                        if (!String.IsNullOrEmpty(errorMessage))
                        {
                            // 登録処理を中断する
                            return false;
                        }
                        else
                        {
                            // これから登録するデータにIGRA項目が存在している場合
                            if (bContainIGRA)
                            {
                                // IGRA以外の分析項目の登録を削除
                                for (int i = data.Registered.Count - 1; i > -1; i--)
                                {
                                    if (data.Registered[i].Item1 != null)
                                    {
                                        MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(data.Registered[i].Item1.Value);
                                        if (!protocol.IsIGRA)
                                        {
                                            data.Registered.RemoveAt(i);
                                        }
                                    }

                                }
                            }
                        }

                        // データ登録
                        foreach (Tuple<Int32?, Int32?, Int32?> dat in data.Registered)
                        {
                            if (dat.Item1.HasValue)
                            {
                                MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(dat.Item1.Value);

                                // 分析項目がIGRAの場合、特別処理
                                if (protocol.IsIGRA)
                                {
                                    // IGRA専用のデータ設定
                                    this.setDispDataForIGRA(data);
                                }
                                // 分析項目がIGRA以外の場合
                                else
                                {

                                    if (this.dataLinkDic.ContainsKey(linkKey))
                                    {
                                        // 既存データを編集
                                        this.modifyDataLinkDic(linkKey, data, false);
                                    }
                                    else
                                    {
                                        // 新規データ作成
                                        this.createNewDataLinkDic(linkKey, data);
                                    }

                                    // ループ処理終了
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 不正データ
                    //Singleton<LogManager>.Instance.Error( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                    result = false;
                }
            }

            return result;

        }


        /// <summary>
        /// データ設定Data set
        /// </summary>
        /// <param name="dataList">設定データリスト</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean SetIGRAHostDispData(List<SpecimenRegistrationGridViewDataSet> dataList)
        {
            Boolean result = true;
            if (this.DataTable != null)
            {
                try
                {
                    // テーブル内容に更新する
                    foreach (SpecimenRegistrationGridViewDataSet data in dataList)
                    {
                        foreach (Tuple<Int32?, Int32?, Int32?> dat in data.Registered)
                        {
                            if (dat.Item1.HasValue)
                            {
                                // 分析項目情報取得
                                MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(dat.Item1.Value);

                                // 分析項目がIGRAの場合
                                if (protocol.IsIGRA)
                                {
                                    // IGRA専用のデータ設定
                                    this.setDispDataForIGRA(data);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 不正データ                  
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// IGRAデータ設定
        /// </summary>
        /// <param name="data"></param>
        private void setDispDataForIGRA(SpecimenRegistrationGridViewDataSet data)
        {
            if (data.RackPosition == 2)
            {
                // ポジション2の検体データ(N)を作成
                Tuple<String, Int32, String> linkKey1 = new Tuple<String, Int32, String>
                    (data.RackID.DispPreCharString, data.RackPosition, data.PatientID);
                data.Comment = "N";

                // 既にデータが存在している場合
                if (this.dataLinkDic.ContainsKey(linkKey1))
                {
                    // 既存データを編集
                    this.modifyDataLinkDic(linkKey1, data, true);
                }
                else
                {
                    // 新規データ作成
                    this.createNewDataLinkDic(linkKey1, data, false);
                }

                // ポジション3に移動
                data.RackPosition++;

                // ポジション1と5のデータを削除
                Tuple<String, Int32, String> deleteLinkKey1 = new Tuple<String, Int32, String>
                    (data.RackID.DispPreCharString, 1, data.PatientID);
                this.deleteDataLincDic(deleteLinkKey1);

                Tuple<String, Int32, String> deleteLinkKey5 = new Tuple<String, Int32, String>
                    (data.RackID.DispPreCharString, 5, data.PatientID);
                this.deleteDataLincDic(deleteLinkKey5);
            }

            if (data.RackPosition == 3)
            {
                // ポジション3の検体データ(T)を作成
                Tuple<String, Int32, String> linkKey2 = new Tuple<String, Int32, String>
                    (data.RackID.DispPreCharString, data.RackPosition, data.PatientID);
                data.Comment = "T";

                // 既にデータが存在している場合
                if (this.dataLinkDic.ContainsKey(linkKey2))
                {
                    // 既存データを編集
                    this.modifyDataLinkDic(linkKey2, data, true);
                }
                else
                {
                    // 新規データ作成
                    this.createNewDataLinkDic(linkKey2, data, false);
                }

                // ポジション4に移動
                data.RackPosition++;
            }

            if (data.RackPosition == 4)
            {
                // ポジション4の検体データ(P)を作成
                Tuple<String, Int32, String> linkKey3 = new Tuple<String, Int32, String>
                    (data.RackID.DispPreCharString, data.RackPosition, data.PatientID);
                data.Comment = "P";

                // 既にデータが存在している場合
                if (this.dataLinkDic.ContainsKey(linkKey3))
                {
                    // 既存データを編集
                    this.modifyDataLinkDic(linkKey3, data, true);
                }
                else
                {
                    // 新規データ作成
                    this.createNewDataLinkDic(linkKey3, data, false);
                }
            }
        }

        /// <summary>
        /// 検体情報テーブル取得
        /// </summary>
        /// <remarks>
        /// 検体情報をDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
            this.createDataLink();
        }

        /// <summary>
        /// 検体情報テーブル書込み
        /// </summary>
        /// <remarks>
        /// 検体情報をDBに書き込みます。
        /// </remarks>
        public void CommitSampleInfo()
        {
            this.updateBaseTable();
        }
        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// DB-検体登録画面表示データ関連付け生成
        /// </summary>
        /// <remarks>
        /// DBのデータと検体登録画面が保持するデータの結びつけを行う情報を生成します。
        /// </remarks>
        protected void createDataLink()
        {
            if (this.DataTable == null)
            {
                return;
            }

            // 画面からDataTable更新の際、ラックIDとラックポジションからデータ行を特定する為の情報
            this.dataLinkDic.Clear();
            if (this.DataTable != null)
            {
                foreach (DataRow row in this.DataTable.Rows)
                {
                    this.dataLinkDic.Add(new Tuple<String, Int32, String>((String)row[STRING_RACKID], (Int32)row[STRING_RACKPOSITION], (String)row[STRING_SAMPLEID]), row);
                }
            }
        }

        /// <summary>
        /// MeasProt1～50に設定されている内容から、希釈倍率と測定回数に分割する
        /// </summary>
        /// <param name="measProtValue"></param>
        /// <returns></returns>
        protected Tuple<Int32, Int32> disassemblyMeasProtValue(Int32 measProtValue)
        {
            //一～十の位は測定回数なので、100で割った余りを設定
            //百の位以降は希釈倍率なので、100で割った値（切り捨て）を設定
            return new Tuple<int, int>(measProtValue/100, measProtValue%100);
        }

        /// <summary>
        /// 希釈倍率と測定回数を結合し、MeasProt1～50に設定できる値にする
        /// </summary>
        /// <param name="dilratio"></param>
        /// <param name="measTimes"></param>
        /// <returns></returns>
        protected Int32? assemblyMeasProtValue(Int32? dilratio, Int32? measTimes)
        {
            if (dilratio != null && measTimes != null)
            {
                return (dilratio * 100) + measTimes;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
