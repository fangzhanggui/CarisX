using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Parameter;
using System.Net;
using Oelco.CarisX.Const;
using Oelco.Common.Comm;
using Oelco.Common.Utility;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// アプリケーション設定
    /// </summary>
    /// <remarks>
    /// アプリケーションの動作に関わる設定を定義します。
    /// </remarks>
    public class AppSettings : ISavePath
    {
        /// <summary>
        /// ファイル保存パス
        /// </summary>
        private const String APPSETTINGS_PATH = @"\UIPackage.xml";

        /// <summary>
        /// 検体番号設定
        /// </summary>
        public SampleNoInfo SampleNoInfo = new SampleNoInfo();

        /// <summary>
        /// シーケンス番号(一般検体)設定
        /// </summary>
        public SampleNoInfo SequencialSampleNoInfo = new SampleNoInfo();
        /// <summary>
        /// シーケンス番号(優先検体)設定
        /// </summary>
        public PriorityNoInfo SequencialPrioritySampleNoInfo = new PriorityNoInfo();        
        /// <summary>
        /// シーケンス番号(キャリブレータ)設定
        /// </summary>
        public CalibNoInfo SequencialCalibNoInfo = new CalibNoInfo();
        /// <summary>
        /// シーケンス番号(精度管理検体)設定
        /// </summary>
        public ControlNoInfo SequencialControlNoInfo = new ControlNoInfo();
        /// <summary>
        /// 受付番号設定
        /// </summary>
        public ReceiptNoInfo ReceiptNoInfo = new ReceiptNoInfo();
        /// <summary>
        /// ユニーク番号設定
        /// </summary>
        public UniqueNoInfo UniqueNoInfo = new UniqueNoInfo();
        /// <summary>
        /// 個体識別番号設定
        /// </summary>
        public IndividuallyNoInfo IndividuallyNoInfo = new IndividuallyNoInfo();
        /// <summary>
        /// スレーブ通信設定
        /// </summary>
        //public SocketParameter SlaveCommSettings = new SocketParameter();
        /// <summary>
        /// ラック搬送通信設定
        /// </summary>
        //public SocketParameter RackTransferCommSettings = new SocketParameter();
        /// <summary>
        /// 通信設定
        /// </summary>
        public ConnectSettings ConnectSettings;

        /// <summary>
        /// 日替わり処理確認日時
        /// </summary>
        public DateTime DateChange = new DateTime( 0 );
        
        /// <summary>
        /// 前回正常終了フラグ
        /// </summary>
        public Boolean SystemCondition = true;

        /// <summary>
        /// DBアクセス設定
        /// </summary>
        public DBAccessSettings DbAccess = new DBAccessSettings();

        /// <summary>
        /// TabletPC入力パネル設定用レジストリキー
        /// </summary>
        public String RegistryKeyTextInputPanel = "ShowIPTipTarget";

        /// <summary>
        /// 初期シーケンス省略フラグ
        /// </summary>
        public Boolean SkipInitializeSequence = false;

        /// <summary>
        /// 試薬バーコード読取設定
        /// </summary>
        public ReadReagentBC ReadReagentBC = new ReadReagentBC();

        /// <summary>
        /// デバッグモードフラグ
        /// </summary>
        public Boolean DebugMode = false;

        #region ISavePath メンバー

        /// <summary>
        /// ファイル保存パス
        /// </summary>
        public String SavePath
        {
            get
            {
                return CarisXConst.PathParam + APPSETTINGS_PATH;
            }
        }

        /// <summary>
        /// バックアップファイル保存パス
        /// </summary>
        public String BackupSavePath
        {
            get
            {
                return CarisXConst.PathBackupParam + APPSETTINGS_PATH;
            }
        }

        #endregion
    }

    ///// <summary>
    ///// 対スレーブ通信設定
    ///// </summary>
    //public class SlaveCommmSettings
    //{
    //    /// <summary>
    //    /// IPアドレス
    //    /// </summary>
    //    public IPAddress ipAddress;

    //    /// <summary>
    //    /// ポート番号
    //    /// </summary>
    //    public Int32 portNo;

    //}

    /// <summary>
    /// DBアクセス設定
    /// </summary>
    /// <remarks>
    /// DBアクセスに関する設定を保持します。
    /// </remarks>
    public class DBAccessSettings
    {
        /// <summary>
        /// DBサーバインスタンス名称
        /// </summary>
        public String InstanceName = String.Empty;
        /// <summary>
        /// DB名称
        /// </summary>
        public String Databasename = String.Empty;

    }

    ///// <summary>
    ///// 対Comm通信設定
    ///// </summary>
    //public class HostCommSettings
    //{
    //    // 対Comm通信設定内容
    //}

    /// <summary>
    /// 連番設定情報
    /// </summary>
    /// <remarks>
    /// 連番クラス関連の設定情報の共通項目を定義します。
    /// 連番クラス関連の設定情報はここに定義する項目全てを必要とする場合、継承して定義してください。
    /// 連番クラス関連の設定情報は必ずしもこのクラスを継承する必要は無い為、このクラスが基底となっている前提の処理を作成しないでください。
    /// ※このクラスを保持ではなく継承としているのは、XmlSerializerにより出力されるXmlファイルのレイアウトを考慮している為です。
    /// </remarks>
    public class SequencialNumberGeneralInfo
    {
        ///// <summary>
        ///// 最大値
        ///// </summary>
        //public Int32 CountMax = Int32.MaxValue;

        ///// <summary>
        ///// 最小値
        ///// </summary>
        //public Int32 CountMin = 1;

        /// <summary>
        /// 現在値
        /// </summary>
        public Int32 CountNow = 1;

        /// <summary>
        /// 最新発番時刻
        /// </summary>
        public DateTime LatestNumberDate;
    }


    /// <summary>
    /// 受付番号設定
    /// </summary>
    /// <remarks>
    /// 受付番号の情報を定義します。
    /// </remarks>
    public class ReceiptNoInfo : SequencialNumberGeneralInfo
    {
        // 現在個別項目無し（シリアライザによる出力Xmlのレイアウト調整や、情報の意味合いとして個別にあるべきなので定義している。）
    }
    /// <summary>
    /// ユニーク番号設定
    /// </summary>
    /// <remarks>
    /// ユニーク番号の情報を定義します。
    /// </remarks>
    public class UniqueNoInfo : SequencialNumberGeneralInfo
    {
        // 現在個別項目無し（シリアライザによる出力Xmlのレイアウト調整や、情報の意味合いとして個別にあるべきなので定義している。）
    }
    /// <summary>
    /// シーケンス番号（一般検体）設定
    /// </summary>
    /// <remarks>
    /// シーケンス番号（一般検体）の情報を定義します。
    /// </remarks>
    public class SampleNoInfo : SequencialNumberGeneralInfo
    {
        // 現在個別項目無し（シリアライザによる出力Xmlのレイアウト調整や、情報の意味合いとして個別にあるべきなので定義している。）
    } 
    /// <summary>
    /// シーケンス番号（優先検体）設定
    /// </summary>
    /// <remarks>
    /// シーケンス番号（優先検体）の情報を定義します。
    /// </remarks>
    public class PriorityNoInfo : SequencialNumberGeneralInfo
    {
        // 現在個別項目無し（シリアライザによる出力Xmlのレイアウト調整や、情報の意味合いとして個別にあるべきなので定義している。）
    } 
    /// <summary>
    /// シーケンス番号（キャリブレータ）設定
    /// </summary>
    /// <remarks>
    /// シーケンス番号（キャリブレータ）の情報を定義します。
    /// </remarks>
    public class CalibNoInfo : SequencialNumberGeneralInfo
    {
        // 現在個別項目無し（シリアライザによる出力Xmlのレイアウト調整や、情報の意味合いとして個別にあるべきなので定義している。）
    }
    /// <summary>
    /// シーケンス番号（精度管理検体）設定
    /// </summary>
    /// <remarks>
    /// シーケンス番号（精度管理検体）の情報を定義します。
    /// </remarks>
    public class ControlNoInfo : SequencialNumberGeneralInfo
    {
        // 現在個別項目無し（シリアライザによる出力Xmlのレイアウト調整や、情報の意味合いとして個別にあるべきなので定義している。）
    }  
    /// <summary>
    /// 個体識別番号設定
    /// </summary>
    /// <remarks>
    /// 個体識別番号の情報を定義します。
    /// </remarks>
    public class IndividuallyNoInfo : SequencialNumberGeneralInfo
    {
        // TODO:個体識別番号必要情報定義
    }


    /// <summary>
    /// 接続設定クラス
    /// </summary>
    public class ConnectSettings
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConnectSettings()
        {
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// スレーブ
        /// </summary>
        public List<SocketParameter> SlaveCommSettings { get; set; }

        /// <summary>
        /// ラック搬送
        /// </summary>
        public List<SocketParameter> RackTransferCommSettings { get; set; }

        /// <summary>
        /// スレーブ接続最大数分の設定を取得する
        /// </summary>
        /// <returns></returns>
        public List<SocketParameter> GetSlaveCommSettings()
        {
            List<SocketParameter> result = new List<SocketParameter>();

            foreach(SocketParameter param in this.SlaveCommSettings)
            {
                result.Add(param);
                Int32 slaveConnectMax = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;
                if (result.Count >= slaveConnectMax)
                {
                    break;
                }
            }

            return result;
        }

        #endregion
    }

    /// <summary>
    /// 試薬バーコード読込設定クラス
    /// </summary>
    public class ReadReagentBC
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ReadReagentBC()
        {
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// スレーブ
        /// </summary>
        public List<ReadBCSetting> Module { get; set; } = new List<ReadBCSetting>();

        /// <summary>
        /// 指定したモジュールIndexの試薬バーコード読取の設定を取得する
        /// </summary>
        /// <param name="moduleIndex">モジュールIndex</param>
        /// <returns>設定値</returns>
        public int[] GetReadReagentBC( int moduleIndex )
        {
            // デフォルト値は全て「読取あり」
            int[] result = new int[CarisXConst.REAGENT_PORT_MAX];

            if (this.Module.Count > moduleIndex)
            {
                for (int portNoIndex = 0; portNoIndex < CarisXConst.REAGENT_PORT_MAX; portNoIndex++)
                {
                    // チェックするビット位置を指定
                    Int64 checkPortBit = ( 1 << portNoIndex );

                    // 指定ビットが立っているか確認
                    if (( this.Module[moduleIndex].BitReadBC & checkPortBit ) != 0)
                    {
                        // ビットが立っている場合、「読取なし」
                        result[portNoIndex] = (int)ReadReagentBCFlag.NotRead;
                    }
                }
            }
            else
            {
                this.Module.Add( new ReadBCSetting() );
            }

            return result;
        }

        /// <summary>
        /// 指定したモジュール番号の試薬バーコード読取の設定を上書きする
        /// </summary>
        /// <param name="moduleIndex">モジュールIndex</param>
        /// <param name="setData">設定値</param>
        public Boolean SetReadReagentBC( int moduleIndex, int[] setData )
        {
            // nullチェック
            if (setData == null)
            {
                return false;
            }

            // 範囲チェック
            if (setData.Count() != CarisXConst.REAGENT_PORT_MAX)
            {
                return false;
            }

            if (this.Module.Count > moduleIndex)
            {
                for (int portNoIndex = 0; portNoIndex < CarisXConst.REAGENT_PORT_MAX; portNoIndex++)
                {
                    // チェックするビット位置を指定
                    Int64 checkPortBit = ( 1 << portNoIndex );

                    // 読取有無確認
                    if (setData[portNoIndex] == (int)ReadReagentBCFlag.NotRead)
                    {
                        // 読取なし => 指定のビットを立てる
                        this.Module[moduleIndex].BitReadBC |= checkPortBit;
                    }
                    else
                    {
                        // 読取あり => 指定のビットを消す
                        this.Module[moduleIndex].BitReadBC &= ( ~checkPortBit );
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 試薬バーコード読込設定クラス
        /// </summary>
        public class ReadBCSetting
        {
            #region [コンストラクタ/デストラクタ]

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ReadBCSetting()
            {

            }

            #endregion

            #region [プロパティ]

            /// <summary>
            /// スレーブ
            /// </summary>
            public Int64 BitReadBC = 0;

            #endregion
        }

        #endregion
    }
}
