using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.CarisX.Const;
using Oelco.CarisX.Parameter;
using Oelco.Common.Utility;
using Oelco.CarisX.Comm;
using Oelco.Common.Parameter;

namespace Oelco.CarisX.Utility
{
    /// <summary>
    /// 値チェッククラス
    /// </summary>
    /// <remarks>
    /// 仕様に纏わる値の正当性のチェック機能を提供します。
    /// </remarks>
    static public class ValueChecker
    {
        #region [publicメソッド]

        /// <summary>
        /// 一般検体ワークシート正当性チェック
        /// </summary>
        /// <remarks>
        /// 一般検体のワークシートの正当性チェックを行います。
        /// </remarks>
        /// <param name="command">チェック対象検査依頼メッセージコマンド</param>
        /// <param name="type">チェック対象サンプル区分</param>
        /// <returns>true:正/false：不正</returns>
        static public Boolean IsValidSpecimenWorkSheet( HostCommCommand_0002 command, HostSampleType type )
        {

            // データのチェック
            // ・装置番号が違うとエラー
            if ( command.DeviceNo != Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.DeviceNoParameter.DeviceNo )
            {
                // メッセージ表示
                CarisXSubFunction.WriteDPRErrorHist( CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: CarisX.Properties.Resources.STRING_LOG_MSG_074 );
                return false;
            }

            if ( command.SampleType != type )
            {
                // サンプル区分が異なります
                CarisXSubFunction.WriteDPRErrorHist( CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: CarisX.Properties.Resources.STRING_LOG_MSG_075 );
                return false;
            }

            Boolean isEnableMeasCount = command.NumOfMeasItem < CarisXConst.REGIST_MEAS_ITEM_MAX_UPPER;
            if ( !isEnableMeasCount )
            {
                // 分析項目が50項目以上指定されています。
                CarisXSubFunction.WriteDPRErrorHist( CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: String.Format( CarisX.Properties.Resources.STRING_LOG_MSG_078, CarisXConst.REGIST_MEAS_ITEM_MAX_UPPER ) );
                return false;
            }

            // ・自動希釈倍率が範囲外でエラー(ホストから送信された値が1-5の範囲外であればtrueになる)
            Boolean outAfterDilRange = ( from v in command.MeasItems
                             where v.afterDil.GetDPRAutoDilution() == 0
                             select v ).Count() != 0;
            if ( outAfterDilRange )
            {
                // メッセージ表示
                CarisXSubFunction.WriteDPRErrorHist( CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: CarisX.Properties.Resources.STRING_LOG_MSG_079 );
                return false;
            }

            bool isEnableManualDil = command.ManualDil <= 0;
            if ( isEnableManualDil )
            {
                // 手希釈倍率が0以下です。
                CarisXSubFunction.WriteDPRErrorHist( CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: CarisX.Properties.Resources.STRING_LOG_MSG_080 );
                return false;
            }

            // ・手希釈倍率不可項目が登録されている上で手希釈倍率が1以外ならエラー
            // 手希釈倍率が設定されている場合、登録されている測定項目の中に手希釈倍率不使用のものがあればエラーとなる。
            var protoDpr = from v in command.MeasItems
                           let proto = Singleton<ParameterFilePreserve<MeasureProtocolInfo>>.Instance.Param.GetDPRProtocolFromHostProtocolNumber( v.protoNo, Singleton<MeasureProtocolManager>.Instance )
                           where proto != null
                           select proto;
            if ( command.ManualDil > 1 )
            {
                // 登録されている分析項目全体に対して手希釈倍率不使用が見つからなかった場合、true
                var manualDilUnUseProtocols =
                    from v in protoDpr
                    where !v.UseManualDil
                    select v;
                if ( manualDilUnUseProtocols.Count() != 0 )
                {
                    // エラーメッセージ
                    CarisXSubFunction.WriteDPRErrorHist( CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: CarisX.Properties.Resources.STRING_LOG_MSG_083 );
                    return false;
                }
            }

            // サンプル区分が異なる場合エラー
            var typeErrorList = from v in protoDpr
                                where ( v.SampleKind & command.SampleMaterialType.GetDPRSpecimemMaterialType().ToProtocolSampleKind() ) == MeasureProtocol.SampleTypeKind.None 
                                select v;
            if ( typeErrorList.Count() != 0 )
            {
                // エラーメッセージ
                CarisXSubFunction.WriteDPRErrorHist( CarisXDPRErrorCode.FromHostWorkSheetFormatError, extStr: CarisX.Properties.Resources.STRING_LOG_MSG_085 );
                return false;
            }

            return true;

        }

