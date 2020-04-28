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
    /// STAT検体グリッド表示内容
    /// </summary>
    public class SpecimenStatRegistrationGridViewDataSet
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
        public SpecimenMaterialType SpecimenType = SpecimenMaterialType.BloodSerumAndPlasma;
        /// <summary>
        /// 分析項目リスト（分析項目インデックス,稀釈倍率,測定回数)Analysis items index, the dilution factor
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
        /// <summary>
        /// 登録種別
        /// </summary>
        public RegistType RegistType = RegistType.Temporary;
        /// <summary>
        /// 容器種別
        /// </summary>
        public SpecimenCupKind VesselType = SpecimenCupKind.Cup;

    }

    /// <summary>
    /// STAT検体DBアクセスクラス
    /// </summary>
    public class SpecimenStatDB : DBAccessControl
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
                return "SELECT * FROM dbo.SpecimenStatRegist";
            }
        }

        #endregion

        #region [定数定義]

        /// <summary>
        /// 検体区分(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_SAMPLEMEASKIND = "sampleMeasKind";
        /// <summary>
        /// ラックID(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_RACKID = "rackID";
        /// <summary>
        /// ラックポジション(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_RACKPOSITION = "rackPosition";
        /// <summary>
        /// 受付番号(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_RECEIPTNO = "receiptNo";
        /// <summary>
        /// 検体ID(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_SAMPLEID = "sampleID";
        /// <summary>
        /// 検体種別(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_SPECIMENMATERIALTYPE = "specimenMaterialType";
        /// <summary>
        /// ステータス(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_STATUS = "status";
        /// <summary>
        /// 手希釈倍率(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MANUALDILUTION = "manualDilution";
        /// <summary>
        /// 登録種別(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_REGISTTYPE = "registType";
        /// <summary>
        /// 容器種別(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_VESSELTYPE = "vesselType";
        /// <summary>
        /// コメント(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_COMMENT = "comment";
        /// <summary>
        /// 分析項目（共通文字）
        /// </summary>
        public const String STRING_MEASPROT = "measProt";
        /// <summary>
        /// 分析項目1(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT1 = STRING_MEASPROT + "1";
        /// <summary>
        /// 分析項目2(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT2 = STRING_MEASPROT + "2";
        /// <summary>
        /// 分析項目3(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT3 = STRING_MEASPROT + "3";
        /// <summary>
        /// 分析項目4(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT4 = STRING_MEASPROT + "4";
        /// <summary>
        /// 分析項目5(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT5 = STRING_MEASPROT + "5";
        /// <summary>
        /// 分析項目6(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT6 = STRING_MEASPROT + "6";
        /// <summary>
        /// 分析項目7(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT7 = STRING_MEASPROT + "7";
        /// <summary>
        /// 分析項目8(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT8 = STRING_MEASPROT + "8";
        /// <summary>
        /// 分析項目9(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT9 = STRING_MEASPROT + "9";
        /// <summary>
        /// 分析項目10(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT10 = STRING_MEASPROT + "10";
        /// <summary>
        /// 分析項目11(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT11 = STRING_MEASPROT + "11";
        /// <summary>
        /// 分析項目12(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT12 = STRING_MEASPROT + "12";
        /// <summary>
        /// 分析項目13(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT13 = STRING_MEASPROT + "13";
        /// <summary>
        /// 分析項目14(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT14 = STRING_MEASPROT + "14";
        /// <summary>
        /// 分析項目15(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT15 = STRING_MEASPROT + "15";
        /// <summary>
        /// 分析項目16(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT16 = STRING_MEASPROT + "16";
        /// <summary>
        /// 分析項目17(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT17 = STRING_MEASPROT + "17";
        /// <summary>
        /// 分析項目18(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT18 = STRING_MEASPROT + "18";
        /// <summary>
        /// 分析項目19(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT19 = STRING_MEASPROT + "19";
        /// <summary>
        /// 分析項目20(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT20 = STRING_MEASPROT + "20";
        /// <summary>
        /// 分析項目21(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT21 = STRING_MEASPROT + "21";
        /// <summary>
        /// 分析項目22(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT22 = STRING_MEASPROT + "22";
        /// <summary>
        /// 分析項目23(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT23 = STRING_MEASPROT + "23";
        /// <summary>
        /// 分析項目24(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT24 = STRING_MEASPROT + "24";
        /// <summary>
        /// 分析項目25(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT25 = STRING_MEASPROT + "25";
        /// <summary>
        /// 分析項目26(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT26 = STRING_MEASPROT + "26";
        /// <summary>
        /// 分析項目27(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT27 = STRING_MEASPROT + "27";
        /// <summary>
        /// 分析項目28(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT28 = STRING_MEASPROT + "28";
        /// <summary>
        /// 分析項目29(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT29 = STRING_MEASPROT + "29";
        /// <summary>
        /// 分析項目30(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT30 = STRING_MEASPROT + "30";
        /// <summary>
        /// 分析項目31(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT31 = STRING_MEASPROT + "31";
        /// <summary>
        /// 分析項目32(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT32 = STRING_MEASPROT + "32";
        /// <summary>
        /// 分析項目33(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT33 = STRING_MEASPROT + "33";
        /// <summary>
        /// 分析項目34(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT34 = STRING_MEASPROT + "34";
        /// <summary>
        /// 分析項目35(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT35 = STRING_MEASPROT + "35";
        /// <summary>
        /// 分析項目36(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT36 = STRING_MEASPROT + "36";
        /// <summary>
        /// 分析項目37(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT37 = STRING_MEASPROT + "37";
        /// <summary>
        /// 分析項目38(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT38 = STRING_MEASPROT + "38";
        /// <summary>
        /// 分析項目39(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT39 = STRING_MEASPROT + "39";
        /// <summary>
        /// 分析項目40(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT40 = STRING_MEASPROT + "40";
        /// <summary>
        /// 分析項目41(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT41 = STRING_MEASPROT + "41";
        /// <summary>
        /// 分析項目42(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT42 = STRING_MEASPROT + "42";
        /// <summary>
        /// 分析項目43(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT43 = STRING_MEASPROT + "43";
        /// <summary>
        /// 分析項目44(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT44 = STRING_MEASPROT + "44";
        /// <summary>
        /// 分析項目45(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT45 = STRING_MEASPROT + "45";
        /// <summary>
        /// 分析項目46(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT46 = STRING_MEASPROT + "46";
        /// <summary>
        /// 分析項目47(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT47 = STRING_MEASPROT + "47";
        /// <summary>
        /// 分析項目48(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT48 = STRING_MEASPROT + "48";
        /// <summary>
        /// 分析項目49(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT49 = STRING_MEASPROT + "49";
        /// <summary>
        /// 分析項目50(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_MEASPROT50 = STRING_MEASPROT + "50";
        /// <summary>
        /// カップボリューム(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_CUPVOL = "cupVol";
        /// <summary>
        /// チューブボリューム(DBテーブル：specimenStatRegist列名)
        /// </summary>
        public const String STRING_TUBEVOL = "tubeVol";
        /// <summary>
        /// 検体番号(DBテーブル：specimenStatRegist列名)
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
        /// <param name="data">測定指示データ</param>
        /// <param name="receiptNumber">受付番号</param>
        /// <param name="comment">コメント</param>
        /// <returns>True:取得成功　False:取得失敗</returns>
        public Boolean MeasureIndicate(AskTypeKind askType, ref IMeasureIndicate data, out Int32 receiptNumber, out String comment, int moduleId, out RegistType registType )
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
            Int32 rackPos = moduleId;

            // データ検索
            IEnumerable<DataRow> result = null;

            //　コピーデータリストを取得
            var dataTableList = this.DataTable.AsEnumerable().ToList();

            // 一時登録検索
            result = from v in dataTableList
                        where ( rackId.DispPreCharString == v[STRING_RACKID].ToString() )
                           && ( "0" == v[STRING_RACKPOSITION].ToString() )
                     select v;

            registType = RegistType.Temporary;

            // 一時登録データが無い場合、固定登録検索
            if (result.Count() == 0)
            {
                // 固定登録検索
                result = from v in dataTableList
                         where ( rackId.DispPreCharString == v[STRING_RACKID].ToString() )
                            && ( rackPos.ToString() == v[STRING_RACKPOSITION].ToString() )
                         select v;

                registType = RegistType.Fixed;
            }

            // 登録データがある場合、ワークシート問合せ結果作成
            if (result.Count() != 0)
            {
                // 問い合わせコマンド応答用情報
                List<MeasItem> measItemList = new List<MeasItem>();

                // 画面表示用データ構造を中間データとしてデータテーブルから取得
                SpecimenStatRegistrationGridViewDataSet viewData = this.getDataStruct( result.First() ) ;

                data.PreDil = viewData.ManualDil;
                data.SpecimenCup = viewData.VesselType;

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
                            measItemList.Clear();
                            if (!string.IsNullOrEmpty(data.SampleID) && (data.SampleID.Trim() != "ERROR" || !data.SampleID.Trim().Contains("???")))
                            {
                                break;//サンプルIDが0以外の場合、LISシステムへのプロジェクトにのみ申請できます。（如果样本ID 不为0 的情况，只能向LIS系统申请项目）
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
                        item.UniqNo = Singleton<UniqueNo>.Instance.CreateNumber(); /* ユニーク番号生成 */
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【STAT検体追加】 Unique number = {0} Rack = {1}-{2} ID = {3}", item.UniqNo, data.RackID, data.SamplePosition, data.SampleID));

                        // 試薬DBクラス利用で試薬ロット取得する
                        item.ReagentLotNo = Singleton<ReagentDB>.Instance.GetNowReagentLotNo(protocol.ReagentCode);// 試薬ロット番号                         
                        measItemList.Add(item);
                        if (protocol.IsIGRA)
                        {
                            break;
                        }
                    }
                }

                // 測定項目を応答データに設定
                data.MeasItemCount = measItemList.Count;
                data.MeasItemArray = measItemList.ToArray();
                receiptNumber = viewData.ReceiptNumber;
                comment = viewData.Comment;
                findData = true; // 検索結果あり
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
        protected IEnumerable<Tuple<String, Int32, String>> searchLinkKey(CarisXIDString rackId, String sampleId)
        {
            IEnumerable<Tuple<String, Int32, String>> searched = null;
            if (rackId.Value == 0)
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
                           where (v.Item1 == rackId.DispPreCharString)
                           select v;
            }
            return searched;
        }

        /// <summary>
        /// 表示内容リンク情報を検索
        /// </summary>
        /// <param name="rackId"></param>
        /// <param name="rackPos"></param>
        /// <returns></returns>
        /// <remarks>一時登録、固定登録のいずれかのデータをピンポイントで検索したい場合に使用</remarks>
        protected IEnumerable<Tuple<String, Int32, String>> searchLinkKey(CarisXIDString rackId, Int32 position)
        {
            IEnumerable<Tuple<String, Int32, String>> searched = null;

            // ラックID、ポジション（一時登録の場合は0）で参照して取得したデータを返す
            searched = from v in this.dataLinkDic.Keys
                       where (v.Item1 == rackId.DispPreCharString && v.Item2 == position)
                       select v;

            return searched;
        }

        /// <summary>
        /// コメント検索
        /// </summary>
        /// <param name="rackId"></param>
        /// <param name="rackPos"></param>
        /// <param name="sampleId"></param>
        /// <returns></returns>
        public String SearchComment(CarisXIDString rackId, String sampleId)
        {
            String comment = String.Empty;

            // 現在読込まれているデータの中から、指定項目のコメントを検索します。
            if (this.DataTable != null)
            {
                var searched = searchLinkKey(rackId, sampleId);
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
        public Int32 SearchReceiptNumber(CarisXIDString rackId, String sampleId)
        {
            Int32 receiptNumber = 0;

            // 現在読込まれているデータの中から、指定項目の受付番号を検索します。
            if (this.DataTable != null)
            {
                var searched = searchLinkKey(rackId, sampleId);
                if (searched.Count() != 0)
                {
                    receiptNumber = this.dataLinkDic[searched.First()].Field<Int32>(STRING_RECEIPTNO);
                }
            }
            return receiptNumber;
        }

        /// <summary>
        /// 一時登録データ検索
        /// </summary>
        /// <returns></returns>
        public Int32 SearchTemporaryData()
        {
            Int32 receiptNumber = 0;

            // 現在読込まれているデータの中から、指定項目の受付番号を検索します。
            if (this.DataTable != null)
            {
                CarisXIDString rackId = new StatRackID();
                var searched = searchLinkKey(rackId.DispPreCharString, CarisXConst.TEMPORARY_STAT_POSITION);
                if (searched.Count() != 0)
                {
                    receiptNumber = this.dataLinkDic[searched.First()].Field<Int32>(STRING_RECEIPTNO);
                }
            }
            return receiptNumber;
        }

        /// <summary>
        /// STAT検体登録画面表示情報取得
        /// </summary>
        /// <remarks>
        /// STAT検体登録テーブルから取得したデータを、STAT検体登録画面表示出力向けに取得します。
        /// </remarks>
        /// <returns>検体登録画面表示データ</returns>
        public List<SpecimenStatRegistrationGridViewDataSet> GetDispData()
        {
            List<SpecimenStatRegistrationGridViewDataSet> result = new List<SpecimenStatRegistrationGridViewDataSet>();

            if (this.DataTable != null)
            {
                try
                {
                    // テーブル内容全て返す
                    foreach (System.Data.DataRow row in this.DataTable.Rows)
                    {
                        SpecimenStatRegistrationGridViewDataSet data = getDataStruct(row);
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
        /// STAT検体登録画面表示情報取得
        /// </summary>
        /// <remarks>
        /// STAT検体登録テーブルから取得したデータを、STAT検体登録画面表示出力向けに取得します。
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
        /// STAT検体登録テーブルから取得したデータと自動希釈倍率を取得します
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
        private SpecimenStatRegistrationGridViewDataSet getDataStruct(DataRow row)
        {
            SpecimenStatRegistrationGridViewDataSet data = new SpecimenStatRegistrationGridViewDataSet();
            data.RackID = (String)row[STRING_RACKID];
            data.RackPosition = (Int32)row[STRING_RACKPOSITION];
            data.ReceiptNumber = (Int32)row[STRING_RECEIPTNO];
            data.PatientID = (String)row[STRING_SAMPLEID];
            data.SpecimenType = (SpecimenMaterialType)row[STRING_SPECIMENMATERIALTYPE];
            data.Comment = (String)row[STRING_COMMENT];
            data.ManualDil = (Int32)row[STRING_MANUALDILUTION];
            data.RegistType = (RegistType)row[STRING_REGISTTYPE];
            data.VesselType = (SpecimenCupKind)row[STRING_VESSELTYPE];

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
                                    where (v.Key.Item3 == deleteInfo.Item3) || (v.Key.Item1 == deleteInfo.Item1 && v.Key.Item2 == deleteInfo.Item2)
                                    select v.Key;
                    }
                    if (keyFinder.Count() != 0)
                    {
                        this.dataLinkDic[keyFinder.First()].Delete();
                    }
                }
                //this.createDataLink();
            }
            return result;
        }

        /// <summary>
        /// 【IssuesNo:14】更新固定注册信息中的接收码
        /// </summary>
        /// <param name="updateKeyList"></param>
        /// <returns></returns>
        public Boolean UpdateReceiptNo(List<Tuple<String, Int32, String>> updateKeyList)
        {
            Boolean result = true;
            if(this.DataTable != null)
            {
                foreach (var deleteInfo in updateKeyList)
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
                                    where (v.Key.Item3 == deleteInfo.Item3) || (v.Key.Item1 == deleteInfo.Item1 && v.Key.Item2 == deleteInfo.Item2)
                                    select v.Key;
                    }
                    if (keyFinder.Count() != 0)
                    {
                        int receiptNo = 0;
                        receiptNo = Singleton<ReceiptNo>.Instance.CreateNumber();
                        this.dataLinkDic[keyFinder.First()].BeginEdit();
                        this.dataLinkDic[keyFinder.First()][STRING_RECEIPTNO] = receiptNo;
                        this.dataLinkDic[keyFinder.First()].EndEdit();
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

            this.ExecuteSql("DELETE FROM dbo.specimenStatRegist");

            sw.Stop();
            // STAT検体登録DB削除時間
            System.Diagnostics.Debug.WriteLine(String.Format("specimenStatRegist delete time:{0}msec", sw.ElapsedMilliseconds));
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                , CarisXLogInfoBaseExtention.Empty, String.Format("specimenStatRegist delete time:{0}msec", sw.ElapsedMilliseconds));
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
        /// データ全削除
        /// </summary>
        /// <remarks>
        /// 登録されている検体情報を全て削除します。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean Delete( RegistType registType )
        {
            Boolean result = true;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            this.ExecuteSql(String.Format("DELETE FROM dbo.specimenStatRegist where registType = {0}", (int)registType));

            sw.Stop();
            // STAT検体登録DB削除時間
            System.Diagnostics.Debug.WriteLine(String.Format("specimenStatRegist delete time:{0}msec", sw.ElapsedMilliseconds));
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                , CarisXLogInfoBaseExtention.Empty, String.Format("specimenStatRegist delete time:{0}msec", sw.ElapsedMilliseconds));
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
        public Boolean SetAskData(AskWorkSheetData data, SpecimenCupKind cupKind)
        {

            Boolean result = true;
            if (this.DataTable != null)
            {
                try
                {
                    Tuple<String, Int32, String> linkKey = null;

                    StatRackID rackId = (StatRackID)data.FromHostCommand.RackID;

                    linkKey = new Tuple<String, Int32, String>(data.FromHostCommand.RackID, data.FromHostCommand.RackPos, data.FromHostCommand.SampleID);

                    // 追加
                    DataRow newRow = this.DataTable.NewRow();

                    newRow.BeginEdit();

                    newRow[STRING_RACKID] = data.FromHostCommand.RackID;
                    newRow[STRING_RACKPOSITION] = data.FromHostCommand.RackPos;
                    newRow[STRING_RECEIPTNO] = data.FromHostCommand.ReceiptNumber;
                    newRow[STRING_SAMPLEID] = data.FromHostCommand.SampleID;
                    newRow[STRING_SPECIMENMATERIALTYPE] = data.FromHostCommand.SampleMaterialType.GetDPRSpecimemMaterialType();
                    newRow[STRING_COMMENT] = data.FromHostCommand.Comment;
                    newRow[STRING_SAMPLEMEASKIND] = data.FromHostCommand.SampleType.GetDPRSampleKind();
                    newRow[STRING_SAMPLENO] = 0; // TODO:不使用Field
                    newRow[STRING_MANUALDILUTION] = data.FromHostCommand.ManualDil;
                    newRow[STRING_REGISTTYPE] = RegistType.Temporary;
                    newRow[STRING_VESSELTYPE] = cupKind;

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

        private void ModifyDataLinkDic(Tuple<String, Int32, String> linkKey, SpecimenStatRegistrationGridViewDataSet data)
        {
            // 更新
            this.dataLinkDic[linkKey].BeginEdit();
            this.dataLinkDic[linkKey][STRING_SAMPLEID] = data.PatientID;
            this.dataLinkDic[linkKey][STRING_SPECIMENMATERIALTYPE] = data.SpecimenType;
            this.dataLinkDic[linkKey][STRING_COMMENT] = data.Comment;
            this.dataLinkDic[linkKey][STRING_MANUALDILUTION] = data.ManualDil;
            this.dataLinkDic[linkKey][STRING_REGISTTYPE] = data.RegistType;
            this.dataLinkDic[linkKey][STRING_VESSELTYPE] = data.VesselType;

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

        private void CreateNewDataLinkDic(Tuple<String, Int32, String> linkKey, SpecimenStatRegistrationGridViewDataSet data, bool bCreateReceiptNo = true)
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
            newRow[STRING_REGISTTYPE] = data.RegistType;
            newRow[STRING_VESSELTYPE] = data.VesselType;

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
        public Boolean SetDispData(List<SpecimenStatRegistrationGridViewDataSet> dataList)
        {
            Boolean result = true;
            if (this.DataTable != null)
            {
                try
                {
                    Tuple<String, Int32, String> linkKey = null;

                    // テーブル内容に更新する
                    foreach (SpecimenStatRegistrationGridViewDataSet data in dataList)
                    {
                        linkKey = new Tuple<String, Int32, String>(data.RackID.DispPreCharString, data.RackPosition, data.PatientID);

                        // あればDataTable更新無ければ追加
                        if (this.dataLinkDic.ContainsKey(linkKey))
                        {
                            ModifyDataLinkDic(linkKey, data);
                        }
                        else
                        {
                            bool bContainIGRA = false;
                            for (int i = 0; i < data.Registered.Count; i++)
                            {
                                if (data.Registered[i].Item1 != null)
                                {
                                    MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(data.Registered[i].Item1.Value);
                                    if (protocol.IsIGRA)
                                    {
                                        bContainIGRA = true;
                                        break;
                                    }
                                }

                            }
                            if (bContainIGRA)
                            {
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

                            foreach (Tuple<Int32?, Int32?, Int32?> dat in data.Registered)
                            {
                                if (dat.Item1.HasValue)
                                {
                                    MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(dat.Item1.Value);

                                    if (protocol.IsIGRA)
                                    {
                                        //IGRA 项目必须是第二位置
                                        if (data.RackPosition != 2)
                                        {
                                            System.Windows.Forms.MessageBox.Show("IGRA 必须在第二个位置注册");
                                            return false;
                                        }
                                        //创建第一个实验信息。
                                        data.Comment = "N";
                                        CreateNewDataLinkDic(linkKey, data);

                                        //获得2个架子及位置号
                                        if (data.RackPosition == CarisXConst.RACK_POS_COUNT)
                                        {
                                            int nNewRackID = data.RackID.Value + 1;
                                            data.RackID = nNewRackID.ToString("0000");
                                            data.RackPosition = 1;
                                        }
                                        else
                                        {
                                            data.RackPosition++;
                                        }
                                        //创建第2个实验信息。
                                        Tuple<String, Int32, String> linkKey1 = new Tuple<String, Int32, String>
                                            (data.RackID.DispPreCharString, data.RackPosition, data.PatientID);
                                        data.Comment = "T";
                                        if (this.dataLinkDic.ContainsKey(linkKey1))
                                        {
                                            ;
                                            ModifyDataLinkDic(linkKey1, data);
                                        }
                                        else
                                        {
                                            CreateNewDataLinkDic(linkKey1, data, false);
                                        }

                                        //获得3个架子及位置号
                                        if (data.RackPosition == CarisXConst.RACK_POS_COUNT)
                                        {
                                            int nNewRackID = data.RackID.Value + 1;
                                            data.RackID = nNewRackID.ToString("0000");
                                            data.RackPosition = 1;
                                        }
                                        else
                                        {
                                            data.RackPosition++;
                                        }
                                        //创建第3个实验信息。
                                        Tuple<String, Int32, String> linkKey2 = new Tuple<String, Int32, String>
                                            (data.RackID.DispPreCharString, data.RackPosition, data.PatientID);
                                        data.Comment = "P";
                                        if (this.dataLinkDic.ContainsKey(linkKey2))
                                        {
                                            ModifyDataLinkDic(linkKey2, data);
                                        }
                                        else
                                        {
                                            CreateNewDataLinkDic(linkKey2, data, false);
                                        }

                                    }
                                    else
                                    {
                                        CreateNewDataLinkDic(linkKey, data);
                                        break;//跳出循环
                                    }
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
        public Boolean SetIGRAHostDispData(List<SpecimenStatRegistrationGridViewDataSet> dataList)
        {
            Boolean result = true;
            if (this.DataTable != null)
            {
                try
                {
                    Tuple<String, Int32, String> linkKey = null;

                    // テーブル内容に更新する
                    foreach (SpecimenStatRegistrationGridViewDataSet data in dataList)
                    {
                        linkKey = new Tuple<String, Int32, String>(data.RackID.DispPreCharString, data.RackPosition, data.PatientID);

                        foreach (Tuple<Int32?, Int32?, Int32?> dat in data.Registered)
                        {
                            if (dat.Item1.HasValue)
                            {
                                MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(dat.Item1.Value);

                                if (protocol.IsIGRA)
                                {

                                    Tuple<String, Int32, String> linkKey0 = new Tuple<String, Int32, String>(data.RackID.DispPreCharString, data.RackPosition, data.PatientID);
                                    //创建第一个实验信息。
                                    data.Comment = "N";
                                    if (this.dataLinkDic.ContainsKey(linkKey0))
                                    {
                                        ModifyDataLinkDic(linkKey0, data);
                                    }
                                    else
                                    {
                                        CreateNewDataLinkDic(linkKey0, data, false);
                                    }

                                    //获得2个架子及位置号                             
                                    {
                                        data.RackPosition++;
                                    }
                                    //创建第2个实验信息。
                                    Tuple<String, Int32, String> linkKey1 = new Tuple<String, Int32, String>(data.RackID.DispPreCharString, data.RackPosition, data.PatientID);
                                    data.Comment = "T";
                                    if (this.dataLinkDic.ContainsKey(linkKey1))
                                    {
                                        ;
                                        ModifyDataLinkDic(linkKey1, data);
                                    }
                                    else
                                    {
                                        CreateNewDataLinkDic(linkKey1, data, false);
                                    }

                                    //获得3个架子及位置号                                  
                                    {
                                        data.RackPosition++;
                                    }
                                    //创建第3个实验信息。
                                    Tuple<String, Int32, String> linkKey2 = new Tuple<String, Int32, String>(data.RackID.DispPreCharString, data.RackPosition, data.PatientID);
                                    data.Comment = "P";
                                    if (this.dataLinkDic.ContainsKey(linkKey2))
                                    {
                                        ModifyDataLinkDic(linkKey2, data);
                                    }
                                    else
                                    {
                                        CreateNewDataLinkDic(linkKey2, data, false);
                                    }

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
        /// STAT検体登録情報テーブル取得
        /// </summary>
        /// <remarks>
        /// STAT検体登録情報をDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
            this.createDataLink();
        }

        /// <summary>
        /// STAT検体登録情報テーブル書込み
        /// </summary>
        /// <remarks>
        /// STAT検体登録情報をDBに書き込みます。
        /// </remarks>
        public void CommitSampleInfo()
        {
            this.updateBaseTable();
        }
        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// DB-STAT検体登録画面表示データ関連付け生成
        /// </summary>
        /// <remarks>
        /// DBのデータとSTAT検体登録画面が保持するデータの結びつけを行う情報を生成します。
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
