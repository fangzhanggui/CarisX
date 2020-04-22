using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 会社ロゴ設定
    /// </summary>
    public class CompanyLogoParameter : AttachmentParameter
    {
        /// <summary>
        /// 会社ロゴ種別
        /// </summary>
        public enum CompanyLogoKind
        {
            LogoDefault = 0,
            LogoOne = 1,
            LogoTwo = 2,
        }

        /// <summary>
        /// 会社ロゴ
        /// </summary>
        public CompanyLogoKind CompanyLogo { get; set; } = CompanyLogoKind.LogoDefault;
    }
}
