using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using Oelco.Common.Utility;

namespace Oelco.CarisX
{
    [RunInstaller( true )]
    public partial class Installer : System.Configuration.Install.Installer
    {
        /// <summary>
        /// 対応カルチャ配列
        /// </summary>
        /// <remarks>
        /// 現在は英語・中国語対応</br>
        /// 非対応のカルチャで動作する際は英語扱いとする。
        /// </remarks>
        public static String[] SupportRegion = { "US", "CN" };
        // 起動時点でAPPフォルダが動的なのと、言語によりフォルダが分かれる為、一部生成も行う。
        // フォルダパスは末尾に\は付いていない。

        /// <summary>
        /// インストールプログラム保存パス
        /// </summary>
        private const String INSTALL_PATH = @"C:\CarisX\";

        /// <summary>
        /// 差し替え前DB保管用ディレクトリ名
        /// </summary>
        /// <remarks>
        /// 差し替えが発生する場合、ユニークな名称に変更する
        /// </remarks>
        private const String DIR_REPLACE_DB_BACKUP = "backup";

        /// <summary>
        /// Dataパス
        /// </summary>
        public static String PathData
        {
            get
            {
                return String.Format(@"{0}Data-{1}\Data\", INSTALL_PATH, SubFunction.GetRegionName(SupportRegion));
            }
        }
        /// <summary>
        /// DefaultDataパス
        /// </summary>
        public static String PathDefaultData
        {
            get
            {
                return String.Format(@"{0}Data-{1}\Default\Data\", INSTALL_PATH, SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// Debugパス
        /// </summary>
        public static String PathDebug
        {
            get
            {
                return String.Format(@"{0}Data-{1}\Debug\", INSTALL_PATH, SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// Paramパス
        /// </summary>
        public static String PathParam
        {
            get
            {
                return String.Format(@"{0}Data-{1}\Param\", INSTALL_PATH, SubFunction.GetRegionName(SupportRegion));
            }
        }
        /// <summary>
        /// DefaultParamパス
        /// </summary>
        public static String PathDefaultParam
        {
            get
            {
                return String.Format(@"{0}Data-{1}\Default\Param\", INSTALL_PATH, SubFunction.GetRegionName(SupportRegion));
            }
        }
        /// <summary>
        /// Systemパス
        /// </summary>
        public static String PathSystem
        {
            get
            {
                return String.Format(@"{0}Data-{1}\System\", INSTALL_PATH, SubFunction.GetRegionName(SupportRegion));
            }
        }
        /// <summary>
        /// DefaultSystemパス
        /// </summary>
        public static String PathDefaultSystem
        {
            get
            {
                return String.Format(@"{0}Data-{1}\Default\System\", INSTALL_PATH, SubFunction.GetRegionName(SupportRegion));
            }
        }
        /// <summary>
        /// Protocolパス
        /// </summary>
        public static String PathProtocol
        {
            get
            {
                return String.Format(@"{0}Data-{1}\Protocol\", INSTALL_PATH, SubFunction.GetRegionName(SupportRegion));
            }
        }
        /// <summary>
        /// DefaultProtocolパス
        /// </summary>
        public static String PathDefaultProtocol
        {
            get
            {
                return String.Format(@"{0}Data-{1}\Default\Protocol\", INSTALL_PATH, SubFunction.GetRegionName(SupportRegion));
            }
        }

        public Installer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// インストール処理
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Install( IDictionary stateSaver )
        {
            base.Install( stateSaver );

            // パラメータの初期化を行うか確認
            if( this.isRunDefaultInitialize() )
            {
                // デフォルト値でパラメータファイルを初期化
                this.defaultIntialize();

            }

            // データベースの差し替えを行うか確認
            if (this.isRunReplaceDatabase())
            {
                // データベースファイルを差し替え
                this.replaceDatabase();
            }
        }

        public override void Rollback( IDictionary savedState )
        {
            base.Rollback( savedState );

        }

        /// <summary>
        /// パラメータの初期化を行うか判定
        /// </summary>
        /// <returns>判定結果</returns>
        public Boolean isRunDefaultInitialize()
        {
            // 判定結果
            Boolean result = false;

            // 判定ファイルパス（UIPackage.xmlファイルをキーにする）
            String judgeFilePath = String.Format(@"{0}UIPackage.xml", PathParam);

            // 初期化判定用ファイルの有無を確認
            if (File.Exists(judgeFilePath) == false)
            {
                // 判定用のファイルが存在しない場合
                // => 初回インストールと見なし、デフォルト値で初期化を行う
                result = true;
            }

            return result;
        }

        /// <summary>
        /// デフォルト値で初期化
        /// </summary>
        public void defaultIntialize()
        {
            try
            {
                // Paramフォルダ内のファイル全てをコピー
                Copy(PathDefaultParam, PathParam);

                // Systemフォルダ内のファイル全てをコピー
                Copy(PathDefaultSystem, PathSystem);

                // Protocolフォルダ内のファイル全てをコピー
                Copy(PathDefaultProtocol, PathProtocol);
            }
            catch (Exception ex)
            {
                // ソース
                string sourceName = "CarisX Installer";

                // ソースが存在していない時は、作成する
                if (!System.Diagnostics.EventLog.SourceExists(sourceName))
                {
                    // ログ名を空白にすると、"Application"となる
                    System.Diagnostics.EventLog.CreateEventSource(sourceName, "");
                }

                // イベントログにエントリを書き込む
                System.Diagnostics.EventLog.WriteEntry(sourceName
                                                      , String.Format("Failure. Copy default parameters.\n{0}\n{1}", ex.Message, ex.StackTrace)
                                                      , System.Diagnostics.EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// データベースの差し替えを行うか判定
        /// </summary>
        /// <returns>判定結果</returns>
        public Boolean isRunReplaceDatabase()
        {
            // 判定結果
            Boolean result = true;

            // 判定ファイルパス（Debugフォルダ内にあるサブディレクトリをキーにする）
            String judgeFilePath = PathDebug;

            // ディレクトリ差し替え判定の有無を確認
            foreach (var directory in Directory.GetDirectories(judgeFilePath, String.Format("{0}*", DIR_REPLACE_DB_BACKUP)))
            {
                // backupフォルダが生成されている場合、DB差し替え済みのはず
                result = false;
                break;
            }

            return result;
        }

        /// <summary>
        /// データベースファイルの差し替え
        /// </summary>
        public void replaceDatabase()
        {
            try
            {
                // 削除ファイル（データベース）パスを生成
                String deleteFilePath = String.Format(@"{0}CALISX.mdf", PathData);

                // データベースファイルが存在する場合
                if (File.Exists(deleteFilePath) == true)
                {

                    // バックアップパスを生成
                    String backupFilePath = String.Format(@"{0}{1}{2}", PathDebug, DIR_REPLACE_DB_BACKUP, DateTime.Now.ToString("yyyyMMdd"));

                    // デバッグフォルダに日付付きでコピー
                    Copy(PathData, backupFilePath);

                    // ファイル削除を実施
                    File.Delete(deleteFilePath);

                    // 削除ファイル（データベースログ）パスを生成
                    // ※CALISX_log.ldfファイルがある場合、DB差し替え時に正しく起動できないため
                    deleteFilePath = String.Format(@"{0}CALISX_log.ldf", PathData);

                    // データベースログファイルが存在する場合
                    if (File.Exists(deleteFilePath) == true)
                    {
                        // ファイル削除を実施
                        File.Delete(deleteFilePath);
                    }
                }

                // Databaseファイルを差し替え
                Copy(PathDefaultData, PathData);
            }
            catch (Exception ex)
            {
                // ソース
                string sourceName = "CarisX Installer";

                // ソースが存在していない時は、作成する
                if (!System.Diagnostics.EventLog.SourceExists(sourceName))
                {
                    // ログ名を空白にすると、"Application"となる
                    System.Diagnostics.EventLog.CreateEventSource(sourceName, "");
                }

                // イベントログにエントリを書き込む
                System.Diagnostics.EventLog.WriteEntry(sourceName
                                                      , String.Format("Failure. Delete database log file.\n{0}\n{1}", ex.Message, ex.StackTrace)
                                                      , System.Diagnostics.EventLogEntryType.Error);
            }

        }

        /// <summary>
        /// ディレクトリおよびファイルコピー
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="copyPath"></param>
        public static void Copy( string sourcePath, string copyPath )
        {
            //既にディレクトリがある場合は削除し、新たにディレクトリ作成
            Directory.CreateDirectory( copyPath );

            //ファイルをコピー
            foreach (var file in Directory.GetFiles( sourcePath ))
            {
                // 上書き許可でコピー
                File.Copy(file, Path.Combine(copyPath, Path.GetFileName(file)), true);
            }

            //ディレクトリの中のディレクトリも再帰的にコピー
            foreach (var directory in Directory.GetDirectories( sourcePath ))
            {
                Copy( directory, Path.Combine( copyPath, Path.GetFileName( directory ) ) );
            }
        }
    }
}
