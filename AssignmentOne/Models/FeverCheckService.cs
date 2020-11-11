using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentOne.Models
{
    public static class FeverCheckService
    {
        public enum DegreeType
        {
            Celcius,
            Fahrenheit,
        }
        public static string CheckFever(float temp, DegreeType degreeType)
        {

            if (degreeType == DegreeType.Fahrenheit)
                temp = FahrenheitToCelcius(temp);

            if (temp > 38)
            {
                return "YOU ARE TO HOT!!!!!";
            }
            else if (temp < 34)
            {
                return "YOU ARE TO COLD!!!!";
            }
            else
            {
                return "YOU ARE OK!!!!!";
            }
        }
        public static float FahrenheitToCelcius(float temp)
        {
            return (temp - 32) * 5 / 9; //wHy iS thIs sO WiErD;
        }
    }
}
