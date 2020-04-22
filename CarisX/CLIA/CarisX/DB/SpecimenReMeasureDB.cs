using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;
using Oelco.CarisX.Utility;

using Oelco.Common.Log;
using Oelco.CarisX.Const;
using Oelco.CarisX.Common;
using Oelco.Common.DB;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using System.Data;
using Oelco.CarisX.Log;


namespace Oelco.CarisX.DB
{

    /// <summary>
    /// 再測定検体グリッド表示内容
    /// </summary>
    public class SpecimenReMeasureGridViewDataSet
    {
        /// <summary>
        /// 分析モジュール番号
        /// </summary>
        public Int32 ModuleNo = 0;
        /// <summary>
        /// シーケンス番号
        /// </summary>
        public Int32 SequenceNumber = 0;
        /// <summary>
        /// 受付番号
        /// </summary>
        public Int32 ReceiptNumber = 0;
        /// <summary>
        /// 検体ID
        /// </summary>
        public String SampleID = String.Empty;
        /// <summary>
        /// ラックID
        /// </summary>
        public CarisXIDString RackID = null;
        /// <summary>
        /// ラックポジション
        /// </summary>
        public Int32 RackPosition = 0;
        /// <summary>
        /// 検体種別
        /// </summary>
        public SpecimenMaterialType SpecimenMaterialType;
        /// <summary>
        /// 分析項目番号
        /// </summary>
        public Int32 MeasprotocolNo;
        /// <summary>
        /// 多重分析番号
        /// </summary>
        public Int32 ReplicationNo;
        /// <summary>
        /// 手稀釈倍率
        /// </summary>
        public Int32 ManualDilution = 0;
        /// <summary>
        /// 自動稀釈倍率
        /// </summary>
        public Int32 AutoDilution = 0;
        /// <summary>
        /// カウント値
        /// </summary>
        public Int32? Count = null;
        /// <summary>
        /// 濃度値
        /// </summary>
        public String Concentration = null;
        /// <summary>
        /// リマーク
        /// </summary>
        public Remark Remark = 0;
        /// <summary>
        /// コメント
        /// </summary>
        public String Comment = String.Empty;
        /// <summary>
        /// 検体識別番号
        /// </summary>
        public Int32 IndividuallyNo = 0;
        /// <summary>
        /// 測定日時
        /// </summary>
        public DateTime MeasureDateTime = DateTime.MaxValue;
        /// <summary>
        /// 自動再検フラグAutomatic retest flag
        /// </summary>
        public Boolean IsAutoReMeasure = false;
        /// <summary>
        /// 測定指示問い合わせ待機フラグ
        /// </summary>
        public Boolean WaitMeasureIndicate = false;
    }

    /// <summary>
    /// 検体DBアクセスクラス
    /// </summary>
    /// <remarks>
    /// 検体DBアクセスクラスです。
    /// </remarks>
    class SpecimenReMeasureDB : DBAccessControl
    {
        /// <summary>
        /// データテーブル-表示内容リンク情報(receiptNo,individuallyNo,measProtocol,ReplicationNumber)
        /// </summary>
        private Dictionary<Tuple<Int32, Int32, Int32, Int32>, DataRow> dataLinkDic = new Dictionary<Tuple<Int32, Int32, Int32, Int32>, DataRow>();

