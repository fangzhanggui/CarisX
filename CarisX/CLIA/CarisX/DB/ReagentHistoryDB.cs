using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.DB;
using Oelco.Common.Utility;
using Oelco.Common.Log;
using System.Data;
using Oelco.CarisX.Const;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Log;
using Oelco.CarisX.Utility;
using System.Reflection;

namespace Oelco.CarisX.DB
{
    /// <summary>
    /// 試薬履歴情報データクラス
    /// </summary>
    public class ReagentHistoryData : DataRowWrapperBase
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public ReagentHistoryData(DataRowWrapperBase data)
            : base(data)
        {
        }
        #endregion

        #region [プロパティ]
        /// <summary>
        /// 試薬コードの取得、設定
        /// </summary>
        public Int32 ReagentCode
        {
            get
            {
                return this.Field<Int32>( ReagentHistoryDB.STRING_REAGENTCODE );
            }
            set
            {
                this.SetField<Int32>( ReagentHistoryDB.STRING_REAGENTCODE, value );
            }
        }

        /// <summary>
        /// 試薬種の取得、設定
        /// </summary>
        public Int32 ReagentTypeDetail
        {
            get
            {
                return this.Field<Int32>(ReagentHistoryDB.STRING_REAGENTTYPEDETAIL);
            }
            set
            {
                this.SetField<Int32>(ReagentHistoryDB.STRING_REAGENTTYPEDETAIL, value);
            }
        }

        /// <summary>
        /// 試薬ロット番号の取得、設定
        /// </summary>
        public String LotNo
        {
            get
            {
                return this.Field<String>(ReagentHistoryDB.STRING_REAGENTLOTNO);
            }
            set
            {
                //試薬バッチ番号8〜6桁（试剂批号8位转为6位）
                String lotNo = value;
                if (Singleton<Oelco.Common.Parameter.ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                {
                    if (!String.IsNullOrEmpty(lotNo))
                    {
                        lotNo = int.Parse(lotNo).ToString();
                    }
                }
                this.SetField<String>(ReagentHistoryDB.STRING_REAGENTLOTNO, lotNo);
            }
        }

        /// <summary>
        /// シリアル番号の取得、設定
        /// </summary>
        public Int32? SerialNo
        {
            get
            {
                return this.Field<Int32?>(ReagentHistoryDB.STRING_REAGENTSERIALNO);
            }
            set
            {
                this.SetField<Int32?>(ReagentHistoryDB.STRING_REAGENTSERIALNO, value);
            }
        }

        /// <summary>
        /// 残量の取得、設定
        /// </summary>
        public Int32 Remain
        {
            get
            {
                return this.Field<Int32>(ReagentHistoryDB.STRING_REAGENTREMAIN);
            }
            set
            {
                this.SetField<Int32>(ReagentHistoryDB.STRING_REAGENTREMAIN, value);
            }
        }

        /// <summary>
        /// 設置日の取得、設定
        /// </summary>
        public DateTime InstallationDate
        {
            get
            {
                return this.Field<DateTime>(ReagentHistoryDB.STRING_INSTALLATIONDATE);
            }
            set
            {
                this.SetField<DateTime>(ReagentHistoryDB.STRING_INSTALLATIONDATE, value);
            }
        }

        /// <summary>
        /// グループ番号の取得、設定
        /// </summary>
        public Int64 GroupNo
        {
            get
            {
                return this.Field<Int64>(ReagentHistoryDB.STRING_GROUPNO);
            }
            set
            {
                this.SetField<Int64>(ReagentHistoryDB.STRING_GROUPNO, value);
            }
        }
        #endregion
    }

