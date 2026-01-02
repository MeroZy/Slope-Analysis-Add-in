using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slope_Analysis_Add_in.Methods
{
    public static class UnitMethods
    {
        public static double ToDegrees(this double radian)
        {
            return radian * (180.0 / Math.PI);
        }
        
        public static double ToRadians(this double degree)
        {
            return degree * (Math.PI / 180.0);
        }

        public static double Rad2Pct(this double radian)
        {
            return Math.Tan(radian) * 100;
        }
    }
}
