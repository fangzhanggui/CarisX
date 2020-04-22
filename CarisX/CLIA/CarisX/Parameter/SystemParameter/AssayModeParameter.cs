using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.CarisX.Const;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 分析方式設定
    /// </summary>
	public class AssayModeParameter : AttachmentParameter
	{
        /// <summary>
        /// 分析方式種別
        /// </summary>
        public enum AssayModeKind
        {
            /// <summary>
            /// シーケンシャル分析
            /// </summary>
            Sequencial = 0,
            /// <summary>
            /// ラックID分析
            /// </summary>
            RackID = 1,
            /// <summary>
            /// 検体ID分析
            /// </summary>
            SampleID = 2
        }

        /// <summary>
        /// 測定種別
        /// </summary>
        public enum MeasKind
        {
            /// <summary>
            /// ラックID
            /// </summary>
            RackID = 0,
            /// <summary>
            /// 検体ID
            /// </summary>
            SampleID = 1
        }

        /// <summary>
        /// 分析方式
        /// </summary>
        public AssayModeKind AssayMode { get; set; } = AssayModeKind.RackID;

        /// <summary>
        /// キャリブレータ測定
        /// </summary>
        public MeasKind CalibMeas { get; set; } = MeasKind.RackID;

        /// <summary>
        /// 精度管理検体測定
        /// </summary>
        public MeasKind ControlMeas { get; set; } = MeasKind.RackID;

        /// <summary>
        /// 分析モード(Module1)
        /// </summary>
        public Boolean UseEmergencyModeForModule1 { get; set; } = false;
        /// <summary>
        /// 分析モード(Module2)
        /// </summary>
        public Boolean UseEmergencyModeForModule2 { get; set; } = false;
        /// <summary>
        /// 分析モード(Module3)
        /// </summary>
        public Boolean UseEmergencyModeForModule3 { get; set; } = false;
        /// <summary>
        /// 分析モード(Module4)
        /// </summary>
        public Boolean UseEmergencyModeForModule4 { get; set; } = false;

        /// <summary>
        /// 急診モード取得
        /// </summary>
        /// <param name="moduleIndex"></param>
        public Boolean GetUseEmergencyMode( Int32 moduleIndex )
        {
            Boolean result = false;

            switch (moduleIndex)
            {
                case (Int32)ModuleIndex.Module1:
                    result = this.UseEmergencyModeForModule1;
                    break;
                case (Int32)ModuleIndex.Module2:
                    result = this.UseEmergencyModeForModule2;
                    break;
                case (Int32)ModuleIndex.Module3:
                    result = this.UseEmergencyModeForModule3;
                    break;
                case (Int32)ModuleIndex.Module4:
                    result = this.UseEmergencyModeForModule4;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 接続台数分のスレーブの急診使用有無によって、急診モードの分析項目ボタンを非活性にするかの判断フラグ
        /// </summary>
        /// <returns>true:急診モードの分析項目ボタンを非活性にする</returns>
        /// <returns>false:急診モードの分析項目ボタンを非活性にしない</returns>
        public Boolean IsProtocolEnabledChangedInEmergencyMode()
        {
            bool enabledFlag = true;

            // 接続するスレーブのマックス件数に合わせて各スレーブの急診使用有無をチェックする
            for (int moduleIndex = 0; moduleIndex < Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModuleConnectParameter.NumOfConnected; moduleIndex++)
            {
                if (this.GetUseEmergencyMode(moduleIndex) == true)
                {
                    enabledFlag = false;
                    break;
                }
            }

            return enabledFlag;
        }
    }


}
 
