using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;

namespace Oelco.CarisX.Print
{
    /// <summary>
    /// ReportClass拡張メソッドクラス
    /// </summary>
    /// <remarks>
    /// ReportClass拡張メソッドクラスです。
    /// </remarks>
    static public class ReportEX
    {
        /// <summary>
        /// レポートセクション取得
        /// </summary>
        /// <remarks>
        /// 引数に指定された名称のセクションを返します。
        /// </remarks>
        /// <param name="cls"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        static public Section GetSection( this ReportClass cls, String section )
        {
            Type typ = cls.GetType();
            return (Section)typ.GetProperty( section ).GetValue( cls, null );
        }
    }
}
