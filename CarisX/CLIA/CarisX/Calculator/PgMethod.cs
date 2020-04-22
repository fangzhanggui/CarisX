using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.CarisX.Log;
using Oelco.Common.Utility;
using Oelco.Common.Log;

namespace Oelco.CarisX.Calculator
{
    public class PgMethod
    {
        public double Pg( double pg1Concent, double pg2Concent )
        {
            if (pg1Concent < 0 || pg2Concent < 0)
            {
                return 0;
            }

            double pg = 0;

            try
            {
                pg = pg1Concent / pg2Concent;
            }
            catch (Exception ex)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, ex.ToString());
            }

            return pg;
        }
    }
}
