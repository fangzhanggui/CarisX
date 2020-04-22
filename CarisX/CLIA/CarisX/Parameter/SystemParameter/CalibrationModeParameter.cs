using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// キャリブレーションモード設定
    /// </summary>
    public class CalibrationModeParameter:AttachmentParameter
    {
        /// <summary>
        /// キャリブレーションモード種別
        /// </summary>
        public enum CalibrationModeKind
        {
            ModeOne = 1,
            ModeTwo = 2,
        }

        /// <summary>
        /// キャリブレーションモード
        /// </summary>
        public CalibrationModeKind CalibrationMode { get; set; } = CalibrationModeKind.ModeOne;
    }
}
