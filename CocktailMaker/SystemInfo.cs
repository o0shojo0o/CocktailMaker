using Iot.Device.CpuTemperature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMaker
{
    public class SystemInfo
    {
        public static double GetCPUTemp()
        {
            CpuTemperature cpuTemperature = new CpuTemperature();

            if (cpuTemperature.IsAvailable)
            {
                return Math.Round(cpuTemperature.Temperature.Celsius, 0);
            }
            else
            {
                return Double.NaN;
            }
        }
    }
}
