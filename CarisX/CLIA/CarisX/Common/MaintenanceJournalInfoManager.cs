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
    /// �����e�i���X�����ꗗ��ʏ��\���f�[�^�N���X
    /// </summary>
    /// <remarks>
    /// ���[�U�[�A�T�[�r�X�}���̃����e�i���X�����ꗗ��ʂ̏��\���p�f�[�^�N���X�ł��B
    /// </remarks>
    public class MaintenanceJournalListData
    {
        #region [�R���X�g���N�^/�f�X�g���N�^]

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="No">�s�ԍ�</param>
        /// <param name="Kind">���</param>
        /// <param name="module">�ڑ��䐔���̃��W���[��</param>
        /// <param name="checkItem">�����e�i���X�������ږ�</param>
        /// <param name="module1">���W���[��1</param>
        /// <param name="maintenanceJournalNo">�����e�i���X�����ԍ�</param>
        /// <param name="unit">���j�b�g�ԍ�</param>
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
        /// �s�ԍ�
        /// </summary>
        public Int32 No
        {
            get;
            set;
        }

        /// <summary>
        /// �����e�i���X�������ږ�
        /// </summary>
        public String checkItem
        {
            get;
            set;
        }

        /// <summary>
        /// �����e�i���X�������ڎ��
        /// </summary>
        public String kind
        {
            get;
            set;
        }

        /// <summary>
        /// ���W���[��1
        /// </summary>
        public String module1
        {
            get;
            set;
        }

        /// <summary>
        /// ���W���[��2
        /// </summary>
        public String module2
        {
            get;
            set;
        }

        /// <summary>
        /// ���W���[��3
        /// </summary>
        public String module3
        {
            get;
            set;
        }

        /// <summary>
        /// ���W���[��4
        /// </summary>
        public String module4
        {

            get;
            set;
        }

        /// <summary>
        /// �����e�i���X�����ԍ�
        /// </summary>
        public String maintenanceJournalNo
        {
            get;
            set;
        }

        /// <summary>
        /// �����e�i���X�������j�b�g�ԍ�
        /// </summary>
        public String unitNo
        {
            get;
            set;
        }

        #endregion
    }


    /// <summary>
    /// �����e�i���X�������Ǘ�
    /// </summary>
    public class MaintenanceJournalInfoManager
    {
        #region [�萔��`]

        /// <summary>
        /// �s�ԍ���L�[
        /// </summary>
        private const String STRING_CHECKNO = "No";
        /// <summary>
        /// �����e�i���X�����`�F�b�N���ږ���L�[
        /// </summary>
        private const String STRING_CHECKITEM = "CheckItem";
        /// <summary>
        /// �����e�i���X�������ڎ�ʗ�L�[
        /// </summary>
        private const String STRING_CHECKKIND = "Kind";
        /// <summary>
        /// Module1��L�[
        /// </summary>
        private const String STRING_CHECKMODULE1 = "Module1";
        /// <summary>
        /// Module2��L�[
        /// </summary>
        private const String STRING_CHECKMODULE2 = "Module2";
        /// <summary>
        /// Module3��L�[
        /// </summary>
        private const String STRING_CHECKMODULE3 = "Module3";
        /// <summary>
        /// Module4��L�[
        /// </summary>
        private const String STRING_CHECKMODULE4 = "Module4";
        /// <summary>
        /// �����e�i���X�����`�F�b�N�ԍ���L�[
        /// </summary>
        private const String STRING_MAINTENANCEJOURNALNO = "MaintenanceJournalNo";
        /// <summary>
        /// �����e�i���X�������j�b�g�ԍ�
        /// </summary>
        private const String STRING_UNITNO = "UnitNo";
        /// <summary>
        /// csv�o�͍��ږ��@Module�ԍ�
        /// </summary>
        private const String STRING_CSV_MODULE_NO = "";
        /// <summary>
        /// csv�o�͍��ږ��@�R�[�h
        /// </summary>
        private const String STRING_CSV_CODE = "Code";
        /// <summary>
        /// csv�o�͍��ږ��@���[�U�[��
        /// </summary>
        private const String STRING_CSV_USER_NAME = "User Name";
        /// <summary>
        /// csv�o�͍��ږ��@���ږ�
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
        ///  �����e�i���X�������
        /// </summary>
        private MaintenanceJournalType mainteJournalType = MaintenanceJournalType.User;
        /// <summary>
        /// ���W���[���ڑ��䐔
        /// </summary>
        private Int32 ModuleNumConnected = 0;

        /// <summary>
        /// ��؂蕶��
        /// </summary>
        protected String separator = String.Empty;

        /// <summary>
        /// �G���R�[�h
        /// </summary>
        protected Encoding enc;

        #endregion

        #region [�N���X�ϐ���`]
        List<Boolean> listDailyCheckItemUser = new List<bool>();
        List<Boolean> listWeeklyCheckItemUser = new List<bool>();
        List<Boolean> listMonthlyCheckItemUser = new List<bool>();
        List<Boolean> listMonthlyCheckItemServiceman = new List<bool>();
        List<Boolean> listYearlyCheckItemServiceman = new List<bool>();
        #endregion

        #region [�C���X�^���X�ϐ���`]

        /// <summary>
        /// daily�̍s
        /// </summary>
        private List<Int32> dailyIndex = new List<Int32>();

        /// <summary>
        /// weekly�̍s
        /// </summary>
        private List<Int32> weeklyIndex = new List<Int32>();

        /// <summary>
        /// Monthly�̍s
        /// </summary>
        private List<Int32> monthlyIndex = new List<Int32>();

        /// <summary>
        /// Yearly�̍s
        /// </summary>
        private List<Int32> yearlyIndex = new List<Int32>();

        /// <summary>
        /// ���[�U�[�p�O���b�h�f�[�^
        /// </summary>
        List<MaintenanceJournalListData> maintenanceJournalListDatasUser = new List<MaintenanceJournalListData>();

        /// <summary>
        /// �T�[�r�X�}���p�O���b�h�f�[�^
        /// </summary>
        List<MaintenanceJournalListData> maintenanceJournalListDatasServiceman = new List<MaintenanceJournalListData>();

        #endregion

        #region [�v���p�e�B]

        #endregion

        #region [public���\�b�h]

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
        /// �����e�i���X������ʊJ�����`�F�b�N
        /// </summary>
        /// <returns></returns>
        public Boolean IsShow()
        {
            try
            {
                // �����e�i���X�����i���[�U�[�p�j�̃`�F�b�N�t���O���m�F
                CarisXMaintenanceUserParameter AllCheckUserFlag = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param;
                CarisXMaintenanceServicemanParameter AllCheckServicemanFlag = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param;

                List<MaintenanceJournalListData> MaintenanceJournalInfo = Singleton<MaintenanceJournalInfoManager>.Instance.ISshowMaintenaceListData();
                // ���b�Z�[�W�t�@�C���̐ݒ�
                MaintenanceJournalCodeDataManager messageList = Singleton<ParameterFilePreserve<MaintenanceJournalCodeDataManager>>.Instance.Param;

                // ��ʂ��Ƃ̃��b�Z�[�W���X�g
                List<List<MaintenanceJournalCodeData>> codeList = new List<List<MaintenanceJournalCodeData>>();
                codeList = CodeListEachKind();
                int messageCountUser = codeList[0].Count + codeList[1].Count + codeList[2].Count;
                int messageCountServiceman = codeList[3].Count + codeList[4].Count;

                // ���[�U�[���A�p�����[�^�t�@�C���擾���s
                if (AllCheckUserFlag.SlaveList == null
                && this.mainteJournalType == MaintenanceJournalType.User)
                {
                    // �����e�i���X������ʂ̃��[�h�Ɏ��s���܂����B�p�����[�^�t�@�C�����m�F���Ă��������B
                    Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_016));
                    return false;
                }
                // �U�[�r�X�}�����A�p�����[�^�t�@�C���擾���s
                else if (AllCheckServicemanFlag.SlaveList == null
                    && this.mainteJournalType == MaintenanceJournalType.Serviceman)
                {
                    // �����e�i���X������ʂ̃��[�h�Ɏ��s���܂����B�p�����[�^�t�@�C�����m�F���Ă��������B
                    Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_016));
                    return false;
                }
                // ���b�Z�[�W�t�@�C���擾���s
                else if (messageList.CodeDataList.Count == 0)
                {
                    // �����e�i���X������ʂ̃��[�h�Ɏ��s���܂����B���b�Z�[�W�t�@�C�����m�F���Ă��������B
                    Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_017));
                    return false;
                }
                // �p�����[�^�t�@�C���ƃ��b�Z�[�W�t�@�C���̐����������Ă��Ȃ�
                else if (MaintenanceJournalInfo.Count == 0)
                {
                    // �����e�i���X������ʂ̃��[�h�Ɏ��s���܂����B���b�Z�[�W�t�@�C�����p�����[�^�t�@�C�����m�F���Ă��������B
                    Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_018));
                    return false;
                }
                // �p�����[�^�t�@�C���ƃ��b�Z�[�W�t�@�C���̃f�[�^���������v���Ȃ�
                else if ((this.mainteJournalType == MaintenanceJournalType.User
                    && messageCountUser != MaintenanceJournalInfo.Count)
                    || (this.mainteJournalType == MaintenanceJournalType.Serviceman
                    && messageCountServiceman != MaintenanceJournalInfo.Count))
                {
                    // �����e�i���X������ʂ̃��[�h�Ɏ��s���܂����B���b�Z�[�W�t�@�C�����p�����[�^�t�@�C�����m�F���Ă��������B
                    Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_018));
                    return false;
                }

                // �t�@�C�����擾�ł��Ă���ꍇ
                if (this.mainteJournalType == MaintenanceJournalType.User)
                {

                    // �����e�i���X�����؂�̃`�F�b�N
                    DateTime nowDateTime = DateTime.Now;
                    if ((AllCheckUserFlag.AllFinishDaily.AddDays(1) < nowDateTime)
                      || (AllCheckUserFlag.AllFinishWeekly.AddDays(7) < nowDateTime)
                      || (AllCheckUserFlag.AllFinishMonthly.AddMonths(1) < nowDateTime))
                    {
                        // �����e�i���X�����i���[�U�[�p�j��\��
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
                    // �����e�i���X�����؂�̃`�F�b�N
                    DateTime nowDateTime = DateTime.Now;
                    if ((AllCheckServicemanFlag.AllFinishMonthly.AddMonths(1) < nowDateTime)
                      || (AllCheckServicemanFlag.AllFinishYearly.AddYears(1) < nowDateTime))
                    {
                        // �����e�i���X�����i�T�[�r�X�}���p�j��\��
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
                // �R���\�[���ɗ�O�o��
                System.Diagnostics.Debug.WriteLine(String.Format("{0} {1} {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace));

                // �����e�i���X������ʂ̃��[�h�Ɏ��s���܂����B���b�Z�[�W�t�@�C�����p�����[�^�t�@�C�����m�F���Ă��������B
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_018));
                return false;
            }
        }

        /// <summary>
        /// �����e�i���X�����Z���N�g��ʊJ�����`�F�b�N
        /// </summary>
        /// <returns></returns>
        public Boolean IsShowSelect()
        {
            // �����e�i���X�����i���[�U�[�p�j�̃`�F�b�N�t���O���m�F
            CarisXMaintenanceUserParameter AllCheckUserFlag = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param;

            // �����e�i���X�����i�T�[�r�X�}���p�j�̃`�F�b�N�t���O���m�F
            CarisXMaintenanceServicemanParameter AllCheckServicemanFlag = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param;

            List<Common.MaintenanceJournalListData> MaintenanceJournalInfo = Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.ISshowMaintenaceListData();
            // ���b�Z�[�W�t�@�C���̐ݒ�
            Oelco.CarisX.Parameter.MaintenanceJournalCodeData.MaintenanceJournalCodeDataManager messageList = Singleton<ParameterFilePreserve<Oelco.CarisX.Parameter.MaintenanceJournalCodeData.MaintenanceJournalCodeDataManager>>.Instance.Param;

            // ��ʂ��Ƃ̃��b�Z�[�W���X�g
            List<List<MaintenanceJournalCodeData>> codeList = new List<List<MaintenanceJournalCodeData>>();
            codeList = CodeListEachKind();
            int messageCountUser = codeList[0].Count + codeList[1].Count + codeList[2].Count;
            int messageCountServiceman = codeList[3].Count + codeList[4].Count;

            // ���[�U�[���A�p�����[�^�t�@�C���擾���s
            if (AllCheckUserFlag.SlaveList == null
                && this.mainteJournalType == MaintenanceJournalType.User)
            {
                // �����e�i���X������ʂ̃��[�h�Ɏ��s���܂����B�p�����[�^�t�@�C�����m�F���Ă��������B
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_016));
                return false;
            }
            // ���b�Z�[�W�t�@�C���擾���s
            else if (messageList.CodeDataList.Count == 0)
            {
                // �����e�i���X������ʂ̃��[�h�Ɏ��s���܂����B���b�Z�[�W�t�@�C�����m�F���Ă��������B
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_017));
                return false;
            }
            // �p�����[�^�t�@�C���ƃ��b�Z�[�W�t�@�C���̐����������Ă��Ȃ�
            else if (MaintenanceJournalInfo.Count == 0)
            {
                // �����e�i���X������ʂ̃��[�h�Ɏ��s���܂����B���b�Z�[�W�t�@�C�����p�����[�^�t�@�C�����m�F���Ă��������B
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_018));
                return false;
            }
            // �p�����[�^�t�@�C���ƃ��b�Z�[�W�t�@�C���̃f�[�^���������v���Ȃ�
            else if ((this.mainteJournalType == MaintenanceJournalType.User
                && messageCountUser != MaintenanceJournalInfo.Count)
                || (this.mainteJournalType == MaintenanceJournalType.Serviceman
                && messageCountServiceman != MaintenanceJournalInfo.Count))
            {
                // �����e�i���X������ʂ̃��[�h�Ɏ��s���܂����B���b�Z�[�W�t�@�C�����p�����[�^�t�@�C�����m�F���Ă��������B
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_018));
                return false;
            }
            // �t�@�C�����擾�ł��Ă���ꍇ
            else
            {
                // �T�[�r�X�}���̏ꍇ�A�ȉ������K�v
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
        /// �����e�i���X�����ۑ����邩�`�F�b�N
        /// </summary>
        /// <returns></returns>
        public Boolean IsSave()
        {
            // �����e�i���X�����i���[�U�[�p�j�̃`�F�b�N�t���O���m�F
            CarisXMaintenanceUserParameter AllCheckUserFlag = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param;
            CarisXMaintenanceServicemanParameter AllCheckServicemanFlag = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param;

            List<Common.MaintenanceJournalListData> MaintenanceJournalInfo = Singleton<Oelco.CarisX.Common.MaintenanceJournalInfoManager>.Instance.ISshowMaintenaceListData();
            // ���b�Z�[�W�t�@�C���̐ݒ�
            Oelco.CarisX.Parameter.MaintenanceJournalCodeData.MaintenanceJournalCodeDataManager messageList = Singleton<ParameterFilePreserve<Oelco.CarisX.Parameter.MaintenanceJournalCodeData.MaintenanceJournalCodeDataManager>>.Instance.Param;

            // ��ʂ��Ƃ̃��b�Z�[�W���X�g
            List<List<MaintenanceJournalCodeData>> codeList = new List<List<MaintenanceJournalCodeData>>();
            codeList = CodeListEachKind();
            int messageCountUser = codeList[0].Count + codeList[1].Count + codeList[2].Count;
            int messageCountServiceman = codeList[3].Count + codeList[4].Count;

            // ���[�U�[���A�p�����[�^�t�@�C���擾���s
            if (AllCheckUserFlag.SlaveList == null
                && this.mainteJournalType == MaintenanceJournalType.User)
            {
                // �����e�i���X������ʂ̃��[�h�Ɏ��s���܂����B�p�����[�^�t�@�C�����m�F���Ă��������B
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_016));
                return false;
            }
            // �U�[�r�X�}�����A�p�����[�^�t�@�C���擾���s
            else if (AllCheckServicemanFlag.SlaveList == null
                && this.mainteJournalType == MaintenanceJournalType.Serviceman)
            {
                // �����e�i���X������ʂ̃��[�h�Ɏ��s���܂����B�p�����[�^�t�@�C�����m�F���Ă��������B
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_016));
                return false;
            }
            // ���b�Z�[�W�t�@�C���擾���s
            else if (messageList.CodeDataList.Count == 0)
            {
                // �����e�i���X������ʂ̃��[�h�Ɏ��s���܂����B���b�Z�[�W�t�@�C�����m�F���Ă��������B
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_017));
                return false;
            }
            // �p�����[�^�t�@�C���ƃ��b�Z�[�W�t�@�C���̐����������Ă��Ȃ�
            else if (MaintenanceJournalInfo.Count == 0)
            {
                // �����e�i���X������ʂ̃��[�h�Ɏ��s���܂����B���b�Z�[�W�t�@�C�����p�����[�^�t�@�C�����m�F���Ă��������B
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_018));
                return false;
            }
            // �p�����[�^�t�@�C���ƃ��b�Z�[�W�t�@�C���̃f�[�^���������v���Ȃ�
            else if ((this.mainteJournalType == MaintenanceJournalType.User
                && messageCountUser != MaintenanceJournalInfo.Count)
                || (this.mainteJournalType == MaintenanceJournalType.Serviceman
                && messageCountServiceman != MaintenanceJournalInfo.Count))
            {
                // �����e�i���X������ʂ̃��[�h�Ɏ��s���܂����B���b�Z�[�W�t�@�C�����p�����[�^�t�@�C�����m�F���Ă��������B
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format(Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_018));
                return false;
            }

            // �t�@�C�����擾�ł��Ă���ꍇ
            if (this.mainteJournalType == MaintenanceJournalType.User)
            {

                // �����e�i���X�����؂�̃`�F�b�N
                DateTime nowDateTime = DateTime.Now;
                if ((AllCheckUserFlag.AllFinishDaily.AddDays(1) < nowDateTime)
                  || (AllCheckUserFlag.AllFinishWeekly.AddDays(7) < nowDateTime)
                  || (AllCheckUserFlag.AllFinishMonthly.AddMonths(1) < nowDateTime))
                {
                    // �����e�i���X�����i���[�U�[�p�j��\��
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
                // �����e�i���X�����؂�̃`�F�b�N
                DateTime nowDateTime = DateTime.Now;
                if ((AllCheckServicemanFlag.AllFinishMonthly.AddDays(1) < nowDateTime)
                  || (AllCheckServicemanFlag.AllFinishYearly.AddDays(1) < nowDateTime))
                {
                    // �����e�i���X�����i�T�[�r�X�}���p�j��\��
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// �����e�i���X�����f�[�^���擾���܂�
        /// </summary>
        /// <returns></returns>
        public List<MaintenanceJournalListData> GetMaintenanceJournalListDatas()
        {
            List<MaintenanceJournalListData> maintenanceJournalDatas = new List<MaintenanceJournalListData>();
            // ���W���[���ڑ��䐔
            ModuleNumConnected = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

            try
            {
                // �p�����[�^�t�@�C���������`�F�b�N
                InitializeCheckParameterFile();

                // ��ʏ����X�V���܂�
                //// user2��ڊJ���ꍇ
                //if (this.maintenanceJournalListDatasUser.Count > 0
                //    && this.mainteJournalType == MaintenanceJournalType.User)
                //{
                //    maintenanceJournalDatas = this.maintenanceJournalListDatasUser;
                //}
                // serviceman2��ڊJ���ꍇ
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
                // ��ʂ̃��[�h�Ɏ��s���܂����B
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("Screen loading failed :{0}", ex.TargetSite));
                return maintenanceJournalDatas;
            }
        }

        /// <summary>
        /// ��ʂ��Ƃ̃C���f�b�N�X��ݒ�
        /// </summary>
        /// <param name="maintenanceJournalType">�^�C�v���</param>
        public void CreateIndex(MaintenanceJournalType maintenanceJournalType)
        {
            // �O���b�h�̃��[�h
            LoadMaintenanceJournalList(maintenanceJournalType);

            string strCol = string.Empty;
            int iNo = 0;

            // �C���f�b�N�X�N���A
            this.dailyIndex.Clear();
            this.weeklyIndex.Clear();
            this.monthlyIndex.Clear();
            this.yearlyIndex.Clear();

            // �O���b�h�쐬��Ɏg�p���܂�
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
        /// IsShow�p�O���b�h�^���X�g
        /// </summary>
        /// <returns></returns>
        public List<MaintenanceJournalListData> ISshowMaintenaceListData()
        {
            // ���W���[���ڑ��䐔
            ModuleNumConnected = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

            // xml�t�@�C�����擾
            Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.LoadRaw();

            // �����e���i���X�����ԍ��A�����e�i���X���ږ����擾
            Singleton<ParameterFilePreserve<MaintenanceJournalCodeDataManager>>.Instance.LoadRaw();

            List<MaintenanceJournalListData> maintenanceJournalList = new List<MaintenanceJournalListData>();

            try
            {
                // ��ʂ��Ƃ̃��b�Z�[�W���X�g
                List<List<MaintenanceJournalCodeData>> codeList = new List<List<MaintenanceJournalCodeData>>();
                codeList = CodeListEachKind();

                int iTargetCodeList = 0;


                // ���[�U�[�p�����e�i���X�����̏ꍇ
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
                // �T�[�r�X�}���p�����e�i���X�����̏ꍇ
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


        #region [private���\�b�h]

        /// <summary>
        /// �����e���i���X�����f�[�^���X�g�쐬
        /// </summary>
        /// <remarks>
        /// �����̎�ʂ̃����e���i���X�����f�[�^���X�g���쐬���܂��B
        /// </remarks>
        /// <param name="maintenanceJournalDatas">�i�[����f�[�^���X�g</param>
        /// <param name="maintenanceJournalType">���</param>
        private void LoadMaintenanceJournalListData(ref List<MaintenanceJournalListData> maintenanceJournalDatas, MaintenanceJournalType maintenanceJournalType)
        {
            // xml�t�@�C�����擾
            Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.LoadRaw();

            // �����e���i���X�����ԍ��A�����e�i���X���ږ����擾
            Singleton<ParameterFilePreserve<MaintenanceJournalCodeDataManager>>.Instance.LoadRaw();

            // ��ʂ��Ƃ̃��b�Z�[�W���X�g
            List<List<MaintenanceJournalCodeData>> codeList = new List<List<MaintenanceJournalCodeData>>();
            codeList = CodeListEachKind();

            int iTargetCodeList = 0;

            List<MaintenanceJournalListData> maintenanceJournalList = new List<MaintenanceJournalListData>();

            // ���[�U�[�p�����e�i���X�����̏ꍇ
            if (maintenanceJournalType == MaintenanceJournalType.User)
            {
                CarisXMaintenanceUserParameter AllCheckFlagCheck = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param;

                // �f�C���[
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

                // �E�B�[�N���[
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

                // �}���X���[
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
            // �T�[�r�X�}���p�����e�i���X�����̏ꍇ
            else if (maintenanceJournalType == MaintenanceJournalType.Serviceman)
            {
                CarisXMaintenanceServicemanParameter AllCheckFlagCheck = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param;

                // �}���X���[
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

                // �C���[���[
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

            // �ԍ����ɕ��ѕς��܂�
            var x = from i in maintenanceJournalList
                    orderby int.Parse(i.maintenanceJournalNo)
                    select i;
            maintenanceJournalList = x.ToList();

            // grid��No��������ƂȂ�悤�ɕύX���܂�
            for (int i = 0; i < maintenanceJournalList.Count; i++)
            {
                maintenanceJournalList[i].No = i + 1;
            }

            maintenanceJournalDatas = maintenanceJournalList;
        }


        /// <summary>
        /// �����e���i���X�����f�[�^���X�g�쐬
        /// </summary>
        /// <remarks>
        /// �����̎�ʂ̃����e���i���X�����f�[�^���X�g���쐬���܂��B
        /// </remarks>
        /// <param name="maintenanceJournalDatas">�i�[����f�[�^���X�g</param>
        /// <param name="maintenanceJournalType">���</param>
        private void newLoadMaintenanceJournalListData(Oelco.Common.GUI.CustomGrid grdMaintenanceList, MaintenanceJournalType maintenanceJournalType)
        {
            // ��ʂ��Ƃ̃��b�Z�[�W���X�g
            List<List<MaintenanceJournalCodeData>> codeList = new List<List<MaintenanceJournalCodeData>>();
            codeList = CodeListEachKind();

            List<MaintenanceJournalListData> maintenanceJournalList = new List<MaintenanceJournalListData>();

            // ���[�U�[�p�����e�i���X�����̏ꍇ
            if (maintenanceJournalType == MaintenanceJournalType.User)
            {
                CarisXMaintenanceUserParameter AllCheckFlagCheck = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param;

                // �f�C���[
                if (AllCheckFlagCheck.AllCheckDaily == false)
                {
                    // kind����grid����擾����f�[�^���X�g���Ⴄ����
                    for (int i = 0; i < this.dailyIndex.Count; i++)
                    {
                        // row�ƈ�����1�����
                        int iIndex = this.dailyIndex[i] - 1;
                        var listObjectgridMaintenanceList = (MaintenanceJournalListData)grdMaintenanceList.Rows[iIndex].ListObject;
                        maintenanceJournalList.Add(listObjectgridMaintenanceList);
                    }
                }

                // �E�B�[�N���[
                if (AllCheckFlagCheck.AllCheckWeekly == false)
                {
                    // kind����grid����擾����f�[�^���X�g���Ⴄ����
                    for (int i = 0; i < this.weeklyIndex.Count; i++)
                    {
                        // row�ƈ�����1�����
                        int iIndex = this.weeklyIndex[i] - 1;
                        var listObjectgridMaintenanceList = (MaintenanceJournalListData)grdMaintenanceList.Rows[iIndex].ListObject;
                        maintenanceJournalList.Add(listObjectgridMaintenanceList);
                    }
                }

                // �}���X���[
                if (AllCheckFlagCheck.AllCheckMonthly == false)
                {
                    // kind����grid����擾����f�[�^���X�g���Ⴄ����
                    for (int i = 0; i < this.monthlyIndex.Count; i++)
                    {
                        // row�ƈ�����1�����
                        int iIndex = this.monthlyIndex[i] - 1;
                        var listObjectgridMaintenanceList = (MaintenanceJournalListData)grdMaintenanceList.Rows[iIndex].ListObject;
                        maintenanceJournalList.Add(listObjectgridMaintenanceList);
                    }
                }

                // �ԍ����ɕ��ѕς��܂�
                var x = from i in maintenanceJournalList
                        orderby int.Parse(i.maintenanceJournalNo)
                        select i;
                maintenanceJournalList = x.ToList();

                // grid��No��������ƂȂ�悤�ɕύX���܂�
                for (int i = 0; i < maintenanceJournalList.Count; i++)
                {
                    maintenanceJournalList[i].No = i + 1;
                }

                this.maintenanceJournalListDatasUser = maintenanceJournalList;
            }
            // �T�[�r�X�}���p�����e�i���X�����̏ꍇ
            else if (maintenanceJournalType == MaintenanceJournalType.Serviceman)
            {
                CarisXMaintenanceServicemanParameter AllCheckFlagCheck = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param;

                // �}���X���[
                if (AllCheckFlagCheck.AllCheckMonthly == false)
                {
                    // kind����grid����擾����f�[�^���X�g���Ⴄ����
                    for (int i = 0; i < this.monthlyIndex.Count; i++)
                    {
                        // row�ƈ�����1�����
                        int iIndex = this.monthlyIndex[i] - 1;
                        var listObjectgridMaintenanceList = (MaintenanceJournalListData)grdMaintenanceList.Rows[iIndex].ListObject;
                        maintenanceJournalList.Add(listObjectgridMaintenanceList);
                    }
                }

                // �C���[���[
                if (AllCheckFlagCheck.AllCheckYearly == false)
                {

                    // kind����grid����擾����f�[�^���X�g���Ⴄ����
                    for (int i = 0; i < this.yearlyIndex.Count; i++)
                    {
                        // row�ƈ�����1�����
                        int iIndex = this.yearlyIndex[i] - 1;
                        var listObjectgridMaintenanceList = (MaintenanceJournalListData)grdMaintenanceList.Rows[iIndex].ListObject;
                        maintenanceJournalList.Add(listObjectgridMaintenanceList);
                    }
                }

                // �ԍ����ɕ��ѕς��܂�
                var x = from i in maintenanceJournalList
                        orderby int.Parse(i.maintenanceJournalNo)
                        select i;
                maintenanceJournalList = x.ToList();

                // grid��No��������ƂȂ�悤�ɕύX���܂�
                for (int i = 0; i < maintenanceJournalList.Count; i++)
                {
                    maintenanceJournalList[i].No = i + 1;
                }

                this.maintenanceJournalListDatasServiceman = maintenanceJournalList;
            }
        }


        /// <summary>
        /// �O���b�h�f�[�^�쐬
        /// </summary>
        /// <param name="maintenanceJournalListDatas">grid�i�[�p�̃f�[�^�^</param>
        /// <param name="codeList">��ʂ��Ƃ̃��b�Z�[�W���X�g</param>
        /// <param name="blModuleList">module���Ƃ̃`�F�b�N�l�̃��X�g</param>
        /// <param name="iKind">enum��int�^�ɂ������</param>
        private void CreateMaintenanceJournalGridData(ref List<MaintenanceJournalListData> maintenanceJournalList, List<List<MaintenanceJournalCodeData>> codeList, List<List<Boolean>> blModuleList, int iKind)
        {
            try
            {

                // �c���ύX
                VerticalandHolizontalChange(ref blModuleList);

                // �O���b�h�\���f�[�^���i�[
                for (int i = 0; i < codeList[iKind].Count; i++)
                {
                    maintenanceJournalList.Add(new MaintenanceJournalListData(i + 1, codeList[iKind][i].Title, codeList[iKind][i].Kind,
                        blModuleList[i], int.Parse(codeList[iKind][i].Code), codeList[iKind][i].Unit));
                }
            }
            catch (Exception ex)
            {
                // �t�@�C���Ǎ��Ɏ��s���܂����B
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("Reading the file failed :{0}", ex.Message));
            }
        }

        /// <summary>
        /// ���X�g�̏c���ύX
        /// </summary>
        /// <param name="listBooleanData">�ύX���������X�g</param>
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
        /// ���X�g�̏c���ύX
        /// </summary>
        /// <param name="listBooleanData">�ύX���������X�g</param>
        public void VerticalandHolizontalChange(ref List<List<String>> listStringData)
        {
            try
            {
                // �c���ύX
                List<List<String>> reverseItemList = new List<List<String>>();
                for (int i = 0; i < listStringData.Last().Count; i++)
                {
                    var listStringDataRow = new List<String>();
                    for (int j = 0; j < listStringData.Count; j++)
                    {
                        // "�������Ă��܂����߁A�����ō폜
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
        /// ��ʂ��Ƃ̃��b�Z�[�W���X�g���쐬
        /// </summary>
        public List<List<MaintenanceJournalCodeData>> CodeListEachKind()
        {
            var maintenanceJournalCodeList = Singleton<ParameterFilePreserve<MaintenanceJournalCodeDataManager>>.Instance.Param.CodeDataList;
            // �d���`�F�b�N
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

            // ��ʂ��Ƃ̃R�[�h���X�g�̐錾
            List<MaintenanceJournalCodeData> codeListUserDaily = new List<MaintenanceJournalCodeData>();
            List<MaintenanceJournalCodeData> codeListUserWeekly = new List<MaintenanceJournalCodeData>();
            List<MaintenanceJournalCodeData> codeListUserMonthly = new List<MaintenanceJournalCodeData>();
            List<MaintenanceJournalCodeData> codeListServicemanMonthly = new List<MaintenanceJournalCodeData>();
            List<MaintenanceJournalCodeData> codeListServicemanYearly = new List<MaintenanceJournalCodeData>();
            List<List<MaintenanceJournalCodeData>> codeListKind = new List<List<MaintenanceJournalCodeData>>();
            // �R�[�h���X�g����ʂ��ƂɐU�蕪����B
            foreach (var rowCodeList in maintenanceJournalCodeDatas)
            {

                // // ��ʂ̓����e���i���X�����ԍ���1~100�̓��[�U�����̃f�C���[
                if (rowCodeList.Unit == "0"
                    && rowCodeList.Kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_009)
                {
                    codeListUserDaily.Add(rowCodeList);
                }
                // ��ʂ̓����e���i���X�����ԍ���101~200�̓��[�U�����̃E�B�[�N���[
                else if (rowCodeList.Unit == "0"
                    && rowCodeList.Kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_010)
                {
                    codeListUserWeekly.Add(rowCodeList);
                }
                // ��ʂ̓����e���i���X�����ԍ���201~300�̓��[�U�����̃}���X���[
                else if (rowCodeList.Unit == "0"
                    && rowCodeList.Kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_011)
                {
                    codeListUserMonthly.Add(rowCodeList);
                }
                // ��ʂ̓����e���i���X�����ԍ���1001~1100�̓T�[�r�X�}�������̃}���X���[
                else if (rowCodeList.Unit != "0"
                    && rowCodeList.Kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_011)
                {
                    codeListServicemanMonthly.Add(rowCodeList);
                }
                // ��ʂ̓����e���i���X�����ԍ���1101~1200�̓��[�U�����̃}���X���[
                else if (rowCodeList.Unit != "0"
                    && rowCodeList.Kind == Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_012)
                {
                    codeListServicemanYearly.Add(rowCodeList);
                }
            }

            // ���X�g�Ɏ�ʂ��Ƃ̃��b�Z�[�W���X�g��ǉ�
            codeListKind.Add(codeListUserDaily);
            codeListKind.Add(codeListUserWeekly);
            codeListKind.Add(codeListUserMonthly);
            codeListKind.Add(codeListServicemanMonthly);
            codeListKind.Add(codeListServicemanYearly);

            return codeListKind;
        }


        /// OK�{�^������
        /// 
        public void OkExecute(Oelco.Common.GUI.CustomGrid grdMaintenanceList)
        {
            try
            {
                // ���[�U�[�p�����e�i���X�����̏ꍇ
                if (this.mainteJournalType == MaintenanceJournalType.User)
                {
                    ParameterFilePreserve<CarisXMaintenanceUserParameter> maintenanceJournalUserList = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance;

                    // ��ʃO���b�h����������X�g�Ɋi�[���܂�
                    OkExecuteUser(ref maintenanceJournalUserList, grdMaintenanceList);

                    //xml�t�@�C���֕ۑ����܂�
                    maintenanceJournalUserList.SaveRaw();

                    // csv�ɕۑ����܂�
                    exportData(grdMaintenanceList);

                    // �ꎞ�I�Ƀ`�F�b�N��Ԃ�ۑ����܂�
                    newLoadMaintenanceJournalListData(grdMaintenanceList, MaintenanceJournalType.User);
                }
                // �T�[�r�X�}���p�����e�i���X�����̏ꍇ
                else if (this.mainteJournalType == MaintenanceJournalType.Serviceman)
                {
                    // �ꎞ�I�Ƀ`�F�b�N��Ԃ�ۑ����܂�
                    newLoadMaintenanceJournalListData(grdMaintenanceList, MaintenanceJournalType.Serviceman);
                }
            }
            catch (Exception ex)
            {
                //OK�{�^������Ɏ��s���܂����B
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("OK button operation failed :{0}", ex.Message));
            }
        }

        /// <summary>
        /// User����OK��������
        /// </summary>
        /// <param name="maintenanceJournalUserList"></param>
        /// <param name="grdMaintenanceList"></param>
        private void OkExecuteUser(ref ParameterFilePreserve<CarisXMaintenanceUserParameter> maintenanceJournalUserList, Oelco.Common.GUI.CustomGrid grdMaintenanceList)
        {
            // �S�`�F�b�N���ǂ����̃t���O���擾���邽�߂̃C���X�^���X�쐬
            var AllCheckFlagCheck = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.Param;
            bool checkOffFlag = false;

            // �s
            // module1 �` 4
            if (AllCheckFlagCheck.AllCheckDaily == false)
            {
                for (int i = 0; i < ModuleNumConnected; i++)
                {
                    // �f�C���[�̃`�F�b�N�󋵂��i�[����
                    for (int j = this.dailyIndex.First() - 1; j < this.dailyIndex.Last(); j++)
                    {
                        // ��ʂ̕�����ŐU�蕪����
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
                // �S�ă`�F�b�N
                if (checkOffFlag == false)
                {
                    maintenanceJournalUserList.Param.AllCheckDaily = true;
                    maintenanceJournalUserList.Param.AllFinishDaily = DateTime.Now;
                }
                // �t���O�����Z�b�g
                checkOffFlag = false;
            }

            if (AllCheckFlagCheck.AllCheckWeekly == false)
            {
                // module1 �` 4
                for (int i = 0; i < ModuleNumConnected; i++)
                {
                    // ���X�g����ʂ̃��X�g��[0]����l���������
                    int cnt = 0;
                    // �E�B�[�N���[�̃`�F�b�N�󋵂��i�[����
                    for (int j = this.weeklyIndex.First() - 1; j < this.weeklyIndex.Last(); j++)
                    {
                        // ��ʂ̕�����ŐU�蕪����
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
                // �S�ă`�F�b�N
                if (checkOffFlag == false)
                {
                    maintenanceJournalUserList.Param.AllCheckWeekly = true;
                    maintenanceJournalUserList.Param.AllFinishWeekly = DateTime.Now;
                }
                // �t���O�����Z�b�g
                checkOffFlag = false;
            }

            if (AllCheckFlagCheck.AllCheckMonthly == false)
            {
                // module1 �` 4
                for (int i = 0; i < ModuleNumConnected; i++)
                {
                    // ���X�g����ʂ̃��X�g��[0]����l���������
                    int cnt = 0;
                    for (int j = this.monthlyIndex.First() - 1; j < this.monthlyIndex.Last(); j++)
                    {
                        // ��ʂ̕�����ŐU�蕪����
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
                // �S�ă`�F�b�N
                if (checkOffFlag == false)
                {
                    maintenanceJournalUserList.Param.AllCheckMonthly = true;
                    maintenanceJournalUserList.Param.AllFinishMonthly = DateTime.Now;
                }
            }
        }

        #region [csv]

        /// <summary>
        /// �`�F�b�N���̃t�@�C���o��
        /// </summary>
        /// <remarks>
        /// �O���b�h�̃`�F�b�N���̃t�@�C���o�͂��܂�
        /// </remarks>
        private void exportData(Oelco.Common.GUI.CustomGrid grdMaintenanceList)
        {
            // �t�@�C���o�͑Ώۂ��擾
            // xml�t�@�C�����擾
            Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.LoadRaw();
            ParameterFilePreserve<CarisXMaintenanceUserParameter> exportUserData = new ParameterFilePreserve<CarisXMaintenanceUserParameter>();
            // xml�t�@�C�����擾
            Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.LoadRaw();
            ParameterFilePreserve<CarisXMaintenanceServicemanParameter> exportServicemanData = new ParameterFilePreserve<CarisXMaintenanceServicemanParameter>();
            List<MaintenanceJournalCodeData> maintenanceJournalCodeList = Singleton<ParameterFilePreserve<MaintenanceJournalCodeDataManager>>.Instance.Param.CodeDataList;

            // �d���`�F�b�N
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
                // ���[�U�[�p�����e�i���X�����̏ꍇ
                if (this.mainteJournalType == MaintenanceJournalType.User)
                {
                    if (dailyIndex.Count != 0)
                    {
                        strKind = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_009;
                        // 202001_Mainte_Dayly.csv
                        saveFileName = DateTime.Now.ToString("yyyyMM") + "_Mainte_Daily.csv";

                        for (int moduleCnt = 0; moduleCnt < ModuleNumConnected; moduleCnt++)
                        {
                            // csv�t�@�C���o�͐�p�X
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
                            // csv�t�@�C���o�͐�p�X
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
                            // csv�t�@�C���o�͐�p�X
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
                // �T�[�r�X�}���p�����e�i���X�����̏ꍇ
                else if (this.mainteJournalType == MaintenanceJournalType.Serviceman)
                {
                    strKind = Oelco.CarisX.Properties.Resources.STRING_DLG_MAINTENANCEJOURNALLIST_011;
                    if (monthlyIndex.Count != 0)
                    {
                        // 202001_Mainte_Dayly.csv
                        saveFileName = DateTime.Now.ToString("yyyy") + "_S_Mainte_Monthly.csv";

                        for (int moduleCnt = 0; moduleCnt < ModuleNumConnected; moduleCnt++)
                        {
                            // csv�t�@�C���o�͐�p�X
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
                            // csv�t�@�C���o�͐�p�X
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
                //csv�쐬�Ɏ��s���܂����B
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("csv creation failed :{0}", ex.Message));
            }

        }

        public void CreateCsv(List<MaintenanceJournalCodeData> maintenanceJournalCodeDatas, int moduleCnt, string strKind, string saveFilePath, Oelco.Common.GUI.CustomGrid grdMaintenanceList)
        {
            // �f�[�^�쐬
            List<List<String>> maintenanceCsvDataItem = new List<List<string>>();

            // csv�t�@�C�������݂���ꍇ�A�����擾
            if (System.IO.File.Exists(saveFilePath))
            {
                // ����csv�̃C���|�[�g
                MaintenanceJournalImportCsv(maintenanceCsvDataItem, saveFilePath);

                // �c���ύX
                VerticalandHolizontalChange(ref maintenanceCsvDataItem);
            }
            // csv�t�@�C�������݂��Ȃ��ꍇ�A���ږ����쐬
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
                // ����̃f�[�^���쐬�i�ŏI��ɒǉ�����f�[�^�j
                maintenanceCsvDataItem = CreateCsvDataInformationUser(maintenanceCsvDataItem, moduleCnt, strKind, grdMaintenanceList);
                // csv�������Ԃō��ڒǉ����Ă邩�`�F�b�N�B�ǉ����Ă����珈�����s���܂�
                CsvAddAdjustmentUser(ref maintenanceCsvDataItem, moduleCnt, grdMaintenanceList, strKind);
            }
            else if (this.mainteJournalType == MaintenanceJournalType.Serviceman)
            {
                maintenanceCsvDataItem = CreateCsvDataInformationServiceman(maintenanceCsvDataItem, moduleCnt, strKind);
                // csv�������Ԃō��ڒǉ����Ă邩�`�F�b�N�B�ǉ����Ă����珈�����s���܂�
                CsvAddAdjustmentServiceman(ref maintenanceCsvDataItem, moduleCnt, strKind);
            }

            // �c���ύX
            VerticalandHolizontalChange(ref maintenanceCsvDataItem);

            // �����e�i���X����CSV�o�͏���
            MaintenanceJournalExportCsv(maintenanceCsvDataItem, saveFilePath, false);
        }

        /// <summary>
        /// csv�������Ԃō��ڒǉ��������̏���
        /// </summary>
        private void CsvAddAdjustmentUser(ref List<List<String>> maintenanceCsvDataItem, int moduleCnt, Oelco.Common.GUI.CustomGrid grdMaintenanceList, string strKind)
        {
            List<List<String>> diffCsvDatas = new List<List<String>>();

            // ����̃f�[�^���쐬�i�ŏI��ɒǉ�����f�[�^�j
            diffCsvDatas = CreateCsvDataItemUser(maintenanceCsvDataItem, moduleCnt, strKind, grdMaintenanceList);

            // �����e���i���X�ԍ���Œǉ����ꂽ�Z���i�s�j���擾
            var rowIndexList = diffCsvDatas[0].Except(maintenanceCsvDataItem[0]).ToList();
            // �O���b�h�̍��ڂɂ����Ċ���csv�ɂȂ��R�[�h��1���ȏ゠�邩
            if (rowIndexList.Count > 0)
            {
                // �R�[�h��
                var diffCsvDatasCodeColumn = diffCsvDatas[0];
                // �ǉ����ꂽ�s�̃C���f�b�N�X�����X�g�ɂ���
                List<Int32> rowDiffIndexList = new List<Int32>();

                // �ǉ����ꂽ�R�[�h�񃊃X�g�̂����A�ǉ����ڂ̃C���f�b�N�X���擾
                for (int i = 0; i < rowIndexList.Count; i++)
                {
                    Int32 rowindex = diffCsvDatasCodeColumn.FindIndex(item => item == rowIndexList[i]); // �C���f�b�N�X������int�^���X�g
                    rowDiffIndexList.Add(rowindex);
                }

                // �Ώۂ̍s�ɒǉ�����B���̏ꍇ�A�{���ȑO�̃f�[�^��͋󔒂Ƃ���
                for (int i = 0; i < rowDiffIndexList.Count; i++)
                {
                    // �ύX���ꂽ���ꂽ�C���f�b�N�X
                    Int32 iTargetAddIndex = rowDiffIndexList[i];

                    for (int j = 0; j < maintenanceCsvDataItem.Count - 1; j++)
                    {
                        if (j <= 1)
                        {
                            String strDiffData = diffCsvDatas[j][iTargetAddIndex];
                            // �R�[�h�� �`�F�b�N�A�C�e����
                            maintenanceCsvDataItem[j].Insert(iTargetAddIndex, strDiffData);
                        }
                        else
                        {
                            // �f�[�^�� �󔒂�ǉ�
                            maintenanceCsvDataItem[j].Insert(iTargetAddIndex, "");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// csv�������Ԃō��ڒǉ��������̏���
        /// </summary>
        private void CsvAddAdjustmentServiceman(ref List<List<String>> maintenanceCsvDataItem, int moduleCnt, string strKind)
        {
            List<List<String>> diffCsvDatas = new List<List<String>>();

            diffCsvDatas = CreateCsvDataItemServiceman(maintenanceCsvDataItem, moduleCnt, strKind);

            // �����e���i���X�ԍ���Œǉ����ꂽ�Z���i�s�j���擾
            var rowIndexList = diffCsvDatas[0].Except(maintenanceCsvDataItem[0]).ToList();
            // �O���b�h�̍��ڂɂ����Ċ���csv�ɂȂ��R�[�h��1���ȏ゠�邩
            if (rowIndexList.Count > 0)
            {
                // �R�[�h��
                var diffCsvDatasCodeColumn = diffCsvDatas[0];
                // �ǉ����ꂽ�s�̃C���f�b�N�X�����X�g�ɂ���
                List<Int32> rowDiffIndexList = new List<Int32>();

                // �ǉ����ꂽ�R�[�h�񃊃X�g�̂����A�ǉ����ڂ̃C���f�b�N�X���擾
                for (int i = 0; i < rowIndexList.Count; i++)
                {
                    Int32 rowindex = diffCsvDatasCodeColumn.FindIndex(item => item == rowIndexList[i]); // �C���f�b�N�X������int�^���X�g
                    rowDiffIndexList.Add(rowindex);
                }

                // �Ώۂ̍s�ɒǉ�����B���̏ꍇ�A�{���ȑO�̃f�[�^��͋󔒂Ƃ���
                for (int i = 0; i < rowDiffIndexList.Count; i++)
                {
                    // �ύX���ꂽ���ꂽ�C���f�b�N�X
                    Int32 iTargetAddIndex = rowDiffIndexList[i];

                    for (int j = 0; j < maintenanceCsvDataItem.Count - 1; j++)
                    {
                        if (j <= 1)
                        {
                            String strDiffData = diffCsvDatas[j][iTargetAddIndex];
                            // �R�[�h�� �`�F�b�N�A�C�e����
                            maintenanceCsvDataItem[j].Insert(iTargetAddIndex, strDiffData);
                        }
                        else
                        {
                            // �f�[�^�� �󔒂�ǉ�
                            maintenanceCsvDataItem[j].Insert(iTargetAddIndex, "");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// CSV�o��(���񖼍쐬��)
        /// </summary>
        /// <remarks>
        /// CSV�o�͂��s���܂��B
        /// </remarks>
        /// <param name="dataList">�o�̓f�[�^</param>
        /// /// <param name="savePath">�ۑ���t�@�C����</param>
        /// <param name="append">[����]�쐬�܂��͏㏑��(false)/�����ɒǉ�(true)/�t�@�C�������݂���ꍇ�ǉ��B���݂��Ȃ��ꍇ�ɍ쐬(null)</param>
        private void MaintenanceJournalExportCsv(List<List<String>> dataList, String savePath, Boolean? append)
        {
            // ��؂蕶���̏�����
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
                    // �s�f�[�^�̏�������
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
        /// CSV����
        /// </summary>
        /// <remarks>
        /// CSV���͂��s���܂��B
        /// </remarks>
        /// <param name="dataList">�o�̓f�[�^</param>
        /// <param name="savePath">�ۑ���t�@�C����</param>
        private Boolean MaintenanceJournalImportCsv(List<List<String>> dataList, String savePath)
        {
            try
            {
                // �ǂݍ��݂���CSV�t�@�C���̃p�X���w�肵�ĊJ��
                System.IO.StreamReader sr = new System.IO.StreamReader(savePath);
                {
                    // �����܂ŌJ��Ԃ�
                    while (!sr.EndOfStream)
                    {
                        // CSV�t�@�C���̈�s��ǂݍ���
                        string line = sr.ReadLine();
                        line = line.Replace("\",", "\"\",");

                        // �ǂݍ��񂾈�s���J���}���ɕ����Ĕz��Ɋi�[����
                        //string[] values = line.Split(',');
                        string[] b = new string[] { "\"," };
                        string[] values = line.Split(b, StringSplitOptions.None);

                        // �z�񂩂烊�X�g�Ɋi�[����
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
        /// CSV�o�͂̍��ڍ쐬
        /// </summary>
        /// <remarks>
        /// CSV�o�͗p�f�[�^�̍��ڂ��쐬���܂��i�c���͋t�j
        /// </remarks>
        /// <param name="CsvData">�o�͂���csv</param>
        /// <param name="moduleNo">���W���[���ԍ�</param>
        /// <param name="kind">���</param>
        /// <returns>CsvData�Ƀf�[�^���i�[</returns>
        private List<List<String>> CreateCsvDataItemUser(List<List<String>> CsvData, int moduleNo, String kind, Oelco.Common.GUI.CustomGrid grdMaintenanceList)
        {
            List<MaintenanceJournalListData> maintenanceListDatas = new List<MaintenanceJournalListData>();

            // Kind��ݒ�
            int cntFirst = 0;
            int cntLast = 0;

            // ��ʂɂ�胋�[�v�p�ϐ��̒l��ݒ肵�܂�
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

            // csv�o�͐�́u��ʁv���ڂ̖��O��ݒ肵�܂�
            // module�̔ԍ�
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

            // grid����Y����kind�̍s�����擾
            for (int i = cntFirst; i < cntLast; i++)
            {
                var listObjectgridMaintenanceList = (MaintenanceJournalListData)grdMaintenanceList.Rows[i].ListObject;
                maintenanceListDatas.Add(listObjectgridMaintenanceList);
            }

            // �f�[�^�쐬�p
            var maintenanceCsvDataItem = new List<List<String>>();
            var maintenanceListDatasAdd = new List<String>();

            // csv�o�͂�1��ڂ��i�[���܂�
            for (int j = 0; j < maintenanceListDatas.Count + 2; j++)
            {

                if (j == 0)
                {
                    maintenanceListDatasAdd.Add(strModuleName);
                }
                else if (j == 1)
                {
                    // ���ږ�
                    maintenanceListDatasAdd.Add(STRING_CSV_CODE);
                }
                else if (j >= 2)
                {
                    // �R�[�h
                    maintenanceListDatasAdd.Add(string.Format("{0:00}", int.Parse(maintenanceListDatas[j - 2].unitNo)) + "-" + string.Format("{0:0000}", int.Parse(maintenanceListDatas[j - 2].maintenanceJournalNo)));
                }

            }
            maintenanceCsvDataItem.Insert(maintenanceCsvDataItem.Count, maintenanceListDatasAdd);

            // csv�o�͂�2��ڂ��i�[���܂�
            maintenanceListDatasAdd = new List<String>();
            // ���ڍ쐬
            for (int j = 0; j < maintenanceListDatas.Count + 2; j++)
            {
                // �v�f�ǉ�
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
                    // ���ږ�
                    maintenanceListDatasAdd.Add(maintenanceListDatas[j - 2].checkItem);
                }

            }
            maintenanceCsvDataItem.Insert(maintenanceCsvDataItem.Count, maintenanceListDatasAdd);

            return maintenanceCsvDataItem;
        }

        ///
        /// <summary>
        /// CSV�o�͂̍��ڍ쐬
        /// </summary>
        /// <remarks>
        /// CSV�o�͗p�f�[�^�̍��ڂ��쐬���܂��i�c���͋t�j
        /// </remarks>
        /// <param name="CsvData">�o�͂���csv</param>
        /// <param name="moduleNo">���W���[���ԍ�</param>
        /// <param name="kind">���</param>
        /// <returns>CsvData�Ƀf�[�^���i�[</returns>
        private List<List<String>> CreateCsvDataItemServiceman(List<List<String>> CsvData, int moduleNo, String kind)
        {
            List<MaintenanceJournalListData> maintenanceListDatas = new List<MaintenanceJournalListData>();

            // Kind��ݒ�
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

            // csv�o�͐�́u��ʁv���ڂ̖��O��ݒ肵�܂�
            // module�̔ԍ�
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

            // �f�[�^�쐬�p
            List<List<String>> maintenanceCsvDataItem = new List<List<String>>();
            List<String> maintenanceListDatasAdd = new List<String>();

            int k = 0;

            // csv�o�͂�1��ڂ��i�[���܂�
            for (int j = 0; j < cntCount + 2; j++)
            {
                if (j == 0)
                {
                    maintenanceListDatasAdd.Add(strModuleName);
                }
                else if (j == 1)
                {
                    // ���ږ�
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

                    // �R�[�h
                    maintenanceListDatasAdd.Add(string.Format("{0:00}", int.Parse(this.maintenanceJournalListDatasServiceman[k].unitNo)) + "-" + string.Format("{0:0000}", int.Parse(this.maintenanceJournalListDatasServiceman[k].maintenanceJournalNo)));
                }
            }
            maintenanceCsvDataItem.Insert(maintenanceCsvDataItem.Count, maintenanceListDatasAdd);

            // csv�o�͂�2��ڂ��i�[���܂�
            maintenanceListDatasAdd = new List<String>();
            // ���ڍ쐬
            for (int j = 0; j < cntCount + 2; j++)
            {
                // �v�f�ǉ�
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

                    // ���ږ�
                    maintenanceListDatasAdd.Add(this.maintenanceJournalListDatasServiceman[k].checkItem);
                }

            }
            maintenanceCsvDataItem.Insert(maintenanceCsvDataItem.Count, maintenanceListDatasAdd);

            return maintenanceCsvDataItem;
        }



        ///
        /// <summary>
        /// CSV�o�͂̃f�[�^�쐬
        /// </summary>
        /// <remarks>
        /// CSV�o�͗p�f�[�^�̍���̏����쐬���܂��i�c���͋t�j
        /// </remarks>
        /// <param name="CsvData">�o�͂���csv</param>
        /// <param name="moduleNo">���W���[���ԍ�</param>
        /// <param name="kind">���</param>
        /// <returns>CsvData�Ƀf�[�^���i�[</returns>
        private List<List<String>> CreateCsvDataInformationUser(List<List<String>> CsvData, int moduleNo, String kind, Oelco.Common.GUI.CustomGrid grdMaintenanceList)
        {
            List<MaintenanceJournalListData> maintenanceListDatas = new List<MaintenanceJournalListData>();

            // Kind��ݒ�
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

            // kind����grid����擾����f�[�^���X�g���Ⴄ����
            for (int i = cntFirst; i < cntLast; i++)
            {
                var listObjectgridMaintenanceList = (MaintenanceJournalListData)grdMaintenanceList.Rows[i].ListObject;
                maintenanceListDatas.Add(listObjectgridMaintenanceList);
            }

            // �f�[�^�쐬�p
            List<String> maintenanceListDatasAdd = new List<String>();

            // ���݃��O�C�����̃��[�U�[���擾
            String nowUserId = Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID;

            // Data�쐬
            // ���ڍ쐬
            for (int j = 0; j < maintenanceListDatas.Count + 2; j++)
            {
                // �v�f�ǉ�
                if (j == 0)
                {
                    // ���O�C�����[�U�[����ݒ肵�܂�
                    maintenanceListDatasAdd.Add(nowUserId);
                }
                else if (j == 1)
                {
                    // ���݂̓��t��ݒ肵�܂�
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
                        // ���ږ�
                        maintenanceListDatasAdd.Add(maintenanceListDatas[j - 2].module4.ToString());
                    }
                }


            }
            CsvData.Insert(CsvData.Count, maintenanceListDatasAdd);

            return CsvData;
        }


        ///
        /// <summary>
        /// CSV�o�͂̃f�[�^�쐬�i�T�[�r�X�}���p�j
        /// </summary>
        /// <remarks>
        /// CSV�o�͗p�f�[�^�̍���̏����쐬���܂��i�c���͋t�j
        /// </remarks>
        /// <param name="CsvData">�o�͂���csv</param>
        /// <param name="moduleNo">���W���[���ԍ�</param>
        /// <param name="kind">���</param>
        /// <returns>CsvData�Ƀf�[�^���i�[</returns>
        private List<List<String>> CreateCsvDataInformationServiceman(List<List<String>> CsvData, int moduleNo, String kind)
        {
            List<MaintenanceJournalListData> maintenanceListDatas = new List<MaintenanceJournalListData>();

            // Kind��ݒ�
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

            // �f�[�^�쐬�p
            var maintenanceListDatasAdd = new List<String>();

            // ���݃��O�C�����̃��[�U�[���擾
            String nowUserId = Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID;

            int k = 0;

            // Data�쐬
            // ���ڍ쐬
            for (int j = 0; j < cntCount + 2; j++)
            {
                // �v�f�ǉ�
                if (j == 0)
                {
                    // ���O�C�����[�U�[����ݒ肵�܂�
                    maintenanceListDatasAdd.Add(nowUserId);
                }
                else if (j == 1)
                {
                    // ���݂̓��t��ݒ肵�܂�
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
                        // ���ږ�
                        maintenanceListDatasAdd.Add(this.maintenanceJournalListDatasServiceman[k].module4.ToString());
                    }
                }


            }
            CsvData.Insert(CsvData.Count, maintenanceListDatasAdd);

            return CsvData;
        }

        #endregion

        /// <summary>
        /// �����e�i���X��ʂɂ�Exit������������,csv�o�͂��܂�
        /// </summary>
        /// <remarks>�`�F�b�N��Ԃ�ۑ���,csv���o�͂��܂�</remarks>
        /// <param name="grdMaintenanceJournalList">csv���\�b�h�̈���</param>
        public void ServicemanExitExecute(Oelco.Common.GUI.CustomGrid grdMaintenanceJournalList)
        {
            try
            {
                ParameterFilePreserve<CarisXMaintenanceServicemanParameter> maintenanceJournalServicemanList = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance;
                // �S�`�F�b�N���ǂ����̃t���O���擾���邽�߂̃C���X�^���X�쐬
                var AllCheckFlagCheck = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.Param;
                bool checkOffFlag = false;

                // �s
                // module1 �` 4
                if (AllCheckFlagCheck.AllCheckMonthly == false)
                {
                    for (int j = 0; j < this.monthlyIndex.Count; j++)
                    {
                        int k = this.monthlyIndex[j] - 1;
                        // ��ʂ̕�����ŐU�蕪����
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
                    // �S�ă`�F�b�N
                    if (checkOffFlag == false)
                    {
                        maintenanceJournalServicemanList.Param.AllCheckMonthly = true;
                        maintenanceJournalServicemanList.Param.AllFinishMonthly = DateTime.Now;
                    }
                    // �t���O�����Z�b�g
                    checkOffFlag = false;
                }

                if (AllCheckFlagCheck.AllCheckYearly == false)
                {
                    for (int j = 0; j < this.yearlyIndex.Count; j++)
                    {
                        int k = this.yearlyIndex[j] - 1;
                        // ��ʂ̕�����ŐU�蕪����
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
                    // �S�ă`�F�b�N
                    if (checkOffFlag == false)
                    {
                        maintenanceJournalServicemanList.Param.AllCheckYearly = true;
                        maintenanceJournalServicemanList.Param.AllFinishYearly = DateTime.Now;
                    }
                    // �t���O�����Z�b�g
                    checkOffFlag = false;
                }

                //xml�t�@�C���֕ۑ�����
                maintenanceJournalServicemanList.SaveRaw();

                // csv�ɕۑ�����
                exportData(grdMaintenanceJournalList);

                // ��ʑJ�ڂ���̂ŏ�����
                this.maintenanceJournalListDatasServiceman = new List<MaintenanceJournalListData>();
            }
            catch (Exception ex)
            {
                // Exit�{�^���̏����Ɏ��s���܂����B
                Singleton<Oelco.Common.Log.LogManager>.Instance.WriteCommonLog(Oelco.Common.Log.LogKind.DebugLog, String.Format("Exit button processing failed :{0}", ex.Message));
            }
        }

        /// <summary>
        /// �p�����[�^�t�@�C��������
        /// </summary>
        public void InitializeCheckParameterFile()
        {
            Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance.LoadRaw();
            Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance.LoadRaw();
            ParameterFilePreserve<CarisXMaintenanceUserParameter> maintenanceJournalUserList = Singleton<ParameterFilePreserve<CarisXMaintenanceUserParameter>>.Instance;
            ParameterFilePreserve<CarisXMaintenanceServicemanParameter> maintenanceJournalServicemanList = Singleton<ParameterFilePreserve<CarisXMaintenanceServicemanParameter>>.Instance;

            Boolean blChangeFlg = false;

            // ���[�U�[�p�����e�i���X�����̏ꍇ
            if (this.mainteJournalType == MaintenanceJournalType.User)
            {

                // �f�C���[
                // ���t���O�������菊������߂��Ă����ꍇ�Afalse�Ƀt���O��S�Ė߂�
                if (maintenanceJournalUserList.Param.AllFinishDaily.AddDays(1) < DateTime.Now
                    && maintenanceJournalUserList.Param.AllCheckDaily == true)
                {
                    blChangeFlg = FlagChange(ref maintenanceJournalUserList, ref maintenanceJournalServicemanList, Kind.U_Daily);
                }

                // �E�B�[�N���[
                // ���t���O�������菊������߂��Ă����ꍇ�Afalse�Ƀt���O��S�Ė߂�
                if (maintenanceJournalUserList.Param.AllFinishWeekly.AddDays(7) < DateTime.Now
                    && maintenanceJournalUserList.Param.AllCheckWeekly == true)
                {
                    blChangeFlg = FlagChange(ref maintenanceJournalUserList, ref maintenanceJournalServicemanList, Kind.U_Weekly);
                }

                // �}���X���[
                // ���t���O�������菊������߂��Ă����ꍇ�Afalse�Ƀt���O��S�Ė߂�
                if (maintenanceJournalUserList.Param.AllFinishMonthly.AddMonths(1) < DateTime.Now
                    && maintenanceJournalUserList.Param.AllCheckMonthly == true)
                {
                    blChangeFlg = FlagChange(ref maintenanceJournalUserList, ref maintenanceJournalServicemanList, Kind.U_Monthly);
                }

                if (blChangeFlg == true)
                {
                    //xml�t�@�C���֕ۑ�����
                    maintenanceJournalUserList.SaveRaw();
                }
            }
            // �T�[�r�X�}���p�����e�i���X�����̏ꍇ
            else if (this.mainteJournalType == MaintenanceJournalType.Serviceman)
            {
                // �}���X���[
                // ���t���O�������菊������߂��Ă����ꍇ�Afalse�Ƀt���O��S�Ė߂�
                if (maintenanceJournalServicemanList.Param.AllFinishMonthly.AddMonths(1) < DateTime.Now
                    && maintenanceJournalServicemanList.Param.AllCheckMonthly == true)
                {
                    blChangeFlg = FlagChange(ref maintenanceJournalUserList, ref maintenanceJournalServicemanList, Kind.S_Monthly);
                }

                // �C���[���[
                // ���t���O�������菊������߂��Ă����ꍇ�Afalse�Ƀt���O��S�Ė߂�
                if (maintenanceJournalServicemanList.Param.AllFinishYearly.AddYears(1) < DateTime.Now
                    && maintenanceJournalServicemanList.Param.AllCheckYearly == true)
                {
                    blChangeFlg = FlagChange(ref maintenanceJournalUserList, ref maintenanceJournalServicemanList, Kind.S_Yearly);
                }

                if (blChangeFlg == true)
                {
                    //xml�t�@�C���֕ۑ�����
                    maintenanceJournalServicemanList.SaveRaw();
                }
            }
        }

        /// <summary>
        /// �p�����[�^�t�@�C����ʂ��Ƃɏ�����
        /// </summary>
        /// <param name="maintenanceJournalUserList">���[�U�[�f�[�^</param>
        /// <param name="maintenanceJournalServicemanList">�T�[�r�X�}���f�[�^</param>
        /// <param name="kind">���</param>
        /// <returns>true : �ύX�L��</returns>
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

            // false�ɑS�Ė߂�
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
        /// ��ʂ��ƂɃp�����[�^�t�@�C���̃p�����[�^��false�֕ύX
        /// </summary>
        /// �p�����[�^�t�@�C����ʂ��Ƃɏ�����
        /// </summary>
        /// <param name="kind">���</param>
        /// <returns>true : �ύX�L��</returns>
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

            // false�ɑS�Ė߂�
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
                //xml�t�@�C���֕ۑ�����
                maintenanceJournalUserList.SaveRaw();
            }
            else if (this.mainteJournalType == MaintenanceJournalType.Serviceman)
            {
                //xml�t�@�C���֕ۑ�����
                maintenanceJournalServicemanList.SaveRaw();
            }

            return true;

        }


        #endregion
    }
}