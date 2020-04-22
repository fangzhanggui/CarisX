using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.CarisX.Log;
using Oelco.Common.Utility;
using Oelco.Common.Log;

namespace Oelco.CarisX.Calculator
{
    public class PsaMethod
    {
        public double Psa( double f_psaConcent, double t_psaConcent )
        {
            if (f_psaConcent < 0 || t_psaConcent < 0)
            {
                return 0;
            }

            double psg = 0;

            try
            {
                  psg = f_psaConcent / t_psaConcent;
            }
            catch (Exception ex)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, ex.ToString());
            }

            return psg;
        }
    }
}
