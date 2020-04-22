using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Oelco.CarisX.Const;
using System.Text.RegularExpressions;
using Oelco.Common.Utility;
using Oelco.CarisX.Parameter;
using System.Windows.Forms;
using Oelco.Common.Const;
using Oelco.CarisX.Log;
using Oelco.Common.Log;
using Oelco.Common.Parameter;
using Oelco.CarisX.Common;
using System.Globalization;
using Oelco.CarisX.DB;
using Oelco.Common.Comm;
using Oelco.CarisX.Comm;
using System.Net.NetworkInformation;
using Oelco.CarisX.Status;
using Oelco.CarisX.GUI;
using System.Reflection;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Xml;
using System.Runtime.InteropServices;
using Oelco.Common.DB;
using System.Data;
using System.Diagnostics;

namespace Oelco.CarisX.Utility
{
    /// <summary>
    /// ラック検索結果
    /// </summary>
    public enum RackFindResult
    {
        /// <summary>
        /// 未検出
        /// </summary>
        NotFound,
        /// <summary>
        /// モジュール１～４で検出
        /// </summary>
        FindOnModule,
        /// <summary>
        /// 待機ラックで検出
        /// </summary>
        FindOnWaitingRack,
        /// <summary>
        /// 回収ラックで検出
        /// </summary>
        FindOnCollectRack,
    }

    /// <summary>
    /// CarisX用補助関数群クラス
    /// </summary>
    static public class CarisXSubFunction
    {
        /// <summary>
        /// DPRエラー書き込み
        /// </summary>
        /// <remarks>
        /// DPRのエラー履歴を追加します。
        /// </remarks>
        /// <param name="code">エラーコード</param>
        /// <param name="extArg">エラー引数</param>
        /// <param name="extStr">拡張文字列</param>
        /// <param name="raiseAlert">アラートを発生させるか否か</param>
        public static void WriteDPRErrorHist(DPRErrorCode code, Int32 extArg = 0, String extStr = "", bool raiseAlert = true)
        {
            CarisXLogInfoErrorLogExtention infoErrLog = new CarisXLogInfoErrorLogExtention()
            {
                ErrorCode = code.ErrorCode,
                ErrorArg = extArg == 0 ? code.Arg : extArg
            };

            // フィルタリング判別フラグ
            Boolean isErrFiltering = false;

            // 分析中またはサンプリング停止中の場合
            if ((Singleton<SystemStatus>.Instance.Status == SystemStatusKind.Assay)
                || (Singleton<SystemStatus>.Instance.Status == SystemStatusKind.SamplingPause))
            {
                // フィルタリングが必要か否か判別する
                isErrFiltering = CarisXSubFunction.IsErrFiltering(infoErrLog.ErrorCode);
            }

            // フィルタリングを行う場合
            if (isErrFiltering)
            {
                // モジュールIDをモジュールインデックスに変換
                int ModuleIndex = ModuleIDToModuleIndex(code.ModuleId);

                // エラー蓄積データリストに追加/上書きとマスターエラー発生回数の更新
                infoErrLog.Counter = Singleton<ErrorDataStorageListManeger>.Instance.ErrorDataStorageList[ModuleIndex].ErrorDataStorageListUpdateOrAdd(code, extStr);
            }
            // フィルタリングを行わない場合
            else
            {
                // 登録可能な場合、エラー履歴に登録
                Singleton<CarisXLogManager>.Instance.Write(LogKind.ErrorHist
                                                         , Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                                         , code.ModuleId
                                                         , infoErrLog
                                                         , extStr);
            }

            // マスターエラー履歴テーブルへ登録
            Singleton<CarisXLogManager>.Instance.Write(LogKind.MasterErrorHist
                                                     , Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                                                     , code.ModuleId
                                                     , infoErrLog
                                                     , extStr);

            //特定のエラーの場合は、Assay画面の洗浄液タンクの状態を赤色に変更する
            switch (code.ErrorCode)
            {
                case (Int32)ErrorCodeWashSolutionTankChangeRed.ErrorCode45:
                case (Int32)ErrorCodeWashSolutionTankChangeRed.ErrorCode105:
                    //洗浄液タンクの状態を更新する通知
                    Singleton<PublicMemory>.Instance.WashSolutionTankStatus = WashSolutionTankStatusKind.Low;
                    Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.WashSolutionTankStatus);
                    break;
            }

            // アラートが必要かチェック
            if (raiseAlert)
            {
                // エラー釦点滅
                //【IssuesNo:6】 传入故障代码信息，根据故障等级闪烁不同的提示灯
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.BlinkErrorButton, infoErrLog);

                // スレーブに警告灯・ブザー送信
                Singleton<NotifyManager>.Instance.PushSignalQueue((Int32)NotifyKind.SendCaution, code);
            }