        /// <summary>
        /// 測定指示データ問合せ内容チェック
        /// </summary>
        /// <remarks>
        /// 測定指示データ問合せコマンド内容の整合性をチェックします、
        /// このチェックはシステムパラメータ設定が検体ID分析の時に呼び出されます。
        /// </remarks>
        /// <param name="ask">測定指示データ問合せ方式</param>
        /// <param name="RackId">対象ラックID</param>
        /// <returns>True:問合せ受付可能 False:問合せ受付不可</returns>
        static public Boolean IsCorrectMeasIndicateAskWhenSampleIDAssay( AskTypeKind ask, String RackId )
        {
            Boolean result = false;
            CarisXIDString idString;

            // ID形式でなければ不正
            if ( !CarisXIDString.TryParse( RackId, out idString ) )
            {
                return false;
            }

            RackIDDefinitionParameter rackIdDefine = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.RackIDDefinitionParameter;

            // 範囲チェックRange check
            switch ( ask )
            {
            case AskTypeKind.RackID:
                // 問合せラックIDが、検体ID分析時有効ラックID範囲内であること
                //result = ( ( idString.Value >= rackIdDefine.MinRackIDSampModeSample ) && ( idString.Value <= rackIdDefine.MaxRackIDSampModeSample ) );
                    result = ((idString.Value >= rackIdDefine.MinRackIDSamp) && (idString.Value <= rackIdDefine.MaxRackIDSamp));
                break;
            case AskTypeKind.SampleID:
                // 問合せラックIDが、ラックID分析時有ラックID範囲内であることThat inquiry rack ID is within the perforated rack ID range when the rack ID analysis
                result = ( ( idString.Value >= rackIdDefine.MinRackIDSamp ) && ( idString.Value <= rackIdDefine.MaxRackIDSamp ) );
                // 問合せラックIDが、検体ID分析時ラックID範囲"外"であることThat inquiry rack ID is a specimen ID analysis during the rack ID range "outside"
                //result &= ( ( idString.Value < rackIdDefine.MinRackIDSampModeSample ) || ( idString.Value > rackIdDefine.MaxRackIDSampModeSample ) );
                //result &= ((idString.Value >= rackIdDefine.MinRackIDSampModeSample) && (idString.Value <= rackIdDefine.MaxRackIDSampModeSample));
                result = ((idString.Value >= rackIdDefine.MinRackIDSampModeSample) && (idString.Value <= rackIdDefine.MaxRackIDSampModeSample));
                break;

            default:
                result = false;
                break;
            }

            return result;
        }

        public static Boolean IsEnableSequenceStartNoSet( params Int32[] sequenceNoSet )
        {
            Boolean isEnable = false;

            // 全ての番号に重複が無いことを確認する
            var uniqueList = sequenceNoSet.Distinct();
            isEnable = uniqueList.Count() == sequenceNoSet.Count();

            return isEnable;
        }


        ///// <summary>
        ///// サンプルID確認
        ///// </summary>
        ///// <remarks>
        ///// ＠＠＠
        ///// </remarks>
        ///// <param name="target">サンプルID確認対象文字列</param>
        ///// <returns>True:サンプルId False:非サンプルId</returns>
        //static public Boolean IsSampleId( String target )
        //{
        //    //TODO:サンプルID確認
        //    return true;
        //}

        ///// <summary>
        ///// 検体ID確認
        ///// </summary>
        ///// <remarks>
        ///// ＠＠＠
        ///// </remarks>
        ///// <param name="target">検体ID確認対象文字列</param>
        ///// <returns>True:検体ID False:非検体Id</returns>
        //static public Boolean IsPatientId( String target )
        //{
        //    //TODO:検体ID確認
        //    return true;
        //}

        // TODO:妥当性確認処理群、単純な成功失敗の項目はBooleanの戻り値のみとし、
        //      複数の結果が必要なものは、それに加えてenum値を引数でout

        #endregion

    }
}