    /// <summary>
    /// 試薬履歴情報DBクラス
    /// </summary>
    public class ReagentHistoryDB : DBAccessControl
    {
        #region [定数定義]
        /// <summary>
        /// 試薬コード(DBテーブル：reagentHistory列名)
        /// </summary>
        public const String STRING_REAGENTCODE = "reagentCode";
        /// <summary>
        /// 試薬種詳細(DBテーブル：reagentHistory列名)
        /// </summary>
        public const String STRING_REAGENTTYPEDETAIL = "reagentTypeDetail";
        /// <summary>
        /// 試薬ロット番号(DBテーブル：reagentHistory列名)
        /// </summary>
        public const String STRING_REAGENTLOTNO = "reagentLotNo";
        /// <summary>
        /// シリアル番号(DBテーブル：reagentHistory列名)
        /// </summary>
        public const String STRING_REAGENTSERIALNO = "reagentSerialNo";
        /// <summary>
        /// 残量(DBテーブル：reagentHistory列名)
        /// </summary>
        public const String STRING_REAGENTREMAIN = "reagentRemain";
        /// <summary>
        /// 設置日(DBテーブル：reagentHistory列名)
        /// </summary>
        public const String STRING_INSTALLATIONDATE = "installationDate";
        /// <summary>
        /// グループ番号(DBテーブル：reagentHistory列名)
        /// </summary>
        public const String STRING_GROUPNO = "groupNo";

        #endregion

