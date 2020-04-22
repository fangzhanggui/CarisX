

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Oelco.Common.Utility;
using System.Globalization;
using Oelco.CarisX.Common;
using Oelco.CarisX.Parameter;
using Oelco.Common.Comm;
using Infragistics.Win.UltraWinGrid;
using Oelco.Common.Const;
using Oelco.CarisX.Utility;

namespace Oelco.CarisX.Const
{
    /// <summary>
    /// CarisX用定数クラス
    /// </summary>
	static public class CarisXConst
    {
        #region "ファイルパス"

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
        /// スレーブプログラム保存パス
        /// </summary>
        public static String PathSlaveProgram
        {
            get
            {
                return String.Format(PATH_SLAVE_PROGRAM, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// データ保存パス
        /// </summary>
        public static String PathData
        {
            get
            {
                return String.Format(PATH_DATA, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }
        /// <summary>
        /// エラー画像保存パス
        /// </summary>
        public static String PathErrImage
        {
            get
            {
                return String.Format(PATH_ERR_IMAGE, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// メンテナンス日誌画像保存パス
        /// </summary>
        public static String PathMaintenanceJournalImage
        {
            get
            {
                return String.Format(PATH_MAINTENANCEJOURNAL_IMAGE, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// パラメータ保存パス
        /// </summary>
        public static String PathParam
        {
            get
            {
                return String.Format(PATH_PARAM, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// システム設定保存パス
        /// </summary>
        public static String PathSystem
        {
            get
            {
                return String.Format(PATH_SYSTEM, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// 分析項目設定保存パス
        /// </summary>
        public static String PathProtocol
        {
            get
            {
                return String.Format(PATH_PROTOCOL, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// Help保存パス
        /// </summary>
        public static String PathHelp
        {
            get
            {
                return String.Format(PATH_HELP, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// エラーメッセージ保存パス
        /// </summary>
        public static String PathError
        {
            get
            {
                return String.Format(PATH_ERROR, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// メンテナンス日誌メッセージ保存パス
        /// </summary>
        public static String PathMaintenanceJournal
        {
            get
            {
                return String.Format(PATH_MAINTENANCEJOURNAL, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// メンテナンス日誌ユーザーモジュール1保存パス
        /// </summary>
        public static String PathMaintenanceJournalUserModule1
        {
            get
            {
                return String.Format(PATH_MAINTENANCEJOURNAL_USER_MODULE1, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// メンテナンス日誌ユーザーモジュール2保存パス
        /// </summary>
        public static String PathMaintenanceJournalUserModule2
        {
            get
            {
                return String.Format(PATH_MAINTENANCEJOURNAL_USER_MODULE2, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// メンテナンス日誌ユーザーモジュール3保存パス
        /// </summary>
        public static String PathMaintenanceJournalUserModule3
        {
            get
            {
                return String.Format(PATH_MAINTENANCEJOURNAL_USER_MODULE3, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// メンテナンス日誌ユーザーモジュール4保存パス
        /// </summary>
        public static String PathMaintenanceJournalUserModule4
        {
            get
            {
                return String.Format(PATH_MAINTENANCEJOURNAL_USER_MODULE4, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        // <summary>
        /// メンテナンス日誌サービスマンモジュール1保存パス
        /// </summary>
        public static String PathMaintenanceJournalServicemanModule1
        {
            get
            {
                return String.Format(PATH_MAINTENANCEJOURNAL_SERVICEMAN_MODULE1, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// メンテナンス日誌サービスマンモジュール2保存パス
        /// </summary>
        public static String PathMaintenanceJournalServicemanModule2
        {
            get
            {
                return String.Format(PATH_MAINTENANCEJOURNAL_SERVICEMAN_MODULE2, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// メンテナンス日誌サービスマンモジュール3保存パス
        /// </summary>
        public static String PathMaintenanceJournalServicemanModule3
        {
            get
            {
                return String.Format(PATH_MAINTENANCEJOURNAL_SERVICEMAN_MODULE3, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// メンテナンス日誌サービスマンモジュール4保存パス
        /// </summary>
        public static String PathMaintenanceJournalServicemanModule4
        {
            get
            {
                return String.Format(PATH_MAINTENANCEJOURNAL_SERVICEMAN_MODULE4, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// ログ保存パス
        /// </summary>
        public static String PathLog
        {
            get
            {
                return String.Format(PATH_LOG, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// 用于保存在线日志
        /// </summary>
        public static String PathOnline//Figu:
        {
            get
            {
                return String.Format(ONLINE_LOG, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// デバッグログ保存パス
        /// </summary>
        public static String PathDebug
        {
            get
            {
                return String.Format(PATH_DEBUG, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// 帳票出力用項目保存パス
        /// </summary>
        public static String PathPrint
        {
            get
            {
                return String.Format(PATH_PRINT, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// CSVファイル他保存パス(既定)
        /// </summary>
        public static String PathExport
        {
            get
            {
                return String.Format(PATH_EXPORT, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        // 2020-02-27 CarisX IoT Add [START]
        /// <summary>
        /// IoTアップロードファイルの一時フォルダパス
        /// </summary>
        public static String PathTemp
        {
            get
            {
                return String.Format(PATH_TEMP, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }
        // 2020-02-27 CarisX IoT Add [END]


        /// <summary>
        /// デフォルトDataパス
        /// </summary>
        public static String PathDefaultData
        {
            get
            {
                return String.Format(PATH_DEFAULT_DATA, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// デフォルトParamパス
        /// </summary>
        public static String PathDefaultParam
        {
            get
            {
                return String.Format(PATH_DEFAULT_PARAM, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// デフォルトSystemパス
        /// </summary>
        public static String PathDefaultSystem
        {
            get
            {
                return String.Format(PATH_DEFAULT_SYSTEM, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// デフォルトProtocolパス
        /// </summary>
        public static String PathDefaultProtocol
        {
            get
            {
                return String.Format(PATH_DEFAULT_PROTOCOL, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// デフォルトHelpパス
        /// </summary>
        public static String PathDefaultHelp
        {
            get
            {
                return String.Format(PATH_DEFAULT_HELP, SubFunction.GetApplicationDirectory(), SubFunction.GetRegionName(SupportRegion));
            }
        }

        /// <summary>
        /// ImportExe保存パス
        /// </summary>
        public static String PathImportProtoExe
        {
            get
            {
                return String.Format(PATH_IMPORTPROTO_EXE, SubFunction.GetApplicationDirectory());
            }
        }

        /// <summary>
        /// バックアップパス
        /// </summary>
        public static String PathBackup
        {
            get
            {
                return String.Format(PATH_BACKUP, SubFunction.GetApplicationDirectory());
            }
        }
        /// <summary>
        /// バックアップParamパス
        /// </summary>
        public static String PathBackupParam
        {
            get
            {
                return String.Format(PATH_BACKUP_PARAM, SubFunction.GetApplicationDirectory());
            }
        }

        /// <summary>
        /// バックアップSystemパス
        /// </summary>
        public static String PathBackupSystem
        {
            get
            {
                return String.Format(PATH_BACKUP_SYSTEM, SubFunction.GetApplicationDirectory());
            }
        }

        /// <summary>
        /// バックアップProtocolパス
        /// </summary>
        public static String PathBackupProtocol
        {
            get
            {
                return String.Format(PATH_BACKUP_PROTOCOL, SubFunction.GetApplicationDirectory());
            }
        }

        /// <summary>
        /// スレーブプログラム保存パス
        /// </summary>
        private const String PATH_SLAVE_PROGRAM = @"{0}Data-{1}";
        /// <summary>
        /// データ保存パス
        /// </summary>
        private const String PATH_DATA = @"{0}Data-{1}\Data";
        /// <summary>
        /// エラー画像保存パス
        /// </summary>
        private const String PATH_ERR_IMAGE = @"{0}Data-{1}\ErrImage";
        /// <summary>
        /// メンテナンス日誌画像保存パス
        /// </summary>
        private const String PATH_MAINTENANCEJOURNAL_IMAGE = @"{0}Data-{1}\MaintenanceJournalImage";
        /// <summary>
        /// パラメータ保存パス
        /// </summary>
        private const String PATH_PARAM = @"{0}Data-{1}\Param";
        /// <summary>
        /// システム設定保存パス
        /// </summary>
        private const String PATH_SYSTEM = @"{0}Data-{1}\System";
        /// <summary>
        /// 分析項目設定保存パス
        /// </summary>
        private const String PATH_PROTOCOL = @"{0}Data-{1}\Protocol";
        /// <summary>
        /// Help保存パス
        /// </summary>
        private const String PATH_HELP = @"{0}Data-{1}\Help";
        /// <summary>
        /// エラーメッセージ保存パス
        /// </summary>
        private const String PATH_ERROR = @"{0}Data-{1}\Err";
        /// <summary>
        /// メンテナンス日誌メッセージ保存パス
        /// </summary>
        private const String PATH_MAINTENANCEJOURNAL = @"{0}Data-{1}\MaintenanceJournal";
        /// <summary>
        /// メンテナンス日誌ファイル出力モジュール1保存パス
        /// </summary>
        private const String PATH_MAINTENANCEJOURNAL_USER_MODULE1 = @"{0}Data-{1}\MaintenanceJournal\User\Module1";
        /// <summary>
        /// メンテナンス日誌ファイル出力モジュール2保存パス
        /// </summary>
        private const String PATH_MAINTENANCEJOURNAL_USER_MODULE2 = @"{0}Data-{1}\MaintenanceJournal\User\Module2";
        /// <summary>
        /// メンテナンス日誌ファイル出力モジュール3保存パス
        /// </summary>
        private const String PATH_MAINTENANCEJOURNAL_USER_MODULE3 = @"{0}Data-{1}\MaintenanceJournal\User\Module3";
        /// <summary>
        /// メンテナンス日誌ファイル出力モジュール4保存パス
        /// </summary>
        private const String PATH_MAINTENANCEJOURNAL_USER_MODULE4 = @"{0}Data-{1}\MaintenanceJournal\User\Module4";
        /// <summary>
        /// メンテナンス日誌ファイル出力サービスマンモジュール1保存パス
        /// </summary>
        private const String PATH_MAINTENANCEJOURNAL_SERVICEMAN_MODULE1 = @"{0}Data-{1}\MaintenanceJournal\Service\Module1";
        /// <summary>
        /// メンテナンス日誌ファイル出力サービスマンモジュール2保存パス
        /// </summary>
        private const String PATH_MAINTENANCEJOURNAL_SERVICEMAN_MODULE2 = @"{0}Data-{1}\MaintenanceJournal\Service\Module2";
        /// <summary>
        /// メンテナンス日誌ファイル出力サービスマンモジュール3保存パス
        /// </summary>
        private const String PATH_MAINTENANCEJOURNAL_SERVICEMAN_MODULE3 = @"{0}Data-{1}\MaintenanceJournal\Service\Module3";
        /// <summary>
        /// メンテナンス日誌ファイル出力サービスマンモジュール4保存パス
        /// </summary>
        private const String PATH_MAINTENANCEJOURNAL_SERVICEMAN_MODULE4 = @"{0}Data-{1}\MaintenanceJournal\Service\Module4";
        /// <summary>
        /// ログ保存パス
        /// </summary>
        private const String PATH_LOG = @"{0}Data-{1}\Log";
        /// <summary>
        /// 保存在线日志
        /// </summary>
        private const String ONLINE_LOG = @"{0}Data-{1}\Online";//Figu:
                                                                /// <summary>
                                                                /// デバッグログ保存パス
                                                                /// </summary>
        private const String PATH_DEBUG = @"{0}Data-{1}\Debug";
        /// <summary>
        /// 帳票出力項目保存パス
        /// </summary>
        private const String PATH_PRINT = @"{0}Data-{1}\Print";
        /// <summary>
        /// CSVファイル他保存パス(既定)
        /// </summary>
        private const String PATH_EXPORT = @"{0}Data-{1}\Export";
        // 2020-02-27 CarisX IoT Add [START]
        /// <summary>
        /// IoT一時ファイルディレクトリパス
        /// </summary>
        private const String PATH_TEMP = @"{0}Data-{1}\Temp";
        // 2020-02-27 CarisX IoT Add [END]
        /// <summary>
        /// デフォルトDataパス
        /// </summary>
        private const String PATH_DEFAULT_DATA = @"{0}Data-{1}\Default\Data";
        /// <summary>
        /// デフォルトParamパス
        /// </summary>
        private const String PATH_DEFAULT_PARAM = @"{0}Data-{1}\Default\Param";
        /// <summary>
        /// デフォルトSystemパス
        /// </summary>
        private const String PATH_DEFAULT_SYSTEM = @"{0}Data-{1}\Default\System";
        /// <summary>
        /// デフォルトProtocolパス
        /// </summary>
        private const String PATH_DEFAULT_PROTOCOL = @"{0}Data-{1}\Default\Protocol";
        /// <summary>
        /// デフォルトHelpパス
        /// </summary>
        private const String PATH_DEFAULT_HELP = @"{0}Data-{1}\Default\Help";
        /// <summary>
        /// ImportExe保存パス
        /// </summary>
        private const String PATH_IMPORTPROTO_EXE = @"{0}ImportProtoExe";
        /// <summary>
        /// バックアップパス
        /// </summary>
        private const String PATH_BACKUP = @"{0}backup";
        /// <summary>
        /// バックアップParamパス
        /// </summary>
        private const String PATH_BACKUP_PARAM = @"{0}backup\Param";
        /// <summary>
        /// バックアップSystemパス
        /// </summary>
        private const String PATH_BACKUP_SYSTEM = @"{0}backup\System";
        /// <summary>
        /// バックアップProtocolパス
        /// </summary>
        private const String PATH_BACKUP_PROTOCOL = @"{0}backup\Protocol";
        #endregion

        /// <summary>
        /// ユーザプログラムバージョン(オプション画面表示)
        /// 
        #region [--- old version ---]
        /// Ver0.00.01: 初版作成
        /// Ver0.00.02: 開発中の不具合修正（詳細は割愛）
        /// Ver0.00.03: Caris200 Ver.1.20.27修正を反映
        /// Ver0.00.04: メンテナンス画面のConfiguration、Motor Parameterを実装
        /// Ver0.00.05: 開発中の不具合修正（詳細は割愛）
        /// Ver0.00.06: 開発中の不具合修正（詳細は割愛）
        /// Ver0.00.07: 開発中の不具合修正（詳細は割愛）
        /// Ver0.00.08: 開発中の不具合修正（詳細は割愛）
        /// Ver0.00.09: 開発中の不具合修正（詳細は割愛）
        /// Ver0.01.00: 結合試験1.01完了版
        /// Ver0.01.01: 初期シーケンス実装
        /// Ver0.01.02: 初期シーケンスを一時的に停止
        /// Ver0.01.03: 初期シーケンスを復活
        /// Ver0.01.04: 開発中の不具合修正（詳細は割愛）
        /// Ver0.01.05: モーターパラメータのオフセットを見直し
        /// Ver0.01.06: 開発中の不具合修正（詳細は割愛）
        /// Ver0.01.07: モーター調整の加算・減算を最新化
        /// Ver0.01.08: 開発中の不具合修正（詳細は割愛）
        /// Ver0.01.09: 開発中の不具合修正（詳細は割愛）
        /// Ver0.01.10: 開発中の不具合修正（詳細は割愛）
        /// Ver0.01.11: 開発中の不具合修正（詳細は割愛）
        /// Ver0.01.12: 開発中の不具合修正（詳細は割愛）
        /// Ver0.01.13: 開発中の不具合修正（詳細は割愛）
        /// Ver0.01.14: 反応容器搬送部ユニットのコンフィグパラメータをトラベラー・廃棄部へ移動
        /// Ver0.01.15: 初期シーケンスが完了後、ユーザーかシミュレータのどちらかを再起動すると
        /// 　　　　　　再実行された初期シーケンスが正常に処理できない件を修正
        /// Ver0.01.16: モーター調整のスレーブとの結合テストでの指摘事項を反映
        /// Ver0.01.17: サンプル分注、R1、R2、その他のユニットテストにパラメータを追加
        /// Ver0.01.18: メンテナンス画面起動時のコンフィグパラメータの通信部分を改修
        /// Ver0.01.19: 実機でのユニットテストで、一時停止を行った際に画面が一時停止→ユニットテスト開始前に戻る件を修正
        /// Ver0.01.20: モーターパラメータ保存時にコマンドが送信されなくなっていたものがあったのを修正
        /// Ver0.01.21: ラック搬送のユニットテストシーケンスの英語リソースを変更
        /// Ver0.01.22: 温度ユニットで温度問合せ中に別ユニットへ遷移できるよう変更
        /// Ver0.01.23: ケース搬送ユニットのユニットテストにSequence7を追加
        /// Ver0.01.24: サンプル分注のモーター調整で、チップNoに91/96を指定できるよう変更
        /// Ver0.01.25: メイン画面のレイアウトを調整
        /// Ver0.01.26: Assay Start時にRack No Useにチェックされた場合はRackへのコマンド送信をしないよう変更
        /// 　　　　　　System Status画面で、SampleBarcodeの保存時にラックへコマンドが送信されるよう変更
        /// Ver0.01.27: ラックの残量コマンド（0108）に対してレスポンスを返すよう設定（内部処理は未対応）
        /// Ver0.01.28: コマンドリスト0.82の内容を反映
        /// Ver0.01.29: 分析終了時にラック搬送へラック排出コマンドを送信するよう変更
        ///             ユーザーを起動している状態でラックの初期シーケンスを一度終了後、再度ラックの初期シーケンスが実行された際に
        ///             初期シーケンスダイアログが閉じなくなる現象を修正
        /// Ver0.01.30: PID定数設定画面の仕様を変更。画面の値をxmlから取得するようにした。（追々やめる可能性あり）
        /// Ver0.01.31: メイン画面のタイトルが再ログイン時にも残っている件を修正
        ///             各画面のツールバーの末尾に区切り線が表示されるよう変更
        ///             Specimen Registration画面のツールバーはオンマウスで青色にならないよう変更
        ///             メンテナンスでレスポンスを受信してログファイルを作成した際、Data-XX\Log\MainteRspフォルダ配下にファイルを作成するよう変更
        ///             システム設定-温度設定にて、反応テーブルおよびB/Fテーブルの設定項目を追加
        ///             Rack No Useにチェックを入れている場合、0502コマンドを受信した際に一部の項目に固定で値を設定するよう変更
        /// Ver0.01.32: 各画面のボタンに対して、オンマウス、クリック、ドラッグが発生した際にボーダー線が表示されないように変更
        ///             ProtocolSetting画面のRegularタブから消えてたAssay Conditionの内容を復活（Detailタブに移動していた）
        ///             コマンドのロット番号がint型だったため、String型に修正 
        ///             ツールバーにオンマウスした際に青色の枠を表示しないよう変更
        /// Ver0.01.33: メンテナンス画面のユニットテストタブに「Repeat Interval」を追加。繰り返し実行時の間隔を指定できるようにした。
        /// Ver0.01.34: [暫定対応] 顧客デモ向けにAFP(1)、HBsAb(36)、HBsAg(37)のみプロトコル送信する。
        /// Ver0.01.35: 初期シーケンスの処理中にラック・モジュールを再起動したら、二重に初期シーケンスが動いてしまう問題を対応。
        ///             システムログ画面のレイアウトを調整（仕様が確定次第再度調整は必要）
        /// Ver0.01.36: コマンドリストver0.83を反映。（0106コマンドの内部処理は除く）
        /// Ver0.01.37: メンテナンス画面終了時に全モーター初期化コマンドを発行して待機するよう変更
        /// Ver0.01.39: Reagnet画面で交換開始してバーコードを入力する際、必ず２１桁入力とし、かつ２１桁目（＝容量）には１のみ入力可能とするよう変更
        ///             Assay Status画面のWaste TankにReagentに保存しているラックの情報を表示するよう変更
        ///             メンテナンス画面終了時にモーター初期化するかどうかの確認ダイアログを表示するよう修正
        ///             プロトコル設定画面に後希釈倍率の項目を追加
        ///             DB定義書に合うよう各DBクラスの内容を修正
        ///             メンテナンス画面を起動する際に、コンフィグパラメータのやり取りが終了するまで画面を表示しないよう修正
        ///             サンプル分注のユニットコンフィグに項目を追加
        ///             プロトコルパラメータコマンドにプロトコル希釈倍率を追加
        /// Ver0.01.40: 試薬履歴情報（ReagentHistory）を追加
        ///             試薬情報に試薬種の項目を追加
        ///             試薬情報の残量、有効期限の設定を試薬履歴情報を考慮した処理に変更
        ///             CalibRegist画面のラックポジションの番号を左から順に１～５になるよう変更
        ///             AssayStatus画面でReagent明細を選択しても青色にならないよう変更
        ///             AssayStatus画面のSpecimen、control、Calibratorの明細を選択した時に青色ではなくオレンジになるよう変更
        ///             各画面の明細のスクロールバーのスタイルをOffice2013に統一
        ///             ラック搬送と接続された時にも初期シーケンスのダイアログが表示されるよう変更
        ///             モジュールに送信するプロトコルパラメータコマンドの対象を追加
        ///             AssayLogにバックグラウンドカウントと測定カウントをそれぞれコンテンツ５、６に保存するよう変更
        ///             AnalysisSettingPanelに全プロトコルが表示されない件を修正
        ///             EditRegistInfoでDEBUG時だけ繰返し回数を１～２０で指定できるよう変更
        ///             一般検体のAssayで登録時に指定した繰返し回数に対して、測定結果が正しく処理出来ていなかった件を修正
        ///             初期シーケンス時のソフトウェア識別コマンドを応答が返るまでリトライする仕組みを追加
        ///             中国語文字列リソースを英語文字列リソースに差し替え
        ///             AssayLogの列を、ユーザーレベル５の時だけコンテンツ６～７（BGカウント、測定カウント）を表示するよう変更
        ///             AssayLogのモジュール番号に１～４の値が設定されるよう修正
        ///             ControlRegistrationで、画面のボタンコントロール数以上に有効な分析項目が存在する場合、ボタンコントロール数以上は処理しないよう変更
        /// Ver0.01.41: モジュールIDの定義見直し
        ///             エラー履歴のファイル出力にて例外発生していたので修正
        ///             エラー履歴および分析履歴のグリッドにモジュール番号ではなくモジュール名を表示するように変更
        ///             DEBUG時は、ファイルを保存する際に暗号化しないよう修正
        ///             試薬準備完了レスポンスコマンドにパラメータを追加
        ///             試薬設置状況通知コマンド（0520）を実装
        ///             試薬種詳細（M,R1,R2,T1,T2）の内容をDBに保存するよう変更
        ///             デバッグ用の処理が残っていた箇所を削除
        ///             廃液バッファ、廃液タンク、洗浄液バッファ、洗浄液タンクの交換に対応（パラメータ追加はまだ）
        ///             Cosumable List画面の内容をCarisXの内容に合わせて修正
        ///             assay結果として受信したダークカウント、バックグラウンドカウント、測定カウントをDBに保持し、ユーザーレベルが５：Developerの場合のみ画面に表示するよう変更
        ///             反応容器設置確認(内側)センサと反応容器設置確認(設置部)センサのラベルが逆転していた件を修正
        ///             レッドクロス問題の一次対策として、残テスト数＞最大テスト数の場合に最大テスト数を設定し、ログを出力するよう変更
        ///             分析ステータスのCommandTextメソッドが動作してしまう場合があった為、一時的に封印
        ///             Assay Status画面のReagent明細に値を設定する際、最大テスト数をそのまま設定するよう修正
        /// Ver0.01.42: Reagent画面のタンク交換用画面を作成
        ///             廃液セット交換開始、廃液セット交換完了、洗浄液供給の各コマンドに、タンクかバッファを指定するパラメータを追加
        ///             メンテナンス画面-Other-Configurationタブに試薬残量クリアボタンおよび処理を追加
        ///             残量系コマンドから、廃液バッファ有無チェックを削除
        ///             廃液タンク、廃液バッファ、廃棄ボックス、洗浄液タンク、洗浄液バッファの画像の表示内容を見直し
        ///             廃液タンク状態問合せコマンド(0521)を実装
        ///             初期シーケンスでエンコード許容量、A Posの内容をXmlに保存されるように修正
        ///             R1、R2でモーター調整時にエンコード許容量の値が正しく送信されるよう修正
        ///             センサーステータス問合せ(0440)、センサー無効コマンド(0441)にセンサーを追加
        ///             センサーステータス、センサー使用有無の画面に、新規追加のセンサーの内容を反映
        ///             測定データコマンド受信時にリマークからのエラーコードが生成された際にエラー通知処理にて例外が発生する問題を修正
        ///             試薬準備完了時に1417コマンドの結果を正しく処理出来ていなかった問題を修正（仮）
        ///             メンテナンス画面で反応テーブル、BFテーブルにある攪拌Zθ軸のモーターパラメータの入力制御を変更
        /// Ver0.01.43: エラーコード45、105を受信した際にAssay画面の洗浄液タンクのアイコンを赤色に変更、洗浄液タンクの交換完了操作時点で緑にもどるよう変更
        ///             Assay Status画面でトリガー１、２の内容がどちらもトリガー１だった件を修正
        ///             ラックからサブイベントを受信してもシステムステータスを変更しないよう変更
        ///             Reagent Listの画面デザインをCarisXに合わせて修正
        ///             1417のパラメータ変更に対応。パラメータにあわせてM、R1、R2の試薬残量をクリアする
        ///             試薬交換時にTurnさせた時の明細の背景色が青色になっていたのでオレンジに変更
        ///             メンテナンス画面の調整
        ///                 反応テーブルユニットテスト…攪拌：戻り値追加
        ///                 BFテーブルユニットテスト…攪拌：戻り値追加
        ///                 R1/R2ユニットテスト…吸引・吐出：戻り値追加
        ///                 サンプル分注ユニットテスト…引数変更、戻り値変更、番号削除・繰り上げ
        ///             reagentHistoryにreagentCodeを追加
        ///             システムオプション画面（モジュール毎）画面および、試薬バーコード読取設定画面、試薬バーコード情報入力画面を追加
        ///             スレーブ初期シーケンスに試薬保冷庫BC読み込み無効コマンド（0493）の送信処理を追加
        ///             UIPackage.iniに試薬バーコード読取設定パラメータを新規追加
        ///             システムオプション画面にある以下の項目をシステムオプション画面（モジュール毎）へ移動
        ///                 消耗品の確認・障害対策・分注器エージング・END処理・アナライザ温度
        /// Ver0.01.44: サンプル分注移送部サンプルシリンジモーターにGain(>100uL)、Offset(>100uL)を追加
        ///             Analytes画面のデザインを調整
        ///             TipCellViewのバーの背景色を透過色に変更
        ///             Consumable ListのDiluentの単位をTests→mLに変更
        ///             検体登録時の希釈倍率設定で、8000倍以上は指定不可にするよう変更
        /// Ver0.01.45: 試薬種詳細（M、R1、R2、T1、T2）、分析項目（１～２００）毎に履歴が４００件を超えれば設置日が古いものから削除するよう変更
        ///             プロトコル選択画面の画面デザインをAnalyteTable画面と同じデザインに変更
        ///             再検査画面でAuto Dilution Ratioに1,10,20,100,200,400,1000,2000,4000,8000を指定できるよう変更
        /// Ver0.01.46: 
        ///             ①複数モジュール化対応（一部スレーブ1固定動作もある）
        ///             ②サンプリング停止時にステータスタップにて停止理由ダイアログ表示対応（現状はビット値を表示（英語リソースがないため））
        ///             ③光学系セルフチェックコマンド(0408）を受信した際のLED基準光源を用いたチェックを削除
        ///             ④光学系セルフチェックコマンド(0408）を受信した際のダーク値のエラー判定条件を「100以上」から「100を超える」に変更
        ///             ⑤ラック搬送のモーターパラメータのオフセットにOffsetRackIDReadingを追加
        ///             ⑥ラック搬送のユニットテストに「20.ラック総合テスト」を追加
        ///             ⑦総アッセイ数を初期シーケンスでスレーブに送信するよう変更
        ///             ⑧総アッセイ数を消耗部品パラメータで管理するよう変更
        ///             ⑨R1試薬分注ユニットに「12.プローブ交換位置に移動」を追加
        ///             ⑩R2試薬分注ユニットに「11.プローブ交換位置に移動」を追加
        ///             ⑪インストーラープロジェクト追加
        /// 
        ///             ＜調整＞
        ///             ・メンテナンスの誤記修正
        ///             ・画面のちらつき対応
        ///             ・残量コマンド受信時にユーザーオブジェクト数が増加することでアプリが強制終了する問題を修正
        ///             ・モジュール毎のオプション画面にて、スレーブバージョン番号を更新するように修正
        ///             
        ///             ＜UMIC出張報告＞
        ///             No.05）同一モジュール内で各ユニットを切り替えた際、モーター、コンフィグのパラメータを再読み込みする
        ///             No.06）reagent bottleグリッドの列幅をXMLから取得せずにプログラム上で固定に変更
        ///             No.10）温度のグラフのY軸を10～90→0～50の範囲に変更
        ///             No.17）メンテナンス画面の起動について、ラックもしくはモジュールに接続中かつWait状態の場合のみクリックできるよう変更
        ///             No.24）Assay Status画面で、Waste BoxとWash Solution Tankの表示場所を交換
        ///             No.27）試薬マッチング対応（併せて、試薬履歴情報テーブルに「groupNo」カラムを追加）
        ///
        /// Ver0.01.47.00: 
        ///             ①複数モジュール化対応（モジュール固定で動作している箇所を修正）
        ///             ②「RackNoUse」および「Assay実行モジュール選択」をユーザーレベル5（developer）の場合のみ表示するように変更
        ///             ③Developer以外のユーザーでログイン時、「RackNoUseとAssay対象モジュールの内容を初期化するよう修正
        ///             ④「Assay実行モジュール選択」にAll(=0）を追加
        ///             ⑤コマンドリストrev0.097の内容を反映
        ///             ⑥システム設定パラメータに「搬送ライン使用有無設定」および「分析モジュール接続台数」、「キャリブレータ測定」、「コントロール測定」を追加
        ///             （※搬送ライン使用有無と分析モジュール接続台数は新規画面追加）
        ///             ⑦サンプリング停止理由を表示する際、ラックの内容も表示するよう変更
        ///             ⑧インストール時に実行ファイルを更新できるようにバージョン管理番号のルールを変更
        ///             
        ///             ＜調整＞
        ///              ・アッセイステータス画面の残量詳細表示が正しく反映されない不具合を改修
        ///              ・アッセイアボート時にException発生の問題を改修
        ///              ・測定情報問い合わせの応答(1502)のカップタイプが常に1( カップなしが入ってこない)となる不具合を改修
        ///              ・Assay Start時の加温待ち中にキャンセルした時、システムステータスが正しく戻らない不具合を改修
        ///              ・検体登録・AssayStart時の残量警告の判定・表示が常に表示される不具合を改修
        ///              ・プロトコル設定でR2分注量、前処理１分注量、前処理２分注量にゼロを許可するよう変更
        ///              
        ///             ＜UMIC出張報告＞
        ///             No.16）システムステータスをラック＋各モジュールそれぞれで管理するよう変更
        ///             No.18）Assay Start時にリンスを行うかどうかの指定をダイアログ上で行えるよう変更
        ///             No.23）温度設定が０℃の場合は、加温待ちの対象外とするよう変更
        /// 
        /// Ver0.01.48.00: 
        ///             ＜調整＞
        ///             ・分析中断後に試薬交換画面が非活性のままとなる不具合を改修
        ///             
        /// Ver0.01.49.00: 
        ///             ①複数モジュール化対応
        ///             ②STAT登録画面の有効化
        ///             ③STAT測定可能モジュールの確認ダイアログを追加
        ///             ④システム設定にラック移動方式を追加
        ///             ⑤ラック移動先問合せ時に、ラック移動方式のモードによってラックの振り分け方法の切り替え処理実装
        ///             ⑥ラック搬送との初期シーケンスにて、システムパラメータコマンドの後に検体バーコード設定コマンドを送信
        ///             
        ///             ＜調整＞
        ///             ・異常終了後の起動後に表示される警告メッセージに対し、全モジュールを再起動したか確認を促すメッセージを追加
        ///             ・AnalysisModeを明細に表示する際、値ではなく文字列で表示するよう変更
        ///             ・ラックユニットテスト31-7、31-8でSamplePositionに0、6を設定できるよう変更
        ///             ・キャリレータ登録画面で、キャリブレーション方式がマスターキャリブかつ複数ラックにまたがるプロトコルを選択した際、画面項目が見切れてしまう問題を修正
        ///             ・警告灯が物理的に存在しないという事なので、警告灯制御コマンドの送信処理を削除
        ///             ・初期シーケンスでダークエラーが発生した際、エラーログのモジュールNOが設定されずどのモジュールでエラーが発生したかわからなかった問題を修正
        ///             ・メイン画面でモジュール選択した時に試薬交換画面の情報が切り替わらない問題を修正
        ///             ・測定指示データ問合せコマンド受信時に検体登録情報から指示データを返す分析項目のみ削除するように修正
        ///             ・ラックプログラムのアップデート実行時にモジュールからのタンク問合せで例外が発生する件を修正
        ///             ・コントロール測定時、ラック移動問合せのタイミングで203、204エラーを表示するよう変更
        ///             
        ///             ＜UMIC出張報告＞
        ///             ・測定結果画面にて、ダーク、BG、リザルトカウント値の表示位置が左寄せになっていたので右寄せに修正
        ///             ・Specimen、Control、CalibrationのResult Logで明細の並びかえが出来るよう変更
        ///             
        /// Ver1.01.50.00: 
        ///     ①システム設定から警告灯使用有無項目を削除
        ///     ②システム設定から接続台数を変更できるように対応
        ///     ③ラックレディコマンド(0101)およびスレーブレディコマンド(0501)のバージョン情報の改訂（5桁→11桁）
        ///     ④ラック移動問合せで待機レーンに戻す時、該当ラックIDで一般検体登録情報が存在する場合、再検情報に移動するよう変更
        ///     ⑤エラーメッセージ202-1を追加
        ///     ⑥キャリブレータ登録を行わずに、キャリブレータ情報と検体IDの内容でキャリブレータ測定が行えるよう対応
        ///     ⑦一般検体測定にて検体ID方式分析に対応
        ///     ⑧試薬のマッチング対応
        ///     ⑨イーサネットのMACアドレスを取得し、ソフトウェア識別コマンドのMACアドレス情報に設定する処理を追加
        ///     ⑩ラック及びモジュールのいずれか一つでもサンプリング停止状態になっている場合、システムステータスにアイコン表示
        ///     ⑪試薬に対し、「検量線無し」または「検量線の有効期限切れ」の場合、ロット番号の色を変更する処理を追加
        ///     ⑫一般検体測定にて、手動再検および自動再検に対応
        ///     ⑬バージョン通知コマンド（0111、0511）のパラメータの桁数を11桁に拡張
        ///     
        ///     ＜調整＞
        ///     ・STATにて、2回測定時に例外発生する問題を修正
        ///     ・System Option画面から初期シーケンス実行時に例外発生する件を修正
        ///     ・試薬バーコード手入力にて、意図的にR試薬およびM試薬の残量を空白に設定して場合、例外発生する件を修正
        ///     ・Specimen Result画面の明細で分析モードの表示を数字ではなく文字（Standard）で表示するよう変更
        ///     ・メイン画面、メンテナンス画面でモジュール選択ボタンを接続状態に合わせて活性・非活性を切り替える動作を実装
        ///     ・LEDカウントは参照しなくなり、コマンドリスト上からも削除されたため、削除
        ///     ・システムパラメータコマンドからキャリブレータ測定方式と制度管理測定方式を削除
        ///     ・オプション画面（モジュール毎）のバージョン情報がリアルタイムに反映するよう修正
        ///     
        ///     ＜UMIC要望＞
        ///     ・測定結果をファイル出力時、選択行だけ出力する場合に選択順ではなく、画面の表示順に合わせて出力するよう変更
        ///     ・分析項目読込時に旧アッセイシーケンスの場合は新アッセイシーケンスとそれに伴った希釈倍率に変換する処理を追加
        ///             
        /// Ver1.01.51.00: 
        ///     ①Total画面を実装
        ///     ②スレーブに対して廃液タンクの状態を返す時に「設置なし、満杯」は想定外の為、ラックの情報をそのままスレーブへ送信するよう変更
        ///     ③各測定結果表示画面のフィルターに、モジュール番号を指定できるよう項目を追加
        ///     ④各測定結果表示画面の明細にモジュール番号の表示を追加
        ///     ⑤サンプリング停止時のアイコン画像（！）を差し替え
        ///     ⑥UIPackage.xmlにモジュール構成フラグ（ModuleConfigFlg）を追加し、ソフトウェア識別コマンドに付与するように変更
        ///     
        ///     ＜調整＞
        ///     ・一般検体登録画面で明細のPatientId、Commentを入力→Enter→クリア→Enterと操作した際に例外が発生する件を修正
        ///     ・STAT検体登録画面で明細のPatientId、Commentを入力→Enter→クリア→Enterと操作した際に例外が発生する件を修正
        ///     ・Calibration Analysis画面でCount項目をクリア→Enter後にSaveを行うと例外が発生する件を修正
        ///     ・ペーストダイアログのボタンデザインをCarisX用に修正
        ///     ・RackIDを入力したまま画面遷移→再表示→Selectボタン押下と操作すると例外が発生する件を修正
        ///     ・ラック、モジュールに繋がっていない状態でシャットダウンを行った時、画面が非活性から復帰出来ない件を修正
        ///     ・自動起動待機処理がラックに対応出来ていなかったので修正
        ///     ・検体ID方式で検体IDのみ入力している場合、検体登録でDeleteを行うと先頭行からしか削除されない不具合を修正
        ///     ・モーターエラー系のメッセージを翻訳対応
        ///     ・エラーコード88-3および88-4から番号のフォーマット指定文言を削除
        ///     
        /// Ver1.01.52.00: 
        ///     ①エラーコード86-1をXMLに追加
        ///     ②廃液タンク状態(ラック情報)通知コマンドの送信処理を追加
        ///     ③急診モードを追加
        ///     ④中国語翻訳対応
        ///     ⑤登録時に検体IDが重複していた場合に警告ダイアログを表示するよう変更
        /// 
        ///     ＜調整＞
        ///     ・Assay Status画面でReagent明細の文字色が変わっているセルを選択した際に、文字色が黒色に変わってしまう件を修正
        ///     ・Total、Assay、Reagent画面の残量によるアイコン変更の閾値をUIPackageから内部メモリに変更。
        ///     ・エラー、オペレーション、パラメータチェンジのログ画面で、明細の並び順が日付（降順）→ID（降順）となるよう変更
        ///     ・再計算したときにforeachの処理中にコレクションに追加・削除が発生しており、InvalidOperationExceptionの例外発生する問題を修正
        ///     ・廃液タンクが満杯または設置なしでAssayを開始した時、警告メッセージ表示後にAssay開始を中止するよう変更
        ///     ・ラックスレーブインストーラーの実行ファイルを構成に追加
        ///     ・ログイン画面にて、OKボタンによる自動ログインを削除
        ///     ・バージョンアップ画面でバージョンの表示桁数が切れている件を修正
        ///     ・検量線画面のEditで右肩下がりになるような値の修正をしてSaveを行った時に例外が発生する件を修正
        ///     ・0x1000000のリマークの文字を「Mx」、ダブルクリックして表示される説明を「Mixing Error」となるよう変更
        ///     ・ユーザーレベル１でログインした際、エラー受信時に画面更新されない不具合を修正
        ///     ・TipCellViewに表示されているバーの幅を短くして区切りが８個分になるよう変更
        ///     ・TipCellViewのバーの色が黄色になる境界値を変更（48以下は黄、49以上は緑）
        ///     ・分析項目設定画面にて、試薬名が11文字の項目を表示すると、文字切れが発生していた件を修正
        ///     ・Assay Status画面の明細３つにモジュール番号の表示を追加
        ///     ・Option-Rinse操作時に画面のメッセージを「リンス実行中」に変更するよう修正
        ///     
        /// Ver1.01.53.00: 
        ///     ①ホスト通信対応、ホスト通信時の希釈倍率変換対応
        ///     ②ラック移動問合せでのPerformanceモードの動作改良（フォークが一番最初に空くモジュールに移動されるようにした）
        ///     ③試薬設置ガイダンスダイアログの表示処理を実装
        ///     ④プロトコル設定画面にR1ユニットの分注順逆転を追加
        ///     ⑤STATの再検査リスト対応（※既存テーブルにカップ種別が無かったため、DBファイルの差し替えが必要）
        ///     ⑥急診モードの分析項目の非表示・非活性対応
        ///     ⑦項目間演算処理（CA125/HE4、PG1/PG2、f-PSA/T-PSA）の実装
        ///     
        ///     ＜調整＞
        ///     ・リアルタイムCSV出力にて、出力される内容が測定結果画面でエクスポートした際と異なる件を修正（並び順は異なる）
        ///     ・初期シーケンスで受信したラックスライダー（X軸）のRack Unloadオフセット値が反映されない件を修正
        ///     ・検量線およびQC精度管理検体の印刷時にチャートデータを保持するためのPrintフォルダを追加
        ///     ・モータ調整の1ピッチ移動量を修正（トラベラー・廃棄部：Ｘ軸、プレトリガ・トリガ分注：プレトリガ・トリガノズルＺ軸）
        ///     ・ファイル出力にて、ユーザーレベルに応じて出力内容の制限がかかっていなかった件を修正
        ///     ・DBからユニーク番号の最新の値を取得するように変更
        ///     ・モジュール構成フラグをユーザー側で保持しないよう変更
        ///     ・Total画面の試薬明細に、R1・R2のみ又はMのみ設置されている試薬が空行で表示されてしまう件を修正
        ///     ・CarisX起動時にバージョンをデバッグログに出力するよう変更
        ///     ・ラック情報のクリア処理をラック排出コマンド送信後に変更
        ///     ・STATの場合、画面上はラックIDを"STAT"と表示するよう変更
        ///     ・Performanceモード時の動作でSTATの数を考慮出来ていなかったので修正
        ///     ・画面上でモジュール２が選択されている状態でモジュール１の試薬保冷庫回転SWを押下すると、モジュール２の試薬保冷庫が回転してしまう件を修正
        ///     ・STAT測定可能モジュールの確認ダイアログの表示処理がモジュール１のみ参照していたため、複数モジュールに対応
        ///     ・検体測定結果画面にSTATと一般検体のデータが存在すると例外が発生する件を修正
        ///     ・モジュール１で検体有りを受信時にモジュール２に固定登録があっても待機状態になってしまう件を修正
        ///     ・STATの一時登録と固定登録がどちらも登録されている時、SW押下待ちにならない場合がある件を修正
        ///     ・測定結果のリアルタイムCSV出力時、一度も該当画面を表示していない時にデータが正しく出力されない件を修正
        ///     ・急診モードで測定した際、帳票のAnalysysModeがEmergencyになっていない件を修正
        /// 
        /// Ver1.01.54.00: 
        ///     ①エラーフィルタリングの実装
        ///     ②エラーレベル追加（Error / Warning / Hint）
        ///     ③エラー履歴に絞り込みフィルター追加
        ///     ④モーター32分割対応
        ///     ⑤サブメニュー表示制御（サブメニューが1つしかない場合、サブメニューを表示しない）
        ///     ⑥Assay画面にチップケースの使用位置表示
        ///     ⑦メンテナンス日誌対応
        ///     ⑧モジュール毎のオプション機能にメンテナンス日誌機能追加
        ///     ⑨TB-IGRA対応
        ///     ⑩自動希釈再検倍率の倍率設定条件の変更
        ///     ⑪リマーク64Bit対応
        ///     
        ///     ＜調整＞
        ///     ・BF1のコンフィグパラメータ画面にBF2パラメータを移動（名称も変更）
        ///     ・モーター32分割対応にて、スピード設定の再調整が必要となるため、メンテナンス画面の各ユニットにて、モータースピードのソフト固定となっていた領域を入力可に変更
        ///     ・ラック移動問合せにて、回収理由のエラー発報を一部実装
        ///     ・ログイン時に入力ミスのメッセージ表示
        ///     ・初期シーケンス時の光学系セルフチェック処理にて、ダーク値チェックの実施有無フラグを追加
        ///     ・ユーザー後起動時に初期通信が動作しない問題の修正
        ///     ・ラック移動位置問合せのコマンドをBCRと装置待機位置とで別々のコマンドに分割する修正を実施（ホストPCから応答がない場合に非同期で動作させるため）
        ///     ・測定順番が1オリジンになっていなかったため、修正
        ///     ・残量コマンド関連に試薬設置日の情報を追加（現時点で設置時間は未対応）
        ///     
        /// Ver1.01.55.00: 
        ///     ①試薬ボトルの仕様優先順位の変更対応（試薬テーブルに設置日時を追加）
        ///     ②測定結果画面にページ切替機能を追加
        ///     ③R1/R2試薬分注ユニットのモーターパラメータにプローブ交換調整位置を追加
        ///     ④コマンド送受信の高速化対応（メイン画面の受信イベント制御タイマーの間隔を1000ms→100msに変更）
        ///     ⑤初期シーケンス中のモーターエラー対応（モーター動作関連のコマンド送信制御、分析開始時にモーターエラー発生モジュールは対象外とする）
        ///     ⑥ラック移動位置問合せ時にラック設置状況から残サイクル数を確認するように変更
        ///     ⑦各シーケンス番号の終了番号が被らないように修正
        ///     ⑧キャリブレータ登録および検量線データをモジュール毎に管理するように変更
        ///     ⑨試薬準備画面にて、分析中に使用中ボトルの交換が行えないようにする制御を追加
        ///     ⑩IoT対応（※但しコンパイルスイッチにて現状は機能OFF）
        ///     ⑪サンプルIDの入力制限対応
        ///     ⑫ラックスレーブのバージョンアップ処理の複数同時実行の対応
        ///     
        ///     ＜調整＞
        ///     ・分析中のアッセイ画面にて、初期値の検量線情報が表示されないように修正
        ///     ・IGRA項目の削除時にポジション2,3,4の登録データも併せて削除されるように調整
        ///     ・メンテナンス日誌にて、再確認処理としてチェック項目のクリアダイアログを表示するように修正
        ///     ・ラック移動問合せにて回収理由のエラーメッセージを追加（90-2,90-3,90-4）
        ///     ・一般検体測定結果画面にて、手希釈倍率変更時にリマーク"Ed"が付与されない問題を修正
        ///     ・メンテナンス日誌にて、省略文字（見切れる際に「...」を表記）および横スクロールバーを追加
        ///     ・ソケット通信制御の改修
        ///     ・DBファイルのアクセスパスをデバッグログに出力するように変更
        ///     ・メンテナンス画面にて、センサー状態のコマンドのやり取りが途中で止まる問題を修正
        ///     ・IGRA時にはラックの検体設置が空の状態でもラック位置問合せにてラック無しで弾かないように修正
        ///     ・複数モジュール接続時に試薬ボトルの残量編集が行えなくなる問題を修正
        /// 
        ///     ＜その他＞
        ///     ・ラック移動方式管理クラスを追加（これに伴い、メイン画面クラスにあったラック移動方式処理をこちらに移動）
        ///     ・受信コマンド処理クラスを追加（これに伴い、メイン画面クラスにあった受信コマンド処理をこちらに移動）
        ///     ・装置コードからモジュールIDへの変換関数名が紛らわしいため、修正
        ///
        /// Ver1.01.56.00: 
        ///     ＜調整＞
        ///     ・サンプリング停止、分析終了時、分析再開時にポーズコマンドが複数台に送られない不具合を修正
        ///     ・コマンド送信処理に排他制御を追加
        /// 
        #endregion // [--- old version ---]
        ///
        /// Ver1.01.57.00: 
        ///     ①障害対策画面の改修
        ///     ②急診検体における希釈倍率の制限
        ///     ③試薬交換状態をモジュール毎に管理
        ///     ④マスターキャリブ方式のキャリブポイント設定の制限
        ///     ⑤エラー88-Xにラックの手動排出を促すメッセージを追加
        ///     
        ///     ＜調整＞
        ///     ・RACK・SLAVEバージョンアップツールにて例外対応
        ///     ・ラック移動位置問合せにて、装置間移動の指定があるにも関わらず、0が指定される問題の対応
        ///     ・アッセイ開始時にスレーブの分析開始後にラックに分析開始動作が行われるよう変更
        ///     ・検量線データを編集した際に例外発生の問題の対応
        ///     ・キャリブレータ登録画面にて分析項目選択後に画面遷移した場合、RackIDのselectボタンが非活性のままとなる問題の対応
        ///     ・キャリブレータ登録画面にて1台構成時にモジュール選択を非表示
        ///     ・画面遷移時に編集中である旨の警告ダイアログの表示
        ///     ・未接続状態のモジュールに画面切り替え可能となるよう変更
        ///     ・キャリブ登録にて手動補正ポイントの内容をラック図にリアルタイムに反映するよう変更
        ///     
        /// </summary>
        /// <remarks>
        /// 変数に定義するバージョン番号はオプション画面に表示するため、「メジャー.マイナー.リビジョン」までとする。
        /// 上記の履歴に記載するバージョン番号は「メジャー.マイナー.リビジョン.ビルド」のすべてを記述してください。
        /// ※インストーラーに定義するバージョン番号は「リビジョン」と「ビルド」を連結したものとする。
        /// 　・メジャー  ：製品を根本から変更する場合に変更されます。
        /// 　・マイナー  ：大幅な仕様変更・機能追加をする場合に変更されます。
        /// 　・リビジョン：仕様変更・機能追加をする場合に変更されます。
        /// 　・ビルド    ：修正パッチごとに変更されます。
        /// </remarks>
        public const String USER_PROGRAM_VERSION = "V1.01.57.00";

        /// <summary>
        /// グリッド行背景色の基本色
        /// </summary>
        static public readonly Color GRID_ROWS_DEFAULT_COLOR = Color.FromArgb(0xFF, 0xFF, 0xFF); // 白

        /// <summary>
        /// グリッド行背景色パターン1（偶数行色等として使用）（検体登録）
        /// </summary>
        static public readonly Color GRID_ROWS_COLOR_PATTERN1 = Color.FromArgb(0xB0, 0xC4, 0xDE); // 水色

        /// <summary>
        /// グリッド行背景色パターン2（偶数行色等として使用）（検体再検査）
        /// </summary>
        static public readonly Color GRID_ROWS_COLOR_PATTERN2 = Color.FromArgb(0xF5, 0xF5, 0xF5); // 明るい灰色

        /// <summary>
        /// グリッド行背景色パターン3（偶数行色等として使用）（精度管理検体登録）
        /// </summary>
        static public readonly Color GRID_ROWS_COLOR_PATTERN3 = Color.FromArgb(0xB0, 0xC4, 0xDE); // 水色

        /// <summary>
        /// グリッド行背景色パターン4（偶数行色等として使用）（キャリブレータステータス）
        /// </summary>
        static public readonly Color GRID_ROWS_COLOR_PATTERN4 = Color.FromArgb(0xB0, 0xC4, 0xDE); // 水色

        /// <summary>
        /// グリッド編集可能セル色(キャリブレータ解析)
        /// </summary>
        static public readonly Color GRID_CELL_CAN_EDIT_COLOR = Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00); // 黄

        /// <summary>
        /// グリッド編集可能セル色(検体再検査)
        /// </summary>
        static public readonly Color GRID_CELL_CAN_EDIT_COLOR2 = Color.FromArgb(0xFF, 0xFF, 0xC1); // 明るい黄


        /// <summary>
        /// リスト項目選択色
        /// </summary>
        static public readonly Color LIST_SELECT_COLOR = Color.FromArgb(0x33, 0x99, 0xff);    // 水色(明)

        /// <summary>
        /// ボタン選択色
        /// </summary>
        static public readonly Color BUTTON_SELECT_COLOR = Color.FromArgb(255, 128, 0);       // オレンジ

        /// <summary>
        /// ラックポジション数
        /// </summary>
        public const Int32 RACK_POS_COUNT = 5;

        /// <summary>
        /// STATポジション数
        /// </summary>
        public const Int32 STAT_POS_COUNT = 1;

        /// <summary>
        /// 外部搬送ポジション数
        /// </summary>
        public const Int32 OUTSIDETRANSFER_POS_COUNT = 1;

        /// <summary>
        /// 允许的稀释的最大的倍数
        /// </summary>
        public const Int32 MaxDILUTION = 8000;

        /// <summary>
        /// サンプルステージラック数
        /// </summary>
        public const Int32 SAMPLE_STAGE_RACK_COUNT = 2;
        /// <summary>
        /// 優先レーンラック数
        /// </summary>
        public const Int32 PRIORITY_LANE_RACK_COUNT = 2;
        /// <summary>
        /// 通常レーンラック数
        /// </summary>
        public const Int32 NORMAL_LANE_RACK_COUNT = 20;
        /// <summary>
        /// 分析中ラック数
        /// </summary>
        public const Int32 INPROCESS_LANE_RACK_COUNT = 2;

        /// <summary>
        /// 一括登録可能ラック最大
        /// </summary>
        public const Int32 SAMPLE_REGIST_RACK_COUNT = 200;

        /// <summary>
        /// 一括登録可能検体数最大
        /// </summary>
        public const Int32 SAMPLE_REGIST_MAX = SAMPLE_REGIST_RACK_COUNT * RACK_POS_COUNT;

        /// <summary>
        /// グリッド最大拡大率
        /// </summary>
        public const Int32 GRID_ZOOM_MAX = 500;

        /// <summary>
        /// グリッド最小拡大率
        /// </summary>
        public const Int32 GRID_ZOOM_MIN = 10;

        /// <summary>
        /// グリッド変動率(％)
        /// </summary>
        public const Int32 GRID_ZOOM_STEP = 10;
        /// <summary>
        /// 分析項目最大登録数
        /// </summary>
        public const Int32 MEAS_PROTO_REGIST_MAX = 50;

        /// <summary>
        /// 一般検体ラックID接頭文字
        /// </summary>
        public const String GENERAL_RACK_ID_PRECHAR = "";

        /// <summary>
        /// キャリブレータラックID接頭文字
        /// </summary>
        public const String CALIB_RACK_ID_PRECHAR = "S";

        /// <summary>
        /// 精度管理検体ラックID接頭文字
        /// </summary>
        public const String CONTROL_RACK_ID_PRECHAR = "C";

        /// <summary>
        /// STAT検体ラックID接頭文字
        /// </summary>
        public const String STAT_RACK_ID_PRECHAR = "E";

        /// <summary>
        /// カップデッドボリューム：μL
        /// </summary>
        public const Int32 SAMPLE_DEADVOLUME_CUP = 100;

        /// <summary>
        /// チューブデッドボリューム：μL
        /// </summary>
        public const Int32 SAMPLE_DEADVOLUME_TUBE = 250;

        /// <summary>
        /// 陽性判定文字
        /// </summary>
        public const String JUDGE_POSITIVE = "+";

        /// <summary>
        /// 陰性判定文字
        /// </summary>
        public const String JUDGE_NEGATIVE = "-";

        /// <summary>
        /// 偽陽性判定
        /// </summary>
        public const String JUDGE_HALF = "+-";

        /// <summary>
        /// ルーチンテーブル数
        /// </summary>
        public const Int32 ROUTINE_TABLE_COUNT = 50;

        /// <summary>
        /// 試薬ボトル交換準備時の待ち時間計算用：最大ポジション
        /// </summary>
        public const Int32 REAGENT_CHANGE_POSITION_MAX = 59;

        /// <summary>
        /// 試薬ボトル交換準備時の待ち時間計算用：1ポジションあたりの待ち時間(18秒)
        /// </summary>
        public const Int32 REAGENT_CHANGE_WAIT_TIME = 18;

        /// <summary>
        /// 最大試薬ボトルポート数
        /// </summary>
        public const Int32 REAGENT_PORT_MAX = 20;

        /// <summary>
        /// 試薬マッチング用グループ番号の再設定用の値
        /// </summary>
        public const Int32 REAGENT_MATCHING_GROUP_NO_RESETTING = -1;


        /// <summary>
        /// 一般検体測定結果のCSV出力ファイル名
        /// </summary>
        public const string EXPORT_CSV_SPECIMENRESULT = "SpecimenResult";
        /// <summary>
        /// キャリブレータ測定結果のCSV出力ファイル名
        /// </summary>
        public const string EXPORT_CSV_CALIBRATORRESULT = "CalibratorResult";
        /// <summary>
        /// 精度管理検体測定結果のCSV出力ファイル名
        /// </summary>
        public const string EXPORT_CSV_CONTROLRESULT = "ControlResult";
        /// <summary>
        /// エラー履歴のCSV出力ファイル名
        /// </summary>
        public const string EXPORT_CSV_ERRORLOG = "ErrorLog";
        /// <summary>
        /// 操作履歴のCSV出力ファイル名
        /// </summary>
        public const string EXPORT_CSV_OPERATIONLOG = "OperationLog";
        /// <summary>
        /// パラメータ変更履歴のCSV出力ファイル名
        /// </summary>
        public const string EXPORT_CSV_PARAMETERCHANGELOG = "PrameterChangeLog";
        /// <summary>
        /// パラメータ変更履歴のCSV出力ファイル名
        /// </summary>
        public const string EXPORT_CSV_ONLINELOG = "OnlineLog";
        /// <summary>
        /// 分析履歴のCSV出力ファイル名
        /// </summary>
        public const string EXPORT_CSV_ASSAYLOG = "AssayLog";

        // 2020-02-27 CarisX IoT Add [START]
        /// <summary>
        /// IoT圧縮パッケージファイル名
        /// </summary>
        public const string EXPORT_ZIP_LOGFILES = "Logfiles";
        // 2020-02-27 CarisX IoT Add [END]

        /// <summary>
        /// CSV出力時にファイルの接尾辞として
        /// </summary>
        public const string EXPORT_CSV_DATETIMEFORMAT = "yyMMdd-HHmmss";

        /// <summary>
        /// 分析中の計算の上限データ数
        /// </summary>
        public const Int32 INASSAY_CALCDATA_LIMIT_MAX = 200;

        /// <summary>
        /// ダーク下限値
        /// </summary>
        public const Int32 DARK_LIMIT_MIN = 100;

        /// <summary>
        /// 測光上限値
        /// </summary>
        //public const Int32 MEASURE_LIMIT_MAX = 80;

        public const Int32 MEASURE_LIMIT_MAX = 80;
        //TSH 測光上限値
        //public const Int32 MEASURE_TSH_LIMIT_MAX = 80;

        //背景值上限
        public const Int32 BACKGROUND_LIMIT_MAX = 5000;

        //反应作废时发光值

        public const Int32 ERRORCHECK_LIMIT_MAX = 1;

        /// <summary>
        /// 測光上限値
        /// </summary>
        public const Int32 MEASURE_LIMIT_MAX_CALIB = 50;

        /// <summary>
        /// マスターカーブ識別日時
        /// </summary>
        static readonly public DateTime MASTER_CURVE_DATE = DateTime.MinValue;

        /// <summary>
        /// マスターカーブ固定ユニーク番号
        /// </summary>
        public const Int32 MASTER_CURVE_UNIQUE_NO = -1;

        /// <summary>
        /// マスターカーブ最大本数
        /// </summary>
        public const Int32 MASTER_CURVE_MAX_COUNT = 5;

        /// <summary>
        /// カウント値、濃度値なしの場合の表示文字列
        /// </summary>
        public const String COUNT_CONCENTRATION_NOTHING = "****";

        /// <summary>
        /// 浓度大于号
        /// </summary>
        public const String CONCENTRATION_GREATER = ">";

        /// <summary>
        /// 浓度小于号
        /// </summary>
        public const String CONCENTRATION_LESS = "<";
        /// 残量"空"時のセル色
        /// </summary>
        public static readonly Color GRID_CELL_REMAIN_COLOR_EMPTY = Color.White;

        /// <summary>
        /// 残量"少量"時のセル色
        /// </summary>
        public static readonly Color GRID_CELL_REMAIN_COLOR_LOW = Color.FromArgb(255, 189, 74);

        /// <summary>
        /// 残量"十分量"時のセル色
        /// </summary>
        public static readonly Color GRID_CELL_REMAIN_COLOR_FULL = Color.FromArgb(12, 172, 00);

        /// <summary>
        /// 分注チップ、反応容器の設定数
        /// </summary>
        public const Int32 REMAIN_SAMPLINGTIP_CELL = 96;

        /// <summary>
        /// 分注チップ、反応容器のケース数
        /// </summary>
        public const Int32 REMAIN_SAMPLINGTIP_CELL_CASE = 8;

        /// <summary>
        /// 分注チップ、反応容器の残量下限値
        /// </summary>
        public const Int32 REMAIN_SAMPLINGTIP_CELL_MIN = 0;

        /// <summary>
        /// 分注チップ、反応容器の残量上限値
        /// </summary>
        public const Int32 REMAIN_SAMPLINGTIP_CELL_MAX = 96;

        /// <summary>
        /// プレトリガ・トリガの残量下限値(μL)
        /// </summary>
        public const Int32 REMAIN_PRETRIGGER_TRIGGER_MIN = 0;

        /// <summary>
        /// プレトリガ・トリガの残量上限値(容量=1)(μL)
        /// </summary>
        public const Int32 REMAIN_PRETRIGGER_TRIGGER_MAX_1 = 200000;

        /// <summary>
        /// プレトリガ・トリガの残量上限値(容量=2)(μL)
        /// </summary>
        public const Int32 REMAIN_PRETRIGGER_TRIGGER_MAX_2 = 300000;

        /// <summary>
        /// 希釈液の残量下限値(μL)
        /// </summary>
        public const Int32 REMAIN_DILUENT_MIN = 0;

        /// <summary>
        /// 希釈液の残量上限値(容量=1)(μL)
        /// </summary>
        public const Int32 REMAIN_DILUENT_MAX_1 = 200000;

        /// <summary>
        /// 希釈液の残量上限値(容量=2)(μL)
        /// </summary>
        public const Int32 REMAIN_DILUENT_MAX_2 = 300000;

        /// <summary>
        /// キャリブレータの濃度値の桁数上限及び表示桁数
        /// </summary>
        public const Int32 CALIB_CONC_LENGTH_AFTER_DEMPOINT = 3;

        /// <summary>
        /// 測定結果の濃度値の実数の最大桁数
        /// </summary>
        public const Int32 RESULT_CONCENTRATION_DIGITS = 10;

        /// <summary>
        /// エラーコード閾値
        /// </summary>
        public const Int32 ERROR_CODE_THRESHOLD = 100;

        /// <summary>
        /// オンライン出力データ上限
        /// </summary>
        public const Int32 TRANSMIT_DATA_MAX = 200;

        /// <summary>
        /// シーケンス番号上限
        /// </summary>
        public const Int32 SEQUENCE_NO_MAX = 9999;

        /// <summary>
        /// 受付番号上限
        /// </summary>
        public const Int32 RECEIPT_NUMBER_MAX = 9999;

        #region __DB関連__

        /// <summary>
        /// 一般/優先検体測定結果の保存上限検体数
        /// </summary>
        public const Int32 MAX_SAMPLERESULT_COUNT = 10000;

        /// <summary>
        /// 一般/優先検体の保存上限分析数
        /// </summary>
        public const Int32 MAX_SAMPLERESULT_TEST_COUNT = 500000;
        /// <summary>
        /// キャリブレータ測定結果の保存上限分析数
        /// </summary>
        public const Int32 MAX_CALIBRESULT_TEST_COUNT = 30000;
        /// <summary>
        /// 精度管理検体測定結果の保存上限分析数
        /// </summary>
        public const Int32 MAX_CONTROLRESULT_TEST_COUNT = 30000;

        /// <summary>
        /// 履歴保存上限件数
        /// </summary>
        public const Int32 MAX_LOG_COUNT = 10000;

        /// <summary>
        /// 試薬履歴保存上限件数
        /// </summary>
        public const Int32 MAX_REAGENT_HISTORY_COUNT = 400;

        /// <summary>
        /// 定性項目の陰性ラックポジション表示
        /// </summary>
        public const string NEGATIVE_POSITION = "NC";
        /// <summary>
        /// 定性項目の陽性ラックポジション表示
        /// </summary>
        public const string POSITIVE_POSITION = "PC";

#endregion

        /// <summary>
        /// 検量線表示色
        /// </summary>
        static public readonly Color[] CHART_CALIB_CURVE_COLOR = new Color[] { Color.Blue, Color.Violet, Color.Red, Color.Orange, Color.Yellow, Color.Green };

        /// <summary>
        /// 定性項目ポイント数
        /// </summary>
        public const Int32 QUALITATIVE_POINT_COUNT = 2;

        /// <summary>
        /// LogitLog濃度値Log係数A
        /// </summary>
        public const Double LOGITLOG_COEF_A = 1.0;
        /// <summary>
        /// LogitLog濃度値Log係数B
        /// </summary>
        public const Double LOGITLOG_COEF_B = 0.00000001;
        /// <summary>
        /// 最大分析項目数
        /// </summary>
        public const Int32 REGIST_MEAS_ITEM_MAX_UPPER = 51;

        /// <summary>
        /// 日付表示用フォーマット文字列の取得
        /// </summary>
        static public String DISPLAY_DATETIME_FORMAT
        {
            get
            {
                return DateTimeFormatInfo.CurrentInfo.ShortDatePattern + " " + DateTimeFormatInfo.CurrentInfo.LongTimePattern;
            }
        }

#region プロトコルコンバータ関連

        /// <summary>
        /// プロトコルコンバータExe名
        /// </summary>
        public static String ProtoConvExeName = CarisXConst.PathImportProtoExe + @"\ProtocolConverter.exe";
        /// <summary>
        /// 出力先フォルダ名
        /// </summary>
        public static String ProtoConvExportDir = CarisXConst.PathImportProtoExe + @"\Protocol";
        /// <summary>
        /// ログファイル名
        /// </summary>
        public const String PROT_CONV_LOG = "ConvertLog.xml";
        /// <summary>
        /// ホスト通信タイムアウト
        /// </summary>
        public const Int32 HOST_COMTIMEOUT = 3000;

        /// <summary>
        /// 濃度値の実数桁数
        /// </summary>
        public const Int32 CONC_REAL_NUMBER_DIGITS = 10;


#endregion


        /// <summary>
        /// 洗浄液時警告容量
        /// </summary>
        public const Int32 WASH_SOLUTION_WARNING_REMAIN = 2500;

        /// <summary>
        /// 試薬保冷庫1ポジション必要設置数(M,R1,R2試薬全設置)
        /// </summary>
        public const Int32 REAGENT_BOTTLE_SETTING_POSITION_COUNT = 3;

        /// <summary>
        /// 温度監視周期(msec）
        /// </summary>
        public const Int32 TEMP_WATCH_TIMER_SECOND = 18000;

        /// <summary>
        /// アナライザ温度適正範囲
        /// </summary>
        public const Double ANALYZER_TEMP_NORMAL_RANGE = 1.0d;

        /// <summary>
        /// アナライザ温度適正範囲2, the range of some items  should  bigger.
        /// </summary>
        public const Double ANALYZER_TEMP_NORMAL_RANGE1 = 1.0d;

        /// <summary>
        /// アナライザ温度適正範囲1.5, the range of some items  should  bigger.
        /// </summary>
        public const Double ANALYZER_TEMP_NORMAL_RANGE1_5 = 1.5d;

        /// <summary>
        /// アナライザ温度適正範囲3, the range of some items  should  bigger.
        /// </summary>
        public const Double ANALYZER_TEMP_NORMAL_RANGE3 = 3.0d;

        /// <summary>
        /// アナライザ温度適正範囲5, the range of some items  should  bigger.
        /// </summary>
        public const Double ANALYZER_TEMP_NORMAL_RANGE5 = 5.0d;

        /// <summary>
        /// シーケンス動作待ち時間
        /// </summary>
        public const Int32 SEQUENCE_WAIT_TIME = 1000 * 30;

        /// <summary>
        /// 希釈液分注量(10倍)(単位:μl)
        /// </summary>
        public const Int32 DIL_DISPENCE_VOLUME_FOR_X10 = 180;

        /// <summary>
        /// 希釈液分注量(20倍)(単位:μl)
        /// </summary>
        public const Int32 DIL_DISPENCE_VOLUME_FOR_X20 = 190;

        /// <summary>
        /// 全モジュールを対象
        /// </summary>
        public const Int32 ALL_MODULEID = -1;

        /// <summary>
        /// STAT（一時登録）のラックポジション
        /// </summary>
        public const Int32 TEMPORARY_STAT_POSITION = 0;

#region _バーコードフォーマット定義_
        /// <summary>
        /// 入力長
        /// </summary>
        public const Int32 BC_INPUT_LENGTH = 21;
        /// <summary>
        /// 日付位置
        /// </summary>
        public const Int32 BC_DAY_POS = 5;
        /// <summary>
        /// 日付長
        /// </summary>
        public const Int32 BC_DAY_LENGTH = 2;
        /// <summary>
        /// 満杯容量位置
        /// </summary>
        public const Int32 BC_CAPACITY_POS = 20;
        /// <summary>
        /// 満杯容量長
        /// </summary>
        public const Int32 BC_CAPACITY_LENGTH = 1;
        /// <summary>
        /// 年月日位置
        /// </summary>
        public const Int32 BC_TIME_POS = 9;
        /// <summary>
        /// 年月日長
        /// </summary>
        public const Int32 BC_TIME_LENGTH = 6;

        /// <summary>
        /// 故障等级
        /// </summary>
        public const String ERROR_LEVEL_1 = "1";

        public const String ERROR_LEVEL_2 = "2";

        public const String ERROR_LEVEL_3 = "3";
#endregion

#region メイン画面のスモールメニュー関連
        public const int ButtonYUpper = 269;                            //スモールメニュー上段のY座標
        public const int ButtonYLower = 584;                            //スモールメニュー下段のY座標
        public const int ButtonMAX6XLeft = 495;                         //スモールメニューにボタンを６つ表示する際の左列のX座標
        public const int ButtonMAX6XMiddle = 810;                       //スモールメニューにボタンを６つ表示する際の中列のX座標
        public const int ButtonMAX6XRight = 1125;                       //スモールメニューにボタンを６つ表示する際の右列のX座標
        public const int ButtonMAX4XLeft = 652;                         //スモールメニューにボタンを４つ表示する際の左列のX座標
        public const int ButtonMAX4XRight = 967;                        //スモールメニューにボタンを４つ表示する際の右列のX座標
        public const int LabelYAdjust = 202;                            //ボタンラベルのY座標の調整値
        public const int LabelYUpper = ButtonYUpper + LabelYAdjust;     //スモールメニュー上段のボタンラベルのY座標
        public const int LabelYLower = ButtonYLower + LabelYAdjust;     //スモールメニュー下段のボタンラベルのY座標
#endregion

    }

    /// <summary>
    /// DPR検出エラーコードデータクラス
    /// </summary>
    /// <remarks>
    /// DPRにより検出されるエラーコードのデータを保持します。
    /// このクラスはCarisXDPRErrorCode内部以外で作成しないで下さい。
    /// </remarks>
    public class DPRErrorCode
    {
        /// <summary>
        /// エラーコード 取得
        /// </summary>
        public Int32 ErrorCode
        {
            get;
            protected set;
        }
        /// <summary>
        /// 引数 設定/取得
        /// </summary>
        public Int32 Arg
        {
            get;
            set;
        }

        /// <summary>
        /// モジュールID
        /// </summary>
        public Int32 ModuleId
        {
            get;
            set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="code">エラーコード</param>
        /// <param name="arg">エラー引数</param>
        /// <param name="moduleId">モジュールID</param>
        public DPRErrorCode(Int32 code, Int32 arg, Int32 moduleId = -1)
        {
            this.ErrorCode = code;
            this.Arg = arg;
            this.ModuleId = moduleId;
        }
    }

    /// <summary>
    /// DPRエラーコードクラス
    /// </summary>
    /// <remarks>
    /// CarisXでのDPRに於いて発生するエラーコードです。
    /// </remarks>
    public static class CarisXDPRErrorCode
    {
        // ■ID読み取りに関するエラー								
        /// <summary>
        /// 希釈液バーコードエラー
        /// </summary>
        /// <remarks>
        /// 希釈液ボトルIDのフォーマットが正しくない、もしくはBCRで読めなかった。
        /// バーコードラベル不良と判断し、エラー画面を表示する。
        /// </remarks>
        public static DPRErrorCode DiluentBarcodeError = new DPRErrorCode(14, 1);
        /// <summary>
        /// プレトリガバーコードエラー
        /// </summary>
        /// <remarks>
        /// プレトリガボトルIDのフォーマットが正しくない、もしくはBCRで読めなかった。
        /// バーコードラベル不良と判断し、エラー画面を表示する。
        /// </remarks>
        public static DPRErrorCode PretriggerBarcodeError = new DPRErrorCode(15, 1);
        /// <summary>
        /// トリガバーコードエラー
        /// </summary>
        /// <remarks>
        /// トリガボトルIDのフォーマットが正しくない、もしくはBCRで読めなかった。
        /// バーコードラベル不良と判断し、エラー画面を表示し、そのR試薬ボトルは存在しないと判断する。
        /// </remarks>
        public static DPRErrorCode TriggerBarcodeError = new DPRErrorCode(16, 1);

        // ■測定に関するエラー
        /// <summary>
        /// ダークエラー
        /// </summary>
        /// <remarks>
        /// ダーク値が100以上あるとき。
        /// エラーを表示し測定結果にリマーク Mdを付加する。
        /// </remarks>
        public static DPRErrorCode DarkError = new DPRErrorCode(30, 1);
        /// <summary>
        /// 測光エラー
        /// </summary>
        /// <remarks>
        /// ダークを減算した後のサンプル測光値が80以下のとき
        /// エラーを表示し測定結果にリマーク Msを付加する。
        /// </remarks>
        public static DPRErrorCode PhotometryError = new DPRErrorCode(31, 1);
        /// <summary>
        /// 測定シャッタエラー
        /// </summary>
        /// <remarks>
        /// 測定用シャッタが0.5秒以内に閉まらなかった場合
        /// エラーを表示する。
        /// </remarks>
        public static DPRErrorCode MeasShutterError = new DPRErrorCode(32, 1);
        /// <summary>
        /// キャリブレーションデータエラー
        /// </summary>
        /// <remarks>
        /// 測定されたカウント値が測定プロトコルパラメータのカウントチェック範囲を超えた場合,もしくは検量線がバイブレーションしている場合
        /// カウントチェックエラーの場合、測定結果にリマーク Crを付加する。
        /// </remarks>
        public static DPRErrorCode CalibrationDataError = new DPRErrorCode(34, 1);
        /// <summary>
        /// ダイナミックレンジ正常範囲外エラー
        /// </summary>
        /// <remarks>
        /// 測定された検体の濃度が測定プロトコルパラメータの濃度ダイナミックレンジ下限値を超えた場合
        /// エラーを表示し測定結果にリマーク　CLを付加する。
        /// </remarks>
        public static DPRErrorCode DynamicRangeOutofNormalRangeErrorLower = new DPRErrorCode(35, 1);
        /// <summary>
        /// ダイナミックレンジ正常範囲外エラー
        /// </summary>
        /// <remarks>
        /// 測定された検体の濃度が測定プロトコルパラメータの濃度ダイナミックレンジ上限値を超えた場合
        /// エラーを表示し測定結果にリマーク　CUを付加する。
        /// </remarks>
        public static DPRErrorCode DynamicRangeOutofNormalRangeErrorUpper = new DPRErrorCode(35, 2);
        /// <summary>
        /// 多重測定内乖離限界エラー
        /// </summary>
        /// <remarks>
        /// 多重測定時、測定されたカウントが測定プロトコルパラメータの多重測定内乖離限界範囲を超えた場合
        /// エラーを表示し測定結果にリマーク Cvを付加する。
        /// </remarks>
        public static DPRErrorCode MultiMeasDivergenceLimitError = new DPRErrorCode(63, 1);
        /// <summary>
        /// 濃度値算出不能エラー
        /// </summary>
        /// <remarks>
        /// 濃度計算不能を検知した場合
        /// エラーを表示し測定結果にリマーク　Ceを付加する。
        /// </remarks>
        public static DPRErrorCode UnableToCalculateConcentrationValue = new DPRErrorCode(36, 2);
        /// <summary>
        /// 管理値範囲外エラー（Xバー管理図）
        /// </summary>
        /// <remarks>
        /// 精度管理検体の測定結果がXバー管理値の範囲外の場合
        /// エラーを表示し測定結果にリマーク　Qrを付加する。
        /// </remarks>
        public static DPRErrorCode OutofControlValueRangeError = new DPRErrorCode(37, 1);
        /// <summary>
        /// 精度管理判定不能エラー
        /// </summary>
        /// <remarks>
        /// Xバー、R管理値が設定されていない場合
        /// エラーを表示し測定結果にリマーク Qsを付加する。
        /// </remarks>
        public static DPRErrorCode QualityControlUndecidableError = new DPRErrorCode(37, 2);
        /// 								
        /// ■通信に関するエラー								
        /// <summary>
        /// マスタ－ホスト間通信エラー
        /// </summary>
        /// <remarks>
        /// ホストへデータ出力するとき、正しくデータを送信できなかった場合。
        /// ・ENQに対して応答がなかったとき。
        /// ・チェックバイトに不一致が生じたとき。
        /// オンライン履歴を参照し確認可。
        /// エラー表示する。
        /// </remarks>
        public static DPRErrorCode CommunicationErrorBetweenMasterAndHost = new DPRErrorCode(50, 1);
        /// <summary>
        /// マスタ－スレーブ間通信エラー
        /// </summary>
        /// <remarks>
        /// マスタPCと本体との通信でエラーを検知した場合
        /// ・ENQに対して応答がなかったとき。
        /// ・チェックバイトに不一致が生じたとき。
        /// エラー表示する。
        /// </remarks>
        public static DPRErrorCode CommunicationErrorBetweenMasterAndSlave = new DPRErrorCode(51, 1);
        /// <summary>
        /// マスタ－ラック搬送間通信エラー
        /// </summary>
        /// <remarks>
        /// マスタPCと本体との通信でエラーを検知した場合
        /// ・ENQに対して応答がなかったとき。
        /// ・チェックバイトに不一致が生じたとき。
        /// エラー表示する。
        /// </remarks>
        public static DPRErrorCode CommunicationErrorBetweenMasterAndRackTransfer = new DPRErrorCode(51, 2);
        /// <summary>
        /// ワークシート問い合わせタイムアウトエラー（マスタ-ホスト間）
        /// </summary>
        /// <remarks>
        /// マスタからのワークシート問い合わせに対し、ホスとがタイムアウト時間内（20秒以内）にワークシートを送信できなかった場合
        /// エラー表示する。
        /// </remarks>
        public static DPRErrorCode AskWorkSheetTimeOut = new DPRErrorCode(55, 1);
        /// <summary>
        /// ワークシートフォーマットエラー（マスタ-ホスト間）
        /// </summary>
        /// <remarks>
        /// ホストから受信したワークシートにフォーマットエラーを検出した場合
        /// エラー表示する。
        /// </remarks>
        public static DPRErrorCode FromHostWorkSheetFormatError = new DPRErrorCode(55, 2);
        /// <summary>
        /// ワークシートエラー（マスタ-ホスト間）
        /// </summary>
        /// <remarks>
        /// ホストから受信したワークシートデータが既に登録されている場合
        /// エラー表示する。
        /// </remarks>
        public static DPRErrorCode FromHostWorkSheetAlreadyExists = new DPRErrorCode(55, 3);
        /// <summary>
        /// ラック移動終了位置決定エラー
        /// </summary>
        /// <remarks>
        /// ラックが回収レーンに戻った場合
        /// エラー表示する。
        /// </remarks>
        public static DPRErrorCode RackTransferError = new DPRErrorCode(90, 0);

        // ■システムに関するエラー								
        /// <summary>
        /// コマンドエラー
        /// </summary>
        /// <remarks>
        /// 予め用意していないコマンドを受信したとき
        /// エラー表示する。
        /// </remarks>
        public static DPRErrorCode CommandError = new DPRErrorCode(60, 0);
        /// <summary>
        /// システムエラー
        /// </summary>
        /// <remarks>
        /// プログラムからのエラーを検出したとき
        /// エラー表示する。
        /// </remarks>
        public static DPRErrorCode SystemError = new DPRErrorCode(61, 0);
        /// <summary>
        /// 試薬ロット切り替わりエラー
        /// </summary>
        /// <remarks>
        /// キャリブレータ分析時、試薬ロットのまたがりを検出したとき
        /// 警告表示し、そのキャリブレータラックは分析をスキップします。
        /// </remarks>
        public static DPRErrorCode CalibReagentLotChangeError = new DPRErrorCode(121, 0);
    }


#region [ Enum ]

#region [ Machine Code ]

    /// <summary>
    /// 装置コード
    /// </summary>
    public enum MachineCode
    {
        /// <summary>
        /// PC
        /// </summary>
        PC = 0x10,
        /// <summary>
        /// スレーブ
        /// </summary>
        Slave = 0x20,
        /// <summary>
        /// ホスト
        /// </summary>
        Host = 0x30,
        /// <summary>
        /// 搬送機
        /// </summary>
        RackTransfer = 0x40
    }

#endregion

#region __DPR検出エラーコード・引数__
    /// <summary>
    /// DPR検出エラーコード
    /// </summary>
    public enum DPRDetectErrorCode : int
    {
        ////////////////////
        // 通信に関するエラー
        ////////////////////
        /// <summary>
        /// マスタ-ホスト間通信エラー
        /// </summary>
        /// <remarks>
        /// ・ENQに応答が無い
        /// ・チェックバイトに不一致が生じた
        /// </remarks>
        DPRToHostCommError = 50,
        /// <summary>
        /// マスタ-スレーブ間通信エラー
        /// </summary>
        DPRToSlaveCommError = 51,
        /// <summary>
        /// ワークシートフォーマットエラー
        /// </summary>
        WorkSheetFormatError = 55,
        /// <summary>
        /// ワークシートエラー
        /// </summary>
        WorkSheetError = 55,



    }

    /// <summary>
    /// DPR検出エラー引数
    /// </summary>
    public enum DPRDetectErrorArg : int
    {
        /// <summary>
        /// 引数1
        /// </summary>
        Arg1 = 1,
        /// <summary>
        /// 引数2
        /// </summary>
        Arg2 = 2,
        /// <summary>
        /// 引数3
        /// </summary>
        Arg3 = 3
    }
#endregion

    /// <summary>
    /// スレーブサブイベント
    /// </summary>
    public enum SlaveSubEvent
    {
        /// <summary>
        /// 待機状態通知
        /// </summary>
        Wait = 1,
        /// <summary>
        /// 分析状態移行中
        /// </summary>
        ToAssay = 2,
        /// <summary>
        /// 分析状態通知
        /// </summary>
        Assay = 3,
        /// <summary>
        /// サンプリング停止要求
        /// </summary>
        RequestSamplingStop = 4,
        /// <summary>
        /// サンプリング停止状態移行中
        /// </summary>
        ToSamplingStop = 5,
        /// <summary>
        /// サンプリング停止状態通知
        /// </summary>
        SamplingStop = 6,
        /// <summary>
        /// サンプリング再開状態通知
        /// </summary>
        SamplingRestart = 7,
        /// <summary>
        /// 分析中断移行中
        /// </summary>
        ToAbortAssay = 8
    }

    /// <summary>
    /// ラック搬送サブイベント
    /// </summary>
    public enum RackTransferSubEvent
    {
        /// <summary>
        /// 待機状態通知
        /// </summary>
        Wait = 1,
        /// <summary>
        /// 稼働状態通知
        /// </summary>
        Running = 2,
        /// <summary>
        /// サンプリング停止状態通知
        /// </summary>
        SamplingStop = 3,
    }

    /// <summary>
    /// 検体カップ種別
    /// </summary>
    public enum SpecimenCupKind
    {
        /// <summary>
        /// 未検出
        /// </summary>
        None = 0,
        /// <summary>
        /// カップ
        /// </summary>
        Cup = 1,
        /// <summary>
        /// チューブ
        /// </summary>
        Tube = 2,
        /// <summary>
        /// カップOnチューブ
        /// </summary>
        CupOnTube = 3
    }

    /// <summary>
    /// ラックステータス
    /// </summary>
    public enum RackStatus
    {
        /// <summary>
        /// 空
        /// </summary>
        Empty = 0,
        /// <summary>
        /// 処理待ち
        /// </summary>
        Waiting = 1,
        /// <summary>
        /// 処理中
        /// </summary>
        InProcess = 2,
        /// <summary>
        /// 処理後
        /// </summary>
        Completed = 3,
        /// <summary>
        /// エラー
        /// </summary>
        Error = 4
    }

    /// <summary>
    /// ラックポジションステータス
    /// </summary>
    public enum RackPosStatus
    {
        /// <summary>
        /// 空
        /// </summary>
        Empty,

        /// <summary>
        /// 検体あり(非エラー)Specimen there (non-error)
        /// </summary>
        Normal,

        /// <summary>
        /// エラー
        /// </summary>
        Error,

        /// <summary>
        /// 未確認
        /// </summary>
        Unknown,

    }


    /// <summary>
    /// NotifyManager通知種別
    /// </summary>
    public enum NotifyKind : int
    {
        /// <summary>
        /// システム状態変化通知
        /// </summary>
        SystemStatusChanged = 0,

        /// <summary>
        /// 初期シーケンス進捗通知
        /// </summary>
        InitializeProgress,

        /// <summary>
        /// 初期シーケンス中断通知
        /// </summary>
        InitializePause,

        /// <summary>
        /// 初期シーケンス再開通知
        /// </summary>
        InitializeStart,

        /// <summary>
        /// 試薬釦点滅通知
        /// </summary>
        BlinkReagentButton,

        /// <summary>
        /// エラー釦点滅通知
        /// </summary>
        BlinkErrorButton,

        /// <summary>
        /// 試薬交換許可通知
        /// </summary>
        ReagentChangeIsAllowed,

        /// <summary>
        /// 試薬交換禁止通知
        /// </summary>
        ReagentChangeIsRefused,

        /// <summary>
        /// 日替わり処理通知
        /// </summary>
        DateChanged,

        /// <summary>
        /// 分析方式種別変更(ラックIDor検体ID)
        /// </summary>
        AssayModeKindChanged,

        /// <summary>
        /// 分析方式種別変更(急診使用ありor全スレーブで急診使用無し)
        /// </summary>
        AssayModeUseOfEmergencyMode,

        /// <summary>
        /// 分析項目測定テーブル変更後通知
        /// </summary>
        AnalyteRoutineTableChanged,

        /// <summary>
        /// 分析項目測定テーブル変更前通知
        /// </summary>
        AnalyteRoutineTableChanging,

        /// <summary>
        /// ラックID割り当てへ変更後通知
        /// </summary>
        RackIdDefinitionChanged,

        /// <summary>
        /// 試薬準備確認応答通知
        /// </summary>
        ReagentPrepareCheckResponse,

        /// <summary>
        /// 試薬準備完了応答通知
        /// </summary>
        ReagentPrepareCompleteResponse,

        /// <summary>
        /// 試薬残量変更確認コマンド通知
        /// </summary>
        ChangeReagentRemainResponse,

        /// <summary>
        /// 試薬交換開始通知
        /// </summary>
        StartReagentTimer,

        /// <summary>
        /// 試薬交換開始中断通知
        /// </summary>
        CancelReagentTimer,

        /// <summary>
        /// リアルタイムデータ通知
        /// </summary>
        RealtimeData,

        /// <summary>
        /// 初期化要求通知
        /// </summary>
        InitializeRequire,

        /// <summary>
        /// インキュベーター温度設定通知
        /// </summary>
        IncubatorTemperatureSetting,

        /// <summary>
        /// 試薬保冷庫温度設定通知
        /// </summary>
        TemperatureSettingColdStorageWarehouseReagent,

        /// <summary>
        /// 消耗品設定通知
        /// </summary>
        StateConsumables,

        /// <summary>
        /// リンス液残量通知
        /// </summary>
        ReagentRemain,

        /// <summary>
        /// リンス処理通知
        /// </summary>
        RinsingSequence,

        /// <summary>
        /// エージング終了通知
        /// </summary>
        SyringeAgingEnd,

        /// <summary>
        /// ユーザレベル変化通知
        /// </summary>
        UserLevelChanged,

        /// <summary>
        /// ラック架設状況変化前通知
        /// </summary>
        BeforeRackLaneChange,

        /// <summary>
        /// 消耗品状態応答通知
        /// </summary>
        SupplieResponced,

        /// <summary>
        /// スレーブプログラムバージョン通知
        /// </summary>
        SlaveVersion,

        /// <summary>
        /// 自動立ち上げ開始通知
        /// </summary>
        AutoStartupStartModule,
        AutoStartupStartRack,

        /// <summary>
        /// 印刷機能有無切替通知
        /// </summary>
        UseOfPrint,

        /// <summary>
        /// ホスト有無切替通知
        /// </summary>
        UseOfHost,

        /// <summary>
        /// 警告灯・ブザー制御送信（設定）
        /// </summary>
        SendCaution,

        /// <summary>
        /// ブザー制御送信（消去）
        /// </summary>
        SendBuzzerCancel,

        /// <summary>
        /// システム終了
        /// </summary>
        SystemEnd,

        /// <summary>
        /// ホストからのワークシート（リアルタイム問合せ）
        /// </summary>
        HostWorkSheet,

        /// <summary>
        /// ホストからのワークシート（バッチ問合せ）
        /// </summary>
        HostWorkSheetBatch,
        HostWorkSheetBatchSTAT,

        /// <summary>
        /// ホストへのバッチ問合せ終了
        /// </summary>
        AskBatchComplete,       //
        AskBatchCompleteSTAT,   //STATの場合

        /// <summary>
        /// ホストへの測定結果送信完了
        /// </summary>
        SendSpecimenDataHostComplete,

        /// <summary>
        /// ホストへの測定結果送信完了
        /// </summary>
        SendControlDataHostComplete,

        /// <summary>
        /// シャットダウン処理（スレーブ）
        /// </summary>
        ShutDownExecuteSlave,

        /// <summary>
        /// 分析項目変更
        /// </summary>
        ChangeProtocolSetting,

        /// <summary>
        /// 温度更新通知
        /// </summary>
        UpdateTemperature,

        /// <summary>
        /// 温度問合せタイマ動作設定通知
        /// </summary>
        SetAskTemperatureTimer,

        /// <summary>
        /// 試薬保冷庫移動応答通知
        /// </summary>
        ReagentCoolerMoveResponse,

        /// <summary>
        /// 洗浄液供給ボタン状態変更通知
        /// </summary>
        WashSolutionExteriorUsable,

        /// <summary>
        /// AnalyteGroup変更通知
        /// </summary>
        ChangeAnalyteGroup,

        /// <summary>
        /// 試薬プローブ交換応答通知
        /// </summary>
        ProbeChangeResponse,

        /// <summary>
        /// キャリブレーションモードの変更通知
        /// </summary>
        CalibrationModeKindChanged,

        /// <summary>
        /// プロトコルバージョン通知
        /// </summary>
        ProtocolVersion,

        /// <summary>
        /// ホストワークシート通知
        /// </summary>
        HostWorkSheetSingle,

        /// <summary>
        /// ラック搬送プログラムバージョン通知
        /// </summary>
        RackTransferVersion,

        /// <summary>
        /// シャットダウン処理（ラック）
        /// </summary>
        ShutDownExecuteRack,

        SmallMenuSpecimenRegistration,
        SmallMenuSpecimenResult,
        SmallMenuSpecimenRetest,
        SmallMenuSpecimenStatRegistration,
        SmallMenuAssayStatus,
        SmallMenuReagentPreparation,
        SmallMenuCalibRegistration,
        SmallMenuCalibStatus,
        SmallMenuCalibAnalysis,
        SmallMenuCalibResult,
        SmallMenuControlRegistration,
        SmallMenuControlQC,
        SmallMenuControlResult,
        SmallMenuSystemStatus,
        SmallMenuSystemAnalytes,
        SmallMenuSystemLog,
        SmallMenuSystemOption,
        SmallMenuSystemAdjustment,
        SmallMenuSystemUser,
        SmallMenuSystemModuleOption,

        ParameterResponse,
        TempPIDResponse,
        MaintenanceAbort,
        ModuleConnect,
        ModuleChange,   //選択しているモジュールを変更

        /// <summary>
        /// 汎用準備開始コマンド通知（プレトリガ、トリガ、ケース（反応容器、サンプル分注チップ））
        /// </summary>
        CommonPrepareStartResponse,

        /// <summary>
        /// 汎用残量変更コマンド通知（希釈液、プレトリガ、トリガ、ケース（反応容器、サンプル分注チップ））
        /// </summary>
        ChangeCommonRemainResponse,

        /// <summary>
        /// モーター初期化完了通知
        /// </summary>
        MotorInitializeCompleted,

        /// <summary>
        /// メンテナンス画面開始処理完了通知
        /// </summary>
        MaintenanceStartCompleted,

        /// <summary>
        /// 洗浄液タンク状態通知
        /// </summary>
        WashSolutionTankStatus,

        /// <summary>
        /// 自動立ち上げチェック通知
        /// </summary>
        AutoStartupCheck,

        /// <summary>
        /// STAT測定可能モジュール更新
        /// </summary>
        UpdateStatMeasurableModule,

        /// <summary>
        /// 試薬設置ガイダンス表示通知
        /// </summary>
        ShowReagentSetGuidance,
        
        /// <summary>
        /// システムステータスアイコン設定通知
        /// </summary>
        SetSystemStatusIcon,
        
        /// <summary>
        /// システムステータス変更通知
        /// </summary>
        ChangeAbortAssay,

        /// <summary>
        /// リアルタイムファイル出力通知
        /// </summary>
        RealtimeOutputFileData,
        
        /// <summary>
        /// リアルタイム印刷通知
        /// </summary>
        RealtimePrintData,

        /// <summary>
        /// ラック、モジュールのモーターエラー通知
        /// </summary>
        CheckErrorRackModule,

        // 2020-02-27 CarisX IoT Add [START]
        /// <summary>
        /// IoTへの接続有無通知
        /// </summary>
        UseOfIoT,

        /// <summary>
        /// IoT障害情報コマンド送信通知
        /// </summary>
        SendErrorToIoT,
        // 2020-02-27 CarisX IoT Add [END]
        
        /// <summary>
        /// 一般検体のIDエラー通知
        /// </summary>
        SpecimenPatientIDformatError,

        /// <summary>
        /// STATの検体IDエラー通知
        /// </summary>
        STATPatientIDformatError,

        /// <summary>
        /// 精度管理の検体IDエラー通知
        /// </summary>
        ControlPatientIDformatError,

        /// <summary>
        /// ラック分析開始コマンド送信通知
        /// </summary>
        RackTransferStartAssay,
    }

    /// <summary>
    /// リアルタイム更新種別
    /// </summary>
    public enum RealtimeDataKind
    {
        /// <summary>
        /// 検体登録画面更新
        /// </summary>
        SampleRegist,
        /// <summary>
        /// STAT登録画面更新
        /// </summary>
        StatRegist,
        /// <summary>
        /// 再検査画面更新
        /// </summary>
        SampleRetest,
        /// <summary>
        /// 分析ステータス画面更新
        /// </summary>
        AssayData,
        /// <summary>
        /// 試薬情報画面更新
        /// </summary>
        ReagentData,
        /// <summary>
        /// 検体測定データ画面更新
        /// </summary>
        SampleResult,
        /// <summary>
        /// キャリブレータ測定データ画面更新
        /// </summary>
        CalibResult,
        /// <summary>
        /// 精度管理検体測定データ画面更新
        /// </summary>
        ControlResult,

        // TODO:リアルタイム更新種別定義
    }


    /// <summary>
    /// 定性項目判定種別
    /// </summary>
    public enum JudgementType : int
    {
        /// <summary>
        /// ポジティブ
        /// </summary>
        Positive = 0,

        /// <summary>
        /// 不確定
        /// </summary>
        Half = 1,

        /// <summary>
        /// ネガティブ
        /// </summary>
        Negative = 2,
    }


    /// <summary>
    /// 分析項目登録状態種別
    /// </summary>
    public enum ProtocolRegistStatus : int
    {
        /// <summary>
        /// 空
        /// </summary>
        Empty = 0,

        /// <summary>
        /// 不確定
        /// </summary>
        Uncertain = 1,

        /// <summary>
        /// 登録済
        /// </summary>
        Registerd = 2,
    }

    /// <summary>
    /// ラック種別
    /// </summary>
    /// <remarks>
    /// ラックの配置されているレーンの種別です。
    /// </remarks>
    public enum RackLane
    {
        /// <summary>
        /// 通常レーン
        /// </summary>
        Normal,
        /// <summary>
        /// 優先レーン
        /// </summary>
        Priority
    }

    /// <summary>
    /// 検体種別
    /// </summary>
    public enum SampleKind : int
    {
        /// <summary>
        /// 一般検体
        /// </summary>
        Sample = 1,
        /// <summary>
        /// 優先検体
        /// </summary>
        Priority = 2,
        /// <summary>
        /// コントロール
        /// </summary>
        Control = 3,
        /// <summary>
        /// キャリブレータ
        /// </summary>
        Calibrator = 4,
        /// <summary>
        /// 搬送ライン(基本的に不使用、測定結果でスレーブより送信されたら、コマンド解析時の時点で一般検体に変換する）
        /// </summary>
        Line = 5
    }

    /// <summary>
    /// 検体種別（移動元）
    /// </summary>
    public enum SampleMoveSourceKind : int
    {
        /// <summary>
        /// 一般検体(ラックから受け取った)
        /// </summary>
        Sample = 0,
        /// <summary>
        /// 外部搬送
        /// </summary>
        Line = 1,
        /// <summary>
        /// スタット（緊急検体、優先検体と同意）
        /// </summary>
        STAT = 2,
    }

    /// <summary>
    /// 検体容器の種類
    /// </summary>
    public enum SampleContainerKind
    {
        /// <summary>
        /// カップ
        /// </summary>
        Cup = 1,
        /// <summary>
        /// チューブ
        /// </summary>
        Tube = 2,
        /// <summary>
        /// カップオンチューブ
        /// </summary>
        CupOnTube = 3
    }

    /// <summary>
    /// サンプル種別
    /// </summary>
    public enum SpecimenMaterialType : int
    {
        /// <summary>
        /// 血清/血漿
        /// </summary>
        BloodSerumAndPlasma = 1,
        /// <summary>
        /// 尿
        /// </summary>
        Urine = 2

    }

    /// <summary>
    /// 試薬種別
    /// </summary>
    public enum ReagentKind : int
    {
        /// <summary>
        /// 試薬
        /// </summary>
        Reagent = 0,
        /// <summary>
        /// プレトリガ
        /// </summary>
        Pretrigger,
        /// <summary>
        /// トリガ
        /// </summary>
        Trigger,
        /// <summary>
        /// 希釈液
        /// </summary>
        Diluent,
        /// <summary>
        /// サンプル分注チップ
        /// </summary>
        SamplingTip,
        /// <summary>
        /// 反応容器
        /// </summary>
        Cell,
        /// <summary>
        /// 廃液バッファー
        /// </summary>
        WasteBuffer,
        /// <summary>
        /// 廃棄ボックス
        /// </summary>
        WasteBox,
        /// <summary>
        /// 洗浄液バッファ
        /// </summary>
        WashSolutionBuffer,
        /// <summary>
        /// 廃液タンク
        /// </summary>
        WasteTank,
        /// <summary>
        /// 洗浄液タンク
        /// </summary>
        WashSolutionTank,   //DBのReagentテーブルには入らない
    }

    /// <summary>
    /// 廃液タンクの状態種別
    /// </summary>
    public enum WasteStatus
    {
        /// <summary>
        /// 未設置
        /// </summary>
        None,
        /// <summary>
        /// 非満杯
        /// </summary>
        NotFull,
        /// <summary>
        /// 満杯
        /// </summary>
        Full
    }

    /// <summary>
    /// 洗浄液タンクの状態種別
    /// </summary>
    public enum WashSolutionStatus
    {
        /// <summary>
        /// 未設置
        /// </summary>
        None,
        /// <summary>
        /// 非満杯
        /// </summary>
        NotFull,
        /// <summary>
        /// 満杯
        /// </summary>
        Full
    }

    /// <summary>
    /// 廃棄ボックスの状態種別
    /// </summary>
    public enum WasteBoxViewStatus
    {
        /// <summary>
        /// 未設置
        /// </summary>
        None,
        /// <summary>
        /// 非満杯
        /// </summary>
        NotFull,
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
        /// <summary>
        /// 満杯
        /// </summary>
        Full
    }

    /// <summary>
    /// 廃棄ボックス状態
    /// </summary>
    public enum WasteBoxStatus
    {
        /// <summary>
        /// 空
        /// </summary>
        Empty = 0,
        /// <summary>
        /// 警告
        /// </summary>
        Warning = 1,
        /// <summary>
        /// 満杯
        /// </summary>
        Full = 2
    }

    /// <summary>
    /// 残量ステータス
    /// </summary>
    public enum RemainStatus
    {
        /// <summary>
        /// 充分量
        /// </summary>
        Full,

        /// <summary>
        /// 少量
        /// </summary>
        Low,

        /// <summary>
        /// 不足量(空)/未設置
        /// </summary>
        Empty,

        /// <summary>
        /// 中間
        /// </summary>
        /// <remarks>
        /// 最後尾に追加しない場合、BottleViewクラスに影響が出る。
        /// </remarks>
        Middle,
    }

    /// <summary>
    /// Total画面のBuffer明細のステータス
    /// </summary>
    public enum TotalBufferStatus
    {
        /// <summary>
        /// なし
        /// </summary>
        None = 0,

        /// <summary>
        /// 〇
        /// </summary>
        circle = 1,

        /// <summary>
        /// ！
        /// </summary>
        exclamation = 2,

        /// <summary>
        /// ×
        /// </summary>
        cross = 3,
    }

    /// <summary>
    /// 試薬ロット選択状態種別
    /// </summary>
    public enum ReagentLotSelect
    {
        /// <summary>
        /// 現ロット
        /// </summary>
        CurrentLot = 0,
        /// <summary>
        /// ロット指定
        /// </summary>
        LotSpecification,
        /// <summary>
        /// 全ロット
        /// </summary>
        LotAll
    }

    /// <summary>
    /// 問い合わせ方式
    /// </summary>
    public enum AskTypeKind
    {
        /// <summary>
        /// ラックID問い合わせ
        /// </summary>
        RackID = 1,
        /// <summary>
        /// 検体ID問い合わせ
        /// </summary>
        SampleID = 2
    }

    /// <summary>
    /// 登録種別
    /// </summary>
    public enum RegistType : int
    {
        /// <summary>
        /// 一時登録
        /// </summary>
        Temporary = 0,
        /// <summary>
        /// 固定登録
        /// </summary>
        Fixed = 1
    }

    /// <summary>
    /// ラック移動方式
    /// </summary>
    public enum RackMovementMethodKind : int
    {
        /// <summary>
        /// パフォーマンス重視
        /// </summary>
        Performance = 1,
        /// <summary>
        /// コスト重視
        /// </summary>
        Cost = 2,
    }

    /// <summary>
    /// 再検情報取得対象種別
    /// </summary>
    public enum GetAutoRemeasKind : int
    {
        /// <summary>
        /// 全て
        /// </summary>
        All = 1,
        /// <summary>
        /// 手動のみ
        /// </summary>
        Manual = 2,
        /// <summary>
        /// 自動のみ
        /// </summary>
        Auto = 3,
        /// <summary>
        /// 画面表示対象のみ
        /// </summary>
        Disp = 4,
    }

    /// <summary>
    /// メンテナンス日誌種別
    /// </summary>
    public enum MaintenanceJournalType : int
    {
        /// <summary>
        /// ユーザー用
        /// </summary>
        User = 0,
        /// <summary>
        /// サービスマン用
        /// </summary>
        Serviceman = 1,
    }

    /// <summary>
    /// メンテナンス日誌種類
    /// </summary>
    public enum Kind : int
    {
        /// <summary>
        /// ユーザーデイリー
        /// </summary>
        U_Daily = 0,
        /// <summary>
        /// ユーザーウィークリー
        /// </summary>
        U_Weekly = 1,
        /// <summary>
        /// ユーザーマンスリー
        /// </summary>
        U_Monthly = 2,
        /// <summary>
        /// サービスマンマンスリー
        /// </summary>
        S_Monthly = 3,
        /// <summary>
        /// サービスマンイヤーリー
        /// </summary>
        S_Yearly = 4
    };

    #endregion

    /// <summary>
    /// Enum用の拡張メソッドクラス
    /// </summary>
    public static class CarisXEnumExtension
    {
#region _JudgementType_

        /// <summary>
        /// 列挙値に関連する文字列の取得
        /// </summary>
        /// <remarks>
        /// 列挙値に関連する文字列を返します。
        /// </remarks>
        /// <param name="type">列挙値</param>
        /// <returns>成功:変換文字列("+"/"-"/"+-") / 失敗:null</returns>
        public static String ToTypeString(this JudgementType type)
        {
            String result = null;
            switch (type)
            {
                case JudgementType.Positive:
                    result = CarisXConst.JUDGE_POSITIVE;
                    break;
                case JudgementType.Negative:
                    result = CarisXConst.JUDGE_NEGATIVE;
                    break;
                case JudgementType.Half:
                    result = CarisXConst.JUDGE_HALF;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 列挙値に関連する文字列からの列挙値の設定
        /// </summary>
        /// <remarks>
        /// 列挙値に関連する文字列からの列挙値を設定します。
        /// </remarks>
        /// <param name="type">関連する文字列</param>
        /// <returns>true:設定成功</returns>
        public static Boolean SetTypeFromString(this String kind, ref JudgementType type)
        {
            Boolean result = false;
            if (kind == CarisXConst.JUDGE_HALF || kind.Contains(CarisXConst.JUDGE_HALF))
            {
                type = JudgementType.Half;
                result = true;
            }
            else if (kind == CarisXConst.JUDGE_POSITIVE || kind.Contains(CarisXConst.JUDGE_POSITIVE))
            {
                type = JudgementType.Positive;
                result = true;
            }
            else if (kind == CarisXConst.JUDGE_NEGATIVE || kind.Contains(CarisXConst.JUDGE_NEGATIVE))
            {
                type = JudgementType.Negative;
                result = true;
            }

            return result;
        }


#endregion

#region _SpecimenMaterialType_

        /// <summary>
        /// 列挙値に関連する文字列の取得
        /// </summary>
        /// <remarks>
        /// 列挙値に関連する文字列を返します。
        /// </remarks>
        /// <param name="type">列挙値</param>
        /// <returns>成功:変換文字列 / 失敗:null</returns>
        public static String ToTypeString(this SpecimenMaterialType type)
        {
            String result = null;
            switch (type)
            {
                case SpecimenMaterialType.BloodSerumAndPlasma:
                    result = Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_008;
                    break;
                case SpecimenMaterialType.Urine:
                    result = Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_009;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 列挙値に関連する文字列からの列挙値の設定
        /// </summary>
        /// <remarks>
        /// 列挙値に関連する文字列からの列挙値を設定します。
        /// </remarks>
        /// <param name="type">関連する文字列</param>
        /// <returns>true:設定成功</returns>
        public static Boolean SetTypeFromString(this String kind, ref SpecimenMaterialType type)
        {
            Boolean result = false;
            if (kind == Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_008)
            {
                type = SpecimenMaterialType.BloodSerumAndPlasma;
                result = true;
            }
            else if (kind == Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_009)
            {
                type = SpecimenMaterialType.Urine;
                result = true;
            }
            return result;
        }

#endregion

#region _ReagentLotSelect_

        /// <summary>
        /// 列挙値に関連する文字列の取得
        /// </summary>
        /// <remarks>
        /// 列挙値に関連する文字列を返します。
        /// </remarks>
        /// <param name="type">列挙値</param>
        /// <returns>成功:変換文字列("+"/"-"/"+-") / 失敗:null</returns>
        public static String ToTypeString(this ReagentLotSelect type)
        {
            String result = null;
            switch (type)
            {
                case ReagentLotSelect.CurrentLot:
                    result = Oelco.CarisX.Properties.Resources.STRING_CONTROLREGIST_000;
                    break;
                case ReagentLotSelect.LotSpecification:
                    result = Oelco.CarisX.Properties.Resources.STRING_CONTROLREGIST_001;
                    break;
                case ReagentLotSelect.LotAll:
                    result = Oelco.CarisX.Properties.Resources.STRING_CONTROLREGIST_002;
                    break;
            }
            return result;
        }

#endregion

#region _SampleInfo.SampleMeasureStatus_

        /// <summary>
        /// 列挙値に関連する文字列の取得
        /// </summary>
        /// <remarks>
        /// 列挙値に関連する文字列を返します。
        /// </remarks>
        /// <param name="type">列挙値</param>
        /// <returns>成功:変換文字列 / 失敗:null</returns>
        public static String ToTypeString(this SampleInfo.SampleMeasureStatus type)
        {
            String result = null;
            switch (type)
            {
                case SampleInfo.SampleMeasureStatus.Wait:
                    result = Oelco.CarisX.Properties.Resources.STRING_ASSAY_060;
                    break;
                case SampleInfo.SampleMeasureStatus.InProcess:
                    result = Oelco.CarisX.Properties.Resources.STRING_ASSAY_061;
                    break;
                case SampleInfo.SampleMeasureStatus.End:
                    result = Oelco.CarisX.Properties.Resources.STRING_ASSAY_062;
                    break;
                case SampleInfo.SampleMeasureStatus.Error:
                    result = Oelco.CarisX.Properties.Resources.STRING_ASSAY_063;
                    break;
            }
            return result;
        }

#endregion

#region _RemainStatus_

        /// <summary>
        /// 列挙値に関連する色の取得
        /// </summary>
        /// <remarks>
        /// 列挙値に関連する色を返します。
        /// </remarks>
        /// <param name="type">列挙値</param>
        /// <returns>成功:色 / 失敗:色(Empty[null])</returns>
        public static Color ToColor(this RemainStatus type)
        {
            Color result = Color.Empty;
            switch (type)
            {
                case RemainStatus.Empty:
                    result = CarisXConst.GRID_CELL_REMAIN_COLOR_EMPTY;
                    break;
                case RemainStatus.Low:
                    result = CarisXConst.GRID_CELL_REMAIN_COLOR_LOW;
                    break;
                case RemainStatus.Full:
                    result = CarisXConst.GRID_CELL_REMAIN_COLOR_FULL;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 列挙値に関連するイメージの取得
        /// </summary>
        /// <remarks>
        /// 列挙値に関連するイメージを返します。
        /// </remarks>
        /// <param name="type">列挙値</param>
        /// <returns>成功:色 / 失敗:色(Empty[null])</returns>
        public static Image ToImage(this RemainStatus type)
        {
            Image result = null;
            switch (type)
            {
                case RemainStatus.Empty:
                    result = global::Oelco.CarisX.Properties.Resources.Image_Indicator_WhiteLarge;
                    break;
                case RemainStatus.Low:
                    result = global::Oelco.CarisX.Properties.Resources.Image_Indicator_YellowLarge;
                    break;
                case RemainStatus.Full:
                    result = global::Oelco.CarisX.Properties.Resources.Image_Indicator_GreenLarge;
                    break;
            }
            return result;
        }
#endregion

#region _MeasureProtocol.CalibrationType_
        /// <summary>
        /// 定量項目であるかどうか
        /// </summary>
        /// <remarks>
        /// 定量項目であるかどうかを返します。
        /// </remarks>
        /// <param name="calibType">キャリブレーションタイプ</param>
        /// <returns>true:定量項目</returns>
        public static Boolean IsQuantitative(this MeasureProtocol.CalibrationType calibType)
        {
            switch (calibType)
            {
                case MeasureProtocol.CalibrationType.Spline:
                case MeasureProtocol.CalibrationType.LogitLog:
                case MeasureProtocol.CalibrationType.FourParameters:
                case MeasureProtocol.CalibrationType.DoubleLogarithmic1:
                case MeasureProtocol.CalibrationType.DoubleLogarithmic2:
                    return true;
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// 定性項目であるかどうか
        /// </summary>
        /// <remarks>
        /// 定量項目であるかどうかを返します。
        /// </remarks>
        /// <param name="calibType">キャリブレーションタイプ</param>
        /// <returns>true:定性項目</returns>
        public static Boolean IsQualitative(this MeasureProtocol.CalibrationType calibType)
        {
            switch (calibType)
            {
                case MeasureProtocol.CalibrationType.CutOff:
                case MeasureProtocol.CalibrationType.INH:
                    return true;
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// 対数値判定
        /// </summary>
        /// <remarks>
        /// 指定のキャリブレーションタイプが対数値かどうかを返します。
        /// </remarks>
        /// <param name="calibType">キャリブレーションタイプ</param>
        /// <returns>true:定性項目</returns>
        public static Boolean IsLogValue(this MeasureProtocol.CalibrationType calibType)
        {
            switch (calibType)
            {
                case MeasureProtocol.CalibrationType.DoubleLogarithmic1:
                case MeasureProtocol.CalibrationType.DoubleLogarithmic2:
                    return true;
                default:
                    break;
            }
            return false;
        }

#endregion

#region _UserLevel_
        /// <summary>
        /// 列挙値に関連するユーザレベルの取得
        /// </summary>
        /// <remarks>
        /// 列挙値に関連するユーザレベル名を返します。
        /// </remarks>
        /// <param name="type">ユーザレベル</param>
        /// <returns>ユーザーレベル名称文字列</returns>
        public static String ToTypeString(this UserLevel type)
        {
            String result = null;
            switch (type)
            {
                case UserLevel.None:
                    result = String.Empty;
                    break;
                case UserLevel.Level1:
                    result = Oelco.CarisX.Properties.Resources.STRING_USERLEVEL_1;
                    break;
                case UserLevel.Level2:
                    result = Oelco.CarisX.Properties.Resources.STRING_USERLEVEL_2;
                    break;
                case UserLevel.Level3:
                    result = Oelco.CarisX.Properties.Resources.STRING_USERLEVEL_3;
                    break;
                case UserLevel.Level4:
                    result = Oelco.CarisX.Properties.Resources.STRING_USERLEVEL_4;
                    break;
                case UserLevel.Level5:
                    result = Oelco.CarisX.Properties.Resources.STRING_USERLEVEL_5;
                    break;
            }
            return result;
        }
#endregion

        /// <summary>
        /// 列挙値に関連するサンプル種の取得
        /// </summary>
        /// <remarks>
        /// 列挙値に関連する分析項目サンプル種を返します。
        /// </remarks>
        /// <param name="sampleType">サンプル種別</param>
        /// <returns>変換後分析項目サンプル種</returns>
        public static MeasureProtocol.SampleTypeKind ToProtocolSampleKind(this SpecimenMaterialType sampleType)
        {
            MeasureProtocol.SampleTypeKind sampleKind;
            switch (sampleType)
            {
                case SpecimenMaterialType.BloodSerumAndPlasma:
                    sampleKind = MeasureProtocol.SampleTypeKind.SerumOrPlasma;
                    break;
                case SpecimenMaterialType.Urine:
                    sampleKind = MeasureProtocol.SampleTypeKind.Urine;
                    break;
                default:
                    sampleKind = MeasureProtocol.SampleTypeKind.None;
                    break;
            }
            return sampleKind;
        }

#region _AnalysisMode_

        /// <summary>
        /// 列挙値に関連する文字列の取得
        /// </summary>
        /// <remarks>
        /// 列挙値に関連する文字列を返します。
        /// </remarks>
        /// <param name="type">列挙値</param>
        /// <returns>成功:変換文字列 / 失敗:null</returns>
        public static String ToTypeString(this AnalysisModeKind type)
        {
            String result = null;
            switch (type)
            {
                case AnalysisModeKind.Standard:
                    result = Properties.Resources.STRING_ANALYSISMODE_000;
                    break;
                case AnalysisModeKind.Emergency:
                    result = Properties.Resources.STRING_ANALYSISMODE_001;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 列挙値に関連する文字列からの列挙値の設定
        /// </summary>
        /// <remarks>
        /// 列挙値に関連する文字列からの列挙値を設定します。
        /// </remarks>
        /// <param name="type">関連する文字列</param>
        /// <returns>true:設定成功</returns>
        public static Boolean SetTypeFromString(this String kind, ref AnalysisModeKind type)
        {
            Boolean result = false;
            if (kind == Properties.Resources.STRING_ANALYSISMODE_000)
            {
                type = AnalysisModeKind.Standard;
                result = true;
            }
            else if (kind == Properties.Resources.STRING_ANALYSISMODE_001)
            {
                type = AnalysisModeKind.Emergency;
                result = true;
            }
            return result;
        }

#endregion

    }

    /// <summary>
    /// 日時用の拡張メソッドクラス
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// 日付表示用文字列変換
        /// </summary>
        /// <remarks>
        /// 引数として渡されている日付を、現在のカルチャ設定に基づいた書式に変換した値を返します。
        /// </remarks>
        /// <param name="dateTime">変化前日付情報</param>
        /// <returns>日付表示文字列</returns>
        public static String ToDispString(this DateTime dateTime)
        {
            return dateTime.ToString(GlobalConst.DISPLAY_DATETIME_FORMAT);
        }
    }

    /// <summary>
    /// 測定項目リスト
    /// </summary>
    public class MeasItem
    {
        /// <summary>
        /// ユニーク番号
        /// </summary>
        public Int32 UniqNo;
        /// <summary>
        /// 測定順番
        /// </summary>
        public Int32 TurnNo;
        /// <summary>
        /// プロトコル番号（分析項目番号）
        /// </summary>
        public Int32 ProtoNo;
        /// <summary>
        /// リプリケーション数（多重測定回数）
        /// </summary>
        public Int32 RepCount;
        /// <summary>
        /// 後希釈倍率（自動稀釈倍率）
        /// </summary>
        public Int32 AutoDil;
        /// <summary>
        /// 試薬ロット番号
        /// </summary>
        public String ReagentLotNo = "";
        /// <summary>
        /// 現ロット使用
        /// </summary>
        public Boolean UseCurrentLot = false;

    }

    /// <summary>
    /// サンプルラックデータ
    /// </summary>
    public class SampleRackData
    {
        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID = "";

        /// <summary>
        /// 検体ID
        /// </summary>
        public Int32[] SampleID;

        /// <summary>
        /// ラックステータス
        /// </summary>
        public RackStatus RackStatus;

        /// <summary>
        /// 残サイクル
        /// </summary>
        public Int32 NumRemainingCycles;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SampleRackData()
        {
            SampleID = new Int32[CarisXConst.RACK_POS_COUNT];
        }
    }

    /// <summary>
    /// STATデータ
    /// </summary>
    public class STATData
    {
        /// <summary>
        /// 検体ID
        /// </summary>
        public Int32[] SampleID;

        /// <summary>
        /// ラックステータス
        /// </summary>
        public RackStatus RackStatus;

        /// <summary>
        /// 残サイクル
        /// </summary>
        public Int32 NumRemainingCycles;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public STATData()
        {
            SampleID = new Int32[CarisXConst.STAT_POS_COUNT];
        }
    }

    /// <summary>
    /// 分量情報
    /// </summary>
    public class AmountInfo
    {
        /// <summary>
        /// 分量（単位は項目により変化する）
        /// </summary>
        public Int32 Amount;
    }
    /// <summary>
    /// 試薬分量情報
    /// </summary>
    public class ReagentAmountInfo : AmountInfo
    {
        /// <summary>
        /// 試薬種別
        /// </summary>
        public Int32 ReagKind;      //試薬種
        /// <summary>
        /// 試薬コード
        /// </summary>
        public Int32 ReagCode;      //試薬コード
        /// <summary>
        /// 先頭分析項目Index(Reagentへの情報参照のみ利用する）
        /// </summary>
        public Int32 ProtocolIndex;

        /// <summary>
        /// 試薬ロット
        /// </summary>
        public String LotNo;

        /// <summary>
        /// R1試薬
        /// </summary>
        public Int32 R1Reagent;

        /// <summary>
        /// R2試薬
        /// </summary>
        public Int32 R2Reagent;

        /// <summary>
        /// M試薬
        /// </summary>
        public Int32 MReagent;

        /// <summary>
        /// 前処理1
        /// </summary>
        public Int32 PreProcess1;

        /// <summary>
        /// 前処理2
        /// </summary>
        public Int32 PreProcess2;
    }

    /// <summary>
    /// テスト回数（希釈液用）
    /// </summary>
    /// <remarks>
    /// 複数分析項目に対して単一試薬を考慮する為追加
    /// </remarks>
    public class TestCountWithDispenceVol
    {
        /// <summary>
        /// 必要試薬テスト回数
        /// </summary>
        public Int32 TestCount = 0;
        /// <summary>
        /// R1試薬分注量
        /// </summary>
        public Int32 R1DispenceVolume = 0;
        /// <summary>
        /// R2試薬分注量
        /// </summary>
        public Int32 R2DispenceVolume = 0;
        /// <summary>
        /// M試薬分注量
        /// </summary>
        public Int32 MDispenceVolume = 0;
    }

    public class ReagentAmountInfoWithProtocol : ReagentAmountInfo
    {
        /// <summary>
        /// 使用量リスト
        /// </summary>
        public List<TestCountWithDispenceVol> UseVolumeList = new List<TestCountWithDispenceVol>();
    }

    /// <summary>
    /// 分量テーブル
    /// </summary>
    /// <remarks>
    /// 分析に必要とする物の分量情報を格納します。
    /// </remarks>
    public class AmountTable
    {
        /// <summary>
        /// 試薬分量テーブル
        /// </summary>
        public Dictionary<Int32, ReagentAmountInfo> ReagentAmountInfo = new Dictionary<Int32, ReagentAmountInfo>(); // 試薬コード・試薬使用量
        /// <summary>
        /// 希釈液分量
        /// </summary>
        public AmountInfo DilutionAmountInfo = new AmountInfo();
        /// <summary>
        /// プレトリガ分量
        /// </summary>
        public AmountInfo PreTriggerAmountInfo = new AmountInfo();
        /// <summary>
        /// トリガ分量
        /// </summary>
        public AmountInfo TriggerAmountInfo = new AmountInfo();
        /// <summary>
        /// サンプル分注チップ分量
        /// </summary>
        public AmountInfo SampleTipAmountInfo = new AmountInfo();
        /// <summary>
        /// 洗浄液分量
        /// </summary>
        public AmountInfo WashContainerAmountInfo = new AmountInfo();
        /// <summary>
        /// リンス液分量
        /// </summary>
        public AmountInfo RinceContainerAmountInfo = new AmountInfo();
        /// <summary>
        /// 前処理液1分量
        /// </summary>
        public AmountInfo PreProcess1AmountInfo = new AmountInfo();
        /// <summary>
        /// 前処理液2分量
        /// </summary>
        public AmountInfo PreProcess2AmountInfo = new AmountInfo();
    }

    /// <summary>
    /// ワークシート問い合わせデータ
    /// </summary>         
    /// <remarks>
    /// 
    /// </remarks>
    public class AskWorkSheetData
    {
        /// <summary>
        /// 問い合わせデータ
        /// </summary>
        public IMeasureIndicate AskData;
        /// <summary>
        /// スレーブ用送信コマンド
        /// </summary>
        public Oelco.CarisX.Comm.SlaveCommCommand_1502 ToDprCommand;
        /// <summary>
        /// ホスト用送信コマンドHost for sending commands
        /// </summary>
        public Oelco.CarisX.Comm.HostCommCommand_0002 FromHostCommand;
        /// <summary>
        /// 問い合わせタイムアウト
        /// </summary>
        public Boolean AskTimeOuted = false;
        /// <summary>
        /// 問い合わせ強制終了
        /// </summary>
        public Boolean AskAborted = false;
    }

    public class OutsideTransfer
    {
        /// <summary>
        /// 検体ID
        /// </summary>
        public Int32[] SampleID;

        /// <summary>
        /// ラックステータス
        /// </summary>
        public RackStatus RackStatus;

        /// <summary>
        /// 残サイクル
        /// </summary>
        public Int32 NumRemainingCycles;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OutsideTransfer()
        {
            SampleID = new Int32 [CarisXConst.OUTSIDETRANSFER_POS_COUNT];
        }
    }

    /// <summary>
    /// 試薬交換通知用オブジェクトクラス
    /// </summary>
    public class NotifyObjectSetReagent
    {
        /// <summary>
        /// モジュールIndex
        /// </summary>
        public int ModuleIndex;

        /// <summary>
        /// オブジェクトデータ
        /// </summary>
        public Object ObjectValue;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="moduleIndex"></param>
        /// <param name="objectValue"></param>
        public NotifyObjectSetReagent(int moduleIndex, Object objectValue)
        {
            this.ModuleIndex = moduleIndex;
            this.ObjectValue = objectValue;
        }
    }

    /// <summary>
    /// モーター番号一覧
    /// </summary>
    public enum MotorNoList
    {
        /// <summary>
        /// ラック搬送部送りX軸
        /// </summary>
        RackTransferSendingXAxis = 1,
        /// <summary>
        /// ラック搬送部戻りX軸
        /// </summary>
        RackTransferBackXAxis = 2,
        /// <summary>
        /// ラック引込部Y軸
        /// </summary>
        RackPullinYAxis = 3,
        /// <summary>
        /// ケース搬送部Y軸
        /// </summary>
        CaseTransferYAxis = 4,
        /// <summary>
        /// ケース搬送部Z軸
        /// </summary>
        CaseTransferZAxis = 5,
        /// <summary>
        /// 試薬保冷庫部テーブルθ軸
        /// </summary>
        ReagentStorageTableThetaAxis = 6,
        /// <summary>
        /// 試薬保冷庫部撹拌θ軸
        /// </summary>
        ReagentStorageMixingThetaAxis = 7,
        /// <summary>
        /// スタット部Y軸
        /// </summary>
        STATYAxis = 8,
        /// <summary>
        /// サンプル分注移送部Y軸
        /// </summary>
        SampleDispenseArmYAxis = 9,
        /// <summary>
        /// サンプル分注移送部Z軸
        /// </summary>
        SampleDispenseArmZAxis = 10,
        /// <summary>
        /// サンプル分注移送部θ軸
        /// </summary>
        SampleDispenseArmThetaAxis = 11,
        /// <summary>
        /// サンプル分注移送部ｻﾝﾌﾟﾙｼﾘﾝｼﾞ
        /// </summary>
        SampleDispenseSyringe = 12,
        /// <summary>
        /// 反応容器搬送部X軸
        /// </summary>
        ReactionCellTransferXAxis = 13,
        /// <summary>
        /// 反応容器搬送部Z軸
        /// </summary>
        ReactionCellTransferZAxis = 14,
        /// <summary>
        /// 反応テーブル部θ軸
        /// </summary>
        ReactionTableThetaAxis = 15,
        /// <summary>
        /// BFテーブル部θ軸
        /// </summary>
        BFTableThetaAxis = 16,
        /// <summary>
        /// 撹拌部　R1撹拌Zθ
        /// </summary>
        ReactionTableR1MixingZThetaAxis = 17,
        /// <summary>
        /// 撹拌部　R2撹拌Zθ
        /// </summary>
        BFTableR2MixingZThetaAxis = 18,
        /// <summary>
        /// 撹拌部　BF1撹拌Zθ
        /// </summary>
        BFTableBF1MixingZThetaAxis = 19,
        /// <summary>
        /// 撹拌部　BF2撹拌Zθ
        /// </summary>
        BFTableBF2MixingZThetaAxis = 20,
        /// <summary>
        /// 撹拌部　ｐTr撹拌Zθ
        /// </summary>
        BFTablePreTriggerMixingZThetaAxis = 21,
        /// <summary>
        /// トラベラー・廃棄部X軸
        /// </summary>
        TravelerXAxis = 22,
        /// <summary>
        /// トラベラー・廃棄部Z軸
        /// </summary>
        TravelerZAxis = 23,
        /// <summary>
        /// 試薬分注1部θ軸
        /// </summary>
        R1DispenseArmThetaAxis = 24,
        /// <summary>
        /// 試薬分注1部Z軸
        /// </summary>
        R1DispenseArmZAxis = 25,
        /// <summary>
        /// 試薬分注2部θ軸
        /// </summary>
        R2DispenseArmThetaAxis = 26,
        /// <summary>
        /// 試薬分注2Z軸
        /// </summary>
        R2DispenseArmZAxis = 27,
        /// <summary>
        /// BF1部Z軸
        /// </summary>
        BF1NozzleZAxis = 28,
        /// <summary>
        /// BF1廃液部Z軸
        /// </summary>
        BF1WasteNozzleZAxis = 29,
        /// <summary>
        /// BF2部Z軸
        /// </summary>
        BF2NozzleZAxis = 30,
        /// <summary>
        /// 希釈液分注部Z軸
        /// </summary>
        DiluentDispenseArmZAxis = 31,
        /// <summary>
        /// プレトリガ・トリガノズルZ軸
        /// </summary>
        TriggerAndPreTriggerDispenseNozzleZAxis = 32,
        /// <summary>
        /// 希釈液ｼﾘﾝｼﾞ
        /// </summary>
        DiluentDispenseSyringe = 33,
        /// <summary>
        /// 試薬分注1ｼﾘﾝｼﾞ
        /// </summary>
        R1DispenseSyringe = 34,
        /// <summary>
        /// 試薬分注2ｼﾘﾝｼﾞ
        /// </summary>
        R2DispenseSyringe = 35,
        /// <summary>
        /// 洗浄液ｼﾘﾝｼﾞ
        /// </summary>
        BFWashSyringe = 36,
        /// <summary>
        /// プレトリガ液ｼﾘﾝｼﾞ
        /// </summary>
        PreTriggerDispenseSyringe = 37,
        /// <summary>
        /// トリガ液ｼﾘﾝｼﾞ
        /// </summary>
        TriggerDispenseSyringe = 38,
        /// <summary>
        /// ラック架設部　ラック設置Y軸
        /// </summary>
        RackSetLoadYAxis = 101,
        /// <summary>
        /// ラック架設部　再検ラック待機Y軸
        /// </summary>
        RackSetUnLoadYAxis = 102,
        /// <summary>
        /// ラック架設部　ラック回収Y軸
        /// </summary>
        RackSetTakeOutYAxis = 103,
        /// <summary>
        /// ラック架設部　ラックフィーダX軸
        /// </summary>
        RackSetLoadFeederXAxis = 104,
        /// <summary>
        /// ラック架設部　再検ラックフィーダX軸
        /// </summary>
        RackSetUnLoadFeederXAxis = 105,
        /// <summary>
        /// ラック架設部　ラックスライダーX軸
        /// </summary>
        RackSetSliderXAxis = 106,

        ///モーター番号２００番台はラック→スレーブとの通信用
        ///10の位を対象のモジュールの番号、1の位を図面上のモーター番号と合わせる

        /// <summary>
        /// ラック搬送部送りX軸（モジュール１用）
        /// </summary>
        RackTransferSendingXAxisM1 = 211,
        /// <summary>
        /// ラック搬送部戻りX軸（モジュール１用）
        /// </summary>
        RackTransferBackXAxisM1 = 212,
        /// <summary>
        /// ラック引込部Y軸（モジュール１用）
        /// </summary>
        RackPullinYAxisM1 = 213,
        /// <summary>
        /// ラック搬送部送りX軸（モジュール２用）
        /// </summary>
        RackTransferSendingXAxisM2 = 221,
        /// <summary>
        /// ラック搬送部戻りX軸（モジュール２用）
        /// </summary>
        RackTransferBackXAxisM2 = 222,
        /// <summary>
        /// ラック引込部Y軸（モジュール２用）
        /// </summary>
        RackPullinYAxisM2 = 223,
        /// <summary>
        /// ラック搬送部送りX軸（モジュール３用）
        /// </summary>
        RackTransferSendingXAxisM3 = 231,
        /// <summary>
        /// ラック搬送部戻りX軸（モジュール３用）
        /// </summary>
        RackTransferBackXAxisM3 = 232,
        /// <summary>
        /// ラック引込部Y軸（モジュール３用）
        /// </summary>
        RackPullinYAxisM3 = 233,
        /// <summary>
        /// ラック搬送部送りX軸（モジュール４用）
        /// </summary>
        RackTransferSendingXAxisM4 = 241,
        /// <summary>
        /// ラック搬送部戻りX軸（モジュール４用）
        /// </summary>
        RackTransferBackXAxisM4 = 242,
        /// <summary>
        /// ラック引込部Y軸（モジュール４用）
        /// </summary>
        RackPullinYAxisM4 = 243,
    }

    /// <summary>
    /// RackTransferとSlaveを合わせたインデックスとの照合に使用。
    /// RackTransferが0スタート。
    /// </summary>
    public enum RackModuleIndex
    {
        RackTransfer = 0,
        Module1 = 1,
        Module2 = 2,
        Module3 = 3,
        Module4 = 4
    }

    /// <summary>
    /// RackTransferのインデックスとの照合に使用。
    /// </summary>
    public enum RackIndex
    {
        RackTransfer1 = 0,
    }

    /// <summary>
    /// Slaveのインデックスとの照合に使用。
    /// </summary>
    public enum ModuleIndex
    {
        Module1 = 0,
        Module2 = 1,
        Module3 = 2,
        Module4 = 3
    }

    /// <summary>
    /// 初期シーケンスの実行方法
    /// </summary>
    [Flags]
    public enum InitializeSequencePattern
    {
        /// <summary>
        /// モジュールの場合
        /// </summary>
        Module = 0x001,
        /// <summary>
        /// ラック搬送の場合
        /// </summary>
        RackTransfer = 0x002,
        /// <summary>
        /// ラックまたはモジュールが、ユーザーよりも先に起動している場合
        /// </summary>
        StartsBeforeUser = 0x004,
        /// <summary>
        /// ラックまたはモジュールが、ユーザーよりも後に起動している場合
        /// </summary>
        StartsAfterUser = 0x008,
    }

    /// <summary>
    /// 初期シーケンスの実行方法
    /// </summary>
    [Flags]
    public enum HostCommunicationSequencePattern
    {
        /// <summary>
        /// ワークシート問合せ（リアルタイム）
        /// </summary>
        AskWorkSheetToHost = 0x001,
        /// <summary>
        /// ワークシート問合せ（バッチ）
        /// </summary>
        AskWorkSheetToHostBatch = 0x002,
        /// <summary>
        /// 測定データ送信（リアルタイム）
        /// </summary>
        SendResultToHost = 0x004,
        /// <summary>
        /// 測定データ送信（バッチ）
        /// </summary>
        SendResultToHostBatch = 0x008,
        /// <summary>
        /// 検体
        /// </summary>
        Specimen = 0x010,
        /// <summary>
        /// コントロール
        /// </summary>
        Control = 0x020,
        /// <summary>
        /// STAT
        /// </summary>
        STAT = 0x040,
    }

    /// <summary>
    /// 位置種別
    /// </summary>
    public enum RackPositionKind
    {
        /// <summary>
        /// ラック待機
        /// </summary>
        Rack,
        /// <summary>
        /// モジュール１
        /// </summary>
        Module1,
        /// <summary>
        /// モジュール２
        /// </summary>
        Module2,
        /// <summary>
        /// モジュール３
        /// </summary>
        Module3,
        /// <summary>
        /// モジュール４
        /// </summary>
        Module4,
        /// <summary>
        /// ラック回収
        /// </summary>
        CollectRack,
    }

    /// <summary>
    /// 分析モード
    /// </summary>
    public enum AnalysisModeKind
    {
        /// <summary>
        /// 通常
        /// </summary>
        Standard,
        /// <summary>
        /// 急診
        /// </summary>
        Emergency,
    }

    /// <summary>
    /// タンクバッファ種別
    /// </summary>
    public enum TankBufferKind
    {
        /// <summary>
        /// タンク
        /// </summary>
        Tank,
        /// <summary>
        /// バッファ
        /// </summary>
        Buffer,
    }

    /// <summary>
    /// 洗浄液タンクの赤色に変更するエラーコード
    /// </summary>
    public enum ErrorCodeWashSolutionTankChangeRed
    {
        ErrorCode45 = 45,
        ErrorCode105 = 105,
    }

    /// <summary>
    /// 洗浄液タンクのステータス
    /// </summary>
    public enum WashSolutionTankStatusKind
    {
        Empty = 0,  //白（使用しない
        Low = 1,    //赤
        Middle = 2, //黄（使用しない）
        Full = 3,   //緑
    }

    /// <summary>
    /// 試薬準備エラー対象
    /// </summary>
    [Flags]
    public enum ReagentPreparationErrorTarget
    {
        /// <summary>
        /// Ｍ試薬の場合
        /// </summary>
        M = 0x001,
        /// <summary>
        /// R1(T1)試薬の場合
        /// </summary>
        R1orT1 = 0x002,
        /// <summary>
        /// R2(T2)試薬の場合
        /// </summary>
        R2orT2 = 0x004,
    }

    /// <summary>
    /// STAT状態の要求
    /// </summary>
    public enum STATStatusRequest
    {
        /// <summary>
        /// 待機
        /// </summary>
        Wait = 0,
        /// <summary>
        /// SW押下待ち
        /// </summary>
        WaitSWPress = 1,
        /// <summary>
        /// 検体取込
        /// </summary>
        SampleUptake = 2,
        /// <summary>
        /// 排出
        /// </summary>
        Emission = 3,
    }

    /// <summary>
    /// STAT状態
    /// </summary>
    public enum STATStatus
    {
        /// <summary>
        /// 受付不可
        /// </summary>
        NotAccepted = 0,
        /// <summary>
        /// 受付可
        /// </summary>
        Acceptable = 1,
        /// <summary>
        /// SW押下
        /// </summary>
        SWPressed = 2,
        /// <summary>
        /// 検体無し
        /// </summary>
        NoSample = 3,
        /// <summary>
        /// 検体有り
        /// </summary>
        OkSample = 4,
        /// <summary>
        /// 吸引完了
        /// </summary>
        Aspiration = 5,
    }

    /// <summary>
    /// 初期シーケンスの進捗状況種別
    /// </summary>
    public enum ProgressInfoEndStatusKind
    {
        /// <summary>
        /// 未処理
        /// </summary>
        NotComplete = 0,
        /// <summary>
        /// 完了
        /// </summary>
        Completed,
        /// <summary>
        /// エラー
        /// </summary>
        Error,
    }

    /// <summary>
    /// モジュールカテゴリビット
    /// </summary>
    /// <remarks>
    /// モジュールによる分析結果の絞込みの条件として使用する分類
    /// ※絞り込みで使用
    /// </remarks>
    [Flags]
    public enum ModuleCategory : long
    {
        /// <summary>
        /// モジュール１
        /// </summary>
        Module1 = 0x0000000000000001,
        /// <summary>
        /// モジュール２
        /// </summary>
        Module2 = 0x0000000000000002,
        /// <summary>
        /// モジュール３
        /// </summary>
        Module3 = 0x0000000000000004,
        /// <summary>
        /// モジュール４
        /// </summary>
        Module4 = 0x0000000000000008,
    }

    /// <summary>
    /// モジュール + 外部搬送カテゴリビット
    /// </summary>
    /// <remarks>
    /// エラー履歴によるフィルタリングの絞り込みの条件として使用する分類
    /// ※絞り込みで使用
    /// </remarks>
    [Flags]
    public enum ErrorFilteringCategory : long
    {
        /// <summary>
        /// モジュール１
        /// </summary>
        Module1 = 0x0000000000000001,
        /// <summary>
        /// モジュール２
        /// </summary>
        Module2 = 0x0000000000000002,
        /// <summary>
        /// モジュール３
        /// </summary>
        Module3 = 0x0000000000000004,
        /// <summary>
        /// モジュール４
        /// </summary>
        Module4 = 0x0000000000000008,
        /// <summary>
        /// ラック搬送
        /// </summary>
        RackTransfer = 0x0000000000000010,
        /// <summary>
        /// DPR
        /// </summary>
        DPR = 0x0000000000000020,
    }

    /// <summary>
    /// 項目間演算
    /// </summary>
    /// /// <remarks>
    /// 項目間演算時に使用する分析項目の分類
    /// </remarks>
    public enum BetweenItemsCalc
    {
        /// <summary>
        /// f-PSA
        /// 分析項目番号:4
        /// </summary>
        f_PAS = 4,
        /// <summary>
        /// T-PSA
        /// 分析項目番号:5
        /// </summary>
        T_PSA = 5,
        /// <summary>
        /// CA125
        /// 分析項目番号:15
        /// </summary>
        CA125 = 15,
        /// <summary>
        /// HE4
        /// 分析項目番号:25
        /// </summary>
        HE4 = 25,
        /// <summary>
        /// PG1
        /// 分析項目番号:26
        /// </summary>
        PG1 = 26,
        /// <summary>
        /// PG2
        /// 分析項目番号:27
        /// </summary>
        PG2 = 27,
    }

    /// <summary>
    /// SequenceHelperからメッセージを送信する際のパラメータ設定用クラス
    /// </summary>         
    public class SequenceHelperMessage
    {
        /// <summary>
        /// モジュールINDEX
        /// </summary>
        public Int32 ModuleIndex { get; set; }

        /// <summary>
        /// メッセージパラメータ
        /// </summary>
        public object MessageParameter { get; set; }
    }

#region "試薬残量関連テーブル"

    /// <summary>
    /// 試薬種
    /// </summary>
    public enum ReagentType
    {
        /// <summary>
        /// M試薬
        /// </summary>
        M = 1,
        /// <summary>
        /// R1/R2試薬
        /// </summary>
        R1R2 = 2,
        /// <summary>
        /// 前処理液T1/T2
        /// </summary>
        T1T2 = 3,
    };

    /// <summary>
    /// 試薬種詳細
    /// </summary>
    public enum ReagentTypeDetail
    {
        /// <summary>
        /// M試薬
        /// </summary>
        M = 1,
        /// <summary>
        /// R1試薬
        /// </summary>
        R1 = 2,
        /// <summary>
        /// R2試薬
        /// </summary>
        R2 = 3,
        /// <summary>
        /// 前準備1
        /// </summary>
        T1 = 4,
        /// <summary>
        /// 前準備2
        /// </summary>
        T2 = 5
    };

    /// <summary>
    /// 試薬バーコード読取フラグ
    /// </summary>
    enum ReadReagentBCFlag
    {
        /// <summary>
        /// 読取あり
        /// </summary>
        Read = 0,
        /// <summary>
        /// 読取なし
        /// </summary>
        NotRead = 1
    };

    /// <summary>
    /// 残量情報
    /// </summary>
    public class RemainingAmountInfo
    {
        /// <summary>
        /// 残量（単位：μL）
        /// </summary>
        public Int32 Remain;          //残量
        /// <summary>
        /// ロット番号(文字列)
        /// </summary>
        public String LotNumber = ""; //ロット番号
        /// <summary>
        /// シリアル番号(文字列)
        /// </summary>
        public Int32 SerialNumber;    //シリアル番号
        /// <summary>
        /// 設置日
        /// </summary>
        public DateTime InstallationData;   //設置日
        /// <summary>
        /// 有効期限
        /// </summary>
        public DateTime TermOfUse;    //有効期限
    }

    /// <summary>
    /// 試薬残量テーブル
    /// </summary>
    public class ReagentRemainTable
    {
        /// <summary>
        /// 試薬種(1:M、2:R1R2、3:前処理液)
        /// </summary>
        public Int32 ReagType;
        /// <summary>
        /// 試薬種詳細(R1,R2,M,T1,T2)
        /// </summary>
        public ReagentTypeDetail ReagTypeDetail;
        /// <summary>
        /// 試薬コード
        /// </summary>
        public Int32 ReagCode;      //試薬コード
        /// <summary>
        /// 残量情報
        /// </summary>
        public RemainingAmountInfo RemainingAmount = new RemainingAmountInfo();
        /// <summary>
        /// メーカーコード
        /// </summary>
        public String MakerCode = "";     //メーカーコード
        /// <summary>
        /// 満杯容量（単位：μL）
        /// </summary>
        public Int32 Capacity;  //満杯容量
    }
    /// <summary>
    /// 稀釈液残量テーブル
    /// </summary>
    public class DilutionRemainTable
    {
        /// <summary>
        /// 残量情報
        /// </summary>
        public RemainingAmountInfo RemainingAmount = new RemainingAmountInfo();
    }
    //プレトリガ残量テーブル
    public class PreTriggerRemainTable
    {
        /// <summary>
        /// 残量情報
        /// </summary>
        public RemainingAmountInfo[] RemainingAmount = new RemainingAmountInfo[2];
        /// <summary>
        /// アクト番号
        /// </summary>
        public Int32 ActNo;         //アクト番号
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PreTriggerRemainTable()
        {
            for (Int32 index = 0; index < RemainingAmount.Length; index++)
            {
                this.RemainingAmount[index] = new RemainingAmountInfo();
            }
        }

    }
    //トリガ残量テーブル
    /// <summary>
    /// トリガ残量テーブル
    /// </summary>
    public class TriggerRemainTable
    {
        /// <summary>
        /// 残量情報
        /// </summary>
        public RemainingAmountInfo[] RemainingAmount = new RemainingAmountInfo[2];
        /// <summary>
        /// アクト番号
        /// </summary>
        public Int32 ActNo;         //アクト番号
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TriggerRemainTable()
        {
            for (Int32 index = 0; index < RemainingAmount.Length; index++)
            {
                this.RemainingAmount[index] = new RemainingAmountInfo();
            }
        }

    }
    //サンプル分注チップ残量テーブル
    /// <summary>
    /// サンプル分注チップ残量テーブル
    /// </summary>
    public class SampleTipRemainTable
    {
        /// <summary>
        /// チップケース上の残量
        /// </summary>
        public Int32[] tipRemainTable = new Int32[CarisXConst.REMAIN_SAMPLINGTIP_CELL_CASE];  //チップケース上の残量
        /// <summary>
        /// アクト番号
        /// </summary>
        public Int32 ActNo;             //アクト番号

    }

    /// <summary>
    /// 反応容器残量テーブル
    /// </summary>
    public class CellRemainTable
    {
        /// <summary>
        /// 反応容器上の残量
        /// </summary>
        public Int32[] reactContainerRemainTable = new Int32[CarisXConst.REMAIN_SAMPLINGTIP_CELL_CASE];   //反応容器上の残量
        /// <summary>
        /// アクト番号
        /// </summary>
        public Int32 ActNo;                         //アクト番号

    }


    /// <summary>
    /// 温度テーブル
    /// </summary>
    public class TemperatureTable
    {
        /// <summary>
        /// 反応テーブル温度
        /// </summary>
        public Double ReactionTableTemp;
        /// <summary>
        /// BFテーブル温度
        /// </summary>
        public Double BFTableTemp;
        /// <summary>
        /// R1プローブ温度
        /// </summary>
        public Double R1ProbeTemp;
        /// <summary>
        /// R2プローブ温度
        /// </summary>
        public Double R2ProbeTemp;
        /// <summary>
        /// B/F1プレヒート温度
        /// </summary>
        public Double BF1PreHeatTemp;
        /// <summary>
        /// B/F2プレヒート温度
        /// </summary>
        public Double BF2PreHeatTemp;
        /// <summary>
        /// 測光部
        /// </summary>
        public Double ChemiluminesoensePtotometryTemp;
        /// <summary>
        /// 試薬保冷庫温度
        /// </summary>
        public Double ReagentBoxTemp;
        /// <summary>
        /// 室温
        /// </summary>
        public Double RoomTemp;
        /// <summary>
        /// 装置温度
        /// </summary>
        public Double AnalyzerTemp;
    }

    /// <summary>
    /// 検定ステータスクラス
    /// </summary>
    public class AssayStatus
    {
        /// <summary>
        /// ユニーク番号・多重測定回ペア数
        /// </summary>
        public const Int32 UNIQUE_PARAM_COUNT = 102; // コマンドリストには101で載っているが、102

        /// <summary>
        /// ユニーク番号・多重測定番号・位置 リストの取得、設定
        /// </summary>
        public List<Tuple<Int32, Int32, Int32>> UniqueNoAndRepAndPosition { get; set; } = new List<Tuple<Int32, Int32, Int32>>();

        /// <summary>
        /// 温度テーブルの取得、設定
        /// </summary>
        public TemperatureTable TemperatureTable { get; set; } = new TemperatureTable();
    }

    /// <summary>
    /// サンプル必要量設定 Aテーブル
    /// </summary>
    public class SampleAmountReqTableA
    {
        /// <summary>
        /// カップデッドボリューム高さの情報を保持します。
        /// </summary>
        public Double HighOfDeadVolForCup;
        /// <summary>
        /// 試験管デッドボリューム高さ(ゴム有)の情報を保持します。
        /// </summary>
        public Double HighOfDeadVolForTubeWithRubber;
        /// <summary>
        /// 試験管デッドボリューム高さ(ゴム無)の情報を保持します。
        /// </summary>
        public Double HighOfDeadVolForTube;
        /// <summary>
        /// カップオンチューブデッドボリューム高さの情報を保持します。
        /// </summary>
        public Double HighOfDeadVolForCupOnTube;
    }

    /// <summary>
    /// サンプル必要量設定 Bテーブル
    /// </summary>
    public class SampleAmountReqTableB
    {
        public const Int32 ELEMENT_COUNT = 27;
        /// <summary>
        /// A列データの情報を保持します。
        /// </summary>
        public Double[] ColA;
        /// <summary>
        /// B列データの情報を保持します。
        /// </summary>
        public Double[] ColB;
        /// <summary>
        /// C列データの情報を保持します。
        /// </summary>
        public Double[] ColC;
        /// <summary>
        /// D列データの情報を保持します。
        /// </summary>
        public Double[] ColD;
        /// <summary>
        /// E列データの情報を保持します。
        /// </summary>
        public Double[] ColE;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public SampleAmountReqTableB()
        {
            ColA = new Double[ELEMENT_COUNT];
            ColB = new Double[ELEMENT_COUNT];
            ColC = new Double[ELEMENT_COUNT];
            ColD = new Double[ELEMENT_COUNT];
            ColE = new Double[ELEMENT_COUNT];
        }
    }

    /// <summary>
    /// サンプル必要量設定クラス
    /// </summary>
    public interface ISampleReqAmount
    {
        /// <summary>
        /// サンプル必要量 Aテーブル
        /// </summary>
        SampleAmountReqTableA TableA
        {
            get;
            set;
        }
        /// <summary>
        /// サンプル必要量 Bテーブル
        /// </summary>
        SampleAmountReqTableB TableB
        {
            get;
            set;
        }
    }
#region "測定データ関連"

    /// <summary>
    /// 測定指示データインターフェース
    /// </summary>
    /// <remarks>
    /// 主に通信コマンドとDB操作クラス間でのデータ中継に利用します。
    /// </remarks>
    public interface IMeasureIndicate
    {
        /// <summary>
        /// ラックID
        /// </summary>
        String RackID { get; set; }

        /// <summary>
        /// 検体区分
        /// </summary>
        SampleKind SampleType { get; set; }

        /// <summary>
        /// 検体ポジション
        /// </summary>
        Int32 SamplePosition { get; set; }

        /// <summary>
        /// 検体識別番号
        /// </summary>
        Int32 IndividuallyNumber { get; set; }

        /// <summary>
        /// 検体ID
        /// </summary>
        String SampleID { get; set; }

        /// <summary>
        /// サンプル種別
        /// </summary>
        SpecimenMaterialType SpecimenMaterial { get; set; }

        /// <summary>
        /// 手希釈倍率
        /// </summary>
        Int32 PreDil { get; set; }

        /// <summary>
        /// 測定項目数
        /// </summary>
        Int32 MeasItemCount { get; set; }

        /// <summary>
        /// カップタイプ
        /// </summary>
        SpecimenCupKind SpecimenCup { get; set; }

        /// <summary>
        /// 測定項目リスト
        /// </summary>
        MeasItem[] MeasItemArray { get; set; }

        /// <summary>
        /// モジュールID
        /// </summary>
        Int32 ModuleID { get; set; }
    }

    /// <summary>
    /// スレーブ分析履歴
    /// </summary>
    /// <remarks>
    /// スレーブから送信される分析履歴のデータクラスです。
    /// </remarks>
    public class SlaveAssayLogInfo
    {
        /// <summary>
        /// 差圧センサー値1
        /// </summary>
        public Int32 DiffSensor1;
        /// <summary>
        /// 差圧センサー値2
        /// </summary>
        public Int32 DiffSensor2;
        /// <summary>
        /// 差圧センサー値3
        /// </summary>
        public Int32 DiffSensor3;
        /// <summary>
        /// 検体分注量
        /// </summary>
        public Int32 SampleDispenseVolume;
        /// <summary>
        /// 検体吸引位置
        /// </summary>
        public Double SampleAspirationPosition;
        /// <summary>
        /// M試薬ポート番号
        /// </summary>
        public Int32 MReagPortNo;
        /// <summary>
        /// M試薬液面位置
        /// </summary>
        public Double MReagLiquidPosition;
        /// <summary>
        /// R1試薬ポート番号
        /// </summary>
        public Int32 R1ReagPortNo;
        /// <summary>
        /// R1試薬液面位置
        /// </summary>
        public Double R1ReagLiquidPosition;
        /// <summary>
        /// R2試薬ポート番号
        /// </summary>
        public Int32 R2ReagPortNo;
        /// <summary>
        /// R2試薬液面位置
        /// </summary>
        public Double R2ReagLiquidPosition;
        /// <summary>
        /// 温度（反応テーブル部）
        /// </summary>
        public Double ReactionTableTemp;
        /// <summary>
        /// 温度（BFテーブル部）
        /// </summary>
        public Double BFTableTemp;
        /// <summary>
        /// 温度（B/F1プレヒート）
        /// </summary>
        public Double BF1PreHeatTemp;
        /// <summary>
        /// 温度（B/F2プレヒート）
        /// </summary>
        public Double BF2PreHeatTemp;
        /// <summary>
        /// 温度（R1プローブプレヒート）
        /// </summary>
        public Double R1ProbeTemp;
        /// <summary>
        /// 温度（R2プローブプレヒート）
        /// </summary>
        public Double R2ProbeTemp;
        /// <summary>
        /// 温度（測光部）
        /// </summary>
        public Double ChemiluminesoensePtotometryTemp;
        /// <summary>
        /// 温度（室温）
        /// </summary>
        public Double RoomTemp;
    }

    /// <summary>
    /// スレーブエラー履歴
    /// </summary>
    /// <remarks>
    /// スレーブから送信されるエラー履歴データクラスです、
    /// 最大3件のエラーが、"###-$$"(#:エラーコード S:)の形式で記録されています。
    /// </remarks>
    public class SlaveErrorLogInfo
    {
        public Int32 ErrorRecord;
        public Tuple<String, String>[] ErrorCodeArg;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveErrorLogInfo()
        {
            ErrorCodeArg = new Tuple<String, String>[3];
        }
    }

    /// <summary>
    /// 測定結果データインターフェース
    /// </summary>
    public interface IMeasureResultData
    {

        /// <summary>
        /// 検体区分
        /// </summary>
        SampleKind SampleKind
        {
            get;
            set;
        }

        /// <summary>
        /// 検体識別番号
        /// </summary>
        Int32 IndividuallyNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 検体ID
        /// </summary>
        String SampleId
        {
            get;
            set;
        }

        /// <summary>
        /// ラックID
        /// </summary>
        String RackID
        {
            get;
            set;
        }

        /// <summary>
        /// ラック位置
        /// </summary>
        Int32 SamplePos
        {
            get;
            set;
        }

        /// <summary>
        /// サンプル種別
        /// </summary>
        SpecimenMaterialType SpecimenMaterialType
        {
            get;
            set;
        }

        /// <summary>
        /// ユニーク番号
        /// </summary>
        Int32 UniqueNo
        {
            get;
            set;
        }

        /// <summary>
        /// 測定順番
        /// </summary>
        Int32 TurnOrder
        {
            get;
            set;
        }

        /// <summary>
        /// プロトコル番号
        /// </summary>
        Int32 MeasProtocolNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 多重測定番号
        /// </summary>
        Int32 RepNo
        {
            get;
            set;
        }

        /// <summary>
        /// ダーク値
        /// </summary>
        Int32 DarkCount
        {
            get;
            set;
        }

        /// <summary>
        /// バックグラウンドカウント
        /// </summary>
        Int32 BGCount { get; set; }

        /// <summary>
        /// 測定カウント
        /// </summary>
        Int32 ResultCount
        {
            get;
            set;
        }

        /// <summary>
        /// リマーク
        /// </summary>
        Oelco.CarisX.Utility.Remark Remark
        {
            get;
            set;
        }

        /// <summary>
        /// 試薬ロット番号
        /// </summary>
        String ReagentLotNumber
        {
            get;
            set;
        }

        /// <summary>
        /// プレトリガロット番号
        /// </summary>
        String PreTriggerLotNo
        {
            get;
            set;
        }

        /// <summary>
        /// トリガロット番号
        /// </summary>
        String TriggerLotNo
        {
            get;
            set;
        }

        /// <summary>
        /// 後希釈倍率
        /// </summary>
        Int32 AfterDilution
        {
            get;
            set;
        }

        /// <summary>
        /// 手希釈倍率
        /// </summary>
        Int32 PreDilution
        {
            get;
            set;
        }

        /// <summary>
        /// カップ種別
        /// </summary>
        SpecimenCupKind CupKind
        {
            get;
            set;
        }

        /// <summary>
        /// 分析履歴
        /// </summary>
        SlaveAssayLogInfo AnalysisLog
        {
            get;
            set;
        }

        /// <summary>
        /// エラー履歴
        /// </summary>
        SlaveErrorLogInfo ErrorLog
        {
            get;
            set;
        }

        /// <summary>
        /// モジュールID
        /// </summary>
        Int32 ModuleID
        {
            get;
            set;
        }
    }
#endregion

    /// <summary>
    /// 残量情報データインターフェース
    /// </summary>
    /// <remarks>
    /// 主に通信コマンドとDB操作クラス間でのデータ中継に利用します。
    /// </remarks>
    public interface IRemainAmountInfoSet
    {

        /// <summary>
        /// 試薬残量テーブル
        /// </summary>
        ReagentRemainTable[] ReagentRemainTable
        {
            get;
            set;
        }
        /// <summary>
        /// 希釈液残量テーブル
        /// </summary>
        DilutionRemainTable DilutionRemainTable
        {
            get;
            set;
        }

        /// <summary>
        /// プレトリガ残量テーブル
        /// </summary>
        PreTriggerRemainTable PreTriggerRemainTable
        {
            get;
            set;
        }

        /// <summary>
        /// トリガ残量テーブル
        /// </summary>
        TriggerRemainTable TriggerRemainTable
        {
            get;
            set;
        }
        /// <summary>
        /// サンプル分注チップ残量テーブル
        /// </summary>
        SampleTipRemainTable SampleTipRemainTable
        {
            get;
            set;
        }

        /// <summary>
        /// セル残量テーブル
        /// </summary>
        CellRemainTable CellRemainTable
        {
            get;
            set;
        }
        /// <summary>
        /// 洗浄液残量
        /// </summary>
        Int32 WashContainerRemain
        {
            get;
            set;
        }

        /// <summary>
        /// リンス液残量
        /// </summary>
        Int32 RinceContainerRemain
        {
            get;
            set;
        }

        /// <summary>
        /// 廃液バッファ満杯フラグ
        /// </summary>
        Boolean IsFullWasteBuffer
        {
            get;
            set;
        }

        /// <summary>
        /// 廃棄ボックス有無
        /// </summary>
        Boolean ExistWasteBox
        {
            get;
            set;
        }

        /// <summary>
        /// 廃棄ボックス満杯状態
        /// </summary>
        WasteBoxStatus WasteBoxCondition
        {
            get;
            set;
        }

        /// <summary>
        /// 取得時刻
        /// </summary>
        DateTime TimeStamp
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 残量情報データインターフェース（ラック搬送）
    /// </summary>
    /// <remarks>
    /// 主に通信コマンドとDB操作クラス間でのデータ中継に利用します。
    /// </remarks>
    public interface IRackRemainAmountInfoSet
    {
        /// <summary>
        /// 廃液満杯フラグ
        /// </summary>
        Boolean IsFullWasteTank
        {
            get;
            set;
        }

        /// <summary>
        /// 廃液タンク有無
        /// </summary>
        Boolean ExistWasteTank
        {
            get;
            set;
        }

        /// <summary>
        /// 取得時刻
        /// </summary>
        DateTime TimeStamp
        {
            get;
            set;
        }
    }
    /// <summary>
    /// 測定結果画面インターフェース
    /// </summary>
    public interface IFormMeasureResult
    {
        /// <summary>
        /// 測定結果列の取得
        /// </summary>
        Dictionary<String, String> ResultGridColumns
        {
            get;
        }
    }

    /// <summary>
    /// キャリブレータロット
    /// </summary>
    public class CalibratorLot
    {
        /// <summary>
        /// キャリブレータロットNo
        /// </summary>
        public String CalibratorLotNo { get; set; }

        /// <summary>
        /// 補正ポイント数
        /// </summary>
        public Int32 ConcCount { get; set; }

        /// <summary>
        /// 補正ポイント
        /// </summary>
        public List<Double> Concentration { get; set; }

        /// <summary>
        /// キャリブレータロット情報 - コンストラクタ
        /// </summary>
        /// <param name="lotNo">キャリブレータロット番号</param>
        /// <param name="concCount">補正ポイント数</param>
        /// <param name="concList">補正ポイントリスト</param>
        public CalibratorLot(String lotNo, Int32 concCount, List<Double> concList)
        {
            this.CalibratorLotNo = lotNo;
            this.ConcCount = concCount;
            this.Concentration = new List<double>();
            foreach (var conc in concList)
            {
                this.Concentration.Add(conc);
            }
        }
    }

#endregion

#region "ホスト用定義"

    /// <summary>
    /// ホスト用サンプル区分
    /// </summary>
    public enum HostSampleType
    {
        /// <summary>
        /// 検体(検体ラック使用、または搬送ライン上の検体)
        /// </summary>
        N = 'N',
        /// <summary>
        /// QC(精度管理)用コントロール(コントロール用ラック使用)
        /// </summary>
        C = 'C'
    }

    /// <summary>
    /// ホスト用サンプル種別
    /// </summary>
    public enum HostSampleKind
    {
        /// <summary>
        /// 血清・血漿
        /// </summary>
        SerumBloodPlasma = 'S',
        /// <summary>
        /// 尿
        /// </summary>
        Urine = 'U'
    }
    /// <summary>
    /// 後希釈
    /// </summary>
    public enum HostAutoDil
    {
        /// <summary>
        /// 希釈なし
        /// </summary>
        NoDil = 1,
        /// <summary>
        /// 10倍
        /// </summary>
        Dil10 = 2,
        /// <summary>
        /// 100倍
        /// </summary>
        Dil100 = 3,
        /// <summary>
        /// 200倍
        /// </summary>
        Dil200 = 4,
        /// <summary>
        /// 1000倍
        /// </summary>
        Dil1000 = 5,
        /// <summary>
        /// 20倍
        /// </summary>
        Dil20 = 6,
        /// <summary>
        /// 400倍
        /// </summary>
        Dil400 = 7,
        /// <summary>
        /// 2000倍
        /// </summary>
        Dil2000 = 8,
        /// <summary>
        /// 4000倍
        /// </summary>
        Dil4000 = 9,
        /// <summary>
        /// 1000倍
        /// </summary>
        Dil8000 = 0,
    }
    #endregion

    #region [拡張メソッド]

    /// <summary>
    /// ホスト-DPR間データ互換拡張メソッドクラス
    /// </summary>
    public static class HostToDPRFormatExtention
    {
        /// <summary>
        /// DPR後希釈倍率取得
        /// </summary>
        /// <remarks>
        /// DPR後希釈倍率取得します
        /// </remarks>
        /// <param name="hostAutoDil">後希釈倍率</param>
        /// <returns>DPR後希釈倍率(数値)</returns>
        public static Int32 GetDPRAutoDilution(this HostAutoDil hostAutoDil)
        {
            Int32 autoDil = 1;

            switch (hostAutoDil)
            {
                case HostAutoDil.NoDil:
                    autoDil = 1;
                    break;
                case HostAutoDil.Dil10:
                    autoDil = 10;
                    break;
                case HostAutoDil.Dil100:
                    autoDil = 100;
                    break;
                case HostAutoDil.Dil200:
                    autoDil = 200;
                    break;
                case HostAutoDil.Dil1000:
                    autoDil = 1000;
                    break;
                case HostAutoDil.Dil20:
                    autoDil = 20;
                    break;
                case HostAutoDil.Dil400:
                    autoDil = 400;
                    break;
                case HostAutoDil.Dil2000:
                    autoDil = 2000;
                    break;
                case HostAutoDil.Dil4000:
                    autoDil = 4000;
                    break;
                case HostAutoDil.Dil8000:
                    autoDil = 8000;
                    break;
                default:
                    autoDil = 0;
                    break;
            }

            return autoDil;
        }

        /// <summary>
        /// DPRサンプル種別取得
        /// </summary>
        /// <remarks>
        /// DPRサンプル種別取得します
        /// </remarks>
        /// <param name="type">ホスト用サンプル区分</param>
        /// <returns>DPRサンプル種別</returns>
        public static SampleKind GetDPRSampleKind(this HostSampleType type)
        {
            SampleKind kind;
            switch (type)
            {
                case HostSampleType.C:
                    kind = SampleKind.Control;
                    break;
                case HostSampleType.N:
                    kind = SampleKind.Sample;
                    break;
                default:
                    kind = SampleKind.Sample;
                    break;
            }
            return kind;
        }


        /// <summary>
        /// ホストサンプル種別取得
        /// </summary>
        /// <remarks>
        /// ホストサンプル種別取得します
        /// </remarks>
        /// <param name="type">サンプル種別</param>
        /// <returns>ホスト用サンプル種別</returns>
        public static HostSampleKind GetHostSampleKind(this SpecimenMaterialType type)
        {
            HostSampleKind hostType;

            switch (type)
            {
                case SpecimenMaterialType.BloodSerumAndPlasma:
                    hostType = HostSampleKind.SerumBloodPlasma;
                    break;
                case SpecimenMaterialType.Urine:
                    hostType = HostSampleKind.Urine;
                    break;
                default:
                    hostType = HostSampleKind.SerumBloodPlasma;
                    break;
            }

            return hostType;
        }

        /// <summary>
        /// サンプル種別取得
        /// </summary>
        /// <remarks>
        /// サンプル種別取得します
        /// </remarks>
        /// <param name="type">ホスト用サンプル種別</param>
        /// <returns>サンプル種別</returns>
        public static SpecimenMaterialType GetDPRSpecimemMaterialType(this HostSampleKind type)
        {
            SpecimenMaterialType dprType;
            switch (type)
            {
                case HostSampleKind.SerumBloodPlasma:
                    dprType = SpecimenMaterialType.BloodSerumAndPlasma;
                    break;
                case HostSampleKind.Urine:
                    dprType = SpecimenMaterialType.Urine;
                    break;
                default:
                    dprType = SpecimenMaterialType.BloodSerumAndPlasma;
                    break;
            }
            return dprType;
        }

        /// <summary>
        /// 温度設定
        /// </summary>
        /// <remarks>
        /// 通信コマンドの温度データをDPRで保持している温度データに設定します。
        /// </remarks>
        /// <param name="dprTemp">DPR側温度データ</param>
        /// <param name="slaveTemp">通信コマンド温度データ</param>
        public static void SetSlaveTemperatureFromDPRTemperature(this Oelco.CarisX.Comm.ItemRSIncTemp slaveTemp, TemperatureParameter dprTemp)
        {
            // 反応テーブル温度
            slaveTemp.ReactionTableTemp = dprTemp.TempReactionTable;
            // BFテーブル温度
            slaveTemp.BFTableTemp = dprTemp.TempBFTable;
            // B/F1プレヒート温度
            slaveTemp.BF1PreHeatTemp = dprTemp.TempBF1PreHeat;
            // B/F2プレヒート温度
            slaveTemp.BF2PreHeatTemp = dprTemp.TempBF2PreHeat;
            // R1プローブ温度
            slaveTemp.R1ProbeTemp = dprTemp.TempR1ProbePreHeat;
            // R2プローブ温度
            slaveTemp.R2ProbeTemp = dprTemp.TempR2ProbePreHeat;
            // 測光部温度
            slaveTemp.ChemiluminesoensePtotometryTemp = dprTemp.TempChemiLightMeas;
        }

        /// <summary>
        /// 温度設定
        /// </summary>
        /// <remarks>
        /// 通信コマンドの温度データをDPRで保持している温度データに設定します。
        /// </remarks>
        /// <param name="dprTemp">DPR側温度データ</param>
        /// <param name="slaveTemp">通信コマンド温度データ</param>
        public static void SetDPRTemperatureFromSlaveTemperature(this Temperature dprTemp, Comm.SlaveCommCommand_1438 slaveTemp)
        {
            // 試薬保冷庫温度
            dprTemp.TempReagentCoolingBox = slaveTemp.CoolerTemp;

            // 室温
            dprTemp.TempRoom = slaveTemp.RoomTemp;

            // 装置温度
            dprTemp.TempDevice = slaveTemp.AnalyzerTemp;

        }

        /// <summary>
        /// 温度設定
        /// </summary>
        /// <remarks>
        /// 通信コマンドの温度データをDPRで保持している温度データに設定します。
        /// </remarks>
        /// <param name="dprTemp">DPR側温度データ</param>
        /// <param name="slaveTemp">通信コマンド温度データ</param>
        public static void SetSlaveTemperatureFromDPRTemperature(this Temperature dprTemp, Comm.ItemRSIncTemp slaveTemp)
        {
            // 反応テーブル温度
            dprTemp.TempReactionTable = slaveTemp.ReactionTableTemp;
            // BFテーブル温度
            dprTemp.TempBFTable = slaveTemp.BFTableTemp;
            // B/F1プレヒート温度
            dprTemp.TempBF1PreHeat = slaveTemp.BF1PreHeatTemp;
            // B/F2プレヒート温度
            dprTemp.TempBF2PreHeat = slaveTemp.BF2PreHeatTemp;
            // R1プローブ温度
            dprTemp.TempR1ProbePreHeat = slaveTemp.R1ProbeTemp;
            // R2プローブ温度
            dprTemp.TempR2ProbePreHeat = slaveTemp.R2ProbeTemp;
            // 測光部温度
            dprTemp.TempChemiLightMeas = slaveTemp.ChemiluminesoensePtotometryTemp;
        }

        /// <summary>
        /// 温度設定
        /// </summary>
        /// <remarks>
        /// 通信コマンドの温度データをDPRで保持している温度データに設定します。
        /// </remarks>
        /// <param name="dprTemp">DPR側温度データ</param>
        /// <param name="slaveTemp">通信コマンド温度データ</param>
        public static void SetDPRTemperatureFromSlaveTemperature(this Temperature dprTemp, TemperatureTable slaveTemp)
        {
            // 反応テーブル温度
            dprTemp.TempReactionTable = slaveTemp.ReactionTableTemp;
            // BFテーブル温度
            dprTemp.TempBFTable = slaveTemp.BFTableTemp;
            // B/F1プレヒート温度
            dprTemp.TempBF1PreHeat = slaveTemp.BF1PreHeatTemp;
            // B/F2プレヒート温度
            dprTemp.TempBF2PreHeat = slaveTemp.BF2PreHeatTemp;
            // R1プローブ温度
            dprTemp.TempR1ProbePreHeat = slaveTemp.R1ProbeTemp;
            // R2プローブ温度
            dprTemp.TempR2ProbePreHeat = slaveTemp.R2ProbeTemp;
            // 測光部温度
            dprTemp.TempChemiLightMeas = slaveTemp.ChemiluminesoensePtotometryTemp;
            // 試薬保冷庫温度
            dprTemp.TempReagentCoolingBox = slaveTemp.ReagentBoxTemp;
            // 室温
            dprTemp.TempRoom = slaveTemp.RoomTemp;
            // 装置温度
            dprTemp.TempDevice = slaveTemp.AnalyzerTemp;

        }
    }

    /// <summary>
    /// データ中継インターフェース向け拡張メソッドクラス
    /// </summary>
    public static class DataRelayIFExtention
    {
        /// <summary>
        /// ISampleReqAmountインターフェース設定処理
        /// </summary>
        /// <remarks>
        /// 設定データクラスから、コマンドクラスへのデータ受け渡し時に使用します。
        /// </remarks>
        /// <param name="dst">被設定対象</param>
        /// <param name="src">設定取得対象</param>
        public static void Set(this ISampleReqAmount dst, ISampleReqAmount src)
        {
            // テーブルA代入
            dst.TableA.HighOfDeadVolForCup = src.TableA.HighOfDeadVolForCup;
            dst.TableA.HighOfDeadVolForCupOnTube = src.TableA.HighOfDeadVolForCupOnTube;
            dst.TableA.HighOfDeadVolForTube = src.TableA.HighOfDeadVolForTube;
            dst.TableA.HighOfDeadVolForTubeWithRubber = src.TableA.HighOfDeadVolForTubeWithRubber;

            // テーブルB代入
            Array.Copy(src.TableB.ColA, dst.TableB.ColA, src.TableB.ColA.Length);
            Array.Copy(src.TableB.ColB, dst.TableB.ColB, src.TableB.ColB.Length);
            Array.Copy(src.TableB.ColC, dst.TableB.ColC, src.TableB.ColC.Length);
            Array.Copy(src.TableB.ColD, dst.TableB.ColD, src.TableB.ColD.Length);
            Array.Copy(src.TableB.ColE, dst.TableB.ColE, src.TableB.ColE.Length);
        }

    }

    /// <summary>
    /// ホスト(シリアル)通信設定向け拡張メソッドクラス
    /// </summary>
    /// <remarks>
    /// ホスト(シリアル)通信設定向け拡張処理を行います
    /// </remarks>
    public static class HostSerialSettingExtention
    {
        /// <summary>
        /// シリアルパラメータへの変換
        /// </summary>
        /// <remarks>
        ///　シリアルパラメータへの変換を行います。
        /// </remarks>
        /// <param name="hostParameter">変換元</param>
        /// <returns>変換後シリアルパラメータ</returns>
        public static SerialParameter ConvertSerialParameter(this HostParameter hostParameter)
        {
            SerialParameter serialParameter = new SerialParameter();
            serialParameter.BaudRate = hostParameter.Baudrate;
            serialParameter.DataBits = hostParameter.DataLength;
            serialParameter.Parity = hostParameter.Parity;

            switch (hostParameter.StopBit)
            {
                case StopBitKind.Bit1:
                    serialParameter.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case StopBitKind.Bit2:
                    serialParameter.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                default:
                    serialParameter.StopBits = System.IO.Ports.StopBits.One;
                    break;
            }
            serialParameter.CommPort = hostParameter.CommPort;

            serialParameter.WriteTimeout = CarisXConst.HOST_COMTIMEOUT;
            serialParameter.ReadTimeout = CarisXConst.HOST_COMTIMEOUT;
            return serialParameter;
        }

    }

    public static class HelpDocument
    {
        public static void openHelpDocumentPage()
        {
            String helpPage = String.Empty;
            String helpFileName = CarisXConst.PathHelp + @"\CarisXHelp.chm";
            if (System.IO.File.Exists(helpFileName))
            {
                CarisXHistoryActionKind currentActionKind = (CarisXHistoryActionKind)Singleton<HistoryManager>.Instance.getCurrentActionKind();
                if (currentActionKind != null)
                {
                    switch (currentActionKind.ActionKind)
                    {
                        case CarisXHistoryKind.ShowAssay:
                            helpPage = "Operation\\AnalysisInterface.jpg";
                            break;
                        case CarisXHistoryKind.ShowCalibAnalysis:
                            helpPage = "Operation\\Calibration\\CalibrationAnalysis.jpg";
                            break;
                        case CarisXHistoryKind.ShowCalibRegistration:
                            helpPage = "Operation\\Calibration\\CalibrationRegister.jpg";
                            break;
                        case CarisXHistoryKind.ShowCalibResult:
                            helpPage = "Operation\\Calibration\\CalibrationResult.jpg";
                            break;
                        case CarisXHistoryKind.ShowCalibStatus:
                            helpPage = "Operation\\Calibration\\CalibrationStatus.jpg";
                            break;
                        case CarisXHistoryKind.ShowControlQC:
                            helpPage = "Operation\\Control\\ControlManger.jpg";
                            break;
                        case CarisXHistoryKind.ShowControlRegistration:
                            helpPage = "Operation\\Control\\ControlRegister.jpg";
                            break;
                        case CarisXHistoryKind.ShowControlResult:
                            helpPage = "Operation\\Control\\ControlResult.jpg";
                            break;
                        case CarisXHistoryKind.ShowProtocolSetting:
                            helpPage = "";
                            break;
                        case CarisXHistoryKind.ShowSetReagent:
                            helpPage = "Operation\\ReagentPreparation.jpg";
                            break;
                        case CarisXHistoryKind.ShowSpecimenRegistration:
                            helpPage = "Operation\\Specimen\\SpecimenRegister.jpg";
                            break;
                        case CarisXHistoryKind.ShowSpecimenStatRegistration:
                            helpPage = "Operation\\Specimen\\SpecimenRegister.jpg";
                            break;
                        case CarisXHistoryKind.ShowSpecimenResult:
                            helpPage = "Operation\\Specimen\\SpecimenResult.jpg";
                            break;
                        case CarisXHistoryKind.ShowSpecimenRetest:
                            helpPage = "Operation\\Specimen\\SpecimenRetest.jpg";
                            break;
                        case CarisXHistoryKind.ShowSystemAnalytes:
                            helpPage = "Operation\\SystemAbout\\AnalyzeSettings\\AnalyzeSettings.jpg";
                            break;
                        case CarisXHistoryKind.ShowSystemConfigration:
                            helpPage = "Operation\\SystemAbout\\SystemSettings\\SystemSettings.jpg";
                            break;
                        case CarisXHistoryKind.ShowSystemLog:
                            helpPage = "Operation\\SystemAbout\\SystemLog\\SystemLog.jpg";
                            break;
                        case CarisXHistoryKind.ShowSystemOption:
                            helpPage = "Operation\\SystemAbout\\Option\\Option.jpg";
                            break;
                        case CarisXHistoryKind.ShowSystemModuleOption:
                            helpPage = "Operation\\SystemAbout\\Option\\Option.jpg";
                            break;
                        case CarisXHistoryKind.ShowSystemUserControl:
                            helpPage = "";
                            break;
                        default:
                            helpPage = "Operation\\MenuInterface.jpg";
                            break;
                    }

                }
                else
                    helpPage = "Operation\\MenuInterface.jpg";

                System.Windows.Forms.Help.ShowHelp(null, helpFileName, System.Windows.Forms.HelpNavigator.Topic, helpPage);
            }
            else
            {
                Oelco.CarisX.GUI.DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_070, "", CarisX.Properties.Resources.STRING_DLG_TITLE_001, Oelco.CarisX.GUI.MessageDialogButtons.Confirm);
            }

        }
    }


#endregion

#region MyRegion
    /// <summary>
    /// スモールメニュー種別
    /// </summary>
    public enum SmallMenuKind
    {
        Specimen,
        Assay,
        Reagent,
        Calibration,
        Control,
        Option,
        System,
    }
#endregion

#region メンテナンス機能用

    /// <summary>
    /// メンテナンス画面のナビバーの内容
    /// </summary>
    public enum MaintenanceMainNavi
    {
        /// <summary>
        /// ラック架設フレーム部、ラック搬送部、ラック引込部
        /// </summary>
        RackAllUnits,
        /// <summary>
        /// ケース搬送
        /// </summary>
        TipCellCaseTransferUnit,
        /// <summary>
        /// 試薬保冷庫
        /// </summary>
        ReagentStorageUnit,
        /// <summary>
        /// スタット部
        /// </summary>
        STATUnit,
        /// <summary>
        /// サンプル分注移送部
        /// </summary>
        SampleDispenseUnit,
        /// <summary>
        /// 反応容器搬送部
        /// </summary>
        ReactionCellTransferUnit,
        /// <summary>
        /// 反応テーブル部
        /// </summary>
        ReactionTableUnit,
        /// <summary>
        /// BFテーブル部
        /// </summary>
        BFTableUnit,
        /// <summary>
        /// トラベラー・廃棄部
        /// </summary>
        TravelerandDisposalUnit,
        /// <summary>
        /// 試薬分注1部
        /// </summary>
        ReagentDispense1Unit,
        /// <summary>
        /// 試薬分注2部
        /// </summary>
        ReagentDispense2Unit,
        /// <summary>
        /// BF1部、BF1廃液部
        /// </summary>
        BF1UnitandBF1WasteLiquidUnit,
        /// <summary>
        /// BF2部
        /// </summary>
        BF2Unit,
        /// <summary>
        /// 希釈液分注部
        /// </summary>
        DiluentDispenseUnit,
        /// <summary>
        /// プレトリガ
        /// </summary>
        PreTriggerDispenseUnit,
        /// <summary>
        /// トリガ分注、化学発光測定部
        /// </summary>
        TriggerDispensingUnitandChemiluminescenceMeasUnit,
        /// <summary>
        /// 流体配管部
        /// </summary>
        FluidandPipingUnit,
        /// <summary>
        /// ラックユニットセンサー部
        /// </summary>
        RackSensorUnit,
        /// <summary>
        /// スレーブユニットセンサー部
        /// </summary>
        SlaveSensorUnit,
        /// <summary>
        /// 温度測定部
        /// </summary>
        TempUnit,
        /// <summary>
        /// その他
        /// </summary>
        Other,
    }

    /// <summary>
    /// メンテナンス画面のツールバーの内容
    /// </summary>
    public enum ToolBarEnable
    {
        /// <summary>
        /// デフォルト
        /// </summary>
        Default = 0,
        /// <summary>
        /// スタート
        /// </summary>
        Start = 1,
        /// <summary>
        /// 一時停止
        /// </summary>
        Pause = 2,
        /// <summary>
        /// 再開
        /// </summary>
        Restart = 3,
        /// <summary>
        /// 終了
        /// </summary>
        Abort = 4,
        /// <summary>
        /// すべてDisable
        /// </summary>
        Disable = 5,
        /// <summary>
        /// 保存デフォルト
        /// </summary>
        DefaultSave = 6,
        /// <summary>
        /// センサー画面と温度取得画面のスタート
        /// </summary>
        SensorAndTempStart = 7,
        /// <summary>
        /// モーター調整位置画面のスタート
        /// </summary>
        Adjust = 8,
        /// <summary>
        /// モーター調整位置画面のプローブ交換スタート
        /// </summary>
        AdjustReplacingProbe = 9
    }

    /// <summary>
    /// メンテナンス画面のタブインデックス
    /// </summary>
    public enum ModuleTabIndex
    {
        /// <summary>
        /// ラック搬送
        /// </summary>
        RackTransfer,
        /// <summary>
        /// スレーブ１
        /// </summary>
        Slave1,
        /// <summary>
        /// スレーブ２
        /// </summary>
        Slave2,
        /// <summary>
        /// スレーブ３
        /// </summary>
        Slave3,
        /// <summary>
        /// スレーブ４
        /// </summary>
        Slave4
    }

    /// <summary>
    /// 各ユニットにあるタブのインデックス
    /// </summary>
    public enum MaintenanceTabIndex
    {
        /// <summary>
        /// Testタブ
        /// </summary>
        Test,
        /// <summary>
        /// Configurationタブ
        /// </summary>
        Config,
        /// <summary>
        /// Motor Parameterタブ
        /// </summary>
        MParam,
        /// <summary>
        /// Motor Adjustmentタブ
        /// </summary>
        MAdjust,
    }

    /// <summary>
    /// 各ユニットのモード（＝どのタブが選択されているか）
    /// </summary>
    public enum MaintenanceUnitMode
    {
        /// <summary>
        /// Testモード
        /// </summary>
        Test,
        /// <summary>
        /// Configurationモード
        /// </summary>
        Config,
        /// <summary>
        /// Motor Parameterモード
        /// </summary>
        MParam,
        /// <summary>
        /// Motor Adjustmentモード
        /// </summary>
        MAdjust,
        /// <summary>
        /// その他モード
        /// </summary>
        Other = 99,
    }

    /// <summary>
    /// ラック搬送Ｎｏ
    /// </summary>
    public enum RackTransferNo
    {
        /// <summary>
        /// ラック搬送１
        /// </summary>
        RackTransfer1
    }

    /// <summary>
    /// 温度制御画面のタブインデックス
    /// </summary>
    public enum TempUnitTabIndex
    {
        /// <summary>
        /// 温度１
        /// </summary>
        Temperature1,
        /// <summary>
        /// 温度２
        /// </summary>
        Temperature2,
        /// <summary>
        /// 温度３
        /// </summary>
        Temperature3,
        /// <summary>
        /// 温度４
        /// </summary>
        Temperature4
    }

    /// <summary>
    /// ユニットの機能番号定義
    /// </summary>         
    public class SequenceItem
    {
        /// <summary>
        /// 機能名
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// 機能番号
        /// </summary>
        public int No { get; set; }
    }

    /// <summary>
    /// ユニット番号一覧
    /// </summary>
    public enum UnitNoList
    {
        RackFrame = 31,
        RackTransfer = 3,
        RackPullin = 4,
        CaseTransfer = 5,
        ReagentStorage = 6,
        STAT = 7,
        SampleDispense = 8,
        ReactionCellTransfer = 9,
        ReactionTable = 10,
        BFTable = 11,
        TravelerDisposal = 13,
        ReagentDispense1 = 15,
        ReagentDispense2 = 16,
        BF1 = 17,
        BF2 = 19,
        DiluentDispense = 20,
        PreTrigger = 21,
        TriggerDispense = 14,
        FluidPiping = 23,
        Other = 1,
    }

    /// <summary>
    /// ラック搬送の機能一覧
    /// </summary>
    public enum RackTransferSequence
    {
        /// <summary>
        /// 原点取り
        /// </summary>
        Init = 1,
        /// <summary>
        /// フィーダー稼働
        /// </summary>
        FeederOperation = 2,
        /// <summary>
        /// ローダー稼働
        /// </summary>
        LoaderOperation = 3,
        /// <summary>
        /// ラック引き込みユニット稼働
        /// </summary>
        ForkOperation = 4,
        /// <summary>
        /// ラックロード(ラック設置→搬送フィーダーまで移動)
        /// </summary>
        RackSetting = 5,
        /// <summary>
        /// ラックID読込
        /// </summary>
        RackIDReading = 6,
        /// <summary>
        /// 検体ID読込
        /// </summary>
        SampleIDReading = 7,
        /// <summary>
        /// チューブタイプ
        /// </summary>
        SampleCupTubeCheck = 8,
        /// <summary>
        /// （送り）装置待機位置へ移動(BCR・待機位置→待機位置)
        /// </summary>
        SendMoveStandby = 9,
        /// <summary>
        /// （戻り）装置待機位置へ移動(BCR・待機位置→待機位置)
        /// </summary>
        BackMoveStandby = 10,
        /// <summary>
        /// （送り）装置取り込み位置へ移動(装置待機→装置)
        /// </summary>
        SendRackCatch = 11,
        /// <summary>
        /// （戻り）装置取り込み位置へ移動(装置待機→装置)
        /// </summary>
        BackRackCatch = 12,
        /// <summary>
        /// （送り）装置取り込み位置へ移動(装置→装置待機)
        /// </summary>
        SendRackRelease = 13,
        /// <summary>
        /// （戻り）装置取り込み位置へ移動(装置→装置待機)
        /// </summary>
        BackRackRelease = 14,
        /// <summary>
        /// ラックアンロード(ラック受け取り→回収フィーダーまで移動)
        /// </summary>
        RackUnload = 15,
        /// <summary>
        /// ラック再検搬送(回収フィーダー→ラック設置まで移動)
        /// </summary>
        RackRetest = 16,
        /// <summary>
        /// ラック回収搬送(回収フィーダー→ラック回収まで移動)
        /// </summary>
        RackTakeOut = 17,
        /// <summary>
        /// ラックアンロード(ラック搬送フィーダー)
        /// </summary>
        RackReturn = 18,
        /// <summary>
        /// ラックカバー LEDテスト
        /// </summary>
        RackCoverLED = 19,
        /// <summary>
        /// ラック総合テスト(ラックを2つ設置レーンに置いてください)
        /// </summary>
        AllRackOperation = 20,
    }

    /// <summary>
    /// ラック搬送のラジオボタン選択値一覧
    /// </summary>
    public enum RackTransferRadioValue
    {
        RotationStop = 0,
        RotationBackRight = 1,
        RotationFrontLeft = 2,
        ForkPositionBack = 1,
        ForkPositionFront = 2,
    }

    /// <summary>
    /// ラック搬送のドロップダウン選択値一覧
    /// </summary>
    public enum RackTransferCmbValue
    {
        FeederTypeLoad = 1,
        FeederTypeUnload = 2,
        FeederTypeSlider = 3,
        PositionLeft = 1,
        PositionMiddle = 2,
        PositionRight = 3,
        LoaderTypeLoadY = 10,             //ローダー種別はローダー種別と装置番号の組み合わせ
        LoaderTypeUnLoadY = 20,
        LoaderTypeTakeOutY = 30,
        LoaderTypeSendX1 = 41,
        LoaderTypeBackX1 = 51,
        LoaderTypeSendX2 = 42,
        LoaderTypeBackX2 = 52,
        LoaderTypeSendX3 = 43,
        LoaderTypeBackX3 = 53,
        LoaderTypeSendX4 = 44,
        LoaderTypeBackX4 = 54,
        PullinForkModule1 = 1,
        PullinForkModule2 = 2,
        PullinForkModule3 = 3,
        PullinForkModule4 = 4,
        PositionABCR = 0,
        PositionAModule1 = 1,
        PositionAModule2 = 2,
        PositionAModule3 = 3,
        PositionBModule1 = 1,
        PositionBModule2 = 2,
        PositionBModule3 = 3,
        PositionBModule4 = 4,
        Module1 = 1,
        Module2 = 2,
        Module3 = 3,
        Module4 = 4,
        ForkOperationPositionBack = 1,
        ForkOperationPosition2ndfromBack = 2,
        ForkOperationPosition2ndfromFront = 3,
        ForkOperationPositionFront = 4,
    }

    /// <summary>
    /// ケース搬送の機能一覧
    /// </summary>
    public enum CaseTransferSequence
    {
        Init = 1,
        AlltheCasesChecking = 2,
        CaseLoading = 3,
        CaseUnloading = 4,
        CaseDoorLockOperation = 5,
        MoveTiptoCatchPosition = 6,
        MoveCelltoCatchPosition = 7,
    }

    /// <summary>
    /// ケース搬送のラジオボタン選択値一覧
    /// </summary>
    public enum CaseTransferRadioValue
    {
        LockOFF = 0,
        LockON = 1,
    }

    /// <summary>
    /// 試薬保冷庫の機能一覧
    /// </summary>
    public enum ReagentStorageSequence
    {
        Init = 1,
        ReagentTableBottle = 2,
        ReagentTableR1R1 = 3,
        ReagentTableR2R1 = 4,
        ReagentTableMR1 = 5,
        ReagentTableR1R2 = 6,
        ReagentTableR2R2 = 7,
        ReagentTableMR2 = 8,
        RBottleCheck = 9,
        MBottlesCheck = 10,
        RBottleBCID = 11,
        MBottlesBCID = 12,
        Mixing = 13,
        LocksUnlocksCover = 14,
        TurnsReagentTable = 15,
        TurnsReagentCover = 16,
        RBottleCheckEX1 = 17,
        MBottlesCheckEX1 = 18,
        RBottleBCIDEX1 = 19,
        MBottlesBCIDEX1 = 20,
    }

    /// <summary>
    /// 試薬保冷庫のラジオボタン選択値一覧
    /// </summary>
    public enum ReagentStorageRadioValue
    {
        LockOFF = 0,
        LockON = 1,
        LockONtoOFF = 2,
    }

    /// <summary>
    /// サンプル分注移送部の機能一覧
    /// </summary>
    public enum SampleDispenseSequence
    {
        Init = 1,
        RackDispensing = 2,
        STATorExternalTransferDispensing = 3,
        ReactionTableDispensing = 4,
        RemovesSampleTip = 5,
        DetectionSensor = 6,
    }

    /// <summary>
    /// サンプル分注移送部のラジオボタン選択値一覧
    /// </summary>
    public enum SampleDispenseRadioValue
    {
        STAT = 0,
        ExternalTransport = 1,
        DilutionPosition = 2,
        PretreatmentPosition = 3,
        Back1 = 11,
        Back2 = 12,
        Back3 = 13,
        Back4 = 14,
        Back5 = 15,
        Front1 = 21,
        Front2 = 22,
        Front3 = 23,
        Front4 = 24,
        Front5 = 25,
    }

    /// <summary>
    /// サンプル分注移送部のコンボボックス選択値一覧
    /// </summary>
    public enum SampleDispenseCmbValue
    {
        /// <summary>
        /// チップ番号１
        /// </summary>
        TipNumber1 = 1,
        /// <summary>
        /// チップ番号６
        /// </summary>
        TipNumber6 = 6,
        /// <summary>
        /// チップ番号９１
        /// </summary>
        TipNumber91 = 91,
        /// <summary>
        /// チップ番号９６
        /// </summary>
        TipNumber96 = 96,
        /// <summary>
        /// サンプルポジション１
        /// </summary>
        SamplePosition1 = 1,
        /// <summary>
        /// サンプルポジション５
        /// </summary>
        SamplePosition5 = 5,
    }

    /// <summary>
    /// 反応容器搬送部の機能一覧
    /// </summary>
    public enum ReactionCellTransferSequence
    {
        Init = 1,
        CatchesfromStorage = 2,
        ReleasestoSettingPosition = 3,
        CatchesfromStorageandRelease = 4,
    }

    /// <summary>
    /// 反応テーブル部の機能一覧
    /// </summary>
    public enum ReactionTableSequence
    {
        Init = 1,
        Step = 2,
        Mixing = 3,
    }

    /// <summary>
    /// BFテーブル部の機能一覧
    /// </summary>
    public enum BFTableSequence
    {
        Init = 1,
        Step = 2,
        MixingR2 = 3,
        MixingBF1 = 4,
        MixingPTr = 5,
        MixingBF2 = 6,
    }

    /// <summary>
    /// トラベラー・廃棄部の機能一覧
    /// </summary>
    public enum TravelerDisposalSequence
    {
        Init = 1,
        DisposalReactionCell = 2,
        MoveBFTablefromReactionTable = 3,
        MoveReactionTablefromBFTable = 4,
        MoveBFTableOuterfromInner = 5,
    }

    /// <summary>
    /// トラベラー・廃棄部のラジオボタン選択値一覧
    /// </summary>
    public enum TravelerDisposalRadioValue
    {
        ReactionTableInner = 0,
        ReactionTableOuter = 1,
        BFTableInner = 2,
        BFTableOuter = 3,
        InnerOuter = 0,
        OuterInner = 1,
    }

    /// <summary>
    /// 試薬分注1部の機能一覧
    /// </summary>
    public enum R1DispenceSequence
    {
        Init = 1,
        R1DispenseandProbeWashing = 2,
        R2DispenseandProbeWashing = 3,
        MDispenseandProbeWashing = 4,
        Prime = 5,
        Rinse = 6,
        MovetoR1washing = 7,
        MovetoR1aspirating = 8,
        MovetoR1dispensing = 9,
        DetectionSensor = 10,
        NozzleWash = 11,
        ProbeReplacement = 12,
    }

    /// <summary>
    /// 試薬分注1部のラジオボタン選択値一覧
    /// </summary>
    public enum R1DispenceRadioValue
    {
        WashOFF = 0,
        WashON = 1,
    }

    /// <summary>
    /// 試薬分注2部の機能一覧
    /// </summary>
    public enum R2DispenceSequence
    {
        Init = 1,
        R2DispenseandProbeWashing = 2,
        MDispenseandProbeWashing = 3,
        Prime = 4,
        Rinse = 5,
        MovetoR2washing = 6,
        MovetoR2aspirating = 7,
        MovetoR2dispensing = 8,
        DetectionSensor = 9,
        NozzleWash = 10,
        ProbeReplacement = 11,
    }

    /// <summary>
    /// 試薬分注2部のラジオボタン選択値一覧
    /// </summary>
    public enum R2DispenceRadioValue
    {
        WashOFF = 0,
        WashON = 1,
    }

    /// <summary>
    /// BF1部の機能一覧
    /// </summary>
    public enum BF1Sequence
    {
        Init = 1,
        BF1 = 2,
        WasteLiquid = 3,
        Prime = 4,
        Rinse = 5,
        MovetoBF1Washing = 6,
        BF1Dispense = 7,
    }

    /// <summary>
    /// BF1部のラジオボタン選択値一覧
    /// </summary>
    public enum BF1RadioValue
    {
        AllNozzle = 0,
        Nozzle1 = 1,
        Nozzle2 = 2,
    }

    /// <summary>
    /// BF2部の機能一覧
    /// </summary>
    public enum BF2Sequence
    {
        Init = 1,
        BF2 = 2,
        WasteLiquid = 3,
        Prime = 4,
        Rinse = 5,
        MovetoBF2Washing = 6,
        BF2Dispense = 7,
    }

    /// <summary>
    /// BF1部のラジオボタン選択値一覧
    /// </summary>
    public enum BF2RadioValue
    {
        AllNozzle = 0,
        Nozzle1 = 1,
        Nozzle2 = 2,
        Nozzle3 = 3,
    }

    /// <summary>
    /// 希釈液分注部の機能一覧
    /// </summary>
    public enum DiluentDispenseSequence
    {
        Init = 1,
        DiluentDispense = 2,
        Prime = 3,
        Rinse = 4,
        MovetoDiluentDispense = 5,
    }

    /// <summary>
    /// プレトリガの機能一覧
    /// </summary>
    public enum PretriggerSequence
    {
        Init = 1,
        DispensePretrigger = 2,
        Prime = 3,
        Rinse = 4,
        PrimeExchangeBottle = 5,
        MovePretriggerDispense = 6,
    }

    /// <summary>
    /// プレトリガのラジオボタン選択値一覧
    /// </summary>
    public enum PretriggerRadioValue
    {
        Bottle1 = 1,
        Bottle2 = 2,
    }

    /// <summary>
    /// トリガ分注の機能一覧
    /// </summary>
    public enum TriggerDispenseSequence
    {
        Init = 1,
        TriggerDispense = 2,
        Prime = 3,
        Rinse = 4,
        PrimeExchangeBottle = 5,
        Measurement = 6,
        DetectorOutput = 7,
        MoveTriggerDispense = 8,
    }

    /// <summary>
    /// トリガ分注のラジオボタン選択値一覧
    /// </summary>
    public enum TriggerDispenseRadioValue
    {
        Bottle1 = 1,
        Bottle2 = 2,
        MeasDark = 0,
        MeasSample = 1,
        MeasBackground = 2,
        MeasLED = 3,
        DetectOFF = 0,
        DetectON = 1,
    }

    /// <summary>
    /// 流体配管部の機能一覧
    /// </summary>
    public enum FluidPipingSequence
    {
        WasteWaterPumpON = 1,
        WasteWaterPumpOFF = 2,
        WasteLiquidEnforcementEffluent = 3,
        WasteLiquidReactionCell = 4,
        WashSolutionTankPumpON = 5,
        WashSolutionTankPumpOFF = 6,
    }

    /// <summary>
    /// その他の機能一覧
    /// </summary>
    public enum OtherSequence
    {
        WarningBeep = 1,
        AllLEDTest = 2,
        ModuleConfig = 95,
        SoftWareTEST1 = 96,
        SoftWareTEST2 = 97,
        CPLDWrite = 98,
        CPLDRead = 99,
    }

    /// <summary>
    /// その他のラジオボタン選択値一覧
    /// </summary>
    public enum OtherRadioValue
    {
        BeepOFF = 0,
        BeepON = 1,
    }

    /// <summary>
    /// モーター調整の停止位置番号
    /// </summary>
    public enum MotorAdjustStopPosition
    {
        RackForkPosition = 1,
        RackForkPosition2,          //使用しない
        CaseCatch,
        MReagentIDReading,
        RReagentIDReading,
        SampleTipCatch,
        RackAspiration,
        STATAspiration,
        DiluentAspiration,
        PretreatAspiration,
        SampleDispense,
        SampleTipRemover,
        LineSampleAspiration,
        ReactionCellCatch,
        ReactionCellRelease,
        ReactionTableInitialization,
        BFTableInitialization,
        ReactionTableInside,
        ReactionTableOutside,
        ReactionCellRemover,
        BFTableOutside,
        BFTableInside,
        R1R1Aspiration,
        R1R2Aspiration,
        R1MAspiration,
        R1ReactionCell,
        R1Cuvette,
        R2R2Aspiration,
        R2MAspiration,
        R2ReactionCell,
        R2Cuvette,
        Wash1Prime,
        WasteWash1Prime,
        Wash2Prime,
        DiluentDispensePrime,
        TriggerPreTriggerDispensePrime,
        LoadFeederRackLoad,
        LoadFeederRackReturnFeeder,
        LoadFeederRackIDReading,
        LoadFeederSampleIDReading,
        LoadFeederTubeSensorReading,
        SliderRackUnLoad,
        SliderRackLoad,
        UnLoadFeederRackUnLoad,
        UnLoadFeederRackRetest,
        UnLoadFeederRackTakeout,
        R1PositioningProbe,
        R2PositioningProbe,
    }

    /// <summary>
    /// モーター調整の粗調整・微調整指定
    /// </summary>
    public enum MotorAdjustCoarseFine
    {
        Coarse,
        Fine,
    }

    /// <summary>
    /// サンプル分注部で指定するポート番号
    /// </summary>
    public enum SampleDispensePortNo
    {
        Back = 1,
        Front = 2,
    }

    /// <summary>
    /// モーター初期化完了状態種別
    /// </summary>
    [Flags]
    public enum MotorInitCompStatusKind : int
    {
        /// <summary>
        /// 初期化用
        /// </summary>
        Init = 0x0000,
        /// <summary>
        /// ラック搬送
        /// </summary>
        Rack = 0x0001,
        /// <summary>
        /// モジュール１
        /// </summary>
        Module1 = 0x0002,
        /// <summary>
        /// モジュール２
        /// </summary>
        Module2 = 0x0004,
        /// <summary>
        /// モジュール３
        /// </summary>
        Module3 = 0x0008,
        /// <summary>
        /// モジュール４
        /// </summary>
        Module4 = 0x0010,
        /// <summary>
        /// 正常終了
        /// </summary>
        Completed = 0x0020,
        /// <summary>
        /// タイムアウト
        /// </summary>
        TimeOut = 0x0040,
    }

    #endregion

    #region エラー関係
    /// <summary>
    /// エラーレベル
    /// </summary>
    public enum ErrorLevelKind
    {
        /// <summary>
        /// エラー
        /// </summary>
        Error = 1,
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
        /// <summary>
        /// 注意
        /// </summary>
        Hint
    }

    /// <summary>
    /// 回収エラー種別
    /// </summary>
    public enum ErrorCollectKind
    {
        /// <summary>
        /// エラーなし
        /// </summary>
        NoError = 0,
        /// <summary>
        /// 分析可能モジュールがない
        /// </summary>
        AssayPlanError = 1,
        /// <summary>
        /// 試薬情報なし
        /// </summary>
        ReagentError = 2,
        /// <summary>
        /// 登録情報なし
        /// </summary>
        RegisterError = 3,
        /// <summary>
        /// 容器情報なし
        /// </summary>
        ContainerError = 4
    }

    #endregion
}
