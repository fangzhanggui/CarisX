using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Parameter;
using Oelco.CarisX.GUI;
using Oelco.CarisX.Const;
using System.Drawing;
using Oelco.CarisX.GUI.Controls;



namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// UI設定クラス
    /// </summary>
    [Serializable()]
    public class CarisXUISettingManager : /*UISettingManager, */ISavePath
    {
        #region ISavePath メンバー

        /// <summary>
        /// 保存パス
        /// </summary>
        public String SavePath
        {
            get
            {
                return this.savePath;
            }
        }

        #endregion

        #region [インスタンス変数定義]
        /// <summary>
        /// 分析ステータス画面UI設定
        /// </summary>
        private FormAssaySettings assaySettings = new FormAssaySettings();
        /// <summary>
        /// キャリブレータ解析画面UI設定
        /// </summary>
        private FormCalibAnalysisSetting calibAnalysisSettings = new FormCalibAnalysisSetting();
        /// <summary>
        /// キャリブレータ登録画面UI設定
        /// </summary>
        private FormCalibResistrationSettings calibResistrationSettings = new FormCalibResistrationSettings();
        /// <summary>
        /// キャリブレータ測定データ画面UI設定
        /// </summary>
        private FormCalibResultSettings calibResultSettings = new FormCalibResultSettings();
        /// <summary>
        /// キャリブレータステータス画面UI設定
        /// </summary>
        private FormCalibStatusSettings calibStatusSettings = new FormCalibStatusSettings();
        /// <summary>
        /// 精度管理検体精度管理画面UI設定
        /// </summary>
        private FormControlQCSettings controlQCSettings = new FormControlQCSettings();
        /// <summary>
        /// 精度管理登録画面UI設定
        /// </summary>
        private FormControlResistrationSettings controlResistrationSettings = new FormControlResistrationSettings();
        /// <summary>
        /// 精度管理検体測定データ画面UI設定
        /// </summary>
        private FormControlResultSettings controlResultSettings = new FormControlResultSettings();
        /// <summary>
        /// 検体登録画面UI設定
        /// </summary>
        private FormSpecimenResistrationSettings specimenResistrationSettings = new FormSpecimenResistrationSettings();
        /// <summary>
        /// 検体測定データ画面UI設定
        /// </summary>
        private FormSpecimenResultSettings specimenResultSettings = new FormSpecimenResultSettings();
        /// <summary>
        /// 検体再検査画面UI設定
        /// </summary>
        private FormSpecimenRetestSettings specimenRetestSettings = new FormSpecimenRetestSettings();
        /// <summary>
        /// 検体再検査画面UI設定
        /// </summary>
        private FormStatRetestSettings statRetestSettings = new FormStatRetestSettings();
        /// <summary>
        /// システム履歴画面UI設定
        /// </summary>
        private FormSystemLogSettings systemLogSettings = new FormSystemLogSettings();
        /// <summary>
        /// ラック状態表示ダイアログUI設定
        /// </summary>
        private DlgRackViewSettings dlgRackViewSettings = new DlgRackViewSettings();
        /// <summary>
        /// 試薬(交換)テーブルダイアログUI設定
        /// </summary>
        private DlgTurnTableSettings dlgTurnTableSettings = new DlgTurnTableSettings();
        /// <summary>
        /// 分析項目パラメータ読込ダイアログUI設定
        /// </summary>
        private DlgImportMeasProtoSettings dlgImportMeasProtoSettings = new DlgImportMeasProtoSettings();
        // maintenance journal
        /// <summary>
        /// メンテナンス日誌パラメータ読込ダイアログUI設定
        /// </summary>
        private DlgMaintenanceListSettings dlgMaintenanceListSettings = new DlgMaintenanceListSettings();
        // maintenance journal
        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CarisXUISettingManager()
        {

            //// 有効タイプリスト作成
            //Type typeTmp;

            //// FormSpecimenResistrationSettings設定項目生成
            //typeTmp = typeof( FormSpecimenResistrationSettings );
            //this.SettingList.Add( typeTmp, new FormSpecimenResistrationSettings() );
            //this.TypeDic.Add( typeTmp, this.SettingList[typeTmp] );


        }
        #endregion

        #region [プロパティ]
        /// <summary>
        /// 分析ステータス画面UI設定の取得、設定
        /// </summary>
        public FormAssaySettings AssaySettings
        {
            get
            {
                return this.assaySettings;
            }
            set
            {
                assaySettings = value;
            }
        }

        /// <summary>
        /// キャリブレータ解析画面UI設定の取得、設定
        /// </summary>
        public FormCalibAnalysisSetting CalibAnalysisSettings
        {
            get
            {
                return this.calibAnalysisSettings;
            }
            set
            {
                calibAnalysisSettings = value;
            }
        }

        /// <summary>
        /// キャリブレータ登録画面UI設定の取得、設定
        /// </summary>
        public FormCalibResistrationSettings CalibResistrationSettings
        {
            get
            {
                return this.calibResistrationSettings;
            }
            set
            {
                calibResistrationSettings = value;
            }
        }

        /// <summary>
        /// キャリブレータ測定データ画面UI設定の取得、設定
        /// </summary>
        public FormCalibResultSettings CalibResultSettings
        {
            get
            {
                return this.calibResultSettings;
            }
            set
            {
                calibResultSettings = value;
            }
        }

        /// <summary>
        /// キャリブレータステータス画面UI設定の取得、設定
        /// </summary>
        public FormCalibStatusSettings CalibStatusSettings
        {
            get
            {
                return this.calibStatusSettings;
            }
            set
            {
                calibStatusSettings = value;
            }
        }

        /// <summary>
        /// 精度管理検体精度管理画面UI設定の取得、設定
        /// </summary>
        public FormControlQCSettings ControlQCSettings
        {
            get
            {
                return this.controlQCSettings;
            }
            set
            {
                controlQCSettings = value;
            }
        }

        /// <summary>
        /// 精度管理登録画面UI設定の取得、設定
        /// </summary>
        public FormControlResistrationSettings ControlResistrationSettings
        {
            get
            {
                return this.controlResistrationSettings;
            }
            set
            {
                controlResistrationSettings = value;
            }
        }

        /// <summary>
        /// 精度管理検体測定データ画面UI設定の取得、設定
        /// </summary>
        public FormControlResultSettings ControlResultSettings
        {
            get
            {
                return this.controlResultSettings;
            }
            set
            {
                controlResultSettings = value;
            }
        }

        /// <summary>
        /// 検体登録画面UI設定の取得、設定
        /// </summary>
        public FormSpecimenResistrationSettings SpecimenResistrationSettings
        {
            get
            {
                return this.specimenResistrationSettings;
            }
            set
            {
                specimenResistrationSettings = value;
            }
        }

        /// <summary>
        /// 検体測定データ画面UI設定の取得、設定
        /// </summary>
        public FormSpecimenResultSettings SpecimenResultSettings
        {
            get
            {
                return specimenResultSettings;
            }
            set
            {
                specimenResultSettings = value;
            }
        }

        /// <summary>
        /// 検体再検査画面UI設定の取得、設定
        /// </summary>
        public FormSpecimenRetestSettings SpecimenRetestSettings
        {
            get
            {
                return specimenRetestSettings;
            }
            set
            {
                specimenRetestSettings = value;
            }
        }

        /// <summary>
        /// STAT検体再検査画面UI設定の取得、設定
        /// </summary>
        public FormStatRetestSettings StatRetestSettings
        {
            get
            {
                return statRetestSettings;
            }
            set
            {
                statRetestSettings = value;
            }
        }

        /// <summary>
        /// システム履歴画面UI設定の取得、設定
        /// </summary>
        public FormSystemLogSettings SystemLogSettings
        {
            get
            {
                return systemLogSettings;
            }
            set
            {
                systemLogSettings = value;
            }
        }

        /// <summary>
        /// ラック状態表示ダイアログUI設定の取得、設定
        /// </summary>
        public DlgRackViewSettings RackViewSettings
        {
            get
            {
                return dlgRackViewSettings;
            }
            set
            {
                dlgRackViewSettings = value;
            }
        }

        /// <summary>
        /// 試薬(交換)テーブルダイアログUI設定の取得、設定
        /// </summary>
        public DlgTurnTableSettings TurnTableSettings
        {
            get
            {
                return dlgTurnTableSettings;
            }
            set
            {
                dlgTurnTableSettings = value;
            }
        }

        /// <summary>
        /// 分析項目パラメータ読込ダイアログUI設定の取得、設定
        /// </summary>
        public DlgImportMeasProtoSettings DlgImportMeasProtoSettings
        {
            get
            {
                return dlgImportMeasProtoSettings;
            }
            set
            {
                dlgImportMeasProtoSettings = value;
            }
        }

        /// <summary>
        /// メンテナンス日誌分析項目パラメータ読込ダイアログUI設定の取得、設定
        /// </summary>
        public DlgMaintenanceListSettings MaintenanceListSettings
        {
            get
            {
                return dlgMaintenanceListSettings;
            }
            set
            {
                dlgMaintenanceListSettings = value;
            }
        }
        


        #endregion


        //public Boolean GetUISettings( IUISetting settingUI ) 
        //{
        //    Boolean result = true;
        //    //Type []settingType = new Type[]{ settingUI.GetSettingsType()};
        //    //Type setting = typeof(IUISetting<>).MakeGenericType( settingType );
        //    //setting.
        //    //Activator.CreateInstance(setting,

        //    // 設定取得
        //    Type settingType = settingUI.GetSettingsType();
        //    if ( this.typeDic.ContainsKey( settingType ) )
        //    {
        //        this.typeDic[settingType] = settingUI.GetSettings();
        //    }
        //    else
        //    {
        //        result = false;
        //    }

        //    return result;
        //}
        //public Boolean SetUISettings( IUISetting settingUI )
        //{
        //    Boolean result = true;

        //    // 設定セット
        //    Type settingType = settingUI.GetSettingsType();
        //    if ( this.typeDic.ContainsKey( settingType ) )
        //    {
        //        settingUI.SetSettings( this.typeDic[settingType] );
        //    }
        //    else
        //    {
        //        result = false;
        //    }

        //    return result;

        //}

        /// <summary>
        /// 保存パス
        /// </summary>
        String savePath = CarisXConst.PathSystem + @"\UISetting.xml"; // TODO:UIPackageのようなパス

    }

    ///// <summary>
    ///// UI設定インターフェース
    ///// </summary>
    ///// <remarks>
    ///// UI設定項目の取得/設定を行います
    ///// </remarks>
    ///// <typeparam name="SettingType">設定データクラス名</typeparam>
    //public interface IUISetting
    //{
    //    Type GetSettingsType();
    //    Object GetSettings();
    //    void SetSettings( Object setting );
    //}
    //public interface IUISettingsTypeGet
    //{
    //    Type GetSettingsType();
    //}


    /// <summary>
    /// 分析ステータス画面UI設定
    /// </summary>
    //[Serializable()]
    public class FormAssaySettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// grdSpecimenグリッド列表示順
        /// </summary>
        private List<String> specimenGridColOrder = new List<String>();
        /// <summary>
        /// grdCalibratorグリッド列表示順
        /// </summary>
        private List<String> calibGridColOrder = new List<String>();
        /// <summary>
        /// grdControlグリッド列表示順
        /// </summary>
        private List<String> controlGridColOrder = new List<String>();
        /// <summary>
        /// grdReagentBottleグリッド列表示順
        /// </summary>
        private List<String> reagentBottleGridColOrder = new List<String>();
        /// <summary>
        /// grdSpecimenグリッド列幅
        /// </summary>
        private List<Int32> specimengridColWidth = new List<Int32>();
        /// <summary>
        /// grdCalibratorグリッド列幅
        /// </summary>
        private List<Int32> calibgridColWidth = new List<Int32>();
        /// <summary>
        /// grdControlグリッド列幅
        /// </summary>
        private List<Int32> controlgridColWidth = new List<Int32>();
        /// <summary>
        /// grdReagentBottleグリッド列幅
        /// </summary>
        private List<Int32> reagentBottleGridColWidth = new List<Int32>();
        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if ( this.gridZoom == 0 )
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }

        /// <summary>
        /// grdSpecimenグリッド列表示順の取得、設定
        /// </summary>
        public List<String> SpecimenGridColOrder
        {
            get
            {
                return this.specimenGridColOrder;
            }
            set
            {
                this.specimenGridColOrder = value;
            }
        }
        /// <summary>
        /// grdCalibratorグリッド列表示順の取得、設定
        /// </summary>
        public List<String> CalibGridColOrder
        {
            get
            {
                return this.calibGridColOrder;
            }
            set
            {
                this.calibGridColOrder = value;
            }
        }
        /// <summary>
        /// grdControlグリッド列表示順の取得、設定
        /// </summary>
        public List<String> ControlGridColOrder
        {
            get
            {
                return this.controlGridColOrder;
            }
            set
            {
                this.controlGridColOrder = value;
            }
        }

        /// <summary>
        /// grdReagentBottleグリッド列表示順の取得、設定
        /// </summary>
        public List<String> ReagentBottleGridColOrder
        {
            get
            {
                return reagentBottleGridColOrder;
            }
            set
            {
                reagentBottleGridColOrder = value;
            }
        }

        /// <summary>
        /// grdSpecimenグリッド列幅の取得、設定
        /// </summary>
        public List<Int32> SpecimenGridColWidth
        {
            get
            {
                return this.specimengridColWidth;
            }
            set
            {
                this.specimengridColWidth = value;
            }
        }

        /// <summary>
        /// grdCalibratorグリッド列幅の取得、設定
        /// </summary>
        public List<Int32> CalibGridColWidth
        {
            get
            {
                return this.calibgridColWidth;
            }
            set
            {
                this.calibgridColWidth = value;
            }
        }

        /// <summary>
        /// grdControlグリッド列幅の取得、設定
        /// </summary>
        public List<Int32> ControlGridColWidth
        {
            get
            {
                return this.controlgridColWidth;
            }
            set
            {
                this.controlgridColWidth = value;
            }
        }

        /// <summary>
        /// grdReagentBottleグリッド列幅の取得、設定
        /// </summary>
        public List<Int32> ReagentBottleGridColWidth
        {
            get
            {
                return reagentBottleGridColWidth;
            }
            set
            {
                reagentBottleGridColWidth = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// キャリブレータ解析画面UI設定
    /// </summary>
    //[Serializable()]
    public class FormCalibAnalysisSetting
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// グリッド列表示順
        /// </summary>
        private List<String> gridColOrder = new List<String>();
        /// <summary>
        /// グリッド列幅
        /// </summary>
        private List<Int32> gridColWidth = new List<Int32>();
        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if ( this.gridZoom == 0 )
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }

        /// <summary>
        /// グリッド列表示順の取得、設定
        /// </summary>
        public List<String> GridColOrder
        {
            get
            {
                return this.gridColOrder;
            }
            set
            {
                this.gridColOrder = value;
            }
        }

        /// <summary>
        /// グリッド列幅の取得、設定
        /// </summary>
        public List<Int32> GridColWidth
        {
            get
            {
                return this.gridColWidth;
            }
            set
            {
                this.gridColWidth = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// キャリブレータ登録画面UI設定
    /// </summary>
    //[Serializable()]
    public class FormCalibResistrationSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// グリッド列表示順
        /// </summary>
        private List<String> gridColOrder = new List<String>();
        /// <summary>
        /// グリッド列幅
        /// </summary>
        private List<Int32> gridColWidth = new List<Int32>();
        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if ( this.gridZoom == 0 )
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }

        /// <summary>
        /// グリッド列表示順の取得、設定
        /// </summary>
        public List<String> GridColOrder
        {
            get
            {
                return this.gridColOrder;
            }
            set
            {
                this.gridColOrder = value;
            }
        }

        /// <summary>
        /// グリッド列幅の取得、設定
        /// </summary>
        public List<Int32> GridColWidth
        {
            get
            {
                return this.gridColWidth;
            }
            set
            {
                this.gridColWidth = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// キャリブレータ測定データ画面UI設定
    /// </summary>
    //[Serializable()]
    public class FormCalibResultSettings : IExportSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// グリッド列表示順
        /// </summary>
        private List<String> gridColOrder = new List<String>();
        /// <summary>
        /// グリッド列幅
        /// </summary>
        private List<Int32> gridColWidth = new List<Int32>();
        /// <summary>
        /// ファイル出力先
        /// </summary>
        private String exportPath = CarisXConst.PathExport;
        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if ( this.gridZoom == 0 )
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }

        /// <summary>
        /// グリッド列表示順の取得、設定
        /// </summary>
        public List<String> GridColOrder
        {
            get
            {
                return this.gridColOrder;
            }
            set
            {
                this.gridColOrder = value;
            }
        }

        /// <summary>
        /// グリッド列幅の取得、設定
        /// </summary>
        public List<Int32> GridColWidth
        {
            get
            {
                return this.gridColWidth;
            }
            set
            {
                this.gridColWidth = value;
            }
        }

        /// <summary>
        /// ファイル出力先の取得、設定
        /// </summary>
        public String ExportPath
        {
            get
            {
                return this.exportPath;
            }
            set
            {
                this.exportPath = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// キャリブレータステータス画面UI設定
    /// </summary>
    //[Serializable()]
    public class FormCalibStatusSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// グリッド列表示順
        /// </summary>
        private List<String> gridColOrder = new List<String>();
        /// <summary>
        /// グリッド列幅
        /// </summary>
        private List<Int32> gridColWidth = new List<Int32>();
        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if ( this.gridZoom == 0 )
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }

        /// <summary>
        /// グリッド列表示順の取得、設定
        /// </summary>
        public List<String> GridColOrder
        {
            get
            {
                return this.gridColOrder;
            }
            set
            {
                this.gridColOrder = value;
            }
        }

        /// <summary>
        /// グリッド列幅の取得、設定
        /// </summary>
        public List<Int32> GridColWidth
        {
            get
            {
                return this.gridColWidth;
            }
            set
            {
                this.gridColWidth = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// 精度管理検体精度管理画面UI設定
    /// </summary>
    //[Serializable()]
    public class FormControlQCSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// グリッド列表示順
        /// </summary>
        private List<String> gridColOrder = new List<String>();
        /// <summary>
        /// グリッド列幅
        /// </summary>
        private List<Int32> gridColWidth = new List<Int32>();
        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if ( this.gridZoom == 0 )
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }

        /// <summary>
        /// グリッド列表示順の取得、設定
        /// </summary>
        public List<String> GridColOrder
        {
            get
            {
                return this.gridColOrder;
            }
            set
            {
                this.gridColOrder = value;
            }
        }

        /// <summary>
        /// グリッド列幅の取得、設定
        /// </summary>
        public List<Int32> GridColWidth
        {
            get
            {
                return this.gridColWidth;
            }
            set
            {
                this.gridColWidth = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// 精度管理登録画面UI設定
    /// </summary>
    //[Serializable()]
    public class FormControlResistrationSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// グリッド列表示順
        /// </summary>
        private List<String> gridColOrder = new List<String>();
        /// <summary>
        /// グリッド列幅
        /// </summary>
        private List<Int32> gridColWidth = new List<Int32>();

        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if ( this.gridZoom == 0 )
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }

        /// <summary>
        /// グリッド列表示順の取得、設定
        /// </summary>
        public List<String> GridColOrder
        {
            get
            {
                return this.gridColOrder;
            }
            set
            {
                this.gridColOrder = value;
            }
        }

        /// <summary>
        /// グリッド列幅の取得、設定
        /// </summary>
        public List<Int32> GridColWidth
        {
            get
            {
                return this.gridColWidth;
            }
            set
            {
                this.gridColWidth = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// 精度管理検体測定データ画面UI設定
    /// </summary>
    //[Serializable()]
    public class FormControlResultSettings : IExportSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// グリッド列表示順
        /// </summary>
        private List<String> gridColOrder = new List<String>();
        /// <summary>
        /// グリッド列幅
        /// </summary>
        private List<Int32> gridColWidth = new List<Int32>();
        /// <summary>
        /// ファイル出力先
        /// </summary>
        private String exportPath = CarisXConst.PathExport;
        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if ( this.gridZoom == 0 )
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }

        /// <summary>
        /// グリッド列表示順の取得、設定
        /// </summary>
        public List<String> GridColOrder
        {
            get
            {
                return this.gridColOrder;
            }
            set
            {
                this.gridColOrder = value;
            }
        }

        /// <summary>
        /// グリッド列幅の取得、設定
        /// </summary>
        public List<Int32> GridColWidth
        {
            get
            {
                return this.gridColWidth;
            }
            set
            {
                this.gridColWidth = value;
            }
        }
        /// <summary>
        /// ファイル出力先の取得、設定
        /// </summary>
        public String ExportPath
        {
            get
            {
                return this.exportPath;
            }
            set
            {
                this.exportPath = value;
            }

        }
        #endregion

    }

    /// <summary>
    /// 検体登録画面UI設定
    /// </summary>
    //[Serializable()]
    public class FormSpecimenResistrationSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// グリッド列表示順
        /// </summary>
        private List<String> gridColOrder = new List<String>();
        /// <summary>
        /// グリッド列幅
        /// </summary>
        private List<Int32> gridColWidth = new List<Int32>();
        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if ( this.gridZoom == 0 )
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }

        /// <summary>
        /// グリッド列表示順の取得、設定
        /// </summary>
        public List<String> GridColOrder
        {
            get
            {
                return this.gridColOrder;
            }
            set
            {
                this.gridColOrder = value;
            }
        }

        /// <summary>
        /// グリッド列幅の取得、設定
        /// </summary>
        public List<Int32> GridColWidth
        {
            get
            {
                return this.gridColWidth;
            }
            set
            {
                this.gridColWidth = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// 検体測定データ画面UI設定
    /// </summary>
    //[Serializable()]
    public class FormSpecimenResultSettings : IExportSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// グリッド列表示順
        /// </summary>
        private List<String> gridColOrder = new List<String>();
        /// <summary>
        /// グリッド列幅
        /// </summary>
        private List<Int32> gridColWidth = new List<Int32>();
        /// <summary>
        /// ファイル出力先
        /// </summary>
        private String exportPath = CarisXConst.PathExport;
        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if ( this.gridZoom == 0 )
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }

        /// <summary>
        /// グリッド列表示順の取得、設定
        /// </summary>
        public List<String> GridColOrder
        {
            get
            {
                return this.gridColOrder;
            }
            set
            {
                this.gridColOrder = value;
            }
        }

        /// <summary>
        /// グリッド列幅の取得、設定
        /// </summary>
        public List<Int32> GridColWidth
        {
            get
            {
                return this.gridColWidth;
            }
            set
            {
                this.gridColWidth = value;
            }
        }

        /// <summary>
        /// ファイル出力先の取得、設定
        /// </summary>
        public String ExportPath
        {
            get
            {
                return this.exportPath;
            }
            set
            {
                this.exportPath = value;
            }

        }
        #endregion

    }

    /// <summary>
    /// 検体再検査画面UI設定
    /// </summary>
    //[Serializable()]
    public class FormSpecimenRetestSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// グリッド列表示順
        /// </summary>
        private List<String> gridColOrder = new List<String>();
        /// <summary>
        /// グリッド列幅
        /// </summary>
        private List<Int32> gridColWidth = new List<Int32>();
        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if ( this.gridZoom == 0 )
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }

        /// <summary>
        /// グリッド列表示順の取得、設定
        /// </summary>
        public List<String> GridColOrder
        {
            get
            {
                return this.gridColOrder;
            }
            set
            {
                this.gridColOrder = value;
            }
        }

        /// <summary>
        /// グリッド列幅の取得、設定
        /// </summary>
        public List<Int32> GridColWidth
        {
            get
            {
                return this.gridColWidth;
            }
            set
            {
                this.gridColWidth = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// STAT検体再検査画面UI設定
    /// </summary>
    //[Serializable()]
    public class FormStatRetestSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// グリッド列表示順
        /// </summary>
        private List<String> gridColOrder = new List<String>();
        /// <summary>
        /// グリッド列幅
        /// </summary>
        private List<Int32> gridColWidth = new List<Int32>();
        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if (this.gridZoom == 0)
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }

        /// <summary>
        /// グリッド列表示順の取得、設定
        /// </summary>
        public List<String> GridColOrder
        {
            get
            {
                return this.gridColOrder;
            }
            set
            {
                this.gridColOrder = value;
            }
        }

        /// <summary>
        /// グリッド列幅の取得、設定
        /// </summary>
        public List<Int32> GridColWidth
        {
            get
            {
                return this.gridColWidth;
            }
            set
            {
                this.gridColWidth = value;
            }
        }
        #endregion
    }

    /// <summary>
    /// システム履歴画面UI設定
    /// </summary>
    //[Serializable()]
    public class FormSystemLogSettings : IExportSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// grdErrorLogグリッド列表示順
        /// </summary>
        private List<String> errorgridColOrder = new List<String>();
        /// <summary>
        /// grdErrorLogグリッド列幅
        /// </summary>
        private List<Int32> errorgridColWidth = new List<Int32>();
        /// <summary>
        /// grdOperationLogグリッド列表示順
        /// </summary>
        private List<String> operationGridColOrder = new List<String>();
        /// <summary>
        /// grdOnlineLogグリッド列表示順
        /// </summary>
        private List<String> onlineGridColOrder = new List<String>();
        /// <summary>
        /// grdOnlineLogグリッド列幅
        /// </summary>
        private List<Int32> onlineGridColWidth = new List<Int32>();
        /// <summary>
        /// grdOperationLogグリッド列幅
        /// </summary>
        private List<Int32> operationGridColWidth = new List<Int32>();
        /// <summary>
        /// grdParameterChangeLogグリッド列表示順
        /// </summary>
        private List<String> parameterChangeGridColOrder = new List<String>();
        /// <summary>
        /// grdParameterChangeLogグリッド列幅
        /// </summary>
        private List<Int32> parameterChangeGridColWidth = new List<Int32>();
        /// <summary>
        /// grdAssayLogグリッド列表示順
        /// </summary>
        private List<String> assaygridColOrder = new List<String>();
        /// <summary>
        /// grdAssayLogグリッド列幅
        /// </summary>
        private List<Int32> assaygridColWidth = new List<Int32>();
        /// <summary>
        /// ファイル出力先
        /// </summary>
        private String exportPath = CarisXConst.PathLog;
        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if ( this.gridZoom == 0 )
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }
        /// <summary>
        /// grdErrorLogグリッド列表示順の取得、設定
        /// </summary>
        public List<String> ErrorGridColOrder
        {
            get
            {
                return this.errorgridColOrder;
            }
            set
            {
                this.errorgridColOrder = value;
            }
        }
        /// <summary>
        /// grdErrorLogグリッド列幅の取得、設定
        /// </summary>
        public List<Int32> ErrorGridColWidth
        {
            get
            {
                return this.errorgridColWidth;
            }
            set
            {
                this.errorgridColWidth = value;
            }
        }
        /// <summary>
        /// grdOperationLogグリッド列表示順の取得、設定
        /// </summary>
        public List<String> OperationGridColOrder
        {
            get
            {
                return this.operationGridColOrder;
            }
            set
            {
                this.operationGridColOrder = value;
            }
        }
        /// <summary>
        /// grdOnlineLogグリッド列表示順の取得、設定
        /// </summary>
        public List<String> OnlinerGridColOrder
        {
            get
            {
                return this.onlineGridColOrder;
            }
            set
            {
                this.onlineGridColOrder = value;
            }
        }
        /// <summary>
        /// grdOnlineLogグリッド列幅の取得、設定
        /// </summary>
        public List<Int32> OnlineGridColWidth
        {
            get
            {
                return this.onlineGridColWidth;
            }
            set
            {
                this.onlineGridColWidth = value;
            }
        }
        /// <summary>
        /// grdOperationLogグリッド列幅の取得、設定
        /// </summary>
        public List<Int32> OperationGridColWidth
        {
            get
            {
                return this.operationGridColWidth;
            }
            set
            {
                this.operationGridColWidth = value;
            }
        }

        /// <summary>
        /// grdParameterChangeLogグリッド列表示順の取得、設定
        /// </summary>
        public List<String> ParameterChangeGridColOrder
        {
            get
            {
                return this.parameterChangeGridColOrder;
            }
            set
            {
                this.parameterChangeGridColOrder = value;
            }
        }
        /// <summary>
        /// grdParameterChangeLogグリッド列幅の取得、設定
        /// </summary>
        public List<Int32> ParameterChangeGridColWidth
        {
            get
            {
                return this.parameterChangeGridColWidth;
            }
            set
            {
                this.parameterChangeGridColWidth = value;
            }
        }
        /// <summary>
        /// grdAssayLogグリッド列表示順の取得、設定
        /// </summary>
        public List<String> AssayGridColOrder
        {
            get
            {
                return this.assaygridColOrder;
            }
            set
            {
                this.assaygridColOrder = value;
            }
        }
        /// <summary>
        /// grdAssayLogグリッド列幅の取得、設定
        /// </summary>
        public List<Int32> AssayGridColWidth
        {
            get
            {
                return this.assaygridColWidth;
            }
            set
            {
                this.assaygridColWidth = value;
            }
        }
        /// <summary>
        /// ファイル出力先の取得、設定の取得、設定
        /// </summary>
        public String ExportPath
        {
            get
            {
                return this.exportPath;
            }
            set
            {
                this.exportPath = value;
            }

        }
        #endregion

    }

    /// <summary>
    /// ラック状態表示ダイアログUI設定
    /// </summary>
    //[Serializable()]
    public class DlgRackViewSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// グリッド列表示順(一般/優先)
        /// </summary>
        private List<String> gridColOrderSpecimen = new List<String>();
        /// <summary>
        /// グリッド列幅(一般/優先)
        /// </summary>
        private List<Int32> gridColWidthSpecimen = new List<Int32>();
        /// <summary>
        /// グリッド列表示順(キャリブレータ)
        /// </summary>
        private List<String> gridColOrderCalibrator = new List<String>();
        /// <summary>
        /// グリッド列幅(キャリブレータ)
        /// </summary>
        private List<Int32> gridColWidthCalibrator = new List<Int32>();
        /// <summary>
        /// グリッド列表示順(コントロール)
        /// </summary>
        private List<String> gridColOrderControl = new List<String>();
        /// <summary>
        /// グリッド列幅(コントロール)
        /// </summary>
        private List<Int32> gridColWidthControl = new List<Int32>();
        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if ( this.gridZoom == 0 )
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }
        /// <summary>
        /// グリッド列表示順(一般/優先検体)の取得、設定
        /// </summary>
        public List<String> GridColOrderSpecimen
        {
            get
            {
                return this.gridColOrderSpecimen;
            }
            set
            {
                this.gridColOrderSpecimen = value;
            }
        }
        /// <summary>
        /// グリッド列幅(一般/優先検体)の取得、設定
        /// </summary>
        public List<Int32> GridColWidthSpecimen
        {
            get
            {
                return this.gridColWidthSpecimen
                ;
            }
            set
            {
                this.gridColWidthSpecimen = value;
            }
        }
        /// <summary>
        /// グリッド列表示順(キャリブレータ)の取得、設定
        /// </summary>
        public List<String> GridColOrderCalibrator
        {
            get
            {
                return this.gridColOrderCalibrator;
            }
            set
            {
                this.gridColOrderCalibrator = value;
            }
        }
        /// <summary>
        /// グリッド列幅(キャリブレータ)の取得、設定
        /// </summary>
        public List<Int32> GridColWidthCalibrator
        {
            get
            {
                return this.gridColWidthCalibrator;
            }
            set
            {
                this.gridColWidthCalibrator = value;
            }
        }
        /// <summary>
        /// グリッド列表示順(コントロール)の取得、設定
        /// </summary>
        public List<String> GridColOrderControl
        {
            get
            {
                return this.gridColOrderControl;
            }
            set
            {
                this.gridColOrderControl = value;
            }
        }
        /// <summary>
        /// グリッド列幅(コントロール)の取得、設定
        /// </summary>
        public List<Int32> GridColWidthControl
        {
            get
            {
                return this.gridColWidthControl;
            }
            set
            {
                this.gridColWidthControl = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// 試薬(交換)テーブルダイアログUI設定
    /// </summary>
    //[Serializable()]
    public class DlgTurnTableSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// grdReagentList1グリッド列表示順
        /// </summary>
        private List<String> reagentList1GridColOrder = new List<String>();
        /// <summary>
        /// grdReagentList1グリッド列幅
        /// </summary>
        private List<Int32> reagentList1gridColWidth = new List<Int32>();
        /// <summary>
        /// grdReagentList2グリッド列表示順
        /// </summary>
        private List<String> reagentList2GridColOrder = new List<String>();
        /// <summary>
        /// grdReagentList2グリッド列幅
        /// </summary>
        private List<Int32> reagentList2gridColWidth = new List<Int32>();
        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if ( this.gridZoom == 0 )
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }
        /// <summary>
        /// grdReagentList1グリッド列表示順の取得、設定
        /// </summary>
        public List<String> ReagentList1GridColOrder
        {
            get
            {
                return this.reagentList1GridColOrder;
            }
            set
            {
                this.reagentList1GridColOrder = value;
            }
        }
        /// <summary>
        /// grdReagentList1グリッド列幅の取得、設定
        /// </summary>
        public List<Int32> ReagentList1GridColWidth
        {
            get
            {
                return this.reagentList1gridColWidth;
            }
            set
            {
                this.reagentList1gridColWidth = value;
            }
        }
        /// <summary>
        /// grdReagentList2グリッド列表示順の取得、設定
        /// </summary>
        public List<String> ReagentList2GridColOrder
        {
            get
            {
                return this.reagentList2GridColOrder;
            }
            set
            {
                this.reagentList2GridColOrder = value;
            }
        }
        /// <summary>
        /// grdReagentList2グリッド列幅の取得、設定
        /// </summary>
        public List<Int32> ReagentList2GridColWidth
        {
            get
            {
                return this.reagentList2gridColWidth;
            }
            set
            {
                this.reagentList2gridColWidth = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// 分析プロトコル読込ダイアログ画面UI設定
    /// </summary>
    //[Serializable()]
    public class DlgImportMeasProtoSettings : IExportSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// ファイル出力先
        /// </summary>
        private String exportPath = CarisXConst.PathLog;
        #endregion

        #region [プロパティ]
       
        /// <summary>
        /// ファイル出力先の取得、設定
        /// </summary>
        public String ExportPath
        {
            get
            {
                return this.exportPath;
            }
            set
            {
                this.exportPath = value;
            }

        }
        #endregion

    }
    /// <summary>
    /// ファイル出力設定
    /// </summary>
    public class ExportSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// ファイル出力先
        /// </summary>
        protected String exportPath = CarisXConst.PathData;
        #endregion

        #region [プロパティ]
        /// <summary>
        /// ファイル出力先の取得、設定
        /// </summary>
        public String ExportPath
        {
            get
            {
                return this.exportPath;
            }
            set
            {
                this.exportPath = value;
            }

        }
        #endregion
    }

    // maintenance journal
    /// <summary>
    /// メンテナンス日誌リストダイアログUI設定
    /// </summary>
    //[Serializable()]
    public class DlgMaintenanceListSettings
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// グリッド倍率
        /// </summary>
        private Int32 gridZoom;
        /// <summary>
        /// グリッド列表示順
        /// </summary>
        private List<String> gridColOrder = new List<String>();
        /// <summary>
        /// グリッド列幅
        /// </summary>
        private List<Int32> gridColWidth = new List<Int32>();
        #endregion

        #region [プロパティ]
        /// <summary>
        /// グリッド倍率の取得、設定
        /// </summary>
        public Int32 GridZoom
        {
            get
            {
                if (this.gridZoom == 0)
                {
                    this.gridZoom = 100;
                }
                return this.gridZoom;
            }
            set
            {
                this.gridZoom = value;
            }
        }
        /// <summary>
        /// grdMaintenanceListグリッド列表示順の取得、設定
        /// </summary>
        public List<String> GridColOrder
        {
            get
            {
                return this.gridColOrder;
            }
            set
            {
                this.gridColOrder = value;
            }
        }
        /// <summary>
        /// grdMaintenanceグリッド列幅の取得、設定
        /// </summary>
        public List<Int32> GridColWidth
        {
            get
            {
                return this.gridColWidth;
            }
            set
            {
                this.gridColWidth = value;
            }
        }
        #endregion

    }
    // maintenance journal

    /// <summary>
    /// ファイル出力設定
    /// </summary>
    public interface IExportSettings
    {
        #region [プロパティ]
        /// <summary>
        /// ファイル出力先の取得、設定
        /// </summary>
        String ExportPath
        {
            get;
            set;
        }
        #endregion
    }
}
