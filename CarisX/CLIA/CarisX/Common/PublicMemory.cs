using Oelco.CarisX.Const;
using Oelco.CarisX.Parameter;
using Oelco.Common.Parameter;
using Oelco.Common.Utility;
using System;
using System.Collections.Generic;

namespace Oelco.CarisX.Common
{

    /// <summary>
    /// 公開メモリ
    /// </summary>
    /// <remarks>
    /// グローバルの変数を保持しておく。Singletonで使用。
    /// ただし、基本的には使用しない事。
    /// </remarks>
    public class PublicMemory
    {
        /// <summary>
        /// 試薬準備完了結果
        /// </summary>
        /// <remarks>
        /// 1417コマンドの内容を保持。
        /// 1417コマンドの内容をメッセージで通知してメイン側で保持しようとすると、0520の処理に間に合わない為、
        /// グローバルでもってメッセージを介さずに保持できるようにする
        /// </remarks>
        public ReagentPreparationErrorTarget[] prepareResult { get; set; } = null;

        /// <summary>
        /// モジュール番号
        /// </summary>
        /// <remarks>
        /// メイン画面で選択されているモジュールの内容を保持
        /// </remarks>
        public ModuleIndex moduleIndex { get; set; } = ModuleIndex.Module1;

        /// <summary>
        /// モジュール温度
        /// </summary>
        public Temperature[] moduleTemperature { get; set; }

        /// <summary>
        /// 最終ラック移動位置
        /// </summary>
        public Dictionary<Int32, Int32> lastRackMovePosition { get; set; } = new Dictionary<int, int>();

        /// <summary>
        /// 洗浄液タンク状態
        /// </summary>
        /// <remarks>
        /// 洗浄液タンクの状態を保持。DBのRemainに設定している値なので、そのまま画面表示のステータスには設定しないこと
        /// </remarks>
        public WashSolutionTankStatusKind WashSolutionTankStatus { get; set; } = WashSolutionTankStatusKind.Full;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PublicMemory()
        {
            //モジュール温度変数を初期化
            moduleTemperature = new Temperature[Enum.GetValues(typeof(ModuleIndex)).Length];
            for (int i = 0; i < moduleTemperature.Length; i++)
            {
                moduleTemperature[i] = new Temperature();
            }
            lastRackMovePosition = new Dictionary<int, int>();
            lastRackMovePosition.Add(0, 0);
        }
    }

}
