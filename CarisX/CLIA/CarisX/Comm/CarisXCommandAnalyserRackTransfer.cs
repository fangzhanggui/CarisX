using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Comm;
using Oelco.Common.Utility;
using Oelco.Common.Log;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Comm
{
    /// <summary>
    /// 通信コマンド解析クラス
    /// </summary>
    /// <remarks>
    /// テキストデータから、通信コマンドを解析し、取得します。
    /// </remarks>
    public class CarisXCommandAnalyserRackTransfer : ICommCommandAnalyser
    {
        #region [定数定義]

        /// <summary>
        /// コマンドID長
        /// </summary>
        private const Int32 COMMAND_ID_LENGTH = 4;
        
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析インターフェース実装
        /// </summary>
        /// <remarks>
        /// コマンド解析を行います。
        /// </remarks>
        /// <param name="target">解析対象文字列</param>
        /// <returns>コマンドデータ</returns>
        public CommCommand AnalyseCommand( String target )
        {
            return AnalyseCarisXCommand( target );
        }

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// CarisXに使用されるコマンドを、文字列から解析します。
        /// </remarks>
        /// <param name="target">解析対象文字列</param>
        /// <returns>コマンドデータ</returns>
        public CarisXCommCommand AnalyseCarisXCommand( String target )
        {
            // 解析対象外であれば終了。
            if ( ( target == String.Empty )
                || ( target == "" )
                || ( target.Length < COMMAND_ID_LENGTH ) )
            {
                return null;
            }

            CarisXCommCommand result = null;
            Type cmdType = null;

            // コマンド識別
            String identify = target.Substring( 0, COMMAND_ID_LENGTH );
            cmdType = commandIdentify( identify );

            // インスタンス生成
            if ( cmdType != null )
            {
                //Boolean successAnalyse = false;
                result = (CarisXCommCommand)Activator.CreateInstance( cmdType );
                // データ設定 コマンドクラス内部で解析されて値が保持される。（コマンドID部分は除いてSetCommandStringに渡す）
                if (target.Length > COMMAND_ID_LENGTH)
                {
                   result.SetCommandString(target.Substring(COMMAND_ID_LENGTH));
                }
                else
                {
                   result.SetCommandString(String.Empty);
                }

            }
            else
            {
                Int32 errArg = 0;
                string errDetail = identify;

                // 文字列→数値変換
                bool convSuccess = Int32.TryParse(identify, out errArg);
                if (convSuccess == true)
                {
                    // 成功している場合、エラー詳細は空白にする
                    errDetail = String.Empty;
                }

                //エラーの場合
                CarisXSubFunction.WriteDPRErrorHist(CarisXDPRErrorCode.CommandError, errArg, errDetail);
            }


            return result;
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// コマンドタイプ解析
        /// </summary>
        /// <remarks>
        /// コマンドIdから、コマンドのタイプを取得します。
        /// </remarks>
        /// <param name="commandId">コマンドId文字列</param>
        /// <returns>コマンドタイプ</returns>
        static protected Type commandIdentify( String commandIdIn )
        {
            Type identified = null;

            // ラック搬送からは空白文字で埋められるので空白を0に変換する。
            String commandId = commandIdIn.Trim().PadLeft( COMMAND_ID_LENGTH, '0' );

            switch ( int.Parse(commandId) )
            {
                // ソフトウェア識別コマンド
                case (int)CommandKind.RackTransferCommand0001:
                    identified = typeof(RackTransferCommCommand_0001);
                    break;
                case (int)CommandKind.RackTransferCommand1001:
                    identified = typeof(RackTransferCommCommand_1001);
                    break;

                // モーターパラメータ調整通知コマンド
                case (int)CommandKind.RackTransferCommand0002:
                    identified = typeof(RackTransferCommCommand_0002);
                    break;
                case (int)CommandKind.RackTransferCommand1002:
                    identified = typeof(RackTransferCommCommand_1002);
                    break;

                // シャットダウンコマンド
                case (int)CommandKind.RackTransferCommand0003:
                    identified = typeof(RackTransferCommCommand_0003);
                    break;
                case (int)CommandKind.RackTransferCommand1003:
                    identified = typeof(RackTransferCommCommand_1003);
                    break;

                // システムパラメータコマンド
                case (int)CommandKind.RackTransferCommand0004:
                    identified = typeof(RackTransferCommCommand_0004);
                    break;
                case (int)CommandKind.RackTransferCommand1004:
                    identified = typeof(RackTransferCommCommand_1004);
                    break;

                // モーター初期化コマンド
                case (int)CommandKind.RackTransferCommand0006:
                    identified = typeof(RackTransferCommCommand_0006);
                    break;
                case (int)CommandKind.RackTransferCommand1006:
                    identified = typeof(RackTransferCommCommand_1006);
                    break;

                // モーターセルフチェックコマンド
                case (int)CommandKind.RackTransferCommand0007:
                    identified = typeof(RackTransferCommCommand_0007);
                    break;
                case (int)CommandKind.RackTransferCommand1007:
                    identified = typeof(RackTransferCommCommand_1007);
                    break;

                // 分析開始コマンド
                case (int)CommandKind.RackTransferCommand0011:
                    identified = typeof(RackTransferCommCommand_0011);
                    break;
                case (int)CommandKind.RackTransferCommand1011:
                    identified = typeof(RackTransferCommCommand_1011);
                    break;

                // ポーズコマンド
                case (int)CommandKind.RackTransferCommand0012:
                    identified = typeof(RackTransferCommCommand_0012);
                    break;
                case (int)CommandKind.RackTransferCommand1012:
                    identified = typeof(RackTransferCommCommand_1012);
                    break;

                // カレンダー設定コマンド
                case (int)CommandKind.RackTransferCommand0013:
                    identified = typeof(RackTransferCommCommand_0013);
                    break;
                case (int)CommandKind.RackTransferCommand1013:
                    identified = typeof(RackTransferCommCommand_1013);
                    break;

                //  残量チェックコマンド
                case (int)CommandKind.RackTransferCommand0014:
                    identified = typeof(RackTransferCommCommand_0014);
                    break;
                case (int)CommandKind.RackTransferCommand1014:
                    identified = typeof(RackTransferCommCommand_1014);
                    break;

                // ユニットテストコマンド
                case (int)CommandKind.RackTransferCommand0039:
                    identified = typeof(RackTransferCommCommand_0039);
                    break;
                case (int)CommandKind.RackTransferCommand1039:
                    identified = typeof(RackTransferCommCommand_1039);
                    break;

                // センサーステータスコマンド
                case (int)CommandKind.RackTransferCommand0040:
                    identified = typeof(RackTransferCommCommand_0040);
                    break;
                case (int)CommandKind.RackTransferCommand1040:
                    identified = typeof(RackTransferCommCommand_1040);
                    break;

                // センサー無効コマンド
                case (int)CommandKind.RackTransferCommand0041:
                    identified = typeof(RackTransferCommCommand_0041);
                    break;
                case (int)CommandKind.RackTransferCommand1041:
                    identified = typeof(RackTransferCommCommand_1041);
                    break;

                // スタートアップ開始コマンド
                case (int)CommandKind.RackTransferCommand0042:
                    identified = typeof(RackTransferCommCommand_0042);
                    break;
                case (int)CommandKind.RackTransferCommand1042:
                    identified = typeof(RackTransferCommCommand_1042);
                    break;

                // スタートアップ終了コマンド
                case (int)CommandKind.RackTransferCommand0043:
                    identified = typeof(RackTransferCommCommand_0043);
                    break;
                case (int)CommandKind.RackTransferCommand1043:
                    identified = typeof(RackTransferCommCommand_1043);
                    break;

                // END処理コマンド
                case (int)CommandKind.RackTransferCommand0044:
                    identified = typeof(RackTransferCommCommand_0044);
                    break;
                case (int)CommandKind.RackTransferCommand1044:
                    identified = typeof(RackTransferCommCommand_1044);
                    break;

                // ラックユニットパラメータコマンド
                case (int)CommandKind.RackTransferCommand0047:
                    identified = typeof(RackTransferCommCommand_0047);
                    break;
                case (int)CommandKind.RackTransferCommand1047:
                    identified = typeof(RackTransferCommCommand_1047);
                    break;

                // ラック接続確認コマンド
                case (int)CommandKind.RackTransferCommand0067:
                    identified = typeof(RackTransferCommCommand_0067);
                    break;
                case (int)CommandKind.RackTransferCommand1067:
                    identified = typeof(RackTransferCommCommand_1067);
                    break;

                // サンプル停止要因問合せコマンド
                case (int)CommandKind.RackTransferCommand0068:
                    identified = typeof(RackTransferCommCommand_0068);
                    break;
                case (int)CommandKind.RackTransferCommand1068:
                    identified = typeof(RackTransferCommCommand_1068);
                    break;

                // ラック排出コマンド
                case (int)CommandKind.RackTransferCommand0069:
                    identified = typeof(RackTransferCommCommand_0069);
                    break;
                case (int)CommandKind.RackTransferCommand1069:
                    identified = typeof(RackTransferCommCommand_1069);
                    break;

                // モーターパラメータ保存コマンド
                case (int)CommandKind.RackTransferCommand0071:
                    identified = typeof(RackTransferCommCommand_0071);
                    break;
                case (int)CommandKind.RackTransferCommand1071:
                    identified = typeof(RackTransferCommCommand_1071);
                    break;

                // モーター調整コマンド
                case (int)CommandKind.RackTransferCommand0073:
                    identified = typeof(RackTransferCommCommand_0073);
                    break;
                case (int)CommandKind.RackTransferCommand1073:
                    identified = typeof(RackTransferCommCommand_1073);
                    break;

                // カレンダーコマンド
                case (int)CommandKind.RackTransferCommand0077:
                    identified = typeof(RackTransferCommCommand_0077);
                    break;
                case (int)CommandKind.RackTransferCommand1077:
                    identified = typeof(RackTransferCommCommand_1077);
                    break;

                // ユニット無効コマンド
                case (int)CommandKind.RackTransferCommand0078:
                    identified = typeof(RackTransferCommCommand_0078);
                    break;
                case (int)CommandKind.RackTransferCommand1078:
                    identified = typeof(RackTransferCommCommand_1078);
                    break;

                // 調整停止コマンド
                case (int)CommandKind.RackTransferCommand0080:
                    identified = typeof(RackTransferCommCommand_0080);
                    break;
                case (int)CommandKind.RackTransferCommand1080:
                    identified = typeof(RackTransferCommCommand_1080);
                    break;

                // 調整再開コマンド
                case (int)CommandKind.RackTransferCommand0081:
                    identified = typeof(RackTransferCommCommand_0081);
                    break;
                case (int)CommandKind.RackTransferCommand1081:
                    identified = typeof(RackTransferCommCommand_1081);
                    break;

                // 検体バーコード設定コマンド
                case (int)CommandKind.RackTransferCommand0082:
                    identified = typeof(RackTransferCommCommand_0082);
                    break;
                case (int)CommandKind.RackTransferCommand1082:
                    identified = typeof(RackTransferCommCommand_1082);
                    break;

                // 分析強制終了コマンド
                case (int)CommandKind.RackTransferCommand0086:
                    identified = typeof(RackTransferCommCommand_0086);
                    break;
                case (int)CommandKind.RackTransferCommand1086:
                    identified = typeof(RackTransferCommCommand_1086);
                    break;

                // ラック設置有無確認コマンド
                case (int)CommandKind.RackTransferCommand0088:
                    identified = typeof(RackTransferCommCommand_0088);
                    break;
                case (int)CommandKind.RackTransferCommand1088:
                    identified = typeof(RackTransferCommCommand_1088);
                    break;

                // ラック状況上書きコマンド
                case (int)CommandKind.RackTransferCommand0089:
                    identified = typeof(RackTransferCommCommand_0089);
                    break;
                case (int)CommandKind.RackTransferCommand1089:
                    identified = typeof(RackTransferCommCommand_1089);
                    break;

                // 再検コマンド
                case (int)CommandKind.RackTransferCommand0090:
                    identified = typeof(RackTransferCommCommand_0090);
                    break;
                case (int)CommandKind.RackTransferCommand1090:
                    identified = typeof(RackTransferCommCommand_1090);
                    break;

                // 分取完了通知コマンド
                case (int)CommandKind.RackTransferCommand0096:
                    identified = typeof(RackTransferCommCommand_0096);
                    break;
                case (int)CommandKind.RackTransferCommand1096:
                    identified = typeof(RackTransferCommCommand_1096);
                    break;

                // 測定完了通知コマンド
                case (int)CommandKind.RackTransferCommand0097:
                    identified = typeof(RackTransferCommCommand_0097);
                    break;
                case (int)CommandKind.RackTransferCommand1097:
                    identified = typeof(RackTransferCommCommand_1097);
                    break;

                // ステータス問合せコマンド
                case (int)CommandKind.RackTransferCommand0098:
                    identified = typeof(RackTransferCommCommand_0098);
                    break;
                case (int)CommandKind.RackTransferCommand1098:
                    identified = typeof(RackTransferCommCommand_1098);
                    break;



                // ラックレディーコマンド
                case (int)CommandKind.RackTransferCommand0101:
                    identified = typeof(RackTransferCommCommand_0101);
                    break;

                // エラー通知コマンド
                case (int)CommandKind.RackTransferCommand0104:
                    identified = typeof(RackTransferCommCommand_0104);
                    break;
                case (int)CommandKind.RackTransferCommand1104:
                    identified = typeof(RackTransferCommCommand_1104);
                    break;

                // ラックイベント通知コマンド
                case (int)CommandKind.RackTransferCommand0105:
                    identified = typeof(RackTransferCommCommand_0105);
                    break;
                case (int)CommandKind.RackTransferCommand1105:
                    identified = typeof(RackTransferCommCommand_1105);
                    break;

                // ラック分析ステータスコマンド
                case (int)CommandKind.RackTransferCommand0106:
                    identified = typeof(RackTransferCommCommand_0106);
                    break;
                case (int)CommandKind.RackTransferCommand1106:
                    identified = typeof(RackTransferCommCommand_1106);
                    break;

                // 残量コマンド
                case (int)CommandKind.RackTransferCommand0108:
                    identified = typeof(RackTransferCommCommand_0108);
                    break;
                case (int)CommandKind.RackTransferCommand1108:
                    identified = typeof(RackTransferCommCommand_1108);
                    break;

                // モーターパラメータ通知コマンド
                case (int)CommandKind.RackTransferCommand0109:
                    identified = typeof(RackTransferCommCommand_0109);
                    break;
                case (int)CommandKind.RackTransferCommand1109:
                    identified = typeof(RackTransferCommCommand_1109);
                    break;

                // バージョン通知コマンド
                case (int)CommandKind.RackTransferCommand0111:
                    identified = typeof(RackTransferCommCommand_0111);
                    break;
                case (int)CommandKind.RackTransferCommand1111:
                    identified = typeof(RackTransferCommCommand_1111);
                    break;

                // ラック情報通知コマンド
                case (int)CommandKind.RackTransferCommand0117:
                    identified = typeof(RackTransferCommCommand_0117);
                    break;
                case (int)CommandKind.RackTransferCommand1117:
                    identified = typeof(RackTransferCommCommand_1117);
                    break;

                // ラック状態通知コマンド
                case (int)CommandKind.RackTransferCommand0118:
                    identified = typeof(RackTransferCommCommand_0118);
                    break;
                case (int)CommandKind.RackTransferCommand1118:
                    identified = typeof(RackTransferCommCommand_1118);
                    break;

                // ラック移動位置問合せ（装置待機位置）コマンド
                case (int)CommandKind.RackTransferCommand0119:
                    identified = typeof(RackTransferCommCommand_0119);
                    break;
                case (int)CommandKind.RackTransferCommand1119:
                    identified = typeof(RackTransferCommCommand_1119);
                    break;

                // ラック移動位置問合せ（BCR）コマンド
                case (int)CommandKind.RackTransferCommand0120:
                    identified = typeof(RackTransferCommCommand_0120);
                    break;
                case (int)CommandKind.RackTransferCommand1120:
                    identified = typeof(RackTransferCommCommand_1120);
                    break;

                default:
                    identified = null;
                    break;
            }

            return identified;
        }
        #endregion

    }
}
