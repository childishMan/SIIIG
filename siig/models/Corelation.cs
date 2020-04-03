using System;
using System.Collections.Generic;
using System.Linq;
using siig.methods;

namespace siig.models
{
    static class Corelation
    {

        private static Tuple<List<double>, List<double>> NormalizeInput(List<double> x1,List<double> x2)
        {
            if (x1.Count > x2.Count)
            {
                while (x2.Count != x1.Count)
                {
                    x2.Add(0);
                }
            }
           else if (x2.Count > x1.Count)
            {
                while (x1.Count != x2.Count)
                {
                    x1.Add(0);
                }
            }

            return Tuple.Create(x1, x2);
        }

        public static List<double> CrossCorealtion(Dictionary<int, double> FirstSignal, Dictionary<int, double> SecondSignal, bool normalize = false)
        {

            var x1 = MyConverter.DictionaryToList(FirstSignal);
            var x2 = MyConverter.DictionaryToList(SecondSignal);

            var tuple = NormalizeInput(x1, x2);

            x1 = tuple.Item1;
            x2 = tuple.Item2;

            var size = x1.Count;
            List<double> corelated = new List<double>();

            for (int i = 0; i < size * 2 - 1; i++)
            {
                corelated.Add(0);
            }

            var sum1 = 0.0;
            var sum2 = 0.0;

            for (int i = 0; i < size; ++i)
            {

                sum1 = sum2 = 0.0;
                for (int k = 0; k < size - i; ++k)
                {
                    sum1 += x1[k] * x2[k + i];
                    sum2 += x2[k] * x1[k + i];
                }

                corelated[size + i - 1] = sum1 / size;
                corelated[size - i - 1] = sum2 / size;
            }

            if (normalize)
            {
                sum1 = x1.Sum((x) => x * x);
                sum2 = x2.Sum((x) => x * x);

                var sf = Math.Sqrt(sum1 * sum2) / size;

                for (int i = 0; i < corelated.Count; i++)
                {
                    corelated[i] = corelated[i] / sf;
                }
            }
            return corelated;
        }

        public static List<double> autoCorelation(Dictionary<int, double> Signal , bool normalize = false)
        {
            var x = MyConverter.DictionaryToList(Signal);

            List<double> corelated = new List<double>();

            var size = x.Count;
            for (int i = 0; i < size; ++i)
            {
                var sum = 0.0;
                for (int k = 0; k < size - i; ++k)
                {
                    sum += x[k] * x[k + i];
                }
                corelated.Add(sum / size);
            }

            if (normalize)
            {
                var sf = corelated.First();
                for (int i = 0; i < corelated.Count; i++)
                {
                    corelated[i] = corelated[i] / sf;
                }
            }

            return corelated;
        }

    }

}
