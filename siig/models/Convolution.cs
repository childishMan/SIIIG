using System.Collections.Generic;
using siig.methods;

namespace siig.models
{
    class Convolution
    {
        public static Dictionary<int, double> Convolute(Dictionary<int, double> FirstSignal,
            Dictionary<int, double> SecondSignal)
        {
            var Ticks = FirstSignal.Count + SecondSignal.Count + 1;

            var x = MyConverter.DictionaryToList(FirstSignal);
            var y = MyConverter.DictionaryToList(SecondSignal);

            var Result = new List<double>();
            double tmp;
            for (int m = 0; m < Ticks; m++)
            {
                tmp = 0;
                for (int k = 0; k < x.Count; k++)
                {
                    if (k > m) continue;
                    if ((m - k) >= y.Count) continue;
                    tmp += x[k] * y[m - k];
                }
                Result.Add(tmp);
            }

            return MyConverter.ListToDictionary(Result);
        }
    }
}
