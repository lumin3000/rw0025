using System;

namespace URandom
{
	public static class NormalDistribution
	{
		public static double StdNormalCDF(double u)
		{
			double num = 1.4142135623730951;
			double[] array = new double[5] { 0.0116111066365377, 0.3951404679838207, 28.46603853776254, 188.742618842651, 3209.377589138469 };
			double[] array2 = new double[5] { 0.1767766952966369, 8.34431643857962, 172.5514762600375, 1813.893686502485, 8044.716608901563 };
			double[] array3 = new double[9] { 2.1531153547440383E-08, 0.5641884969886701, 8.883149794388377, 66.11919063714163, 298.6351381974001, 881.952221241769, 1712.0476126340707, 2051.0783778260716, 1230.3393547979972 };
			double[] array4 = new double[9] { 1.0, 15.744926110709835, 117.6939508913125, 537.1811018620099, 1621.3895745666903, 3290.7992357334597, 4362.619090143247, 3439.3676741437216, 1230.3393548037495 };
			double[] array5 = new double[6] { 0.016315387137302097, 0.30532663496123236, 0.36034489994980445, 0.12578172611122926, 0.016083785148742275, 0.0006587491615298378 };
			double[] array6 = new double[6] { 1.0, 2.568520192289822, 1.8729528499234604, 0.5279051029514285, 0.06051834131244132, 0.0023352049762686918 };
			if (double.IsNaN(u))
			{
				return double.NaN;
			}
			if (double.IsInfinity(u))
			{
				return (!(u < 0.0)) ? 1.0 : 0.0;
			}
			double num2 = Math.Abs(u);
			double num3;
			if (num2 <= 15.0 / 32.0 * num)
			{
				num3 = num2 * num2;
				num2 = u * ((((array[0] * num3 + array[1]) * num3 + array[2]) * num3 + array[3]) * num3 + array[4]) / ((((array2[0] * num3 + array2[1]) * num3 + array2[2]) * num3 + array2[3]) * num3 + array2[4]);
				return 0.5 + num2;
			}
			num3 = Math.Exp((0.0 - num2) * num2 / 2.0) / 2.0;
			if (num2 <= 4.0)
			{
				num2 /= num;
				num2 = ((((((((array3[0] * num2 + array3[1]) * num2 + array3[2]) * num2 + array3[3]) * num2 + array3[4]) * num2 + array3[5]) * num2 + array3[6]) * num2 + array3[7]) * num2 + array3[8]) / ((((((((array4[0] * num2 + array4[1]) * num2 + array4[2]) * num2 + array4[3]) * num2 + array4[4]) * num2 + array4[5]) * num2 + array4[6]) * num2 + array4[7]) * num2 + array4[8]);
				num2 = num3 * num2;
			}
			else
			{
				num3 = num3 * num / num2;
				num2 = 2.0 / (num2 * num2);
				num2 = num2 * (((((array5[0] * num2 + array5[1]) * num2 + array5[2]) * num2 + array5[3]) * num2 + array5[4]) * num2 + array5[5]) / (((((array6[0] * num2 + array6[1]) * num2 + array6[2]) * num2 + array6[3]) * num2 + array6[4]) * num2 + array6[5]);
				num2 = num3 * (0.5641895835477563 - num2);
			}
			return (!(u < 0.0)) ? (1.0 - num2) : num2;
		}

		public static double StdNormalINV(double p)
		{
			double[] array = new double[6] { -39.69683028665376, 220.9460984245205, -275.9285104469687, 138.357751867269, -30.66479806614716, 2.506628277459239 };
			double[] array2 = new double[5] { -54.47609879822406, 161.5858368580409, -155.6989798598866, 66.80131188771972, -13.28068155288572 };
			double[] array3 = new double[6] { -0.007784894002430293, -0.3223964580411365, -2.400758277161838, -2.549732539343734, 4.374664141464968, 2.938163982698783 };
			double[] array4 = new double[4] { 0.007784695709041462, 0.3224671290700398, 2.445134137142996, 3.754408661907416 };
			if (double.IsNaN(p) || p > 1.0 || p < 0.0)
			{
				return double.NaN;
			}
			if (p == 0.0)
			{
				return double.NegativeInfinity;
			}
			if (p == 1.0)
			{
				return double.PositiveInfinity;
			}
			double num = ((!(p < 1.0 - p)) ? (1.0 - p) : p);
			double num3;
			double num2;
			if (num > 0.02425)
			{
				num2 = num - 0.5;
				num3 = num2 * num2;
				num2 = num2 * (((((array[0] * num3 + array[1]) * num3 + array[2]) * num3 + array[3]) * num3 + array[4]) * num3 + array[5]) / (((((array2[0] * num3 + array2[1]) * num3 + array2[2]) * num3 + array2[3]) * num3 + array2[4]) * num3 + 1.0);
			}
			else
			{
				num3 = Math.Sqrt(-2.0 * Math.Log(num));
				num2 = (((((array3[0] * num3 + array3[1]) * num3 + array3[2]) * num3 + array3[3]) * num3 + array3[4]) * num3 + array3[5]) / ((((array4[0] * num3 + array4[1]) * num3 + array4[2]) * num3 + array4[3]) * num3 + 1.0);
			}
			num3 = StdNormalCDF(num2) - num;
			num3 = num3 * 2.506628274631 * Math.Exp(num2 * num2 / 2.0);
			num2 -= num3 / (1.0 + num2 * num3 / 2.0);
			return (!(p > 0.5)) ? num2 : (0.0 - num2);
		}

		public static double Normalize(double rn, float temperature)
		{
			double num = StdNormalINV(rn);
			num /= (double)temperature;
			return 1.0 / (1.0 + Math.Exp(0.0 - num));
		}
	}
}
