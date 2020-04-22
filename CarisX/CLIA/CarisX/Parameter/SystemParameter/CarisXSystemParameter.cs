using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Const;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// CarisXシステムパラメータ
    /// </summary>
    /// <remarks>
    /// CarisXのシステムパラメータ管理を行います。
    /// </remarks>
    public class CarisXSystemParameter : ISavePath
    {

        #region [インスタンス変数定義]

        /// <summary>
        /// 自動シャットダウン設定
        /// </summary>
        private AutoShutdownParameter autoShutdownParameter = new AutoShutdownParameter();
        /// <summary>
        /// 分析方式設定
        /// </summary>
        private AssayModeParameter assayModeParameter = new AssayModeParameter();


        /// <summary>
        /// 校准方式設定
        /// </summary>

        private CalibrationModeParameter calibrationModeParameter = new CalibrationModeParameter();

        /// <summary>
        /// ホスト設定
        /// </summary>
        private HostParameter hostParameter = new HostParameter();

        // 2020-02-27 CarisX IoT Add [START]
        /// <summary>
        /// IoT設定
        /// </summary>
        private IoTParameter ioTParameter = new IoTParameter();
        // 2020-02-27 CarisX IoT Add [END]

        /// <summary>
        /// 自動プライミング設定
        /// </summary>
        private AutoPrimeParameter autoPrimeParameter = new AutoPrimeParameter();
        /// <summary>
        /// 自動起動設定
        /// </summary>
        private AutoStartupTimerParameter autoStartupTimerParameter = new AutoStartupTimerParameter();
        /// <summary>
        /// サイクルタイム設定
        /// </summary>
        private CycleTimeParameter cycleTimeParameter = new CycleTimeParameter();
        /// <summary>
        /// 装置No.設定
        /// </summary>
        private DeviceNoParameter deviceNoParameter = new DeviceNoParameter();
        /// <summary>
        /// エラー音、警告音設定
        /// </summary>
        private ErrWarningBeepParameter errWarningBeepParameter = new ErrWarningBeepParameter();
        /// <summary>
        /// フラッシュプライミング設定
        /// </summary>
        private FlushPrimeParameter flushParameter = new FlushPrimeParameter();
        /// <summary>
        /// シーケンス番号発番方法設定
        /// </summary>
        private HowToCreateSequenceNoParameter howToCreateSequenceNoParameter = new HowToCreateSequenceNoParameter();
        /// <summary>
        /// ソフトウエアキーボード設定
        /// </summary>
        private KeyBoardParameter keyBoardParameter = new KeyBoardParameter();
        /// <summary>
        /// 測定結果ファイル作成設定
        /// </summary>
        private MeasurementResultFileParameter measurementResultFileParameter = new MeasurementResultFileParameter();
        /// <summary>
        /// 測光設定
        /// </summary>
        private PhotometryParameter photometryParameter = new PhotometryParameter();
        /// <summary>
        /// プライム設定
        /// </summary>
        private PrimeParameter primeParameter = new PrimeParameter();
        /// <summary>
        /// プリンタ設定
        /// </summary>
        private PrinterParameter printerParameter = new PrinterParameter();
        /// <summary>
        /// 検体吸引エラー後の処理設定
        /// </summary>
        private ProcessAfterSampleAspiratingErrorParameter processAfterSampleAspiratingErrorParameter = new ProcessAfterSampleAspiratingErrorParameter();
        /// <summary>
        /// 希釈液不足時の分析状態設定
        /// </summary>
        private ProcessAtDiluentShortageParameter processAtDiluentShortageParameter = new ProcessAtDiluentShortageParameter();
        /// <summary>
        /// 試薬ロット切替わり時の処理設定
        /// </summary>
        private ProcessAtReagentLotChange processAtReagentLotChange = new ProcessAtReagentLotChange();
        /// <summary>
        /// 試薬不足時の分析の状況設定
        /// </summary>
        private ProcessAtReagentShortageParameter processAtReagentShortageParameter = new ProcessAtReagentShortageParameter();
        /// <summary>
        /// ラックID割り当て設定
        /// </summary>
        private RackIDDefinitionParameter rackIDDefinitionParameter = new RackIDDefinitionParameter();
        /// <summary>
        /// 起動時の残量チェック設定
        /// </summary>
        private ReagentCheckAtStartUpParameter reagentCheckAtStartUpParameter = new ReagentCheckAtStartUpParameter();
        /// <summary>
        /// 検体バーコードリーダー設定
        /// </summary>
        private SampleBCRParameter sampleBCRParameter = new SampleBCRParameter();
        /// <summary>
        /// サンプルラック架設部カバーオープンエラー通知時間設定
        /// </summary>
        private SampleLoaderCoverOpenErrorNotificationTimeParameter sampleLoaderCoverOpenErrorNotificationTimeParameter = new SampleLoaderCoverOpenErrorNotificationTimeParameter();
        /// <summary>
        /// 温度設定
        /// </summary>
        private TemperatureParameter temperatureParameter = new TemperatureParameter();
        /// <summary>
        /// 警告灯使用有無設定
        /// </summary>
        private WarningLightParameter warningLightParameter = new WarningLightParameter();
        /// <summary>
        /// 洗浄、分注設定
        /// </summary>
        private WashDispVolParameter washDispVolParameter = new WashDispVolParameter();
        /// <summary>
        /// 試薬ボトルの泡チェック
        /// </summary>
        private BubbleCheckParameter bubbleCheckParameter = new BubbleCheckParameter();

        /// <summary>
        /// 一般・優先検体シーケンス番号発番履歴
        /// </summary>
        private SampleSequenceNumberHistory sampleSequenceNumberHistory = new SampleSequenceNumberHistory();

		/// <summary>
		/// 洗浄液の外部からの供給
		/// </summary>
		private WashSolutionFromExterior washSolutionFromExterior = new WashSolutionFromExterior();

		/// <summary>
		/// アッセイ前のリンス実行有無
		/// </summary>
		private RinseExecutionBeforeAssay rinseExecutionBeforeAssay = new RinseExecutionBeforeAssay();

		/// <summary>
		/// 搬送ライン使用有無設定
		/// </summary>
		private TransferSystemParameter transferSystemParameter = new TransferSystemParameter();

        /// <summary>
        /// 分析モジュール接続台数
        /// </summary>
        private AssayModuleConnectParameter assayModuleConnectParameter = new AssayModuleConnectParameter();


        /// <summary>
        /// 公司LOGO的选择 add by marxsu
        /// </summary>
        private CompanyLogoParameter companyLogoParameter = new CompanyLogoParameter();

        /// <summary>
        /// 项目版本号参数 add by marxsu
        /// </summary>
        private ProtocolVersionParameter protocolVersionParameter = new ProtocolVersionParameter();

        /// <summary>
        /// XML保存パス
        /// </summary>
        private String savePath = CarisXConst.PathSystem + @"\SystemParameter.xml";
        /// <summary>
        /// デフォルトのXML読み込み先のパス
        /// </summary>
        private String defaultSavePath = CarisXConst.PathSystem + @"\SystemParameter.xml";
        /// <summary>
        /// バックアップの保存パス
        /// </summary>
        private String baukupSavePath = CarisXConst.PathBackupSystem + @"\SystemParameter.xml";


       
        #endregion

        #region [プロパティ]
        /// <summary>
        /// 保存ファイルパス
        /// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public String SavePath
        {
            get
            {
                return savePath;
            }
            set
            {
                savePath = value;
            }
        }

        /// <summary>
        /// バックアップ保存パス
        /// </summary>
        public String BackupSavePath
        {
            get
            {
                return baukupSavePath;
            }
        }

        /// <summary>
        /// デフォルトの保存ファイルパス
        /// </summary>
        public String DefaultSavePath
        {
            get
            {
                return defaultSavePath;
            }
        }

        /// <summary>
        /// 自動シャットダウン設定取得／設定
        /// </summary>
        public AutoShutdownParameter AutoShutdownParameter
        {
            get
            {
                return this.autoShutdownParameter;
            }
            set
            {
                this.autoShutdownParameter = value;
            }
        }

        /// <summary>
        /// 分析方式設定 取得/設定
        /// </summary>
        public AssayModeParameter AssayModeParameter
        {
            get
            {
                return this.assayModeParameter;
            }
            set
            {
                this.assayModeParameter = value;
            }
        }


        /// <summary>
        /// 校准方式設定 取得/設定
        /// </summary>
        public CalibrationModeParameter CalibrationModeParameter
        {
            get
            {
                return this.calibrationModeParameter;
            }
            set
            {
                this.calibrationModeParameter = value;
            }
        }

        /// <summary>
        /// 公司LOGO設定 取得/設定
        /// </summary>
        public CompanyLogoParameter CompanyLogoParameter
        {
            get
            {
                return this.companyLogoParameter;
            }
            set
            {
                this.companyLogoParameter = value;
            }
        }

        //项目版本号设置

        public ProtocolVersionParameter ProtocolVersionParameter
        {
            get
            {
                return this.protocolVersionParameter;
            }
            set
            {
                this.protocolVersionParameter = value;
            }
        }

        /// <summary>
        /// ホスト設定 取得/設定
        /// </summary>
        public HostParameter HostParameter
        {
            get
            {
                return this.hostParameter;
            }
            set
            {
                this.hostParameter = value;
            }
        }

        // 2020-02-27 CarisX IoT Add [START]
        /// <summary>
        /// IoT設定
        /// </summary>
        public IoTParameter IoTParameter
        {
            get
            {
                return this.ioTParameter;
            }
            set
            {
                this.ioTParameter = value;
            }

        }
        // 2020-02-27 CarisX IoT Add [END]

        /// <summary>
        /// 自動プライミング設定 取得/設定
        /// </summary>
        public AutoPrimeParameter AutoPrimeParameter
        {
            get
            {
                return this.autoPrimeParameter;
            }
            set
            {
                this.autoPrimeParameter = value;
            }
        }

        /// <summary>
        /// 自動起動設定 取得/設定
        /// </summary>
        public AutoStartupTimerParameter AutoStartupTimerParameter
        {
            get
            {
                return this.autoStartupTimerParameter;
            }
            set
            {
                this.autoStartupTimerParameter = value;
            }
        }

        /// <summary>
        /// サイクルタイム設定 取得/設定
        /// </summary>
        public CycleTimeParameter CycleTimeParameter
        {
            get
            {
                return this.cycleTimeParameter;
            }
            set
            {
                this.cycleTimeParameter = value;
            }
        }

        /// <summary>
        /// 装置No.設定 取得/設定
        /// </summary>
        public DeviceNoParameter DeviceNoParameter
        {
            get
            {
                return this.deviceNoParameter;
            }
            set
            {
                this.deviceNoParameter = value;
            }
        }

        /// <summary>
        /// エラー音、警告音設定 取得/設定
        /// </summary>
        public ErrWarningBeepParameter ErrWarningBeepParameter
        {
            get
            {
                return this.errWarningBeepParameter;
            }
            set
            {
                this.errWarningBeepParameter = value;
            }
        }

        /// <summary>
        /// フラッシュプライミング設定 取得/設定
        /// </summary>
        public FlushPrimeParameter FlushParameter
        {
            get
            {
                return this.flushParameter;
            }
            set
            {
                this.flushParameter = value;
            }
        }

        /// <summary>
        /// シーケンス番号発番方法 取得/設定
        /// </summary>
        public HowToCreateSequenceNoParameter HowToCreateSequenceNoParameter
        {
            get
            {
                return this.howToCreateSequenceNoParameter;
            }
            set
            {
                this.howToCreateSequenceNoParameter = value;
            }
        }

        /// <summary>
        /// ソフトウエアキーボード設定 取得/設定
        /// </summary>
        public KeyBoardParameter KeyBoardParameter
        {
            get
            {
                return this.keyBoardParameter;
            }
            set
            {
                this.keyBoardParameter = value;
            }
        }

        /// <summary>
        /// 測定結果ファイル作成設定 取得/設定
        /// </summary>
        public MeasurementResultFileParameter MeasurementResultFileParameter
        {
            get
            {
                return this.measurementResultFileParameter;
            }
            set
            {
                this.measurementResultFileParameter = value;
            }
        }

        /// <summary>
        /// 測光設定 取得/設定
        /// </summary>
        public PhotometryParameter PhotometryParameter
        {
            get
            {
                return this.photometryParameter;
            }
            set
            {
                this.photometryParameter = value;
            }
        }

        /// <summary>
        /// プライム設定 取得/設定
        /// </summary>
        public PrimeParameter PrimeParameter
        {
            get
            {
                return this.primeParameter;
            }
            set
            {
                this.primeParameter = value;
            }
        }

        /// <summary>
        /// プリンタ設定 取得/設定
        /// </summary>
        public PrinterParameter PrinterParameter
        {
            get
            {
                return this.printerParameter;
            }
            set
            {
                this.printerParameter = value;
            }
        }

        /// <summary>
        /// 検体吸引エラー後の処理設定 取得/設定
        /// </summary>
        public ProcessAfterSampleAspiratingErrorParameter ProcessAfterSampleAspiratingErrorParameter
        {
            get
            {
                return this.processAfterSampleAspiratingErrorParameter;
            }
            set
            {
                this.processAfterSampleAspiratingErrorParameter = value;
            }
        }

        /// <summary>
        /// 希釈液不足時の分析状態設定 取得/設定
        /// </summary>
        public ProcessAtDiluentShortageParameter ProcessAtDiluentShortageParameter
        {
            get
            {
                return this.processAtDiluentShortageParameter;
            }
            set
            {
                this.processAtDiluentShortageParameter = value;
            }
        }

        /// <summary>
        /// 試薬ロット切替わり時の処理設定 取得/設定
        /// </summary>
        public ProcessAtReagentLotChange ProcessAtReagentLotChange
        {
            get
            {
                return this.processAtReagentLotChange;
            }
            set
            {
                this.processAtReagentLotChange = value;
            }
        }

        /// <summary>
        /// 試薬不足時の分析の状況設定 取得/設定
        /// </summary>
        public ProcessAtReagentShortageParameter ProcessAtReagentShortageParameter
        {
            get
            {
                return this.processAtReagentShortageParameter;
            }
            set
            {
                this.processAtReagentShortageParameter = value;
            }
        }

        /// <summary>
        /// ラックID割り当て設定 取得/設定
        /// </summary>
        public RackIDDefinitionParameter RackIDDefinitionParameter
        {
            get
            {
                return this.rackIDDefinitionParameter;
            }
            set
            {
                this.rackIDDefinitionParameter = value;
            }
        }

        /// <summary>
        /// 起動時の残量チェック設定 取得/設定
        /// </summary>
        public ReagentCheckAtStartUpParameter ReagentCheckAtStartUpParameter
        {
            get
            {
                return this.reagentCheckAtStartUpParameter;
            }
            set
            {
                this.reagentCheckAtStartUpParameter = value;
            }
        }

        /// <summary>
        /// 検体バーコードリーダー設定 取得/設定
        /// </summary>
        public SampleBCRParameter SampleBCRParameter
        {
            get
            {
                return this.sampleBCRParameter;
            }
            set
            {
                this.sampleBCRParameter = value;
            }
        }

        /// <summary>
        /// サンプルラック架設部カバーオープンエラー通知時間設定 取得/設定
        /// </summary>
        public SampleLoaderCoverOpenErrorNotificationTimeParameter SampleLoaderCoverOpenErrorNotificationTimeParameter
        {
            get
            {
                return this.sampleLoaderCoverOpenErrorNotificationTimeParameter;
            }
            set
            {
                this.sampleLoaderCoverOpenErrorNotificationTimeParameter = value;
            }
        }

        /// <summary>
        /// 温度設定 取得/設定
        /// </summary>
        public TemperatureParameter TemperatureParameter
        {
            get
            {
                return this.temperatureParameter;
            }
            set
            {
                this.temperatureParameter = value;
            }
        }

        /// <summary>
        /// 警告灯使用有無設定 取得/設定
        /// </summary>
        public WarningLightParameter WarningLightParameter
        {
            get
            {
                return this.warningLightParameter;
            }
            set
            {
                this.warningLightParameter = value;
            }
        }

        /// <summary>
        /// 洗浄、分注設定 取得/設定
        /// </summary>
        public WashDispVolParameter WashDispVolParameter
        {
            get
            {
                return this.washDispVolParameter;
            }
            set
            {
                this.washDispVolParameter = value;
            }
        }

        /// <summary>
        /// シーケンス番号発番履歴 取得/設定
        /// </summary>
        public SampleSequenceNumberHistory SampleSequenceNumberHistory
        {
            get
            {
                return this.sampleSequenceNumberHistory;
            }
            set
            {
                this.sampleSequenceNumberHistory = value;
            }
        }

        /// <summary>
        /// 泡検知使用有無
        /// </summary>
        public BubbleCheckParameter BubbleCheckParameter
        {
            get
            {
                return this.bubbleCheckParameter;
            }
            set
            {
                this.bubbleCheckParameter = value;
            }
        }

		/// <summary>
		/// 洗浄液の外部からの供給
		/// </summary>
		public WashSolutionFromExterior WashSolutionFromExterior
		{
			get 
			{
				return washSolutionFromExterior; 
			}
			set 
			{
				washSolutionFromExterior = value; 
			}
		}

        /// <summary>
        /// アッセイ前のリンス実行有無
        /// </summary>
        public RinseExecutionBeforeAssay RinseExecutionBeforeAssay
        {
            get
            {
                return rinseExecutionBeforeAssay;
            }
            set
            {
                rinseExecutionBeforeAssay = value;
            }
        }

        /// <summary>
        /// 搬送ライン使用有無設定
        /// </summary>
        public TransferSystemParameter TransferSystemParameter
        {
            get
            {
                return transferSystemParameter;
            }
            set
            {
                transferSystemParameter = value;
            }
        }

        /// <summary>
        /// 分析モジュール接続台数
        /// </summary>
        public AssayModuleConnectParameter AssayModuleConnectParameter
        {
            get
            {
                return assayModuleConnectParameter;
            }
            set
            {
                assayModuleConnectParameter = value;
            }
        }

        /// <summary>
        /// ラック移動方式
        /// </summary>
        public RackMovementMethodParameter RackMovementMethodParameter { get; set; } = new RackMovementMethodParameter();

        #endregion

        #region [publicメソッド]

        ///// <summary>
        ///// パラメータ全保存
        ///// </summary>
        ///// <returns>TRUE:成功 FALSE:失敗</returns>
        //public Boolean SaveAllParameters()
        //{
        //    // TODO:パラメータ全保存

        //    return true;
        //}

        ///// <summary>
        ///// 自動シャットダウン設定保存
        ///// </summary>
        ///// <returns>TRUE:成功 FALSE:失敗</returns>
        //public Boolean SaveAutoShutdownParameter()
        //{
        //    // TODO:自動シャットダウン設定保存
        //    return true;
        //}

        ///// <summary>
        ///// パラメータ全読込
        ///// </summary>
        ///// <returns>TRUE:成功 FALSE:失敗</returns>
        //public Boolean LoadAllParameters()
        //{
        //    // TODO:パラメータ全読込
        //    return true;
        //}

        ///// <summary>
        ///// 自動シャットダウン設定読込
        ///// </summary>
        ///// <returns>TRUE:成功 FALSE:失敗</returns>
        //public Boolean LoadAutoShutdownParameter()
        //{
        //    // TODO:自動シャットダウン設定読込
        //    return true;
        //}

        #endregion

    }
}
 