        #region [プロパティ]

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT * FROM dbo.reagentHistory";
            }
        }

        #endregion

        #region [インスタンス変数]

        /// <summary>
        /// 更新時刻
        /// </summary>
        private DateTime[] timeStamp;

        #endregion

        #region [コンストラクタ]
        public ReagentHistoryDB()
        {
            timeStamp = new DateTime[Enum.GetValues(typeof(RackModuleIndex)).Length];
            for (int i = 0; i < timeStamp.Length; i++)
            {
                timeStamp[i] = DateTime.MinValue;
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 残量情報取得
        /// </summary>
        /// <remarks>
        /// 残量情報をDBから取得します。
        /// </remarks>
        /// <param name="remainInfo">残量情報</param>
        public void GetReagentHistory(ref IRemainAmountInfoSet remainInfo)
        {
            String dbgMsgHead = String.Format("[[Investigation log]]ReagentHistoryDB::{0} ", MethodBase.GetCurrentMethod().Name);
            String dbgMsg = dbgMsgHead;

            // 残量情報をDBデータから設定
            List<ReagentHistoryData> list = this.GetData();

            foreach (var reagentTable in remainInfo.ReagentRemainTable)
            {
                var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(reagentTable.ReagCode);
                if (protocol != null)
                {
                    dbgMsg = dbgMsgHead + String.Format("Serch reagenthistory ReagentCode{0}, ReagentTypeDetail={1}, LotNo={2}, SerialNo={3} "
                        , reagentTable.ReagCode, reagentTable.ReagTypeDetail, reagentTable.RemainingAmount.LotNumber, reagentTable.RemainingAmount.SerialNumber);

                    var ReagentHistoryDataList = list.Where(v => ( v.ReagentCode == reagentTable.ReagCode ) 
                                                                && (v.ReagentTypeDetail == (Int32)reagentTable.ReagTypeDetail)
                                                                && (v.LotNo == reagentTable.RemainingAmount.LotNumber)
                                                                && (v.SerialNo == reagentTable.RemainingAmount.SerialNumber));
                    if (ReagentHistoryDataList.Count() == 0)
                    {
                        dbgMsg = dbgMsg + String.Format("not found data ");

                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);

                        //履歴情報が無い場合
                        //残量をゼロにする
                        reagentTable.RemainingAmount.Remain = 0;

                        //開封日算出（設置日＋試薬開封後有効期間）
                        DateTime openDate = reagentTable.RemainingAmount.InstallationData.AddDays(protocol.DayOfReagentValid);

                        //開封日が有効期限より早い場合は上書きする
                        if (openDate < reagentTable.RemainingAmount.TermOfUse)
                        {
                            reagentTable.RemainingAmount.TermOfUse = openDate;
                        }
                    }
                    foreach (var ReagentHistoryData in ReagentHistoryDataList)
                    {
                        dbgMsg = dbgMsg + String.Format("found data ");

                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);

                        //履歴情報に持つ残量で上書き
                        reagentTable.RemainingAmount.Remain = ReagentHistoryData.Remain;

                        // 設置日の取得
                        reagentTable.RemainingAmount.InstallationData = ReagentHistoryData.InstallationDate;

                        //開封日算出（設置日＋試薬開封後有効期間）
                        DateTime openDate = ReagentHistoryData.InstallationDate.AddDays(protocol.DayOfReagentValid);

                        //開封日が有効期限より早い場合は上書きする
                        if (openDate < reagentTable.RemainingAmount.TermOfUse )
                        {
                            reagentTable.RemainingAmount.TermOfUse = openDate;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 残量履歴情報設定
        /// </summary>
        /// <remarks>
        /// 残量履歴情報をDBに設定します。
        /// </remarks>
        /// <param name="remainInfo">残量情報</param>
        public void SetReagentHistory(IRemainAmountInfoSet remainInfo, Boolean overwrite, Int32 moduleID, Boolean errorNotify)
        {
            String dbgMsgHead = String.Format("[[Investigation log]]ReagentHistoryDB::{0} ", MethodBase.GetCurrentMethod().Name);
            String dbgMsg = dbgMsgHead;

            // 古い時刻のデータで更新しないようにする。
            if (remainInfo.TimeStamp >= this.timeStamp[moduleID])
            {
                //試薬履歴情報を取得
                List<ReagentHistoryData> list = this.GetData();

                //試薬残量情報のデータ分処理を行う
                for (int i = 0; i < remainInfo.ReagentRemainTable.Length; i++)
                {
                    dbgMsg = dbgMsgHead;

                    //処理する試薬残量情報を設定
                    var reagentTable = remainInfo.ReagentRemainTable[i];

                    //試薬コードがゼロの場合は処理しない。（プロトコル番号で参照するとプロトコル情報が取得出来てしまうので回避する
                    if (reagentTable.ReagCode == 0) continue;

                    //プロトコル情報が取得できる内容のみ反映する
                    var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(reagentTable.ReagCode);
                    if (protocol != null)
                    {
                        dbgMsg = dbgMsg + String.Format("Serch reagenthistory ReagentCode{0}, ReagentTypeDetail={1}, LotNo={2}, SerialNo={3} "
                            , reagentTable.ReagCode, reagentTable.ReagTypeDetail, reagentTable.RemainingAmount.LotNumber, reagentTable.RemainingAmount.SerialNumber);

                        var ReagentHistoryDataList = list.Where(v => ( v.ReagentCode == reagentTable.ReagCode )
                                                                    && (v.ReagentTypeDetail == (Int32)reagentTable.ReagTypeDetail)
                                                                    && (v.LotNo == reagentTable.RemainingAmount.LotNumber)
                                                                    && (v.SerialNo == reagentTable.RemainingAmount.SerialNumber));
                        if (ReagentHistoryDataList.Count() == 0)
                        {
                            dbgMsg = dbgMsg + String.Format("Registration because there is no data {0} ", reagentTable.RemainingAmount.Remain);

                            //試薬履歴情報にデータが存在しない場合は登録
                            ReagentHistoryData newRow = new ReagentHistoryData(this.DataTable.NewRow());
                            newRow.ReagentCode = reagentTable.ReagCode;
                            newRow.ReagentTypeDetail = (Int32)reagentTable.ReagTypeDetail;
                            newRow.LotNo = reagentTable.RemainingAmount.LotNumber;
                            newRow.SerialNo = reagentTable.RemainingAmount.SerialNumber;
                            newRow.InstallationDate = reagentTable.RemainingAmount.InstallationData;
                            newRow.Remain = reagentTable.RemainingAmount.Remain;
                            newRow.GroupNo = 0; //別途SetGroupNoで設定する

                            list.Add(newRow);
                        }
                        else
                        {
                            dbgMsg = dbgMsg + String.Format("found data ");

                            //取得した残量履歴情報を抽出
                            var ReagentHistoryData = ReagentHistoryDataList.FirstOrDefault();

                            //キーでデータが存在する場合
                            if (overwrite)
                            {
                                dbgMsg = dbgMsg + String.Format("update remain {0} ", reagentTable.RemainingAmount.Remain);

                                //上書きを行う場合のみ処理を行う
                                ReagentHistoryData.Remain = reagentTable.RemainingAmount.Remain;
                            }
                            else
                            {
                                dbgMsg = dbgMsg + String.Format("no update ");
                            }
                        }

                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                    }
                }

                //グループ番号の設定
                SetGroupNo(remainInfo, moduleID, ref list, errorNotify);

                this.SetData(list);

                this.timeStamp[moduleID] = remainInfo.TimeStamp;
            }
            else
            {
                dbgMsg = dbgMsg + String.Format("Do not process old time data");
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
            }
        }

        /// <summary>
        /// グループ番号設定
        /// </summary>
        /// <remarks>
        /// R1、R2、M試薬の正しい組合せに対して、グループ番号を設定します。（マッチングチェック用）
        /// 条件：同一ポート内で試薬コード・ロット番号・メーカーコードが一致するR試薬とM試薬の組合せ
        /// </remarks>
        /// <param name="remainInfo">残量情報</param>
        /// <param name="moduleID">モジュールID</param>
        public void SetGroupNo(IRemainAmountInfoSet remainInfo, Int32 moduleID, ref List<ReagentHistoryData> list, Boolean errorNotify)
        {
            String dbgMsgHead = String.Format("[[Investigation log]]ReagentHistoryDB::{0} ", MethodBase.GetCurrentMethod().Name);
            String dbgMsg = dbgMsgHead;

            ReagentRemainTable reagR1 = new ReagentRemainTable();
            ReagentRemainTable reagR2 = new ReagentRemainTable();
            ReagentRemainTable reagM = new ReagentRemainTable();
            Boolean errFlgPort = false;     //エラーフラグ立った場合は200-3エラーを発報

            for (int i = 0; i < CarisXConst.REAGENT_PORT_MAX; i++)
            {
                String strPortNo = String.Format("PortNo = {0} ", i + 1);
                dbgMsg = dbgMsgHead + String.Format("Port{0} [reagCode reagtype makercode]=>", i + 1 );

                //R1は設置されていない場合があるので、Rの情報はR2ベースで確認する
                errFlgPort = false;

                reagR1 = remainInfo.ReagentRemainTable[(i * 3)];
                reagR2 = remainInfo.ReagentRemainTable[(i * 3) + 1];
                reagM = remainInfo.ReagentRemainTable[(i * 3) + 2];

                if (reagR2.ReagCode != 0 && reagM.ReagCode != 0)
                {
                    dbgMsg = dbgMsg + String.Format("[{0} {1} {2}],", reagR1.ReagCode, reagR1.ReagType, reagR1.MakerCode);
                    dbgMsg = dbgMsg + String.Format("[{0} {1} {2}],", reagR2.ReagCode, reagR2.ReagType, reagR2.MakerCode);
                    dbgMsg = dbgMsg + String.Format("[{0} {1} {2}] ", reagM.ReagCode, reagM.ReagType, reagM.MakerCode);

                    //M試薬とR試薬の組合せになっていない場合はエラー
                    if (!(reagR2.ReagType == (Int32)ReagentType.R1R2 && reagM.ReagType == (Int32)ReagentType.M))
                    {
                        dbgMsg = dbgMsg + String.Format("Not a combination of M reagent and R reagent[{0},{1}] ", reagR2.ReagType, reagM.ReagType);
                        errFlgPort = true;
                    }

                    //メーカーコードが一致しない場合はエラー
                    if (reagR2.MakerCode != reagM.MakerCode)
                    {
                        dbgMsg = dbgMsg + String.Format("Not matching maker code[{0},{1}] ", reagR2.MakerCode, reagM.MakerCode);
                        errFlgPort = true;
                    }

                    //試薬コードが一致しない場合はエラー
                    if (reagR2.ReagCode != reagM.ReagCode)
                    {
                        dbgMsg = dbgMsg + String.Format("Not matching reagent code[{0},{1}] ", reagR2.ReagCode, reagM.ReagCode);
                        errFlgPort = true;
                    }

                    //ロット番号が一致しない場合はエラー
                    if (reagR2.RemainingAmount.LotNumber != reagM.RemainingAmount.LotNumber)
                    {
                        dbgMsg = dbgMsg + String.Format("Not matching lot no[{0},{1}] ", reagR2.RemainingAmount.LotNumber, reagM.RemainingAmount.LotNumber);
                        errFlgPort = true;
                    }

                    //エラーがなかった場合
                    if (!errFlgPort)
                    {
                        var reagHistoryR1 = list.Where(v => (v.ReagentCode == reagR1.ReagCode)
                                                            && (v.ReagentTypeDetail == (Int32)reagR1.ReagTypeDetail)
                                                            && (v.LotNo == reagR1.RemainingAmount.LotNumber)
                                                            && (v.SerialNo == reagR1.RemainingAmount.SerialNumber)).FirstOrDefault();
                        var reagHistoryR2 = list.Where(v => (v.ReagentCode == reagR2.ReagCode)
                                                            && (v.ReagentTypeDetail == (Int32)reagR2.ReagTypeDetail)
                                                            && (v.LotNo == reagR2.RemainingAmount.LotNumber)
                                                            && (v.SerialNo == reagR2.RemainingAmount.SerialNumber)).FirstOrDefault();
                        var reagHistoryM = list.Where(v => (v.ReagentCode == reagM.ReagCode)
                                                        && (v.ReagentTypeDetail == (Int32)reagM.ReagTypeDetail)
                                                        && (v.LotNo == reagM.RemainingAmount.LotNumber)
                                                        && (v.SerialNo == reagM.RemainingAmount.SerialNumber)).FirstOrDefault();

                        if (reagHistoryR2 != null && reagHistoryM != null)
                        {
                            if (reagHistoryR2.GroupNo == 0 && reagHistoryM.GroupNo == 0)
                            {
                                //履歴情報が存在するが、グループ番号が設定されていないのでグループ番号を設定する

                                //グループ番号の最大値＋１をグループ番号とする
                                Int64 GroupNo;
                                if (list.Count == 0)
                                    GroupNo = 1;
                                else
                                    GroupNo = list.Max(v => v.GroupNo) >= 0 ? list.Max(v => v.GroupNo) + 1 : 1;

                                if (reagHistoryR1 != null) reagHistoryR1.GroupNo = GroupNo;     //R1の情報が取得出来ている場合のみ反映
                                reagHistoryR2.GroupNo = GroupNo;
                                reagHistoryM.GroupNo = GroupNo;

                                dbgMsg = dbgMsg + String.Format("set to group no {0}", GroupNo);
                                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                            }
                            else
                            {
                                //グループ番号が設定されている履歴情報が存在する

                                dbgMsg = dbgMsg + String.Format("already set to group no R2:{0} M:{1}", reagHistoryR2.GroupNo, reagHistoryM.GroupNo);
                                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                            }
                        }
                        else
                        {
                            //履歴情報が存在しない（発生しない予定）
                            dbgMsg = dbgMsg + String.Format("not found reagent hisotyr");
                            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                        }
                    }
                    else
                    {
                        //グループ化出来ない組合せ
                        dbgMsg = dbgMsg + String.Format("grouping error");
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                    }

                    //エラーがあり、エラー通知を行う場合
                    if (errFlgPort && errorNotify)
                    {
                        //200-3のエラーを発報
                        CarisXSubFunction.WriteDPRErrorHist(new DPRErrorCode(200, 3, moduleID), 0, strPortNo);
                    }
                }
                else
                {
                    //試薬コードが設定されていない（この時は200-3のエラーを発報しない）
                    dbgMsg = dbgMsg + String.Format("reagent not setting reagentcode R2:{0} M:{1}", reagR2.ReagCode, reagM.ReagCode);
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                }
            }
        }

        /// <summary>
        /// 試薬履歴情報テーブルの取得
        /// </summary>
        /// <remarks>
        /// 試薬履歴情報テーブルを返します。
        /// </remarks>
        /// <returns></returns>
        public List<ReagentHistoryData> GetData()
        {
            List<ReagentHistoryData> result = new List<ReagentHistoryData>();

            if (this.DataTable != null)
            {
                try
                {
                    //　コピーデータリストを取得
                    var dataTableList = this.DataTable.AsEnumerable().ToList();

                    result = dataTableList.Select(row => new ReagentHistoryData(row)).ToList();
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
        /// 残量情報取得
        /// </summary>
        /// <remarks>
        /// 残量情報をDBから取得します。
        /// </remarks>
        /// <param name="remainInfo">残量情報</param>
        public List<Int64> GetGroupNoList(IRemainAmountInfoSet remainInfo)
        {
            String dbgMsgHead = String.Format("[[Investigation log]]ReagentHistoryDB::{0} ", MethodBase.GetCurrentMethod().Name);
            String dbgMsg = dbgMsgHead;

            // 残量情報をDBデータから設定
            List<ReagentHistoryData> list = this.GetData();
            List<Int64> rtnlist = new List<Int64>();

            foreach (var reagentTable in remainInfo.ReagentRemainTable)
            {
                var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolNo(reagentTable.ReagCode);
                if (protocol != null)
                {
                    dbgMsg = dbgMsgHead + String.Format("Serch reagenthistory ReagentCode{0}, ReagentTypeDetail={1}, LotNo={2}, SerialNo={3} "
                        , reagentTable.ReagCode, reagentTable.ReagTypeDetail, reagentTable.RemainingAmount.LotNumber, reagentTable.RemainingAmount.SerialNumber);

                    var ReagentHistoryDataList = list.Where(v => (v.ReagentCode == reagentTable.ReagCode)
                                                                && (v.ReagentTypeDetail == (Int32)reagentTable.ReagTypeDetail)
                                                                && (v.LotNo == reagentTable.RemainingAmount.LotNumber)
                                                                && (v.SerialNo == reagentTable.RemainingAmount.SerialNumber));
                    if (ReagentHistoryDataList.Count() == 0)
                    {
                        dbgMsg = dbgMsg + String.Format("not found ");

                        rtnlist.Add(0);
                    }
                    foreach (var ReagentHistoryData in ReagentHistoryDataList)
                    {
                        dbgMsg = dbgMsg + String.Format("found ");

                        rtnlist.Add(ReagentHistoryData.GroupNo);
                    }

                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                }
                else
                {
                    rtnlist.Add(0);
                }
            }

            return rtnlist;
        }

        /// <summary>
        /// 試薬履歴情報データの設定
        /// </summary>
        /// <remarks>
        /// 試薬履歴情報データの同期を行います。
        /// </remarks>
        /// <param name="list">変更、削除操作済みデータ</param>
        public void SetData(List<ReagentHistoryData> list)
        {
            list.SyncDataListToDataTable(this.DataTable);
        }

        /// <summary>
        /// 試薬履歴情報取得
        /// </summary>
        /// <remarks>
        /// 試薬履歴情報をDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
        }

        /// <summary>
        /// 試薬履歴情報テーブル書込み
        /// </summary>
        /// <remarks>
        /// 試薬履歴情報をDBに書き込みます。
        /// </remarks>
        public void CommitData()
        {
            this.updateBaseTable();
        }

        /// <summary>
        /// 件数上限を超えるデータの削除
        /// </summary>
        /// <remarks>
        /// 試薬履歴情報テーブルで、同一試薬・試薬種詳細・ロットを持つデータは400件までの管理とする為、
        /// それを超えた際は過去の情報を削除する
        /// </remarks>
        /// <returns></returns>
        protected override void removeLimitOver()
        {
            String dbgMsgHead = String.Format("[[Investigation log]]ReagentHistoryDB::{0} ", MethodBase.GetCurrentMethod().Name);
            String dbgMsg = dbgMsgHead;

            if (this.DataTable != null)
            {
                //　コピーデータリストを取得
                var dataTableList = this.DataTable.AsEnumerable().ToList();

                //試薬、試薬種詳細、ロットでグルーピング
                var groupdatalist = dataTableList.Select(row => new ReagentHistoryData(row))
                    .GroupBy(data => new { data.ReagentCode, data.ReagentTypeDetail });
                foreach (var groupdata in groupdatalist)
                {
                    dbgMsg = dbgMsgHead + String.Format("key reagent:{0}、typedetail:{1}、count:{2} ", groupdata.Key.ReagentCode, groupdata.Key.ReagentTypeDetail, groupdata.Count());

                    //規定の件数上限を超えているか
                    if (groupdata.Count() > CarisXConst.MAX_REAGENT_HISTORY_COUNT)
                    {
                        //規定の件数上限を超えている場合、該当件数以上は
                        dbgMsg += String.Format("delete because it exceeds the upper limit ");

                        var groupoverlimitdata = groupdata.OrderByDescending(data => data.InstallationDate).Skip(CarisXConst.MAX_REAGENT_HISTORY_COUNT).ToList();
                        groupoverlimitdata.DeleteAllDataList();
                        this.SetData(groupoverlimitdata);
                    }
                    else
                    {
                        dbgMsg += String.Format("do not delete because it exceeds the upper limit ");

                    }

                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, dbgMsg);
                }
            }
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

            this.ExecuteSql("DELETE FROM dbo.reagentHistory");

            sw.Stop();
            System.Diagnostics.Debug.WriteLine(String.Format("試薬残量履歴DB削除時間:{0}ミリ秒", sw.ElapsedMilliseconds));
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                , CarisXLogInfoBaseExtention.Empty, String.Format("試薬残量履歴DB削除時間:{0}ミリ秒", sw.ElapsedMilliseconds));
            sw.Reset();
            sw.Start();
            this.LoadDB();
            sw.Stop();
            System.Diagnostics.Debug.WriteLine(String.Format("DBロード時間{0}ミリ秒", sw.ElapsedMilliseconds));
            Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                , CarisXLogInfoBaseExtention.Empty, String.Format("DBロード時間{0}ミリ秒", sw.ElapsedMilliseconds));

            return result;
        }

        /// <summary>
        /// DBの設置時刻の型変換クエリ
        /// <remarks>
        /// 今後、カラムの型変換を行う際は中のクエリを変更するような処理に変える
        /// </remarks>
        /// </summary>
        public void ChangeTableDataType()
        {
            String sql;
            sql = String.Format("alter table dbo.reagentHistory alter column {0} datetime2(7)", ReagentHistoryDB.STRING_INSTALLATIONDATE);
            this.ExecuteSql(sql);
        }

        /// <summary>
        /// 設置日の若い試薬のロット番号を返す
        /// </summary>
        /// <param name="reagentData">分析可能な試薬データ</param>
        /// <returns>指定の試薬コードの試薬ロット番号</returns>
        public String GetLotNo( List<ReagentData> reagentData)
        {
            String reagentLotNumber = String.Empty;
            EnumerableRowCollection<ReagentHistoryData> reagentHistoryDatas = null;

            // 試薬履歴データの取得
            reagentHistoryDatas = this.DataTable.AsEnumerable().Select(d => new ReagentHistoryData(d));

            // テーブルの結合
            // ロット番号とシリアル番号が等しいデータを内部結合する
            // 設置日で昇順のソートを行う
            var result = reagentHistoryDatas.Join(reagentData, e => new { e.LotNo, e.SerialNo }, 
                         d => new { d.LotNo, d.SerialNo },
                         ( e, d ) => new
                         {
                             LotNo = d.LotNo,
                             InstallationDate = e.InstallationDate
                         }).OrderBy(v => v.InstallationDate).ToList();

            // テーブルの内部結合ができた場合
            if (result != null)
            {
                reagentLotNumber = result.First().LotNo;
            }

            return reagentLotNumber;
        }

        #endregion


    }
}
