using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Comm;
using Oelco.Common.Utility;
using Oelco.CarisX.Parameter;
using System.Globalization;

using Oelco.CarisX.Const;
using Oelco.CarisX.Utility;
using Oelco.Common.Parameter;

namespace Oelco.CarisX.Comm
{
    /// <summary>
    /// コマンド共通パラメータ
    /// </summary>
    public enum CommandControlParameter
    {
        /// <summary>
        /// 中断
        /// </summary>
        Abort = 0,
        /// <summary>
        /// 初期化
        /// </summary>
        Init = 1,
        /// <summary>
        /// 開始
        /// </summary>
        Start = 2,
        /// <summary>
        /// 停止
        /// </summary>
        Pause = 3,
        /// <summary>
        /// 再開
        /// </summary>
        Restart = 4,
        /// <summary>
        /// 設定
        /// </summary>
        Set = 5,
        /// <summary>
        /// 問合せ
        /// </summary>
        Ask = 6
    }

    /// <summary>
    /// コマンド種別
    /// </summary>
    public enum CommandKind : int
    {
        /// <summary>
        /// 不明
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// ソフトウエア識別コマンド 
        /// </summary>
        Command0401 = 0401,
        /// <summary>
        /// ソフトウエア識別コマンド（レスポンス）
        /// </summary>
        Command1401 = 1401,
        /// <summary>
        /// モーターパラメータ調整通知コマンド
        /// </summary>
        Command0402 = 0402,
        /// <summary>
        /// モーターパラメータ調整通知コマンド（レスポンス）
        /// </summary>
        Command1402 = 1402,
        /// <summary>
        /// シャットダウンコマンド
        /// </summary>
        Command0403 = 0403,
        /// <summary>
        /// シャットダウンコマンド（レスポンス）
        /// </summary>
        Command1403 = 1403,
        /// <summary>
        /// システムパラメータコマンド 
        /// </summary>
        Command0404 = 0404,
        /// <summary>
        /// システムパラメータコマンド（レスポンス）
        /// </summary>
        Command1404 = 1404,
        /// <summary>
        /// プロトコルパラメータコマンド
        /// </summary>
        Command0405 = 0405,
        /// <summary>
        /// プロトコルパラメータコマンド（レスポンス）
        /// </summary>
        Command1405 = 1405,
        /// <summary>
        /// モーター初期化コマンド
        /// </summary>
        Command0406 = 0406,
        /// <summary>
        /// モーター初期化コマンド（レスポンス）
        /// </summary>
        Command1406 = 1406,
        /// <summary>
        /// モーターセルフチェックコマンド
        /// </summary>
        Command0407 = 0407,
        /// <summary>
        /// モーターセルフチェックコマンド（レスポンス）
        /// </summary>
        Command1407 = 1407,
        /// <summary>
        /// 光学系セルフチェックコマンド
        /// </summary>
        Command0408 = 0408,
        /// <summary>
        /// 光学系セルフチェックコマンド（レスポンス）
        /// </summary>
        Command1408 = 1408,
        /// <summary>
        /// プライムコマンド
        /// </summary>
        Command0409 = 0409,
        /// <summary>
        /// プライムコマンド（レスポンス）
        /// </summary>
        Command1409 = 1409,
        /// <summary>
        /// リンス処理コマンド
        /// </summary>
        Command0410 = 0410,
        /// <summary>
        /// リンス処理コマンド（レスポンス）
        /// </summary>
        Command1410 = 1410,
        /// <summary>
        /// 分析開始コマンド
        /// </summary>
        Command0411 = 0411,
        /// <summary>
        /// 分析開始コマンド（レスポンス）
        /// </summary>
        Command1411 = 1411,
        /// <summary>
        /// ポーズコマンド
        /// </summary>
        Command0412 = 0412,
        /// <summary>
        /// ポーズコマンド（レスポンス）
        /// </summary>
        Command1412 = 1412,
        /// <summary>
        /// カレンダー設定コマンド
        /// </summary>
        Command0413 = 0413,
        /// <summary>
        /// カレンダー設定コマンド（レスポンス）
        /// </summary>
        Command1413 = 1413,
        /// <summary>
        /// 残量チェックコマンド
        /// </summary>
        Command0414 = 0414,
        /// <summary>
        /// 残量チェックコマンド（レスポンス）
        /// </summary>
        Command1414 = 1414,
        /// <summary>
        /// 試薬準備確認コマンド
        /// </summary>
        Command0415 = 0415,
        /// <summary>
        /// 試薬準備確認コマンド（レスポンス）
        /// </summary>
        Command1415 = 1415,
        /// <summary>
        /// 試薬準備開始コマンド
        /// </summary>
        Command0416 = 0416,
        /// <summary>
        /// 試薬準備開始コマンド（レスポンス）
        /// </summary>
        Command1416 = 1416,
        /// <summary>
        /// 試薬準備完了コマンド
        /// </summary>
        Command0417 = 0417,
        /// <summary>
        /// 試薬準備完了コマンド（レスポンス）
        /// </summary>
        Command1417 = 1417,
        /// <summary>
        /// 希釈液準備確認コマンド
        /// </summary>
        Command0418 = 0418,
        /// <summary>
        /// 希釈液準備確認コマンド（レスポンス）
        /// </summary>
        Command1418 = 1418,
        /// <summary>
        /// 希釈液準備開始コマンド
        /// </summary>
        Command0419 = 0419,
        /// <summary>
        /// 希釈液準備開始コマンド（レスポンス）
        /// </summary>
        Command1419 = 1419,
        /// <summary>
        /// 希釈液準備完了コマンド
        /// </summary>
        Command0420 = 0420,
        /// <summary>
        /// 希釈液準備完了コマンド（レスポンス）
        /// </summary>
        Command1420 = 1420,
        /// <summary>
        /// プレトリガ準備開始コマンド
        /// </summary>
        Command0421 = 0421,
        /// <summary>
        /// プレトリガ準備開始コマンド（レスポンス）
        /// </summary>
        Command1421 = 1421,
        /// <summary>
        /// プレトリガ準備完了コマンド
        /// </summary>
        Command0422 = 0422,
        /// <summary>
        /// プレトリガ準備完了コマンド（レスポンス）
        /// </summary>
        Command1422 = 1422,
        /// <summary>
        /// トリガ準備開始コマンド
        /// </summary>
        Command0423 = 0423,
        /// <summary>
        /// トリガ準備開始コマンド（レスポンス）
        /// </summary>
        Command1423 = 1423,
        /// <summary>
        /// トリガ準備完了コマンド
        /// </summary>
        Command0424 = 0424,
        /// <summary>
        /// トリガ準備完了コマンド（レスポンス）
        /// </summary>
        Command1424 = 1424,
        /// <summary>
        /// ケース(反応容器・サンプル分注チップ)準備開始コマンド
        /// </summary>
        Command0425 = 0425,
        /// <summary>
        /// ケース(反応容器・サンプル分注チップ)準備開始コマンド（レスポンス）
        /// </summary>
        Command1425 = 1425,
        /// <summary>
        /// ケース(反応容器・サンプル分注チップ)準備完了コマンド
        /// </summary>
        Command0426 = 0426,
        /// <summary>
        /// ケース(反応容器・サンプル分注チップ)準備完了コマンド（レスポンス）
        /// </summary>
        Command1426 = 1426,
        /// <summary>
        /// 試薬残量変更確認コマンド
        /// </summary>
        Command0427 = 0427,
        /// <summary>
        /// 試薬残量変更確認コマンド（レスポンス）
        /// </summary>
        Command1427 = 1427,
        /// <summary>
        /// 試薬残量変更終了コマンド
        /// </summary>
        Command0428 = 0428,
        /// <summary>
        /// 試薬残量変更終了コマンド（レスポンス）
        /// </summary>
        Command1428 = 1428,
        /// <summary>
        /// 試薬残量変更コマンド
        /// </summary>
        Command0429 = 0429,
        /// <summary>
        /// 試薬残量変更コマンド（レスポンス）
        /// </summary>
        Command1429 = 1429,
        /// <summary>
        /// 希釈液残量変更コマンド
        /// </summary>
        Command0430 = 0430,
        /// <summary>
        /// 希釈液残量変更コマンド（レスポンス）
        /// </summary>
        Command1430 = 1430,
        /// <summary>
        /// プレトリガ残量変更コマンド
        /// </summary>
        Command0431 = 0431,
        /// <summary>
        /// プレトリガ残量変更コマンド（レスポンス）
        /// </summary>
        Command1431 = 1431,
        /// <summary>
        /// トリガ残量変更コマンド
        /// </summary>
        Command0432 = 0432,
        /// <summary>
        /// トリガ残量変更コマンド（レスポンス）
        /// </summary>
        Command1432 = 1432,
        /// <summary>
        /// ケース(反応容器・サンプル分注チップ)残量変更コマンド
        /// </summary>
        Command0433 = 0433,
        /// <summary>
        /// ケース(反応容器・サンプル分注チップ)残量変更コマンド（レスポンス）
        /// </summary>
        Command1433 = 1433,
        /// <summary>
        /// 残量セットコマンド
        /// </summary>
        Command0434 = 0434,
        /// <summary>
        /// 残量セットコマンド（レスポンス）
        /// </summary>
        Command1434 = 1434,
        /// <summary>
        /// 廃液ボトルセット開始コマンド
        /// </summary>
        Command0435 = 0435,
        /// <summary>
        /// 廃液ボトルセット開始コマンド（レスポンス）
        /// </summary>
        Command1435 = 1435,
        /// <summary>
        /// 廃液ボトルセット完了コマンド
        /// </summary>
        Command0436 = 0436,
        /// <summary>
        /// 廃液ボトルセット完了コマンド（レスポンス）
        /// </summary>
        Command1436 = 1436,
        /// <summary>
        /// インキュベーター温度設定問い合わせコマンド
        /// </summary>
        Command0437 = 0437,
        /// <summary>
        /// インキュベーター温度設定問い合わせコマンド（レスポンス）
        /// </summary>
        Command1437 = 1437,
        /// <summary>
        /// 試薬保冷庫温度設定問い合わせコマンド
        /// </summary>
        Command0438 = 0438,
        /// <summary>
        /// 試薬保冷庫温度設定問い合わせコマンド（レスポンス）
        /// </summary>
        Command1438 = 1438,
        /// <summary>
        /// ユニットテストコマンド
        /// </summary>
        Command0439 = 0439,
        /// <summary>
        /// ユニットテストコマンド（レスポンス）
        /// </summary>
        Command1439 = 1439,
        /// <summary>
        /// センサーステータス問合せコマンド
        /// </summary>
        Command0440 = 0440,
        /// <summary>
        /// センサーステータス問合せコマンド（レスポンス）
        /// </summary>
        Command1440 = 1440,
        /// <summary>
        /// センサー無効コマンド
        /// </summary>
        Command0441 = 0441,
        /// <summary>
        /// センサー無効コマンド（レスポンス）
        /// </summary>
        Command1441 = 1441,
        /// <summary>
        /// スタートアップ開始コマンド
        /// </summary>
        Command0442 = 0442,
        /// <summary>
        /// スタートアップ開始コマンド（レスポンス）
        /// </summary>
        Command1442 = 1442,
        /// <summary>
        /// スタートアップ終了コマンド
        /// </summary>
        Command0443 = 0443,
        /// <summary>
        /// スタートアップ終了コマンド（レスポンス）
        /// </summary>
        Command1443 = 1443,
        /// <summary>
        /// 寿命部品使用回数設定問い合わせコマンド
        /// </summary>
        Command0444 = 0444,
        /// <summary>
        /// 寿命部品使用回数設定問い合わせコマンド（レスポンス）
        /// </summary>
        Command1444 = 1444,
        /// <summary>
        /// プレトリガアクトコマンド
        /// </summary>
        Command0445 = 0445,
        /// <summary>
        /// プレトリガアクトコマンド（レスポンス）
        /// </summary>
        Command1445 = 1445,
        /// <summary>
        /// トリガアクトコマンド
        /// </summary>
        Command0446 = 0446,
        /// <summary>
        /// トリガアクトコマンド（レスポンス）
        /// </summary>
        Command1446 = 1446,
        /// <summary>
        /// 廃液タンク状態(ラック情報)通知コマンド 
        /// </summary>
        Command0447 = 0447,
        /// <summary>
        /// 廃液タンク状態(ラック情報)通知コマンド（レスポンス）
        /// </summary>
        Command1447 = 1447,
        /// <summary>
        /// ケース搬送ユニットパラメータコマンド 
        /// </summary>
        Command0448 = 0448,
        /// <summary>
        /// ケース搬送ユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1448 = 1448,
        /// <summary>
        /// 試薬保冷庫ユニットパラメータコマンド
        /// </summary>
        Command0449 = 0449,
        /// <summary>
        /// 試薬保冷庫ユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1449 = 1449,
        /// <summary>
        /// スタットユニットパラメータコマンド
        /// </summary>
        Command0450 = 0450,
        /// <summary>
        /// スタットユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1450 = 1450,
        /// <summary>
        /// サンプル分注ユニットパラメータコマンド
        /// </summary>
        Command0451 = 0451,
        /// <summary>
        /// サンプル分注ユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1451 = 1451,
        /// <summary>
        /// 反応容器搬送ユニットパラメータコマンド
        /// </summary>
        Command0452 = 0452,
        /// <summary>
        /// 反応容器搬送ユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1452 = 1452,
        /// <summary>
        /// 反応テーブルユニットパラメータコマンド 
        /// </summary>
        Command0453 = 0453,
        /// <summary>
        /// 反応テーブルユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1453 = 1453,
        /// <summary>
        /// BFテーブルユニットパラメータコマンド 
        /// </summary>
        Command0454 = 0454,
        /// <summary>
        /// BFテーブルユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1454 = 1454,
        /// <summary>
        /// トラベラーユニットパラメータコマンド 
        /// </summary>
        Command0455 = 0455,
        /// <summary>
        /// トラベラーユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1455 = 1455,
        /// <summary>
        /// 試薬分注1部ユニットパラメータパラメータコマンド 
        /// </summary>
        Command0456 = 0456,
        /// <summary>
        /// 試薬分注1部ユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1456 = 1456,
        /// <summary>
        /// 試薬分注2部ユニットパラメータコマンド
        /// </summary>
        Command0457 = 0457,
        /// <summary>
        /// 試薬分注2部ユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1457 = 1457,
        /// <summary>
        /// BF1ユニットパラメータコマンド 
        /// </summary>
        Command0458 = 0458,
        /// <summary>
        /// BF1ユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1458 = 1458,
        /// <summary>
        /// BF2ユニットパラメータコマンド
        /// </summary>
        Command0459 = 0459,
        /// <summary>
        /// BF2ユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1459 = 1459,
        /// <summary>
        /// 希釈分注ユニットパラメータコマンド
        /// </summary>
        Command0460 = 0460,
        /// <summary>
        /// 希釈分注ユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1460 = 1460,
        /// <summary>
        /// プレトリガユニットパラメータコマンド
        /// </summary>
        Command0461 = 0461,
        /// <summary>
        /// プレトリガユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1461 = 1461,
        /// <summary>
        /// トリガ分注測光ユニットパラメータコマンド
        /// </summary>
        Command0462 = 0462,
        /// <summary>
        /// トリガ分注測光ユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1462 = 1462,
        /// <summary>
        /// 流体配管ユニットパラメータコマンド
        /// </summary>
        Command0463 = 0463,
        /// <summary>
        /// 流体配管ユニットパラメータコマンド（レスポンス）
        /// </summary>
        Command1463 = 1463,
        /// <summary>
        /// 警告灯制御コマンド
        /// </summary>
        Command0464 = 0464,
        /// <summary>
        /// 警告灯制御コマンド（レスポンス）
        /// </summary>
        Command1464 = 1464,
        /// <summary>
        /// ブザー制御コマンド
        /// </summary>
        Command0465 = 0465,
        /// <summary>
        /// ブザー制御コマンド（レスポンス）
        /// </summary>
        Command1465 = 1465,
        /// <summary>
        /// 簡易プライムコマンド
        /// </summary>
        Command0466 = 0466,
        /// <summary>
        /// 簡易プライムコマンド（レスポンス）
        /// </summary>
        Command1466 = 1466,
        /// <summary>
        /// スレーブ接続確認コマンド
        /// </summary>
        Command0467 = 0467,
        /// <summary>
        /// スレーブ接続確認コマンド（レスポンス）
        /// </summary>
        Command1467 = 1467,
        /// <summary>
        /// サンプル停止要因問合せコマンド
        /// </summary>
        Command0468 = 0468,
        /// <summary>
        /// サンプル停止要因問合せコマンド（レスポンス）
        /// </summary>
        Command1468 = 1468,
        /// <summary>
        /// ラック排出コマンド
        /// </summary>
        Command0469 = 0469,
        /// <summary>
        /// ラック排出コマンド（レスポンス）
        /// </summary>
        Command1469 = 1469,
        /// <summary>
        /// モーターパラメータ保存コマンド
        /// </summary>
        Command0471 = 0471,
        /// <summary>
        /// モーターパラメータ保存コマンド（レスポンス）
        /// </summary>
        Command1471 = 1471,
        /// <summary>
        /// PID制御開始コマンド
        /// </summary>
        Command0472 = 0472,
        /// <summary>
        /// PID制御開始コマンド（レスポンス）
        /// </summary>
        Command1472 = 1472,
        /// <summary>
        /// モーター調整コマンド
        /// </summary>
        Command0473 = 0473,
        /// <summary>
        /// モーター調整コマンド（レスポンス）
        /// </summary>
        Command1473 = 1473,
        /// <summary>
        /// PID定数設定コマンド
        /// </summary>
        Command0474 = 0474,
        /// <summary>
        /// PID定数設定コマンド（レスポンス）
        /// </summary>
        Command1474 = 1474,
        /// <summary>
        /// 残量クリアコマンド
        /// </summary>
        Command0475 = 0475,
        /// <summary>
        /// 残量クリアコマンド（レスポンス）
        /// </summary>
        Command1475 = 1475,
        /// <summary>
        /// 準備中断コマンド
        /// </summary>
        Command0476 = 0476,
        /// <summary>
        /// 準備中断コマンド（レスポンス）
        /// </summary>
        Command1476 = 1476,
        /// <summary>
        /// カレンダーコマンド
        /// </summary>
        Command0477 = 0477,
        /// <summary>
        /// カレンダーコマンド（レスポンス）
        /// </summary>
        Command1477 = 1477,
        /// <summary>
        /// ユニット無効コマンド
        /// </summary>
        Command0478 = 0478,
        /// <summary>
        /// ユニット無効コマンド（レスポンス）
        /// </summary>
        Command1478 = 1478,
        /// <summary>
        /// 試薬ロットサンプル停止要因解除コマンド
        /// </summary>
        Command0479 = 0479,
        /// <summary>
        /// 試薬ロットサンプル停止要因解除コマンド（レスポンス）
        /// </summary>
        Command1479 = 1479,
        /// <summary>
        /// 調整位置停止コマンド
        /// </summary>
        Command0480 = 0480,
        /// <summary>
        /// 調整位置停止コマンド（レスポンス）
        /// </summary>
        Command1480 = 1480,
        /// <summary>
        /// 調整位置再開コマンド
        /// </summary>
        Command0481 = 0481,
        /// <summary>
        /// 調整位置再開コマンド（レスポンス）
        /// </summary>
        Command1481 = 1481,
        /// <summary>
        /// シリンジエージングコマンド
        /// </summary>
        Command0483 = 0483,
        /// <summary>
        /// シリンジエージングコマンド（レスポンス）
        /// </summary>
        Command1483 = 1483,
        /// <summary>
        /// 総アッセイ数設定コマンド
        /// </summary>
        Command0484 = 0484,
        /// <summary>
        /// 総アッセイ数設定コマンド（レスポンス）
        /// </summary>
        Command1484 = 1484,
        /// <summary>
        /// サンプル必要量コマンド
        /// </summary>
        Command0485 = 0485,
        /// <summary>
        /// サンプル必要量コマンド（レスポンス）
        /// </summary>
        Command1485 = 1485,
        /// <summary>
        /// 分析強制終了コマンド
        /// </summary>
        Command0486 = 0486,
        /// <summary>
        /// 分析強制終了コマンド（レスポンス）
        /// </summary>
        Command1486 = 1486,
        /// <summary>
        /// 試薬保冷庫テーブル移動コマンド
        /// </summary>
        Command0487 = 0487,
        /// <summary>
        /// 試薬保冷庫テーブル移動コマンド（レスポンス）
        /// </summary>
        Command1487 = 1487,
        /// <summary>
        /// ラック設置有無確認コマンド
        /// </summary>
        Command0488 = 0488,
        /// <summary>
        /// ラック設置有無確認コマンド（レスポンス）
        /// </summary>
        Command1488 = 1488,
        /// <summary>
        /// ラック設置状況上書きコマンド
        /// </summary>
        Command0489 = 0489,
        /// <summary>
        /// ラック設置状況上書きコマンド（レスポンス）
        /// </summary>
        Command1489 = 1489,
        /// <summary>
        /// 再検コマンド
        /// </summary>
        Command0490 = 0490,
        /// <summary>
        /// 再検コマンド（レスポンス）
        /// </summary>
        Command1490 = 1490,
        /// <summary>
        /// STAT状態通知コマンド
        /// </summary>
        Command0491 = 0491,
        /// <summary>
        /// STAT状態通知コマンド（レスポンス）
        /// </summary>
        Command1491 = 1491,
        /// <summary>
        /// 試薬保冷庫BC読み込み無効マンド
        /// </summary>
        Command0493 = 0493,
        /// <summary>
        /// 試薬保冷庫BC読み込み無効コマンド（レスポンス）
        /// </summary>
        Command1493 = 1493,
        /// <summary>
        /// 試薬保冷庫テーブルSW移動許可コマンド
        /// </summary>
        Command0494 = 0494,
        /// <summary>
        /// 試薬保冷庫テーブルSW移動許可コマンド（レスポンス）
        /// </summary>
        Command1494 = 1494,
        /// <summary>
        /// 洗浄液供給コマンド
        /// </summary>
        Command0495 = 0495,
        /// <summary>
        /// 洗浄液供給コマンド（レスポンス）
        /// </summary>
        Command1495 = 1495,
        /// <summary>
        /// プローブ交換コマンド
        /// </summary>
        Command0497 = 0497,
        /// <summary>
        /// プローブ交換コマンド（レスポンス）
        /// </summary>
        Command1497 = 1497,
        /// <summary>
        /// ステータス問い合わせコマンド
        /// </summary>
        Command0498 = 0498,
        /// <summary>
        /// ステータス問い合わせコマンド（レスポンス）
        /// </summary>
        Command1498 = 1498,
        /// <summary>
        /// プログラム転送用コマンド
        /// </summary>
        Command0499 = 0499,
        /// <summary>
        /// プログラム転送用コマンド（レスポンス）
        /// </summary>
        Command1499 = 1499,

        /// <summary>
        /// サブレディコマンド
        /// </summary>
        Command0501 = 0501,
        /// ※サブレディは応答なし
        /// <summary>
        /// 測定指示データ問い合わせコマンド
        /// </summary>
        Command0502 = 0502,
        /// <summary>
        /// 測定指示データ問い合わせコマンド（レスポンス）
        /// </summary>
        Command1502 = 1502,
        /// <summary>
        /// 測定データコマンド
        /// </summary>
        Command0503 = 0503,
        /// <summary>
        /// 測定データコマンド（レスポンス）
        /// </summary>
        Command1503 = 1503,
        /// <summary>
        /// エラーコマンド(スレーブ用)
        /// </summary>
        Command0504 = 0504,
        /// <summary>
        /// エラーコマンド(スレーブ用)（レスポンス）
        /// </summary>
        Command1504 = 1504,
        /// <summary>
        /// サブイベントコマンド
        /// </summary>
        Command0505 = 0505,
        /// <summary>
        /// サブイベントコマンド（レスポンス）
        /// </summary>
        Command1505 = 1505,
        /// <summary>
        /// 分析ステータスコマンド
        /// </summary>
        Command0506 = 0506,
        /// <summary>
        /// 分析ステータスコマンド（レスポンス）
        /// </summary>
        Command1506 = 1506,
        /// <summary>
        /// 分析終了コマンド
        /// </summary>
        Command0507 = 0507,
        /// <summary>
        /// 分析終了コマンド（レスポンス）
        /// </summary>
        Command1507 = 1507,
        /// <summary>
        /// 残量コマンド(スレーブ用)
        /// </summary>
        Command0508 = 0508,
        /// <summary>
        /// 残量コマンド(スレーブ用)（レスポンス）
        /// </summary>
        Command1508 = 1508,
        /// <summary>
        /// モーターパラメータ設定コマンド
        /// </summary>
        Command0509 = 0509,
        /// <summary>
        /// モーターパラメータ設定コマンド（レスポンス）
        /// </summary>
        Command1509 = 1509,
        /// <summary>
        /// マスタカーブ情報コマンド
        /// </summary>
        Command0510 = 0510,
        /// <summary>
        /// マスターカーブ情報コマンド（レスポンス）
        /// </summary>
        Command1510 = 1510,
        /// <summary>
        /// バージョン通知コマンド(スレーブ用)
        /// </summary>
        Command0511 = 0511,
        /// <summary>
        /// バージョン通知コマンド(スレーブ用)（レスポンス）
        /// </summary>
        Command1511 = 1511,
        /// <summary>
        /// 試薬ロット確認コマンド
        /// </summary>
        Command0512 = 0512,
        /// <summary>
        /// 試薬ロット確認コマンド（レスポンス）
        /// </summary>
        Command1512 = 1512,
        /// <summary>
        /// キャリブレーション測定確認コマンド
        /// </summary>
        Command0513 = 0513,
        /// <summary>
        /// キャリブレーション測定確認コマンド（レスポンス）
        /// </summary>
        Command1513 = 1513,
        /// <summary>
        /// 総アッセイ数通知コマンド
        /// </summary>
        Command0514 = 0514,
        /// <summary>
        /// 総アッセイ数通知コマンド（レスポンス）
        /// </summary>
        Command1514 = 1514,
        /// <summary>
        /// ラック設置状況コマンド
        /// </summary>
        Command0515 = 0515,
        /// <summary>
        /// ラック設置状況コマンド（レスポンス）
        /// </summary>
        Command1515 = 1515,
        /// <summary>
        /// 試薬テーブル回転SW押下通知コマンド
        /// </summary>
        Command0516 = 0516,
        /// <summary>
        /// 試薬テーブル回転SW押下通知コマンド（レスポンス）
        /// </summary>
        Command1516 = 1516,
        /// <summary>
        /// 試薬設置状況通知コマンド
        /// </summary>
        Command0520 = 0520,
        /// <summary>
        /// 試薬設置状況通知コマンド（レスポンス）
        /// </summary>
        Command1520 = 1520,
        /// <summary>
        /// 廃液タンク状態問合せコマンド
        /// </summary>
        Command0521 = 0521,
        /// <summary>
        /// 廃液タンク状態問合せコマンド（レスポンス）
        /// </summary>
        Command1521 = 1521,
        /// <summary>
        /// キャリブレータ情報通知コマンド
        /// </summary>
        Command0522 = 0522,
        /// <summary>
        /// キャリブレータ情報通知コマンド（レスポンス）
        /// </summary>
        Command1522 = 1522,
        /// <summary>
        /// STAT状態通知コマンド
        /// </summary>
        Command0591 = 0591,
        /// <summary>
        /// STAT状態通知コマンド（レスポンス）
        /// </summary>
        Command1591 = 1591,
        /// <summary>
        /// 分取完了通知コマンド
        /// </summary>
        Command0596 = 0596,
        /// <summary>
        /// 分取完了通知コマンド（レスポンス）
        /// </summary>
        Command1596 = 1596,

        /// <summary>
        /// 検査依頼問合せコマンド
        /// </summary>
        HostCommand0001 = 1001,
        /// <summary>
        /// 検査依頼コマンド
        /// </summary>
        HostCommand0002 = 1002,
        /// <summary>
        /// 検査結果コマンド
        /// </summary>
        HostCommand0003 = 1003,
        /// <summary>
        /// 装置ステータス問合せコマンド
        /// </summary>
        HostCommand0004 = 1004,
        /// <summary>
        /// 装置ステータスコマンド
        /// </summary>
        HostCommand0005 = 1005,

        /// <summary>
        /// ソフトウェア識別コマンド
        /// </summary>
        RackTransferCommand0001 = 0001,
        /// <summary>
        /// ソフトウェア識別コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1001 = 1001,
        /// <summary>
        /// モーターパラメータ調整通知コマンド
        /// </summary>
        RackTransferCommand0002 = 0002,
        /// <summary>
        /// モーターパラメータ調整通知コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1002 = 1002,
        /// <summary>
        /// シャットダウンコマンド
        /// </summary>
        RackTransferCommand0003 = 0003,
        /// <summary>
        /// シャットダウンコマンド（レスポンス）
        /// </summary>
        RackTransferCommand1003 = 1003,
        /// <summary>
        /// システムパラメータコマンド
        /// </summary>
        RackTransferCommand0004 = 0004,
        /// <summary>
        /// システムパラメータコマンド（レスポンス）
        /// </summary>
        RackTransferCommand1004 = 1004,
        /// <summary>
        /// モーター初期化コマンド
        /// </summary>
        RackTransferCommand0006 = 0006,
        /// <summary>
        /// モーター初期化コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1006 = 1006,
        /// <summary>
        /// モーターセルフチェックコマンド
        /// </summary>
        RackTransferCommand0007 = 0007,
        /// <summary>
        /// モーターセルフチェックコマンド（レスポンス）
        /// </summary>
        RackTransferCommand1007 = 1007,
        /// <summary>
        /// 分析開始コマンド
        /// </summary>
        RackTransferCommand0011 = 0011,
        /// <summary>
        /// 分析開始コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1011 = 1011,
        /// <summary>
        /// ポーズコマンド
        /// </summary>
        RackTransferCommand0012 = 0012,
        /// <summary>
        /// ポーズコマンド（レスポンス）
        /// </summary>
        RackTransferCommand1012 = 1012,
        /// <summary>
        /// カレンダー設定コマンド
        /// </summary>
        RackTransferCommand0013 = 0013,
        /// <summary>
        /// カレンダー設定コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1013 = 1013,
        /// <summary>
        /// 残量チェックコマンド
        /// </summary>
        RackTransferCommand0014 = 0014,
        /// <summary>
        /// 残量チェックコマンド（レスポンス）
        /// </summary>
        RackTransferCommand1014 = 1014,
        /// <summary>
        /// ユニットテストコマンド
        /// </summary>
        RackTransferCommand0039 = 0039,
        /// <summary>
        /// ユニットテストコマンド（レスポンス）
        /// </summary>
        RackTransferCommand1039 = 1039,
        /// <summary>
        /// センサーステータスコマンド
        /// </summary>
        RackTransferCommand0040 = 0040,
        /// <summary>
        /// センサーステータスコマンド（レスポンス）
        /// </summary>
        RackTransferCommand1040 = 1040,
        /// <summary>
        /// センサー無効コマンド
        /// </summary>
        RackTransferCommand0041 = 0041,
        /// <summary>
        /// センサー無効コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1041 = 1041,
        /// <summary>
        /// スタートアップ開始コマンド
        /// </summary>
        RackTransferCommand0042 = 0042,
        /// <summary>
        /// スタートアップ開始コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1042 = 1042,
        /// <summary>
        /// スタートアップ終了コマンド
        /// </summary>
        RackTransferCommand0043 = 0043,
        /// <summary>
        /// スタートアップ終了コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1043 = 1043,
        /// <summary>
        /// END処理コマンド
        /// </summary>
        RackTransferCommand0044 = 0044,
        /// <summary>
        /// END処理コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1044 = 1044,
        /// <summary>
        /// ラックユニットパラメータコマンド
        /// </summary>
        RackTransferCommand0047 = 0047,
        /// <summary>
        /// ラックユニットパラメータコマンド（レスポンス）
        /// </summary>
        RackTransferCommand1047 = 1047,
        /// <summary>
        /// ラック接続確認コマンド
        /// </summary>
        RackTransferCommand0067 = 0067,
        /// <summary>
        /// ラック接続確認コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1067 = 1067,
        /// <summary>
        /// サンプル停止要因問合せコマンド
        /// </summary>
        RackTransferCommand0068 = 0068,
        /// <summary>
        /// サンプル停止要因問合せコマンド（レスポンス）
        /// </summary>
        RackTransferCommand1068 = 1068,
        /// <summary>
        /// ラック排出コマンド
        /// </summary>
        RackTransferCommand0069 = 0069,
        /// <summary>
        /// ラック排出コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1069 = 1069,
        /// <summary>
        /// モーターパラメータ保存コマンド
        /// </summary>
        RackTransferCommand0071 = 0071,
        /// <summary>
        /// モーターパラメータ保存コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1071 = 1071,
        /// <summary>
        /// モーター調整コマンド
        /// </summary>
        RackTransferCommand0073 = 0073,
        /// <summary>
        /// モーター調整コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1073 = 1073,
        /// <summary>
        /// カレンダーコマンド
        /// </summary>
        RackTransferCommand0077 = 0077,
        /// <summary>
        /// カレンダーコマンド（レスポンス）
        /// </summary>
        RackTransferCommand1077 = 1077,
        /// <summary>
        /// ユニット無効コマンド
        /// </summary>
        RackTransferCommand0078 = 0078,
        /// <summary>
        /// ユニット無効コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1078 = 1078,
        /// <summary>
        /// 調整位置停止コマンド
        /// </summary>
        RackTransferCommand0080 = 0080,
        /// <summary>
        /// 調整位置停止コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1080 = 1080,
        /// <summary>
        /// 調整位置再開コマンド
        /// </summary>
        RackTransferCommand0081 = 0081,
        /// <summary>
        /// 調整位置再開コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1081 = 1081,
        /// <summary>
        /// 検体バーコード設定コマンド
        /// </summary>
        RackTransferCommand0082 = 0082,
        /// <summary>
        /// 検体バーコード設定コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1082 = 1082,
        /// <summary>
        /// 分析強制終了コマンド
        /// </summary>
        RackTransferCommand0086 = 0086,
        /// <summary>
        /// 分析強制終了コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1086 = 1086,
        /// <summary>
        /// ラック設置有無確認コマンド
        /// </summary>
        RackTransferCommand0088 = 0088,
        /// <summary>
        /// ラック設置有無確認コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1088 = 1088,
        /// <summary>
        /// ラック状況上書きコマンド
        /// </summary>
        RackTransferCommand0089 = 0089,
        /// <summary>
        /// ラック状況上書きコマンド（レスポンス）
        /// </summary>
        RackTransferCommand1089 = 1089,
        /// <summary>
        /// 再検コマンド
        /// </summary>
        RackTransferCommand0090 = 0090,
        /// <summary>
        /// 再検コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1090 = 1090,
        /// <summary>
        /// 分取完了通知コマンド
        /// </summary>
        RackTransferCommand0096 = 0096,
        /// <summary>
        /// 分取完了通知コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1096 = 1096,
        /// <summary>
        /// 測定完了通知コマンド
        /// </summary>
        RackTransferCommand0097 = 0097,
        /// <summary>
        /// 測定完了通知コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1097 = 1097,
        /// <summary>
        /// ステータス問合せコマンド
        /// </summary>
        RackTransferCommand0098 = 0098,
        /// <summary>
        /// ステータス問合せコマンド（レスポンス）
        /// </summary>
        RackTransferCommand1098 = 1098,


        /// <summary>
        /// ラックレディーコマンド
        /// </summary>
        RackTransferCommand0101 = 0101,
        /// <summary>
        /// エラー通知コマンド(ラック搬送用)
        /// </summary>
        RackTransferCommand0104 = 0104,
        /// <summary>
        /// エラー通知コマンド(ラック搬送用)（レスポンス）
        /// </summary>
        RackTransferCommand1104 = 1104,
        /// <summary>
        /// ラックイベント通知コマンド
        /// </summary>
        RackTransferCommand0105 = 0105,
        /// <summary>
        /// ラックイベント通知コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1105 = 1105,
        /// <summary>
        /// ラック分析ステータスコマンド
        /// </summary>
        RackTransferCommand0106 = 0106,
        /// <summary>
        /// ラック分析ステータスコマンド（レスポンス）
        /// </summary>
        RackTransferCommand1106 = 1106,
        /// <summary>
        /// 残量コマンド(ラック搬送用)
        /// </summary>
        RackTransferCommand0108 = 0108,
        /// <summary>
        /// 残量コマンド(ラック搬送用)（レスポンス）
        /// </summary>
        RackTransferCommand1108 = 1108,
        /// <summary>
        /// モーターパラメータ通知コマンド
        /// </summary>
        RackTransferCommand0109 = 0109,
        /// <summary>
        /// モーターパラメータ通知コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1109 = 1109,
        /// <summary>
        /// バージョン通知コマンド(ラック搬送用)
        /// </summary>
        RackTransferCommand0111 = 0111,
        /// <summary>
        /// バージョン通知コマンド(ラック搬送用)（レスポンス）
        /// </summary>
        RackTransferCommand1111 = 1111,
        /// <summary>
        /// ラック情報通知コマンド
        /// </summary>
        RackTransferCommand0117 = 0117,
        /// <summary>
        /// ラック情報通知コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1117 = 1117,
        /// <summary>
        /// ラック状態通知コマンド
        /// </summary>
        RackTransferCommand0118 = 0118,
        /// <summary>
        /// ラック状態通知コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1118 = 1118,
        /// <summary>
        /// ラック移動位置問合せ（装置待機位置）コマンド
        /// </summary>
        RackTransferCommand0119 = 0119,
        /// <summary>
        /// ラック移動位置問合せ（装置待機位置）コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1119 = 1119,
        /// <summary>
        /// ラック移動位置問合せ（BCR）コマンド
        /// </summary>
        RackTransferCommand0120 = 0120,
        /// <summary>
        /// ラック移動位置問合せ（BCR）コマンド（レスポンス）
        /// </summary>
        RackTransferCommand1120 = 1120,

        /// <summary>
        /// ラックキャッチ要求コマンド
        /// </summary>
        RackAndSlaveCommand0201 = 0201,
        /// <summary>
        /// ラックキャッチ要求コマンド（レスポンス）
        /// </summary>
        RackAndSlaveCommand1201 = 1201,
        /// <summary>
        /// ラックリリース要求コマンド
        /// </summary>
        RackAndSlaveCommand0202 = 0202,
        /// <summary>
        /// ラックリリース要求コマンド（レスポンス）
        /// </summary>
        RackAndSlaveCommand1202 = 1202,
        /// <summary>
        /// ラックキャッチコマンド
        /// </summary>
        RackAndSlaveCommand0301 = 0301,
        /// <summary>
        /// ラックキャッチコマンド（レスポンス）
        /// </summary>
        RackAndSlaveCommand1301 = 1301,
        /// <summary>
        /// ラックリリースコマンド
        /// </summary>
        RackAndSlaveCommand0302 = 0302,
        /// <summary>
        /// ラックリリースコマンド（レスポンス）
        /// </summary>
        RackAndSlaveCommand1302 = 1302,

        // 2020-02-27 CarisX IoT Add [START]
        /// <summary>
        /// IoT測定結果コマンド
        /// </summary>
        IoTCommand0001 = 10,
        /// <summary>
        /// IoT障害情報コマンド
        /// </summary>
        IoTCommand0002 = 20,
        /// <summary>
        /// IoT日付情報コマンド
        /// </summary>
        IoTCommand0003 = 30,
        // 2020-02-27 CarisX IoT Add [END]
    }

    /// <summary>
    /// PID設定コマンド 温調場所
    /// </summary>
    public enum PIDTempArea
    {
        /// <summary>
        /// すべて
        /// </summary>
        All = 0,
        /// <summary>
        /// 反応テーブル温度
        /// </summary>
        ReactionTableTemp = 1,
        /// <summary>
        /// B/Fテーブル温度
        /// </summary>
        BFTableTemp = 2,
        /// <summary>
        /// B/F1プレヒート温度
        /// </summary>
        BF1PreheatTemp = 3,
        /// <summary>
        /// B/F2プレヒート温度
        /// </summary>
        BF2PreheatTemp = 4,
        /// <summary>
        /// R1プローブプレヒート温度
        /// </summary>
        R1PreheatTemp = 5,
        /// <summary>
        /// R2プローブプレヒート温度
        /// </summary>
        R2PreheatTemp = 6,
        /// <summary>
        /// 測光部温度
        /// </summary>
        PtotometryTemp = 7,
    }

    /// <summary>
    /// PID設定コマンド コントロール
    /// </summary>
    public enum PIDControl
    {
        /// <summary>
        /// Stop
        /// </summary>
        Stop = 0,
        /// <summary>
        /// Start
        /// </summary>
        Start = 1
    }

    /// <summary>
    /// モジュール種別
    /// </summary>
    public enum ModuleKind
    {
        /// <summary>
        /// Slave
        /// </summary>
        Slave = 0,
        /// <summary>
        /// RackTransfer
        /// </summary>
        RackTransfer = 1
    }

    /// <summary>
    /// CarisXコマンド処理
    /// </summary>
    /// <remarks>
    /// CarisXで使用される通信コマンドの処理提供します。
    /// このクラスではホスト、スレーブ双方のコマンドを取り扱います。
    /// </remarks>
    public class CarisXCommCommand : CommCommand
    {
        #region [クラス変数定義]

        /// <summary>
        /// 保持コマンド種別
        /// </summary>
        private CommandKind commKind = CommandKind.Unknown;

        /// <summary>
        /// コマンドID
        /// </summary>
        private Int32 commandId = 0;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 保持コマンド種別
        /// </summary>
        public CommandKind CommKind
        {
            get
            {
                return this.commKind;
            }
            set
            {
                this.commKind = value;
            }
        }

        /// <summary>
        /// コマンドID
        /// </summary>
        public override Int32 CommandId
        {
            get
            {
                return this.commandId;
            }
            set
            {
                this.commandId = value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド文字列の設定
        /// </summary>
        /// <remarks>
        /// コマンド文字列を設定します
        /// </remarks>
        /// <param name="commandStr"></param>
        /// <returns></returns>
        public override Boolean SetCommandString(String commandStr)
        {

            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            return true;
        }

        #endregion
    }

    // CarisXコマンドクラス群
    #region コマンドクラス

    /// <summary>
    /// モータースピード定義
    /// </summary>
    public class ItemMParam
    {
        /// <summary>
        /// 初期速度
        /// </summary>
        public Int32 InitSpeed;
        /// <summary>
        /// 最高速度
        /// </summary>
        public Int32 TopSpeed;
        /// <summary>
        /// 加速度
        /// </summary>
        public Int32 Accel;
        /// <summary>
        /// 定速度
        /// </summary>
        public Int32 ConstSpeed;
    }

    /// <summary>
    /// プロトコルリスト
    /// </summary>
    public class ItemProtoParam
    {
        /// <summary>
        /// アッセイシーケンス
        /// </summary>
        public Int32 AssaySeq;
        /// <summary>
        /// 前処理シーケンス
        /// </summary>
        public Int32 preProcessSeq;
        /// <summary>
        /// 試薬コード
        /// </summary>
        public Int32 ReagCode;
        /// <summary>
        /// サンプル分注量
        /// </summary>
        public Int32 SmpDispenseVolume;
        /// <summary>
        /// M試薬分注量
        /// </summary>
        public Int32 MReagDispenseVolume;
        /// <summary>
        /// R1分注量
        /// </summary>
        public Int32 R1DispenseVolume;
        /// <summary>
        /// R2分注量
        /// </summary>
        public Int32 R2DispenseVolume;
        /// <summary>
        /// 前処理液1(R1)分注量
        /// </summary>
        public Int32 PreProsess1DispenseVolume;
        /// <summary>
        /// 前処理液2(R2)分注量
        /// </summary>
        public Int32 PreProsess2DispenseVolume;
        /// <summary>
        /// 試薬開封後有効期間 　日
        /// </summary>
        public Int32 DayOfReagentValid;
        /// <summary>
        /// キャリブレーの希釈状態
        /// </summary>
        public Int32 DiluCalibOrControl;
        /// <summary>
        /// TB_IGRA使用有無
        /// </summary>
        public byte IsTB_IGRA;
        /// <summary>
        /// プロトコル希釈倍率
        /// </summary>
        public Int32 ProtocolDilutionRatio;
        /// <summary>
        /// 急診有無
        /// </summary>
        public byte AnalysisMode;
        /// <summary>
        /// R1ユニットの分注順逆転
        /// </summary>
        public byte ReverseDispensingOrderR1;
    }

    /// <summary>
    /// 設定温度・温度
    /// </summary>
    public class ItemRSIncTemp
    {
        /// <summary>
        /// 反応テーブル
        /// </summary>
        public Double ReactionTableTemp;
        /// <summary>
        /// BFテーブル
        /// </summary>
        public Double BFTableTemp;
        /// <summary>
        /// B/F1プレヒート温度
        /// </summary>
        public Double BF1PreHeatTemp;
        /// <summary>
        /// B/F2プレヒート温度
        /// </summary>
        public Double BF2PreHeatTemp;
        /// <summary>
        /// R1プローブプレヒート温度
        /// </summary>
        public Double R1ProbeTemp;
        /// <summary>
        /// R2プローブプレヒート温度
        /// </summary>
        public Double R2ProbeTemp;
        /// <summary>
        /// 測光部温度
        /// </summary>
        public Double ChemiluminesoensePtotometryTemp;
    }

    #region Slave

    #region User→Slave
    /// <summary>
    /// ソフトウェア識別コマンド
    /// </summary>
    /// <remarks>
    /// ソフトウェア識別コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0401 : RackTransferCommCommand_0001
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0401()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0401;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// ソフトウェア識別コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ソフトウェア識別コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1401 : RackTransferCommCommand_1001
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1401()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1401;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// モーターパラメータ調整通知コマンド
    /// </summary>
    /// <remarks>
    /// モーターパラメータ調整通知コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0402 : RackTransferCommCommand_0002
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0402()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0402;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// モーターパラメータ調整通知コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// モーターパラメータ調整通知コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1402 : RackTransferCommCommand_1002
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1402()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1402;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// シャットダウンコマンド
    /// </summary>
    /// <remarks>
    /// シャットダウンコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0403 : RackTransferCommCommand_0003
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0403()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0403;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// シャットダウンコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// シャットダウンコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1403 : RackTransferCommCommand_1003
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1403()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1403;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// システムパラメータコマンド
    /// </summary>
    /// <remarks>
    /// システムパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0404 : RackTransferCommCommand_0004
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0404()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0404;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// システムパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// システムパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1404 : RackTransferCommCommand_1004
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1404()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1404;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// プロトコルパラメータコマンド
    /// </summary>
    /// <remarks>
    /// プロトコルパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0405 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0405()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0405;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// プロトコル番号
        /// </summary>
        public Int32 ProtoNo { get; set; } = 0;

        /// <summary>
        /// プロトコルリスト
        /// </summary>
        public ItemProtoParam ItemProtoParam { get; set; } = new ItemProtoParam();

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.ProtoNo.ToString(), 3);
                builder.Append(this.ItemProtoParam.AssaySeq.ToString(), 1);
                builder.Append(this.ItemProtoParam.preProcessSeq.ToString(), 1);
                builder.Append(this.ItemProtoParam.ReagCode.ToString(), 3);
                builder.Append(this.ItemProtoParam.SmpDispenseVolume.ToString(), 4);
                builder.Append(this.ItemProtoParam.MReagDispenseVolume.ToString(), 4);
                builder.Append(this.ItemProtoParam.R1DispenseVolume.ToString(), 4);
                builder.Append(this.ItemProtoParam.R2DispenseVolume.ToString(), 4);
                builder.Append(this.ItemProtoParam.PreProsess1DispenseVolume.ToString(), 4);
                builder.Append(this.ItemProtoParam.PreProsess2DispenseVolume.ToString(), 4);
                builder.Append(this.ItemProtoParam.DayOfReagentValid.ToString(), 4);
                builder.Append(this.ItemProtoParam.DiluCalibOrControl.ToString(), 1);
                builder.Append(this.ItemProtoParam.IsTB_IGRA.ToString(), 1);
                builder.Append(this.ItemProtoParam.ProtocolDilutionRatio.ToString(), 4);
                builder.Append(this.ItemProtoParam.AnalysisMode.ToString(), 1);
                builder.Append(this.ItemProtoParam.ReverseDispensingOrderR1.ToString(), 1);
                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// プロトコル値設定
        /// </summary>
        /// <param name="protocol"></param>
		public void SetProtocolParameter(MeasureProtocol protocol)
        {
            this.ProtoNo = protocol.ProtocolNo;                                                 //分析項目番号
            this.ItemProtoParam.AssaySeq = (Int32)protocol.AssaySequence;                       //アッセイシーケンス
            this.ItemProtoParam.preProcessSeq = (Int32)protocol.PreProcessSequence;             //前処理シーケンス
            this.ItemProtoParam.ReagCode = protocol.ReagentCode;                                //試薬コード
            this.ItemProtoParam.SmpDispenseVolume = protocol.SmpDispenseVolume;                 //サンプル分注量
            this.ItemProtoParam.MReagDispenseVolume = protocol.MReagDispenseVolume;             //M試薬分注量
            this.ItemProtoParam.R1DispenseVolume = protocol.R1DispenseVolume;                   //R1分注量 
            this.ItemProtoParam.R2DispenseVolume = protocol.R2DispenseVolume;                   //R2分注量
            this.ItemProtoParam.PreProsess1DispenseVolume = protocol.PreProsess1DispenseVolume; //前処理液1分注量
            this.ItemProtoParam.PreProsess2DispenseVolume = protocol.PreProsess2DispenseVolume; //前処理液2分注量
            this.ItemProtoParam.DayOfReagentValid = protocol.DayOfReagentValid;
            this.ItemProtoParam.DiluCalibOrControl = protocol.DiluCalibOrControl;               //校准品是否稀释的控制
            this.ItemProtoParam.IsTB_IGRA = Convert.ToByte(protocol.IsIGRA);                    //判断项目是否是TB-IGRA
            this.ItemProtoParam.AnalysisMode = Convert.ToByte(protocol.UseEmergencyMode);       //急診有無

            // 急診使用有の場合
            if (protocol.UseEmergencyMode == true)
            {
                // プロトコル希釈倍率を1に固定する
                this.ItemProtoParam.ProtocolDilutionRatio = 1;
            }
            // 急診使用無の場合
            else
            {
                //プロトコル希釈倍率
                this.ItemProtoParam.ProtocolDilutionRatio = (Int32)protocol.ProtocolDilutionRatio;  
            }

            this.ItemProtoParam.ReverseDispensingOrderR1 = Convert.ToByte(protocol.ReverseDispensingOrderR1);     //R1ユニットの分注順逆転
        }

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 3));
            resultList.Add(text_data.spoilInt(out this.ItemProtoParam.AssaySeq, 1));
            resultList.Add(text_data.spoilInt(out this.ItemProtoParam.preProcessSeq, 1));
            resultList.Add(text_data.spoilInt(out this.ItemProtoParam.ReagCode, 3));
            resultList.Add(text_data.spoilInt(out this.ItemProtoParam.SmpDispenseVolume, 4));
            resultList.Add(text_data.spoilInt(out this.ItemProtoParam.MReagDispenseVolume, 4));
            resultList.Add(text_data.spoilInt(out this.ItemProtoParam.R1DispenseVolume, 4));
            resultList.Add(text_data.spoilInt(out this.ItemProtoParam.R2DispenseVolume, 4));
            resultList.Add(text_data.spoilInt(out this.ItemProtoParam.PreProsess1DispenseVolume, 4));
            resultList.Add(text_data.spoilInt(out this.ItemProtoParam.PreProsess2DispenseVolume, 4));
            resultList.Add(text_data.spoilInt(out this.ItemProtoParam.DayOfReagentValid, 4));
            resultList.Add(text_data.spoilInt(out this.ItemProtoParam.DiluCalibOrControl, 1));
            resultList.Add(text_data.spoilByte(out this.ItemProtoParam.IsTB_IGRA, 1));
            resultList.Add(text_data.spoilInt(out this.ItemProtoParam.ProtocolDilutionRatio, 4));
            resultList.Add(text_data.spoilByte(out this.ItemProtoParam.AnalysisMode, 1));
            resultList.Add(text_data.spoilByte(out this.ItemProtoParam.ReverseDispensingOrderR1, 1));
            this.ProtoNo = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }
    /// <summary>
    /// プロトコルパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// プロトコルパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1405 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1405()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1405;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// モーター初期化コマンド
    /// </summary>
    /// <remarks>
    /// モーター初期化コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0406 : RackTransferCommCommand_0006
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0406()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0406;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// モーター初期化コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// モーター初期化コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1406 : RackTransferCommCommand_1006
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1406()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1406;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// モーターセルフチェックコマンド
    /// </summary>
    /// <remarks>
    /// モーターセルフチェックコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0407 : RackTransferCommCommand_0007
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0407()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0407;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// モーターセルフチェックコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// モーターセルフチェックコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1407 : RackTransferCommCommand_1007
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1407()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1407;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 光学系セルフチェックコマンド
    /// </summary>
    /// <remarks>
    /// 光学系セルフチェックコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0408 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0408()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0408;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// 光学系セルフチェックコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 光学系セルフチェックコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1408 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1408()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1408;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ダーク
        /// </summary>
        public Int32 Dark { get; set; } = 0;

        /// <summary>
        /// チェック実施有無
        /// </summary>
        public Boolean ExecuteCheck { get; set; } = true;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.Dark.ToString(), 8);
                builder.Append(this.ExecuteCheck.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            Int32 tmpdata1 = 0;
            Int32 tmpdata2 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 8));
            this.Dark = tmpdata1;

            // 実施有無が正常に取得できた場合
            if( text_data.spoilInt(out tmpdata2, 1) )
            {
                Boolean tmpdata3 = false;
                if (Boolean.TryParse(tmpdata2.ToString(), out tmpdata3))
                {
                    this.ExecuteCheck = tmpdata3;
                }
            }

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// プライムコマンド
    /// </summary>
    /// <remarks>
    /// プライムコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0409 : CarisXCommCommand
    {
        #region [定数定義]
        /// <summary>
        /// プライム識別
        /// </summary>
        public enum ItemPrimeKind
        {
            /// <summary>
            /// 希釈液
            /// </summary>
            Diltred = 1,
            /// <summary>
            /// R1
            /// </summary>
            R1 = 2,
            /// <summary>
            /// R2
            /// </summary>
            R2 = 3,
            /// <summary>
            /// B/F1
            /// </summary>
            BF1 = 4,
            /// <summary>
            /// B/F2
            /// </summary>
            BF2 = 5,
            /// <summary>
            /// プレトリガ
            /// </summary>
            PreTrigger = 6,
            /// <summary>
            /// トリガ
            /// </summary>
            Trigger = 7,
            /// <summary>
            /// 全体
            /// </summary>
            All = 8
        }

        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0409()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0409;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// プライム識別
        /// </summary>
        public ItemPrimeKind ItemPrime { get; set; } = ItemPrimeKind.Diltred;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.ItemPrime).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            this.ItemPrime = (ItemPrimeKind)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion

    }
    /// <summary>
    /// プライムコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// プライムコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1409 : CarisXCommCommand
    {
        #region [定数定義]
        /// <summary>
        /// プライム識別
        /// </summary>
        public enum ItemPrimeKind
        {
            /// <summary>
            /// 希釈液
            /// </summary>
            Diltred = 1,
            /// <summary>
            /// R1
            /// </summary>
            R1 = 2,
            /// <summary>
            /// R2
            /// </summary>
            R2 = 3,
            /// <summary>
            /// B/F1
            /// </summary>
            BF1 = 4,
            /// <summary>
            /// B/F2
            /// </summary>
            BF2 = 5,
            /// <summary>
            /// プレトリガ
            /// </summary>
            PreTrigger = 6,
            /// <summary>
            /// トリガ
            /// </summary>
            Trigger = 7,
            /// <summary>
            /// 全体
            /// </summary>
            All = 8
        }

        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1409()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1409;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// プライム識別
        /// </summary>
        public ItemPrimeKind ItemPrime { get; set; } = ItemPrimeKind.Diltred;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.ItemPrime).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            this.ItemPrime = (ItemPrimeKind)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
        #endregion
    }

    /// <summary>
    /// リンス処理コマンド
    /// </summary>
    /// <remarks>
    /// リンス処理コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0410 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0410()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0410;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// リンス処理コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// リンス処理コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1410 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1410()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1410;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 結果
        /// </summary>
        public Int32 Result { get; set; } = 0;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.Result.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            Result = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// 分析開始コマンド
    /// </summary>
    /// <remarks>
    /// 分析開始コマンドデータクラス。
    /// 生成及びパースを行います。yr
    /// </remarks>
    public class SlaveCommCommand_0411 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0411()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0411;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// コントロール
        /// </summary>
        public CommandControlParameter Control { get; set; } = CommandControlParameter.Start;

        /// <summary>
        /// リンス有無（false:リンス実行なし、true:リンス実行あり）
        /// </summary>
        public Boolean RinseExecution { get; set; } = false;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.Control).ToString(), 1);
                builder.Append(Convert.ToInt32(this.RinseExecution).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if SIMULATOR
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            Byte tmpdata2;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            this.Control = (CommandControlParameter)tmpdata1;
            this.RinseExecution = Convert.ToBoolean(tmpdata2);

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 分析開始コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 分析開始コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1411 : RackTransferCommCommand_1011
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1411()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1411;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// ポーズコマンド
    /// </summary>
    /// <remarks>
    /// ポーズコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0412 : RackTransferCommCommand_0012
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0412()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0412;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// ポーズコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ポーズコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1412 : RackTransferCommCommand_1012
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1412()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1412;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// カレンダー設定コマンド
    /// </summary>
    /// <remarks>
    /// カレンダー設定コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0413 : RackTransferCommCommand_0013
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0413()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0413;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// カレンダー設定コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// カレンダー設定コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1413 : RackTransferCommCommand_1013
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1413()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1413;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 残量チェックコマンド
    /// </summary>
    /// <remarks>
    /// 残量チェックコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0414 : CarisXCommCommand
    {
        #region [定数定義]
        /// <summary>
        /// 容量チェック種別
        /// </summary>
        public enum RetCheckRemainCom
        {
            /// <summary>
            /// 情報のみ
            /// </summary>
            Info = 1,
            /// <summary>
            /// 全チェック
            /// </summary>
            AllCheck = 2
        }

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0414()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0414;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 容量チェック種別
        /// </summary>
        public RetCheckRemainCom KindRemainCheck { get; set; } = RetCheckRemainCom.Info;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.KindRemainCheck).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            this.KindRemainCheck = (RetCheckRemainCom)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 残量チェックコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 残量チェックコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1414 : CarisXCommCommand, IRemainAmountInfoSet
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1414()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1414;
            this.CommandId = (int)this.CommKind;

            for (Int32 i = 0; i < ReagentRemainTable.Length; i++)
            {
                ReagentRemainTable[i] = new ReagentRemainTable();
            }
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬残量テーブル
        /// </summary>
        public ReagentRemainTable[] ReagentRemainTable { get; set; } = new ReagentRemainTable[60];

        /// <summary>
        /// 希釈液残量テーブル
        /// </summary>
        public DilutionRemainTable DilutionRemainTable { get; set; } = new DilutionRemainTable();

        /// <summary>
        /// プレトリガ残量テーブル
        /// </summary>
        public PreTriggerRemainTable PreTriggerRemainTable { get; set; } = new PreTriggerRemainTable();

        /// <summary>
        /// トリガ残量テーブル
        /// </summary>
        public TriggerRemainTable TriggerRemainTable { get; set; } = new TriggerRemainTable();

        /// <summary>
        /// サンプル分注チップ残量テーブル
        /// </summary>
        public SampleTipRemainTable SampleTipRemainTable { get; set; } = new SampleTipRemainTable();

        /// <summary>
        /// 反応容器残量テーブル
        /// </summary>
        public CellRemainTable CellRemainTable { get; set; } = new CellRemainTable();

        /// <summary>
        /// 洗浄液残量
        /// </summary>
        public Int32 WashContainerRemain { get; set; } = 0;

        /// <summary>
        /// リンス液残量
        /// </summary>
        public Int32 RinceContainerRemain { get; set; } = 0;

        /// <summary>
        /// 廃液バッファ満杯フラグ
        /// </summary>
        public Boolean IsFullWasteBuffer { get; set; } = false;

        /// <summary>
        /// 廃棄ボックス有無
        /// </summary>
        public Boolean ExistWasteBox { get; set; } = false;

        /// <summary>
        /// 廃棄ボックス満杯状態
        /// </summary>
		/// <remarks>
		/// 仮にスレーブから定義外の値が指定された場合、使用箇所ではdefaultケース扱いとなる。
		/// </remarks>
        public WasteBoxStatus WasteBoxCondition { get; set; } = WasteBoxStatus.Empty;

        /// <summary>
        /// 取得時刻
        /// </summary>
        public DateTime TimeStamp { get; set; } = DateTime.MinValue;

#if SIMULATOR
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                Int32 i;
                for (i = 0; i < this.ReagentRemainTable.Count(); i++)
                {
                    builder.Append(this.ReagentRemainTable[i].ReagType.ToString(), 1);
                    builder.Append(this.ReagentRemainTable[i].ReagCode.ToString(), 3);
                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.Remain.ToString(), 7);
                    if (!string.IsNullOrEmpty(this.ReagentRemainTable[i].RemainingAmount.LotNumber))
                    {
                        builder.Append(int.Parse(this.ReagentRemainTable[i].RemainingAmount.LotNumber).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.ReagentRemainTable[i].RemainingAmount.LotNumber, 8);
                    }

                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.SerialNumber.ToString(), 5);
                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.TermOfUse.ToString("yyMMdd"), 6);
                    builder.Append(this.ReagentRemainTable[i].MakerCode ?? "  ", 2);
                    builder.Append(this.ReagentRemainTable[i].Capacity.ToString(), 7);
                }

                builder.Append(this.DilutionRemainTable.RemainingAmount.Remain.ToString(), 7);
                if (!string.IsNullOrEmpty(this.DilutionRemainTable.RemainingAmount.LotNumber))
                {
                    builder.Append(int.Parse(this.DilutionRemainTable.RemainingAmount.LotNumber).ToString("00000000"), 8);
                }
                else
                {
                    builder.Append(this.DilutionRemainTable.RemainingAmount.LotNumber, 8);
                }
                builder.Append(this.DilutionRemainTable.RemainingAmount.TermOfUse.ToString("yyMMdd"), 6);

                for (i = 0; i < this.PreTriggerRemainTable.RemainingAmount.Length; i++)
                {
                    builder.Append(this.PreTriggerRemainTable.RemainingAmount[i].Remain.ToString(), 7);
                    if (!string.IsNullOrEmpty(this.PreTriggerRemainTable.RemainingAmount[i].LotNumber))
                    {
                        builder.Append(int.Parse(this.PreTriggerRemainTable.RemainingAmount[i].LotNumber).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.PreTriggerRemainTable.RemainingAmount[i].LotNumber, 8);
                    }
                    builder.Append(this.PreTriggerRemainTable.RemainingAmount[i].TermOfUse.ToString("yyMMdd"), 6);
                }
                builder.Append(this.PreTriggerRemainTable.ActNo.ToString(), 1);

                for (i = 0; i < this.TriggerRemainTable.RemainingAmount.Length; i++)
                {
                    builder.Append(this.TriggerRemainTable.RemainingAmount[i].Remain.ToString(), 7);

                    if (!string.IsNullOrEmpty(this.TriggerRemainTable.RemainingAmount[i].LotNumber))
                    {
                        builder.Append(int.Parse(this.TriggerRemainTable.RemainingAmount[i].LotNumber).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.TriggerRemainTable.RemainingAmount[i].LotNumber, 8);
                    }

                    builder.Append(this.TriggerRemainTable.RemainingAmount[i].TermOfUse.ToString("yyMMdd"), 6);
                }
                builder.Append(this.TriggerRemainTable.ActNo.ToString(), 1);

                for (i = 0; i < this.SampleTipRemainTable.tipRemainTable.Count(); i++)
                {
                    builder.Append(this.SampleTipRemainTable.tipRemainTable[i].ToString(), 3);
                }
                builder.Append(this.SampleTipRemainTable.ActNo.ToString(), 1);

                for (i = 0; i < this.CellRemainTable.reactContainerRemainTable.Count(); i++)
                {
                    builder.Append(this.CellRemainTable.reactContainerRemainTable[i].ToString(), 3);
                }
                builder.Append(this.CellRemainTable.ActNo.ToString(), 1);

                builder.Append(this.WashContainerRemain.ToString(), 5);
                builder.Append(this.RinceContainerRemain.ToString(), 5);
                builder.Append(Convert.ToByte(this.IsFullWasteBuffer).ToString(), 1);
                builder.Append(Convert.ToByte(this.ExistWasteBox).ToString(), 1);
                builder.Append(Convert.ToByte(this.WasteBoxCondition).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            this.TimeStamp = DateTime.Now;

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 i = 0;
            String dateMonthStr;

            for (i = 0; i < this.ReagentRemainTable.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].ReagType, 1));
                switch (ReagentRemainTable[i].ReagType)
                {
                    case (Int32)ReagentType.M:      //M試薬
                        ReagentRemainTable[i].ReagTypeDetail = ReagentTypeDetail.M;
                        break;
                    case (Int32)ReagentType.R1R2:   //R1R2試薬
                        ReagentRemainTable[i].ReagTypeDetail = ((i % 3) == 0) ? ReagentTypeDetail.R1 : ReagentTypeDetail.R2;    //1件目はR1、2件目はR2
                        break;
                    case (Int32)ReagentType.T1T2:   //前処理液
                        ReagentRemainTable[i].ReagTypeDetail = ((i % 3) == 0) ? ReagentTypeDetail.T1 : ReagentTypeDetail.T2;    //1件目はT1、2件目はT2
                        break;
                }
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].ReagCode, 3));
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].RemainingAmount.Remain, 7));
                resultList.Add(text_data.spoilString(out ReagentRemainTable[i].RemainingAmount.LotNumber, 8));
                //TODO：CarisXで対応必須
                //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo 
                //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                //{
                //    if (!String.IsNullOrEmpty(ReagentRemainTable[i].RemainingAmount.LotNumber))
                //    {
                //        int nReagentLot = 0;
                //        if (int.TryParse(ReagentRemainTable[i].RemainingAmount.LotNumber, out nReagentLot))
                //        {
                //            ReagentRemainTable[i].RemainingAmount.LotNumber = nReagentLot.ToString();
                //        }
                //    }
                //}
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].RemainingAmount.SerialNumber, 5));

                resultList.Add(text_data.spoilString(out dateMonthStr, 12));
                if (!String.IsNullOrWhiteSpace(ReagentRemainTable [i].RemainingAmount.LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDDHHMMSS(dateMonthStr, out ReagentRemainTable [i].RemainingAmount.InstallationData));
                }

                resultList.Add(text_data.spoilString(out dateMonthStr, 6));
                if (!String.IsNullOrWhiteSpace(ReagentRemainTable[i].RemainingAmount.LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out ReagentRemainTable[i].RemainingAmount.TermOfUse));
                }
                resultList.Add(text_data.spoilString(out ReagentRemainTable[i].MakerCode, 2));
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].Capacity, 7));
            }

            resultList.Add(text_data.spoilInt(out DilutionRemainTable.RemainingAmount.Remain, 7));
            resultList.Add(text_data.spoilString(out DilutionRemainTable.RemainingAmount.LotNumber, 8));
            //TODO：CarisXで対応必須
            //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo 
            //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
            //{
            //    if (!String.IsNullOrEmpty(DilutionRemainTable.RemainingAmount.LotNumber))
            //    {
            //        int nReagentLot = 0;
            //        if (int.TryParse(DilutionRemainTable.RemainingAmount.LotNumber, out nReagentLot))
            //        {
            //            DilutionRemainTable.RemainingAmount.LotNumber = nReagentLot.ToString();
            //        }
            //    }
            //}
            resultList.Add(text_data.spoilString(out dateMonthStr, 6));

            if (!String.IsNullOrWhiteSpace(DilutionRemainTable.RemainingAmount.LotNumber))
            {
                resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out DilutionRemainTable.RemainingAmount.TermOfUse));
            }

            for (i = 0; i < this.PreTriggerRemainTable.RemainingAmount.Length; i++)
            {
                resultList.Add(text_data.spoilInt(out PreTriggerRemainTable.RemainingAmount[i].Remain, 7));
                resultList.Add(text_data.spoilString(out PreTriggerRemainTable.RemainingAmount[i].LotNumber, 8));
                //TODO：CarisXで対応必須
                //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                //{
                //    if (!String.IsNullOrEmpty(PreTriggerRemainTable.RemainingAmount[i].LotNumber))
                //    {
                //        int nReagentLot = 0;
                //        if (int.TryParse(PreTriggerRemainTable.RemainingAmount[i].LotNumber, out nReagentLot))
                //        {
                //            PreTriggerRemainTable.RemainingAmount[i].LotNumber = nReagentLot.ToString();
                //        }
                //    }
                //}
                resultList.Add(text_data.spoilString(out dateMonthStr, 6));
                if (!String.IsNullOrWhiteSpace(PreTriggerRemainTable.RemainingAmount[i].LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out PreTriggerRemainTable.RemainingAmount[i].TermOfUse));
                }
            }
            resultList.Add(text_data.spoilInt(out PreTriggerRemainTable.ActNo, 1));

            for (i = 0; i < this.PreTriggerRemainTable.RemainingAmount.Length; i++)
            {
                resultList.Add(text_data.spoilInt(out TriggerRemainTable.RemainingAmount[i].Remain, 7));
                resultList.Add(text_data.spoilString(out TriggerRemainTable.RemainingAmount[i].LotNumber, 8));
                //TODO：CarisXで対応必須
                //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                //{
                //    if (!String.IsNullOrEmpty(TriggerRemainTable.RemainingAmount[i].LotNumber))
                //    {
                //        int nReagentLot = 0;
                //        if (int.TryParse(TriggerRemainTable.RemainingAmount[i].LotNumber, out nReagentLot))
                //        {
                //            TriggerRemainTable.RemainingAmount[i].LotNumber = nReagentLot.ToString();
                //        }
                //    }
                //}
                resultList.Add(text_data.spoilString(out dateMonthStr, 6));
                if (!String.IsNullOrWhiteSpace(TriggerRemainTable.RemainingAmount[i].LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out TriggerRemainTable.RemainingAmount[i].TermOfUse));
                }
            }
            resultList.Add(text_data.spoilInt(out TriggerRemainTable.ActNo, 1));

            for (i = 0; i < this.SampleTipRemainTable.tipRemainTable.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out SampleTipRemainTable.tipRemainTable[i], 3));
            }
            resultList.Add(text_data.spoilInt(out SampleTipRemainTable.ActNo, 1));

            for (i = 0; i < this.CellRemainTable.reactContainerRemainTable.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out CellRemainTable.reactContainerRemainTable[i], 3));
            }
            resultList.Add(text_data.spoilInt(out CellRemainTable.ActNo, 1));

            Int32 tmpdata1 = 0;
            Int32 tmpdata2 = 0;
            Byte tmpdata3 = 0;
            Byte tmpdata4 = 0;
            Byte tmpdata5 = 0;

            resultList.Add(text_data.spoilInt(out tmpdata1, 5));
            resultList.Add(text_data.spoilInt(out tmpdata2, 5));
            resultList.Add(text_data.spoilByte(out tmpdata3, 1));
            resultList.Add(text_data.spoilByte(out tmpdata4, 1));
            resultList.Add(text_data.spoilByte(out tmpdata5, 1));

            this.WashContainerRemain = tmpdata1;
            this.RinceContainerRemain = tmpdata2;
            this.IsFullWasteBuffer = Convert.ToBoolean(tmpdata3);
            this.ExistWasteBox = Convert.ToBoolean(tmpdata4);
            this.WasteBoxCondition = (WasteBoxStatus)tmpdata5;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// 試薬準備確認コマンド
    /// </summary>
    /// <remarks>
    /// 試薬準備確認コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0415 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0415()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0415;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// 試薬準備確認コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬準備確認コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1415 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1415()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1415;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 試薬準備開始コマンド
    /// </summary>
    /// <remarks>
    /// 試薬準備開始コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0416 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0416()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0416;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬 準備テーブル 
        /// </summary>
        public Byte[] PrepareFlag { get; set; } = new Byte[20];

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                for (Int32 i = 0; i < this.PrepareFlag.Count(); i++)
                {
                    builder.Append(this.PrepareFlag[i].ToString(), 1);
                }

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            for (Int32 i = 0; i < this.PrepareFlag.Count(); i++)
            {
                resultList.Add(text_data.spoilByte(out PrepareFlag[i], 1));
            }

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 試薬準備開始コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬準備開始コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1416 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1416()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1416;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 試薬準備完了コマンド
    /// </summary>
    /// <remarks>
    /// 試薬準備完了コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0417 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0417()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0417;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// 試薬準備完了コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬準備完了コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1417 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1417()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1417;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬 準備テーブル 
        /// </summary>
        public ReagentPreparationErrorTarget[] PrepareResultFlag { get; set; } = new ReagentPreparationErrorTarget[20];

#if SIMULATOR
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                for (Int32 i = 0; i < this.PrepareResultFlag.Count(); i++)
                {
                    builder.Append(((int)this.PrepareResultFlag[i]).ToString(), 1);
                }

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1 = 0;

            for (Int32 i = 0; i < this.PrepareResultFlag.Count(); i++)
            {
                resultList.Add(text_data.spoilByte(out tmpdata1, 1));
                PrepareResultFlag[i] = (ReagentPreparationErrorTarget)tmpdata1;
            }

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
        #endregion
    }

    /// <summary>
    /// 希釈液準備確認コマンド
    /// </summary>
    /// <remarks>
    /// 希釈液準備確認コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0418 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0418()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0418;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// 希釈液準備確認コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 希釈液準備確認コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1418 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1418()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1418;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 希釈液準備開始コマンド
    /// </summary>
    /// <remarks>
    /// 希釈液準備開始コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0419 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0419()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0419;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// 希釈液準備開始コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 希釈液準備開始コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1419 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1419()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1419;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 希釈液準備完了コマンド
    /// </summary>
    /// <remarks>
    /// 希釈液準備完了コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0420 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0420()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0420;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 残量
        /// </summary>
        public Int32 Remain { get; set; } = 0;

        /// <summary>
        /// ロット番号
        /// </summary>
        public String LotNumber { get; set; } = "";

        /// <summary>
        /// 有効期限
        /// </summary>
        public DateTime TermOfUse { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.Remain.ToString(), 7);
                builder.Append(this.LotNumber.ToString(), 8);
                builder.Append(this.TermOfUse.ToString("yyMMdd"), 6);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            String tmpdata2 = "";
            String tmpdata3 = "";
            DateTime tmpDateTime;

            resultList.Add(text_data.spoilInt(out tmpdata1, 7));
            resultList.Add(text_data.spoilString(out tmpdata2, 8));
            resultList.Add(text_data.spoilString(out tmpdata3, 6));
            resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(tmpdata3, out tmpDateTime));

            this.Remain = tmpdata1;
            this.LotNumber = tmpdata2;
            this.TermOfUse = tmpDateTime;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 希釈液準備完了コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 希釈液準備完了コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1420 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1420()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1420;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// プレトリガ準備開始コマンド
    /// </summary>
    /// <remarks>
    /// プレトリガ準備開始コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0421 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0421()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0421;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]
        /// <summary>
        /// ボトル番号
        /// </summary>
        public Int32 BottleNo { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.BottleNo.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            this.BottleNo = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// プレトリガ準備開始コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// プレトリガ準備開始コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1421 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1421()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1421;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// プレトリガ準備完了コマンド
    /// </summary>
    /// <remarks>
    /// プレトリガ準備完了コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0422 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0422()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0422;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ボトル番号
        /// </summary>
        public Int32 BottleNo { get; set; } = 0;

        /// <summary>
        /// 残量
        /// </summary>
        public Int32 Remain { get; set; } = 0;

        /// <summary>
        /// ロット番号
        /// </summary>
        public String LotNumber { get; set; } = "";

        /// <summary>
        /// 有効期限
        /// </summary>
        public DateTime TermOfUse { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.BottleNo.ToString(), 1);
                builder.Append(this.Remain.ToString(), 7);

                if (!string.IsNullOrEmpty(this.LotNumber))
                {
                    builder.Append((int.Parse(this.LotNumber)).ToString("00000000"), 8);
                }
                else
                {
                    builder.Append(this.LotNumber, 8);
                }

                builder.Append(this.TermOfUse.ToString("yyMMdd"), 6);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            Int32 tmpdata2;
            String tmpdata3;
            String tmpdata4;

            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilInt(out tmpdata2, 7));
            resultList.Add(text_data.spoilString(out tmpdata3, 8));
            resultList.Add(text_data.spoilString(out tmpdata4, 6));

            this.BottleNo = tmpdata1;
            this.Remain = tmpdata2;
            this.LotNumber = tmpdata3;

            if (Singleton<Oelco.Common.Parameter.ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
            {
                if (!String.IsNullOrEmpty(this.LotNumber))
                {
                    int nReagentLot = 0;
                    if (int.TryParse(this.LotNumber, out nReagentLot))
                    {
                        this.LotNumber = nReagentLot.ToString();
                    }
                }
            }

            if (!String.IsNullOrWhiteSpace(this.LotNumber))
            {
                DateTime tmpDateTime;
                resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(tmpdata4, out tmpDateTime));
                this.TermOfUse = tmpDateTime;
            }

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// プレトリガ準備完了コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// プレトリガ準備完了コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1422 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1422()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1422;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// トリガ準備開始コマンド
    /// </summary>
    /// <remarks>
    /// トリガ準備開始コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0423 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0423()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0423;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]
        /// <summary>
        /// ボトル番号
        /// </summary>
        public Int32 BottleNo { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.BottleNo.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            this.BottleNo = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// トリガ準備開始コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// トリガ準備開始コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1423 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1423()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1423;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// トリガ準備完了コマンド
    /// </summary>
    /// <remarks>
    /// トリガ準備完了コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0424 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0424()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0424;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ボトル番号
        /// </summary>
        public Int32 BottleNo { get; set; } = 0;

        /// <summary>
        /// 残量
        /// </summary>
        public Int32 Remain { get; set; } = 0;

        /// <summary>
        /// ロット番号
        /// </summary>
        public String LotNumber { get; set; } = "";

        /// <summary>
        /// 有効期限
        /// </summary>
        public DateTime TermOfUse { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.BottleNo.ToString(), 1);
                builder.Append(this.Remain.ToString(), 7);

                if (!string.IsNullOrEmpty(this.LotNumber))
                {
                    builder.Append((int.Parse(this.LotNumber)).ToString("00000000"), 8);
                }
                else
                {
                    builder.Append(this.LotNumber, 8);
                }

                builder.Append(this.TermOfUse.ToString("yyMMdd"), 6);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            Int32 tmpdata2;
            String tmpdata3;
            String tmpdata4;
            DateTime tmpDateTime;

            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilInt(out tmpdata2, 7));
            resultList.Add(text_data.spoilString(out tmpdata3, 8));
            resultList.Add(text_data.spoilString(out tmpdata4, 6));
            resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(tmpdata4, out tmpDateTime));

            this.BottleNo = tmpdata1;
            this.Remain = tmpdata2;
            this.LotNumber = tmpdata3;
            this.TermOfUse = tmpDateTime;

            if (!String.IsNullOrEmpty(this.LotNumber))
            {
                int nReagentLot = 0;
                if (int.TryParse(this.LotNumber, out nReagentLot))
                {
                    this.LotNumber = nReagentLot.ToString();
                }
            }

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// トリガ準備完了コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// トリガ準備完了コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1424 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1424()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1424;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// ケース(反応容器・サンプル分注チップ)準備開始コマンド
    /// </summary>
    /// <remarks>
    /// ケース(反応容器・サンプル分注チップ)準備開始コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0425 : CarisXCommCommand
    {
        /// <summary>
        /// 交換対象ON
        /// </summary>
        public const Byte EXCHANGE_ON = 1;

        /// <summary>
        /// 交換対象OFF
        /// </summary>
        public const Byte EXCHANGE_OFF = 0;

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0425()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0425;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]
        /// <summary>
        /// 準備フラグ
        /// </summary>
        public Byte[] BottleNo { get; set; } = new Byte[8];
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                for (Int32 i = 0; i < this.BottleNo.Count(); i++)
                {
                    builder.Append(this.BottleNo[i].ToString(), 1);
                }
                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            for (Int32 i = 0; i < this.BottleNo.Count(); i++)
            {
                resultList.Add(text_data.spoilByte(out BottleNo[i], 1));
            }

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// ケース(反応容器・サンプル分注チップ)準備開始コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ケース(反応容器・サンプル分注チップ)準備開始コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1425 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1425()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1425;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// ケース(反応容器・サンプル分注チップ)準備完了コマンド
    /// </summary>
    /// <remarks>
    /// ケース(反応容器・サンプル分注チップ)準備完了コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0426 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0426()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0426;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// ケース(反応容器・サンプル分注チップ)準備完了コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ケース(反応容器・サンプル分注チップ)準備完了コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1426 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1426()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1426;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 試薬残量変更確認コマンド
    /// </summary>
    /// <remarks>
    /// 試薬残量変更確認コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0427 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0427()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0427;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// 試薬残量変更確認コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬残量変更確認コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1427 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1427()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1427;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 試薬残量変更終了コマンド
    /// </summary>
    /// <remarks>
    /// 試薬残量変更終了コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0428 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0428()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0428;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// 試薬残量変更終了コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬残量変更終了コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1428 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1428()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1428;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 試薬残量変更コマンド
    /// </summary>
    /// <remarks>
    /// 試薬残量変更コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0429 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0429()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0429;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ポート番号
        /// </summary>
        public Int32 PortNumber { get; set; } = 0;

        /// <summary>
        /// ロット番号
        /// </summary>
        public String LotNumber { get; set; } = "";

        /// <summary>
        /// 残量
        /// </summary>
        public Int32 Remain { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.PortNumber.ToString(), 2);
                if (!string.IsNullOrEmpty(this.LotNumber))
                {
                    builder.Append((int.Parse(this.LotNumber)).ToString("00000000"), 8);
                }
                else
                {
                    builder.Append(this.LotNumber, 8);
                }
                builder.Append(this.Remain.ToString(), 7);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            String tmpdata2;
            Int32 tmpdata3;

            resultList.Add(text_data.spoilInt(out tmpdata1, 2));
            resultList.Add(text_data.spoilString(out tmpdata2, 8));
            resultList.Add(text_data.spoilInt(out tmpdata3, 7));

            this.PortNumber = tmpdata1;
            this.LotNumber = tmpdata2;
            this.Remain = tmpdata3;

            if (Singleton<Oelco.Common.Parameter.ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
            {
                if (!String.IsNullOrEmpty(this.LotNumber))
                {
                    int nReagentLot = 0;
                    if (int.TryParse(this.LotNumber, out nReagentLot))
                    {
                        this.LotNumber = nReagentLot.ToString();
                    }
                }
            }

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 試薬残量変更コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬残量変更コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1429 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1429()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1429;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 希釈液残量変更コマンド
    /// </summary>
    /// <remarks>
    /// 希釈液残量変更コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0430 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0430()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0430;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ロット番号
        /// </summary>
        public String LotNumber { get; set; } = "";

        /// <summary>
        /// 残量
        /// </summary>
        public Int32 Remain { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.LotNumber, 8);
                builder.Append(this.Remain.ToString(), 7);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            String tmpdata1;
            Int32 tmpdata2;

            resultList.Add(text_data.spoilString(out tmpdata1, 8));
            resultList.Add(text_data.spoilInt(out tmpdata2, 7));

            this.LotNumber = tmpdata1;
            this.Remain = tmpdata2;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 希釈液残量変更コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 希釈液残量変更コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1430 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1430()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1430;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// プレトリガ残量変更コマンド
    /// </summary>
    /// <remarks>
    /// プレトリガ残量変更コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0431 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0431()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0431;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ポート番号
        /// </summary>
        public Int32 PortNumber { get; set; } = 0;

        /// <summary>
        /// ロット番号
        /// </summary>
        public String LotNumber { get; set; } = "";

        /// <summary>
        /// 残量
        /// </summary>
        public Int32 Remain { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.PortNumber.ToString(), 1);
                builder.Append(this.LotNumber, 8);
                builder.Append(this.Remain.ToString(), 7);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            String tmpdata2;
            Int32 tmpdata3;

            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilString(out tmpdata2, 8));
            resultList.Add(text_data.spoilInt(out tmpdata3, 7));

            this.PortNumber = tmpdata1;
            this.LotNumber = tmpdata2;
            this.Remain = tmpdata3;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// プレトリガ残量変更コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// プレトリガ残量変更コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1431 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1431()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1431;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// トリガ残量変更コマンド
    /// </summary>
    /// <remarks>
    /// トリガ残量変更コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0432 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0432()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0432;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ポート番号
        /// </summary>
        public Int32 PortNumber { get; set; } = 0;

        /// <summary>
        /// ロット番号
        /// </summary>
        public String LotNumber { get; set; } = "";

        /// <summary>
        /// 残量
        /// </summary>
        public Int32 Remain { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.PortNumber.ToString(), 1);
                builder.Append(this.LotNumber, 8);
                builder.Append(this.Remain.ToString(), 7);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            String tmpdata2;
            Int32 tmpdata3;

            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilString(out tmpdata2, 8));
            resultList.Add(text_data.spoilInt(out tmpdata3, 7));

            this.PortNumber = tmpdata1;
            this.LotNumber = tmpdata2;
            this.Remain = tmpdata3;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// トリガ残量変更コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// トリガ残量変更コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1432 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1432()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1432;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// ケース(反応容器・サンプル分注チップ)残量変更コマンド
    /// </summary>
    /// <remarks>
    /// ケース(反応容器・サンプル分注チップ)残量変更コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0433 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0433()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0433;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ポート番号
        /// </summary>
        public Int32 PortNumber { get; set; } = 0;

        /// <summary>
        /// 残量
        /// </summary>
        public Int32 Remain { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.PortNumber.ToString(), 2);
                builder.Append(this.Remain.ToString(), 3);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            Int32 tmpdata2;

            resultList.Add(text_data.spoilInt(out tmpdata1, 2));
            resultList.Add(text_data.spoilInt(out tmpdata2, 3));

            this.PortNumber = tmpdata1;
            this.Remain = tmpdata2;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// ケース(反応容器・サンプル分注チップ)残量変更コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ケース(反応容器・サンプル分注チップ)残量変更コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1433 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1433()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1433;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 残量セットコマンド
    /// </summary>
    /// <remarks>
    /// 残量セットコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0434 : CarisXCommCommand, IRemainAmountInfoSet
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0434()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0434;
            this.CommandId = (int)this.CommKind;

            for (Int32 i = 0; i < ReagentRemainTable.Length; i++)
            {
                ReagentRemainTable[i] = new ReagentRemainTable();
            }
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬残量テーブル
        /// </summary>
        public ReagentRemainTable[] ReagentRemainTable { get; set; } = new ReagentRemainTable[60];

        /// <summary>
        /// 希釈液残量テーブル
        /// </summary>
        public DilutionRemainTable DilutionRemainTable { get; set; } = new DilutionRemainTable();

        /// <summary>
        /// プレトリガ残量テーブル
        /// </summary>
        public PreTriggerRemainTable PreTriggerRemainTable { get; set; } = new PreTriggerRemainTable();

        /// <summary>
        /// トリガ残量テーブル
        /// </summary>
        public TriggerRemainTable TriggerRemainTable { get; set; } = new TriggerRemainTable();

        /// <summary>
        /// サンプル分注チップ残量テーブル
        /// </summary>
        public SampleTipRemainTable SampleTipRemainTable { get; set; } = new SampleTipRemainTable();

        /// <summary>
        /// セル容器残量テーブル
        /// </summary>
        public CellRemainTable CellRemainTable { get; set; } = new CellRemainTable();

        /// <summary>
        /// 洗浄液残量
        /// </summary>
        public Int32 WashContainerRemain { get; set; } = 0;

        /// <summary>
        /// リンス液残量
        /// </summary>
        public Int32 RinceContainerRemain { get; set; } = 0;

        /// <summary>
        /// 廃液バッファ満杯フラグ
        /// </summary>
        public Boolean IsFullWasteBuffer { get; set; } = false;

        /// <summary>
        /// 廃棄ボックス有無
        /// </summary>
        public Boolean ExistWasteBox { get; set; } = false;

        /// <summary>
        /// 廃棄ボックス満杯状態
        /// </summary>
        /// <remarks>
        /// 仮にスレーブから定義外の値が指定された場合、使用箇所ではdefaultケース扱いとなる。
        /// </remarks>
        public WasteBoxStatus WasteBoxCondition { get; set; } = WasteBoxStatus.Empty;

        /// <summary>
        /// 取得時刻
        /// </summary>
        public DateTime TimeStamp { get; set; } = DateTime.MinValue;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                Int32 i;
                for (i = 0; i < this.ReagentRemainTable.Count(); i++)
                {
                    builder.Append(this.ReagentRemainTable[i].ReagType.ToString(), 1);
                    builder.Append(this.ReagentRemainTable[i].ReagCode.ToString(), 3);
                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.Remain.ToString(), 7);
                    if (!string.IsNullOrEmpty(this.ReagentRemainTable[i].RemainingAmount.LotNumber))
                    {
                        builder.Append(int.Parse(this.ReagentRemainTable[i].RemainingAmount.LotNumber).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.ReagentRemainTable[i].RemainingAmount.LotNumber, 8);
                    }

                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.SerialNumber.ToString(), 5);
                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.InstallationData.ToString("yyMMddHHmmss"), 12);
                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.TermOfUse.ToString("yyMMdd"), 6);
                    builder.Append(this.ReagentRemainTable[i].MakerCode ?? "  ", 2);
                    builder.Append(this.ReagentRemainTable[i].Capacity.ToString(), 7);
                }

                builder.Append(this.DilutionRemainTable.RemainingAmount.Remain.ToString(), 7);
                if (!string.IsNullOrEmpty(this.DilutionRemainTable.RemainingAmount.LotNumber))
                {
                    builder.Append(int.Parse(this.DilutionRemainTable.RemainingAmount.LotNumber).ToString("00000000"), 8);
                }
                else
                {
                    builder.Append(this.DilutionRemainTable.RemainingAmount.LotNumber, 8);
                }
                builder.Append(this.DilutionRemainTable.RemainingAmount.TermOfUse.ToString("yyMMdd"), 6);

                for (i = 0; i < this.PreTriggerRemainTable.RemainingAmount.Length; i++)
                {
                    builder.Append(this.PreTriggerRemainTable.RemainingAmount[i].Remain.ToString(), 7);
                    if (!string.IsNullOrEmpty(this.PreTriggerRemainTable.RemainingAmount[i].LotNumber))
                    {
                        builder.Append(int.Parse(this.PreTriggerRemainTable.RemainingAmount[i].LotNumber).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.PreTriggerRemainTable.RemainingAmount[i].LotNumber, 8);
                    }
                    builder.Append(this.PreTriggerRemainTable.RemainingAmount[i].TermOfUse.ToString("yyMMdd"), 6);
                }
                builder.Append(this.PreTriggerRemainTable.ActNo.ToString(), 1);

                for (i = 0; i < this.TriggerRemainTable.RemainingAmount.Length; i++)
                {
                    builder.Append(this.TriggerRemainTable.RemainingAmount[i].Remain.ToString(), 7);

                    if (!string.IsNullOrEmpty(this.TriggerRemainTable.RemainingAmount[i].LotNumber))
                    {
                        builder.Append(int.Parse(this.TriggerRemainTable.RemainingAmount[i].LotNumber).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.TriggerRemainTable.RemainingAmount[i].LotNumber, 8);
                    }

                    builder.Append(this.TriggerRemainTable.RemainingAmount[i].TermOfUse.ToString("yyMMdd"), 6);
                }
                builder.Append(this.TriggerRemainTable.ActNo.ToString(), 1);

                for (i = 0; i < this.SampleTipRemainTable.tipRemainTable.Count(); i++)
                {
                    builder.Append(this.SampleTipRemainTable.tipRemainTable[i].ToString(), 3);
                }
                builder.Append(this.SampleTipRemainTable.ActNo.ToString(), 1);

                for (i = 0; i < this.CellRemainTable.reactContainerRemainTable.Count(); i++)
                {
                    builder.Append(this.CellRemainTable.reactContainerRemainTable[i].ToString(), 3);
                }
                builder.Append(this.CellRemainTable.ActNo.ToString(), 1);

                builder.Append(this.WashContainerRemain.ToString(), 5);
                builder.Append(this.RinceContainerRemain.ToString(), 5);
                builder.Append(Convert.ToByte(this.IsFullWasteBuffer).ToString(), 1);
                builder.Append(Convert.ToByte(this.ExistWasteBox).ToString(), 1);
                builder.Append(Convert.ToByte(this.WasteBoxCondition).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if SIMULATOR
        /// <summary>
        /// 残量チェックコマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// 残量チェックコマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            this.TimeStamp = DateTime.Now;

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 i = 0;
            String dateMonthStr;

            for (i = 0; i < this.ReagentRemainTable.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].ReagType, 1));
                switch (ReagentRemainTable[i].ReagType)
                {
                    case (Int32)ReagentType.M:      //M試薬
                        ReagentRemainTable[i].ReagTypeDetail = ReagentTypeDetail.M;
                        break;
                    case (Int32)ReagentType.R1R2:   //R1R2試薬
                        ReagentRemainTable[i].ReagTypeDetail = ((i % 3) == 0) ? ReagentTypeDetail.R1 : ReagentTypeDetail.R2;    //1件目はR1、2件目はR2
                        break;
                    case (Int32)ReagentType.T1T2:   //前処理液
                        ReagentRemainTable[i].ReagTypeDetail = ((i % 3) == 0) ? ReagentTypeDetail.T1 : ReagentTypeDetail.T2;    //1件目はT1、2件目はT2
                        break;
                }
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].ReagCode, 3));//update for enlarge Protocols
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].RemainingAmount.Remain, 7));
                resultList.Add(text_data.spoilString(out ReagentRemainTable[i].RemainingAmount.LotNumber, 8));
                //TODO：CarisXで対応必須
                //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                //{
                //    if (!String.IsNullOrEmpty(ReagentRemainTable[i].RemainingAmount.LotNumber))
                //    {
                //        int nReagentLot = 0;
                //        if (int.TryParse(ReagentRemainTable[i].RemainingAmount.LotNumber, out nReagentLot))
                //        {
                //            ReagentRemainTable[i].RemainingAmount.LotNumber = nReagentLot.ToString();
                //        }
                //    }
                //}

                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].RemainingAmount.SerialNumber, 5));

                resultList.Add(text_data.spoilString(out dateMonthStr, 6));

                if (!String.IsNullOrWhiteSpace(ReagentRemainTable[i].RemainingAmount.LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out ReagentRemainTable[i].RemainingAmount.TermOfUse));
                }

                resultList.Add(text_data.spoilString(out ReagentRemainTable[i].MakerCode, 2));
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].Capacity, 7));
            }


            resultList.Add(text_data.spoilInt(out DilutionRemainTable.RemainingAmount.Remain, 7));
            resultList.Add(text_data.spoilString(out DilutionRemainTable.RemainingAmount.LotNumber, 8));
            //TODO：CarisXで対応必須
            //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
            //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
            //{
            //    if (!String.IsNullOrEmpty(DilutionRemainTable.RemainingAmount.LotNumber))
            //    {
            //        int nReagentLot = 0;
            //        if (int.TryParse(DilutionRemainTable.RemainingAmount.LotNumber, out nReagentLot))
            //        {
            //            DilutionRemainTable.RemainingAmount.LotNumber = nReagentLot.ToString();
            //        }
            //    }
            //}
            resultList.Add(text_data.spoilString(out dateMonthStr, 6));

            if (!String.IsNullOrWhiteSpace(DilutionRemainTable.RemainingAmount.LotNumber))
            {
                resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out DilutionRemainTable.RemainingAmount.TermOfUse));
            }

            for (i = 0; i < this.PreTriggerRemainTable.RemainingAmount.Length; i++)
            {
                resultList.Add(text_data.spoilInt(out PreTriggerRemainTable.RemainingAmount[i].Remain, 7));
                resultList.Add(text_data.spoilString(out PreTriggerRemainTable.RemainingAmount[i].LotNumber, 8));
                //TODO：CarisXで対応必須
                //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                //{
                //    if (!String.IsNullOrEmpty(PreTriggerRemainTable.RemainingAmount[i].LotNumber))
                //    {
                //        int nReagentLot = 0;
                //        if (int.TryParse(PreTriggerRemainTable.RemainingAmount[i].LotNumber, out nReagentLot))
                //        {
                //            PreTriggerRemainTable.RemainingAmount[i].LotNumber = nReagentLot.ToString();
                //        }
                //    }
                //}
                resultList.Add(text_data.spoilString(out dateMonthStr, 6));
                if (!String.IsNullOrWhiteSpace(PreTriggerRemainTable.RemainingAmount[i].LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out PreTriggerRemainTable.RemainingAmount[i].TermOfUse));
                }
            }
            resultList.Add(text_data.spoilInt(out PreTriggerRemainTable.ActNo, 1));

            for (i = 0; i < this.PreTriggerRemainTable.RemainingAmount.Length; i++)
            {
                resultList.Add(text_data.spoilInt(out TriggerRemainTable.RemainingAmount[i].Remain, 7));
                resultList.Add(text_data.spoilString(out TriggerRemainTable.RemainingAmount[i].LotNumber, 8));
                //TODO：CarisXで対応必須
                //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                //{
                //    if (!String.IsNullOrEmpty(TriggerRemainTable.RemainingAmount[i].LotNumber))
                //    {
                //        int nReagentLot = 0;
                //        if (int.TryParse(TriggerRemainTable.RemainingAmount[i].LotNumber, out nReagentLot))
                //        {
                //            TriggerRemainTable.RemainingAmount[i].LotNumber = nReagentLot.ToString();
                //        }
                //    }
                //}
                resultList.Add(text_data.spoilString(out dateMonthStr, 6));
                if (!String.IsNullOrWhiteSpace(TriggerRemainTable.RemainingAmount[i].LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out TriggerRemainTable.RemainingAmount[i].TermOfUse));
                }
            }
            resultList.Add(text_data.spoilInt(out TriggerRemainTable.ActNo, 1));

            for (i = 0; i < this.SampleTipRemainTable.tipRemainTable.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out SampleTipRemainTable.tipRemainTable[i], 3));
            }
            resultList.Add(text_data.spoilInt(out SampleTipRemainTable.ActNo, 1));

            for (i = 0; i < this.CellRemainTable.reactContainerRemainTable.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out CellRemainTable.reactContainerRemainTable[i], 3));
            }
            resultList.Add(text_data.spoilInt(out CellRemainTable.ActNo, 1));

            Int32 tmpdata1 = 0;
            Int32 tmpdata2 = 0;
            Byte tmpdata3 = 0;
            Byte tmpdata4 = 0;
            Byte tmpdata5 = 0;

            resultList.Add(text_data.spoilInt(out tmpdata1, 5));
            resultList.Add(text_data.spoilInt(out tmpdata2, 5));
            resultList.Add(text_data.spoilByte(out tmpdata3, 1));
            resultList.Add(text_data.spoilByte(out tmpdata4, 1));
            resultList.Add(text_data.spoilByte(out tmpdata5, 1));

            this.WashContainerRemain = tmpdata1;
            this.RinceContainerRemain = tmpdata2;
            this.IsFullWasteBuffer = Convert.ToBoolean(tmpdata3);
            this.ExistWasteBox = Convert.ToBoolean(tmpdata4);
            this.WasteBoxCondition = (WasteBoxStatus)tmpdata5;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 残量セットコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 残量セットコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1434 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1434()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1434;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 廃液ボトルセット開始コマンド
    /// </summary>
    /// <remarks>
    /// 廃液ボトルセット開始コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0435 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0435()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0435;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 種別
        /// </summary>
        public TankBufferKind tankBufferKind { get; set; } = TankBufferKind.Tank;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((int)this.tankBufferKind).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if SIMULATOR
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;

            resultList.Add(text_data.spoilByte(out tmpdata1, 1));

            this.tankBufferKind = (TankBufferKind)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 廃液ボトルセット開始コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 廃液ボトルセット開始コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1435 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1435()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1435;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 廃液ボトルセット完了コマンド
    /// </summary>
    /// <remarks>
    /// 廃液ボトルセット完了コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0436 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0436()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0436;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 種別
        /// </summary>
        public TankBufferKind tankBufferKind { get; set; } = TankBufferKind.Tank;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((int)this.tankBufferKind).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if SIMULATOR
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;

            resultList.Add(text_data.spoilByte(out tmpdata1, 1));

            this.tankBufferKind = (TankBufferKind)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 廃液ボトルセット完了コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 廃液ボトルセット完了コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1436 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1436()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1436;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// インキュベーター温度設定問い合わせコマンド
    /// </summary>
    /// <remarks>
    /// インキュベーター温度設定問い合わせコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0437 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0437()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0437;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// コントロール
        /// </summary>
        public CommandControlParameter Ctrl { get; set; } = CommandControlParameter.Abort;

        /// <summary>
        /// 設定温度
        /// </summary>
        public ItemRSIncTemp Temp { get; set; } = new ItemRSIncTemp();

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.Ctrl).ToString(), 1);
                builder.Append(this.Temp.ReactionTableTemp.ToString("f1"), 4);
                builder.Append(this.Temp.BFTableTemp.ToString("f1"), 4);
                builder.Append(this.Temp.BF1PreHeatTemp.ToString("f1"), 4);
                builder.Append(this.Temp.BF2PreHeatTemp.ToString("f1"), 4);
                builder.Append(this.Temp.R1ProbeTemp.ToString("f1"), 4);
                builder.Append(this.Temp.R2ProbeTemp.ToString("f1"), 4);
                builder.Append(this.Temp.ChemiluminesoensePtotometryTemp.ToString("f1"), 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;

            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilDouble(out Temp.ReactionTableTemp, 4));
            resultList.Add(text_data.spoilDouble(out Temp.BFTableTemp, 4));
            resultList.Add(text_data.spoilDouble(out Temp.BF1PreHeatTemp, 4));
            resultList.Add(text_data.spoilDouble(out Temp.BF2PreHeatTemp, 4));
            resultList.Add(text_data.spoilDouble(out Temp.R1ProbeTemp, 4));
            resultList.Add(text_data.spoilDouble(out Temp.R2ProbeTemp, 4));
            resultList.Add(text_data.spoilDouble(out Temp.ChemiluminesoensePtotometryTemp, 4));

            Ctrl = (CommandControlParameter)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }
    /// <summary>
    /// インキュベーター温度設定問い合わせコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// インキュベーター温度設定問い合わせコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1437 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1437()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1437;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// コントロール
        /// </summary>
        public CommandControlParameter Ctrl { get; set; } = CommandControlParameter.Abort;

        /// <summary>
        /// 設定温度
        /// </summary>
        public ItemRSIncTemp Temp { get; set; } = new ItemRSIncTemp();

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.Ctrl).ToString(), 1);
                builder.Append(this.Temp.ReactionTableTemp.ToString("f1"), 4);
                builder.Append(this.Temp.BFTableTemp.ToString("f1"), 4);
                builder.Append(this.Temp.BF1PreHeatTemp.ToString("f1"), 4);
                builder.Append(this.Temp.BF2PreHeatTemp.ToString("f1"), 4);
                builder.Append(this.Temp.R1ProbeTemp.ToString("f1"), 4);
                builder.Append(this.Temp.R2ProbeTemp.ToString("f1"), 4);
                builder.Append(this.Temp.ChemiluminesoensePtotometryTemp.ToString("f1"), 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;

            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilDouble(out Temp.ReactionTableTemp, 4));
            resultList.Add(text_data.spoilDouble(out Temp.BFTableTemp, 4));
            resultList.Add(text_data.spoilDouble(out Temp.BF1PreHeatTemp, 4));
            resultList.Add(text_data.spoilDouble(out Temp.BF2PreHeatTemp, 4));
            resultList.Add(text_data.spoilDouble(out Temp.R1ProbeTemp, 4));
            resultList.Add(text_data.spoilDouble(out Temp.R2ProbeTemp, 4));
            resultList.Add(text_data.spoilDouble(out Temp.ChemiluminesoensePtotometryTemp, 4));

            Ctrl = (CommandControlParameter)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// 試薬保冷庫温度設定問い合わせコマンド
    /// </summary>
    /// <remarks>
    /// 試薬保冷庫温度設定問い合わせコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0438 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0438()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0438;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// コントロール
        /// </summary>
        public CommandControlParameter Ctrl { get; set; } = CommandControlParameter.Ask;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.Ctrl).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;

            resultList.Add(text_data.spoilInt(out tmpdata1, 1));

            Ctrl = (CommandControlParameter)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }
    /// <summary>
    /// 試薬保冷庫温度設定問い合わせコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬保冷庫温度設定問い合わせコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1438 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1438()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1438;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// コントロール
        /// </summary>
        public CommandControlParameter Ctrl { get; set; } = CommandControlParameter.Set;

        /// <summary>
        /// 保冷庫温度
        /// </summary>
        public Double CoolerTemp { get; set; } = 0;

        /// <summary>
        /// 装置外の温度(室温)
        /// </summary>
        public Double RoomTemp { get; set; } = 0;

        /// <summary>
        /// 装置内の温度
        /// </summary>
        public Double AnalyzerTemp { get; set; } = 0;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.Ctrl).ToString(), 1);
                builder.Append(this.CoolerTemp.ToString(), 4);
                builder.Append(this.RoomTemp.ToString(), 4);
                builder.Append(this.AnalyzerTemp.ToString(), 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 試薬保冷庫温度設定問い合わせコマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// 試薬保冷庫温度設定問い合わせコマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            Double tmpdata2;
            Double tmpdata3;
            Double tmpdata4;

            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilDouble(out tmpdata2, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata3, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata4, 4));

            Ctrl = (CommandControlParameter)tmpdata1;
            CoolerTemp = tmpdata2;
            RoomTemp = tmpdata3;
            AnalyzerTemp = tmpdata4;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// ユニットテストコマンド
    /// </summary>
    /// <remarks>
    /// ユニットテストコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0439 : RackTransferCommCommand_0039
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0439()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0439;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// ユニットテストコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ユニットテストコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1439 : RackTransferCommCommand_1039
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1439()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1439;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// センサーステータス問合せコマンド
    /// </summary>
    /// <remarks>
    /// センサーステータス問合せコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0440 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0440()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0440;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// センサーステータス問合せコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// センサーステータス問合せコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1440 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1440()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1440;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// （本体フレーム部）ケース扉検知センサ
        /// </summary>
        public Byte CaseDoorDetective { get; set; } = 0;

        /// <summary>
        /// （本体フレーム部）廃棄ボックス満杯センサ
        /// </summary>
        public Byte DrainBoxFull { get; set; } = 0;

        /// <summary>
        /// （本体フレーム部）廃棄ボックス有無センサ
        /// </summary>
        public Byte UsableDrainBox { get; set; } = 0;

        /// <summary>
        /// （ケース搬送部）ケース搬送有無センサ
        /// </summary>
        public Byte UsableTipCellCaseTransfer { get; set; } = 0;

        /// <summary>
        /// （ケース搬送部）ケース有無センサ
        /// </summary>
        public Byte UsableTipCellCase { get; set; } = 0;

        /// <summary>
        /// （試薬保冷庫）試薬保冷庫カバー検知センサ
        /// </summary>
        public Byte ReagStorageCoverDetective { get; set; } = 0;

        /// <summary>
        /// （試薬保冷庫）R試薬ボトル有無センサ
        /// </summary>
        public Byte UsableRReagBottle { get; set; } = 0;

        /// <summary>
        /// （試薬保冷庫）M試薬ボトル有無センサ
        /// </summary>
        public Byte UsableMReagBottle { get; set; } = 0;

        /// <summary>
        /// （サンプル分注移送部）分注チップキャッチ有無センサ
        /// </summary>
        public Byte UsableDispenceTipCatch { get; set; } = 0;

        /// <summary>
        /// （反応容器搬送部）反応容器キャッチ有無センサ
        /// </summary>
        public Byte UsableReactionCellCatch { get; set; } = 0;

        /// <summary>
        /// （反応テーブル部）反応容器設置確認(外側)センサ
        /// </summary>
        public Byte ReactionCellSettingCheckOuter { get; set; } = 0;

        /// <summary>
        /// （反応テーブル部）反応容器設置確認(内側)センサ
        /// </summary>
        public Byte ReactionCellSettingCheckInner { get; set; } = 0;

        /// <summary>
        /// （反応テーブル部）反応容器設置確認(設置部)センサ
        /// </summary>
        public Byte ReactionCellSettingCheckSettingPosition { get; set; } = 0;

        /// <summary>
        /// （BFテーブル部）反応容器設置確認(BF1)センサ
        /// </summary>
        public Byte ReactionCellSettingCheckBF1 { get; set; } = 0;

        /// <summary>
        /// （BFテーブル部）反応容器設置確認(BF2)センサ
        /// </summary>
        public Byte ReactionCellSettingCheckBF2 { get; set; } = 0;

        /// <summary>
        /// （攪拌部）R1撹拌Zθ確認センサ
        /// </summary>
        public Byte R1MixingZThetaCheck { get; set; } = 0;

        /// <summary>
        /// （攪拌部）R2撹拌Zθ確認センサ
        /// </summary>
        public Byte R2MixingZThetaCheck { get; set; } = 0;

        /// <summary>
        /// （攪拌部）BF1撹拌Zθ確認センサ
        /// </summary>
        public Byte BF1MixingZThetaCheck { get; set; } = 0;

        /// <summary>
        /// （攪拌部）BF2撹拌Zθ確認センサ
        /// </summary>
        public Byte BF2MixingZThetaCheck { get; set; } = 0;

        /// <summary>
        /// （攪拌部）PTr撹拌Zθ確認センサ
        /// </summary>
        public Byte PTrMixingZThetaCheck { get; set; } = 0;

        /// <summary>
        /// （化学発光測定部）測光部シャッターソレノイド確認センサ
        /// </summary>
        public Byte PhotometryShutterSolenoidCheck { get; set; } = 0;

        /// <summary>
        /// （試薬分注1部）R1ノズル衝突検知センサー
        /// </summary>
        public Byte ReagDispense1NozzleClashDetective { get; set; } = 0;

        /// <summary>
        /// （試薬分注2部）R2ノズル衝突検知センサー
        /// </summary>
        public Byte ReagDispense2NozzleClashDetective { get; set; } = 0;

        /// <summary>
        /// （BF1部、BF1廃液部）BF1ノズル1 廃液確認センサ
        /// </summary>
        public Byte BF1Nozzle1DrainCheck { get; set; } = 0;

        /// <summary>
        /// （BF1部、BF1廃液部）BF1ノズル2 廃液確認センサ
        /// </summary>
        public Byte BF1Nozzle2DrainCheck { get; set; } = 0;

        /// <summary>
        /// （BF2部）BF2ノズル1 廃液確認センサ
        /// </summary>
        public Byte BF2Nozzle1DrainCheck { get; set; } = 0;

        /// <summary>
        /// （BF2部）BF2ノズル2 廃液確認センサ
        /// </summary>
        public Byte BF2Nozzle2DrainCheck { get; set; } = 0;

        /// <summary>
        /// （BF2部）BF2ノズル3 廃液確認センサ
        /// </summary>
        public Byte BF2Nozzle3DrainCheck { get; set; } = 0;

        /// <summary>
        /// （流体配管部）洗浄液バッファ有無センサ(残量分検知できる位置へ変更)
        /// </summary>
        public Byte UsableWashBuffer { get; set; } = 0;

        /// <summary>
        /// （流体配管部）洗浄液バッファ満杯センサ
        /// </summary>
        public Byte WashBufferFull { get; set; } = 0;

        /// <summary>
        /// （流体配管部）廃液バッファ満杯センサ(導通基板へ)
        /// </summary>
        public Byte DrainBufferFull { get; set; } = 0;

        /// <summary>
        /// （流体配管部）プレトリガ１液有無センサ
        /// </summary>
        public Byte UsablePreTrigger1 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）プレトリガ２液有無センサ
        /// </summary>
        public Byte UsablePreTrigger2 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）トリガ１液有無センサ
        /// </summary>
        public Byte UsableTrigger1 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）トリガ２液有無センサ
        /// </summary>
        public Byte UsableTrigger2 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）希釈液有無センサ(導通基板へ)
        /// </summary>
        public Byte UsableDiluent { get; set; } = 0;

        /// <summary>
        /// （流体配管部）精製水有無センサ
        /// </summary>
        public Byte UsablePurifiedWater { get; set; } = 0;

        /// <summary>
        /// （流体配管部）洗浄液引込ポンプ用圧力センサ
        /// </summary>
        public Byte PressWashPullInPump { get; set; } = 0;

        /// <summary>
        /// （流体配管部）廃液ポンプ1用圧力センサ
        /// </summary>
        public Byte PressDrainPump1 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）廃液ポンプ2用圧力センサ
        /// </summary>
        public Byte PressDrainPump2 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）廃液ポンプ3用圧力センサ
        /// </summary>
        public Byte PressDrainPump3 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）廃液ポンプ4用圧力センサ
        /// </summary>
        public Byte PressDrainPump4 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）体外廃液ポンプ用圧力センサ
        /// </summary>
        public Byte PressExtracorporealDrainPump { get; set; } = 0;

        /// <summary>
        /// スタット容器有無センサ
        /// </summary>
        public Byte STATTubeCheck { get; set; } = 0;

        /// <summary>
        /// 試薬蓋開閉センサ1
        /// </summary>
        public Byte R1MReagentOpenClose { get; set; } = 0;

        /// <summary>
        /// 試薬蓋開閉センサ2
        /// </summary>
        public Byte R2MReagentOpenClose { get; set; } = 0;

        /// <summary>
        /// 反応容器廃棄確認センサ
        /// </summary>
        public Byte CellDisposeCheck { get; set; } = 0;

        /// <summary>
        /// STAT設置スイッチ
        /// </summary>
        public Byte STATSwitch { get; set; } = 0;

        /// <summary>
        /// 試薬保冷庫回転スイッチ
        /// </summary>
        public Byte ReagentTableTurnSwitch { get; set; } = 0;

        /// <summary>
        /// 検体液面検知センサ基板
        /// </summary>
        public Int32 SampleAspirationData { get; set; } = 0;

#if SIMULATOR
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.CaseDoorDetective.ToString(), 1);
                builder.Append(this.DrainBoxFull.ToString(), 1);
                builder.Append(this.UsableDrainBox.ToString(), 1);
                builder.Append(this.UsableTipCellCaseTransfer.ToString(), 1);
                builder.Append(this.UsableTipCellCase.ToString(), 1);
                builder.Append(this.ReagStorageCoverDetective.ToString(), 1);
                builder.Append(this.UsableRReagBottle.ToString(), 1);
                builder.Append(this.UsableMReagBottle.ToString(), 1);
                builder.Append(this.UsableDispenceTipCatch.ToString(), 1);
                builder.Append(this.UsableReactionCellCatch.ToString(), 1);
                builder.Append(this.ReactionCellSettingCheckOuter.ToString(), 1);
                builder.Append(this.ReactionCellSettingCheckInner.ToString(), 1);
                builder.Append(this.ReactionCellSettingCheckSettingPosition.ToString(), 1);
                builder.Append(this.R1MixingZThetaCheck.ToString(), 1);
                builder.Append(this.ReactionCellSettingCheckBF1.ToString(), 1);
                builder.Append(this.ReactionCellSettingCheckBF2.ToString(), 1);
                builder.Append(this.R2MixingZThetaCheck.ToString(), 1);
                builder.Append(this.BF1MixingZThetaCheck.ToString(), 1);
                builder.Append(this.BF2MixingZThetaCheck.ToString(), 1);
                builder.Append(this.PTrMixingZThetaCheck.ToString(), 1);
                builder.Append(this.PhotometryShutterSolenoidCheck.ToString(), 1);
                builder.Append(this.ReagDispense1NozzleClashDetective.ToString(), 1);
                builder.Append(this.ReagDispense2NozzleClashDetective.ToString(), 1);
                builder.Append(this.BF1Nozzle1DrainCheck.ToString(), 1);
                builder.Append(this.BF1Nozzle2DrainCheck.ToString(), 1);
                builder.Append(this.BF2Nozzle1DrainCheck.ToString(), 1);
                builder.Append(this.BF2Nozzle2DrainCheck.ToString(), 1);
                builder.Append(this.BF2Nozzle3DrainCheck.ToString(), 1);
                builder.Append(this.UsableWashBuffer.ToString(), 1);
                builder.Append(this.WashBufferFull.ToString(), 1);
                builder.Append(this.DrainBufferFull.ToString(), 1);
                builder.Append(this.UsablePreTrigger1.ToString(), 1);
                builder.Append(this.UsablePreTrigger2.ToString(), 1);
                builder.Append(this.UsableTrigger1.ToString(), 1);
                builder.Append(this.UsableTrigger2.ToString(), 1);
                builder.Append(this.UsableDiluent.ToString(), 1);
                builder.Append(this.UsablePurifiedWater.ToString(), 1);
                builder.Append(this.PressWashPullInPump.ToString(), 1);
                builder.Append(this.PressDrainPump1.ToString(), 1);
                builder.Append(this.PressDrainPump2.ToString(), 1);
                builder.Append(this.PressDrainPump3.ToString(), 1);
                builder.Append(this.PressDrainPump4.ToString(), 1);
                builder.Append(this.PressExtracorporealDrainPump.ToString(), 1);
                builder.Append(this.STATTubeCheck.ToString(), 1);
                builder.Append(this.R1MReagentOpenClose.ToString(), 1);
                builder.Append(this.R2MReagentOpenClose.ToString(), 1);
                builder.Append(this.CellDisposeCheck.ToString(), 1);
                builder.Append(this.STATSwitch.ToString(), 1);
                builder.Append(this.ReagentTableTurnSwitch.ToString(), 1);
                builder.Append(this.SampleAspirationData.ToString(), 8);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// センサーステータス問い合わせコマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// センサーステータス問い合わせコマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1 = 0;
            Byte tmpdata2 = 0;
            Byte tmpdata3 = 0;
            Byte tmpdata4 = 0;
            Byte tmpdata5 = 0;
            Byte tmpdata6 = 0;
            Byte tmpdata7 = 0;
            Byte tmpdata8 = 0;
            Byte tmpdata9 = 0;
            Byte tmpdata10 = 0;
            Byte tmpdata11 = 0;
            Byte tmpdata12 = 0;
            Byte tmpdata13 = 0;
            Byte tmpdata14 = 0;
            Byte tmpdata15 = 0;
            Byte tmpdata16 = 0;
            Byte tmpdata17 = 0;
            Byte tmpdata18 = 0;
            Byte tmpdata19 = 0;
            Byte tmpdata20 = 0;
            Byte tmpdata21 = 0;
            Byte tmpdata22 = 0;
            Byte tmpdata23 = 0;
            Byte tmpdata24 = 0;
            Byte tmpdata25 = 0;
            Byte tmpdata26 = 0;
            Byte tmpdata27 = 0;
            Byte tmpdata28 = 0;
            Byte tmpdata29 = 0;
            Byte tmpdata30 = 0;
            Byte tmpdata31 = 0;
            Byte tmpdata32 = 0;
            Byte tmpdata33 = 0;
            Byte tmpdata34 = 0;
            Byte tmpdata35 = 0;
            Byte tmpdata36 = 0;
            Byte tmpdata37 = 0;
            Byte tmpdata38 = 0;
            Byte tmpdata39 = 0;
            Byte tmpdata40 = 0;
            Byte tmpdata41 = 0;
            Byte tmpdata42 = 0;
            Byte tmpdata43 = 0;
            Byte tmpdata44 = 0;
            Byte tmpdata45 = 0;
            Byte tmpdata46 = 0;
            Byte tmpdata47 = 0;
            Byte tmpdata48 = 0;
            Byte tmpdata49 = 0;
            Int32 tmpdata50 = 0;

            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            resultList.Add(text_data.spoilByte(out tmpdata3, 1));
            resultList.Add(text_data.spoilByte(out tmpdata4, 1));
            resultList.Add(text_data.spoilByte(out tmpdata5, 1));
            resultList.Add(text_data.spoilByte(out tmpdata6, 1));
            resultList.Add(text_data.spoilByte(out tmpdata7, 1));
            resultList.Add(text_data.spoilByte(out tmpdata8, 1));
            resultList.Add(text_data.spoilByte(out tmpdata9, 1));
            resultList.Add(text_data.spoilByte(out tmpdata10, 1));
            resultList.Add(text_data.spoilByte(out tmpdata11, 1));
            resultList.Add(text_data.spoilByte(out tmpdata12, 1));
            resultList.Add(text_data.spoilByte(out tmpdata13, 1));
            resultList.Add(text_data.spoilByte(out tmpdata14, 1));
            resultList.Add(text_data.spoilByte(out tmpdata15, 1));
            resultList.Add(text_data.spoilByte(out tmpdata16, 1));
            resultList.Add(text_data.spoilByte(out tmpdata17, 1));
            resultList.Add(text_data.spoilByte(out tmpdata18, 1));
            resultList.Add(text_data.spoilByte(out tmpdata19, 1));
            resultList.Add(text_data.spoilByte(out tmpdata20, 1));
            resultList.Add(text_data.spoilByte(out tmpdata21, 1));
            resultList.Add(text_data.spoilByte(out tmpdata22, 1));
            resultList.Add(text_data.spoilByte(out tmpdata23, 1));
            resultList.Add(text_data.spoilByte(out tmpdata24, 1));
            resultList.Add(text_data.spoilByte(out tmpdata25, 1));
            resultList.Add(text_data.spoilByte(out tmpdata26, 1));
            resultList.Add(text_data.spoilByte(out tmpdata27, 1));
            resultList.Add(text_data.spoilByte(out tmpdata28, 1));
            resultList.Add(text_data.spoilByte(out tmpdata29, 1));
            resultList.Add(text_data.spoilByte(out tmpdata30, 1));
            resultList.Add(text_data.spoilByte(out tmpdata31, 1));
            resultList.Add(text_data.spoilByte(out tmpdata32, 1));
            resultList.Add(text_data.spoilByte(out tmpdata33, 1));
            resultList.Add(text_data.spoilByte(out tmpdata34, 1));
            resultList.Add(text_data.spoilByte(out tmpdata35, 1));
            resultList.Add(text_data.spoilByte(out tmpdata36, 1));
            resultList.Add(text_data.spoilByte(out tmpdata37, 1));
            resultList.Add(text_data.spoilByte(out tmpdata38, 1));
            resultList.Add(text_data.spoilByte(out tmpdata39, 1));
            resultList.Add(text_data.spoilByte(out tmpdata40, 1));
            resultList.Add(text_data.spoilByte(out tmpdata41, 1));
            resultList.Add(text_data.spoilByte(out tmpdata42, 1));
            resultList.Add(text_data.spoilByte(out tmpdata43, 1));
            resultList.Add(text_data.spoilByte(out tmpdata44, 1));
            resultList.Add(text_data.spoilByte(out tmpdata45, 1));
            resultList.Add(text_data.spoilByte(out tmpdata46, 1));
            resultList.Add(text_data.spoilByte(out tmpdata47, 1));
            resultList.Add(text_data.spoilByte(out tmpdata48, 1));
            resultList.Add(text_data.spoilByte(out tmpdata49, 1));
            resultList.Add(text_data.spoilInt(out tmpdata50, 8));

            CaseDoorDetective = tmpdata1;
            DrainBoxFull = tmpdata2;
            UsableDrainBox = tmpdata3;
            UsableTipCellCaseTransfer = tmpdata4;
            UsableTipCellCase = tmpdata5;
            ReagStorageCoverDetective = tmpdata6;
            UsableRReagBottle = tmpdata7;
            UsableMReagBottle = tmpdata8;
            UsableDispenceTipCatch = tmpdata9;
            UsableReactionCellCatch = tmpdata10;
            ReactionCellSettingCheckOuter = tmpdata11;
            ReactionCellSettingCheckInner = tmpdata12;
            ReactionCellSettingCheckSettingPosition = tmpdata13;
            R1MixingZThetaCheck = tmpdata14;
            ReactionCellSettingCheckBF1 = tmpdata15;
            ReactionCellSettingCheckBF2 = tmpdata16;
            R2MixingZThetaCheck = tmpdata17;
            BF1MixingZThetaCheck = tmpdata18;
            BF2MixingZThetaCheck = tmpdata19;
            PTrMixingZThetaCheck = tmpdata20;
            PhotometryShutterSolenoidCheck = tmpdata21;
            ReagDispense1NozzleClashDetective = tmpdata22;
            ReagDispense2NozzleClashDetective = tmpdata23;
            BF1Nozzle1DrainCheck = tmpdata24;
            BF1Nozzle2DrainCheck = tmpdata25;
            BF2Nozzle1DrainCheck = tmpdata26;
            BF2Nozzle2DrainCheck = tmpdata27;
            BF2Nozzle3DrainCheck = tmpdata28;
            UsableWashBuffer = tmpdata29;
            WashBufferFull = tmpdata30;
            DrainBufferFull = tmpdata31;
            UsablePreTrigger1 = tmpdata32;
            UsablePreTrigger2 = tmpdata33;
            UsableTrigger1 = tmpdata34;
            UsableTrigger2 = tmpdata35;
            UsableDiluent = tmpdata36;
            UsablePurifiedWater = tmpdata37;
            PressWashPullInPump = tmpdata38;
            PressDrainPump1 = tmpdata39;
            PressDrainPump2 = tmpdata40;
            PressDrainPump3 = tmpdata41;
            PressDrainPump4 = tmpdata42;
            PressExtracorporealDrainPump = tmpdata43;
            STATTubeCheck = tmpdata44;
            R1MReagentOpenClose = tmpdata45;
            R2MReagentOpenClose = tmpdata46;
            CellDisposeCheck = tmpdata47;
            STATSwitch = tmpdata48;
            ReagentTableTurnSwitch = tmpdata49;
            SampleAspirationData = tmpdata50;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion

    }

    /// <summary>
    /// センサー無効コマンド
    /// </summary>
    /// <remarks>
    /// センサー無効コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0441 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0441()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0441;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// （本体フレーム部）ケース扉検知センサ
        /// </summary>
        public Byte CaseDoorDetective { get; set; } = 0;

        /// <summary>
        /// （本体フレーム部）廃棄ボックス満杯センサ
        /// </summary>
        public Byte DrainBoxFull { get; set; } = 0;

        /// <summary>
        /// （本体フレーム部）廃棄ボックス有無センサ
        /// </summary>
        public Byte UsableDrainBox { get; set; } = 0;

        /// <summary>
        /// （ケース搬送部）ケース搬送有無センサ
        /// </summary>
        public Byte UsableTipCellCaseTransfer { get; set; } = 0;

        /// <summary>
        /// （ケース搬送部）ケース有無センサ
        /// </summary>
        public Byte UsableTipCellCase { get; set; } = 0;

        /// <summary>
        /// （試薬保冷庫）試薬保冷庫カバー検知センサ
        /// </summary>
        public Byte ReagStorageCoverDetective { get; set; } = 0;

        /// <summary>
        /// （試薬保冷庫）R試薬ボトル有無センサ
        /// </summary>
        public Byte UsableRReagBottle { get; set; } = 0;

        /// <summary>
        /// （試薬保冷庫）M試薬ボトル有無センサ
        /// </summary>
        public Byte UsableMReagBottle { get; set; } = 0;

        /// <summary>
        /// （サンプル分注移送部）分注チップキャッチ有無センサ
        /// </summary>
        public Byte UsableDispenceTipCatch { get; set; } = 0;

        /// <summary>
        /// （反応容器搬送部）反応容器キャッチ有無センサ
        /// </summary>
        public Byte UsableReactionCellCatch { get; set; } = 0;

        /// <summary>
        /// （反応テーブル部）反応容器設置確認(外側)センサ
        /// </summary>
        public Byte ReactionCellSettingCheckOuter { get; set; } = 0;

        /// <summary>
        /// （反応テーブル部）反応容器設置確認(内側)センサ
        /// </summary>
        public Byte ReactionCellSettingCheckInner { get; set; } = 0;

        /// <summary>
        /// （反応テーブル部）反応容器設置確認(設置部)センサ
        /// </summary>
        public Byte ReactionCellSettingCheckSettingPosition { get; set; } = 0;

        /// <summary>
        /// （BFテーブル部）反応容器設置確認(BF1)センサ
        /// </summary>
        public Byte ReactionCellSettingCheckBF1 { get; set; } = 0;

        /// <summary>
        /// （BFテーブル部）反応容器設置確認(BF2)センサ
        /// </summary>
        public Byte ReactionCellSettingCheckBF2 { get; set; } = 0;

        /// <summary>
        /// （攪拌部）R1撹拌Zθ確認センサ
        /// </summary>
        public Byte R1MixingZThetaCheck { get; set; } = 0;

        /// <summary>
        /// （攪拌部）R2撹拌Zθ確認センサ
        /// </summary>
        public Byte R2MixingZThetaCheck { get; set; } = 0;

        /// <summary>
        /// （攪拌部）BF1撹拌Zθ確認センサ
        /// </summary>
        public Byte BF1MixingZThetaCheck { get; set; } = 0;

        /// <summary>
        /// （攪拌部）BF2撹拌Zθ確認センサ
        /// </summary>
        public Byte BF2MixingZThetaCheck { get; set; } = 0;

        /// <summary>
        /// （攪拌部）PTr撹拌Zθ確認センサ
        /// </summary>
        public Byte PTrMixingZThetaCheck { get; set; } = 0;

        /// <summary>
        /// （化学発光測定部）測光部シャッターソレノイド確認センサ
        /// </summary>
        public Byte PhotometryShutterSolenoidCheck { get; set; } = 0;

        /// <summary>
        /// （試薬分注1部）R1ノズル衝突検知センサー
        /// </summary>
        public Byte ReagDispense1NozzleClashDetective { get; set; } = 0;

        /// <summary>
        /// （試薬分注2部）R2ノズル衝突検知センサー
        /// </summary>
        public Byte ReagDispense2NozzleClashDetective { get; set; } = 0;

        /// <summary>
        /// （BF1部、BF1廃液部）BF1ノズル1 廃液確認センサ
        /// </summary>
        public Byte BF1Nozzle1DrainCheck { get; set; } = 0;

        /// <summary>
        /// （BF1部、BF1廃液部）BF1ノズル2 廃液確認センサ
        /// </summary>
        public Byte BF1Nozzle2DrainCheck { get; set; } = 0;

        /// <summary>
        /// （BF2部）BF2ノズル1 廃液確認センサ
        /// </summary>
        public Byte BF2Nozzle1DrainCheck { get; set; } = 0;

        /// <summary>
        /// （BF2部）BF2ノズル2 廃液確認センサ
        /// </summary>
        public Byte BF2Nozzle2DrainCheck { get; set; } = 0;

        /// <summary>
        /// （BF2部）BF2ノズル3 廃液確認センサ
        /// </summary>
        public Byte BF2Nozzle3DrainCheck { get; set; } = 0;

        /// <summary>
        /// （流体配管部）洗浄液バッファ有無センサ(残量分検知できる位置へ変更)
        /// </summary>
        public Byte UsableWashBuffer { get; set; } = 0;

        /// <summary>
        /// （流体配管部）洗浄液バッファ満杯センサ
        /// </summary>
        public Byte WashBufferFull { get; set; } = 0;

        /// <summary>
        /// （流体配管部）廃液バッファ満杯センサ(導通基板へ)
        /// </summary>
        public Byte DrainBufferFull { get; set; } = 0;

        /// <summary>
        /// （流体配管部）プレトリガ１液有無センサ
        /// </summary>
        public Byte UsablePreTrigger1 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）プレトリガ２液有無センサ
        /// </summary>
        public Byte UsablePreTrigger2 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）トリガ１液有無センサ
        /// </summary>
        public Byte UsableTrigger1 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）トリガ２液有無センサ
        /// </summary>
        public Byte UsableTrigger2 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）希釈液有無センサ(導通基板へ)
        /// </summary>
        public Byte UsableDiluent { get; set; } = 0;

        /// <summary>
        /// （流体配管部）精製水有無センサ
        /// </summary>
        public Byte UsablePurifiedWater { get; set; } = 0;

        /// <summary>
        /// （流体配管部）洗浄液引込ポンプ用圧力センサ
        /// </summary>
        public Byte PressWashPullInPump { get; set; } = 0;

        /// <summary>
        /// （流体配管部）廃液ポンプ1用圧力センサ
        /// </summary>
        public Byte PressDrainPump1 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）廃液ポンプ2用圧力センサ
        /// </summary>
        public Byte PressDrainPump2 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）廃液ポンプ3用圧力センサ
        /// </summary>
        public Byte PressDrainPump3 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）廃液ポンプ4用圧力センサ
        /// </summary>
        public Byte PressDrainPump4 { get; set; } = 0;

        /// <summary>
        /// （流体配管部）体外廃液ポンプ用圧力センサ
        /// </summary>
        public Byte PressExtracorporealDrainPump { get; set; } = 0;

        /// <summary>
        /// スタット容器有無センサ
        /// </summary>
        public Byte STATTubeCheck { get; set; } = 0;

        /// <summary>
        /// 試薬蓋開閉センサ1
        /// </summary>
        public Byte R1MReagentOpenClose { get; set; } = 0;

        /// <summary>
        /// 試薬蓋開閉センサ2
        /// </summary>
        public Byte R2MReagentOpenClose { get; set; } = 0;

        /// <summary>
        /// 反応容器廃棄確認センサ
        /// </summary>
        public Byte CellDisposeCheck { get; set; } = 0;

        /// <summary>
        /// STAT設置スイッチ
        /// </summary>
        public Byte STATSwitch { get; set; } = 0;

        /// <summary>
        /// 試薬保冷庫回転スイッチ
        /// </summary>
        public Byte ReagentTableTurnSwitch { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.CaseDoorDetective.ToString(), 1);
                builder.Append(this.DrainBoxFull.ToString(), 1);
                builder.Append(this.UsableDrainBox.ToString(), 1);
                builder.Append(this.UsableTipCellCaseTransfer.ToString(), 1);
                builder.Append(this.UsableTipCellCase.ToString(), 1);
                builder.Append(this.ReagStorageCoverDetective.ToString(), 1);
                builder.Append(this.UsableRReagBottle.ToString(), 1);
                builder.Append(this.UsableMReagBottle.ToString(), 1);
                builder.Append(this.UsableDispenceTipCatch.ToString(), 1);
                builder.Append(this.UsableReactionCellCatch.ToString(), 1);
                builder.Append(this.ReactionCellSettingCheckOuter.ToString(), 1);
                builder.Append(this.ReactionCellSettingCheckInner.ToString(), 1);
                builder.Append(this.ReactionCellSettingCheckSettingPosition.ToString(), 1);
                builder.Append(this.R1MixingZThetaCheck.ToString(), 1);
                builder.Append(this.ReactionCellSettingCheckBF1.ToString(), 1);
                builder.Append(this.ReactionCellSettingCheckBF2.ToString(), 1);
                builder.Append(this.R2MixingZThetaCheck.ToString(), 1);
                builder.Append(this.BF1MixingZThetaCheck.ToString(), 1);
                builder.Append(this.BF2MixingZThetaCheck.ToString(), 1);
                builder.Append(this.PTrMixingZThetaCheck.ToString(), 1);
                builder.Append(this.PhotometryShutterSolenoidCheck.ToString(), 1);
                builder.Append(this.ReagDispense1NozzleClashDetective.ToString(), 1);
                builder.Append(this.ReagDispense2NozzleClashDetective.ToString(), 1);
                builder.Append(this.BF1Nozzle1DrainCheck.ToString(), 1);
                builder.Append(this.BF1Nozzle2DrainCheck.ToString(), 1);
                builder.Append(this.BF2Nozzle1DrainCheck.ToString(), 1);
                builder.Append(this.BF2Nozzle2DrainCheck.ToString(), 1);
                builder.Append(this.BF2Nozzle3DrainCheck.ToString(), 1);
                builder.Append(this.UsableWashBuffer.ToString(), 1);
                builder.Append(this.WashBufferFull.ToString(), 1);
                builder.Append(this.DrainBufferFull.ToString(), 1);
                builder.Append(this.UsablePreTrigger1.ToString(), 1);
                builder.Append(this.UsablePreTrigger2.ToString(), 1);
                builder.Append(this.UsableTrigger1.ToString(), 1);
                builder.Append(this.UsableTrigger2.ToString(), 1);
                builder.Append(this.UsableDiluent.ToString(), 1);
                builder.Append(this.UsablePurifiedWater.ToString(), 1);
                builder.Append(this.PressWashPullInPump.ToString(), 1);
                builder.Append(this.PressDrainPump1.ToString(), 1);
                builder.Append(this.PressDrainPump2.ToString(), 1);
                builder.Append(this.PressDrainPump3.ToString(), 1);
                builder.Append(this.PressDrainPump4.ToString(), 1);
                builder.Append(this.PressExtracorporealDrainPump.ToString(), 1);
                builder.Append(this.STATTubeCheck.ToString(), 1);
                builder.Append(this.R1MReagentOpenClose.ToString(), 1);
                builder.Append(this.R2MReagentOpenClose.ToString(), 1);
                builder.Append(this.CellDisposeCheck.ToString(), 1);
                builder.Append(this.STATSwitch.ToString(), 1);
                builder.Append(this.ReagentTableTurnSwitch.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if SIMULATOR
        /// <summary>
        /// センサーステータス問い合わせコマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// センサーステータス問い合わせコマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1 = 0;
            Byte tmpdata2 = 0;
            Byte tmpdata3 = 0;
            Byte tmpdata4 = 0;
            Byte tmpdata5 = 0;
            Byte tmpdata6 = 0;
            Byte tmpdata7 = 0;
            Byte tmpdata8 = 0;
            Byte tmpdata9 = 0;
            Byte tmpdata10 = 0;
            Byte tmpdata11 = 0;
            Byte tmpdata12 = 0;
            Byte tmpdata13 = 0;
            Byte tmpdata14 = 0;
            Byte tmpdata15 = 0;
            Byte tmpdata16 = 0;
            Byte tmpdata17 = 0;
            Byte tmpdata18 = 0;
            Byte tmpdata19 = 0;
            Byte tmpdata20 = 0;
            Byte tmpdata21 = 0;
            Byte tmpdata22 = 0;
            Byte tmpdata23 = 0;
            Byte tmpdata24 = 0;
            Byte tmpdata25 = 0;
            Byte tmpdata26 = 0;
            Byte tmpdata27 = 0;
            Byte tmpdata28 = 0;
            Byte tmpdata29 = 0;
            Byte tmpdata30 = 0;
            Byte tmpdata31 = 0;
            Byte tmpdata32 = 0;
            Byte tmpdata33 = 0;
            Byte tmpdata34 = 0;
            Byte tmpdata35 = 0;
            Byte tmpdata36 = 0;
            Byte tmpdata37 = 0;
            Byte tmpdata38 = 0;
            Byte tmpdata39 = 0;
            Byte tmpdata40 = 0;
            Byte tmpdata41 = 0;
            Byte tmpdata42 = 0;
            Byte tmpdata43 = 0;
            Byte tmpdata44 = 0;
            Byte tmpdata45 = 0;
            Byte tmpdata46 = 0;
            Byte tmpdata47 = 0;
            Byte tmpdata48 = 0;
            Byte tmpdata49 = 0;

            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            resultList.Add(text_data.spoilByte(out tmpdata3, 1));
            resultList.Add(text_data.spoilByte(out tmpdata4, 1));
            resultList.Add(text_data.spoilByte(out tmpdata5, 1));
            resultList.Add(text_data.spoilByte(out tmpdata6, 1));
            resultList.Add(text_data.spoilByte(out tmpdata7, 1));
            resultList.Add(text_data.spoilByte(out tmpdata8, 1));
            resultList.Add(text_data.spoilByte(out tmpdata9, 1));
            resultList.Add(text_data.spoilByte(out tmpdata10, 1));
            resultList.Add(text_data.spoilByte(out tmpdata11, 1));
            resultList.Add(text_data.spoilByte(out tmpdata12, 1));
            resultList.Add(text_data.spoilByte(out tmpdata13, 1));
            resultList.Add(text_data.spoilByte(out tmpdata14, 1));
            resultList.Add(text_data.spoilByte(out tmpdata15, 1));
            resultList.Add(text_data.spoilByte(out tmpdata16, 1));
            resultList.Add(text_data.spoilByte(out tmpdata17, 1));
            resultList.Add(text_data.spoilByte(out tmpdata18, 1));
            resultList.Add(text_data.spoilByte(out tmpdata19, 1));
            resultList.Add(text_data.spoilByte(out tmpdata20, 1));
            resultList.Add(text_data.spoilByte(out tmpdata21, 1));
            resultList.Add(text_data.spoilByte(out tmpdata22, 1));
            resultList.Add(text_data.spoilByte(out tmpdata23, 1));
            resultList.Add(text_data.spoilByte(out tmpdata24, 1));
            resultList.Add(text_data.spoilByte(out tmpdata25, 1));
            resultList.Add(text_data.spoilByte(out tmpdata26, 1));
            resultList.Add(text_data.spoilByte(out tmpdata27, 1));
            resultList.Add(text_data.spoilByte(out tmpdata28, 1));
            resultList.Add(text_data.spoilByte(out tmpdata29, 1));
            resultList.Add(text_data.spoilByte(out tmpdata30, 1));
            resultList.Add(text_data.spoilByte(out tmpdata31, 1));
            resultList.Add(text_data.spoilByte(out tmpdata32, 1));
            resultList.Add(text_data.spoilByte(out tmpdata33, 1));
            resultList.Add(text_data.spoilByte(out tmpdata34, 1));
            resultList.Add(text_data.spoilByte(out tmpdata35, 1));
            resultList.Add(text_data.spoilByte(out tmpdata36, 1));
            resultList.Add(text_data.spoilByte(out tmpdata37, 1));
            resultList.Add(text_data.spoilByte(out tmpdata38, 1));
            resultList.Add(text_data.spoilByte(out tmpdata39, 1));
            resultList.Add(text_data.spoilByte(out tmpdata40, 1));
            resultList.Add(text_data.spoilByte(out tmpdata41, 1));
            resultList.Add(text_data.spoilByte(out tmpdata42, 1));
            resultList.Add(text_data.spoilByte(out tmpdata43, 1));
            resultList.Add(text_data.spoilByte(out tmpdata44, 1));
            resultList.Add(text_data.spoilByte(out tmpdata45, 1));
            resultList.Add(text_data.spoilByte(out tmpdata46, 1));
            resultList.Add(text_data.spoilByte(out tmpdata47, 1));
            resultList.Add(text_data.spoilByte(out tmpdata48, 1));
            resultList.Add(text_data.spoilByte(out tmpdata49, 1));

            CaseDoorDetective = tmpdata1;
            DrainBoxFull = tmpdata2;
            UsableDrainBox = tmpdata3;
            UsableTipCellCaseTransfer = tmpdata4;
            UsableTipCellCase = tmpdata5;
            ReagStorageCoverDetective = tmpdata6;
            UsableRReagBottle = tmpdata7;
            UsableMReagBottle = tmpdata8;
            UsableDispenceTipCatch = tmpdata9;
            UsableReactionCellCatch = tmpdata10;
            ReactionCellSettingCheckOuter = tmpdata11;
            ReactionCellSettingCheckInner = tmpdata12;
            ReactionCellSettingCheckSettingPosition = tmpdata13;
            R1MixingZThetaCheck = tmpdata14;
            ReactionCellSettingCheckBF1 = tmpdata15;
            ReactionCellSettingCheckBF2 = tmpdata16;
            R2MixingZThetaCheck = tmpdata17;
            BF1MixingZThetaCheck = tmpdata18;
            BF2MixingZThetaCheck = tmpdata19;
            PTrMixingZThetaCheck = tmpdata20;
            PhotometryShutterSolenoidCheck = tmpdata21;
            ReagDispense1NozzleClashDetective = tmpdata22;
            ReagDispense2NozzleClashDetective = tmpdata23;
            BF1Nozzle1DrainCheck = tmpdata24;
            BF1Nozzle2DrainCheck = tmpdata25;
            BF2Nozzle1DrainCheck = tmpdata26;
            BF2Nozzle2DrainCheck = tmpdata27;
            BF2Nozzle3DrainCheck = tmpdata28;
            UsableWashBuffer = tmpdata29;
            WashBufferFull = tmpdata30;
            DrainBufferFull = tmpdata31;
            UsablePreTrigger1 = tmpdata32;
            UsablePreTrigger2 = tmpdata33;
            UsableTrigger1 = tmpdata34;
            UsableTrigger2 = tmpdata35;
            UsableDiluent = tmpdata36;
            UsablePurifiedWater = tmpdata37;
            PressWashPullInPump = tmpdata38;
            PressDrainPump1 = tmpdata39;
            PressDrainPump2 = tmpdata40;
            PressDrainPump3 = tmpdata41;
            PressDrainPump4 = tmpdata42;
            PressExtracorporealDrainPump = tmpdata43;
            STATTubeCheck = tmpdata44;
            R1MReagentOpenClose = tmpdata45;
            R2MReagentOpenClose = tmpdata46;
            CellDisposeCheck = tmpdata47;
            STATSwitch = tmpdata48;
            ReagentTableTurnSwitch = tmpdata49;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }
    /// <summary>
    /// センサー無効コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// センサー無効コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1441 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1441()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1441;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// スタートアップ開始コマンド
    /// </summary>
    /// <remarks>
    /// スタートアップ開始コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0442 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0442()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0442;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// スタートアップ開始コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// スタートアップ開始コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1442 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1442()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1442;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// スタートアップ終了コマンド
    /// </summary>
    /// <remarks>
    /// スタートアップ終了コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0443 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0443()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0443;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// スタートアップ終了コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// スタートアップ終了コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1443 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1443()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1443;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 寿命部品使用回数設定・問合せコマンド
    /// </summary>
    /// <remarks>
    /// 寿命部品使用回数設定・問合せコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0444 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0444()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0444;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 問合せ種別
        /// </summary>
        public CommandControlParameter Control { get; set; } = CommandControlParameter.Start;

        /// <summary>
        /// 検体シリンジ使用回数
        /// </summary>
        public Int32 SampleCyl { get; set; } = 0;

        /// <summary>
        /// R1分注シリンジ使用回数
        /// </summary>
        public Int32 CylR1 { get; set; } = 0;

        /// <summary>
        /// R2分注シリンジ使用回数
        /// </summary>
        public Int32 CylR2 { get; set; } = 0;

        /// <summary>
        /// 試薬分注洗浄液シリンジ使用回数
        /// </summary>
        public Int32 ReagWashCyl { get; set; } = 0;

        /// <summary>
        /// 希釈液分注シリンジ使用回数
        /// </summary>
        public Int32 DilutieCyl { get; set; } = 0;

        /// <summary>
        /// 洗浄１シリンジ使用回数
        /// </summary>
        public Int32 Wash1Cyl { get; set; } = 0;

        /// <summary>
        /// 洗浄2シリンジ使用回数
        /// </summary>
        public Int32 Wash2Cyl { get; set; } = 0;

        /// <summary>
        /// プレトリガ分注シリンジ使用回数
        /// </summary>
        public Int32 PreTreggerCyl { get; set; } = 0;

        /// <summary>
        /// トリガ分注シリンジ使用回数
        /// </summary>
        public Int32 TreggerCyl { get; set; } = 0;

        /// <summary>
        /// 体外廃液ポンプ使用時間(単位:時間)
        /// </summary>
        public Int32 OutDrainPump { get; set; } = 0;

        /// <summary>
        /// 体外廃液ポンプチューブ使用時間(単位:時間)
        /// </summary>
        public Int32 OutDrainPumpTube { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.Control).ToString(), 1);
                builder.Append(this.SampleCyl.ToString(), 7);
                builder.Append(this.CylR1.ToString(), 7);
                builder.Append(this.CylR2.ToString(), 7);
                builder.Append(this.ReagWashCyl.ToString(), 7);
                builder.Append(this.DilutieCyl.ToString(), 7);
                builder.Append(this.Wash1Cyl.ToString(), 7);
                builder.Append(this.Wash2Cyl.ToString(), 7);
                builder.Append(this.PreTreggerCyl.ToString(), 7);
                builder.Append(this.TreggerCyl.ToString(), 7);
                builder.Append(this.OutDrainPump.ToString(), 7);
                builder.Append(this.OutDrainPumpTube.ToString(), 7);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            Int32 tmpdata2;
            Int32 tmpdata3;
            Int32 tmpdata4;
            Int32 tmpdata5;
            Int32 tmpdata6;
            Int32 tmpdata7;
            Int32 tmpdata8;
            Int32 tmpdata9;
            Int32 tmpdata10;
            Int32 tmpdata11;
            Int32 tmpdata12;

            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilInt(out tmpdata2, 7));
            resultList.Add(text_data.spoilInt(out tmpdata3, 7));
            resultList.Add(text_data.spoilInt(out tmpdata4, 7));
            resultList.Add(text_data.spoilInt(out tmpdata5, 7));
            resultList.Add(text_data.spoilInt(out tmpdata6, 7));
            resultList.Add(text_data.spoilInt(out tmpdata7, 7));
            resultList.Add(text_data.spoilInt(out tmpdata8, 7));
            resultList.Add(text_data.spoilInt(out tmpdata9, 7));
            resultList.Add(text_data.spoilInt(out tmpdata10, 7));
            resultList.Add(text_data.spoilInt(out tmpdata11, 7));
            resultList.Add(text_data.spoilInt(out tmpdata12, 7));

            this.Control = (CommandControlParameter)tmpdata1;
            this.SampleCyl = tmpdata2;
            this.CylR1 = tmpdata3;
            this.CylR2 = tmpdata4;
            this.ReagWashCyl = tmpdata5;
            this.DilutieCyl = tmpdata6;
            this.Wash1Cyl = tmpdata7;
            this.Wash2Cyl = tmpdata8;
            this.PreTreggerCyl = tmpdata9;
            this.TreggerCyl = tmpdata10;
            this.OutDrainPump = tmpdata11;
            this.OutDrainPumpTube = tmpdata12;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 寿命部品使用回数設定・問合せコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 寿命部品使用回数設定・問合せコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1444 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1444()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1444;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 問合せ種別
        /// </summary>
        public CommandControlParameter Control { get; set; } = CommandControlParameter.Start;

        /// <summary>
        /// 検体シリンジ使用回数
        /// </summary>
        public Int32 SampleCyl { get; set; } = 0;

        /// <summary>
        /// R1分注シリンジ使用回数
        /// </summary>
        public Int32 CylR1 { get; set; } = 0;

        /// <summary>
        /// R2分注シリンジ使用回数
        /// </summary>
        public Int32 CylR2 { get; set; } = 0;

        /// <summary>
        /// 試薬分注洗浄液シリンジ使用回数
        /// </summary>
        public Int32 ReagWashCyl { get; set; } = 0;

        /// <summary>
        /// 希釈液分注シリンジ使用回数
        /// </summary>
        public Int32 DilutieCyl { get; set; } = 0;

        /// <summary>
        /// 洗浄１シリンジ使用回数
        /// </summary>
        public Int32 Wash1Cyl { get; set; } = 0;

        /// <summary>
        /// 洗浄2シリンジ使用回数
        /// </summary>
        public Int32 Wash2Cyl { get; set; } = 0;

        /// <summary>
        /// プレトリガ分注シリンジ使用回数
        /// </summary>
        public Int32 PreTreggerCyl { get; set; } = 0;

        /// <summary>
        /// トリガ分注シリンジ使用回数
        /// </summary>
        public Int32 TreggerCyl { get; set; } = 0;

        /// <summary>
        /// 体外廃液ポンプ使用時間(単位:時間)
        /// </summary>
        public Int32 OutDrainPump { get; set; } = 0;

        /// <summary>
        /// 体外廃液ポンプチューブ使用時間(単位:時間)
        /// </summary>
        public Int32 OutDrainPumpTube { get; set; } = 0;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.Control).ToString(), 1);
                builder.Append(this.SampleCyl.ToString(), 7);
                builder.Append(this.CylR1.ToString(), 7);
                builder.Append(this.CylR2.ToString(), 7);
                builder.Append(this.ReagWashCyl.ToString(), 7);
                builder.Append(this.DilutieCyl.ToString(), 7);
                builder.Append(this.Wash1Cyl.ToString(), 7);
                builder.Append(this.Wash2Cyl.ToString(), 7);
                builder.Append(this.PreTreggerCyl.ToString(), 7);
                builder.Append(this.TreggerCyl.ToString(), 7);
                builder.Append(this.OutDrainPump.ToString(), 7);
                builder.Append(this.OutDrainPumpTube.ToString(), 7);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            Int32 tmpdata2;
            Int32 tmpdata3;
            Int32 tmpdata4;
            Int32 tmpdata5;
            Int32 tmpdata6;
            Int32 tmpdata7;
            Int32 tmpdata8;
            Int32 tmpdata9;
            Int32 tmpdata10;
            Int32 tmpdata11;
            Int32 tmpdata12;

            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilInt(out tmpdata2, 7));
            resultList.Add(text_data.spoilInt(out tmpdata3, 7));
            resultList.Add(text_data.spoilInt(out tmpdata4, 7));
            resultList.Add(text_data.spoilInt(out tmpdata5, 7));
            resultList.Add(text_data.spoilInt(out tmpdata6, 7));
            resultList.Add(text_data.spoilInt(out tmpdata7, 7));
            resultList.Add(text_data.spoilInt(out tmpdata8, 7));
            resultList.Add(text_data.spoilInt(out tmpdata9, 7));
            resultList.Add(text_data.spoilInt(out tmpdata10, 7));
            resultList.Add(text_data.spoilInt(out tmpdata11, 7));
            resultList.Add(text_data.spoilInt(out tmpdata12, 7));

            this.Control = (CommandControlParameter)tmpdata1;
            this.SampleCyl = tmpdata2;
            this.CylR1 = tmpdata3;
            this.CylR2 = tmpdata4;
            this.ReagWashCyl = tmpdata5;
            this.DilutieCyl = tmpdata6;
            this.Wash1Cyl = tmpdata7;
            this.Wash2Cyl = tmpdata8;
            this.PreTreggerCyl = tmpdata9;
            this.TreggerCyl = tmpdata10;
            this.OutDrainPump = tmpdata11;
            this.OutDrainPumpTube = tmpdata12;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
        #endregion
    }

    /// <summary>
    /// プレトリガアクトコマンド
    /// </summary>
    /// <remarks>
    /// プレトリガアクトコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0445 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0445()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0445;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 優先ボトル
        /// </summary>
        public Int32 PriorityBottle { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.PriorityBottle.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            this.PriorityBottle = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// プレトリガアクトコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// プレトリガアクトコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1445 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1445()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1445;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// トリガアクトコマンド
    /// </summary>
    /// <remarks>
    /// トリガボトルアクトコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0446 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0446()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0446;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 優先ボトル
        /// </summary>
        public Int32 PriorityBottle { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.PriorityBottle.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            this.PriorityBottle = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// トリガアクトコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// トリガアクトコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1446 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1446()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1446;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }


    /// <summary>
    /// 廃液タンク状態(ラック情報)通知コマンド
    /// </summary>
    /// <remarks>
    /// 廃液タンク状態(ラック情報)通知コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0447 : RackTransferCommCommand_0108
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0447()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0447;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// 廃液タンク状態(ラック情報)通知コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 廃液タンク状態(ラック情報)通知コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1447 : RackTransferCommCommand_1108
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1447()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1447;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// ケース搬送ユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// ケース搬送ユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0448 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0448()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0448;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// ケース搬送ユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ケース搬送ユニットパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1448 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1448()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1448;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 試薬保冷庫ユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// 試薬保冷庫ユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0449 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0449()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0449;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// 試薬保冷庫ユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬保冷庫ユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1449 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1449()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1449;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// スタットユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// スタットユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0450 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0450()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0450;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// スタットユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// スタットユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1450 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1450()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1450;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// サンプル分注ユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// サンプル分注ユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0451 : CarisXCommCommand
    {
        #region [定数定義]
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
        /// サンプルの種類
        /// </summary>
        public enum SmpKind
        {
            /// <summary>
            /// 血清
            /// </summary>
            Serum = 1,
            /// <summary>
            /// 尿
            /// </summary>
            Urine = 2,
            /// <summary>
            /// その他
            /// </summary>
            Other = 3,
        }
        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0451()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0451;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 検体シリンジ待機位置
        /// </summary>
        public Int32 DispenceCyringeStandbyPosition { get; set; } = 0;

        /// <summary>
        /// 検体吐き残し量
        /// </summary>
        public Int32 SmpSurplusVolume { get; set; } = 0;

        /// <summary>
        /// 検体吐出量
        /// </summary>
        public Int32 SmpDispenseVolume { get; set; } = 0;

        /// <summary>
        /// 検体吸引後遅延時間
        /// </summary>
        public Double WaitTimeAfterSuckingUpForSample { get; set; } = 0;

        /// <summary>
        /// 容器種
        /// </summary>
        public SampleContainerKind ContainerKind { get; set; } = SampleContainerKind.Cup;

        /// <summary>
        /// サンプル種
        /// </summary>
        public SmpKind SampleKind { get; set; } = SmpKind.Serum;

        /// <summary>
        /// 液面検知センサ使用有無
        /// </summary>
        public Byte LiquidLevelSensor { get; set; } = 0;

        /// <summary>
        /// 泡チェック使用有無
        /// </summary>
        public Byte BubbleCheck { get; set; } = 0;

        /// <summary>
        /// 泡チェック閾値1
        /// </summary>
        public Int32 UpperOfPress1 { get; set; } = 0;

        /// <summary>
        /// 泡チェック閾値2
        /// </summary>
        public Int32 UpperOfPress2 { get; set; } = 0;

        /// <summary>
        /// 吸引エラー閾値上限（血清）
        /// </summary>
        public Int32 UpperOfSuckingUpErr1ForSerum { get; set; } = 0;

        /// <summary>
        /// 吸引エラー閾値下限（血清）
        /// </summary>
        public Int32 LowerOfSuckingUpErr1ForSerum { get; set; } = 0;

        /// <summary>
        /// 吸引エラー閾値上限（尿）
        /// </summary>
        public Int32 UpperOfSuckingUpErr1ForUrine { get; set; } = 0;

        /// <summary>
        /// 吸引エラー閾値下限（尿）
        /// </summary>
        public Int32 LowerOfSuckingUpErr1ForUrine { get; set; } = 0;

        /// <summary>
        /// リークエラー分注量下限
        /// </summary>
        public Int32 LowerOfLeakErrFluidVolume { get; set; } = 0;

        /// <summary>
        /// リークエラー閾値下限
        /// </summary>
        public Int32 LowerOfLeakErrPress { get; set; } = 0;

        /// <summary>
        /// 吐出エラー閾値
        /// </summary>
        public Int32 ThresholdDefectiveDischarge { get; set; } = 0;

        /// <summary>
        /// 前処理検体分注量
        /// </summary>
        public Int32 PreSampleDispenseVolume { get; set; } = 0;

        /// <summary>
        /// 泡チェック閾値1（>100uL）
        /// </summary>
        public Int32 UpperOfPress1Over100 { get; set; } = 0;

        /// <summary>
        /// 泡チェック閾値2（>100uL）
        /// </summary>
        public Int32 UpperOfPress2Over100 { get; set; } = 0;

        /// <summary>
        /// 吸引エラー閾値上限（血清）（>100uL）
        /// </summary>
        public Int32 UpperOfSuckingUpErr1ForSerumOver100 { get; set; } = 0;

        /// <summary>
        /// 吸引エラー閾値下限（血清）（>100uL）
        /// </summary>
        public Int32 LowerOfSuckingUpErr1ForSerumOver100 { get; set; } = 0;

        /// <summary>
        /// 吸引エラー閾値上限（尿）（>100uL）
        /// </summary>
        public Int32 UpperOfSuckingUpErr1ForUrineOver100 { get; set; } = 0;

        /// <summary>
        /// 吸引エラー閾値下限（尿）（>100uL）
        /// </summary>
        public Int32 LowerOfSuckingUpErr1ForUrineOver100 { get; set; } = 0;

        /// <summary>
        /// リークエラー分注量下限（>100uL）
        /// </summary>
        public Int32 LowerOfLeakErrFluidVolumeOver100 { get; set; } = 0;

        /// <summary>
        /// リークエラー閾値下限（>100uL）
        /// </summary>
        public Int32 LowerOfLeakErrPressOver100 { get; set; } = 0;

        /// <summary>
        /// 吐出エラー閾値（>100uL）
        /// </summary>
        public Int32 ThresholdDefectiveDischargeOver100 { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.DispenceCyringeStandbyPosition.ToString(), 3);
                builder.Append(this.SmpSurplusVolume.ToString(), 3);
                builder.Append(this.SmpDispenseVolume.ToString(), 3);
                builder.Append(this.WaitTimeAfterSuckingUpForSample.ToString("f1"), 4);
                builder.Append(((Int32)this.ContainerKind).ToString(), 1);
                builder.Append(((Int32)this.SampleKind).ToString(), 1);
                builder.Append(this.LiquidLevelSensor.ToString(), 1);
                builder.Append(this.BubbleCheck.ToString(), 1);
                builder.Append(this.UpperOfPress1.ToString(), 5);
                builder.Append(this.UpperOfPress2.ToString(), 5);
                builder.Append(this.UpperOfSuckingUpErr1ForSerum.ToString(), 5);
                builder.Append(this.LowerOfSuckingUpErr1ForSerum.ToString(), 5);
                builder.Append(this.UpperOfSuckingUpErr1ForUrine.ToString(), 5);
                builder.Append(this.LowerOfSuckingUpErr1ForUrine.ToString(), 5);
                builder.Append(this.LowerOfLeakErrFluidVolume.ToString(), 5);
                builder.Append(this.LowerOfLeakErrPress.ToString(), 5);
                builder.Append(this.ThresholdDefectiveDischarge.ToString(), 5);
                builder.Append(this.PreSampleDispenseVolume.ToString(), 3);
                builder.Append(this.UpperOfPress1Over100.ToString(), 5);
                builder.Append(this.UpperOfPress2Over100.ToString(), 5);
                builder.Append(this.UpperOfSuckingUpErr1ForSerumOver100.ToString(), 5);
                builder.Append(this.LowerOfSuckingUpErr1ForSerumOver100.ToString(), 5);
                builder.Append(this.UpperOfSuckingUpErr1ForUrineOver100.ToString(), 5);
                builder.Append(this.LowerOfSuckingUpErr1ForUrineOver100.ToString(), 5);
                builder.Append(this.LowerOfLeakErrFluidVolumeOver100.ToString(), 5);
                builder.Append(this.LowerOfLeakErrPressOver100.ToString(), 5);
                builder.Append(this.ThresholdDefectiveDischargeOver100.ToString(), 5);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            Int32 tmpdata2 = 0;
            Int32 tmpdata3 = 0;
            Double tmpdata4 = 0;
            Byte tmpdata5 = 0;
            Byte tmpdata6 = 0;
            Byte tmpdata7 = 0;
            Byte tmpdata8 = 0;
            Int32 tmpdata9 = 0;
            Int32 tmpdata10 = 0;
            Int32 tmpdata11 = 0;
            Int32 tmpdata12 = 0;
            Int32 tmpdata13 = 0;
            Int32 tmpdata14 = 0;
            Int32 tmpdata15 = 0;
            Int32 tmpdata16 = 0;
            Int32 tmpdata17 = 0;
            Int32 tmpdata18 = 0;
            Int32 tmpdata19 = 0;
            Int32 tmpdata20 = 0;
            Int32 tmpdata21 = 0;
            Int32 tmpdata22 = 0;
            Int32 tmpdata23 = 0;
            Int32 tmpdata24 = 0;
            Int32 tmpdata25 = 0;
            Int32 tmpdata26 = 0;
            Int32 tmpdata27 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 3));
            resultList.Add(text_data.spoilInt(out tmpdata2, 3));
            resultList.Add(text_data.spoilInt(out tmpdata3, 3));
            resultList.Add(text_data.spoilDouble(out tmpdata4, 4));
            resultList.Add(text_data.spoilByte(out tmpdata5, 1));
            resultList.Add(text_data.spoilByte(out tmpdata6, 1));
            resultList.Add(text_data.spoilByte(out tmpdata7, 1));
            resultList.Add(text_data.spoilByte(out tmpdata8, 1));
            resultList.Add(text_data.spoilInt(out tmpdata9, 5));
            resultList.Add(text_data.spoilInt(out tmpdata10, 5));
            resultList.Add(text_data.spoilInt(out tmpdata11, 5));
            resultList.Add(text_data.spoilInt(out tmpdata12, 5));
            resultList.Add(text_data.spoilInt(out tmpdata13, 5));
            resultList.Add(text_data.spoilInt(out tmpdata14, 5));
            resultList.Add(text_data.spoilInt(out tmpdata15, 5));
            resultList.Add(text_data.spoilInt(out tmpdata16, 5));
            resultList.Add(text_data.spoilInt(out tmpdata17, 5));
            resultList.Add(text_data.spoilInt(out tmpdata18, 3));
            resultList.Add(text_data.spoilInt(out tmpdata19, 5));
            resultList.Add(text_data.spoilInt(out tmpdata20, 5));
            resultList.Add(text_data.spoilInt(out tmpdata21, 5));
            resultList.Add(text_data.spoilInt(out tmpdata22, 5));
            resultList.Add(text_data.spoilInt(out tmpdata23, 5));
            resultList.Add(text_data.spoilInt(out tmpdata24, 5));
            resultList.Add(text_data.spoilInt(out tmpdata25, 5));
            resultList.Add(text_data.spoilInt(out tmpdata26, 5));
            resultList.Add(text_data.spoilInt(out tmpdata27, 5));
            this.DispenceCyringeStandbyPosition = tmpdata1;
            this.SmpSurplusVolume = tmpdata2;
            this.SmpDispenseVolume = tmpdata3;
            this.WaitTimeAfterSuckingUpForSample = tmpdata4;
            this.ContainerKind = (SampleContainerKind)tmpdata5;
            this.SampleKind = (SmpKind)tmpdata6;
            this.LiquidLevelSensor = tmpdata7;
            this.BubbleCheck = tmpdata8;
            this.UpperOfPress1 = tmpdata9;
            this.UpperOfPress2 = tmpdata10;
            this.UpperOfSuckingUpErr1ForSerum = tmpdata11;
            this.LowerOfSuckingUpErr1ForSerum = tmpdata12;
            this.UpperOfSuckingUpErr1ForUrine = tmpdata13;
            this.LowerOfSuckingUpErr1ForUrine = tmpdata14;
            this.LowerOfLeakErrFluidVolume = tmpdata15;
            this.LowerOfLeakErrPress = tmpdata16;
            this.ThresholdDefectiveDischarge = tmpdata17;
            this.PreSampleDispenseVolume = tmpdata18;
            this.UpperOfPress1Over100 = tmpdata19;
            this.UpperOfPress2Over100 = tmpdata20;
            this.UpperOfSuckingUpErr1ForSerumOver100 = tmpdata21;
            this.LowerOfSuckingUpErr1ForSerumOver100 = tmpdata22;
            this.UpperOfSuckingUpErr1ForUrineOver100 = tmpdata23;
            this.LowerOfSuckingUpErr1ForUrineOver100 = tmpdata24;
            this.LowerOfLeakErrFluidVolumeOver100 = tmpdata25;
            this.LowerOfLeakErrPressOver100 = tmpdata26;
            this.ThresholdDefectiveDischargeOver100 = tmpdata27;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// サンプル分注ユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// サンプル分注ユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1451 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1451()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1451;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 反応容器搬送ユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// 反応容器搬送ユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0452 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0452()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0452;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// 反応容器搬送ユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 反応容器搬送ユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1452 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1452()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1452;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 反応テーブルユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// 反応テーブルユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0453 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0453()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0453;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 反応テーブル部攪拌時間
        /// </summary>
        public Double StirringTime { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.StirringTime.ToString("f1"), 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Double tmpdata1;
            resultList.Add(text_data.spoilDouble(out tmpdata1, 4));
            this.StirringTime = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 反応テーブルユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 反応テーブルユニットパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1453 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1453()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1453;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// BFテーブルユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// BFテーブルユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0454 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0454()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0454;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// R2攪拌時間
        /// </summary>
        public Double StirringTimeR2 { get; set; } = 0;

        /// <summary>
        /// BF1攪拌時間
        /// </summary>
        public Double StirringTimeBF1 { get; set; } = 0;

        /// <summary>
        /// BF2攪拌時間
        /// </summary>
        public Double StirringTimeBF2 { get; set; } = 0;

        /// <summary>
        /// Pretrigger攪拌時間
        /// </summary>
        public Double StirringTimePretrigger { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.StirringTimeR2.ToString("f1"), 4);
                builder.Append(this.StirringTimeBF1.ToString("f1"), 4);
                builder.Append(this.StirringTimeBF2.ToString("f1"), 4);
                builder.Append(this.StirringTimePretrigger.ToString("f1"), 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Double tmpdata1;
            Double tmpdata2;
            Double tmpdata3;
            Double tmpdata4;
            resultList.Add(text_data.spoilDouble(out tmpdata1, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata2, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata3, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata4, 4));
            this.StirringTimeR2 = tmpdata1;
            this.StirringTimeBF1 = tmpdata2;
            this.StirringTimeBF2 = tmpdata3;
            this.StirringTimePretrigger = tmpdata4;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// BFテーブルユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// BFテーブルユニットパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1454 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1454()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1454;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// トラベラーユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// トラベラーユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0455 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0455()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0455;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 反応容器搬送後遅延時間
        /// </summary>
        public Double WaitTimeAfterReactContainer { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.WaitTimeAfterReactContainer.ToString("f1"), 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Double tmpdata1;

            resultList.Add(text_data.spoilDouble(out tmpdata1, 4));

            this.WaitTimeAfterReactContainer = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// トラベラーユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// トラベラーユニットパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1455 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1455()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1455;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 試薬分注1部ユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// 試薬分注1部ユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0456 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0456()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0456;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬吸引後遅延時間
        /// </summary>
        public Double WaitTimeAfterSuckingUp { get; set; } = 0;

        /// <summary>
        /// 試薬吐出後遅延時間
        /// </summary>
        public Double WaitTimeAfterDispense { get; set; } = 0;

        /// <summary>
        /// プライム回数
        /// </summary>
        public Int32 NoOfPrimeTimes { get; set; } = 0;

        /// <summary>
        /// リンス回数
        /// </summary>
        public Int32 NoOfRinseTimes { get; set; } = 0;

        /// <summary>
        /// プライム量
        /// </summary>
        public Int32 PrimeVolume { get; set; } = 0;

        /// <summary>
        /// 試薬分注量
        /// </summary>
        public Int32 DispenseVolume { get; set; } = 0;

        /// <summary>
        /// 試薬シリンジ待機位置
        /// </summary>
        public Int32 StandbyPosition { get; set; } = 0;

        /// <summary>
        /// 静電容量センサー使用有無
        /// </summary>
        public Byte UsableElecCapaSensor { get; set; } = 0;

        /// <summary>
        /// 試薬吐き残し量
        /// </summary>
        public Int32 VomitVolume { get; set; } = 0;

        /// <summary>
        /// 試薬追い出しエアー量
        /// </summary>
        public Int32 EjectorAirVolume { get; set; } = 0;

        /// <summary>
        /// 試薬プローブ分離エアー量
        /// </summary>
        public Int32 ProbeSeparationAir { get; set; } = 0;

        /// <summary>
        /// 試薬吸引後エアー引き上げ量
        /// </summary>
        public Int32 AirLiftingAfterSuckingUp { get; set; } = 0;

        /// <summary>
        /// 試薬液面オフセット
        /// </summary>
        public Double LiquidSurfaceOffset { get; set; } = 0;

        /// <summary>
        /// ノズル洗浄時間
        /// </summary>
        public Double NozzleWashTime { get; set; } = 0;

        /// <summary>
        /// 気泡混入バルブON時間
        /// </summary>
        public Double BubbleMixingValveONTime { get; set; } = 0;

        /// <summary>
        /// 気泡混入バルブOFF時間
        /// </summary>
        public Double BubbleMixingValveOFFTime { get; set; } = 0;

        /// <summary>
        /// 気泡混入バルブ動作回数
        /// </summary>
        public Int32 BubbleMixingValveFrequency { get; set; } = 0;

        /// <summary>
        /// 洗浄後洗浄液吐出量1
        /// </summary>
        public Int32 WashVomitVolume1 { get; set; } = 0;

        /// <summary>
        /// 洗浄後洗浄液吐出量2
        /// </summary>
        public Int32 WashVomitVolume2 { get; set; } = 0;

        /// <summary>
        /// プライム時間
        /// </summary>
        public Double PrimeTime { get; set; } = 0;

        /// <summary>
        /// 吐出方法
        /// </summary>
        public Byte DispenseMode { get; set; } = 0;

        /// <summary>
        /// 廃液時間
        /// </summary>
        public Double WasteTime { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.WaitTimeAfterSuckingUp.ToString("f1"), 4);
                builder.Append(this.WaitTimeAfterDispense.ToString("f1"), 4);
                builder.Append(this.NoOfPrimeTimes.ToString(), 2);
                builder.Append(this.NoOfRinseTimes.ToString(), 2);
                builder.Append(this.PrimeVolume.ToString(), 4);
                builder.Append(this.DispenseVolume.ToString(), 4);
                builder.Append(this.StandbyPosition.ToString(), 4);
                builder.Append(this.UsableElecCapaSensor.ToString(), 1);
                builder.Append(this.VomitVolume.ToString(), 4);
                builder.Append(this.EjectorAirVolume.ToString(), 4);
                builder.Append(this.ProbeSeparationAir.ToString(), 4);
                builder.Append(this.AirLiftingAfterSuckingUp.ToString(), 4);
                builder.Append(this.LiquidSurfaceOffset.ToString("f1"), 3);
                builder.Append(this.NozzleWashTime.ToString("f1"), 3);
                builder.Append(this.BubbleMixingValveONTime.ToString("f1"), 3);
                builder.Append(this.BubbleMixingValveOFFTime.ToString("f1"), 3);
                builder.Append(this.BubbleMixingValveFrequency.ToString(), 2);
                builder.Append(this.WashVomitVolume1.ToString(), 4);
                builder.Append(this.WashVomitVolume2.ToString(), 4);
                builder.Append(this.PrimeTime.ToString("f1"), 4);
                builder.Append(this.DispenseMode.ToString(), 1);
                builder.Append(this.WasteTime.ToString("f1"), 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Double tmpdata1 = 0;
            Double tmpdata2 = 0;
            Int32 tmpdata3 = 0;
            Int32 tmpdata4 = 0;
            Int32 tmpdata5 = 0;
            Int32 tmpdata6 = 0;
            Int32 tmpdata7 = 0;
            Byte tmpdata8 = 0;
            Int32 tmpdata9 = 0;
            Int32 tmpdata10 = 0;
            Int32 tmpdata11 = 0;
            Int32 tmpdata12 = 0;
            Double tmpdata13 = 0;
            Double tmpdata14 = 0;
            Double tmpdata15 = 0;
            Double tmpdata16 = 0;
            Int32 tmpdata17 = 0;
            Int32 tmpdata18 = 0;
            Int32 tmpdata19 = 0;
            Double tmpdata20 = 0;
            Byte tmpdata21 = 0;
            Double tmpdata22 = 0;

            resultList.Add(text_data.spoilDouble(out tmpdata1, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata2, 4));
            resultList.Add(text_data.spoilInt(out tmpdata3, 2));
            resultList.Add(text_data.spoilInt(out tmpdata4, 2));
            resultList.Add(text_data.spoilInt(out tmpdata5, 4));
            resultList.Add(text_data.spoilInt(out tmpdata6, 4));
            resultList.Add(text_data.spoilInt(out tmpdata7, 4));
            resultList.Add(text_data.spoilByte(out tmpdata8, 1));
            resultList.Add(text_data.spoilInt(out tmpdata9, 4));
            resultList.Add(text_data.spoilInt(out tmpdata10, 4));
            resultList.Add(text_data.spoilInt(out tmpdata11, 4));
            resultList.Add(text_data.spoilInt(out tmpdata12, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata13, 3));
            resultList.Add(text_data.spoilDouble(out tmpdata14, 3));
            resultList.Add(text_data.spoilDouble(out tmpdata15, 3));
            resultList.Add(text_data.spoilDouble(out tmpdata16, 3));
            resultList.Add(text_data.spoilInt(out tmpdata17, 2));
            resultList.Add(text_data.spoilInt(out tmpdata18, 4));
            resultList.Add(text_data.spoilInt(out tmpdata19, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata20, 4));
            resultList.Add(text_data.spoilByte(out tmpdata21, 1));
            resultList.Add(text_data.spoilDouble(out tmpdata22, 4));

            WaitTimeAfterSuckingUp = tmpdata1;
            WaitTimeAfterDispense = tmpdata2;
            NoOfPrimeTimes = tmpdata3;
            NoOfRinseTimes = tmpdata4;
            PrimeVolume = tmpdata5;
            DispenseVolume = tmpdata6;
            StandbyPosition = tmpdata7;
            UsableElecCapaSensor = tmpdata8;
            VomitVolume = tmpdata9;
            EjectorAirVolume = tmpdata10;
            ProbeSeparationAir = tmpdata11;
            AirLiftingAfterSuckingUp = tmpdata12;
            LiquidSurfaceOffset = tmpdata13;
            NozzleWashTime = tmpdata14;
            BubbleMixingValveONTime = tmpdata15;
            BubbleMixingValveOFFTime = tmpdata16;
            BubbleMixingValveFrequency = tmpdata17;
            WashVomitVolume1 = tmpdata18;
            WashVomitVolume2 = tmpdata19;
            PrimeTime = tmpdata20;
            DispenseMode = tmpdata21;
            WasteTime = tmpdata22;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 試薬分注1部ユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬分注1部ユニットパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1456 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1456()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1456;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 試薬分注2部ユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// 試薬分注2部ユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0457 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0457()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0457;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬吸引後遅延時間
        /// </summary>
        public Double WaitTimeAfterSuckingUp { get; set; } = 0;

        /// <summary>
        /// 試薬吐出後遅延時間
        /// </summary>
        public Double WaitTimeAfterDispense { get; set; } = 0;

        /// <summary>
        /// プライム回数
        /// </summary>
        public Int32 NoOfPrimeTimes { get; set; } = 0;

        /// <summary>
        /// リンス回数
        /// </summary>
        public Int32 NoOfRinseTimes { get; set; } = 0;

        /// <summary>
        /// プライム量
        /// </summary>
        public Int32 PrimeVolume { get; set; } = 0;

        /// <summary>
        /// 試薬分注量
        /// </summary>
        public Int32 DispenseVolume { get; set; } = 0;

        /// <summary>
        /// 試薬シリンジ待機位置
        /// </summary>
        public Int32 StandbyPosition { get; set; } = 0;

        /// <summary>
        /// 静電容量センサー使用有無
        /// </summary>
        public Byte UsableElecCapaSensor { get; set; } = 0;

        /// <summary>
        /// 試薬吐き残し量
        /// </summary>
        public Int32 VomitVolume { get; set; } = 0;

        /// <summary>
        /// 試薬追い出しエアー量
        /// </summary>
        public Int32 EjectorAirVolume { get; set; } = 0;

        /// <summary>
        /// 試薬プローブ分離エアー量
        /// </summary>
        public Int32 ProbeSeparationAir { get; set; } = 0;

        /// <summary>
        /// 試薬吸引後エアー引き上げ量
        /// </summary>
        public Int32 AirLiftingAfterSuckingUp { get; set; } = 0;

        /// <summary>
        /// 試薬液面オフセット
        /// </summary>
        public Double LiquidSurfaceOffset { get; set; } = 0;

        /// <summary>
        /// ノズル洗浄時間
        /// </summary>
        public Double NozzleWashTime { get; set; } = 0;

        /// <summary>
        /// 気泡混入バルブON時間
        /// </summary>
        public Double BubbleMixingValveONTime { get; set; } = 0;

        /// <summary>
        /// 気泡混入バルブOFF時間
        /// </summary>
        public Double BubbleMixingValveOFFTime { get; set; } = 0;

        /// <summary>
        /// 気泡混入バルブ動作回数
        /// </summary>
        public Int32 BubbleMixingValveFrequency { get; set; } = 0;

        /// <summary>
        /// 洗浄後洗浄液吐出量1
        /// </summary>
        public Int32 WashVomitVolume1 { get; set; } = 0;

        /// <summary>
        /// 洗浄後洗浄液吐出量2
        /// </summary>
        public Int32 WashVomitVolume2 { get; set; } = 0;

        /// <summary>
        /// プライム時間
        /// </summary>
        public Double PrimeTime { get; set; } = 0;

        /// <summary>
        /// 吐出方法
        /// </summary>
        public Byte DispenseMode { get; set; } = 0;

        /// <summary>
        /// 廃液時間
        /// </summary>
        public Double WasteTime { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.WaitTimeAfterSuckingUp.ToString("f1"), 4);
                builder.Append(this.WaitTimeAfterDispense.ToString("f1"), 4);
                builder.Append(this.NoOfPrimeTimes.ToString(), 2);
                builder.Append(this.NoOfRinseTimes.ToString(), 2);
                builder.Append(this.PrimeVolume.ToString(), 4);
                builder.Append(this.DispenseVolume.ToString(), 4);
                builder.Append(this.StandbyPosition.ToString(), 4);
                builder.Append(this.UsableElecCapaSensor.ToString(), 1);
                builder.Append(this.VomitVolume.ToString(), 4);
                builder.Append(this.EjectorAirVolume.ToString(), 4);
                builder.Append(this.ProbeSeparationAir.ToString(), 4);
                builder.Append(this.AirLiftingAfterSuckingUp.ToString(), 4);
                builder.Append(this.LiquidSurfaceOffset.ToString("f1"), 3);
                builder.Append(this.NozzleWashTime.ToString("f1"), 3);
                builder.Append(this.BubbleMixingValveONTime.ToString("f1"), 3);
                builder.Append(this.BubbleMixingValveOFFTime.ToString("f1"), 3);
                builder.Append(this.BubbleMixingValveFrequency.ToString(), 2);
                builder.Append(this.WashVomitVolume1.ToString(), 4);
                builder.Append(this.WashVomitVolume2.ToString(), 4);
                builder.Append(this.PrimeTime.ToString("f1"), 4);
                builder.Append(this.DispenseMode.ToString(), 1);
                builder.Append(this.WasteTime.ToString("f1"), 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Double tmpdata1 = 0;
            Double tmpdata2 = 0;
            Int32 tmpdata3 = 0;
            Int32 tmpdata4 = 0;
            Int32 tmpdata5 = 0;
            Int32 tmpdata6 = 0;
            Int32 tmpdata7 = 0;
            Byte tmpdata8 = 0;
            Int32 tmpdata9 = 0;
            Int32 tmpdata10 = 0;
            Int32 tmpdata11 = 0;
            Int32 tmpdata12 = 0;
            Double tmpdata13 = 0;
            Double tmpdata14 = 0;
            Double tmpdata15 = 0;
            Double tmpdata16 = 0;
            Int32 tmpdata17 = 0;
            Int32 tmpdata18 = 0;
            Int32 tmpdata19 = 0;
            Double tmpdata20 = 0;
            Byte tmpdata21 = 0;
            Double tmpdata22 = 0;

            resultList.Add(text_data.spoilDouble(out tmpdata1, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata2, 4));
            resultList.Add(text_data.spoilInt(out tmpdata3, 2));
            resultList.Add(text_data.spoilInt(out tmpdata4, 2));
            resultList.Add(text_data.spoilInt(out tmpdata5, 4));
            resultList.Add(text_data.spoilInt(out tmpdata6, 4));
            resultList.Add(text_data.spoilInt(out tmpdata7, 4));
            resultList.Add(text_data.spoilByte(out tmpdata8, 1));
            resultList.Add(text_data.spoilInt(out tmpdata9, 4));
            resultList.Add(text_data.spoilInt(out tmpdata10, 4));
            resultList.Add(text_data.spoilInt(out tmpdata11, 4));
            resultList.Add(text_data.spoilInt(out tmpdata12, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata13, 3));
            resultList.Add(text_data.spoilDouble(out tmpdata14, 3));
            resultList.Add(text_data.spoilDouble(out tmpdata15, 3));
            resultList.Add(text_data.spoilDouble(out tmpdata16, 3));
            resultList.Add(text_data.spoilInt(out tmpdata17, 2));
            resultList.Add(text_data.spoilInt(out tmpdata18, 4));
            resultList.Add(text_data.spoilInt(out tmpdata19, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata20, 4));
            resultList.Add(text_data.spoilByte(out tmpdata21, 1));
            resultList.Add(text_data.spoilDouble(out tmpdata22, 4));

            WaitTimeAfterSuckingUp = tmpdata1;
            WaitTimeAfterDispense = tmpdata2;
            NoOfPrimeTimes = tmpdata3;
            NoOfRinseTimes = tmpdata4;
            PrimeVolume = tmpdata5;
            DispenseVolume = tmpdata6;
            StandbyPosition = tmpdata7;
            UsableElecCapaSensor = tmpdata8;
            VomitVolume = tmpdata9;
            EjectorAirVolume = tmpdata10;
            ProbeSeparationAir = tmpdata11;
            AirLiftingAfterSuckingUp = tmpdata12;
            LiquidSurfaceOffset = tmpdata13;
            NozzleWashTime = tmpdata14;
            BubbleMixingValveONTime = tmpdata15;
            BubbleMixingValveOFFTime = tmpdata16;
            BubbleMixingValveFrequency = tmpdata17;
            WashVomitVolume1 = tmpdata18;
            WashVomitVolume2 = tmpdata19;
            PrimeTime = tmpdata20;
            DispenseMode = tmpdata21;
            WasteTime = tmpdata22;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 試薬分注2部ユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬分注2部ユニットパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1457 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1457()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1457;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// BF1ユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// BF1ユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0458 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0458()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0458;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 洗浄液吸引後遅延時間
        /// </summary>
        public Double WaitTimeAfterSuckingUp { get; set; } = 0;

        /// <summary>
        /// 洗浄液吐出後遅延時間
        /// </summary>
        public Double WaitTimeAfterDispense { get; set; } = 0;

        /// <summary>
        /// 洗浄間隔
        /// </summary>
        public Double Interval { get; set; } = 0;

        /// <summary>
        /// 洗浄回数
        /// </summary>
        public Int32 NoOfWashTimes { get; set; } = 0;

        /// <summary>
        /// プライム回数
        /// </summary>
        public Int32 NoOfPrimeTimes { get; set; } = 0;

        /// <summary>
        /// リンス回数
        /// </summary>
        public Int32 NoOfRinseTimes { get; set; } = 0;

        /// <summary>
        /// プライム量
        /// </summary>
        public Int32 PrimeVolume { get; set; } = 0;

        /// <summary>
        /// 洗浄液量
        /// </summary>
        public Int32 WashVolume { get; set; } = 0;

        /// <summary>
        /// 廃液　洗浄液吸引後遅延時間
        /// </summary>
        public Double WaitTimeWaste { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.WaitTimeAfterSuckingUp.ToString("f1"), 4);
                builder.Append(this.WaitTimeAfterDispense.ToString("f1"), 4);
                builder.Append(this.Interval.ToString("f1"), 4);
                builder.Append(this.NoOfWashTimes.ToString(), 2);
                builder.Append(this.NoOfPrimeTimes.ToString(), 2);
                builder.Append(this.NoOfRinseTimes.ToString(), 2);
                builder.Append(this.PrimeVolume.ToString(), 4);
                builder.Append(this.WashVolume.ToString(), 4);
                builder.Append(this.WaitTimeWaste.ToString("f1"), 3);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Double tmpdata1;
            Double tmpdata2;
            Double tmpdata3;
            Int32 tmpdata4;
            Int32 tmpdata5;
            Int32 tmpdata6;
            Int32 tmpdata7;
            Int32 tmpdata8;
            Double tmpdata9;
            resultList.Add(text_data.spoilDouble(out tmpdata1, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata2, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata3, 4));
            resultList.Add(text_data.spoilInt(out tmpdata4, 2));
            resultList.Add(text_data.spoilInt(out tmpdata5, 2));
            resultList.Add(text_data.spoilInt(out tmpdata6, 2));
            resultList.Add(text_data.spoilInt(out tmpdata7, 4));
            resultList.Add(text_data.spoilInt(out tmpdata8, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata9, 3));
            this.WaitTimeAfterSuckingUp = tmpdata1;
            this.WaitTimeAfterDispense = tmpdata2;
            this.Interval = tmpdata3;
            this.NoOfWashTimes = tmpdata4;
            this.NoOfPrimeTimes = tmpdata5;
            this.NoOfRinseTimes = tmpdata6;
            this.PrimeVolume = tmpdata7;
            this.WashVolume = tmpdata8;
            this.WaitTimeWaste = tmpdata9;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// BF1ユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// BF1ユニットパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1458 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1458()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1458;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// BF2ユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// BF2ユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0459 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0459()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0459;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 洗浄液吸引後遅延時間
        /// </summary>
        public Double WaitTimeAfterSuckingUp { get; set; } = 0;

        /// <summary>
        /// 洗浄液吐出後遅延時間
        /// </summary>
        public Double WaitTimeAfterDispense { get; set; } = 0;

        /// <summary>
        /// 洗浄間隔
        /// </summary>
        public Double Interval { get; set; } = 0;

        /// <summary>
        /// 洗浄回数
        /// </summary>
        public Int32 NoOfWashTimes { get; set; } = 0;

        /// <summary>
        /// プライム回数
        /// </summary>
        public Int32 NoOfPrimeTimes { get; set; } = 0;

        /// <summary>
        /// リンス回数
        /// </summary>
        public Int32 NoOfRinseTimes { get; set; } = 0;

        /// <summary>
        /// プライム量
        /// </summary>
        public Int32 PrimeVolume { get; set; } = 0;

        /// <summary>
        /// 洗浄液量
        /// </summary>
        public Int32 WashVolume { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.WaitTimeAfterSuckingUp.ToString("f1"), 4);
                builder.Append(this.WaitTimeAfterDispense.ToString("f1"), 4);
                builder.Append(this.Interval.ToString("f1"), 4);
                builder.Append(this.NoOfWashTimes.ToString(), 2);
                builder.Append(this.NoOfPrimeTimes.ToString(), 2);
                builder.Append(this.NoOfRinseTimes.ToString(), 2);
                builder.Append(this.PrimeVolume.ToString(), 4);
                builder.Append(this.WashVolume.ToString(), 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Double tmpdata1;
            Double tmpdata2;
            Double tmpdata3;
            Int32 tmpdata4;
            Int32 tmpdata5;
            Int32 tmpdata6;
            Int32 tmpdata7;
            Int32 tmpdata8;
            resultList.Add(text_data.spoilDouble(out tmpdata1, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata2, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata3, 4));
            resultList.Add(text_data.spoilInt(out tmpdata4, 2));
            resultList.Add(text_data.spoilInt(out tmpdata5, 2));
            resultList.Add(text_data.spoilInt(out tmpdata6, 2));
            resultList.Add(text_data.spoilInt(out tmpdata7, 4));
            resultList.Add(text_data.spoilInt(out tmpdata8, 4));
            this.WaitTimeAfterSuckingUp = tmpdata1;
            this.WaitTimeAfterDispense = tmpdata2;
            this.Interval = tmpdata3;
            this.NoOfWashTimes = tmpdata4;
            this.NoOfPrimeTimes = tmpdata5;
            this.NoOfRinseTimes = tmpdata6;
            this.PrimeVolume = tmpdata7;
            this.WashVolume = tmpdata8;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// BF2ユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// BF2ユニットパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1459 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1459()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1459;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 希釈分注ユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// 希釈分注ユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0460 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0460()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0460;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 希釈液吸引後遅延時間
        /// </summary>
        public Double WaitTimeAfterSuckingUp { get; set; } = 0;

        /// <summary>
        /// 希釈液吐出後遅延時間
        /// </summary>
        public Double WaitTimeAfterDispense { get; set; } = 0;

        /// <summary>
        /// 希釈シリンジ待機位置
        /// </summary>
        public Int32 StandbyPosition { get; set; } = 0;

        /// <summary>
        /// プライム回数
        /// </summary>
        public Int32 NoOfDilutiePrimeTimes { get; set; } = 0;

        /// <summary>
        /// リンス回数
        /// </summary>
        public Int32 NoOfDilutieRinseTimes { get; set; } = 0;

        /// <summary>
        /// プライム量
        /// </summary>
        public Int32 DilutiePrimeVolume { get; set; } = 0;

        /// <summary>
        /// 希釈液分注量
        /// </summary>
        public Int32 DispenseVolume { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.WaitTimeAfterSuckingUp.ToString("f1"), 4);
                builder.Append(this.WaitTimeAfterDispense.ToString("f1"), 4);
                builder.Append(this.StandbyPosition.ToString(), 3);
                builder.Append(this.NoOfDilutiePrimeTimes.ToString(), 2);
                builder.Append(this.NoOfDilutieRinseTimes.ToString(), 2);
                builder.Append(this.DilutiePrimeVolume.ToString(), 4);
                builder.Append(this.DispenseVolume.ToString(), 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Double tmpdata1;
            Double tmpdata2;
            Int32 tmpdata3;
            Int32 tmpdata4;
            Int32 tmpdata5;
            Int32 tmpdata6;
            Int32 tmpdata7;
            resultList.Add(text_data.spoilDouble(out tmpdata1, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata2, 4));
            resultList.Add(text_data.spoilInt(out tmpdata3, 3));
            resultList.Add(text_data.spoilInt(out tmpdata4, 2));
            resultList.Add(text_data.spoilInt(out tmpdata5, 2));
            resultList.Add(text_data.spoilInt(out tmpdata6, 4));
            resultList.Add(text_data.spoilInt(out tmpdata7, 4));
            this.WaitTimeAfterSuckingUp = tmpdata1;
            this.WaitTimeAfterDispense = tmpdata2;
            this.StandbyPosition = tmpdata3;
            this.NoOfDilutiePrimeTimes = tmpdata4;
            this.NoOfDilutieRinseTimes = tmpdata5;
            this.DilutiePrimeVolume = tmpdata6;
            this.DispenseVolume = tmpdata7;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 希釈分注ユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 希釈分注ユニットパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1460 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1460()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1460;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// プレトリガユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// プレトリガユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0461 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0461()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0461;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// pTr吸引後遅延時間
        /// </summary>
        public Double WaitTimeAfterSuckingUp { get; set; } = 0;

        /// <summary>
        /// pTr吐出後遅延時間
        /// </summary>
        public Double WaitTimeAfterDispense { get; set; } = 0;

        /// <summary>
        /// pTrシリンジ待機位置
        /// </summary>
        public Int32 StandbyPosition { get; set; } = 0;

        /// <summary>
        /// プライム回数
        /// </summary>
        public Int32 NoOfPrimeTimes { get; set; } = 0;

        /// <summary>
        /// リンス回数
        /// </summary>
        public Int32 NoOfRinseTimes { get; set; } = 0;

        /// <summary>
        /// プライム量
        /// </summary>
        public Int32 PrimeVolume { get; set; } = 0;

        /// <summary>
        /// pTr吐出量
        /// </summary>
        public Int32 DispanseVolume { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.WaitTimeAfterSuckingUp.ToString("f1"), 4);
                builder.Append(this.WaitTimeAfterDispense.ToString("f1"), 4);
                builder.Append(this.StandbyPosition.ToString(), 4);
                builder.Append(this.NoOfPrimeTimes.ToString(), 2);
                builder.Append(this.NoOfRinseTimes.ToString(), 2);
                builder.Append(this.PrimeVolume.ToString(), 4);
                builder.Append(this.DispanseVolume.ToString(), 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Double tmpdata1;
            Double tmpdata2;
            Int32 tmpdata3;
            Int32 tmpdata4;
            Int32 tmpdata5;
            Int32 tmpdata6;
            Int32 tmpdata7;
            resultList.Add(text_data.spoilDouble(out tmpdata1, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata2, 4));
            resultList.Add(text_data.spoilInt(out tmpdata3, 4));
            resultList.Add(text_data.spoilInt(out tmpdata4, 2));
            resultList.Add(text_data.spoilInt(out tmpdata5, 2));
            resultList.Add(text_data.spoilInt(out tmpdata6, 4));
            resultList.Add(text_data.spoilInt(out tmpdata7, 4));
            this.WaitTimeAfterSuckingUp = tmpdata1;
            this.WaitTimeAfterDispense = tmpdata2;
            this.StandbyPosition = tmpdata3;
            this.NoOfPrimeTimes = tmpdata4;
            this.NoOfRinseTimes = tmpdata5;
            this.PrimeVolume = tmpdata6;
            this.DispanseVolume = tmpdata7;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// プレトリガユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// プレトリガユニットパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1461 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1461()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1461;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// トリガ分注測光ユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// トリガ分注測光ユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0462 : CarisXCommCommand
    {
        #region [定数定義]

        /// <summary>
        /// 測光モード
        /// </summary>
        public enum PhotometryModeKind
        {
            /// <summary>
            /// 面積法
            /// </summary>
            AreaMethod = 1,
            /// <summary>
            /// ピーク法
            /// </summary>
            PeakMethod = 2
        }

        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0462()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0462;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// Tr吸引後遅延時間
        /// </summary>
        public Double WaitTimeAfterSuckingUp { get; set; } = 0;

        /// <summary>
        /// Tr吐出後遅延時間
        /// </summary>
        public Double WaitTimeAfterDispense { get; set; } = 0;

        /// <summary>
        /// Trシリンジ待機位置
        /// </summary>
        public Int32 StandbyPosition { get; set; } = 0;

        /// <summary>
        /// プライム回数
        /// </summary>
        public Int32 NoOfPrimeTimes { get; set; } = 0;

        /// <summary>
        /// リンス回数
        /// </summary>
        public Int32 NoOfRinseTimes { get; set; } = 0;

        /// <summary>
        /// プライム量
        /// </summary>
        public Int32 PrimeVolume { get; set; } = 0;

        /// <summary>
        /// Tr吐出量
        /// </summary>
        public Int32 DispanseVolume { get; set; } = 0;

        /// <summary>
        /// 測光モード
        /// </summary>
        public PhotometryModeKind PhotometryMode { get; set; } = PhotometryModeKind.AreaMethod;

        /// <summary>
        /// ゲートタイム
        /// </summary>
        public Int32 ExposureTime { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.WaitTimeAfterSuckingUp.ToString("f1"), 4);
                builder.Append(this.WaitTimeAfterDispense.ToString("f1"), 4);
                builder.Append(this.StandbyPosition.ToString(), 4);
                builder.Append(this.NoOfPrimeTimes.ToString(), 2);
                builder.Append(this.NoOfRinseTimes.ToString(), 2);
                builder.Append(this.PrimeVolume.ToString(), 4);
                builder.Append(this.DispanseVolume.ToString(), 4);
                builder.Append(((Int32)this.PhotometryMode).ToString(), 1);
                builder.Append(this.ExposureTime.ToString(), 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Double tmpdata1;
            Double tmpdata2;
            Int32 tmpdata3;
            Int32 tmpdata4;
            Int32 tmpdata5;
            Int32 tmpdata6;
            Int32 tmpdata7;
            Int32 tmpdata8;
            Int32 tmpdata9;
            resultList.Add(text_data.spoilDouble(out tmpdata1, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata2, 4));
            resultList.Add(text_data.spoilInt(out tmpdata3, 4));
            resultList.Add(text_data.spoilInt(out tmpdata4, 2));
            resultList.Add(text_data.spoilInt(out tmpdata5, 2));
            resultList.Add(text_data.spoilInt(out tmpdata6, 4));
            resultList.Add(text_data.spoilInt(out tmpdata7, 4));
            resultList.Add(text_data.spoilInt(out tmpdata8, 1));
            resultList.Add(text_data.spoilInt(out tmpdata9, 4));
            this.WaitTimeAfterSuckingUp = tmpdata1;
            this.WaitTimeAfterDispense = tmpdata2;
            this.StandbyPosition = tmpdata3;
            this.NoOfPrimeTimes = tmpdata4;
            this.NoOfRinseTimes = tmpdata5;
            this.PrimeVolume = tmpdata6;
            this.DispanseVolume = tmpdata7;
            this.PhotometryMode = (PhotometryModeKind)tmpdata8;
            this.ExposureTime = tmpdata9;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// トリガ分注測光ユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// トリガ分注測光ユニットパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1462 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1462()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1462;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 流体配管ユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// 流体配管ユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0463 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0463()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0463;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// 流体配管ユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 流体配管ユニットパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1463 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1463()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1463;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 警告灯制御コマンド
    /// </summary>
    /// <remarks>
    /// 警告灯制御コマンドデータクラス。
    /// 生成及びパースを行います。
    /// CarisXでは未使用
    /// </remarks>
    public class SlaveCommCommand_0464 : CarisXCommCommand
    {
        #region [定数定義]

        /// <summary>
        /// 警告灯コントロール
        /// </summary>
        public enum CautionCtrlKind
        {
            /// <summary>
            /// OFF
            /// </summary>
            OFF = 0,
            /// <summary>
            /// ON
            /// </summary>
            ON = 1
        }

        /// <summary>
        /// 警告灯色
        /// </summary>
        public enum CautionColorKind
        {
            /// <summary>
            /// 青
            /// </summary>
            Blue = 1,
            /// <summary>
            /// 黄
            /// </summary>
            Yellow = 2,
            /// <summary>
            /// 赤
            /// </summary>
            Red = 3
        }
        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0464()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0464;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 警告灯コントロール
        /// </summary>
        public CautionCtrlKind CautionCtrl { get; set; } = CautionCtrlKind.OFF;

        /// <summary>
        /// 警告灯色
        /// </summary>
        public CautionColorKind CautionColor { get; set; } = CautionColorKind.Blue;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.CautionCtrl).ToString(), 1);
                builder.Append(((Int32)this.CautionColor).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            Int32 tmpdata2;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilInt(out tmpdata2, 1));
            this.CautionCtrl = (CautionCtrlKind)tmpdata1;
            this.CautionColor = (CautionColorKind)tmpdata2;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 警告灯制御コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 警告灯制御コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// CarisXでは未使用
    /// </remarks>
    public class SlaveCommCommand_1464 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1464()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1464;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// ブザー制御コマンド
    /// </summary>
    /// <remarks>
    /// ブザー制御コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0465 : CarisXCommCommand
    {
        #region [定数定義]

        /// <summary>
        /// ブザーコントロール
        /// </summary>
        public enum ControlKind
        {
            /// <summary>
            /// OFF
            /// </summary>
            OFF = 0,
            /// <summary>
            /// ON
            /// </summary>
            ON = 1
        }

        /// <summary>
        /// 音色
        /// </summary>
        public enum SoundKind
        {
            /// <summary>
            /// エラー
            /// </summary>
            Error = 1,
            /// <summary>
            /// 警告
            /// </summary>
            Warning = 2,
            /// <summary>
            /// その他
            /// </summary>
            Other = 3
        }

        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0465()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0465;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ブザーコントロール
        /// </summary>
        public ControlKind Ctrl { get; set; } = ControlKind.OFF;

        /// <summary>
        /// 音色
        /// </summary>
        public SoundKind Sound { get; set; } = SoundKind.Error;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.Ctrl).ToString(), 1);
                builder.Append(((Int32)this.Sound).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            Int32 tmpdata2;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilInt(out tmpdata2, 1));
            this.Ctrl = (ControlKind)tmpdata1;
            this.Sound = (SoundKind)tmpdata2;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// ブザー制御コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ブザー制御コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1465 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1465()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1465;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 簡易プライムコマンド
    /// </summary>
    /// <remarks>
    /// 簡易プライムコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0466 : CarisXCommCommand
    {
        #region [定数定義]

        /// <summary>
        /// プライム識別
        /// </summary>
        public enum PrimeAreaKind
        {
            /// <summary>
            /// 希釈液
            /// </summary>
            Dilution = 1,
            /// <summary>
            /// R1
            /// </summary>
            R1 = 2,
            /// <summary>
            /// R2
            /// </summary>
            R2 = 3,
            /// <summary>
            /// B/F1
            /// </summary>
            BF1 = 4,
            /// <summary>
            /// B/F2
            /// </summary>
            BF2 = 5,
            /// <summary>
            /// プレトリガ
            /// </summary>
            PreTrigger = 6,
            /// <summary>
            /// トリガ
            /// </summary>
            Trigger = 7,
            /// <summary>
            /// 全体
            /// </summary>
            All = 8
        }

        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0466()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0466;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// プライム識別
        /// </summary>
        public PrimeAreaKind PrimeArea { get; set; } = PrimeAreaKind.Dilution;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.PrimeArea).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            PrimeArea = (PrimeAreaKind)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }
    /// <summary>
    /// 簡易プライムコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 簡易プライムコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1466 : CarisXCommCommand
    {
        #region [定数定義]

        /// <summary>
        /// プライム識別
        /// </summary>
        public enum PrimeAreaKind
        {
            /// <summary>
            /// 希釈液
            /// </summary>
            Dilution = 1,
            /// <summary>
            /// R1
            /// </summary>
            R1 = 2,
            /// <summary>
            /// R2
            /// </summary>
            R2 = 3,
            /// <summary>
            /// 洗浄液1
            /// </summary>
            Wash1 = 4,
            /// <summary>
            /// 洗浄液2
            /// </summary>
            Wash2 = 5,
            /// <summary>
            /// プレトリガ
            /// </summary>
            PreTrigger = 6,
            /// <summary>
            /// トリガ
            /// </summary>
            Trigger = 7,
            /// <summary>
            /// 全体
            /// </summary>
            All = 8
        }

        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1466()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1466;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// プライム識別
        /// </summary>
        public PrimeAreaKind PrimeArea { get; set; } = PrimeAreaKind.Dilution;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.PrimeArea).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 簡易プライムコマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// 簡易プライムコマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            PrimeArea = (PrimeAreaKind)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// スレーブ接続確認コマンド
    /// </summary>
    /// <remarks>
    /// スレーブ接続確認コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0467 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0467()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0467;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// スレーブ接続確認コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// スレーブ接続確認コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1467 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1467()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1467;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// サンプル停止要因問い合わせコマンド
    /// </summary>
    /// <remarks>
    /// サンプル停止要因問い合わせコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0468 : RackTransferCommCommand_0068
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0468()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0468;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// サンプル停止要因問い合わせコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// サンプル停止要因問い合わせコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1468 : RackTransferCommCommand_1068
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1468()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1468;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// ラック排出コマンド
    /// </summary>
    /// <remarks>
    /// ラック排出コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0469 : RackTransferCommCommand_0069
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0469()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0469;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// ラック排出コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラック排出コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1469 : RackTransferCommCommand_1069
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1469()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1469;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// モーターパラメータ保存コマンド
    /// </summary>
    /// <remarks>
    /// モーターパラメータ保存コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0471 : RackTransferCommCommand_0071
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0471()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0471;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// モーターパラメータ保存コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// モーターパラメータ保存コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1471 : RackTransferCommCommand_1071
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1471()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1471;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// PID制御開始コマンド
    /// </summary>
    /// <remarks>
    /// PID制御開始コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0472 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0472()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0472;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 温調場所
        /// </summary>
        public PIDTempArea TempArea { get; set; } = PIDTempArea.All;

        /// <summary>
        /// コントロール
        /// </summary>
        public PIDControl Control { get; set; } = PIDControl.Start;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.TempArea).ToString(), 1);
                builder.Append(((Int32)this.Control).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            Int32 tmpdata2;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilInt(out tmpdata2, 1));
            this.TempArea = (PIDTempArea)tmpdata1;
            this.Control = (PIDControl)tmpdata2;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }
    /// <summary>
    /// PID制御開始コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// PID制御開始コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1472 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1472()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1472;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// モーター調整コマンド
    /// </summary>
    /// <remarks>
    /// モーター調整コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0473 : RackTransferCommCommand_0073
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0473()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0473;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// モーター調整コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// モーター調整コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1473 : RackTransferCommCommand_1073
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1473()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1473;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// PID定数設定コマンド
    /// </summary>
    /// <remarks>
    /// PID定数設定コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0474 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0474()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0474;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 温調場所
        /// </summary>
        public PIDTempArea TempArea { get; set; } = PIDTempArea.All;

        /// <summary>
        /// 比例定数設定(1ch)
        /// </summary>
        public Double ProportionalConstValue { get; set; } = 0;

        /// <summary>
        /// 積分定数設定(1ch)
        /// </summary>
        public Double IntegralConstvalue { get; set; } = 0;

        /// <summary>
        /// 微分定数設定(1ch)
        /// </summary>
        public Double DifferentialConstValue { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.TempArea).ToString(), 1);
                builder.Append(this.ProportionalConstValue.ToString(), 6);
                builder.Append(this.IntegralConstvalue.ToString(), 6);
                builder.Append(this.DifferentialConstValue.ToString(), 6);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            Double tmpdata2;
            Double tmpdata3;
            Double tmpdata4;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilDouble(out tmpdata2, 6));
            resultList.Add(text_data.spoilDouble(out tmpdata3, 6));
            resultList.Add(text_data.spoilDouble(out tmpdata4, 6));
            this.TempArea = (PIDTempArea)tmpdata1;
            this.ProportionalConstValue = tmpdata2;
            this.IntegralConstvalue = tmpdata3;
            this.DifferentialConstValue = tmpdata4;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// PID定数設定コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// PID定数設定コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1474 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1474()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1474;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 残量クリアコマンド
    /// </summary>
    /// <remarks>
    /// 残量クリアコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0475 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0475()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0475;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// 残量クリアコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 残量クリアコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1475 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1475()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1475;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 準備中断コマンド
    /// </summary>
    /// <remarks>
    /// 準備中断コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0476 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0476()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0476;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// 準備中断コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 準備中断コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1476 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1476()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1476;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// カレンダーコマンド
    /// </summary>
    /// <remarks>
    /// カレンダーコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0477 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0477()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0477;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// カレンダー
        /// </summary>
        public Int32 Time { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.Time.ToString(), 8);
                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 8));
            this.Time = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// カレンダーコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// カレンダーコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1477 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1477()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1477;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// ユニット無効コマンド
    /// </summary>
    /// <remarks>
    /// ユニット無効コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0478 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0478()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0478;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ケース搬送(05)
        /// </summary>
        public Byte UsableCaseUnit { get; set; } = 0;

        /// <summary>
        /// 試薬保冷庫(06)
        /// </summary>
        public Byte UsableReagCoolerUnit { get; set; } = 0;

        /// <summary>
        /// スタット部(07)
        /// </summary>
        public Byte UsableSTATUnit { get; set; } = 0;

        /// <summary>
        /// サンプル分注移送部(08)
        /// </summary>
        public Byte UsableSampleDispenseUnit { get; set; } = 0;

        /// <summary>
        /// 反応容器搬送部(09)
        /// </summary>
        public Byte UsableReactionCellUnit { get; set; } = 0;

        /// <summary>
        /// 反応テーブル部(10)
        /// </summary>
        public Byte UsableReactionTableUnit { get; set; } = 0;

        /// <summary>
        /// BFテーブル部(11)
        /// </summary>
        public Byte UsableBFTableUnit { get; set; } = 0;

        /// <summary>
        /// トラベラー・廃棄部(13)
        /// </summary>
        public Byte UsableTravelerDisposalUnit { get; set; } = 0;

        /// <summary>
        /// トリガ分注・化学発光測定部(14)
        /// </summary>
        public Byte UsableTriggerPtotometryUnit { get; set; } = 0;

        /// <summary>
        /// 試薬分注1部(15)
        /// </summary>
        public Byte UsableR1Unit { get; set; } = 0;

        /// <summary>
        /// 試薬分注2部(16)
        /// </summary>
        public Byte UsableR2Unit { get; set; } = 0;

        /// <summary>
        /// BF1部(17)
        /// </summary>
        public Byte UsableBF1Unit { get; set; } = 0;

        /// <summary>
        /// BF2部(19)
        /// </summary>
        public Byte UsableBF2Unit { get; set; } = 0;

        /// <summary>
        /// 希釈液分注部(20)
        /// </summary>
        public Byte UsableDiluentUnit { get; set; } = 0;

        /// <summary>
        /// プレトリガ分注(21)
        /// </summary>
        public Byte UsablePreTriggerUnit { get; set; } = 0;

        /// <summary>
        /// 流体配管部(23)
        /// </summary>
        public Byte UsableFluidPipingUnit { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.UsableCaseUnit.ToString(), 1);
                builder.Append(this.UsableReagCoolerUnit.ToString(), 1);
                builder.Append(this.UsableSTATUnit.ToString(), 1);
                builder.Append(this.UsableSampleDispenseUnit.ToString(), 1);
                builder.Append(this.UsableReactionCellUnit.ToString(), 1);
                builder.Append(this.UsableReactionTableUnit.ToString(), 1);
                builder.Append(this.UsableBFTableUnit.ToString(), 1);
                builder.Append(this.UsableTravelerDisposalUnit.ToString(), 1);
                builder.Append(this.UsableTriggerPtotometryUnit.ToString(), 1);
                builder.Append(this.UsableR1Unit.ToString(), 1);
                builder.Append(this.UsableR2Unit.ToString(), 1);
                builder.Append(this.UsableBF1Unit.ToString(), 1);
                builder.Append(this.UsableBF2Unit.ToString(), 1);
                builder.Append(this.UsableDiluentUnit.ToString(), 1);
                builder.Append(this.UsablePreTriggerUnit.ToString(), 1);
                builder.Append(this.UsableFluidPipingUnit.ToString(), 1);
                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            Byte tmpdata2;
            Byte tmpdata3;
            Byte tmpdata4;
            Byte tmpdata5;
            Byte tmpdata6;
            Byte tmpdata7;
            Byte tmpdata8;
            Byte tmpdata9;
            Byte tmpdata10;
            Byte tmpdata11;
            Byte tmpdata12;
            Byte tmpdata13;
            Byte tmpdata14;
            Byte tmpdata15;
            Byte tmpdata16;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            resultList.Add(text_data.spoilByte(out tmpdata3, 1));
            resultList.Add(text_data.spoilByte(out tmpdata4, 1));
            resultList.Add(text_data.spoilByte(out tmpdata5, 1));
            resultList.Add(text_data.spoilByte(out tmpdata6, 1));
            resultList.Add(text_data.spoilByte(out tmpdata7, 1));
            resultList.Add(text_data.spoilByte(out tmpdata8, 1));
            resultList.Add(text_data.spoilByte(out tmpdata9, 1));
            resultList.Add(text_data.spoilByte(out tmpdata10, 1));
            resultList.Add(text_data.spoilByte(out tmpdata11, 1));
            resultList.Add(text_data.spoilByte(out tmpdata12, 1));
            resultList.Add(text_data.spoilByte(out tmpdata13, 1));
            resultList.Add(text_data.spoilByte(out tmpdata14, 1));
            resultList.Add(text_data.spoilByte(out tmpdata15, 1));
            resultList.Add(text_data.spoilByte(out tmpdata16, 1));
            this.UsableCaseUnit = tmpdata1;
            this.UsableReagCoolerUnit = tmpdata2;
            this.UsableSTATUnit = tmpdata3;
            this.UsableSampleDispenseUnit = tmpdata4;
            this.UsableReactionCellUnit = tmpdata5;
            this.UsableReactionTableUnit = tmpdata6;
            this.UsableBFTableUnit = tmpdata7;
            this.UsableTravelerDisposalUnit = tmpdata8;
            this.UsableTriggerPtotometryUnit = tmpdata9;
            this.UsableR1Unit = tmpdata10;
            this.UsableR2Unit = tmpdata11;
            this.UsableBF1Unit = tmpdata12;
            this.UsableBF2Unit = tmpdata13;
            this.UsableDiluentUnit = tmpdata14;
            this.UsablePreTriggerUnit = tmpdata15;
            this.UsableFluidPipingUnit = tmpdata16;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// ユニット無効コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ユニット無効コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1478 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1478()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1478;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 試薬ロットサンプル停止要因解除コマンド
    /// </summary>
    /// <remarks>
    /// 試薬ロットサンプル停止要因解除コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0479 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0479()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0479;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// 試薬ロットサンプル停止要因解除コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬ロットサンプル停止要因解除コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1479 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1479()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1479;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 調整位置停止コマンド
    /// </summary>
    /// <remarks>
    /// 調整位置停止コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0480 : RackTransferCommCommand_0080
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0480()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0480;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// 調整位置停止コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 調整位置停止コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1480 : RackTransferCommCommand_1080
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1480()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1480;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 調整位置再開コマンド
    /// </summary>
    /// <remarks>
    /// 調整位置再開コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0481 : RackTransferCommCommand_0081
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0481()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0481;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// 調整位置再開コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 調整位置再開コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1481 : RackTransferCommCommand_1081
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        ///　コンストラクタ
        /// </summary>
        public SlaveCommCommand_1481()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1481;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// シリンジエージングコマンド
    /// </summary>
    /// <remarks>
    /// シリンジエージングコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0483 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0483()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0483;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// シリンジモーター番号
        /// </summary>
        public Int32 NoCyMotor { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.NoCyMotor.ToString(), 3);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 3));
            this.NoCyMotor = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }
    /// <summary>
    /// シリンジエージングコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// シリンジエージングコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1483 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1483()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1483;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// シリンジモーター番号
        /// </summary>
        public Int32 NoCyMotor { get; set; } = 0;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.NoCyMotor.ToString(), 3);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 3));
            this.NoCyMotor = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// 総アッセイ数設定コマンド
    /// </summary>
    /// <remarks>
    /// 総アッセイ数設定コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0484 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0484()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0484;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// アッセイ数
        /// </summary>
        public Int32 NoOfAssay { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.NoOfAssay.ToString(), 8);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 8));
            this.NoOfAssay = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }
    /// <summary>
    /// 総アッセイ数設定コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 総アッセイ数設定コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1484 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1484()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1484;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// サンプル必要量コマンド
    /// </summary>
    /// <remarks>
    /// サンプル必要量コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0485 : CarisXCommCommand, ISampleReqAmount
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0485()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0485;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// Aテーブル
        /// </summary>
        public SampleAmountReqTableA TableA { get; set; } = new SampleAmountReqTableA();

        /// <summary>
        /// Bテーブル
        /// </summary>
        public SampleAmountReqTableB TableB { get; set; } = new SampleAmountReqTableB();

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.TableA.HighOfDeadVolForCup.ToString("f3"), 5);
                builder.Append(this.TableA.HighOfDeadVolForTubeWithRubber.ToString("f3"), 5);
                builder.Append(this.TableA.HighOfDeadVolForTube.ToString("f3"), 5);
                builder.Append(this.TableA.HighOfDeadVolForCupOnTube.ToString("f3"), 5);

                Int32 i;
                for (i = 0; i < this.TableB.ColA.Count(); i++)
                {
                    builder.Append(this.TableB.ColA[i].ToString("f3"), 5);
                }

                for (i = 0; i < this.TableB.ColB.Count(); i++)
                {
                    builder.Append(this.TableB.ColB[i].ToString("f3"), 5);
                }

                for (i = 0; i < this.TableB.ColC.Count(); i++)
                {
                    builder.Append(this.TableB.ColC[i].ToString("f3"), 5);
                }

                for (i = 0; i < this.TableB.ColD.Count(); i++)
                {
                    builder.Append(this.TableB.ColD[i].ToString("f3"), 5);
                }

                for (i = 0; i < this.TableB.ColE.Count(); i++)
                {
                    builder.Append(this.TableB.ColE[i].ToString("f3"), 5);
                }

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            resultList.Add(text_data.spoilDouble(out this.TableA.HighOfDeadVolForCup, 5));
            resultList.Add(text_data.spoilDouble(out this.TableA.HighOfDeadVolForTubeWithRubber, 5));
            resultList.Add(text_data.spoilDouble(out this.TableA.HighOfDeadVolForTube, 5));
            resultList.Add(text_data.spoilDouble(out this.TableA.HighOfDeadVolForCupOnTube, 5));

            Int32 i;
            for (i = 0; i < this.TableB.ColA.Count(); i++)
            {
                resultList.Add(text_data.spoilDouble(out this.TableB.ColA[i], 5));
            }

            for (i = 0; i < this.TableB.ColB.Count(); i++)
            {
                resultList.Add(text_data.spoilDouble(out this.TableB.ColB[i], 5));
            }

            for (i = 0; i < this.TableB.ColC.Count(); i++)
            {
                resultList.Add(text_data.spoilDouble(out this.TableB.ColC[i], 5));
            }

            for (i = 0; i < this.TableB.ColD.Count(); i++)
            {
                resultList.Add(text_data.spoilDouble(out this.TableB.ColD[i], 5));
            }

            for (i = 0; i < this.TableB.ColE.Count(); i++)
            {
                resultList.Add(text_data.spoilDouble(out this.TableB.ColE[i], 5));
            }

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// サンプル必要量コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// サンプル必要量コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1485 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1485()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1485;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

    }

    /// <summary>
    /// 分析強制終了コマンド
    /// </summary>
    /// <remarks>
    /// 分析強制終了コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0486 : RackTransferCommCommand_0086
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0486()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0486;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// 分析強制終了コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 分析強制終了コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1486 : RackTransferCommCommand_1086
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1486()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1486;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 試薬保冷庫テーブル移動コマンド
    /// </summary>
    /// <remarks>
    /// 試薬保冷庫テーブル移動コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0487 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0487()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0487;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// パラメータ
        /// </summary>
        public Int32 PortNumber { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.PortNumber.ToString(), 2);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 2));
            this.PortNumber = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }
    /// <summary>
    /// 試薬保冷庫テーブル移動コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬保冷庫テーブル移動コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1487 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1487()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1487;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// パラメータ
        /// </summary>
        public Int32 PortNumber { get; set; } = 0;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.PortNumber.ToString(), 2);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 試薬保冷庫テーブル移動コマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// 試薬保冷庫テーブル移動コマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 2));
            this.PortNumber = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// ラック設置有無確認コマンド
    /// </summary>
    /// <remarks>
    /// ラック設置有無確認コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0488 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0488()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0488;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// ラック設置有無確認コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラック設置有無確認コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1488 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1488()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1488;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラック引込奥
        /// </summary>
        public SampleRackData RackRetractionBack { get; set; } = new SampleRackData();

        /// <summary>
        /// ラック引込手前
        /// </summary>
        public SampleRackData RackRetractionFront { get; set; } = new SampleRackData();

        /// <summary>
        /// STAT
        /// </summary>
        public STATData STATInstallation { get; set; } = new STATData();

        /// <summary>
        /// 外部搬送
        /// </summary>
        public OutsideTransfer OutsideTransfer { get; set; } = new OutsideTransfer();

        /// <summary>
        /// 順番
        /// </summary>
        public Int32 [] TurnOrder { get; set; } = new Int32 [4];

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                Int32 i = 0;
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackRetractionBack.RackID.ToString(), 4);
                for (i = 0; i < this.RackRetractionBack.SampleID.Count(); i++)
                {
                    builder.Append(this.RackRetractionBack.SampleID [i].ToString(), 7);
                }
                builder.Append(( (Int32)this.RackRetractionBack.RackStatus ).ToString(), 1);
                builder.Append(( this.RackRetractionBack.NumRemainingCycles ).ToString(), 5);

                builder.Append(this.RackRetractionFront.RackID.ToString(), 4);
                for (i = 0; i < this.RackRetractionFront.SampleID.Count(); i++)
                {
                    builder.Append(this.RackRetractionFront.SampleID [i].ToString(), 7);
                }
                builder.Append(( (Int32)this.RackRetractionFront.RackStatus ).ToString(), 1);
                builder.Append(( this.RackRetractionFront.NumRemainingCycles ).ToString(), 5);

                for (i = 0; i < this.STATInstallation.SampleID.Count(); i++)
                {
                    builder.Append(this.STATInstallation.SampleID [i].ToString(), 7);
                }
                builder.Append(( (Int32)this.STATInstallation.RackStatus ).ToString(), 1);
                builder.Append(( this.STATInstallation.NumRemainingCycles ).ToString(), 5);

                for (i = 0; i < this.OutsideTransfer.SampleID.Count(); i++)
                {
                    builder.Append(this.OutsideTransfer.SampleID [i].ToString(), 7);
                }
                builder.Append(( (Int32)this.OutsideTransfer.RackStatus ).ToString(), 1);
                builder.Append(( this.OutsideTransfer.NumRemainingCycles ).ToString(), 5);

                for (i = 0; i < this.TurnOrder.Count(); i++)
                {
                    builder.Append(this.TurnOrder [i].ToString(), 1);
                }

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// ラック設置有無確認コマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// ラック設置有無確認コマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 i = 0;
            Byte tmpdata1;
            Byte tmpdata2;
            Byte tmpdata3;
            String tmpdata4;
            Int32 tmpdata5 = 0;
            Byte tmpdat6;

            //ラック引込奥
            resultList.Add(text_data.spoilString(out this.RackRetractionBack.RackID, 4));
            for (i = 0; i < this.RackRetractionBack.SampleID.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out this.RackRetractionBack.SampleID [i], 7));
            }
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilInt(out tmpdata5, 5));

            tmpdata5 = 0;

            //ラック引込手前
            resultList.Add(text_data.spoilString(out this.RackRetractionFront.RackID, 4));
            for (i = 0; i < this.RackRetractionFront.SampleID.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out this.RackRetractionFront.SampleID [i], 7));
            }
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            resultList.Add(text_data.spoilInt(out tmpdata5, 5));

            tmpdata5 = 0;

            //STAT
            for (i = 0; i < this.STATInstallation.SampleID.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out this.STATInstallation.SampleID [i], 7));
            }
            resultList.Add(text_data.spoilByte(out tmpdata3, 1));
            resultList.Add(text_data.spoilInt(out tmpdata5, 5));

            tmpdata5 = 0;

            // 外部搬送
            for (i = 0; i < this.OutsideTransfer.SampleID.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out this.OutsideTransfer.SampleID [i], 7));
            }
            resultList.Add(text_data.spoilByte(out tmpdat6, 1));
            resultList.Add(text_data.spoilInt(out tmpdata5, 5));

            //順番
            for (i = 0; i < this.TurnOrder.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out this.TurnOrder [i], 1));
            }

            resultList.Add(text_data.spoilString(out tmpdata4, 4));

            this.RackRetractionBack.RackStatus = (RackStatus)tmpdata1;
            this.RackRetractionFront.RackStatus = (RackStatus)tmpdata2;
            this.STATInstallation.RackStatus = (RackStatus)tmpdata3;
            this.OutsideTransfer.RackStatus = (RackStatus)tmpdat6;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// ラック設置状況上書きコマンド
    /// </summary>
    /// <remarks>
    /// ラック設置状況上書きコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0489 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0489()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0489;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラック引込奥
        /// </summary>
        public SampleRackData RackRetractionBack { get; set; } = new SampleRackData();

        /// <summary>
        /// ラック引込手前
        /// </summary>
        public SampleRackData RackRetractionFront { get; set; } = new SampleRackData();

        /// <summary>
        /// STAT
        /// </summary>
        public STATData STATInstallation { get; set; } = new STATData();

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                Int32 i = 0;
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackRetractionBack.RackID.ToString(), 4);

                for (i = 0; i < this.RackRetractionBack.SampleID.Count(); i++)
                {
                    builder.Append(this.RackRetractionBack.SampleID[i].ToString(), 7);
                }

                builder.Append(((Int32)this.RackRetractionBack.RackStatus).ToString(), 1);

                builder.Append(this.RackRetractionFront.RackID.ToString(), 4);

                for (i = 0; i < this.RackRetractionFront.SampleID.Count(); i++)
                {
                    builder.Append(this.RackRetractionFront.SampleID[i].ToString(), 7);
                }

                builder.Append(((Int32)this.RackRetractionFront.RackStatus).ToString(), 1);

                for (i = 0; i < this.STATInstallation.SampleID.Count(); i++)
                {
                    builder.Append(this.STATInstallation.SampleID[i].ToString(), 7);
                }

                builder.Append(((Int32)this.STATInstallation.RackStatus).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            Byte tmpdata2;
            Byte tmpdata3;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));

            Int32 i = 0;
            resultList.Add(text_data.spoilString(out this.RackRetractionBack.RackID, 4));
            for (i = 0; i < this.RackRetractionBack.SampleID.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out this.RackRetractionBack.SampleID[i], 7));
            }
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));

            resultList.Add(text_data.spoilString(out this.RackRetractionFront.RackID, 4));
            for (i = 0; i < this.RackRetractionFront.SampleID.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out this.RackRetractionFront.SampleID[i], 7));
            }
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));

            for (i = 0; i < this.STATInstallation.SampleID.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out this.STATInstallation.SampleID[i], 7));
            }
            resultList.Add(text_data.spoilByte(out tmpdata3, 1));

            this.RackRetractionBack.RackStatus = (RackStatus)tmpdata1;
            this.RackRetractionFront.RackStatus = (RackStatus)tmpdata2;
            this.STATInstallation.RackStatus = (RackStatus)tmpdata3;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// ラック設置状況上書きコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラック設置状況上書きコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1489 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1489()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1489;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 再検コマンド
    /// </summary>
    /// <remarks>
    /// 再検コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0490 : CarisXCommCommand
    {
        /// <summary>
        /// ラックID・ラック位置
        /// </summary>
        public class RackIdPos
        {
            /// <summary>
            /// ラックID
            /// </summary>
            public CarisXIDString rackId;
            /// <summary>
            /// ラックポジション
            /// </summary>
            public Int32 rackPos = 0;
            public RackIdPos(CarisXIDString rackId, Int32 rackPos)
            {
                this.rackId = rackId;
                this.rackPos = rackPos;
            }
        }

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0490()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0490;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 再検ポジション数
        /// </summary>
        public Int32 RetestPosCount
        {
            get
            {
                return this.RackIDPosList.Count;
            }
        }

        /// <summary>
        /// ラックID・ポジション
        /// </summary>
        public List<RackIdPos> RackIDPosList { get; } = new List<RackIdPos>();

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackIDPosList.Count.ToString(), 3);

                foreach (var idPos in this.RackIDPosList)
                {
                    builder.Append(idPos.rackId.DispPreCharString, 4);
                    builder.Append(idPos.rackPos.ToString(), 1);
                }

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            String tmpdata2;
            int tmpdata3;
            resultList.Add(text_data.spoilInt(out tmpdata1, 3));

            for (int i = 0; i < tmpdata1; i++)
            {
                resultList.Add(text_data.spoilString(out tmpdata2, 4));
                resultList.Add(text_data.spoilInt(out tmpdata3, 1));
                this.RackIDPosList.Add(new RackIdPos(tmpdata2, tmpdata3));
            }

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 再検コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 再検コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1490 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1490()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1490;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// STAT状態通知コマンド
    /// </summary>
    /// <remarks>
    /// STAT状態通知コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0491 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0491()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0491;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 要求
        /// </summary>
        public STATStatusRequest Request { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append( ( (Int32)this.Request ).ToString(), 1 );

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if SIMULATOR
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            Request = (STATStatusRequest)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// STAT状態通知コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// STAT状態通知コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1491 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1491()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1491;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 実施結果
        /// </summary>
        public Int32 Result { get; set; }

#if SIMULATOR
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.Result.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString( String commandStr )
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString( commandStr );

            TextData text_data = new TextData( commandStr );

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add( text_data.spoilInt( out tmpdata1, 1 ) );
            Result = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains( false );

            return setSuccess;
        }
        #endregion
    }

    /// <summary>
    /// 試薬保冷庫BC読み込み無効コマンド
    /// </summary>
    /// <remarks>
    /// 試薬保冷庫BC読み込み無効コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0493 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0493()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0493;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ポート番号
        /// </summary>
        public int[] ReadReagBC { get; set; } = new int[CarisXConst.REAGENT_PORT_MAX];

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();

                foreach (Int32 portReadReagBC in this.ReadReagBC)
                {
                    builder.Append(portReadReagBC.ToString(), 1);
                }

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion
    }
    /// <summary>
    /// 試薬保冷庫BC読み込み無効コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬保冷庫BC読み込み無効コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1493 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1493()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1493;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 試薬保冷庫テーブルSW移動許可コマンド
    /// </summary>
    /// <remarks>
    /// 試薬保冷庫テーブルSW移動許可コマンドクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0494 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0494()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0494;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]
        /// <summary>
        /// ポート番号
        /// </summary>
        public Byte SwParam { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.SwParam.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            this.SwParam = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 試薬保冷庫テーブルSW移動許可コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬保冷庫テーブルSW移動許可コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1494 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1494()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1494;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 洗浄液供給コマンド
    /// </summary>
    /// <remarks>
    /// 洗浄液供給コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0495 : CarisXCommCommand
    {
        #region [ 定数定義 ]
        /// <summary>
        /// 洗浄液供給コマンド指定パラメータ
        /// </summary>
        public enum WashSolutionSupplyStatus
        {
            /// <summary>
            /// 中断
            /// </summary>
            Stop = 0,
            /// <summary>
            /// 開始
            /// </summary>
            Start = 1
        }
        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0495()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0495;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]
        /// <summary>
        /// 状態
        /// </summary>
        public WashSolutionSupplyStatus Status { get; set; } = WashSolutionSupplyStatus.Stop;

        /// <summary>
        /// 種別
        /// </summary>
        public TankBufferKind tankBufferKind { get; set; } = TankBufferKind.Tank;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((int)this.Status).ToString(), 1);
                builder.Append(((int)this.tankBufferKind).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if SIMULATOR
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            Byte tmpdata2;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            this.Status = (WashSolutionSupplyStatus)tmpdata1;
            this.tankBufferKind = (TankBufferKind)tmpdata2;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 洗浄液供給コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 洗浄液供給コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1495 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1495()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1495;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// プローブ交換コマンド
    /// </summary>
    /// <remarks>
    /// プローブ交換を行います。
    /// </remarks>
    public class SlaveCommCommand_0497 : CarisXCommCommand
    {
        #region [ 定数定義 ]
        /// <summary>
        /// 試薬プローブ交換コマンド指定パラメータ
        /// </summary>
        public enum probeUnit
        {
            /// <summary>
            /// R1Unit
            /// </summary>
            R1Unit = 1,
            /// <summary>
            /// R2Unit
            /// </summary>
            R2Unit = 2
        }
        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0497()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0497;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]
        /// <summary>
        /// 状態
        /// </summary>
        public probeUnit Status { get; set; } = probeUnit.R1Unit;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.Status).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if SIMULATOR

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;

            resultList.Add(text_data.spoilInt(out tmpdata1, 1));

            this.Status = (probeUnit)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
        
#endif

        #endregion
    }
    /// <summary>
    /// 試薬プローブ交換コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬プローブ交換コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1497 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1497()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1497;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 実施結果
        /// </summary>
        public Int32 Result { get; set; } = 0;

        /// <summary>
        /// オフセット
        /// </summary>
        public Double Offset { get; set; } = 0;

#if SIMULATOR
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.Result.ToString(), 1);
                builder.Append(this.Offset.ToString(), 5);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 試薬プローブ交換コマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// 試薬プローブ交換コマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            Double tmpdata2 = 0;

            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilDouble(out tmpdata2, 5));

            this.Result = tmpdata1;
            this.Offset = tmpdata2;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// ステータス問い合わせコマンド
    /// </summary>
    /// <remarks>
    /// ステータス問い合わせコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0498 : RackTransferCommCommand_0098
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0498()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0498;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// ステータス問い合わせコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ステータス問い合わせコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1498 : RackTransferCommCommand_1098
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1498()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1498;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    #endregion

    #region Slave→User

    /// <summary>
    /// サブレディコマンド
    /// </summary>
    /// <remarks>
    /// サブレディコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0501 : RackTransferCommCommand_0101
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0501()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0501;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 測定指示情報問合せコマンド
    /// </summary>
    /// <remarks>
    /// 測定指示情報問合せコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0502 : CarisXCommCommand
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 検体ID
        /// </summary>
        private String sampleId = String.Empty;

        /// <summary>
        /// ラックID
        /// </summary>
        private String rackId = String.Empty;

        private String reagentLot = String.Empty;
        private String calbratorConcentration = String.Empty;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0502()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0502;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID { get; set; } = "";

        /// <summary>
        /// 検体種別
        /// </summary>
        public SampleMoveSourceKind SampleMoveSource { get; set; } = 0;

        /// <summary>
        /// 検体ポジション
        /// </summary>
        public Int32 SamplePosition { get; set; } = 0;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackID, 4);
                builder.Append(((int)this.SampleMoveSource).ToString(), 1);
                builder.Append(this.SamplePosition.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            String tmpdata1;
            Int32 tmpdata2;
            Int32 tmpdata3;

            resultList.Add(text_data.spoilString(out tmpdata1, 4));
            resultList.Add(text_data.spoilInt(out tmpdata2, 1));
            resultList.Add(text_data.spoilInt(out tmpdata3, 1));

            RackID = tmpdata1;
            SampleMoveSource = (SampleMoveSourceKind)tmpdata2;
            SamplePosition = tmpdata3;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }
    /// <summary>
    /// 測定指示データ問い合わせコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 測定指示データ問い合わせコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1502 : CarisXCommCommand, IMeasureIndicate
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1502()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1502;
            this.CommandId = (int)this.CommKind;
            // TODO:可変調データ、測定項目数」最大値50でnewしてメモリ確保しています
            this.MeasItemArray = new MeasItem[MeasItemCount];

        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID { get; set; } = "";

        /// <summary>
        /// 検体区分
        /// </summary>
        public SampleKind SampleType { get; set; } = SampleKind.Sample;

        /// <summary>
        /// 検体ポジション
        /// </summary>
        public Int32 SamplePosition { get; set; } = 0;

        /// <summary>
        /// 検体識別番号
        /// </summary>
        public Int32 IndividuallyNumber { get; set; } = 0;

        /// <summary>
        /// 検体ID
        /// </summary>
        public String SampleID { get; set; } = String.Empty;

        /// <summary>
        /// サンプル種別
        /// </summary>
        public SpecimenMaterialType SpecimenMaterial { get; set; } = SpecimenMaterialType.BloodSerumAndPlasma;

        /// <summary>
        /// 手希釈倍率
        /// </summary>
        public Int32 PreDil { get; set; } = 0;

        /// <summary>
        /// 測定項目数
        /// </summary>
        public Int32 MeasItemCount { get; set; } = 0;

        /// <summary>
        /// カップタイプ
        /// </summary>
        public SpecimenCupKind SpecimenCup { get; set; } = SpecimenCupKind.None;

        /// <summary>
        /// 測定項目リスト
        /// </summary>
        public MeasItem[] MeasItemArray { get; set; }

        /// <summary>
        /// モジュールID
        /// </summary>
        /// <remarks>
        /// 内部処理上欲しい為追加。コマンドのやり取りで送受信は行わない
        /// </remarks>
        public Int32 ModuleID { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackID, 4);
                builder.Append(((Int32)this.SampleType).ToString(), 1);
                builder.Append(this.SamplePosition.ToString(), 1);
                builder.Append(this.IndividuallyNumber.ToString(), 7);
                builder.Append(this.SampleID, 16);
                builder.Append(((Int32)this.SpecimenMaterial).ToString(), 1);
                builder.Append(this.PreDil.ToString(), 4);
                builder.Append(this.MeasItemCount.ToString(), 2);
                builder.Append(((Int32)this.SpecimenCup).ToString(), 1);

                for (Int32 i = 0; i < MeasItemCount; i++)
                {
                    builder.Append(this.MeasItemArray[i].UniqNo.ToString(), 7);
                    builder.Append(this.MeasItemArray[i].TurnNo.ToString(), 2);
                    builder.Append(this.MeasItemArray[i].ProtoNo.ToString(), 3);
                    builder.Append(this.MeasItemArray[i].RepCount.ToString(), 2);
                    builder.Append(this.MeasItemArray[i].AutoDil.ToString(), 4);

                    if (!string.IsNullOrEmpty(this.MeasItemArray[i].ReagentLotNo))
                    {
                        builder.Append(int.Parse(this.MeasItemArray[i].ReagentLotNo).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.MeasItemArray[i].ReagentLotNo, 8);
                    }
                }

                //builder.Append(this.IndividuallyNumber.ToString(), 7);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

#if DEBUG
        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            String tmpdata1 = "";
            Byte tmpdata2 = 0;
            Int32 tmpdata3 = 0;
            Int32 tmpdata4 = 0;
            String tmpdata5 = "";
            Byte tmpdata6 = 0;
            Int32 tmpdata7 = 0;
            Int32 tmpdata8 = 0;
            Byte tmpdata9 = 0;
            resultList.Add(text_data.spoilString(out tmpdata1, 4));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            resultList.Add(text_data.spoilInt(out tmpdata3, 1));
            resultList.Add(text_data.spoilInt(out tmpdata4, 7));
            resultList.Add(text_data.spoilString(out tmpdata5, 16));
            resultList.Add(text_data.spoilByte(out tmpdata6, 1));
            resultList.Add(text_data.spoilInt(out tmpdata7, 4));
            resultList.Add(text_data.spoilInt(out tmpdata8, 2));
            resultList.Add(text_data.spoilByte(out tmpdata9, 1));

            this.RackID = tmpdata1;
            this.SampleType = (SampleKind)tmpdata2;
            this.SamplePosition = tmpdata3;
            this.IndividuallyNumber = tmpdata4;
            this.SampleID = tmpdata5;
            this.SpecimenMaterial = (SpecimenMaterialType)tmpdata6;
            this.PreDil = tmpdata7;
            this.MeasItemCount = tmpdata8;
            this.SpecimenCup = (SpecimenCupKind)tmpdata9;

            MeasItemArray = new MeasItem[MeasItemCount];
            for (Int32 i = 0; i < MeasItemCount; i++)
            {
                MeasItemArray[i] = new MeasItem();
                resultList.Add(text_data.spoilInt(out this.MeasItemArray[i].UniqNo, 7));
                resultList.Add(text_data.spoilInt(out this.MeasItemArray[i].TurnNo, 2));
                resultList.Add(text_data.spoilInt(out this.MeasItemArray[i].ProtoNo, 3));
                resultList.Add(text_data.spoilInt(out this.MeasItemArray[i].RepCount, 2));
                resultList.Add(text_data.spoilInt(out this.MeasItemArray[i].AutoDil, 4));
                resultList.Add(text_data.spoilString(out this.MeasItemArray[i].ReagentLotNo, 8));
            }

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
#endif
    }

    /// <summary>
    /// 測定データコマンド
    /// </summary>
    /// <remarks>
    /// 測定データコマンドデータクラス。
    /// パースを行います。
    /// </remarks>
    public class SlaveCommCommand_0503 : CarisXCommCommand, IMeasureResultData
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0503()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0503;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 検体区分
        /// </summary>
        public SampleKind SampleKind { get; set; } = SampleKind.Sample;

        /// <summary>
        /// 検体識別番号
        /// </summary>
        public Int32 IndividuallyNumber { get; set; } = 0;

        /// <summary>
        /// 検体ID
        /// </summary>
        public String SampleId { get; set; } = "";

        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID { get; set; } = "";

        /// <summary>
        /// 検体ポジション
        /// </summary>
        public Int32 SamplePos { get; set; } = 0;

        /// <summary>
        /// サンプル種別
        /// </summary>
        public SpecimenMaterialType SpecimenMaterialType { get; set; } = SpecimenMaterialType.BloodSerumAndPlasma;

        /// <summary>
        /// ユニーク番号
        /// </summary>
        public Int32 UniqueNo { get; set; } = 0;

        /// <summary>
        /// 測定順番
        /// </summary>
        public Int32 TurnOrder { get; set; } = 0;

        /// <summary>
        /// プロトコル番号
        /// </summary>
        public Int32 MeasProtocolNumber { get; set; } = 0;

        /// <summary>
        /// リプリケーション番号
        /// </summary>
        public Int32 RepNo { get; set; } = 0;

        /// <summary>
        /// ダーク値
        /// </summary>
        public Int32 DarkCount { get; set; } = 0;

        /// <summary>
        /// バックグラウンドカウント
        /// </summary>
        public Int32 BGCount { get; set; } = 0;

        /// <summary>
        /// 測定カウント
        /// </summary>
        public Int32 ResultCount { get; set; } = 0;

        /// <summary>
        /// リマーク
        /// </summary>
        public Remark Remark { get; set; } = 0;

        /// <summary>
        /// 試薬ロット番号
        /// </summary>
        public String ReagentLotNumber { get; set; } = "";

        /// <summary>
        /// プレトリガロット番号
        /// </summary>
        public String PreTriggerLotNo { get; set; } = "";

        /// <summary>
        /// トリガロット番号
        /// </summary>
        public String TriggerLotNo { get; set; } = "";

        /// <summary>
        /// 後希釈倍率
        /// </summary>
        public Int32 AfterDilution { get; set; } = 0;

        /// <summary>
        /// 手希釈倍率
        /// </summary>
        public Int32 PreDilution { get; set; } = 0;

        /// <summary>
        /// カップ種別
        /// </summary>
        public SpecimenCupKind CupKind { get; set; } = SpecimenCupKind.Cup;

        /// <summary>
        /// 分析履歴
        /// </summary>
        public SlaveAssayLogInfo AnalysisLog { get; set; } = new SlaveAssayLogInfo();

        /// <summary>
        /// エラー履歴
        /// </summary>
        public SlaveErrorLogInfo ErrorLog { get; set; } = new SlaveErrorLogInfo();

        /// <summary>
        /// モジュールID
        /// </summary>
        public Int32 ModuleID { get; set; } = 0;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((int)this.SampleKind).ToString(), 1);
                builder.Append(this.IndividuallyNumber.ToString(), 7);
                builder.Append(this.SampleId, 16);
                builder.Append(this.RackID, 4);
                builder.Append(this.SamplePos.ToString(), 1);
                builder.Append(((int)this.SpecimenMaterialType).ToString(), 1);
                builder.Append(this.UniqueNo.ToString(), 7);
                builder.Append(this.TurnOrder.ToString(), 2);
                builder.Append(this.MeasProtocolNumber.ToString(), 3);
                builder.Append(this.RepNo.ToString(), 3);
                builder.Append(this.DarkCount.ToString(), 8);
                builder.Append(this.BGCount.ToString(), 8);
                builder.Append(this.ResultCount.ToString(), 8);
                builder.Append(this.Remark.Value.ToString(), 10);
                builder.Append(this.ReagentLotNumber.ToString(), 8);
                builder.Append(this.PreTriggerLotNo.ToString(), 8);
                builder.Append(this.TriggerLotNo.ToString(), 8);
                builder.Append(this.AfterDilution.ToString(), 4);
                builder.Append(this.PreDilution.ToString(), 4);
                builder.Append(((int)this.CupKind).ToString(), 1);
                builder.Append(this.AnalysisLog.DiffSensor1.ToString(), 5);
                builder.Append(this.AnalysisLog.DiffSensor2.ToString(), 5);
                builder.Append(this.AnalysisLog.DiffSensor3.ToString(), 5);
                builder.Append(this.AnalysisLog.SampleDispenseVolume.ToString(), 5);
                builder.Append(this.AnalysisLog.SampleAspirationPosition.ToString(), 4);
                builder.Append(this.AnalysisLog.MReagPortNo.ToString(), 2);
                builder.Append(this.AnalysisLog.MReagLiquidPosition.ToString(), 4);
                builder.Append(this.AnalysisLog.R1ReagPortNo.ToString(), 2);
                builder.Append(this.AnalysisLog.R1ReagLiquidPosition.ToString(), 4);
                builder.Append(this.AnalysisLog.R2ReagPortNo.ToString(), 2);
                builder.Append(this.AnalysisLog.R2ReagLiquidPosition.ToString(), 4);
                builder.Append(this.AnalysisLog.ReactionTableTemp.ToString(), 4);
                builder.Append(this.AnalysisLog.BFTableTemp.ToString(), 4);
                builder.Append(this.AnalysisLog.BF1PreHeatTemp.ToString(), 4);
                builder.Append(this.AnalysisLog.BF2PreHeatTemp.ToString(), 4);
                builder.Append(this.AnalysisLog.R1ProbeTemp.ToString(), 4);
                builder.Append(this.AnalysisLog.R2ProbeTemp.ToString(), 4);
                builder.Append(this.AnalysisLog.ChemiluminesoensePtotometryTemp.ToString(), 4);
                builder.Append(this.AnalysisLog.RoomTemp.ToString(), 4);
                builder.Append(this.ErrorLog.ErrorRecord.ToString(), 1);
                for (Int32 i = 0; i < this.ErrorLog.ErrorCodeArg.Count(); i++)
                {
                    builder.Append(this.ErrorLog.ErrorCodeArg[i].Item1, 3);
                    builder.Append(this.ErrorLog.ErrorCodeArg[i].Item2, 3);
                }

                builder.Append(this.Remark.Value.ToString(), 16);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            Int32 tmpdata2;
            String tmpdata3;
            String tmpdata4;
            Int32 tmpdata5;
            Int32 tmpdata6;
            Int32 tmpdata7;
            Int32 tmpdata8;
            Int32 tmpdata9;
            Int32 tmpdata10;
            Int32 tmpdata11;
            Int32 tmpdata12;
            Int32 tmpdata13;
            Int64 tmpdata14;
            String tmpdata15;
            String tmpdata16;
            String tmpdata17;
            Int32 tmpdata18;
            Int32 tmpdata19;
            Int32 tmpdata20;
            Int32 tmpdata21;
            Int32 tmpdata22;
            Int32 tmpdata23;
            Int32 tmpdata24;
            Double tmpdata25;
            Int32 tmpdata26;
            Double tmpdata27;
            Int32 tmpdata28;
            Double tmpdata29;
            Int32 tmpdata30;
            Double tmpdata31;
            Double tmpdata32;
            Double tmpdata33;
            Double tmpdata34;
            Double tmpdata35;
            Double tmpdata36;
            Double tmpdata37;
            Double tmpdata38;
            Double tmpdata39;
            Int32 tmpdata40;

            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilInt(out tmpdata2, 7));
            resultList.Add(text_data.spoilString(out tmpdata3, 16));
            resultList.Add(text_data.spoilString(out tmpdata4, 4));
            resultList.Add(text_data.spoilInt(out tmpdata5, 1));
            resultList.Add(text_data.spoilInt(out tmpdata6, 1));
            resultList.Add(text_data.spoilInt(out tmpdata7, 7));
            resultList.Add(text_data.spoilInt(out tmpdata8, 2));
            resultList.Add(text_data.spoilInt(out tmpdata9, 3));//update for enlarge Protocols
            resultList.Add(text_data.spoilInt(out tmpdata10, 3));
            resultList.Add(text_data.spoilInt(out tmpdata11, 8));
            resultList.Add(text_data.spoilInt(out tmpdata12, 8));
            resultList.Add(text_data.spoilInt(out tmpdata13, 8));
            resultList.Add(text_data.spoilLong(out tmpdata14, 10));
            //TODO：CarisXで対応必須
            //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
            //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
            //{
            //    resultList.Add(text_data.spoilString(out tmpdata15, 8));
            //    if (!String.IsNullOrEmpty(tmpdata15))
            //    {
            //        int nReagentLot = 0;
            //        if (int.TryParse(tmpdata15, out nReagentLot))
            //        {
            //            tmpdata15 = nReagentLot.ToString();
            //        }
            //    }
            //    resultList.Add(text_data.spoilString(out tmpdata16, 8));
            //    if (!String.IsNullOrEmpty(tmpdata16))
            //    {
            //        int nPreTriggerLot = 0;
            //        if (int.TryParse(tmpdata16, out nPreTriggerLot))
            //        {
            //            tmpdata16 = nPreTriggerLot.ToString();
            //        }
            //    }
            //    resultList.Add(text_data.spoilString(out tmpdata17, 8));
            //    if (!String.IsNullOrEmpty(tmpdata17))
            //    {
            //        int nTriggerLot = 0;
            //        if (int.TryParse(tmpdata17, out nTriggerLot))
            //        {
            //            tmpdata17 = nTriggerLot.ToString();
            //        }
            //    }
            //}
            //else
            //{
            //    resultList.Add(text_data.spoilString(out tmpdata15, 8));
            //    resultList.Add(text_data.spoilString(out tmpdata16, 8));
            //    resultList.Add(text_data.spoilString(out tmpdata17, 8));
            //}
            resultList.Add(text_data.spoilString(out tmpdata15, 8));
            resultList.Add(text_data.spoilString(out tmpdata16, 8));
            resultList.Add(text_data.spoilString(out tmpdata17, 8));

            resultList.Add(text_data.spoilInt(out tmpdata18, 4));
            resultList.Add(text_data.spoilInt(out tmpdata19, 4));
            resultList.Add(text_data.spoilInt(out tmpdata20, 1));
            resultList.Add(text_data.spoilInt(out tmpdata21, 5));
            resultList.Add(text_data.spoilInt(out tmpdata22, 5));
            resultList.Add(text_data.spoilInt(out tmpdata23, 5));
            resultList.Add(text_data.spoilInt(out tmpdata24, 5));
            resultList.Add(text_data.spoilDouble(out tmpdata25, 4));
            resultList.Add(text_data.spoilInt(out tmpdata26, 2));
            resultList.Add(text_data.spoilDouble(out tmpdata27, 4));
            resultList.Add(text_data.spoilInt(out tmpdata28, 2));
            resultList.Add(text_data.spoilDouble(out tmpdata29, 4));
            resultList.Add(text_data.spoilInt(out tmpdata30, 2));
            resultList.Add(text_data.spoilDouble(out tmpdata31, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata32, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata33, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata34, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata35, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata36, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata37, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata38, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata39, 4));
            resultList.Add(text_data.spoilInt(out tmpdata40, 1));

            this.SampleKind = (SampleKind)tmpdata1;
            this.SampleKind = this.SampleKind == SampleKind.Line ? SampleKind.Sample : this.SampleKind; // 搬送ラインが来たら一般検体扱いする。
            this.IndividuallyNumber = tmpdata2;
            this.SampleId = tmpdata3;
            this.RackID = tmpdata4;
            this.SamplePos = tmpdata5;
            this.SpecimenMaterialType = (SpecimenMaterialType)tmpdata6;
            this.UniqueNo = tmpdata7;
            this.TurnOrder = tmpdata8;
            this.MeasProtocolNumber = tmpdata9;
            this.RepNo = tmpdata10;
            this.DarkCount = tmpdata11;
            this.BGCount = tmpdata12;
            this.ResultCount = tmpdata13;
            this.Remark = tmpdata14;
            this.ReagentLotNumber = tmpdata15;
            this.PreTriggerLotNo = tmpdata16;
            this.TriggerLotNo = tmpdata17;
            this.AfterDilution = tmpdata18;
            this.PreDilution = tmpdata19;
            this.CupKind = (SpecimenCupKind)tmpdata20;
            this.AnalysisLog.DiffSensor1 = tmpdata21;
            this.AnalysisLog.DiffSensor2 = tmpdata22;
            this.AnalysisLog.DiffSensor3 = tmpdata23;
            this.AnalysisLog.SampleDispenseVolume = tmpdata24;
            this.AnalysisLog.SampleAspirationPosition = tmpdata25;
            this.AnalysisLog.MReagPortNo = tmpdata26;
            this.AnalysisLog.MReagLiquidPosition = tmpdata27;
            this.AnalysisLog.R1ReagPortNo = tmpdata28;
            this.AnalysisLog.R1ReagLiquidPosition = tmpdata29;
            this.AnalysisLog.R2ReagPortNo = tmpdata30;
            this.AnalysisLog.R2ReagLiquidPosition = tmpdata31;
            this.AnalysisLog.ReactionTableTemp = tmpdata32;
            this.AnalysisLog.BFTableTemp = tmpdata33;
            this.AnalysisLog.BF1PreHeatTemp = tmpdata34;
            this.AnalysisLog.BF2PreHeatTemp = tmpdata35;
            this.AnalysisLog.R1ProbeTemp = tmpdata36;
            this.AnalysisLog.R2ProbeTemp = tmpdata37;
            this.AnalysisLog.ChemiluminesoensePtotometryTemp = tmpdata38;
            this.AnalysisLog.RoomTemp = tmpdata39;
            this.ErrorLog.ErrorRecord = tmpdata40;

            for (Int32 i = 0; i < this.ErrorLog.ErrorCodeArg.Count(); i++)
            {
                string code, arg;
                resultList.Add(text_data.spoilString(out code, 3));
                resultList.Add(text_data.spoilString(out arg, 3));
                ErrorLog.ErrorCodeArg[i] = new Tuple<String, String>(code.Replace(" ", ""), arg.Replace("-", "").Replace(" ", "")); // エラーコードと引数の間にスペースがある為削除する
            }

            resultList.Add(CarisXSubFunction.SpoilHex(out tmpdata14, 16, commandStr, 205));
            this.Remark = tmpdata14;

            // モジュールIDはこのタイミングで設定したくても、CommNoが設定されていないので出来ない。
            // 当クラスの値を使用するタイミングで設定する

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion

    }
    /// <summary>
    /// 測定データコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 測定データコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1503 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1503()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1503;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// エラーコマンド
    /// </summary>
    /// <remarks>
    /// エラーコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0504 : RackTransferCommCommand_0104
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0504()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0504;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// エラーコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// エラーコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1504 : RackTransferCommCommand_1104
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1504()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1504;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// スレーブイベントコマンド
    /// </summary>
    /// <remarks>
    /// スレーブイベントコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0505 : CarisXCommCommand
    {

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0505()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0505;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// イベント
        /// </summary>
        public SlaveSubEvent SubEvent { get; set; } = SlaveSubEvent.Wait;

        /// <summary>
        /// 引数1
        /// </summary>
        public Int32 SubEventArg1 { get; set; } = 0;

#if SIMULATOR
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((int)this.SubEvent).ToString(), 2);
                builder.Append(this.SubEventArg1.ToString(), 10);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// サブイベントコマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// サブイベントコマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0; ;
            Int32 tmpdata2 = 0; ;
            resultList.Add(text_data.spoilInt(out tmpdata1, 2));
            resultList.Add(text_data.spoilInt(out tmpdata2, 10));
            SubEvent = (SlaveSubEvent)tmpdata1;
            SubEventArg1 = tmpdata2;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion

    }
    /// <summary>
    /// スレーブイベントコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// スレーブイベントコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1505 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1505()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1505;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 分析ステータスコマンド
    /// </summary>
    /// <remarks>
    /// 分析ステータスコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0506 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0506()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0506;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// Assayステータス
        /// </summary>
        public AssayStatus AssayStatus { get; set; } = new AssayStatus();

#if SIMULATOR
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();

                for (Int32 i = 0; i < AssayStatus.UNIQUE_PARAM_COUNT; i++)
                {
                    builder.Append(this.AssayStatus.UniqueNoAndRepAndPosition[i].Item1.ToString(), 7);
                    builder.Append(this.AssayStatus.UniqueNoAndRepAndPosition[i].Item2.ToString(), 2);
                }
                builder.Append(this.AssayStatus.TemperatureTable.ReactionTableTemp.ToString(), 4);
                builder.Append(this.AssayStatus.TemperatureTable.BFTableTemp.ToString(), 4);
                builder.Append(this.AssayStatus.TemperatureTable.R1ProbeTemp.ToString(), 4);
                builder.Append(this.AssayStatus.TemperatureTable.R2ProbeTemp.ToString(), 4);
                builder.Append(this.AssayStatus.TemperatureTable.BF1PreHeatTemp.ToString(), 4);
                builder.Append(this.AssayStatus.TemperatureTable.BF2PreHeatTemp.ToString(), 4);
                builder.Append(this.AssayStatus.TemperatureTable.ChemiluminesoensePtotometryTemp.ToString(), 4);
                builder.Append(this.AssayStatus.TemperatureTable.RoomTemp.ToString(), 4);
                builder.Append(this.AssayStatus.TemperatureTable.ReagentBoxTemp.ToString(), 4);
                builder.Append(this.AssayStatus.TemperatureTable.AnalyzerTemp.ToString(), 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 分析ステータスコマンド解析
        /// </summary>
        /// <remarks>
        /// 分析ステータスコマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット

            Int32 uniqueNo;
            Int32 repNo;
            for (Int32 i = 0; i < AssayStatus.UNIQUE_PARAM_COUNT; i++)
            {
                resultList.Add(text_data.spoilInt(out uniqueNo, 7));
                resultList.Add(text_data.spoilInt(out repNo, 2));
                if ((uniqueNo != 0) && (repNo != 0))
                {
                    this.AssayStatus.UniqueNoAndRepAndPosition.Add(new Tuple<Int32, Int32, Int32>(uniqueNo, repNo, i));
                }
            }

            resultList.Add(text_data.spoilDouble(out this.AssayStatus.TemperatureTable.ReactionTableTemp, 4));
            resultList.Add(text_data.spoilDouble(out this.AssayStatus.TemperatureTable.BFTableTemp, 4));
            resultList.Add(text_data.spoilDouble(out this.AssayStatus.TemperatureTable.BF1PreHeatTemp, 4));
            resultList.Add(text_data.spoilDouble(out this.AssayStatus.TemperatureTable.BF2PreHeatTemp, 4));
            resultList.Add(text_data.spoilDouble(out this.AssayStatus.TemperatureTable.R1ProbeTemp, 4));
            resultList.Add(text_data.spoilDouble(out this.AssayStatus.TemperatureTable.R2ProbeTemp, 4));
            resultList.Add(text_data.spoilDouble(out this.AssayStatus.TemperatureTable.ChemiluminesoensePtotometryTemp, 4));
            resultList.Add(text_data.spoilDouble(out this.AssayStatus.TemperatureTable.ReagentBoxTemp, 4));
            resultList.Add(text_data.spoilDouble(out this.AssayStatus.TemperatureTable.RoomTemp, 4));
            resultList.Add(text_data.spoilDouble(out this.AssayStatus.TemperatureTable.AnalyzerTemp, 4));


            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }
    /// <summary>
    /// 分析ステータスコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 分析ステータスコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1506 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1506()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1506;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 分析終了コマンド
    /// </summary>
    /// <remarks>
    /// 分析終了コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0507 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0507()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0507;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// 分析終了コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 分析終了コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1507 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1507()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1507;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 残量コマンド
    /// </summary>
    /// <remarks>
    /// 残量コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0508 : CarisXCommCommand, IRemainAmountInfoSet
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0508()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0508;
            this.CommandId = (int)this.CommKind;

            ReagentRemainTable = new ReagentRemainTable[60];
            for (Int32 i = 0; i < ReagentRemainTable.Length; i++)
            {
                ReagentRemainTable[i] = new ReagentRemainTable();
            }
            DilutionRemainTable = new DilutionRemainTable();
            PreTriggerRemainTable = new PreTriggerRemainTable();
            TriggerRemainTable = new TriggerRemainTable();
            SampleTipRemainTable = new SampleTipRemainTable();
            CellRemainTable = new CellRemainTable();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬残量テーブル
        /// </summary>
        public ReagentRemainTable[] ReagentRemainTable { get; set; }

        /// <summary>
        /// 希釈液残量テーブル
        /// </summary>
        public DilutionRemainTable DilutionRemainTable { get; set; }

        /// <summary>
        /// プレトリガ残量テーブル
        /// </summary>
        public PreTriggerRemainTable PreTriggerRemainTable { get; set; }

        /// <summary>
        /// トリガ残量テーブル
        /// </summary>
        public TriggerRemainTable TriggerRemainTable { get; set; }

        /// <summary>
        /// サンプル分注チップ残量テーブル
        /// </summary>
        public SampleTipRemainTable SampleTipRemainTable { get; set; }

        /// <summary>
        /// 反応容器残量テーブル
        /// </summary>
        public CellRemainTable CellRemainTable { get; set; }

        /// <summary>
        /// 洗浄液残量
        /// </summary>
        public Int32 WashContainerRemain { get; set; } = 0;

        /// <summary>
        /// リンス液残量
        /// </summary>
        public Int32 RinceContainerRemain { get; set; } = 0;

        /// <summary>
        /// 廃液バッファ満杯フラグ
        /// </summary>
        public Boolean IsFullWasteBuffer { get; set; } = false;

        /// <summary>
        /// 廃棄ボックス有無
        /// </summary>
        public Boolean ExistWasteBox { get; set; } = false;

        /// <summary>
        /// 廃棄ボックス満杯状態
        /// </summary>
        /// <remarks>
        /// 仮にスレーブから定義外の値が指定された場合、使用箇所ではdefaultケース扱いとなる。
        /// </remarks>
        public WasteBoxStatus WasteBoxCondition { get; set; } = WasteBoxStatus.Empty;

        /// <summary>
        /// 取得時刻
        /// </summary>
        public DateTime TimeStamp { get; set; } = DateTime.MinValue;

#if SIMULATOR
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                Int32 i;

                //試薬残量テーブル
                for (i = 0; i < this.ReagentRemainTable.Count(); i++)
                {
                    builder.Append(this.ReagentRemainTable[i].ReagType.ToString(), 1);
                    builder.Append(this.ReagentRemainTable[i].ReagCode.ToString(), 3);
                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.Remain.ToString(), 7);
                    if (!string.IsNullOrEmpty(this.ReagentRemainTable[i].RemainingAmount.LotNumber))
                    {
                        builder.Append(int.Parse(this.ReagentRemainTable[i].RemainingAmount.LotNumber).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.ReagentRemainTable[i].RemainingAmount.LotNumber, 8);
                    }

                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.SerialNumber.ToString(), 5);
                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.TermOfUse.ToString("yyMMdd"), 6);
                    builder.Append(this.ReagentRemainTable[i].MakerCode ?? "  ", 2);
                    builder.Append(this.ReagentRemainTable[i].Capacity.ToString(), 7);
                }

                //希釈液残量テーブル
                builder.Append(this.DilutionRemainTable.RemainingAmount.Remain.ToString(), 7);
                if (!string.IsNullOrEmpty(this.DilutionRemainTable.RemainingAmount.LotNumber))
                {
                    builder.Append(int.Parse(this.DilutionRemainTable.RemainingAmount.LotNumber).ToString("00000000"), 8);
                }
                else
                {
                    builder.Append(this.DilutionRemainTable.RemainingAmount.LotNumber, 8);
                }
                builder.Append(this.DilutionRemainTable.RemainingAmount.TermOfUse.ToString("yyMMdd"), 6);

                //プレトリガ残量テーブル
                for (i = 0; i < this.PreTriggerRemainTable.RemainingAmount.Length; i++)
                {
                    builder.Append(this.PreTriggerRemainTable.RemainingAmount[i].Remain.ToString(), 7);
                    if (!string.IsNullOrEmpty(this.PreTriggerRemainTable.RemainingAmount[i].LotNumber))
                    {
                        builder.Append(int.Parse(this.PreTriggerRemainTable.RemainingAmount[i].LotNumber).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.PreTriggerRemainTable.RemainingAmount[i].LotNumber, 8);
                    }
                    builder.Append(this.PreTriggerRemainTable.RemainingAmount[i].TermOfUse.ToString("yyMMdd"), 6);
                }
                builder.Append(this.PreTriggerRemainTable.ActNo.ToString(), 1);

                //トリガ残量テーブル
                for (i = 0; i < this.TriggerRemainTable.RemainingAmount.Length; i++)
                {
                    builder.Append(this.TriggerRemainTable.RemainingAmount[i].Remain.ToString(), 7);

                    if (!string.IsNullOrEmpty(this.TriggerRemainTable.RemainingAmount[i].LotNumber))
                    {
                        builder.Append(int.Parse(this.TriggerRemainTable.RemainingAmount[i].LotNumber).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.TriggerRemainTable.RemainingAmount[i].LotNumber, 8);
                    }

                    builder.Append(this.TriggerRemainTable.RemainingAmount[i].TermOfUse.ToString("yyMMdd"), 6);
                }
                builder.Append(this.TriggerRemainTable.ActNo.ToString(), 1);

                //サンプル分注チップ残量テーブル
                for (i = 0; i < this.SampleTipRemainTable.tipRemainTable.Count(); i++)
                {
                    builder.Append(this.SampleTipRemainTable.tipRemainTable[i].ToString(), 3);
                }
                builder.Append(this.SampleTipRemainTable.ActNo.ToString(), 1);

                //反応容器残量テーブル
                for (i = 0; i < this.CellRemainTable.reactContainerRemainTable.Count(); i++)
                {
                    builder.Append(this.CellRemainTable.reactContainerRemainTable[i].ToString(), 3);
                }
                builder.Append(this.CellRemainTable.ActNo.ToString(), 1);

                builder.Append(this.WashContainerRemain.ToString(), 5);
                builder.Append(this.RinceContainerRemain.ToString(), 5);
                builder.Append(Convert.ToByte(this.IsFullWasteBuffer).ToString(), 1);
                builder.Append(Convert.ToByte(this.ExistWasteBox).ToString(), 1);
                builder.Append(Convert.ToByte(this.WasteBoxCondition).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 残量コマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// 残量コマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            this.TimeStamp = DateTime.Now;

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 i = 0;
            String dateMonthStr;

            //試薬残量テーブル
            for (i = 0; i < this.ReagentRemainTable.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].ReagType, 1));
                switch (ReagentRemainTable[i].ReagType)
                {
                    case (Int32)ReagentType.M:      //M試薬
                        ReagentRemainTable[i].ReagTypeDetail = ReagentTypeDetail.M;
                        break;
                    case (Int32)ReagentType.R1R2:   //R1R2試薬
                        ReagentRemainTable[i].ReagTypeDetail = ((i % 3) == 0) ? ReagentTypeDetail.R1 : ReagentTypeDetail.R2;    //1件目はR1、2件目はR2
                        break;
                    case (Int32)ReagentType.T1T2:   //前処理液
                        ReagentRemainTable[i].ReagTypeDetail = ((i % 3) == 0) ? ReagentTypeDetail.T1 : ReagentTypeDetail.T2;    //1件目はT1、2件目はT2
                        break;
                }
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].ReagCode, 3));//update for enlarge Protocols
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].RemainingAmount.Remain, 7));
                resultList.Add(text_data.spoilString(out ReagentRemainTable[i].RemainingAmount.LotNumber, 8));
                //TODO：CarisXで対応必須
                //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                //{
                //    if (!String.IsNullOrEmpty(ReagentRemainTable[i].RemainingAmount.LotNumber))
                //    {
                //        int nReagentLot = 0;
                //        if (int.TryParse(ReagentRemainTable[i].RemainingAmount.LotNumber, out nReagentLot))
                //        {
                //            ReagentRemainTable[i].RemainingAmount.LotNumber = nReagentLot.ToString();
                //        }
                //    }
                //}
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].RemainingAmount.SerialNumber, 5));

                resultList.Add(text_data.spoilString(out dateMonthStr, 12));

                if (!String.IsNullOrWhiteSpace(ReagentRemainTable [i].RemainingAmount.LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDDHHMMSS(dateMonthStr, out ReagentRemainTable [i].RemainingAmount.InstallationData));
                }

                resultList.Add(text_data.spoilString(out dateMonthStr, 6));

                if (!String.IsNullOrWhiteSpace(ReagentRemainTable[i].RemainingAmount.LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out ReagentRemainTable[i].RemainingAmount.TermOfUse));
                }

                resultList.Add(text_data.spoilString(out ReagentRemainTable[i].MakerCode, 2));
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].Capacity, 7));
            }

            //希釈液残量テーブル
            resultList.Add(text_data.spoilInt(out DilutionRemainTable.RemainingAmount.Remain, 7));
            resultList.Add(text_data.spoilString(out DilutionRemainTable.RemainingAmount.LotNumber, 8));
            //TODO：CarisXで対応必須
            //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
            //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
            //{
            //    if (!String.IsNullOrEmpty(DilutionRemainTable.RemainingAmount.LotNumber))
            //    {
            //        int nReagentLot = 0;
            //        if (int.TryParse(DilutionRemainTable.RemainingAmount.LotNumber, out nReagentLot))
            //        {
            //            DilutionRemainTable.RemainingAmount.LotNumber = nReagentLot.ToString();
            //        }
            //    }
            //}
            resultList.Add(text_data.spoilString(out dateMonthStr, 6));
            if (!String.IsNullOrWhiteSpace(DilutionRemainTable.RemainingAmount.LotNumber))
            {
                resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out DilutionRemainTable.RemainingAmount.TermOfUse));
            }

            //プレトリガ残量テーブル
            for (i = 0; i < this.PreTriggerRemainTable.RemainingAmount.Length; i++)
            {
                resultList.Add(text_data.spoilInt(out PreTriggerRemainTable.RemainingAmount[i].Remain, 7));
                resultList.Add(text_data.spoilString(out PreTriggerRemainTable.RemainingAmount[i].LotNumber, 8));
                //TODO：CarisXで対応必須
                //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                //{
                //    if (!String.IsNullOrEmpty(PreTriggerRemainTable.RemainingAmount[i].LotNumber))
                //    {
                //        int nReagentLot = 0;
                //        if (int.TryParse(PreTriggerRemainTable.RemainingAmount[i].LotNumber, out nReagentLot))
                //        {
                //            PreTriggerRemainTable.RemainingAmount[i].LotNumber = nReagentLot.ToString();
                //        }
                //    }
                //}

                resultList.Add(text_data.spoilString(out dateMonthStr, 6));

                if (!String.IsNullOrWhiteSpace(PreTriggerRemainTable.RemainingAmount[i].LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out PreTriggerRemainTable.RemainingAmount[i].TermOfUse));
                }
            }
            resultList.Add(text_data.spoilInt(out PreTriggerRemainTable.ActNo, 1));

            //トリガ残量テーブル
            for (i = 0; i < this.TriggerRemainTable.RemainingAmount.Length; i++)
            {
                resultList.Add(text_data.spoilInt(out TriggerRemainTable.RemainingAmount[i].Remain, 7));
                resultList.Add(text_data.spoilString(out TriggerRemainTable.RemainingAmount[i].LotNumber, 8));
                //TODO：CarisXで対応必須
                //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                //{
                //    if (!String.IsNullOrEmpty(TriggerRemainTable.RemainingAmount[i].LotNumber))
                //    {
                //        int nReagentLot = 0;
                //        if (int.TryParse(TriggerRemainTable.RemainingAmount[i].LotNumber, out nReagentLot))
                //        {
                //            TriggerRemainTable.RemainingAmount[i].LotNumber = nReagentLot.ToString();
                //        }
                //    }
                //}
                resultList.Add(text_data.spoilString(out dateMonthStr, 6));
                if (!String.IsNullOrWhiteSpace(TriggerRemainTable.RemainingAmount[i].LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out TriggerRemainTable.RemainingAmount[i].TermOfUse));
                }
            }
            resultList.Add(text_data.spoilInt(out TriggerRemainTable.ActNo, 1));

            //サンプル分注チップ残量テーブル
            for (i = 0; i < SampleTipRemainTable.tipRemainTable.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out SampleTipRemainTable.tipRemainTable[i], 3));
            }
            resultList.Add(text_data.spoilInt(out SampleTipRemainTable.ActNo, 1));

            //反応容器残量テーブル
            for (i = 0; i < CellRemainTable.reactContainerRemainTable.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out CellRemainTable.reactContainerRemainTable[i], 3));
            }
            resultList.Add(text_data.spoilInt(out CellRemainTable.ActNo, 1));

            Int32 tmpdata1;
            Int32 tmpdata2;
            Byte tmpdata3;
            Byte tmpdata4;
            Byte tmpdata5;

            resultList.Add(text_data.spoilInt(out tmpdata1, 5));
            resultList.Add(text_data.spoilInt(out tmpdata2, 5));
            resultList.Add(text_data.spoilByte(out tmpdata3, 1));
            resultList.Add(text_data.spoilByte(out tmpdata4, 1));
            resultList.Add(text_data.spoilByte(out tmpdata5, 1));

            WashContainerRemain = tmpdata1;
            RinceContainerRemain = tmpdata2;
            IsFullWasteBuffer = Convert.ToBoolean(tmpdata3);
            ExistWasteBox = Convert.ToBoolean(tmpdata4);
            WasteBoxCondition = (WasteBoxStatus)tmpdata5;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }
    /// <summary>
    /// 残量コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 残量コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1508 : CarisXCommCommand, IRackRemainAmountInfoSet
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1508()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1508;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 廃液満杯センサ
        /// </summary>
        public Boolean IsFullWasteTank { get; set; } = false;

        /// <summary>
        /// 廃液タンク有無センサ
        /// </summary>
        public Boolean ExistWasteTank { get; set; } = false;

        /// <summary>
        /// 取得時刻
        /// </summary>
        public DateTime TimeStamp { get; set; } = DateTime.MinValue;
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(Convert.ToByte(this.IsFullWasteTank).ToString(), 1);
                builder.Append(Convert.ToByte(this.ExistWasteTank).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            this.TimeStamp = DateTime.Now;

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1 = 0;
            Byte tmpdata2 = 0;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            this.IsFullWasteTank = Convert.ToBoolean(tmpdata1);
            this.ExistWasteTank = Convert.ToBoolean(tmpdata2);

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }

    /// <summary>
    /// モーターパラメータ設定コマンド
    /// </summary>
    /// <remarks>
    /// モーターパラメータ設定コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0509 : RackTransferCommCommand_0002
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0509()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0509;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// モーターパラメータ設定コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// モーターパラメータ設定コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1509 : RackTransferCommCommand_1002
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1509()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1509;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 分析項目番号、ポイント数、カウント
    /// </summary>
    public class MasterCurveInfo
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 分析項目番号
        /// </summary>
        public Int32 ProtoNo;

        /// <summary>
        /// ポイント数
        /// </summary>
        public Int32 PointCount;

        /// <summary>
        /// 濃度値
        /// </summary>
        public Double[] ConcAry;

        /// <summary>
        /// カウント値
        /// </summary>
        public Int32[] CountAry;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MasterCurveInfo()
        {
            CountAry = new Int32[8];
            ConcAry = new Double[8];
        }

        #endregion
    }
    /// <summary>
    /// マスターカーブ情報コマンド
    /// </summary>
    /// <remarks>
    /// マスターカーブ情報コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0510 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0510()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0510;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]
        /// <summary>
        /// 試薬コード
        /// </summary>
        public Int32 ReagCode { get; set; } = 0;

        /// <summary>
        /// プロトコル数
        /// </summary>
        public Int32 ProtocolCount { get; set; } = 0;

        /// <summary>
        /// マスターカーブ情報
        /// </summary>
        public MasterCurveInfo[] MasterCurve { get; set; }

        /// <summary>
        /// ロット番号
        /// </summary>
        public String LotNumber { get; set; } = "";

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.ReagCode.ToString(), 3);
                builder.Append(this.ProtocolCount.ToString(), 1);

                if (ProtocolCount != 0)
                {
                    for (int i = 0; i < ProtocolCount; i++)
                    {
                        builder.Append(this.MasterCurve[i].ProtoNo.ToString(), 3);
                        builder.Append(this.MasterCurve[i].PointCount.ToString(), 1);
                        for (int j = 0; j < this.MasterCurve[i].CountAry.Count(); j++)
                        {
                            builder.Append(this.MasterCurve[i].ConcAry[j].ToString(), 6);
                            builder.Append(this.MasterCurve[i].CountAry[j].ToString(), 8);
                        }
                    }
                }
                builder.Append(this.LotNumber.ToString(), 8);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// マスタカーブ情報コマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// マスタカーブ情報コマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 i = 0;
            Int32 j = 0;
            Int32 tmpdata1;
            Int32 tmpdata2;
            String tmpdata3;
            resultList.Add(text_data.spoilInt(out tmpdata1, 3));//update for enlarge Protocols
            resultList.Add(text_data.spoilInt(out tmpdata2, 1));

            ReagCode = tmpdata1;
            ProtocolCount = tmpdata2;

            if (ProtocolCount != 0)
            {
                MasterCurve = new MasterCurveInfo[ProtocolCount];

                for (i = 0; i < ProtocolCount; i++)
                {
                    MasterCurve[i] = new MasterCurveInfo();
                    resultList.Add(text_data.spoilInt(out MasterCurve[i].ProtoNo, 3));
                    resultList.Add(text_data.spoilInt(out MasterCurve[i].PointCount, 1));
                    for (j = 0; j < this.MasterCurve[i].CountAry.Count(); j++)
                    {
                        resultList.Add(text_data.spoilDouble(out MasterCurve[i].ConcAry[j], 6));
                        resultList.Add(text_data.spoilInt(out MasterCurve[i].CountAry[j], 8));
                    }
                }
            }

            resultList.Add(text_data.spoilString(out tmpdata3, 8));
            LotNumber = tmpdata3;
            if (Singleton<Oelco.Common.Parameter.ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
            {
                if (!string.IsNullOrEmpty(LotNumber))
                {
                    int nlotNo = 0;
                    if (int.TryParse(LotNumber, out nlotNo))
                    {
                        LotNumber = nlotNo.ToString();
                    }
                }
            }

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }
    /// <summary>
    /// マスターカーブ情報コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// マスターカーブ情報コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1510 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1510()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1510;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// バージョンコマンド
    /// </summary>
    /// <remarks>
    /// バージョンコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0511 : RackTransferCommCommand_0111
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0511()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0511;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// バージョンコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// バージョンコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1511 : RackTransferCommCommand_1111
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1511()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1511;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 試薬ロット確認コマンド
    /// </summary>
    /// <remarks>
    /// 試薬ロット確認コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0512 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0512()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0512;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬ロット番号
        /// </summary>
        public String ReagentLotNumber { get; set; } = "";

        /// <summary>
        /// プロトコル番号
        /// </summary>
        public Int32 ProtocolNo { get; set; } = 0;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.ReagentLotNumber, 8);
                builder.Append(this.ProtocolNo.ToString(), 3);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            String tmpdata1;
            Int32 tmpdata2;
            resultList.Add(text_data.spoilString(out tmpdata1, 8));
            ReagentLotNumber = tmpdata1;
            if (Singleton<Oelco.Common.Parameter.ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
            {
                int nReagentLot = 0;
                if (int.TryParse(ReagentLotNumber, out nReagentLot))
                {
                    ReagentLotNumber = nReagentLot.ToString();
                }
            }

            resultList.Add(text_data.spoilInt(out tmpdata2, 3));
            ProtocolNo = tmpdata2;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
        #endregion
    }
    /// <summary>
    /// 試薬ロット確認コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬ロット確認コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。  
    /// </remarks>
    public class SlaveCommCommand_1512 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1512()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1512;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 検量線存在有無
        /// </summary>
        public Byte UsableCallibration { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.UsableCallibration.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

#if DEBUG
        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1 = 0;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            this.UsableCallibration = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
#endif
    }

    /// <summary>
    /// キャリブレーション測定確認コマンド
    /// </summary>
    /// <remarks>
    /// キャリブレーション測定確認コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0513 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0513()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0513;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// キャリブレーション測定確認コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// キャリブレーション測定確認コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1513 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1513()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1513;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 総アッセイ数通知コマンド
    /// </summary>
    /// <remarks>
    /// 総アッセイ数通知コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0514 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0514()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0514;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 総アッセイ数
        /// </summary>
        public Int32 AssayTotalCount { get; set; } = 0;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.AssayTotalCount.ToString(), 8);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 8));
            AssayTotalCount = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }
    /// <summary>
    /// 総アッセイ数通知コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 総アッセイ数通知コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1514 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1514()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1514;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// ラック設置状況コマンド
    /// </summary>
    /// <remarks>
    /// ラック設置状況コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0515 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0515()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0515;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラック引込奥
        /// </summary>
        public SampleRackData RackRetractionBack { get; set; } = new SampleRackData();

        /// <summary>
        /// ラック引込手前
        /// </summary>
        public SampleRackData RackRetractionFront { get; set; } = new SampleRackData();

        /// <summary>
        /// STAT
        /// </summary>
        public STATData STATInstallation { get; set; } = new STATData();

        /// <summary>
        /// 外部搬送
        /// </summary>
        public OutsideTransfer OutsideTransfer { get; set; } = new OutsideTransfer();

        /// <summary>
        /// 順番
        /// </summary>
        public Int32[] TurnOrder { get; set; } = new Int32[4];


#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                Int32 i = 0;
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackRetractionBack.RackID.ToString(), 4);
                for (i = 0; i < this.RackRetractionBack.SampleID.Count(); i++)
                {
                    builder.Append(this.RackRetractionBack.SampleID[i].ToString(), 7);
                }
                builder.Append(((Int32)this.RackRetractionBack.RackStatus).ToString(), 1);
                builder.Append((this.RackRetractionBack.NumRemainingCycles).ToString(), 5);

                builder.Append(this.RackRetractionFront.RackID.ToString(), 4);
                for (i = 0; i < this.RackRetractionFront.SampleID.Count(); i++)
                {
                    builder.Append(this.RackRetractionFront.SampleID[i].ToString(), 7);
                }
                builder.Append(((Int32)this.RackRetractionFront.RackStatus).ToString(), 1);
                builder.Append((this.RackRetractionFront.NumRemainingCycles ).ToString(), 5);

                for (i = 0; i < this.STATInstallation.SampleID.Count(); i++)
                {
                    builder.Append(this.STATInstallation.SampleID[i].ToString(), 7);
                }
                builder.Append(((Int32)this.STATInstallation.RackStatus).ToString(), 1);
                builder.Append((this.STATInstallation.NumRemainingCycles).ToString(), 5);

                for (i = 0; i < this.OutsideTransfer.SampleID.Count(); i++)
                {
                    builder.Append(this.OutsideTransfer.SampleID [i].ToString(), 7);
                }
                builder.Append(( (Int32)this.OutsideTransfer.RackStatus ).ToString(), 1);
                builder.Append(( this.OutsideTransfer.NumRemainingCycles ).ToString(), 5);

                for (i = 0; i < this.TurnOrder.Count(); i++)
                {
                    builder.Append(this.TurnOrder[i].ToString(), 1);
                }

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// ラック設置状況コマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// ラック設置状況コマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 i = 0;
            Byte tmpdata1;
            Byte tmpdata2;
            Byte tmpdata3;
            Byte tmpdat6;

            //ラック引込奥
            resultList.Add(text_data.spoilString(out this.RackRetractionBack.RackID, 4));
            for (i = 0; i < this.RackRetractionBack.SampleID.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out this.RackRetractionBack.SampleID[i], 7));
            }
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilInt(out this.RackRetractionBack.NumRemainingCycles, 5));
            
            //ラック引込手前
            resultList.Add(text_data.spoilString(out this.RackRetractionFront.RackID, 4));
            for (i = 0; i < this.RackRetractionFront.SampleID.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out this.RackRetractionFront.SampleID[i], 7));
            }
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            resultList.Add(text_data.spoilInt(out this.RackRetractionFront.NumRemainingCycles, 5));

            //STAT
            for (i = 0; i < this.STATInstallation.SampleID.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out this.STATInstallation.SampleID[i], 7));
            }
            resultList.Add(text_data.spoilByte(out tmpdata3, 1));
            resultList.Add(text_data.spoilInt(out STATInstallation.NumRemainingCycles, 5));

            // 外部搬送
            for (i = 0; i < this.OutsideTransfer.SampleID.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out this.OutsideTransfer.SampleID [i], 7));
            }
            resultList.Add(text_data.spoilByte(out tmpdat6, 1));
            resultList.Add(text_data.spoilInt(out OutsideTransfer.NumRemainingCycles, 5));

            //順番
            for (i = 0; i < this.TurnOrder.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out this.TurnOrder[i], 1));
            }



            this.RackRetractionBack.RackStatus = (RackStatus)tmpdata1;
            this.RackRetractionFront.RackStatus = (RackStatus)tmpdata2;
            this.STATInstallation.RackStatus = (RackStatus)tmpdata3;
            this.OutsideTransfer.RackStatus = (RackStatus)tmpdat6;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }
    /// <summary>
    /// ラック設置状況コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラック設置状況コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1515 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1515()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1515;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 試薬テーブル回転SW押下通知コマンド
    /// </summary>
    /// <remarks>
    /// 試薬テーブル回転SW押下通知コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0516 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0516()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0516;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// 試薬テーブル回転SW押下通知コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬テーブル回転SW押下通知コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1516 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1516()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1516;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 試薬設置状況通知コマンド
    /// </summary>
    /// <remarks>
    /// 試薬設置状況通知コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0520 : CarisXCommCommand, IRemainAmountInfoSet
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0520()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0520;
            this.CommandId = (int)this.CommKind;

            ReagentRemainTable = new ReagentRemainTable[60];
            for (Int32 i = 0; i < ReagentRemainTable.Length; i++)
            {
                ReagentRemainTable[i] = new ReagentRemainTable();
            }
            DilutionRemainTable = new DilutionRemainTable();
            PreTriggerRemainTable = new PreTriggerRemainTable();
            TriggerRemainTable = new TriggerRemainTable();
            SampleTipRemainTable = new SampleTipRemainTable();
            CellRemainTable = new CellRemainTable();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬残量テーブル
        /// </summary>
        public ReagentRemainTable[] ReagentRemainTable { get; set; }

        /// <summary>
        /// 希釈液残量テーブル
        /// </summary>
        public DilutionRemainTable DilutionRemainTable { get; set; }

        /// <summary>
        /// プレトリガ残量テーブル
        /// </summary>
        public PreTriggerRemainTable PreTriggerRemainTable { get; set; }

        /// <summary>
        /// トリガ残量テーブル
        /// </summary>
        public TriggerRemainTable TriggerRemainTable { get; set; }

        /// <summary>
        /// サンプル分注チップ残量テーブル
        /// </summary>
        public SampleTipRemainTable SampleTipRemainTable { get; set; }

        /// <summary>
        /// 反応容器残量テーブル
        /// </summary>
        public CellRemainTable CellRemainTable { get; set; }

        /// <summary>
        /// 洗浄液残量
        /// </summary>
        public Int32 WashContainerRemain { get; set; } = 0;

        /// <summary>
        /// リンス液残量
        /// </summary>
        public Int32 RinceContainerRemain { get; set; } = 0;

        /// <summary>
        /// 廃液バッファ満杯フラグ
        /// </summary>
        public Boolean IsFullWasteBuffer { get; set; } = false;

        /// <summary>
        /// 廃棄ボックス有無
        /// </summary>
        public Boolean ExistWasteBox { get; set; } = false;

        /// <summary>
        /// 廃棄ボックス満杯状態
        /// </summary>
        /// <remarks>
        /// 仮にスレーブから定義外の値が指定された場合、使用箇所ではdefaultケース扱いとなる。
        /// </remarks>
        public WasteBoxStatus WasteBoxCondition { get; set; } = WasteBoxStatus.Empty;

        /// <summary>
        /// 取得時刻
        /// </summary>
        public DateTime TimeStamp { get; set; } = DateTime.MinValue;

#if SIMULATOR
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                Int32 i;

                //試薬残量テーブル
                for (i = 0; i < this.ReagentRemainTable.Count(); i++)
                {
                    builder.Append(this.ReagentRemainTable[i].ReagType.ToString(), 1);
                    builder.Append(this.ReagentRemainTable[i].ReagCode.ToString(), 3);
                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.Remain.ToString(), 7);
                    if (!string.IsNullOrEmpty(this.ReagentRemainTable[i].RemainingAmount.LotNumber))
                    {
                        builder.Append(int.Parse(this.ReagentRemainTable[i].RemainingAmount.LotNumber).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.ReagentRemainTable[i].RemainingAmount.LotNumber, 8);
                    }

                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.SerialNumber.ToString(), 5);
                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.InstallationData.ToString("yyMMdd"), 6);
                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.TermOfUse.ToString("yyMMdd"), 6);
                    builder.Append(this.ReagentRemainTable[i].MakerCode ?? "  ", 2);
                    builder.Append(this.ReagentRemainTable[i].Capacity.ToString(), 7);
                }

                //希釈液残量テーブル
                builder.Append(this.DilutionRemainTable.RemainingAmount.Remain.ToString(), 7);
                if (!string.IsNullOrEmpty(this.DilutionRemainTable.RemainingAmount.LotNumber))
                {
                    builder.Append(int.Parse(this.DilutionRemainTable.RemainingAmount.LotNumber).ToString("00000000"), 8);
                }
                else
                {
                    builder.Append(this.DilutionRemainTable.RemainingAmount.LotNumber, 8);
                }
                builder.Append(this.DilutionRemainTable.RemainingAmount.TermOfUse.ToString("yyMMdd"), 6);

                //プレトリガ残量テーブル
                for (i = 0; i < this.PreTriggerRemainTable.RemainingAmount.Length; i++)
                {
                    builder.Append(this.PreTriggerRemainTable.RemainingAmount[i].Remain.ToString(), 7);
                    if (!string.IsNullOrEmpty(this.PreTriggerRemainTable.RemainingAmount[i].LotNumber))
                    {
                        builder.Append(int.Parse(this.PreTriggerRemainTable.RemainingAmount[i].LotNumber).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.PreTriggerRemainTable.RemainingAmount[i].LotNumber, 8);
                    }
                    builder.Append(this.PreTriggerRemainTable.RemainingAmount[i].TermOfUse.ToString("yyMMdd"), 6);
                }
                builder.Append(this.PreTriggerRemainTable.ActNo.ToString(), 1);

                //トリガ残量テーブル
                for (i = 0; i < this.TriggerRemainTable.RemainingAmount.Length; i++)
                {
                    builder.Append(this.TriggerRemainTable.RemainingAmount[i].Remain.ToString(), 7);

                    if (!string.IsNullOrEmpty(this.TriggerRemainTable.RemainingAmount[i].LotNumber))
                    {
                        builder.Append(int.Parse(this.TriggerRemainTable.RemainingAmount[i].LotNumber).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.TriggerRemainTable.RemainingAmount[i].LotNumber, 8);
                    }

                    builder.Append(this.TriggerRemainTable.RemainingAmount[i].TermOfUse.ToString("yyMMdd"), 6);
                }
                builder.Append(this.TriggerRemainTable.ActNo.ToString(), 1);

                //サンプル分注チップ残量テーブル
                for (i = 0; i < this.SampleTipRemainTable.tipRemainTable.Count(); i++)
                {
                    builder.Append(this.SampleTipRemainTable.tipRemainTable[i].ToString(), 3);
                }
                builder.Append(this.SampleTipRemainTable.ActNo.ToString(), 1);

                //反応容器残量テーブル
                for (i = 0; i < this.CellRemainTable.reactContainerRemainTable.Count(); i++)
                {
                    builder.Append(this.CellRemainTable.reactContainerRemainTable[i].ToString(), 3);
                }
                builder.Append(this.CellRemainTable.ActNo.ToString(), 1);

                builder.Append(this.WashContainerRemain.ToString(), 5);
                builder.Append(this.RinceContainerRemain.ToString(), 5);
                builder.Append(Convert.ToByte(this.IsFullWasteBuffer).ToString(), 1);
                builder.Append(Convert.ToByte(this.ExistWasteBox).ToString(), 1);
                builder.Append(Convert.ToByte(this.WasteBoxCondition).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 残量コマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// 残量コマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            this.TimeStamp = DateTime.Now;

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 i = 0;
            String dateMonthStr;

            //試薬残量テーブル
            for (i = 0; i < this.ReagentRemainTable.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].ReagType, 1));
                switch (ReagentRemainTable[i].ReagType)
                {
                    case (Int32)ReagentType.M:      //M試薬
                        ReagentRemainTable[i].ReagTypeDetail = ReagentTypeDetail.M;
                        break;
                    case (Int32)ReagentType.R1R2:   //R1R2試薬
                        ReagentRemainTable[i].ReagTypeDetail = ((i % 3) == 0) ? ReagentTypeDetail.R1 : ReagentTypeDetail.R2;    //1件目はR1、2件目はR2
                        break;
                    case (Int32)ReagentType.T1T2:   //前処理液
                        ReagentRemainTable[i].ReagTypeDetail = ((i % 3) == 0) ? ReagentTypeDetail.T1 : ReagentTypeDetail.T2;    //1件目はT1、2件目はT2
                        break;
                }
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].ReagCode, 3));
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].RemainingAmount.Remain, 7));
                resultList.Add(text_data.spoilString(out ReagentRemainTable[i].RemainingAmount.LotNumber, 8));
                //TODO：CarisXで対応必須
                //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                //{
                //    if (!String.IsNullOrEmpty(ReagentRemainTable[i].RemainingAmount.LotNumber))
                //    {
                //        int nReagentLot = 0;
                //        if (int.TryParse(ReagentRemainTable[i].RemainingAmount.LotNumber, out nReagentLot))
                //        {
                //            ReagentRemainTable[i].RemainingAmount.LotNumber = nReagentLot.ToString();
                //        }
                //    }
                //}
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].RemainingAmount.SerialNumber, 5));

                resultList.Add(text_data.spoilString(out dateMonthStr, 12));

                if (!String.IsNullOrWhiteSpace(ReagentRemainTable [i].RemainingAmount.LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDDHHMMSS(dateMonthStr, out ReagentRemainTable [i].RemainingAmount.InstallationData));
                }

                resultList.Add(text_data.spoilString(out dateMonthStr, 6));

                if (!String.IsNullOrWhiteSpace(ReagentRemainTable[i].RemainingAmount.LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out ReagentRemainTable[i].RemainingAmount.TermOfUse));
                }

                resultList.Add(text_data.spoilString(out ReagentRemainTable[i].MakerCode, 2));
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].Capacity, 7));
            }

            //希釈液残量テーブル
            resultList.Add(text_data.spoilInt(out DilutionRemainTable.RemainingAmount.Remain, 7));
            resultList.Add(text_data.spoilString(out DilutionRemainTable.RemainingAmount.LotNumber, 8));
            //TODO：CarisXで対応必須
            //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
            //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
            //{
            //    if (!String.IsNullOrEmpty(DilutionRemainTable.RemainingAmount.LotNumber))
            //    {
            //        int nReagentLot = 0;
            //        if (int.TryParse(DilutionRemainTable.RemainingAmount.LotNumber, out nReagentLot))
            //        {
            //            DilutionRemainTable.RemainingAmount.LotNumber = nReagentLot.ToString();
            //        }
            //    }
            //}
            resultList.Add(text_data.spoilString(out dateMonthStr, 6));
            if (!String.IsNullOrWhiteSpace(DilutionRemainTable.RemainingAmount.LotNumber))
            {
                resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out DilutionRemainTable.RemainingAmount.TermOfUse));
            }

            //プレトリガ残量テーブル
            for (i = 0; i < this.PreTriggerRemainTable.RemainingAmount.Length; i++)
            {
                resultList.Add(text_data.spoilInt(out PreTriggerRemainTable.RemainingAmount[i].Remain, 7));
                resultList.Add(text_data.spoilString(out PreTriggerRemainTable.RemainingAmount[i].LotNumber, 8));
                //TODO：CarisXで対応必須
                //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                //{
                //    if (!String.IsNullOrEmpty(PreTriggerRemainTable.RemainingAmount[i].LotNumber))
                //    {
                //        int nReagentLot = 0;
                //        if (int.TryParse(PreTriggerRemainTable.RemainingAmount[i].LotNumber, out nReagentLot))
                //        {
                //            PreTriggerRemainTable.RemainingAmount[i].LotNumber = nReagentLot.ToString();
                //        }
                //    }
                //}

                resultList.Add(text_data.spoilString(out dateMonthStr, 6));

                if (!String.IsNullOrWhiteSpace(PreTriggerRemainTable.RemainingAmount[i].LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out PreTriggerRemainTable.RemainingAmount[i].TermOfUse));
                }
            }
            resultList.Add(text_data.spoilInt(out PreTriggerRemainTable.ActNo, 1));

            //トリガ残量テーブル
            for (i = 0; i < this.TriggerRemainTable.RemainingAmount.Length; i++)
            {
                resultList.Add(text_data.spoilInt(out TriggerRemainTable.RemainingAmount[i].Remain, 7));
                resultList.Add(text_data.spoilString(out TriggerRemainTable.RemainingAmount[i].LotNumber, 8));
                //TODO：CarisXで対応必須
                //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                //{
                //    if (!String.IsNullOrEmpty(TriggerRemainTable.RemainingAmount[i].LotNumber))
                //    {
                //        int nReagentLot = 0;
                //        if (int.TryParse(TriggerRemainTable.RemainingAmount[i].LotNumber, out nReagentLot))
                //        {
                //            TriggerRemainTable.RemainingAmount[i].LotNumber = nReagentLot.ToString();
                //        }
                //    }
                //}
                resultList.Add(text_data.spoilString(out dateMonthStr, 6));
                if (!String.IsNullOrWhiteSpace(TriggerRemainTable.RemainingAmount[i].LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out TriggerRemainTable.RemainingAmount[i].TermOfUse));
                }
            }
            resultList.Add(text_data.spoilInt(out TriggerRemainTable.ActNo, 1));

            //サンプル分注チップ残量テーブル
            for (i = 0; i < SampleTipRemainTable.tipRemainTable.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out SampleTipRemainTable.tipRemainTable[i], 3));
            }
            resultList.Add(text_data.spoilInt(out SampleTipRemainTable.ActNo, 1));

            //反応容器残量テーブル
            for (i = 0; i < CellRemainTable.reactContainerRemainTable.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out CellRemainTable.reactContainerRemainTable[i], 3));
            }
            resultList.Add(text_data.spoilInt(out CellRemainTable.ActNo, 1));

            Int32 tmpdata1;
            Int32 tmpdata2;
            Byte tmpdata3;
            Byte tmpdata4;
            Byte tmpdata5;

            resultList.Add(text_data.spoilInt(out tmpdata1, 5));
            resultList.Add(text_data.spoilInt(out tmpdata2, 5));
            resultList.Add(text_data.spoilByte(out tmpdata3, 1));
            resultList.Add(text_data.spoilByte(out tmpdata4, 1));
            resultList.Add(text_data.spoilByte(out tmpdata5, 1));

            WashContainerRemain = tmpdata1;
            RinceContainerRemain = tmpdata2;
            IsFullWasteBuffer = Convert.ToBoolean(tmpdata3);
            ExistWasteBox = Convert.ToBoolean(tmpdata4);
            WasteBoxCondition = (WasteBoxStatus)tmpdata5;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }
    /// <summary>
    /// 試薬設置状況通知コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 試薬設置状況通知コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1520 : CarisXCommCommand, IRemainAmountInfoSet
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1520()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1520;
            this.CommandId = (int)this.CommKind;

            ReagentRemainTable = new ReagentRemainTable[60];
            for (Int32 i = 0; i < ReagentRemainTable.Length; i++)
            {
                ReagentRemainTable[i] = new ReagentRemainTable();
            }
            DilutionRemainTable = new DilutionRemainTable();
            PreTriggerRemainTable = new PreTriggerRemainTable();
            TriggerRemainTable = new TriggerRemainTable();
            SampleTipRemainTable = new SampleTipRemainTable();
            CellRemainTable = new CellRemainTable();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 試薬残量テーブル
        /// </summary>
        public ReagentRemainTable[] ReagentRemainTable { get; set; }

        /// <summary>
        /// 希釈液残量テーブル
        /// </summary>
        public DilutionRemainTable DilutionRemainTable { get; set; }

        /// <summary>
        /// プレトリガ残量テーブル
        /// </summary>
        public PreTriggerRemainTable PreTriggerRemainTable { get; set; }

        /// <summary>
        /// トリガ残量テーブル
        /// </summary>
        public TriggerRemainTable TriggerRemainTable { get; set; }

        /// <summary>
        /// サンプル分注チップ残量テーブル
        /// </summary>
        public SampleTipRemainTable SampleTipRemainTable { get; set; }

        /// <summary>
        /// 反応容器残量テーブル
        /// </summary>
        public CellRemainTable CellRemainTable { get; set; }

        /// <summary>
        /// 洗浄液残量
        /// </summary>
        public Int32 WashContainerRemain { get; set; } = 0;

        /// <summary>
        /// リンス液残量
        /// </summary>
        public Int32 RinceContainerRemain { get; set; } = 0;

        /// <summary>
        /// 廃液バッファ満杯フラグ
        /// </summary>
        public Boolean IsFullWasteBuffer { get; set; } = false;

        /// <summary>
        /// 廃棄ボックス有無
        /// </summary>
        public Boolean ExistWasteBox { get; set; } = false;

        /// <summary>
        /// 廃棄ボックス満杯状態
        /// </summary>
        /// <remarks>
        /// 仮にスレーブから定義外の値が指定された場合、使用箇所ではdefaultケース扱いとなる。
        /// </remarks>
        public WasteBoxStatus WasteBoxCondition { get; set; } = WasteBoxStatus.Empty;

        /// <summary>
        /// 取得時刻
        /// </summary>
        public DateTime TimeStamp { get; set; } = DateTime.MinValue;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                Int32 i;

                //試薬残量テーブル
                for (i = 0; i < this.ReagentRemainTable.Count(); i++)
                {
                    builder.Append(this.ReagentRemainTable[i].ReagType.ToString(), 1);
                    builder.Append(this.ReagentRemainTable[i].ReagCode.ToString(), 3);
                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.Remain.ToString(), 7);
                    if (!string.IsNullOrEmpty(this.ReagentRemainTable[i].RemainingAmount.LotNumber))
                    {
                        builder.Append(int.Parse(this.ReagentRemainTable[i].RemainingAmount.LotNumber).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.ReagentRemainTable[i].RemainingAmount.LotNumber, 8);
                    }

                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.SerialNumber.ToString(), 5);
                    builder.Append(this.ReagentRemainTable[i].RemainingAmount.InstallationData.ToString("yyMMddHHmmss"), 12);
                    if (this.ReagentRemainTable[i].RemainingAmount.TermOfUse == DateTime.MinValue)
                        builder.Append(" ", 6);
                    else
                        builder.Append(this.ReagentRemainTable[i].RemainingAmount.TermOfUse.ToString("yyMMdd"), 6);
                    builder.Append(this.ReagentRemainTable[i].MakerCode ?? "  ", 2);
                    builder.Append(this.ReagentRemainTable[i].Capacity.ToString(), 7);
                }

                //希釈液残量テーブル
                builder.Append(this.DilutionRemainTable.RemainingAmount.Remain.ToString(), 7);
                if (!string.IsNullOrEmpty(this.DilutionRemainTable.RemainingAmount.LotNumber))
                {
                    builder.Append(int.Parse(this.DilutionRemainTable.RemainingAmount.LotNumber).ToString("00000000"), 8);
                }
                else
                {
                    builder.Append(this.DilutionRemainTable.RemainingAmount.LotNumber, 8);
                }
                builder.Append(this.DilutionRemainTable.RemainingAmount.TermOfUse.ToString("yyMMdd"), 6);

                //プレトリガ残量テーブル
                for (i = 0; i < this.PreTriggerRemainTable.RemainingAmount.Length; i++)
                {
                    builder.Append(this.PreTriggerRemainTable.RemainingAmount[i].Remain.ToString(), 7);
                    if (!string.IsNullOrEmpty(this.PreTriggerRemainTable.RemainingAmount[i].LotNumber))
                    {
                        builder.Append(int.Parse(this.PreTriggerRemainTable.RemainingAmount[i].LotNumber).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.PreTriggerRemainTable.RemainingAmount[i].LotNumber, 8);
                    }
                    builder.Append(this.PreTriggerRemainTable.RemainingAmount[i].TermOfUse.ToString("yyMMdd"), 6);
                }
                builder.Append(this.PreTriggerRemainTable.ActNo.ToString(), 1);

                //トリガ残量テーブル
                for (i = 0; i < this.TriggerRemainTable.RemainingAmount.Length; i++)
                {
                    builder.Append(this.TriggerRemainTable.RemainingAmount[i].Remain.ToString(), 7);

                    if (!string.IsNullOrEmpty(this.TriggerRemainTable.RemainingAmount[i].LotNumber))
                    {
                        builder.Append(int.Parse(this.TriggerRemainTable.RemainingAmount[i].LotNumber).ToString("00000000"), 8);
                    }
                    else
                    {
                        builder.Append(this.TriggerRemainTable.RemainingAmount[i].LotNumber, 8);
                    }

                    builder.Append(this.TriggerRemainTable.RemainingAmount[i].TermOfUse.ToString("yyMMdd"), 6);
                }
                builder.Append(this.TriggerRemainTable.ActNo.ToString(), 1);

                //サンプル分注チップ残量テーブル
                for (i = 0; i < this.SampleTipRemainTable.tipRemainTable.Count(); i++)
                {
                    builder.Append(this.SampleTipRemainTable.tipRemainTable[i].ToString(), 3);
                }
                builder.Append(this.SampleTipRemainTable.ActNo.ToString(), 1);

                //反応容器残量テーブル
                for (i = 0; i < this.CellRemainTable.reactContainerRemainTable.Count(); i++)
                {
                    builder.Append(this.CellRemainTable.reactContainerRemainTable[i].ToString(), 3);
                }
                builder.Append(this.CellRemainTable.ActNo.ToString(), 1);

                builder.Append(this.WashContainerRemain.ToString(), 5);
                builder.Append(this.RinceContainerRemain.ToString(), 5);
                builder.Append(Convert.ToByte(this.IsFullWasteBuffer).ToString(), 1);
                builder.Append(Convert.ToByte(this.ExistWasteBox).ToString(), 1);
                builder.Append(Convert.ToByte(this.WasteBoxCondition).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if SIMULATOR
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            this.TimeStamp = DateTime.Now;

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 i = 0;
            String dateMonthStr;

            //試薬残量テーブル
            for (i = 0; i < this.ReagentRemainTable.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].ReagType, 1));
                switch (ReagentRemainTable[i].ReagType)
                {
                    case (Int32)ReagentType.M:      //M試薬
                        ReagentRemainTable[i].ReagTypeDetail = ReagentTypeDetail.M;
                        break;
                    case (Int32)ReagentType.R1R2:   //R1R2試薬
                        ReagentRemainTable[i].ReagTypeDetail = ((i % 3) == 0) ? ReagentTypeDetail.R1 : ReagentTypeDetail.R2;    //1件目はR1、2件目はR2
                        break;
                    case (Int32)ReagentType.T1T2:   //前処理液
                        ReagentRemainTable[i].ReagTypeDetail = ((i % 3) == 0) ? ReagentTypeDetail.T1 : ReagentTypeDetail.T2;    //1件目はT1、2件目はT2
                        break;
                }
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].ReagCode, 3));
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].RemainingAmount.Remain, 7));
                resultList.Add(text_data.spoilString(out ReagentRemainTable[i].RemainingAmount.LotNumber, 8));
                //TODO：CarisXで対応必須
                //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                //{
                //    if (!String.IsNullOrEmpty(ReagentRemainTable[i].RemainingAmount.LotNumber))
                //    {
                //        int nReagentLot = 0;
                //        if (int.TryParse(ReagentRemainTable[i].RemainingAmount.LotNumber, out nReagentLot))
                //        {
                //            ReagentRemainTable[i].RemainingAmount.LotNumber = nReagentLot.ToString();
                //        }
                //    }
                //}
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].RemainingAmount.SerialNumber, 5));
                resultList.Add(text_data.spoilString(out dateMonthStr, 6));

                if (!String.IsNullOrWhiteSpace(ReagentRemainTable[i].RemainingAmount.LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out ReagentRemainTable[i].RemainingAmount.TermOfUse));
                }

                resultList.Add(text_data.spoilString(out ReagentRemainTable[i].MakerCode, 2));
                resultList.Add(text_data.spoilInt(out ReagentRemainTable[i].Capacity, 7));
            }

            //希釈液残量テーブル
            resultList.Add(text_data.spoilInt(out DilutionRemainTable.RemainingAmount.Remain, 7));
            resultList.Add(text_data.spoilString(out DilutionRemainTable.RemainingAmount.LotNumber, 8));
            //TODO：CarisXで対応必須
            //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
            //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
            //{
            //    if (!String.IsNullOrEmpty(DilutionRemainTable.RemainingAmount.LotNumber))
            //    {
            //        int nReagentLot = 0;
            //        if (int.TryParse(DilutionRemainTable.RemainingAmount.LotNumber, out nReagentLot))
            //        {
            //            DilutionRemainTable.RemainingAmount.LotNumber = nReagentLot.ToString();
            //        }
            //    }
            //}
            resultList.Add(text_data.spoilString(out dateMonthStr, 6));
            if (!String.IsNullOrWhiteSpace(DilutionRemainTable.RemainingAmount.LotNumber))
            {
                resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out DilutionRemainTable.RemainingAmount.TermOfUse));
            }

            //プレトリガ残量テーブル
            for (i = 0; i < this.PreTriggerRemainTable.RemainingAmount.Length; i++)
            {
                resultList.Add(text_data.spoilInt(out PreTriggerRemainTable.RemainingAmount[i].Remain, 7));
                resultList.Add(text_data.spoilString(out PreTriggerRemainTable.RemainingAmount[i].LotNumber, 8));
                //TODO：CarisXで対応必須
                //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                //{
                //    if (!String.IsNullOrEmpty(PreTriggerRemainTable.RemainingAmount[i].LotNumber))
                //    {
                //        int nReagentLot = 0;
                //        if (int.TryParse(PreTriggerRemainTable.RemainingAmount[i].LotNumber, out nReagentLot))
                //        {
                //            PreTriggerRemainTable.RemainingAmount[i].LotNumber = nReagentLot.ToString();
                //        }
                //    }
                //}

                resultList.Add(text_data.spoilString(out dateMonthStr, 6));

                if (!String.IsNullOrWhiteSpace(PreTriggerRemainTable.RemainingAmount[i].LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out PreTriggerRemainTable.RemainingAmount[i].TermOfUse));
                }
            }
            resultList.Add(text_data.spoilInt(out PreTriggerRemainTable.ActNo, 1));

            //トリガ残量テーブル
            for (i = 0; i < this.TriggerRemainTable.RemainingAmount.Length; i++)
            {
                resultList.Add(text_data.spoilInt(out TriggerRemainTable.RemainingAmount[i].Remain, 7));
                resultList.Add(text_data.spoilString(out TriggerRemainTable.RemainingAmount[i].LotNumber, 8));
                //TODO：CarisXで対応必須
                //if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CompanyLogoParameter.CompanyLogo
                //    == CompanyLogoParameter.CompanyLogoKind.LogoTwo)
                //{
                //    if (!String.IsNullOrEmpty(TriggerRemainTable.RemainingAmount[i].LotNumber))
                //    {
                //        int nReagentLot = 0;
                //        if (int.TryParse(TriggerRemainTable.RemainingAmount[i].LotNumber, out nReagentLot))
                //        {
                //            TriggerRemainTable.RemainingAmount[i].LotNumber = nReagentLot.ToString();
                //        }
                //    }
                //}
                resultList.Add(text_data.spoilString(out dateMonthStr, 6));
                if (!String.IsNullOrWhiteSpace(TriggerRemainTable.RemainingAmount[i].LotNumber))
                {
                    resultList.Add(SubFunction.DateTimeTryParseExactForYYMMDD(dateMonthStr, out TriggerRemainTable.RemainingAmount[i].TermOfUse));
                }
            }
            resultList.Add(text_data.spoilInt(out TriggerRemainTable.ActNo, 1));

            //サンプル分注チップ残量テーブル
            for (i = 0; i < SampleTipRemainTable.tipRemainTable.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out SampleTipRemainTable.tipRemainTable[i], 3));
            }
            resultList.Add(text_data.spoilInt(out SampleTipRemainTable.ActNo, 1));

            //反応容器残量テーブル
            for (i = 0; i < CellRemainTable.reactContainerRemainTable.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out CellRemainTable.reactContainerRemainTable[i], 3));
            }
            resultList.Add(text_data.spoilInt(out CellRemainTable.ActNo, 1));

            Int32 tmpdata1;
            Int32 tmpdata2;
            Byte tmpdata3;
            Byte tmpdata4;
            Byte tmpdata5;

            resultList.Add(text_data.spoilInt(out tmpdata1, 5));
            resultList.Add(text_data.spoilInt(out tmpdata2, 5));
            resultList.Add(text_data.spoilByte(out tmpdata3, 1));
            resultList.Add(text_data.spoilByte(out tmpdata4, 1));
            resultList.Add(text_data.spoilByte(out tmpdata5, 1));

            WashContainerRemain = tmpdata1;
            RinceContainerRemain = tmpdata2;
            IsFullWasteBuffer = Convert.ToBoolean(tmpdata3);
            ExistWasteBox = Convert.ToBoolean(tmpdata4);
            WasteBoxCondition = (WasteBoxStatus)tmpdata5;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }

    /// <summary>
    /// 廃液タンク状態問合せコマンド
    /// </summary>
    /// <remarks>
    /// 廃液タンク状態問合せコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0521 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0521()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0521;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// 廃液タンク状態問合せコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 廃液タンク状態問合せコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1521 : CarisXCommCommand, IRackRemainAmountInfoSet
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1521()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1521;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 廃液満杯センサ
        /// </summary>
        public Boolean IsFullWasteTank { get; set; } = false;

        /// <summary>
        /// 廃液タンク有無センサ
        /// </summary>
        public Boolean ExistWasteTank { get; set; } = false;

        /// <summary>
        /// 取得時刻
        /// </summary>
        public DateTime TimeStamp { get; set; } = DateTime.MinValue;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(Convert.ToByte(this.IsFullWasteTank).ToString(), 1);
                builder.Append(Convert.ToByte(this.ExistWasteTank).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

#if SIMULATOR
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            this.TimeStamp = DateTime.Now;

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1 = 0;
            Byte tmpdata2 = 0;

            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));

            this.IsFullWasteTank = Convert.ToBoolean(tmpdata1);
            this.ExistWasteTank = Convert.ToBoolean(tmpdata2);

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }

    /// <summary>
    /// キャリブレータ情報通知コマンド
    /// </summary>
    /// <remarks>
    /// キャリブレータ情報通知コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0522 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0522()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0522;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ポート番号
        /// </summary>
        public Int32 PortNo { get; set; }

        /// <summary>
        /// 試薬コード
        /// </summary>
        public Int32 ReagCode { get; set; }

        /// <summary>
        /// キャリブレータ本数
        /// </summary>
        public Int32 CalibratorLotCount { get; set; }

        /// <summary>
        /// キャリブレータロット
        /// </summary>
        public List<CalibratorLot> CalibratorLot { get; set; }

#if SIMULATOR
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.PortNo.ToString(), 2);
                builder.Append(this.ReagCode.ToString(), 3);
                builder.Append(this.CalibratorLot.Count.ToString(), 1);

                foreach (var calib in this.CalibratorLot)
                {
                    builder.Append(calib.CalibratorLotNo, 6);
                    builder.Append(calib.Concentration.Count.ToString(), 1);
                    foreach (var conc in calib.Concentration)
                    {
                        builder.Append(conc.ToString(), 6);
                    }
                }

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;     //ポート番号
            Int32 tmpdata2;     //試薬コード
            Int32 tmpdata3;     //キャリブレータ本数
            String tmpdata4;    //キャリブレータロットNo
            Int32 tmpdata5;     //補正ポイント数
            Double tmpdata6;    //補正ポイント
            List<Double> conc = new List<Double>();

            CalibratorLot = new List<CalibratorLot>();

            resultList.Add(text_data.spoilInt(out tmpdata1, 2));
            resultList.Add(text_data.spoilInt(out tmpdata2, 3));
            resultList.Add(text_data.spoilInt(out tmpdata3, 1));

            for (int i = 0; i < tmpdata3; i++)
            {
                conc = new List<Double>();

                resultList.Add(text_data.spoilString(out tmpdata4, 6));
                resultList.Add(text_data.spoilInt(out tmpdata5, 1));
                for (int j = 0; j < tmpdata5; j++)
                {
                    resultList.Add(text_data.spoilDouble(out tmpdata6, 6));
                    conc.Add(tmpdata6);
                }

                CalibratorLot.Add(new CalibratorLot(tmpdata4, tmpdata5, conc));
            }

            PortNo = tmpdata1;
            ReagCode = tmpdata2;
            CalibratorLotCount = tmpdata3;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
        #endregion
    }
    /// <summary>
    /// キャリブレータ情報通知コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// キャリブレータ情報通知コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1522 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1522()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1522;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// STAT状態通知コマンド
    /// </summary>
    /// <remarks>
    /// STAT状態通知コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0591 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0591()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0591;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 状態
        /// </summary>
        public STATStatus Status { get; set; }

#if SIMULATOR
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.Status).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            Status = (STATStatus)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
        #endregion
    }
    /// <summary>
    /// STAT状態通知コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// STAT状態通知コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1591 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1591()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1591;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 分取完了通知コマンド
    /// </summary>
    /// <remarks>
    /// 分取完了通知コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_0596 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_0596()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command0596;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID { get; set; }

        /// <summary>
        /// 検体種別
        /// </summary>
        public Int32 SampleKind { get; set; }

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackID, 4);
                builder.Append(this.SampleKind.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            String tmpdata1 = "";
            Int32 tmpdata2 = 0;
            resultList.Add(text_data.spoilString(out tmpdata1, 4));
            resultList.Add(text_data.spoilInt(out tmpdata2, 2));
            this.RackID = tmpdata1;
            this.SampleKind = tmpdata2;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }
    /// <summary>
    /// 分取完了通知コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 分取完了通知コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class SlaveCommCommand_1596 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SlaveCommCommand_1596()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.Command1596;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    #endregion

    #endregion

    #region Host
    /// <summary>
    /// 検査依頼問い合わせメッセージコマンド
    /// </summary>
    /// <remarks>
    /// 検査依頼問い合わせメッセージコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class HostCommCommand_0001 : CarisXCommCommand
    {
        #region [定数定義]

        /// <summary>
        /// サンプル区分
        /// </summary>
        public enum SampleType
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
        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// メッセージ識別
        /// </summary>
        private String msg = "R";

        /// <summary>
        /// 装置番号
        /// </summary>
        private Int32 deviceNo = 1;

        /// <summary>
        /// サンプル区分(N固定)
        /// </summary>
        private SampleType sampleType = SampleType.N;

        ///// <summary>
        ///// 問い合わせID
        ///// </summary>
        //private String askID;

        /// <summary>
        /// ラックID
        /// </summary>
        private CarisXIDString rackId = String.Empty;

        /// <summary>
        /// ラックポジション
        /// </summary>
        private Int32 rackPos = 0;
        /// <summary>
        /// 検体ID
        /// </summary>
        private String sampleId = String.Empty;
        /// <summary>
        /// 受付番号
        /// </summary>
        private Int32 receiptNo = 0;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HostCommCommand_0001()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.HostCommand0001;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// メッセージ識別
        /// </summary>
        public String Msg
        {
            get
            {
                return msg;
            }
            set
            {
                msg = value;
            }
        }

        /// <summary>
        /// 装置番号
        /// </summary>
        public Int32 DeviceNo
        {
            get
            {
                return deviceNo;
            }
            set
            {
                deviceNo = value;
            }
        }

        /// <summary>
        /// サンプル区分
        /// </summary>
        public SampleType SampleType1
        {
            get
            {
                return this.sampleType;
            }
            set
            {
                this.sampleType = value;
            }
        }

        /////// <summary>
        /////// 問い合わせID
        /////// </summary>
        ////public String AskID
        ////{
        ////   get
        ////   {
        ////       return askID;
        ////   }
        ////   set
        ////   {
        ////       askID = value;
        ////   }
        ////}

        /// <summary>
        /// ラックID
        /// </summary>
        public CarisXIDString RackId
        {
            set
            {
                this.rackId = value;
            }
            get
            {
                return this.rackId;
            }
        }

        /// <summary>
        /// ラックポジション
        /// </summary>
        public Int32 RackPos
        {
            get
            {
                return rackPos;
            }
            set
            {
                rackPos = value;
            }
        }

        /// <summary>
        /// 検体ID
        /// </summary>
        public String SampleId
        {
            get
            {
                return this.sampleId;
            }
            set
            {
                this.sampleId = value;
            }
        }

        /// <summary>
        /// 受付番号
        /// </summary>
        public Int32 ReceiptNo
        {
            get
            {
                return receiptNo;
            }
            set
            {
                receiptNo = value;
            }
        }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.msg.ToString(), 1);
                builder.Append(this.deviceNo.ToString(), 1);

                // Gでは設定画面があり、付与する・しないがある。
                // HostSimでは無いこと前提で処理されており、CarisXのオンライン仕様書ではN固定の表記となっている。
                // HostSim使用の為固定で付与しないものとする。
                //builder.Append( this.sampleType.ToString(), 1 );
                builder.Append(this.sampleType.ToString(), 1);

                // 受付番号が設定されていれば受付番号を使用
                // 受付番号が設定されていない状態でサンプルIDが設定されていればサンプルIDを使用
                // 受付番号・サンプルIDいずれも設定されていなければラックID-Posを使用。
                if (this.receiptNo != 0)
                {
                    builder.Append(this.receiptNo.ToString(), 16);
                }
                else if (this.sampleId != String.Empty)
                {
                    builder.AppendRight(this.sampleId, 16);
                }
                else
                {

                    builder.AppendRight(String.Format("{0}-{1}", this.rackId.DispPreCharString, this.rackPos), 16);
                }

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion
    }


    /// <summary>
    /// 測定項目情報
    /// </summary>
    public class ProtoInfo
    {
        /// <summary>
        /// 測定項目番号
        /// </summary>
        public Int32 protoNo;        // 測定項目番号
        /// <summary>
        /// 後希釈倍率
        /// </summary>
        public HostAutoDil afterDil;       // 後希釈倍率
    }

    /// <summary>
    /// 検査依頼メッセージコマンド
    /// </summary>
    /// <remarks>
    /// 検査依頼メッセージコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class HostCommCommand_0002 : CarisXCommCommand
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// メッセージ識別
        /// </summary>
        private String msg = "W";

        /// <summary>
        /// 装置番号
        /// </summary>
        private Int32 deviceNo = 1;

        /// <summary>
        /// サンプル区分
        /// </summary>
        private HostSampleType hostSampleType = HostSampleType.N;

        /// <summary>
        /// 受付ID
        /// </summary>
        private Int32 recepID = 0;

        /// <summary>
        /// サンプルID
        /// </summary>
        private String smpID = String.Empty;

        /// <summary>
        /// ラックID
        /// </summary>
        private String rackID = String.Empty;

        /// <summary>
        /// ポジション
        /// </summary>
        private Int32 rackPos = 0;

        /// <summary>
        /// 検体種別
        /// </summary>
        private HostSampleKind smpKind = HostSampleKind.SerumBloodPlasma;

        /// <summary>
        /// サンプルロット番号
        /// </summary>
        private String sampleLot = String.Empty;

        /// <summary>
        /// 手希釈倍率
        /// </summary>
        private Int32 manualDil = 0;

        /// <summary>
        /// コメント
        /// </summary>
        private String comment = "";

        /// <summary>
        /// 測定項目数
        /// </summary>
        private Int32 numOfMeasItem = 0;

        /// <summary>
        /// 測定項目情報
        /// </summary>
        private List<ProtoInfo> measItems = new List<ProtoInfo>();

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HostCommCommand_0002()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.HostCommand0002;
            this.CommandId = (int)this.CommKind;

            // デバイス番号を設定から取得する
            this.deviceNo = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.DeviceNoParameter.DeviceNo;

            ////TODO:可変調のデータ、「測定項目数」最大値の50バイト分だけ固定長でメモリ確保
            //measItems = new ProtoInfo[50];
            //for ( Int32 i = 0; i < measItems.Count(); i++ )
            //    measItems[i] = new ProtoInfo();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// メッセージ識別
        /// </summary>
        public String MsgId
        {
            get
            {
                return msg;
            }
            set
            {
                msg = value;
            }
        }

        /// <summary>
        /// 装置番号
        /// </summary>
        public Int32 DeviceNo
        {
            get
            {
                return deviceNo;
            }
            set
            {
                deviceNo = value;
            }
        }

        /// <summary>
        /// サンプル区分
        /// </summary>
        public HostSampleType SampleType
        {
            get
            {
                return this.hostSampleType;
            }
            set
            {
                this.hostSampleType = value;
            }
        }

        /// <summary>
        /// 受付番号
        /// </summary>
        public Int32 ReceiptNumber
        {
            get
            {
                return recepID;
            }
            set
            {
                recepID = value;
            }
        }

        /// <summary>
        /// サンプルID
        /// </summary>
        public String SampleID
        {
            get
            {
                return smpID;
            }
            set
            {
                smpID = value;
            }
        }

        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID
        {
            get
            {
                return rackID;
            }
            set
            {
                rackID = value;
            }
        }

        /// <summary>
        /// ラックポジション
        /// </summary>
        public Int32 RackPos
        {
            get
            {
                return rackPos;
            }
            set
            {
                rackPos = value;
            }
        }

        /// <summary>
        /// サンプル種別
        /// </summary>
        public HostSampleKind SampleMaterialType
        {
            get
            {
                return smpKind;
            }
            set
            {
                smpKind = value;
            }
        }

        /// <summary>
        /// サンプルロット
        /// </summary>
        /// <remarks>
        /// サンプルが検体の場合はスペース、精度管理検体の場合は精度管理検体ロットがセットされます。
        /// </remarks>
        public String SampleLotNumber
        {
            get
            {
                return sampleLot;
            }
            set
            {
                sampleLot = value;
            }
        }

        /// <summary>
        /// 手希釈倍率
        /// </summary>
        public Int32 ManualDil
        {
            get
            {
                return manualDil;
            }
            set
            {
                manualDil = value;
            }
        }

        /// <summary>
        /// コメント
        /// </summary>
        public String Comment
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
            }
        }

        /// <summary>
        /// 測定項目数
        /// </summary>
        public Int32 NumOfMeasItem
        {
            get
            {
                return numOfMeasItem;
            }
            set
            {
                numOfMeasItem = value;
            }
        }

        /// <summary>
        /// 測定項目情報
        /// </summary>
        public List<ProtoInfo> MeasItems
        {
            get
            {
                return this.measItems;
            }
            set
            {
                this.measItems = value;
            }
        }


        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 検査依頼メッセージコマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// 検査依頼メッセージコマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット  
            //Int32 tmpSampleType;
            //Int32 tmpSmpKind;
            Int32 tmpAfterDil;
            String tmpString;
            Boolean resultTemp = false;
            resultList.Add(text_data.spoilString(out msg, 1));
            resultList.Add(text_data.spoilInt(out deviceNo, 1));

            // サンプル区分はシステムパラメータに因らず読み込む。
            resultTemp = text_data.spoilString(out tmpString, 1);
            switch (tmpString)
            {
                case "N":
                    hostSampleType = HostSampleType.N;
                    break;
                case "C":
                    hostSampleType = HostSampleType.C;
                    break;
                default:
                    if (resultTemp == false && Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UseHostSampleType)
                    {
                        resultList.Add(false);
                    }
                    break;
            }
            //resultList.Add( text_data.spoilInt( out recepID, 4 ) );
            resultList.Add(text_data.spoilString(out tmpString, 4));//只是用于调试LiS 
            Int32 outData = 0;
            if (Int32.TryParse(tmpString, out outData))
            {
                recepID = outData;
            }
            resultList.Add(text_data.spoilString(out smpID, 16));
            resultList.Add(text_data.spoilString(out rackID, 4));
            resultList.Add(text_data.spoilString(out tmpString, 1));
            rackPos = SubFunction.SafeParseInt32(tmpString);

            text_data.spoilString(out tmpString, 1);
            switch (tmpString)
            {
                case "S":
                    this.SampleMaterialType = HostSampleKind.SerumBloodPlasma;
                    resultList.Add(true);
                    break;
                case "U":
                    this.SampleMaterialType = HostSampleKind.Urine;
                    resultList.Add(true);
                    break;
                default:
                    resultList.Add(false);
                    break;

            }
            resultList.Add(text_data.spoilString(out sampleLot, 8));
            resultList.Add(text_data.spoilInt(out manualDil, 4));

            // コメントはシステムパラメータにより読み込む。
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableComment)
            {
                resultList.Add(text_data.spoilString(out comment, 80));
            }
            resultList.Add(text_data.spoilString(out tmpString, 2));
            numOfMeasItem = SubFunction.SafeParseInt32(tmpString);
            //resultList.Add( text_data.spoilInt( out numOfMeasItem, 3 ) );

            for (Int32 i = 0; i < this.numOfMeasItem; i++)
            {
                var protoInfo = new ProtoInfo();
                resultList.Add(text_data.spoilInt(out protoInfo.protoNo, 3));
                resultList.Add(text_data.spoilInt(out tmpAfterDil, 1));
                protoInfo.afterDil = (HostAutoDil)tmpAfterDil;
                this.measItems.Add(protoInfo);
            }

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// 検査結果メッセージコマンド
    /// </summary>
    /// <remarks>
    /// 検査結果メッセージコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class HostCommCommand_0003 : CarisXCommCommand
    {
        #region [定数定義]

        ///// <summary>
        ///// サンプル区分
        ///// </summary>
        //public enum SampleType
        //{
        //    /// <summary>
        //    /// 検体(検体ラック使用、または搬送ライン上の検体)
        //    /// </summary>
        //    N = 'N',
        //    /// <summary>
        //    /// QC(精度管理)用コントロール(コントロール用ラック使用)
        //    /// </summary>
        //    C = 'C'
        //}

        ///// <summary>
        ///// サンプル種別
        ///// </summary>
        //public enum SmpKind
        //{
        //    /// <summary>
        //    /// 血清・血漿
        //    /// </summary>
        //    SerumBloodPlasma = 'S',
        //    /// <summary>
        //    /// 尿
        //    /// </summary>
        //    Urine = 'U'
        //}

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// メッセージ識別
        /// </summary>
        private String msg = "D";

        /// <summary>
        /// 装置番号
        /// </summary>
        private Int32 deviceNo = 1;

        /// <summary>
        /// サンプル区分
        /// </summary>
        private HostSampleType sampleType = HostSampleType.N;

        /// <summary>
        /// 受付ID
        /// </summary>
        private Int32 recepID;

        /// <summary>
        /// シーケンスNo.
        /// </summary>
        public Int32 seqNo;

        /// <summary>
        /// サンプルID
        /// </summary>
        private String sampleId;

        /// <summary>
        /// ラックID
        /// </summary>
        private String rackID;

        /// <summary>
        /// ポジション
        /// </summary>
        private Int32 rackPos;

        /// <summary>
        /// サンプル種別
        /// </summary>
        private HostSampleKind smpKind = HostSampleKind.SerumBloodPlasma;

        /// <summary>
        /// サンプルロット番号
        /// </summary>
        private String smpLot;

        /// <summary>
        /// 手希釈倍率
        /// </summary>
        private Int32 manualDil;

        /// <summary>
        /// コメント
        /// </summary>
        private String comment = "";

        /// <summary>
        /// 項目番号
        /// </summary>
        private Int32 protoNo;

        /// <summary>
        /// カウント値
        /// </summary>
        private String dispCount;

        /// <summary>
        /// 濃度値
        /// </summary>
        private String conc;

        /// <summary>
        /// 判定
        /// </summary>
        private String judge = "";

        /// <summary>
        /// リマーク
        /// </summary>
        private Int64 remark;

        /// <summary>
        /// 後希釈倍率
        /// </summary>
        private HostAutoDil autoDil;

        /// <summary>
        /// 試薬ロット番号
        /// </summary>
        private String reagLotNo;

        /// <summary>
        /// 測定日
        /// </summary>
        private DateTime measDateTime;

        ///// <summary>
        ///// 測定時間
        ///// </summary>
        //private Int32 measTime;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HostCommCommand_0003()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.HostCommand0003;
            this.CommandId = (int)this.CommKind;

            // 設定情報からデバイス番号を取得する。
            this.deviceNo = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.DeviceNoParameter.DeviceNo;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// メッセージ識別
        /// </summary>
        public String Msg
        {
            get
            {
                return msg;
            }
            set
            {
                msg = value;
            }
        }

        /// <summary>
        /// 装置番号
        /// </summary>
        public Int32 DeviceNo
        {
            get
            {
                return deviceNo;
            }
            set
            {
                deviceNo = value;
            }
        }

        /// <summary>
        /// サンプル区分
        /// </summary>
        public HostSampleType SampleType
        {
            get
            {
                return sampleType;
            }
            set
            {
                sampleType = value;
            }
        }

        /// <summary>
        /// 受付番号
        /// </summary>
        public Int32 ReceiptNumber
        {
            get
            {
                return recepID;
            }
            set
            {
                recepID = value;
            }
        }

        /// <summary>
        /// シーケンスNo.
        /// </summary>
        public Int32 SeqNo
        {
            get
            {
                return seqNo;
            }
            set
            {
                seqNo = value;
            }
        }

        /// <summary>
        /// サンプルID
        /// </summary>
        public String SampleId
        {
            get
            {
                return sampleId;
            }
            set
            {
                sampleId = value;
            }
        }

        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID
        {
            get
            {
                return rackID;
            }
            set
            {
                rackID = value;
            }
        }

        /// <summary>
        /// ポジション
        /// </summary>
        public Int32 RackPos
        {
            get
            {
                return rackPos;
            }
            set
            {
                rackPos = value;
            }
        }

        /// <summary>
        /// サンプル種別
        /// </summary>
        public HostSampleKind HostSampleKind
        {
            get
            {
                return smpKind;
            }
            set
            {
                smpKind = value;
            }
        }

        /// <summary>
        /// サンプルロット
        /// </summary>
        public String SampleLot
        {
            get
            {
                return smpLot;
            }
            set
            {
                smpLot = value;
            }
        }

        /// <summary>
        /// 手希釈倍率
        /// </summary>
        public Int32 ManualDil
        {
            get
            {
                return manualDil;
            }
            set
            {
                manualDil = value;
            }
        }

        /// <summary>
        /// コメント
        /// </summary>
        public String Comment
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
            }
        }

        /// <summary>
        /// 項目番号
        /// </summary>
        public Int32 ProtocolNumber
        {
            get
            {
                return protoNo;
            }
            set
            {
                protoNo = value;
            }
        }

        /// <summary>
        /// カウント値
        /// </summary>
        public String DispCount
        {
            get
            {
                return dispCount;
            }
            set
            {
                dispCount = value;
            }
        }

        /// <summary>
        /// 濃度値
        /// </summary>
        public String Conc
        {
            get
            {
                return conc;
            }
            set
            {
                conc = value;
            }
        }

        /// <summary>
        /// 判定
        /// </summary>
        public String Judge
        {
            get
            {
                return judge;
            }
            set
            {
                judge = value;
            }
        }

        /// <summary>
        /// リマーク
        /// </summary>
        public Int64 Remark
        {
            get
            {
                return remark;
            }
            set
            {
                remark = value;
            }
        }

        /// <summary>
        /// 後希釈倍率
        /// </summary>
        public HostAutoDil AutoDilution
        {
            get
            {
                return this.autoDil;
            }
            set
            {
                this.autoDil = value;
            }
        }

        /// <summary>
        /// 試薬ロット番号
        /// </summary>
        public String ReagLotNo
        {
            get
            {
                return reagLotNo;
            }
            set
            {
                reagLotNo = value;
            }
        }

        /// <summary>
        /// 測定日時
        /// </summary>
        public DateTime MeasDateTime
        {
            get
            {
                return measDateTime;
            }
            set
            {
                measDateTime = value;
            }
        }

        ///// <summary>
        ///// 測定時間
        ///// </summary>
        //public Int32 MeasTime
        //{
        //    get
        //    {
        //        return measTime;
        //    }
        //    set
        //    {
        //        measTime = value;
        //    }
        //}

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.msg.ToString(), 1);
                builder.Append(this.deviceNo.ToString(), 1);
                builder.Append(this.sampleType.ToString(), 1);
                builder.AppendRight(this.recepID.ToString(), 4);
                builder.Append(this.seqNo.ToString(), 4);
                builder.Append(this.sampleId, 16);
                builder.Append(this.rackID, 4);
                builder.Append(this.rackPos.ToString(), 1);
                builder.Append(this.smpKind.ToString(), 1);
                builder.AppendRight(this.smpLot, 8);
                builder.AppendRight(this.manualDil.ToString(), 4);

                // コメントはフラグを見て判断する
                if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.HostParameter.UsableComment)
                {
                    builder.AppendRight(this.comment, 80);
                }

                builder.Append(this.protoNo.ToString(), 3);
                builder.Append(this.dispCount, 8);
                builder.AppendRight(this.conc, 10);
                builder.AppendRight(this.judge, 10);

                // リマークが付与されていない場合、スペースを送信する。
                if (this.remark == 0)
                {
                    builder.Append(this.remark.ToString(" "), 16);
                }
                else
                {
                    builder.Append(this.remark.ToString("X"), 16);
                }

                builder.Append(((Int32)this.autoDil).ToString(), 1);
                builder.Append(this.reagLotNo, 8);
                builder.Append(this.measDateTime.ToString("yyyyMMdd"), 8);
                builder.Append(this.measDateTime.ToString("HHmmss"), 6);
                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion
    }


    /// <summary>
    /// 装置ステータス問い合わせメッセージコマンド
    /// </summary>
    /// <remarks>
    /// 装置ステータス問い合わせメッセージコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class HostCommCommand_0004 : CarisXCommCommand
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// メッセージ識別
        /// </summary>
        private String msg = "Q";

        /// <summary>
        /// 装置番号
        /// </summary>
        private Int32 deviceNo = 1;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HostCommCommand_0004()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.HostCommand0004;
            this.CommandId = (int)this.CommKind;

            // 設定情報からデバイス番号を取得する。
            this.deviceNo = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.DeviceNoParameter.DeviceNo;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// メッセージ識別
        /// </summary>
        public String Msg
        {
            get
            {
                return msg;
            }
            set
            {
                msg = value;
            }
        }

        /// <summary>
        /// 装置番号
        /// </summary>
        public Int32 DeviceNo
        {
            get
            {
                return deviceNo;
            }
            set
            {
                deviceNo = value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 装置ステータス問い合わせコマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// 装置ステータス問い合わせコマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット  
            resultList.Add(text_data.spoilString(out msg, 1));
            resultList.Add(text_data.spoilInt(out deviceNo, 1));

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }


    /// <summary>
    /// 装置ステータスメッセージコマンド
    /// </summary>
    /// <remarks>
    /// 装置ステータスメッセージコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class HostCommCommand_0005 : CarisXCommCommand
    {
        #region [定数定義]

        /// <summary>
        /// 検査依頼受付ステータス
        /// </summary>
        public enum InspectionAcceptStatus
        {
            /// <summary>
            /// 検査依頼受付不可
            /// </summary>
            NotInspectionAccept = 0,
            /// <summary>
            /// 依頼受付可(待機中)
            /// </summary>
            WaitingInspectionAccept = 1,
            /// <summary>
            /// 依頼受付可(分析中)
            /// </summary>
            AnalysingInspectionAccept = 2
        }

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// メッセージ識別
        /// </summary>
        private String msg = "S";

        /// <summary>
        /// 装置番号
        /// </summary>
        private Int32 deviceNo = 1;

        /// <summary>
        /// ステータス
        /// </summary>
        private InspectionAcceptStatus status = InspectionAcceptStatus.NotInspectionAccept;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HostCommCommand_0005()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.HostCommand0005;
            this.CommandId = (int)this.CommKind;

            // 設定情報からデバイス番号を取得する。
            this.deviceNo = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.DeviceNoParameter.DeviceNo;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// メッセージ識別
        /// </summary>
        public String Msg
        {
            get
            {
                return msg;
            }
            set
            {
                msg = value;
            }
        }

        /// <summary>
        /// 装置番号
        /// </summary>
        public Int32 DeviceNo
        {
            get
            {
                return deviceNo;
            }
            set
            {
                deviceNo = value;
            }
        }

        /// <summary>
        /// ステータス
        /// </summary>
        public InspectionAcceptStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.msg.ToString(), 1);
                builder.Append(this.deviceNo.ToString(), 1);
                builder.Append(((Int32)this.status).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion
    }

    #endregion

    #region RackTransfer

    #region User→Rack

    /// <summary>
    /// ソフトウェア識別コマンド
    /// </summary>
    /// <remarks>
    /// ソフトウェア識別コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0001 : CarisXCommCommand
    {
        #region [定数定義]
        /// <summary>
        /// ソフトウェアー種別
        /// </summary>
        public enum SoftwareKind
        {
            /// <summary>
            /// DPR側ソフト終了
            /// </summary>
            Shutdown = 0,
            /// <summary>
            /// ランニング
            /// </summary>
            Running = 1,
            /// <summary>
            /// メンテナンス
            /// </summary>
            Maintenance = 2
        }

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0001()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0001;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// コントロール
        /// </summary>
        public CommandControlParameter Control { get; set; } = CommandControlParameter.Abort;

        /// <summary>
        /// ソフトウェアー種別
        /// </summary>
        public SoftwareKind SoftwareKind1 { get; set; } = SoftwareKind.Maintenance;

        /// <summary>
        /// MACアドレス
        /// </summary>
        public String MacAddress { get; set; } = CarisXSubFunction.GetLocalMacAddress();

        /// <summary>
        /// モジュール構成フラグ
        /// </summary>
        public Int32 ModuleConfigFlg { get; set; } = 0; //不要になったがコマンドの変更すると調整が必要なのでそのまま。

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.SoftwareKind1).ToString(), 1);
                builder.Append(((Int32)this.Control).ToString(), 1);
                builder.Append(this.MacAddress, 12);
                builder.Append(((Int32)this.ModuleConfigFlg).ToString(), 5);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1 = 0;
            Byte tmpdata2 = 0;
            String tmpdata3 = "";
            Int32 tmpdata4 = 0;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            resultList.Add(text_data.spoilString(out tmpdata3, 12));
            resultList.Add(text_data.spoilInt(out tmpdata4, 5));
            this.SoftwareKind1 = (SoftwareKind)tmpdata1;
            this.Control = (CommandControlParameter)tmpdata2;
            this.MacAddress = tmpdata3;
            this.ModuleConfigFlg = tmpdata4;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }
    /// <summary>
    /// ソフトウェア識別コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ソフトウェア識別コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1001 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1001()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1001;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// モーターパラメータ設定コマンド
    /// </summary>
    /// <remarks>
    /// モーターパラメータ設定コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0002 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0002()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0002;
            this.CommandId = (int)this.CommKind;

            this.MotorSpeed = new ItemMParam[6];
            for (Int32 i = 0; i < this.MotorSpeed.Count(); i++)
                this.MotorSpeed[i] = new ItemMParam();
            this.MotorOffset = new double[10];
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// モーター番号
        /// </summary>
        public Int32 MotorNo { get; set; } = 0;

        /// <summary>
        /// モータースピード
        /// </summary>
        public ItemMParam[] MotorSpeed { get; set; }

        /// <summary>
        /// モーターオフセット
        /// </summary>
        public double[] MotorOffset { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.MotorNo.ToString(), 3);

                Int32 i;
                for (i = 0; i < this.MotorSpeed.Count(); i++)
                {
                    builder.Append(this.MotorSpeed[i].InitSpeed.ToString(), 6);
                    builder.Append(this.MotorSpeed[i].TopSpeed.ToString(), 6);
                    builder.Append(this.MotorSpeed[i].Accel.ToString(), 6);
                    builder.Append(this.MotorSpeed[i].ConstSpeed.ToString(), 6);
                }
                for (i = 0; i < this.MotorOffset.Count(); i++)
                {
                    builder.Append(this.MotorOffset[i].ToString("F4"), 6);
                }

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

        //0109コマンド等でも継承するため、#if DEBUGは書かない

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 3));
            for (int i = 0; i < this.MotorSpeed.Count(); i++)
            {
                resultList.Add(text_data.spoilInt(out this.MotorSpeed[i].InitSpeed, 6));
                resultList.Add(text_data.spoilInt(out this.MotorSpeed[i].TopSpeed, 6));
                resultList.Add(text_data.spoilInt(out this.MotorSpeed[i].Accel, 6));
                resultList.Add(text_data.spoilInt(out this.MotorSpeed[i].ConstSpeed, 6));
            }
            for (int i = 0; i < this.MotorOffset.Count(); i++)
            {
                resultList.Add(text_data.spoilDouble(out this.MotorOffset[i], 6));
            }
            this.MotorNo = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }
    /// <summary>
    /// モーターパラメータ設定コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// モーターパラメータ設定コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1002 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1002()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1002;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// シャットダウンコマンド
    /// </summary>
    /// <remarks>
    /// シャットダウンコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0003 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0003()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0003;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// シャットダウンコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// シャットダウンコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1003 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1003()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1003;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// システムパラメータコマンド
    /// </summary>
    /// <remarks>
    /// システムパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0004 : CarisXCommCommand
    {
        #region [定数定義]

        /// <summary>
        /// 自動立ち上げ日
        /// </summary>
        [Flags]
        public enum DayOfWeek
        {
            /// <summary>
            /// 指定無し
            /// </summary>
            None = 0x0,
            /// <summary>
            /// 月曜日
            /// </summary>
            Monday = 0x1,
            /// <summary>
            /// 火曜日
            /// </summary>
            Tuesday = 0x2,
            /// <summary>
            /// 水曜日
            /// </summary>
            Wednesday = 0x4,
            /// <summary>
            /// 木曜日
            /// </summary>
            Thursday = 0x8,
            /// <summary>
            /// 金曜日
            /// </summary>
            Friday = 0x10,
            /// <summary>
            /// 土曜日
            /// </summary>
            Saturday = 0x20,
            /// <summary>
            /// 日曜日
            /// </summary>
            Sunday = 0x40
        }

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0004()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0004;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 検体バーコード使用有無
        /// </summary>
        public Byte UsableSampleBC { get; set; } = 0;

        /// <summary>
        /// 検体ID固定長
        /// </summary>
        public Byte UsableFixedLengthForSampleID { get; set; } = 0;

        /// <summary>
        /// 検体ID桁数
        /// </summary>
        public Int32 SampleIDLength { get; set; } = 0;

        /// <summary>
        /// 自動立ち上げタイマー使用有無
        /// </summary>
        public Byte UsableAutoStartUp { get; set; } = 0;

        /// <summary>
        /// 自動立ち上げ日(曜日)
        /// </summary>
        public DayOfWeek DayForAutoStartUp { get; set; } = DayOfWeek.Monday;

        /// <summary>
        /// 自動立ち上げ時間(時) 
        /// </summary>
        public Int32 HourForAutoStartUp { get; set; } = 0;

        /// <summary>
        /// 自動立ち上げ時間(分)
        /// </summary>
        public Int32 MinForAutoStartUp { get; set; } = 0;

        /// <summary>
        /// 自動シャットダウン使用有無
        /// </summary>
        public Byte UsableAutoShutdown { get; set; } = 0;

        /// <summary>
        /// 自動シャットダウン時間
        /// </summary>
        public Int32 TimeForAutoShutdown { get; set; } = 1;

        /// <summary>
        /// 自動プライム使用有無
        /// </summary>
        public Byte UsableAutoPrime { get; set; } = 0;

        /// <summary>
        /// 警告灯使用有無
        /// </summary>
        public Byte UsableWarningLight { get; set; } = 0;

        /// <summary>
        /// フラッシュプライム起動時間 
        /// </summary>
        public Int32 TimeFlashPrime { get; set; } = 0;

        /// <summary>
        /// エラー音量
        /// </summary>
        public Int32 VolumeError { get; set; } = 0;

        /// <summary>
        /// 起動時の残量チェック
        /// </summary>
        public Byte CheckRemainStartUp { get; set; } = 0;

        /// <summary>
        /// ラックカバーエラー通知時間
        /// </summary>
        public Int32 TimeRackCoverErrorNotification { get; set; } = 1;

        /// <summary>
        /// 希釈液プライム回数
        /// </summary>
        public Int32 NoOfDilutionPrimeTimes { get; set; } = 0;

        /// <summary>
        /// R1プライム回数
        /// </summary>
        public Int32 NoOfR1PrimeTimes { get; set; } = 0;

        /// <summary>
        /// R2プライム回数
        /// </summary>
        public Int32 NoOfR2PrimeTimes { get; set; } = 0;

        /// <summary>
        /// B/F1プライム回数
        /// </summary>
        public Int32 NoOfBF1PrimeTimes { get; set; } = 0;

        /// <summary>
        /// B/F2プライム回数
        /// </summary>
        public Int32 NoOfBF2PrimeTimes { get; set; } = 0;

        /// <summary>
        /// プレトリガプライム回数
        /// </summary>
        public Int32 NoOfPreTriggerPrimeTimes { get; set; } = 0;

        /// <summary>
        /// トリガプライム回数
        /// </summary>
        public Int32 NoOfTriggerPrimeTimes { get; set; } = 0;

        /// <summary>
        /// 希釈液プライム量
        /// </summary>
        public Int32 VolumeDilutionPrime { get; set; } = 0;

        /// <summary>
        /// R1プライム量
        /// </summary>
        public Int32 VolumePrimeR1 { get; set; } = 0;

        /// <summary>
        /// R2プライム量
        /// </summary>
        public Int32 VolumePrimeR2 { get; set; } = 0;

        /// <summary>
        /// B/F1プライム量
        /// </summary>
        public Int32 VolumePrimeBF1 { get; set; } = 0;

        /// <summary>
        /// B/F2プライム量
        /// </summary>
        public Int32 VolumePrimeBF2 { get; set; } = 0;

        /// <summary>
        /// プレトリガープライム量
        /// </summary>
        public Int32 VolumePreTriggerPrime { get; set; } = 0;

        /// <summary>
        /// トリガープライム量
        /// </summary>
        public Int32 VolumeTriggerPrime { get; set; } = 0;

        /// <summary>
        /// サイクル時間
        /// </summary>
        public Int32 TimeCycle { get; set; } = 0;

        /// <summary>
        /// 反応テーブル温度
        /// </summary>
        public Double TempReActTable { get; set; } = 0;

        /// <summary>
        /// B/Fテーブル温度
        /// </summary>
        public Double TempBFTable { get; set; } = 0;

        /// <summary>
        /// B/F1プローブプレヒート温度
        /// </summary>
        public Double TempPreheatBF1 { get; set; } = 0;

        /// <summary>
        /// B/F2プローブプレヒート温度
        /// </summary>
        public Double TempPreheatBF2 { get; set; } = 0;

        /// <summary>
        /// R1プローブプレヒート温度
        /// </summary>
        public Double TempPreheatR1 { get; set; } = 0;

        /// <summary>
        /// R2プローブプレヒート温度
        /// </summary>
        public Double TempPreheatR2 { get; set; } = 0;

        /// <summary>
        /// 測光部温度
        /// </summary>
        public Double TempPtotometry { get; set; } = 0;

        /// <summary>
        /// 洗浄液分注量
        /// </summary>
        public Int32 VolumeWashDispense { get; set; } = 0;

        /// <summary>
        /// プレトリガ分注量
        /// </summary>
        public Int32 VolumePreTriggerDispense { get; set; } = 0;

        /// <summary>
        /// トリガ分注量
        /// </summary>
        public Int32 VolumeTriggerDispense { get; set; } = 0;

        /// <summary>
        /// 測光モード
        /// </summary>
        public Byte ModePhotometry { get; set; } = 0;

        /// <summary>
        /// ゲート時間
        /// </summary>
        public Int32 GateTime { get; set; } = 0;

        /// <summary>
        /// 試薬不足時の分析の状態
        /// </summary>
        public Byte StateOfReagShortageAnalysis { get; set; } = 0;

        /// <summary>
        /// 希釈液不足時の分析の状態
        /// </summary>
        public Byte StateOfDilutionShortageAnalysis { get; set; } = 0;

        /// <summary>
        /// 分析方式
        /// </summary>
        public Int32 TypeAssay { get; set; } = 0;

        /// <summary>
        /// 目詰まり検体吐き戻し有無
        /// </summary>
        public Byte UsablePutBackClogging { get; set; } = 0;

        /// <summary>
        /// 半量吸い検体吐き戻し有無
        /// </summary>
        public Byte UsablePutBackAtHalf { get; set; } = 0;

        /// <summary>
        /// 試薬ロット切替わり時の処理
        /// </summary>
        public Byte ProcessExchangeReagentLot { get; set; } = 0;

        /// <summary>
        /// 温度エラー時のサンプリング停止有無
        /// </summary>
        public Byte IfDoSampStopInTempErr { get; set; } = 0;

        /// <summary>
        /// 試薬ボトルの泡チェック
        /// </summary>
        public Byte UseBubbleCheck { get; set; } = 0;

        /// <summary>
        /// 検体ラック番号下限
        /// </summary>
        public Int32 SampleRackNoLower { get; set; } = 1;

        /// <summary>
        /// 検体ラック番号上限
        /// </summary>
        public Int32 SampleRackNoUpper { get; set; } = 1;

        /// <summary>
        /// コントロールラック番号下限
        /// </summary>
        public Int32 ControlRackNoLower { get; set; } = 1;

        /// <summary>
        /// コントロールラック番号上限
        /// </summary>
        public Int32 ControlRackNoUpper { get; set; } = 1;

        /// <summary>
        /// キャリブレーションラック番号下限
        /// </summary>
        public Int32 CalibRackNoLower { get; set; } = 1;

        /// <summary>
        /// キャリブレーションラック番号上限
        /// </summary>
        public Int32 CalibRackNoUpper { get; set; } = 1;

        /// <summary>
        /// 搬送ライン使用有無
        /// </summary>
        public Byte UsableTransferLine { get; set; } = 0;

        /// <summary>
        /// 分析モジュール接続台数
        /// </summary>
        public Int32 NoOfModulesConnected { get; set; } = 1;
        
        /// <summary>
        /// 急診有無
        /// </summary>
        public Byte UseEmergencyMode { get; set; } = 0;
                
        /// <summary>
        /// システムパラメータ設定
        /// </summary>
        /// <remarks>
        /// システムパラメータ設定します
        /// </remarks>
        /// <param name="parameter"></param>
        public void SetSystemParameter(CarisXSystemParameter parameter, Int32 moduleIndex = 0 )
        {
            this.UsableSampleBC = Convert.ToByte(parameter.SampleBCRParameter.Enable);                                                  // 検体バーコード使用有無
            this.UsableFixedLengthForSampleID = Convert.ToByte(parameter.SampleBCRParameter.UsableRackIDSampFixedLength);               // 検体ID固定長
            this.SampleIDLength = parameter.SampleBCRParameter.RackIDSampDigit;                                                         // 検体ID桁数

            this.UsableAutoStartUp = Convert.ToByte(parameter.AutoStartupTimerParameter.Enable);                                        // 自動起動タイマー使用有無
            this.DayForAutoStartUp = (DayOfWeek)parameter.AutoStartupTimerParameter.SelectDayOfWeek;                                    // 自動立ち上げ日(曜日)
            this.HourForAutoStartUp = parameter.AutoStartupTimerParameter.AutoStartupHour;                                              // 自動立ち上げ時間(時)
            this.MinForAutoStartUp = parameter.AutoStartupTimerParameter.AutoStartupMinute;                                             // 自動立ち上げ時間(分)

            this.UsableAutoShutdown = Convert.ToByte(parameter.AutoShutdownParameter.Enable);                                           // 自動シャットダウン使用有無
            this.TimeForAutoShutdown = parameter.AutoShutdownParameter.AutoShutdownTime;                                                // 自動シャットダウン時間

            this.UsableAutoPrime = Convert.ToByte(parameter.AutoPrimeParameter.Enable);                                                 // 自動プライム使用有無
            this.UsableWarningLight = Convert.ToByte(parameter.WarningLightParameter.Enable);                                           // 警告灯使用有無
            this.TimeFlashPrime = parameter.FlushParameter.FlushPrimeTime;                                                              // フラッシュプライム起動時間
            this.VolumeError = (Int32)parameter.ErrWarningBeepParameter.BeepVolume;                                                     // エラー音量
            this.CheckRemainStartUp = Convert.ToByte(parameter.ReagentCheckAtStartUpParameter.Enable);                                  // 起動時の残量チェック
            this.TimeRackCoverErrorNotification = parameter.SampleLoaderCoverOpenErrorNotificationTimeParameter.SampCoverOpenTime;      // ラックカバーエラー通知時間

            this.NoOfDilutionPrimeTimes = parameter.PrimeParameter.PrimeCountDilu;                                                      // 希釈液プライム回数
            this.NoOfR1PrimeTimes = parameter.PrimeParameter.PrimeCountR1;                                                              // R1プライム回数
            this.NoOfR2PrimeTimes = parameter.PrimeParameter.PrimeCountR2;                                                              // R2プライム回数
            this.NoOfBF1PrimeTimes = parameter.PrimeParameter.PrimeCountBF1;                                                            // B/F1プライム回数
            this.NoOfBF2PrimeTimes = parameter.PrimeParameter.PrimeCountBF2;                                                            // B/F2プライム回数
            this.NoOfPreTriggerPrimeTimes = parameter.PrimeParameter.PrimeCountPreTrig;                                                 // プレトリガープライム回数
            this.NoOfTriggerPrimeTimes = parameter.PrimeParameter.PrimeCountTrig;                                                       // トリガープライム回数

            this.VolumeDilutionPrime = parameter.PrimeParameter.PrimeVolumeDilu;                                                        // 希釈液プライム量
            this.VolumePrimeR1 = parameter.PrimeParameter.PrimeVolumeR1;                                                                // R1プライム量
            this.VolumePrimeR2 = parameter.PrimeParameter.PrimeVolumeR2;                                                                // R2プライム量
            this.VolumePrimeBF1 = parameter.PrimeParameter.PrimeVolumeBF1;                                                              // B/F1プライム量
            this.VolumePrimeBF2 = parameter.PrimeParameter.PrimeVolumeBF2;                                                              // B/F2プライム量
            this.VolumePreTriggerPrime = parameter.PrimeParameter.PrimeVolumePreTrig;                                                   // プレトリガープライム量
            this.VolumeTriggerPrime = parameter.PrimeParameter.PrimeVolumeTrig;                                                         // トリガープライム量

            this.TimeCycle = parameter.CycleTimeParameter.CycleTime;                                                                    // サイクル時間

            this.TempReActTable = parameter.TemperatureParameter.TempReactionTable;                                                     // 反応テーブル温度
            this.TempBFTable = parameter.TemperatureParameter.TempBFTable;                                                              // B/Fテーブル温度
            this.TempPreheatBF1 = parameter.TemperatureParameter.TempBF1PreHeat;                                                        // B/F1プレヒート温度
            this.TempPreheatBF2 = parameter.TemperatureParameter.TempBF2PreHeat;                                                        // B/F2プレヒート温度
            this.TempPreheatR1 = parameter.TemperatureParameter.TempR1ProbePreHeat;                                                     // R1プローブプレヒート温度
            this.TempPreheatR2 = parameter.TemperatureParameter.TempR2ProbePreHeat;                                                     // R2プローブプレヒート温度
            this.TempPtotometry = parameter.TemperatureParameter.TempChemiLightMeas;                                                    // 測光部温度

            this.VolumeWashDispense = parameter.WashDispVolParameter.DispVolWash;                                                       // 洗浄液分注量.
            this.VolumePreTriggerDispense = parameter.WashDispVolParameter.DispVolPreTrig;                                              // プレトリガ分注量.
            this.VolumeTriggerDispense = parameter.WashDispVolParameter.DispVolTrig;                                                    // トリガ分注量.

            this.ModePhotometry = Convert.ToByte(parameter.PhotometryParameter.PhotoMode);                                              // 測光モード.
            this.GateTime = parameter.PhotometryParameter.MeasTime;                                                                     // ゲート時間
            this.StateOfReagShortageAnalysis = Convert.ToByte(parameter.ProcessAtReagentShortageParameter.ReagShortAssayStatus);        // 試薬不足時の分析状態
            this.StateOfDilutionShortageAnalysis = Convert.ToByte(parameter.ProcessAtDiluentShortageParameter.DiluShortAssayStatus);    // 希釈液不足時の分析状態
            this.TypeAssay = (Int32)parameter.AssayModeParameter.AssayMode;                                                             // 分析方式
            this.UsablePutBackClogging = Convert.ToByte(parameter.ProcessAfterSampleAspiratingErrorParameter.UsablePutBack);            // 目詰まり検体吐き戻し有無
            this.UsablePutBackAtHalf = Convert.ToByte(parameter.ProcessAfterSampleAspiratingErrorParameter.UsablePutBackAtHalf);        // 半量吸い検体吐き戻し有無
            this.ProcessExchangeReagentLot = Convert.ToByte(parameter.ProcessAtReagentLotChange.ReagLotChangeProc);                     // 試薬ロット切替わり時の処理
            this.IfDoSampStopInTempErr = parameter.TemperatureParameter.TempErrorSamplingStopExistence;                                 // 温度エラー時のサンプリグ停止有無
            this.UseBubbleCheck = Convert.ToByte(parameter.BubbleCheckParameter.Enable);                                                // 試薬ボトルの泡チェック

            this.SampleRackNoLower = parameter.RackIDDefinitionParameter.MinRackIDSamp;                                                 // 検体ラック番号下限
            this.SampleRackNoUpper = parameter.RackIDDefinitionParameter.MaxRackIDSamp;                                                 // 検体ラック番号上限
            this.ControlRackNoLower = parameter.RackIDDefinitionParameter.MinRackIDCtrl;                                                // コントロールラック番号下限
            this.ControlRackNoUpper = parameter.RackIDDefinitionParameter.MaxRackIDCtrl;                                                // コントロールラック番号上限
            this.CalibRackNoLower = parameter.RackIDDefinitionParameter.MinRackIDCalib;                                                 // キャリブレーションラック番号下限
            this.CalibRackNoUpper = parameter.RackIDDefinitionParameter.MaxRackIDCalib;                                                 // キャリブレーションラック番号上限

            this.UsableTransferLine = Convert.ToByte(parameter.TransferSystemParameter.Enable);                                         // 搬送ライン使用有無
            this.NoOfModulesConnected = parameter.AssayModuleConnectParameter.NumOfConnected;                                           // 分析モジュール接続台数
            this.UseEmergencyMode = Convert.ToByte(parameter.AssayModeParameter.GetUseEmergencyMode(moduleIndex));                      // 急診有無
        }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.UsableSampleBC.ToString(), 1);
                builder.Append(this.UsableFixedLengthForSampleID.ToString(), 1);
                builder.Append(this.SampleIDLength.ToString(), 2);
                builder.Append(this.UsableAutoStartUp.ToString(), 1);
                builder.Append(((Int32)this.DayForAutoStartUp).ToString(), 3);
                builder.Append(this.HourForAutoStartUp.ToString(), 2);
                builder.Append(this.MinForAutoStartUp.ToString(), 2);
                builder.Append(this.UsableAutoShutdown.ToString(), 1);
                builder.Append(this.TimeForAutoShutdown.ToString(), 4);
                builder.Append(this.UsableAutoPrime.ToString(), 1);
                builder.Append(this.UsableWarningLight.ToString(), 1);
                builder.Append(this.TimeFlashPrime.ToString(), 3);
                builder.Append(((Int32)this.VolumeError).ToString(), 1);
                builder.Append(this.CheckRemainStartUp.ToString(), 1);
                builder.Append(this.TimeRackCoverErrorNotification.ToString(), 3);
                builder.Append(this.NoOfDilutionPrimeTimes.ToString(), 2);
                builder.Append(this.NoOfR1PrimeTimes.ToString(), 2);
                builder.Append(this.NoOfR2PrimeTimes.ToString(), 2);
                builder.Append(this.NoOfBF1PrimeTimes.ToString(), 2);
                builder.Append(this.NoOfBF2PrimeTimes.ToString(), 2);
                builder.Append(this.NoOfPreTriggerPrimeTimes.ToString(), 2);
                builder.Append(this.NoOfTriggerPrimeTimes.ToString(), 2);
                builder.Append(this.VolumeDilutionPrime.ToString(), 4);
                builder.Append(this.VolumePrimeR1.ToString(), 4);
                builder.Append(this.VolumePrimeR2.ToString(), 4);
                builder.Append(this.VolumePrimeBF1.ToString(), 4);
                builder.Append(this.VolumePrimeBF2.ToString(), 4);
                builder.Append(this.VolumePreTriggerPrime.ToString(), 4);
                builder.Append(this.VolumeTriggerPrime.ToString(), 4);
                builder.Append(this.TimeCycle.ToString(), 2);
                builder.Append(this.TempReActTable.ToString("f1"), 4);
                builder.Append(this.TempBFTable.ToString("f1"), 4);
                builder.Append(this.TempPreheatBF1.ToString("f1"), 4);
                builder.Append(this.TempPreheatBF2.ToString("f1"), 4);
                builder.Append(this.TempPreheatR1.ToString("f1"), 4);
                builder.Append(this.TempPreheatR2.ToString("f1"), 4);
                builder.Append(this.TempPtotometry.ToString("f1"), 4);
                builder.Append(this.VolumeWashDispense.ToString(), 4);
                builder.Append(this.VolumePreTriggerDispense.ToString(), 4);
                builder.Append(this.VolumeTriggerDispense.ToString(), 4);
                builder.Append(this.ModePhotometry.ToString(), 1);
                builder.Append(this.GateTime.ToString(), 4);
                builder.Append(this.StateOfReagShortageAnalysis.ToString(), 1);
                builder.Append(this.StateOfDilutionShortageAnalysis.ToString(), 1);
                builder.Append(this.TypeAssay.ToString(), 1);
                builder.Append(this.UsablePutBackClogging.ToString(), 1);
                builder.Append(this.UsablePutBackAtHalf.ToString(), 1);
                builder.Append(this.ProcessExchangeReagentLot.ToString(), 1);
                builder.Append(this.IfDoSampStopInTempErr.ToString(), 1);
                builder.Append(this.UseBubbleCheck.ToString(), 1);
                builder.Append(this.SampleRackNoLower.ToString(), 4);
                builder.Append(this.SampleRackNoUpper.ToString(), 4);
                builder.Append(this.ControlRackNoLower.ToString(), 4);
                builder.Append(this.ControlRackNoUpper.ToString(), 4);
                builder.Append(this.CalibRackNoLower.ToString(), 4);
                builder.Append(this.CalibRackNoUpper.ToString(), 4);
                builder.Append(this.UsableTransferLine.ToString(), 1);
                builder.Append(this.NoOfModulesConnected.ToString(), 1);
                builder.Append(this.UseEmergencyMode.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if SIMULATOR
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1 = 0;
            Byte tmpdata2 = 0;
            Int32 tmpdata3 = 0;
            Byte tmpdata4 = 0;
            Int32 tmpdata5 = 0;
            Int32 tmpdata6 = 0;
            Int32 tmpdata7 = 0;
            Byte tmpdata8 = 0;
            Int32 tmpdata9 = 0;
            Byte tmpdata10 = 0;
            Byte tmpdata11 = 0;
            Int32 tmpdata12 = 0;
            Int32 tmpdata13 = 0;
            Byte tmpdata14 = 0;
            Int32 tmpdata15 = 0;
            Int32 tmpdata16 = 0;
            Int32 tmpdata17 = 0;
            Int32 tmpdata18 = 0;
            Int32 tmpdata19 = 0;
            Int32 tmpdata20 = 0;
            Int32 tmpdata21 = 0;
            Int32 tmpdata22 = 0;
            Int32 tmpdata23 = 0;
            Int32 tmpdata24 = 0;
            Int32 tmpdata25 = 0;
            Int32 tmpdata26 = 0;
            Int32 tmpdata27 = 0;
            Int32 tmpdata28 = 0;
            Int32 tmpdata29 = 0;
            Int32 tmpdata30 = 0;
            Double tmpdata31 = 0;
            Double tmpdata32 = 0;
            Double tmpdata33 = 0;
            Double tmpdata34 = 0;
            Double tmpdata35 = 0;
            Double tmpdata36 = 0;
            Double tmpdata37 = 0;
            Int32 tmpdata38 = 0;
            Int32 tmpdata39 = 0;
            Int32 tmpdata40 = 0;
            Byte tmpdata41 = 0;
            Int32 tmpdata42 = 0;
            Byte tmpdata43 = 0;
            Byte tmpdata44 = 0;
            Byte tmpdata45 = 0;
            Byte tmpdata46 = 0;
            Byte tmpdata47 = 0;
            Byte tmpdata48 = 0;
            Byte tmpdata49 = 0;
            Int32 tmpdata50 = 0;
            Int32 tmpdata51 = 0;
            Int32 tmpdata52 = 0;
            Int32 tmpdata53 = 0;
            Int32 tmpdata54 = 0;
            Int32 tmpdata55 = 0;
            Int32 tmpdata56 = 0;
            Byte tmpdata59 = 0;
            Int32 tmpdata60 = 0;
            Byte tmpdata61 = 0;

            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            resultList.Add(text_data.spoilInt(out tmpdata3, 2));
            resultList.Add(text_data.spoilByte(out tmpdata4, 1));
            resultList.Add(text_data.spoilInt(out tmpdata5, 3));
            resultList.Add(text_data.spoilInt(out tmpdata6, 2));
            resultList.Add(text_data.spoilInt(out tmpdata7, 2));
            resultList.Add(text_data.spoilByte(out tmpdata8, 1));
            resultList.Add(text_data.spoilInt(out tmpdata9, 4));
            resultList.Add(text_data.spoilByte(out tmpdata10, 1));
            resultList.Add(text_data.spoilByte(out tmpdata11, 1));
            resultList.Add(text_data.spoilInt(out tmpdata12, 3));
            resultList.Add(text_data.spoilInt(out tmpdata13, 1));
            resultList.Add(text_data.spoilByte(out tmpdata14, 1));
            resultList.Add(text_data.spoilInt(out tmpdata15, 3));
            resultList.Add(text_data.spoilInt(out tmpdata16, 2));
            resultList.Add(text_data.spoilInt(out tmpdata17, 2));
            resultList.Add(text_data.spoilInt(out tmpdata18, 2));
            resultList.Add(text_data.spoilInt(out tmpdata19, 2));
            resultList.Add(text_data.spoilInt(out tmpdata20, 2));
            resultList.Add(text_data.spoilInt(out tmpdata21, 2));
            resultList.Add(text_data.spoilInt(out tmpdata22, 2));
            resultList.Add(text_data.spoilInt(out tmpdata23, 4));
            resultList.Add(text_data.spoilInt(out tmpdata24, 4));
            resultList.Add(text_data.spoilInt(out tmpdata25, 4));
            resultList.Add(text_data.spoilInt(out tmpdata26, 4));
            resultList.Add(text_data.spoilInt(out tmpdata27, 4));
            resultList.Add(text_data.spoilInt(out tmpdata28, 4));
            resultList.Add(text_data.spoilInt(out tmpdata29, 4));
            resultList.Add(text_data.spoilInt(out tmpdata30, 2));
            resultList.Add(text_data.spoilDouble(out tmpdata31, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata32, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata33, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata34, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata35, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata36, 4));
            resultList.Add(text_data.spoilDouble(out tmpdata37, 4));
            resultList.Add(text_data.spoilInt(out tmpdata38, 4));
            resultList.Add(text_data.spoilInt(out tmpdata39, 4));
            resultList.Add(text_data.spoilInt(out tmpdata40, 4));
            resultList.Add(text_data.spoilByte(out tmpdata41, 1));
            resultList.Add(text_data.spoilInt(out tmpdata42, 4));
            resultList.Add(text_data.spoilByte(out tmpdata43, 1));
            resultList.Add(text_data.spoilByte(out tmpdata44, 1));
            resultList.Add(text_data.spoilInt(out tmpdata50, 1));       // 場所移動
            resultList.Add(text_data.spoilByte(out tmpdata45, 1));
            resultList.Add(text_data.spoilByte(out tmpdata46, 1));
            resultList.Add(text_data.spoilByte(out tmpdata47, 1));
            resultList.Add(text_data.spoilByte(out tmpdata48, 1));
            resultList.Add(text_data.spoilByte(out tmpdata49, 1));
            resultList.Add(text_data.spoilInt(out tmpdata51, 4));
            resultList.Add(text_data.spoilInt(out tmpdata52, 4));
            resultList.Add(text_data.spoilInt(out tmpdata53, 4));
            resultList.Add(text_data.spoilInt(out tmpdata54, 4));
            resultList.Add(text_data.spoilInt(out tmpdata55, 4));
            resultList.Add(text_data.spoilInt(out tmpdata56, 4));
            resultList.Add(text_data.spoilByte(out tmpdata59, 1));
            resultList.Add(text_data.spoilInt(out tmpdata60, 1));
            resultList.Add(text_data.spoilByte(out tmpdata61, 1));

            this.UsableSampleBC = tmpdata1;
            this.UsableFixedLengthForSampleID = tmpdata2;
            this.SampleIDLength = tmpdata3;
            this.UsableAutoStartUp = tmpdata4;
            this.DayForAutoStartUp = (DayOfWeek)tmpdata5;
            this.HourForAutoStartUp = tmpdata6;
            this.MinForAutoStartUp = tmpdata7;
            this.UsableAutoShutdown = tmpdata8;
            this.TimeForAutoShutdown = tmpdata9;
            this.UsableAutoPrime = tmpdata10;
            this.UsableWarningLight = tmpdata11;
            this.TimeFlashPrime = tmpdata12;
            this.VolumeError = tmpdata13;
            this.CheckRemainStartUp = tmpdata14;
            this.TimeRackCoverErrorNotification = tmpdata15;
            this.NoOfDilutionPrimeTimes = tmpdata16;
            this.NoOfR1PrimeTimes = tmpdata17;
            this.NoOfR2PrimeTimes = tmpdata18;
            this.NoOfBF1PrimeTimes = tmpdata19;
            this.NoOfBF2PrimeTimes = tmpdata20;
            this.NoOfPreTriggerPrimeTimes = tmpdata21;
            this.NoOfTriggerPrimeTimes = tmpdata22;
            this.VolumeDilutionPrime = tmpdata23;
            this.VolumePrimeR1 = tmpdata24;
            this.VolumePrimeR2 = tmpdata25;
            this.VolumePrimeBF1 = tmpdata26;
            this.VolumePrimeBF2 = tmpdata27;
            this.VolumePreTriggerPrime = tmpdata28;
            this.VolumeTriggerPrime = tmpdata29;
            this.TimeCycle = tmpdata30;
            this.TempReActTable = tmpdata31;
            this.TempBFTable = tmpdata32;
            this.TempPreheatBF1 = tmpdata33;
            this.TempPreheatBF2 = tmpdata34;
            this.TempPreheatR1 = tmpdata35;
            this.TempPreheatR2 = tmpdata36;
            this.TempPtotometry = tmpdata37;
            this.VolumeWashDispense = tmpdata38;
            this.VolumePreTriggerDispense = tmpdata39;
            this.VolumeTriggerDispense = tmpdata40;
            this.ModePhotometry = tmpdata41;
            this.GateTime = tmpdata42;
            this.StateOfReagShortageAnalysis = tmpdata43;
            this.StateOfDilutionShortageAnalysis = tmpdata44;
            this.TypeAssay = tmpdata50;                         // 場所移動
            this.UsablePutBackClogging = tmpdata45;
            this.UsablePutBackAtHalf = tmpdata46;
            this.ProcessExchangeReagentLot = tmpdata47;
            this.IfDoSampStopInTempErr = tmpdata48;
            this.UseBubbleCheck = tmpdata49;
            this.SampleRackNoLower = tmpdata51;
            this.SampleRackNoUpper = tmpdata52;
            this.ControlRackNoLower = tmpdata53;
            this.ControlRackNoUpper = tmpdata54;
            this.CalibRackNoLower = tmpdata55;
            this.CalibRackNoUpper = tmpdata56;
            this.UsableTransferLine = tmpdata59;
            this.NoOfModulesConnected = tmpdata60;
            this.UseEmergencyMode = tmpdata61;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }
    /// <summary>
    /// システムパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// システムパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1004 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1004()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1004;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// モーター初期化コマンド
    /// </summary>
    /// <remarks>
    /// モーター初期化コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0006 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0006()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0006;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// モーター番号
        /// </summary>
        public Int32 MotorNumber { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append((this.MotorNumber).ToString(), 3);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 3));
            this.MotorNumber = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// モーター初期化コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// モーター初期化コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1006 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1006()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1006;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// モーターセルフチェックコマンド
    /// </summary>
    /// <remarks>
    /// モーターセルフチェックコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0007 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0007()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0007;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// モーター番号
        /// </summary>
        public Int32 MotorNumber { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append((this.MotorNumber).ToString(), 3);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 3));
            this.MotorNumber = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// モーターセルフチェックコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// モーターセルフチェックコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1007 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1007()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1007;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 分析開始コマンド
    /// </summary>
    /// <remarks>
    /// 分析開始コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0011 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0011()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0011;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// コントロール
        /// </summary>
        public CommandControlParameter Control { get; set; } = CommandControlParameter.Start;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.Control).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            this.Control = (CommandControlParameter)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif
        #endregion
    }
    /// <summary>
    /// 分析開始コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 分析開始コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1011 : CarisXCommCommand
    {
        #region [定数定義]
        /// <summary>
        /// 移動結果
        /// </summary>
        public enum MoveResultKind
        {
            CanNot = 0,
            Done = 1
        }
        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1011()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1011;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 移動結果
        /// </summary>
        public MoveResultKind MoveResult { get; set; }

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.MoveResult).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            MoveResult = (MoveResultKind)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// ポーズコマンド
    /// </summary>
    /// <remarks>
    /// ポーズコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0012 : CarisXCommCommand
    {
        #region [定数定義]
        /// <summary>
        /// 引数1
        /// </summary>
        public enum StopParameter
        {
            /// <summary>
            /// サンプリング
            /// </summary>
            SamplingStop = 1,
            /// <summary>
            /// 全体
            /// </summary>
            AllStop = 2
        }
        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0012()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0012;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// コントロール
        /// </summary>
        public CommandControlParameter Control { get; set; } = CommandControlParameter.Pause;

        /// <summary>
        /// 停止種別
        /// </summary>
        public StopParameter Stop { get; set; } = StopParameter.SamplingStop;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.Control).ToString(), 1);
                builder.Append(((Int32)this.Stop).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            Byte tmpdata2;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            this.Control = (CommandControlParameter)tmpdata1;
            this.Stop = (StopParameter)tmpdata2;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }
    /// <summary>
    /// ポーズコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ポーズコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1012 : CarisXCommCommand
    {
        #region [定数定義]
        /// <summary>
        /// ポーズ結果
        /// </summary>
        public enum PauseResultKind
        {
            CanNot = 0,
            Done = 1
        }
        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1012()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1012;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ポーズ結果
        /// </summary>
        public PauseResultKind PauseResult { get; set; }

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.PauseResult).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            PauseResult = (PauseResultKind)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// カレンダー設定コマンド
    /// </summary>
    /// <remarks>
    /// カレンダー設定コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0013 : CarisXCommCommand
    {
        #region [定数定義]
        /// <summary>
        /// 曜日
        /// </summary>
        public enum Week
        {
            /// <summary>
            /// 月曜日
            /// </summary>
            Monday = 0,
            /// <summary>
            /// 火曜日
            /// </summary>
            Tuesday = 1,
            /// <summary>
            /// 水曜日
            /// </summary>
            Wednesday = 2,
            /// <summary>
            /// 木曜日
            /// </summary>
            Thursday = 3,
            /// <summary>
            /// 金曜日
            /// </summary>
            Friday = 4,
            /// <summary>
            /// 土曜日
            /// </summary>
            Saturday = 5,
            /// <summary>
            /// 日曜日
            /// </summary>
            Sunday = 6
        }

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0013()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0013;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 年
        /// </summary>
        public Int32 Year { get; set; } = 0;

        /// <summary>
        /// 月
        /// </summary>
        public Int32 Month { get; set; } = 0;

        /// <summary>
        /// 日
        /// </summary>
        public Int32 Day { get; set; } = 0;

        /// <summary>
        /// 時
        /// </summary>
        public Int32 Hour { get; set; } = 0;

        /// <summary>
        /// 分
        /// </summary>
        public Int32 Minute { get; set; } = 0;

        /// <summary>
        /// 秒
        /// </summary>
        public Int32 Second { get; set; } = 0;

        /// <summary>
        /// 曜日
        /// </summary>
        public Week CmdDayOfWeek { get; set; } = Week.Monday;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append((this.Year % 100).ToString(), 2);
                builder.Append(this.Month.ToString(), 2);
                builder.Append(this.Day.ToString(), 2);
                builder.Append(this.Hour.ToString(), 2);
                builder.Append(this.Minute.ToString(), 2);
                builder.Append(this.Second.ToString(), 2);
                builder.Append(((Int32)this.CmdDayOfWeek).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 日付設定
        /// </summary>
        /// <remarks>
        /// .NetフレームワークのDateTime型から通信フォーマットに合わせた値を設定します。
        /// </remarks>
        /// <param name="dateTime">日付</param>
        public void SetDateTime(DateTime dateTime)
        {
            // 年月日時分秒を設定
            this.Year = dateTime.Year;
            this.Month = dateTime.Month;
            this.Day = dateTime.Day;
            this.CmdDayOfWeek = this.convertDateTimeToCmdWeek(dateTime);
            this.Hour = dateTime.Hour;
            this.Minute = dateTime.Minute;
            this.Second = dateTime.Second;
        }

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            Int32 tmpdata2;
            Int32 tmpdata3;
            Int32 tmpdata4;
            Int32 tmpdata5;
            Int32 tmpdata6;
            Byte tmpdata7;
            resultList.Add(text_data.spoilInt(out tmpdata1, 2));
            resultList.Add(text_data.spoilInt(out tmpdata2, 2));
            resultList.Add(text_data.spoilInt(out tmpdata3, 2));
            resultList.Add(text_data.spoilInt(out tmpdata4, 2));
            resultList.Add(text_data.spoilInt(out tmpdata5, 2));
            resultList.Add(text_data.spoilInt(out tmpdata6, 2));
            resultList.Add(text_data.spoilByte(out tmpdata7, 1));
            Year = tmpdata1 * 100;
            Month = tmpdata2;
            Day = tmpdata3;
            Hour = tmpdata4;
            Minute = tmpdata5;
            Second = tmpdata6;
            CmdDayOfWeek = (Week)tmpdata7;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// 曜日値変換
        /// </summary>
        /// <remarks>
        /// .Netの曜日定義から、対スレーブ通信コマンド用曜日定義に変換します。
        /// </remarks>
        /// <param name="dateTime">.Netフレームワークの曜日値</param>
        /// <returns>対スレーブ通信コマンド用曜日定義</returns>
        protected Week convertDateTimeToCmdWeek(DateTime dateTime)
        {
            Week cmdWeek = Week.Monday;
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    cmdWeek = Week.Monday;
                    break;
                case DayOfWeek.Tuesday:
                    cmdWeek = Week.Tuesday;
                    break;
                case DayOfWeek.Wednesday:
                    cmdWeek = Week.Wednesday;
                    break;
                case DayOfWeek.Thursday:
                    cmdWeek = Week.Thursday;
                    break;
                case DayOfWeek.Friday:
                    cmdWeek = Week.Friday;
                    break;
                case DayOfWeek.Saturday:
                    cmdWeek = Week.Saturday;
                    break;
                case DayOfWeek.Sunday:
                    cmdWeek = Week.Sunday;
                    break;
                default:
                    break;
            }
            return cmdWeek;
        }

        #endregion
    }
    /// <summary>
    /// カレンダー設定コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// カレンダー設定コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1013 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1013()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1013;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 残量チェックコマンド
    /// </summary>
    /// <remarks>
    /// 残量チェックコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0014 : CarisXCommCommand
    {
        #region [定数定義]
        /// <summary>
        /// 容量チェック種別
        /// </summary>
        public enum RetCheckRemainCom
        {
            /// <summary>
            /// 情報のみ
            /// </summary>
            Info = 1,
            /// <summary>
            /// 全チェック
            /// </summary>
            AllCheck = 2
        }

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0014()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0014;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 容量チェック種別
        /// </summary>
        public RetCheckRemainCom KindRemainCheck { get; set; } = RetCheckRemainCom.Info;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.KindRemainCheck).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

#if DEBUG
        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            this.KindRemainCheck = (RetCheckRemainCom)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
#endif

        #endregion
    }

    /// <summary>
    /// 残量チェックコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 残量チェックコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1014 : CarisXCommCommand, IRackRemainAmountInfoSet
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1014()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1014;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 廃液満杯センサ
        /// </summary>
        public Boolean IsFullWasteTank { get; set; } = false;

        /// <summary>
        /// 廃液タンク有無センサ
        /// </summary>
        public Boolean ExistWasteTank { get; set; } = false;

        /// <summary>
        /// 取得時刻
        /// </summary>
        public DateTime TimeStamp { get; set; } = DateTime.MinValue;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(Convert.ToByte(this.IsFullWasteTank).ToString(), 1);
                builder.Append(Convert.ToByte(this.ExistWasteTank).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            this.TimeStamp = DateTime.Now;

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1 = 0;
            Byte tmpdata2 = 0;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            this.IsFullWasteTank = Convert.ToBoolean(tmpdata1);
            this.ExistWasteTank = Convert.ToBoolean(tmpdata2);

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// ユニットテストコマンド
    /// </summary>
    /// <remarks>
    /// ユニットテストコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0039 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0039()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0039;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ユニット番号
        /// </summary>
        public Int32 UnitNo { get; set; } = 0;

        /// <summary>
        /// コントロール
        /// </summary>
        public CommandControlParameter Control { get; set; } = CommandControlParameter.Start;

        /// <summary>
        /// 機能番号
        /// </summary>
        public Int32 FuncNo { get; set; } = 0;

        /// <summary>
        /// 引数1
        /// </summary>
        public Int32 Arg1 { get; set; } = 0;

        /// <summary>
        /// 引数2
        /// </summary>
        public Int32 Arg2 { get; set; } = 0;

        /// <summary>
        /// 引数3
        /// </summary>
        public Int32 Arg3 { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.UnitNo.ToString(), 2);
                builder.Append(((Int32)this.Control).ToString(), 1);
                builder.Append(this.FuncNo.ToString(), 2);
                builder.Append(this.Arg1.ToString(), 5);
                builder.Append(this.Arg2.ToString(), 5);
                builder.Append(this.Arg3.ToString(), 5);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

#if DEBUG
        #region [publicメソッド]

        /// <summary>
        /// ユニットテストコマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// ユニットテストコマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            Int32 tmpdata2;
            Int32 tmpdata3;
            Int32 tmpdata4;
            Int32 tmpdata5;
            Int32 tmpdata6;
            resultList.Add(text_data.spoilInt(out tmpdata1, 2));
            resultList.Add(text_data.spoilInt(out tmpdata2, 1));
            resultList.Add(text_data.spoilInt(out tmpdata3, 2));
            resultList.Add(text_data.spoilInt(out tmpdata4, 5));
            resultList.Add(text_data.spoilInt(out tmpdata5, 5));
            resultList.Add(text_data.spoilInt(out tmpdata6, 5));
            UnitNo = tmpdata1;
            Control = (CommandControlParameter)tmpdata2;
            FuncNo = tmpdata3;
            Arg1 = tmpdata4;
            Arg2 = tmpdata5;
            Arg3 = tmpdata6;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
#endif
    }
    /// <summary>
    /// ユニットテストコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ユニットテストコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1039 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1039()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1039;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ユニット番号
        /// </summary>
        public Int32 UnitNo { get; set; }

        /// <summary>
        /// コントロール
        /// </summary>
        public CommandControlParameter Control { get; set; }

        /// <summary>
        /// 機能番号
        /// </summary>
        public Int32 FuncNo { get; set; }

        /// <summary>
        /// 引数1
        /// </summary>
        public Int32 Arg1 { get; set; }

        /// <summary>
        /// 引数2
        /// </summary>
        public Int32 Arg2 { get; set; }

        /// <summary>
        /// 引数3
        /// </summary>
        public Int32 Arg3 { get; set; }

        /// <summary>
        /// レスポンス
        /// </summary>
        public String Response { get; set; }

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.UnitNo.ToString(), 2);
                builder.Append(((Int32)this.Control).ToString(), 1);
                builder.Append(this.FuncNo.ToString(), 2);
                builder.Append(this.Arg1.ToString(), 5);
                builder.Append(this.Arg2.ToString(), 5);
                builder.Append(this.Arg3.ToString(), 5);
                builder.Append(this.Response, 300);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// ユニットテストコマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// ユニットテストコマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            Int32 tmpdata2;
            Int32 tmpdata3;
            Int32 tmpdata4;
            Int32 tmpdata5;
            Int32 tmpdata6;
            String tmpdata7;
            resultList.Add(text_data.spoilInt(out tmpdata1, 2));
            resultList.Add(text_data.spoilInt(out tmpdata2, 1));
            resultList.Add(text_data.spoilInt(out tmpdata3, 2));
            resultList.Add(text_data.spoilInt(out tmpdata4, 5));
            resultList.Add(text_data.spoilInt(out tmpdata5, 5));
            resultList.Add(text_data.spoilInt(out tmpdata6, 5));
            resultList.Add(text_data.spoilString(out tmpdata7, 300));
            UnitNo = tmpdata1;
            Control = (CommandControlParameter)tmpdata2;
            FuncNo = tmpdata3;
            Arg1 = tmpdata4;
            Arg2 = tmpdata5;
            Arg3 = tmpdata6;
            Response = tmpdata7;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// センサステータス問い合わせコマンド
    /// </summary>
    /// <remarks>
    /// センサステータス問い合わせコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0040 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0040()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0040;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// センサステータス問い合わせコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// センサステータス問い合わせコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1040 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1040()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1040;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// サンプル容器種識別センサ1
        /// </summary>
        public Byte SampleContainerTypeIdentify1 { get; set; } = 0;

        /// <summary>
        /// サンプル容器種識別センサ2
        /// </summary>
        public Byte SampleContainerTypeIdentify2 { get; set; } = 0;

        /// <summary>
        /// サンプル容器種識別センサ3
        /// </summary>
        public Byte SampleContainerTypeIdentify3 { get; set; } = 0;

        /// <summary>
        /// ラック回収レーン前センサ
        /// </summary>
        public Byte RackCollectionLaneFront { get; set; } = 0;

        /// <summary>
        /// ラック回収レーン奥センサ
        /// </summary>
        public Byte RackCollectionLaneBack { get; set; } = 0;

        /// <summary>
        /// ラック設置レーン前センサ
        /// </summary>
        public Byte RackInstallationLaneFront { get; set; } = 0;

        /// <summary>
        /// ラック設置レーン奥センサ
        /// </summary>
        public Byte RackInstallationLaneBack { get; set; } = 0;

        /// <summary>
        /// ラック待機レーン前センサ
        /// </summary>
        public Byte RackStandbyLaneFront { get; set; } = 0;

        /// <summary>
        /// ラック待機レーン奥センサ
        /// </summary>
        public Byte RackStandbyLaneBack { get; set; } = 0;

        /// <summary>
        /// ラックフィーダ受取センサ
        /// </summary>
        public Byte RackFeederCatch { get; set; } = 0;

        /// <summary>
        /// 返却ラックフィーダ受取センサ
        /// </summary>
        public Byte ReturnRackFeederCatch { get; set; } = 0;

        /// <summary>
        /// ラック設置検知センサ(受光)
        /// </summary>
        public Byte RackSettingDetectiveLightReception { get; set; } = 0;

        /// <summary>
        /// 回収・再検ラック蓋センサ
        /// </summary>
        public Byte CollectionAndRetestRackCover { get; set; } = 0;

        /// <summary>
        /// 廃液満杯センサ
        /// </summary>
        public Byte DrainTankFull { get; set; } = 0;

        /// <summary>
        /// 廃液タンク有無センサ
        /// </summary>
        public Byte UsableDrainTank { get; set; } = 0;

        /// <summary>
        /// 装置１　ラック送り待機位置センサ
        /// </summary>
        public Byte RackSendStandbyPosition1 { get; set; } = 0;

        /// <summary>
        /// 装置１　ラック戻し待機位置センサ
        /// </summary>
        public Byte RackBackStandbyPosition1 { get; set; } = 0;

        /// <summary>
        /// 装置１　ラック引込フォーク受取りセンサ1
        /// </summary>
        public Byte RackPullInForkCatch11 { get; set; } = 0;

        /// <summary>
        /// 装置１　ラック引込フォーク受取りセンサ2
        /// </summary>
        public Byte RackPullInForkCatch21 { get; set; } = 0;

        /// <summary>
        /// 装置２　ラック送り待機位置センサ
        /// </summary>
        public Byte RackSendStandbyPosition2 { get; set; } = 0;

        /// <summary>
        /// 装置２　ラック戻し待機位置センサ
        /// </summary>
        public Byte RackBackStandbyPosition2 { get; set; } = 0;

        /// <summary>
        /// 装置２　ラック引込フォーク受取りセンサ1
        /// </summary>
        public Byte RackPullInForkCatch12 { get; set; } = 0;

        /// <summary>
        /// 装置２　ラック引込フォーク受取りセンサ2
        /// </summary>
        public Byte RackPullInForkCatch22 { get; set; } = 0;

        /// <summary>
        /// 装置３　ラック送り待機位置センサ
        /// </summary>
        public Byte RackSendStandbyPosition3 { get; set; } = 0;

        /// <summary>
        /// 装置３　ラック戻し待機位置センサ
        /// </summary>
        public Byte RackBackStandbyPosition3 { get; set; } = 0;

        /// <summary>
        /// 装置３　ラック引込フォーク受取りセンサ1
        /// </summary>
        public Byte RackPullInForkCatch13 { get; set; } = 0;

        /// <summary>
        /// 装置３　ラック引込フォーク受取りセンサ2
        /// </summary>
        public Byte RackPullInForkCatch23 { get; set; } = 0;

        /// <summary>
        /// 装置４　ラック送り待機位置センサ
        /// </summary>
        public Byte RackSendStandbyPosition4 { get; set; } = 0;

        /// <summary>
        /// 装置４　ラック戻し待機位置センサ
        /// </summary>
        public Byte RackBackStandbyPosition4 { get; set; } = 0;

        /// <summary>
        /// 装置４　ラック引込フォーク受取りセンサ1
        /// </summary>
        public Byte RackPullInForkCatch14 { get; set; } = 0;

        /// <summary>
        /// 装置４　ラック引込フォーク受取りセンサ2
        /// </summary>
        public Byte RackPullInForkCatch24 { get; set; } = 0;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.SampleContainerTypeIdentify1.ToString(), 1);
                builder.Append(this.SampleContainerTypeIdentify2.ToString(), 1);
                builder.Append(this.SampleContainerTypeIdentify3.ToString(), 1);
                builder.Append(this.RackCollectionLaneFront.ToString(), 1);
                builder.Append(this.RackCollectionLaneBack.ToString(), 1);
                builder.Append(this.RackInstallationLaneFront.ToString(), 1);
                builder.Append(this.RackInstallationLaneBack.ToString(), 1);
                builder.Append(this.RackStandbyLaneFront.ToString(), 1);
                builder.Append(this.RackStandbyLaneBack.ToString(), 1);
                builder.Append(this.RackFeederCatch.ToString(), 1);
                builder.Append(this.ReturnRackFeederCatch.ToString(), 1);
                builder.Append(this.RackSettingDetectiveLightReception.ToString(), 1);
                builder.Append(this.CollectionAndRetestRackCover.ToString(), 1);
                builder.Append(this.DrainTankFull.ToString(), 1);
                builder.Append(this.UsableDrainTank.ToString(), 1);
                builder.Append(this.RackSendStandbyPosition1.ToString(), 1);
                builder.Append(this.RackBackStandbyPosition1.ToString(), 1);
                builder.Append(this.RackPullInForkCatch11.ToString(), 1);
                builder.Append(this.RackPullInForkCatch21.ToString(), 1);
                builder.Append(this.RackSendStandbyPosition2.ToString(), 1);
                builder.Append(this.RackBackStandbyPosition2.ToString(), 1);
                builder.Append(this.RackPullInForkCatch12.ToString(), 1);
                builder.Append(this.RackPullInForkCatch22.ToString(), 1);
                builder.Append(this.RackSendStandbyPosition3.ToString(), 1);
                builder.Append(this.RackBackStandbyPosition3.ToString(), 1);
                builder.Append(this.RackPullInForkCatch13.ToString(), 1);
                builder.Append(this.RackPullInForkCatch23.ToString(), 1);
                builder.Append(this.RackSendStandbyPosition4.ToString(), 1);
                builder.Append(this.RackBackStandbyPosition4.ToString(), 1);
                builder.Append(this.RackPullInForkCatch14.ToString(), 1);
                builder.Append(this.RackPullInForkCatch24.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1 = 0;
            Byte tmpdata2 = 0;
            Byte tmpdata3 = 0;
            Byte tmpdata4 = 0;
            Byte tmpdata5 = 0;
            Byte tmpdata6 = 0;
            Byte tmpdata7 = 0;
            Byte tmpdata8 = 0;
            Byte tmpdata9 = 0;
            Byte tmpdata10 = 0;
            Byte tmpdata11 = 0;
            Byte tmpdata12 = 0;
            Byte tmpdata13 = 0;
            Byte tmpdata14 = 0;
            Byte tmpdata15 = 0;
            Byte tmpdata16 = 0;
            Byte tmpdata17 = 0;
            Byte tmpdata18 = 0;
            Byte tmpdata19 = 0;
            Byte tmpdata20 = 0;
            Byte tmpdata21 = 0;
            Byte tmpdata22 = 0;
            Byte tmpdata23 = 0;
            Byte tmpdata24 = 0;
            Byte tmpdata25 = 0;
            Byte tmpdata26 = 0;
            Byte tmpdata27 = 0;
            Byte tmpdata28 = 0;
            Byte tmpdata29 = 0;
            Byte tmpdata30 = 0;
            Byte tmpdata31 = 0;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            resultList.Add(text_data.spoilByte(out tmpdata3, 1));
            resultList.Add(text_data.spoilByte(out tmpdata4, 1));
            resultList.Add(text_data.spoilByte(out tmpdata5, 1));
            resultList.Add(text_data.spoilByte(out tmpdata6, 1));
            resultList.Add(text_data.spoilByte(out tmpdata7, 1));
            resultList.Add(text_data.spoilByte(out tmpdata8, 1));
            resultList.Add(text_data.spoilByte(out tmpdata9, 1));
            resultList.Add(text_data.spoilByte(out tmpdata10, 1));
            resultList.Add(text_data.spoilByte(out tmpdata11, 1));
            resultList.Add(text_data.spoilByte(out tmpdata12, 1));
            resultList.Add(text_data.spoilByte(out tmpdata13, 1));
            resultList.Add(text_data.spoilByte(out tmpdata14, 1));
            resultList.Add(text_data.spoilByte(out tmpdata15, 1));
            resultList.Add(text_data.spoilByte(out tmpdata16, 1));
            resultList.Add(text_data.spoilByte(out tmpdata17, 1));
            resultList.Add(text_data.spoilByte(out tmpdata18, 1));
            resultList.Add(text_data.spoilByte(out tmpdata19, 1));
            resultList.Add(text_data.spoilByte(out tmpdata20, 1));
            resultList.Add(text_data.spoilByte(out tmpdata21, 1));
            resultList.Add(text_data.spoilByte(out tmpdata22, 1));
            resultList.Add(text_data.spoilByte(out tmpdata23, 1));
            resultList.Add(text_data.spoilByte(out tmpdata24, 1));
            resultList.Add(text_data.spoilByte(out tmpdata25, 1));
            resultList.Add(text_data.spoilByte(out tmpdata26, 1));
            resultList.Add(text_data.spoilByte(out tmpdata27, 1));
            resultList.Add(text_data.spoilByte(out tmpdata28, 1));
            resultList.Add(text_data.spoilByte(out tmpdata29, 1));
            resultList.Add(text_data.spoilByte(out tmpdata30, 1));
            resultList.Add(text_data.spoilByte(out tmpdata31, 1));
            this.SampleContainerTypeIdentify1 = tmpdata1;
            this.SampleContainerTypeIdentify2 = tmpdata2;
            this.SampleContainerTypeIdentify3 = tmpdata3;
            this.RackCollectionLaneFront = tmpdata4;
            this.RackCollectionLaneBack = tmpdata5;
            this.RackInstallationLaneFront = tmpdata6;
            this.RackInstallationLaneBack = tmpdata7;
            this.RackStandbyLaneFront = tmpdata8;
            this.RackStandbyLaneBack = tmpdata9;
            this.RackFeederCatch = tmpdata10;
            this.ReturnRackFeederCatch = tmpdata11;
            this.RackSettingDetectiveLightReception = tmpdata12;
            this.CollectionAndRetestRackCover = tmpdata13;
            this.DrainTankFull = tmpdata14;
            this.UsableDrainTank = tmpdata15;
            this.RackSendStandbyPosition1 = tmpdata16;
            this.RackBackStandbyPosition1 = tmpdata17;
            this.RackPullInForkCatch11 = tmpdata18;
            this.RackPullInForkCatch21 = tmpdata19;
            this.RackSendStandbyPosition2 = tmpdata20;
            this.RackBackStandbyPosition2 = tmpdata21;
            this.RackPullInForkCatch12 = tmpdata22;
            this.RackPullInForkCatch22 = tmpdata23;
            this.RackSendStandbyPosition3 = tmpdata24;
            this.RackBackStandbyPosition3 = tmpdata25;
            this.RackPullInForkCatch13 = tmpdata26;
            this.RackPullInForkCatch23 = tmpdata27;
            this.RackSendStandbyPosition4 = tmpdata28;
            this.RackBackStandbyPosition4 = tmpdata29;
            this.RackPullInForkCatch14 = tmpdata30;
            this.RackPullInForkCatch24 = tmpdata31;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// センサー無効コマンド
    /// </summary>
    /// <remarks>
    /// センサー無効コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0041 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0041()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0041;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// サンプル容器種識別センサ1
        /// </summary>
        public Byte SampleContainerTypeIdentify1 { get; set; } = 0;

        /// <summary>
        /// サンプル容器種識別センサ2
        /// </summary>
        public Byte SampleContainerTypeIdentify2 { get; set; } = 0;

        /// <summary>
        /// サンプル容器種識別センサ3
        /// </summary>
        public Byte SampleContainerTypeIdentify3 { get; set; } = 0;

        /// <summary>
        /// ラック回収レーン前センサ
        /// </summary>
        public Byte RackCollectionLaneFront { get; set; } = 0;

        /// <summary>
        /// ラック回収レーン奥センサ
        /// </summary>
        public Byte RackCollectionLaneBack { get; set; } = 0;

        /// <summary>
        /// ラック設置レーン前センサ
        /// </summary>
        public Byte RackInstallationLaneFront { get; set; } = 0;

        /// <summary>
        /// ラック設置レーン奥センサ
        /// </summary>
        public Byte RackInstallationLaneBack { get; set; } = 0;

        /// <summary>
        /// ラック待機レーン前センサ
        /// </summary>
        public Byte RackStandbyLaneFront { get; set; } = 0;

        /// <summary>
        /// ラック待機レーン奥センサ
        /// </summary>
        public Byte RackStandbyLaneBack { get; set; } = 0;

        /// <summary>
        /// ラックフィーダ受取センサ
        /// </summary>
        public Byte RackFeederCatch { get; set; } = 0;

        /// <summary>
        /// 返却ラックフィーダ受取センサ
        /// </summary>
        public Byte ReturnRackFeederCatch { get; set; } = 0;

        /// <summary>
        /// ラック設置検知センサ(受光)
        /// </summary>
        public Byte RackSettingDetectiveLightReception { get; set; } = 0;

        /// <summary>
        /// 回収・再検ラック蓋センサ
        /// </summary>
        public Byte CollectionAndRetestRackCover { get; set; } = 0;

        /// <summary>
        /// 廃液満杯センサ
        /// </summary>
        public Byte DrainTankFull { get; set; } = 0;

        /// <summary>
        /// 廃液タンク有無センサ
        /// </summary>
        public Byte UsableDrainTank { get; set; } = 0;

        /// <summary>
        /// 装置１　ラック送り待機位置センサ
        /// </summary>
        public Byte RackSendStandbyPosition1 { get; set; } = 0;

        /// <summary>
        /// 装置１　ラック戻し待機位置センサ
        /// </summary>
        public Byte RackBackStandbyPosition1 { get; set; } = 0;

        /// <summary>
        /// 装置１　ラック引込フォーク受取りセンサ1
        /// </summary>
        public Byte RackPullInForkCatch11 { get; set; } = 0;

        /// <summary>
        /// 装置１　ラック引込フォーク受取りセンサ2
        /// </summary>
        public Byte RackPullInForkCatch21 { get; set; } = 0;

        /// <summary>
        /// 装置２　ラック送り待機位置センサ
        /// </summary>
        public Byte RackSendStandbyPosition2 { get; set; } = 0;

        /// <summary>
        /// 装置２　ラック戻し待機位置センサ
        /// </summary>
        public Byte RackBackStandbyPosition2 { get; set; } = 0;

        /// <summary>
        /// 装置２　ラック引込フォーク受取りセンサ1
        /// </summary>
        public Byte RackPullInForkCatch12 { get; set; } = 0;

        /// <summary>
        /// 装置２　ラック引込フォーク受取りセンサ2
        /// </summary>
        public Byte RackPullInForkCatch22 { get; set; } = 0;

        /// <summary>
        /// 装置３　ラック送り待機位置センサ
        /// </summary>
        public Byte RackSendStandbyPosition3 { get; set; } = 0;

        /// <summary>
        /// 装置３　ラック戻し待機位置センサ
        /// </summary>
        public Byte RackBackStandbyPosition3 { get; set; } = 0;

        /// <summary>
        /// 装置３　ラック引込フォーク受取りセンサ1
        /// </summary>
        public Byte RackPullInForkCatch13 { get; set; } = 0;

        /// <summary>
        /// 装置３　ラック引込フォーク受取りセンサ2
        /// </summary>
        public Byte RackPullInForkCatch23 { get; set; } = 0;

        /// <summary>
        /// 装置４　ラック送り待機位置センサ
        /// </summary>
        public Byte RackSendStandbyPosition4 { get; set; } = 0;

        /// <summary>
        /// 装置４　ラック戻し待機位置センサ
        /// </summary>
        public Byte RackBackStandbyPosition4 { get; set; } = 0;

        /// <summary>
        /// 装置４　ラック引込フォーク受取りセンサ1
        /// </summary>
        public Byte RackPullInForkCatch14 { get; set; } = 0;

        /// <summary>
        /// 装置４　ラック引込フォーク受取りセンサ2
        /// </summary>
        public Byte RackPullInForkCatch24 { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.SampleContainerTypeIdentify1.ToString(), 1);
                builder.Append(this.SampleContainerTypeIdentify2.ToString(), 1);
                builder.Append(this.SampleContainerTypeIdentify3.ToString(), 1);
                builder.Append(this.RackCollectionLaneFront.ToString(), 1);
                builder.Append(this.RackCollectionLaneBack.ToString(), 1);
                builder.Append(this.RackInstallationLaneFront.ToString(), 1);
                builder.Append(this.RackInstallationLaneBack.ToString(), 1);
                builder.Append(this.RackStandbyLaneFront.ToString(), 1);
                builder.Append(this.RackStandbyLaneBack.ToString(), 1);
                builder.Append(this.RackFeederCatch.ToString(), 1);
                builder.Append(this.ReturnRackFeederCatch.ToString(), 1);
                builder.Append(this.RackSettingDetectiveLightReception.ToString(), 1);
                builder.Append(this.CollectionAndRetestRackCover.ToString(), 1);
                builder.Append(this.DrainTankFull.ToString(), 1);
                builder.Append(this.UsableDrainTank.ToString(), 1);
                builder.Append(this.RackSendStandbyPosition1.ToString(), 1);
                builder.Append(this.RackBackStandbyPosition1.ToString(), 1);
                builder.Append(this.RackPullInForkCatch11.ToString(), 1);
                builder.Append(this.RackPullInForkCatch21.ToString(), 1);
                builder.Append(this.RackSendStandbyPosition2.ToString(), 1);
                builder.Append(this.RackBackStandbyPosition2.ToString(), 1);
                builder.Append(this.RackPullInForkCatch12.ToString(), 1);
                builder.Append(this.RackPullInForkCatch22.ToString(), 1);
                builder.Append(this.RackSendStandbyPosition3.ToString(), 1);
                builder.Append(this.RackBackStandbyPosition3.ToString(), 1);
                builder.Append(this.RackPullInForkCatch13.ToString(), 1);
                builder.Append(this.RackPullInForkCatch23.ToString(), 1);
                builder.Append(this.RackSendStandbyPosition4.ToString(), 1);
                builder.Append(this.RackBackStandbyPosition4.ToString(), 1);
                builder.Append(this.RackPullInForkCatch14.ToString(), 1);
                builder.Append(this.RackPullInForkCatch24.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

#if DEBUG
        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1 = 0;
            Byte tmpdata2 = 0;
            Byte tmpdata3 = 0;
            Byte tmpdata4 = 0;
            Byte tmpdata5 = 0;
            Byte tmpdata6 = 0;
            Byte tmpdata7 = 0;
            Byte tmpdata8 = 0;
            Byte tmpdata9 = 0;
            Byte tmpdata10 = 0;
            Byte tmpdata11 = 0;
            Byte tmpdata12 = 0;
            Byte tmpdata13 = 0;
            Byte tmpdata14 = 0;
            Byte tmpdata15 = 0;
            Byte tmpdata16 = 0;
            Byte tmpdata17 = 0;
            Byte tmpdata18 = 0;
            Byte tmpdata19 = 0;
            Byte tmpdata20 = 0;
            Byte tmpdata21 = 0;
            Byte tmpdata22 = 0;
            Byte tmpdata23 = 0;
            Byte tmpdata24 = 0;
            Byte tmpdata25 = 0;
            Byte tmpdata26 = 0;
            Byte tmpdata27 = 0;
            Byte tmpdata28 = 0;
            Byte tmpdata29 = 0;
            Byte tmpdata30 = 0;
            Byte tmpdata31 = 0;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            resultList.Add(text_data.spoilByte(out tmpdata3, 1));
            resultList.Add(text_data.spoilByte(out tmpdata4, 1));
            resultList.Add(text_data.spoilByte(out tmpdata5, 1));
            resultList.Add(text_data.spoilByte(out tmpdata6, 1));
            resultList.Add(text_data.spoilByte(out tmpdata7, 1));
            resultList.Add(text_data.spoilByte(out tmpdata8, 1));
            resultList.Add(text_data.spoilByte(out tmpdata9, 1));
            resultList.Add(text_data.spoilByte(out tmpdata10, 1));
            resultList.Add(text_data.spoilByte(out tmpdata11, 1));
            resultList.Add(text_data.spoilByte(out tmpdata12, 1));
            resultList.Add(text_data.spoilByte(out tmpdata13, 1));
            resultList.Add(text_data.spoilByte(out tmpdata14, 1));
            resultList.Add(text_data.spoilByte(out tmpdata15, 1));
            resultList.Add(text_data.spoilByte(out tmpdata16, 1));
            resultList.Add(text_data.spoilByte(out tmpdata17, 1));
            resultList.Add(text_data.spoilByte(out tmpdata18, 1));
            resultList.Add(text_data.spoilByte(out tmpdata19, 1));
            resultList.Add(text_data.spoilByte(out tmpdata20, 1));
            resultList.Add(text_data.spoilByte(out tmpdata21, 1));
            resultList.Add(text_data.spoilByte(out tmpdata22, 1));
            resultList.Add(text_data.spoilByte(out tmpdata23, 1));
            resultList.Add(text_data.spoilByte(out tmpdata24, 1));
            resultList.Add(text_data.spoilByte(out tmpdata25, 1));
            resultList.Add(text_data.spoilByte(out tmpdata26, 1));
            resultList.Add(text_data.spoilByte(out tmpdata27, 1));
            resultList.Add(text_data.spoilByte(out tmpdata28, 1));
            resultList.Add(text_data.spoilByte(out tmpdata29, 1));
            resultList.Add(text_data.spoilByte(out tmpdata30, 1));
            resultList.Add(text_data.spoilByte(out tmpdata31, 1));
            this.SampleContainerTypeIdentify1 = tmpdata1;
            this.SampleContainerTypeIdentify2 = tmpdata2;
            this.SampleContainerTypeIdentify3 = tmpdata3;
            this.RackCollectionLaneFront = tmpdata4;
            this.RackCollectionLaneBack = tmpdata5;
            this.RackInstallationLaneFront = tmpdata6;
            this.RackInstallationLaneBack = tmpdata7;
            this.RackStandbyLaneFront = tmpdata8;
            this.RackStandbyLaneBack = tmpdata9;
            this.RackFeederCatch = tmpdata10;
            this.ReturnRackFeederCatch = tmpdata11;
            this.RackSettingDetectiveLightReception = tmpdata12;
            this.CollectionAndRetestRackCover = tmpdata13;
            this.DrainTankFull = tmpdata14;
            this.UsableDrainTank = tmpdata15;
            this.RackSendStandbyPosition1 = tmpdata16;
            this.RackBackStandbyPosition1 = tmpdata17;
            this.RackPullInForkCatch11 = tmpdata18;
            this.RackPullInForkCatch21 = tmpdata19;
            this.RackSendStandbyPosition2 = tmpdata20;
            this.RackBackStandbyPosition2 = tmpdata21;
            this.RackPullInForkCatch12 = tmpdata22;
            this.RackPullInForkCatch22 = tmpdata23;
            this.RackSendStandbyPosition3 = tmpdata24;
            this.RackBackStandbyPosition3 = tmpdata25;
            this.RackPullInForkCatch13 = tmpdata26;
            this.RackPullInForkCatch23 = tmpdata27;
            this.RackSendStandbyPosition4 = tmpdata28;
            this.RackBackStandbyPosition4 = tmpdata29;
            this.RackPullInForkCatch14 = tmpdata30;
            this.RackPullInForkCatch24 = tmpdata31;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
#endif
    }
    /// <summary>
    /// センサー無効コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// センサー無効コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1041 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1041()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1041;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// スタートアップ開始コマンド
    /// </summary>
    /// <remarks>
    /// スタートアップ開始コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0042 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0042()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0042;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// スタートアップ開始コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// スタートアップ開始コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1042 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1042()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1042;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// スタートアップ終了コマンド
    /// </summary>
    /// <remarks>
    /// スタートアップ終了コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0043 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0043()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0043;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// スタートアップ終了コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// スタートアップ終了コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1043 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1043()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1043;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// ＥＮＤ処理コマンド
    /// </summary>
    /// <remarks>
    /// ＥＮＤ処理コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0044 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0044()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0044;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// ＥＮＤ処理コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ＥＮＤ処理コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1044 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1044()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1044;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// ラックユニットパラメータコマンド
    /// </summary>
    /// <remarks>
    /// ラックユニットパラメータコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0047 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0047()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0047;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラック設置ローダー時間
        /// </summary>
        public Int32 InstallationLoaderTime { get; set; } = 0;

        /// <summary>
        /// 再検ラック待機ローダー時間
        /// </summary>
        public Int32 RetestWaitLoaderTime { get; set; } = 0;

        /// <summary>
        /// ラック回収ローダー時間
        /// </summary>
        public Int32 CollectionLoaderTime { get; set; } = 0;

        /// <summary>
        /// ラック送りローダー時間
        /// </summary>
        public Int32 SendLoaderTime { get; set; } = 0;

        /// <summary>
        /// ラック戻りローダー時間
        /// </summary>
        public Int32 BackLoaderTime { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.InstallationLoaderTime.ToString(), 3);
                builder.Append(this.RetestWaitLoaderTime.ToString(), 3);
                builder.Append(this.CollectionLoaderTime.ToString(), 3);
                builder.Append(this.SendLoaderTime.ToString(), 3);
                builder.Append(this.BackLoaderTime.ToString(), 3);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

#if DEBUG
        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            Int32 tmpdata2 = 0;
            Int32 tmpdata3 = 0;
            Int32 tmpdata4 = 0;
            Int32 tmpdata5 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 3));
            resultList.Add(text_data.spoilInt(out tmpdata2, 3));
            resultList.Add(text_data.spoilInt(out tmpdata3, 3));
            resultList.Add(text_data.spoilInt(out tmpdata4, 3));
            resultList.Add(text_data.spoilInt(out tmpdata5, 3));
            this.InstallationLoaderTime = tmpdata1;
            this.RetestWaitLoaderTime = tmpdata2;
            this.CollectionLoaderTime = tmpdata3;
            this.SendLoaderTime = tmpdata4;
            this.BackLoaderTime = tmpdata5;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
#endif

    }
    /// <summary>
    /// ラックユニットパラメータコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラックユニットパラメータコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1047 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1047()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1047;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// ラック接続確認コマンド
    /// </summary>
    /// <remarks>
    /// ラック接続確認コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0067 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0067()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0067;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// ラック接続確認コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラック接続確認コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1067 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1067()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1067;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// サンプル停止要因問合せコマンド
    /// </summary>
    /// <remarks>
    /// サンプル停止要因問合せコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0068 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0068()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0068;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// サンプル停止要因問合せコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// サンプル停止要因問合せコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1068 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1068()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1068;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 要因
        /// </summary>
        public Int32 Factor { get; set; } = 0;

#if SIMULATOR
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.Factor.ToString(), 8);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 8));
            this.Factor = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// ラック排出コマンド
    /// </summary>
    /// <remarks>
    /// ラック排出コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0069 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0069()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0069;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// ラック排出コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラック排出コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1069 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1069()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1069;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// モーターパラメータ保存コマンド
    /// </summary>
    /// <remarks>
    /// モーターパラメータ保存コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0071 : RackTransferCommCommand_0002
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0071()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0071;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// モーターパラメータ保存コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// モーターパラメータ保存コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1071 : RackTransferCommCommand_1002
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1071()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1071;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// モーター調整コマンド
    /// </summary>
    /// <remarks>
    /// モーター調整コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0073 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0073()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0073;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// モーター番号
        /// </summary>
        public Int32 MotorNo { get; set; } = 0;

        /// <summary>
        /// 相対移動距離
        /// </summary>
        public Double Distance { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.MotorNo.ToString(), 3);
                builder.Append(this.Distance.ToString(), 6);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

#if DEBUG
        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            Double tmpdata2 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 3));
            resultList.Add(text_data.spoilDouble(out tmpdata2, 6));
            this.MotorNo = tmpdata1;
            this.Distance = tmpdata2;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
#endif
    }
    /// <summary>
    /// モーター調整コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// モーター調整コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1073 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1073()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1073;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// カレンダーコマンド
    /// </summary>
    /// <remarks>
    /// カレンダーコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0077 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0077()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0077;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// パラメータ
        /// </summary>
        public Int32 Time { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.Time.ToString(), 8);
                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

#if DEBUG
        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 8));
            this.Time = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
#endif
    }
    /// <summary>
    /// ユニット無効コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ユニット無効コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1077 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1077()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1077;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// ユニット無効コマンド
    /// </summary>
    /// <remarks>
    /// ユニット無効コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0078 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0078()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0078;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラック架設フレーム部
        /// </summary>
        public Byte UsableRackFrame { get; set; } = 0;

        /// <summary>
        /// ラック搬送部　分析１
        /// </summary>
        public Byte UsableRackTransfer1 { get; set; } = 0;

        /// <summary>
        /// ラック引込部　分析１
        /// </summary>
        public Byte UsableRackRetraction1 { get; set; } = 0;

        /// <summary>
        /// ラック搬送部　分析２
        /// </summary>
        public Byte UsableRackTransfer2 { get; set; } = 0;

        /// <summary>
        /// ラック引込部　分析２
        /// </summary>
        public Byte UsableRackRetraction2 { get; set; } = 0;

        /// <summary>
        /// ラック搬送部　分析３
        /// </summary>
        public Byte UsableRackTransfer3 { get; set; } = 0;

        /// <summary>
        /// ラック引込部　分析３
        /// </summary>
        public Byte UsableRackRetraction3 { get; set; } = 0;

        /// <summary>
        /// ラック搬送部　分析４
        /// </summary>
        public Byte UsableRackTransfer4 { get; set; } = 0;

        /// <summary>
        /// ラック引込部　分析４
        /// </summary>
        public Byte UsableRackRetraction4 { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.UsableRackFrame.ToString(), 1);
                builder.Append(this.UsableRackTransfer1.ToString(), 1);
                builder.Append(this.UsableRackRetraction1.ToString(), 1);
                builder.Append(this.UsableRackTransfer2.ToString(), 1);
                builder.Append(this.UsableRackRetraction2.ToString(), 1);
                builder.Append(this.UsableRackTransfer3.ToString(), 1);
                builder.Append(this.UsableRackRetraction3.ToString(), 1);
                builder.Append(this.UsableRackTransfer4.ToString(), 1);
                builder.Append(this.UsableRackRetraction4.ToString(), 1);
                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

#if DEBUG
        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1 = 0;
            Byte tmpdata2 = 0;
            Byte tmpdata3 = 0;
            Byte tmpdata4 = 0;
            Byte tmpdata5 = 0;
            Byte tmpdata6 = 0;
            Byte tmpdata7 = 0;
            Byte tmpdata8 = 0;
            Byte tmpdata9 = 0;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            resultList.Add(text_data.spoilByte(out tmpdata3, 1));
            resultList.Add(text_data.spoilByte(out tmpdata4, 1));
            resultList.Add(text_data.spoilByte(out tmpdata5, 1));
            resultList.Add(text_data.spoilByte(out tmpdata6, 1));
            resultList.Add(text_data.spoilByte(out tmpdata7, 1));
            resultList.Add(text_data.spoilByte(out tmpdata8, 1));
            resultList.Add(text_data.spoilByte(out tmpdata9, 1));
            this.UsableRackFrame = tmpdata1;
            this.UsableRackTransfer1 = tmpdata2;
            this.UsableRackRetraction1 = tmpdata3;
            this.UsableRackTransfer2 = tmpdata4;
            this.UsableRackRetraction2 = tmpdata5;
            this.UsableRackTransfer3 = tmpdata6;
            this.UsableRackRetraction3 = tmpdata7;
            this.UsableRackTransfer4 = tmpdata8;
            this.UsableRackRetraction4 = tmpdata9;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
#endif
    }
    /// <summary>
    /// ユニット無効コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ユニット無効コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1078 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1078()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1078;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 調整位置停止コマンド
    /// </summary>
    /// <remarks>
    /// 調整位置停止コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0080 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0080()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0080;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 停止位置番号
        /// </summary>
        public Int32 Pos { get; set; } = 0;

        /// <summary>
        /// 引数1
        /// </summary>
        public Int32 Arg1 { get; set; } = 0;

        /// <summary>
        /// 引数2
        /// </summary>
        public Int32 Arg2 { get; set; } = 0;

        /// <summary>
        /// 引数3
        /// </summary>
        public Int32 Arg3 { get; set; } = 0;

        /// <summary>
        /// 引数4
        /// </summary>
        public Int32 Arg4 { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.Pos.ToString(), 3);
                builder.Append(this.Arg1.ToString(), 6);
                builder.Append(this.Arg2.ToString(), 6);
                builder.Append(this.Arg3.ToString(), 6);
                builder.Append(this.Arg4.ToString(), 6);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

#if DEBUG
        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            Int32 tmpdata2 = 0;
            Int32 tmpdata3 = 0;
            Int32 tmpdata4 = 0;
            Int32 tmpdata5 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 3));
            resultList.Add(text_data.spoilInt(out tmpdata2, 6));
            resultList.Add(text_data.spoilInt(out tmpdata3, 6));
            resultList.Add(text_data.spoilInt(out tmpdata4, 6));
            resultList.Add(text_data.spoilInt(out tmpdata5, 6));
            Pos = tmpdata1;
            Arg1 = tmpdata2;
            Arg2 = tmpdata3;
            Arg3 = tmpdata4;
            Arg4 = tmpdata5;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
#endif
    }
    /// <summary>
    /// 調整位置停止コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 調整位置停止コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1080 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1080()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1080;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 調整位置開始コマンド
    /// </summary>
    /// <remarks>
    /// 調整位置開始コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0081 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0081()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0081;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 停止位置番号
        /// </summary>
        public Int32 Pos { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.Pos.ToString(), 3);
                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

#if DEBUG
        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 3));
            Pos = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
#endif
    }
    /// <summary>
    /// 調整位置開始コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 調整位置開始コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1081 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        ///　コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1081()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1081;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// 検体バーコード設定コマンド
    /// </summary>
    /// <remarks>
    /// 検体バーコード設定コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0082 : CarisXCommCommand
    {
        #region [定数定義]
        /// <summary>
        /// バーコード種
        /// </summary>
        public enum Kind
        {
            /// <summary>
            /// NW-7,ITF,CODE39,CODE128
            /// </summary>
            NW7ITFCODE39CODE128 = 1,
            /// <summary>
            /// ITF,2of5,CODE39,CODE128
            /// </summary>
            ITF2of5CODE39CODE128 = 2,
            /// <summary>
            /// NW-7,ITF,2of5,CODE128
            /// </summary>
            NW7ITF2of5CODE128 = 3,
            /// <summary>
            /// NW-7,ITF,CODE39,2of5
            /// </summary>
            NW7ITFCODE392of5 = 4
        }
        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0082()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0082;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// バーコード種別
        /// </summary>
        public Kind Kind1 { get; set; } = Kind.ITF2of5CODE39CODE128;

        /// <summary>
        /// チェックデジット検査有無
        /// </summary>
        public Byte CDCheck { get; set; } = 0;

        /// <summary>
        /// チェックデジット転送
        /// </summary>
        public Byte CDTrans { get; set; } = 0;

        /// <summary>
        /// ST/SPコード転送
        /// </summary>
        public Byte STSPTrans { get; set; } = 0;

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)Kind1).ToString(), 1);
                builder.Append(this.CDCheck.ToString(), 1);
                builder.Append(this.CDTrans.ToString(), 1);
                builder.Append(this.STSPTrans.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

#if DEBUG
        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            Byte tmpdata2 = 0;
            Byte tmpdata3 = 0;
            Byte tmpdata4 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            resultList.Add(text_data.spoilByte(out tmpdata3, 1));
            resultList.Add(text_data.spoilByte(out tmpdata4, 1));
            Kind1 = (Kind)tmpdata1;
            CDCheck = tmpdata2;
            CDTrans = tmpdata3;
            STSPTrans = tmpdata4;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
#endif
    }
    /// <summary>
    /// 検体バーコード設定コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 検体バーコード設定コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1082 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1082()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1082;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 分析強制終了コマンド
    /// </summary>
    /// <remarks>
    /// 分析強制終了コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0086 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0086()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0086;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// 分析強制終了コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 分析強制終了コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1086 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1086()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1086;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// ラック設置有無確認コマンド
    /// </summary>
    /// <remarks>
    /// ラック設置有無確認コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0088 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0088()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0088;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// ラック設置有無確認コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラック設置有無確認コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1088 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1088()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1088;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// ラック状況上書きコマンド
    /// </summary>
    /// <remarks>
    /// ラック状況上書きコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0089 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0089()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0089;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// ラック状況上書きコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラック状況上書きコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1089 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1089()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1089;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 再検コマンド
    /// </summary>
    /// <remarks>
    /// 再検コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0090 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0090()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0090;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 再検ポジション数
        /// </summary>
        public Int32 ReTestPositionNo { get; set; }

        /// <summary>
        /// ラックID
        /// </summary>
        public String[] RackId { get; set; }

        /// <summary>
        /// ラックポジション
        /// </summary>
        public Int32[] RackPosition { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.ReTestPositionNo.ToString(), 3);

                for (Int32 i = 0; i < this.ReTestPositionNo; i++)
                {
                    builder.Append(this.RackId[i], 4);
                    builder.Append(this.RackPosition[i].ToString(), 1);
                }

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

#if DEBUG
        #region [publicメソッド]

        /// <summary>
        /// 再検コマンド解析
        /// </summary>
        /// <remarks>
        /// 再検コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpReTestPositionNo = 0;
            String[] tmpRackId = new String[] { String.Empty };
            Int32[] tmpRackPosition = new Int32[] { 0 };

            resultList.Add(text_data.spoilInt(out tmpReTestPositionNo, 3));

            tmpRackId = new String[tmpReTestPositionNo];
            tmpRackPosition = new Int32[tmpReTestPositionNo];

            for (int i = 0; i < tmpReTestPositionNo; i++)
            {
                resultList.Add(text_data.spoilString(out tmpRackId[i], 4));
                resultList.Add(text_data.spoilInt(out tmpRackPosition[i], 1));
            }

            ReTestPositionNo = tmpReTestPositionNo;
            RackId = tmpRackId;
            RackPosition = tmpRackPosition;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
#endif
    }
    /// <summary>
    /// 再検コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 再検コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1090 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1090()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1090;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 分取完了通知コマンド
    /// </summary>
    /// <remarks>
    /// 分取完了通知コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0096 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0096()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0096;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラックＩＤ
        /// </summary>
        public String RackId { get; set; }

        /// <summary>
        /// 装置番号
        /// </summary>
        public Int32 DeviceNo { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackId, 4);
                builder.Append(this.DeviceNo.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

#if DEBUG
        #region [publicメソッド]

        /// <summary>
        /// 分取完了通知コマンド解析
        /// </summary>
        /// <remarks>
        /// 分取完了通知コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            String tmpRackId = String.Empty;
            Int32 tmpDeviceNo = 0;
            Int32 tmpForkposition = 0;
            resultList.Add(text_data.spoilString(out tmpRackId, 4));
            resultList.Add(text_data.spoilInt(out tmpDeviceNo, 1));
            resultList.Add(text_data.spoilInt(out tmpForkposition, 1));
            RackId = tmpRackId;
            DeviceNo = tmpDeviceNo;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
#endif

    }
    /// <summary>
    /// 分取完了通知コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 分取完了通知コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1096 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1096()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1096;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 測定完了通知コマンド
    /// </summary>
    /// <remarks>
    /// 測定完了通知コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0097 : CarisXCommCommand
    {
        #region [定数定義]
        /// <summary>
        /// 再検査有無
        /// </summary>
        public enum ReTestKind
        {
            No = 0,
            Yes = 1
        }
        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0097()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0097;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラックＩＤ
        /// </summary>
        public String RackId { get; set; }

        /// <summary>
        /// 再検査有無
        /// </summary>
        public ReTestKind ReTestWith { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackId, 4);
                builder.Append(((int)this.ReTestWith).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

#if DEBUG
        #region [publicメソッド]

        /// <summary>
        /// 測定完了通知コマンド解析
        /// </summary>
        /// <remarks>
        /// 測定完了通知コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            String tmpRackId = String.Empty;
            Int32 tmpReTestWith = 0;
            resultList.Add(text_data.spoilString(out tmpRackId, 4));
            resultList.Add(text_data.spoilInt(out tmpReTestWith, 1));
            RackId = tmpRackId;
            ReTestWith = (ReTestKind)tmpReTestWith;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
#endif

    }
    /// <summary>
    /// 測定完了通知コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 測定完了通知コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1097 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1097()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1097;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// ステータス問い合わせコマンド
    /// </summary>
    /// <remarks>
    /// ステータス問い合わせコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0098 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0098()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0098;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// ステータス問い合わせコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ステータス問い合わせコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1098 : CarisXCommCommand
    {
        #region [定数定義]

        /// <summary>
        /// ステータス
        /// </summary>
        public enum StatusKind
        {
            /// <summary>
            /// 準備中
            /// </summary>
            StartUp = 1,
            /// <summary>
            /// 待機中
            /// </summary>
            Waiting = 2,
            /// <summary>
            /// 分析中
            /// </summary>
            Analysising = 3,
            /// <summary>
            /// スタンバイ中
            /// </summary>
            StandBy = 4,
            /// <summary>
            /// サンプル停止中
            /// </summary>
            StippingSmp = 5,
            /// <summary>
            /// プライム中
            /// </summary>
            Priming = 6,
            /// <summary>
            /// リンス中
            /// </summary>
            Rinsing = 7,
        }

        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1098()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1098;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ステータス
        /// </summary>
        public StatusKind Status { get; set; } = StatusKind.StartUp;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((Int32)this.Status).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            this.Status = (StatusKind)tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
        #endregion
    }

    #endregion

    #region Rack→User

    /// <summary>
    /// ラックレディーコマンド
    /// </summary>
    /// <remarks>
    /// ラックレディーコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0101 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0101()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0101;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// バージョン文字列
        /// </summary>
        public String Version { get; set; }

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.AppendRight(this.Version, 11);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            String tmpParamStr;
            resultList.Add(text_data.spoilString(out tmpParamStr, 11));
            Version = tmpParamStr;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// エラー通知コマンド
    /// </summary>
    /// <remarks>
    /// エラー通知コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0104 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0104()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0104;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// エラーコード
        /// </summary>
        public Int32 ErrorCode { get; set; } = 0;

        /// <summary>
        /// 引数
        /// </summary>
        public Int32 Arg { get; set; } = 0;

        /// <summary>
        /// 文字列
        /// </summary>
        public String Str { get; set; } = "";

#if SIMULATOR
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.ErrorCode.ToString(), 4);
                builder.Append(this.Arg.ToString(), 4);
                builder.Append(this.Str, 80);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// エラーコマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// エラーコマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            Int32 tmpdata2 = 0;
            String tmpdata3 = "";
            resultList.Add(text_data.spoilInt(out tmpdata1, 4));
            resultList.Add(text_data.spoilInt(out tmpdata2, 4));
            resultList.Add(text_data.spoilString(out tmpdata3, 80));
            ErrorCode = tmpdata1;
            Arg = tmpdata2;
            Str = tmpdata3;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }
    /// <summary>
    /// エラー通知コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// エラー通知コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1104 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1104()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1104;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// ラックイベント通知コマンド
    /// </summary>
    /// <remarks>
    /// ラックイベント通知コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0105 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0105()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0105;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// イベント
        /// </summary>
        public RackTransferSubEvent SubEvent { get; set; } = RackTransferSubEvent.Wait;

        /// <summary>
        /// 引数1
        /// </summary>
        public Int32 SubEventArg1 { get; set; } = 0;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((int)this.SubEvent).ToString(), 2);
                builder.Append(this.SubEventArg1.ToString(), 10);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// サブイベントコマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// サブイベントコマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0; ;
            Int32 tmpdata2 = 0; ;
            resultList.Add(text_data.spoilInt(out tmpdata1, 2));
            resultList.Add(text_data.spoilInt(out tmpdata2, 10));
            SubEvent = (RackTransferSubEvent)tmpdata1;
            SubEventArg1 = tmpdata2;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }
    /// <summary>
    /// ラックイベント通知コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラックイベント通知コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1105 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1105()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1105;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// ラック分析ステータスコマンド
    /// </summary>
    /// <remarks>
    /// ラック分析ステータスコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0106 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0106()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0106;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラック数
        /// </summary>
        public Int32 RackCount
        {
            get
            {
                return this.RackID.Count;
            }
        }

        /// <summary>
        /// ラックID
        /// </summary>
        public List<String> RackID { get; set; } = new List<String>();

#if SIMULATOR
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackID.Count.ToString(), 2);
                foreach (var rackId in this.RackID)
                {
                    builder.Append(rackId, 4);
                }

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// サブイベントコマンド（レスポンス）解析
        /// </summary>
        /// <remarks>
        /// サブイベントコマンド（レスポンス）の解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1;
            String tmpdata2;
            resultList.Add(text_data.spoilInt(out tmpdata1, 2));

            for (int i = 0; i < tmpdata1; i++)
            {
                resultList.Add(text_data.spoilString(out tmpdata2, 4));
                this.RackID.Add(tmpdata2);
            }

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }
    /// <summary>
    /// ラック分析ステータスコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラック分析ステータスコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1106 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1106()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1106;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// 残量コマンド
    /// </summary>
    /// <remarks>
    /// 残量コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0108 : CarisXCommCommand, IRackRemainAmountInfoSet
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0108()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0108;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 廃液満杯センサ
        /// </summary>
        public Boolean IsFullWasteTank { get; set; } = false;

        /// <summary>
        /// 廃液タンク有無センサ
        /// </summary>
        public Boolean ExistWasteTank { get; set; } = false;

        /// <summary>
        /// 取得時刻
        /// </summary>
        public DateTime TimeStamp { get; set; } = DateTime.MinValue;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(Convert.ToByte(this.IsFullWasteTank).ToString(), 1);
                builder.Append(Convert.ToByte(this.ExistWasteTank).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            this.TimeStamp = DateTime.Now;

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1 = 0;
            Byte tmpdata2 = 0;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            this.IsFullWasteTank = Convert.ToBoolean(tmpdata1);
            this.ExistWasteTank = Convert.ToBoolean(tmpdata2);

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }
    /// <summary>
    /// 残量コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// 残量コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1108 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1108()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1108;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }

    /// <summary>
    /// モーターパラメータ設定コマンド
    /// </summary>
    /// <remarks>
    /// モーターパラメータ設定コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0109 : RackTransferCommCommand_0002
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0109()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0109;
            this.CommandId = (int)this.CommKind;
        }

        #endregion
    }
    /// <summary>
    /// モーターパラメータ設定コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// モーターパラメータ設定コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1109 : RackTransferCommCommand_1002
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1109()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1109;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// バージョンコマンド
    /// </summary>
    /// <remarks>
    /// バージョンコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0111 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0111()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0111;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// バージョン文字列
        /// </summary>
        public String Version { get; set; }

#if SIMULATOR
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.Version, 11);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            String tmpdata1;
            resultList.Add(text_data.spoilString(out tmpdata1, 11));
            Version = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }
    /// <summary>
    /// バージョンコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// バージョンコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1111 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1111()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1111;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// ラック情報通知コマンド
    /// </summary>
    /// <remarks>
    /// ラック情報通知コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0117 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0117()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0117;
            this.CommandId = (int)this.CommKind;

            this.CupKind = new SpecimenCupKind[CarisXConst.RACK_POS_COUNT];
            this.SampleId = new String[CarisXConst.RACK_POS_COUNT];
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID { get; set; } = "";

        /// <summary>
        /// カップ種別
        /// </summary>
        public SpecimenCupKind[] CupKind { get; set; }

        /// <summary>
        /// 検体ID
        /// </summary>
        public String[] SampleId { get; set; }

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackID, 4);

                Int32 i;
                for (i = 0; i < this.CupKind.Count(); i++)
                {
                    builder.Append(((int)this.CupKind[i]).ToString(), 1);
                }
                for (i = 0; i < this.SampleId.Count(); i++)
                {
                    builder.Append(this.SampleId[i].ToString(), 16);
                }

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            String tmpdata1;
            Byte tmpdata2;

            resultList.Add(text_data.spoilString(out tmpdata1, 4));
            this.RackID = tmpdata1;

            for (int i = 0; i < this.CupKind.Count(); i++)
            {
                resultList.Add(text_data.spoilByte(out tmpdata2, 1));
                this.CupKind[i] = (SpecimenCupKind)tmpdata2;
            }

            for (int i = 0; i < this.SampleId.Count(); i++)
            {
                resultList.Add(text_data.spoilString(out this.SampleId[i], 16));
            }

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
        #endregion

    }
    /// <summary>
    /// ラック情報通知コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラック情報通知コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1117 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1117()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1117;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// ラック状態通知コマンド
    /// </summary>
    /// <remarks>
    /// ラック状態通知コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0118 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0118()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0118;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }
    /// <summary>
    /// ラック状態通知コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラック状態通知コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1118 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1118()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1118;
            this.CommandId = (int)this.CommKind;
        }
        #endregion
    }

    /// <summary>
    /// ラック移動位置問合せ（装置待機位置）コマンド
    /// </summary>
    /// <remarks>
    /// ラック移動位置問合せコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0119 : CarisXCommCommand
    {
        #region [定数定義]
        /// <summary>
        /// 位置種別
        /// </summary>
        public enum PositionKind
        {
            BCR,
            Slave1,
            Slave2,
            Slave3,
            Slave4
        }
        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0119()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0119;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID { get; set; } = "";

        /// <summary>
        /// 開始位置
        /// </summary>
        public PositionKind StartPosition { get; set; } = PositionKind.BCR;

#if DEBUG
        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackID, 4);
                builder.Append(((int)this.StartPosition).ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
#endif

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            String tmpdata1;
            Int32 tmpdata2;
            resultList.Add(text_data.spoilString(out tmpdata1, 4));
            resultList.Add(text_data.spoilInt(out tmpdata2, 1));
            RackID = tmpdata1;
            StartPosition = (PositionKind)tmpdata2;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
        #endregion

    }
    /// <summary>
    /// ラック移動位置問合せ（装置待機位置）コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラック移動位置問合せコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1119 : CarisXCommCommand
    {
        #region [定数定義]

        /// <summary>
        /// 装置間移動有無
        /// </summary>
        public enum MoveBetweenDeviceKind
        {
            No,
            Yes
        }
        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1119()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1119;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 停止位置
        /// </summary>
        public RackPositionKind StopPosition { get; set; } = RackPositionKind.Rack;

        /// <summary>
        /// 装置間移動有無
        /// </summary>
        public MoveBetweenDeviceKind MoveBetweenDevice { get; set; } = MoveBetweenDeviceKind.No;

        /// <summary>
        /// 装置間移動有無
        /// </summary>
        public String RackID { get; set; } = "";

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(((int)this.StopPosition).ToString(), 1);
                builder.Append(((int)this.MoveBetweenDevice).ToString(), 1);
                builder.Append(this.RackID, 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Byte tmpdata1;
            Byte tmpdata2;
            String tmpdata3;
            resultList.Add(text_data.spoilByte(out tmpdata1, 1));
            resultList.Add(text_data.spoilByte(out tmpdata2, 1));
            resultList.Add(text_data.spoilString(out tmpdata3, 4));
            StopPosition = (RackPositionKind)tmpdata1;
            MoveBetweenDevice = (MoveBetweenDeviceKind)tmpdata2;
            RackID = tmpdata3;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }
        #endregion

    }

    /// <summary>
    /// ラック移動位置問合せ（BCR）コマンド
    /// </summary>
    /// <remarks>
    /// ラック移動位置問合せコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_0120 : RackTransferCommCommand_0119
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_0120()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand0120;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

    }

    /// <summary>
    /// ラック移動位置問合せ（BCR）コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラック移動位置問合せ（BCR）コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackTransferCommCommand_1120 : RackTransferCommCommand_1119
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackTransferCommCommand_1120()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackTransferCommand1120;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

    }
    #endregion

    #endregion

    #region RackAndSlave

    #region Rack→Slave

    /// <summary>
    /// ラックキャッチ要求コマンド
    /// </summary>
    /// <remarks>
    /// ラックキャッチ要求コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackAndSlaveCommCommand_0201 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackAndSlaveCommCommand_0201()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackAndSlaveCommand0201;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID { get; set; }

        /// <summary>
        /// キャッチポジション
        /// </summary>
        public Int32 CatchPosition { get; set; }

        /// <summary>
        /// フォークポジション
        /// </summary>
        public Int32 ForkPosition { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackID, 4);
                builder.Append(this.CatchPosition.ToString(), 1);
                builder.Append(this.ForkPosition.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            String tmpdata1 = "";
            Int32 tmpdata2 = 0;
            Int32 tmpdata3 = 0;
            resultList.Add(text_data.spoilString(out tmpdata1, 4));
            resultList.Add(text_data.spoilInt(out tmpdata2, 1));
            resultList.Add(text_data.spoilInt(out tmpdata3, 1));
            this.RackID = tmpdata1;
            this.CatchPosition = tmpdata2;
            this.ForkPosition = tmpdata3;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// ラックキャッチ要求コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラックキャッチ要求コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackAndSlaveCommCommand_1201 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackAndSlaveCommCommand_1201()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackAndSlaveCommand1201;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 要求受付結果
        /// </summary>
        public Int32 RequestResult { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RequestResult.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            this.RequestResult = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// ラックリリース要求コマンド
    /// </summary>
    /// <remarks>
    /// ラックリリース要求コマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackAndSlaveCommCommand_0202 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackAndSlaveCommCommand_0202()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackAndSlaveCommand0202;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID { get; set; }

        /// <summary>
        /// リリースポジション
        /// </summary>
        public Int32 ReleasePosition { get; set; }

        /// <summary>
        /// フォークポジション
        /// </summary>
        public Int32 ForkPosition { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackID, 4);
                builder.Append(this.ReleasePosition.ToString(), 1);
                builder.Append(this.ForkPosition.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            String tmpdata1 = "";
            Int32 tmpdata2 = 0;
            Int32 tmpdata3 = 0;
            resultList.Add(text_data.spoilString(out tmpdata1, 4));
            resultList.Add(text_data.spoilInt(out tmpdata2, 1));
            resultList.Add(text_data.spoilInt(out tmpdata3, 1));
            this.RackID = tmpdata1;
            this.ReleasePosition = tmpdata2;
            this.ForkPosition = tmpdata3;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// ラックリリース要求コマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラックリリース要求コマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackAndSlaveCommCommand_1202 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackAndSlaveCommCommand_1202()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackAndSlaveCommand1202;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 要求受付結果
        /// </summary>
        public Int32 RequestResult { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RequestResult.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            this.RequestResult = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    #endregion

    #region Slave→Rack

    /// <summary>
    /// ラックキャッチコマンド
    /// </summary>
    /// <remarks>
    /// ラックキャッチコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackAndSlaveCommCommand_0301 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackAndSlaveCommCommand_0301()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackAndSlaveCommand0301;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackID, 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            String tmpdata1 = "";
            resultList.Add(text_data.spoilString(out tmpdata1, 4));
            this.RackID = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// ラックキャッチコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラックキャッチコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackAndSlaveCommCommand_1301 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackAndSlaveCommCommand_1301()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackAndSlaveCommand1301;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// キャッチ結果
        /// </summary>
        public Int32 CatchResult { get; set; }

        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID { get; set; }

        /// <summary>
        /// フォークポジション
        /// </summary>
        public Int32 ForkPosition { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.CatchResult.ToString(), 1);
                builder.Append(this.RackID, 4);
                builder.Append(this.ForkPosition.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            String tmpdata2 = "";
            Int32 tmpdata3 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilString(out tmpdata2, 4));
            resultList.Add(text_data.spoilInt(out tmpdata3, 1));
            this.CatchResult = tmpdata1;
            this.RackID = tmpdata2;
            this.ForkPosition = tmpdata3;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// ラックリリースコマンド
    /// </summary>
    /// <remarks>
    /// ラックリリースコマンドデータクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackAndSlaveCommCommand_0302 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackAndSlaveCommCommand_0302()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackAndSlaveCommand0302;
            this.CommandId = (int)this.CommKind;
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.RackID, 4);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            String tmpdata1 = "";
            resultList.Add(text_data.spoilString(out tmpdata1, 4));
            this.RackID = tmpdata1;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    /// <summary>
    /// ラックリリースコマンド（レスポンス）
    /// </summary>
    /// <remarks>
    /// ラックリリースコマンド（レスポンス）データクラス。
    /// 生成及びパースを行います。
    /// </remarks>
    public class RackAndSlaveCommCommand_1302 : CarisXCommCommand
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RackAndSlaveCommCommand_1302()
        {
            // Idと種別をコンストラクタで設定
            this.CommKind = CommandKind.RackAndSlaveCommand1302;
            this.CommandId = (int)this.CommKind;
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// リリース結果
        /// </summary>
        public Int32 ReleaseResult { get; set; }

        /// <summary>
        /// ラックID
        /// </summary>
        public String RackID { get; set; }

        /// <summary>
        /// フォークポジション
        /// </summary>
        public Int32 ForkPosition { get; set; }

        /// <summary>
        /// コマンドテキスト 設定/取得
        /// </summary>
        public override String CommandText
        {
            get
            {
                // メンバ内容からテキスト生成する。
                TextDataBuilder builder = new TextDataBuilder();
                builder.Append(this.ReleaseResult.ToString(), 1);
                builder.Append(this.RackID, 4);
                builder.Append(this.ForkPosition.ToString(), 1);

                // テキスト化
                base.CommandText = builder.GetAppendString();

                return base.CommandText;
            }
            set
            {
                base.CommandText = value;
            }
        }
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// コマンド解析
        /// </summary>
        /// <remarks>
        /// コマンドの解析を行います。
        /// </remarks>
        /// <param name="commandStr">コマンド文字列</param>
        /// <returns>True:解析成功 False:解析失敗</returns>
        public override Boolean SetCommandString(String commandStr)
        {
            // ベースでコマンド文字列の設定を行う
            base.SetCommandString(commandStr);

            TextData text_data = new TextData(commandStr);

            Boolean setSuccess = false;

            // 項目別結果リスト
            List<Boolean> resultList = new List<Boolean>();

            // コマンドキャストチェック・メンバへのセット
            Int32 tmpdata1 = 0;
            String tmpdata2 = "";
            Int32 tmpdata3 = 0;
            resultList.Add(text_data.spoilInt(out tmpdata1, 1));
            resultList.Add(text_data.spoilString(out tmpdata2, 4));
            resultList.Add(text_data.spoilInt(out tmpdata3, 1));
            this.ReleaseResult = tmpdata1;
            this.RackID = tmpdata2;
            this.ForkPosition = tmpdata3;

            // 失敗が一つでも含まれていれば変換は正常終了しない。
            setSuccess = !resultList.Contains(false);

            return setSuccess;
        }

        #endregion
    }

    #endregion

    #endregion

    // 2020-02-27 CarisX IoT Add [START]
    #region IoT

    /// <summary>
    /// IoT測定结果 
    /// </summary>
    public class IoTCommCommand_0010
    {

        #region 私有字段

        /// <summary>
        /// 机种 如 Caris200: 1   CarisX: 2				
        /// </summary>
        private short model_id;
        /// <summary>
        /// 命令ID	
        /// </summary>
        private short command_id;
        /// <summary>
        /// 仪器编号	
        /// </summary>
        private long machine_serial_number;

        /// <summary>
        /// 样本区分	
        /// </summary>
        private short sample_meas_kind;

        /// <summary>
        /// 受理No.	
        /// </summary>
        private int receipt_number;

        /// <summary>
        /// 时序No.	
        /// </summary>
        private short sequence_no;

        /// <summary>
        /// 样本架ID	
        /// </summary>
        private string rack_id;

        /// <summary>
        /// 样本架Pos.	
        /// </summary>
        private short rack_position;

        /// <summary>
        /// 样本类别	
        /// </summary>
        private short specimen_material_type;

        /// <summary>
        /// 样本批号	
        /// </summary>
        private string sample_lot;

        /// <summary>
        /// 质控名	
        /// </summary>
        private string control_name;

        /// <summary>
        /// 手动稀释倍率		
        /// </summary>
        private short manual_dilution;

        /// <summary>
        /// 试剂项目	
        /// </summary>
        private string reagent_item;

        /// <summary>
        /// Count值	
        /// </summary>
        private int count_value;

        /// <summary>
        /// 浓度值	
        /// </summary>
        private double concentration;

        /// <summary>
        /// 判定
        /// </summary>
        private string judgment;

        /// <summary>
        /// Remark	
        /// </summary>
        private long remark;

        /// <summary>
        /// 后稀释倍率		
        /// </summary>
        private short auto_dilution_ratio;

        /// <summary>
        /// 试剂批号	
        /// </summary>
        private string reagent_lot_no;

        /// <summary>
        /// 预激发液批号
        /// </summary>
        private string pretrigger_lot_no;

        /// <summary>
        /// 激发液批号
        /// </summary>
        private string trigger_lot_no;

        /// <summary>
        /// 校准状态
        /// </summary>
        private DateTime calibcurve_datetime;

        /// <summary>
        /// 测试日期时间		形式：YYYY/MM/DD hh:mm:ss.fff						
        /// </summary>
        private DateTime measuring_datetime;

        private int s1;

        private int s2;

        private int s3;

        /// <summary>
        /// 样本分注量		
        /// </summary>
        private short dispense_volume;

        /// <summary>
        /// 样本吸取位置		
        /// </summary>
        private double sample_aspiration;

        /// <summary>
        /// M试剂端口号		
        /// </summary>
        private short m_reagent_port_no;

        /// <summary>
        /// M试剂液面位置		
        /// </summary>
        private double m_sample_position;

        /// <summary>
        /// R1试剂端口号		
        /// </summary>
        private short r1_reagent_port_no;

        /// <summary>
        /// R1试剂液面位置			
        /// </summary>
        private double r1_sample_position;

        /// <summary>
        /// R2试剂端口号		
        /// </summary>
        private short r2_reagent_port_no;

        /// <summary>
        /// R2试剂液面位置			
        /// </summary>
        private double r2_sample_position;

        /// <summary>
        /// 温度（免疫反应槽部）				
        /// </summary>
        private double temperature_1;

        /// <summary>
        /// 温度（R1针）		
        /// </summary>
        private double temperature_2;

        /// <summary>
        /// 温度（R2针）		
        /// </summary>
        private double temperature_3;

        /// <summary>
        /// 温度（B/F1）		
        /// </summary>
        private double temperature_4;

        /// <summary>
        /// 温度（B/F2）		
        /// </summary>
        private double temperature_5;

        /// <summary>
        /// 温度（化学发光测试部）				
        /// </summary>
        private double temperature_6;

        /// <summary>
        /// 温度（室温）		
        /// </summary>
        private double temperature_7;
        #endregion 

        public IoTCommCommand_0010()
        {
            this.command_id = (Int32)CommandKind.IoTCommand0001;
        }

        #region 公共属性
        /// <summary>
        /// 命令ID	
        /// </summary>
        public short Command_id
        {
            get { return command_id; }
            set { command_id = value; }
        }

        /// <summary>
        /// 机种
        /// </summary>
        public short Model_id
        {
            get { return model_id; }
            set { model_id = value; }
        }

        /// <summary>
        /// 仪器编号	
        /// </summary>
        public long Machine_serial_number
        {
            get { return machine_serial_number; }
            set { machine_serial_number = value; }
        }

        /// <summary>
        /// 样本区分	
        /// </summary>
        public short Sample_meas_kind
        {
            get { return sample_meas_kind; }
            set { sample_meas_kind = value; }
        }

        /// <summary>
        /// 受理No.	
        /// </summary>
        public int Receipt_number
        {
            get { return receipt_number; }
            set { receipt_number = value; }
        }

        /// <summary>
        /// 时序No.	
        /// </summary>
        public short Sequence_no
        {
            get { return sequence_no; }
            set { sequence_no = value; }
        }

        /// <summary>
        /// 样本架ID	
        /// </summary>
        public string Rack_id
        {
            get { return rack_id; }
            set { rack_id = value; }
        }

        /// <summary>
        /// 样本架Pos.		
        /// </summary>
        public short Rack_position
        {
            get { return rack_position; }
            set { rack_position = value; }
        }

        /// <summary>
        /// 样本类别	
        /// </summary>
        public short Specimen_material_type
        {
            get { return specimen_material_type; }
            set { specimen_material_type = value; }
        }

        /// <summary>
        /// 样本批号	
        /// </summary>
        public string Sample_lot
        {
            get { return sample_lot; }
            set { sample_lot = value; }
        }

        /// <summary>
        /// 质控名	
        /// </summary>
        public string Control_name
        {
            get { return control_name; }
            set { control_name = value; }
        }

        /// <summary>
        /// 手动稀释倍率		
        /// </summary>
        public short Manual_dilution
        {
            get { return manual_dilution; }
            set { manual_dilution = value; }
        }

        /// <summary>
        /// 试剂项目	
        /// </summary>
        public string Reagent_item
        {
            get { return reagent_item; }
            set { reagent_item = value; }
        }

        /// <summary>
        /// Count值	
        /// </summary>
        public int Count_value
        {
            get { return count_value; }
            set { count_value = value; }
        }

        /// <summary>
        /// 浓度值	
        /// </summary>
        public double Concentration
        {
            get { return concentration; }
            set { concentration = value; }
        }

        /// <summary>
        /// 判定	
        /// </summary>
        public string Judgment
        {
            get { return judgment; }
            set { judgment = value; }
        }

        /// <summary>
        /// Remark	
        /// </summary>
        public long Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// 后稀释倍率		
        /// </summary>
        public short Auto_dilution_ratio
        {
            get { return auto_dilution_ratio; }
            set { auto_dilution_ratio = value; }
        }

        /// <summary>
        /// 试剂批号	
        /// </summary>
        public string Reagent_lot_no
        {
            get { return reagent_lot_no; }
            set { reagent_lot_no = value; }
        }

        /// <summary>
        /// 预激发液批号
        /// </summary>
        public string Pretrigger_lot_no
        {
            get { return pretrigger_lot_no; }
            set { pretrigger_lot_no = value; }
        }

        /// <summary>
        /// 激发液批号
        /// </summary>
        public string Trigger_lot_no
        {
            get { return trigger_lot_no; }
            set { trigger_lot_no = value; }
        }

        /// <summary>
        /// 校准状态
        /// </summary>
        public DateTime Calibcurve_datetime
        {
            get { return calibcurve_datetime; }
            set { calibcurve_datetime = value; }
        }

        /// <summary>
        /// 测试日期时间		
        /// </summary>
        public DateTime Measuring_datetime
        {
            get { return measuring_datetime; }
            set { measuring_datetime = value; }
        }

        /// <summary>
        /// S1
        /// </summary>
        public int S1
        {
            get { return s1; }
            set { s1 = value; }
        }

        /// <summary>
        /// S2
        /// </summary>
        public int S2
        {
            get { return s2; }
            set { s2 = value; }
        }

        /// <summary>
        /// S3
        /// </summary>
        public int S3
        {
            get { return s3; }
            set { s3 = value; }
        }

        /// <summary>
        /// 样本分注量		
        /// </summary>
        public short Dispense_volume
        {
            get { return dispense_volume; }
            set { dispense_volume = value; }
        }

        /// <summary>
        /// 样本吸取位置		
        /// </summary>
        public double Sample_aspiration
        {
            get { return sample_aspiration; }
            set { sample_aspiration = value; }
        }

        /// <summary>
        /// M试剂端口号		
        /// </summary>
        public short M_Reagent_port_no
        {
            get { return m_reagent_port_no; }
            set { m_reagent_port_no = value; }
        }

        /// <summary>
        /// M试剂液面位置		
        /// </summary>
        public double M_Sample_position
        {
            get { return m_sample_position; }
            set { m_sample_position = value; }
        }

        /// <summary>
        /// R1试剂端口号		
        /// </summary>
        public short R1_reagent_port_no
        {
            get { return r1_reagent_port_no; }
            set { r1_reagent_port_no = value; }
        }

        /// <summary>
        /// R1试剂液面位置			
        /// </summary>
        public double R1_sample_position
        {
            get { return r1_sample_position; }
            set { r1_sample_position = value; }
        }

        /// <summary>
        /// R2试剂端口号		
        /// </summary>
        public short R2_reagent_port_no
        {
            get { return r2_reagent_port_no; }
            set { r2_reagent_port_no = value; }
        }

        /// <summary>
        /// R2试剂液面位置			
        /// </summary>
        public double R2_sample_position
        {
            get { return r2_sample_position; }
            set { r2_sample_position = value; }
        }

        /// <summary>
        /// 温度（免疫反应槽部）				
        /// </summary>
        public double Temperature_1
        {
            get { return temperature_1; }
            set { temperature_1 = value; }
        }

        /// <summary>
        /// 温度（R1针）		
        /// </summary>
        public double Temperature_2
        {
            get { return temperature_2; }
            set { temperature_2 = value; }
        }

        /// <summary>
        /// 温度（R2针）		
        /// </summary>
        public double Temperature_3
        {
            get { return temperature_3; }
            set { temperature_3 = value; }
        }

        /// <summary>
        /// 温度（B/F1）		
        /// </summary>
        public double Temperature_4
        {
            get { return temperature_4; }
            set { temperature_4 = value; }
        }

        /// <summary>
        /// 温度（B/F2）		
        /// </summary>
        public double Temperature_5
        {
            get { return temperature_5; }
            set { temperature_5 = value; }
        }

        /// <summary>
        /// 温度（化学发光测试部）				
        /// </summary>
        public double Temperature_6
        {
            get { return temperature_6; }
            set { temperature_6 = value; }
        }

        /// <summary>
        /// 温度（室温）		
        /// </summary>
        public double Temperature_7
        {
            get { return temperature_7; }
            set { temperature_7 = value; }
        }
        #endregion
    }

    /// <summary>
    ///  IoT故障数据
    /// </summary>
    public class IoTCommConmand_0020
    {
        #region 私有变量
        /// <summary>
        /// 命令ID	
        /// </summary>
        private short command_id;

        /// <summary>
        /// 机种
        /// </summary>
        private short model_id;

        /// <summary>
        /// 仪器编号	
        /// </summary>
        private long machine_serial_number;

        /// <summary>
        /// 发生日期时间		形式：YYYY/MM/DD hh:mm:ss.fff						
        /// </summary>
        private DateTime acquired_datetime;

        /// <summary>
        /// 错误代码	
        /// </summary>
        private int error_code;

        /// <summary>
        /// 引数
        /// </summary>
        private int error_arg;

        /// <summary>
        /// 文字列	
        /// </summary>
        private string contents;

        /// <summary>
        /// 试剂项目	
        /// </summary>
        private string reagent_item;

        /// <summary>
        /// 错误等级	
        /// </summary>
        private short error_level;
        #endregion

        public IoTCommConmand_0020()
        {
            this.command_id = (Int32)CommandKind.IoTCommand0002;
        }

        #region 公共属性
        /// <summary>
        /// 命令ID	
        /// </summary>
        public short Command_id
        {
            get { return command_id; }
            set { command_id = value; }
        }

        /// <summary>
        /// 机种	
        /// </summary>
        public short Model_id
        {
            get { return model_id; }
            set { model_id = value; }
        }

        /// <summary>
        /// 仪器编号	
        /// </summary>
        public long Machine_serial_number
        {
            get { return machine_serial_number; }
            set { machine_serial_number = value; }
        }

        /// <summary>
        /// 发生日期时间		
        /// </summary>
        public DateTime Acquired_datetime
        {
            get { return acquired_datetime; }
            set { acquired_datetime = value; }
        }

        /// <summary>
        /// 错误代码	
        /// </summary>
        public int Error_code
        {
            get { return error_code; }
            set { error_code = value; }
        }

        /// <summary>
        /// 引数
        /// </summary>
        public int Error_arg
        {
            get { return error_arg; }
            set { error_arg = value; }
        }

        /// <summary>
        /// 文字列	
        /// </summary>
        public string Contents
        {
            get { return contents; }
            set { contents = value; }
        }

        /// <summary>
        /// 试剂项目	
        /// </summary>
        public string Reagent_item
        {
            get { return reagent_item; }
            set { reagent_item = value; }
        }

        /// <summary>
        /// 错误等级	
        /// </summary>
        public short Error_level
        {
            get { return error_level; }
            set { error_level = value; }
        }
        #endregion 
    }

    /// <summary>
    ///  IoT使用日期数据
    /// </summary>
    public class IoTCommConmand_0030
    {
        /// <summary>
        /// 命令ID	
        /// </summary>
        private short command_id;

        /// <summary>
        /// 机种
        /// </summary>
        private short model_id;

        /// <summary>
        /// 仪器编号	
        /// </summary>
        private long machine_serial_number;

        /// <summary>
        /// 设定时间	
        /// </summary>
        private DateTime datetime;

        public IoTCommConmand_0030()
        {
            this.command_id = (Int32)CommandKind.IoTCommand0003;
        }

        /// <summary>
        /// 命令ID
        /// </summary>
        public short Command_id
        {
            get { return command_id; }
            set { command_id = value; }
        }

        /// <summary>
        /// 机种
        /// </summary>
        public short Model_id
        {
            get { return model_id; }
            set { model_id = value; }
        }

        /// <summary>
        /// 仪器编号
        /// </summary>
        public long Machine_serial_number
        {
            get { return machine_serial_number; }
            set { machine_serial_number = value; }
        }

        /// <summary>
        /// 设定时间	
        /// </summary>
        public DateTime Datetime
        {
            get { return datetime; }
            set { datetime = value; }
        }
    }

    #endregion
    // 2020-02-27 CarisX IoT Add [END]

    #endregion


}
