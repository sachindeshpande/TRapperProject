using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class MathUtilities
    {
        public enum DeviationType
        {
            Population,
            Sample
        }

        public static double Deviation(double[] Values,int pNumValues)
        {
            return Deviation(Values, DeviationType.Sample, pNumValues);
        }

        public static double Deviation(double[] Values, DeviationType CalculationType, int pNumValues)
        {
            double SumOfValuesSquared = 0;
            double SumOfValues = 0;
            //Calculate the sum of all the values
//            foreach (double item in Values)
            for(int lIndex = 0; lIndex < pNumValues; lIndex++)
                SumOfValues += Values[lIndex];

            //Calculate the sum of all the values squared
//            foreach (double item in Values)
            for (int lIndex = 0; lIndex < pNumValues; lIndex++)
                SumOfValuesSquared += Math.Pow(Values[lIndex], 2);

            double lSquareRootSum = 0;

            if (CalculationType == DeviationType.Sample)
            {
                lSquareRootSum = (SumOfValuesSquared - Math.Pow(SumOfValues, 2) / pNumValues) / (pNumValues - 1);
            }
            else
            {
                lSquareRootSum = (SumOfValuesSquared - Math.Pow(SumOfValues, 2) / pNumValues) / pNumValues;
            }

            if (lSquareRootSum < ProjectCommon.ProjectConstants.REAL_SMALL_DOUBLE_VALUE)
                return 0;
            else if (lSquareRootSum < ProjectCommon.ProjectConstants.SMALL_DOUBLE_VALUE)
                return ProjectCommon.ProjectConstants.SMALL_DOUBLE_VALUE;
            else
                return Math.Sqrt(lSquareRootSum);
        }

        public static double Average(double[] Values)
        {
            double SumOfValues = 0;
            //Calculate the sum of all the values
            foreach (double item in Values)
            {
                SumOfValues += item;
            }

            return SumOfValues / Values.Length;
        }

    }
}