            //【IssuesNo:16】IoTへ障害情報の通知処理
            SendIoTErrorCommand(infoErrLog, code.ModuleId, extStr);
        }

        /// <summary>
        /// ホスト向け跡希釈取得
        /// </summary>
        /// <remarks>
        /// 希釈倍率をホスト向け後希釈倍率で取得します。
        /// </remarks>
        /// <param name="dilution">希釈倍率</param>
        /// <returns>ホスト向け後希釈</returns>
        public static HostAutoDil GetHostAutoDil(Int32 dilution)
        {
            HostAutoDil result = HostAutoDil.NoDil;
            switch (dilution)
            {
                case 10:
                    result = HostAutoDil.Dil10;
                    break;
                case 100:
                    result = HostAutoDil.Dil100;
                    break;
                case 200:
                    result = HostAutoDil.Dil200;
                    break;
                case 1000:
                    result = HostAutoDil.Dil1000;
                    break;
                case 20:
                    result = HostAutoDil.Dil20;
                    break;
                case 400:
                    result = HostAutoDil.Dil400;
                    break;
                case 2000:
                    result = HostAutoDil.Dil2000;
                    break;
                case 4000:
                    result = HostAutoDil.Dil4000;
                    break;
                case 8000:
                    result = HostAutoDil.Dil8000;
                    break;
                default:
                    result = HostAutoDil.NoDil;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 設置ラック検索
        /// </summary>
        /// <remarks>
        /// 設置ラックの中から指定のラックIDを検索します。
        /// この検索は再検査時に使用されます。
        /// </remarks>
        /// <param name="rackId">ラックID</param>
        /// <returns>検索結果</returns>
        static public RackFindResult SearchRack(CarisXIDString rackId)
        {
            RackFindResult result = RackFindResult.NotFound;

            //ラックIDがラック位置管理にあるか
            if (Singleton<RackPositionManager>.Instance.RackPosition.ContainsKey(rackId.DispPreCharString))
            {
                //ラック位置の判定
                switch (Singleton<RackPositionManager>.Instance.RackPosition[rackId.DispPreCharString])
                {
                    case RackPositionKind.Rack:
                        result = RackFindResult.FindOnWaitingRack;
                        break;
                    case RackPositionKind.Module1:
                    case RackPositionKind.Module2:
                    case RackPositionKind.Module3:
                    case RackPositionKind.Module4:
                        result = RackFindResult.FindOnModule;
                        break;
                    case RackPositionKind.CollectRack:
                        result = RackFindResult.FindOnCollectRack;
                        break;
                }
            }

            return result;

        }

        /// <summary>
        /// DateTime取得
        /// </summary>
        /// <remarks>
        /// 指定した文字列がDateTimeにパース可能であればそのままに、
        /// パース不可であればDateTime.Minを返します。
        /// 利用される指定書式は既定の地域情報を利用します。
        /// </remarks>
        /// <param name="str">日時文字列</param>
        /// <returns>日時</returns>
        static public DateTime GetValidDateTime(String str)
        {
            DateTime result;

            if (!DateTime.TryParse(str, out result))
            {
                result = DateTime.MinValue;
            }

            return result;
        }

        /// <summary>
        /// Int32変換
        /// </summary>
        /// <remarks>
        /// Int32変換を行います。変換に失敗した場合、ゼロを返します。
        /// </remarks>
        /// <param name="str"></param>
        /// <returns>Int32変換結果</returns>
        static public Int32 Int32InnerTryParse(String str)
        {
            Int32 val = 0;
            try
            {
                val = Int32.Parse(str);
            }
            catch
            {
            }
            return val;
        }

        /// <summary>
        /// TimeSpan変換
        /// </summary>
        /// <remarks>
        /// TimeSpan変換処理を行います。変換に失敗した場合、TimeSpanの最小値を返します。
        /// </remarks>
        /// <param name="str">変換対象文字列</param>
        /// <returns>TimeSpan変換結果</returns>
        static public TimeSpan TimeSpanInnerTryParseFromHour(String str)
        {
            TimeSpan val;
            try
            {
                val = TimeSpan.FromHours(Double.Parse(str));
            }
            catch
            {
                val = TimeSpan.MinValue;
            }

            return val;
        }

        /// <summary>
        /// 残り分析時間取得
        /// </summary>
        /// <remarks>
        /// 反応槽位置より、残り分析時間を取得します。
        /// （反応槽位置=分析ステータスコマンド内の配列Index[0-101]）
        /// </remarks>
        /// <param name="pos">反応槽位置</param>
        /// <returns>残り分析時間</returns>
        static public TimeSpan GetTimeSpanFromAssayStatusPosition(Int32 pos)
        {
            Int32 second = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.CycleTimeParameter.CycleTime * ((AssayStatus.UNIQUE_PARAM_COUNT - 1) - pos);
            if (second < 0)
            {
                second = 0;
            }
            return TimeSpan.FromSeconds(second);
        }

        /// <summary>
        /// 使用期限取得
        /// </summary>
        /// <remarks>
        /// ロット番号に含まれる3桁の使用期限からDateTime型の情報を取得します。
        /// </remarks>
        /// <param name="YMM">使用期限(YMM)</param>
        /// <returns>使用期限(DateTime)</returns>
        static public DateTime GetYearMonthFromYMM(string YMM)
        {
            // 使用期限(年)の下一桁目の値を取得
            string ExDateY = YMM[0].ToString();
            int iExDateY = Int32.Parse(ExDateY);

            // 現在の時刻(年)の下一桁目の値を取得
            DateTime date = DateTime.Now;
            string TodayDateY = date.Year.ToString();
            TodayDateY = TodayDateY[3].ToString();
            int iTodayDateY = Int32.Parse(TodayDateY);

            // 現在の時刻(年)の下二桁までの値を取得
            int YY = date.Year - 2000;

            int Sa = iExDateY - iTodayDateY;
            if (Sa == 1 || Sa == -9)
                YY += 1;
            else if (Sa == 2 || Sa == -8)
                YY += 2;
            else if (Sa == 3 || Sa == -7)
                YY += 3;
            else if (Sa == 9 || Sa == -1)
                YY -= 1;
            else if (Sa == 8 || Sa == -2)
                YY -= 2;
            else if (Sa == 7 || Sa == -3)
                YY -= 3;
            else if (Sa == 6 || Sa == -4)
                YY -= 4;
            else if (Sa == 5 || Sa == -5)
                YY -= 5;
            else if (Sa == 4 || Sa == -6)
                YY -= 6;

            Int32 year = Int32.Parse("20" + YY.ToString("00"));
            Int32 month = Int32.Parse(YMM.Remove(0, 1));
            DateTime yearMonth = new DateTime(year, month, 1);

            return yearMonth;
        }

        /// <summary>
        /// ロット番号からの年月、シリアル番号取得
        /// </summary>
        /// <remarks>
        /// ロット番号を解析し、年月データ、シリアル番号を取得します。
        /// </remarks>
        /// <param name="lotNo">ロット番号</param>
        /// <param name="yearMonth">年月データ</param>
        /// <param name="serialNumber">シリアル番号</param>
        /// <returns>取得結果(true:成功／false:失敗)</returns>
        static public Boolean GetYearMonthAndSerialFromLotNo(String lotNo, out DateTime yearMonth, out Int32 serialNumber)
        {
            Boolean convertSuccess = true;
            yearMonth = DateTime.MinValue;
            serialNumber = 0;
            try
            {
                // 番号
                serialNumber = Convert.ToInt32(lotNo[3]);
                yearMonth = GetYearMonthFromYMM(lotNo.Remove(3, 1));
            }
            catch (Exception ex)
            {
                // ロット番号のフォーマットが異なります
                System.Diagnostics.Debug.WriteLine("ロット番号のフォーマットが異なります" + ex.Message);
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "ロット番号のフォーマットが異なります" + ex.StackTrace);
                convertSuccess = false;
            }

            return convertSuccess;
        }

        /// <summary>
        /// 分析項目取得
        /// </summary>
        /// <remarks>
        /// グリッドの分析項目表示文字列から、分析項目設定情報を作成します。
        /// </remarks>
        /// <param name="joinedProtocolNames">分析項目表示文字列</param>
        /// <returns>分析項目設定情報</returns>
        static public List<Tuple<Int32?, Int32?>> SplitProtocolNames(String joinedProtocolNames)
        {
            List<Tuple<Int32?, Int32?>> names = new List<Tuple<Int32?, Int32?>>();
            String[] namesAry = joinedProtocolNames.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (String name in namesAry)
            {
                // AAA(x10)の数値部分を正規表現で切り出す
                Regex regex = new Regex(".*(x(?<value>[0-9]+))");
                Match match = regex.Match(name);
                Int32? dilRatio = 1;
                String name2 = name;

                if (match.Success)
                {
                    dilRatio = Int32.Parse(match.Groups["value"].Value); // コード上からのみ操作される文字列なので変換例外発生しない。
                    name2 = name.Substring(0, name.IndexOf('('));
                }

                Int32 protoIndex = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName(name2).ProtocolIndex;
                names.Add(new Tuple<Int32?, Int32?>(protoIndex, dilRatio));
            }

            return names;
        }

        /// <summary>
        /// サンプル種別取得
        /// </summary>
        /// <remarks>
        /// サンプル種別の、グリッド表示文字列からサンプル種別値を取得します。
        /// </remarks>
        /// <param name="kind">サンプル種別文字列</param>
        /// <returns>サンプル種別値</returns>
        static public SpecimenMaterialType GetSampleKindFromGridItemString(String kind)
        {
            SpecimenMaterialType kindType = SpecimenMaterialType.BloodSerumAndPlasma;

            if (kind == Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_008)
            {
                kindType = SpecimenMaterialType.BloodSerumAndPlasma;
            }
            else if (kind == Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_009)
            {
                kindType = SpecimenMaterialType.Urine;
            }
            //else if ( kind == Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_010 )
            //{
            //    kindType = SpecimenMaterialType.Other;
            //}
            else
            {
            }
            return kindType;
        }

        /// <summary>
        /// サンプル種別文字列取得
        /// </summary>
        /// <remarks>
        /// サンプル種別から、グリッド表示文字列を取得します。
        /// </remarks>
        /// <param name="kind">サンプル種別値</param>
        /// <returns>サンプル種別文字列</returns>
        static public String GetSampleKindGridItemString(SpecimenMaterialType kind)
        {
            String kindString = String.Empty;

            switch (kind)
            {
                case SpecimenMaterialType.BloodSerumAndPlasma:
                    kindString = Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_008;
                    break;
                case SpecimenMaterialType.Urine:
                    kindString = Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_009;
                    break;
                //case SpecimenMaterialType.Other:
                //    kindString = Oelco.CarisX.Properties.Resources.STRING_SPECIMENREGIST_010;
                //    break;
                default:
                    break;
            }

            return kindString;
        }

        /// <summary>
        /// 容器種別取得
        /// </summary>
        /// <remarks>
        /// 容器種別の、グリッド表示文字列から容器種別値を取得します。
        /// </remarks>
        /// <param name="kind">容器種別文字列</param>
        /// <returns>容器種別値</returns>
        static public SpecimenCupKind GetSpecimenCupKindFromGridItemString(String kind)
        {
            SpecimenCupKind kindType = SpecimenCupKind.Cup;

            if (kind == Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_018)
            {
                kindType = SpecimenCupKind.Cup;
            }
            else if (kind == Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_019)
            {
                kindType = SpecimenCupKind.Tube;
            }
            else if (kind == Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_020)
            {
                kindType = SpecimenCupKind.CupOnTube;
            }
            else
            {
            }
            return kindType;
        }

        /// <summary>
        /// 容器種別文字列取得
        /// </summary>
        /// <remarks>
        /// 容器種別から、グリッド表示文字列を取得します。
        /// </remarks>
        /// <param name="kind">容器種別値</param>
        /// <returns>容器種別文字列</returns>
        static public String GetSpecimenCupKindGridItemString(SpecimenCupKind kind)
        {
            String kindString = String.Empty;

            switch (kind)
            {
                case SpecimenCupKind.Cup:
                    kindString = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_018;
                    break;
                case SpecimenCupKind.Tube:
                    kindString = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_019;
                    break;
                case SpecimenCupKind.CupOnTube:
                    kindString = Oelco.CarisX.Properties.Resources.STRING_SPECIMENSTATREGIST_020;
                    break;
                default:
                    break;
            }

            return kindString;
        }

        /// <summary>
        /// ファイル保存ダイアログの表示
        /// </summary>
        /// <remarks>
        /// ファイル保存ダイアログを表示します。
        /// </remarks>
        /// <param name="selectFileName">ユーザー指定のファイル名(フルパス)</param>
        /// <param name="fileKind">ファイル種別(拡張子種別)</param>
        /// <param name="initFileName">ファイル名(パス、拡張子含まない)</param>
        /// <param name="summaryName">要約名(タイトルバー表示用)</param>
        /// <param name="setting">ファイル出力設定</param>
        /// <returns>ダイアログの戻り値</returns>
        static public DialogResult ShowSaveCSVFileDialog(out String selectFileName, OutputFileKind fileKind, String initFileName, String summaryName, IExportSettings setting)
        {
            selectFileName = null;
            try
            {
                DialogResult result = SubFunction.ShowSaveCSVFileDialog(out selectFileName, fileKind, setting.ExportPath, initFileName + DateTime.Now.ToString(CarisXConst.EXPORT_CSV_DATETIMEFORMAT), summaryName);

                if (result == DialogResult.OK)
                {
                    // UI設定として出力先ディレクトリを保存
                    setting.ExportPath = selectFileName.Substring(0, selectFileName.LastIndexOf('\\') + 1);
                }

                return result;
            }
            catch (Exception ex)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                return DialogResult.Cancel;
            }
        }

        /// <summary>
        /// ファイルを開くダイアログの表示
        /// </summary>
        /// <remarks>
        /// ファイルを開くダイアログを表示します。
        /// </remarks>
        /// <param name="selectFileName">選択されるファイル名</param>
        /// <param name="fileKind">ファイル種別</param>
        /// <param name="initFileName">初期値のファイル名</param>
        /// <param name="summaryName">ダイアログタイトル</param>
        /// <param name="setting">ファイル選択パス設定</param>
        /// <returns>ダイアログ戻り値</returns>
        static public DialogResult ShowOpenFileDialog(out String selectFileName, OutputFileKind fileKind, String initFileName, String summaryName, IExportSettings setting)
        {
            selectFileName = null;
            try
            {
                DialogResult result = SubFunction.ShowOpenFileDialog(out selectFileName, fileKind, setting.ExportPath, initFileName, summaryName);

                if (result == DialogResult.OK)
                {
                    // UI設定として出力先ディレクトリを保存
                    setting.ExportPath = selectFileName.Substring(0, selectFileName.LastIndexOf('\\') + 1);
                }

                return result;
            }
            catch (Exception ex)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                            CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                return DialogResult.Cancel;
            }
        }

        /// <summary>
        /// 実残量より表示残量を取得
        /// プレトリガ/トリガ：μL→テスト数
        /// 希釈液：μL→ml
        /// </summary>
        /// <remarks>
        /// 実残量より表示残量を取得します。
        /// </remarks>
        /// <param name="kind">試薬種別</param>
        /// <param name="remain">実残量</param>
        /// <returns>表示残量</returns>
        static public Int32 GetDispRemainCount(ReagentKind kind, Int32? remain)
        {
            Int32 DispenseVolume = 1;

            switch (kind)
            {
                case ReagentKind.Pretrigger:
                    DispenseVolume = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolPreTrig;
                    break;
                case ReagentKind.Trigger:
                    DispenseVolume = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolTrig;
                    break;
                case ReagentKind.Diluent:
                    DispenseVolume = 1000;  // μL⇒ml変換用
                    break;
            }
            if (remain.HasValue && DispenseVolume != 0)
            {
                return remain.Value / DispenseVolume;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 残量値より実残量を取得
        /// プレトリガ/トリガ：テスト数→μL
        /// 希釈液：ml→μL
        /// </summary>
        /// <remarks>
        /// 残量値(テスト数、またはml)を元に指定の試薬種別(プレトリガ、トリガ、希釈液)の実残量(μL)を返します。
        /// </remarks>
        /// <param name="kind">試薬種別</param>
        /// <param name="testCount">残量値</param>
        /// <returns>実残量(μL)</returns>
        static public Int32 GetRealRemainCount(ReagentKind kind, Int32? testCount)
        {
            Int32 DispenseVolume = 1;

            switch (kind)
            {
                case ReagentKind.Pretrigger:
                    DispenseVolume = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolPreTrig;
                    break;
                case ReagentKind.Trigger:
                    DispenseVolume = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.WashDispVolParameter.DispVolTrig;
                    break;
                case ReagentKind.Diluent:
                    DispenseVolume = 1000;  // ml⇒μL変換用
                    break;
            }
            if (testCount.HasValue)
            {
                return testCount.Value * DispenseVolume;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 濃度値桁数算出処理
        /// </summary>
        /// <remarks>
        /// 濃度値の桁数を整数・小数別に算出します。
        /// </remarks>
        /// <param name="measureProtocol">分析項目</param>
        /// <returns>1番目:整数桁数,2番目:小数桁数</returns>
        static public Tuple<Int32, Int32> GetDigitsConcentration(MeasureProtocol measureProtocol)
        {
            return new Tuple<Int32, Int32>(CarisXConst.RESULT_CONCENTRATION_DIGITS - measureProtocol.LengthAfterDemPoint, measureProtocol.LengthAfterDemPoint);
        }

        /// <summary>
        /// 濃度値入力コントロールMaskInput(NumericEditor用)
        /// </summary>
        /// <remarks>
        /// MaskInput設定用文字列を分析項目の濃度値小数点以下桁数を基に編集し、編集結果を返します。
        /// </remarks>
        /// <param name="protocolIndex">分析項目インデックス</param>
        /// <returns>MaskInput用文字列</returns>
        static public string GetConcNumericEditorMaskInput(Int32 protocolIndex)
        {
            String rtn = "";
            var insertIndexDecimalPoint = CarisXConst.CONC_REAL_NUMBER_DIGITS - Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(protocolIndex).LengthAfterDemPoint;
            var digits = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(protocolIndex).LengthAfterDemPoint;
            if (digits == 0)
            {
                rtn = new String('n', CarisXConst.CONC_REAL_NUMBER_DIGITS);
            }
            else
            {
                rtn = "{" + String.Format(@"double:{0}.{1}:c", insertIndexDecimalPoint, digits.ToString()) + "}";
            }
            return rtn;

        }

        /// <summary>
        /// 実残量より表示残量を取得
        /// 希釈液：μL→ml
        /// </summary>
        /// <remarks>
        /// 実残量より表示残量を取得します。
        /// </remarks>
        /// <param name="remain">実残量</param>
        /// <returns>表示残量(小数点切り上げ）</returns>
        static public String GetDispRemainDilCountWithDigit(Int32 remain)
        {
            const Int32 DilUnitConvertCoef = 1000;  // μL⇒ml変換係数
            const Int32 DilUnitNumberOfDigits = 0;
            Double value = (Double)(remain / (Decimal)DilUnitConvertCoef);
            return SubFunction.TruncateParse(Math.Ceiling(value), DilUnitNumberOfDigits);
        }

        // 検体登録時に試薬チェックを実施するための処理を、共通処理として追加する。

        // FormMainFrameにあるprivateメソッドを流用している。
        // こちらを修正するときにはFormMainFrameにも同様の修正をする必要がある。
        /// <summary>
        /// 残量問合せ
        /// </summary>
        /// <remarks>
        /// スレーブに残量確認を行います。
        /// </remarks>
        /// <returns>True:問合せ成功 False:問合せ失敗</returns>
        public static Boolean AskReagentRemain()
        {
            Boolean askSuccess = false;

            CarisXSequenceHelper.SequenceSyncObject syncData;
            Boolean SkipRack = false;

            if (Singleton<Oelco.CarisX.Comm.CarisXCommManager>.Instance.GetRackTransferCommStatus() != ConnectionStatus.Online)
            {
                //ラック搬送と繋がっていない場合
                syncData = new CarisXSequenceHelper.SequenceSyncObject();
                SkipRack = true;
            }
            else
            {
                //ラック搬送と繋がっている場合

                //ラックへの残量チェックコマンド送信
                syncData = Singleton<CarisXSequenceHelperManager>.Instance.RackTransfer.AskRackReagentRemain();
                while (!syncData.EndSequence.WaitOne(10))
                {
                    // ここをDoEventsでの待ちにしない場合、上位の処理をブロック単位に切り分けて複数段階での実行を行う事になり
                    // コード全体の見通しが悪くなる為使用する。メインスレッドをブロックして構わない場合この限りではない。
                    Application.DoEvents();
                }
                if (syncData.Status == CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success)
                {
                    SetRackReagentRemain(syncData.SequenceResultData as IRackRemainAmountInfoSet);
                    askSuccess = true;
                }
            }

            if (syncData.Status == CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success || SkipRack)
            {
                //スレーブへの残量チェックコマンド送信
                CarisXSequenceHelper.SequenceSyncObject[] syncDataWasteList = new CarisXSequenceHelper.SequenceSyncObject[Enum.GetValues(typeof(ModuleIndex)).Length];
                foreach (Int32 moduleindex in Enum.GetValues(typeof(ModuleIndex)))
                {
                    syncDataWasteList[moduleindex] = new CarisXSequenceHelper.SequenceSyncObject();
                }

                //先に各モジュールにコマンドを送信する
                foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
                {
                    if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                    {
                        //接続されているモジュールそれぞれに残量チェックコマンドを送信する
                        syncDataWasteList[moduleindex] = Singleton<CarisXSequenceHelperManager>.Instance.Slave[moduleindex].AskReagentRemain();
                    }
                }

                //各モジュールに送信しきってからレスポンスを待機する
                foreach (int moduleindex in Enum.GetValues(typeof(ModuleIndex)))
                {
                    if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(moduleindex) == ConnectionStatus.Online)
                    {
                        while (!syncDataWasteList[moduleindex].EndSequence.WaitOne(10))
                        {
                            // ここをDoEventsでの待ちにしない場合、上位の処理をブロック単位に切り分けて複数段階での実行を行う事になり
                            // コード全体の見通しが悪くなる為使用する。メインスレッドをブロックして構わない場合この限りではない。
                            Application.DoEvents();
                        }
                        if (syncDataWasteList[moduleindex].Status == CarisXSequenceHelper.SequenceSyncObject.SequenceStatus.Success)
                        {
                            //残量チェックの場合は履歴情報の残量を上書きしない
                            SetReagentRemain(syncDataWasteList[moduleindex].SequenceResultData as IRemainAmountInfoSet, false, ModuleIndexToModuleId((ModuleIndex)moduleindex), false);
                            askSuccess = true;
                        }
                    }
                }
            }

            return askSuccess;
        }

        /// <summary>
        /// 残量更新
        /// </summary>
        /// <param name="remain">残量情報</param>
        public static void SetReagentRemain(IRemainAmountInfoSet remain, Boolean overwrite, Int32 moduleId, Boolean errorNotify)
        {
            // 残量更新
            Singleton<ReagentHistoryDB>.Instance.SetReagentHistory(remain, overwrite, moduleId, errorNotify);
            Singleton<ReagentHistoryDB>.Instance.CommitData();
            Singleton<ReagentHistoryDB>.Instance.GetReagentHistory(ref remain);
            Singleton<ReagentDB>.Instance.SetReagentRemain(remain, moduleId);
            Singleton<ReagentDB>.Instance.CommitData();

            // 表示更新
            RealtimeDataAgent.LoadReagentRemainData();
        }

        /// <summary>
        /// ラック搬送の試薬情報を設定
        /// </summary>
        /// <param name="remain"></param>
        public static void SetRackReagentRemain(IRackRemainAmountInfoSet remain)
        {
            // 残量更新
            Singleton<ReagentDB>.Instance.SetRackReagentRemain(remain);
            Singleton<ReagentDB>.Instance.CommitData();

            // 表示更新
            RealtimeDataAgent.LoadReagentRemainData();
        }


        // FormMainFrameより移動
        /// <summary>
        /// 試薬残量確認
        /// </summary>
        /// <returns>True:後続処理の停止が必要なワーニング有り False:後続処理実施可</returns>
        /// <remarks>
        /// DBに登録されている分析項目で使用する試薬の残量を確認します。
        /// 廃液タンクが満杯もしくは設置なしの場合は、Assay時に後続処理を実施させない
        /// </remarks>
        static public Boolean ReagentRemainWarning()
        {
            Boolean rtnVal = false;

            Singleton<ReagentDB>.Instance.LoadDB();

            // 残量チェックコマンド
            // それぞれ残量が0であれば警告を出す

            // 登録内容から使用量を取得する
            AmountTable needAmount = HybridDataMediator.GetAllRegisterdSampleNeedAmount();

            List<String> errMessage = new List<String>();

            // 廃液タンク
            var wasteTankData = Singleton<ReagentDB>.Instance.GetData(ReagentKind.WasteTank, (Int32)RackModuleIndex.RackTransfer).FirstOrDefault();
            if (wasteTankData != null)
            {
                var tankStatus = Singleton<ReagentRemainStatusInfo>.Instance.GetWasteStatus(ReagentKind.WasteTank, (wasteTankData.Remain ?? 0), (wasteTankData.IsUse ?? false));
                if (tankStatus == WasteStatus.None || tankStatus == WasteStatus.Full)
                {
                    // 廃液タンクを交換してください
                    errMessage.Add(String.Format(Properties.Resources.STRING_DLG_MSG_255));
                    rtnVal = true;  //廃液タンクの交換が必要な場合、Assay開始できないようにする
                }
            }

            // 洗浄液タンク

            var washSolutionTankData = Singleton<ReagentRemainStatusInfo>.Instance
                .GetRemainStatus(ReagentKind.WashSolutionTank, (Int32)Singleton<PublicMemory>.Instance.WashSolutionTankStatus);
            if (washSolutionTankData == RemainStatus.Empty || washSolutionTankData == RemainStatus.Low)
            {
                // 洗浄液がありません
                errMessage.Add(String.Format(Properties.Resources.STRING_DLG_MSG_256));
            }

            //モジュール毎にチェック処理を行う
            foreach (Int32 moduleid in Enum.GetValues(typeof(RackModuleIndex)))
            {
                //ラックは対象外
                if (moduleid == (Int32)RackModuleIndex.RackTransfer)
                {
                    continue;
                }

                //モジュール１～４でも繋がっていない場合は
                if (Singleton<CarisXCommManager>.Instance.GetSlaveCommStatus(ModuleIDToModuleIndex(moduleid)) != ConnectionStatus.Online)
                {
                    continue;
                }

                // add start 修正内容 : モーターエラー処理追加
                // モーターエラーのスレーブは処理を行わない
                if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[moduleid] == Status.SystemStatusKind.MotorError)
                {
                    continue;
                }
                // add end 修正内容 : モーターエラー処理追加

                // DB内容から各残量を取得する
                AmountTable remainAmount = HybridDataMediator.GetReagentDBAmount(moduleid);

                Int32 shortCount = 0;
                String shortCountDisp = String.Empty;
                shortCount = needAmount.PreTriggerAmountInfo.Amount - remainAmount.PreTriggerAmountInfo.Amount;
                if (shortCount > 0)
                {
                    shortCount = GetDispRemainCount(ReagentKind.Pretrigger, shortCount);
                    // プレトリガがありません
                    errMessage.Add(String.Format(Properties.Resources.STRING_DLG_MSG_214, moduleid, Properties.Resources.STRING_DLG_MSG_215, shortCount));
                }
                shortCount = needAmount.TriggerAmountInfo.Amount - remainAmount.TriggerAmountInfo.Amount;
                if (shortCount > 0)
                {
                    shortCount = GetDispRemainCount(ReagentKind.Trigger, shortCount);
                    // トリガがありません
                    errMessage.Add(String.Format(Properties.Resources.STRING_DLG_MSG_214, moduleid, Properties.Resources.STRING_DLG_MSG_216, shortCount));
                }
                shortCount = needAmount.SampleTipAmountInfo.Amount - remainAmount.SampleTipAmountInfo.Amount;
                if (shortCount > 0)
                {
                    // サンプリングチップ＆セルがありません
                    errMessage.Add(String.Format(Properties.Resources.STRING_DLG_MSG_214, moduleid, Properties.Resources.STRING_DLG_MSG_217, shortCount));
                }
                shortCount = needAmount.DilutionAmountInfo.Amount - remainAmount.DilutionAmountInfo.Amount;
                if (shortCount > 0)
                {
                    shortCountDisp = GetDispRemainDilCountWithDigit(shortCount);
                    // 希釈液がありません
                    errMessage.Add(String.Format(Properties.Resources.STRING_DLG_MSG_214, moduleid, Properties.Resources.STRING_DLG_MSG_219, shortCountDisp));
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<CarisXUserLevelManager>.Instance.NowUserID
                        , CarisXLogInfoBaseExtention.Empty, String.Format("希釈液不足量詳細 = {0}μl", shortCount));
                }

                // 洗浄液バッファ
                Int32 washRemain = Singleton<ReagentDB>.Instance.GetData(ReagentKind.WashSolutionBuffer, moduleid).First().Remain ?? 0;

                // 洗浄液残量が2500以下なら警告（センサー有無を問わない）
                if (washRemain <= CarisXConst.WASH_SOLUTION_WARNING_REMAIN)
                {
                    // 洗浄液がありません
                    errMessage.Add(String.Format(Properties.Resources.STRING_DLG_MSG_110, moduleid));
                }

                // 廃液バッファ
                var wasteBuffer = (WasteStatus)(Singleton<ReagentDB>.Instance.GetData(ReagentKind.WasteBuffer, moduleid).First().Remain);
                if (wasteBuffer == WasteStatus.None || wasteBuffer == WasteStatus.Full)
                {
                    // 廃液バッファを交換してください
                    errMessage.Add(String.Format(Properties.Resources.STRING_DLG_MSG_202, moduleid));
                }

                // 廃棄ボックス
                var wasteBox = (WasteBoxViewStatus)(Singleton<ReagentDB>.Instance.GetData(ReagentKind.WasteBox, moduleid).First().Remain);
                if (wasteBox != WasteBoxViewStatus.NotFull)
                {
                    // 廃棄ボックスを交換してください
                    errMessage.Add(String.Format(Properties.Resources.STRING_DLG_MSG_203, moduleid));
                }

                // 試薬チェック
                foreach (var needReagent in needAmount.ReagentAmountInfo)
                {
                    // 試薬コードに対して、各種分析プロトコルで使用する分量を順次残量から減算していき、最初に不足が発生して以降のテスト数合計が不足数となる。
                    if (remainAmount.ReagentAmountInfo.ContainsKey(needReagent.Key))
                    {
                        Int32 shortTestCount = 0;
                        var totalRemainAmount = remainAmount.ReagentAmountInfo[needReagent.Key];
                        foreach (var useDetail in (needReagent.Value as ReagentAmountInfoWithProtocol).UseVolumeList) // 必要試薬量情報からのReagentAmountInfoWithProtocol変換は必ず成功する。
                        {
                            for (Int32 i = 0; i < useDetail.TestCount; i++)
                            {
                                totalRemainAmount.R1Reagent -= useDetail.R1DispenceVolume;
                                totalRemainAmount.R2Reagent -= useDetail.R2DispenceVolume;
                                totalRemainAmount.MReagent -= useDetail.MDispenceVolume;
                                if ((totalRemainAmount.R1Reagent < 0) || (totalRemainAmount.R2Reagent < 0) || (totalRemainAmount.MReagent < 0))
                                {
                                    shortTestCount++;
                                }
                            }
                        }
                        if (shortTestCount != 0)
                        {
                            var protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(needAmount.ReagentAmountInfo[needReagent.Key].ProtocolIndex);                        // 試薬名 不足数 x
                            errMessage.Add(String.Format(Properties.Resources.STRING_DLG_MSG_214, moduleid, protocol.ReagentName, shortTestCount));
                        }
                    }
                    else
                    {
                        // 試薬未設置
                        var reagentName = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(needReagent.Value.ProtocolIndex).ReagentName;
                        errMessage.Add(String.Format(Properties.Resources.STRING_DLG_MSG_204, moduleid, reagentName));
                    }
                }
            }

            // エラーメッセージ表示
            if (errMessage.Count > 0)
            {
                Oelco.CarisX.GUI.DlgShortReagentView.Show(errMessage);
            }

            return rtnVal;
        }

        /// <summary>
        /// ラック搬送側モーター番号リスト取得
        /// </summary>
        /// <returns>モーター番号リスト</returns>
        static public List<MotorNoList> GetMotorNoListForRackTransfer()
        {
            // モーター番号リスト生成
            List<MotorNoList> motorNoList = new List<MotorNoList>()
            {
                MotorNoList.RackTransferSendingXAxisM1,                  // 211
                MotorNoList.RackTransferBackXAxisM1,                     // 212
                MotorNoList.RackPullinYAxisM1,                           // 213
                MotorNoList.RackTransferSendingXAxisM2,                  // 221
                MotorNoList.RackTransferBackXAxisM2,                     // 222
                MotorNoList.RackPullinYAxisM2,                           // 223
                MotorNoList.RackTransferSendingXAxisM3,                  // 231
                MotorNoList.RackTransferBackXAxisM3,                     // 232
                MotorNoList.RackPullinYAxisM3,                           // 233
                MotorNoList.RackTransferSendingXAxisM4,                  // 241
                MotorNoList.RackTransferBackXAxisM4,                     // 242
                MotorNoList.RackPullinYAxisM4,                           // 243
                MotorNoList.RackSetLoadYAxis,                            // 101
                MotorNoList.RackSetUnLoadYAxis,                          // 102
                MotorNoList.RackSetTakeOutYAxis,                         // 103
                MotorNoList.RackSetLoadFeederXAxis,                      // 104
                MotorNoList.RackSetUnLoadFeederXAxis,                    // 105
                MotorNoList.RackSetSliderXAxis,                          // 106
            };

            return motorNoList;
        }

        /// <summary>
        /// モジュール用モーター番号リスト取得
        /// </summary>
        /// <returns>モーター番号リスト</returns>
        static public List<MotorNoList> GetMotorNoListForModule()
        {
            // モーター番号リスト生成
            List<MotorNoList> motorNoList = new List<MotorNoList>()
            {
                MotorNoList.CaseTransferYAxis,                          // 4
                MotorNoList.CaseTransferZAxis,                          // 5
                MotorNoList.ReagentStorageTableThetaAxis,               // 6
                MotorNoList.ReagentStorageMixingThetaAxis,              // 7
                MotorNoList.STATYAxis,                                  // 8
                MotorNoList.SampleDispenseArmYAxis,                     // 9
                MotorNoList.SampleDispenseArmZAxis,                     // 10
                MotorNoList.SampleDispenseArmThetaAxis,                 // 11
                MotorNoList.SampleDispenseSyringe,                      // 12
                MotorNoList.ReactionCellTransferXAxis,                  // 13            
                MotorNoList.ReactionCellTransferZAxis,                  // 14
                MotorNoList.ReactionTableThetaAxis,                     // 15
                MotorNoList.BFTableThetaAxis,                           // 16
                MotorNoList.ReactionTableR1MixingZThetaAxis,            // 17
                MotorNoList.BFTableR2MixingZThetaAxis,                  // 18
                MotorNoList.BFTableBF1MixingZThetaAxis,                 // 19
                MotorNoList.BFTableBF2MixingZThetaAxis,                 // 20
                MotorNoList.BFTablePreTriggerMixingZThetaAxis,          // 21
                MotorNoList.TravelerXAxis,                              // 22
                MotorNoList.TravelerZAxis,                              // 23
                MotorNoList.R1DispenseArmThetaAxis,                     // 24
                MotorNoList.R1DispenseArmZAxis,                         // 25
                MotorNoList.R2DispenseArmThetaAxis,                     // 26
                MotorNoList.R2DispenseArmZAxis,                         // 27
                MotorNoList.BF1NozzleZAxis,                             // 28
                MotorNoList.BF1WasteNozzleZAxis,                        // 29
                MotorNoList.BF2NozzleZAxis,                             // 30
                MotorNoList.DiluentDispenseArmZAxis,                    // 31
                MotorNoList.TriggerAndPreTriggerDispenseNozzleZAxis,    // 32
                MotorNoList.DiluentDispenseSyringe,                     // 33
                MotorNoList.R1DispenseSyringe,                          // 34
                MotorNoList.R2DispenseSyringe,                          // 35
                MotorNoList.BFWashSyringe,                              // 36
                MotorNoList.PreTriggerDispenseSyringe,                  // 37
                MotorNoList.TriggerDispenseSyringe,                     // 38
            };

            return motorNoList;
        }

        /// <summary>
        /// 装置コードからモジュールIDへの変換
        /// </summary>
        /// <param name="code">装置コード</param>
        /// <returns>モジュールID</returns>
        static public int MachineCodeToRackModuleIndex(MachineCode code)
        {
            int moduleId = 0;

            // ユーザーアプリ
            if (MachineCode.PC <= code && MachineCode.Slave > code)
            {
                moduleId = code - MachineCode.PC;
            }
            // スレーブ
            else if (MachineCode.Slave <= code && MachineCode.Host > code)
            {
                moduleId = (code - MachineCode.Slave) + (int)RackModuleIndex.Module1;
            }
            // ホスト
            else if (MachineCode.Host <= code && MachineCode.RackTransfer > code)
            {
                moduleId = code - MachineCode.Host;
            }
            // ラック搬送
            else if (MachineCode.RackTransfer <= code)
            {
                moduleId = (code - MachineCode.RackTransfer) + (int)RackModuleIndex.RackTransfer;
            }

            return moduleId;
        }

        /// <summary>
        /// モジュールIDからモジュールIndexへの変換
        /// </summary>
        /// <param name="moduleId">モジュールID</param>
        /// <returns>モジュールIndex</returns>
        static public int ModuleIDToModuleIndex(Int32 moduleId)
        {
            int moduleIdx = 0;

            switch (moduleId)
            {
                case (int)RackModuleIndex.RackTransfer:
                    moduleIdx = moduleId;
                    break;
                case (int)RackModuleIndex.Module1:
                    moduleIdx = (int)ModuleIndex.Module1;
                    break;
                case (int)RackModuleIndex.Module2:
                    moduleIdx = (int)ModuleIndex.Module2;
                    break;
                case (int)RackModuleIndex.Module3:
                    moduleIdx = (int)ModuleIndex.Module3;
                    break;
                case (int)RackModuleIndex.Module4:
                    moduleIdx = (int)ModuleIndex.Module4;
                    break;
                default:
                    //ユーザーの場合はmoduleIdに-1が設定されるので、0としてモジュール１に渡すようにする
                    break;
            }

            return moduleIdx;
        }

        /// <summary>
        /// 装置コードからモジュールIndexへの変換
        /// </summary>
        /// <param name="code">装置コード</param>
        /// <returns>モジュールIndex</returns>
        static public int MachineCodeToModuleIndex(MachineCode code)
        {
            int moduleID = MachineCodeToRackModuleIndex(code);
            return ModuleIDToModuleIndex(moduleID);
        }

        /// <summary>
        /// モジュールIndexからモジュールID(RackModuleIndex)への変換
        /// </summary>
        /// <param name="code">装置コード</param>
        /// <returns>モジュールIndex</returns>
        static public int ModuleIndexToModuleId(ModuleIndex moduleIndex)
        {
            return (Int32)moduleIndex + (Int32)RackModuleIndex.Module1;
        }

        /// <summary>
        /// モジュールIDからモジュール名への変換
        /// </summary>
        /// <param name="moduleId">モジュールID</param>
        /// <returns>モジュール名</returns>
        static public String ModuleIdToModuleName(Int32 moduleId)
        {
            String moduleName = String.Empty;

            switch ((RackModuleIndex)moduleId)
            {
                case RackModuleIndex.RackTransfer:
                    moduleName = Oelco.CarisX.Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_024;
                    break;

                case RackModuleIndex.Module1:
                    moduleName = Oelco.CarisX.Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_025;
                    break;

                case RackModuleIndex.Module2:
                    moduleName = Oelco.CarisX.Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_026;
                    break;

                case RackModuleIndex.Module3:
                    moduleName = Oelco.CarisX.Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_027;
                    break;

                case RackModuleIndex.Module4:
                    moduleName = Oelco.CarisX.Properties.Resources_Maintenance.STRING_MAINTENANCE_MAIN_028;
                    break;

                default:
                    break;
            }

            return moduleName;
        }

        /// <summary>
        /// 【IssuesNo:16】ModuleID转换成IoT云端ID
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        static public long ModuleIdToIoTNo(Int32 moduleId = CarisXConst.ALL_MODULEID)
        {
            long iotNo = 0;
            switch ((RackModuleIndex)moduleId)
            {
                case RackModuleIndex.Module1:
                    iotNo = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave1No;
                    break;
                case RackModuleIndex.Module2:
                    iotNo = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave2No;
                    break;
                case RackModuleIndex.Module3:
                    iotNo = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave3No;
                    break;
                case RackModuleIndex.Module4:
                    iotNo = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave4No;
                    break;
                default:
                    iotNo = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave1No;
                    break;
            }
            return iotNo;
        }

        /// <summary>
        /// MACアドレス取得
        /// </summary>
        /// <remarks>
        /// 有効なインターフェースかつネットワークタイプがEthernetのものを返す
        /// </remarks>
        /// <returns>MACアドレス文字列</returns>
        static public String GetLocalMacAddress()
        {
            String macaddress = String.Empty;

            var macaddressList = new List<PhysicalAddress>();

            // 物理インターフェース情報をすべて取得
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();

            // 各インターフェースごとの情報を調べる
            foreach (var adapter in interfaces)
            {
                // 有効なインターフェースのみを対象とする
                if ((adapter.OperationalStatus != OperationalStatus.Up)
                    && (adapter.NetworkInterfaceType != NetworkInterfaceType.Ethernet))
                {
                    continue;
                }

                // MACアドレス
                macaddressList.Add(adapter.GetPhysicalAddress());
            }

            if (macaddressList.Count > 0)
            {
                macaddress = macaddressList[0].ToString();
            }

            return macaddress;
        }

        /// <summary>
        /// ビットマップの拡縮
        /// </summary>
        /// <remarks>
        /// imgをimageRatioの比率で拡縮を行う
        /// </remarks>
        /// <param name="img">拡縮する画像</param>
        /// <param name="imageRatio">拡縮率</param>
        /// <returns>scalingBmp:拡縮されたビットマップ</returns>
        static public Bitmap ScalingBitmap(Image img, double imageRatio)
        {
            Bitmap tempBmp = new Bitmap(img);
            int w = (int)(img.Width * imageRatio);
            int h = (int)(img.Height * imageRatio);

            // 拡縮するグラフを格納するビットマップ
            Bitmap scalingBmp = new Bitmap(w, h);

            // 拡縮したグラフを入れるキャンバスの作成
            Graphics g = Graphics.FromImage(scalingBmp);

            // 補間方法を指定し、拡縮したグラフをキャンバスに描く
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(tempBmp, 0, 0, w, h);

            tempBmp.Dispose();
            g.Dispose();

            return scalingBmp;
        }

        /// <summary>
        /// ターンテーブル上の試薬名を取得
        /// </summary>
        /// <returns></returns>
        static public Dictionary<Int32, List<String>> GetReagentNamesInTurnTable()
        {
            Dictionary<Int32, List<String>> resultDicReagentDatas = new Dictionary<Int32, List<String>>();

            // 接続台数取得
            Int32 connectModuleCount = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

            // 接続するモジュールの試薬状況を取得
            for (Int32 moduleIndex = 0; moduleIndex < connectModuleCount; moduleIndex++)
            {
                // 表示する試薬データを取得
                var singletonInstanceGetData = Singleton<ReagentDB>.Instance.GetData(moduleId: CarisXSubFunction.ModuleIndexToModuleId((ModuleIndex)moduleIndex));
                Func<int, String> reagentCodeToProtocolName = (reagentCode) =>
                {
                    MeasureProtocol measureProtocol = Singleton<MeasureProtocolManager>.Instance.MeasureProtocolList.FirstOrDefault((protocol) => protocol.ReagentCode == reagentCode);
                    if (measureProtocol != null)
                    {
                        return measureProtocol.ProtocolName;
                    }
                    else
                    {
                        return String.Empty;
                    }
                };

                // 表示用にポート単位での表示に変換（※マッチング処理等も実施）
                var reagentDatas = (from grp in
                                        (from data in singletonInstanceGetData
                                         where data.ReagentKind == (Int32)ReagentKind.Reagent
                                         orderby data.PortNo
                                         group data by (Int32)((data.PortNo - 1) / 3))
                                    let analytes = grp.All((reagentdata) => reagentdata.ReagentCode == grp.First().ReagentCode) ? reagentCodeToProtocolName(grp.First().ReagentCode.Value) : null
                                    let protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName(analytes)
                                    let MultiLotNo = !grp.All((regDat) => regDat.LotNo == grp.First().LotNo)
                                    let MultiMakerCd = !grp.All((regDat) => regDat.MakerCode == grp.First().MakerCode)
                                    let CombinationMandR = grp.All((regDat) => (regDat.ReagentType == (Int32)ReagentType.M || regDat.ReagentType == (Int32)ReagentType.R1R2))
                                    select new
                                    {
                                        Data = new
                                        {
                                            PortNo = grp.Key + 1,
                                            Analytes = (!MultiLotNo && !MultiMakerCd && CombinationMandR) ? analytes : string.Empty,
                                            LotNo = (!MultiLotNo && !MultiMakerCd && CombinationMandR && !String.IsNullOrEmpty(analytes) && grp.First().LotNo != null) ? grp.First().LotNo : String.Empty,
                                            ExpirationDate = (!MultiLotNo && !MultiMakerCd && CombinationMandR && !String.IsNullOrEmpty(analytes) && grp.First().ExpirationDate.HasValue) ? grp.First().ExpirationDate.Value.ToShortDateString() : String.Empty,
                                            StabilityDate = (!MultiLotNo && !MultiMakerCd && CombinationMandR && !String.IsNullOrEmpty(analytes) && grp.First().StabilityDate.HasValue) ? grp.First().StabilityDate.Value.ToShortDateString() : String.Empty,
                                            // 残量（ul→テストに変換する。分注量＝0の場合は計算できないので空文字とする）
                                            Remain = (!MultiLotNo && !MultiMakerCd && CombinationMandR && !String.IsNullOrEmpty(analytes)
                                                && protocol != null && protocol.R2DispenseVolume > 0 && protocol.MReagDispenseVolume > 0)
                                                ? (protocol.R1DispenseVolume > 0
                                                    ? new[] { grp.First((data) => data.PortNo - 1 == grp.Key * 3).Remain / protocol.R1DispenseVolume
                                                    , grp.First((data) => data.PortNo - 1 == grp.Key * 3 + 1).Remain / protocol.R2DispenseVolume
                                                    , grp.First((data) => data.PortNo - 1 == grp.Key * 3 + 2).Remain / protocol.MReagDispenseVolume }.Min().ToString()
                                                    : new[] { grp.First((data) => data.PortNo - 1 == grp.Key * 3 + 1).Remain / protocol.R2DispenseVolume
                                                    , grp.First((data) => data.PortNo - 1 == grp.Key * 3 + 2).Remain / protocol.MReagDispenseVolume }.Min().ToString())
                                                    : String.Empty
                                        },
                                        ExirationData = grp.First().ExpirationDate
                                    }).ToList();

                //対象データが無くてもひとまず行は作成する
                resultDicReagentDatas.Add(moduleIndex, new List<String>());

                foreach (var reagentData in reagentDatas)
                {
                    // 分析項目名が取得できたデータのみ対象とする
                    if (reagentData.Data.Analytes != String.Empty)
                    {
                        // 必要な情報だけ詰め直し
                        resultDicReagentDatas[moduleIndex].Add(reagentData.Data.Analytes);
                    }
                }
            }

            return resultDicReagentDatas;
        }

        /// <summary>
        /// 重複している試薬を取得
        /// </summary>
        /// <param name="moduleIndex">処理対象のモジュールインデックス（全モジュール対象時は-1）</param>
        /// <param name="reagentDatas">全モジュールの試薬搭載情報</param>
        /// <returns>重複している試薬名のリスト</returns>
        static public List<String> GetDuplicationAnalytes(Int32 moduleIndex, Dictionary<Int32, List<String>> reagentDatas)
        {
            // 重複している分析項目リストを生成
            List<String> rtnVal = new List<String>();

            // モジュール番号を設定（初期値=モジュール1）
            Int32 targetModuleIndex = 0;

            // 最大カウントに接続台数を設定
            Int32 maxCount = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

            // ラック移動方式を取得
            RackMovementMethodKind rackMovementMethod = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackMovementMethodParameter.RackMovementMethod;

            // ラック移動方式によって処理分岐
            switch (rackMovementMethod)
            {
                // パフォーマンス時
                case RackMovementMethodKind.Performance:

                    // 指定のモジュールのみチェックするため、モジュール番号と最大カウントを調整
                    if (moduleIndex != CarisXConst.ALL_MODULEID)
                    {
                        targetModuleIndex = moduleIndex;
                        maxCount = targetModuleIndex + 1;
                    }

                    // 重複する試薬をリストアップ
                    for (; targetModuleIndex < maxCount; targetModuleIndex++)
                    {
                        // 試薬データがある場合、重複チェックを行う
                        if (reagentDatas.ContainsKey(targetModuleIndex) && reagentDatas[targetModuleIndex].Count() > 0)
                        {
                            // 試薬データを試薬名でグループ化
                            var reagentGroups = reagentDatas[targetModuleIndex].GroupBy(analytes => analytes);
                            foreach (var reagentGroup in reagentGroups)
                            {
                                // グループ内に複数データが存在する => 重複している
                                if ((reagentGroup != null) && (reagentGroup.Count() > 1))
                                {
                                    // 重複リストに追加されていない場合
                                    if (rtnVal.Contains(reagentGroup.Key) == false)
                                    {
                                        // 重複リストに追加
                                        rtnVal.Add(reagentGroup.Key);
                                    }
                                }
                            }
                        }
                    }

                    break;

                // コストモード時
                case RackMovementMethodKind.Cost:

                    // 指定のモジュールのみチェックするため、モジュール番号を調整（最大カウントは変えない）
                    if (moduleIndex != CarisXConst.ALL_MODULEID)
                    {
                        targetModuleIndex = moduleIndex;
                    }

                    // ターゲットとする試薬データを生成
                    List<String> targetReagentData = new List<String>();

                    // チェック済みリストを生成
                    List<String> alreadyCheckAnalytesList = new List<String>();

                    // 全モジュールチェック
                    for (; targetModuleIndex < maxCount; targetModuleIndex++)
                    {
                        // ターゲット変更
                        targetReagentData = reagentDatas[targetModuleIndex];
                        foreach (String targetAnalytes in targetReagentData)
                        {
                            // 重複リストに追加済みの場合は対象外
                            if (rtnVal.Contains(targetAnalytes))
                            {
                                continue;
                            }

                            // チェック済みリストに追加済みの場合は対象外
                            if (alreadyCheckAnalytesList.Contains(targetAnalytes))
                            {
                                continue;
                            }

                            // 重複チェック
                            Boolean isDuplicaton = false;
                            for (Int32 checkIndex = 0; checkIndex < maxCount; checkIndex++)
                            {
                                // ターゲットと同じモジュール番号は対象外
                                if (checkIndex == targetModuleIndex)
                                {
                                    continue;
                                }

                                // チェックするモジュールに重複する試薬がないかチェック
                                if (reagentDatas[checkIndex].Contains(targetAnalytes))
                                {
                                    rtnVal.Add(targetAnalytes);
                                    isDuplicaton = true;
                                    break;
                                }
                            }

                            // 重複がなければ、チェック済みリストに追加
                            if (isDuplicaton == false)
                            {
                                alreadyCheckAnalytesList.Add(targetAnalytes);
                            }
                        }
                    }
                    break;

                default:
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// STAT測定可能モジュールの確認ダイアログを表示
        /// </summary>
        /// <param name="PatientID">患者ID（検体ID）</param>
        /// <param name="protocolList">測定プロトコルリスト</param>
        static public void ShowCheckStatMeasurableModule(String PatientID, List<Int32> protocolList)
        {
            List<Int32> moduleIndexList = new List<Int32>();

            //接続されているモジュールに対して処理する
            foreach (int moduleId in Singleton<SystemStatus>.Instance.GetConnectedModuleId())
            {
                //モジュールインデックスを退避
                Int32 moduleIndex = ModuleIDToModuleIndex(moduleId);

                //STAT状態が受付可の場合のみ処理する
                if (Singleton<SystemStatus>.Instance.ModuleSTATStatus[moduleIndex] == STATStatus.Acceptable)
                {
                    //測定するプロトコルの試薬が該当モジュールにあるかチェックする
                    foreach (Int32 protocolIndex in protocolList)
                    {
                        // 分析項目情報取得
                        var measProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(protocolIndex);

                        // 試薬があるか
                        String reagentLotNo = Singleton<ReagentDB>.Instance.GetNowReagentLotNo(measProtocol.ReagentCode, moduleId: moduleId);
                        if (reagentLotNo != String.Empty)
                        {
                            // STAT状態通知コマンドをスレーブへ送信
                            SlaveCommCommand_0491 cmd0491 = new SlaveCommCommand_0491();
                            cmd0491.Request = STATStatusRequest.WaitSWPress;                // SW押下待ち
                            Singleton<CarisXCommManager>.Instance.PushSendQueueSlave(moduleIndex, cmd0491);

                            // モジュールIndexリストへ登録
                            moduleIndexList.Add(moduleIndex);

                            //繰返しを抜ける
                            break;
                        }
                    }
                }
            }

            // STAT測定可能モジュールの確認ダイアログを表示
            using (DlgCheckStatMeasurableModule dlg = new DlgCheckStatMeasurableModule(PatientID, moduleIndexList))
            {
                dlg.ShowDialog();
            }
        }
        /// <summary>
        /// バックアップ処理
        /// </summary>
        /// <returns></returns>
        static public void Backup()
        {
            // バックアップのParamフォルダがない場合
            if (!System.IO.File.Exists(CarisXConst.PathBackupParam))
            {
                // バックアップParamフォルダを作成
                System.IO.Directory.CreateDirectory(CarisXConst.PathBackupParam);
            }
            // バックアップのSystemフォルダがない場合
            if (!System.IO.File.Exists(CarisXConst.PathBackupSystem))
            {
                // バックアップSsytemフォルダを作成
                System.IO.Directory.CreateDirectory(CarisXConst.PathBackupSystem);
            }
            // バックアップのProtocolフォルダがない場合
            if (!System.IO.File.Exists(CarisXConst.PathBackupProtocol))
            {
                // バックアップProtocolフォルダを作成
                System.IO.Directory.CreateDirectory(CarisXConst.PathBackupProtocol);
            }

            try
            {
                // UIPackage.xmlのコピー
                System.IO.File.Copy(Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SavePath,
                                    Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.BackupSavePath, true);
                // SensorParameter.xmlのコピー
                System.IO.File.Copy(Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.Param.SavePath,
                                    Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.Param.BackupSavePath, true);
                // MotorParameter.xmlのコピー
                System.IO.File.Copy(Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance.Param.SavePath,
                                    Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance.Param.BackupSavePath, true);
                // \ConfigParameter.xmlのコピー
                System.IO.File.Copy(Singleton<ParameterFilePreserve<CarisXConfigParameter>>.Instance.Param.SavePath,
                                    Singleton<ParameterFilePreserve<CarisXConfigParameter>>.Instance.Param.BackupSavePath, true);
                // SystemParameter.xmlのコピー
                System.IO.File.Copy(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SavePath,
                                    Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.BackupSavePath, true);
                // SupplieParam.xmlのコピー
                System.IO.File.Copy(Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.SavePath,
                                    Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.BackupSavePath, true);
                // Protocolフォルダのコピー(Protocolはフォルダ内すべてのxmlファイルをコピーするためフォルダごとコピーする)
                Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(CarisXConst.PathProtocol, CarisXConst.PathBackupProtocol, true);

            }
            catch (Exception ex)
            {
                // バックアップの作成に失敗しました。
                System.Diagnostics.Debug.WriteLine("バックアップの作成に失敗しました" + ex.Message);
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "バックアップの作成に失敗しました" + ex.StackTrace);
            }

        }

        /// <summary>
        /// バックアップデータを基データに上書き
        /// </summary>
        /// <returns></returns>
        static public void Restore()
        {
            /// バックアップデータの上書き実行フラグ
            /// true;上書き可能
            /// false;上書き不可
            bool existenceCheck = true;

            // バックアップのParamフォルダがない場合
            if (!System.IO.File.Exists(CarisXConst.PathBackupParam))
            {
                existenceCheck = false;
            }
            // バックアップのSsytemフォルダがない場合
            if (!System.IO.File.Exists(CarisXConst.PathBackupSystem))
            {
                existenceCheck = false;
            }
            // バックアップのProtocolフォルダがない場合
            if (!System.IO.File.Exists(CarisXConst.PathBackupProtocol))
            {
                existenceCheck = false;
            }

            // 各種バックアップフォルダがある場合
            if (existenceCheck)
            {
                /// 上書き成功フラグ
                /// true:成功
                /// false:失敗
                bool restoreSuccess = true;

                try
                {
                    // UIPackage.xmlのコピー
                    System.IO.File.Copy(Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.BackupSavePath,
                                        Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.SavePath, true);
                    // SensorParameter.xmlのコピー
                    System.IO.File.Copy(Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.Param.BackupSavePath,
                                        Singleton<ParameterFilePreserve<CarisXSensorParameter>>.Instance.Param.SavePath, true);
                    // MotorParameter.xmlのコピー
                    System.IO.File.Copy(Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance.Param.BackupSavePath,
                                        Singleton<ParameterFilePreserve<CarisXMotorParameter>>.Instance.Param.SavePath, true);
                    // \ConfigParameter.xmlのコピー
                    System.IO.File.Copy(Singleton<ParameterFilePreserve<CarisXConfigParameter>>.Instance.Param.BackupSavePath,
                                        Singleton<ParameterFilePreserve<CarisXConfigParameter>>.Instance.Param.SavePath, true);
                    // SystemParameter.xmlのコピー
                    System.IO.File.Copy(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.BackupSavePath,
                                        Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.SavePath, true);
                    // SupplieParam.xmlのコピー
                    System.IO.File.Copy(Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.BackupSavePath,
                                        Singleton<ParameterFilePreserve<SupplieParameter>>.Instance.Param.SavePath, true);

                    // Protocolフォルダのコピー(Protocolはフォルダ内すべてのxmlファイルをコピーするためフォルダごとコピーする)
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(CarisXConst.PathBackupProtocol, CarisXConst.PathProtocol, true);


                }
                catch (Exception ex)
                {
                    restoreSuccess = false;
                    // バックアップの上書きに失敗しました。
                    System.Diagnostics.Debug.WriteLine("バックアップの上書きに失敗しました" + ex.Message);
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                        , CarisXLogInfoBaseExtention.Empty, "バックアップの上書きに失敗しました" + ex.StackTrace);
                }

                // バックアップの上書きが成功した場合
                if (restoreSuccess)
                {
                    try
                    {
                        // バックアップフォルダの削除
                        System.IO.Directory.Delete(CarisXConst.PathBackup, true);
                    }
                    catch (Exception ex)
                    {
                        // バックアップフォルダの削除に失敗しました
                        System.Diagnostics.Debug.WriteLine("バックアップフォルダの削除に失敗しました" + ex.Message);
                        Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                            , CarisXLogInfoBaseExtention.Empty, "バックアップフォルダの削除に失敗しました" + ex.StackTrace);
                    }
                }

            }
        }

        /// <summary>
        /// エラーをフィルタリングするかの判定
        /// </summary>
        /// <param name="ErrorCode">エラーコード</param>
        /// <returns>
        /// true:フィルタリングする
        /// false:フィルタリングしない
        /// </returns>
        static public Boolean IsErrFiltering(Int32 ErrorCode)
        {
            // フィルタリングするかのフラグ
            // true:する
            // false:しない
            Boolean result = false;

            //　フィルタリングするエラーコードの判別
            switch (ErrorCode)
            {
                case 20: // 温度が正常範囲外
                case 21: // 温度が正常範囲外
                case 40: // ポンプがエラー
                case 41: // 廃液バッファが満杯
                case 72: // 希釈液がセットされていない
                case 100: // 試薬が不足
                case 101: // R試薬が不足
                case 102: // 希釈液が不足
                case 103: // プレトリガが不足
                case 104: // トリガが不足
                case 106: // サンプリングチップが不足
                case 107: // 反応容器が不足
                case 108: // 廃液タンクがセットされていない
                case 109: // 廃液タンクが満杯
                case 110: // 廃棄ボックスがセットされていない
                case 111: // 廃棄ボックスが満杯
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }

        /// <summary>
        /// エラーコードと引数からエラーレベルを取得する 
        /// </summary>
        /// <param name="errorCode">エラーコード</param>
        /// <param name="errorArg">引数</param>
        /// <returns></returns>
        static public ErrorLevelKind GetErrorLevel(Int32 errorCode, Int32 errorArg)
        {
            ErrorLevelKind resultErrorLevel = ErrorLevelKind.Error;

            switch (errorCode)
            {
                case 1: // タイムアウト
                case 2: // PPMCにエラーが発生
                case 3: // モーターがスリップ
                case 17: // チューブ有無検出センサーが故障
                case 32: // 測定用シャッタの動作が正しくない
                case 40: // ポンプがエラー
                case 41: // 廃液バッファが満杯
                case 45: // 洗浄液周り
                case 50: // DP-ホスト間で通信エラーが発生
                case 60: // コマンドエラーが発生
                case 61: // システムエラーが発生
                case 62: // サイクルタイムオーバーエラーが発生
                case 80: // ケースを架設部に戻せない
                case 84: // 搬送の失敗
                case 85: // 試薬保冷庫のペルチェが故障
                case 88: // ラックの搬送に失敗
                    // エラーレベルをエラーに変更
                    resultErrorLevel = ErrorLevelKind.Error;
                    break;
                case 10: // M試薬バーコードIDの読み取りに失敗
                case 11: // R試薬バーコードIDの読み取りに失敗
                case 12: // サンプルバーコードIDの読み取りに失敗
                case 13: // ラックバーコードIDの読み取りに失敗
                case 14: // 希釈液バーコードIDの読み取りに失敗
                case 15: // プレトリガバーコードIDの読み取りに失敗
                case 16: // トリガバーコードIDの読み取りに失敗
                case 20: // 温度が正常範囲外
                case 21: // 試薬保冷庫の温度が正常範囲外
                case 30: // ダークエラー
                case 31: // 測光エラー
                case 33: // 測定検出器がエラー
                case 34: // キャリブレーションデータエラー
                case 35: // ダイナミックレンジエラー
                case 36: // 算出不能エラー
                case 42: // 洗浄液の分注に失敗
                case 43: // 洗浄液の分注に失敗
                case 52: // 通信エラーが発生
                case 53: // 通信エラーが発生
                case 54: // タイムアウトエラーが発生
                case 55: // タイムアウトエラーが発生
                case 75: // 吸引エラーが発生
                case 82: // ケースの検出の失敗
                case 86: // ラックの排出に失敗
                case 87: // ラックが満杯
                case 108: // 廃液タンクがセットされていない
                case 109: // 廃液タンクが満杯
                case 110: // 廃棄ボックスがセットされていない
                case 111: // 廃棄ボックスが満杯
                case 117: // 希釈液ボトルがセットされていない
                case 118: // プレトリガボトルがセットされていない
                case 119: // トリガボトルがセットされていない
                case 120: // 廃液タンクが接続されていない
                case 124: // 泡を検出
                case 125: // 試薬ノズルが衝突
                case 200: // マッチしていない
                    // エラーレベルを警告に変更
                    resultErrorLevel = ErrorLevelKind.Warning;
                    break;
                case 70: // M試薬がセットされていない
                case 71: // R試薬がセットされていない
                case 72: // 希釈液がセットされていない
                case 73: // 前処理液がセットされていない
                case 89: // 同一IDのラックが装置内にある
                case 90: // 
                case 100: // M試薬が不足
                case 101: // R試薬が不足
                case 102: // 希釈液が不足
                case 103: // プレトリガが不足
                case 104: // トリガが不足
                case 105: // 洗浄液が不足
                case 106: // サンプリングチップが不足
                case 107: // 反応容器が不足
                case 113: // 薬保冷庫のカバーが開いたまま
                case 114: // 試薬保冷庫のカバーが開いたまま
                case 115: // ケース架設部の扉が開いたまま
                case 116: // ケース架設部の扉が開いたまま
                case 121: // 試薬ロットの切り替わりを検出
                case 122: // リンス液が不足
                case 123: // ラックカバーが開いたまま
                case 201: // プライムの切替
                    // エラーレベルを注意に変更
                    resultErrorLevel = ErrorLevelKind.Hint;
                    break;
                case 37:
                    if (errorArg == 1)
                    {
                        // エラーレベルを警告に変更
                        resultErrorLevel = ErrorLevelKind.Warning;
                    }
                    else
                    {
                        // エラーレベルを注意に変更
                        resultErrorLevel = ErrorLevelKind.Hint;
                    }
                    break;
                case 51:
                    if (errorArg == 1)
                    {
                        // エラーレベルをエラーに変更
                        resultErrorLevel = ErrorLevelKind.Error;
                    }
                    else
                    {
                        // エラーレベルを警告に変更
                        resultErrorLevel = ErrorLevelKind.Warning;
                    }
                    break;
                case 74:
                    if ((errorArg == 5) || (errorArg == 9))
                    {
                        // エラーレベルをエラーに変更
                        resultErrorLevel = ErrorLevelKind.Error;
                    }
                    else
                    {
                        // エラーレベルを警告に変更
                        resultErrorLevel = ErrorLevelKind.Warning;
                    }
                    break;
            }
            return resultErrorLevel;
        }

        /// <summary>
        /// 文字列から16進数を抽出
        /// </summary>
        /// <param name="outData">抽出値</param>
        /// <param name="len">長さ</param>
        /// <param name="targetString">対象文字列</param>
        /// <param name="currentPos"></param>
        /// <returns></returns>
        static public Boolean SpoilHex(out Int64 outData, Int32 len, String targetString, Int32 currentPos)
        {
            if (targetString.Length < (currentPos + len))
            {
                outData = 0;
                return false;
            }
            String strOut = targetString.Substring(currentPos, len);
            currentPos += len;

            bool result = true;
            try
            {
                outData = Convert.ToInt64(strOut, 16);
            }
            catch
            {
                outData = 0;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 分析処理を行うかどうか判定
        /// </summary>
        /// <remarks>
        /// 下記の場合、分析を行わない
        /// ・ラックがモーターエラーの場合  ・全てのモジュールがモーターエラーの場合
        /// </remarks>
        public static bool CheckMotorErrorAllRackModule()
        {
            // モーターエラーによる分析不可ダイアログ表示可否フラグ（初期値：表示しない）
            Boolean isShowMotorErrorDlg = false;

            // ラック搬送がモーターエラーの場合
            if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.RackTransfer] == Status.SystemStatusKind.MotorError)
            {
                // ダイアログ表示可否フラグをONにする
                isShowMotorErrorDlg = true;
            }

            // 全てのモジュールがモーターエラーの場合
            // スレーブ接続台数
            int numOfConnectedCount = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;

            // チェック対象となるモジュール数
            int targetModuleCount = 0;

            // モーターエラーモジュール数
            int motorErrorModuleCount = 0;

            // 接続されているモジュールの状態チェック
            for (int rackModuleIdx = 1; rackModuleIdx <= numOfConnectedCount; rackModuleIdx++)
            {
                // 未接続以外の場合
                if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[rackModuleIdx] != SystemStatusKind.NoLink)
                {
                    // チェック対象モジュール数をカウントアップ
                    targetModuleCount += 1;
                }

                // モーターエラーの場合
                if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[rackModuleIdx] == SystemStatusKind.MotorError)
                {
                    // モーターエラーモジュール数をカウントアップ
                    motorErrorModuleCount += 1;
                }
            }

            // チェック対象のすべてのモジュールがモーターエラーの場合
            if ((motorErrorModuleCount != 0)
             && (targetModuleCount == motorErrorModuleCount))
            {
                // ダイアログ表示可否フラグをONにする
                isShowMotorErrorDlg = true;
            }

            // ダイアログ表示が必要かチェック
            if (isShowMotorErrorDlg)
            {
                // モーターエラーとなっているモジュール名の取得
                string rackModuleIndices = CarisXSubFunction.GetMotorErrorRackModule();

                // モーターエラーによる分析不可ダイアログを表示
                DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_267, rackModuleIndices, CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.Confirm);
            }

            return isShowMotorErrorDlg;
        }

        /// <summary>
        /// シーケンス番号のシーケンス番号終了値の変更
        /// </summary>
        static public void SequenceEndCountChange()
        {
            // 検体種別ごとにシーケンス番号開始値を持つリスト
            Dictionary<SampleKind, Int32> sequenceDataDic = new Dictionary<SampleKind, int>()
            {
                // 検体種別ごとにシーケンス番号開始値を設定
                { SampleKind.Sample, Singleton<SequencialSampleNo>.Instance.StartCount },
                { SampleKind.Priority, Singleton<SequencialPrioritySampleNo>.Instance.StartCount },
                { SampleKind.Control, Singleton<SequencialControlNo>.Instance.StartCount },
                { SampleKind.Calibrator, Singleton<SequencialCalibNo>.Instance.StartCount },
            };

            // シーケンス番号開始値で降順にソートする
            var sequenceDataList = sequenceDataDic.OrderByDescending(v => v.Value);

            Int32 sequenceEndCount = CarisXConst.SEQUENCE_NO_MAX;

            // シーケンス番号終了値を次に大きいシーケンス番号のシーケンス番号開始位置 - 1の値に変更する
            foreach (var sequenceData in sequenceDataList)
            {
                // 変更する検体種別
                switch (sequenceData.Key)
                {
                    // 一般検体の場合
                    case SampleKind.Sample:
                        Singleton<SequencialSampleNo>.Instance.EndCount = sequenceEndCount;
                        break;

                    // 優先検体の場合
                    case SampleKind.Priority:
                        Singleton<SequencialPrioritySampleNo>.Instance.EndCount = sequenceEndCount;
                        break;

                    // 精度管理の場合
                    case SampleKind.Control:
                        Singleton<SequencialControlNo>.Instance.EndCount = sequenceEndCount;
                        break;

                    // キャリブレータの場合
                    case SampleKind.Calibrator:
                        Singleton<SequencialCalibNo>.Instance.EndCount = sequenceEndCount;
                        break;

                    default:
                        // 何もしない
                        break;
                }

                // シーケンス番号終了値をシーケンス番号開始値 - 1に変更する
                sequenceEndCount = sequenceData.Value - 1;
            }
        }

        /// <summary>
        /// モーターエラーのラック、スレーブを取得
        /// </summary>
        /// <returns>モーターエラーのラック、スレーブのみをListに格納</returns>
        static public string GetMotorErrorRackModule()
        {
            string result = string.Empty;
            List<RackModuleIndex> rackModuleIndices = new List<RackModuleIndex>();

            // モーターエラーのラック、スレーブのみをListに格納
            if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.RackTransfer] == Status.SystemStatusKind.MotorError)
            {
                rackModuleIndices.Add(RackModuleIndex.RackTransfer);
            }

            if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module1] == Status.SystemStatusKind.MotorError)
            {
                rackModuleIndices.Add(RackModuleIndex.Module1);
            }

            if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module2] == Status.SystemStatusKind.MotorError)
            {
                rackModuleIndices.Add(RackModuleIndex.Module2);
            }

            if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module3] == Status.SystemStatusKind.MotorError)
            {
                rackModuleIndices.Add(RackModuleIndex.Module3);
            }

            if (Singleton<Status.SystemStatus>.Instance.ModuleStatus[(Int32)RackModuleIndex.Module4] == Status.SystemStatusKind.MotorError)
            {
                rackModuleIndices.Add(RackModuleIndex.Module4);
            }

            // 赤文字で表示するための、モーターエラーのラック、モジュールを文字列に変換
            StringBuilder sb = new StringBuilder();
            int cnt = 0;
            foreach (var str in rackModuleIndices)
            {
                if (cnt > 0)
                {
                    sb.Append(", ");
                }

                sb.Append(str.ToString());
                cnt++;
            }
            result = sb.ToString();

            return result;
        }


        // 2020-02-27 CarisX IoT Add [START]
        /// <summary>
        /// Content:IoT创建JSon;Add by:Fang;Date:2019-01-03
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        static public string CreateJSon<T>(T model)
        {
            string jsonString = string.Empty;
            if (model == null)
            {
                return jsonString;
            }

            try
            {
                jsonString = "{";

                PropertyInfo[] propertyInfos = model.GetType().GetProperties();
                for (int i = 0; i < propertyInfos.Count(); i++)
                {
                    jsonString += ("\"" + propertyInfos[i].Name + "\":");
                    object value = propertyInfos[i].GetValue(model, null);
                    if (value == null)
                    {
                        jsonString += ",";
                        continue;
                    }
                    if (value.GetType() == typeof(DateTime))
                    {
                        DateTime dateTime = (DateTime)value;
                        value = dateTime.ToString("yyyy/MM/dd HH:mm:ss.fff");
                    }
                    if (value.GetType() == typeof(String))
                    {
                        jsonString += ("\"" + value + "\"");
                    }
                    else
                    {
                        jsonString += value;
                    }

                    if (i < (propertyInfos.Count() - 1))
                    {
                        jsonString += ",";
                    }
                }
                jsonString += "}";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return jsonString;
        }

        /// <summary>
        /// 拷贝系统日志【IssuesNo:16】调整为根据文件修改日期来筛选需要拷贝的文件
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="targetDirectory"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="sSearchPattern"></param>
        static public void CopyLog(string sourceDirectory, string targetDirectory, DateTime startDate, DateTime endDate, string sSearchPattern)
        {
            try
            {
                if (Directory.Exists(sourceDirectory))
                {
                    if(!Directory.Exists(targetDirectory))
                    {
                        Directory.CreateDirectory(targetDirectory);
                    }
                    string[] files = Directory.GetFiles(sourceDirectory, sSearchPattern);
                    if (files != null && files.Length > 0)
                    {
                        var sourceFiles = files.ToList().FindAll(file => (new FileInfo(file).LastWriteTime.Date.CompareTo(startDate) >= 0) && (new FileInfo(file).LastWriteTime.Date.CompareTo(endDate) <= 0));
                        foreach (string sourceFile in sourceFiles)
                        {
                            string targetFile = targetDirectory + "\\" + Path.GetFileName(sourceFile);
                            if (CheckFileStatus(targetFile))
                                File.Delete(targetFile);
                            File.Copy(sourceFile, targetFile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///////////////////////////////////////
        // IoT関連コマンド送信処理
        ///////////////////////////////////////
        /// <summary>
        /// Send MeasureResult to the IoT Hub 【IssuesNo:16】优化调整
        /// </summary>
        /// <param name="command"></param>
        /// <param name="calcData"></param>
        static public void SendIoTMeasureResult(SlaveCommCommand_0503 command, Calculator.CalcData calcData)
        {
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Enable &&
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadMeasurementData &&
                Singleton<CarisXCommIoTManager>.Instance.IoTStatus == IoTStatusKind.OnLine)//增加连接状态判断
            {
                IoTCommCommand_0010 sendData = new IoTCommCommand_0010();
                try
                {
                    var dat = Singleton<InProcessSampleInfoManager>.Instance.SearchInProcessSampleFromIndividuallyNumber(calcData.IndividuallyNo).First();
                    sendData.Model_id = 2; //固定Model_id，以前是Caris200:1 Wan200+:2
                    sendData.Machine_serial_number = ModuleIdToIoTNo(command.ModuleID);//多台联机状态应对
                    sendData.Command_id = (short)CommandKind.IoTCommand0001;
                    sendData.Sample_meas_kind = (short)command.SampleKind;
                    sendData.Receipt_number = dat.ReceiptNumber;
                    sendData.Sequence_no = (short)dat.SequenceNumber;
                    sendData.Rack_id = string.IsNullOrEmpty(calcData.RackID.DispPreCharString) ? string.Empty : calcData.RackID.DispPreCharString;
                    sendData.Rack_position = (short)(calcData.RackPosition.HasValue ? calcData.RackPosition.Value : 0);
                    sendData.Specimen_material_type = (short)command.SpecimenMaterialType;

                    if (command.SampleKind == SampleKind.Control)
                    {
                        var record = from v in Singleton<ControlResultDB>.Instance.GetData()
                                     where v.GetIndividuallyNo() == calcData.IndividuallyNo
                                     select v;
                        sendData.Sample_lot = string.IsNullOrEmpty(record.First().ControlLotNo) ? string.Empty : record.First().ControlLotNo;
                        sendData.Control_name = string.IsNullOrEmpty(record.First().ControlName) ? string.Empty : record.First().ControlName;
                    }
                    else
                    {
                        sendData.Sample_lot = string.IsNullOrEmpty(calcData.SampleID) ? string.Empty : calcData.SampleID;
                        sendData.Control_name = string.Empty;
                    }

                    sendData.Manual_dilution = (short)calcData.ManualDilution;
                    MeasureProtocol protocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(calcData.ProtocolIndex);
                    sendData.Reagent_item = protocol != null ? protocol.ProtocolName : string.Empty;
                    sendData.Count_value = (int)(calcData.CalcInfoReplication.CountValue.HasValue ? calcData.CalcInfoReplication.CountValue : 0);
                    //浓度值，后期结合IOT可能需要调整
                    sendData.Concentration = (double)(calcData.CalcInfoReplication.Concentration.HasValue ? calcData.CalcInfoReplication.Concentration : 0.0);
                    sendData.Judgment = string.IsNullOrEmpty(calcData.Judgement) ? string.Empty : calcData.Judgement;
                    sendData.Remark = calcData.CalcInfoReplication.Remark.Value;
                    sendData.Auto_dilution_ratio = (short)GetHostAutoDil(command.AfterDilution);
                    sendData.Reagent_lot_no = string.IsNullOrEmpty(calcData.ReagentLotNo) ? string.Empty : calcData.ReagentLotNo;
                    sendData.Pretrigger_lot_no = string.IsNullOrEmpty(command.PreTriggerLotNo) ? string.Empty : command.PreTriggerLotNo;
                    sendData.Trigger_lot_no = string.IsNullOrEmpty(command.TriggerLotNo) ? string.Empty : command.TriggerLotNo;
                    sendData.Calibcurve_datetime = (DateTime)(calcData.UseCalcCalibCurveApprovalDate.HasValue ? calcData.UseCalcCalibCurveApprovalDate.Value : DateTime.Now);
                    sendData.Measuring_datetime = calcData.MeasureDateTime;
                    sendData.S1 = command.AnalysisLog.DiffSensor1;
                    sendData.S2 = command.AnalysisLog.DiffSensor2;
                    sendData.S3 = command.AnalysisLog.DiffSensor3;
                    sendData.Dispense_volume = (short)command.AnalysisLog.SampleDispenseVolume;
                    sendData.Sample_aspiration = command.AnalysisLog.SampleAspirationPosition;
                    sendData.M_Reagent_port_no = (short)command.AnalysisLog.MReagPortNo;
                    sendData.M_Sample_position = command.AnalysisLog.MReagLiquidPosition;
                    sendData.R1_reagent_port_no = (short)command.AnalysisLog.R1ReagPortNo;
                    sendData.R1_sample_position = command.AnalysisLog.R1ReagLiquidPosition;
                    sendData.R2_reagent_port_no = (short)command.AnalysisLog.R2ReagPortNo;
                    sendData.R2_sample_position = command.AnalysisLog.R2ReagLiquidPosition;
                    sendData.Temperature_1 = command.AnalysisLog.ReactionTableTemp;
                    sendData.Temperature_2 = command.AnalysisLog.R1ProbeTemp;
                    sendData.Temperature_3 = command.AnalysisLog.R2ProbeTemp;
                    sendData.Temperature_4 = command.AnalysisLog.BF1PreHeatTemp;
                    sendData.Temperature_5 = command.AnalysisLog.BF2PreHeatTemp;
                    sendData.Temperature_6 = command.AnalysisLog.ChemiluminesoensePtotometryTemp;
                    sendData.Temperature_7 = command.AnalysisLog.RoomTemp;
                    string message = CreateJSon(sendData);
                    Singleton<CarisXCommIoTManager>.Instance.SendMessage(message);
                }
                catch (Exception ex)
                {
                    Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("[[Investigation log]]CarisXSubFunction::{0} Detail = {1}", MethodBase.GetCurrentMethod().Name, ex.Message + "\n" + ex.StackTrace));
                }
            }
        }

        /// <summary>
        /// Send ErrorCommand to the IoT Hub【IssuesNo:16】优化调整
        /// </summary>
        /// <param name="command"></param>
        /// <param name="calcData"></param>
        static public void SendIoTErrorCommand(CarisXLogInfoErrorLogExtention infoErrLog, Int32 moduleId, String extStr = "")
        {
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Enable &&
                Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadErrorCommand &&
                Singleton<CarisXCommIoTManager>.Instance.IoTStatus == IoTStatusKind.OnLine)
            {
                try
                {
                    IoTCommConmand_0020 sendData = new IoTCommConmand_0020();
                    sendData.Model_id = 2; 
                    sendData.Machine_serial_number = ModuleIdToIoTNo(moduleId); 
                    sendData.Command_id = (short)CommandKind.IoTCommand0002;
                    sendData.Acquired_datetime = DateTime.Now;
                    sendData.Error_code = infoErrLog.ErrorCode;
                    sendData.Error_arg = infoErrLog.ErrorArg;
                    sendData.Error_level = (short)GetErrorLevel(infoErrLog.ErrorCode, infoErrLog.ErrorArg);
                    sendData.Contents = extStr;
                    sendData.Reagent_item = string.Empty;
                    string message = CreateJSon(sendData);
                    Singleton<CarisXCommIoTManager>.Instance.SendMessage(message);
                }
                catch (Exception ex)
                {
                    Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("[[Investigation log]]CarisXSubFunction::{0} Detail = {1}", MethodBase.GetCurrentMethod().Name, ex.Message + "\n" + ex.StackTrace));
                }
            }
        }

        /// <summary>
        /// Send DueDate Data to the IoT Hub【IssuesNo:16】优化调整
        /// </summary>
        static public void SendIoTDueDate()
        {
            //使用IoT&&上传交期数据&&本地交期数据为有效时间
            if (Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Enable &&
               Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.UploadDueDate &&
               DateTime.Compare(Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Delivery_date, DateTime.MaxValue) < 0 &&
                Singleton<CarisXCommIoTManager>.Instance.IoTStatus == IoTStatusKind.OnLine)
            {
                try
                {
                    IoTCommConmand_0030 sendData = new IoTCommConmand_0030();
                    sendData.Model_id = 2; 
                    sendData.Machine_serial_number = ModuleIdToIoTNo();
                    sendData.Command_id = (short)CommandKind.IoTCommand0003;
                    sendData.Datetime = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Delivery_date;
                    string message = CreateJSon(sendData);
                    Singleton<CarisXCommIoTManager>.Instance.SendMessage(message);
                }
                catch (Exception ex)
                {
                    Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("[[Investigation log]]CarisXSubFunction::{0} Detail = {1}", MethodBase.GetCurrentMethod().Name, ex.Message + "\n" + ex.StackTrace));
                }

            }
        }

        /// <summary>
        /// 【IssuesNo:16】将系统日志打包为zip文件（ErrorHist、OperationLog、OnlineHist、ParamChangeHist、AnalyseHist、DebugLog、Log）
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        static public string PackageSystemLogs(DateTime startDate, DateTime endDate)
        {
            string fileName = string.Empty;
            try
            {
                if (!Directory.Exists(CarisXConst.PathTemp))
                {
                    Directory.CreateDirectory(CarisXConst.PathTemp);
                }

                //导出系统日志文件
                ExportLog(startDate, endDate, LogKind.ErrorHist);
                ExportLog(startDate, endDate, LogKind.OperationHist);
                ExportLog(startDate, endDate, LogKind.ParamChangeHist);
                ExportLog(startDate, endDate, LogKind.AnalyseHist);
                ExportLog(startDate, endDate, LogKind.DebugLog);
                ExportLog(startDate, endDate, LogKind.OnlineHist);
                ExportLog(startDate, endDate, LogKind.OriginLog);

                // 圧縮ファイル名およびファイルパス生成
                fileName = string.Format(@"{0}\W-{1}-{2}_{3}.zip", CarisXConst.PathTemp, Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.IoTParameter.Slave1No.ToString(),
                    DateTime.Now.ToString(CarisXConst.EXPORT_CSV_DATETIMEFORMAT), CarisXConst.EXPORT_ZIP_LOGFILES);

                // ファイル圧縮処理
                ZipFiles(CarisXConst.PathTemp, fileName,true);
               
            }
            catch (Exception ex)
            {
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("[[Investigation log]]CarisXSubFunction::{0} Detail = {1}", MethodBase.GetCurrentMethod().Name, ex.Message + "\n" + ex.StackTrace));
            }
            return fileName;
        }

        /// <summary>
        /// 【IssuesNo:16】导出系统日志到临时文件夹
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="logKind"></param>
        static public void ExportLog(DateTime startDate, DateTime endDate, LogKind logKind)
        {
            switch (logKind)
            {
                case LogKind.ErrorHist:
                    Singleton<ErrorLogDB>.Instance.LoadDB();
                    Singleton<DataHelper>.Instance.ExportCsv(
                                Singleton<ErrorLogDB>.Instance.GetErrorLog().FindAll(data => (data.WriteTime.Date.CompareTo(startDate.Date) >= 0) && (data.WriteTime.Date.CompareTo(endDate.Date) <= 0)),
                                Singleton<FormSystemLog>.Instance.ErrorlogGridColumns,
                                string.Format("{0}\\{1}{2}{3}", CarisXConst.PathTemp, CarisXConst.EXPORT_CSV_ERRORLOG, DateTime.Now.ToString(CarisXConst.EXPORT_CSV_DATETIMEFORMAT), ".csv "),
                                null);
                    break;
                case LogKind.OperationHist:
                    Singleton<OperationLogDB>.Instance.LoadDB();
                    Singleton<DataHelper>.Instance.ExportCsv(
                                Singleton<OperationLogDB>.Instance.GetOperationLog().FindAll(data => (data.WriteTime.Date.CompareTo(startDate.Date) >= 0) && (data.WriteTime.Date.CompareTo(endDate.Date) <= 0)),
                                Singleton<FormSystemLog>.Instance.OperationlogGridColumns,
                                string.Format("{0}\\{1}{2}{3}", CarisXConst.PathTemp, CarisXConst.EXPORT_CSV_OPERATIONLOG, DateTime.Now.ToString(CarisXConst.EXPORT_CSV_DATETIMEFORMAT), ".csv "),
                                null);
                    break;
                case LogKind.ParamChangeHist:
                    Singleton<ParameterChangeLogDB>.Instance.LoadDB();
                    Singleton<DataHelper>.Instance.ExportCsv(
                                Singleton<ParameterChangeLogDB>.Instance.GetParameterChangeLog().FindAll(data => (data.WriteTime.Date.CompareTo(startDate.Date) >= 0) && (data.WriteTime.Date.CompareTo(endDate.Date) <= 0)),
                                Singleton<FormSystemLog>.Instance.ParameterChangeLogGridColumns,
                                string.Format("{0}\\{1}{2}{3}", CarisXConst.PathTemp, CarisXConst.EXPORT_CSV_PARAMETERCHANGELOG, DateTime.Now.ToString(CarisXConst.EXPORT_CSV_DATETIMEFORMAT), ".csv "),
                                null);
                    break;
                case LogKind.AnalyseHist:
                    Singleton<AnalyzeLogDB>.Instance.LoadDB();
                    Singleton<DataHelper>.Instance.ExportCsv(
                                Singleton<AnalyzeLogDB>.Instance.GetAnalyzeLog().FindAll(data => (data.WriteTime.Date.CompareTo(startDate.Date) >= 0) && (data.WriteTime.Date.CompareTo(endDate.Date) <= 0)),
                                Singleton<FormSystemLog>.Instance.AnalyzelogGridColumns,
                                string.Format("{0}\\{1}{2}{3}", CarisXConst.PathTemp, CarisXConst.EXPORT_CSV_ASSAYLOG, DateTime.Now.ToString(CarisXConst.EXPORT_CSV_DATETIMEFORMAT), ".csv "),
                                null);
                    break;
                case LogKind.DebugLog:
                    CopyLog(CarisXConst.PathDebug, CarisXConst.PathTemp, startDate, endDate, "*debuglog_*.log");
                    break;
                case LogKind.OnlineHist:
                    CopyLog(CarisXConst.PathOnline, CarisXConst.PathTemp, startDate, endDate, "*online_*.log");
                    break;
                case LogKind.OriginLog://注意多台联机的处理
                    CopyLog(CarisXConst.PathLogRackTransfer, CarisXConst.PathTempRackTransfer, startDate, endDate, "*file.txt");
                    int numOfConnected = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected;
                    switch (numOfConnected)
                    {
                        case 1:
                            CopyLog(CarisXConst.PathLogModule1, CarisXConst.PathTempModule1, startDate, endDate, "*file.txt");
                            break;
                        case 2:
                            CopyLog(CarisXConst.PathLogModule1, CarisXConst.PathTempModule1, startDate, endDate, "*file.txt");
                            CopyLog(CarisXConst.PathLogModule2, CarisXConst.PathTempModule2, startDate, endDate, "*file.txt");
                            break;
                        case 3:
                            CopyLog(CarisXConst.PathLogModule1, CarisXConst.PathTempModule1, startDate, endDate, "*file.txt");
                            CopyLog(CarisXConst.PathLogModule2, CarisXConst.PathTempModule2, startDate, endDate, "*file.txt");
                            CopyLog(CarisXConst.PathLogModule3, CarisXConst.PathTempModule3, startDate, endDate, "*file.txt");
                            break;
                        case 4:
                            CopyLog(CarisXConst.PathLogModule1, CarisXConst.PathTempModule1, startDate, endDate, "*file.txt");
                            CopyLog(CarisXConst.PathLogModule2, CarisXConst.PathTempModule2, startDate, endDate, "*file.txt");
                            CopyLog(CarisXConst.PathLogModule3, CarisXConst.PathTempModule3, startDate, endDate, "*file.txt");
                            CopyLog(CarisXConst.PathLogModule4, CarisXConst.PathTempModule4, startDate, endDate, "*file.txt");
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            return;
        }

        /// <summary>
        /// 【IssuesNo:16】Zip压缩文件夹
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="desFile"></param>
        static public void ZipFiles(string srcFile, string desFile,bool isDeleteSrcFileFlag = false)
        {
            try
            {
                using (ZipOutputStream zipOutputStream = new ZipOutputStream(File.Open(desFile, FileMode.Create)))
                {
                    if (Directory.Exists(srcFile))
                    {
                        string baseDir = srcFile.EndsWith("\\") ? srcFile.Substring(0, srcFile.Length - 1) : srcFile;
                        DoZipFile(zipOutputStream, srcFile, baseDir, isDeleteSrcFileFlag);
                    }
                    else
                    {
                        DoZipFile(zipOutputStream, srcFile, "", isDeleteSrcFileFlag);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 【IssuesNo:16】Zip压缩文件夹(迭代)
        /// </summary>
        static private void DoZipFile(ZipOutputStream zipOutputStream, string file, string baseDir, bool isDeleteSrcFileFlag = false)
        {
            try
            {
                if (Directory.Exists(file))
                {
                    var files = Directory.GetFiles(file, "*", SearchOption.AllDirectories).Where(
                        p => Path.GetExtension(p).Equals(".log") || Path.GetExtension(p).Equals(".csv") || Path.GetExtension(p).Equals(".txt")).ToList();
                    for (int i = 0; i < files.Count; i++)
                    {
                        DoZipFile(zipOutputStream, files[i], baseDir);
                        if(isDeleteSrcFileFlag && CheckFileStatus(files[i]))
                        {
                            File.Delete(files[i]);
                        }
                    }
                }
                else
                {
                    using (FileStream fs = File.OpenRead(file))
                    {
                        string fileName = string.Empty;
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);

                        if (!string.IsNullOrEmpty(baseDir))
                        {
                            DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(file));

                            while (!directoryInfo.FullName.Equals(baseDir))
                            {
                                fileName = directoryInfo.Name + Path.DirectorySeparatorChar + fileName;
                                directoryInfo = directoryInfo.Parent;
                            }
                        }

                        fileName += Path.GetFileName(file);
                        zipOutputStream.PutNextEntry(new ZipEntry(fileName));
                        zipOutputStream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Send System Files to the IoT Hub【IssuesNo:16】优化调整
        /// <remarks>
        /// </remarks>
        /// </summary>
        static public void SendIoTSystemFiles(DateTime startDate, DateTime endDate)
        {
            string filePath = string.Empty;
            try
            {
                //打包系统日志成zip文件
                filePath = PackageSystemLogs(startDate, endDate);
                // IoTクラウドへ圧縮ファイル送信
                Singleton<CarisXCommIoTManager>.Instance.SendFiles(filePath);
            }
            catch (Exception ex)
            {
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("[[Investigation log]]CarisXSubFunction::{0} Detail = {1}", MethodBase.GetCurrentMethod().Name, ex.Message + "\n" + ex.StackTrace));
            }
        }

        /// <summary>
        /// Send System Files to the IoT Hub(Batch)【IssuesNo:16】优化调整
        /// </summary>
        static public void SendIoTSystemFilesBatch()
        {
            try
            {
                if (!Directory.Exists(CarisXConst.PathTemp))
                    return;

                foreach (string fileName in Directory.GetFiles(CarisXConst.PathTemp, "*.zip"))
                {
                    Singleton<CarisXCommIoTManager>.Instance.SendFiles(fileName);
                }
            }
            catch (Exception ex)
            {
                Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("[[Investigation log]]CarisXSubFunction::{0} Detail = {1}", MethodBase.GetCurrentMethod().Name, ex.Message + "\n" + ex.StackTrace));
            }
        }

        // 2020-02-27 CarisX IoT Add [END]

        /// <summary>
        /// 検体IDチェック
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        static public String IsValidForPatientID(String text)
        {
            String str = text;

            // 通信上使用できない文字があるかチェック
            str = CarisXSubFunction.IsValidForTransmit(str);

            // SQLに使用できない文字があるかチェック
            str = CarisXSubFunction.IsValidForSQL(str);

            // CSVに使用できない文字があるかチェック
            str = CarisXSubFunction.IsValidForCSV(str);

            return str;
        }

        /// <summary>
        /// テキスト文字のチェック（通信上で使用できない文字）
        /// </summary>
        /// <param name="text">テキスト文字列</param>
        /// <returns></returns>
        static public String IsValidForTransmit(String text)
        {
            String str = String.Empty;

            // [a]~[z],[A]~[Z],[0]~[9]以外の文字を削除
            Regex deleteStr = new Regex(@"[^a-zA-Z0-9]");
            str = deleteStr.Replace(text, "");

            return str;
        }

        /// <summary>
        /// テキスト文字のチェック（SQLに使用できない文字）
        /// </summary>
        /// <param name="text">テキスト文字列</param>
        /// <returns></returns>
        static public String IsValidForSQL(String text)
        {
            String str = String.Empty;

            // [']を削除
            Regex deleteStr = new Regex(@"\'");
            str = deleteStr.Replace(text, "");

            return str;
        }

        /// <summary>
        /// テキスト文字のチェック（CSVに使用できない文字）
        /// </summary>
        /// <param name="text">テキスト文字列</param>
        static public String IsValidForCSV(String text)
        {
            String str = String.Empty;

            // [空白、改行],[；],[,],[Tab],[”]を削除
            Regex deleteStr = new Regex(@"[\s\;\,\t\""]");
            str = deleteStr.Replace(text, "");

            return str;
        }

        /// <summary>
        /// 画面が編集中の場合、遷移するか確認メッセージを出力します。
        /// </summary>
        /// <returns>画面遷移を行う / 行わない</returns>
        static public Boolean IsEditsMessageShow()
        {
            // 画面遷移を行うかどうか。 行う: true, しない: false
            bool result = false;

            // Form共通の編集中フラグをチェックする
            if (Oelco.Common.GUI.FormChildBase.IsEdit == true)
            {
                // 画面遷移確認
                // 「編集中に異なる画面を開こうとしました。編集内容が破棄される場合があります。よろしいですか？」
                DialogResult dlgRet = DlgMessage.Show(CarisX.Properties.Resources.STRING_DLG_MSG_265, String.Empty,
                    CarisX.Properties.Resources.STRING_DLG_TITLE_001, MessageDialogButtons.OKCancel);

                if (dlgRet == DialogResult.OK)
                {
                    // 画面遷移を行う
                    result = true;
                }
                else
                {
                    // 画面遷移を行わない
                }
            }
            // 編集中ではない
            else
            {
                // 画面遷移を行う
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 【IssuesNo:12】检查条码信息是否存在于BarCodeDB.xml注册表中
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        static public Boolean IsExistBarCode(string barCode)
        {
            Boolean result = false;
            if (File.Exists(CarisXConst.PathBarCodeDB))
            {
                try
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(CarisXConst.PathBarCodeDB);
                    if (document != null)
                    {
                        XmlNode xmlNode = document.DocumentElement;
                        XmlNodeList nodeList = xmlNode.ChildNodes;
                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            XmlNode node = nodeList.Item(i);
                            if (barCode.Equals(node.InnerText))
                            {
                                return true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                        CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// 【IssuesNo:12】注册条码到BarCode.xml文件中
        /// </summary>
        /// <param name="barCode">条码信息</param>
        static public void RegistBarCodeInfo(string barCode)
        {
            if (string.IsNullOrEmpty(barCode))
            {
                return;
            }

            XmlDocument xmlDocument;
            XmlNode parentNode;
            XmlNodeList xmlNodeList;
            XmlElement newNode;

            try
            {
                if (!File.Exists(CarisXConst.PathBarCodeDB))
                {
                    CarisXSubFunction.CreateBarCodeDB(barCode);
                }
                else
                {
                    xmlDocument = new XmlDocument();
                    xmlDocument.Load(CarisXConst.PathBarCodeDB);
                    if (xmlDocument == null)
                    {
                        CarisXSubFunction.CreateBarCodeDB(barCode);
                        return;
                    }
                    else
                    {
                        parentNode = xmlDocument.DocumentElement;
                        xmlNodeList = parentNode.ChildNodes;
                        //出于XML文件的读取的效率问题，这里限定保存300条，逐步覆盖早期的条码；
                        if (xmlNodeList.Count > 300)
                        {
                            parentNode.RemoveChild(xmlNodeList[0]);
                        }
                        newNode = xmlDocument.CreateElement("BarCode");
                        newNode.InnerText = barCode;
                        parentNode.AppendChild(newNode);
                        //保存
                        xmlDocument.Save(CarisXConst.PathBarCodeDB);
                    }

                }
            }
            catch (Exception ex)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                    CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
                //注册表打开异常时，清空条码，重新创建
                CarisXSubFunction.CreateBarCodeDB(barCode);
            }

        }

        /// <summary>
        /// 【IssuesNo:12】创建BarCodeDB.xml管理激发液、预激发液和稀释液条码信息
        /// </summary>
        /// <param name="barCode">条码信息</param>
        static public void CreateBarCodeDB(string barCode)
        {
            XmlDocument xmlDocument;
            XmlElement parentNode;
            XmlElement newNode;
            try
            {
                xmlDocument = new XmlDocument();
                parentNode = xmlDocument.CreateElement("BarCodeDB");
                xmlDocument.AppendChild(parentNode);
                newNode = xmlDocument.CreateElement("BarCode");
                newNode.InnerText = barCode;
                parentNode.AppendChild(newNode);

                if (File.Exists(CarisXConst.PathBarCodeDB))
                    File.Delete(CarisXConst.PathBarCodeDB);

                xmlDocument.Save(CarisXConst.PathBarCodeDB);
            }
            catch (Exception ex)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                        CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
            }
        }


        ///
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
        /// <summary>
        /// 【IssuesNo:16】判断文件状态是否可以被使用
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static public bool CheckFileStatus(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return false;
            }

            IntPtr HFILE_ERROR = new IntPtr(-1);
            IntPtr vHandle = _lopen(fileName, 2 | 0x40);
            if (vHandle == HFILE_ERROR)
            {
                return false;
            }

            CloseHandle(vHandle);
            return true;
        }
    }
}
