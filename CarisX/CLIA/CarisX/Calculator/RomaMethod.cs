using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.CarisX.Log;
using Oelco.Common.Utility;
using Oelco.Common.Log;

namespace Oelco.CarisX.Calculator
{
    public class RomaMethod
    {
        public double preRoma(double ca125Concent, double he4Concent)
        {
            if (ca125Concent < 0 || he4Concent < 0)
                return 0;
            double roma = 0;
            try
            {
                double PI = -12.0 + 2.38 * Math.Log(he4Concent, Math.E) + 0.0626 * Math.Log(ca125Concent,Math.E);
                roma = Math.Pow(Math.E,PI) / (1 + Math.Pow(Math.E,PI)) * 100;
                
            }
            catch (Exception ex)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, ex.ToString());
            }
            return roma;
        }

        public double postRoma(double ca125Concent, double he4Concent)
        {
            if (ca125Concent < 0 || he4Concent < 0)
                return 0;
            double roma = 0;
            try
            {
                double PI = -8.09 + 1.04 * Math.Log(he4Concent, Math.E) + 0.732 * Math.Log(ca125Concent,Math.E);
                roma = Math.Pow(Math.E,PI) / (1 + Math.Pow(Math.E,PI)) * 100;
            }
            catch (Exception ex)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, ex.ToString());
            }
            return roma;
        }

    }
}
