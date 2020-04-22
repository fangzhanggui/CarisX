using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Calculator
{
    public class IGRAMethod
    {
        private double? conB;
        private double? conA;
        private double? conC;

        public IGRAMethod(double? dCon1, double? dCon2, double? dCon3)
        {
            conB = dCon1;
            conA = dCon2;
            conC = dCon3;
        }

        public JudgementType CaculateJudge()
       {
           // if (conB == null || conA== null || conC == null)
            if(conB == null)
            {
                return JudgementType.Half;
            }
            
           if (conB > 5000)
           {
               return JudgementType.Half;
           }
           else//<=5000
           {
               if (conA == null)
               {
                   return JudgementType.Half;
               }
              double a_b = conA.Value - conB.Value;
              //double c_b = conC.Value - conB.Value;
              double  pencent25OfB = 0.25* conB.Value;
               if ( a_b>=14 && a_b >= pencent25OfB)
               {
                   return JudgementType.Positive;
               }
               else
               {
                   if (conC == null)
                   {
                        return JudgementType.Half;
                   }
                   else
                   {
                       double c_b = conC.Value - conB.Value;
                       if (a_b < 14 && c_b >= 20)
                       {
                           return JudgementType.Negative;
                       }
                       else if (a_b >= 14 && a_b < pencent25OfB && c_b >= 20)
                       {
                           return JudgementType.Negative;
                       }
                       else if (a_b < 14 && c_b < 20)//后面只需判断 Half就可以了
                       {
                           return JudgementType.Half;
                       }
                       else if (a_b >= 14 && a_b < pencent25OfB && c_b < 20)
                       {
                           return JudgementType.Half;
                       }
                       else
                       {
                           return JudgementType.Half;
                       }
                   }
               }
              
           }
       }
    }
}
