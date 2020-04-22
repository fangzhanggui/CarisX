using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.CarisX.Parameter;
using Oelco.CarisX.Comm;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Utility
{
    /// <summary>
    /// コマンドデータ仲介クラス
    /// </summary>
    /// <remarks>
    /// コマンドデータ-データクラス間でのデータ受け渡しを中継します。
    /// </remarks>
    public class CommandDataMediator
    {
        /// <summary>
        /// 消耗品データ設定(パラメータクラス→コマンドクラス)
        /// </summary>
        /// <remarks>
        /// 消耗品データ設定のパラメータクラスからコマンドクラスへ設定します。
        /// </remarks>
        /// <param name="supplie">コマンド共通パラメータ</param>
        /// <param name="command">消耗品パラメータ</param>
        /// <param name="control">寿命部品使用回数問い合わせコマンド</param>
        public static void SetSupplieParamToCmd(int moduleIdx, CommandControlParameter control, SupplieParameter supplie, ref SlaveCommCommand_0444 command )
        {
            command.Control = control;
            command.SampleCyl = supplie.SlaveList[moduleIdx].SampleDispensingSyringePackin.UseCount;
            command.CylR1 = supplie.SlaveList[moduleIdx].R1DispensingSyringePackin.UseCount;
            command.CylR2 = supplie.SlaveList[moduleIdx].R2DispensingSyringePackin.UseCount;
            command.ReagWashCyl = supplie.SlaveList[moduleIdx].ReagentDispensingSyringePackin.UseCount;
            command.DilutieCyl = supplie.SlaveList[moduleIdx].DiluentDispensingSyringePackin.UseCount;
            command.OutDrainPump = (Int32)supplie.SlaveList[moduleIdx].OutDrainPump.UseTime.TimeSpan.TotalMinutes;
            command.OutDrainPumpTube = (Int32)supplie.SlaveList[moduleIdx].OutDrainPumpTube.UseTime.TimeSpan.TotalMinutes;
            command.PreTreggerCyl = supplie.SlaveList[moduleIdx].PreTriggerDispensingSyringePackin.UseCount;
            command.TreggerCyl = supplie.SlaveList[moduleIdx].TriggerDispensingSyringePackin.UseCount;
            command.Wash1Cyl = supplie.SlaveList[moduleIdx].Wash1DispensingSyringePackin.UseCount;
            command.Wash2Cyl = supplie.SlaveList[moduleIdx].Wash2DispensingSyringePackin.UseCount;
        }
        /// <summary>
        /// 消耗品データ設定(コマンドクラス→パラメータクラス)
        /// </summary>
        /// <remarks>
        /// 消耗品データの設定を行います。
        /// </remarks>
        /// <param name="command">寿命部品使用回数問い合わせコマンド（レスポンス）</param>
        /// <param name="supplie">消耗品パラメータ</param>
        public static void SetSupplieCmdToParam(SlaveCommCommand_1444 command, ref SupplieParameter supplie )
        {
            // モジュールIndex取得
            Int32 moduleIndex = CarisXSubFunction.MachineCodeToModuleIndex((MachineCode)command.CommNo);

            supplie.SlaveList[moduleIndex].SampleDispensingSyringePackin.UseCount = command.SampleCyl;
            supplie.SlaveList[moduleIndex].R1DispensingSyringePackin.UseCount = command.CylR1;
            supplie.SlaveList[moduleIndex].R2DispensingSyringePackin.UseCount = command.CylR2;
            supplie.SlaveList[moduleIndex].ReagentDispensingSyringePackin.UseCount = command.ReagWashCyl;
            supplie.SlaveList[moduleIndex].DiluentDispensingSyringePackin.UseCount = command.DilutieCyl;
            supplie.SlaveList[moduleIndex].OutDrainPump.UseTime = TimeSpan.FromMinutes( command.OutDrainPump );
            supplie.SlaveList[moduleIndex].OutDrainPumpTube.UseTime = TimeSpan.FromMinutes( command.OutDrainPumpTube );
            supplie.SlaveList[moduleIndex].PreTriggerDispensingSyringePackin.UseCount = command.PreTreggerCyl;
            supplie.SlaveList[moduleIndex].TriggerDispensingSyringePackin.UseCount = command.TreggerCyl;
            supplie.SlaveList[moduleIndex].Wash1DispensingSyringePackin.UseCount = command.Wash1Cyl;
            supplie.SlaveList[moduleIndex].Wash2DispensingSyringePackin.UseCount = command.Wash2Cyl;
        }

        /// <summary>
        /// 総アッセイ数設定(パラメータクラス→コマンドクラス)
        /// </summary>
        /// <remarks>
        /// 総アッセイ数設定のパラメータクラスからコマンドクラスへ設定します。
        /// </remarks>
        /// <param name="supplie">コマンド許打つうパラメータ</param>
        /// <param name="command">消耗品パラメータ</param>
        /// <param name="control">寿命部品使用回数問い合わせコマンド</param>
        public static void SetTotalAssayTimesToCmd(int moduleIdx, SupplieParameter supplie, ref SlaveCommCommand_0484 command)
        {
            command.NoOfAssay = supplie.SlaveList[moduleIdx].TotalAssay;
        }
    }
}
