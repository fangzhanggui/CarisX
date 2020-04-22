using System;
using System.Collections.Generic;
using System.Linq;
using Oelco.CarisX.Const;
using Oelco.CarisX.Parameter;
using System.Text;
using Oelco.Common.Parameter;
using Oelco.Common.Utility;
using Oelco.CarisX.Parameter.MaintenanceJournalCodeData;
using Infragistics.Win;
using Oelco.CarisX.DB;
using Oelco.CarisX.GUI;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Common;
using System.Reflection;
using Infragistics.Win.UltraWinDataSource;

namespace Oelco.CarisX.Common
{
    /// <summary>
    /// メンテナンス日誌一覧画面情報表示データクラス
    /// </summary>
    /// <remarks>
    /// ユーザー、サービスマンのメンテナンス日誌一覧画面の情報表示用データクラスです。
    /// </remarks>
    public class MaintenanceJournalListData
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="No">行番号</param>
        /// <param name="Kind">種別</param>
        /// <param name="module">接続台数分のモジュール</param>
        /// <param name="checkItem">メンテナンス日誌項目名</param>
        /// <param name="module1">モジュール1</param>
        /// <param name="maintenanceJournalNo">メンテナンス日誌番号</param>
        /// <param name="unit">ユニット番号</param>
        /// 
        public MaintenanceJournalListData(Int32 No, String checkItem, String kind, List<bool> module, Int32? maintenanceJournalNo, String unit)
        {
            this.No = No;
            this.checkItem = checkItem;
            this.kind = kind;
            for (int i = 0; i < module.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        this.module1 = module[0] == true ? Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013 : Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_014;
                        break;
                    case 1:
                        this.module2 = module[1] == true ? Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013 : Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_014;
                        break;
                    case 2:
                        this.module3 = module[2] == true ? Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013 : Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_014;
                        break;
                    case 3:
                        this.module4 = module[3] == true ? Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013 : Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_014;
                        break;
                    default:
                        break;
                }
            }
            this.maintenanceJournalNo = maintenanceJournalNo.ToString();
            this.unitNo = unit;
        }

        #endregion

        #region [Accessor]

        /// <summary>
        /// 行番号
        /// </summary>
        public Int32 No
        {
            get;
            set;
        }

        /// <summary>
        /// メンテナンス日誌項目名
        /// </summary>
        public String checkItem
        {
            get;
            set;
        }

        /// <summary>
        /// メンテナンス日誌項目種別
        /// </summary>
        public String kind
        {
            get;
            set;
        }

        /// <summary>
        /// モジュール1
        /// </summary>
        public String module1
        {
            get;
            set;
        }

        /// <summary>
        /// モジュール2
        /// </summary>
        public String module2
        {
            get;
            set;
        }

        /// <summary>
        /// モジュール3
        /// </summary>
        public String module3
        {
            get;
            set;
        }

        /// <summary>
        /// モジュール4
        /// </summary>
        public String module4
        {

            get;
            set;
        }

        /// <summary>
        /// メンテナンス日誌番号
        /// </summary>
        public String maintenanceJournalNo
        {
            get;
            set;
        }

        /// <summary>
        /// メンテナンス日誌ユニット番号
        /// </summary>
        public String unitNo
        {
            get;
            set;
        }

        #endregion
    }


    /// <summary>
    /// メンテナンス日誌情報管理
    /// </summary>
    public class MaintenanceJournalInfoManager
    {
        #region [定数定義]

        /// <summary>
        /// 行番号列キー
        /// </summary>
        private const String STRING_CHECKNO = "No";
        /// <summary>
        /// メンテナンス日誌チェック項目名列キー
        /// </summary>
        private const String STRING_CHECKITEM = "CheckItem";
        /// <summary>
        /// メンテナンス日誌項目種別列キー
        /// </summary>
        private const String STRING_CHECKKIND = "Kind";
        /// <summary>
        /// Module1列キー
        /// </summary>
        private const String STRING_CHECKMODULE1 = "Module1";
        /// <summary>
        /// Module2列キー
        /// </summary>
        private const String STRING_CHECKMODULE2 = "Module2";
        /// <summary>
        /// Module3列キー
        /// </summary>
        private const String STRING_CHECKMODULE3 = "Module3";
        /// <summary>
        /// Module4列キー
        /// </summary>
        private const String STRING_CHECKMODULE4 = "Module4";
        /// <summary>
        /// メンテナンス日誌チェック番号列キー
        /// </summary>
        private const String STRING_MAINTENANCEJOURNALNO = "MaintenanceJournalNo";
        /// <summary>
        /// メンテナンス日誌ユニット番号
        /// </summary>
        private const String STRING_UNITNO = "UnitNo";
        /// <summary>
        /// csv出力項目名　Module番号
        /// </summary>
        private const String STRING_CSV_MODULE_NO = "";
        /// <summary>
        /// csv出力項目名　コード
        /// </summary>
        private const String STRING_CSV_CODE = "Code";
        /// <summary>
        /// csv出力項目名　ユーザー名
        /// </summary>
        private const String STRING_CSV_USER_NAME = "User Name";
        /// <summary>
        /// csv出力項目名　項目名
        /// </summary>
        private const String STRING_CSV_CHECK_ITEM = "Check Item";
        /// <summary>
        /// Daily
        /// </summary>
        private const String STRING_DAILY = "Daily";
        /// <summary>
        /// Daily
        /// </summary>
        private const String STRING_WEEKLY = "Weekly";
        /// <summary>
        /// Daily
        /// </summary>
        private const String STRING_MONTHLY = "Monthly";
        /// <summary>
        /// Daily
        /// </summary>
        private const String STRING_YEARLY = "Yearly";

        /// <summary>
        ///  メンテナンス日誌種別
        /// </summary>
        private MaintenanceJournalType mainteJournalType = MaintenanceJournalType.User;
        /// <summary>
        /// モジュール接続台数
        /// </summary>
        private Int32 ModuleNumConnected = 0;

        /// <summary>
        /// 区切り文字
        /// </summary>
        protected String separator = String.Empty;

        /// <summary>
        /// エンコード
        /// </summary>
        protected Encoding enc;

        #endregion

        #region [クラス変数定義]
        List<Boolean> listDailyCheckItemUser = new List<bool>();
        List<Boolean> listWeeklyCheckItemUser = new List<bool>();
        List<Boolean> listMonthlyCheckItemUser = new List<bool>();
        List<Boolean> listMonthlyCheckItemServiceman = new List<bool>();
        List<Boolean> listYearlyCheckItemServiceman = new List<bool>();
        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// dailyの行
        /// </summary>
        private List<Int32> dailyIndex = new List<Int32>();

        /// <summary>
        /// weeklyの行
        /// </summary>
        private List<Int32> weeklyIndex = new List<Int32>();

        /// <summary>
        /// Monthlyの行
        /// </summary>
        private List<Int32> monthlyIndex = new List<Int32>();

        /// <summary>
        /// Yearlyの行
        /// </summary>
        private List<Int32> yearlyIndex = new List<Int32>();

        /// <summary>
        /// ユーザー用グリッドデータ
        /// </summary>
        List<MaintenanceJournalListData> maintenanceJournalListDatasUser = new List<MaintenanceJournalListData>();

        /// <summary>
        /// サービスマン用グリッドデータ
        /// </summary>
        List<MaintenanceJournalListData> maintenanceJournalListDatasServiceman = new List<MaintenanceJournalListData>();

        #endregion

        #region [プロパティ]

        #endregion

        #region [publicメソッド]

        public void SetMaintenanceJournalType(MaintenanceJournalType mainteJournalType)
        {
            this.mainteJournalType = mainteJournalType;
        }

        public void LoadMaintenanceJournalList(MaintenanceJournalType maintenanceJournalType)
        {
            if (maintenanceJournalType == MaintenanceJournalType.User)
            {
                LoadMaintenanceJournalListData(ref this.maintenanceJournalListDatasUser, MaintenanceJournalType.User);
            }
            else if (maintenanceJournalType == MaintenanceJournalType.Serviceman)
            {
                LoadMaintenanceJournalListData(ref this.maintenanceJournalListDatasServiceman, MaintenanceJournalType.Serviceman);
            }
        }

        /// <summary>
        /// メンテナンス日誌画面開くかチェック
        /// </summary>
        /// <returns></returns>
        public Boolean IsShow()
        {
            try
            {
                // メンテナンス日誌（ユーザー用）のチェックフラグを確認
                CarisXMaintenanceUserParameter AllCheckUserFlag = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param;
                CarisXMaintenanceServicemanParameter AllCheckServicemanFlag = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param;

                List<MaintenanceJournalListData> MaintenanceJournalInfo = Singleton<MaintenanceJournalInfoManager>.Instance.ISshowMaintenaceListData();
                // メッセージファイルの設定
                MaintenanceJournalCodeDataManager messageList = Singleton<ParameterFilePreserve<MaintenanceJournalCodeDataManager>>.Instance.Param;

                // 種別ごとのメッセージリスト
                List<List<MaintenanceJournalCodeData>> codeList = new List<List<MaintenanceJournalCodeData>>();
                codeList = CodeListEachKind();
                int messageCountUser = codeList[0].Count + codeList[1].Count + codeList[2].Count;
                int messageCountServiceman = codeList[3].Count + codeList[4].Count;

                // ユーザー時、パラメータファイル取得失敗
                if (AllCheckUserFlag.SlaveList == null
                && this.mainteJournalType == MaintenanceJournalType.User)
                {
                    // メンテナンス日誌画面のロードに失敗しました。パラメータファイルを確認してください。
                    Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_016));
                    return false;
                }
                // ザービスマン時、パラメータファイル取得失敗
                else if (AllCheckServicemanFlag.SlaveList == null
                    && this.mainteJournalType == MaintenanceJournalType.Serviceman)
                {
                    // メンテナンス日誌画面のロードに失敗しました。パラメータファイルを確認してください。
                    Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_016));
                    return false;
                }
                // メッセージファイル取得失敗
                else if (messageList.CodeDataList.Count == 0)
                {
                    // メンテナンス日誌画面のロードに失敗しました。メッセージファイルを確認してください。
                    Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_017));
                    return false;
                }
                // パラメータファイルとメッセージファイルの整合性が取れていない
                else if (MaintenanceJournalInfo.Count == 0)
                {
                    // メンテナンス日誌画面のロードに失敗しました。メッセージファイルかパラメータファイルを確認してください。
                    Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_018));
                    return false;
                }
                // パラメータファイルとメッセージファイルのデータ件数が合致しない
                else if ((this.mainteJournalType == MaintenanceJournalType.User
                    && messageCountUser != MaintenanceJournalInfo.Count)
                    || (this.mainteJournalType == MaintenanceJournalType.Serviceman
                    && messageCountServiceman != MaintenanceJournalInfo.Count))
                {
                    // メンテナンス日誌画面のロードに失敗しました。メッセージファイルかパラメータファイルを確認してください。
                    Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_018));
                    return false;
                }

                // ファイルを取得できている場合
                if (this.mainteJournalType == MaintenanceJournalType.User)
                {

                    // メンテナンス期限切れのチェック
                    DateTime nowDateTime = DateTime.Now;
                    if ((AllCheckUserFlag.AllFinishDaily.AddDays(1) < nowDateTime)
                      || (AllCheckUserFlag.AllFinishWeekly.AddDays(7) < nowDateTime)
                      || (AllCheckUserFlag.AllFinishMonthly.AddMonths(1) < nowDateTime))
                    {
                        // メンテナンス日誌（ユーザー用）を表示
                        return true;
                    }

                    if (AllCheckUserFlag.AllCheckDaily == false
                            || AllCheckUserFlag.AllCheckWeekly == false
                            || AllCheckUserFlag.AllCheckMonthly == false)
                    {
                        return true;
                    }

                    return false;
                }
                else
                {
                    // メンテナンス期限切れのチェック
                    DateTime nowDateTime = DateTime.Now;
                    if ((AllCheckServicemanFlag.AllFinishMonthly.AddMonths(1) < nowDateTime)
                      || (AllCheckServicemanFlag.AllFinishYearly.AddYears(1) < nowDateTime))
                    {
                        // メンテナンス日誌（サービスマン用）を表示
                        return true;
                    }

                    if (AllCheckServicemanFlag.AllCheckMonthly == false
                            || AllCheckServicemanFlag.AllCheckYearly == false)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                // コンソールに例外出力
                System.Diagnostics.Debug.WriteLine(String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));

                // メンテナンス日誌画面のロードに失敗しました。メッセージファイルかパラメータファイルを確認してください。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_018));
                return false;
            }
        }

        /// <summary>
        /// メンテナンス日誌セレクト画面開くかチェック
        /// </summary>
        /// <returns></returns>
        public Boolean IsShowSelect()
        {
            // メンテナンス日誌（ユーザー用）のチェックフラグを確認
            CarisXMaintenanceUserParameter AllCheckUserFlag = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param;

            // メンテナンス日誌（サービスマン用）のチェックフラグを確認
            CarisXMaintenanceServicemanParameter AllCheckServicemanFlag = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param;

            List<Common.MaintenanceJournalListData> MaintenanceJournalInfo = Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.ISshowMaintenaceListData();
            // メッセージファイルの設定
            Oelco.CarisX.Parameter.MaintenanceJournalCodeData.MaintenanceJournalCodeDataManager messageList = Singleton<ParameterFilePreserve<Oelco.CarisX.Parameter.MaintenanceJournalCodeData.MaintenanceJournalCodeDataManager>>.Instance.Param;

            // 種別ごとのメッセージリスト
            List<List<MaintenanceJournalCodeData>> codeList = new List<List<MaintenanceJournalCodeData>>();
            codeList = CodeListEachKind();
            int messageCountUser = codeList[0].Count + codeList[1].Count + codeList[2].Count;
            int messageCountServiceman = codeList[3].Count + codeList[4].Count;

            // ユーザー時、パラメータファイル取得失敗
            if (AllCheckUserFlag.SlaveList == null
                && this.mainteJournalType == MaintenanceJournalType.User)
            {
                // メンテナンス日誌画面のロードに失敗しました。パラメータファイルを確認してください。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_016));
                return false;
            }
            // メッセージファイル取得失敗
            else if (messageList.CodeDataList.Count == 0)
            {
                // メンテナンス日誌画面のロードに失敗しました。メッセージファイルを確認してください。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_017));
                return false;
            }
            // パラメータファイルとメッセージファイルの整合性が取れていない
            else if (MaintenanceJournalInfo.Count == 0)
            {
                // メンテナンス日誌画面のロードに失敗しました。メッセージファイルかパラメータファイルを確認してください。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_018));
                return false;
            }
            // パラメータファイルとメッセージファイルのデータ件数が合致しない
            else if ((this.mainteJournalType == MaintenanceJournalType.User
                && messageCountUser != MaintenanceJournalInfo.Count)
                || (this.mainteJournalType == MaintenanceJournalType.Serviceman
                && messageCountServiceman != MaintenanceJournalInfo.Count))
            {
                // メンテナンス日誌画面のロードに失敗しました。メッセージファイルかパラメータファイルを確認してください。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_018));
                return false;
            }
            // ファイルを取得できている場合
            else
            {
                // サービスマンの場合、以下条件必要
                if(this.mainteJournalType == MaintenanceJournalType.Serviceman)
                {
                    if (AllCheckServicemanFlag.AllCheckMonthly == true
                      && AllCheckServicemanFlag.AllCheckYearly == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                
                return true;

            }
        }

        /// <summary>
        /// メンテナンス日誌保存するかチェック
        /// </summary>
        /// <returns></returns>
        public Boolean IsSave()
        {
            // メンテナンス日誌（ユーザー用）のチェックフラグを確認
            CarisXMaintenanceUserParameter AllCheckUserFlag = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param;
            CarisXMaintenanceServicemanParameter AllCheckServicemanFlag = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param;

            List<Common.MaintenanceJournalListData> MaintenanceJournalInfo = Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.ISshowMaintenaceListData();
            // メッセージファイルの設定
            Oelco.CarisX.Parameter.MaintenanceJournalCodeData.MaintenanceJournalCodeDataManager messageList = Singleton<ParameterFilePreserve<Oelco.CarisX.Parameter.MaintenanceJournalCodeData.MaintenanceJournalCodeDataManager>>.Instance.Param;

            // 種別ごとのメッセージリスト
            List<List<MaintenanceJournalCodeData>> codeList = new List<List<MaintenanceJournalCodeData>>();
            codeList = CodeListEachKind();
            int messageCountUser = codeList[0].Count + codeList[1].Count + codeList[2].Count;
            int messageCountServiceman = codeList[3].Count + codeList[4].Count;

            // ユーザー時、パラメータファイル取得失敗
            if (AllCheckUserFlag.SlaveList == null
                && this.mainteJournalType == MaintenanceJournalType.User)
            {
                // メンテナンス日誌画面のロードに失敗しました。パラメータファイルを確認してください。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_016));
                return false;
            }
            // ザービスマン時、パラメータファイル取得失敗
            else if (AllCheckServicemanFlag.SlaveList == null
                && this.mainteJournalType == MaintenanceJournalType.Serviceman)
            {
                // メンテナンス日誌画面のロードに失敗しました。パラメータファイルを確認してください。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_016));
                return false;
            }
            // メッセージファイル取得失敗
            else if (messageList.CodeDataList.Count == 0)
            {
                // メンテナンス日誌画面のロードに失敗しました。メッセージファイルを確認してください。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_017));
                return false;
            }
            // パラメータファイルとメッセージファイルの整合性が取れていない
            else if (MaintenanceJournalInfo.Count == 0)
            {
                // メンテナンス日誌画面のロードに失敗しました。メッセージファイルかパラメータファイルを確認してください。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_018));
                return false;
            }
            // パラメータファイルとメッセージファイルのデータ件数が合致しない
            else if ((this.mainteJournalType == MaintenanceJournalType.User
                && messageCountUser != MaintenanceJournalInfo.Count)
                || (this.mainteJournalType == MaintenanceJournalType.Serviceman
                && messageCountServiceman != MaintenanceJournalInfo.Count))
            {
                // メンテナンス日誌画面のロードに失敗しました。メッセージファイルかパラメータファイルを確認してください。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_018));
                return false;
            }

            // ファイルを取得できている場合
            if (this.mainteJournalType == MaintenanceJournalType.User)
            {

                // メンテナンス期限切れのチェック
                DateTime nowDateTime = DateTime.Now;
                if ((AllCheckUserFlag.AllFinishDaily.AddDays(1) < nowDateTime)
                  || (AllCheckUserFlag.AllFinishWeekly.AddDays(7) < nowDateTime)
                  || (AllCheckUserFlag.AllFinishMonthly.AddMonths(1) < nowDateTime))
                {
                    // メンテナンス日誌（ユーザー用）を表示
                    return true;
                }

                if (AllCheckUserFlag.AllCheckDaily == false
                        || AllCheckUserFlag.AllCheckWeekly == false
                        || AllCheckUserFlag.AllCheckMonthly == false)
                {
                    return true;
                }

                return false;
            }
            else
            {
                // メンテナンス期限切れのチェック
                DateTime nowDateTime = DateTime.Now;
                if ((AllCheckServicemanFlag.AllFinishMonthly.AddDays(1) < nowDateTime)
                  || (AllCheckServicemanFlag.AllFinishYearly.AddDays(1) < nowDateTime))
                {
                    // メンテナンス日誌（サービスマン用）を表示
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// メンテナンス日誌データを取得します
        /// </summary>
        /// <returns></returns>
        public List<MaintenanceJournalListData> GetMaintenanceJournalListDatas()
        {
            List<MaintenanceJournalListData> maintenanceJournalDatas = new List<MaintenanceJournalListData>();
            // モジュール接続台数
            ModuleNumConnected = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

            try
            {
                // パラメータファイル初期化チェック
                InitializeCheckParameterFile();

                // 画面情報を更新します
                //// user2回目開く場合
                //if (this.maintenanceJournalListDatasUser.Count > 0
                //    && this.mainteJournalType == MaintenanceJournalType.User)
                //{
                //    maintenanceJournalDatas = this.maintenanceJournalListDatasUser;
                //}
                // serviceman2回目開く場合
                if (this.maintenanceJournalListDatasServiceman.Count > 0
                    && this.mainteJournalType == MaintenanceJournalType.Serviceman)
                {
                    maintenanceJournalDatas = this.maintenanceJournalListDatasServiceman;
                }
                else
                {
                    this.LoadMaintenanceJournalListData(ref maintenanceJournalDatas, this.mainteJournalType);
                }

                return maintenanceJournalDatas;
            }
            catch (Exception ex)
            {
                // 画面のロードに失敗しました。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("Screen loading failed :{0}", ex.TargetSite));
                return maintenanceJournalDatas;
            }
        }

        /// <summary>
        /// 種別ごとのインデックスを設定
        /// </summary>
        /// <param name="maintenanceJournalType">タイプ種別</param>
        public void CreateIndex(MaintenanceJournalType maintenanceJournalType)
        {
            // グリッドのロード
            LoadMaintenanceJournalList(maintenanceJournalType);

            string strCol = string.Empty;
            int iNo = 0;

            // インデックスクリア
            this.dailyIndex.Clear();
            this.weeklyIndex.Clear();
            this.monthlyIndex.Clear();
            this.yearlyIndex.Clear();

            // グリッド作成後に使用します
            if (maintenanceJournalType == MaintenanceJournalType.User)
            {
                MaintenanceJournalListData listRow = this.maintenanceJournalListDatasUser[0];

                for (int i = 1; i <= this.maintenanceJournalListDatasUser.Count; i++)
                {
                    listRow = this.maintenanceJournalListDatasUser[i - 1];
                    strCol = listRow.kind;
                    iNo = listRow.No;

                    switch (strCol)
                    {
                        case STRING_DAILY:
                            this.dailyIndex.Add(iNo);
                            break;
                        case STRING_WEEKLY:
                            this.weeklyIndex.Add(iNo);
                            break;
                        case STRING_MONTHLY:
                            this.monthlyIndex.Add(iNo);
                            break;
                    }
                }
            }
            else
            {
                MaintenanceJournalListData listRow = this.maintenanceJournalListDatasServiceman[0];

                for (int i = 1; i <= this.maintenanceJournalListDatasServiceman.Count; i++)
                {
                    listRow = this.maintenanceJournalListDatasServiceman[i - 1];
                    strCol = listRow.kind;
                    iNo = listRow.No;

                    switch (strCol)
                    {
                        case STRING_MONTHLY:
                            this.monthlyIndex.Add(iNo);
                            break;
                        case STRING_YEARLY:
                            this.yearlyIndex.Add(iNo);
                            break;
                    }
                }
            }

        }

        /// <summary>
        /// IsShow用グリッド型リスト
        /// </summary>
        /// <returns></returns>
        public List<MaintenanceJournalListData> ISshowMaintenaceListData()
        {
            // モジュール接続台数
            ModuleNumConnected = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

            // xmlファイルを取得
            Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.LoadRaw();

            // メンテンナンス日誌番号、メンテナンス項目名を取得
            Singleton<ParameterFilePreserve<MaintenanceJournalCodeDataManager>>.Instance.LoadRaw();

            List<MaintenanceJournalListData> maintenanceJournalList = new List<MaintenanceJournalListData>();

            try
            {
                // 種別ごとのメッセージリスト
                List<List<MaintenanceJournalCodeData>> codeList = new List<List<MaintenanceJournalCodeData>>();
                codeList = CodeListEachKind();

                int iTargetCodeList = 0;


                // ユーザー用メンテナンス日誌の場合
                if (this.mainteJournalType == MaintenanceJournalType.User)
                {
                    CarisXMaintenanceUserParameter AllCheckFlagCheck = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param;

                    iTargetCodeList = (int)Kind.U_Daily;
                    List<List<Boolean>> dailyblModuleList = new List<List<Boolean>>();
                    for (int i = 0; i < ModuleNumConnected; i++)
                    {
                        dailyblModuleList.Add(Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param.SlaveList[i].DailyCheckItem);

                    }

                    CreateMaintenanceJournalGridData(ref maintenanceJournalList, codeList, dailyblModuleList, iTargetCodeList);


                    iTargetCodeList = (int)Kind.U_Weekly;
                    List<List<Boolean>> weeklyblModuleList = new List<List<Boolean>>();
                    for (int i = 0; i < ModuleNumConnected; i++)
                    {
                        weeklyblModuleList.Add(Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param.SlaveList[i].WeeklyCheckItem);
                    }

                    CreateMaintenanceJournalGridData(ref maintenanceJournalList, codeList, weeklyblModuleList, iTargetCodeList);

                    iTargetCodeList = (int)Kind.U_Monthly;
                    List<List<Boolean>> monthlyblModuleList = new List<List<Boolean>>();
                    for (int i = 0; i < ModuleNumConnected; i++)
                    {
                        monthlyblModuleList.Add(Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param.SlaveList[i].MonthlyCheckItem);
                    }

                    CreateMaintenanceJournalGridData(ref maintenanceJournalList, codeList, monthlyblModuleList, iTargetCodeList);

                }
                // サービスマン用メンテナンス日誌の場合
                else if (this.mainteJournalType == MaintenanceJournalType.Serviceman)
                {
                    CarisXMaintenanceServicemanParameter AllCheckFlagCheck = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param;

                    iTargetCodeList = (int)Kind.S_Monthly;
                    List<List<Boolean>> monthlyblModuleList = new List<List<Boolean>>();
                    for (int i = 0; i < ModuleNumConnected; i++)
                    {
                        monthlyblModuleList.Add(Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param.SlaveList[i].MonthlyCheckItem);
                    }

                    CreateMaintenanceJournalGridData(ref maintenanceJournalList, codeList, monthlyblModuleList, iTargetCodeList);

                    iTargetCodeList = (int)Kind.S_Yearly;
                    List<List<Boolean>> yearlyblModuleList = new List<List<Boolean>>();
                    for (int i = 0; i < ModuleNumConnected; i++)
                    {
                        yearlyblModuleList.Add(Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param.SlaveList[i].YearlyCheckItem);
                    }

                    CreateMaintenanceJournalGridData(ref maintenanceJournalList, codeList, yearlyblModuleList, iTargetCodeList);

                }

                return maintenanceJournalList;
            }
            catch (Exception)
            {
                return maintenanceJournalList;
            }
        }

        #endregion


        #region [privateメソッド]

        /// <summary>
        /// メンテンナンス日誌データリスト作成
        /// </summary>
        /// <remarks>
        /// 引数の種別のメンテンナンス日誌データリストを作成します。
        /// </remarks>
        /// <param name="maintenanceJournalDatas">格納するデータリスト</param>
        /// <param name="maintenanceJournalType">種別</param>
        private void LoadMaintenanceJournalListData(ref List<MaintenanceJournalListData> maintenanceJournalDatas, MaintenanceJournalType maintenanceJournalType)
        {
            // xmlファイルを取得
            Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.LoadRaw();

            // メンテンナンス日誌番号、メンテナンス項目名を取得
            Singleton<ParameterFilePreserve<MaintenanceJournalCodeDataManager>>.Instance.LoadRaw();

            // 種別ごとのメッセージリスト
            List<List<MaintenanceJournalCodeData>> codeList = new List<List<MaintenanceJournalCodeData>>();
            codeList = CodeListEachKind();

            int iTargetCodeList = 0;

            List<MaintenanceJournalListData> maintenanceJournalList = new List<MaintenanceJournalListData>();

            // ユーザー用メンテナンス日誌の場合
            if (maintenanceJournalType == MaintenanceJournalType.User)
            {
                CarisXMaintenanceUserParameter AllCheckFlagCheck = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param;

                // デイリー
                if (AllCheckFlagCheck.AllCheckDaily == false)
                {
                    iTargetCodeList = (int)Kind.U_Daily;
                    List<List<Boolean>> blModuleList = new List<List<Boolean>>();
                    for (int i = 0; i < ModuleNumConnected; i++)
                    {
                        blModuleList.Add(Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param.SlaveList[i].DailyCheckItem);

                    }

                    CreateMaintenanceJournalGridData(ref maintenanceJournalList, codeList, blModuleList, iTargetCodeList);
                }

                // ウィークリー
                if (AllCheckFlagCheck.AllCheckWeekly == false)
                {
                    iTargetCodeList = (int)Kind.U_Weekly;
                    List<List<Boolean>> blModuleList = new List<List<Boolean>>();
                    for (int i = 0; i < ModuleNumConnected; i++)
                    {
                        blModuleList.Add(Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param.SlaveList[i].WeeklyCheckItem);
                    }

                    CreateMaintenanceJournalGridData(ref maintenanceJournalList, codeList, blModuleList, iTargetCodeList);
                }

                // マンスリー
                if (AllCheckFlagCheck.AllCheckMonthly == false)
                {
                    iTargetCodeList = (int)Kind.U_Monthly;
                    List<List<Boolean>> blModuleList = new List<List<Boolean>>();
                    for (int i = 0; i < ModuleNumConnected; i++)
                    {
                        blModuleList.Add(Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param.SlaveList[i].MonthlyCheckItem);
                    }

                    CreateMaintenanceJournalGridData(ref maintenanceJournalList, codeList, blModuleList, iTargetCodeList);
                }
            }
            // サービスマン用メンテナンス日誌の場合
            else if (maintenanceJournalType == MaintenanceJournalType.Serviceman)
            {
                CarisXMaintenanceServicemanParameter AllCheckFlagCheck = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param;

                // マンスリー
                if (AllCheckFlagCheck.AllCheckMonthly == false)
                {
                    iTargetCodeList = (int)Kind.S_Monthly;
                    List<List<Boolean>> blModuleList = new List<List<Boolean>>();
                    for (int i = 0; i < ModuleNumConnected; i++)
                    {
                        blModuleList.Add(Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param.SlaveList[i].MonthlyCheckItem);
                    }

                    CreateMaintenanceJournalGridData(ref maintenanceJournalList, codeList, blModuleList, iTargetCodeList);
                }

                // イヤーリー
                if (AllCheckFlagCheck.AllCheckYearly == false)
                {

                    iTargetCodeList = (int)Kind.S_Yearly;
                    List<List<Boolean>> blModuleList = new List<List<Boolean>>();
                    for (int i = 0; i < ModuleNumConnected; i++)
                    {
                        blModuleList.Add(Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param.SlaveList[i].YearlyCheckItem);
                    }

                    CreateMaintenanceJournalGridData(ref maintenanceJournalList, codeList, blModuleList, iTargetCodeList);

                }

            }

            // 番号順に並び変えます
            var x = from i in maintenanceJournalList
                    orderby int.Parse(i.maintenanceJournalNo)
                    select i;
            maintenanceJournalList = x.ToList();

            // gridのNo列を昇順となるように変更します
            for (int i = 0; i < maintenanceJournalList.Count; i++)
            {
                maintenanceJournalList[i].No = i + 1;
            }

            maintenanceJournalDatas = maintenanceJournalList;
        }


        /// <summary>
        /// メンテンナンス日誌データリスト作成
        /// </summary>
        /// <remarks>
        /// 引数の種別のメンテンナンス日誌データリストを作成します。
        /// </remarks>
        /// <param name="maintenanceJournalDatas">格納するデータリスト</param>
        /// <param name="maintenanceJournalType">種別</param>
        private void newLoadMaintenanceJournalListData(Oelco.Common.GUI.CustomGrid grdMaintenanceList, MaintenanceJournalType maintenanceJournalType)
        {
            // 種別ごとのメッセージリスト
            List<List<MaintenanceJournalCodeData>> codeList = new List<List<MaintenanceJournalCodeData>>();
            codeList = CodeListEachKind();

            List<MaintenanceJournalListData> maintenanceJournalList = new List<MaintenanceJournalListData>();

            // ユーザー用メンテナンス日誌の場合
            if (maintenanceJournalType == MaintenanceJournalType.User)
            {
                CarisXMaintenanceUserParameter AllCheckFlagCheck = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param;

                // デイリー
                if (AllCheckFlagCheck.AllCheckDaily == false)
                {
                    // kind毎にgridから取得するデータリストが違うため
                    for (int i = 0; i < this.dailyIndex.Count; i++)
                    {
                        // rowと引数で1ずれる
                        int iIndex = this.dailyIndex[i] - 1;
                        var listObjectgridMaintenanceList = (MaintenanceJournalListData)grdMaintenanceList.Rows[iIndex].ListObject;
                        maintenanceJournalList.Add(listObjectgridMaintenanceList);
                    }
                }

                // ウィークリー
                if (AllCheckFlagCheck.AllCheckWeekly == false)
                {
                    // kind毎にgridから取得するデータリストが違うため
                    for (int i = 0; i < this.weeklyIndex.Count; i++)
                    {
                        // rowと引数で1ずれる
                        int iIndex = this.weeklyIndex[i] - 1;
                        var listObjectgridMaintenanceList = (MaintenanceJournalListData)grdMaintenanceList.Rows[iIndex].ListObject;
                        maintenanceJournalList.Add(listObjectgridMaintenanceList);
                    }
                }

                // マンスリー
                if (AllCheckFlagCheck.AllCheckMonthly == false)
                {
                    // kind毎にgridから取得するデータリストが違うため
                    for (int i = 0; i < this.monthlyIndex.Count; i++)
                    {
                        // rowと引数で1ずれる
                        int iIndex = this.monthlyIndex[i] - 1;
                        var listObjectgridMaintenanceList = (MaintenanceJournalListData)grdMaintenanceList.Rows[iIndex].ListObject;
                        maintenanceJournalList.Add(listObjectgridMaintenanceList);
                    }
                }

                // 番号順に並び変えます
                var x = from i in maintenanceJournalList
                        orderby int.Parse(i.maintenanceJournalNo)
                        select i;
                maintenanceJournalList = x.ToList();

                // gridのNo列を昇順となるように変更します
                for (int i = 0; i < maintenanceJournalList.Count; i++)
                {
                    maintenanceJournalList[i].No = i + 1;
                }

                this.maintenanceJournalListDatasUser = maintenanceJournalList;
            }
            // サービスマン用メンテナンス日誌の場合
            else if (maintenanceJournalType == MaintenanceJournalType.Serviceman)
            {
                CarisXMaintenanceServicemanParameter AllCheckFlagCheck = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param;

                // マンスリー
                if (AllCheckFlagCheck.AllCheckMonthly == false)
                {
                    // kind毎にgridから取得するデータリストが違うため
                    for (int i = 0; i < this.monthlyIndex.Count; i++)
                    {
                        // rowと引数で1ずれる
                        int iIndex = this.monthlyIndex[i] - 1;
                        var listObjectgridMaintenanceList = (MaintenanceJournalListData)grdMaintenanceList.Rows[iIndex].ListObject;
                        maintenanceJournalList.Add(listObjectgridMaintenanceList);
                    }
                }

                // イヤーリー
                if (AllCheckFlagCheck.AllCheckYearly == false)
                {

                    // kind毎にgridから取得するデータリストが違うため
                    for (int i = 0; i < this.yearlyIndex.Count; i++)
                    {
                        // rowと引数で1ずれる
                        int iIndex = this.yearlyIndex[i] - 1;
                        var listObjectgridMaintenanceList = (MaintenanceJournalListData)grdMaintenanceList.Rows[iIndex].ListObject;
                        maintenanceJournalList.Add(listObjectgridMaintenanceList);
                    }
                }

                // 番号順に並び変えます
                var x = from i in maintenanceJournalList
                        orderby int.Parse(i.maintenanceJournalNo)
                        select i;
                maintenanceJournalList = x.ToList();

                // gridのNo列を昇順となるように変更します
                for (int i = 0; i < maintenanceJournalList.Count; i++)
                {
                    maintenanceJournalList[i].No = i + 1;
                }

                this.maintenanceJournalListDatasServiceman = maintenanceJournalList;
            }
        }


        /// <summary>
        /// グリッドデータ作成
        /// </summary>
        /// <param name="maintenanceJournalListDatas">grid格納用のデータ型</param>
        /// <param name="codeList">種別ごとのメッセージリスト</param>
        /// <param name="blModuleList">moduleごとのチェック値のリスト</param>
        /// <param name="iKind">enumをint型にした種別</param>
        private void CreateMaintenanceJournalGridData(ref List<MaintenanceJournalListData> maintenanceJournalList, List<List<MaintenanceJournalCodeData>> codeList, List<List<Boolean>> blModuleList, int iKind)
        {
            try
            {

                // 縦横変更
                VerticalandHolizontalChange(ref blModuleList);

                // グリッド表示データを格納
                for (int i = 0; i < codeList[iKind].Count; i++)
                {
                    maintenanceJournalList.Add(new MaintenanceJournalListData(i + 1, codeList[iKind][i].Title, codeList[iKind][i].Kind,
                        blModuleList[i], int.Parse(codeList[iKind][i].Code), codeList[iKind][i].Unit));
                }
            }
            catch (Exception ex)
            {
                // ファイル読込に失敗しました。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("Reading the file failed :{0}", ex.Message));
            }
        }

        /// <summary>
        /// リストの縦横変更
        /// </summary>
        /// <param name="listBooleanData">変更したいリスト</param>
        public void VerticalandHolizontalChange(ref List<List<Boolean>> listBooleanData)
        {
            try
            {

                var listBool = new List<List<bool>>();
                for (int i = 0; i < listBooleanData.Last().Count; i++)
                {
                    var listBoolItemRow = new List<bool>();
                    for (int j = 0; j < listBooleanData.Count; j++)
                    {
                        listBoolItemRow.Add(listBooleanData[j][i]);
                    }
                    listBool.Insert(listBool.Count, listBoolItemRow);
                }
                listBooleanData = listBool;

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// リストの縦横変更
        /// </summary>
        /// <param name="listBooleanData">変更したいリスト</param>
        public void VerticalandHolizontalChange(ref List<List<String>> listStringData)
        {
            try
            {
                // 縦横変更
                List<List<String>> reverseItemList = new List<List<String>>();
                for (int i = 0; i < listStringData.Last().Count; i++)
                {
                    var listStringDataRow = new List<String>();
                    for (int j = 0; j < listStringData.Count; j++)
                    {
                        // "が入ってしまうため、ここで削除
                        listStringDataRow.Add(listStringData[j][i].Replace("\"", ""));
                    }
                    reverseItemList.Insert(reverseItemList.Count, listStringDataRow);
                }
                listStringData = reverseItemList;

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 種別ごとのメッセージリストを作成
        /// </summary>
        public List<List<MaintenanceJournalCodeData>> CodeListEachKind()
        {
            var maintenanceJournalCodeList = Singleton<ParameterFilePreserve<MaintenanceJournalCodeDataManager>>.Instance.Param.CodeDataList;
            // 重複チェック
            List<int> b = new List<int>();
            var maintenanceJournalCodeDatas = new List<MaintenanceJournalCodeData>(maintenanceJournalCodeList);
            for (int i = maintenanceJournalCodeList.Count; i > 1; i--)
            {
                if (i >= 1 && maintenanceJournalCodeDatas[i - 1].Code == maintenanceJournalCodeDatas[i - 2].Code
                    && maintenanceJournalCodeDatas[i - 1].Kind == maintenanceJournalCodeDatas[i - 2].Kind)
                {
                    maintenanceJournalCodeDatas.Remove(maintenanceJournalCodeDatas[i - 1]);
                }
            }

            // 種別ごとのコードリストの宣言
            List<MaintenanceJournalCodeData> codeListUserDaily = new List<MaintenanceJournalCodeData>();
            List<MaintenanceJournalCodeData> codeListUserWeekly = new List<MaintenanceJournalCodeData>();
            List<MaintenanceJournalCodeData> codeListUserMonthly = new List<MaintenanceJournalCodeData>();
            List<MaintenanceJournalCodeData> codeListServicemanMonthly = new List<MaintenanceJournalCodeData>();
            List<MaintenanceJournalCodeData> codeListServicemanYearly = new List<MaintenanceJournalCodeData>();
            List<List<MaintenanceJournalCodeData>> codeListKind = new List<List<MaintenanceJournalCodeData>>();
            // コードリストを種別ごとに振り分ける。
            foreach (var rowCodeList in maintenanceJournalCodeDatas)
            {

                // // 種別はメンテンナンス日誌番号が1~100はユーザ権限のデイリー
                if (rowCodeList.Unit == "0"
                    && rowCodeList.Kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_009)
                {
                    codeListUserDaily.Add(rowCodeList);
                }
                // 種別はメンテンナンス日誌番号が101~200はユーザ権限のウィークリー
                else if (rowCodeList.Unit == "0"
                    && rowCodeList.Kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_010)
                {
                    codeListUserWeekly.Add(rowCodeList);
                }
                // 種別はメンテンナンス日誌番号が201~300はユーザ権限のマンスリー
                else if (rowCodeList.Unit == "0"
                    && rowCodeList.Kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_011)
                {
                    codeListUserMonthly.Add(rowCodeList);
                }
                // 種別はメンテンナンス日誌番号が1001~1100はサービスマン権限のマンスリー
                else if (rowCodeList.Unit != "0"
                    && rowCodeList.Kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_011)
                {
                    codeListServicemanMonthly.Add(rowCodeList);
                }
                // 種別はメンテンナンス日誌番号が1101~1200はユーザ権限のマンスリー
                else if (rowCodeList.Unit != "0"
                    && rowCodeList.Kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_012)
                {
                    codeListServicemanYearly.Add(rowCodeList);
                }
            }

            // リストに種別ごとのメッセージリストを追加
            codeListKind.Add(codeListUserDaily);
            codeListKind.Add(codeListUserWeekly);
            codeListKind.Add(codeListUserMonthly);
            codeListKind.Add(codeListServicemanMonthly);
            codeListKind.Add(codeListServicemanYearly);

            return codeListKind;
        }


        /// OKボタン処理
        /// 
        public void OkExecute(Oelco.Common.GUI.CustomGrid grdMaintenanceList)
        {
            try
            {
                // ユーザー用メンテナンス日誌の場合
                if (this.mainteJournalType == MaintenanceJournalType.User)
                {
                    ParameterFilePreserve<CarisXMaintenanceUserParameter> maintenanceJournalUserList = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance;

                    // 画面グリッドから情報をリストに格納します
                    OkExecuteUser(ref maintenanceJournalUserList, grdMaintenanceList);

                    //xmlファイルへ保存します
                    maintenanceJournalUserList.SaveRaw();

                    // csvに保存します
                    exportData(grdMaintenanceList);

                    // 一時的にチェック状態を保存します
                    newLoadMaintenanceJournalListData(grdMaintenanceList, MaintenanceJournalType.User);
                }
                // サービスマン用メンテナンス日誌の場合
                else if (this.mainteJournalType == MaintenanceJournalType.Serviceman)
                {
                    // 一時的にチェック状態を保存します
                    newLoadMaintenanceJournalListData(grdMaintenanceList, MaintenanceJournalType.Serviceman);
                }
            }
            catch (Exception ex)
            {
                //OKボタン操作に失敗しました。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("OK button operation failed :{0}", ex.Message));
            }
        }

        /// <summary>
        /// User時のOK押下処理
        /// </summary>
        /// <param name="maintenanceJournalUserList"></param>
        /// <param name="grdMaintenanceList"></param>
        private void OkExecuteUser(ref ParameterFilePreserve<CarisXMaintenanceUserParameter> maintenanceJournalUserList, Oelco.Common.GUI.CustomGrid grdMaintenanceList)
        {
            // 全チェックかどうかのフラグを取得するためのインスタンス作成
            var AllCheckFlagCheck = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param;
            bool checkOffFlag = false;

            // 行
            // module1 ～ 4
            if (AllCheckFlagCheck.AllCheckDaily == false)
            {
                for (int i = 0; i < ModuleNumConnected; i++)
                {
                    // デイリーのチェック状況を格納する
                    for (int j = this.dailyIndex.First() - 1; j < this.dailyIndex.Last(); j++)
                    {
                        // 画面の文字列で振り分ける
                        if (grdMaintenanceList.Rows[j].Cells[i + 3].Value.ToString() == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013)
                        {
                            maintenanceJournalUserList.Param.SlaveList[i].DailyCheckItem[j] = true;
                        }
                        else
                        {
                            maintenanceJournalUserList.Param.SlaveList[i].DailyCheckItem[j] = false;
                            checkOffFlag = true;
                        }
                    }

                }
                // 全てチェック
                if (checkOffFlag == false)
                {
                    maintenanceJournalUserList.Param.AllCheckDaily = true;
                    maintenanceJournalUserList.Param.AllFinishDaily = DateTime.Now;
                }
                // フラグをリセット
                checkOffFlag = false;
            }

            if (AllCheckFlagCheck.AllCheckWeekly == false)
            {
                // module1 ～ 4
                for (int i = 0; i < ModuleNumConnected; i++)
                {
                    // リストから別のリストの[0]から値を代入する為
                    int cnt = 0;
                    // ウィークリーのチェック状況を格納する
                    for (int j = this.weeklyIndex.First() - 1; j < this.weeklyIndex.Last(); j++)
                    {
                        // 画面の文字列で振り分ける
                        if (grdMaintenanceList.Rows[j].Cells[i + 3].Value.ToString() == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013)
                        {
                            maintenanceJournalUserList.Param.SlaveList[i].WeeklyCheckItem[cnt] = true;
                        }
                        else
                        {
                            maintenanceJournalUserList.Param.SlaveList[i].WeeklyCheckItem[cnt] = false;
                            checkOffFlag = true;
                        }
                        cnt++;
                    }
                }
                // 全てチェック
                if (checkOffFlag == false)
                {
                    maintenanceJournalUserList.Param.AllCheckWeekly = true;
                    maintenanceJournalUserList.Param.AllFinishWeekly = DateTime.Now;
                }
                // フラグをリセット
                checkOffFlag = false;
            }

            if (AllCheckFlagCheck.AllCheckMonthly == false)
            {
                // module1 ～ 4
                for (int i = 0; i < ModuleNumConnected; i++)
                {
                    // リストから別のリストの[0]から値を代入する為
                    int cnt = 0;
                    for (int j = this.monthlyIndex.First() - 1; j < this.monthlyIndex.Last(); j++)
                    {
                        // 画面の文字列で振り分ける
                        if (grdMaintenanceList.Rows[j].Cells[i + 3].Value.ToString() == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013)
                        {
                            maintenanceJournalUserList.Param.SlaveList[i].MonthlyCheckItem[cnt] = true;
                        }
                        else
                        {
                            maintenanceJournalUserList.Param.SlaveList[i].MonthlyCheckItem[cnt] = false;
                            checkOffFlag = true;
                        }
                        cnt++;
                    }
                }
                // 全てチェック
                if (checkOffFlag == false)
                {
                    maintenanceJournalUserList.Param.AllCheckMonthly = true;
                    maintenanceJournalUserList.Param.AllFinishMonthly = DateTime.Now;
                }
            }
        }

        #region [csv]

        /// <summary>
        /// チェック情報のファイル出力
        /// </summary>
        /// <remarks>
        /// グリッドのチェック情報のファイル出力します
        /// </remarks>
        private void exportData(Oelco.Common.GUI.CustomGrid grdMaintenanceList)
        {
            // ファイル出力対象を取得
            // xmlファイルを取得
            Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.LoadRaw();
            ParameterFilePreserve<CarisXMaintenanceUserParameter> exportUserData = new ParameterFilePreserve<CarisXMaintenanceUserParameter>();
            // xmlファイルを取得
            Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.LoadRaw();
            ParameterFilePreserve<CarisXMaintenanceServicemanParameter> exportServicemanData = new ParameterFilePreserve<CarisXMaintenanceServicemanParameter>();
            List<MaintenanceJournalCodeData> maintenanceJournalCodeList = Singleton<ParameterFilePreserve<MaintenanceJournalCodeDataManager>>.Instance.Param.CodeDataList;

            // 重複チェック
            List<int> b = new List<int>();
            var maintenanceJournalCodeDatas = new List<MaintenanceJournalCodeData>(maintenanceJournalCodeList);
            for (int i = maintenanceJournalCodeList.Count; i > 1; i--)
            {
                if (i >= 1 && maintenanceJournalCodeDatas[i - 1].Code == maintenanceJournalCodeDatas[i - 2].Code
                    && maintenanceJournalCodeDatas[i - 1].Kind == maintenanceJournalCodeDatas[i - 2].Kind)
                {
                    maintenanceJournalCodeDatas.Remove(maintenanceJournalCodeDatas[i - 1]);
                }
            }

            string saveFileName = string.Empty;
            string saveFilePath = string.Empty;
            string strKind = string.Empty;

            try
            {
                // ユーザー用メンテナンス日誌の場合
                if (this.mainteJournalType == MaintenanceJournalType.User)
                {
                    if (dailyIndex.Count != 0)
                    {
                        strKind = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_009;
                        // 202001_Mainte_Dayly.csv
                        saveFileName = DateTime.Now.ToString("yyyyMM") + "_Mainte_Daily.csv";

                        for (int moduleCnt = 0; moduleCnt < ModuleNumConnected; moduleCnt++)
                        {
                            // csvファイル出力先パス
                            switch (moduleCnt)
                            {
                                case 0:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalUserModule1 + "\\" + saveFileName;
                                    break;
                                case 1:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalUserModule2 + "\\" + saveFileName;
                                    break;
                                case 2:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalUserModule3 + "\\" + saveFileName;
                                    break;
                                case 3:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalUserModule4 + "\\" + saveFileName;
                                    break;
                            }

                            CreateCsv(maintenanceJournalCodeDatas, moduleCnt, strKind, saveFilePath, grdMaintenanceList);
                        }
                    }

                    if (weeklyIndex.Count != 0)
                    {
                        strKind = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_010;
                        // 202001_Mainte_Weekly.csv
                        saveFileName = DateTime.Now.ToString("yyyyMM") + "_Mainte_Weekly.csv";

                        for (int moduleCnt = 0; moduleCnt < ModuleNumConnected; moduleCnt++)
                        {
                            // csvファイル出力先パス
                            switch (moduleCnt)
                            {
                                case 0:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalUserModule1 + "\\" + saveFileName;
                                    break;
                                case 1:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalUserModule2 + "\\" + saveFileName;
                                    break;
                                case 2:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalUserModule3 + "\\" + saveFileName;
                                    break;
                                case 3:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalUserModule4 + "\\" + saveFileName;
                                    break;
                            }
                            CreateCsv(maintenanceJournalCodeDatas, moduleCnt, strKind, saveFilePath, grdMaintenanceList);
                        }
                    }

                    if (monthlyIndex.Count != 0)
                    {
                        strKind = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_011;
                        // 2020_Mainte_Monthly.csv
                        saveFileName = DateTime.Now.ToString("yyyy") + "_Mainte_Monthly.csv";

                        for (int moduleCnt = 0; moduleCnt < ModuleNumConnected; moduleCnt++)
                        {
                            // csvファイル出力先パス
                            switch (moduleCnt)
                            {
                                case 0:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalUserModule1 + "\\" + saveFileName;
                                    break;
                                case 1:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalUserModule2 + "\\" + saveFileName;
                                    break;
                                case 2:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalUserModule3 + "\\" + saveFileName;
                                    break;
                                case 3:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalUserModule4 + "\\" + saveFileName;
                                    break;
                            }
                            CreateCsv(maintenanceJournalCodeDatas, moduleCnt, strKind, saveFilePath, grdMaintenanceList);
                        }
                    }
                }
                // サービスマン用メンテナンス日誌の場合
                else if (this.mainteJournalType == MaintenanceJournalType.Serviceman)
                {
                    strKind = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_011;
                    if (monthlyIndex.Count != 0)
                    {
                        // 202001_Mainte_Dayly.csv
                        saveFileName = DateTime.Now.ToString("yyyy") + "_S_Mainte_Monthly.csv";

                        for (int moduleCnt = 0; moduleCnt < ModuleNumConnected; moduleCnt++)
                        {
                            // csvファイル出力先パス
                            switch (moduleCnt)
                            {
                                case 0:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalServicemanModule1 + "\\" + saveFileName;
                                    break;
                                case 1:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalServicemanModule2 + "\\" + saveFileName;
                                    break;
                                case 2:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalServicemanModule3 + "\\" + saveFileName;
                                    break;
                                case 3:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalServicemanModule4 + "\\" + saveFileName;
                                    break;
                            }
                            CreateCsv(maintenanceJournalCodeDatas, moduleCnt, strKind, saveFilePath, grdMaintenanceList);
                        }
                    }

                    if (yearlyIndex.Count != 0)
                    {
                        strKind = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_012;
                        // yearly.csv
                        saveFileName = "S_Mainte_Yearly.csv";

                        for (int moduleCnt = 0; moduleCnt < ModuleNumConnected; moduleCnt++)
                        {
                            // csvファイル出力先パス
                            switch (moduleCnt)
                            {
                                case 0:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalServicemanModule1 + "\\" + saveFileName;
                                    break;
                                case 1:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalServicemanModule2 + "\\" + saveFileName;
                                    break;
                                case 2:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalServicemanModule3 + "\\" + saveFileName;
                                    break;
                                case 3:
                                    saveFilePath = CarisXConst.PathMaintenanceJournalServicemanModule4 + "\\" + saveFileName;
                                    break;
                            }
                            CreateCsv(maintenanceJournalCodeDatas, moduleCnt, strKind, saveFilePath, grdMaintenanceList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //csv作成に失敗しました。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("csv creation failed :{0}", ex.Message));
            }

        }

        public void CreateCsv(List<MaintenanceJournalCodeData> maintenanceJournalCodeDatas, int moduleCnt, string strKind, string saveFilePath, Oelco.Common.GUI.CustomGrid grdMaintenanceList)
        {
            // データ作成
            List<List<String>> maintenanceCsvDataItem = new List<List<string>>();

            // csvファイルが存在する場合、情報を取得
            if (System.IO.File.Exists(saveFilePath))
            {
                // 既存csvのインポート
                MaintenanceJournalImportCsv(maintenanceCsvDataItem, saveFilePath);

                // 縦横変更
                VerticalandHolizontalChange(ref maintenanceCsvDataItem);
            }
            // csvファイルが存在しない場合、項目名を作成
            else
            {
                if (this.mainteJournalType == MaintenanceJournalType.User)
                {
                    maintenanceCsvDataItem = CreateCsvDataItemUser(maintenanceCsvDataItem, moduleCnt, strKind, grdMaintenanceList);
                }
                else if (this.mainteJournalType == MaintenanceJournalType.Serviceman)
                {
                    maintenanceCsvDataItem = CreateCsvDataItemServiceman(maintenanceCsvDataItem, moduleCnt, strKind);
                }
            }

            if (this.mainteJournalType == MaintenanceJournalType.User)
            {
                // 今回のデータを作成（最終列に追加するデータ）
                maintenanceCsvDataItem = CreateCsvDataInformationUser(maintenanceCsvDataItem, moduleCnt, strKind, grdMaintenanceList);
                // csvがある状態で項目追加してるかチェック。追加していたら処理も行います
                CsvAddAdjustmentUser(ref maintenanceCsvDataItem, moduleCnt, grdMaintenanceList, strKind);
            }
            else if (this.mainteJournalType == MaintenanceJournalType.Serviceman)
            {
                maintenanceCsvDataItem = CreateCsvDataInformationServiceman(maintenanceCsvDataItem, moduleCnt, strKind);
                // csvがある状態で項目追加してるかチェック。追加していたら処理も行います
                CsvAddAdjustmentServiceman(ref maintenanceCsvDataItem, moduleCnt, strKind);
            }

            // 縦横変更
            VerticalandHolizontalChange(ref maintenanceCsvDataItem);

            // メンテナンス日誌CSV出力処理
            MaintenanceJournalExportCsv(maintenanceCsvDataItem, saveFilePath, false);
        }

        /// <summary>
        /// csvがある状態で項目追加した時の処理
        /// </summary>
        private void CsvAddAdjustmentUser(ref List<List<String>> maintenanceCsvDataItem, int moduleCnt, Oelco.Common.GUI.CustomGrid grdMaintenanceList, string strKind)
        {
            List<List<String>> diffCsvDatas = new List<List<String>>();

            // 今回のデータを作成（最終列に追加するデータ）
            diffCsvDatas = CreateCsvDataItemUser(maintenanceCsvDataItem, moduleCnt, strKind, grdMaintenanceList);

            // メンテンナンス番号列で追加されたセル（行）を取得
            var rowIndexList = diffCsvDatas[0].Except(maintenanceCsvDataItem[0]).ToList();
            // グリッドの項目にあって既存csvにないコード列が1件以上あるか
            if (rowIndexList.Count > 0)
            {
                // コード列
                var diffCsvDatasCodeColumn = diffCsvDatas[0];
                // 追加された行のインデックスをリストにする
                List<Int32> rowDiffIndexList = new List<Int32>();

                // 追加されたコード列リストのうち、追加項目のインデックスを取得
                for (int i = 0; i < rowIndexList.Count; i++)
                {
                    Int32 rowindex = diffCsvDatasCodeColumn.FindIndex(item => item == rowIndexList[i]); // インデックスだけのint型リスト
                    rowDiffIndexList.Add(rowindex);
                }

                // 対象の行に追加する。その場合、本日以前のデータ列は空白とする
                for (int i = 0; i < rowDiffIndexList.Count; i++)
                {
                    // 変更されたされたインデックス
                    Int32 iTargetAddIndex = rowDiffIndexList[i];

                    for (int j = 0; j < maintenanceCsvDataItem.Count - 1; j++)
                    {
                        if (j <= 1)
                        {
                            String strDiffData = diffCsvDatas[j][iTargetAddIndex];
                            // コード列 チェックアイテム列
                            maintenanceCsvDataItem[j].Insert(iTargetAddIndex, strDiffData);
                        }
                        else
                        {
                            // データ列 空白を追加
                            maintenanceCsvDataItem[j].Insert(iTargetAddIndex, "");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// csvがある状態で項目追加した時の処理
        /// </summary>
        private void CsvAddAdjustmentServiceman(ref List<List<String>> maintenanceCsvDataItem, int moduleCnt, string strKind)
        {
            List<List<String>> diffCsvDatas = new List<List<String>>();

            diffCsvDatas = CreateCsvDataItemServiceman(maintenanceCsvDataItem, moduleCnt, strKind);

            // メンテンナンス番号列で追加されたセル（行）を取得
            var rowIndexList = diffCsvDatas[0].Except(maintenanceCsvDataItem[0]).ToList();
            // グリッドの項目にあって既存csvにないコード列が1件以上あるか
            if (rowIndexList.Count > 0)
            {
                // コード列
                var diffCsvDatasCodeColumn = diffCsvDatas[0];
                // 追加された行のインデックスをリストにする
                List<Int32> rowDiffIndexList = new List<Int32>();

                // 追加されたコード列リストのうち、追加項目のインデックスを取得
                for (int i = 0; i < rowIndexList.Count; i++)
                {
                    Int32 rowindex = diffCsvDatasCodeColumn.FindIndex(item => item == rowIndexList[i]); // インデックスだけのint型リスト
                    rowDiffIndexList.Add(rowindex);
                }

                // 対象の行に追加する。その場合、本日以前のデータ列は空白とする
                for (int i = 0; i < rowDiffIndexList.Count; i++)
                {
                    // 変更されたされたインデックス
                    Int32 iTargetAddIndex = rowDiffIndexList[i];

                    for (int j = 0; j < maintenanceCsvDataItem.Count - 1; j++)
                    {
                        if (j <= 1)
                        {
                            String strDiffData = diffCsvDatas[j][iTargetAddIndex];
                            // コード列 チェックアイテム列
                            maintenanceCsvDataItem[j].Insert(iTargetAddIndex, strDiffData);
                        }
                        else
                        {
                            // データ列 空白を追加
                            maintenanceCsvDataItem[j].Insert(iTargetAddIndex, "");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// CSV出力(※列名作成後)
        /// </summary>
        /// <remarks>
        /// CSV出力を行います。
        /// </remarks>
        /// <param name="dataList">出力データ</param>
        /// /// <param name="savePath">保存先ファイル名</param>
        /// <param name="append">[既定]作成または上書き(false)/末尾に追加(true)/ファイルが存在する場合追加。存在しない場合に作成(null)</param>
        private void MaintenanceJournalExportCsv(List<List<String>> dataList, String savePath, Boolean? append)
        {
            // 区切り文字の初期化
            this.separator = (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator != ",") ? "," : "\t";
            //modified by dong zhang for output the *.csv file ,use this format Mircrosoft Excel will load the file Normal way. 
            enc = Encoding.GetEncoding("utf-8");
            try
            {
                String dir = System.IO.Path.GetDirectoryName(savePath);
                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
                append = append ?? System.IO.File.Exists(savePath);
                using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(savePath, append.Value, this.enc))
                {
                    // 行データの書き込み
                    foreach (var data in dataList)
                    {
                        String strData = "\"" + string.Join("\"" + this.separator + "\"", data) + "\"";

                        streamWriter.WriteLine(strData);
                    }
                }
            }
            catch (Exception ex)
            {
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("failed to ExportCsv:{0}", ex.Message));
            }
        }

        /// <summary>
        /// CSV入力
        /// </summary>
        /// <remarks>
        /// CSV入力を行います。
        /// </remarks>
        /// <param name="dataList">出力データ</param>
        /// <param name="savePath">保存先ファイル名</param>
        private Boolean MaintenanceJournalImportCsv(List<List<String>> dataList, String savePath)
        {
            try
            {
                // 読み込みたいCSVファイルのパスを指定して開く
                System.IO.StreamReader sr = new System.IO.StreamReader(savePath);
                {
                    // 末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        // CSVファイルの一行を読み込む
                        string line = sr.ReadLine();
                        line = line.Replace("\",", "\"\",");

                        // 読み込んだ一行をカンマ毎に分けて配列に格納する
                        //string[] values = line.Split(',');
                        string[] b = new string[] { "\"," };
                        string[] values = line.Split(b, StringSplitOptions.None);

                        // 配列からリストに格納する
                        List<string> lists = new List<string>();
                        lists.AddRange(values);

                        dataList.Add(lists);
                    }
                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }
                return true;
            }
            catch (Exception ex)
            {
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("failed to ExportCsv:{0}", ex.Message));
                return false;
            }
        }


        ///
        /// <summary>
        /// CSV出力の項目作成
        /// </summary>
        /// <remarks>
        /// CSV出力用データの項目を作成します（縦横は逆）
        /// </remarks>
        /// <param name="CsvData">出力するcsv</param>
        /// <param name="moduleNo">モジュール番号</param>
        /// <param name="kind">種別</param>
        /// <returns>CsvDataにデータを格納</returns>
        private List<List<String>> CreateCsvDataItemUser(List<List<String>> CsvData, int moduleNo, String kind, Oelco.Common.GUI.CustomGrid grdMaintenanceList)
        {
            List<MaintenanceJournalListData> maintenanceListDatas = new List<MaintenanceJournalListData>();

            // Kindを設定
            int cntFirst = 0;
            int cntLast = 0;

            // 種別によりループ用変数の値を設定します
            // Daily
            if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_009)
            {
                cntFirst = this.dailyIndex.First() - 1;
                cntLast = this.dailyIndex.Last();
            }
            // Weekly
            else if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_010)
            {
                cntFirst = this.weeklyIndex.First() - 1;
                cntLast = this.weeklyIndex.Last();
            }
            // Monthly
            else if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_011)
            {
                cntFirst = this.monthlyIndex.First() - 1;
                cntLast = this.monthlyIndex.Last();
            }

            String strModuleName = "";

            // csv出力先の「種別」項目の名前を設定します
            // moduleの番号
            if (moduleNo == 0)
            {
                strModuleName = STRING_CHECKMODULE1;
            }
            // Weekly
            else if (moduleNo == 1)
            {
                strModuleName = STRING_CHECKMODULE2;
            }
            // Monthly
            else if (moduleNo == 2)
            {
                strModuleName = STRING_CHECKMODULE3;
            }
            // Yearly
            else if (moduleNo == 3)
            {
                strModuleName = STRING_CHECKMODULE4;
            }

            // gridから該当のkindの行情報を取得
            for (int i = cntFirst; i < cntLast; i++)
            {
                var listObjectgridMaintenanceList = (MaintenanceJournalListData)grdMaintenanceList.Rows[i].ListObject;
                maintenanceListDatas.Add(listObjectgridMaintenanceList);
            }

            // データ作成用
            var maintenanceCsvDataItem = new List<List<String>>();
            var maintenanceListDatasAdd = new List<String>();

            // csv出力の1列目を格納します
            for (int j = 0; j < maintenanceListDatas.Count + 2; j++)
            {

                if (j == 0)
                {
                    maintenanceListDatasAdd.Add(strModuleName);
                }
                else if (j == 1)
                {
                    // 項目名
                    maintenanceListDatasAdd.Add(STRING_CSV_CODE);
                }
                else if (j >= 2)
                {
                    // コード
                    maintenanceListDatasAdd.Add(string.Format("{0:00}", int.Parse(maintenanceListDatas[j - 2].unitNo)) + "-" + string.Format("{0:0000}", int.Parse(maintenanceListDatas[j - 2].maintenanceJournalNo)));
                }

            }
            maintenanceCsvDataItem.Insert(maintenanceCsvDataItem.Count, maintenanceListDatasAdd);

            // csv出力の2列目を格納します
            maintenanceListDatasAdd = new List<String>();
            // 項目作成
            for (int j = 0; j < maintenanceListDatas.Count + 2; j++)
            {
                // 要素追加
                if (j == 0)
                {
                    maintenanceListDatasAdd.Add(STRING_CSV_USER_NAME);
                }
                else if (j == 1)
                {
                    maintenanceListDatasAdd.Add(STRING_CSV_CHECK_ITEM);
                }
                else if (j >= 2)
                {
                    // 項目名
                    maintenanceListDatasAdd.Add(maintenanceListDatas[j - 2].checkItem);
                }

            }
            maintenanceCsvDataItem.Insert(maintenanceCsvDataItem.Count, maintenanceListDatasAdd);

            return maintenanceCsvDataItem;
        }

        ///
        /// <summary>
        /// CSV出力の項目作成
        /// </summary>
        /// <remarks>
        /// CSV出力用データの項目を作成します（縦横は逆）
        /// </remarks>
        /// <param name="CsvData">出力するcsv</param>
        /// <param name="moduleNo">モジュール番号</param>
        /// <param name="kind">種別</param>
        /// <returns>CsvDataにデータを格納</returns>
        private List<List<String>> CreateCsvDataItemServiceman(List<List<String>> CsvData, int moduleNo, String kind)
        {
            List<MaintenanceJournalListData> maintenanceListDatas = new List<MaintenanceJournalListData>();

            // Kindを設定
            int cntCount = 0;

            // Monthly
            if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_011)
            {
                cntCount = this.monthlyIndex.Count;
            }
            // Yearly
            else if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_012)
            {
                cntCount = this.yearlyIndex.Count;
            }

            String strModuleName = "";

            // csv出力先の「種別」項目の名前を設定します
            // moduleの番号
            if (moduleNo == 0)
            {
                strModuleName = STRING_CHECKMODULE1;
            }
            else if (moduleNo == 1)
            {
                strModuleName = STRING_CHECKMODULE2;
            }
            else if (moduleNo == 2)
            {
                strModuleName = STRING_CHECKMODULE3;
            }
            else if (moduleNo == 3)
            {
                strModuleName = STRING_CHECKMODULE4;
            }

            // データ作成用
            List<List<String>> maintenanceCsvDataItem = new List<List<String>>();
            List<String> maintenanceListDatasAdd = new List<String>();

            int k = 0;

            // csv出力の1列目を格納します
            for (int j = 0; j < cntCount + 2; j++)
            {
                if (j == 0)
                {
                    maintenanceListDatasAdd.Add(strModuleName);
                }
                else if (j == 1)
                {
                    // 項目名
                    maintenanceListDatasAdd.Add(STRING_CSV_CODE);
                }
                else if (j >= 2)
                {
                    // Monthly
                    if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_011)
                    {
                        k = this.monthlyIndex[j - 2] - 1;
                    }
                    // Yearly
                    else if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_012)
                    {
                        k = this.yearlyIndex[j - 2] - 1;
                    }

                    // コード
                    maintenanceListDatasAdd.Add(string.Format("{0:00}", int.Parse(this.maintenanceJournalListDatasServiceman[k].unitNo)) + "-" + string.Format("{0:0000}", int.Parse(this.maintenanceJournalListDatasServiceman[k].maintenanceJournalNo)));
                }
            }
            maintenanceCsvDataItem.Insert(maintenanceCsvDataItem.Count, maintenanceListDatasAdd);

            // csv出力の2列目を格納します
            maintenanceListDatasAdd = new List<String>();
            // 項目作成
            for (int j = 0; j < cntCount + 2; j++)
            {
                // 要素追加
                if (j == 0)
                {
                    maintenanceListDatasAdd.Add(STRING_CSV_USER_NAME);
                }
                else if (j == 1)
                {
                    maintenanceListDatasAdd.Add(STRING_CSV_CHECK_ITEM);
                }
                else if (j >= 2)
                {
                    // Monthly
                    if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_011)
                    {
                        k = this.monthlyIndex[j - 2] - 1;
                    }
                    // Yearly
                    else if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_012)
                    {
                        k = this.yearlyIndex[j - 2] - 1;
                    }

                    // 項目名
                    maintenanceListDatasAdd.Add(this.maintenanceJournalListDatasServiceman[k].checkItem);
                }

            }
            maintenanceCsvDataItem.Insert(maintenanceCsvDataItem.Count, maintenanceListDatasAdd);

            return maintenanceCsvDataItem;
        }



        ///
        /// <summary>
        /// CSV出力のデータ作成
        /// </summary>
        /// <remarks>
        /// CSV出力用データの今回の情報を作成します（縦横は逆）
        /// </remarks>
        /// <param name="CsvData">出力するcsv</param>
        /// <param name="moduleNo">モジュール番号</param>
        /// <param name="kind">種別</param>
        /// <returns>CsvDataにデータを格納</returns>
        private List<List<String>> CreateCsvDataInformationUser(List<List<String>> CsvData, int moduleNo, String kind, Oelco.Common.GUI.CustomGrid grdMaintenanceList)
        {
            List<MaintenanceJournalListData> maintenanceListDatas = new List<MaintenanceJournalListData>();

            // Kindを設定
            int cntFirst = 0;
            int cntLast = 0;
            // Daily
            if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_009)
            {
                cntFirst = this.dailyIndex.First() - 1;
                cntLast = this.dailyIndex.Last();
            }
            // Weekly
            else if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_010)
            {
                cntFirst = this.weeklyIndex.First() - 1;
                cntLast = this.weeklyIndex.Last();
            }
            // Monthly
            else if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_011)
            {
                cntFirst = this.monthlyIndex.First() - 1;
                cntLast = this.monthlyIndex.Last();
            }

            // kind毎にgridから取得するデータリストが違うため
            for (int i = cntFirst; i < cntLast; i++)
            {
                var listObjectgridMaintenanceList = (MaintenanceJournalListData)grdMaintenanceList.Rows[i].ListObject;
                maintenanceListDatas.Add(listObjectgridMaintenanceList);
            }

            // データ作成用
            List<String> maintenanceListDatasAdd = new List<String>();

            // 現在ログイン中のユーザー名取得
            String nowUserId = Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID;

            // Data作成
            // 項目作成
            for (int j = 0; j < maintenanceListDatas.Count + 2; j++)
            {
                // 要素追加
                if (j == 0)
                {
                    // ログインユーザー名を設定します
                    maintenanceListDatasAdd.Add(nowUserId);
                }
                else if (j == 1)
                {
                    // 現在の日付を設定します
                    maintenanceListDatasAdd.Add(DateTime.Now.ToString());
                }
                else if (j >= 2)
                {
                    // Module1
                    if (moduleNo == 0)
                    {
                        maintenanceListDatasAdd.Add(maintenanceListDatas[j - 2].module1.ToString());
                    }
                    // Module2
                    else if (moduleNo == 1)
                    {
                        maintenanceListDatasAdd.Add(maintenanceListDatas[j - 2].module2.ToString());
                    }
                    // Module3
                    else if (moduleNo == 2)
                    {
                        maintenanceListDatasAdd.Add(maintenanceListDatas[j - 2].module3.ToString());
                    }
                    // Module4
                    else
                    {
                        // 項目名
                        maintenanceListDatasAdd.Add(maintenanceListDatas[j - 2].module4.ToString());
                    }
                }


            }
            CsvData.Insert(CsvData.Count, maintenanceListDatasAdd);

            return CsvData;
        }


        ///
        /// <summary>
        /// CSV出力のデータ作成（サービスマン用）
        /// </summary>
        /// <remarks>
        /// CSV出力用データの今回の情報を作成します（縦横は逆）
        /// </remarks>
        /// <param name="CsvData">出力するcsv</param>
        /// <param name="moduleNo">モジュール番号</param>
        /// <param name="kind">種別</param>
        /// <returns>CsvDataにデータを格納</returns>
        private List<List<String>> CreateCsvDataInformationServiceman(List<List<String>> CsvData, int moduleNo, String kind)
        {
            List<MaintenanceJournalListData> maintenanceListDatas = new List<MaintenanceJournalListData>();

            // Kindを設定
            int cntCount = 0;

            // Monthly
            if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_011)
            {
                cntCount = this.monthlyIndex.Count();
            }
            // Yearly
            else if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_012)
            {
                cntCount = this.yearlyIndex.Count();
            }

            // データ作成用
            var maintenanceListDatasAdd = new List<String>();

            // 現在ログイン中のユーザー名取得
            String nowUserId = Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID;

            int k = 0;

            // Data作成
            // 項目作成
            for (int j = 0; j < cntCount + 2; j++)
            {
                // 要素追加
                if (j == 0)
                {
                    // ログインユーザー名を設定します
                    maintenanceListDatasAdd.Add(nowUserId);
                }
                else if (j == 1)
                {
                    // 現在の日付を設定します
                    maintenanceListDatasAdd.Add(DateTime.Now.ToString());
                }
                else if (j >= 2)
                {
                    // Monthly
                    if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_011)
                    {
                        k = this.monthlyIndex[j - 2] - 1;
                    }
                    // Yearly
                    else if (kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_012)
                    {
                        k = this.yearlyIndex[j - 2] - 1;
                    }

                    // Module1
                    if (moduleNo == 0)
                    {
                        maintenanceListDatasAdd.Add(this.maintenanceJournalListDatasServiceman[k].module1.ToString());
                    }
                    // Module2
                    else if (moduleNo == 1)
                    {
                        maintenanceListDatasAdd.Add(this.maintenanceJournalListDatasServiceman[k].module2.ToString());
                    }
                    // Module3
                    else if (moduleNo == 2)
                    {
                        maintenanceListDatasAdd.Add(this.maintenanceJournalListDatasServiceman[k].module3.ToString());
                    }
                    // Module4
                    else
                    {
                        // 項目名
                        maintenanceListDatasAdd.Add(this.maintenanceJournalListDatasServiceman[k].module4.ToString());
                    }
                }


            }
            CsvData.Insert(CsvData.Count, maintenanceListDatasAdd);

            return CsvData;
        }

        #endregion

        /// <summary>
        /// メンテナンス画面にてExitを押下した時,csv出力します
        /// </summary>
        /// <remarks>チェック状態を保存し,csvを出力します</remarks>
        /// <param name="grdMaintenanceJournalList">csvメソッドの引数</param>
        public void ServicemanExitExecute(Oelco.Common.GUI.CustomGrid grdMaintenanceJournalList)
        {
            try
            {
                ParameterFilePreserve<CarisXMaintenanceServicemanParameter> maintenanceJournalServicemanList = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance;
                // 全チェックかどうかのフラグを取得するためのインスタンス作成
                var AllCheckFlagCheck = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param;
                bool checkOffFlag = false;

                // 行
                // module1 ～ 4
                if (AllCheckFlagCheck.AllCheckMonthly == false)
                {
                    for (int j = 0; j < this.monthlyIndex.Count; j++)
                    {
                        int k = this.monthlyIndex[j] - 1;
                        // 画面の文字列で振り分ける
                        if (this.maintenanceJournalListDatasServiceman[k].module1 == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013)
                        {
                            maintenanceJournalServicemanList.Param.SlaveList[0].MonthlyCheckItem[j] = true;
                        }
                        else
                        {
                            maintenanceJournalServicemanList.Param.SlaveList[0].MonthlyCheckItem[j] = false;
                            checkOffFlag = true;
                        }

                        if (ModuleNumConnected >= 2)
                        {
                            if (this.maintenanceJournalListDatasServiceman[k].module2 == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013)
                            {
                                maintenanceJournalServicemanList.Param.SlaveList[1].MonthlyCheckItem[j] = true;
                            }
                            else
                            {
                                maintenanceJournalServicemanList.Param.SlaveList[1].MonthlyCheckItem[j] = false;
                                checkOffFlag = true;
                            }
                        }

                        if (ModuleNumConnected >= 3)
                        {
                            if (this.maintenanceJournalListDatasServiceman[k].module3 == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013)
                            {
                                maintenanceJournalServicemanList.Param.SlaveList[2].MonthlyCheckItem[j] = true;
                            }
                            else
                            {
                                maintenanceJournalServicemanList.Param.SlaveList[2].MonthlyCheckItem[j] = false;
                                checkOffFlag = true;
                            }
                        }

                        if (ModuleNumConnected >= 4)
                        {
                            if (this.maintenanceJournalListDatasServiceman[k].module4 == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013)
                            {
                                maintenanceJournalServicemanList.Param.SlaveList[3].MonthlyCheckItem[j] = true;
                            }
                            else
                            {
                                maintenanceJournalServicemanList.Param.SlaveList[3].MonthlyCheckItem[j] = false;
                                checkOffFlag = true;
                            }
                        }

                    }
                    // 全てチェック
                    if (checkOffFlag == false)
                    {
                        maintenanceJournalServicemanList.Param.AllCheckMonthly = true;
                        maintenanceJournalServicemanList.Param.AllFinishMonthly = DateTime.Now;
                    }
                    // フラグをリセット
                    checkOffFlag = false;
                }

                if (AllCheckFlagCheck.AllCheckYearly == false)
                {
                    for (int j = 0; j < this.yearlyIndex.Count; j++)
                    {
                        int k = this.yearlyIndex[j] - 1;
                        // 画面の文字列で振り分ける
                        if (this.maintenanceJournalListDatasServiceman[k].module1 == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013)
                        {
                            maintenanceJournalServicemanList.Param.SlaveList[0].YearlyCheckItem[j] = true;
                        }
                        else
                        {
                            maintenanceJournalServicemanList.Param.SlaveList[0].YearlyCheckItem[j] = false;
                            checkOffFlag = true;
                        }

                        if (ModuleNumConnected >= 2)
                        {
                            if (this.maintenanceJournalListDatasServiceman[k].module2 == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013)
                            {
                                maintenanceJournalServicemanList.Param.SlaveList[1].YearlyCheckItem[j] = true;
                            }
                            else
                            {
                                maintenanceJournalServicemanList.Param.SlaveList[1].YearlyCheckItem[j] = false;
                                checkOffFlag = true;
                            }
                        }

                        if (ModuleNumConnected >= 3)
                        {
                            if (this.maintenanceJournalListDatasServiceman[k].module3 == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013)
                            {
                                maintenanceJournalServicemanList.Param.SlaveList[2].YearlyCheckItem[j] = true;
                            }
                            else
                            {
                                maintenanceJournalServicemanList.Param.SlaveList[2].YearlyCheckItem[j] = false;
                                checkOffFlag = true;
                            }
                        }

                        if (ModuleNumConnected >= 4)
                        {
                            if (this.maintenanceJournalListDatasServiceman[k].module4 == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_013)
                            {
                                maintenanceJournalServicemanList.Param.SlaveList[3].YearlyCheckItem[j] = true;
                            }
                            else
                            {
                                maintenanceJournalServicemanList.Param.SlaveList[3].YearlyCheckItem[j] = false;
                                checkOffFlag = true;
                            }
                        }

                    }
                    // 全てチェック
                    if (checkOffFlag == false)
                    {
                        maintenanceJournalServicemanList.Param.AllCheckYearly = true;
                        maintenanceJournalServicemanList.Param.AllFinishYearly = DateTime.Now;
                    }
                    // フラグをリセット
                    checkOffFlag = false;
                }

                //xmlファイルへ保存する
                maintenanceJournalServicemanList.SaveRaw();

                // csvに保存する
                exportData(grdMaintenanceJournalList);

                // 画面遷移するので初期化
                this.maintenanceJournalListDatasServiceman = new List<MaintenanceJournalListData>();
            }
            catch (Exception ex)
            {
                // Exitボタンの処理に失敗しました。
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("Exit button processing failed :{0}", ex.Message));
            }
        }

        /// <summary>
        /// パラメータファイル初期化
        /// </summary>
        public void InitializeCheckParameterFile()
        {
            Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.LoadRaw();
            Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.LoadRaw();
            ParameterFilePreserve<CarisXMaintenanceUserParameter> maintenanceJournalUserList = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance;
            ParameterFilePreserve<CarisXMaintenanceServicemanParameter> maintenanceJournalServicemanList = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance;

            Boolean blChangeFlg = false;

            // ユーザー用メンテナンス日誌の場合
            if (this.mainteJournalType == MaintenanceJournalType.User)
            {

                // デイリー
                // 日付が前回日時より所定日時過ぎていた場合、falseにフラグを全て戻す
                if (maintenanceJournalUserList.Param.AllFinishDaily.AddDays(1) < DateTime.Now
                    && maintenanceJournalUserList.Param.AllCheckDaily == true)
                {
                    blChangeFlg = FlagChange(ref maintenanceJournalUserList, ref maintenanceJournalServicemanList, Kind.U_Daily);
                }

                // ウィークリー
                // 日付が前回日時より所定日時過ぎていた場合、falseにフラグを全て戻す
                if (maintenanceJournalUserList.Param.AllFinishWeekly.AddDays(7) < DateTime.Now
                    && maintenanceJournalUserList.Param.AllCheckWeekly == true)
                {
                    blChangeFlg = FlagChange(ref maintenanceJournalUserList, ref maintenanceJournalServicemanList, Kind.U_Weekly);
                }

                // マンスリー
                // 日付が前回日時より所定日時過ぎていた場合、falseにフラグを全て戻す
                if (maintenanceJournalUserList.Param.AllFinishMonthly.AddMonths(1) < DateTime.Now
                    && maintenanceJournalUserList.Param.AllCheckMonthly == true)
                {
                    blChangeFlg = FlagChange(ref maintenanceJournalUserList, ref maintenanceJournalServicemanList, Kind.U_Monthly);
                }

                if (blChangeFlg == true)
                {
                    //xmlファイルへ保存する
                    maintenanceJournalUserList.SaveRaw();
                }
            }
            // サービスマン用メンテナンス日誌の場合
            else if (this.mainteJournalType == MaintenanceJournalType.Serviceman)
            {
                // マンスリー
                // 日付が前回日時より所定日時過ぎていた場合、falseにフラグを全て戻す
                if (maintenanceJournalServicemanList.Param.AllFinishMonthly.AddMonths(1) < DateTime.Now
                    && maintenanceJournalServicemanList.Param.AllCheckMonthly == true)
                {
                    blChangeFlg = FlagChange(ref maintenanceJournalUserList, ref maintenanceJournalServicemanList, Kind.S_Monthly);
                }

                // イヤーリー
                // 日付が前回日時より所定日時過ぎていた場合、falseにフラグを全て戻す
                if (maintenanceJournalServicemanList.Param.AllFinishYearly.AddYears(1) < DateTime.Now
                    && maintenanceJournalServicemanList.Param.AllCheckYearly == true)
                {
                    blChangeFlg = FlagChange(ref maintenanceJournalUserList, ref maintenanceJournalServicemanList, Kind.S_Yearly);
                }

                if (blChangeFlg == true)
                {
                    //xmlファイルへ保存する
                    maintenanceJournalServicemanList.SaveRaw();
                }
            }
        }

        /// <summary>
        /// パラメータファイル種別ごとに初期化
        /// </summary>
        /// <param name="maintenanceJournalUserList">ユーザーデータ</param>
        /// <param name="maintenanceJournalServicemanList">サービスマンデータ</param>
        /// <param name="kind">種別</param>
        /// <returns>true : 変更有り</returns>
        public Boolean FlagChange(ref ParameterFilePreserve<CarisXMaintenanceUserParameter> maintenanceJournalUserList,
            ref ParameterFilePreserve<CarisXMaintenanceServicemanParameter> maintenanceJournalServicemanList, Kind kind)
        {
            Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.LoadRaw();
            
            this.listDailyCheckItemUser = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param.SlaveList[0].DailyCheckItem;
            this.listWeeklyCheckItemUser = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param.SlaveList[0].WeeklyCheckItem;
            this.listMonthlyCheckItemUser = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param.SlaveList[0].MonthlyCheckItem;
            this.listMonthlyCheckItemServiceman = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param.SlaveList[0].MonthlyCheckItem;
            this.listYearlyCheckItemServiceman = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param.SlaveList[0].YearlyCheckItem;

            List<Boolean> listCheckItem = new List<bool>();
            switch (kind)
            {
                case Kind.U_Daily:
                    listCheckItem = listDailyCheckItemUser;
                    maintenanceJournalUserList.Param.AllCheckDaily = false;
                    break;
                case Kind.U_Weekly:
                    listCheckItem = listWeeklyCheckItemUser;
                    maintenanceJournalUserList.Param.AllCheckWeekly = false;
                    break;
                case Kind.U_Monthly:
                    listCheckItem = listMonthlyCheckItemUser;
                    maintenanceJournalUserList.Param.AllCheckMonthly = false;
                    break;
                case Kind.S_Monthly:
                    listCheckItem = listMonthlyCheckItemServiceman;
                    maintenanceJournalServicemanList.Param.AllCheckMonthly = false;
                    break;
                case Kind.S_Yearly:
                    listCheckItem = listYearlyCheckItemServiceman;
                    maintenanceJournalServicemanList.Param.AllCheckYearly = false;
                    break;
            }

            // falseに全て戻す
            for (int j = 0; j < ModuleNumConnected; j++)
            {
                for (int i = 0; i < listCheckItem.Count; i++)
                {
                    switch (kind)
                    {
                        case Kind.U_Daily:
                            maintenanceJournalUserList.Param.SlaveList[j].DailyCheckItem[i] = false;
                            break;
                        case Kind.U_Weekly:
                            maintenanceJournalUserList.Param.SlaveList[j].WeeklyCheckItem[i] = false;
                            break;
                        case Kind.U_Monthly:
                            maintenanceJournalUserList.Param.SlaveList[j].MonthlyCheckItem[i] = false;
                            break;
                        case Kind.S_Monthly:
                            maintenanceJournalServicemanList.Param.SlaveList[j].MonthlyCheckItem[i] = false;
                            break;
                        case Kind.S_Yearly:
                            maintenanceJournalServicemanList.Param.SlaveList[j].YearlyCheckItem[i] = false;
                            break;
                    }

                }
            }

            return true;
        }

        /// <summary>
        /// 種別ごとにパラメータファイルのパラメータをfalseへ変更
        /// </summary>
        /// パラメータファイル種別ごとに初期化
        /// </summary>
        /// <param name="kind">種別</param>
        /// <returns>true : 変更有り</returns>
        public Boolean FlagChange(Kind kind)
        {
            Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.LoadRaw();
            ParameterFilePreserve<CarisXMaintenanceUserParameter> maintenanceJournalUserList = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance;
            ParameterFilePreserve<CarisXMaintenanceServicemanParameter> maintenanceJournalServicemanList = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance;

            if (this.mainteJournalType == MaintenanceJournalType.User)
            {
                this.listDailyCheckItemUser = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param.SlaveList[0].DailyCheckItem;
                this.listWeeklyCheckItemUser = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param.SlaveList[0].WeeklyCheckItem;
                this.listMonthlyCheckItemUser = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param.SlaveList[0].MonthlyCheckItem;
            }
            else if (this.mainteJournalType == MaintenanceJournalType.Serviceman)
            {
                this.listMonthlyCheckItemServiceman = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param.SlaveList[0].MonthlyCheckItem;
                this.listYearlyCheckItemServiceman = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param.SlaveList[0].YearlyCheckItem;
            }



            List<Boolean> listCheckItem = new List<bool>();
            switch (kind)
            {
                case Kind.U_Daily:
                    listCheckItem = listDailyCheckItemUser;
                    maintenanceJournalUserList.Param.AllCheckDaily = false;
                    break;
                case Kind.U_Weekly:
                    listCheckItem = listWeeklyCheckItemUser;
                    maintenanceJournalUserList.Param.AllCheckWeekly = false;
                    break;
                case Kind.U_Monthly:
                    listCheckItem = listMonthlyCheckItemUser;
                    maintenanceJournalUserList.Param.AllCheckMonthly = false;
                    break;
                case Kind.S_Monthly:
                    listCheckItem = listMonthlyCheckItemServiceman;
                    maintenanceJournalServicemanList.Param.AllCheckMonthly = false;
                    break;
                case Kind.S_Yearly:
                    listCheckItem = listYearlyCheckItemServiceman;
                    maintenanceJournalServicemanList.Param.AllCheckYearly = false;
                    break;
            }

            // falseに全て戻す
            for (int j = 0; j < ModuleNumConnected; j++)
            {
                for (int i = 0; i < listCheckItem.Count; i++)
                {
                    switch (kind)
                    {
                        case Kind.U_Daily:
                            maintenanceJournalUserList.Param.SlaveList[j].DailyCheckItem[i] = false;
                            break;
                        case Kind.U_Weekly:
                            maintenanceJournalUserList.Param.SlaveList[j].WeeklyCheckItem[i] = false;
                            break;
                        case Kind.U_Monthly:
                            maintenanceJournalUserList.Param.SlaveList[j].MonthlyCheckItem[i] = false;
                            break;
                        case Kind.S_Monthly:
                            maintenanceJournalServicemanList.Param.SlaveList[j].MonthlyCheckItem[i] = false;
                            break;
                        case Kind.S_Yearly:
                            maintenanceJournalServicemanList.Param.SlaveList[j].YearlyCheckItem[i] = false;
                            break;
                    }

                }
            }

            if (this.mainteJournalType == MaintenanceJournalType.User)
            {
                //xmlファイルへ保存する
                maintenanceJournalUserList.SaveRaw();
            }
            else if (this.mainteJournalType == MaintenanceJournalType.Serviceman)
            {
                //xmlファイルへ保存する
                maintenanceJournalServicemanList.SaveRaw();
            }

            return true;

        }


        #endregion
    }
}