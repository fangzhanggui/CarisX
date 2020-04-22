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
using Oelco.CarisX.Status;
using Oelco.CarisX.Utility;
using Oelco.Common.Parameter;

namespace Oelco.CarisX.DB
{
    /// <summary>
    /// 試薬情報データクラス
    /// </summary>
    public class ReagentData : DataRowWrapperBase
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data"></param>
        public ReagentData(DataRowWrapperBase data)
            : base(data)
        {
        }
        #endregion

        #region [プロパティ]
        /// <summary>
        /// モジュール番号の取得、設定
        /// </summary>
        public Int32 ModuleNo
        {
            get
            {
                return this.Field<Int32>(ReagentDB.STRING_MODULENO);
            }
            set
            {
                this.SetField<Int32>(ReagentDB.STRING_MODULENO, value);
            }
        }

        /// <summary>
        /// 試薬コードの取得、設定
        /// </summary>
        public Int32? ReagentCode
        {
            get
            {
                return this.Field<Int32?>(ReagentDB.STRING_REAGENTCODE);
            }
            set
            {
                this.SetField<Int32?>(ReagentDB.STRING_REAGENTCODE, value);
            }
        }

        /// <summary>
        /// 試薬種別の取得、設定
        /// </summary>
        public Int32 ReagentKind
        {
            // DBの試薬種別フィールドが試薬種別詳細（M,R1,R2,T1,T2)・試薬種別（試薬、チップ、セル等）と競合する為、
            // 試薬コードが設定されている場合は試薬種別=試薬で固定返却するよう対応。
            // また、従来では使用されていなかったsetを削除、同等の機能をSetReagentDetailKindへ実装。
            get
            {
                if (this.ReagentCode.HasValue)
                {
                    return (Int32)Oelco.CarisX.Const.ReagentKind.Reagent;
                }
                else
                {
                    return this.Field<Int32>(ReagentDB.STRING_REAGENTKIND);
                }
            }
        }

        /// <summary>
        /// 試薬詳細取得
        /// </summary>
        /// <returns></returns>
        public Int32 GetReagentDetailKind()
        {
            return this.Field<Int32>(ReagentDB.STRING_REAGENTKIND);
        }

        /// <summary>
        /// 試薬詳細設定
        /// </summary>
        /// <param name="reagentDetailkind"></param>
        public void SetReagentDetailKind(Int32 reagentDetailkind)
        {
            this.SetField<Int32>(ReagentDB.STRING_REAGENTKIND, reagentDetailkind);
        }

        /// <summary>
        /// 設置ポート番号の取得、設定
        /// </summary>
        public Int32? PortNo
        {
            get
            {
                return this.Field<Int32?>(ReagentDB.STRING_REAGENTPORTNO);
            }
            set
            {
                this.SetField<Int32?>(ReagentDB.STRING_REAGENTPORTNO, value);
            }
        }

        /// <summary>
        /// 試薬ロット番号の取得、設定
        /// </summary>
        public String LotNo
        {
            get
            {
                return this.Field<String>(ReagentDB.STRING_REAGENTLOTNO);
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
                this.SetField<String>(ReagentDB.STRING_REAGENTLOTNO, lotNo);
            }
        }

        /// <summary>
        /// シリアル番号の取得、設定
        /// </summary>
        public Int32? SerialNo
        {
            get
            {
                return this.Field<Int32?>(ReagentDB.STRING_REAGENTSERIALNO);
            }
            set
            {
                this.SetField<Int32?>(ReagentDB.STRING_REAGENTSERIALNO, value);
            }
        }

        /// <summary>
        /// 有効期限の取得、設定
        /// </summary>
        public DateTime? ExpirationDate
        {
            get
            {
                return this.Field<DateTime?>(ReagentDB.STRING_EXPIRATIONDATE);
            }
            set
            {
                this.SetField<DateTime?>(ReagentDB.STRING_EXPIRATIONDATE, value);
            }
        }

        /// <summary>
        /// 使用期限の取得、設定
        /// </summary>
        public DateTime? StabilityDate
        {
            get
            {
                return this.Field<DateTime?>(ReagentDB.STRING_STABILITYDATE);
            }
            set
            {
                this.SetField<DateTime?>(ReagentDB.STRING_STABILITYDATE, value);
            }
        }

        /// <summary>
        /// 残量の取得、設定
        /// </summary>
        public Int32? Remain
        {
            get
            {
                return this.Field<Int32?>(ReagentDB.STRING_REAGENTREMAIN);
            }
            set
            {
                this.SetField<Int32?>(ReagentDB.STRING_REAGENTREMAIN, value);
            }
        }

        /// <summary>
        /// 使用状態の取得、設定
        /// </summary>
        public Boolean? IsUse
        {
            get
            {
                return this.Field<Boolean?>(ReagentDB.STRING_ISUSE);
            }
            set
            {
                this.SetField<Boolean?>(ReagentDB.STRING_ISUSE, value);
            }
        }

