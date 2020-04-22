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
    public class CarisXCommandAnalyserSlave : ICommCommandAnalyser
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

            // スレーブからは空白文字で埋められるので空白を0に変換する。
            String commandId = commandIdIn.Trim().PadLeft( COMMAND_ID_LENGTH, '0' );

            switch (int.Parse(commandId))
            {
                // ソフトウェア識別コマンド
                case (int)CommandKind.Command0401:
                    identified = typeof(SlaveCommCommand_0401);
                    break;
                case (int)CommandKind.Command1401:
                    identified = typeof(SlaveCommCommand_1401);
                    break;

                // モーターパラメータ調整通知コマンド
                case (int)CommandKind.Command0402:
                    identified = typeof(SlaveCommCommand_0402);
                    break;
                case (int)CommandKind.Command1402:
                    identified = typeof(SlaveCommCommand_1402);
                    break;

                // シャットダウンコマンド
                case (int)CommandKind.Command0403:
                    identified = typeof(SlaveCommCommand_0403);
                    break;
                case (int)CommandKind.Command1403:
                    identified = typeof(SlaveCommCommand_1403);
                    break;

                // システムパラメータコマンド
                case (int)CommandKind.Command0404:
                    identified = typeof(SlaveCommCommand_0404);
                    break;
                case (int)CommandKind.Command1404:
                    identified = typeof(SlaveCommCommand_1404);
                    break;

                // プロトコルパラメータコマンド
                case (int)CommandKind.Command0405:
                    identified = typeof(SlaveCommCommand_0405);
                    break;
                case (int)CommandKind.Command1405:
                    identified = typeof(SlaveCommCommand_1405);
                    break;

                // モーター初期化コマンド
                case (int)CommandKind.Command0406:
                    identified = typeof(SlaveCommCommand_0406);
                    break;
                case (int)CommandKind.Command1406:
                    identified = typeof(SlaveCommCommand_1406);
                    break;

                // モーターセルフチェックコマンド
                case (int)CommandKind.Command0407:
                    identified = typeof(SlaveCommCommand_0407);
                    break;
                case (int)CommandKind.Command1407:
                    identified = typeof(SlaveCommCommand_1407);
                    break;

                // 光学系セルフチェックコマンド
                case (int)CommandKind.Command0408:
                    identified = typeof(SlaveCommCommand_0408);
                    break;
                case (int)CommandKind.Command1408:
                    identified = typeof(SlaveCommCommand_1408);
                    break;

                // プライムコマンド
                case (int)CommandKind.Command0409:
                    identified = typeof(SlaveCommCommand_0409);
                    break;
                case (int)CommandKind.Command1409:
                    identified = typeof(SlaveCommCommand_1409);
                    break;

                // リンス処理コマンド
                case (int)CommandKind.Command0410:
                    identified = typeof(SlaveCommCommand_0410);
                    break;
                case (int)CommandKind.Command1410:
                    identified = typeof(SlaveCommCommand_1410);
                    break;

                // 分析開始コマンド
                case (int)CommandKind.Command0411:
                    identified = typeof(SlaveCommCommand_0411);
                    break;
                case (int)CommandKind.Command1411:
                    identified = typeof(SlaveCommCommand_1411);
                    break;

                // ポーズコマンド
                case (int)CommandKind.Command0412:
                    identified = typeof(SlaveCommCommand_0412);
                    break;
                case (int)CommandKind.Command1412:
                    identified = typeof(SlaveCommCommand_1412);
                    break;

                // カレンダー設定コマンド
                case (int)CommandKind.Command0413:
                    identified = typeof(SlaveCommCommand_0413);
                    break;
                case (int)CommandKind.Command1413:
                    identified = typeof(SlaveCommCommand_1413);
                    break;

                // 残量チェックコマンド
                case (int)CommandKind.Command0414:
                    identified = typeof(SlaveCommCommand_0414);
                    break;
                case (int)CommandKind.Command1414:
                    identified = typeof(SlaveCommCommand_1414);
                    break;

                // 試薬準備確認コマンド
                case (int)CommandKind.Command0415:
                    identified = typeof(SlaveCommCommand_0415);
                    break;
                case (int)CommandKind.Command1415:
                    identified = typeof(SlaveCommCommand_1415);
                    break;

                // 試薬準備開始コマンド
                case (int)CommandKind.Command0416:
                    identified = typeof(SlaveCommCommand_0416);
                    break;
                case (int)CommandKind.Command1416:
                    identified = typeof(SlaveCommCommand_1416);
                    break;

                // 試薬準備完了コマンド
                case (int)CommandKind.Command0417:
                    identified = typeof(SlaveCommCommand_0417);
                    break;
                case (int)CommandKind.Command1417:
                    identified = typeof(SlaveCommCommand_1417);
                    break;

                // 希釈液準備確認コマンド
                case (int)CommandKind.Command0418:
                    identified = typeof(SlaveCommCommand_0418);
                    break;
                case (int)CommandKind.Command1418:
                    identified = typeof(SlaveCommCommand_1418);
                    break;

                // 希釈液準備開始コマンド
                case (int)CommandKind.Command0419:
                    identified = typeof(SlaveCommCommand_0419);
                    break;
                case (int)CommandKind.Command1419:
                    identified = typeof(SlaveCommCommand_1419);
                    break;

                // 希釈液準備完了コマンド
                case (int)CommandKind.Command0420:
                    identified = typeof(SlaveCommCommand_0420);
                    break;
                case (int)CommandKind.Command1420:
                    identified = typeof(SlaveCommCommand_1420);
                    break;

                // プレトリガ準備開始コマンド
                case (int)CommandKind.Command0421:
                    identified = typeof(SlaveCommCommand_0421);
                    break;
                case (int)CommandKind.Command1421:
                    identified = typeof(SlaveCommCommand_1421);
                    break;

                // プレトリガ準備完了コマンド
                case (int)CommandKind.Command0422:
                    identified = typeof(SlaveCommCommand_0422);
                    break;
                case (int)CommandKind.Command1422:
                    identified = typeof(SlaveCommCommand_1422);
                    break;

                // トリガ準備開始コマンド
                case (int)CommandKind.Command0423:
                    identified = typeof(SlaveCommCommand_0423);
                    break;
                case (int)CommandKind.Command1423:
                    identified = typeof(SlaveCommCommand_1423);
                    break;

                // トリガ準備完了コマンド
                case (int)CommandKind.Command0424:
                    identified = typeof(SlaveCommCommand_0424);
                    break;
                case (int)CommandKind.Command1424:
                    identified = typeof(SlaveCommCommand_1424);
                    break;

                // ケース(反応容器・サンプル分注チップ)準備開始コマンド
                case (int)CommandKind.Command0425:
                    identified = typeof(SlaveCommCommand_0425);
                    break;
                case (int)CommandKind.Command1425:
                    identified = typeof(SlaveCommCommand_1425);
                    break;

                // ケース(反応容器・サンプル分注チップ)準備完了コマンド
                case (int)CommandKind.Command0426:
                    identified = typeof(SlaveCommCommand_0426);
                    break;
                case (int)CommandKind.Command1426:
                    identified = typeof(SlaveCommCommand_1426);
                    break;

                // 試薬残量変更確認コマンド
                case (int)CommandKind.Command0427:
                    identified = typeof(SlaveCommCommand_0427);
                    break;
                case (int)CommandKind.Command1427:
                    identified = typeof(SlaveCommCommand_1427);
                    break;

                // 試薬残量変更終了コマンド
                case (int)CommandKind.Command0428:
                    identified = typeof(SlaveCommCommand_0428);
                    break;
                case (int)CommandKind.Command1428:
                    identified = typeof(SlaveCommCommand_1428);
                    break;

                // 試薬残量変更コマンド
                case (int)CommandKind.Command0429:
                    identified = typeof(SlaveCommCommand_0429);
                    break;
                case (int)CommandKind.Command1429:
                    identified = typeof(SlaveCommCommand_1429);
                    break;

                // 希釈液残量変更コマンド
                case (int)CommandKind.Command0430:
                    identified = typeof(SlaveCommCommand_0430);
                    break;
                case (int)CommandKind.Command1430:
                    identified = typeof(SlaveCommCommand_1430);
                    break;

                // プレトリガ残量変更コマンド
                case (int)CommandKind.Command0431:
                    identified = typeof(SlaveCommCommand_0431);
                    break;
                case (int)CommandKind.Command1431:
                    identified = typeof(SlaveCommCommand_1431);
                    break;

                // トリガ残量変更コマンド
                case (int)CommandKind.Command0432:
                    identified = typeof(SlaveCommCommand_0432);
                    break;
                case (int)CommandKind.Command1432:
                    identified = typeof(SlaveCommCommand_1432);
                    break;

                // ケース(反応容器・サンプル分注チップ)残量変更コマンド
                case (int)CommandKind.Command0433:
                    identified = typeof(SlaveCommCommand_0433);
                    break;
                case (int)CommandKind.Command1433:
                    identified = typeof(SlaveCommCommand_1433);
                    break;

                // 残量セットコマンド
                case (int)CommandKind.Command0434:
                    identified = typeof(SlaveCommCommand_0434);
                    break;
                case (int)CommandKind.Command1434:
                    identified = typeof(SlaveCommCommand_1434);
                    break;

                // 廃液ボトルセット開始コマンド
                case (int)CommandKind.Command0435:
                    identified = typeof(SlaveCommCommand_0435);
                    break;
                case (int)CommandKind.Command1435:
                    identified = typeof(SlaveCommCommand_1435);
                    break;

                // 廃液ボトルセット完了コマンド
                case (int)CommandKind.Command0436:
                    identified = typeof(SlaveCommCommand_0436);
                    break;
                case (int)CommandKind.Command1436:
                    identified = typeof(SlaveCommCommand_1436);
                    break;

                // インキュベータ温度設定問い合わせコマンド
                case (int)CommandKind.Command0437:
                    identified = typeof(SlaveCommCommand_0437);
                    break;
                case (int)CommandKind.Command1437:
                    identified = typeof(SlaveCommCommand_1437);
                    break;

                // 試薬保冷庫温度設定問い合わせコマンド
                case (int)CommandKind.Command0438:
                    identified = typeof(SlaveCommCommand_0438);
                    break;
                case (int)CommandKind.Command1438:
                    identified = typeof(SlaveCommCommand_1438);
                    break;

                // ユニットテストコマンド
                case (int)CommandKind.Command0439:
                    identified = typeof(SlaveCommCommand_0439);
                    break;
                case (int)CommandKind.Command1439:
                    identified = typeof(SlaveCommCommand_1439);
                    break;

                // センサーステータス問合せコマンド
                case (int)CommandKind.Command0440:
                    identified = typeof(SlaveCommCommand_0440);
                    break;
                case (int)CommandKind.Command1440:
                    identified = typeof(SlaveCommCommand_1440);
                    break;

                // センサー無効コマンド
                case (int)CommandKind.Command0441:
                    identified = typeof(SlaveCommCommand_0441);
                    break;
                case (int)CommandKind.Command1441:
                    identified = typeof(SlaveCommCommand_1441);
                    break;

                // スタートアップ開始コマンド
                case (int)CommandKind.Command0442:
                    identified = typeof(SlaveCommCommand_0442);
                    break;
                case (int)CommandKind.Command1442:
                    identified = typeof(SlaveCommCommand_1442);
                    break;

                // スタートアップ終了コマンド
                case (int)CommandKind.Command0443:
                    identified = typeof(SlaveCommCommand_0443);
                    break;
                case (int)CommandKind.Command1443:
                    identified = typeof(SlaveCommCommand_1443);
                    break;

                // 寿命部品使用回数設定・問合せコマンド
                case (int)CommandKind.Command0444:
                    identified = typeof(SlaveCommCommand_0444);
                    break;
                case (int)CommandKind.Command1444:
                    identified = typeof(SlaveCommCommand_1444);
                    break;

                // プレトリガアクトコマンド
                case (int)CommandKind.Command0445:
                    identified = typeof(SlaveCommCommand_0445);
                    break;
                case (int)CommandKind.Command1445:
                    identified = typeof(SlaveCommCommand_1445);
                    break;

                // トリガアクトコマンド
                case (int)CommandKind.Command0446:
                    identified = typeof(SlaveCommCommand_0446);
                    break;
                case (int)CommandKind.Command1446:
                    identified = typeof(SlaveCommCommand_1446);
                    break;

                // 廃液タンク状態(ラック情報)通知コマンド
                case (int)CommandKind.Command0447:
                    identified = typeof( SlaveCommCommand_0447 );
                    break;
                case (int)CommandKind.Command1447:
                    identified = typeof( SlaveCommCommand_1447 );
                    break;

                // ケース搬送ユニットパラメータ
                case (int)CommandKind.Command0448:
                    identified = typeof( SlaveCommCommand_0448 );
                    break;
                case (int)CommandKind.Command1448:
                    identified = typeof( SlaveCommCommand_1448 );
                    break;

                // 試薬保冷庫ユニットパラメータコマンド
                case (int)CommandKind.Command0449:
                    identified = typeof(SlaveCommCommand_0449);
                    break;
                case (int)CommandKind.Command1449:
                    identified = typeof(SlaveCommCommand_1449);
                    break;

                // スタットユニットパラメータ
                case (int)CommandKind.Command0450:
                    identified = typeof(SlaveCommCommand_0450);
                    break;
                case (int)CommandKind.Command1450:
                    identified = typeof(SlaveCommCommand_1450);
                    break;

                // サンプル分注ユニットパラメータコマンド
                case (int)CommandKind.Command0451:
                    identified = typeof(SlaveCommCommand_0451);
                    break;
                case (int)CommandKind.Command1451:
                    identified = typeof(SlaveCommCommand_1451);
                    break;

                // 反応容器搬送ユニットパラメータコマンド
                case (int)CommandKind.Command0452:
                    identified = typeof(SlaveCommCommand_0452);
                    break;
                case (int)CommandKind.Command1452:
                    identified = typeof(SlaveCommCommand_1452);
                    break;

                // 反応テーブルユニットパラメータコマンド
                case (int)CommandKind.Command0453:
                    identified = typeof(SlaveCommCommand_0453);
                    break;
                case (int)CommandKind.Command1453:
                    identified = typeof(SlaveCommCommand_1453);
                    break;

                // BFテーブルユニットパラメータコマンド
                case (int)CommandKind.Command0454:
                    identified = typeof(SlaveCommCommand_0454);
                    break;
                case (int)CommandKind.Command1454:
                    identified = typeof(SlaveCommCommand_1454);
                    break;

                // トラベラーユニットパラメータコマンド
                case (int)CommandKind.Command0455:
                    identified = typeof(SlaveCommCommand_0455);
                    break;
                case (int)CommandKind.Command1455:
                    identified = typeof(SlaveCommCommand_1455);
                    break;

                // 試薬分注1部ユニットパラメータコマンド
                case (int)CommandKind.Command0456:
                    identified = typeof(SlaveCommCommand_0456);
                    break;
                case (int)CommandKind.Command1456:
                    identified = typeof(SlaveCommCommand_1456);
                    break;

                // 試薬分注2部ユニットパラメータコマンド
                case (int)CommandKind.Command0457:
                    identified = typeof(SlaveCommCommand_0457);
                    break;
                case (int)CommandKind.Command1457:
                    identified = typeof(SlaveCommCommand_1457);
                    break;

                // BF1ユニットパラメータコマンド
                case (int)CommandKind.Command0458:
                    identified = typeof(SlaveCommCommand_0458);
                    break;
                case (int)CommandKind.Command1458:
                    identified = typeof(SlaveCommCommand_1458);
                    break;

                // BF2ユニットパラメータコマンド
                case (int)CommandKind.Command0459:
                    identified = typeof(SlaveCommCommand_0459);
                    break;
                case (int)CommandKind.Command1459:
                    identified = typeof(SlaveCommCommand_1459);
                    break;

                // 希釈分注ユニットパラメータコマンド
                case (int)CommandKind.Command0460:
                    identified = typeof(SlaveCommCommand_0460);
                    break;
                case (int)CommandKind.Command1460:
                    identified = typeof(SlaveCommCommand_1460);
                    break;

                // プレトリガユニットパラメータコマンド
                case (int)CommandKind.Command0461:
                    identified = typeof(SlaveCommCommand_0461);
                    break;
                case (int)CommandKind.Command1461:
                    identified = typeof(SlaveCommCommand_1461);
                    break;

                // トリガ分注測光ユニットパラメータコマンド
                case (int)CommandKind.Command0462:
                    identified = typeof(SlaveCommCommand_0462);
                    break;
                case (int)CommandKind.Command1462:
                    identified = typeof(SlaveCommCommand_1462);
                    break;

                // 流体配管ユニットパラメータコマンド
                case (int)CommandKind.Command0463:
                    identified = typeof(SlaveCommCommand_0463);
                    break;
                case (int)CommandKind.Command1463:
                    identified = typeof(SlaveCommCommand_1463);
                    break;

                // 警告灯制御コマンド
                case (int)CommandKind.Command0464:
                    identified = typeof(SlaveCommCommand_0464);
                    break;
                case (int)CommandKind.Command1464:
                    identified = typeof(SlaveCommCommand_1464);
                    break;

                // ブザー制御コマンド
                case (int)CommandKind.Command0465:
                    identified = typeof(SlaveCommCommand_0465);
                    break;
                case (int)CommandKind.Command1465:
                    identified = typeof(SlaveCommCommand_1465);
                    break;

                // 簡易プライムコマンド
                case (int)CommandKind.Command0466:
                    identified = typeof(SlaveCommCommand_0466);
                    break;
                case (int)CommandKind.Command1466:
                    identified = typeof(SlaveCommCommand_1466);
                    break;

                // スレーブ接続確認コマンド
                case (int)CommandKind.Command0467:
                    identified = typeof(SlaveCommCommand_0467);
                    break;
                case (int)CommandKind.Command1467:
                    identified = typeof(SlaveCommCommand_1467);
                    break;

                // サンプル停止要因問合せコマンド
                case (int)CommandKind.Command0468:
                    identified = typeof(SlaveCommCommand_0468);
                    break;
                case (int)CommandKind.Command1468:
                    identified = typeof(SlaveCommCommand_1468);
                    break;

                // ラック排出コマンド
                case (int)CommandKind.Command0469:
                    identified = typeof(SlaveCommCommand_0469);
                    break;
                case (int)CommandKind.Command1469:
                    identified = typeof(SlaveCommCommand_1469);
                    break;

                // モーターパラメータ保存コマンド
                case (int)CommandKind.Command0471:
                    identified = typeof(SlaveCommCommand_0471);
                    break;
                case (int)CommandKind.Command1471:
                    identified = typeof(SlaveCommCommand_1471);
                    break;

                // PID制御開始コマンド
                case (int)CommandKind.Command0472:
                    identified = typeof(SlaveCommCommand_0472);
                    break;
                case (int)CommandKind.Command1472:
                    identified = typeof(SlaveCommCommand_1472);
                    break;

                // モーター調整コマンド
                case (int)CommandKind.Command0473:
                    identified = typeof(SlaveCommCommand_0473);
                    break;
                case (int)CommandKind.Command1473:
                    identified = typeof(SlaveCommCommand_1473);
                    break;

                // PID定数設定コマンド
                case (int)CommandKind.Command0474:
                    identified = typeof(SlaveCommCommand_0474);
                    break;
                case (int)CommandKind.Command1474:
                    identified = typeof(SlaveCommCommand_1474);
                    break;

                // 残量クリアコマンド
                case (int)CommandKind.Command0475:
                    identified = typeof(SlaveCommCommand_0475);
                    break;
                case (int)CommandKind.Command1475:
                    identified = typeof(SlaveCommCommand_1475);
                    break;

                // 準備中断コマンド
                case (int)CommandKind.Command0476:
                    identified = typeof(SlaveCommCommand_0476);
                    break;
                case (int)CommandKind.Command1476:
                    identified = typeof(SlaveCommCommand_1476);
                    break;

                // カレンダーコマンド
                case (int)CommandKind.Command0477:
                    identified = typeof(SlaveCommCommand_0477);
                    break;
                case (int)CommandKind.Command1477:
                    identified = typeof(SlaveCommCommand_1477);
                    break;

                // ユニット無効コマンド
                case (int)CommandKind.Command0478:
                    identified = typeof(SlaveCommCommand_0478);
                    break;
                case (int)CommandKind.Command1478:
                    identified = typeof(SlaveCommCommand_1478);
                    break;

                // 試薬ロットサンプル停止要因解除コマンド
                case (int)CommandKind.Command0479:
                    identified = typeof(SlaveCommCommand_0479);
                    break;
                case (int)CommandKind.Command1479:
                    identified = typeof(SlaveCommCommand_1479);
                    break;

                // 調整位置停止コマンド
                case (int)CommandKind.Command0480:
                    identified = typeof(SlaveCommCommand_0480);
                    break;
                case (int)CommandKind.Command1480:
                    identified = typeof(SlaveCommCommand_1480);
                    break;

                // 調整位置再開コマンド
                case (int)CommandKind.Command0481:
                    identified = typeof(SlaveCommCommand_0481);
                    break;
                case (int)CommandKind.Command1481:
                    identified = typeof(SlaveCommCommand_1481);
                    break;

                // シリンジエージングコマンド
                case (int)CommandKind.Command0483:
                    identified = typeof(SlaveCommCommand_0483);
                    break;
                case (int)CommandKind.Command1483:
                    identified = typeof(SlaveCommCommand_1483);
                    break;

                // 総アッセイ数設定コマンド
                case (int)CommandKind.Command0484:
                    identified = typeof(SlaveCommCommand_0484);
                    break;
                case (int)CommandKind.Command1484:
                    identified = typeof(SlaveCommCommand_1484);
                    break;

                // サンプル必要量設定コマンド
                case (int)CommandKind.Command0485:
                    identified = typeof(SlaveCommCommand_0485);
                    break;
                case (int)CommandKind.Command1485:
                    identified = typeof(SlaveCommCommand_1485);
                    break;

                // 分析強制終了コマンド
                case (int)CommandKind.Command0486:
                    identified = typeof(SlaveCommCommand_0486);
                    break;
                case (int)CommandKind.Command1486:
                    identified = typeof(SlaveCommCommand_1486);
                    break;

                // 試薬保冷庫テーブル移動コマンド
                case (int)CommandKind.Command0487:
                    identified = typeof(SlaveCommCommand_0487);
                    break;
                case (int)CommandKind.Command1487:
                    identified = typeof(SlaveCommCommand_1487);
                    break;

                // ラック設置有無確認コマンド
                case (int)CommandKind.Command0488:
                    identified = typeof(SlaveCommCommand_0488);
                    break;
                case (int)CommandKind.Command1488:
                    identified = typeof(SlaveCommCommand_1488);
                    break;

                // ラック設置状況上書きコマンド
                case (int)CommandKind.Command0489:
                    identified = typeof(SlaveCommCommand_0489);
                    break;
                case (int)CommandKind.Command1489:
                    identified = typeof(SlaveCommCommand_1489);
                    break;

                // 再検コマンド
                case (int)CommandKind.Command0490:
                    identified = typeof(SlaveCommCommand_0490);
                    break;
                case (int)CommandKind.Command1490:
                    identified = typeof(SlaveCommCommand_1490);
                    break;

                // STAT状態通知コマンド
                case (int)CommandKind.Command0491:
                    identified = typeof(SlaveCommCommand_0491);
                    break;
                case (int)CommandKind.Command1491:
                    identified = typeof(SlaveCommCommand_1491);
                    break;

                // 試薬保冷庫BC読み込み無効コマンド
                case (int)CommandKind.Command0493:
                    identified = typeof(SlaveCommCommand_0493);
                    break;
                case (int)CommandKind.Command1493:
                    identified = typeof(SlaveCommCommand_1493);
                    break;

                // 試薬保冷庫テーブルSW移動許可コマンド
                case (int)CommandKind.Command0494:
                    identified = typeof(SlaveCommCommand_0494);
                    break;
                case (int)CommandKind.Command1494:
                    identified = typeof(SlaveCommCommand_1494);
                    break;

                // 洗浄液供給コマンド
                case (int)CommandKind.Command0495:
                    identified = typeof(SlaveCommCommand_0495);
                    break;
                case (int)CommandKind.Command1495:
                    identified = typeof(SlaveCommCommand_1495);
                    break;

                // 試薬プローブ調整コマンド
                case (int)CommandKind.Command0497:
                    identified = typeof(SlaveCommCommand_0497);
                    break;
                case (int)CommandKind.Command1497:
                    identified = typeof(SlaveCommCommand_1497);
                    break;

                // ステータス問合せコマンド
                case (int)CommandKind.Command0498:
                    identified = typeof(SlaveCommCommand_0498);
                    break;
                case (int)CommandKind.Command1498:
                    identified = typeof(SlaveCommCommand_1498);
                    break;

                
                
                // サブレディコマンド
                case (int)CommandKind.Command0501:
                    identified = typeof(SlaveCommCommand_0501);
                    break;
                // 測定指示データ問い合わせコマンド
                case (int)CommandKind.Command0502:
                    identified = typeof(SlaveCommCommand_0502);
                    break;
                case (int)CommandKind.Command1502:
                    identified = typeof(SlaveCommCommand_1502);
                    break;

                // 測定データコマンド
                case (int)CommandKind.Command0503:
                    identified = typeof(SlaveCommCommand_0503);
                    break;
                case (int)CommandKind.Command1503:
                    identified = typeof(SlaveCommCommand_1503);
                    break;

                // エラーコマンド
                case (int)CommandKind.Command0504:
                    identified = typeof(SlaveCommCommand_0504);
                    break;
                case (int)CommandKind.Command1504:
                    identified = typeof(SlaveCommCommand_1504);
                    break;

                // サブイベントコマンド
                case (int)CommandKind.Command0505:
                    identified = typeof(SlaveCommCommand_0505);
                    break;
                case (int)CommandKind.Command1505:
                    identified = typeof(SlaveCommCommand_1505);
                    break;

                // 分析ステータスコマンド
                case (int)CommandKind.Command0506:
                    identified = typeof(SlaveCommCommand_0506);
                    break;
                case (int)CommandKind.Command1506:
                    identified = typeof(SlaveCommCommand_1506);
                    break;

                // 分析終了コマンド
                case (int)CommandKind.Command0507:
                    identified = typeof(SlaveCommCommand_0507);
                    break;
                case (int)CommandKind.Command1507:
                    identified = typeof(SlaveCommCommand_1507);
                    break;

                // 残量コマンド
                case (int)CommandKind.Command0508:
                    identified = typeof(SlaveCommCommand_0508);
                    break;
                case (int)CommandKind.Command1508:
                    identified = typeof(SlaveCommCommand_1508);
                    break;

                // モーターパラメータ設定コマンド
                case (int)CommandKind.Command0509:
                    identified = typeof(SlaveCommCommand_0509);
                    break;
                case (int)CommandKind.Command1509:
                    identified = typeof(SlaveCommCommand_1509);
                    break;

                // マスターカーブ情報コマンド
                case (int)CommandKind.Command0510:
                    identified = typeof(SlaveCommCommand_0510);
                    break;
                case (int)CommandKind.Command1510:
                    identified = typeof(SlaveCommCommand_1510);
                    break;

                // バージョンコマンド
                case (int)CommandKind.Command0511:
                    identified = typeof(SlaveCommCommand_0511);
                    break;
                case (int)CommandKind.Command1511:
                    identified = typeof(SlaveCommCommand_1511);
                    break;

                // 試薬ロット確認コマンド
                case (int)CommandKind.Command0512:
                    identified = typeof(SlaveCommCommand_0512);
                    break;
                case (int)CommandKind.Command1512:
                    identified = typeof(SlaveCommCommand_1512);
                    break;

                // キャリブレーション測定確認コマンド
                case (int)CommandKind.Command0513:
                    identified = typeof(SlaveCommCommand_0513);
                    break;
                case (int)CommandKind.Command1513:
                    identified = typeof(SlaveCommCommand_1513);
                    break;

                // 総アッセイ数通知コマンド
                case (int)CommandKind.Command0514:
                    identified = typeof(SlaveCommCommand_0514);
                    break;
                case (int)CommandKind.Command1514:
                    identified = typeof(SlaveCommCommand_1514);
                    break;

                // ラック設置状況コマンド
                case (int)CommandKind.Command0515:
                    identified = typeof(SlaveCommCommand_0515);
                    break;
                case (int)CommandKind.Command1515:
                    identified = typeof(SlaveCommCommand_1515);
                    break;

                // 試薬テーブル回転SW押下通知コマンド
                case (int)CommandKind.Command0516:
                    identified = typeof(SlaveCommCommand_0516);
                    break;
                case (int)CommandKind.Command1516:
                    identified = typeof(SlaveCommCommand_1516);
                    break;

                // 試薬設置状況通知コマンド
                case (int)CommandKind.Command0520:
                    identified = typeof(SlaveCommCommand_0520);
                    break;
                case (int)CommandKind.Command1520:
                    identified = typeof(SlaveCommCommand_1520);
                    break;

                // 廃液タンク状態問合せコマンド
                case (int)CommandKind.Command0521:
                    identified = typeof(SlaveCommCommand_0521);
                    break;
                case (int)CommandKind.Command1521:
                    identified = typeof(SlaveCommCommand_1521);
                    break;

                // キャリブレータ情報通知コマンド
                case (int)CommandKind.Command0522:
                    identified = typeof(SlaveCommCommand_0522);
                    break;
                case (int)CommandKind.Command1522:
                    identified = typeof(SlaveCommCommand_1522);
                    break;

                // STAT状態通知コマンド
                case (int)CommandKind.Command0591:
                    identified = typeof(SlaveCommCommand_0591);
                    break;
                case (int)CommandKind.Command1591:
                    identified = typeof(SlaveCommCommand_1591);
                    break;

                // 分取完了通知コマンド
                case (int)CommandKind.Command0596:
                    identified = typeof(SlaveCommCommand_0596);
                    break;
                case (int)CommandKind.Command1596:
                    identified = typeof(SlaveCommCommand_1596);
                    break;

                default:
                    identified = null;
                    break;
            }

            // TODO:最新のコマンドリストにより、増加分あり

            return identified;
        }
        #endregion

    }
}