        #region [プロパティ]

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT * FROM dbo.specimenReMeasure";
            }
        }

        #endregion

        #region [定数定義]

        /// <summary>
        /// 分析モジュール番号(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_MODULENO = "moduleNo";
        /// <summary>
        /// 受付番号(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_RECEIPTNO = "receiptNo";
        /// <summary>
        /// 検体識別番号(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_INDIVIDUALLYNO = "individuallyNo";
        /// <summary>
        /// 分析項目番号(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_MEASPROTOCOLNO = "measProtocolNo";
        /// <summary>
        /// 繰り返し番号(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_REPLICATIONNUMBER = "replicationNumber";
        /// <summary>
        /// シーケンス番号(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_SEQUENCENO = "sequenceNo";
        /// <summary>
        /// 検体ID(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_SAMPLEID = "sampleId";
        /// <summary>
        /// ラックID(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_RACKID = "rackId";
        /// <summary>
        /// ラックポジション(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_RACKPOSITION = "rackPosition";
        /// <summary>
        /// 検体種別(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_SPECIMENMATERIALTYPE = "specimenMaterialType";
        /// <summary>
        /// リマークID(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_REMARKID = "remarkId";
        /// <summary>
        /// カウント値(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_COUNTVAL = "countVal";
        /// <summary>
        /// 濃度値(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_CONCENTRATION = "concentration";
        /// <summary>
        /// 手希釈倍率(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_MANUALDILUTION = "manualDilution";
        /// <summary>
        /// 自動希釈倍率(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_AUTODILUTION = "autoDilution";
        /// <summary>
        /// 測定日時(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_MEASUREDATETIME = "measureDateTime";
        /// <summary>
        /// カートリッジロット番号(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_CARTRIDGELOTNO = "cartridgeLotNo";
        /// <summary>
        /// リマークメッセージ(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_REMARKMESSAGE = "remarkMessage";
        /// <summary>
        /// コメント(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_COMMENT = "comment";
        /// <summary>
        /// 自動再検フラグ(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_ISAUTOREMEASURE = "isAutoReMeasure";
        /// <summary>
        /// 測定指示問い合わせ待機フラグ(DBテーブル：SpecimenReMeasure列名)
        /// </summary>
        public const String STRING_WAITMEASUREINDICATE = "waitMeasureIndicate";


        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 測定指示
        /// </summary>
        /// <remarks>
        /// スレーブからの測定指示データ問合せ応答データの検索を行います。
        /// 検索に必要な情報を保持するオブジェクトと、結果を格納するオブジェクトは同一です。
        /// </remarks>
        /// <param name="askType">問合せ法式Query method expressions</param>
        /// <param name="moduleId">測定指示データMeasurement instruction data</param>
        /// <param name="assaySchedules"></param>
        /// <param name="data">測定指示データ</param>
        /// <param name="sequenceNo">sequence番号</param>
        /// <param name="receiptNumber">受付番号</param>
        /// <param name="comment">コメント</param>
        /// <param name="errorArg">エラー引数</param>
        /// <returns>True:取得成功　False:取得失敗</returns>
        public Boolean MeasureIndicate(AskTypeKind askType, int moduleId, List<AssaySchedule> assaySchedules, ref IMeasureIndicate data,
            out Int32 sequenceNo, out Int32 receiptNumber, out String comment, ref Int32 errorArg )
        {
            Boolean findData = false;
            sequenceNo = 0;

            receiptNumber = 0;
            comment = String.Empty;
            // 検体検索→この関数ではしない
            // 検体にシーケンス番号付与(→DBへ)
            // 検体に検体識別番号付与(→DBへ)
            // 分析項目毎にユニーク番号付与(→この時点ではメモリ保持のみ)

            String searchId = data.SampleID;
            CarisXIDString rackId = data.RackID;
            Int32 rackPos = data.SamplePosition;

            // データ検索Data retrieval
            IEnumerable<DataRow> result = null;

            // ラックID検索・サンプルID検索
            // 既に読込まれたテーブルデータより検索を行う。
            switch (askType)
            {
                case AskTypeKind.RackID:
                    var groupVal = from v in this.DataTable.AsEnumerable()
                                   where rackId.DispPreCharString == (v[STRING_RACKID] == null ? String.Empty : v[STRING_RACKID].ToString()) &&
                                   rackPos == (v[STRING_RACKPOSITION] == null ? 0 : (Int32)v[STRING_RACKPOSITION]) &&
                                   (Boolean)v[STRING_WAITMEASUREINDICATE] == true
                                   group v by (Int32)v[STRING_MEASPROTOCOLNO];

                    // 分析項目に対して単一化する
                    var list = new Dictionary<Int32, DataRow>();
                    foreach (var keyval in groupVal)
                    {
                        foreach (var val in keyval)
                        {
                            if (!list.ContainsKey(keyval.Key))
                            {
                                list[keyval.Key] = val;
                            }
                        }
                    }
                    result = list.Values;

                    break;

                case AskTypeKind.SampleID:
                    groupVal = from v in this.DataTable.AsEnumerable()
                               where
                               (searchId == v[STRING_SAMPLEID].ToString()
                               || (rackId.DispPreCharString == (v[STRING_RACKID] == null ? String.Empty : v[STRING_RACKID].ToString()) &&
                               rackPos == (v[STRING_RACKPOSITION] == null ? 0 : (Int32)v[STRING_RACKPOSITION]))
                               ) && (Boolean)v[STRING_WAITMEASUREINDICATE] == true
                               group v by (Int32)v[STRING_MEASPROTOCOLNO];

                    // 分析項目に対して単一化する
                    list = new Dictionary<Int32, DataRow>();
                    foreach (var keyval in groupVal)
                    {
                        foreach (var val in keyval)
                        {
                            if (!list.ContainsKey(keyval.Key))
                            {
                                list[keyval.Key] = val;
                            }
                        }
                    }
                    result = list.Values;

                    break;
            }

            // 結果セットにデータ追加The data added to the result set
            if (result.Count() != 0)
            {
                // データリスト生成
                IEnumerable<SpecimenReMeasureGridViewDataSet> dataListTmp = from v in result
                                                                            select this.getDataStruct(v, GetAutoRemeasKind.All);
                List<MeasItem> measItemList = new List<MeasItem>();

                // 同一検体レベルで共通のデータは、検索結果の1件目を使用する。
                var first = dataListTmp.First();

                // 検体が測定指示データ問い合わせ待機状態であれば取得する
                // 手動で再検査指示されるか、自動再検のデータであれば測定支持データ問い合わせ待機状態となる。
                if (first.WaitMeasureIndicate)
                {
                    data.PreDil = first.ManualDilution;
                    // ID・Pos・Nameは装置から送信されてくる
                    data.SpecimenMaterial = first.SpecimenMaterialType;
                    data.IndividuallyNumber = first.IndividuallyNo;

                    // ラックID分析時に検体BCRが使用設定になっているない場合、DBからのデータを渡す。
                    if (!string.IsNullOrEmpty(first.SampleID))
                    {
                        if (first.SampleID.Trim().Length != 0)
                        {
                            data.SampleID = first.SampleID.Trim();
                        }
                    }

                    // SequenceNoは、IMeasureIndicateのデータ構造に含まれない、かつ最新の発番でもない為、呼び元が知る為には引数に必要。
                    sequenceNo = first.SequenceNumber;

                    foreach (var dataSingle in dataListTmp)
                    {
                        MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(dataSingle.MeasprotocolNo);
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
                            item.AutoDil = dataSingle.AutoDilution; // 後希釈倍率
                        }

                        item.ProtoNo = dataSingle.MeasprotocolNo;
                        item.RepCount = protocol.RepNoForSample;
                        item.TurnNo = Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.GetTurnOrder(protocol.ProtocolName);
                        item.UniqNo = 1;

                        item.ReagentLotNo = Singleton<ReagentDB>.Instance.GetNowReagentLotNo(protocol.ReagentCode, moduleId: moduleId);
                        if (item.ReagentLotNo != string.Empty)
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
                                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, "", String.Format("【検体追加】【再】 Unique number = {0} Rack = {1}-{2} ID = {3}", item.UniqNo, data.RackID, data.SamplePosition, data.SampleID));

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

                    if (measItemList.Count != 0)
                    {
                        // 測定項目を応答データに設定
                        data.MeasItemCount = measItemList.Count;
                        data.MeasItemArray = measItemList.ToArray();

                        receiptNumber = first.ReceiptNumber;
                        comment = first.Comment;
                        findData = true; // 検索結果あり
                    }
                }
            }
            return findData;
        }

        /// <summary>
        /// 検体登録画面表示情報取得
        /// </summary>
        /// <remarks>
        /// 検体登録テーブルから取得したデータを、検体登録画面表示出力向けに取得します。
        /// </remarks>
        /// <param name="getAutoReMeasureData">自動再検データ取得フラグ</param>
        /// <returns>検体登録画面表示データSpecimen registration screen display data</returns>
        public List<SpecimenReMeasureGridViewDataSet> GetDispData(GetAutoRemeasKind getAutoReMeasureData = GetAutoRemeasKind.Disp)
        {
            List<SpecimenReMeasureGridViewDataSet> result = new List<SpecimenReMeasureGridViewDataSet>();

            if (this.DataTable != null)
            {
                try
                {
                    // テーブル内容で表示を行う物は全て返す
                    foreach (System.Data.DataRow row in this.DataTable.Rows)
                    {
                        SpecimenReMeasureGridViewDataSet data = getDataStruct(row, getAutoReMeasureData);
                        if (data != null)
                        {
                            result.Add(data);
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
        /// DBデータ変換DB data conversion
        /// </summary>
        /// <remarks>
        /// DBから取得したデータを、アプリケーション内で利用するデータ型に変換します。
        /// </remarks>
        /// <param name="row">データ行</param>
        /// <param name="targetData">非表示データ取得フラグ </param>
        /// <returns>変換済データ</returns>
        private SpecimenReMeasureGridViewDataSet getDataStruct(System.Data.DataRow row, GetAutoRemeasKind targetData)
        {
            SpecimenReMeasureGridViewDataSet data = null;
            Boolean isAutoRemeasure = row.Field<Boolean>(STRING_ISAUTOREMEASURE);
            Boolean waitMeasureIndicate = row.Field<Boolean>(STRING_WAITMEASUREINDICATE);

            // 自動再検査のデータ、かつ自動再検査データ取得設定されていない場合 Automatic re-examination of the data, and if it is not automatic re-inspection data acquisition settings
            // 或いは、測定指示問い合わせ待機を除いて取得する。Alternatively, to get with the exception of the measurement instruction inquiry waiting.
            Boolean visibleData = true;
            switch (targetData)
            {
                case GetAutoRemeasKind.Manual:
                    //手動再検データのみ取得
                    visibleData = (!isAutoRemeasure);
                    break;
                case GetAutoRemeasKind.Auto:
                    //自動再検データのみ取得
                    visibleData = (isAutoRemeasure);
                    break;
                case GetAutoRemeasKind.Disp:
                    //手動再検かつ測定指示問合せ待機状態ではない（＝再検指示を出していない）データのみ取得
                    visibleData = (!isAutoRemeasure) && (!waitMeasureIndicate);
                    break;
                default:
                    //全て取得
                    visibleData = true;
                    break;
            }

            if (visibleData)
            {
                data = new SpecimenReMeasureGridViewDataSet();
                data.ModuleNo = row.Field<Int32>(STRING_MODULENO);
                data.SequenceNumber = row.Field<Int32>(STRING_SEQUENCENO);
                data.ReceiptNumber = row.Field<Int32>(STRING_RECEIPTNO);
                data.SampleID = row.Field<String>(STRING_SAMPLEID);
                data.RackID = row.Field<String>(STRING_RACKID);
                data.RackPosition = row.Field<Int32>(STRING_RACKPOSITION);
                data.SpecimenMaterialType = row.Field<SpecimenMaterialType>(STRING_SPECIMENMATERIALTYPE);
                data.MeasprotocolNo = row.Field<Int32>(STRING_MEASPROTOCOLNO);
                data.ManualDilution = row.Field<Int32>(STRING_MANUALDILUTION);
                data.AutoDilution = row.Field<Int32>(STRING_AUTODILUTION);
                data.Count = row.Field<Int32?>(STRING_COUNTVAL);
                data.Concentration = row.Field<String>(STRING_CONCENTRATION);
                data.Remark = row.Field<Int64>(STRING_REMARKID);
                data.Comment = row.Field<String>(STRING_COMMENT);
                data.IndividuallyNo = row.Field<Int32>(STRING_INDIVIDUALLYNO);
                data.WaitMeasureIndicate = row.Field<Boolean>(STRING_WAITMEASUREINDICATE);
                data.IsAutoReMeasure = row.Field<Boolean>(STRING_ISAUTOREMEASURE);
                data.ReplicationNo = row.Field<Int32>(STRING_REPLICATIONNUMBER);
                data.MeasureDateTime = row.Field<DateTime>(STRING_MEASUREDATETIME);
            }

            return data;
        }

        /// <summary>
        /// 検体識別番号による関連付け情報取得
        /// </summary>
        /// <remarks>
        /// 検体識別番号によりDBのデータ行識別情報を返します。
        /// </remarks>
        /// <param name="individuallyNo">検体識別番号</param>
        /// <returns>データ行識別情報</returns>
        private List<Tuple<Int32, Int32, Int32, Int32>> linkKeyFromIndividuallyNo(Int32 individuallyNo)
        {
            // ＤＢ情報からら検体識別番号でキーを取り出す。
            IEnumerable<Tuple<Int32, Int32, Int32, Int32>> searched = from v in this.dataLinkDic
                                                                      where v.Key.Item2 == individuallyNo
                                                                      select v.Key;

            return searched.ToList();
        }
        /// <summary>
        /// 検体識別番号・分析項目番号による関連付け情報取得
        /// </summary>
        /// <remarks>
        /// 検体識別番号・分析項目番号によりDBのデータ行識別情報を返します。
        /// </remarks>
        /// <param name="individuallyNo">検体識別番号</param>
        /// <param name="protocolNo">分析項目番号</param>
        /// <returns>データ行識別情報</returns>
        private List<Tuple<Int32, Int32, Int32, Int32>> linkKeyFromIndividuallyNoAndProtocolNo(Int32 individuallyNo, Int32 protocolNo)
        {
            // ＤＢ情報からら検体識別番号でキーを取り出す。
            IEnumerable<Tuple<Int32, Int32, Int32, Int32>> searched = from v in this.dataLinkDic
                                                                      where v.Key.Item2 == individuallyNo &&
                                                                       v.Key.Item3 == protocolNo
                                                                      select v.Key;

            return searched.ToList();
        }

        /// <summary>
        /// データ削除
        /// </summary>
        /// <remarks>
        /// 検体識別番号から、再測定DBの情報を削除します。
        /// </remarks>
        /// <param name="individuallyNo">検体識別番号</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean DeleteData(Int32 individuallyNo, Int32 protocolNo)
        {
            List<Tuple<Int32, Int32, Int32, Int32>> deleteKey = this.linkKeyFromIndividuallyNoAndProtocolNo(individuallyNo, protocolNo);
            return this.DeleteData(deleteKey);
        }

        /// <summary>
        /// データ削除
        /// </summary>
        /// <remarks>
        /// 登録されている検体情報を指定して削除します。
        /// </remarks>
        /// <param name="deleteKeyList">削除情報リスト</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean DeleteData(List<Tuple<Int32, Int32, Int32, Int32>> deleteKeyList)
        {
            Boolean result = true;
            if (this.DataTable != null && deleteKeyList != null)
            {
                foreach (var deleteInfo in deleteKeyList)
                {
                    if (this.dataLinkDic.ContainsKey(deleteInfo))
                    {
                        this.dataLinkDic[deleteInfo].Delete();
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

            if (this.DataTable != null)
            {
                foreach (DataRow dat in this.DataTable.Rows)
                {
                    dat.Delete();
                }
                this.CommitSampleReMeasureInfo();
            }

            this.LoadDB();

            return result;
        }

        /// <summary>
        /// データ設定
        /// </summary>
        /// <remarks>
        /// データ設定しますThis data set
        /// </remarks>
        /// <param name="dataList">設定データリスト</param>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean SetDispData(List<SpecimenReMeasureGridViewDataSet> dataList)
        {
            Boolean result = true;
            if (this.DataTable != null)
            {
                try
                {
                    Tuple<Int32, Int32, Int32, Int32> linkKey = null;

                    // テーブル内容に更新する
                    foreach (SpecimenReMeasureGridViewDataSet data in dataList)
                    {

                        // *受付番号作成*
                        linkKey = new Tuple<Int32, Int32, Int32, Int32>
                            (data.ReceiptNumber, data.IndividuallyNo, data.MeasprotocolNo, data.ReplicationNo);

                        // あればDataTable更新無ければ追加
                        if (this.dataLinkDic.ContainsKey(linkKey))
                        {
                            // 更新
                            this.dataLinkDic[linkKey].BeginEdit();

                            // 検体問合せが来た際にシーケンスNoは作成される。
                            this.dataLinkDic[linkKey][STRING_SEQUENCENO] = data.SequenceNumber;

                            // 編集は稀釈倍率のみ
                            this.dataLinkDic[linkKey][STRING_MANUALDILUTION] = data.ManualDilution;
                            this.dataLinkDic[linkKey][STRING_AUTODILUTION] = data.AutoDilution;

                            // 再検査指定の編集であれば設定する。
                            this.dataLinkDic[linkKey][STRING_WAITMEASUREINDICATE] = data.WaitMeasureIndicate;

                            this.dataLinkDic[linkKey].EndEdit();
                        }
                        else
                        {
                            // 追加
                            DataRow newRow = this.DataTable.NewRow();

                            newRow.BeginEdit();

                            // シーケンスNoは検体問い合わせ時に発行されるので登録時点ではNull
                            newRow[STRING_MODULENO] = data.ModuleNo;
                            newRow[STRING_RECEIPTNO] = data.ReceiptNumber;
                            newRow[STRING_RACKID] = data.RackID.DispPreCharString;
                            newRow[STRING_RACKPOSITION] = data.RackPosition;
                            newRow[STRING_REPLICATIONNUMBER] = data.ReplicationNo;
                            newRow[STRING_SAMPLEID] = data.SampleID;
                            newRow[STRING_SPECIMENMATERIALTYPE] = data.SpecimenMaterialType;
                            newRow[STRING_COMMENT] = data.Comment;
                            newRow[STRING_MEASPROTOCOLNO] = data.MeasprotocolNo;
                            newRow[STRING_AUTODILUTION] = data.AutoDilution;
                            newRow[STRING_MANUALDILUTION] = data.ManualDilution;
                            newRow[STRING_COUNTVAL] = data.Count;
                            newRow[STRING_CONCENTRATION] = data.Concentration;
                            newRow[STRING_REMARKID] = data.Remark.Value;
                            newRow[STRING_INDIVIDUALLYNO] = data.IndividuallyNo;
                            newRow[STRING_ISAUTOREMEASURE] = data.IsAutoReMeasure;
                            newRow[STRING_WAITMEASUREINDICATE] = false;
                            newRow[STRING_SEQUENCENO] = data.SequenceNumber;
                            newRow[STRING_MEASUREDATETIME] = data.MeasureDateTime;

                            newRow.EndEdit();
                            this.DataTable.Rows.Add(newRow);
                            this.dataLinkDic[linkKey] = newRow;

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
        /// データ追加
        /// </summary>
        /// <remarks>
        /// 再測定検体グリッド表示内容を追加します。
        /// </remarks>
        /// <param name="dataList">追加データリスト</param>
        /// <returns>True:成功 false:失敗</returns>
        public Boolean AddRemesureData(List<SpecimenReMeasureGridViewDataSet> dataList)
        {
            Boolean result = true;
            if (this.DataTable != null)
            {
                try
                {
                    // 重複check
                    var datas = dataList.ToDictionary((data) => new Tuple<Int32, Int32, Int32, Int32>
                        (data.ReceiptNumber, data.IndividuallyNo, data.MeasprotocolNo, data.ReplicationNo));
                    foreach (var key in datas.Keys)
                    {
                        result &= !this.dataLinkDic.ContainsKey(key);
                    }

                    if (result)
                    {
                        // テーブル内容に更新するupdate the table contents
                        foreach (var data in datas)
                        {
                            // *受付番号作成*  not Referece

                            // 追加
                            DataRow newRow = this.DataTable.NewRow();

                            newRow.BeginEdit();
                            // シーケンスNoは検体問い合わせ時に発行されるので登録時点ではNull
                            newRow.SetField<Int32>(STRING_MODULENO, data.Value.ModuleNo);
                            newRow.SetField<Int32>(STRING_SEQUENCENO, data.Value.SequenceNumber);
                            newRow.SetField<Int32>(STRING_RECEIPTNO, data.Value.ReceiptNumber);
                            newRow.SetField<String>(STRING_RACKID, data.Value.RackID.DispPreCharString);
                            newRow.SetField<Int32>(STRING_RACKPOSITION, data.Value.RackPosition);
                            newRow.SetField<String>(STRING_SAMPLEID, data.Value.SampleID);
                            newRow.SetField<Int32>(STRING_SPECIMENMATERIALTYPE, (Int32)data.Value.SpecimenMaterialType);
                            newRow.SetField<String>(STRING_COMMENT, data.Value.Comment);
                            newRow.SetField<Int32>(STRING_MEASPROTOCOLNO, data.Value.MeasprotocolNo);
                            newRow.SetField<Int32>(STRING_AUTODILUTION, data.Value.AutoDilution);
                            newRow.SetField<Int32>(STRING_MANUALDILUTION, data.Value.ManualDilution);
                            newRow.SetField<Int32?>(STRING_COUNTVAL, data.Value.Count);
                            newRow.SetField<String>(STRING_CONCENTRATION, data.Value.Concentration);
                            newRow.SetField<Int64>(STRING_REMARKID, data.Value.Remark);
                            newRow.SetField<Int32>(STRING_INDIVIDUALLYNO, data.Value.IndividuallyNo);
                            newRow.SetField<DateTime>(STRING_MEASUREDATETIME, data.Value.MeasureDateTime);
                            newRow.SetField<Boolean>(STRING_ISAUTOREMEASURE, data.Value.IsAutoReMeasure);
                            newRow.SetField<Boolean>(STRING_WAITMEASUREINDICATE, false);
                            newRow.SetField<Int32>(STRING_REPLICATIONNUMBER, data.Value.ReplicationNo);

                            newRow.EndEdit();
                            this.DataTable.Rows.Add(newRow);
                            this.dataLinkDic[data.Key] = newRow;
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                          CarisXLogInfoBaseExtention.Empty, "add remeasure data over");
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
        public void CommitSampleReMeasureInfo()
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
            foreach (DataRow row in this.DataTable.Rows)
            {
                this.dataLinkDic.Add(new Tuple<Int32, Int32, Int32, Int32>
                    (row.Field<Int32>(STRING_RECEIPTNO), row.Field<Int32>(STRING_INDIVIDUALLYNO)
                    , row.Field<Int32>(STRING_MEASPROTOCOLNO), row.Field<Int32>(STRING_REPLICATIONNUMBER)), row);
            }
        }

        #endregion
    }
}