        /// <summary>
        /// 容量の取得、設定
        /// </summary>
        public Int32 Capacity
        {
            get
            {
                //【注意】使用する場合、DBから取得した値が必ずNullでないこと
                return this.Field<Int32>(ReagentDB.STRING_BOTTOLECAPACITY);
            }
            set
            {
                this.SetField<Int32>(ReagentDB.STRING_BOTTOLECAPACITY, value);
            }
        }

        /// <summary>
        /// メーカーコードの取得、設定
        /// </summary>
        public String MakerCode
        {
            get
            {
                return this.Field<String>(ReagentDB.STRING_MAKERCODE);
            }
            set
            {
                this.SetField<String>(ReagentDB.STRING_MAKERCODE, value);
            }
        }

        /// <summary>
        /// 試薬種(M,R1/R2,T1/T2)の取得、設定
        /// </summary>
        public Int32 ReagentType
        {
            get
            {
                return this.Field<Int32>(ReagentDB.STRING_REAGENTTYPE);
            }
            set
            {
                this.SetField<Int32>(ReagentDB.STRING_REAGENTTYPE, value);
            }
        }

        /// <summary>
        /// 試薬種詳細(M,R1,R2,T1,T2)の取得、設定
        /// </summary>
        public Int32 ReagentTypeDetail
        {
            get
            {
                return this.Field<Int32>(ReagentDB.STRING_REAGENTTYPEDETAIL);
            }
            set
            {
                this.SetField<Int32>(ReagentDB.STRING_REAGENTTYPEDETAIL, value);
            }
        }
        #endregion
    }

    /// <summary>
    /// 試薬情報DBクラス
    /// </summary>
    public class ReagentDB : DBAccessControl
    {
        #region [定数定義]
        /// <summary>
        /// モジュール番号(DBテーブル：reagent列名)
        /// </summary>
        public const String STRING_MODULENO = "moduleNo";
        /// <summary>
        /// 試薬コード(DBテーブル：reagent列名)
        /// </summary>
        public const String STRING_REAGENTCODE = "reagentCode";
        /// <summary>
        /// 試薬種別(DBテーブル：reagent列名)
        /// </summary>
        public const String STRING_REAGENTKIND = "reagentKind";
        /// <summary>
        /// 設置ポート番号(DBテーブル：reagent列名)
        /// </summary>
        public const String STRING_REAGENTPORTNO = "reagentPortNo";
        /// <summary>
        /// 試薬ロット番号(DBテーブル：reagent列名)
        /// </summary>
        public const String STRING_REAGENTLOTNO = "reagentLotNo";
        /// <summary>
        /// シリアル番号(DBテーブル：reagent列名)
        /// </summary>
        public const String STRING_REAGENTSERIALNO = "reagentSerialNo";
        /// <summary>
        /// 有効期限(DBテーブル：reagent列名)
        /// </summary>
        public const String STRING_EXPIRATIONDATE = "expirationDate";
        /// <summary>
        /// 使用期限(DBテーブル：reagent列名)
        /// </summary>
        public const String STRING_STABILITYDATE = "stabilityDate";
        /// <summary>
        /// 残量(DBテーブル：reagent列名)
        /// </summary>
        public const String STRING_REAGENTREMAIN = "reagentRemain";
        /// <summary>
        /// 使用状態(DBテーブル：reagent列名)
        /// </summary>
        public const String STRING_ISUSE = "isUse";
        /// <summary>
        /// 容量(DBテーブル：reagent列名)
        /// </summary>
        public const String STRING_BOTTOLECAPACITY = "bottoleCapacity";
        /// <summary>
        /// メーカーコード(DBテーブル：reagent列名)
        /// </summary>
        public const String STRING_MAKERCODE = "makerCode";
        /// <summary>
        /// 試薬種(DBテーブル：reagent列名)
        /// </summary>
        public const String STRING_REAGENTTYPE = "reagentType";
        /// <summary>
        /// 試薬種詳細(DBテーブル：reagent列名)
        /// </summary>
        public const String STRING_REAGENTTYPEDETAIL = "reagentTypeDetail";

        #endregion

        #region [プロパティ]

        /// <summary>
        /// データテーブル取得SQL
        /// </summary>
        protected override String baseTableSelectSql
        {
            get
            {
                return "SELECT * FROM dbo.reagent";
            }
        }

        #endregion

        #region [インスタンス変数]

        /// <summary>
        /// 更新時刻
        /// </summary>
        /// <remarks>
        /// 複数モジュールを接続した時、全モジュールを処理中に時間が経過したせいで反映してくれない場合があるので、モジュール毎に時間を管理する
        /// </remarks>
        private DateTime[] timeStamp;

        #endregion

        #region [コンストラクタ]
        public ReagentDB()
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
        public void GetReagentRemain(ref IRemainAmountInfoSet remainInfo, Int32 moduleNo)
        {
            // 残量情報をDBデータから設定
            List<ReagentData> list = this.GetData(moduleId: moduleNo);

            foreach (ReagentKind reagentKind in Enum.GetValues(typeof(ReagentKind)).OfType<ReagentKind>())
            {
                var kindReagentDataList = list.Where((data) => data.ReagentKind == (Int32)reagentKind);
                foreach (ReagentData data in kindReagentDataList)
                {
                    switch (reagentKind)
                    {
                        case ReagentKind.Reagent:               // 試薬
                            remainInfo.ReagentRemainTable[data.PortNo.Value - 1].ReagType = data.ReagentType;
                            remainInfo.ReagentRemainTable[data.PortNo.Value - 1].ReagTypeDetail = (ReagentTypeDetail)data.ReagentTypeDetail;
                            remainInfo.ReagentRemainTable[data.PortNo.Value - 1].ReagCode = data.ReagentCode ?? 0;
                            remainInfo.ReagentRemainTable[data.PortNo.Value - 1].Capacity = data.Capacity;
                            remainInfo.ReagentRemainTable[data.PortNo.Value - 1].MakerCode = data.MakerCode;
                            remainInfo.ReagentRemainTable[data.PortNo.Value - 1].RemainingAmount.Remain = data.Remain ?? 0;
                            remainInfo.ReagentRemainTable[data.PortNo.Value - 1].RemainingAmount.LotNumber = data.LotNo;
                            remainInfo.ReagentRemainTable[data.PortNo.Value - 1].RemainingAmount.SerialNumber = data.SerialNo ?? 0;
                            remainInfo.ReagentRemainTable[data.PortNo.Value - 1].RemainingAmount.TermOfUse = data.ExpirationDate ?? DateTime.MinValue;
                            remainInfo.ReagentRemainTable[data.PortNo.Value - 1].RemainingAmount.Remain = data.Remain ?? 0;
                            break;
                        case ReagentKind.Pretrigger:            // プレトリガ
                            if (data.IsUse.HasValue && data.IsUse.Value)
                            {
                                remainInfo.PreTriggerRemainTable.ActNo = data.PortNo.Value;
                            }
                            remainInfo.PreTriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].Remain = data.Remain ?? 0;
                            remainInfo.PreTriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].LotNumber = data.LotNo;
                            remainInfo.PreTriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].SerialNumber = data.SerialNo ?? 0;
                            remainInfo.PreTriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].TermOfUse = data.ExpirationDate ?? DateTime.MinValue;
                            break;
                        case ReagentKind.Trigger:               // トリガ
                            if (data.IsUse.HasValue && data.IsUse.Value)
                            {
                                remainInfo.TriggerRemainTable.ActNo = data.PortNo.Value;
                            }
                            remainInfo.TriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].Remain = data.Remain ?? 0;
                            remainInfo.TriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].LotNumber = data.LotNo;
                            remainInfo.TriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].SerialNumber = data.SerialNo ?? 0;
                            remainInfo.TriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].TermOfUse = data.ExpirationDate ?? DateTime.MinValue;
                            break;
                        case ReagentKind.Diluent:               // 希釈液
                            remainInfo.DilutionRemainTable.RemainingAmount.Remain = data.Remain ?? 0;
                            remainInfo.DilutionRemainTable.RemainingAmount.LotNumber = data.LotNo;
                            remainInfo.DilutionRemainTable.RemainingAmount.SerialNumber = data.SerialNo ?? 0;
                            remainInfo.DilutionRemainTable.RemainingAmount.TermOfUse = data.ExpirationDate ?? DateTime.MinValue;
                            break;
                        case ReagentKind.SamplingTip:          // サンプル分注チップ
                            if (data.IsUse.HasValue && data.IsUse.Value)
                            {
                                remainInfo.SampleTipRemainTable.ActNo = data.PortNo.Value;
                            }
                            remainInfo.SampleTipRemainTable.tipRemainTable[data.PortNo.Value - 1] = data.Remain ?? 0;
                            break;
                        case ReagentKind.Cell:                  // 反応容器
                            if (data.IsUse.HasValue && data.IsUse.Value)
                            {
                                remainInfo.CellRemainTable.ActNo = data.PortNo.Value;
                            }
                            remainInfo.CellRemainTable.reactContainerRemainTable[data.PortNo.Value - 1] = data.Remain ?? 0;
                            break;
                        case ReagentKind.WasteBuffer:           // 廃液バッファ
                            remainInfo.IsFullWasteBuffer = ((data.Remain ?? (int)WasteStatus.Full) == (int)WasteStatus.Full);       //nullの場合はFull扱いとする
                            break;
                        case ReagentKind.WasteBox:              // 廃棄ボックス
                            if (!data.IsUse.HasValue)
                            {
                                data.IsUse = false;
                            }
                            remainInfo.ExistWasteBox = data.IsUse.Value;

                            // Remainへは、セット時に有無フラグと一体化された値となっており、有の場合はWasteBoxStatusに対して1加算されている。
                            // 未設置の場合は0となっている。1以上に対しては1減算、それ以外へは0扱いとすることでWasteBoxStatus型の値となる。
                            Int32 conditionValue = (data.Remain ?? 0) > 0 ? data.Remain.Value - 1 : 0;
                            remainInfo.WasteBoxCondition = (WasteBoxStatus)conditionValue;
                            break;
                        case ReagentKind.WashSolutionBuffer:      // 洗浄液タンク
                            remainInfo.WashContainerRemain = data.Remain ?? 0;
                            break;
                        default:
                            continue;
                    }
                }
            }
        }

        /// <summary>
        /// 残量情報設定
        /// </summary>
        /// <remarks>
        /// 残量情報をDBに設定します。
        /// </remarks>
        /// <param name="remainInfo">残量情報</param>
        public void SetReagentRemain(IRemainAmountInfoSet remainInfo, Int32 moduleId)
        {
            // 古い時刻のデータで更新しないようにする。
            if (remainInfo.TimeStamp >= this.timeStamp[moduleId])
            {
                List<ReagentData> list = this.GetData(moduleId: moduleId);
                foreach (ReagentKind reagentKind in Enum.GetValues(typeof(ReagentKind)).OfType<ReagentKind>())
                {
                    var kindReagentDataList = list.Where((data) => data.ReagentKind == (Int32)reagentKind);
                    foreach (ReagentData data in kindReagentDataList)
                    {
                        switch (reagentKind)
                        {
                            case ReagentKind.Reagent:               // 試薬
                                Int32 index = data.PortNo.Value - 1;
                                data.ReagentType = remainInfo.ReagentRemainTable[index].ReagType;
                                data.ReagentTypeDetail = (Int32)remainInfo.ReagentRemainTable[index].ReagTypeDetail;
                                data.ReagentCode = remainInfo.ReagentRemainTable[index].ReagCode;
                                data.MakerCode = remainInfo.ReagentRemainTable[index].MakerCode;
                                data.Capacity = remainInfo.ReagentRemainTable[index].Capacity;
                                data.LotNo = remainInfo.ReagentRemainTable[index].RemainingAmount.LotNumber;
                                data.SerialNo = remainInfo.ReagentRemainTable[index].RemainingAmount.SerialNumber;
                                data.ExpirationDate = remainInfo.ReagentRemainTable[index].RemainingAmount.TermOfUse;
                                data.ExpirationDate = (data.ExpirationDate == DateTime.MinValue) ? null : data.ExpirationDate;
                                data.Remain = remainInfo.ReagentRemainTable[index].RemainingAmount.Remain;
                                data.Capacity = remainInfo.ReagentRemainTable[index].Capacity;
                                break;
                            case ReagentKind.Pretrigger:            // プレトリガ
                                data.IsUse = data.PortNo == remainInfo.PreTriggerRemainTable.ActNo;
                                data.LotNo = remainInfo.PreTriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].LotNumber;
                                data.SerialNo = remainInfo.PreTriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].SerialNumber;
                                data.ExpirationDate = remainInfo.PreTriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].TermOfUse;
                                data.ExpirationDate = data.ExpirationDate == DateTime.MinValue ? null : data.ExpirationDate;
                                data.Remain = remainInfo.PreTriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].Remain;
                                break;
                            case ReagentKind.Trigger:               // トリガ
                                data.IsUse = data.PortNo == remainInfo.TriggerRemainTable.ActNo;
                                data.LotNo = remainInfo.TriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].LotNumber;
                                data.SerialNo = remainInfo.TriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].SerialNumber;
                                data.ExpirationDate = remainInfo.TriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].TermOfUse;
                                data.ExpirationDate = data.ExpirationDate == DateTime.MinValue ? null : data.ExpirationDate;
                                data.Remain = remainInfo.TriggerRemainTable.RemainingAmount[data.PortNo.Value - 1].Remain;
                                break;
                            case ReagentKind.Diluent:               // 希釈液
                                data.LotNo = remainInfo.DilutionRemainTable.RemainingAmount.LotNumber;
                                data.SerialNo = remainInfo.DilutionRemainTable.RemainingAmount.SerialNumber;
                                data.ExpirationDate = remainInfo.DilutionRemainTable.RemainingAmount.TermOfUse;
                                data.ExpirationDate = data.ExpirationDate == DateTime.MinValue ? null : data.ExpirationDate;
                                data.Remain = remainInfo.DilutionRemainTable.RemainingAmount.Remain;
                                break;
                            case ReagentKind.SamplingTip:          // サンプル分注チップ
                                data.IsUse = data.PortNo == remainInfo.SampleTipRemainTable.ActNo;
                                data.Remain = remainInfo.SampleTipRemainTable.tipRemainTable[data.PortNo.Value - 1];
                                break;
                            case ReagentKind.Cell:                  // 反応容器
                                data.IsUse = data.PortNo == remainInfo.CellRemainTable.ActNo;
                                data.Remain = remainInfo.CellRemainTable.reactContainerRemainTable[data.PortNo.Value - 1];
                                break;
                            case ReagentKind.WasteBuffer:           // 廃液バッファ
                                data.Remain = 1;
                                data.Remain += Convert.ToInt32(remainInfo.IsFullWasteBuffer);   // 0=バッファ無し 1=バッファ有り、あき有り 2=バッファ有り、満杯

                                break;
                            case ReagentKind.WasteBox:              // 廃棄ボックス
                                data.IsUse = remainInfo.ExistWasteBox;
                                data.Remain = 0; // 0=無し 1=有り（空) 2=有り（警告）　3=有り（満杯）
                                if (remainInfo.ExistWasteBox)
                                {
                                    data.Remain = 1;
                                    data.Remain += Convert.ToInt32(remainInfo.WasteBoxCondition);
                                }
                                break;
                            case ReagentKind.WashSolutionBuffer:      // 洗浄液タンク
                                data.Remain = remainInfo.WashContainerRemain;
                                break;
                        }
                    }
                }

                this.SetData(list);

                this.timeStamp[moduleId] = remainInfo.TimeStamp;
            }
        }

        /// <summary>
        /// 残量情報取得（ラック搬送）
        /// </summary>
        /// <remarks>
        /// ラック搬送の残量情報をDBから取得します。
        /// </remarks>
        /// <param name="remainInfo">残量情報</param>
        public void GetRackReagentRemain(ref IRackRemainAmountInfoSet remainInfo)
        {
            // 残量情報をDBデータから設定
            List<ReagentData> list = this.GetData(moduleId: (int)RackModuleIndex.RackTransfer);

            // 同一試薬コード・種別を持つデータのカウンタ
            Dictionary<Tuple<Int32, Int32>, Int32> groupedPortCounter = new Dictionary<Tuple<Int32, Int32>, Int32>();

            foreach (ReagentKind reagentKind in Enum.GetValues(typeof(ReagentKind)).OfType<ReagentKind>())
            {
                var kindReagentDataList = list.Where((data) => data.ReagentKind == (Int32)reagentKind);
                foreach (ReagentData data in kindReagentDataList)
                {
                    switch (reagentKind)
                    {
                        case ReagentKind.WasteTank:             // 廃液タンク
                            if (!data.IsUse.HasValue)
                            {
                                data.IsUse = false;
                            }
                            remainInfo.ExistWasteTank = data.IsUse.Value;
                            remainInfo.IsFullWasteTank = Convert.ToBoolean(data.Remain);
                            break;
                        default:
                            continue;
                    }
                }
            }
        }

        /// <summary>
        /// 残量情報設定
        /// </summary>
        /// <remarks>
        /// 残量情報をDBに設定します。
        /// </remarks>
        /// <param name="remainInfo">残量情報</param>
        public void SetRackReagentRemain(IRackRemainAmountInfoSet remainInfo)
        {
            // 古い時刻のデータで更新しないようにする。
            if (remainInfo.TimeStamp >= this.timeStamp[(Int32)RackModuleIndex.RackTransfer])
            {
                List<ReagentData> list = this.GetData(moduleId: (int)RackModuleIndex.RackTransfer);
                foreach (ReagentKind reagentKind in Enum.GetValues(typeof(ReagentKind)).OfType<ReagentKind>())
                {
                    var kindReagentDataList = list.Where((data) => data.ReagentKind == (Int32)reagentKind);
                    foreach (ReagentData data in kindReagentDataList)
                    {
                        switch (reagentKind)
                        {
                            case ReagentKind.WasteTank:                                     // 廃液タンク
                                data.IsUse = remainInfo.ExistWasteTank;                     // 0:無　1:有
                                data.Remain = Convert.ToInt32(remainInfo.IsFullWasteTank);  // 0:空　1:満杯

                                break;
                        }
                    }
                }

                this.SetData(list);

                this.timeStamp[(Int32)RackModuleIndex.RackTransfer] = remainInfo.TimeStamp;
            }
        }

        /// <summary>
        /// 試薬情報テーブルの取得
        /// </summary>
        /// <remarks>
        /// 試薬種別を条件に取得した試薬情報テーブルを返します。
        /// </remarks>
        /// <param name="kind">試薬種別</param>
        /// <param name="moduleId">モジュールId（未指定：モジュール１～４全て、0：ラック、1～4：モジュール１～４のいづれか１つ</param>
        /// <returns></returns>
        public List<ReagentData> GetData(ReagentKind? kind = null, int moduleId = CarisXConst.ALL_MODULEID)
        {
            List<ReagentData> result = new List<ReagentData>();

            if (this.DataTable != null)
            {
                try
                {
                    //対象となるモジュールNoの値を設定する
                    int[] inModuleNo = new int[] { };
                    if (moduleId == CarisXConst.ALL_MODULEID)
                    {
                        //モジュール１～４を対象とする場合
                        //接続されているモジュールだけ対象とする
                        inModuleNo = Singleton<SystemStatus>.Instance.GetConnectedModuleId().ToArray();
                    }
                    else
                    {
                        //ラックまたは特定のモジュールを対象とする場合
                        inModuleNo = new int[] { moduleId };
                    }

                    if (kind.HasValue)
                    {
                        result = this.DataTable.Copy().AsEnumerable().Select(row =>
                           new ReagentData(row)).Where(data =>
                           data.ReagentKind == (int)kind && (inModuleNo.Contains(data.ModuleNo))).ToList();
                    }
                    else
                    {
                        result = this.DataTable.Copy().AsEnumerable().Select(row =>
                            new ReagentData(row)).Where(data => (inModuleNo.Contains(data.ModuleNo))).ToList();
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
        /// 試薬ロット番号の試薬コード指定取得
        /// </summary>
        /// <remarks>
        /// 試薬ロット番号を条件に取得した試薬コードを返します。
        /// </remarks>
        /// <returns>指定の試薬コードの全試薬ロット番号</returns>
        public String[] GetReagentLotNo(Int32 reagentCode, int moduleId = CarisXConst.ALL_MODULEID)
        {
            IEnumerable<String> result = new String[] { };

            if (this.DataTable != null)
            {
                try
                {
                    //対象となるモジュールNoの値を設定する
                    int[] tmpModuleNo = new int[] { };
                    if (moduleId == CarisXConst.ALL_MODULEID)
                    {
                        //モジュール１～４を対象とする場合
                        //接続されているモジュールだけ対象とする
                        tmpModuleNo = Singleton<SystemStatus>.Instance.GetConnectedModuleId().ToArray();
                    }
                    else
                    {
                        //ラックまたは特定のモジュールを対象とする場合
                        tmpModuleNo = new int[] { moduleId };
                    }

                    List<int> inModuleNo = new List<int> { };
                    foreach (var moduleNo in tmpModuleNo)
                    {
                        // モジュールIndex取得（ラック搬送の場合、スレーブ1(=0)になる）
                        Int32 moduleIndex = CarisXSubFunction.ModuleIDToModuleIndex(moduleNo);
                        bool moduleEnabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.GetUseEmergencyMode(moduleIndex);
                        MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(reagentCode);

                        // モジュールの急診使用無しの場合かつ、急診ありの分析項目はレスポンスデータに含めない
                        if ((moduleEnabledFlag == false) && (protocol.UseEmergencyMode == true))
                        {
                            continue;
                        }

                        inModuleNo.Add(moduleNo);
                    }

                    result = (from v in this.DataTable.AsEnumerable()
                              let data = new ReagentData(v)
                              where data.ReagentCode == reagentCode 
                              && data.ReagentKind == (Int32)ReagentKind.Reagent 
                              && data.Remain.HasValue 
                              && data.Remain > 0 
                              && (inModuleNo.Contains(data.ModuleNo))
                              select data.LotNo).Distinct();
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    //Singleton<LogManager>.Instance.Error( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// 試薬ロット番号の試薬コード指定取得(現ロット)
        /// </summary>
        /// <remarks>
        ///　試薬コードを条件に取得した試薬ロット番号を返します。
        /// </remarks>
        /// <returns>指定の試薬コードの全試薬ロット番号</returns>
        public String GetNowReagentLotNo(Int32 reagentCode, Boolean allowZeroRemain = false, int moduleId = CarisXConst.ALL_MODULEID)
        {
            String reagentLotNumber = String.Empty;

            // 現ロット取得処理
            // 現ロットは最小のロット番号
            if (this.DataTable != null)
            {
                try
                {
                    //対象となるモジュールNoの値を設定する
                    int[] tmpModuleNo = new int[] { };
                    if (moduleId == CarisXConst.ALL_MODULEID)
                    {
                        //モジュール１～４を対象とする場合
                        //接続されているモジュールだけ対象とする
                        tmpModuleNo = Singleton<SystemStatus>.Instance.GetConnectedModuleId().ToArray();
                    }
                    else
                    {
                        //ラックまたは特定のモジュールを対象とする場合
                        tmpModuleNo = new int[] { moduleId };
                    }

                    List<int> inModuleNo = new List<int> { };
                    foreach (var moduleNo in tmpModuleNo)
                    {
                        // モジュールIndex取得（ラック搬送の場合、スレーブ1(=0)になる）
                        Int32 moduleIndex = CarisXSubFunction.ModuleIDToModuleIndex(moduleNo);
                        bool moduleEnabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.GetUseEmergencyMode(moduleIndex);
                        MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(reagentCode);

                        // モジュールの急診使用無しの場合かつ、急診ありの分析項目はレスポンスデータに含めない
                        if ((moduleEnabledFlag == false) && (protocol.UseEmergencyMode == true))
                        {
                            continue;
                        }

                        inModuleNo.Add(moduleNo);
                    }

                    EnumerableRowCollection<ReagentData> result = null;
                    if (allowZeroRemain)
                    {
                        result = from v in this.DataTable.AsEnumerable()
                                 let data = new ReagentData(v)
                                 where data.ReagentCode == reagentCode
                                 && data.ReagentKind == (Int32)ReagentKind.Reagent
                                 && data.Remain.HasValue
                                 && (inModuleNo.Contains(data.ModuleNo))
                                 orderby data.LotNo ascending
                                 select data;
                    }
                    else
                    {
                        result = from v in this.DataTable.AsEnumerable()
                                 let data = new ReagentData(v)
                                 where data.ReagentCode == reagentCode
                                 && data.ReagentKind == (Int32)ReagentKind.Reagent
                                 && data.Remain.HasValue
                                 && data.Remain > 0
                                 && (inModuleNo.Contains(data.ModuleNo))
                                 orderby data.LotNo ascending
                                 select data;
                    }

                    if (result.Count() != 0)
                    {
                        // 設置日が最も早い試薬のロット番号を渡す
                        reagentLotNumber = Singleton<ReagentHistoryDB>.Instance.GetLotNo(result.ToList());
                    }
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                           CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }

            return reagentLotNumber;
        }

        /// <summary>
        /// 優先ロット番号取得
        /// </summary>
        /// <param name="reagentCode"></param>
        /// <param name="lotNo"></param>
        /// <returns></returns>
        public String GetLotSpecificationNo(Int32 reagentCode, String lotNo, Int32 moduleId)
        {
            String reagentLotNumber = String.Empty;

            //対象となるモジュールNoの値を設定する
            int[] tmpModuleNo = new int[] { };
            if (moduleId == CarisXConst.ALL_MODULEID)
            {
                //モジュール１～４を対象とする場合
                //接続されているモジュールだけ対象とする
                tmpModuleNo = Singleton<SystemStatus>.Instance.GetConnectedModuleId().ToArray();
            }
            else
            {
                //ラックまたは特定のモジュールを対象とする場合
                tmpModuleNo = new int[] { moduleId };
            }

            List<int> inModuleNo = new List<int> { };
            foreach (var moduleNo in tmpModuleNo)
            {
                // モジュールIndex取得（ラック搬送の場合、スレーブ1(=0)になる）
                Int32 moduleIndex = CarisXSubFunction.ModuleIDToModuleIndex(moduleNo);
                bool moduleEnabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.GetUseEmergencyMode(moduleIndex);
                MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(reagentCode);

                // モジュールの急診使用無しの場合かつ、急診ありの分析項目はレスポンスデータに含めない
                if ((moduleEnabledFlag == false) && (protocol.UseEmergencyMode == true))
                {
                    continue;
                }

                inModuleNo.Add(moduleNo);
            }

            EnumerableRowCollection<ReagentData> lotSpecificationResult = null;
            lotSpecificationResult = from v in this.DataTable.AsEnumerable()
                                     let controlReagentdata = new ReagentData(v)
                                     where controlReagentdata.ReagentCode == reagentCode
                                     && controlReagentdata.ReagentKind == (Int32)ReagentKind.Reagent
                                     && controlReagentdata.Remain.HasValue
                                     && controlReagentdata.Remain > 0
                                     && controlReagentdata.LotNo == lotNo
                                     && (inModuleNo.Contains(controlReagentdata.ModuleNo))
                                     select controlReagentdata;
            if (lotSpecificationResult.Count() != 0)
            {
                reagentLotNumber = lotSpecificationResult.First().LotNo;
            }
            return reagentLotNumber;
        }

        /// <summary>
        /// 試薬種別と設置ポート番号から残量、ロット番号、シリアル番号を取得
        /// </summary>
        /// <remarks>
        /// 試薬種別と設置ポート番号から取得した残量、ロット番号、シリアル番号を返します。
        /// </remarks>
        /// <param name="reagentKind">試薬種別</param>
        /// <param name="portNo">設置ポート番号</param>
        /// <param name="ref remain">残量</param>
        /// <param name="ref lotNumber">ロット番号</param>
        /// <param name="ref serialNumber">シリアル番号</param>
        /// <param name="ref termOfUse">有効期限</param>
        /// <returns>成功・失敗</returns>
        public Boolean GetReagentRemainLotSerial(
            ReagentKind reagentKind, Int32 portNo, ref Int32 remain, ref String lotNumber, ref Int32 serialNumber, ref DateTime termOfUse)
        {
            Boolean bRet = false;
            List<ReagentData> list = this.GetData();
            var kindReagentDataList = list.Where((data) => (data.ReagentKind == (Int32)reagentKind) && (data.PortNo == portNo));
            remain = 0;
            lotNumber = String.Empty;
            serialNumber = 0;
            foreach (ReagentData data in kindReagentDataList)
            {
                remain = data.Remain ?? 0;
                lotNumber = data.LotNo;
                serialNumber = data.SerialNo ?? 0;
                termOfUse = data.ExpirationDate ?? DateTime.MinValue;
                bRet = true;//成功
                break; // 1行だけのはずなのでbreak;
            }

            return bRet;
        }

        /// <summary>
        /// 指定の試薬種別と設置ポート番号の場所に残量、ロット番号、シリアル番号を設定
        /// </summary>
        /// <remarks>
        /// 指定の試薬種別と設置ポート番号の場所に残量、ロット番号、シリアル番号を設定します。
        /// </remarks>
        /// <param name="reagentKind">試薬種別</param>
        /// <param name="portNo">設置ポート番号</param>
        /// <param name="remain">残量</param>
        /// <param name="lotNumber">ロット番号</param>
        /// <param name="serialNumber">シリアル番号</param>
        /// <returns>成功・失敗</returns>
        public Boolean SetReagentRemainLotSerial(
            ReagentKind reagentKind, Int32 portNo, Int32 remain, String lotNumber, Int32 serialNumber)
        {
            Boolean bRet = false;
            List<ReagentData> list = this.GetData();
            var kindReagentDataList = list.Where((data) => (data.ReagentKind == (Int32)reagentKind) && (data.PortNo == portNo));
            foreach (ReagentData data in kindReagentDataList)
            {
                data.Remain = remain;
                data.LotNo = lotNumber;
                data.SerialNo = serialNumber;
                this.SetData(list);
                bRet = true;//成功
                break; // 1行だけのはずなのでbreak;
            }

            return bRet;
        }

        /// <summary>
        /// 試薬ロット番号の試薬コード指定取得
        /// </summary>
        /// <remarks>
        /// 試薬ロット番号を条件に取得した試薬コードを返します。
        /// </remarks>
        /// <returns>指定の試薬コードの全試薬ロット番号</returns>
        public Int32 GetTargetModuleNo(Int32 reagentCode, Int32 moduleNo)
        {
            Int32 result = -1;

            if (this.DataTable != null)
            {
                try
                {
                    //試薬情報から、同一試薬、モジュールのデータがあるか確認する
                    var reagentList = this.DataTable.AsEnumerable().Select(dt => new ReagentData(dt))
                        .Where(data => data.ReagentCode == reagentCode && data.ModuleNo == moduleNo && data.ReagentKind == (Int32)ReagentKind.Reagent && data.Remain.HasValue && data.Remain > 0)
                        .Select(data => data.ModuleNo).ToList();
                    if (reagentList.Count() == 0)
                    {

                        //対象となるモジュールNoの値を設定する
                        int[] enableAllModuleNo = Singleton<SystemStatus>.Instance.GetConnectedModuleId().ToArray();

                        //試薬情報から、同一試薬が他のモジュールにあるか確認する
                        reagentList = this.DataTable.AsEnumerable().Select(dt => new ReagentData(dt))
                            .Where(data => data.ReagentCode == reagentCode 
                            && data.ReagentKind == (Int32)ReagentKind.Reagent 
                            && data.Remain.HasValue 
                            && data.Remain > 0
                            && (enableAllModuleNo.Contains(data.ModuleNo))
                            )
                            .OrderBy(data => data.ModuleNo).Select(data => data.ModuleNo).ToList();
                    }

                    if (reagentList.Count() != 0)
                    {
                        //データがあった場合、取得したモジュール番号を設定する
                        result = reagentList[0];
                    }
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    //Singleton<LogManager>.Instance.Error( ex.Message );
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog
                        , Singleton<Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }

            return result;
        }

        /// <summary>
        /// 試薬情報の取得（ポート番号、試薬コード指定）
        /// </summary>
        /// <remarks>
        /// 試薬ロット番号を条件に取得した試薬コードを返します。
        /// </remarks>
        /// <param name="portNo">ポート番号（１～２０で指定）</param>
        /// <param name="reagentCode">試薬コード</param>
        /// <param name="reagTypeDetail">試薬種詳細</param>
        /// <param name="moduleNo">モジュールID（１～４で指定）</param>
        /// <returns>指定の試薬コードの全試薬ロット番号</returns>
        public ReagentData GetReagentData(Int32 portNo, Int32 reagentCode, ReagentTypeDetail reagTypeDetail, Int32 moduleNo)
        {
            ReagentData result = null;

            Int32 reagPortNo = 0;
            switch (reagTypeDetail)
            {
                case ReagentTypeDetail.R1:
                case ReagentTypeDetail.T1:
                    reagPortNo = portNo * 3 - 2;
                    break;
                case ReagentTypeDetail.R2:
                case ReagentTypeDetail.T2:
                    reagPortNo = portNo * 3 - 1;
                    break;
                case ReagentTypeDetail.M:
                    reagPortNo = portNo * 3;
                    break;
            }

            if (this.DataTable != null)
            {
                try
                {
                    //試薬情報から、同一試薬、モジュールのデータがあるか確認する
                    var reagentList = this.DataTable.AsEnumerable().Select(dt => new ReagentData(dt))
                        .Where(data => data.ReagentCode == reagentCode
                            && data.ReagentTypeDetail == (Int32)reagTypeDetail
                            && data.ModuleNo == moduleNo
                            && data.PortNo == reagPortNo
                            && data.ReagentKind == (Int32)ReagentKind.Reagent);
                    if (reagentList.Count() != 0)
                    {
                        //データがあった場合、取得したモジュール番号を設定する
                        result = reagentList.FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    // DB内部に不正データ
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog
                        , Singleton<Utility.CarisXUserLevelManager>.Instance.NowUserID, CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                }
            }

            return result;
        }

        /// <summary>
        /// 試薬情報データの設定
        /// </summary>
        /// <remarks>
        /// 試薬情報データの同期を行います。
        /// </remarks>
        /// <param name="list">変更、削除操作済みデータ</param>
        public void SetData(List<ReagentData> list)
        {
            list.SyncDataListToDataTable(this.DataTable);
        }

        /// <summary>
        /// 試薬テーブル取得
        /// </summary>
        /// <remarks>
        /// 試薬情報をDBから読込みます。
        /// </remarks>
        public override void LoadDB()
        {
            this.fillBaseTable();
        }

        /// <summary>
        /// 試薬情報テーブル書込み
        /// </summary>
        /// <remarks>
        /// 試薬情報をDBに書き込みます。
        /// </remarks>
        public void CommitData()
        {
            this.updateBaseTable();
        }
        #endregion
    }
}
